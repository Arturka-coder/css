using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace laba_3.Models;

[XmlRoot("text")]
public class Text
{
    [XmlElement("sentence")]
    public List<Sentence> Sentences { get; set; } = new();

    public Text() { }

    public Text(string rawText)
    {
        ParseText(rawText);
    }

    private void ParseText(string rawText)
    {
        var sentenceMatches = Regex.Matches(rawText, @"[^.!?]+[.!?]?");

        foreach (Match sMatch in sentenceMatches)
        {
            string rawSentence = sMatch.Value.Trim();
            if (string.IsNullOrEmpty(rawSentence)) continue;

            Sentence sentence = new Sentence();
            var tokens = Regex.Matches(rawSentence, @"[\wА-Яа-яЁёƏəĞğIıİiÖöŞşÜü]+|[.,\/#!$%\^&\*;:{}=\-_`~()?]");

            foreach (Match token in tokens)
            {
                string val = token.Value;
                if (Regex.IsMatch(val, @"[\wА-Яа-яЁёƏəĞğIıİiÖöŞşÜü]+"))
                {
                    sentence.Words.Add(new Word(val));
                }
                else
                {
                    sentence.Punctuations.Add(new Punctuation(val));
                }
            }

            if (sentence.Words.Count > 0 || sentence.Punctuations.Count > 0)
            {
                Sentences.Add(sentence);
            }
        }
    }

    public List<Sentence> GetSortedByWordCount()
    {
        return Sentences.OrderBy(s => s.Words.Count).ToList();
    }

    public List<Sentence> GetSortedByLength()
    {
        return Sentences.OrderBy(s => s.ToString().Length).ToList();
    }

    public HashSet<string> GetUniqueWordsInQuestions(int length)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var sentence in Sentences.Where(s => s.IsQuestion()))
        {
            foreach (var word in sentence.Words.Where(w => w.Value.Length == length))
            {
                result.Add(word.Value);
            }
        }
        return result;
    }

    public void RemoveWordsByLengthAndConsonant(int length)
    {
        string vowels = "аеёиоуыэюяaeiouy";
        foreach (var sentence in Sentences)
        {
            sentence.Words.RemoveAll(w => 
                w.Value.Length == length && 
                !string.IsNullOrEmpty(w.Value) && 
                !vowels.Contains(char.ToLower(w.Value[0]))
            );
        }
    }
    public void ReplaceWordsInSentence(int sentenceIndex, int wordLength, string replacement)
    {
        if (sentenceIndex >= 0 && sentenceIndex < Sentences.Count)
        {
            foreach (var word in Sentences[sentenceIndex].Words)
            {
                if (word.Value.Length == wordLength)
                {
                    word.Value = replacement;
                }
            }
        }
    }

    public void RemoveStopWords(HashSet<string> stopWords)
    {
        foreach (var sentence in Sentences)
        {
            sentence.Words.RemoveAll(w => stopWords.Contains(w.Value.ToLower()));
        }
    }
}
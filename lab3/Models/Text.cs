using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace laba_3.Models;

[XmlRoot("text")]
public class Text
{
    [XmlElement("sentence")]
    public List<Sentence> Sentences { get; set; } = new();

    public static Text ParseFile(string path)
    {
        var rawText = File.ReadAllText(path, Encoding.UTF8);
        var textObj = new Text();

        string sentencePattern = @"(?<=[.!?])\s+";
        var sentenceStrings = Regex.Split(rawText, sentencePattern);

        foreach (var rawSentence in sentenceStrings)
        {
            if (string.IsNullOrWhiteSpace(rawSentence)) continue;

            var sentence = new Sentence();
            string tokenPattern = @"\w+|[^\w\s]";
            var matches = Regex.Matches(rawSentence, tokenPattern);

            foreach (Match match in matches)
            {
                string val = match.Value;
                if (Regex.IsMatch(val, @"^\w+$"))
                {
                    var word = new Word(val);
                    sentence.Words.Add(word);
                    sentence.Tokens.Add(word);
                }
                else
                {
                    sentence.Tokens.Add(new Punctuation(val));
                }
            }

            if (sentence.Tokens.Count > 0)
                textObj.Sentences.Add(sentence);
        }

        return textObj;
    }

    public IEnumerable<Sentence> OrderByWordCount() => Sentences.OrderBy(s => s.WordCount);

    public IEnumerable<Sentence> OrderByLength() => Sentences.OrderBy(s => s.FullLength);

    public List<Word> FindWordsInQuestions(int length)
    {
        return Sentences
            .Where(s => s.IsQuestion())
            .SelectMany(s => s.Words)
            .Where(w => w.Length == length)
            .GroupBy(w => w.Value.ToLower())
            .Select(g => g.First())
            .ToList();
    }

    public int RemoveWordsStartingWithConsonant(int length)
    {
        int count = 0;
        foreach (var sentence in Sentences)
        {
            count += sentence.Words.RemoveAll(w => w.Length == length && w.StartsWithConsonant());
            sentence.Tokens.RemoveAll(t => t is Word w && w.Length == length && w.StartsWithConsonant());
        }
        return count;
    }

    public int ReplaceWordsInSentence(int sentenceNumber, int length, string replacement)
    {
        int count = 0;
        var sentence = Sentences[sentenceNumber - 1];

        for (int i = 0; i < sentence.Tokens.Count; i++)
        {
            if (sentence.Tokens[i] is Word w && w.Length == length)
            {
                var newWord = new Word(replacement);
                sentence.Tokens[i] = newWord;

                int wordIdx = sentence.Words.IndexOf(w);
                if (wordIdx != -1) sentence.Words[wordIdx] = newWord;
                count++;
            }
        }
        return count;
    }

    public int RemoveStopWords(IEnumerable<string> stopWordsLines)
    {
        var stopSet = new HashSet<string>(stopWordsLines.Select(w => w.Trim()), StringComparer.OrdinalIgnoreCase);
        int count = 0;

        foreach (var sentence in Sentences)
        {
            count += sentence.Words.RemoveAll(w => stopSet.Contains(w.Value));
            sentence.Tokens.RemoveAll(t => t is Word w && stopSet.Contains(w.Value));
        }
        return count;
    }

    public void ExportToXml(string outputPath)
    {
        var serializer = new XmlSerializer(typeof(Text));
        using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);
        serializer.Serialize(writer, this);
    }

    public override string ToString() => string.Join(" ", Sentences.Select(s => s.ToString()));
}
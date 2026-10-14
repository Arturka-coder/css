using System.Text;
using System.Text.RegularExpressions;
using laba_3.Models;

namespace laba_3.Services;

public static class TextParser
{
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
}
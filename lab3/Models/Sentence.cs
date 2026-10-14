using System.Text;
using System.Xml.Serialization;

namespace laba_3.Models;

[XmlRoot("sentence")]
public class Sentence
{
    [XmlElement("word")]
    public List<Word> Words { get; set; } = new();

    [XmlIgnore]
    public List<Token> Tokens { get; set; } = new();

    // Пустой конструктор обязателен для XmlSerializer!
    public Sentence() { }

    public int WordCount => Words.Count;
    public int FullLength => ToString().Length;

    public bool IsQuestion()
    {
        var lastPunct = Tokens.OfType<Punctuation>().LastOrDefault();
        return lastPunct != null && lastPunct.Value.Contains('?');
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Tokens.Count; i++)
        {
            var current = Tokens[i];
            sb.Append(current.Value);

            if (i < Tokens.Count - 1 && current is Word && Tokens[i + 1] is Word)
                sb.Append(' ');
        }
        return sb.ToString().Trim();
    }
}
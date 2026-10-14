using System.Text;
using System.Xml.Serialization;

namespace laba_3.Models;

[XmlRoot("sentence")]
public class Sentence
{
    [XmlElement("word")]
    public List<Word> Words { get; set; } = new();

    [XmlElement("punctuation")]
    public List<Punctuation> Punctuations { get; set; } = new();

    public Sentence() { }

    // Проверка на вопросительное предложение (нужно для пункта 3 ТЗ)
    public bool IsQuestion()
    {
        return Punctuations.Any(p => p.Value.Contains('?'));
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var word in Words)
        {
            sb.Append(word.Value).Append(" ");
        }
        return sb.ToString().Trim();
    }
}
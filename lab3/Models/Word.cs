using System.Xml.Serialization;

namespace laba_3.Models;

public class Word
{
    [XmlText]
    public string Value { get; set; } = string.Empty;

    public Word() { }

    public Word(string value)
    {
        Value=value;

    }

    public override string ToString() => Value;
}
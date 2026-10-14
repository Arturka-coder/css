using System.Xml.Serialization;

namespace laba_3.Models;

public class Punctuation : Token
{
    public override string Value { get; set; } = string.Empty;

    public Punctuation() { }

    public Punctuation(string value) => Value = value;

    public override string ToString() => Value;
}
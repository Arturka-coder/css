namespace lab4.Models;

public sealed class Word
{
    public Word(string value, int lineNumber)
    {
        Value = value;
        LineNumber = lineNumber;
    }

    public string Value { get; }
    public int LineNumber { get; }

    public override string ToString() => Value;
}
namespace laba_3.Models;

public class Symbol
{
    public char Value { get; set; }

    public Symbol(char value)
    {
        Value = value;
    }

    public bool IsVowel()
    {
        char c = char.ToLower(Value);
        return "aeiouyаеёиоуыэюя".Contains(c);
    }

    public bool IsConsonant() => char.IsLetter(Value) && !IsVowel();
}
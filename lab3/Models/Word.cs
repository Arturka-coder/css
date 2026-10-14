using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace laba_3.Models;

public class Word : Token
{
    private readonly List<Symbol> _symbols = new();

    public override string Value
    {
        get => string.Concat(_symbols.Select(s => s.Value));
        set
        {
            _symbols.Clear();
            if (!string.IsNullOrEmpty(value))
            {
                foreach (char c in value)
                    _symbols.Add(new Symbol(c));
            }
        }
    }

    public Word() { }

    public Word(string value) => Value = value;

    [XmlIgnore]
    public int Length => _symbols.Count;

    public bool StartsWithConsonant() => _symbols.Count > 0 && _symbols[0].IsConsonant();

    public override string ToString() => Value;
}
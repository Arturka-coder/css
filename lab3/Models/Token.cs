
using System.Xml.Serialization;

namespace laba_3.Models;

public abstract class Token
{
    [XmlText]
    public virtual string Value { get; set; } = string.Empty;
}
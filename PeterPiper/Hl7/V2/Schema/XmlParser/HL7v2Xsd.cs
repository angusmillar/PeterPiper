using System.Xml.Linq;

namespace PeterPiper.Hl7.V2.Schema.XmlParser;

public sealed class HL7v2Xsd
{
  public sealed class Filename
  {
    public const string DataTypes = "datatypes.xsd";
    public const string Fields = "fields.xsd";
    public const string Segments = "segments.xsd";
    public const string Messages = "messages.xsd";
  }

  public sealed class Namespace
  {
    public const string XMLSchema = "http://www.w3.org/2001/XMLSchema";
    public const string Hl7 = "urn:com.sun:encoder-hl7-1.0";
  }

  public sealed class Elements
  {
    public static readonly XName SimpleType = XName.Get("simpleType", Namespace.XMLSchema);
    public static readonly XName ComplexType = XName.Get("complexType", Namespace.XMLSchema);
    public static readonly XName Sequence = XName.Get("sequence", Namespace.XMLSchema);
    public static readonly XName Element = XName.Get("element", Namespace.XMLSchema);
    public static readonly XName Include = XName.Get("include", Namespace.XMLSchema);
    public static readonly XName Choice = XName.Get("choice", Namespace.XMLSchema);
    public static readonly XName Annotation = XName.Get("annotation", Namespace.XMLSchema);
    public static readonly XName Appinfo = XName.Get("appinfo", Namespace.XMLSchema);
    public static readonly XName Restriction = XName.Get("restriction", Namespace.XMLSchema);

    public static readonly XName Item = XName.Get("Item", Namespace.Hl7);
    public static readonly XName Type = XName.Get("Type", Namespace.Hl7);
    public static readonly XName LongName = XName.Get("LongName", Namespace.Hl7);
    public static readonly XName Table = XName.Get("Table", Namespace.Hl7);
  }

  public sealed class Attributes
  {
    public static readonly XName SchemaLocation = "schemaLocation";
    public static readonly XName Name = "name";
    public static readonly XName Base = "base";
    public static readonly XName Ref = "ref";
    public static readonly XName MinOccurs = "minOccurs";
    public static readonly XName MaxOccurs = "maxOccurs";
    public static readonly XName Source = "source";
  }
}
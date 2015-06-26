using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Glib.Hl7.V2.Schema.XmlParser
{
  public sealed class HL7v2Xsd
  {
    public sealed class Filename
    {
      public static string DataTypes = "datatypes.xsd";
      public static string Fields = "fields.xsd";
      public static string Segments = "segments.xsd";
      public static string Messages = "messages.xsd";
    }

    public sealed class Namespace
    {
      public static string XMLSchema = "http://www.w3.org/2001/XMLSchema";
      public static string Hl7 = "urn:com.sun:encoder-hl7-1.0";
    }

    public sealed class Elements
    {
      public static XName SimpleType = XName.Get("simpleType", HL7v2Xsd.Namespace.XMLSchema);
      public static XName ComplexType = XName.Get("complexType", HL7v2Xsd.Namespace.XMLSchema);
      public static XName Sequence = XName.Get("sequence", HL7v2Xsd.Namespace.XMLSchema);
      public static XName Element = XName.Get("element", HL7v2Xsd.Namespace.XMLSchema);
      public static XName Include = XName.Get("include", HL7v2Xsd.Namespace.XMLSchema);
      public static XName Choice = XName.Get("choice", HL7v2Xsd.Namespace.XMLSchema);
      public static XName Annotation = XName.Get("annotation", HL7v2Xsd.Namespace.XMLSchema);
      public static XName Appinfo = XName.Get("appinfo", HL7v2Xsd.Namespace.XMLSchema);
      public static XName Restriction = XName.Get("restriction", HL7v2Xsd.Namespace.XMLSchema);

      public static XName Item = XName.Get("Item", HL7v2Xsd.Namespace.Hl7);
      public static XName Type = XName.Get("Type", HL7v2Xsd.Namespace.Hl7);
      public static XName LongName = XName.Get("LongName", HL7v2Xsd.Namespace.Hl7);
      public static XName Table = XName.Get("Table", HL7v2Xsd.Namespace.Hl7);
    }

    public sealed class Attributes
    {
      public static XName SchemaLocation = "schemaLocation";
      public static XName Name = "name";
      public static XName Base = "base";
      public static XName Ref = "ref";
      public static XName MinOccurs = "minOccurs";
      public static XName MaxOccurs = "maxOccurs";
      public static XName Source = "source";
    }
  }
}

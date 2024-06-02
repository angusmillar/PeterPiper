using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PeterPiper.Hl7.V2.Schema.XmlParser;

public class FieldParser
{
  public Dictionary<string, Schema.Model.SegmentStructure> Run(XDocument xDocument, Dictionary<string, Schema.Model.SegmentStructure> oSegmentFieldDictionary)
  {
    var documentRoot = xDocument.Root;
    ArgumentNullException.ThrowIfNull(documentRoot);
    
    foreach (var detail in documentRoot.DescendantsAndSelf().Elements().Where(d => d.Name == HL7v2Xsd.Elements.ComplexType))
    {
      if (detail.Element(HL7v2Xsd.Elements.Sequence) != null)
      {
        XAttribute nameAttribute = detail.Attribute(HL7v2Xsd.Attributes.Name);
        ArgumentNullException.ThrowIfNull(nameAttribute);

        XElement SequenceElement = detail.Element(HL7v2Xsd.Elements.Sequence);
        ArgumentNullException.ThrowIfNull(SequenceElement);
        
        string SegmentCode = nameAttribute.Value.Split('.')[0].Trim();
        foreach (var item in SequenceElement.Elements(HL7v2Xsd.Elements.Element))
        {
          XAttribute refAttribute = item.Attribute(HL7v2Xsd.Attributes.Ref);
          ArgumentNullException.ThrowIfNull(refAttribute);
          
          XAttribute minOccursAttribute = item.Attribute(HL7v2Xsd.Attributes.MinOccurs);
          ArgumentNullException.ThrowIfNull(minOccursAttribute);

          XAttribute maxOccursAttribute = item.Attribute(HL7v2Xsd.Attributes.MaxOccurs);
          ArgumentNullException.ThrowIfNull(maxOccursAttribute);
          
          int position = Convert.ToInt32(refAttribute.Value.Split('.')[1]);
          string min = minOccursAttribute.Value;
          string max = maxOccursAttribute.Value;

          if (min == "1")
          {
            oSegmentFieldDictionary[SegmentCode].SegmentFieldList[position].IsMandatory = true;
          }
          else
          {
            oSegmentFieldDictionary[SegmentCode].SegmentFieldList[position].IsMandatory = false;
          }
          
          if (max.ToUpper() == "UNBOUNDED")
          {
            oSegmentFieldDictionary[SegmentCode].SegmentFieldList[position].CanRepeat = true;
          }
          else
          {
            oSegmentFieldDictionary[SegmentCode].SegmentFieldList[position].CanRepeat = false;
          }
          
        }
      }
    }
    return oSegmentFieldDictionary;
  }
}
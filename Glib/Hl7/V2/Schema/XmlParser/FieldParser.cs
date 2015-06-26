using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Glib.Hl7.V2.Schema.XmlParser
{
  public class FieldParser
  {
    public Dictionary<string, Schema.Model.SegmentStructure> Run(XDocument xDocument, Dictionary<string, Schema.Model.SegmentStructure> oSegmentFieldDictionary)
    {      
      foreach (var detail in xDocument.Root.DescendantsAndSelf().Elements().Where(d => d.Name == HL7v2Xsd.Elements.ComplexType))
      {
        if (detail.Element(HL7v2Xsd.Elements.Sequence) != null)
        {
          string SegmentCode = detail.Attribute(HL7v2Xsd.Attributes.Name).Value.Split('.')[0].Trim();
          foreach (var item in detail.Element(HL7v2Xsd.Elements.Sequence).Elements(HL7v2Xsd.Elements.Element))
          {
            int postion = Convert.ToInt32(item.Attribute(HL7v2Xsd.Attributes.Ref).Value.Split('.')[1]);
            string min = item.Attribute(HL7v2Xsd.Attributes.MinOccurs).Value;
            string max = item.Attribute(HL7v2Xsd.Attributes.MaxOccurs).Value;
            if (min == "1")
              oSegmentFieldDictionary[SegmentCode].SegmentFieldList[postion].IsMandatory = true;
            else
              oSegmentFieldDictionary[SegmentCode].SegmentFieldList[postion].IsMandatory = false;

            if (max.ToUpper() == "UNBOUNDED")
              oSegmentFieldDictionary[SegmentCode].SegmentFieldList[postion].CanRepeat = true;
            else
              oSegmentFieldDictionary[SegmentCode].SegmentFieldList[postion].CanRepeat = false;
          }
        }
      }
      return oSegmentFieldDictionary;
    }
  }
}

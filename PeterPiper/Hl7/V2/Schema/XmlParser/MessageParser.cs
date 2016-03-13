using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PeterPiper.Hl7.V2.Schema.XmlParser
{
  class MessageParser
  {
    public List<string> Run(XDocument xDocument)
    { 
      List<string> oFilnameList = new List<string>(); 
      foreach (var detail in xDocument.Root.DescendantsAndSelf().Elements().Where(d => d.Name == HL7v2Xsd.Elements.Include))
      {
        oFilnameList.Add(detail.Attribute(HL7v2Xsd.Attributes.SchemaLocation).Value);
      }
      return oFilnameList;
    }
  }
}

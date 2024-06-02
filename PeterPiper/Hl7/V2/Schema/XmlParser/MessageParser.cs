using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PeterPiper.Hl7.V2.Schema.XmlParser;

class MessageParser
{
  public List<string> Run(XDocument xDocument)
  { 
    List<string> fileNameList = new List<string>();

    var documentRoot = xDocument.Root;
    ArgumentNullException.ThrowIfNull(documentRoot);
    
    foreach (var detail in documentRoot.DescendantsAndSelf().Elements().Where(d => d.Name == HL7v2Xsd.Elements.Include))
    {
      XAttribute schemaLocationAttribute = detail.Attribute(HL7v2Xsd.Attributes.SchemaLocation);
      ArgumentNullException.ThrowIfNull(schemaLocationAttribute);
      
      fileNameList.Add(schemaLocationAttribute.Value);
    }
    return fileNameList;
  }
}
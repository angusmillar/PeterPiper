using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PeterPiper.Hl7.V2.Schema.XmlParser;

public class SegmentParser
{
  private XDocument _XDocument;
  public Dictionary<string, Model.SegmentStructure> Run(XDocument xDocument, List<Model.DataTypeBase> dataTypeList)
  {
    _XDocument = xDocument;
      
    Dictionary<string, Model.SegmentStructure> SegmentDictionary = new Dictionary<string, Model.SegmentStructure>();

    ArgumentNullException.ThrowIfNull(_XDocument.Root);
    
    foreach (var detail in _XDocument.Root.DescendantsAndSelf().Elements().Where(d => d.Name == HL7v2Xsd.Elements.ComplexType))
    {        
      ArgumentNullException.ThrowIfNull(detail.Attribute(HL7v2Xsd.Attributes.Name));
      
      string segmentFieldDescriptor = detail.Attribute(HL7v2Xsd.Attributes.Name)!.Value;

      if (segmentFieldDescriptor.Length < 2)
      {
        throw new Exception("The Segment Code and field position could not be sourced from the 'complexType' name attribute. " +
                            "XML File: " + HL7v2Xsd.Filename.Fields + @" XML path: root\complexType[name]");
      }
      
      var segmentCode = segmentFieldDescriptor.Split('.')[0];
      var fieldPosition = segmentFieldDescriptor.Split('.')[1];

      XElement annotationElement = detail.Element(HL7v2Xsd.Elements.Annotation);
      ArgumentNullException.ThrowIfNull(annotationElement);
      
      XElement fragment = (
        from x in annotationElement.Elements(HL7v2Xsd.Elements.Appinfo) 
        where x.Attribute(HL7v2Xsd.Attributes.Source) is not null &  
              x.Attribute(HL7v2Xsd.Attributes.Source)!.Value == "urn:com.sun:encoder" 
        select x).Single();
      
      XElement itemElement = fragment.Element(HL7v2Xsd.Elements.Item);
      ArgumentNullException.ThrowIfNull(itemElement);
      
      XElement typeElement = fragment.Element(HL7v2Xsd.Elements.Type);
      ArgumentNullException.ThrowIfNull(typeElement);
      
      XElement longNameElement = fragment.Element(HL7v2Xsd.Elements.LongName);
      ArgumentNullException.ThrowIfNull(longNameElement);
      
      string item = itemElement.Value;
      string type = typeElement.Value;
      string longName = longNameElement.Value;
      
      string table = string.Empty;
      if (fragment.Element(HL7v2Xsd.Elements.Table) != null)
      {
        XElement tableElement = fragment.Element(HL7v2Xsd.Elements.Table);
        ArgumentNullException.ThrowIfNull(tableElement);
        table = tableElement.Value;
      }
      
      if (!SegmentDictionary.ContainsKey(segmentCode))
      {
        Model.SegmentStructure oSegment = new Model.SegmentStructure();
        oSegment.Code = segmentCode.Trim();
        SegmentDictionary.Add(oSegment.Code, oSegment);
      }

      Model.SegmentField oSegField = new Model.SegmentField();
      try
      {
        oSegField.ItemNumber = Convert.ToInt32(item);
      }
      catch (Exception Exec)
      {
        throw new Exception("The HL7 item number can not be converted to an integer. \n" +
                            "Item was Segment: " + segmentCode + ", Field: " + fieldPosition + 
                            ", item that could not be converted to an integer was: " + item, Exec);
      }

      if (table != string.Empty)
      {
        try
        {
          oSegField.Hl7TableIndex = Convert.ToInt32(table.Substring(3, table.Length - 3));
        }
        catch (Exception Exec)
        {
          throw new Exception("The HL7 Table number can not be converted to an integer. \n" +
                              "Item was Segment: " + segmentCode + ", Field: " + fieldPosition +
                              ", Table number that could not be converted to an integer was: " + table, Exec);
        }
      }
      else
      {
        oSegField.Hl7TableIndex = 0;
      }
                
      oSegField.Description = longName;
      try
      {
        //the xsd before V2.5 use "*' for the varies datatype and then post V2.5 they being to use "var" instead of "*".
        //here I map back to "*".
        if (type == "var")
          type = "*";
        oSegField.DataType = dataTypeList.Single(x => x.Code == type);
      }
      catch (Exception Exec)
      {
        throw new Exception("A Field's data type can not be located in the parsed data type list. \n" +
                            "data type was found in Segment: " + segmentCode + ", Field: " + fieldPosition +
                            ", the data type code that could not be found was: " + type, Exec);

      }
      int FieldPositionInteger;
      try
      {
        FieldPositionInteger = Convert.ToInt32(fieldPosition);
      }
      catch (Exception Exec)
      {
        throw new Exception("The Field position number could not be converted to an integer. Source of field number was from the 'complexType' name attribute as follows: \n" +
                            "XML File: " + HL7v2Xsd.Filename.Fields + @" XML path: root\complexType[name], the data found for the field position was: " + fieldPosition, Exec);
      }
      SegmentDictionary[segmentCode.Trim()].SegmentFieldList.Add(FieldPositionInteger, oSegField);
    }
    //Here we add a Z segment which has no fields as Z segments are custom segments and the 
    //fields any implementation uses are undefined by the HL7 Standard.
    Model.SegmentStructure oZSegment = new Model.SegmentStructure();      
    oZSegment.Code = "anyZSegment";
    SegmentDictionary.Add(oZSegment.Code, oZSegment);

    //Here we add a 'anyHL7Segment' which has no fields as any segment type is only known at run time      
    Model.SegmentStructure oAnySegment = new Model.SegmentStructure();      
    oAnySegment.Code = "anyHL7Segment";
    SegmentDictionary.Add(oAnySegment.Code, oZSegment);
      
    return SegmentDictionary;
  }  
}
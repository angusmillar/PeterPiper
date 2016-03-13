using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PeterPiper.Hl7.V2.Schema.XmlParser
{
  public class SegmentParser
  {
    private XDocument xDoc = null;
    public Dictionary<string, Schema.Model.SegmentStructure> Run(XDocument xDocument, List<Model.DataTypeBase> oDataTypeList)
    {
      xDoc = xDocument;
      
      Dictionary<string, Schema.Model.SegmentStructure> SegmentDictionary = new Dictionary<string, Model.SegmentStructure>();

      foreach (var detail in xDoc.Root.DescendantsAndSelf().Elements().Where(d => d.Name == HL7v2Xsd.Elements.ComplexType))
      {        
        string SegmentFieldDescriptor = detail.Attribute(HL7v2Xsd.Attributes.Name).Value;
        string SegmentCode = string.Empty;
        string FieldPosition = string.Empty;
        try
        {
          SegmentCode = SegmentFieldDescriptor.Split('.')[0];
          FieldPosition = SegmentFieldDescriptor.Split('.')[1];          
        }
        catch (Exception Exec)
        {
          throw new Exception("The Segment Code and field position could not be sourced from the 'complexType' name attribute. \n" +
                              "XML File: " + HL7v2Xsd.Filename.Fields + @" XML path: root\complexType[name]", Exec);
        }
        XElement fragment = (from x in detail.Element(HL7v2Xsd.Elements.Annotation).Elements(HL7v2Xsd.Elements.Appinfo) where x.Attribute(HL7v2Xsd.Attributes.Source).Value == "urn:com.sun:encoder" select x).Single();
        string Item = fragment.Element(HL7v2Xsd.Elements.Item).Value;
        string Type = fragment.Element(HL7v2Xsd.Elements.Type).Value;
        var LongName = fragment.Element(HL7v2Xsd.Elements.LongName).Value;
        string Table = string.Empty;
        if (fragment.Element(HL7v2Xsd.Elements.Table) != null)
          Table = fragment.Element(HL7v2Xsd.Elements.Table).Value;

        if (!SegmentDictionary.ContainsKey(SegmentCode))
        {
          Schema.Model.SegmentStructure oSegment = new Model.SegmentStructure();
          oSegment.Code = SegmentCode.Trim();
          SegmentDictionary.Add(oSegment.Code, oSegment);
        }

        Schema.Model.SegmentField oSegField = new Model.SegmentField();
        try
        {
          oSegField.ItemNumber = Convert.ToInt32(Item);
        }
        catch (Exception Exec)
        {
          throw new Exception("The HL7 item number can not be converted to an integer. \n" +
                              "Item was Segment: " + SegmentCode + ", Field: " + FieldPosition + 
                              ", item that could not be converted to an integer was: " + Item, Exec);
        }

        if (Table != string.Empty)
        {
          try
          {
            oSegField.Hl7TableIndex = Convert.ToInt32(Table.Substring(3, Table.Length - 3));
          }
          catch (Exception Exec)
          {
            throw new Exception("The HL7 Table number can not be converted to an integer. \n" +
                                "Item was Segment: " + SegmentCode + ", Field: " + FieldPosition +
                                ", Table number that could not be converted to an integer was: " + Table, Exec);
          }
        }
        else
        {
          oSegField.Hl7TableIndex = 0;
        }
                
        oSegField.Description = LongName;
        try
        {
          //the xsd before V2.5 use "*' for the varies datatype and then post V2.5 they being to use "var" instead of "*".
          //here I map back to "*".
          if (Type == "var")
            Type = "*";
          oSegField.DataType = oDataTypeList.Single(x => x.Code == Type);
        }
        catch (Exception Exec)
        {
          throw new Exception("A Field's data type can not be located in the parsed data type list. \n" +
                              "data type was found in Segment: " + SegmentCode + ", Field: " + FieldPosition +
                              ", the data type code that could not be found was: " + Type, Exec);

        }
        int FieldPositionInteger = 0;
        try
        {
          FieldPositionInteger = Convert.ToInt32(FieldPosition);
        }
        catch (Exception Exec)
        {
          throw new Exception("The Field position number could not be converted to an integer. Source of field number was from the 'complexType' name attribute as follows: \n" +
                              "XML File: " + HL7v2Xsd.Filename.Fields + @" XML path: root\complexType[name], the data found for the field position was: " + FieldPosition, Exec);
        }
        SegmentDictionary[SegmentCode.Trim()].SegmentFieldList.Add(FieldPositionInteger, oSegField);
      }
      //Here we add a Z segment which has no fields as Z segments are custom segments and the 
      //fields any implementation uses are undefined by the HL7 Standard.
      Schema.Model.SegmentStructure oZSegment = new Model.SegmentStructure();      
      oZSegment.Code = "anyZSegment";;
      SegmentDictionary.Add(oZSegment.Code, oZSegment);

      //Here we add a 'anyHL7Segment' which has no fields as any segment type is only known at run time      
      Schema.Model.SegmentStructure oAnySegment = new Model.SegmentStructure();      
      oAnySegment.Code = "anyHL7Segment";
      SegmentDictionary.Add(oAnySegment.Code, oZSegment);
      
      return SegmentDictionary;
    }  
  }
}

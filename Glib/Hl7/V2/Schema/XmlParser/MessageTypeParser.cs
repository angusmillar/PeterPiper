using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Glib.Hl7.V2.Schema.XmlParser
{
  public class MessageTypeParser
  {
    private Dictionary<string, Schema.Model.MessageSegmentGroup> MessageSegmentGroupDic = new Dictionary<string, Model.MessageSegmentGroup>();
    private Dictionary<string, Schema.Model.SegmentStructure> oSegmentDictionary;
    private Schema.Model.MessageStructure oMessageStructure;

    public Schema.Model.MessageStructure Run(XDocument xDocument, Dictionary<string, Schema.Model.SegmentStructure> oSegmentDic)
    {
      oSegmentDictionary = oSegmentDic;
      oMessageStructure = new Model.MessageStructure();
      
      foreach (var detail in xDocument.Root.DescendantsAndSelf().Elements().Where(d => d.Name == HL7v2Xsd.Elements.ComplexType))
      {
        if (String.IsNullOrWhiteSpace(oMessageStructure.MessageType))
          ResolveMessageTypeAndEvent(oMessageStructure, detail);

        //Covers some errors in the standard which render some message types as not implementable. 
        if (EdgeCaseResolution(oMessageStructure, detail))
          return oMessageStructure;

        if (detail.Attribute(HL7v2Xsd.Attributes.Name).Value.Split('.')[1].Trim() == "CONTENT")
        {
          foreach (var element in detail.Elements(HL7v2Xsd.Elements.Sequence).Elements(HL7v2Xsd.Elements.Element))
          {
            if (element.Attribute(HL7v2Xsd.Attributes.Ref).Value.Contains('.'))
            {
              oMessageStructure.MessageItemList.Add(
                MessageSegmentGroupDic[element.Attribute(HL7v2Xsd.Attributes.Ref).Value.Split('.')[1].Trim()]);                
            }
            else
            {
              oMessageStructure.MessageItemList.Add(ResolveMessageSegment(element));
            }
          }   
        }
        else if (detail.Attribute(HL7v2Xsd.Attributes.Name).Value.Split('.')[1].Trim() != "CONTENT")
        {    
          if (detail.Element(HL7v2Xsd.Elements.Sequence) != null)
          {

            ResolveMessageSegmentGroup(detail.Attribute(HL7v2Xsd.Attributes.Name).Value.Split('.')[1].Trim(),
                                     detail.Element(HL7v2Xsd.Elements.Sequence));
          }
          else
          {                       
            ResolveMessageSegmentGroup(detail.Attribute(HL7v2Xsd.Attributes.Name).Value.Split('.')[1].Trim(),
                                        detail.Element(HL7v2Xsd.Elements.Choice));
          }                                        
        }
        else
        {
          throw new Exception("Unexpected name value found: " + HL7v2Xsd.Attributes.Name.ToString() + "=" + detail.Attribute(HL7v2Xsd.Attributes.Name).Value);
        }

      }
      return oMessageStructure;
    }

    private bool EdgeCaseResolution(Model.MessageStructure oMessageStructure, XElement detail)
    {
      if (oMessageStructure.MessageType == "SUR" && oMessageStructure.MessageEvent == "P09")
      {
        oMessageStructure.Notes = "This message and event is deprecated for v2.5 due to it being " +
                                   "technical flawed and not implementable Refer to Chapter 7.11.2 - SUR " +
                                   "for more information.";
        return true;
      }
      return false;
    }

    private static void ResolveMessageTypeAndEvent(Schema.Model.MessageStructure oMessageStructure, XElement detail)
    {     
      string MessageTypeAndEvent = string.Empty;
      MessageTypeAndEvent = detail.Attribute(HL7v2Xsd.Attributes.Name).Value.Split('.')[0].Trim();

      //Work out the MessageType and Event from the first group found,
      if (MessageTypeAndEvent.Contains('_'))
      {
        oMessageStructure.MessageType = MessageTypeAndEvent.Split('_')[0].Trim();
        oMessageStructure.MessageEvent = MessageTypeAndEvent.Split('_')[1].Trim();
      }
      else
      {
        oMessageStructure.MessageType = MessageTypeAndEvent;
        oMessageStructure.MessageEvent = String.Empty;
      }
    }

    private Schema.Model.MessageSegmentGroup ResolveMessageSegmentGroup(String GroupName, XElement Sequence)
    {                  
      Schema.Model.MessageSegmentGroup oMsgSegGroup = new Model.MessageSegmentGroup();
      oMsgSegGroup.SegmentGroupName = GroupName;
      foreach (var element in Sequence.Elements(HL7v2Xsd.Elements.Element))
      {
        string Ref = element.Attribute(HL7v2Xsd.Attributes.Ref).Value.Trim();                
        bool CanRepeat = (element.Attribute(HL7v2Xsd.Attributes.MaxOccurs).Value.Trim().ToUpper() == "UNBOUNDED");
        bool IsMandatory = (element.Attribute(HL7v2Xsd.Attributes.MinOccurs).Value.Trim().ToUpper() == "1");

        if (Ref.Contains('.'))
        {
          Schema.Model.MessageSegmentGroup oSubSegGroup = new Model.MessageSegmentGroup();
          oSubSegGroup.CanRepeat = CanRepeat;
          oSubSegGroup.IsMandatory = IsMandatory;
          if (MessageSegmentGroupDic.ContainsKey(Ref.Split('.')[1]))
          {
            oSubSegGroup.SegmentGroupItemList = MessageSegmentGroupDic[Ref.Split('.')[1]].SegmentGroupItemList;
            oSubSegGroup.SegmentGroupName = MessageSegmentGroupDic[Ref.Split('.')[1]].SegmentGroupName;
            oMsgSegGroup.SegmentGroupItemList.Add(oSubSegGroup);
          }
          else
          {
            throw new Exception(String.Format("The Message structure for {0}^{1} has a segment group named {3} which is not defined within the same file.", oMessageStructure.MessageType, oMessageStructure.MessageEvent, Ref.Split('.')[1]));
          }
        }
        else
        {
          oMsgSegGroup.SegmentGroupItemList.Add(ResolveMessageSegment(element));
        }
      }
      MessageSegmentGroupDic.Add(oMsgSegGroup.SegmentGroupName, oMsgSegGroup);
      return oMsgSegGroup;
    }

    private Schema.Model.MessageSegment ResolveMessageSegment(XElement Element)
    {      
      Schema.Model.MessageSegment oMsgSeg = new Model.MessageSegment();
      string SegmentCode = Element.Attribute(HL7v2Xsd.Attributes.Ref).Value.Trim();
      string min = Element.Attribute(HL7v2Xsd.Attributes.MinOccurs).Value.Trim();
      string max = Element.Attribute(HL7v2Xsd.Attributes.MaxOccurs).Value.Trim();
      oMsgSeg.CanRepeat = (max.ToUpper() == "UNBOUNDED");
      oMsgSeg.IsMandatory = (min.ToUpper() == "1");
      oMsgSeg.Segment = oSegmentDictionary[SegmentCode];
      return oMsgSeg;
    }
  }
}

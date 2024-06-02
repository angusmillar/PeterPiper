using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PeterPiper.Hl7.V2.Schema.XmlParser;

public class MessageTypeParser
{
    private readonly Dictionary<string, Model.MessageSegmentGroup> _MessageSegmentGroupDic =
        new Dictionary<string, Model.MessageSegmentGroup>();

    private Dictionary<string, Model.SegmentStructure> _SegmentDictionary;
    private Model.MessageStructure _MessageStructure;

    public Model.MessageStructure Run(XDocument xDocument, Dictionary<string, Model.SegmentStructure> segmentDictionary)
    {
        _SegmentDictionary = segmentDictionary;
        _MessageStructure = new Model.MessageStructure();

        XElement documentRootElement = xDocument.Root;
        ArgumentNullException.ThrowIfNull(documentRootElement);

        foreach (var detail in documentRootElement.DescendantsAndSelf().Elements()
                     .Where(d => d.Name == HL7v2Xsd.Elements.ComplexType))
        {
            if (String.IsNullOrWhiteSpace(_MessageStructure.MessageType))
            {
                ResolveMessageTypeAndEvent(_MessageStructure, detail);
            }

            //Covers some errors in the standard which render some message types as not implementable. 
            if (EdgeCaseResolution(_MessageStructure, detail))
            {
                return _MessageStructure;
            }

            XAttribute nameAttribute = detail.Attribute(HL7v2Xsd.Attributes.Name);
            ArgumentNullException.ThrowIfNull(nameAttribute);


            if (nameAttribute.Value.Split('.')[1].Trim().Equals("CONTENT"))
            {
                foreach (var element in detail.Elements(HL7v2Xsd.Elements.Sequence).Elements(HL7v2Xsd.Elements.Element))
                {
                    XAttribute refAttribute = element.Attribute(HL7v2Xsd.Attributes.Ref);
                    ArgumentNullException.ThrowIfNull(refAttribute);

                    if (refAttribute.Value.Contains('.'))
                    {
                        _MessageStructure.MessageItemList.Add(
                            _MessageSegmentGroupDic[refAttribute.Value.Split('.')[1].Trim()]);
                    }
                    else
                    {
                        _MessageStructure.MessageItemList.Add(ResolveMessageSegment(element));
                    }
                }
            }
            else if (!nameAttribute.Value.Split('.')[1].Trim().Equals("CONTENT"))
            {
                if (detail.Element(HL7v2Xsd.Elements.Sequence) != null)
                {
                    ResolveMessageSegmentGroup(nameAttribute.Value.Split('.')[1].Trim(),
                        detail.Element(HL7v2Xsd.Elements.Sequence));
                }
                else
                {
                    ResolveMessageSegmentGroup(nameAttribute.Value.Split('.')[1].Trim(),
                        detail.Element(HL7v2Xsd.Elements.Choice));
                }
            }
            else
            {
                throw new Exception("Unexpected name value found: " + HL7v2Xsd.Attributes.Name + "=" +
                                    nameAttribute.Value);
            }
        }

        return _MessageStructure;
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

    private static void ResolveMessageTypeAndEvent(Model.MessageStructure messageStructure, XElement detail)
    {
        XAttribute nameAttribute = detail.Attribute(HL7v2Xsd.Attributes.Name);
        ArgumentNullException.ThrowIfNull(nameAttribute);

        string MessageTypeAndEvent = nameAttribute.Value.Split('.')[0].Trim();

        //Work out the MessageType and Event from the first group found,
        if (MessageTypeAndEvent.Contains('_'))
        {
            messageStructure.MessageType = MessageTypeAndEvent.Split('_')[0].Trim();
            messageStructure.MessageEvent = MessageTypeAndEvent.Split('_')[1].Trim();
        }
        else
        {
            messageStructure.MessageType = MessageTypeAndEvent;
            messageStructure.MessageEvent = String.Empty;
        }
    }

    private Model.MessageSegmentGroup ResolveMessageSegmentGroup(String groupName, XElement sequence)
    {
        Model.MessageSegmentGroup oMsgSegGroup = new Model.MessageSegmentGroup();
        oMsgSegGroup.SegmentGroupName = groupName;
        foreach (var element in sequence.Elements(HL7v2Xsd.Elements.Element))
        {
            XAttribute refAttribute = element.Attribute(HL7v2Xsd.Attributes.Ref);
            ArgumentNullException.ThrowIfNull(refAttribute);

            XAttribute minOccursAttribute = element.Attribute(HL7v2Xsd.Attributes.MinOccurs);
            ArgumentNullException.ThrowIfNull(minOccursAttribute);

            XAttribute maxOccursAttribute = element.Attribute(HL7v2Xsd.Attributes.MaxOccurs);
            ArgumentNullException.ThrowIfNull(maxOccursAttribute);

            string Ref = refAttribute.Value.Trim();
            bool IsMandatory = (minOccursAttribute.Value.Trim().ToUpper() == "1");
            bool CanRepeat = (maxOccursAttribute.Value.Trim().ToUpper() == "UNBOUNDED");

            if (Ref.Contains('.'))
            {
                Model.MessageSegmentGroup oSubSegGroup = new Model.MessageSegmentGroup();
                oSubSegGroup.CanRepeat = CanRepeat;
                oSubSegGroup.IsMandatory = IsMandatory;
                if (_MessageSegmentGroupDic.ContainsKey(Ref.Split('.')[1]))
                {
                    oSubSegGroup.SegmentGroupItemList = _MessageSegmentGroupDic[Ref.Split('.')[1]].SegmentGroupItemList;
                    oSubSegGroup.SegmentGroupName = _MessageSegmentGroupDic[Ref.Split('.')[1]].SegmentGroupName;
                    oMsgSegGroup.SegmentGroupItemList.Add(oSubSegGroup);
                }
                else
                {
                    throw new Exception(
                        $"The Message structure for {_MessageStructure.MessageType}^{_MessageStructure.MessageEvent} has a segment group named {Ref.Split('.')[1]} which is not defined within the same file.");
                }
            }
            else
            {
                oMsgSegGroup.SegmentGroupItemList.Add(ResolveMessageSegment(element));
            }
        }

        _MessageSegmentGroupDic.Add(oMsgSegGroup.SegmentGroupName, oMsgSegGroup);
        return oMsgSegGroup;
    }

    private Model.MessageSegment ResolveMessageSegment(XElement element)
    {
        Model.MessageSegment oMsgSeg = new Model.MessageSegment();

        XAttribute refAttribute = element.Attribute(HL7v2Xsd.Attributes.Ref);
        ArgumentNullException.ThrowIfNull(refAttribute);

        XAttribute minOccursAttribute = element.Attribute(HL7v2Xsd.Attributes.MinOccurs);
        ArgumentNullException.ThrowIfNull(minOccursAttribute);

        XAttribute maxOccursAttribute = element.Attribute(HL7v2Xsd.Attributes.MaxOccurs);
        ArgumentNullException.ThrowIfNull(maxOccursAttribute);

        string SegmentCode = refAttribute.Value.Trim();
        string min = minOccursAttribute.Value.Trim();
        string max = maxOccursAttribute.Value.Trim();
        oMsgSeg.CanRepeat = (max.ToUpper() == "UNBOUNDED");
        oMsgSeg.IsMandatory = (min.ToUpper() == "1");
        oMsgSeg.Segment = _SegmentDictionary[SegmentCode];
        return oMsgSeg;
    }
}
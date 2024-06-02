using System;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support.Tools;

namespace PeterPiper.Hl7.V2.Support.Standard;

public class Acknowledgement
{
  private const string AcknowledgementMessageStructureCode = "ACK";
  private const string MSAAcknowledgementMessageText = "HL7 Acknowledgment";
  /// <summary>
  /// Builds a HL7 Acknowledgement message for the given HL7 Message.
  /// The Acknowledgement code will be the code string for the given AcknowledgmentCodeType    
  /// </summary>
  /// 
  /// <param name="oReceivedMessage"></param>
  /// <param name="acknowledgmentType"></param>
  /// <returns></returns>
  public IMessage GenerateAcknowledgementMessage(IMessage oReceivedMessage, Hl7Table.Table_0008.AcknowledgmentCodeType acknowledgmentType)
  {    
    return BuildAcknowledgementMessage(oReceivedMessage, Hl7Table.Table_0008.ConvertTypeToCode(acknowledgmentType));    
  }    
  private IMessage BuildAcknowledgementMessage(IMessage receivedMessage, string acknowledgmentCode)
  {
    IMessage oAckMessage = Creator.Message(receivedMessage.MessageVersion, AcknowledgementMessageStructureCode, "", receivedMessage.MessageControlID);
    oAckMessage.Segment(Segments.Msh.Code).Field(15).ClearAll();
    oAckMessage.Segment(Segments.Msh.Code).Field(16).ClearAll();
    oAckMessage.Segment(Segments.Msh.Code).Field(7).Convert.DateTime.SetDateTimeOffset(DateTimeOffset.Now, true, DateTimeSupportTools.DateTimePrecision.DateHourMinSecMilli);      
    oAckMessage.Segment(Segments.Msh.Code).Field(3).AsStringRaw = receivedMessage.Segment(Segments.Msh.Code).Field(5).AsStringRaw;
    oAckMessage.Segment(Segments.Msh.Code).Field(4).AsStringRaw = receivedMessage.Segment(Segments.Msh.Code).Field(6).AsStringRaw;
    oAckMessage.Segment(Segments.Msh.Code).Field(5).AsStringRaw = receivedMessage.Segment(Segments.Msh.Code).Field(3).AsStringRaw;
    oAckMessage.Segment(Segments.Msh.Code).Field(6).AsStringRaw = receivedMessage.Segment(Segments.Msh.Code).Field(4).AsStringRaw;
    
    ISegment oMsa = Creator.Segment(Segments.Msa.Code);
    oMsa.Field(1).AsString = acknowledgmentCode;
    oMsa.Field(2).AsString = receivedMessage.MessageControlID;
    oMsa.Field(3).AsString = MSAAcknowledgementMessageText;      
    oAckMessage.Add(oMsa);
    
    return oAckMessage;
  }
}
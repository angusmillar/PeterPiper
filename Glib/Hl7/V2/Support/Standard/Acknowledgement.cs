using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.Hl7.V2.Model;

namespace Glib.Hl7.V2.Support.Standard
{
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
    /// <param name="AcknowledgmentType"></param>
    /// <returns></returns>
    public Message GenerateAcknowledgementMessage(Message oReceivedMessage, Hl7Table.Table_0008.AcknowledgmentCodeType AcknowledgmentType)
    {    
      return BuildAcknowledgementMessage(oReceivedMessage, Support.Standard.Hl7Table.Table_0008.ConvertTypeToCode(AcknowledgmentType));    
    }    
    private Message BuildAcknowledgementMessage(Message oRecivedMessage, string AcknowledgmentCode)
    {
      Message oAckMessage = new Message(oRecivedMessage.MessageVersion, AcknowledgementMessageStructureCode, "", oRecivedMessage.MessageControlID);
      oAckMessage.Segment(Support.Standard.Segments.Msh.Code).Field(15).ClearAll();
      oAckMessage.Segment(Support.Standard.Segments.Msh.Code).Field(16).ClearAll();
      oAckMessage.Segment(Support.Standard.Segments.Msh.Code).Field(7).DateTimeSupport.SetDateTimeOffset(DateTimeOffset.Now, true, Content.DateTimeSupportTools.DateTimePrecision.DateHourMinSecMilli);      
      oAckMessage.Segment(Support.Standard.Segments.Msh.Code).Field(3).AsStringRaw = oRecivedMessage.Segment(Support.Standard.Segments.Msh.Code).Field(5).AsStringRaw;
      oAckMessage.Segment(Support.Standard.Segments.Msh.Code).Field(4).AsStringRaw = oRecivedMessage.Segment(Support.Standard.Segments.Msh.Code).Field(6).AsStringRaw;
      oAckMessage.Segment(Support.Standard.Segments.Msh.Code).Field(5).AsStringRaw = oRecivedMessage.Segment(Support.Standard.Segments.Msh.Code).Field(3).AsStringRaw;
      oAckMessage.Segment(Support.Standard.Segments.Msh.Code).Field(6).AsStringRaw = oRecivedMessage.Segment(Support.Standard.Segments.Msh.Code).Field(4).AsStringRaw;
      Segment oMSA = new Segment(Support.Standard.Segments.Msa.Code);
      oMSA.Field(1).AsString = AcknowledgmentCode;
      oMSA.Field(2).AsString = oRecivedMessage.MessageControlID;
      oMSA.Field(3).AsString = MSAAcknowledgementMessageText;      
      oAckMessage.Add(oMSA);
      return oAckMessage;
    }
  }
}

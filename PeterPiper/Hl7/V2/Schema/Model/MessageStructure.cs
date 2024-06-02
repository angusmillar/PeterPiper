
using System.Collections.Generic;

namespace PeterPiper.Hl7.V2.Schema.Model;

public class MessageStructure
{
  public string MessageType { get; set; }
  public string  MessageEvent { get; set; }

  public string MessageTypeDescription { get; set; }
  public string Notes { get; set; }
  public List<MessageItemBase> MessageItemList { get; set; } = new ();
}
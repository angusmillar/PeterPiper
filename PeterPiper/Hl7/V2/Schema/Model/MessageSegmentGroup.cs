
using System.Collections.Generic;

namespace PeterPiper.Hl7.V2.Schema.Model;

public class MessageSegmentGroup : MessageItemBase
{
  public string SegmentGroupName { get; set; }

  public List<MessageItemBase> SegmentGroupItemList { get; set; } = new ();
}
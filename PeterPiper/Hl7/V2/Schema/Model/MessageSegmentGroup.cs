using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterPiper.Hl7.V2.Schema.Model
{
  public class MessageSegmentGroup : MessageItemBase
  {
    private string _SegmentGroupName;
    public string SegmentGroupName
    {
      get { return _SegmentGroupName; }
      set { _SegmentGroupName = value; }
    }
    
    private List<MessageItemBase> _SegmentGroupItemList = new List<MessageItemBase>();
    public List<MessageItemBase> SegmentGroupItemList
    {
      get { return _SegmentGroupItemList; }
      set { _SegmentGroupItemList = value; }
    }

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Schema.Model
{
  public class MessageSegment : MessageItemBase
  {
    private SegmentStructure _Segment;
    public SegmentStructure Segment
    {
      get { return _Segment; }
      set { _Segment = value; }
    }

  }
}

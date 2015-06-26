using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Schema.Model
{
  public class SegmentStructure
  {
    private string  _Code;
    public string  Code
    {
      get { return _Code; }
      set { _Code = value; }
    }

    private Dictionary<int, SegmentField> _SegmentFieldList = new Dictionary<int, SegmentField>();
    public Dictionary<int, SegmentField> SegmentFieldList
    {
      get { return _SegmentFieldList; }
      set { _SegmentFieldList = value; }
    }
  }
}

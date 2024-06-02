
using System.Collections.Generic;

namespace PeterPiper.Hl7.V2.Schema.Model;

public class SegmentStructure
{
  public string  Code { get; set; }
  public Dictionary<int, SegmentField> SegmentFieldList { get; set; } = new ();
}

using System.Collections.Generic;

namespace PeterPiper.Hl7.V2.Schema.Model;

public class VersionSchema
{
  public VersionsSupported Version { get; set; }
  public List<MessageStructure> MessageStructureList { get; set; }
  public IEnumerable<Primitive> PrimitiveList { get; set; }
  public IEnumerable<Composite> CompositeList { get; set; }
  public Dictionary<string, SegmentStructure> SegmentDictionary { get; set; }
}
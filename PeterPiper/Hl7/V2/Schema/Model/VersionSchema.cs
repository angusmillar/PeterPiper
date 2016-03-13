using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterPiper.Hl7.V2.Schema.Model
{
  public class VersionSchema
  {
    private Model.VersionsSupported _Version;
    public Model.VersionsSupported Version
    {
      get { return _Version; }
      set { _Version = value; }
    }

    private List<Schema.Model.MessageStructure> _MessageStructureList;
    public List<Schema.Model.MessageStructure> MessageStructureList
    {
      get { return _MessageStructureList; }
      set { _MessageStructureList = value; }
    }

    private IEnumerable<Primitive> _PrimitiveList;
    public IEnumerable<Primitive> PrimitiveList
    {
      get { return _PrimitiveList; }
      set { _PrimitiveList = value; }
    }

    private IEnumerable<Composite> _CompositeList;
    public IEnumerable<Composite> CompositeList
    {
      get { return _CompositeList; }
      set { _CompositeList = value; }
    }

    private Dictionary<string,SegmentStructure> _SegmentDictionary;
    public Dictionary<string, SegmentStructure> SegmentDictionary
    {
      get { return _SegmentDictionary; }
      set { _SegmentDictionary = value; }
    }
    


  }
}

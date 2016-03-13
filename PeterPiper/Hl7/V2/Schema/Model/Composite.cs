using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterPiper.Hl7.V2.Schema.Model
{
  public class Composite : DataTypeBase
  {    
    private Dictionary<int,CompositeItem> _CompositeItem = new Dictionary<int,CompositeItem>();
    public Dictionary<int, CompositeItem> CompositeItem
    {
      get { return _CompositeItem; }
      set { _CompositeItem = value; }
    }

  }

}

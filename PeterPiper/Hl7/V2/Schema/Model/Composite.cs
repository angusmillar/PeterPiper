
using System.Collections.Generic;

namespace PeterPiper.Hl7.V2.Schema.Model;

public class Composite : DataTypeBase
{
  public Dictionary<int, CompositeItem> CompositeItem { get; set; } = new ();
}
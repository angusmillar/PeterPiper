using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public abstract class Any : Hxit
  {
    private Enum.NullFlavor _NullFlavor;
    public Enum.NullFlavor NullFlavor
    {
      get { return _NullFlavor; }
      set { _NullFlavor = value; }
    }        
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public class Integer : QTY
  {
    private int _value;
    public int value
    {
      get { return _value; }
      set { _value = value; }
    }
    
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public class Real : QTY
  {
    private decimal _value;
    public decimal value
    {
      get { return _value; }
      set { _value = value; }
    }
    
  }
}

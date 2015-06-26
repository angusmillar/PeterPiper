using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public class Quantity : QTY
  {
    private decimal _value;
    public decimal value
    {
      get { return _value; }
      set { _value = value; }
    }

    private CodedText _unit;
    public CodedText unit
    {
      get { return _unit; }
      set { _unit = value; }
    }
  }
}

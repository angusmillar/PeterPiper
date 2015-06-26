using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public class QuantityRatio : QTY
  {
    private Quantity _denominator;
    public Quantity denominator
    {
      get { return _denominator; }
      set { _denominator = value; }
    }

    private Quantity _numerator;
    public Quantity numerator
    {
      get { return _numerator; }
      set { _numerator = value; }
    }
    
  }
}

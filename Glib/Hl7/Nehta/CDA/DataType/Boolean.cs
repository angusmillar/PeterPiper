using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public class Boolean : Any
  {
    private bool _boolean_literal;
    public bool boolean_literal
    {
      get { return _boolean_literal; }
      set { _boolean_literal = value; }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public class Link : Any
  {
    private string _value;
    public string value
    {
      get { return _value; }
      set { _value = value; }
    }
  }
}

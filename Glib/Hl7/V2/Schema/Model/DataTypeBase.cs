using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Schema.Model
{
  abstract public class DataTypeBase
  {
    private string _Code;
    public string Code
    {
      get { return _Code; }
      set { _Code = value; }
    }
  }
}

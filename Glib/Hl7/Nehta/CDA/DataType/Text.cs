using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public class Text : Any
  {
    private string _value;
    public string value
    {
      get { return _value; }
      set { _value = value; }
    }

    private CodedText _language;
    public CodedText language
    {
      get { return _language; }
      set { _language = value; }
    }

    private string[] _translation;
    public string[] translation
    {
      get { return _translation; }
      set { _translation = value; }
    }    
  }
}

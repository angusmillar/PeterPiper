using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public class CodedText : Any
  {
    private string _originalText;
    public string originalText
    {
      get { return _originalText; }
      set { _originalText = value; }
    }

    private string  _code;
    public string  code
    {
      get { return _code; }
      set { _code = value; }
    }

    private string _codeSystem;
    public string codeSystem
    {
      get { return _codeSystem; }
      set { _codeSystem = value; }
    }

    private string _codeSystemName;
    public string codeSystemName
    {
      get { return _codeSystemName; }
      set { _codeSystemName = value; }
    }

    private string _codeSystemVersion;
    public string codeSystemVersion
    {
      get { return _codeSystemVersion; }
      set { _codeSystemVersion = value; }
    }

    private string _displayName;
    public string displayName
    {
      get { return _displayName; }
      set { _displayName = value; }
    }
    
  }
}

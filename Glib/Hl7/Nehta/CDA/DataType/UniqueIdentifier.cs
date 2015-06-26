using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public class UniqueIdentifier : Any
  {
    private string _root;
    public string root
    {
      get { return _root; }
      set { _root = value; }
    }

    private string _extension;
    public string extension
    {
      get { return _extension; }
      set { _extension = value; }
    }

    private Boolean _displayable;
    public Boolean  displayable
    {
      get { return _displayable; }
      set { _displayable = value; }
    }

    private string _identifierName;
    public string identifierName
    {
      get { return _identifierName; }
      set { _identifierName = value; }
    }

    private Enum.IdentifierScope _identifierScope;
    public Enum.IdentifierScope identifierScope
    {
      get { return _identifierScope; }
      set { _identifierScope = value; }
    }

    private Enum.IdentifierReliability _reliability;
    public Enum.IdentifierReliability reliability
	  {
      get { return _reliability; }
      set { _reliability = value; }
	  }	
    
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Support
{
  public class MessageDelimiters
  {
    public MessageDelimiters(char Field, char Repeat, char Component, char SubComponent, char Escape)
    {
      _Field = Field;
      _Repeat = Repeat;
      _Component = Component;
      _SubComponent = SubComponent;
      _Escape = Escape;
    }
    public MessageDelimiters()
    {
    }
    private char _Field = Hl7.V2.Support.Standard.Delimiters.Field;
    public char Field
    {
      get
      {
        return _Field;
      }
    }

    private char _Repeat = Hl7.V2.Support.Standard.Delimiters.Repeat;
    public char Repeat
    {
      get
      {
        return _Repeat;
      }
    }

    private char _Component = Hl7.V2.Support.Standard.Delimiters.Component;
    public char Component
    {
      get
      {
        return _Component;
      }
    }

    private char _SubComponent = Hl7.V2.Support.Standard.Delimiters.SubComponent;
    public char SubComponent
    {
      get
      {
        return _SubComponent;
      }
    }

    private char _Escape = Hl7.V2.Support.Standard.Delimiters.Escape;
    public char Escape
    {
      get
      {
        return _Escape;
      }
    }
  }
}

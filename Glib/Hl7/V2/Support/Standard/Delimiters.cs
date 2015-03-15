using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Support.Standard
{
  public static class Delimiters
  {
    public static char Field = '|';
    public static char Repeat = '~';
    public static char Component = '^';
    public static char SubComponent = '&';
    public static char Escape = '\\';
    public static char SegmentTerminator = '\r';
  }
}

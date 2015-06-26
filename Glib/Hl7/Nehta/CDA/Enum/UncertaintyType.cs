using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Glib.Hl7.Nehta.CDA.Enum
{
  public enum UncertaintyType
  {
    [Description("Need to find out what this code system is, it comes from ISO 21090 ???")]
    WHATISTHISSET,
    [Description("Normal")]
    Normal
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Glib.Hl7.Nehta.CDA.Enum
{
  /// <summary>
  /// CodingRationale OID: 2.16.840.1.113883.5.1074
  /// </summary>
  public enum CodingRationale
  {
    [Description("originally produced code")]
    O,
    [Description("original and required")]
    OR,
    [Description("post-coded")]
    P,
    [Description("post-coded and required")]
    PR,
    [Description("required")]
    R
  }
}

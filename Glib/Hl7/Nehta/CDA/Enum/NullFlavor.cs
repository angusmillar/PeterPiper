using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Glib.Hl7.Nehta.CDA.Enum
{
  /// <summary>
  /// NullFlavor Enumeration OID: 2.16.840.1.113883.5.1008
  /// </summary>
  public enum NullFlavor
  {
    [Description("No information")]
    NI,
    [Description("Other")]
    OTH,
    [Description("Positive infinity")]
    PINF,
    [Description("Negative infinity")]
    NINF,
    [Description("Unknown")]
    UNK,
    [Description("Asked but unknown")]
    ASKU,
    [Description("Temporarily unavailable")]
    NAV,
    [Description("Not asked")]
    NASK,
    [Description("Sufficient quantity")]
    QS,
    [Description("Trace")]
    TRC,
    [Description("Masked")]
    MSK,
    [Description("Not applicable")]
    NA
  }

}

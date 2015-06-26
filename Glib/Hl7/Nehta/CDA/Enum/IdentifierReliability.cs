using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Glib.Hl7.Nehta.CDA.Enum
{
  /// <summary>
  /// IdentifierReliability OID: 2.16.840.1.113883.5.1117
  /// </summary>
  public enum IdentifierReliability
  {
    [Description("Issued by System")]
    ISS,
    [Description("Unverified by system")]
    UNV,
    [Description("Verified by system")]
    VRF
  }
}

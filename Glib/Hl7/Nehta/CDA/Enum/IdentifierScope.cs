using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Glib.Hl7.Nehta.CDA.Enum
{
  /// <summary>
  /// IdentifierScope OID: 2.16.840.1.113883.1.11.20276
  /// </summary>
  public enum IdentifierScope
  {
    [Description("Business Identifier")]
    BUSN,
    [Description("Object Identifier")]
    OBJ,
    [Description("Version Identifier")]
    VER,
    [Description("View Specific Identifier")]
    VW
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Glib.Hl7.Nehta.CDA.Enum
{
  /// <summary>
  /// CompressionAlgorithm OID: 2.16.840.1.113883.5.1009
  /// </summary>
  public enum CompressionAlgorithm
  {
    [Description("bzip")]
    BZ,
    [Description("deflate")]
    DF,
    [Description("gzip")]
    GZ,
    [Description("compress")]
    Z,
    [Description("Z7")]
    Z7,
    [Description("zlib")]
    ZL
  }
}

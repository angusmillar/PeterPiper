using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Schema.Model
{
  public enum VersionsSupported {NotSupported, V2_3, V2_3_1, V2_4, V2_5, V2_5_1 };

  public static class Version
  {
    public static VersionsSupported GetVersionFromString(string VersionString)
    {
      string Version = VersionString.Replace("V","");
      switch (Version)
      {
        case "2.3":
          return VersionsSupported.V2_3;
        case "2.3.1":
          return VersionsSupported.V2_3_1;       
        case "2.4":
          return VersionsSupported.V2_4;
        case "2.5":
          return VersionsSupported.V2_5;
        case "2.5.1":
          return VersionsSupported.V2_5_1;
        default:
          return VersionsSupported.NotSupported;          
      }
    }
  }
}

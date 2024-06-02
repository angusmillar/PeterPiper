
namespace PeterPiper.Hl7.V2.Schema.Model;

public enum VersionsSupported {NotSupported, V2_3, V2_3_1, V2_4, V2_5, V2_5_1 };

public static class Version
{
  public static VersionsSupported GetVersionFromString(string versionString)
  {
    string Version = versionString.Replace("V","");
    return Version switch
    {
      "2.3" => VersionsSupported.V2_3,
      "2.3.1" => VersionsSupported.V2_3_1,
      "2.4" => VersionsSupported.V2_4,
      "2.5" => VersionsSupported.V2_5,
      "2.5.1" => VersionsSupported.V2_5_1,
      _ => VersionsSupported.NotSupported
    };
  }
}
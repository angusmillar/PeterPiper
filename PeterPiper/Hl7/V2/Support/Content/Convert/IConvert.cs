using System;
namespace PeterPiper.Hl7.V2.Support.Content.Convert
{
  public interface IConvert
  {
    IBase64 Base64 { get; }
    IDateTime DateTime { get; }
  }
}

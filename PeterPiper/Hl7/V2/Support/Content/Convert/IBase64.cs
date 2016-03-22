using System;
namespace PeterPiper.Hl7.V2.Support.Content.Convert
{
  public interface IBase64
  {
    byte[] Decode();
    void Encode(byte[] item);
  }
}

using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IContentBase : IModelBase
  {
    PeterPiper.Hl7.V2.Support.Content.DateTimeSupport DateTimeSupport { get; }
    byte[] FromBase64();
    void ToBase64(byte[] item);
  }
}

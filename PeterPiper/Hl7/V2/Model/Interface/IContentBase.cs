using System;
using PeterPiper.Hl7.V2.Support.Content.Convert.Interface;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IContentBase : IModelBase
  {
    IConvert Convert { get; }
    //PeterPiper.Hl7.V2.Support.Content.Convert.Implementation.DateTime DateTimeSupport { get; }
    //byte[] FromBase64();
    //void ToBase64(byte[] item);
  }
}

using System;
using PeterPiper.Hl7.V2.Support.Content.Convert;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IContentBase : IModelBase
  {
    IConvert Convert { get; }
  }
}

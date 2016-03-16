using System;
using PeterPiper.Hl7.V2.Support.Content.Convert.Interface;

namespace PeterPiper.Hl7.V2.Model.Interface
{
  public interface IContentBase : IModelBase
  {
    IConvert Convert { get; }
  }
}

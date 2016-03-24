using System;
using PeterPiper.Hl7.V2.Support.Content.Convert;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IContentBase : IModelBase
  {
    /// <summary>
    /// Get access to all content converters available for this instance. 
    /// </summary>
    IConvert Convert { get; }
  }
}

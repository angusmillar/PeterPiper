using System;
using System.Collections.Generic;
using PeterPiper.Hl7.V2.Model.ModelSupport.Interface;

namespace PeterPiper.Hl7.V2.Model.Interface
{
  public interface IModelBase
  {
    //string AsString { get; set; }
    //string AsStringRaw { get; set; }
    int? Index { get; }
    IPathDetailBase PathDetail { get; }
    IEnumerable<object> UtilityObjectList { get; set; }
  }
}

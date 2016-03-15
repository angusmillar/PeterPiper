using System;
namespace PeterPiper.Hl7.V2.Model
{
  public interface IModelBase
  {
    //string AsString { get; set; }
    //string AsStringRaw { get; set; }
    int? Index { get; }
    PeterPiper.Hl7.V2.Model.ModelSupport.PathDetailBase PathDetail { get; }
    System.Collections.Generic.List<object> UtilityObjectList { get; set; }
  }
}

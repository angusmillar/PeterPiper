using System;
using System.Collections.Generic;
using PeterPiper.Hl7.V2.Model.ModelSupport;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IModelBase
  { 
    /// <summary>
    /// The index the HL7 V2 part instance is located within its parent structure
    /// </summary>
    int? Index { get; }
    /// <summary>
    /// Returns the Path Detail for the instance, the string meta data expressing the location for example "PID-3.1.2"
    /// </summary>
    IPathDetailBase PathDetail { get; }
    /// <summary>
    /// Get or set a user's defined general use object instance 
    /// </summary>
    IEnumerable<object> UtilityObjectList { get; set; }
  }
}

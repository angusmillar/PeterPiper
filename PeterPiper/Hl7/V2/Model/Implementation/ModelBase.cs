using System;
using System.Collections.Generic;

namespace PeterPiper.Hl7.V2.Model.Implementation;

internal abstract class ModelBase : IModelBase
{
  internal bool _Temporary = true;
  internal int? _Index;
  internal ModelBase _Parent = null;

  public ModelBase()
  {
    Delimiters = new MessageDelimiters();
  }
  public ModelBase(IMessageDelimiters Delimiters)
  {
    this.Delimiters = Delimiters as MessageDelimiters;
  }

  public virtual string AsString {get; set;}    
  public virtual string AsStringRaw {get; set;}
    
  public int? Index => _Index;

  public ModelSupport.IPathDetailBase  PathDetail => ModelSupport.PathDetailFactory.GetPathDetail(this);

  internal MessageDelimiters Delimiters { get; set; }

  private IEnumerable<object> _UtilityObjectsList;
  public IEnumerable<object> UtilityObjectList
  {
    get
    {
      if (_UtilityObjectsList == null)
      {
        _UtilityObjectsList = new List<object>();
      }
      return _UtilityObjectsList;
    }
    set => _UtilityObjectsList = value;
  }

  internal bool ValidateDelimiters(MessageDelimiters delimitersToCompaire)
  {
    if (Delimiters.Field != delimitersToCompaire.Field)
      return false;
    if (Delimiters.Component != delimitersToCompaire.Component)
      return false;
    if (Delimiters.SubComponent != delimitersToCompaire.SubComponent)
      return false;
    if (Delimiters.Repeat != delimitersToCompaire.Repeat)
      return false;
    if (Delimiters.Escape != delimitersToCompaire.Escape)
      return false;
    return true; 
  }
  internal void ValidateItemNotInUse(ModelBase item)
  {
    if (item._Parent != null && item._Index != null && item._Temporary != true)
    {
      throw new ArgumentException("The object instance passed is in use within another structure. This is not allowed. Have you forgotten to Clone() the instance before reusing.");
    }
  }

}
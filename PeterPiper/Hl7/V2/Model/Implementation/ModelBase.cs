﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Model.Implementation;

namespace PeterPiper.Hl7.V2.Model.Implementation
{
  internal abstract class ModelBase : IModelBase
  {
    internal bool _Temporary = true;
    internal int? _Index;
    internal ModelBase _Parent = null;

    public ModelBase()
    {
      _Delimiters = new MessageDelimiters();
    }
    public ModelBase(IMessageDelimiters Delimiters)
    {
      this._Delimiters = Delimiters as MessageDelimiters;
    }

    public virtual string AsString {get; set;}    
    public virtual string AsStringRaw {get; set;}
    
    public int? Index
    {
      get
      {
        return _Index;
      }
    }

    public ModelSupport.IPathDetailBase  PathDetail
    {
      get
      {
        return  ModelSupport.PathDetailFactory.GetPathDetail(this);
      }
    }

    private MessageDelimiters _Delimiters;
    internal MessageDelimiters Delimiters
    {
      get
      {
        return _Delimiters;
      }
      set
      {
        _Delimiters = value;
      }
    }

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
      set
      {
        _UtilityObjectsList = value;
      }
    }

    internal bool ValidateDelimiters(MessageDelimiters DelimitersToCompaire)
    {
      if (_Delimiters.Field != DelimitersToCompaire.Field)
        return false;
      if (_Delimiters.Component != DelimitersToCompaire.Component)
        return false;
      if (_Delimiters.SubComponent != DelimitersToCompaire.SubComponent)
        return false;
      if (_Delimiters.Repeat != DelimitersToCompaire.Repeat)
        return false;
      if (_Delimiters.Escape != DelimitersToCompaire.Escape)
        return false;
      return true; ;
    }
    internal void ValidateItemNotInUse(ModelBase item)
    {
      if (item._Parent != null && item._Index != null && item._Temporary != true)
        throw new ArgumentException("The object instance passed is in use within another structure. This is not allowed. Have you forgotten to Clone() the instance before reusing.");
    }

  }
}

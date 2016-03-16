using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model.Interface;
using PeterPiper.Hl7.V2.Model.Implementation;

namespace PeterPiper.Hl7.V2.Model.Implementation
{
  internal class Field : ContentBase, IContentBase, IField
  {
    private Dictionary<int, Component> _ComponentDictonary;

    //Creator Factory used Constructors
    internal Field()
    {
      _ComponentDictonary = new Dictionary<int, Component>();
      _Temporary = true;
      _Index = null;
      _Parent = null;
    }
    internal Field(IMessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _ComponentDictonary = new Dictionary<int, Component>();
      _Temporary = true;
      _Index = null;
      _Parent = null;
    }
    internal Field(string StringRaw)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;

      if (ValidateStringRaw(StringRaw))
      {
        _ComponentDictonary = ParseFieldRawStringToComponent(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }
    internal Field(string StringRaw, IMessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;

      if (ValidateStringRaw(StringRaw))
      {
        _ComponentDictonary = ParseFieldRawStringToComponent(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }
    
    //Only internal Constructors
    internal Field(string StringRaw, MessageDelimiters CustomDelimiters, bool Temporary, int? Index, ContentBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;

      if (ValidateStringRaw(StringRaw))
      {
        _ComponentDictonary = ParseFieldRawStringToComponent(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }
    internal Field(ModelSupport.ContentTypeInternal ContentTypeInternal, MessageDelimiters CustomDelimiters, bool Temporary, int? Index, ContentBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      _ComponentDictonary = ParseFieldRawStringToComponent(string.Empty, ContentTypeInternal);
    }


    //Instance access
    public IMessageDelimiters MessageDelimiters
    {
      get
      {
        return this.Delimiters;
      }
    } 
    public IField Clone()
    {
      return new Field(this.AsStringRaw, this.Delimiters, false, null, null);
    }
    public override string ToString()
    {
      return this.AsString;
    }
    public override string AsString
    {
      get
      {
        if (_ComponentDictonary.Count == 0)
          return string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();
        _ComponentDictonary.OrderByDescending(i => i.Key);
        for (int i = 1; i < _ComponentDictonary.Keys.Max() + 1; i++)
        {
          if (_ComponentDictonary.ContainsKey(i))
          {
            oStringBuilder.Append(_ComponentDictonary[i].AsString);
          }
          if (i != _ComponentDictonary.Keys.Max())
            oStringBuilder.Append(this.Delimiters.Component);
        }
        return oStringBuilder.ToString();
      }
      set
      {
        this.AsStringRaw = Support.Standard.Escapes.Encode(value, this.Delimiters);
      }
    }
    public override string AsStringRaw
    {
      get
      {
        if (_ComponentDictonary.Count == 0)
          return string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();
        _ComponentDictonary.OrderByDescending(i => i.Key);
        for (int i = 1; i < _ComponentDictonary.Keys.Max() + 1; i++)
        {
          if (_ComponentDictonary.ContainsKey(i))
          {
            oStringBuilder.Append(_ComponentDictonary[i].AsStringRaw);
          }
          if (i != _ComponentDictonary.Keys.Max())
            oStringBuilder.Append(this.Delimiters.Component);
        }
        return oStringBuilder.ToString();
      }
      set
      {
        if (value == String.Empty)
        {
          RemoveFromParent();
        }
        else if (ValidateStringRaw(value))
        {
          _ComponentDictonary = ParseFieldRawStringToComponent(value, ModelSupport.ContentTypeInternal.Unknown);
        }
      }
    }
    public bool IsEmpty
    {
      get
      {
        return (_ComponentDictonary.Count == 0);
      }
    }
    public bool IsHL7Null
    {
      get
      {
        return this.GetComponent(1).IsHL7Null;
      }
    }
    public void ClearAll()
    {
      _ComponentDictonary.Clear();
      RemoveFromParent();
    }
    public void Set(int index, IContent item)
    {
      ValidateItemNotInUse(item as Content);
      this.ContentSet(item as Content, index);
    }
    public void Set(int index, ISubComponent item)
    {
      ValidateItemNotInUse(item as SubComponent);
      this.SubComponentSet(item as SubComponent, index);
    }
    public void Set(int index, IComponent item)
    {
      var Component = item as Component;
      ValidateItemNotInUse(Component);
      this.ComponentSet(Component, index);
    }    
    public void Add(IComponent item)
    {
      var Component = item as Component;
      ValidateItemNotInUse(Component);
      this.ComponentAppend(Component);
    }
    public void Add(ISubComponent item)
    {
      ValidateItemNotInUse(item as SubComponent);
      this.SubComponentAppend(item as SubComponent);
    }
    public void Add(IContent item)
    {
      ValidateItemNotInUse(item as Content);
      this.ContentAppend(item as Content);
    }
    public void Insert(int index, IComponent item)
    {
      var Component = item as Component;
      ValidateItemNotInUse(Component);
      this.ComponentInsertBefore(Component, index);
    }
    public void Insert(int index, ISubComponent item)
    {
      ValidateItemNotInUse(item as SubComponent);
      this.SubComponentInsertBefore(item as SubComponent, index);
    }
    public void Insert(int index, IContent item)
    {
      ValidateItemNotInUse(item as Content);
      this.ContentInsertBefore(item as Content, index);
    }
    public void RemoveComponentAt(int index)
    {
      this.ComponentRemoveAt(index);
    }
    public void RemoveSubComponentAt(int index)
    {
      this.SubComponentRemoveAt(index);
    }
    public void RemoveContentAt(int index)
    {
      this.ContentRemoveAt(index);
    }
    public int ComponentCount
    {
      get
      {
        return this.CountComponent;
      }
    }
    public int SubComponentCount
    {
      get
      {
        return this.CountSubComponent;
      }
    }
    public int ContentCount
    {
      get
      {
        return this.CountContent;
      }
    }
    public IComponent Component(int index)
    {
      if (index == 0)
        throw new ArgumentException("Component is a one based index, zero is not a valid index");
      return this.GetComponent(index);
    }
    public ISubComponent SubComponent(int index)
    {
      if (index == 0)
        throw new ArgumentException("SubComponent is a one based index, zero is not a valid index");    
      return GetSubComponent(index);
    }    
    public IContent Content(int index)
    {
      return GetContent(index);
    }    
    public ReadOnlyCollection<IComponent> ComponentList
    {
      get
      {
        List<IComponent> oNewList = new List<IComponent>();
        int Counter = 1;
        foreach (var item in _ComponentDictonary.OrderBy(x => x.Key))
        {
          if (item.Key != Counter)
          {
            while (Counter != item.Key)
            {
              oNewList.Add(new Component(string.Empty, this.Delimiters, true, Counter, this));
              Counter++;
            }
            oNewList.Add(item.Value);
            Counter++;
          }
          else
          {
            oNewList.Add(item.Value);
            Counter++;
          }
        }
        return oNewList.AsReadOnly();
      }
    }

    //Component
    internal Component GetComponent(int index)
    {
      if (_ComponentDictonary.ContainsKey(index))
        return _ComponentDictonary[index];
      else
        return new Component(string.Empty, this.Delimiters, true, index, this);
    }
    internal int CountComponent
    {
      get
      {
        if (_ComponentDictonary.Count > 0)
          return _ComponentDictonary.Keys.Max();
        else
          return 0;
      }
    }
    internal Component ComponentSet(Component Component, int Index)
    {
      if (Component.AsStringRaw == String.Empty)
      {
        RemoveChild(Index);
        Component._Index = null;
        Component._Parent = null;
        Component._Temporary = true;
        return Component;
      }
      else
      {
        Component._Index = Index;
        Component._Parent = this;
        if (SetToDictonary(Component))
          Component._Temporary = false;
        return _ComponentDictonary[System.Convert.ToInt32(Component._Index)];
      }
    }
    internal Component ComponentAppend(Component Component)
    {
      if (_ComponentDictonary.Count > 0)
      {
        return this.ComponentInsertBefore(Component, _ComponentDictonary.Keys.Max() + 1);
      }
      else
      {
        Component._Index = 1;
        Component._Parent = this;
        if (SetToDictonary(Component))
          Component._Temporary = false;
        return _ComponentDictonary[1];
      }

      //====================================================
      //int InsertAtIndex = 1;
      //if (_ComponentDictonary.Count > 0)
      //  InsertAtIndex = _ComponentDictonary.Keys.Max() + 1;
      //Component._Index = InsertAtIndex;
      //Component._Parent = this;
      //Component._Temporary = false;
      //_ComponentDictonary.Add(InsertAtIndex, Component);
      //return _ComponentDictonary[_ComponentDictonary.Keys.Max()];
    }
    internal Component ComponentInsertBefore(Component Component, int Index)
    {
      if (Index == 0)
        throw new ArgumentOutOfRangeException("Element is a one based index, zero is not a valid index.");

      int ComponentInsertedAt = 0;
      //Empty Dic so just add as first itme 
      if (_ComponentDictonary.Count == 0)
      {
        ComponentInsertedAt = Index;
        Component._Index = ComponentInsertedAt;
        Component._Parent = this;
        if (SetToDictonary(Component))
          Component._Temporary = false;
      }
      //Asked to insert before an index larger than the largest in Dic so just add to the end
      else if (_ComponentDictonary.Keys.Max() < Index)
      {
        ComponentInsertedAt = Index;
        Component._Index = ComponentInsertedAt;
        Component._Parent = this;
        if (SetToDictonary(Component))
          Component._Temporary = false;
        //_ContentDictonary.Add(_ContentDictonary.Keys.Max() + 1, Content);
      }
      //Asked to insert within items already in the Dic so cycle through moving each item higher or equal up by one then just add the new item
      //The Content Dictonary is different than all the others as it is to never have gaps between items and it is Zero based.
      else
      {
        foreach (var item in _ComponentDictonary.Reverse())
        {
          if (item.Key >= Index)
          {
            item.Value._Index++;
            if (_ComponentDictonary.ContainsKey(item.Key + 1))
            {
              _ComponentDictonary.Remove(item.Key);
              _ComponentDictonary[item.Key + 1] = item.Value;
            }
            else
            {
              _ComponentDictonary.Remove(item.Key);
              _ComponentDictonary.Add(item.Key + 1, item.Value);
            }
          }
        }
        Component._Index = Index;
        Component._Parent = this;
        SetParent();
        Component._Temporary = false;
        _ComponentDictonary[Index] = Component;
        ComponentInsertedAt = Index;
      }
      return _ComponentDictonary[ComponentInsertedAt];


      //int ComponentInsertedAt = 0;
      //if (_ComponentDictonary.ContainsKey(Index))
      //{
      //  foreach (var item in _ComponentDictonary.Reverse())
      //  {
      //    if (item.Key >= Index)
      //    {
      //      item.Value._Index++;
      //      if (item.Key >= Index)
      //      {
      //        _ComponentDictonary.Remove(item.Key);
      //        item.Value._Index++;
      //        _ComponentDictonary.Add(item.Key + 1, item.Value);
      //      }
      //    }
      //  }
      //  ComponentInsertedAt = Index;
      //  Component._Index = ComponentInsertedAt;
      //  Component._Parent = this;
      //  if (SetToDictonary(Component))
      //    Component._Temporary = false;        
      //}
      //else
      //{
      //  ComponentInsertedAt = Index;
      //  Component._Index = ComponentInsertedAt;
      //  Component._Parent = this;
      //  if (SetToDictonary(Component))
      //    Component._Temporary = false;
      //}
      //return _ComponentDictonary[ComponentInsertedAt];
    }
    internal bool ComponentRemoveAt(int Index)
    {
      if (_ComponentDictonary.ContainsKey(Index))
      {
        Dictionary<int, Component> oNewDic = new Dictionary<int, Component>();
        foreach (var item in _ComponentDictonary)
        {
          if (item.Key < Index)
          {
            oNewDic.Add(item.Key, item.Value);
          }
          else if (item.Key > Index)
          {
            item.Value._Index--;
            oNewDic.Add(item.Key - 1, item.Value);
          }
        }
        _ComponentDictonary = oNewDic;
        if (_ComponentDictonary.Count == 0)
          RemoveFromParent();
        return true;
      }
      return false;      
    }

    //SubComponets
    internal SubComponent GetSubComponent(int index)
    {
      if (_ComponentDictonary.ContainsKey(1))
        return _ComponentDictonary[1].GetSubComponent(index);
      else
      {
        //_ComponentDictonary = new Dictionary<int, Component>();
        //_ComponentDictonary.Add(1, new Component(string.Empty, this.Delimiters, true, 1, this));
        //return _ComponentDictonary[1].GetSubComponent(index);
        Component oComponent = new Component(string.Empty, this.Delimiters, true, 1, this);
        return oComponent.GetSubComponent(index);
      }
    }
    internal int CountSubComponent
    {
      get
      {
        if (_ComponentDictonary.ContainsKey(1))
          return _ComponentDictonary[1].CountSubComponent;
        else
          return 0;
      }
    }
    internal SubComponent SubComponentSet(SubComponent SubComponent, int Index)
    {
      if (_ComponentDictonary.ContainsKey(1))
      {
        return _ComponentDictonary[1].SubComponentSet(SubComponent, Index);
      }
      else
      {
        Component oComponent = new Component(string.Empty, this.Delimiters, true, 1, this);
        return oComponent.SubComponentSet(SubComponent, Index);
      }     
    }
    internal SubComponent SubComponentAppend(SubComponent SubComponent)
    {
      if (_ComponentDictonary.ContainsKey(1))
        return _ComponentDictonary[1].SubComponentAppend(SubComponent);
      else
      {
        //_ComponentDictonary = new Dictionary<int, Component>();
        //_ComponentDictonary.Add(1, new Component(SubComponent, SubComponent.Delimiters, true, 1, this));
        //return _ComponentDictonary[1].GetSubComponent(1);
        Component oComponent = new Component(String.Empty, SubComponent.Delimiters, true, 1, this);
        return oComponent.SubComponentAppend(SubComponent);
      }
    }
    internal SubComponent SubComponentInsertBefore(SubComponent SubComponent, int Index)
    {
      if (_ComponentDictonary.ContainsKey(1))
        return _ComponentDictonary[1].SubComponentInsertBefore(SubComponent, Index);
      else
      {
        //_ComponentDictonary = new Dictionary<int, Component>();
        //_ComponentDictonary.Add(1, new Component(string.Empty, this.Delimiters, true, 1, this));
        //_ComponentDictonary[1].SubComponentInsertBefore(SubComponent, Index);
        //return _ComponentDictonary[1].GetSubComponent(Index);
        Component oComponent = new Component(string.Empty, this.Delimiters, true, 1, this);
        return oComponent.SubComponentInsertBefore(SubComponent, Index);
      }
    }
    internal bool SubComponentRemoveAt(int Index)
    {
      if (_ComponentDictonary.ContainsKey(1))
        return _ComponentDictonary[1].SubComponentRemoveAt(Index);
      else
        return false;
    }

    //Content
    internal Content GetContent(int index)
    {
      if (_ComponentDictonary.ContainsKey(1))
        return _ComponentDictonary[1].GetContent(index);
      else
      {
        //_ComponentDictonary = new Dictionary<int, Component>();
        //_ComponentDictonary.Add(1, new Component(string.Empty, this.Delimiters, true, 1, this));
        //return _ComponentDictonary[1].GetContent(index);
        Component oComponent = new Component(string.Empty, this.Delimiters, true, 1, this);
        return oComponent.GetContent(index);
      }
    }
    internal int CountContent
    {
      get
      {
        if (_ComponentDictonary.ContainsKey(1))
          return _ComponentDictonary[1].CountContent;
        else
          return 0;
      }
    }
    internal Content ContentSet(Content Content, int Index)
    {
      if (_ComponentDictonary.ContainsKey(1))
      {
        return _ComponentDictonary[1].ContentSet(Content, Index);
      }
      else
      {
        //_ComponentDictonary.Add(1, new Component(string.Empty, this.Delimiters, true, 1, this));
        //return _ComponentDictonary[1].ContentSet(Content, Index);
        Component oComponent = new Component(string.Empty, this.Delimiters, true, 1, this);
        return oComponent.ContentSet(Content, Index);
      }
    }
    internal Content ContentAppend(Content Content)
    {
      if (_ComponentDictonary.ContainsKey(1))
        return _ComponentDictonary[1].ContentAppend(Content);
      else
      {
        //if (Content.AsStringRaw != String.Empty)
        //  SetParent();
        //_ComponentDictonary = new Dictionary<int, Component>();
        //_ComponentDictonary.Add(1, new Component(string.Empty, this.Delimiters, true, 1, this));
        //_ComponentDictonary[1].Add(Content);
        //return _ComponentDictonary[1].GetContent(0);
        Component oComponent = new Component(string.Empty, this.Delimiters, true, 1, this);
        return oComponent.ContentAppend(Content);
      }
    }
    internal Content ContentInsertBefore(Content Content, int Index)
    {
      if (_ComponentDictonary.ContainsKey(1))
      {
        return _ComponentDictonary[1].ContentInsertBefore(Content, Index);
      }
      else
      {
        //_ComponentDictonary.Add(1, new Component(string.Empty, this.Delimiters, true, 1, this));
        //return _ComponentDictonary[1].ContentInsertBefore(Content, Index);
        Component oComponent = new Component(string.Empty, this.Delimiters, true, 1, this);
        return oComponent.ContentInsertBefore(Content, Index);
      }
    }
    internal bool ContentRemoveAt(int Index)
    {
      if (_ComponentDictonary.ContainsKey(1))
        return _ComponentDictonary[1].ContentRemoveAt(Index);
      else
        return false;
    }

    internal bool SetToDictonary(Component oComponent)
    {
      try
      {
        if (_ComponentDictonary.ContainsKey(System.Convert.ToInt32(oComponent._Index)))
        {
          _ComponentDictonary[System.Convert.ToInt32(oComponent._Index)]._Temporary = true;
          _ComponentDictonary[System.Convert.ToInt32(oComponent._Index)]._Parent = null;
          _ComponentDictonary[System.Convert.ToInt32(oComponent._Index)]._Index = null;
          _ComponentDictonary[System.Convert.ToInt32(oComponent._Index)] = oComponent;
        }
        else
        {
          _ComponentDictonary.Add(System.Convert.ToInt32(oComponent._Index), oComponent);
        }
        SetParent();
        return true;
      }
      catch (Exception Exec)
      {
        throw new ApplicationException("Error setting Component into Field Parent", Exec);
      }
    }
    private void SetParent()
    {
      if (this._Temporary)
      {
        if (this._Parent is Element)
        {
          Element oElement = this._Parent as Element;
          if (oElement.SetToDictonary(this))
          {
            this._Temporary = false;
          }
        }
      }
    }
    internal void RemoveChild(int Index)
    {
      try
      {
        if (_ComponentDictonary.ContainsKey(Index))
        {
          _ComponentDictonary[Index]._Temporary = true;
          _ComponentDictonary[Index]._Index = null;
          _ComponentDictonary[Index]._Parent = null;
        }
        _ComponentDictonary.Remove(Index);
        if (_ComponentDictonary.Count == 0)
        {
          RemoveFromParent();
        }
      }
      catch
      {
        throw new ApplicationException(String.Format("Field's Component Dictonary did not contain Component Index {0} for removal call from Component Instance", Index));
      }
    }
    private void RemoveFromParent()
    {
      if (this._Index != null)
      {
        try
        {
          Element oElement = this._Parent as Element;
          oElement.RemoveChild(System.Convert.ToInt32(this._Index));
        }
        catch (InvalidCastException oInvalidCastExec)
        {
          throw new ApplicationException("Casting of Field's parent to Element throws Invalid Cast Exception, check innner exception for more detail", oInvalidCastExec);
        }
      }
    }

    //Parseing and Validation
    private Dictionary<int, Component> ParseFieldRawStringToComponent(String StringRaw, ModelSupport.ContentTypeInternal ContentTypeInternal)
    {
      //Example:  "first^Second^Third^\H\Forth bold\N\^fith&SubHere^Six";
      _ComponentDictonary = new Dictionary<int, Component>();
      if (ContentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters || ContentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator)
      {
        _ComponentDictonary.Add(1, new Component(ContentTypeInternal, this.Delimiters, false, 1, this));
      }
      else
      {
        if (StringRaw.Contains(this.Delimiters.Component))
        {
          string[] ComponentParts = StringRaw.Split(this.Delimiters.Component);
          int CompoenentPositionCounter = 1;
          foreach (string Part in ComponentParts)
          {
            if (Part != string.Empty)
            {
              //_ComponentDictonary.Add(CompoenentPositionCounter, new Component(Part, this.Delimiters, false, CompoenentPositionCounter, this));
              new Component(Part, this.Delimiters, true, CompoenentPositionCounter, this);
            }
            CompoenentPositionCounter++;
          }
        }
        else
        {
          //_ComponentDictonary.Add(1, new Component(StringRaw, this.Delimiters, this._Temporary, 1, this));
          new Component(StringRaw, this.Delimiters, true, 1, this);
        }
      }
      return _ComponentDictonary;
    }
    private bool ValidateStringRaw(string StringRaw)
    {
      Char[] CharatersNotAlowed = { this.Delimiters.Field,
                                    this.Delimiters.Repeat};

      if (StringRaw.IndexOfAny(CharatersNotAlowed) != -1)
      {
        throw new System.ArgumentException(String.Format("Field data cannot contain HL7 V2 Delimiters of : {0}", CharatersNotAlowed));
      }
      return true;
    }
  }
}

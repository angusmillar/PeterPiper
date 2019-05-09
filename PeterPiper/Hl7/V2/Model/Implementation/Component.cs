using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation
{
  internal class Component : ContentBase, IComponent
  {
    private Dictionary<int, SubComponent> _SubComponentDictonary;

    //Creator Factory used Constructors
    internal Component()
    {
      _SubComponentDictonary = new Dictionary<int, SubComponent>();
      _Temporary = true;
      _Index = null;
      _Parent = null;
    }
    internal Component(IMessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _SubComponentDictonary = new Dictionary<int, SubComponent>();
      _Temporary = true;
      _Index = null;
      _Parent = null;
    }
    internal Component(string StringRaw)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;

      if (ValidateStringRaw(StringRaw))
      {
        _SubComponentDictonary = ParseComponentRawStringToSubComponent(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }
    internal Component(string StringRaw, IMessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;

      if (ValidateStringRaw(StringRaw))
      {
        _SubComponentDictonary = ParseComponentRawStringToSubComponent(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }
    
    //Only internal Constructors
    internal Component(SubComponent SubComponent, MessageDelimiters CustomDelimiters, bool Temporary,
                       int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      ValidateItemNotInUse(SubComponent);
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      _SubComponentDictonary = new Dictionary<int, SubComponent>();
      _SubComponentDictonary.Add(1, SubComponent);

    }
    internal Component(string StringRaw, MessageDelimiters CustomDelimiters, bool Temporary,
                       int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;

      if (ValidateStringRaw(StringRaw))
      {
        _SubComponentDictonary = ParseComponentRawStringToSubComponent(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }
    internal Component(ModelSupport.ContentTypeInternal ContentTypeInternal,
                       MessageDelimiters CustomDelimiters, bool Temporary, int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      _SubComponentDictonary = ParseComponentRawStringToSubComponent(string.Empty, ContentTypeInternal);
    }

    //Instance access
    public IMessageDelimiters MessageDelimiters
    {
      get
      {
        return this.Delimiters;
      }
    } 
    public IComponent Clone()
    {
      return new Component(this.AsStringRaw, this.Delimiters, true, null, null);
    }
    public override string ToString()
    {
      return this.AsString;
    }
    public override string AsString
    {
      get
      {
        return GetAsStringOrAsRawString(false);
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
        return GetAsStringOrAsRawString(true);
      }
      set
      {
        if (value == String.Empty)
        {
          RemoveFromParent();
        }
        else if (ValidateStringRaw(value))
        {
          _SubComponentDictonary = ParseComponentRawStringToSubComponent(value, ModelSupport.ContentTypeInternal.Unknown);
        }
      }
    }    
    public bool IsEmpty
    {
      get
      {
        return (_SubComponentDictonary.Count == 0);
      }
    }
    public bool IsHL7Null
    {
      get
      {
        if (this.SubComponent(1).IsHL7Null)
        {
          if (this.SubComponentCount == 1)
          {
            return true;
          }
        }
        return false;        
      }
    }
    public void ClearAll()
    {
      _SubComponentDictonary.Clear();
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
    public void RemoveSubComponentAt(int index)
    {
      this.SubComponentRemoveAt(index);
    }
    public void RemoveContentAt(int index)
    {
      this.ContentRemoveAt(index);
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
    public ISubComponent SubComponent(int index)
    {
      if (index == 0)
        throw new PeterPiperException("SubComponent is a one based index, zero is not a valid index");
      return GetSubComponent(index);
    }
    public IContent Content(int index)
    {
      return GetContent(index);
    }
    public ReadOnlyCollection<ISubComponent> SubComponentList
    {
      get
      {
        List<ISubComponent> oNewList = new List<ISubComponent>();
        int Counter = 1;
        foreach (var item in _SubComponentDictonary.OrderBy(x => x.Key))
        {
          if (item.Key != Counter)
          {
            while (Counter != item.Key)
            {
              oNewList.Add(new SubComponent(string.Empty, this.Delimiters, true, Counter, this));
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

    //SubComponents    
    internal SubComponent GetSubComponent(int index)
    {
      if (_SubComponentDictonary.ContainsKey(index))
        return _SubComponentDictonary[index];
      else
        return new SubComponent(string.Empty, this.Delimiters, true, index, this);
    }
    internal int CountSubComponent
    {
      get
      {
        if (_SubComponentDictonary.Count > 0)
          return _SubComponentDictonary.Keys.Max();
        else
          return 0;
      }
    }
    internal SubComponent SubComponentSet(SubComponent SubComponent, int Index)
    {
      if (SubComponent.AsStringRaw == String.Empty)
      {
        RemoveChild(Index);
        SubComponent._Index = null;
        SubComponent._Parent = null;
        SubComponent._Temporary = true;
        return SubComponent;
      }
      else
      {
        SubComponent._Index = Index;
        SubComponent._Parent = this;
        if (SetToDictonary(SubComponent))
          _SubComponentDictonary[Index]._Temporary = false;
        return _SubComponentDictonary[System.Convert.ToInt32(SubComponent._Index)];
      }
    }
    internal SubComponent SubComponentAppend(SubComponent SubComponent)
    {
      if (_SubComponentDictonary.Count > 0)
      {
        return SubComponentInsertBefore(SubComponent, _SubComponentDictonary.Keys.Max() + 1);
      }
      else
      {
        SubComponent._Index = 1;
        SubComponent._Parent = this;
        if (SetToDictonary(SubComponent))
          SubComponent._Temporary = false;
        return _SubComponentDictonary[1];
      }
    }
    internal SubComponent SubComponentInsertBefore(SubComponent SubComponent, int Index)
    {
      if (Index == 0)
        throw new PeterPiperException("SubComponent is a one based index, zero is not a valid index.");

      int SubComponentInsertedAt = 0;
      //Empty dictionary so just add as first item 
      if (_SubComponentDictonary.Count == 0)
      {
        SubComponentInsertedAt = Index;
        SubComponent._Index = SubComponentInsertedAt;
        SubComponent._Parent = this;
        if (SetToDictonary(SubComponent))
          SubComponent._Temporary = false;
      }
      //Asked to insert before an index larger than the largest in dictionary so just add to the end
      else if (_SubComponentDictonary.Keys.Max() < Index)
      {
        SubComponentInsertedAt = Index;
        SubComponent._Index = SubComponentInsertedAt;
        SubComponent._Parent = this;
        if (SetToDictonary(SubComponent))
          SubComponent._Temporary = false;
        //_ContentDictonary.Add(_ContentDictonary.Keys.Max() + 1, Content);
      }
      //Asked to insert within items already in the Dictionary so cycle through moving each item higher or equal up by one then just add the new item
      //The Content dictionary is different than all the others as it is to never have gaps between items and it is Zero based.
      else
      {
        foreach (var item in _SubComponentDictonary.Reverse())
        {
          if (item.Key >= Index)
          {
            item.Value._Index++;
            if (_SubComponentDictonary.ContainsKey(item.Key + 1))
            {
              _SubComponentDictonary.Remove(item.Key);
              _SubComponentDictonary[item.Key + 1] = item.Value;              
            }
            else
            {
              _SubComponentDictonary.Remove(item.Key);
              _SubComponentDictonary.Add(item.Key + 1, item.Value);              
            }
            
          }
        }
        SubComponent._Index = Index;
        SubComponent._Parent = this;
        SetParent();
        SubComponent._Temporary = false;
        _SubComponentDictonary[Index] = SubComponent;
        SubComponentInsertedAt = Index;
      }
      return _SubComponentDictonary[SubComponentInsertedAt];
    }    
    internal bool SubComponentRemoveAt(int Index)
    {
      if (_SubComponentDictonary.ContainsKey(Index))
      {
        Dictionary<int, SubComponent> oNewDic = new Dictionary<int, SubComponent>();
        foreach (var item in _SubComponentDictonary)
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
        _SubComponentDictonary = oNewDic;
        if (_SubComponentDictonary.Count == 0)
          RemoveFromParent();
        return true;
      }
      return false;
    }

    //Content    
    internal Content GetContent(int index)
    {
      if (_SubComponentDictonary.ContainsKey(1))
        return _SubComponentDictonary[1].GetContent(index);
      else
      {
        //_SubComponentDictonary = new Dictionary<int, SubComponent>();
        //_SubComponentDictonary.Add(1, new SubComponent(string.Empty, this.Delimiters, false, 0, this));
        //return _SubComponentDictonary[1].GetContent(index);
        SubComponent oSubComponent = new SubComponent(string.Empty, this.Delimiters, false, 0, this);
        return oSubComponent.GetContent(index);
      }
    }
    internal int CountContent
    {
      get
      {
        if (_SubComponentDictonary.ContainsKey(1))
          return _SubComponentDictonary[1].CountContent;
        else
          return 0;
      }
    }
    internal Content ContentSet(Content Content, int Index)
    {
      if (_SubComponentDictonary.ContainsKey(1))
      {
        return _SubComponentDictonary[1].ContentSet(Index, Content);
      }
      else
      {
        //_SubComponentDictonary.Add(1, new SubComponent(string.Empty, this.Delimiters, true, 1, this));
        //return _SubComponentDictonary[1].ContentSet(Index, Content);
        SubComponent oSubComponent = new SubComponent(string.Empty, this.Delimiters, true, 1, this);
        return oSubComponent.ContentSet(Index, Content);
      }
    }
    internal Content ContentAppend(Content Content)
    {
      if (_SubComponentDictonary.ContainsKey(1))
        return _SubComponentDictonary[1].ContentAppend(Content);
      else
      {
        //_SubComponentDictonary = new Dictionary<int, SubComponent>();
        //_SubComponentDictonary.Add(1, new SubComponent(Content, this.Delimiters, true, 0, this));
        SubComponent oSubComponent = new SubComponent(String.Empty, this.Delimiters, true, 1, this);
        return oSubComponent.ContentAppend(Content);
      }
    }
    internal Content ContentInsertBefore(Content Content, int Index)
    {
      if (_SubComponentDictonary.ContainsKey(1))
      {
        return _SubComponentDictonary[1].ContentInsertBefore(Content, Index);
      }
      else
      {
        //_SubComponentDictonary.Add(1, new SubComponent(Content, this.Delimiters, true, 0, this));
        SubComponent oSubComponent = new SubComponent(String.Empty, this.Delimiters, true, 1, this);
        return oSubComponent.ContentInsertBefore(Content, Index);
      }
    }
    internal bool ContentRemoveAt(int Index)
    {
      if (_SubComponentDictonary.ContainsKey(1))
        return _SubComponentDictonary[1].ContentRemoveAt(Index);
      else
        return false;
    }
    
    //Building
    private string GetAsStringOrAsRawString(bool RawString)
    {
      if (_SubComponentDictonary.Count == 0)
        return string.Empty;
      StringBuilder oStringBuilder = new StringBuilder();
      _SubComponentDictonary.OrderByDescending(i => i.Key);
      for (int i = 1; i < _SubComponentDictonary.Keys.Max() + 1; i++)
      {
        if (_SubComponentDictonary.ContainsKey(i))
        {
          if (RawString)
            oStringBuilder.Append(_SubComponentDictonary[i].AsStringRaw);
          else
            oStringBuilder.Append(_SubComponentDictonary[i].AsString);
        }
        if (i != _SubComponentDictonary.Keys.Max())
          oStringBuilder.Append(this.Delimiters.SubComponent);
      }
      return oStringBuilder.ToString();
    }

    //Maintenance
    internal bool SetToDictonary(SubComponent oSubComponent)
    {
      try
      {
        if (_SubComponentDictonary.ContainsKey(System.Convert.ToInt32(oSubComponent._Index)))
        {
          _SubComponentDictonary[System.Convert.ToInt32(oSubComponent._Index)]._Temporary = true;
          _SubComponentDictonary[System.Convert.ToInt32(oSubComponent._Index)]._Parent = null;
          _SubComponentDictonary[System.Convert.ToInt32(oSubComponent._Index)]._Index = null;
          _SubComponentDictonary[System.Convert.ToInt32(oSubComponent._Index)] = oSubComponent;
        }
        else
        {
          _SubComponentDictonary.Add(System.Convert.ToInt32(oSubComponent._Index), oSubComponent);
        }
        SetParent();
        return true;
      }
      catch (Exception Exec)
      {
        throw new PeterPiperException("Error setting SubComponent into Component Parent", Exec);
      }
    }
    private void SetParent()
    {
      if (this._Temporary)
      {
        if (this._Parent is Field)
        {
          Field oField = this._Parent as Field;
          if (oField.SetToDictonary(this))
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
        if (_SubComponentDictonary.ContainsKey(Index))
        {
          _SubComponentDictonary[Index]._Temporary = true;
          _SubComponentDictonary[Index]._Index = null;
          _SubComponentDictonary[Index]._Parent = null;
        }
        _SubComponentDictonary.Remove(Index);
        if (_SubComponentDictonary.Count == 0)
        {
          RemoveFromParent();
        }
      }
      catch
      {
        throw new PeterPiperException(String.Format("Component's SubComponent dictionary did not contain SubComponent Index {0} for removal call from SubComponent Instance", Index));
      }
    }
    private void RemoveFromParent()
    {
      if (this._Index != null)
      {
        try
        {
          Field oField = this._Parent as Field;
          oField.RemoveChild(System.Convert.ToInt32(this._Index));
        }
        catch (InvalidCastException oInvalidCastExec)
        {
          throw new PeterPiperException("Casting of Component's parent to Field throws Invalid Cast Exception, check inner exception for more detail", oInvalidCastExec);
        }
      }
    }

    //Parsing and Validation
    private Dictionary<int, SubComponent> ParseComponentRawStringToSubComponent(String StringRaw, ModelSupport.ContentTypeInternal ContentTypeInternal)
    {
      //Example:  "First&Second&Third\H\bold\N\&forth";
      _SubComponentDictonary = new Dictionary<int, SubComponent>();
      if (ContentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters || ContentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator)
      {
        _SubComponentDictonary.Add(1, new SubComponent(ContentTypeInternal, this.Delimiters, false, 1, this));
      }
      else
      {
        if (StringRaw.Contains(this.Delimiters.SubComponent))
        {
          string[] SubComponentParts = StringRaw.Split(this.Delimiters.SubComponent);
          int SubCompoenentPositionCounter = 1;
          foreach (string Part in SubComponentParts)
          {
            if (Part != string.Empty)
            {
              //_SubComponentDictonary.Add(SubCompoenentPositionCounter, new SubComponent(Part, this.Delimiters, false, SubCompoenentPositionCounter, this));
              new SubComponent(Part, this.Delimiters, true, SubCompoenentPositionCounter, this);
            }
            SubCompoenentPositionCounter++;
          }
        }
        else
        {
          //_SubComponentDictonary.Add(1, new SubComponent(StringRaw, this.Delimiters, this._Temporary, 1, this));
          new SubComponent(StringRaw, this.Delimiters, true, 1, this);
        }
      }
      return _SubComponentDictonary;
    }
    private bool ValidateStringRaw(string StringRaw)
    {
      Char[] CharatersNotAlowed = { this.Delimiters.Field,
                                    this.Delimiters.Component,
                                    this.Delimiters.Repeat};

      if (StringRaw.IndexOfAny(CharatersNotAlowed) != -1)
      {
        throw new PeterPiperException(String.Format("Component data cannot contain HL7 V2 Delimiters of : {0}", CharatersNotAlowed));
      }
      return true;
    }
  }
}

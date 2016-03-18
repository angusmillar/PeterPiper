using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model.Interface;
using PeterPiper.Hl7.V2.Model.Implementation;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation
{
  internal class Element : ContentBase, IElement
  {
    internal bool _IsMainSeparator = false;
    internal bool _IsEncodingCharacters = false;

    private Dictionary<int, Field> _RepeatDictonary;

    //Creator Factory used Constructors
    internal Element()
    {
      _RepeatDictonary = new Dictionary<int, Field>();
      _Temporary = true;
      _Index = null;
      _Parent = null;
    }
    internal Element(IMessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _RepeatDictonary = new Dictionary<int, Field>();
      _Temporary = true;
      _Index = null;
      _Parent = null;
    }
    internal Element(string StringRaw)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;

      if (ValidateStringRaw(StringRaw))
      {
        _RepeatDictonary = ParseElementRawStringToRepeat(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }
    internal Element(string StringRaw, IMessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;

      if (ValidateStringRaw(StringRaw))
      {
        _RepeatDictonary = ParseElementRawStringToRepeat(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }

    //Only internal Constructors
    internal Element(Field Field, MessageDelimiters CustomDelimiters, bool Temporary, int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      ValidateItemNotInUse(Field);
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      _RepeatDictonary = new Dictionary<int, Field>();
      Field._Parent = this;
      _RepeatDictonary.Add(1, Field);

    }
    internal Element(string StringRaw, MessageDelimiters CustomDelimiters, bool Temporary, int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;

      if (ValidateStringRaw(StringRaw))
      {
        _RepeatDictonary = ParseElementRawStringToRepeat(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }
    internal Element(ModelSupport.ContentTypeInternal ContentTypeInternal, MessageDelimiters CustomDelimiters, bool Temporary, int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      _IsMainSeparator = (ContentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator);
      _IsEncodingCharacters = (ContentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters);

      _RepeatDictonary = ParseElementRawStringToRepeat(string.Empty, ContentTypeInternal);
    }

    //Instance access
    public IMessageDelimiters MessageDelimiters
    {
      get
      {
        return this.Delimiters;
      }
    } 
    public IElement Clone()
    {
      return new Element(this.AsStringRaw, this.Delimiters, true, null, null);
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
          _RepeatDictonary = ParseElementRawStringToRepeat(value, ModelSupport.ContentTypeInternal.Unknown);
        }
      }
    }
    public bool IsEmpty
    {
      get
      {
        return (_RepeatDictonary.Count == 0);
      }
    }
    public bool IsHL7Null
    {
      get
      {
        return this.GetRepeat(1).IsHL7Null;
      }
    }
    public void ClearAll()
    {
      _RepeatDictonary.Clear();
      RemoveFromParent();
    }
    public void Set(int index, IContent item)
    {
      this.ContentSet(item as Content, index);
    }
    public void Set(int index, ISubComponent item)
    {
      this.SubComponentSet(item as SubComponent, index);
    }
    public void Add(IField item)
    {
      ValidateItemNotInUse(item as Field);
      this.RepeatAppend(item as Field);
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
    public void Insert(int index, IField item)
    {
      ValidateItemNotInUse(item as Field);
      this.RepeatInsertBefore(item as Field, index);
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
    public void RemoveRepeatAt(int index)
    {
      this.RepeatRemoveAt(index);
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
    public int RepeatCount
    {
      get
      {
        return this.CountRepeat;
      }
    }
    public int ComponentCount
    {
      get
      {
        return this.CountComponent;
      }
    }
    public int SubComponetCount
    {
      get
      {
        return this.CountSubComponet;
      }
    }
    public int ContentCount
    {
      get
      {
        return this.CountContent;
      }
    }
    public IField Repeat(int index)
    {
      if (index == 0)
        throw new PeterPiperArgumentException("Repeat is a one based index, zero is not a valid index");
      return this.GetRepeat(index);
    }
    public IComponent Component(int index)
    {
      if (index == 0)
        throw new PeterPiperArgumentException("Component is a one based index, zero is not a valid index");
      return this.GetComponent(index);
    }
    public ISubComponent SubComponent(int index)
    {
      if (index == 0)
        throw new PeterPiperArgumentException("SubComponent is a one based index, zero is not a valid index");
      return this.GetSubComponent(index);
    }
    public IContent Content(int index)
    {
      return this.GetContent(index);
    }
    public ReadOnlyCollection<IField> RepeatList
    {
      get
      {
        List<IField> oNewList = new List<IField>();
        int Counter = 1;
        foreach (var item in _RepeatDictonary.OrderBy(x => x.Key))
        {
          if (item.Key != Counter)
          {
            while (Counter != item.Key)
            {
              oNewList.Add(new Field(string.Empty, this.Delimiters, true, Counter, this));
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

    //Field / Repeat    
    internal Field GetRepeat(int index)
    {
      IsAccessibleElement();
      if (_RepeatDictonary.ContainsKey(index))
        return _RepeatDictonary[index];
      else
        return new Field(string.Empty, this.Delimiters, true, index, this);
    }
    internal int CountRepeat
    {
      get
      {
        if (_RepeatDictonary.Count > 0)
          return _RepeatDictonary.Keys.Max();
        else
          return 0;
      }
    }
    internal Field RepeatAppend(Field Repeat)
    {
      if (_RepeatDictonary.Count > 0)
      {
        return RepeatInsertBefore(Repeat, _RepeatDictonary.Keys.Max() + 1);
      }
      else
      {
        Repeat._Index = 1;
        Repeat._Parent = this;
        if (SetToDictonary(Repeat))
          Repeat._Temporary = false;
        return _RepeatDictonary[1];
      }
      //----------------------------------------------------
      //int InsertAtIndex = 1;
      //if (_RepeatDictonary.Count > 0)
      //  InsertAtIndex = _RepeatDictonary.Keys.Max() + 1;
      //Repeat._Index = InsertAtIndex;
      //Repeat._Parent = this;
      //Repeat._Temporary = false;
      //_RepeatDictonary.Add(InsertAtIndex, Repeat);
      //return _RepeatDictonary[_RepeatDictonary.Keys.Max()];
    }
    internal Field RepeatInsertBefore(Field Repeat, int Index)
    {
      if (Index == 0)
        throw new PeterPiperArgumentException("Element is a one based index, zero is not a valid index.");

      int RepeatInsertedAt = 0;
      //Empty Dic so just add as first itme 
      if (_RepeatDictonary.Count == 0)
      {
        RepeatInsertedAt = Index;
        Repeat._Index = RepeatInsertedAt;
        Repeat._Parent = this;
        if (SetToDictonary(Repeat))
          Repeat._Temporary = false;
      }
      //Asked to insert before an index larger than the largest in Dic so just add to the end
      else if (_RepeatDictonary.Keys.Max() < Index)
      {
        RepeatInsertedAt = Index;
        Repeat._Index = RepeatInsertedAt;
        Repeat._Parent = this;
        if (SetToDictonary(Repeat))
          Repeat._Temporary = false;
        //_ContentDictonary.Add(_ContentDictonary.Keys.Max() + 1, Content);
      }
      //Asked to insert within items already in the Dic so cycle through moving each item higher or equal up by one then just add the new item
      //The Content Dictonary is different than all the others as it is to never have gaps between items and it is Zero based.
      else
      {
        foreach (var item in _RepeatDictonary.Reverse())
        {
          if (item.Key >= Index)
          {
            item.Value._Index++;
            if (_RepeatDictonary.ContainsKey(item.Key + 1))
            {
              _RepeatDictonary.Remove(item.Key);
              _RepeatDictonary[item.Key + 1] = item.Value;
            }
            else
            {
              _RepeatDictonary.Remove(item.Key);
              _RepeatDictonary.Add(item.Key + 1, item.Value);
            }
          }
        }
        Repeat._Index = Index;
        Repeat._Parent = this;
        SetParent();
        Repeat._Temporary = false;
        _RepeatDictonary[Index] = Repeat;
        RepeatInsertedAt = Index;
      }
      return _RepeatDictonary[RepeatInsertedAt];
      
      //int RepeatInsertedAt = 0;
      //if (_RepeatDictonary.ContainsKey(Index))
      //{
      //  foreach (var item in _RepeatDictonary.Reverse())
      //  {
      //    if (item.Key >= Index)
      //    {
      //      item.Value._Index++;
      //      if (item.Key >= Index)
      //      {
      //        _RepeatDictonary.Remove(item.Key);
      //        item.Value._Index++;
      //        _RepeatDictonary.Add(item.Key + 1, item.Value);
      //      }
      //    }
      //  }
      //  RepeatInsertedAt = Index;
      //  Repeat._Index = RepeatInsertedAt;
      //  Repeat._Parent = this;
      //  if (SetToDictonary(Repeat))
      //    Repeat._Temporary = false;        
      //}
      //else
      //{
      //  RepeatInsertedAt = Index;
      //  Repeat._Index = RepeatInsertedAt;
      //  Repeat._Parent = this;
      //  if (SetToDictonary(Repeat))
      //    Repeat._Temporary = false;
      //}
      //return _RepeatDictonary[RepeatInsertedAt];
    }
    internal bool RepeatRemoveAt(int Index)
    {
      if (_RepeatDictonary.ContainsKey(Index))
      {
        Dictionary<int, Field> oNewDic = new Dictionary<int, Field>();
        foreach (var item in _RepeatDictonary)
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
        _RepeatDictonary = oNewDic;
        if (_RepeatDictonary.Count == 0)
          RemoveFromParent();
        return true;
      }
      return false;
    }

    //Component    
    internal Component GetComponent(int index)
    {
      if (_RepeatDictonary.ContainsKey(1))
        return _RepeatDictonary[1].GetComponent(index);
      else
      {
        //_RepeatDictonary = new Dictionary<int, Field>();
        //_RepeatDictonary.Add(1, new Field(string.Empty, this.Delimiters, true, 1, this));
        //return _RepeatDictonary[1].GetComponent(index);
        Field oField =  new Field(string.Empty, this.Delimiters, true, 1, this);
        return oField.GetComponent(index); 
      }
    }
    internal int CountComponent
    {
      get
      {
        if (_RepeatDictonary.ContainsKey(1))
          return _RepeatDictonary[1].CountComponent;
        else
          return 0;
      }
    }
    internal Component ComponentAppend(Component Component)
    {
      if (_RepeatDictonary.ContainsKey(1))
        return _RepeatDictonary[1].ComponentAppend(Component);
      else
      {
        //_RepeatDictonary = new Dictionary<int, Field>();
        //_RepeatDictonary.Add(1, new Field(string.Empty, this.Delimiters, true, 1, this));
        //_RepeatDictonary[1].ComponentAppend(Component);
        //return _RepeatDictonary[1].GetComponent(1);
        Field oField = new Field(string.Empty, this.Delimiters, true, 1, this);
        return oField.ComponentAppend(Component);
      }
    }
    internal Component ComponentInsertBefore(Component Component, int Index)
    {
      if (Index == 0)
        throw new PeterPiperArgumentException("Element is a one based index, zero is not a valid index.");

      if (_RepeatDictonary.ContainsKey(1))
        return _RepeatDictonary[1].ComponentInsertBefore(Component, Index);
      else
      {
        //_RepeatDictonary = new Dictionary<int, Field>();
        //_RepeatDictonary.Add(1, new Field(string.Empty, this.Delimiters, true, Index, this));
        //_RepeatDictonary[1].ComponentInsertBefore(Component, Index);
        //return _RepeatDictonary[1].GetComponent(Index);
        Field oField = new Field(string.Empty, this.Delimiters, true, Index, this);
        return oField.ComponentInsertBefore(Component, Index);
      }
    }
    internal bool ComponentRemoveAt(int Index)
    {
      if (_RepeatDictonary.ContainsKey(Index))      
        return _RepeatDictonary[1].ComponentRemoveAt(Index);
      else
        return false;
    }

    //SubComponent    
    internal SubComponent GetSubComponent(int index)
    {
      if (_RepeatDictonary.ContainsKey(1))
        return _RepeatDictonary[1].GetSubComponent(index);
      else
      {
        Field oField = new Field(string.Empty, this.Delimiters, true, 1, this);
        return oField.GetSubComponent(index);
      }
    }
    internal int CountSubComponet
    {
      get
      {
        if (_RepeatDictonary.ContainsKey(1))
          return _RepeatDictonary[1].CountSubComponent;
        else
          return 0;
      }
    }
    internal SubComponent SubComponentSet(SubComponent SubComponent, int Index)
    {
      if (_RepeatDictonary.ContainsKey(1))
      {
        return _RepeatDictonary[1].SubComponentSet(SubComponent, Index);
      }
      else
      {
        //_RepeatDictonary.Add(1, new Field(string.Empty, this.Delimiters, true, 1, this));
        //return _RepeatDictonary[1].ContentSet(Content, Index);
        Field oField = new Field(string.Empty, this.Delimiters, true, 1, this);
        return oField.SubComponentSet(SubComponent, Index);
      }
    }
    internal SubComponent SubComponentAppend(SubComponent SubComponent)
    {
      if (_RepeatDictonary.ContainsKey(1))
        return _RepeatDictonary[1].SubComponentAppend(SubComponent);
      else
      {
        //_RepeatDictonary = new Dictionary<int, Field>();
        //_RepeatDictonary.Add(1, new Field(string.Empty, this.Delimiters, true, 1, this));
        //_RepeatDictonary[1].SubComponentAppend(SubComponent);
        //return _RepeatDictonary[1].GetSubComponent(1);
        Field oField = new Field(string.Empty, this.Delimiters, true, 1, this);
        return oField.SubComponentAppend(SubComponent);
      }
    }
    internal SubComponent SubComponentInsertBefore(SubComponent SubComponent, int Index)
    {
      if (Index == 0)
        throw new PeterPiperArgumentException("Element is a one based index, zero is not a valid index.");

      if (_RepeatDictonary.ContainsKey(1))
        return _RepeatDictonary[1].SubComponentInsertBefore(SubComponent, Index);
      else
      {
        //_RepeatDictonary = new Dictionary<int, Field>();
        //_RepeatDictonary.Add(1, new Field(string.Empty, this.Delimiters, true, 1, this));
        //_RepeatDictonary[1].SubComponentInsertBefore(SubComponent, Index);
        //return _RepeatDictonary[1].GetSubComponent(Index);
        Field oField = new Field(string.Empty, this.Delimiters, true, 1, this);
        return oField.SubComponentInsertBefore(SubComponent, Index);
      }
    }
    internal bool SubComponentRemoveAt(int Index)
    {
      if (_RepeatDictonary.ContainsKey(Index))
      {
        return _RepeatDictonary[1].SubComponentRemoveAt(Index);
      }
      return false;
    }

    //Content        
    internal Content GetContent(int index)
    {
      if (_RepeatDictonary.ContainsKey(1))
        return _RepeatDictonary[1].GetContent(index);
      else
      {
        //_RepeatDictonary = new Dictionary<int, Field>();
        //_RepeatDictonary.Add(1, new Field(string.Empty, this.Delimiters, true, 1, this));
        //return _RepeatDictonary[1].GetContent(index);
        Field oField = new Field(string.Empty, this.Delimiters, true, 1, this);
        return oField.GetContent(index);
      }
    }
    internal int CountContent
    {
      get
      {
        if (_RepeatDictonary.ContainsKey(1))
          return _RepeatDictonary[1].CountContent;
        else
          return 0;
      }
    }
    internal Content ContentSet(Content Content, int Index)
    {
      if (_RepeatDictonary.ContainsKey(1))
      {
        return _RepeatDictonary[1].ContentSet(Content, Index);
      }
      else
      {
        //_RepeatDictonary.Add(1, new Field(string.Empty, this.Delimiters, true, 1, this));
        //return _RepeatDictonary[1].ContentSet(Content, Index);
        Field oField = new Field(string.Empty, this.Delimiters, true, 1, this);
        return oField.ContentSet(Content, Index);
      }
    }
    internal Content ContentAppend(Content Content)
    {
      if (_RepeatDictonary.ContainsKey(1))
        return _RepeatDictonary[1].ContentAppend(Content);
      else
      {
        //_RepeatDictonary = new Dictionary<int, Field>();
        //_RepeatDictonary.Add(1, new Field(string.Empty, this.Delimiters, true, 1, this));
        //_RepeatDictonary[1].ContentAppend(Content);
        //return _RepeatDictonary[1].GetContent(0);
        Field oField = new Field(string.Empty, this.Delimiters, true, 1, this);
        return oField.ContentAppend(Content);
      }
    }
    internal Content ContentInsertBefore(Content Content, int Index)
    {
      if (_RepeatDictonary.ContainsKey(1))
        return _RepeatDictonary[1].ContentInsertBefore(Content, Index);
      else
      {
        //_RepeatDictonary = new Dictionary<int, Field>();
        //_RepeatDictonary.Add(1, new Field(string.Empty, this.Delimiters, true, 1, this));
        //return _RepeatDictonary[1].GetContent(0);
        Field oField = new Field(string.Empty, this.Delimiters, true, 1, this);
        return oField.GetContent(0);
      }
    }
    internal bool ContentRemoveAt(int Index)
    {
      if (_RepeatDictonary.ContainsKey(Index))
      {
        return _RepeatDictonary[1].ContentRemoveAt(Index);
      }
      return false;
    }

    //Building
    private string GetAsStringOrAsRawString(bool RawString)
    {
      if (_RepeatDictonary.Count == 0)
        return string.Empty;
      StringBuilder oStringBuilder = new StringBuilder();
      _RepeatDictonary.OrderByDescending(i => i.Key);
      for (int i = 1; i < _RepeatDictonary.Keys.Max() + 1; i++)
      {
        if (_RepeatDictonary.ContainsKey(i))
        {
          if (RawString)
            oStringBuilder.Append(_RepeatDictonary[i].AsStringRaw);
          else
            oStringBuilder.Append(_RepeatDictonary[i].AsString);
        }
        if (i != _RepeatDictonary.Keys.Max())
          oStringBuilder.Append(this.Delimiters.Repeat);
      }
      return oStringBuilder.ToString();
    }

    //Maintenance
    internal bool SetToDictonary(Field oField)
    {
      try
      {
        if (_RepeatDictonary.ContainsKey(System.Convert.ToInt32(oField._Index)))
        {
          _RepeatDictonary[System.Convert.ToInt32(oField._Index)]._Temporary = true;
          _RepeatDictonary[System.Convert.ToInt32(oField._Index)]._Parent = null;
          _RepeatDictonary[System.Convert.ToInt32(oField._Index)]._Index = null;
          _RepeatDictonary[System.Convert.ToInt32(oField._Index)] = oField;
        }
        else
        {
          _RepeatDictonary.Add(System.Convert.ToInt32(oField._Index), oField);
        }
        SetParent();
        return true;
      }
      catch (Exception Exec)
      {
        throw new PeterPiperException("Error setting Field into Element parent", Exec);
      }
    }
    private void SetParent()
    {
      if (this._Temporary)
      {
        if (this._Parent is Segment)
        {
          Segment oSegment = this._Parent as Segment;
          if (oSegment.SetToDictonary(this))
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
        if (_RepeatDictonary.ContainsKey(Index))
        {
          _RepeatDictonary[Index]._Temporary = true;
          _RepeatDictonary[Index]._Index = null;
          _RepeatDictonary[Index]._Parent = null;
        }
        _RepeatDictonary.Remove(Index);
        if (_RepeatDictonary.Count == 0)
        {
          RemoveFromParent();
        }
      }
      catch
      {
        throw new PeterPiperException(String.Format("Elements's Repeat dictionary did not contain repeat Index {0} for removal call from Repeat Instance", Index));
      }
    }
    private void RemoveFromParent()
    {
      if (this._Index != null)
      {
        try
        {
          Segment oSegment = this._Parent as Segment;
          oSegment.RemoveChild(System.Convert.ToInt32(this._Index));
        }
        catch (InvalidCastException oInvalidCastExec)
        {
          throw new PeterPiperException("Casting of Elements parent to Segment throws Invalid Cast Exception, check inner exception for more detail", oInvalidCastExec);
        }
      }
    }

    //Parsing and Validation
    private Dictionary<int, Field> ParseElementRawStringToRepeat(String StringRaw, ModelSupport.ContentTypeInternal ContentTypeInternal)
    {
      //Example:  "First1^Second1^Third1^~First2^Second2&\\H\\Second22\\N\\^Third2";
      _RepeatDictonary = new Dictionary<int, Field>();
      if (ContentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters || ContentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator)
      {
        _RepeatDictonary.Add(1, new Field(ContentTypeInternal, this.Delimiters, false, 1, this));
      }
      else
      {
        if (StringRaw.Contains(this.Delimiters.Repeat))
        {
          string[] ElementParts = StringRaw.Split(this.Delimiters.Repeat);
          int RepeatPositionCounter = 1;
          foreach (string Part in ElementParts)
          {
            if (Part != string.Empty)
            {
              //_RepeatDictonary.Add(RepeatPositionCounter, new Field(Part, this.Delimiters, false, RepeatPositionCounter, this));
              new Field(Part, this.Delimiters, true, RepeatPositionCounter, this);
            }
            RepeatPositionCounter++;
          }
        }
        else
        {
          //_RepeatDictonary.Add(1, new Field(StringRaw, this.Delimiters, false, 1, this));
          new Field(StringRaw, this.Delimiters, true, 1, this);
        }
      }
      return _RepeatDictonary;
    }
    private bool ValidateStringRaw(string StringRaw)
    {
      Char[] CharatersNotAlowed = { this.Delimiters.Field };

      if (StringRaw.IndexOfAny(CharatersNotAlowed) != -1)
      {
        throw new PeterPiperArgumentException(String.Format("Element data cannot contain HL7 V2 Delimiters of : {0}", CharatersNotAlowed));
      }
      return true;
    }
    internal bool IsAccessibleElement()
    {
      if (this._IsMainSeparator)
      {
        StringBuilder sb = new StringBuilder();
        sb.Append("MSH-1 contains the 'Main Separator' character and is not accessible from the Field or Element object instance as it is critical to message construction.");
        sb.Append(Environment.NewLine);
        sb.Append("Instead, you can access the read only property call 'MainSeparator' from the Message object instance.");
        throw new PeterPiperArgumentException(sb.ToString());
      }
      else if (this._IsEncodingCharacters)
      {
        StringBuilder sb = new StringBuilder();
        sb.Append("MSH-2 contains the 'Encoding Characters' and is not accessible from the Field or Element object instance as it is critical to message construction.");
        sb.Append(Environment.NewLine);
        sb.Append("Instead, you can access the read only property call 'EscapeSequence' from the Message object instance.");
        throw new PeterPiperArgumentException(sb.ToString());
      }
      return true;
    }
  }

}

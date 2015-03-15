using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Model
{
  public class SubComponent : ContentBase
  {

    private Dictionary<int, Content> _ContentDictonary;

    //Constructors
    public SubComponent()
    {
      _ContentDictonary = new Dictionary<int, Content>();
      _Temporary = true;
      _Index = null;
      _Parent = null;
    }
    public SubComponent(Support.MessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _ContentDictonary = new Dictionary<int, Content>();
      _Temporary = true;
      _Index = null;
      _Parent = null;
    }
    public SubComponent(string StringRaw)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      if (ValidateStringRaw(StringRaw))
      {
        _ContentDictonary = ParseSubComponentRawStringToContent(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }
    public SubComponent(string StringRaw, Support.MessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      if (ValidateStringRaw(StringRaw))
      {
        _ContentDictonary = ParseSubComponentRawStringToContent(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }

    internal SubComponent(Content Content, Support.MessageDelimiters CustomDelimiters,
                          Boolean Temporary, int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      ValidateItemNotInUse(Content);
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      _ContentDictonary = new Dictionary<int, Content>();
      Content._Index = 0;
      Content._Parent = this;
      Content._Temporary = false;
      _ContentDictonary.Add(0, Content);
      this.SetParent();
    }
    internal SubComponent(string StringRaw, Support.MessageDelimiters CustomDelimiters,
                          Boolean Temporary, int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      if (ValidateStringRaw(StringRaw))
      {
        _ContentDictonary = ParseSubComponentRawStringToContent(StringRaw, ModelSupport.ContentTypeInternal.Unknown);
      }
    }
    internal SubComponent(ModelSupport.ContentTypeInternal ContentTypeInternal,
                          Support.MessageDelimiters CustomDelimiters, Boolean Temporary,
                          int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      _ContentDictonary = ParseSubComponentRawStringToContent(string.Empty, ContentTypeInternal);
    }

    //Instance access
    public SubComponent Clone()
    {
      return new SubComponent(this.AsStringRaw, this.Delimiters, true, null, null);
    }
    public override string ToString()
    {
      return this.AsString;
    }
    public override string AsString
    {
      get
      {
        if (_ContentDictonary.Count == 0)
          return string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();
        for (int i = 0; i < _ContentDictonary.Keys.Max() + 1; i++)
        {
          if (_ContentDictonary.ContainsKey(i))
          {
            if (_ContentDictonary[i].ContentType == Support.Content.ContentType.Text)
            {
              oStringBuilder.Append(_ContentDictonary[i].AsString);
            }
            else
            {
              // oStringBuilder.Append(this.Delimiters.Escape);
              oStringBuilder.Append(_ContentDictonary[i].AsString);
              // oStringBuilder.Append(this.Delimiters.Escape);
            }
          }
          else
          {
            throw new ApplicationException("Content dictonary has non-sequential items, this is a libaray error.");
          }
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
        if (_ContentDictonary.Count == 0)
          return string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();
        for (int i = 0; i < _ContentDictonary.Keys.Max() + 1; i++)
        {
          if (_ContentDictonary.ContainsKey(i))
          {
            if (_ContentDictonary[i].ContentType == Support.Content.ContentType.Text)
            {
              oStringBuilder.Append(_ContentDictonary[i].AsStringRaw);
            }
            else
            {
              oStringBuilder.Append(this.Delimiters.Escape);
              oStringBuilder.Append(_ContentDictonary[i].AsStringRaw);
              oStringBuilder.Append(this.Delimiters.Escape);
            }
          }
          else
          {
            throw new ApplicationException("Content dictonary has non-sequential items, this is a libaray error.");
          }
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
          _ContentDictonary = ParseSubComponentRawStringToContent(value, ModelSupport.ContentTypeInternal.Unknown);
        }
      }
    }
    public bool IsEmpty
    {
      get
      {
        return (_ContentDictonary.Count == 0);
      }
    }
    public bool IsHL7Null
    {
      get
      {
        return this.GetContent(0).IsHL7Null;
      }
    }
    public void ClearAll()
    {
      _ContentDictonary.Clear();
      RemoveFromParent();
    }
    public void Set(int index, Content item)
    {
      ValidateItemNotInUse(item);
      this.ContentSet(index, item);
    }
    public void Add(Content item)
    {
      ValidateItemNotInUse(item);
      this.ContentAppend(item);
    }
    public void Insert(int index, Content item)
    {
      ValidateItemNotInUse(item);
      this.ContentInsertBefore(item, index);
    }
    public void RemoveContentAt(int index)
    {
      this.ContentRemoveAt(index);
    }
    public int ContentCount
    {
      get
      {
        return this.CountContent;
      }
    }
    public Content Content(int index)
    {
      return this.GetContent(index);
    }
    public ReadOnlyCollection<Content> ContentList
    {
      get
      {
        return _ContentDictonary.OrderBy(x => x.Key).Select(i => i.Value).ToList().AsReadOnly();
      }
    }

    //Content    
    internal Content GetContent(int index)
    {
      if (_ContentDictonary.ContainsKey(index))
        return _ContentDictonary[index];
      else
      {
        if (_ContentDictonary.Count == 1 && _ContentDictonary[0]._Temporary == true)
        {
          return new Content(string.Empty, ModelSupport.ContentTypeInternal.Text, this.Delimiters, true, 0, this);
        }
        else
        {
          if (_ContentDictonary.Count == 0)
          {
            return new Content(string.Empty, ModelSupport.ContentTypeInternal.Text, this.Delimiters, true, 0, this);
          }
          else
          {
            return new Content(string.Empty, ModelSupport.ContentTypeInternal.Text, this.Delimiters, true, _ContentDictonary.Keys.Max() + 1, this);
          }
        }
      }
    }
    internal int CountContent
    {
      get
      {
        if (_ContentDictonary.Count == 0)
          return 0;
        else
          return _ContentDictonary.Keys.Max() + 1;
      }
    }
    internal Content ContentSet(int Index, Content Content)
    {
      if (Content.AsStringRaw == String.Empty)
      {
        RemoveChild(Index);
        Content._Index = null;
        Content._Parent = null;
        Content._Temporary = true;
        return Content;
      }
      else
      {
        Content._Index = Index;
        Content._Parent = this;
        if (SetToDictonary(Content))
          Content._Temporary = false;
        return _ContentDictonary[Convert.ToInt32(Content._Index)];
      }
    }
    internal Content ContentAppend(Content Content)
    {
      if (_ContentDictonary.Count > 0)
      {
        return ContentInsertBefore(Content, _ContentDictonary.Keys.Max() + 1);
      }
      else
      {
        Content._Index = 0;
        Content._Parent = this;
        if (SetToDictonary(Content))
          Content._Temporary = false;
        return _ContentDictonary[0];
      }
    }
    internal Content ContentInsertBefore(Content Content, int Index)
    {
      int LastIndexUsed = 0;
      //Empty Dic so just add as first itme 
      if (_ContentDictonary.Count == 0)
      {
        LastIndexUsed = 0;
        Content._Index = LastIndexUsed;
        Content._Parent = this;
        if (SetToDictonary(Content))
          Content._Temporary = false;
      }
      //Asked to insert before an index larger than the largest in Dic so just add to the end
      else if (_ContentDictonary.Keys.Max() < Index)
      {
        LastIndexUsed = _ContentDictonary.Keys.Max() + 1;
        Content._Index = LastIndexUsed;
        Content._Parent = this;
        if (SetToDictonary(Content))
          Content._Temporary = false;
        //_ContentDictonary.Add(_ContentDictonary.Keys.Max() + 1, Content);
      }
      //Asked to insert within items already in the Dic so cycle through moving each item higher or equal up by one then just add the new item
      //The Content Dictonary is different than all the others as it is to never have gaps between items and it is Zero based.
      else
      {
        foreach (var item in _ContentDictonary.Reverse())
        {
          if (item.Key >= Index)
          {
            item.Value._Index++;
            if (_ContentDictonary.ContainsKey(item.Key + 1))
            {
              _ContentDictonary[item.Key + 1] = item.Value;
            }
            else
            {
              _ContentDictonary.Add(item.Key + 1, item.Value);
            }
          }
        }
        Content._Index = Index;
        Content._Parent = this;
        SetParent();
        Content._Temporary = false;
        _ContentDictonary[Index] = Content;
        LastIndexUsed = Index;
      }
      return _ContentDictonary[LastIndexUsed];
    }
    internal bool ContentRemoveAt(int Index)
    {
      if (_ContentDictonary.Count >= Index + 1)
      {
        Dictionary<int, Content> oNewDic = new Dictionary<int, Content>();
        foreach (var item in _ContentDictonary)
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
        _ContentDictonary = oNewDic;
        if (_ContentDictonary.Count == 0)
          RemoveFromParent();
        return true;
      }
      else
      {
        return false;
      }
    }

    //Maintenance
    internal bool SetToDictonary(Content oContent)
    {
      try
      {
        if (_ContentDictonary.ContainsKey(Convert.ToInt32(oContent._Index)))
        {
          //this 1 line n=below is new is it ok?. Do we need to do this else where, I think so?
          _ContentDictonary[Convert.ToInt32(oContent._Index)]._Temporary = true;
          _ContentDictonary[Convert.ToInt32(oContent._Index)]._Parent = null;
          _ContentDictonary[Convert.ToInt32(oContent._Index)]._Index = null;
          //---------------------------------------
          _ContentDictonary[Convert.ToInt32(oContent._Index)] = oContent;
        }
        else
        {
          if (_ContentDictonary.Count > 0)
          {
            int InsertContentIndex = _ContentDictonary.Keys.Max() + 1;
            oContent._Index = InsertContentIndex;
            _ContentDictonary.Add(InsertContentIndex, oContent);
          }
          else
          {
            oContent._Index = 0;
            _ContentDictonary.Add(0, oContent);
          }
        }
        SetParent();
        return true;
      }
      catch (Exception Exec)
      {
        throw new ApplicationException("Error setting Content into SubComponent Parent", Exec);
      }
    }
    private void SetParent()
    {
      if (this._Temporary)
      {
        if (this._Parent is Component)
        {
          Component oComponent = this._Parent as Component;
          if (oComponent.SetToDictonary(this))
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
        if (_ContentDictonary.ContainsKey(Index))
        {
          _ContentDictonary[Index]._Temporary = true;
          _ContentDictonary[Index]._Index = null;
          _ContentDictonary[Index]._Parent = null;
        }
        this.ContentRemoveAt(Index);
        if (_ContentDictonary.Count == 0)
        {
          RemoveFromParent();
        }
      }
      catch
      {
        throw new ApplicationException(String.Format("SubComponent's ContentDictonary Dictonary did not contain Content Index {0} for removal call from Content Instance", Index));
      }
    }
    private void RemoveFromParent()
    {
      if (this._Index != null)
      {
        try
        {
          Component oComponent = this._Parent as Component;
          oComponent.RemoveChild(Convert.ToInt32(this._Index));
        }
        catch (InvalidCastException oInvalidCastExec)
        {
          throw new ApplicationException("Casting of SubComponent's parent to Component throws Invalid Cast Exception, check innner exception for more detail", oInvalidCastExec);
        }
      }
    }
    private void ConstructorCommon()
    {
    }
    
    //Parsing and Validation
    private Dictionary<int, Content> ParseSubComponentRawStringToContent(String StringRaw, ModelSupport.ContentTypeInternal ContentTypeInternal)
    {
      //Example:  "\\N\\this is not Highlighted\\H\\ This is higlighted \\N\\ This is not";
      _ContentDictonary = new Dictionary<int, Content>();
      if (ContentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters || ContentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator)
      {
        _ContentDictonary.Add(0, new Content(string.Empty, ContentTypeInternal, this.Delimiters, false, 0, this));
      }
      else
      {
        if (StringRaw.Contains(this.Delimiters.Escape))
        {
          string[] ContentParts = StringRaw.Split(this.Delimiters.Escape);
          bool InEscape = false;
          int SubCompoenentPositionCounter = 0;
          foreach (string Part in ContentParts)
          {
            if (InEscape)
            {
              new Content(Part.Trim(), ModelSupport.ContentTypeInternal.Escape, this.Delimiters, true, SubCompoenentPositionCounter, this);
              SubCompoenentPositionCounter++;
              InEscape = false;
            }
            else
            {
              if (Part != string.Empty)
              {
                new Content(Part, ModelSupport.ContentTypeInternal.Text, this.Delimiters, true, SubCompoenentPositionCounter, this);
                SubCompoenentPositionCounter++;
              }
              InEscape = true;
            }
          }
        }
        else
        {
          new Content(StringRaw, ModelSupport.ContentTypeInternal.Text, this.Delimiters, true, 0, this);
        }
      }
      return _ContentDictonary;
    }
    private bool ValidateStringRaw(string StringRaw)
    {
      Char[] CharatersNotAlowed = { this.Delimiters.Field,
                                    this.Delimiters.Component,
                                    this.Delimiters.SubComponent,
                                    this.Delimiters.Repeat};

      if (StringRaw.IndexOfAny(CharatersNotAlowed) != -1)
      {
        string DelimitersNotAlowed = String.Format("{0}{1}{2}{3}", CharatersNotAlowed[0], CharatersNotAlowed[1], CharatersNotAlowed[2], CharatersNotAlowed[3]);
        throw new System.ArgumentException(String.Format("SubComponent data cannot contain HL7 V2 Delimiters of : {0}", DelimitersNotAlowed));
      }
      return true;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


namespace Glib.Hl7.V2.Model
{
  public class Segment : ModelBase
  {
    private Dictionary<int, Element> _ElementDictonary;
  
    //Constructors
    public Segment(string StringRaw)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      StringRaw = ValidateStringRaw(StringRaw);
      _ElementDictonary = ParseSegmentRawStringToElement(StringRaw);
    }
    public Segment(string StringRaw, Support.MessageDelimiters CustomDelimiters)
      : base(CustomDelimiters)
    {
      _Temporary = true;
      _Index = null;
      _Parent = null;
      StringRaw = ValidateStringRaw(StringRaw);
      _ElementDictonary = ParseSegmentRawStringToElement(StringRaw);
    }
    internal Segment(string StringRaw, Support.MessageDelimiters CustomDelimiters, bool Temporary, int? Index, ModelBase Parent)
      : base(CustomDelimiters)
    {
      _Temporary = Temporary;
      _Index = Index;
      _Parent = Parent;
      StringRaw = ValidateStringRaw(StringRaw);
      _ElementDictonary = ParseSegmentRawStringToElement(StringRaw);
    }    

    //Instance access
    public Support.MessageDelimiters MessageDelimiters
    {
      get
      {
        return this.Delimiters;
      }
    } 
    public Segment Clone()
    {
      return new Segment(this.AsStringRaw, this.Delimiters, true, null, null);
    }    
    public override string ToString()
    {
      return this.AsString;
    }
    public override string AsString
    {
      get
      {
        string SegmentPrefix = string.Empty;
        if (_IsMSH)
          SegmentPrefix = string.Format("{0}", this._Code);
        else
          SegmentPrefix = string.Format("{0}{1}", this._Code, this.Delimiters.Field);

        if (this._IsMSH && _ElementDictonary.Count == 2)
        {
          return string.Format("{0}{1}{2}{3}", SegmentPrefix, this.Delimiters.Field, _ElementDictonary[2].AsStringRaw, this.Delimiters.Field); ;
        }
        else if (_ElementDictonary.Count == 0)
        {
          return SegmentPrefix;
        }
        StringBuilder oStringBuilder = new StringBuilder(SegmentPrefix);
        _ElementDictonary.OrderByDescending(i => i.Key);
        for (int i = 1; i < _ElementDictonary.Keys.Max() + 1; i++)
        {
          if (_ElementDictonary.ContainsKey(i))
          {
            oStringBuilder.Append(_ElementDictonary[i].AsString);
          }
          if (i != _ElementDictonary.Keys.Max())
          {
            if (_IsMSH)
            {
              if (i != 1)
                oStringBuilder.Append(this.Delimiters.Field);
            }
            else
            {
              oStringBuilder.Append(this.Delimiters.Field);
            }
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
        string SegmentPrefix = string.Empty;
        if (_IsMSH)
          SegmentPrefix = string.Format("{0}", this._Code);
        else
          SegmentPrefix = string.Format("{0}{1}", this._Code, this.Delimiters.Field);

        if (this._IsMSH && _ElementDictonary.Count == 2)
        {
          return string.Format("{0}{1}{2}{3}", SegmentPrefix, this.Delimiters.Field, _ElementDictonary[2].AsStringRaw, this.Delimiters.Field); ;
        }
        else if (_ElementDictonary.Count == 0)
        {
          return SegmentPrefix;
        }
        StringBuilder oStringBuilder = new StringBuilder(SegmentPrefix);
        _ElementDictonary.OrderByDescending(i => i.Key);
        for (int i = 1; i < _ElementDictonary.Keys.Max() + 1; i++)
        {
          if (_ElementDictonary.ContainsKey(i))
          {
            oStringBuilder.Append(_ElementDictonary[i].AsStringRaw);
          }
          if (i != _ElementDictonary.Keys.Max())
          {
            if (_IsMSH)
            {
              if (i != 1)
                oStringBuilder.Append(this.Delimiters.Field);
            }
            else
            {
              oStringBuilder.Append(this.Delimiters.Field);
            }
          }          
        }
        return oStringBuilder.ToString();
      }
      set
      {
        if (this._IsMSH || this._Index == 1)
          throw new ArgumentException("Unable to modify an existing MSH segment instance with the AsString or AsStringRaw properties. /n You need to create a new Segment instance and use it's constructor or selectively edit this segment's parts.");
        value = ValidateStringRaw(value);
        _ElementDictonary = ParseSegmentRawStringToElement(value);
      }
    }
    public bool IsEmpty
    {
      get
      {
        return (_ElementDictonary.Count == 0);
      }
    }
    public void ClearAll()
    {
      if (this._IsMSH)
      {
        Dictionary<int, Element> oNewDic = new Dictionary<int, Model.Element>();
        oNewDic.Add(1, new Element(ModelSupport.ContentTypeInternal.MainSeparator, _ElementDictonary[1].Delimiters, false, 1, this));
        oNewDic.Add(2, new Element(ModelSupport.ContentTypeInternal.EncodingCharacters, _ElementDictonary[2].Delimiters, false, 2, this));       
        _ElementDictonary = oNewDic;
      }
      else
      {
        _ElementDictonary.Clear();
      }
    }   
    private string _Code;
    public string Code
    {
      get
      {
        return _Code;
      }
    }    
    private bool _IsMSH = false;
    internal bool IsMSH
    {
      get
      {
        return _IsMSH;
      }
    }    
    public void Add(Element item)
    {
      ValidateItemNotInUse(item);
      this.ElementAppend(item);
    }
    public void Add(Field item)
    {
      ValidateItemNotInUse(item);
      this.FieldAppend(item);
    }
    public void Insert(int index, Element item)
    {
      if (index == 0)
        throw new ArgumentOutOfRangeException("Element index is a one based index, zero in not allowed");
      ValidateItemNotInUse(item);
      this.ElementInsertBefore(item, index);
    }
    public void Insert(int index, Field item)
    {
      if (index == 0)
        throw new ArgumentOutOfRangeException("Field is a one based index, zero is not a valid index.");
      ValidateItemNotInUse(item);
      this.FieldInsertBefore(item, index);
    }
    public void RemoveElementAt(int index)
    {
      if (index == 0)
        throw new ArgumentOutOfRangeException("Element index is a one based index, zero in not allowed");
      this.ElementRemoveAt(index);
    }
    public void RemoveFieldAt(int index)
    {
      if (index == 0)
        throw new ArgumentOutOfRangeException("Element index is a one based index, zero in not allowed");
      this.FieldRemoveAt(index);
    }
    public int ElementCount
    {
      get
      {
        return this.CountElement;
      }
    }
    public int FieldCount
    {
      get
      {
        return this.CountField;
      }
    }
    public Element Element(int index)
    {
      if (index == 0)
        throw new ArgumentOutOfRangeException("Element index is a one based index, zero in not allowed");
      return this.GetElement(index);
    }    
    public Field Field(int index)
    {
      if (Index == 0)
        throw new ArgumentOutOfRangeException("Element index is a one based index, zero in not allowed");
      return this.GetField(index);
    }    
    public ReadOnlyCollection<Element> ElementList
    {
      get
      {
        List<Element> oNewList = new List<Element>();
        int Counter = 1;
        foreach (var item in _ElementDictonary.OrderBy(x => x.Key))
        {
          if (item.Key != Counter)
          {
            while (Counter != item.Key)
            {
              oNewList.Add(new Element(string.Empty, this.Delimiters, true, Counter, this));
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

    //Element    
    internal Element GetElement(int index)
    {
      if (_ElementDictonary.ContainsKey(index))
      {
        _ElementDictonary[index].IsAccessibleElement();
        return _ElementDictonary[index];
      }
      else
        return new Element(string.Empty, this.Delimiters, true, index, this);
    }
    internal int CountElement
    {
      get
      {
        if (_ElementDictonary.Count > 0)
          return _ElementDictonary.Keys.Max();
        else
          return 0;
      }
    }
    internal Element ElementAppend(Element Element)
    {
      if (_ElementDictonary.Count > 0)
      {
        return ElementInsertBefore(Element, _ElementDictonary.Keys.Max() + 1);
      }
      else
      {
        Element._Index = 1;
        Element._Parent = this;
        if (SetToDictonary(Element))
          Element._Temporary = false;
        return _ElementDictonary[1];
      }
      //-----------------------------------------------
      //int InsertAtIndex = 1;
      //if (_ElementDictonary.Count > 0)
      //  InsertAtIndex = _ElementDictonary.Keys.Max() + 1;
      //Element._Index = InsertAtIndex;
      //Element._Parent = this;
      //Element._Temporary = false;
      //_ElementDictonary.Add(InsertAtIndex, Element);
      //return _ElementDictonary[_ElementDictonary.Keys.Max()];
    }
    internal Element ElementInsertBefore(Element Element, int Index)
    {
      int ElementInsertedAt = 0;

      if (this._IsMSH)
      {
       if (Index == 1 || Index == 2)
         if (_ElementDictonary.ContainsKey(Index))
           _ElementDictonary[Index].IsAccessibleElement();
      }
      //Empty Dic so just add as first itme 
      if (_ElementDictonary.Count == 0)
      {
        ElementInsertedAt = Index;
        Element._Index = ElementInsertedAt;
        Element._Parent = this;        
        if (SetToDictonary(Element))
          Element._Temporary = false;
      }
      //Asked to insert before an index larger than the largest in Dic so just add to the end
      else if (_ElementDictonary.Keys.Max() < Index)
      {
        ElementInsertedAt = Index;
        Element._Index = ElementInsertedAt;
        Element._Parent = this;
        if (SetToDictonary(Element))
          Element._Temporary = false;
        //_ContentDictonary.Add(_ContentDictonary.Keys.Max() + 1, Content);
      }
      //Asked to insert within items already in the Dic so cycle through moving each item higher or equal up by one then just add the new item
      //The Content Dictonary is different than all the others as it is to never have gaps between items and it is Zero based.
      else
      {
        foreach (var item in _ElementDictonary.Reverse())
        {
          if (item.Key >= Index)
          {
            item.Value._Index++;
            if (_ElementDictonary.ContainsKey(item.Key + 1))
            {
              _ElementDictonary.Remove(item.Key);
              _ElementDictonary[item.Key + 1] = item.Value;
            }
            else
            {
              _ElementDictonary.Remove(item.Key);
              _ElementDictonary.Add(item.Key + 1, item.Value);
            }
          }
        }
        Element._Index = Index;
        Element._Parent = this;
        SetParent();
        Element._Temporary = false;
        _ElementDictonary[Index] = Element;
        ElementInsertedAt = Index;
      }
      return _ElementDictonary[ElementInsertedAt];
      //int ElementInsertedAt = 0;
      //if (_ElementDictonary.ContainsKey(Index))
      //{
      //  _ElementDictonary[Index].IsAccessibleElement();        
      //  foreach (var item in _ElementDictonary.OrderBy(x => x.Key).Reverse())
      //  {
      //    if (item.Key >= Index)
      //    {                       
      //      _ElementDictonary.Remove(item.Key);
      //      item.Value._Index++;
      //      _ElementDictonary.Add(item.Key + 1, item.Value);           
      //    }
      //  }
      //  ElementInsertedAt = Index;
      //  Element._Index = ElementInsertedAt;
      //  Element._Parent = this;
      //  if (SetToDictonary(Element))
      //    Element._Temporary = false;        
      //}
      //else
      //{
      //  ElementInsertedAt = Index;
      //  Element._Index = ElementInsertedAt;
      //  Element._Parent = this;
      //  if (SetToDictonary(Element))
      //    Element._Temporary = false;
      //}
      //return _ElementDictonary[ElementInsertedAt];
    }    
    internal bool ElementRemoveAt(int Index)
    {
      if (_ElementDictonary.Keys.Max() >= Index)
      {
        if (this._IsMSH)
        {
          if (_ElementDictonary.ContainsKey(Index))
             _ElementDictonary[Index].IsAccessibleElement();
        }
        Dictionary<int, Element> oNewDic = new Dictionary<int, Element>();
        foreach (var item in _ElementDictonary)
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
        _ElementDictonary = oNewDic;       
        return true;
      }
      return false;
      //----------------------------------------------


      //if (_ElementDictonary.ContainsKey(Index))
      //{
      //  _ElementDictonary[Index].IsAccessibleElement();
      //  _ElementDictonary.Remove(Index);
      //  return true;
      //}
      //return false;
    }

    //Field    
    internal Field GetField(int index)
    {
      if (_ElementDictonary.ContainsKey(index))
        return _ElementDictonary[index].GetRepeat(1);
      else
      {
        Element oElement = new Element(String.Empty, this.Delimiters, true, index, this);
        return oElement.RepeatAppend(new Field(string.Empty, this.Delimiters, true, 1, oElement));        
      }
    }
    internal int CountField
    {
      get
      {
        return this.CountElement;
      }
    }
    internal Field FieldAppend(Field Field)
    {
      if (_ElementDictonary.Count > 0)
      {
        Element oElement = new Element(string.Empty, this.Delimiters, true, _ElementDictonary.Keys.Max() + 1, this);        
        return oElement.RepeatAppend(Field);                
      }
      else
      {
        //_ElementDictonary = new Dictionary<int, Element>();
        //_ElementDictonary.Add(1, new Element(string.Empty, this.Delimiters, true, 1, this));
        //_ElementDictonary[1].RepeatAppend(Field);
        //return _ElementDictonary[1].GetRepeat(1);
        Element oElement = new Element(string.Empty, this.Delimiters, true, 1, this);
        return oElement.RepeatAppend(Field);
      }
    }        
    internal Field FieldInsertBefore(Field Field, int Index)
    {   
      Element oElement = new Element(String.Empty, this.Delimiters, true,Index, this);
      return this.ElementInsertBefore(oElement, Index).RepeatAppend(Field);     
    }
    internal bool FieldRemoveAt(int Index)
    {
      return this.ElementRemoveAt(Index);
    }

    //Maintenance
    internal bool SetToDictonary(Element oElement)
    {
      try
      {
        if (_ElementDictonary.ContainsKey(Convert.ToInt32(oElement._Index)))
        {
          _ElementDictonary[Convert.ToInt32(oElement._Index)]._Temporary = true;
          _ElementDictonary[Convert.ToInt32(oElement._Index)]._Parent = null;
          _ElementDictonary[Convert.ToInt32(oElement._Index)]._Index= null;
          _ElementDictonary[Convert.ToInt32(oElement._Index)] = oElement;
        }
        else
        {
          _ElementDictonary.Add(Convert.ToInt32(oElement._Index), oElement);
        }        
        SetParent();
        return true;
      }
      catch (Exception Exec)
      {
        throw new ApplicationException("Error setting Element into Segment parent", Exec);
      }
    }
    private void SetParent()
    {
      if (this._Temporary)
      {
        if (this._Parent is Message)
        {
          Message oMessage = this._Parent as Message;
          if (oMessage.SetContent(this))
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
        if (_ElementDictonary.ContainsKey(Index))
        {
          _ElementDictonary[Index]._Temporary = true;
          _ElementDictonary[Index]._Index = null;
          _ElementDictonary[Index]._Parent = null;
        }
        _ElementDictonary.Remove(Index);
      }
      catch
      {
        throw new ApplicationException(String .Format("Segment's Element Dictonary did not contain element Index {0} for removal call from Element Instance",Index));
      }      
    }

    //Parsing and Validation
    private Dictionary<int, Element> ParseSegmentRawStringToElement(String StringRaw)
    {
      //Example:  "PID|||First1^Second1^Third1^~First2^Second2&\\H\\Second22\\N\\^Third2|||";
      _Code = StringRaw.Substring(0, 3);
      if (_Code == Support.Standard.Segments.Msh.Code)
      {
        _IsMSH = true;
        return ParseMSHSegmentStringToElement(StringRaw.Substring(3, StringRaw.Length - 3));
      }
      else
      {
        _IsMSH = false;
        return ParseNormalSegmentStringToElement(StringRaw.Substring(4, StringRaw.Length - 4));
      }
    }
    private Dictionary<int, Model.Element> ParseNormalSegmentStringToElement(string StringRaw)
    {
      _ElementDictonary = new Dictionary<int, Element>();
      if (StringRaw.Contains(this.Delimiters.Field))
      {
        string[] ElementParts = StringRaw.Split(this.Delimiters.Field);
        int ElementPositionCounter = 1;
        foreach (string Part in ElementParts)
        {
          if (Part != string.Empty)
          {
            //_ElementDictonary.Add(ElementPositionCounter, new Element(Part, this.Delimiters, false, ElementPositionCounter, this));
            new Element(Part, this.Delimiters, true, ElementPositionCounter, this);
          }
          ElementPositionCounter++;
        }
      }
      else
      {
        //_ElementDictonary.Add(1, new Element(StringRaw, this.Delimiters, false, 1, this));
        new Element(StringRaw, this.Delimiters, true, 1, this);
      }
      return _ElementDictonary;
    }
    private Dictionary<int, Model.Element> ParseMSHSegmentStringToElement(string StringRaw)
    {
      //example: |^~\&|HNAM RADNET|PAH^00011|IMPAX-CV|QH|20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1|||AL|NE|AU|8859/1|EN     
      _ElementDictonary = new Dictionary<int, Element>();
      if (StringRaw.Contains(this.Delimiters.Field))
      {
        string[] ElementParts = StringRaw.Split(this.Delimiters.Field);
        if (ElementParts.Length > 11)
        {
          int ElementPositionCounter = 1;
          foreach (string Part in ElementParts)
          {
            if (Part != string.Empty && ElementPositionCounter > 2)
            {
              _ElementDictonary.Add(ElementPositionCounter, new Element(Part, this.Delimiters, false, ElementPositionCounter, this));
            }
            else if (ElementPositionCounter == 1)
            {
              _ElementDictonary.Add(ElementPositionCounter, new Element( ModelSupport.ContentTypeInternal.MainSeparator, this.Delimiters, false, ElementPositionCounter, this));
            }
            else if (ElementPositionCounter == 2)
            {
              _ElementDictonary.Add(ElementPositionCounter, new Element( ModelSupport.ContentTypeInternal.EncodingCharacters, this.Delimiters, false, ElementPositionCounter, this));
            }
            ElementPositionCounter++;
          }
        }
        else
        {
          throw new ArgumentException("MSH Segment being parsed has less than 12 Fields, MSH Segments must have a minimum of 12 Fields to include HL7 Version Field");
        }
      }
      else
      {
        throw new ArgumentException("MSH Segment being parsed has no Fields, MSH Segments must have a minimum of 12 Fields to include HL7 Version Field");
      }
      return _ElementDictonary;
    }
    private string ValidateStringRaw(string StringRaw)
    {
      StringRaw = ValidateSegmentCode(StringRaw);
      Char[] CharatersAllowed = { this.Delimiters.Field };

      if (StringRaw.IndexOfAny(CharatersAllowed) < 0)
      {
        throw new System.ArgumentException(String.Format("Segments must begin with a three character code followed by a HL7 Field delimiter. Segments must end with only a carriage return (Hex 13).", this.Delimiters.Field));
      }      
      else if (StringRaw.TrimStart().Substring(3, 1) != this.Delimiters.Field.ToString())
      {
        throw new System.ArgumentException(String.Format("Segments must begin with a three character code followed by a HL7 Field delimiter. Segments must end with only a carriage return (Hex 13).", this.Delimiters.Field));
      }      
      else if (!System.Text.RegularExpressions.Regex.IsMatch(StringRaw.TrimStart().Substring(0, 3), @"^[A-Z0-9]+$"))
      {
        throw new System.ArgumentException("Segment data must begin with a three character uppercase alpha code");
      }
      return StringRaw;
    }
    private string ValidateSegmentCode(string StringRaw)
    {
      Char[] CharatersAllowed = { this.Delimiters.Field };
      if (StringRaw.IndexOfAny(CharatersAllowed) < 0)
      {
        if (StringRaw.Length == 3)
        {
          return StringRaw += this.Delimiters.Field;
        }
        else if (StringRaw.Length == 4 && StringRaw.Substring(3, 1).ToCharArray()[0] == this.Delimiters.Field)
        {
          return StringRaw;
        }
        else
        {
          throw new System.ArgumentException(String.Format("Segments must begin with a three character code followed by a HL7 Field delimiter. Segments must end with only a carriage return (Hex 13).", this.Delimiters.Field));
        }
      }
      else
      {
        if (StringRaw.IndexOf(this.Delimiters.Field) != 3)
        {
          throw new System.ArgumentException(String.Format("Segments must begin with a three character code followed by a HL7 Field delimiter. Segments must end with only a carriage return (Hex 13).", this.Delimiters.Field));
        }
        else
        {
          return StringRaw;
        }
      }

    }

  }
}

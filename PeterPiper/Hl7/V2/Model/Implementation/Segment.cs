using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation;

internal class Segment : ModelBase, ISegment
{
    private Dictionary<int, Element> _ElementDictonary;

    private readonly string[] _HeaderSegmentCodes = new[]
        { Support.Standard.Segments.Msh.Code, Support.Standard.Segments.Bhs.Code, Support.Standard.Segments.Fhs.Code };

    //Creator Factory used Constructors
    internal Segment(string stringRaw)
    {
        _Temporary = true;
        _Index = null;
        _Parent = null;
        stringRaw = ValidateStringRaw(stringRaw);
        _ElementDictonary = ParseSegmentRawStringToElement(stringRaw);
    }

    internal Segment(string stringRaw, IMessageDelimiters customDelimiters)
        : base(customDelimiters)
    {
        _Temporary = true;
        _Index = null;
        _Parent = null;
        stringRaw = ValidateStringRaw(stringRaw);
        _ElementDictonary = ParseSegmentRawStringToElement(stringRaw);
    }

    //Only internal Constructors
    internal Segment(string stringRaw, MessageDelimiters customDelimiters, bool temporary, int? index, ModelBase parent)
        : base(customDelimiters)
    {
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;
        stringRaw = ValidateStringRaw(stringRaw);
        _ElementDictonary = ParseSegmentRawStringToElement(stringRaw);
    }

    //Instance access
    public IMessageDelimiters MessageDelimiters => Delimiters;

    public ISegment Clone()
    {
        return new Segment(AsStringRaw, Delimiters, true, null, null);
    }

    public override string ToString()
    {
        return AsString;
    }

    public override string AsString
    {
        get => GetAsStringOrAsRawString(false);
        set => AsStringRaw = Support.Standard.Escapes.Encode(value, Delimiters);
    }

    public override string AsStringRaw
    {
        get => GetAsStringOrAsRawString(true);
        set
        {
            if (IsHeaderSegment() || _Index == 1)
                throw new PeterPiperException(
                    $"Unable to modify an existing {_Code} segment instance with the AsString " +
                    $"or AsStringRaw properties. /n You need to create a new Segment instance and use " +
                    $"it's constructor or selectively edit this segment's parts.");
            value = ValidateStringRaw(value);
            _ElementDictonary = ParseSegmentRawStringToElement(value);
        }
    }

    public bool IsEmpty => (_ElementDictonary.Count == 0);

    public void ClearAll()
    {
        if (IsHeaderSegment())
        {
            Dictionary<int, Element> oNewDic = new Dictionary<int, Element>();
            oNewDic.Add(1,
                new Element(ModelSupport.ContentTypeInternal.MainSeparator, _ElementDictonary[1].Delimiters, false, 1,
                    this));
            oNewDic.Add(2,
                new Element(ModelSupport.ContentTypeInternal.EncodingCharacters, _ElementDictonary[2].Delimiters, false,
                    2, this));
            _ElementDictonary = oNewDic;
            return;
        }

        _ElementDictonary.Clear();
    }

    private string _Code;

    public string Code => _Code;

    private bool _IsMSH = false;

    internal bool IsMSH => _IsMSH;

    public void Add(IElement item)
    {
        ValidateItemNotInUse(item as Element);
        ElementAppend(item as Element);
    }

    public void Add(IField item)
    {
        ValidateItemNotInUse(item as Field);
        FieldAppend(item as Field);
    }

    public void Insert(int index, IElement item)
    {
        ThrowIfIndexIsZero(index);
        ValidateItemNotInUse(item as Element);
        ElementInsertBefore(item as Element, index);
    }

    public void Insert(int index, IField item)
    {
        ThrowIfIndexIsZero(index);
        ValidateItemNotInUse(item as Field);
        FieldInsertBefore(item as Field, index);
    }

    public void RemoveElementAt(int index)
    {
        ThrowIfIndexIsZero(index);
        ElementRemoveAt(index);
    }

    public void RemoveFieldAt(int index)
    {
        ThrowIfIndexIsZero(index);
        FieldRemoveAt(index);
    }

    public int ElementCount => CountElement;

    public bool HasElements => CountElement > 1;

    public int FieldCount => CountField;

    public bool HasFields => CountField > 1;

    public IElement Element(int index)
    {
        ThrowIfIndexIsZero(index);
        return GetElement(index);
    }

    public IField Field(int index)
    {
        ThrowIfIndexIsZero(index);
        return GetField(index);
    }

    public ReadOnlyCollection<IElement> ElementList
    {
        get
        {
            List<IElement> oNewList = new List<IElement>();
            int Counter = 1;
            foreach (var item in _ElementDictonary.OrderBy(x => x.Key))
            {
                if (item.Key != Counter)
                {
                    while (Counter != item.Key)
                    {
                        oNewList.Add(new Element(string.Empty, Delimiters, true, Counter, this));
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
        if (_ElementDictonary.TryGetValue(index, out Element value))
        {
            value.IsAccessibleElement();
            return value;
        }

        return new Element(string.Empty, Delimiters, true, index, this);
    }

    internal int CountElement
    {
        get
        {
            if (_ElementDictonary.Count > 0)
            {
                return _ElementDictonary.Keys.Max();
            }

            return 0;
        }
    }

    internal Element ElementAppend(Element element)
    {
        if (_ElementDictonary.Count > 0)
        {
            return ElementInsertBefore(element, _ElementDictonary.Keys.Max() + 1);
        }

        element._Index = 1;
        element._Parent = this;
        if (SetToDictonary(element))
            element._Temporary = false;
        return _ElementDictonary[1];
    }

    internal Element ElementInsertBefore(Element element, int index)
    {
        int ElementInsertedAt = 0;

        if (IsHeaderSegment())
        {
            if (index == 1 || index == 2)
            {
                if (_ElementDictonary.ContainsKey(index))
                {
                    _ElementDictonary[index].IsAccessibleElement();
                }
            }
        }

        //Empty Dic so just add as first item 
        if (_ElementDictonary.Count == 0)
        {
            ElementInsertedAt = index;
            element._Index = ElementInsertedAt;
            element._Parent = this;
            if (SetToDictonary(element))
            {
                element._Temporary = false;
            }

            return _ElementDictonary[ElementInsertedAt];
        }

        //Asked to insert before an index larger than the largest in Dic so just add to the end
        if (_ElementDictonary.Keys.Max() < index)
        {
            ElementInsertedAt = index;
            element._Index = ElementInsertedAt;
            element._Parent = this;
            if (SetToDictonary(element))
            {
                element._Temporary = false;
            }

            return _ElementDictonary[ElementInsertedAt];
        }
        //Asked to insert within items already in the Dic so cycle through moving each item higher or equal up by one then just add the new item
        //The Content Dictionary is different than all the others as it is to never have gaps between items and it is Zero based.

        foreach (var item in _ElementDictonary.Reverse())
        {
            if (item.Key >= index)
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

        element._Index = index;
        element._Parent = this;
        SetParent();
        element._Temporary = false;
        _ElementDictonary[index] = element;
        ElementInsertedAt = index;

        return _ElementDictonary[ElementInsertedAt];
    }

    internal bool ElementRemoveAt(int index)
    {
        if (_ElementDictonary.Keys.Max() >= index)
        {
            if (IsHeaderSegment())
            {
                if (_ElementDictonary.ContainsKey(index))
                    _ElementDictonary[index].IsAccessibleElement();
            }

            Dictionary<int, Element> oNewDic = new Dictionary<int, Element>();
            foreach (var item in _ElementDictonary)
            {
                if (item.Key < index)
                {
                    oNewDic.Add(item.Key, item.Value);
                }
                else if (item.Key > index)
                {
                    item.Value._Index--;
                    oNewDic.Add(item.Key - 1, item.Value);
                }
            }

            _ElementDictonary = oNewDic;
            return true;
        }

        return false;
    }

    //Field    

    internal Field GetField(int index)
    {
        if (_ElementDictonary.TryGetValue(index, out Element value))
        {
            return value.GetRepeat(1);
        }

        Element oElement = new Element(String.Empty, Delimiters, true, index, this);
        return oElement.RepeatAppend(new Field(string.Empty, Delimiters, true, 1, oElement));
    }

    internal int CountField => CountElement;

    internal Field FieldAppend(Field field)
    {
        if (_ElementDictonary.Count > 0)
        {
            Element oElement = new Element(string.Empty, Delimiters, true, _ElementDictonary.Keys.Max() + 1, this);
            return oElement.RepeatAppend(field);
        }
        else
        {
            Element oElement = new Element(string.Empty, Delimiters, true, 1, this);
            return oElement.RepeatAppend(field);
        }
    }

    internal Field FieldInsertBefore(Field field, int index)
    {
        Element oElement = new Element(String.Empty, Delimiters, true, index, this);
        return ElementInsertBefore(oElement, index).RepeatAppend(field);
    }

    internal bool FieldRemoveAt(int index)
    {
        return ElementRemoveAt(index);
    }

    //Building

    private string GetAsStringOrAsRawString(bool rawString)
    {
        string SegmentPrefix;

        if (IsHeaderSegment())
        {
            SegmentPrefix = _Code;
        }
        else
        {
            SegmentPrefix = $"{_Code}{Delimiters.Field}";
        }

        if (IsHeaderSegment() && _ElementDictonary.Count == 2)
        {
            if (_IsMSH)
            {
                return $"{SegmentPrefix}{Delimiters.Field}{_ElementDictonary[2].AsStringRaw}{Delimiters.Field}";
            }

            return $"{SegmentPrefix}{Delimiters.Field}{_ElementDictonary[2].AsStringRaw}";
        }

        if (_ElementDictonary.Count == 0)
        {
            return SegmentPrefix;
        }

        StringBuilder oStringBuilder = new StringBuilder(SegmentPrefix);
        _ElementDictonary.OrderByDescending(i => i.Key);
        for (int i = 1; i < _ElementDictonary.Keys.Max() + 1; i++)
        {
            if (_ElementDictonary.ContainsKey(i))
            {
                if (rawString)
                    oStringBuilder.Append(_ElementDictonary[i].AsStringRaw);
                else
                    oStringBuilder.Append(_ElementDictonary[i].AsString);
            }

            if (i != _ElementDictonary.Keys.Max())
            {
                if (IsHeaderSegment())
                {
                    if (i != 1)
                        oStringBuilder.Append(Delimiters.Field);
                }
                else
                {
                    oStringBuilder.Append(Delimiters.Field);
                }
            }
        }

        return oStringBuilder.ToString();
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
                _ElementDictonary[Convert.ToInt32(oElement._Index)]._Index = null;
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
            throw new PeterPiperException("Error setting Element into Segment parent", Exec);
        }
    }

    private void SetParent()
    {
        if (_Temporary)
        {
            if (_Parent is Message oMessage)
            {
                if (oMessage.SetContent(this))
                {
                    _Temporary = false;
                }
            }
        }
    }

    internal void RemoveChild(int index)
    {
        try
        {
            if (_ElementDictonary.ContainsKey(index))
            {
                _ElementDictonary[index]._Temporary = true;
                _ElementDictonary[index]._Index = null;
                _ElementDictonary[index]._Parent = null;
            }

            _ElementDictonary.Remove(index);
        }
        catch
        {
            throw new PeterPiperException(
                $"Segment's Element Dictonary did not contain element Index {index} for removal call from Element Instance");
        }
    }

    //Parsing and Validation

    private Dictionary<int, Element> ParseSegmentRawStringToElement(String stringRaw)
    {
        //Example:  "PID|||First1^Second1^Third1^~First2^Second2&\\H\\Second22\\N\\^Third2|||";
        _Code = stringRaw.Substring(0, 3);
        if (IsHeaderSegment())
        {
            _IsMSH = (_Code == Support.Standard.Segments.Msh.Code);
            return ParseHeaderSegmentStringToElement(stringRaw.Substring(3, stringRaw.Length - 3), _Code);
        }

        _IsMSH = false;
        return ParseNormalSegmentStringToElement(stringRaw.Substring(4, stringRaw.Length - 4));
    }

    private Dictionary<int, Element> ParseNormalSegmentStringToElement(string stringRaw)
    {
        _ElementDictonary = new Dictionary<int, Element>();
        if (stringRaw.Contains(Delimiters.Field))
        {
            string[] ElementParts = stringRaw.Split(Delimiters.Field);
            int ElementPositionCounter = 1;
            foreach (string Part in ElementParts)
            {
                if (Part != string.Empty)
                {
                    //_ElementDictonary.Add(ElementPositionCounter, new Element(Part, this.Delimiters, false, ElementPositionCounter, this));
                    new Element(Part, Delimiters, true, ElementPositionCounter, this);
                }

                ElementPositionCounter++;
            }
        }
        else
        {
            //_ElementDictonary.Add(1, new Element(StringRaw, this.Delimiters, false, 1, this));
            new Element(stringRaw, Delimiters, true, 1, this);
        }

        return _ElementDictonary;
    }

    private Dictionary<int, Element> ParseHeaderSegmentStringToElement(string stringRaw, string code)
    {
        //example: |^~\&|HNAM RADNET|PAH^00011|IMPAX-CV|QH|20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1|||AL|NE|AU|8859/1|EN     
        _ElementDictonary = new Dictionary<int, Element>();

        if (!stringRaw.Contains(Delimiters.Field))
        {
            if (code == Support.Standard.Segments.Msh.Code)
            {
                throw new PeterPiperException(
                    "MSH Segment being parsed has no Fields, MSH Segments must have a minimum of 12 Fields to include HL7 Version Field");
            }

            //This will be BHS & FHS Segment s for HL7 Batches
            throw new PeterPiperException(
                $"{code} Segment being parsed has no Fields, {code} Segments must have a minimum of 2 Fields, {code}-2 being the Encoding Characters");
        }

        string[] ElementParts = stringRaw.Split(Delimiters.Field);
        if (code == Support.Standard.Segments.Msh.Code && ElementParts.Length < 12)
        {
            throw new PeterPiperException(
                $"MSH Segment being parsed has less than 12 Fields, MSH Segments must have a minimum of 12 Fields to include HL7 Version Field in MSH-12");
        }

        int ElementPositionCounter = 1;
        foreach (string Part in ElementParts)
        {
            if (Part != string.Empty && ElementPositionCounter > 2)
            {
                _ElementDictonary.Add(ElementPositionCounter,
                    new Element(Part, Delimiters, false, ElementPositionCounter, this));
            }
            else if (ElementPositionCounter == 1)
            {
                _ElementDictonary.Add(ElementPositionCounter,
                    new Element(ModelSupport.ContentTypeInternal.MainSeparator, Delimiters, false,
                        ElementPositionCounter, this));
            }
            else if (ElementPositionCounter == 2)
            {
                _ElementDictonary.Add(ElementPositionCounter,
                    new Element(ModelSupport.ContentTypeInternal.EncodingCharacters, Delimiters, false,
                        ElementPositionCounter, this));
            }

            ElementPositionCounter++;
        }

        return _ElementDictonary;
    }

    private string ValidateStringRaw(string stringRaw)
    {
        if (stringRaw.Length == 3)
        {
            stringRaw += Delimiters.Field;
        }

        stringRaw = ValidateSegmentCode(stringRaw);
        Char[] CharactersAllowed = { Delimiters.Field };

        if (stringRaw.IndexOfAny(CharactersAllowed) < 0)
        {
            throw new PeterPiperException(
                "Segments must begin with a three character code followed by a HL7 Field delimiter. Segments must end with only a carriage return (Hex 13).");
        }

        if (stringRaw.TrimStart().Substring(3, 1) != Delimiters.Field.ToString())
        {
            throw new PeterPiperException(
                "Segments must begin with a three character code followed by a HL7 Field delimiter. Segments must end with only a carriage return (Hex 13).");
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(stringRaw.TrimStart().Substring(0, 3), @"^[A-Z0-9]+$"))
        {
            throw new PeterPiperException("Segment data must begin with a three character upper-case alpha code");
        }

        return stringRaw;
    }

    private string ValidateSegmentCode(string stringRaw)
    {
        Char[] CharatersAllowed = { Delimiters.Field };
        if (stringRaw.IndexOfAny(CharatersAllowed) < 0)
        {
            if (stringRaw.Length == 4 && stringRaw.Substring(3, 1).ToCharArray()[0] == Delimiters.Field)
            {
                return stringRaw;
            }

            throw new PeterPiperException("Segments must begin with a three character code followed by a HL7 " +
                                          "Field delimiter. Segments must end with only a carriage return (Hex 13).");
        }

        if (stringRaw.IndexOf(Delimiters.Field) != 3)
        {
            throw new PeterPiperException("Segments must begin with a three character code followed by a " +
                                          "HL7 Field delimiter. Segments must end with only a carriage return (Hex 13).");
        }

        return stringRaw;
    }

    private bool IsHeaderSegment()
    {
        return _HeaderSegmentCodes.Contains(_Code);
    }

    private static void ThrowIfIndexIsZero(int index)
    {
        if (index == 0)
        {
            throw new PeterPiperException("Element index is a one based index, zero in not allowed");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation;

internal class Element : ContentBase, IElement
{
    internal bool IsMainSeparator = false;
    internal bool IsEncodingCharacters = false;

    private Dictionary<int, Field> _RepeatDictonary;

    //Creator Factory used Constructors
    internal Element()
    {
        _RepeatDictonary = new Dictionary<int, Field>();
        _Temporary = true;
        _Index = null;
        _Parent = null;
    }

    internal Element(IMessageDelimiters customDelimiters)
        : base(customDelimiters)
    {
        _RepeatDictonary = new Dictionary<int, Field>();
        _Temporary = true;
        _Index = null;
        _Parent = null;
    }

    internal Element(string stringRaw)
    {
        _Temporary = true;
        _Index = null;
        _Parent = null;

        if (ValidateStringRaw(stringRaw))
        {
            _RepeatDictonary = ParseElementRawStringToRepeat(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    internal Element(string stringRaw, IMessageDelimiters customDelimiters)
        : base(customDelimiters)
    {
        _Temporary = true;
        _Index = null;
        _Parent = null;

        if (ValidateStringRaw(stringRaw))
        {
            _RepeatDictonary = ParseElementRawStringToRepeat(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    //Only internal Constructors
    internal Element(Field field, MessageDelimiters customDelimiters, bool temporary, int? index, ModelBase parent)
        : base(customDelimiters)
    {
        ValidateItemNotInUse(field);
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;
        _RepeatDictonary = new Dictionary<int, Field>();
        field._Parent = this;
        _RepeatDictonary.Add(1, field);
    }

    internal Element(string stringRaw, MessageDelimiters customDelimiters, bool temporary, int? index, ModelBase parent)
        : base(customDelimiters)
    {
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;

        if (ValidateStringRaw(stringRaw))
        {
            _RepeatDictonary = ParseElementRawStringToRepeat(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    internal Element(ModelSupport.ContentTypeInternal contentTypeInternal, MessageDelimiters customDelimiters,
        bool temporary, int? index, ModelBase parent)
        : base(customDelimiters)
    {
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;
        IsMainSeparator = (contentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator);
        IsEncodingCharacters = (contentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters);

        _RepeatDictonary = ParseElementRawStringToRepeat(string.Empty, contentTypeInternal);
    }

    //Instance access
    public IMessageDelimiters MessageDelimiters => Delimiters;

    public IElement Clone()
    {
        return new Element(AsStringRaw, Delimiters, true, null, null);
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

    public bool IsEmpty => (_RepeatDictonary.Count == 0);

    public bool IsHL7Null => GetRepeat(1).IsHL7Null;

    public void ClearAll()
    {
        _RepeatDictonary.Clear();
        RemoveFromParent();
    }

    public void Set(int index, IContent item)
    {
        ContentSet(item as Content, index);
    }

    public void Set(int index, ISubComponent item)
    {
        SubComponentSet(item as SubComponent, index);
    }

    public void Add(IField item)
    {
        ValidateItemNotInUse(item as Field);
        RepeatAppend(item as Field);
    }

    public void Add(IComponent item)
    {
        var Component = item as Component;
        ValidateItemNotInUse(Component);
        ComponentAppend(Component);
    }

    public void Add(ISubComponent item)
    {
        ValidateItemNotInUse(item as SubComponent);
        SubComponentAppend(item as SubComponent);
    }

    public void Add(IContent item)
    {
        ValidateItemNotInUse(item as Content);
        ContentAppend(item as Content);
    }

    public void Insert(int index, IField item)
    {
        ValidateItemNotInUse(item as Field);
        RepeatInsertBefore(item as Field, index);
    }

    public void Insert(int index, IComponent item)
    {
        var Component = item as Component;
        ValidateItemNotInUse(Component);
        ComponentInsertBefore(Component, index);
    }

    public void Insert(int index, ISubComponent item)
    {
        ValidateItemNotInUse(item as SubComponent);
        SubComponentInsertBefore(item as SubComponent, index);
    }

    public void Insert(int index, IContent item)
    {
        ValidateItemNotInUse(item as Content);
        ContentInsertBefore(item as Content, index);
    }

    public void RemoveRepeatAt(int index)
    {
        RepeatRemoveAt(index);
    }

    public void RemoveComponentAt(int index)
    {
        ComponentRemoveAt(index);
    }

    public void RemoveSubComponentAt(int index)
    {
        SubComponentRemoveAt(index);
    }

    public void RemoveContentAt(int index)
    {
        ContentRemoveAt(index);
    }

    public int RepeatCount => CountRepeat;

    public bool HasRepeats => CountRepeat > 1;

    public int ComponentCount => CountComponent;

    public bool HasComponents => CountComponent > 1;

    public int SubComponentCount => CountSubComponet;

    public bool HasSubComponents => CountSubComponet > 1;

    public int ContentCount => CountContent;

    public bool HasContents => CountContent > 1;

    public IField Repeat(int index)
    {
        if (index == 0)
            throw new PeterPiperException("Repeat is a one based index, zero is not a valid index");
        return GetRepeat(index);
    }

    public IComponent Component(int index)
    {
        if (index == 0)
            throw new PeterPiperException("Component is a one based index, zero is not a valid index");
        return GetComponent(index);
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

    public ReadOnlyCollection<IField> RepeatList
    {
        get
        {
            List<IField> oNewList = new();
            int Counter = 1;
            foreach (var item in _RepeatDictonary.OrderBy(x => x.Key))
            {
                if (item.Key != Counter)
                {
                    while (Counter != item.Key)
                    {
                        oNewList.Add(new Field(string.Empty, Delimiters, true, Counter, this));
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
        if (_RepeatDictonary.TryGetValue(index, out var repeat))
        {
            return repeat;
        }

        return new Field(string.Empty, Delimiters, true, index, this);
    }

    internal int CountRepeat
    {
        get
        {
            if (_RepeatDictonary.Count > 0)
            {
                return _RepeatDictonary.Keys.Max();
            }

            return 0;
        }
    }

    internal Field RepeatAppend(Field repeat)
    {
        if (_RepeatDictonary.Count > 0)
        {
            return RepeatInsertBefore(repeat, _RepeatDictonary.Keys.Max() + 1);
        }

        repeat._Index = 1;
        repeat._Parent = this;
        if (SetToDictonary(repeat))
        {
            repeat._Temporary = false;
        }

        return _RepeatDictonary[1];
    }

    internal Field RepeatInsertBefore(Field repeat, int index)
    {
        if (index == 0)
        {
            throw new PeterPiperException("Element is a one based index, zero is not a valid index.");
        }

        int RepeatInsertedAt;
        //Empty Dic so just add as first item 
        if (_RepeatDictonary.Count == 0)
        {
            RepeatInsertedAt = index;
            repeat._Index = RepeatInsertedAt;
            repeat._Parent = this;
            if (SetToDictonary(repeat))
                repeat._Temporary = false;
        }
        //Asked to insert before an index larger than the largest in Dic so just add to the end
        else if (_RepeatDictonary.Keys.Max() < index)
        {
            RepeatInsertedAt = index;
            repeat._Index = RepeatInsertedAt;
            repeat._Parent = this;
            if (SetToDictonary(repeat))
                repeat._Temporary = false;
            //_ContentDictonary.Add(_ContentDictonary.Keys.Max() + 1, Content);
        }
        //Asked to insert within items already in the Dic so cycle through moving each item higher or equal up by one then just add the new item
        //The Content Dictonary is different than all the others as it is to never have gaps between items and it is Zero based.
        else
        {
            foreach (var item in _RepeatDictonary.Reverse())
            {
                if (item.Key >= index)
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

            repeat._Index = index;
            repeat._Parent = this;
            SetParent();
            repeat._Temporary = false;
            _RepeatDictonary[index] = repeat;
            RepeatInsertedAt = index;
        }

        return _RepeatDictonary[RepeatInsertedAt];
    }

    internal bool RepeatRemoveAt(int index)
    {
        if (!_RepeatDictonary.ContainsKey(index))
        {
            return false;
        }

        Dictionary<int, Field> oNewDic = new Dictionary<int, Field>();
        foreach (var item in _RepeatDictonary)
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

        _RepeatDictonary = oNewDic;
        if (_RepeatDictonary.Count == 0)
        {
            RemoveFromParent();
        }

        return true;
    }

    //Component    
    internal Component GetComponent(int index)
    {
        if (_RepeatDictonary.TryGetValue(1, out Field value))
        {
            return value.GetComponent(index);
        }

        Field oField = new Field(string.Empty, Delimiters, true, 1, this);
        return oField.GetComponent(index);
    }

    internal int CountComponent
    {
        get
        {
            if (_RepeatDictonary.TryGetValue(1, out var value))
            {
                return value.CountComponent;
            }

            return 0;
        }
    }

    internal Component ComponentAppend(Component component)
    {
        if (_RepeatDictonary.TryGetValue(1, out Field value))
        {
            return value.ComponentAppend(component);
        }

        Field oField = new Field(string.Empty, Delimiters, true, 1, this);
        return oField.ComponentAppend(component);
    }

    internal Component ComponentInsertBefore(Component component, int index)
    {
        if (index == 0)
        {
            throw new PeterPiperException("Element is a one based index, zero is not a valid index.");
        }

        if (_RepeatDictonary.TryGetValue(1, out Field value))
        {
            return value.ComponentInsertBefore(component, index);
        }

        Field oField = new Field(string.Empty, Delimiters, true, index, this);
        return oField.ComponentInsertBefore(component, index);
    }

    internal bool ComponentRemoveAt(int index)
    {
        if (_RepeatDictonary.ContainsKey(index))
        {
            return _RepeatDictonary[1].ComponentRemoveAt(index);
        }

        return false;
    }

    //SubComponent    
    internal SubComponent GetSubComponent(int index)
    {
        if (_RepeatDictonary.ContainsKey(1))
        {
            return _RepeatDictonary[1].GetSubComponent(index);
        }

        Field oField = new Field(string.Empty, Delimiters, true, 1, this);
        return oField.GetSubComponent(index);
    }

    internal int CountSubComponet
    {
        get
        {
            if (_RepeatDictonary.TryGetValue(1, out Field value))
            {
                return value.CountSubComponent;
            }

            return 0;
        }
    }

    internal SubComponent SubComponentSet(SubComponent subComponent, int index)
    {
        if (_RepeatDictonary.TryGetValue(1, out Field value))
        {
            return value.SubComponentSet(subComponent, index);
        }

        Field oField = new Field(string.Empty, Delimiters, true, 1, this);
        return oField.SubComponentSet(subComponent, index);
    }

    internal SubComponent SubComponentAppend(SubComponent subComponent)
    {
        if (_RepeatDictonary.TryGetValue(1, out Field value))
        {
            return value.SubComponentAppend(subComponent);
        }

        Field oField = new Field(string.Empty, Delimiters, true, 1, this);
        return oField.SubComponentAppend(subComponent);
    }

    internal SubComponent SubComponentInsertBefore(SubComponent subComponent, int index)
    {
        if (index == 0)
        {
            throw new PeterPiperException("Element is a one based index, zero is not a valid index.");
        }

        if (_RepeatDictonary.ContainsKey(1))
        {
            return _RepeatDictonary[1].SubComponentInsertBefore(subComponent, index);
        }


        Field oField = new Field(string.Empty, Delimiters, true, 1, this);
        return oField.SubComponentInsertBefore(subComponent, index);
    }

    internal bool SubComponentRemoveAt(int index)
    {
        if (_RepeatDictonary.ContainsKey(index))
        {
            return _RepeatDictonary[1].SubComponentRemoveAt(index);
        }

        return false;
    }

    //Content        
    internal Content GetContent(int index)
    {
        if (_RepeatDictonary.TryGetValue(1, out Field value))
        {
            return value.GetContent(index);
        }

        Field oField = new Field(string.Empty, Delimiters, true, 1, this);
        return oField.GetContent(index);
    }

    internal int CountContent
    {
        get
        {
            if (_RepeatDictonary.TryGetValue(1, out Field value))
            {
                return value.CountContent;
            }

            return 0;
        }
    }

    internal Content ContentSet(Content content, int index)
    {
        if (_RepeatDictonary.TryGetValue(1, out Field value))
        {
            return value.ContentSet(content, index);
        }

        Field oField = new Field(string.Empty, Delimiters, true, 1, this);
        return oField.ContentSet(content, index);
    }

    internal Content ContentAppend(Content content)
    {
        if (_RepeatDictonary.TryGetValue(1, out Field value))
        {
            return value.ContentAppend(content);
        }

        Field oField = new Field(string.Empty, Delimiters, true, 1, this);
        return oField.ContentAppend(content);
    }

    internal Content ContentInsertBefore(Content content, int index)
    {
        if (_RepeatDictonary.TryGetValue(1, out Field value))
        {
            return value.ContentInsertBefore(content, index);
        }

        Field oField = new Field(string.Empty, Delimiters, true, 1, this);
        return oField.GetContent(0);
    }

    internal bool ContentRemoveAt(int index)
    {
        if (_RepeatDictonary.ContainsKey(index))
        {
            return _RepeatDictonary[1].ContentRemoveAt(index);
        }

        return false;
    }

    //Building
    private string GetAsStringOrAsRawString(bool rawString)
    {
        if (_RepeatDictonary.Count == 0)
            return string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();
        _RepeatDictonary.OrderByDescending(i => i.Key);
        for (int i = 1; i < _RepeatDictonary.Keys.Max() + 1; i++)
        {
            if (_RepeatDictonary.ContainsKey(i))
            {
                if (rawString)
                    oStringBuilder.Append(_RepeatDictonary[i].AsStringRaw);
                else
                    oStringBuilder.Append(_RepeatDictonary[i].AsString);
            }

            if (i != _RepeatDictonary.Keys.Max())
                oStringBuilder.Append(Delimiters.Repeat);
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
        if (_Temporary)
        {
            if (_Parent is Segment oSegment)
            {
                if (oSegment.SetToDictonary(this))
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
            if (_RepeatDictonary.ContainsKey(index))
            {
                _RepeatDictonary[index]._Temporary = true;
                _RepeatDictonary[index]._Index = null;
                _RepeatDictonary[index]._Parent = null;
            }

            _RepeatDictonary.Remove(index);
            if (_RepeatDictonary.Count == 0)
            {
                RemoveFromParent();
            }
        }
        catch
        {
            throw new PeterPiperException(
                $"Element's Repeat dictionary did not contain repeat Index {index} for removal");
        }
    }

    private void RemoveFromParent()
    {
        if (_Index != null)
        {
            if (_Parent is Segment oSegment)
            {
                oSegment.RemoveChild(System.Convert.ToInt32(_Index));
                return;
            }

            throw new PeterPiperException(
                "Casting of Elements parent to Segment throws Invalid Cast Exception, check inner exception for more detail");
        }
    }

    //Parsing and Validation
    private Dictionary<int, Field> ParseElementRawStringToRepeat(String stringRaw,
        ModelSupport.ContentTypeInternal contentTypeInternal)
    {
        //Example:  "First1^Second1^Third1^~First2^Second2&\\H\\Second22\\N\\^Third2";
        _RepeatDictonary = new Dictionary<int, Field>();
        if (contentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters ||
            contentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator)
        {
            _RepeatDictonary.Add(1, new Field(contentTypeInternal, Delimiters, false, 1, this));
        }
        else
        {
            if (stringRaw.Contains(Delimiters.Repeat))
            {
                string[] ElementParts = stringRaw.Split(Delimiters.Repeat);
                int RepeatPositionCounter = 1;
                foreach (string Part in ElementParts)
                {
                    if (Part != string.Empty)
                    {
                        //_RepeatDictonary.Add(RepeatPositionCounter, new Field(Part, this.Delimiters, false, RepeatPositionCounter, this));
                        new Field(Part, Delimiters, true, RepeatPositionCounter, this);
                    }

                    RepeatPositionCounter++;
                }
            }
            else
            {
                //_RepeatDictonary.Add(1, new Field(StringRaw, this.Delimiters, false, 1, this));
                new Field(stringRaw, Delimiters, true, 1, this);
            }
        }

        return _RepeatDictonary;
    }

    private bool ValidateStringRaw(string stringRaw)
    {
        Char[] CharactersNotAllowed = {Delimiters.Field};

        if (stringRaw.IndexOfAny(CharactersNotAllowed) != -1)
        {
            string ErrorChars = string.Join(" or ", CharactersNotAllowed);
            throw new PeterPiperException($"Element data cannot contain HL7 V2 Delimiters of : {ErrorChars}");
        }

        return true;
    }

    internal bool IsAccessibleElement()
    {
        if (IsMainSeparator)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "MSH-1 contains the 'Main Separator' character and is not accessible from the Field or Element object instance as it is critical to message construction.");
            sb.Append(Environment.NewLine);
            sb.Append(
                "Instead, you can access the read only property call 'MainSeparator' from the Message object instance.");
            throw new PeterPiperException(sb.ToString());
        } 
        if (IsEncodingCharacters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "MSH-2 contains the 'Encoding Characters' and is not accessible from the Field or Element object instance as it is critical to message construction.");
            sb.Append(Environment.NewLine);
            sb.Append(
                "Instead, you can access the read only property call 'EscapeSequence' from the Message object instance.");
            throw new PeterPiperException(sb.ToString());
        }

        return true;
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation;

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

    internal Component(IMessageDelimiters customDelimiters)
        : base(customDelimiters)
    {
        _SubComponentDictonary = new Dictionary<int, SubComponent>();
        _Temporary = true;
        _Index = null;
        _Parent = null;
    }

    internal Component(string stringRaw)
    {
        _Temporary = true;
        _Index = null;
        _Parent = null;

        if (ValidateStringRaw(stringRaw))
        {
            _SubComponentDictonary =
                ParseComponentRawStringToSubComponent(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    internal Component(string stringRaw, IMessageDelimiters customDelimiters)
        : base(customDelimiters)
    {
        _Temporary = true;
        _Index = null;
        _Parent = null;

        if (ValidateStringRaw(stringRaw))
        {
            _SubComponentDictonary =
                ParseComponentRawStringToSubComponent(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    //Only internal Constructors
    internal Component(SubComponent subComponent, MessageDelimiters customDelimiters, bool temporary,
        int? index, ModelBase parent)
        : base(customDelimiters)
    {
        ValidateItemNotInUse(subComponent);
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;
        _SubComponentDictonary = new Dictionary<int, SubComponent> { { 1, subComponent } };
    }

    internal Component(string stringRaw, MessageDelimiters customDelimiters, bool temporary,
        int? index, ModelBase parent)
        : base(customDelimiters)
    {
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;

        if (ValidateStringRaw(stringRaw))
        {
            _SubComponentDictonary =
                ParseComponentRawStringToSubComponent(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    internal Component(ModelSupport.ContentTypeInternal contentTypeInternal,
        MessageDelimiters customDelimiters, bool temporary, int? index, ModelBase parent)
        : base(customDelimiters)
    {
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;
        _SubComponentDictonary = ParseComponentRawStringToSubComponent(string.Empty, contentTypeInternal);
    }

    //Instance access
    public IMessageDelimiters MessageDelimiters => Delimiters;

    public IComponent Clone()
    {
        return new Component(AsStringRaw, Delimiters, true, null, null);
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
            if (value == string.Empty)
            {
                RemoveFromParent();
            }
            else if (ValidateStringRaw(value))
            {
                _SubComponentDictonary =
                    ParseComponentRawStringToSubComponent(value, ModelSupport.ContentTypeInternal.Unknown);
            }
        }
    }

    public bool IsEmpty => (_SubComponentDictonary.Count == 0);

    public bool IsHL7Null
    {
        get
        {
            if (SubComponent(1).IsHL7Null)
            {
                if (SubComponentCount == 1)
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
        ContentSet(item as Content, index);
    }

    public void Set(int index, ISubComponent item)
    {
        ValidateItemNotInUse(item as SubComponent);
        SubComponentSet(item as SubComponent, index);
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

    public void RemoveSubComponentAt(int index)
    {
        SubComponentRemoveAt(index);
    }

    public void RemoveContentAt(int index)
    {
        ContentRemoveAt(index);
    }

    public int SubComponentCount => CountSubComponent;

    public bool HasSubComponents => CountSubComponent > 1;

    public int ContentCount => CountContent;

    public bool HasContents => CountContent > 1;

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
            List<ISubComponent> oNewList = new();
            int Counter = 1;
            foreach (var item in _SubComponentDictonary.OrderBy(x => x.Key))
            {
                if (item.Key != Counter)
                {
                    while (Counter != item.Key)
                    {
                        oNewList.Add(new SubComponent(string.Empty, Delimiters, true, Counter, this));
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
        if (_SubComponentDictonary.TryGetValue(index, out var component))
        {
            return component;
        }

        return new SubComponent(string.Empty, Delimiters, true, index, this);
    }

    internal int CountSubComponent
    {
        get
        {
            if (_SubComponentDictonary.Count > 0)
            {
                return _SubComponentDictonary.Keys.Max();
            }

            return 0;
        }
    }

    internal SubComponent SubComponentSet(SubComponent subComponent, int index)
    {
        if (subComponent.AsStringRaw == string.Empty)
        {
            RemoveChild(index);
            subComponent._Index = null;
            subComponent._Parent = null;
            subComponent._Temporary = true;
            return subComponent;
        }

        subComponent._Index = index;
        subComponent._Parent = this;
        if (SetToDictonary(subComponent))
        {
            _SubComponentDictonary[index]._Temporary = false;
        }

        return _SubComponentDictonary[System.Convert.ToInt32(subComponent._Index)];
    }

    internal SubComponent SubComponentAppend(SubComponent subComponent)
    {
        if (_SubComponentDictonary.Count > 0)
        {
            return SubComponentInsertBefore(subComponent, _SubComponentDictonary.Keys.Max() + 1);
        }

        subComponent._Index = 1;
        subComponent._Parent = this;
        if (SetToDictonary(subComponent))
        {
            subComponent._Temporary = false;
        }

        return _SubComponentDictonary[1];
    }

    internal SubComponent SubComponentInsertBefore(SubComponent subComponent, int index)
    {
        if (index == 0)
            throw new PeterPiperException("SubComponent is a one based index, zero is not a valid index.");

        int SubComponentInsertedAt = 0;
        //Empty dictionary so just add as first item 
        if (_SubComponentDictonary.Count == 0)
        {
            SubComponentInsertedAt = index;
            subComponent._Index = SubComponentInsertedAt;
            subComponent._Parent = this;
            if (SetToDictonary(subComponent))
                subComponent._Temporary = false;
        }
        //Asked to insert before an index larger than the largest in dictionary so just add to the end
        else if (_SubComponentDictonary.Keys.Max() < index)
        {
            SubComponentInsertedAt = index;
            subComponent._Index = SubComponentInsertedAt;
            subComponent._Parent = this;
            if (SetToDictonary(subComponent))
                subComponent._Temporary = false;
            //_ContentDictonary.Add(_ContentDictonary.Keys.Max() + 1, Content);
        }
        //Asked to insert within items already in the Dictionary so cycle through moving each item higher or equal up by one then just add the new item
        //The Content dictionary is different than all the others as it is to never have gaps between items and it is Zero based.
        else
        {
            foreach (var item in _SubComponentDictonary.Reverse())
            {
                if (item.Key >= index)
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

            subComponent._Index = index;
            subComponent._Parent = this;
            SetParent();
            subComponent._Temporary = false;
            _SubComponentDictonary[index] = subComponent;
            SubComponentInsertedAt = index;
        }

        return _SubComponentDictonary[SubComponentInsertedAt];
    }

    internal bool SubComponentRemoveAt(int index)
    {
        if (_SubComponentDictonary.ContainsKey(index))
        {
            Dictionary<int, SubComponent> oNewDic = new Dictionary<int, SubComponent>();
            foreach (var item in _SubComponentDictonary)
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
        {
            return _SubComponentDictonary[1].GetContent(index);
        }

        SubComponent oSubComponent = new SubComponent(string.Empty, Delimiters, false, 0, this);
        return oSubComponent.GetContent(index);
    }

    internal int CountContent
    {
        get
        {
            if (_SubComponentDictonary.TryGetValue(1, out var value))
            {
                return value.CountContent;
            }

            return 0;
        }
    }

    internal Content ContentSet(Content content, int index)
    {
        if (_SubComponentDictonary.TryGetValue(1, out SubComponent value))
        {
            return value.ContentSet(index, content);
        }

        SubComponent oSubComponent = new SubComponent(string.Empty, Delimiters, true, 1, this);
        return oSubComponent.ContentSet(index, content);
    }

    internal Content ContentAppend(Content content)
    {
        if (_SubComponentDictonary.TryGetValue(1, out SubComponent value))
        {
            return value.ContentAppend(content);
        }

        SubComponent oSubComponent = new SubComponent(String.Empty, Delimiters, true, 1, this);
        return oSubComponent.ContentAppend(content);
    }

    internal Content ContentInsertBefore(Content content, int index)
    {
        if (_SubComponentDictonary.TryGetValue(1, out SubComponent value))
        {
            return value.ContentInsertBefore(content, index);
        }

        SubComponent oSubComponent = new SubComponent(String.Empty, Delimiters, true, 1, this);
        return oSubComponent.ContentInsertBefore(content, index);
    }

    internal bool ContentRemoveAt(int index)
    {
        if (_SubComponentDictonary.TryGetValue(1, out SubComponent value))
        {
            return value.ContentRemoveAt(index);
        }

        return false;
    }

    //Building
    private string GetAsStringOrAsRawString(bool rawString)
    {
        if (_SubComponentDictonary.Count == 0)
            return string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();
        _SubComponentDictonary.OrderByDescending(i => i.Key);
        for (int i = 1; i < _SubComponentDictonary.Keys.Max() + 1; i++)
        {
            if (_SubComponentDictonary.ContainsKey(i))
            {
                if (rawString)
                    oStringBuilder.Append(_SubComponentDictonary[i].AsStringRaw);
                else
                    oStringBuilder.Append(_SubComponentDictonary[i].AsString);
            }

            if (i != _SubComponentDictonary.Keys.Max())
                oStringBuilder.Append(Delimiters.SubComponent);
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
        if (_Temporary)
        {
            if (_Parent is Field oField)
            {
                if (oField.SetToDictonary(this))
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
            if (_SubComponentDictonary.ContainsKey(index))
            {
                _SubComponentDictonary[index]._Temporary = true;
                _SubComponentDictonary[index]._Index = null;
                _SubComponentDictonary[index]._Parent = null;
            }

            _SubComponentDictonary.Remove(index);
            if (_SubComponentDictonary.Count == 0)
            {
                RemoveFromParent();
            }
        }
        catch
        {
            throw new PeterPiperException(String.Format(
                "Component's SubComponent dictionary did not contain SubComponent Index {0} for removal call from SubComponent Instance",
                index));
        }
    }

    private void RemoveFromParent()
    {
        if (_Index != null)
        {
            if (_Parent is Field oField)
            {
                oField.RemoveChild(System.Convert.ToInt32(_Index));
                return;
            }

            throw new PeterPiperException(
                "Casting of Component's parent to Field throws Invalid Cast Exception, check inner exception for more detail");
        }
    }

    //Parsing and Validation
    private Dictionary<int, SubComponent> ParseComponentRawStringToSubComponent(String stringRaw,
        ModelSupport.ContentTypeInternal contentTypeInternal)
    {
        //Example:  "First&Second&Third\H\bold\N\&forth";
        _SubComponentDictonary = new Dictionary<int, SubComponent>();
        if (contentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters ||
            contentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator)
        {
            _SubComponentDictonary.Add(1, new SubComponent(contentTypeInternal, Delimiters, false, 1, this));
        }
        else
        {
            if (stringRaw.Contains(Delimiters.SubComponent))
            {
                string[] SubComponentParts = stringRaw.Split(Delimiters.SubComponent);
                int SubCompoenentPositionCounter = 1;
                foreach (string Part in SubComponentParts)
                {
                    if (Part != string.Empty)
                    {
                        //_SubComponentDictonary.Add(SubCompoenentPositionCounter, new SubComponent(Part, this.Delimiters, false, SubCompoenentPositionCounter, this));
                        new SubComponent(Part, Delimiters, true, SubCompoenentPositionCounter, this);
                    }

                    SubCompoenentPositionCounter++;
                }
            }
            else
            {
                //_SubComponentDictonary.Add(1, new SubComponent(StringRaw, this.Delimiters, this._Temporary, 1, this));
                new SubComponent(stringRaw, Delimiters, true, 1, this);
            }
        }

        return _SubComponentDictonary;
    }

    private bool ValidateStringRaw(string stringRaw)
    {
        Char[] CharactersNotAllowed =
        {
            Delimiters.Field,
            Delimiters.Component,
            Delimiters.Repeat
        };

        if (stringRaw.IndexOfAny(CharactersNotAllowed) != -1)
        {
            string ErrorChars = string.Join(" or ", CharactersNotAllowed);
            throw new PeterPiperException($"Component data cannot contain HL7 V2 Delimiters of : {ErrorChars}");
        }

        return true;
    }
}
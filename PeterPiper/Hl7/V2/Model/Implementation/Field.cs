using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation;

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

    internal Field(IMessageDelimiters customDelimiters)
        : base(customDelimiters)
    {
        _ComponentDictonary = new Dictionary<int, Component>();
        _Temporary = true;
        _Index = null;
        _Parent = null;
    }

    internal Field(string stringRaw)
    {
        _Temporary = true;
        _Index = null;
        _Parent = null;

        if (ValidateStringRaw(stringRaw))
        {
            _ComponentDictonary = ParseFieldRawStringToComponent(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    internal Field(string stringRaw, IMessageDelimiters customDelimiters)
        : base(customDelimiters)
    {
        _Temporary = true;
        _Index = null;
        _Parent = null;

        if (ValidateStringRaw(stringRaw))
        {
            _ComponentDictonary = ParseFieldRawStringToComponent(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    //Only internal Constructors
    internal Field(string stringRaw, MessageDelimiters customDelimiters, bool temporary, int? index, ContentBase parent)
        : base(customDelimiters)
    {
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;

        if (ValidateStringRaw(stringRaw))
        {
            _ComponentDictonary = ParseFieldRawStringToComponent(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    internal Field(ModelSupport.ContentTypeInternal contentTypeInternal, MessageDelimiters customDelimiters,
        bool temporary, int? index, ContentBase parent)
        : base(customDelimiters)
    {
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;
        _ComponentDictonary = ParseFieldRawStringToComponent(string.Empty, contentTypeInternal);
    }


    //Instance access
    public IMessageDelimiters MessageDelimiters => Delimiters;

    public IField Clone()
    {
        return new Field(AsStringRaw, Delimiters, false, null, null);
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
                _ComponentDictonary = ParseFieldRawStringToComponent(value, ModelSupport.ContentTypeInternal.Unknown);
            }
        }
    }

    public bool IsEmpty => (_ComponentDictonary.Count == 0);

    public bool IsHL7Null
    {
        get
        {
            if (GetComponent(1).IsHL7Null)
            {
                if (ComponentCount == 1)
                {
                    return true;
                }
            }

            return false;
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
        ContentSet(item as Content, index);
    }

    public void Set(int index, ISubComponent item)
    {
        ValidateItemNotInUse(item as SubComponent);
        SubComponentSet(item as SubComponent, index);
    }

    public void Set(int index, IComponent item)
    {
        var Component = item as Component;
        ValidateItemNotInUse(Component);
        ComponentSet(Component, index);
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

    public int ComponentCount => CountComponent;

    public bool HasComponents => CountComponent > 1;

    public int SubComponentCount => CountSubComponent;

    public bool HasSubComponents => CountSubComponent > 1;

    public int ContentCount => CountContent;

    public bool HasContents => CountContent > 1;

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
                        oNewList.Add(new Component(string.Empty, Delimiters, true, Counter, this));
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
        if (_ComponentDictonary.TryGetValue(index, out var component))
        {
            return component;
        }

        return new Component(string.Empty, Delimiters, true, index, this);
    }

    internal int CountComponent
    {
        get
        {
            if (_ComponentDictonary.Count > 0)
            {
                return _ComponentDictonary.Keys.Max();
            }

            return 0;
        }
    }

    internal Component ComponentSet(Component component, int index)
    {
        if (component.AsStringRaw == string.Empty)
        {
            RemoveChild(index);
            component._Index = null;
            component._Parent = null;
            component._Temporary = true;
            return component;
        }
        else
        {
            component._Index = index;
            component._Parent = this;
            if (SetToDictonary(component))
                component._Temporary = false;
            return _ComponentDictonary[System.Convert.ToInt32(component._Index)];
        }
    }

    internal Component ComponentAppend(Component component)
    {
        if (_ComponentDictonary.Count > 0)
        {
            return ComponentInsertBefore(component, _ComponentDictonary.Keys.Max() + 1);
        }
        else
        {
            component._Index = 1;
            component._Parent = this;
            if (SetToDictonary(component))
                component._Temporary = false;
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

    internal Component ComponentInsertBefore(Component component, int index)
    {
        if (index == 0)
            throw new PeterPiperException("Element is a one based index, zero is not a valid index.");

        int ComponentInsertedAt;
        //Empty Dic so just add as first item 
        if (_ComponentDictonary.Count == 0)
        {
            ComponentInsertedAt = index;
            component._Index = ComponentInsertedAt;
            component._Parent = this;
            if (SetToDictonary(component))
                component._Temporary = false;
        }
        //Asked to insert before an index larger than the largest in Dic so just add to the end
        else if (_ComponentDictonary.Keys.Max() < index)
        {
            ComponentInsertedAt = index;
            component._Index = ComponentInsertedAt;
            component._Parent = this;
            if (SetToDictonary(component))
                component._Temporary = false;
            //_ContentDictonary.Add(_ContentDictonary.Keys.Max() + 1, Content);
        }
        //Asked to insert within items already in the Dic so cycle through moving each item higher or equal up by one then just add the new item
        //The Content Dictonary is different than all the others as it is to never have gaps between items and it is Zero based.
        else
        {
            foreach (var item in _ComponentDictonary.Reverse())
            {
                if (item.Key >= index)
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

            component._Index = index;
            component._Parent = this;
            SetParent();
            component._Temporary = false;
            _ComponentDictonary[index] = component;
            ComponentInsertedAt = index;
        }

        return _ComponentDictonary[ComponentInsertedAt];
    }

    internal bool ComponentRemoveAt(int index)
    {
        if (_ComponentDictonary.ContainsKey(index))
        {
            Dictionary<int, Component> oNewDic = new Dictionary<int, Component>();
            foreach (var item in _ComponentDictonary)
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

            _ComponentDictonary = oNewDic;
            if (_ComponentDictonary.Count == 0)
            {
                RemoveFromParent();
            }

            return true;
        }

        return false;
    }

    //SubComponents
    internal SubComponent GetSubComponent(int index)
    {
        if (_ComponentDictonary.ContainsKey(1))
        {
            return _ComponentDictonary[1].GetSubComponent(index);
        }

        Component oComponent = new Component(string.Empty, Delimiters, true, 1, this);
        return oComponent.GetSubComponent(index);
    }

    internal int CountSubComponent
    {
        get
        {
            if (_ComponentDictonary.TryGetValue(1, out var value))
            {
                return value.CountSubComponent;
            }

            return 0;
        }
    }

    internal SubComponent SubComponentSet(SubComponent subComponent, int index)
    {
        if (_ComponentDictonary.TryGetValue(1, out Component value))
        {
            return value.SubComponentSet(subComponent, index);
        }

        Component oComponent = new Component(string.Empty, Delimiters, true, 1, this);
        return oComponent.SubComponentSet(subComponent, index);
    }

    internal SubComponent SubComponentAppend(SubComponent subComponent)
    {
        if (_ComponentDictonary.TryGetValue(1, out Component value))
        {
            return value.SubComponentAppend(subComponent);
        }

        Component oComponent = new Component(string.Empty, subComponent.Delimiters, true, 1, this);
        return oComponent.SubComponentAppend(subComponent);
    }

    internal SubComponent SubComponentInsertBefore(SubComponent subComponent, int index)
    {
        if (_ComponentDictonary.TryGetValue(1, out Component value))
        {
            return value.SubComponentInsertBefore(subComponent, index);
        }

        Component oComponent = new Component(string.Empty, Delimiters, true, 1, this);
        return oComponent.SubComponentInsertBefore(subComponent, index);
    }

    internal bool SubComponentRemoveAt(int index)
    {
        if (_ComponentDictonary.TryGetValue(1, out Component value))
        {
            return value.SubComponentRemoveAt(index);
        }

        return false;
    }

    //Content
    internal Content GetContent(int index)
    {
        if (_ComponentDictonary.TryGetValue(1, out Component value))
        {
            return value.GetContent(index);
        }

        Component oComponent = new Component(string.Empty, Delimiters, true, 1, this);
        return oComponent.GetContent(index);
    }

    internal int CountContent
    {
        get
        {
            if (_ComponentDictonary.TryGetValue(1, out var value))
            {
                return value.CountContent;
            }

            return 0;
        }
    }

    internal Content ContentSet(Content content, int index)
    {
        if (_ComponentDictonary.TryGetValue(1, out Component value))
        {
            return value.ContentSet(content, index);
        }

        Component oComponent = new Component(string.Empty, Delimiters, true, 1, this);
        return oComponent.ContentSet(content, index);
    }

    internal Content ContentAppend(Content content)
    {
        if (_ComponentDictonary.TryGetValue(1, out Component value))
        {
            return value.ContentAppend(content);
        }

        Component oComponent = new Component(string.Empty, Delimiters, true, 1, this);
        return oComponent.ContentAppend(content);
    }

    internal Content ContentInsertBefore(Content content, int index)
    {
        if (_ComponentDictonary.TryGetValue(1, out Component value))
        {
            return value.ContentInsertBefore(content, index);
        }

        Component oComponent = new Component(string.Empty, Delimiters, true, 1, this);
        return oComponent.ContentInsertBefore(content, index);
    }

    internal bool ContentRemoveAt(int index)
    {
        if (_ComponentDictonary.TryGetValue(1, out Component value))
        {
            return value.ContentRemoveAt(index);
        }

        return false;
    }

    //Building
    private string GetAsStringOrAsRawString(bool rawString)
    {
        if (_ComponentDictonary.Count == 0)
            return string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();
        _ComponentDictonary.OrderByDescending(i => i.Key);
        for (int i = 1; i < _ComponentDictonary.Keys.Max() + 1; i++)
        {
            if (_ComponentDictonary.ContainsKey(i))
            {
                if (rawString)
                    oStringBuilder.Append(_ComponentDictonary[i].AsStringRaw);
                else
                    oStringBuilder.Append(_ComponentDictonary[i].AsString);
            }

            if (i != _ComponentDictonary.Keys.Max())
                oStringBuilder.Append(Delimiters.Component);
        }

        return oStringBuilder.ToString();
    }

    //Maintenance
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
            throw new PeterPiperException("Error setting Component into Field Parent", Exec);
        }
    }

    private void SetParent()
    {
        if (_Temporary)
        {
            if (_Parent is Element oElement)
            {
                if (oElement.SetToDictonary(this))
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
            if (_ComponentDictonary.TryGetValue(index, out Component value))
            {
                value._Temporary = true;
                value._Index = null;
                value._Parent = null;
            }

            _ComponentDictonary.Remove(index);
            if (_ComponentDictonary.Count == 0)
            {
                RemoveFromParent();
            }
        }
        catch
        {
            throw new PeterPiperException(
                $"Field's Component dictionary did not contain Component Index {index} for removal call from Component Instance");
        }
    }

    private void RemoveFromParent()
    {
        if (_Index != null)
        {
            if (_Parent is Element oElement)
            {
                oElement.RemoveChild(System.Convert.ToInt32(_Index));
                return;
            }

            throw new PeterPiperException(
                "Casting of Field's parent to Element throws Invalid Cast Exception, check inner exception for more detail");
        }
    }

    //Parsing and Validation
    private Dictionary<int, Component> ParseFieldRawStringToComponent(String stringRaw,
        ModelSupport.ContentTypeInternal contentTypeInternal)
    {
        //Example:  "first^Second^Third^\H\Forth bold\N\^fith&SubHere^Six";
        _ComponentDictonary = new Dictionary<int, Component>();
        if (contentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters ||
            contentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator)
        {
            _ComponentDictonary.Add(1, new Component(contentTypeInternal, Delimiters, false, 1, this));
        }
        else
        {
            if (stringRaw.Contains(Delimiters.Component))
            {
                string[] ComponentParts = stringRaw.Split(Delimiters.Component);
                int CompoenentPositionCounter = 1;
                foreach (string Part in ComponentParts)
                {
                    if (Part != string.Empty)
                    {
                        new Component(Part, Delimiters, true, CompoenentPositionCounter, this);
                    }

                    CompoenentPositionCounter++;
                }
            }
            else
            {
                //_ComponentDictonary.Add(1, new Component(StringRaw, this.Delimiters, this._Temporary, 1, this));
                new Component(stringRaw, Delimiters, true, 1, this);
            }
        }

        return _ComponentDictonary;
    }

    private bool ValidateStringRaw(string stringRaw)
    {
        Char[] CharatersNotAlowed =
        {
            Delimiters.Field,
            Delimiters.Repeat
        };

        if (stringRaw.IndexOfAny(CharatersNotAlowed) != -1)
        {
            string ErrorChars = string.Join(" or ", CharatersNotAlowed);
            throw new PeterPiperException($"Field data cannot contain HL7 V2 Delimiters of : {ErrorChars}");
        }

        return true;
    }
}
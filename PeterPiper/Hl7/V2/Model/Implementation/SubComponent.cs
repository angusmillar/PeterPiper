using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation;

internal class SubComponent : ContentBase, ISubComponent
{
    private Dictionary<int, Content> _ContentDictonary;

    //Creator Factory used Constructors
    internal SubComponent()
    {
        _ContentDictonary = new Dictionary<int, Content>();
        _Temporary = true;
        _Index = null;
        _Parent = null;
    }

    internal SubComponent(IMessageDelimiters customDelimiters)
        : base(customDelimiters)
    {
        _ContentDictonary = new Dictionary<int, Content>();
        _Temporary = true;
        _Index = null;
        _Parent = null;
    }

    internal SubComponent(string stringRaw)
    {
        _Temporary = true;
        _Index = null;
        _Parent = null;
        if (ValidateStringRaw(stringRaw))
        {
            _ContentDictonary =
                ParseSubComponentRawStringToContent(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    internal SubComponent(string stringRaw, IMessageDelimiters customDelimiters)
        : base(customDelimiters)
    {
        _Temporary = true;
        _Index = null;
        _Parent = null;
        if (ValidateStringRaw(stringRaw))
        {
            _ContentDictonary =
                ParseSubComponentRawStringToContent(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    //Only internal Constructors
    internal SubComponent(Content content, MessageDelimiters customDelimiters,
        Boolean temporary, int? index, ModelBase parent)
        : base(customDelimiters)
    {
        ValidateItemNotInUse(content);
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;
        _ContentDictonary = new Dictionary<int, Content>();
        content._Index = 0;
        content._Parent = this;
        content._Temporary = false;
        _ContentDictonary.Add(0, content);
        this.SetParent();
    }

    internal SubComponent(string stringRaw, MessageDelimiters customDelimiters,
        Boolean temporary, int? index, ModelBase parent)
        : base(customDelimiters)
    {
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;
        if (ValidateStringRaw(stringRaw))
        {
            _ContentDictonary =
                ParseSubComponentRawStringToContent(stringRaw, ModelSupport.ContentTypeInternal.Unknown);
        }
    }

    internal SubComponent(ModelSupport.ContentTypeInternal contentTypeInternal,
        MessageDelimiters customDelimiters, Boolean temporary,
        int? index, ModelBase parent)
        : base(customDelimiters)
    {
        _Temporary = temporary;
        _Index = index;
        _Parent = parent;
        _ContentDictonary = ParseSubComponentRawStringToContent(string.Empty, contentTypeInternal);
    }

    //Instance access
    public IMessageDelimiters MessageDelimiters => Delimiters;

    public ISubComponent Clone()
    {
        return new SubComponent(AsStringRaw, Delimiters, true, null, null);
    }

    public override string ToString()
    {
        return this.AsString;
    }

    public override string AsString
    {
        get => GetAsStringOrAsRawString(false);
        set => AsStringRaw = Support.Standard.Escapes.Encode(value, Delimiters);
    }

    private string GetAsStringOrAsRawString(bool rawString)
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
                    if (rawString)
                    {
                        oStringBuilder.Append(this.Delimiters.Escape);
                        oStringBuilder.Append(_ContentDictonary[i].AsStringRaw);
                        oStringBuilder.Append(this.Delimiters.Escape);
                    }
                    else
                    {
                        //The 'Content.AsString' will return the correct string for a given escape, for instance if the escape 
                        //is ampersand '&' which is escaped generally as \T\ then AsString will return '&', yet if the escape 
                        //is a HighlightOn escape '\H\' then AsString will return empty string.
                        //Given this we are correct to call AsString as below.
                        oStringBuilder.Append(_ContentDictonary[i].AsString);
                    }
                }
            }
            else
            {
                throw new PeterPiperException(
                    "Content dictionary has non-sequential items, this is a library error.");
            }
        }

        return oStringBuilder.ToString();
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
                _ContentDictonary =
                    ParseSubComponentRawStringToContent(value, ModelSupport.ContentTypeInternal.Unknown);
            }
        }
    }

    public bool IsEmpty => (_ContentDictonary.Count == 0);

    public bool IsHL7Null
    {
        get
        {
            if (GetContent(0).IsHL7Null)
            {
                if (ContentCount == 1)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public void ClearAll()
    {
        _ContentDictonary.Clear();
        RemoveFromParent();
    }

    public void Set(int index, IContent item)
    {
        ValidateItemNotInUse(item as Content);
        ContentSet(index, item as Content);
    }

    public void Add(IContent item)
    {
        ValidateItemNotInUse(item as Content);
        this.ContentAppend(item as Content);
    }

    public void Insert(int index, IContent item)
    {
        ValidateItemNotInUse(item as Content);
        ContentInsertBefore(item as Content, index);
    }

    public void RemoveContentAt(int index)
    {
        ContentRemoveAt(index);
    }

    public int ContentCount => CountContent;

    public bool HasContents => CountContent > 1;

    public IContent Content(int index)
    {
        return GetContent(index);
    }

    public ReadOnlyCollection<IContent> ContentList
    {
        get { return _ContentDictonary.OrderBy(x => x.Key).Select(i => i.Value as IContent).ToList().AsReadOnly(); }
    }

    //Content    
    internal Content GetContent(int index)
    {
        if (_ContentDictonary.TryGetValue(index, out var content))
        {
            return content;
        }

        if (_ContentDictonary.Count == 1 && _ContentDictonary[0]._Temporary)
        {
            return new Content(string.Empty, ModelSupport.ContentTypeInternal.Text, this.Delimiters, true, 0, this);
        }

        if (_ContentDictonary.Count == 0)
        {
            return new Content(string.Empty, ModelSupport.ContentTypeInternal.Text, this.Delimiters, true, 0,
                this);
        }

        return new Content(string.Empty, ModelSupport.ContentTypeInternal.Text, this.Delimiters, true,
            _ContentDictonary.Keys.Max() + 1, this);
    }

    internal int CountContent
    {
        get
        {
            if (_ContentDictonary.Count == 0)
            {
                return 0;
            }

            return _ContentDictonary.Keys.Max() + 1;
        }
    }

    internal Content ContentSet(int index, Content content)
    {
        if (content.AsStringRaw == String.Empty)
        {
            RemoveChild(index);
            content._Index = null;
            content._Parent = null;
            content._Temporary = true;
            return content;
        }

        content._Index = index;
        content._Parent = this;
        if (SetToDictonary(content))
            content._Temporary = false;
        return _ContentDictonary[System.Convert.ToInt32(content._Index)];
    }

    internal Content ContentAppend(Content content)
    {
        if (_ContentDictonary.Count > 0)
        {
            return ContentInsertBefore(content, _ContentDictonary.Keys.Max() + 1);
        }

        content._Index = 0;
        content._Parent = this;
        if (SetToDictonary(content))
            content._Temporary = false;
        return _ContentDictonary[0];
    }

    internal Content ContentInsertBefore(Content content, int index)
    {
        int LastIndexUsed;
        //Empty dictionary so just add as first item 
        if (_ContentDictonary.Count == 0)
        {
            LastIndexUsed = 0;
            content._Index = LastIndexUsed;
            content._Parent = this;
            if (SetToDictonary(content))
                content._Temporary = false;
        }
        //Asked to insert before an index larger than the largest in dictionary so just add to the end
        else if (_ContentDictonary.Keys.Max() < index)
        {
            LastIndexUsed = _ContentDictonary.Keys.Max() + 1;
            content._Index = LastIndexUsed;
            content._Parent = this;
            if (SetToDictonary(content))
                content._Temporary = false;
        }
        //Asked to insert within items already in the dictionary so cycle through moving each item higher or equal up by one then just add the new item
        //The Content dictionary is different from all the others as it is to never have gaps between items and it is Zero based.
        else
        {
            foreach (var item in _ContentDictonary.Reverse())
            {
                if (item.Key >= index)
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

            content._Index = index;
            content._Parent = this;
            SetParent();
            content._Temporary = false;
            _ContentDictonary[index] = content;
            LastIndexUsed = index;
        }

        return _ContentDictonary[LastIndexUsed];
    }

    internal bool ContentRemoveAt(int index)
    {
        if (_ContentDictonary.Count >= index + 1)
        {
            Dictionary<int, Content> oNewDic = new Dictionary<int, Content>();
            foreach (var item in _ContentDictonary)
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
            if (_ContentDictonary.ContainsKey(System.Convert.ToInt32(oContent._Index)))
            {
                _ContentDictonary[System.Convert.ToInt32(oContent._Index)]._Temporary = true;
                _ContentDictonary[System.Convert.ToInt32(oContent._Index)]._Parent = null;
                _ContentDictonary[System.Convert.ToInt32(oContent._Index)]._Index = null;
                //---------------------------------------
                _ContentDictonary[System.Convert.ToInt32(oContent._Index)] = oContent;
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
            throw new PeterPiperException("Error setting Content into SubComponent Parent", Exec);
        }
    }

    private void SetParent()
    {
        if (_Temporary)
        {
            if (_Parent is Component oComponent)
            {
                if (oComponent.SetToDictonary(this))
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
            if (_ContentDictonary.ContainsKey(index))
            {
                _ContentDictonary[index]._Temporary = true;
                _ContentDictonary[index]._Index = null;
                _ContentDictonary[index]._Parent = null;
            }

            ContentRemoveAt(index);
            if (_ContentDictonary.Count == 0)
            {
                RemoveFromParent();
            }
        }
        catch
        {
            throw new PeterPiperException(
                $"SubComponent's ContentDictonary dictionary did not contain Content Index {index} for removal call from Content Instance");
        }
    }

    private void RemoveFromParent()
    {
        if (_Index != null)
        {
            if (_Parent is Component oComponent)
            {
                oComponent.RemoveChild(System.Convert.ToInt32(this._Index));
            }
            else
            {
                throw new PeterPiperException(
                    "Casting of SubComponent's parent to Component throws Invalid Cast Exception, check inner exception for more detail");
            }
        }
    }

    private void ConstructorCommon()
    {
    }

    //Parsing and Validation
    private Dictionary<int, Content> ParseSubComponentRawStringToContent(String stringRaw,
        ModelSupport.ContentTypeInternal contentTypeInternal)
    {
        //Example:  "\\N\\this is not Highlighted\\H\\ This is highlighted \\N\\ This is not";
        _ContentDictonary = new Dictionary<int, Content>();
        if (contentTypeInternal == ModelSupport.ContentTypeInternal.EncodingCharacters ||
            contentTypeInternal == ModelSupport.ContentTypeInternal.MainSeparator)
        {
            _ContentDictonary.Add(0,
                new Content(string.Empty, contentTypeInternal, this.Delimiters, false, 0, this));
        }
        else
        {
            if (stringRaw.Contains(Delimiters.Escape))
            {
                string[] ContentParts = stringRaw.Split(Delimiters.Escape);
                bool InEscape = false;
                int SubCompoenentPositionCounter = 0;
                foreach (string Part in ContentParts)
                {
                    if (InEscape)
                    {
                        new Content(Part.Trim(), ModelSupport.ContentTypeInternal.Escape, Delimiters, true,
                            SubCompoenentPositionCounter, this);
                        SubCompoenentPositionCounter++;
                        InEscape = false;
                    }
                    else
                    {
                        if (Part != string.Empty)
                        {
                            new Content(Part, ModelSupport.ContentTypeInternal.Text, this.Delimiters, true,
                                SubCompoenentPositionCounter, this);
                            SubCompoenentPositionCounter++;
                        }

                        InEscape = true;
                    }
                }
            }
            else
            {
                new Content(stringRaw, ModelSupport.ContentTypeInternal.Text, this.Delimiters, true, 0, this);
            }
        }

        return _ContentDictonary;
    }

    private bool ValidateStringRaw(string stringRaw)
    {
        Char[] CharactersNotAllowed =
        {
            Delimiters.Field,
            Delimiters.Component,
            Delimiters.SubComponent,
            Delimiters.Repeat
        };

        if (stringRaw.IndexOfAny(CharactersNotAllowed) != -1)
        {
            string DelimitersNotAllowed =
                $"{CharactersNotAllowed[0]}{CharactersNotAllowed[1]}{CharactersNotAllowed[2]}{CharactersNotAllowed[3]}";
            throw new PeterPiperException(
                $"SubComponent data cannot contain HL7 V2 Delimiters of : {DelimitersNotAllowed}");
        }

        return true;
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation;

internal class Message : ModelBase, IMessage
{
    private Dictionary<int, Segment> _SegmentDictonary;
    private readonly bool _ParseMSHSegmentOnly = false;

    //Creator Factory used Constructors
    internal Message(string messageVersion, string messageType, string messageTrigger,
        string messageControlID = "<GUID>", string messageStructure = "")
    {
        //need to validate version and message types somewhat.
        StringBuilder MSHTemplate = new StringBuilder(Support.Standard.Segments.Msh.Code);
        MSHTemplate.Append(Delimiters.Field);
        MSHTemplate.Append(Delimiters.Component);
        MSHTemplate.Append(Delimiters.Repeat);
        MSHTemplate.Append(Delimiters.Escape);
        MSHTemplate.Append(Delimiters.SubComponent);
        MSHTemplate.Append(Delimiters.Field, 5);
        MSHTemplate.Append(Support.Tools.DateTimeSupportTools.AsString(DateTimeOffset.Now, true,
            Support.Tools.DateTimeSupportTools.DateTimePrecision.DateHourMinSecMilli));
        MSHTemplate.Append(Delimiters.Field, 2);
        MSHTemplate.Append(messageType);
        MSHTemplate.Append(Delimiters.Component);
        MSHTemplate.Append(messageTrigger);
        if (messageStructure != String.Empty)
        {
            MSHTemplate.Append(Delimiters.Component);
            MSHTemplate.Append(messageStructure);
        }

        MSHTemplate.Append(Delimiters.Field);
        if (messageControlID == "<GUID>")
            MSHTemplate.Append(Guid.NewGuid());
        else
            MSHTemplate.Append(messageControlID);
        MSHTemplate.Append(Delimiters.Field);
        MSHTemplate.Append(Support.Standard.Hl7Table.Table_0103.Production);
        MSHTemplate.Append(Delimiters.Field);
        MSHTemplate.Append(messageVersion);
        MSHTemplate.Append(Delimiters.Field, 3);
        MSHTemplate.Append("AL");
        MSHTemplate.Append(Delimiters.Field);
        MSHTemplate.Append("NE");

        _SegmentDictonary = new Dictionary<int, Segment>();
        _SegmentDictonary.Add(1, new Segment(MSHTemplate.ToString(), Delimiters, false, 1, this));
    }

    internal Message(ISegment item)
        : base(item.MessageDelimiters)
    {
        if (item is Segment Segment)
        {
            ValidateItemNotInUse(Segment);
            if (Segment.IsMSH)
            {
                _SegmentDictonary = new Dictionary<int, Segment>();
                Segment._Index = 1;
                Segment._Parent = this;
                Segment._Temporary = false;
                _SegmentDictonary.Add(1, Segment);
                return;
            }

            throw new PeterPiperException(
                "The Segment instance passed in is not a MSH Segment, only a MSH Segment can be passed in on creation / instantiation of a Message");
        }

        throw new InvalidCastException($"The ISegment instance passed in is not of type {nameof(Segment)}");
    }

    internal Message(string stringRaw, bool parseMSHSegmentOnly = false)
    {
        List<string> MessageList = stringRaw.Split(Support.Standard.Delimiters.SegmentTerminator).ToList();
        if (parseMSHSegmentOnly)
        {
            _ParseMSHSegmentOnly = true;
            ParseMshSegmentOnly(MessageList);
            return;
        }

        if (ValidateStringRaw(MessageList))
        {
            _SegmentDictonary = ParseMessageRawStringToSegment(MessageList);
        }
    }

    internal Message(List<string> collection, bool parseMSHSegmentOnly = false)
    {
        if (collection.Count <= 0)
        {
            throw new PeterPiperException("The message list passed has no content.");
        }

        if (parseMSHSegmentOnly)
        {
            _ParseMSHSegmentOnly = true;
            ParseMshSegmentOnly(collection);
            return;
        }

        if (ValidateStringRaw(collection))
        {
            _SegmentDictonary = ParseMessageRawStringToSegment(collection);
        }
    }

    //Instance access
    public IMessage Clone()
    {
        return new Message(AsStringRaw);
    }

    public override string ToString()
    {
        return AsString;
    }

    public override string AsString
    {
        get
        {
            if (_SegmentDictonary.Count == 0)
                return string.Empty;
            StringBuilder oStringBuilder = new StringBuilder();
            _SegmentDictonary.OrderByDescending(i => i.Key);
            for (int i = 1; i < _SegmentDictonary.Keys.Max() + 1; i++)
            {
                if (_SegmentDictonary.TryGetValue(i, out Segment value))
                {
                    oStringBuilder.Append(value.AsString);
                    oStringBuilder.Append(Support.Standard.Delimiters.SegmentTerminator);
                }
            }

            return oStringBuilder.ToString();
        }
        set =>
            throw new PeterPiperException("While setting a message using AsString() could technically work " +
                                          "it would make no sense to have the message's escape characters in MSH-1 " +
                                          "& MSH-2 re-escaped. You should be using AsStringRaw()");
    }

    public override string AsStringRaw
    {
        get
        {
            if (_SegmentDictonary.Count == 0)
                return string.Empty;
            StringBuilder oStringBuilder = new StringBuilder();
            _SegmentDictonary.OrderByDescending(i => i.Key);
            for (int i = 1; i < _SegmentDictonary.Keys.Max() + 1; i++)
            {
                if (_SegmentDictonary.TryGetValue(i, out Segment value))
                {
                    oStringBuilder.Append(value.AsStringRaw);
                    oStringBuilder.Append(Support.Standard.Delimiters.SegmentTerminator);
                }
            }

            return oStringBuilder.ToString();
        }
        set
        {
            List<string> MessageList = value.Split(Support.Standard.Delimiters.SegmentTerminator).ToList();
            if (ValidateStringRaw(MessageList))
            {
                _SegmentDictonary = ParseMessageRawStringToSegment(MessageList);
            }
        }
    }

    public void ClearAll()
    {
        Segment oMSH = _SegmentDictonary[1].Clone() as Segment;
        _SegmentDictonary = new Dictionary<int, Segment>();
        _SegmentDictonary.Add(1, oMSH);
        _SegmentDictonary[1].ClearAll();
    }

    public string EscapeSequence =>
        $"{Delimiters.Component}{Delimiters.Repeat}{Delimiters.Escape}{Delimiters.SubComponent}";

    public string MainSeparator => Delimiters.Field.ToString();

    public string MessageType => GetSegment(1).GetField(9).GetComponent(1).AsString;

    public string MessageTrigger => GetSegment(1).GetField(9).GetComponent(2).AsString;

    public string MessageStructure
    {
        get
        {
            if (GetSegment(1).GetField(9).GetComponent(3).IsEmpty)
            {
                return string.Empty;
            }

            return GetSegment(1).GetField(9).GetComponent(3).AsString;
        }
    }

    public string MessageVersion => GetSegment(1).GetField(12).Component(1).SubComponent(1).AsString;

    public string MessageControlID => GetSegment(1).GetField(10).AsStringRaw;

    public bool IsParseMSHSegmentOnly => _ParseMSHSegmentOnly;

    public DateTimeOffset MessageCreationDateTime => GetSegment(1).GetField(7).Convert.DateTime.GetDateTimeOffset();

    public IMessageDelimiters MessageDelimiters => Delimiters;

    public void Add(ISegment item)
    {
        ValidateItemNotInUse(item as Segment);
        SegmentAppend(item as Segment);
    }

    public void Insert(int index, ISegment item)
    {
        ValidateItemNotInUse(item as Segment);
        SegmentInsertBefore(item as Segment, index);
    }

    public bool RemoveSegmentAt(int index)
    {
        return SegmentRemoveAt(index);
    }

    public int SegmentCount(string code)
    {
        ValidateSegmentCode(code);
        return _SegmentDictonary.Count(x => x.Value.Code == code);
    }

    public int SegmentCount()
    {
        return CountSegment;
    }

    public ISegment Segment(string code)
    {
        ValidateSegmentCode(code);
        return GetSegment(code);
    }

    public ISegment Segment(int index)
    {
        ThrowIfIndexIsZeroForSegment(index);
        return GetSegment(index);
    }

    public ReadOnlyCollection<ISegment> SegmentList(string code)
    {
        ValidateSegmentCode(code);
        return _SegmentDictonary.OrderBy(x => x.Key).Select(i => i.Value as ISegment).ToList()
            .Where(x => x.Code == code).ToList().AsReadOnly();
    }

    public ReadOnlyCollection<ISegment> SegmentList()
    {
        return _SegmentDictonary.OrderBy(x => x.Key).Select(i => i.Value as ISegment).ToList().AsReadOnly();
    }

    //Segment
    internal Segment GetSegment(int index)
    {
        if (_SegmentDictonary.ContainsKey(index))
            return _SegmentDictonary[index];
        else
            return null;
    }

    internal Segment GetSegment(string code)
    {
        ValidateSegmentCode(code);
        Segment oSeg = _SegmentDictonary.FirstOrDefault(i =>
            string.Equals(i.Value.Code, code, StringComparison.CurrentCultureIgnoreCase)).Value;
        if (oSeg != null)
        {
            return oSeg;
        }

        throw new PeterPiperException("There is no segment with a segment code of " + code +
                                      " within the message. \n Perhaps you should test for the segments existence " +
                                      "with SegmentCount first or create the segment in the message first.");
    }

    internal int CountSegment => _SegmentDictonary.Count;

    internal Segment SegmentAppend(Segment segment)
    {
        ValidateSegmentAddition(segment);

        int InsertAtIndex = 1;
        if (_SegmentDictonary.Count > 0)
        {
            InsertAtIndex = _SegmentDictonary.Keys.Max() + 1;
        }

        segment._Index = InsertAtIndex;
        segment._Parent = this;
        segment._Temporary = false;
        _SegmentDictonary.Add(InsertAtIndex, segment);
        //No need to set the parent as there is non at te moment.
        return _SegmentDictonary[_SegmentDictonary.Keys.Max()];
    }

    internal Segment SegmentInsertBefore(Segment segment, int index)
    {
        ThrowIfIndexIsZeroForSegment(index);
        ValidateSegmentAddition(segment);
        int SegmentInsertedAt = 0;
        //Empty Dic so just add as first itme 
        // Does this check if the first segment is a MSH??
        if (_SegmentDictonary.Count == 0)
        {
            SegmentInsertedAt = 1;
            segment._Index = SegmentInsertedAt;
            segment._Parent = this;
            segment._Temporary = false;
            _SegmentDictonary.Add(SegmentInsertedAt, segment);
            return _SegmentDictonary[SegmentInsertedAt];
        }

        //Asked to insert before an index larger than the largest in Dic so just add to the end
        if (_SegmentDictonary.Keys.Max() < index)
        {
            SegmentInsertedAt = _SegmentDictonary.Keys.Max() + 1;
            segment._Index = SegmentInsertedAt;
            segment._Parent = this;
            segment._Temporary = false;
            _SegmentDictonary.Add(SegmentInsertedAt, segment);
            return _SegmentDictonary[SegmentInsertedAt];
        }
        //Asked to insert within items already in the Dic so cycle through moving each item higher or equal up by one then just add the new item

        foreach (var item in _SegmentDictonary.Reverse())
        {
            if (item.Key >= index)
            {
                item.Value._Index++;
                if (_SegmentDictonary.ContainsKey(item.Key + 1))
                {
                    _SegmentDictonary[item.Key + 1] = item.Value;
                }
                else
                {
                    _SegmentDictonary.Add(item.Key + 1, item.Value);
                }
            }
        }

        SegmentInsertedAt = index;
        segment._Index = SegmentInsertedAt;
        segment._Parent = this;
        segment._Temporary = false;
        _SegmentDictonary[SegmentInsertedAt] = segment;

        return _SegmentDictonary[SegmentInsertedAt];
    }

    internal bool SegmentRemoveAt(int index)
    {
        ThrowIfIndexIsZeroForSegment(index);
        if (index == 1)
        {
            throw new PeterPiperException(
                "Index one is the MSH Segment. This segment can not be removed, it can be modified or a new Message instance can be created");
        }

        if (_SegmentDictonary.Count >= index)
        {
            Dictionary<int, Segment> oNewDic = new Dictionary<int, Segment>();
            foreach (var item in _SegmentDictonary)
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

            _SegmentDictonary = oNewDic;
            return true;
        }

        return false;
    }

    //Maintenance
    internal bool SetContent(Segment oSegment)
    {
        try
        {
            if (_SegmentDictonary.ContainsKey(Convert.ToInt32(oSegment._Index)))
            {
                _SegmentDictonary[Convert.ToInt32(oSegment._Index)]._Temporary = true;
                _SegmentDictonary[Convert.ToInt32(oSegment._Index)] = oSegment;
            }
            else
            {
                _SegmentDictonary.Add(Convert.ToInt32(oSegment._Index), oSegment);
            }

            return true;
        }
        catch (Exception Exec)
        {
            throw new PeterPiperException("Error setting Segment into Message parent", Exec);
        }
    }

    //Parsing and Validation
    private bool ValidateSegmentCode(string code)
    {
        if (code.Length != 3)
        {
            throw new PeterPiperException("All Segment codes must be 3 characters in length.");
        }

        return true;
    }

    private Dictionary<int, Segment> ParseMessageRawStringToSegment(List<string> stringRawList)
    {
        _SegmentDictonary = new Dictionary<int, Segment>();
        int SegmentPositionCounter = 1;
        foreach (string Part in stringRawList)
        {
            if (Part != string.Empty)
            {
                Segment oCurrentSegment = new Segment(Part, Delimiters, false, SegmentPositionCounter, this);
                if (_SegmentDictonary.Count > 0 && oCurrentSegment.Code == Support.Standard.Segments.Msh.Code)
                    throw new PeterPiperException(
                        "Second MSH segment found when parsing new message. Single messages must only have " +
                        "one MSH segment as the first segment.");
                _SegmentDictonary.Add(SegmentPositionCounter, oCurrentSegment);
            }

            SegmentPositionCounter++;
        }

        return _SegmentDictonary;
    }

    private bool ValidateStringRaw(List<string> stringRawList)
    {
        string FirstSegment = stringRawList[0];
        if (IsSegmentCode(FirstSegment, Support.Standard.Segments.Msh.Code))
        {
            Delimiters = ExtractDelimitersFromStringRaw(FirstSegment);
            string[] mshSegmentFields = stringRawList[0].Split(Delimiters.Field);
            if (mshSegmentFields.Length < 12)
            {
                throw new PeterPiperException(
                    $"The passed message's {Support.Standard.Segments.Msh.Code} Segment does not have the " +
                    $"minimum number of Fields. There must be at least 12 to allow for the Message Version Field");
            }
        }
        else
        {
            throw new PeterPiperException(String.Format(
                "The passed message must begin with the Message Header Segment and code: '{0}'",
                Support.Standard.Segments.Msh.Code));
        }

        return true;
    }

    private void ValidateSegmentAddition(Segment oSegment)
    {
        if (oSegment.IsMSH)
        {
            throw new PeterPiperException(
                "An MSH Segment can not be added to a Message instance, it must be provided on Message instance creation / instantiation");
        }

        if (!ValidateDelimiters(oSegment.Delimiters))
        {
            throw new PeterPiperException(
                "The Segment instance being added to this parent Message instance has custom delimiters that are different than the parent, this is not allowed");
        }
    }

    private void ParseMshSegmentOnly(List<string> messageList)
    {
        if (ValidateStringRaw(messageList))
        {
            _SegmentDictonary = new Dictionary<int, Segment>();
            _SegmentDictonary.Add(1, new Segment(messageList[0]));
            if (_SegmentDictonary[1].Code.ToUpper() != Support.Standard.Segments.Msh.Code)
            {
                throw new PeterPiperException(
                    $"The passed message must begin with the Message Header Segment and code: '{Support.Standard.Segments.Msh.Code}'");
            }
        }
    }

    internal static MessageDelimiters ExtractDelimitersFromStringRaw(string stringRaw)
    {
        string segmentCode = stringRaw.Substring(0, 3);
        char fieldSeparator = Convert.ToChar(stringRaw.Substring(3, 1));
        char componentSeparator = Convert.ToChar(stringRaw.Substring(4, 1));
        char repeatSeparator = Convert.ToChar(stringRaw.Substring(5, 1));
        char escapeSeparator = Convert.ToChar(stringRaw.Substring(6, 1));
        char subComponentSeparator = Convert.ToChar(stringRaw.Substring(7, 1));

        if (IsSegmentCode(stringRaw, Support.Standard.Segments.Msh.Code))
        {
            char FirstFieldSeparatorFound = Convert.ToChar(stringRaw.Substring(8, 1));
            if (FirstFieldSeparatorFound != fieldSeparator)
            {
                throw new PeterPiperException(String.Format(
                    "The passed message's defined Field separator at MSH-1: '{1}' \n has not been used as the first Field separator between MSH-2 & MSH-3, found separator of: '{2}'",
                    segmentCode, fieldSeparator, FirstFieldSeparatorFound));
            }
        }

        return new MessageDelimiters(fieldSeparator,
            repeatSeparator,
            componentSeparator,
            subComponentSeparator,
            escapeSeparator);
    }

    internal static bool IsSegmentCode(string segmentRawString, string code)
    {
        return (segmentRawString.Substring(0, 3).ToUpper() == code);
    }

    private static void ThrowIfIndexIsZeroForSegment(int index)
    {
        if (index == 0)
        {
            throw new PeterPiperException("Segment index is a one based index, zero in not allowed");
        }
    }
}
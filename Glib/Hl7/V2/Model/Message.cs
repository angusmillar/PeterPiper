using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Model
{
  public class Message : ModelBase
  {
    private Dictionary<int, Segment> _SegmentDictonary;
    private bool _ParseMSHSegmentOnly = false;
    //Constructors
    public Message(string MessageVersion, string MessageType, string MessageTrigger, string MessageControlID = "<GUID>", string MessageStructure = "")
    {      
      //need to validate version and message types somewhat.
      StringBuilder MSHTemplate = new StringBuilder(Support.Standard.Segments.Msh.Code);
      MSHTemplate.Append(this.Delimiters.Field);
      MSHTemplate.Append(this.Delimiters.Component);
      MSHTemplate.Append(this.Delimiters.Repeat);
      MSHTemplate.Append(this.Delimiters.Escape);
      MSHTemplate.Append(this.Delimiters.SubComponent);
      MSHTemplate.Append(this.Delimiters.Field,5);
      MSHTemplate.Append(Support.Content.DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSecMilli(DateTimeOffset.Now,true));
      MSHTemplate.Append(this.Delimiters.Field, 2);
      MSHTemplate.Append(MessageType);
      MSHTemplate.Append(this.Delimiters.Component);
      MSHTemplate.Append(MessageTrigger);
      if (MessageStructure != String.Empty)
      {
        MSHTemplate.Append(this.Delimiters.Component);
        MSHTemplate.Append(MessageStructure);
      }
      MSHTemplate.Append(this.Delimiters.Field);
      if (MessageControlID == "<GUID>")
        MSHTemplate.Append(Guid.NewGuid());
      else
        MSHTemplate.Append(MessageControlID);
      MSHTemplate.Append(this.Delimiters.Field);
      MSHTemplate.Append(Support.Standard.Hl7Table.Table_0103.Production);
      MSHTemplate.Append(this.Delimiters.Field);
      MSHTemplate.Append(MessageVersion);
      MSHTemplate.Append(this.Delimiters.Field, 3);
      MSHTemplate.Append("AL");
      MSHTemplate.Append(this.Delimiters.Field);
      MSHTemplate.Append("NE");
      
      _SegmentDictonary = new Dictionary<int, Segment>();      
      _SegmentDictonary.Add(1, new Segment(MSHTemplate.ToString(),this.Delimiters,false,1,this));
      
    }
    public Message(Segment item)
      : base(item.Delimiters)
    {
      ValidateItemNotInUse(item);
      if (item.IsMSH)
      {
        _SegmentDictonary = new Dictionary<int, Segment>();
        item._Index = 1;
        item._Parent = this;
        item._Temporary = false;
        _SegmentDictonary.Add(1, item);
      }
      else
      {
        throw new ArgumentException("The Segment instance passed in is not a MSH Segment, only a MSH Segment can be passed in on creation / instantiation of a Message");
      }
    }
    public Message(string StringRaw, bool ParseMSHSegmentOnly = false)
    {      
      List<string> MessageList = StringRaw.Split(Support.Standard.Delimiters.SegmentTerminator).ToList();
      if (ParseMSHSegmentOnly)
      {
        _ParseMSHSegmentOnly = true;
        ParseMshSegmentOnly(MessageList);
      }
      else
      {
        if (ValidateStringRaw(MessageList))
        {
          _SegmentDictonary = ParseMessageRawStringToSegment(MessageList);
        }
      }
    }    
    public Message(List<string> collection, bool ParseMSHSegmentOnly = false)
    {
      if (collection.Count > 0)
      {
        if (ParseMSHSegmentOnly)
        {
          _ParseMSHSegmentOnly = true;
          ParseMshSegmentOnly(collection);
        }
        else
        {
          if (ValidateStringRaw(collection))
          {
            _SegmentDictonary = ParseMessageRawStringToSegment(collection);
          }
        }
      }
      else
      {
        throw new ApplicationException("The message list passed has no content.");
      }
    }

    //Instance access
    public Message Clone()
    {
      return new Message(this.AsStringRaw);
    }
    public override string ToString()
    {
      return this.AsString;
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
          if (_SegmentDictonary.ContainsKey(i))
          {
            oStringBuilder.Append(_SegmentDictonary[i].AsString);
            oStringBuilder.Append(Support.Standard.Delimiters.SegmentTerminator);
          }
        }
        return oStringBuilder.ToString();        
      }
      set
      {
        throw new ArgumentException("While setting a message using AsString() could technicaly work it would make no sense to have the message's escape charaters in MSH-1 & MSH-2 re-escaped. You should be using AsStringRaw()");
      }
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
          if (_SegmentDictonary.ContainsKey(i))
          {
            oStringBuilder.Append(_SegmentDictonary[i].AsStringRaw);
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

      Segment oMSH = _SegmentDictonary[1].Clone();      
      _SegmentDictonary = new Dictionary<int, Segment>();           
      _SegmentDictonary.Add(1, oMSH);
      _SegmentDictonary[1].ClearAll(); ;
      
    }
    public string EscapeSequence
    {
      get
      {
        return String.Format("{0}{1}{2}{3}", this.Delimiters.Component, this.Delimiters.Repeat, this.Delimiters.Escape, this.Delimiters.SubComponent);
      }
    }
    public string MainSeparator
    {
      get
      {
        return String.Format("{0}", this.Delimiters.Field);
      }
    }
    public string MessageType
    {
      get
      {
        return this.GetSegment(1).GetField(9).GetComponent(1).AsString;
      }
    }
    public string MessageTrigger
    {
      get
      {
        return this.GetSegment(1).GetField(9).GetComponent(2).AsString;
      }
    }
    public string MessageStructure
    {
      get
      {
        if (this.GetSegment(1).GetField(9).GetComponent(3).IsEmpty)
          return string.Empty;
        else
          return this.GetSegment(1).GetField(9).GetComponent(3).AsString;
      }
    }
    public string MessageVersion
    {
      get
      {
        return this.GetSegment(1).GetField(12).AsString;
      }
    }
    public string MessageControlID
    {
      get
      {
        return this.GetSegment(1).GetField(10).AsStringRaw;
      }
    }
    public bool IsParseMSHSegmentOnly
    {
      get
      {
        return _ParseMSHSegmentOnly;
      }
    }
    
    public DateTimeOffset MessageCreationDateTime
    {
      get
      {
        return this.GetSegment(1).GetField(7).AsDateHourMinSecMilli();
      }
    }
    public Support.MessageDelimiters MessageDelimiters
    {
      get
      {
        return this.Delimiters;
      }
    }    
    public void Add(Segment item)
    {
      ValidateItemNotInUse(item);
      this.SegmentAppend(item);
    }
    public void Insert(int index, Segment item)
    {
      ValidateItemNotInUse(item);
      this.SegmentInsertBefore(item, index);
    }
    public bool RemoveSegmentAt(int index)
    {
      return this.SegmentRemoveAt(index);
    }    
    public int SegmentCount(string Code)
    {
      ValidateSegmentCode(Code);            
      return _SegmentDictonary.Count(x => x.Value.Code == Code);            
    }
    public int SegmentCount()
    {
      return this.CountSegment;
    }   
    public Segment Segment(string Code)
    {
      ValidateSegmentCode(Code);
      return GetSegment(Code);
    }    
    public Segment Segment(int index)
    {
      if (index == 0)
        throw new ArgumentOutOfRangeException("Element is a one based index, zero is not a valid index");
      return this.GetSegment(index); 

    }    
    public ReadOnlyCollection<Segment> SegmentList(string Code)
    {
      ValidateSegmentCode(Code);      
      return _SegmentDictonary.OrderBy(x => x.Key).Select(i => i.Value).ToList().Where(x => x.Code == Code).ToList().AsReadOnly();      
    }
    public ReadOnlyCollection<Segment> SegmentList()
    {
      return _SegmentDictonary.OrderBy(x => x.Key).Select(i => i.Value).ToList().AsReadOnly();
    }

    //Segment
    internal Segment GetSegment(int index)
    {
      if (_SegmentDictonary.ContainsKey(index))
        return _SegmentDictonary[index];
      else
        return null;
    }    
    internal Segment GetSegment(string Code)
    {
      ValidateSegmentCode(Code);      
      Segment oSeg = _SegmentDictonary.FirstOrDefault(i => i.Value.Code.ToUpper() == Code.ToUpper()).Value;
      if (oSeg != null)
        return oSeg;
      else
        throw new ArgumentOutOfRangeException("There is no segment with a segment code of " + Code + " within the message. \n Perhaps you should test for the segments existence with SegmentCount first or create the segment in the message first.");      
    }
    internal int CountSegment
    {
      get
      {
        return _SegmentDictonary.Count;
      }
    }
    internal Segment SegmentAppend(Segment Segment)
    {
      ValidateSegemntAdditon(Segment);
      
      int InsertAtIndex = 1;
      if (_SegmentDictonary.Count > 0)
        InsertAtIndex = _SegmentDictonary.Keys.Max() + 1;
      Segment._Index = InsertAtIndex;
      Segment._Parent = this;
      Segment._Temporary = false;
      _SegmentDictonary.Add(InsertAtIndex, Segment);
      //No need to set the parent as there is non at te moment.
      return _SegmentDictonary[_SegmentDictonary.Keys.Max()];
    }    
    internal Segment SegmentInsertBefore(Segment Segment, int Index)
    {
      if (Index == 0)
        throw new ArgumentOutOfRangeException("Segment index is a one based index, zero in not allowed");
      ValidateSegemntAdditon(Segment);
      int SegmentInsertedAt = 0;
      //Empty Dic so just add as first itme 
      // Does this check if the first segment is a MSH??
      if (_SegmentDictonary.Count == 0)
      {
        SegmentInsertedAt = 1;
        Segment._Index = SegmentInsertedAt;
        Segment._Parent = this;
        Segment._Temporary = false;
        _SegmentDictonary.Add(SegmentInsertedAt, Segment);
      }
      //Asked to insert before an index larger than the largest in Dic so just add to the end
      else if (_SegmentDictonary.Keys.Max() < Index)
      {
        SegmentInsertedAt = _SegmentDictonary.Keys.Max() + 1;
        Segment._Index = SegmentInsertedAt;
        Segment._Parent = this;
        Segment._Temporary = false;
        _SegmentDictonary.Add(SegmentInsertedAt, Segment);
      }
      //Asked to insert within items already in the Dic so cycle through moving each item higher or equal up by one then just add the new item
      else
      {
        foreach (var item in _SegmentDictonary.Reverse())
        {
          if (item.Key >= Index)
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
        SegmentInsertedAt = Index;
        Segment._Index = SegmentInsertedAt;
        Segment._Parent = this;
        Segment._Temporary = false;
        _SegmentDictonary[SegmentInsertedAt] = Segment;
      }
      return _SegmentDictonary[SegmentInsertedAt];
    }
    internal bool SegmentRemoveAt(int Index)
    {
      if (Index == 0)
        throw new ArgumentOutOfRangeException("Segment index is a one based index, zero in not allowed");
      else if (Index == 1)
      {
        throw new ArgumentException("Index one is the MSH Segment. This segment can not be removed, it can be modified or a new Message instance can be created");
      }
      else if (_SegmentDictonary.Count >= Index)
      {
        Dictionary<int, Segment> oNewDic = new Dictionary<int, Segment>();
        foreach (var item in _SegmentDictonary)
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
        _SegmentDictonary = oNewDic;
        return true;
      }
      else
      {
        return false;
      }
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
        throw new ApplicationException("Error setting Segment into Message parent", Exec);
      }
    }

    //Parsing and Validation
    private bool ValidateSegmentCode(string Code)
    {
      if (Code.Length != 3)
        throw new ArgumentException("All Segment codes must be 3 characters in length.");
      return true;
    }
    private Dictionary<int, Segment> ParseMessageRawStringToSegment(List<string> StringRawList)
    {
      _SegmentDictonary = new Dictionary<int, Segment>();
      int SegmentPositionCounter = 1;
      foreach (string Part in StringRawList)
      {
        if (Part != string.Empty)
        {
          Segment oCurrentSegment =  new Segment(Part, this.Delimiters, false, SegmentPositionCounter, this);
          if (_SegmentDictonary.Count > 0 && oCurrentSegment.Code == Support.Standard.Segments.Msh.Code)
            throw new ArgumentException("Second MSH segment found when parseing new message. Single messages must only have one MSH segment as the first segment.");
          _SegmentDictonary.Add(SegmentPositionCounter, oCurrentSegment);
        }
        SegmentPositionCounter++;
      }
      return _SegmentDictonary;
    }
    private bool ValidateStringRaw(List<string> StringRawList)
    {
      if (StringRawList[0].Substring(0, 3).ToUpper() == Support.Standard.Segments.Msh.Code)
      {
        char MessagesFieldSeparator = Convert.ToChar(StringRawList[0].Substring(3, 1));
        char MessagesComponentSeparator = Convert.ToChar(StringRawList[0].Substring(4, 1));
        char MessagesRepeatSeparator = Convert.ToChar(StringRawList[0].Substring(5, 1));
        char MessagesEscapeSeparator = Convert.ToChar(StringRawList[0].Substring(6, 1));
        char MessagesSubComponentSeparator = Convert.ToChar(StringRawList[0].Substring(7, 1));
        char FirstFieldSeparatorFound = Convert.ToChar(StringRawList[0].Substring(8, 1));
        if (FirstFieldSeparatorFound == MessagesFieldSeparator)
        {
          this.Delimiters = new Support.MessageDelimiters(MessagesFieldSeparator,
                                                            MessagesRepeatSeparator,
                                                            MessagesComponentSeparator,
                                                            MessagesSubComponentSeparator,
                                                            MessagesEscapeSeparator);

          string[] MSHSegmentFields = StringRawList[0].Split(MessagesFieldSeparator);
          if (MSHSegmentFields.Length < 12)
          {
            throw new ArgumentException(String.Format("The passed message's MSH Segment does not have the minimum number of Fields. There must be at least 12 to allow for the Message Version Field"));
          }
        }
        else
        {
          throw new ArgumentException(String.Format("The passed message's defined Field separator at MSH-1: '{0}' \n has not been used as the first Field separator between MSH-2 & MSH-3, found separator of: '{1}'", MessagesFieldSeparator, FirstFieldSeparatorFound));
        }
      }
      else
      {
        throw new ArgumentException(String.Format("The passed message must begin with the Message Header Segment and code: '{0}'", Support.Standard.Segments.Msh.Code));
      }
      return true;
    }
    private bool ValidateSegemntAdditon(Segment oSegment)
    {
      if (oSegment.IsMSH)
      {
        throw new ArgumentException("An MSH Segment can not be added to a Message instance, it must be provided on Message instance creation / instantiation");
      }
      else if (!ValidateDelimiters(oSegment.Delimiters))
      {
        throw new ArgumentException("The Segment instance being added to this partent Message instance has custom delimiters that are different than the parent, this is not allowed");
      }
      else
      {
        return true;
      }
    }
    private void ParseMshSegmentOnly(List<string> MessageList)
    {
      if (ValidateStringRaw(MessageList))
      {
        _SegmentDictonary = new Dictionary<int, Segment>();
        _SegmentDictonary.Add(1, new Segment(MessageList[0]));
        if (_SegmentDictonary[1].Code.ToUpper() != Support.Standard.Segments.Msh.Code)
          throw new ArgumentException(String.Format("The passed message must begin with the Message Header Segment and code: '{0}'", Support.Standard.Segments.Msh.Code));
      }
    }
  }
}

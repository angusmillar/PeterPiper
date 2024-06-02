using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation;

public class Batch : IBatch
{
    private MessageDelimiters _Delimiters;
    private readonly List<Message> _MessageList = new();
    private ISegment _BatchHeader;
    private ISegment _BatchTrailer;

    internal Batch(string stringRaw)
    {
        List<string> BatchSegmentList = stringRaw.Split(Support.Standard.Delimiters.SegmentTerminator).ToList();
        if (string.IsNullOrWhiteSpace(BatchSegmentList.Last()))
        {
            BatchSegmentList.Remove(BatchSegmentList.Last());
        }

        if (!BatchSegmentList.Any())
        {
            throw new PeterPiperException(
                $"The passed batch must begin with the Batch Header Segment and code: '{Support.Standard.Segments.Bhs.Code}'");
        }

        string bhsSegmentStringRaw = BatchSegmentList.First();
        if (Implementation.Message.IsSegmentCode(bhsSegmentStringRaw, Support.Standard.Segments.Bhs.Code))
        {
            this._Delimiters = Implementation.Message.ExtractDelimitersFromStringRaw(bhsSegmentStringRaw);
            _BatchHeader = new Segment(bhsSegmentStringRaw, this._Delimiters, true, null, null);
            BatchSegmentList.Remove(bhsSegmentStringRaw);
        }
        else
        {
            throw new PeterPiperException(
                $"The passed message must begin with the Batch Header Segment and code: '{Support.Standard.Segments.Bhs.Code}'");
        }

        if (Implementation.Message.IsSegmentCode(BatchSegmentList.Last(), Support.Standard.Segments.Bts.Code))
        {
            _BatchTrailer = new Segment(BatchSegmentList.Last(), this._Delimiters, true, null, null);
            BatchSegmentList.Remove(BatchSegmentList.Last());
        }

        List<List<string>> MessageSegmentList = GetMessageSegmentList(BatchSegmentList);
        for (int i = 0; i < MessageSegmentList.Count; i++)
        {
            try
            {
                Message Message = new Message(MessageSegmentList[i]);
                try
                {
                    this.AddMessage(Message);
                }
                catch (Exception exception)
                {
                    throw new PeterPiperException(
                        $"Message {i} in the Batch is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.",
                        exception);
                }
            }
            catch (PeterPiperException peterPiperException)
            {
                throw new PeterPiperException($"Message {i} in the Batch was unable to be parsed.",
                    peterPiperException);
            }
        }
    }

    internal Batch(List<string> segmentStringRawList, MessageDelimiters messageDelimiters)
    {
        this._Delimiters = messageDelimiters;
        List<string> BatchSegmentList = segmentStringRawList;
        if (!BatchSegmentList.Any())
        {
            throw new PeterPiperException(
                $"The passed batch must begin with the Batch Header Segment and code: '{Support.Standard.Segments.Bhs.Code}'");
        }

        string bhsSegmentStringRaw = BatchSegmentList.First();
        if (Implementation.Message.IsSegmentCode(bhsSegmentStringRaw, Support.Standard.Segments.Bhs.Code))
        {
            _BatchHeader = new Segment(bhsSegmentStringRaw, messageDelimiters);
            BatchSegmentList.Remove(bhsSegmentStringRaw);
        }
        else
        {
            throw new PeterPiperException(
                $"The passed message must begin with the Batch Header Segment and code: '{Support.Standard.Segments.Bhs.Code}'");
        }

        if (Implementation.Message.IsSegmentCode(BatchSegmentList.Last(), Support.Standard.Segments.Bts.Code))
        {
            _BatchTrailer = new Segment(BatchSegmentList.Last(), this._Delimiters);
            BatchSegmentList.Remove(BatchSegmentList.Last());
        }

        List<List<string>> MessageSegmentList = GetMessageSegmentList(BatchSegmentList);
        for (int i = 0; i < MessageSegmentList.Count; i++)
        {
            try
            {
                Message Message = new Message(MessageSegmentList[i]);
                try
                {
                    this.AddMessage(Message);
                }
                catch (Exception exception)
                {
                    throw new PeterPiperException(
                        $"Message {i} in the Batch is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.",
                        exception);
                }
            }
            catch (PeterPiperException peterPiperException)
            {
                throw new PeterPiperException($"Message {i} in the Batch was unable to be parsed.",
                    peterPiperException);
            }
        }
    }

    internal Batch(ISegment batchHeaderSegment, List<IMessage> messageList, ISegment batchTrailerSegment)
    {
        if (!Implementation.Message.IsSegmentCode(batchHeaderSegment.Code, Support.Standard.Segments.Bhs.Code))
        {
            throw new PeterPiperException(
                $"The provided Batch Header Segment (BHS) has the incorrect code of {batchHeaderSegment.Code}");
        }

        if (!Implementation.Message.IsSegmentCode(batchTrailerSegment.Code, Support.Standard.Segments.Bts.Code))
        {
            throw new PeterPiperException(
                $"The provided Batch Trailer Segment (BTS) has the incorrect code of {batchTrailerSegment.Code}");
        }

        _Delimiters = batchHeaderSegment.MessageDelimiters as MessageDelimiters;
        _BatchHeader = batchHeaderSegment;

        if (!ValidateDelimiters(batchTrailerSegment.MessageDelimiters))
        {
            throw new PeterPiperException(
                "The provided Batch Trailer Segment (BTS) has different HL7 Delimiters than used by the Batch Header Segment (BHS), this is not allowed.");
        }

        _BatchTrailer = batchTrailerSegment;

        for (int i = 0; i < messageList.Count; i++)
        {
            try
            {
                this.AddMessage(messageList[i]);
            }
            catch (Exception exception)
            {
                throw new PeterPiperException(
                    $"Message {i} in the Batch is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.",
                    exception);
            }
        }
    }

    internal Batch(ISegment batchHeaderSegment, List<IMessage> messageList)
    {
        if (!Implementation.Message.IsSegmentCode(batchHeaderSegment.Code, Support.Standard.Segments.Bhs.Code))
        {
            throw new PeterPiperException(
                $"The provided Batch Header Segment (BHS) has the incorrect code of {batchHeaderSegment.Code}");
        }

        _Delimiters = batchHeaderSegment.MessageDelimiters as MessageDelimiters;
        _BatchHeader = batchHeaderSegment;

        for (int i = 0; i < messageList.Count; i++)
        {
            try
            {
                this.AddMessage(messageList[i]);
            }
            catch (Exception exception)
            {
                throw new PeterPiperException(
                    $"Message {i} in the Batch is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.",
                    exception);
            }
        }
    }

    internal Batch(ISegment batchHeaderSegment)
    {
        if (!Implementation.Message.IsSegmentCode(batchHeaderSegment.Code, Support.Standard.Segments.Bhs.Code))
        {
            throw new PeterPiperException(
                $"The provided Batch Header Segment (BHS) has the incorrect code of {batchHeaderSegment.Code}");
        }

        _Delimiters = batchHeaderSegment.MessageDelimiters as MessageDelimiters;
        BatchHeader = batchHeaderSegment;
    }

    internal Batch()
    {
        _BatchHeader = new Segment(Support.Standard.Segments.Bhs.Code + Support.Standard.Delimiters.Field);
        _Delimiters = BatchHeader.MessageDelimiters as MessageDelimiters;
    }

    public ISegment BatchHeader
    {
        get => _BatchHeader;
        set
        {
            if (!Implementation.Message.IsSegmentCode(value.Code, Support.Standard.Segments.Bhs.Code))
            {
                throw new PeterPiperException(
                    $"The provided Batch Header Segment (BHS) has the incorrect code of {value.Code}");
            }

            if (!ValidateDelimiters(value.MessageDelimiters))
            {
                throw new PeterPiperException(
                    "The provided Batch Header Segment (BHS) has different HL7 Delimiters than used to first construct this Batch object, this is not allowed.");
            }

            _BatchHeader = value;
        }
    }

    public ISegment BatchTrailer
    {
        get => _BatchTrailer;
        set
        {
            if (!ValidateDelimiters(value.MessageDelimiters))
            {
                throw new PeterPiperException(
                    "The provided Batch Trailer Segment (BTS) has different HL7 Delimiters than used by the Batch Header Segment (BHS), this is not allowed.");
            }

            _BatchTrailer = value;
        }
    }


    private bool ValidateDelimiters(IMessageDelimiters delimitersToCompare)
    {
        if (_Delimiters.Field != delimitersToCompare.Field)
            return false;
        if (_Delimiters.Component != delimitersToCompare.Component)
            return false;
        if (_Delimiters.SubComponent != delimitersToCompare.SubComponent)
            return false;
        if (_Delimiters.Repeat != delimitersToCompare.Repeat)
            return false;
        if (_Delimiters.Escape != delimitersToCompare.Escape)
            return false;
        return true;
    }

    private static List<List<string>> GetMessageSegmentList(List<string> batchSegmentList)
    {
        var MessageSegmentList = new List<List<string>>();
        List<string> SegmentList = null;
        foreach (var Segment in batchSegmentList)
        {
            if (SegmentList is null)
            {
                SegmentList = new List<string>();
                if (Implementation.Message.IsSegmentCode(Segment, Support.Standard.Segments.Msh.Code))
                {
                    SegmentList.Add(Segment);
                }
                else
                {
                    throw new PeterPiperException(
                        $"The second Segment of a Batch passed must begin with the Message Header Segment and code: '{Support.Standard.Segments.Msh.Code}'");
                }
            }
            else if (Implementation.Message.IsSegmentCode(Segment, Support.Standard.Segments.Msh.Code))
            {
                MessageSegmentList.Add(SegmentList);
                SegmentList = new List<string>();
                SegmentList.Add(Segment);
            }
            else
            {
                SegmentList.Add(Segment);
            }
        }

        MessageSegmentList.Add(SegmentList);
        return MessageSegmentList;
    }

    internal MessageDelimiters Delimiters
    {
        get => _Delimiters;
        set => _Delimiters = value;
    }

    public string AsString
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(BatchHeader.AsString);
            sb.Append(Support.Standard.Delimiters.SegmentTerminator);
            _MessageList.ForEach(msg => sb.Append(msg.AsString));
            if (BatchTrailer != null)
            {
                sb.AppendLine(BatchTrailer.AsString);
                sb.Append(Support.Standard.Delimiters.SegmentTerminator);
            }

            return sb.ToString();
        }
    }

    public override string ToString()
    {
        return AsString;
    }

    public string AsStringRaw
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(BatchHeader.AsStringRaw);
            sb.Append(Support.Standard.Delimiters.SegmentTerminator);
            _MessageList.ForEach(msg => sb.Append(msg.AsStringRaw));
            if (BatchTrailer != null)
            {
                sb.Append(BatchTrailer.AsStringRaw);
                sb.Append(Support.Standard.Delimiters.SegmentTerminator);
            }

            return sb.ToString();
        }
    }

    public void ClearAll()
    {
        _MessageList.Clear();
        _BatchTrailer = null;
        _BatchHeader.ClearAll();
    }

    public IBatch Clone()
    {
        List<IMessage> ClonedMessageList = new List<IMessage>();
        _MessageList.ForEach(x => ClonedMessageList.Add(x.Clone()));
        return new Batch(batchHeaderSegment: BatchHeader.Clone(), messageList: ClonedMessageList,
            batchTrailerSegment: BatchTrailer.Clone());
    }

    public string EscapeSequence =>
        $"{Delimiters.Component}{Delimiters.Repeat}{Delimiters.Escape}{Delimiters.SubComponent}";

    public string MainSeparator => Delimiters.Field.ToString();

    public IMessageDelimiters MessageDelimiters => _Delimiters;

    public void AddMessage(IMessage item)
    {
        if (ValidateDelimiters(item.MessageDelimiters))
        {
            _MessageList.Add(item as Message);
        }
        else
        {
            throw new PeterPiperException(
                "The Message being added to the Batch is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.");
        }
    }

    public void InsertMessage(int index, IMessage item)
    {
        _MessageList.Insert(index, item as Message);
    }

    public void RemoveMessageAt(int index)
    {
        _MessageList.RemoveAt(index);
    }

    public IMessage Message(int index)
    {
        return _MessageList[index];
    }

    public int MessageCount()
    {
        return _MessageList.Count;
    }

    public ReadOnlyCollection<IMessage> MessageList()
    {
        return _MessageList.Select(i => i as IMessage).ToList().AsReadOnly();
    }
}
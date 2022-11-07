using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation
{
    public class Batch : IBatch
    {
        private MessageDelimiters _Delimiters;
        private readonly List<Message> _MessageList = new List<Message>();
        private ISegment _BatchHeader;
        private ISegment _BatchTrailer;

        internal Batch(string stringRaw)
        {
            List<string> BatchSegmentList = stringRaw.Split(Support.Standard.Delimiters.SegmentTerminator).ToList();
            if (!BatchSegmentList.Any())
            {
                throw new PeterPiperException(String.Format("The passed batch must begin with the Batch Header Segment and code: '{0}'", Support.Standard.Segments.Bhs.Code));
            }

            string bhsSegmentStringRaw = BatchSegmentList.First();
            if (Implementation.Message.IsSegmentCode(bhsSegmentStringRaw, Support.Standard.Segments.Bhs.Code))
            {
                this.Delimiters = Implementation.Message.ExtractDelimitersFromStringRaw(bhsSegmentStringRaw);
                _BatchHeader = new Segment(bhsSegmentStringRaw, this.Delimiters);
                BatchSegmentList.Remove(bhsSegmentStringRaw);
                
            }
            else
            {
                throw new PeterPiperException(String.Format("The passed message must begin with the Batch Header Segment and code: '{0}'", Support.Standard.Segments.Bhs.Code));
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
                    catch (Exception e)
                    {
                        throw new PeterPiperException(String.Format("Message {i} in the Batch is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.", i), e);
                    }
                }
                catch (PeterPiperException peterPiperException)
                {
                    throw new PeterPiperException(String.Format("Message {i} in the Batch was unable to be parsed.", i), peterPiperException);
                }
            }
        }
        internal Batch(List<string> segmentStringRawList, MessageDelimiters messageDelimiters)
        {
            this._Delimiters = messageDelimiters;
            List<string> BatchSegmentList = segmentStringRawList;
            if (!BatchSegmentList.Any())
            {
                throw new PeterPiperException(String.Format("The passed batch must begin with the Batch Header Segment and code: '{0}'", Support.Standard.Segments.Bhs.Code));
            }
            
            string bhsSegmentStringRaw = BatchSegmentList.First();
            if (Implementation.Message.IsSegmentCode(bhsSegmentStringRaw, Support.Standard.Segments.Bhs.Code))
            {
                _BatchHeader = new Segment(bhsSegmentStringRaw, messageDelimiters);
                BatchSegmentList.Remove(bhsSegmentStringRaw);
                
            }
            else
            {
                throw new PeterPiperException(String.Format("The passed message must begin with the Batch Header Segment and code: '{0}'", Support.Standard.Segments.Bhs.Code));
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
                    catch (Exception e)
                    {
                        throw new PeterPiperException(String.Format("Message {i} in the Batch is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.", i), e);
                    }
                }
                catch (PeterPiperException peterPiperException)
                {
                    throw new PeterPiperException(String.Format("Message {i} in the Batch was unable to be parsed.", i), peterPiperException);
                }
            }
        }
        internal Batch(ISegment batchHeaderSegment, List<IMessage> messageList, ISegment batchTrailerSegment)
        {
            if (!Implementation.Message.IsSegmentCode(batchHeaderSegment.Code, Support.Standard.Segments.Bhs.Code))
            {
                throw new PeterPiperException(String.Format("The provided Batch Header Segment (BHS) has the incorrect code of {0}", batchHeaderSegment.Code));
            }

            if (!Implementation.Message.IsSegmentCode(batchTrailerSegment.Code, Support.Standard.Segments.Bts.Code))
            {
                throw new PeterPiperException(String.Format("The provided Batch Trailer Segment (BTS) has the incorrect code of {0}", batchTrailerSegment.Code));
            }

            _Delimiters = batchHeaderSegment.MessageDelimiters as MessageDelimiters;
            _BatchHeader = batchHeaderSegment;
            
            if (!ValidateDelimiters(batchTrailerSegment.MessageDelimiters))
            {
                throw new PeterPiperException("The provided Batch Trailer Segment (BTS) has different HL7 Delimiters than used by the Batch Header Segment (BHS), this is not allowed.");
            }
            _BatchTrailer = batchTrailerSegment;

            for (int i = 0; i < messageList.Count; i++)
            {
                try
                {
                    this.AddMessage(messageList[i]);
                }
                catch (Exception e)
                {
                    throw new PeterPiperException(String.Format("Message {i} in the Batch is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.", i), e);
                }
            }
        }
        internal Batch(ISegment batchHeaderSegment, List<IMessage> messageList)
        {
            if (!Implementation.Message.IsSegmentCode(batchHeaderSegment.Code, Support.Standard.Segments.Bhs.Code))
            {
                throw new PeterPiperException(String.Format("The provided Batch Header Segment (BHS) has the incorrect code of {0}", batchHeaderSegment.Code));
            }

            _Delimiters = batchHeaderSegment.MessageDelimiters as MessageDelimiters;
            _BatchHeader = batchHeaderSegment;

            for (int i = 0; i < messageList.Count; i++)
            {
                try
                {
                    this.AddMessage(messageList[i]);
                }
                catch (Exception e)
                {
                    throw new PeterPiperException(String.Format("Message {i} in the Batch is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.", i), e);
                }
            }
        }
        internal Batch(ISegment batchHeaderSegment)
        {
            if (!Implementation.Message.IsSegmentCode(batchHeaderSegment.Code, Support.Standard.Segments.Bhs.Code))
            {
                throw new PeterPiperException(String.Format("The provided Batch Header Segment (BHS) has the incorrect code of {0}", batchHeaderSegment.Code));
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
                    throw new PeterPiperException(String.Format("The provided Batch Header Segment (BHS) has the incorrect code of {0}", value.Code));
                }

                if (!ValidateDelimiters(value.MessageDelimiters))
                {
                    throw new PeterPiperException("The provided Batch Header Segment (BHS) has different HL7 Delimiters than used to first construct this Batch object, this is not allowed.");
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
                    throw new PeterPiperException("The provided Batch Trailer Segment (BTS) has different HL7 Delimiters than used by the Batch Header Segment (BHS), this is not allowed.");
                }

                _BatchTrailer = value;
            }
        }

        
        private bool ValidateDelimiters(IMessageDelimiters DelimitersToCompaire)
        {
            if (_Delimiters.Field != DelimitersToCompaire.Field)
                return false;
            if (_Delimiters.Component != DelimitersToCompaire.Component)
                return false;
            if (_Delimiters.SubComponent != DelimitersToCompaire.SubComponent)
                return false;
            if (_Delimiters.Repeat != DelimitersToCompaire.Repeat)
                return false;
            if (_Delimiters.Escape != DelimitersToCompaire.Escape)
                return false;
            return true;
        }
        private static List<List<string>> GetMessageSegmentList(List<string> BatchSegmentList)
        {
            var MessageSegmentList = new List<List<string>>();
            List<string> SegmentList = null;
            foreach (var Segment in BatchSegmentList)
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
                        throw new PeterPiperException(String.Format("The second Segment of a Batch passed must begin with the Message Header Segment and code: '{0}'",
                            Support.Standard.Segments.Msh.Code));
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
            get { return _Delimiters; }
            set { _Delimiters = value; }
        }
        public string AsString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(BatchHeader.AsString);
                sb.Append(Support.Standard.Delimiters.SegmentTerminator);
                _MessageList.ForEach(Msg => sb.Append(Msg.AsString));
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
                _MessageList.ForEach(Msg => sb.Append(Msg.AsStringRaw));
                if (BatchTrailer != null)
                {
                    sb.AppendLine(BatchTrailer.AsStringRaw);
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
            _MessageList.ForEach(x => ClonedMessageList.Add(x.Clone() as IMessage));
            return new Batch(batchHeaderSegment: BatchHeader.Clone(), messageList: ClonedMessageList, batchTrailerSegment: BatchTrailer.Clone());
        }

        public string EscapeSequence
        {
            get { return String.Format("{0}{1}{2}{3}", this.Delimiters.Component, this.Delimiters.Repeat, this.Delimiters.Escape, this.Delimiters.SubComponent); }
        }

        public string MainSeparator
        {
            get { return String.Format("{0}", this.Delimiters.Field); }
        }
        
        public IMessageDelimiters MessageDelimiters
        {
            get { return _Delimiters; }
        }
        public void AddMessage(IMessage item)
        {
            if (ValidateDelimiters(item.MessageDelimiters))
            {
                _MessageList.Add(item as Message);
            }
            else
            {
                throw new PeterPiperException("The Message being added to the Batch is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.");
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
}
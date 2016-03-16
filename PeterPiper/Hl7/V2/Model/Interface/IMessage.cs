using System;

namespace PeterPiper.Hl7.V2.Model.Interface
{
  public interface IMessage : IModelBase
  {
    void Add(ISegment item);
    string AsString { get; set; }
    string AsStringRaw { get; set; }
    void ClearAll();
    IMessage Clone();
    string EscapeSequence { get; }
    void Insert(int index, ISegment item);
    bool IsParseMSHSegmentOnly { get; }
    string MainSeparator { get; }
    string MessageControlID { get; }
    DateTimeOffset MessageCreationDateTime { get; }
    IMessageDelimiters MessageDelimiters { get; }
    string MessageStructure { get; }
    string MessageTrigger { get; }
    string MessageType { get; }
    string MessageVersion { get; }
    bool RemoveSegmentAt(int index);
    ISegment Segment(int index);
    ISegment Segment(string Code);
    int SegmentCount();
    int SegmentCount(string Code);
    System.Collections.ObjectModel.ReadOnlyCollection<ISegment> SegmentList();
    System.Collections.ObjectModel.ReadOnlyCollection<ISegment> SegmentList(string Code);
    string ToString();
  }
}

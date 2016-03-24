using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IMessage : IModelBase
  {
    /// <summary>
    /// Add a segment to the end of the message. 
    /// </summary>
    /// <param name="item"></param>
    void Add(ISegment item);
    /// <summary>
    /// Get the whole message as an decoded string (un-escaped), generally not useful! 
    /// </summary>
    string AsString { get; set; }
    /// <summary>
    /// Get the whole message as an encoded string. 
    /// </summary>
    string AsStringRaw { get; set; }
    /// <summary>
    /// Will clear the whole message and leave you with 'MSH|^~\&|'.
    /// </summary>
    void ClearAll();
    /// <summary>
    /// Clone the current messages as a new instance.
    /// </summary>
    /// <returns></returns>
    IMessage Clone();
    /// <summary>
    /// Returns a string containing the escape sequences in use for the message.
    /// defaults to '^~\&' unless a custom escape sequences is given on message creation.
    /// </summary>
    string EscapeSequence { get; }
    /// <summary>
    /// Insert a segment into the message at a given index.
    /// Index is a one based index.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, ISegment item);
    /// <summary>
    /// True if the initial message creation was told to only parse the MSH segment.
    /// False if the whole message was parsed.
    /// </summary>
    bool IsParseMSHSegmentOnly { get; }
    /// <summary>
    /// Returns the Main delimiters in use between fields / Elements.
    /// Defaults as '|' unless a custom escape sequences was given on message creation.
    /// </summary>
    string MainSeparator { get; }
    /// <summary>
    /// Returns the Message Control ID from MSH-10.
    /// </summary>
    string MessageControlID { get; }
    /// <summary>
    /// Returns the Message Creation Date & Time from MSH-7 as a DateTimeOffset.
    /// </summary>
    DateTimeOffset MessageCreationDateTime { get; }
    /// <summary>
    /// Return the IMessageDelimiters instance containing the Message Delimiters in use for the message.  
    /// </summary>
    IMessageDelimiters MessageDelimiters { get; }
    /// <summary>
    /// Returns the Message Structure from MSH-9.3
    /// </summary>
    string MessageStructure { get; }
    /// <summary>
    /// Returns the Message Trigger from MSH-9.2
    /// </summary>
    string MessageTrigger { get; }
    /// <summary>
    /// Returns the Message Type from MSH-9.1
    /// </summary>
    string MessageType { get; }
    /// <summary>
    /// Returns the Message Version Number from MSH-12.1
    /// </summary>
    string MessageVersion { get; }
    /// <summary>
    /// Remove a segment from the message at a given index.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    bool RemoveSegmentAt(int index);
    /// <summary>
    /// Returns the ISegment instance for the given index in the message.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    ISegment Segment(int index);
    /// <summary>
    /// Returns the ISegment instance for first segment found to have the 
    /// given 3 character segment code in the message
    /// </summary>
    /// <param name="Code"></param>
    /// <returns></returns>
    ISegment Segment(string Code);
    /// <summary>
    /// Returns a total count of segments in the message
    /// </summary>
    /// <returns></returns>
    int SegmentCount();
    /// <summary>
    /// Returns a total count of segments in the message with the given segment code
    /// </summary>
    /// <returns></returns>    
    int SegmentCount(string Code);
    /// <summary>
    /// Returns a Read Only Collection of all segments in the message
    /// </summary>
    /// <returns></returns>
    System.Collections.ObjectModel.ReadOnlyCollection<ISegment> SegmentList();
    /// <summary>
    /// Returns a Read Only Collection of all segments in the message with the given segment code
    /// </summary>
    /// <param name="Code"></param>
    /// <returns></returns>
    System.Collections.ObjectModel.ReadOnlyCollection<ISegment> SegmentList(string Code);
    /// <summary>
    /// Get the whole message as an decoded string (un-escaped), generally not useful! 
    /// </summary>
    /// <returns></returns>
    string ToString();
  }
}

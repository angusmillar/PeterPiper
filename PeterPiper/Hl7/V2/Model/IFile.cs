using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IFile 
  {
    /// <summary>
    /// The Batch Header (BHS) Segment. 
    /// </summary>
    /// <param name="item"></param>
    ISegment FileHeader { get; set; }
    
    /// <summary>
    /// The Batch Trailer (BTS) Segment. 
    /// </summary>
    /// <param name="item"></param>
    ISegment FileTrailer { get; set; }
    
    /// <summary>
    /// Add a segment to the end of the message. 
    /// </summary>
    /// <param name="item"></param>
    void Add(IBatch item);
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
    IFile Clone();
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
    void Insert(int index, IBatch item);
    /// <summary>
    /// Returns the Main delimiters in use between fields / Elements.
    /// Defaults as '|' unless a custom escape sequences was given on message creation.
    /// </summary>
    string MainSeparator { get; }
    /// <summary>
    /// Returns the Message Creation Date & Time from MSH-7 as a DateTimeOffset.
    /// </summary>
    DateTimeOffset FileCreationDateTime { get; }
    /// <summary>
    /// Return the IMessageDelimiters instance containing the Message Delimiters in use for the message.  
    /// </summary>
    IMessageDelimiters MessageDelimiters { get; }
    /// <summary>
    /// Remove a segment from the message at a given index.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    bool RemoveBatchAt(int index);
    /// <summary>
    /// Returns the ISegment instance for the given index in the message.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IBatch Batch(int index);
    /// <summary>
    /// Returns the ISegment instance for first segment found to have the 
    /// given 3 character segment code in the message
    /// </summary>
    /// <param name="Code"></param>
    /// <returns></returns>
    IBatch Batch(string Code);
    /// <summary>
    /// Returns a total count of segments in the message
    /// </summary>
    /// <returns></returns>
    int BatchCount();
    
    /// <summary>
    /// Returns a Read Only Collection of all segments in the message
    /// </summary>
    /// <returns></returns>
    System.Collections.ObjectModel.ReadOnlyCollection<IBatch> BatchList();
    
    /// <summary>
    /// Get the whole message as an decoded string (un-escaped), generally not useful! 
    /// </summary>
    /// <returns></returns>
    string ToString();
  }
}

using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IBatch 
  {
    /// <summary>
    /// Get or Set the Batch Header (BHS) Segment. 
    /// </summary>
    /// <param name="item"></param>
    ISegment BatchHeader { get; set; }
    
    /// <summary>
    /// Get or Set the Batch Trailer (BTS) Segment. 
    /// </summary>
    /// <param name="item"></param>
    ISegment BatchTrailer { get; set; }
    
    /// <summary>
    /// Add a message to the end of the batch message list. 
    /// </summary>
    /// <param name="item"></param>
    void AddMessage(IMessage item);
    
    /// <summary>
    /// Get the whole Batch as an decoded string (un-escaped), generally not useful! 
    /// </summary>
    string AsString { get; }
    
    /// <summary>
    /// Get the whole Batch as an HL7 encoded string. 
    /// </summary>
    string AsStringRaw { get; }
    
    /// <summary>
    /// Will clear the whole Batch, all messages, BTS Segment and leaves you with a BHS Segment consisting of 'BHS|^~\&|'.
    /// </summary>
    void ClearAll();
    
    /// <summary>
    /// Clone the current Batch as a new instance.
    /// </summary>
    /// <returns></returns>
    IBatch Clone();
    
    /// <summary>
    /// Returns a string containing the escape sequences in use for the Batch.
    /// defaults to '^~\&' unless a custom escape sequences is given on Batch BHS Segment creation.
    /// </summary>
    string EscapeSequence { get; }
    
    /// <summary>
    /// Insert a message into the Batch at a given index.
    /// Index is a zero based.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void InsertMessage(int index, IMessage item);
    
    /// <summary>
    /// Returns the Main delimiters in use between fields / Elements.
    /// Defaults as '|' unless a custom escape sequences was given on Batch BHS Segment creation.
    /// </summary>
    string MainSeparator { get; }
    
    /// <summary>
    /// Return the IMessageDelimiters instance containing the Message Delimiters in use for the Batch.  
    /// </summary>
    IMessageDelimiters MessageDelimiters { get; }
    
    /// <summary>
    /// Remove a Message from the Batch at a given zero based index.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    void RemoveMessageAt(int index);
    
    /// <summary>
    /// Returns a total count of Messages in the Batch in the message
    /// </summary>
    /// <returns></returns>
    int MessageCount();
    
    /// <summary>
    /// Returns a Read Only Collection of all messages in the batch
    /// </summary>
    /// <returns></returns>
    System.Collections.ObjectModel.ReadOnlyCollection<IMessage> MessageList();
    
    /// <summary>
    /// Get the whole Batch as an decoded string (un-escaped), generally not useful! 
    /// </summary>
    /// <returns></returns>
    string ToString();
  }
}

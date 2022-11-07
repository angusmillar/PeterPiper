using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IFile 
  {
    /// <summary>
    /// Get or Set the File Header (FHS) Segment. 
    /// </summary>
    /// <param name="item"></param>
    ISegment FileHeader { get; set; }
    
    /// <summary>
    /// Get or Set the File Trailer (BTS) Segment. 
    /// </summary>
    /// <param name="item"></param>
    ISegment FileTrailer { get; set; }
    
    /// <summary>
    /// Add a Batch to the end of the File batch list. 
    /// </summary>
    /// <param name="item"></param>
    void AddBatch(IBatch item);
    
    /// <summary>
    /// Get the whole File as an decoded string (un-escaped), generally not useful! 
    /// </summary>
    string AsString { get; }
    
    /// <summary>
    /// Get the whole File as an HL7 encoded string. 
    /// </summary>
    string AsStringRaw { get; }
    
    /// <summary>
    /// Will clear the whole File, all Batches, FTS Segment and leaves you with a FHS Segment consisting of 'FHS|^~\&|'.
    /// </summary>
    void ClearAll();
    
    /// <summary>
    /// Clone the current File as a new instance.
    /// </summary>
    /// <returns></returns>
    IFile Clone();
    
    /// <summary>
    /// Returns a string containing the escape sequences in use for the File.
    /// defaults to '^~\&' unless a custom escape sequences is given on File BHS Segment creation.
    /// </summary>
    string EscapeSequence { get; }
    
    /// <summary>
    /// Insert a Batch into the File at a given index.
    /// Index is a zero based.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void InsertBatch(int index, IBatch item);
    
    /// <summary>
    /// Returns the Main delimiters in use between fields / Elements.
    /// Defaults as '|' unless a custom escape sequences was given on File BHS Segment creation.
    /// </summary>
    string MainSeparator { get; }
    
    /// <summary>
    /// Return the IMessageDelimiters instance containing the Message Delimiters in use for the File.  
    /// </summary>
    IMessageDelimiters MessageDelimiters { get; }
    
    /// <summary>
    /// Remove a Batch from the File at a given zero based index.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    void RemoveBatchAt(int index);
    
    /// <summary>
    /// Returns a total count of Batches in the File
    /// </summary>
    /// <returns></returns>
    int BatchCount();
    
    /// <summary>
    /// Returns a Read Only Collection of all Batches in the File
    /// </summary>
    /// <returns></returns>
    System.Collections.ObjectModel.ReadOnlyCollection<IBatch> BatchList();
    
    /// <summary>
    /// Get the whole Batch as an decoded string (un-escaped), generally not useful! 
    /// </summary>
    /// <returns></returns>
    string ToString();
  }
}

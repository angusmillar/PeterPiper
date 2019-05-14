using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface ISegment : IModelBase
  {
    /// <summary>
    /// Add a element to the end of the segment
    /// </summary>
    /// <param name="item"></param>
    void Add(IElement item);
    /// <summary>
    /// Add a field to the end of the segment
    /// </summary>
    /// <param name="item"></param>
    void Add(IField item);
    /// <summary>
    /// Get the whole segment as an decoded string (un-escaped), generally not useful! 
    /// </summary>
    string AsString { get; set; }
    /// <summary>
    /// Get the whole segment as an encoded string
    /// </summary>
    string AsStringRaw { get; set; }
    /// <summary>
    /// Will clear the whole segment and leave you with just the segment code
    /// </summary>
    void ClearAll();
    /// <summary>
    /// Clone the current segment into a new instance
    /// </summary>
    /// <returns></returns>
    ISegment Clone();
    /// <summary>
    /// Returns the 3 character segment code
    /// </summary>
    string Code { get; }
    /// <summary>
    /// Returns an IElement instance for the given index in the segment
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IElement Element(int index);
    /// <summary>
    /// Returns the total element count in the segment 
    /// </summary>
    int ElementCount { get; }
    /// <summary>
    /// Returns True is the total element count in the segment is greater than 1 
    /// </summary>
    bool HasElements { get; }
    /// <summary>
    /// Returns a Read Only Collection of all elements in the segment
    /// </summary>
    System.Collections.ObjectModel.ReadOnlyCollection<IElement> ElementList { get; }
    /// <summary>
    /// Returns an IField instance for the given index in the segment
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IField Field(int index);
    /// <summary>
    /// Returns the total field count in the segment 
    /// </summary>
    int FieldCount { get; }
    /// <summary>
    /// Returns True is the total field count in the segment is greater than 1 
    /// </summary>
    bool HasFields { get; }
    /// <summary>
    /// Insert a element into the segment at a given index
    /// Index is a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, IElement item);
    /// <summary>
    /// Insert a field into the segment at a given index
    /// Index is a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, IField item);
    /// <summary>
    /// True if the segment contains no elements or fields, it will have a segment code
    /// False if the segment contains one or more elements or fields
    /// </summary>
    bool IsEmpty { get; }
    /// <summary>
    /// Return an IMessageDelimiters instance containing the Message Delimiters in use for the segment.  
    /// </summary>
    IMessageDelimiters MessageDelimiters { get; }
    /// <summary>
    /// Remove a element from the segment at a given index
    /// Uses a one based index    
    /// </summary>
    /// <param name="index"></param>
    void RemoveElementAt(int index);
    /// <summary>
    /// Remove a field from the segment at a given index
    /// Uses a one based index    
    /// </summary>
    /// <param name="index"></param>
    void RemoveFieldAt(int index);
    /// <summary>
    /// Get the whole segment as an decoded string (un-escaped), generally not useful! 
    /// </summary>
    /// <returns></returns>
    string ToString();
  }
}

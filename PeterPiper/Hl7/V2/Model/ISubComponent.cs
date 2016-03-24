using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface ISubComponent : IContentBase
  {
    /// <summary>
    /// Add a content to the end of the subcomponent.
    /// </summary>
    /// <param name="item"></param>
    void Add(IContent item);
    /// <summary>
    /// Get the whole subcomponent as an decoded string (human readable).     
    /// </summary>
    string AsString { get; set; }
    /// <summary>
    /// Get the whole subcomponent as an encoded string (as seen in the message).
    /// </summary>
    string AsStringRaw { get; set; }
    /// <summary>
    /// Will clear the whole subcomponent.
    /// </summary>
    void ClearAll();
    /// <summary>
    /// Clone the current subcomponent into a new instance.
    /// </summary>
    /// <returns></returns>
    ISubComponent Clone();
    /// <summary>
    /// Returns the content instance at the given index of the subcomponent.
    /// Content always uses a zero based index.    
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IContent Content(int index);
    /// <summary>
    /// Returns the total content count of the subcomponent. 
    /// </summary>
    int ContentCount { get; }
    /// <summary>
    /// Returns a collection of all Content in the subcomponent.
    /// </summary>
    System.Collections.ObjectModel.ReadOnlyCollection<IContent> ContentList { get; }
    /// <summary>
    /// Insert content at the given index into the subcomponent.
    /// Content always uses a zero based index.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, IContent item);
    /// <summary>
    /// True if the subcomponent contains no content.
    /// False if the subcomponent contains content.
    /// </summary>
    bool IsEmpty { get; }
    /// <summary>
    /// True if the subcomponent contains HL7 Null only |""|
    /// False if the subcomponent does not contain HL7 Null only |""|
    /// </summary>
    bool IsHL7Null { get; }
    /// <summary>
    /// Return the IMessageDelimiters instance containing the Message Delimiters in use for the subcomponent.  
    /// </summary>
    IMessageDelimiters MessageDelimiters { get; }
    /// <summary>
    /// Remove the content at the given index from the subcomponent.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    void RemoveContentAt(int index);
    /// <summary>
    /// Set / overwrite, the content from the subcomponent.
    /// Content always uses a zero based index.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Set(int index, IContent item);
    /// <summary>
    /// Get the whole subcomponent as an decoded string (human readable).
    /// </summary>
    /// <returns></returns>
    string ToString();
  }
}

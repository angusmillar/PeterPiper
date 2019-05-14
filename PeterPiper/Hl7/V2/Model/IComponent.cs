using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IComponent : IContentBase
  {    
    /// <summary>
    /// Add a content to the end of the first subcomponent of the component. 
    /// </summary>
    /// <param name="item"></param>
    void Add(IContent item);
    /// <summary>
    /// Add a subcomponent to the end of the component. 
    /// </summary>
    /// <param name="item"></param>
    void Add(ISubComponent item);
    /// <summary>
    /// Get the whole component as an decoded string (human readable).     
    /// </summary>
    string AsString { get; set; }
    /// <summary>
    /// Get the whole component as an encoded string (as seen in the message).
    /// </summary>
    string AsStringRaw { get; set; }
    /// <summary>
    /// Will clear the whole component.
    /// </summary>
    void ClearAll();
    /// <summary>
    /// Clone the current component into a new instance.
    /// </summary>
    /// <returns></returns>
    IComponent Clone();
    /// <summary>
    /// Returns the content instance at the given index of the first subcomponent of the component. 
    /// Content always uses a zero based index.    
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IContent Content(int index);
    /// <summary>
    /// Returns the total content count of the first subcomponent of the component. 
    /// </summary>
    int ContentCount { get; }
    /// <summary>
    /// Returns True if the total content count of the first subcomponent of the component is greater than 1. 
    /// </summary>
    bool HasContents { get; }
    /// <summary>
    /// Insert content at the given index into the first subcomponent of the component.
    /// Content always uses a zero based index.
    /// /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, IContent item);
    /// <summary>
    /// Insert a subcomponent at the given index of the component.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, ISubComponent item);
    /// <summary>
    /// True if the component contains no content.
    /// False if the component contains content.
    /// </summary>
    bool IsEmpty { get; }
    /// <summary>
    /// True if the component contains HL7 Null only |""|
    /// False if the component does not contain HL7 Null only |""|
    /// </summary>
    bool IsHL7Null { get; }
    /// <summary>
    /// Return the IMessageDelimiters instance containing the Message Delimiters in use for the component.  
    /// </summary>
    IMessageDelimiters MessageDelimiters { get; }
    /// <summary>
    /// Remove the content at the given index from the first subcomponent of the component.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    void RemoveContentAt(int index);
    /// <summary>
    /// Remove the subcomponent at the given index from the component.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    void RemoveSubComponentAt(int index);
    /// <summary>
    /// Set / overwrite, the content from the first subcomponent of the component.
    /// Content always uses a zero based index.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Set(int index, IContent item);
    /// <summary>
    /// Set / overwrite, the subcomponent at the given index of the component.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Set(int index, ISubComponent item);
    /// <summary>
    /// Returns the subcomponent instance at the given index of the component.
    /// Uses a one based index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    ISubComponent SubComponent(int index);
    /// <summary>
    /// Returns the total subcomponent count of the component.
    /// </summary>
    int SubComponentCount { get; }
    /// <summary>
    /// Returns True if the total subcomponent count of the component is greater than 1.
    /// </summary>
    bool HasSubComponents { get; }
    /// <summary>
    /// Returns a collection of all subcomponents in the component.
    /// </summary>
    System.Collections.ObjectModel.ReadOnlyCollection<ISubComponent> SubComponentList { get; }
    /// <summary>
    /// Get the whole field as an decoded string (human readable).
    /// </summary>
    /// <returns></returns>
    string ToString();
  }
}

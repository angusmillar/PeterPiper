using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IField : IContentBase
  {
    /// <summary>
    /// Add a component to the end of the field
    /// </summary>
    /// <param name="item"></param>
    void Add(IComponent item);
    /// <summary>
    /// Add a content to the first component's first subcomponent of the field
    /// </summary>
    /// <param name="item"></param>
    void Add(IContent item);
    /// <summary>
    /// Add a subcomponent to the first component of the field
    /// </summary>
    /// <param name="item"></param>
    void Add(ISubComponent item);
    /// <summary>
    /// Get the whole field as an decoded string (human readable)     
    /// </summary>
    string AsString { get; set; }
    /// <summary>
    /// Get the whole field as an encoded string (as seen in the message)
    /// </summary>
    string AsStringRaw { get; set; }
    /// <summary>
    /// Will clear the whole field
    /// </summary>
    void ClearAll();
    /// <summary>
    /// Clone the current field into a new instance
    /// </summary>
    /// <returns></returns>
    IField Clone();
    /// <summary>
    /// Returns the component instance at the given index in the field
    /// Uses a one based index   
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IComponent Component(int index);
    /// <summary>
    /// Returns the total component count of the field
    /// </summary>
    int ComponentCount { get; }
    /// <summary>
    /// Returns a collection of all components in the field
    /// </summary>
    System.Collections.ObjectModel.ReadOnlyCollection<IComponent> ComponentList { get; }
    /// <summary>
    /// Returns the content instance at the given index of the first component's first subcomponent of the field
    /// Content always uses a zero based index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IContent Content(int index);
    /// <summary>
    /// Returns the total content count of the first component's first subcomponent of the field
    /// </summary>
    int ContentCount { get; }
    /// <summary>
    /// Insert a component at the given index into the field
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, IComponent item);
    /// <summary>
    /// Insert content at the given index into the first component's first subcomponent of the field
    /// Content always uses a zero based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, IContent item);
    /// <summary>
    /// Insert a subcomponent at the given index into the first component of the field
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, ISubComponent item);
    /// <summary>
    /// True if the element contains no content
    /// False if the element contains content
    /// </summary>
    bool IsEmpty { get; }
    /// <summary>
    /// True if the field contains HL7 Null only |""|
    /// False if the field does not contain HL7 Null only |""|
    /// </summary>
    bool IsHL7Null { get; }
    /// <summary>
    /// Return the IMessageDelimiters instance containing the Message Delimiters in use for the field.  
    /// </summary>
    IMessageDelimiters MessageDelimiters { get; }
    /// <summary>
    /// Remove the component at the given index from the field
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    void RemoveComponentAt(int index);
    /// <summary>
    /// Remove the content at the given index from the first component's first subcomponent of the field
    /// Content always uses a zero based index
    /// </summary>
    /// <param name="index"></param>
    void RemoveContentAt(int index);
    /// <summary>
    /// Remove the subcomponent at the given index from the first component of the field
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    void RemoveSubComponentAt(int index);
    /// <summary>
    /// <summary>
    /// Set / overwrite, the component at the given index of the field
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Set(int index, IComponent item);
    /// <summary>
    /// Set / overwrite, the content from the first component's first subcomponent of the field
    /// Content always uses a zero based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Set(int index, IContent item);
    /// <summary>
    /// Set / overwrite, the subcomponent at the given index from the first component of the field
    /// Uses a one based index   
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Set(int index, ISubComponent item);
    /// <summary>
    /// Returns the subcomponent instance at the given index from the first component of the field
    /// Uses a one based index   
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    ISubComponent SubComponent(int index);
    /// <summary>
    /// Returns the total subcomponent count from the first component of the field
    /// </summary>
    int SubComponentCount { get; }
    /// <summary>
    /// Get the whole field as an decoded string (human readable)     
    /// </summary>
    /// <returns></returns>
    string ToString();
  }
}

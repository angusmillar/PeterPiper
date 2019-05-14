using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IElement : IContentBase
  {
    /// <summary>
    /// Add a component to the end of the first field repeat in the element 
    /// </summary>
    /// <param name="item"></param>
    void Add(IComponent item);
    /// <summary>
    /// Add a content to the end of the first component's first subcomponent of the first repeat field in the element
    /// </summary>
    /// <param name="item"></param>
    void Add(IContent item);
    /// <summary>
    /// Add a field to the end of the elements's field repeats
    /// </summary>
    /// <param name="item"></param>
    void Add(IField item);
    /// <summary>
    /// Add a subcomponent to the end of first component of the first repeat field in the element
    /// </summary>
    /// <param name="item"></param>
    void Add(ISubComponent item);
    /// <summary>
    /// Get the whole element as an decoded string (human readable) 
    /// </summary>
    string AsString { get; set; }
    /// <summary>
    /// Get the whole element as an encoded string (as seen in the message)
    /// </summary>
    string AsStringRaw { get; set; }
    /// <summary>
    /// Will clear the whole element
    /// </summary>
    void ClearAll();
    /// <summary>
    /// Clone the current element into a new instance
    /// </summary>
    /// <returns></returns>
    IElement Clone();
    /// <summary>
    /// Returns the component instance at the given index of the first field repeat in the element
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IComponent Component(int index);
    /// <summary>
    /// Returns the total component count of the first field repeat in the element
    /// </summary>
    int ComponentCount { get; }
    /// <summary>
    /// Returns the True is the total component count of the first field repeat in the element is greater than zero
    /// </summary>
    bool HasComponents { get; }
    /// <summary>
    /// Returns the content instance at the given index of the first component's first subcomponent of the first field repeat in the element
    /// Content always uses a zero based index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IContent Content(int index);
    /// <summary>
    /// Returns the total content count of the first component's first subcomponent of the first field repeat in the element
    /// </summary>
    int ContentCount { get; }
    /// <summary>
    /// Returns True if the total content count of the first component's first subcomponent of the first field repeat in the element is greater than 1
    /// </summary>
    bool HasContents { get; }
    /// <summary>
    /// Insert a component at the given index into the first repeat in the element
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, IComponent item);
    /// <summary>
    /// Insert content at the given index into the first component's first subcomponent of the first field repeat in the element
    /// Content always uses a zero based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, IContent item);
    /// <summary>
    /// Insert a field repeat at the given index into the element
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, IField item);
    /// <summary>
    /// Insert a subcomponent at the given index into the first component of the first repeat field in the element
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Insert(int index, ISubComponent item);
    /// <summary>
    /// True if the element contains no content
    /// False if the segment contains content
    /// </summary>
    bool IsEmpty { get; }
    /// <summary>
    /// True if the element contains HL7 Null only |""|
    /// False if the segment does not contain HL7 Null only |""|
    /// </summary>
    bool IsHL7Null { get; }
    /// <summary>
    /// Return the IMessageDelimiters instance containing the Message Delimiters in use for the element.  
    /// </summary>
    IMessageDelimiters MessageDelimiters { get; }
    /// <summary>
    /// Remove the component at the given index from the first repeat in the element
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    void RemoveComponentAt(int index);
    /// <summary>
    /// Remove the content at the given index from the first component's first subcomponent of the first field repeat in the element
    /// Content always uses a zero based index
    /// </summary>
    /// <param name="index"></param>
    void RemoveContentAt(int index);
    /// <summary>
    /// Remove the field repeat at the given index from the element
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    void RemoveRepeatAt(int index);
    /// <summary>
    /// Remove the subcomponent at the given index from the first component of the first repeat field in the element
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    void RemoveSubComponentAt(int index);
    /// <summary>
    /// Returns the field repeat instance at the given index in the element
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IField Repeat(int index);
    /// <summary>
    /// Returns the total field repeat count of the element
    /// </summary>
    int RepeatCount { get; }
    /// <summary>
    /// Returns the total field repeat count of the element
    /// </summary>
    bool HasRepeats { get; }
    /// <summary>
    /// Returns a collection of all repeat fields in the element
    /// </summary>
    System.Collections.ObjectModel.ReadOnlyCollection<IField> RepeatList { get; }
    /// <summary>
    /// Set / overwrite, the content at the given index from the first component's first subcomponent of the first field repeat in the element
    /// Content always uses a zero based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Set(int index, IContent item);
    /// <summary>
    /// Set / overwrite, the subcomponent at the given index from the first component of the first repeat field in the element
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    void Set(int index, ISubComponent item);
    /// <summary>
    /// Returns the subcomponent instance at the given index from the first component of the first repeat field in the element
    /// Uses a one based index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    ISubComponent SubComponent(int index);
    /// <summary>
    /// Returns the total subcomponent count from the first component of the first repeat field in the element
    /// </summary>
    int SubComponentCount { get; }
    /// <summary>
    /// Returns the True is the total SubComponent count of the first field repeat in the element is greater than zero
    /// </summary>
    bool HasSubComponents { get; }
    /// <summary>
    /// Get the whole element as an decoded string (human readable) 
    /// </summary>
    /// <returns></returns>
    string ToString();
  }
}

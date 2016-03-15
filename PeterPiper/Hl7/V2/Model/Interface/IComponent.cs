using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IComponent : IContentBase
  {    
    void Add(IContent item);
    void Add(ISubComponent item);
    string AsString { get; set; }
    string AsStringRaw { get; set; }
    void ClearAll();
    IComponent Clone();
    IContent Content(int index);
    int ContentCount { get; }
    void Insert(int index, IContent item);
    void Insert(int index, ISubComponent item);
    bool IsEmpty { get; }
    bool IsHL7Null { get; }
    PeterPiper.Hl7.V2.Support.MessageDelimiters MessageDelimiters { get; }
    void RemoveContentAt(int index);
    void RemoveSubComponentAt(int index);
    void Set(int index, IContent item);
    void Set(int index, ISubComponent item);
    ISubComponent SubComponent(int index);
    int SubComponentCount { get; }
    System.Collections.ObjectModel.ReadOnlyCollection<ISubComponent> SubComponentList { get; }
    string ToString();
  }
}

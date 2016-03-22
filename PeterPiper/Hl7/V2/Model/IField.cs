using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IField : IContentBase
  {
    void Add(IComponent item);
    void Add(IContent item);
    void Add(ISubComponent item);
    string AsString { get; set; }
    string AsStringRaw { get; set; }
    void ClearAll();
    IField Clone();
    IComponent Component(int index);
    int ComponentCount { get; }
    System.Collections.ObjectModel.ReadOnlyCollection<IComponent> ComponentList { get; }
    IContent Content(int index);
    int ContentCount { get; }
    void Insert(int index, IComponent item);
    void Insert(int index, IContent item);
    void Insert(int index, ISubComponent item);
    bool IsEmpty { get; }
    bool IsHL7Null { get; }
    IMessageDelimiters MessageDelimiters { get; }
    void RemoveComponentAt(int index);
    void RemoveContentAt(int index);
    void RemoveSubComponentAt(int index);
    void Set(int index, IComponent item);
    void Set(int index, IContent item);
    void Set(int index, ISubComponent item);
    ISubComponent SubComponent(int index);
    int SubComponentCount { get; }
    string ToString();
  }
}

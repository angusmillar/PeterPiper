using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IElement : IContentBase
  {
    void Add(IComponent item);
    void Add(IContent item);
    void Add(IField item);
    void Add(ISubComponent item);
    string AsString { get; set; }
    string AsStringRaw { get; set; }
    void ClearAll();
    IElement Clone();
    IComponent Component(int index);
    int ComponentCount { get; }
    IContent Content(int index);
    int ContentCount { get; }
    void Insert(int index, IComponent item);
    void Insert(int index, IContent item);
    void Insert(int index, IField item);
    void Insert(int index, ISubComponent item);
    bool IsEmpty { get; }
    bool IsHL7Null { get; }
    IMessageDelimiters MessageDelimiters { get; }
    void RemoveComponentAt(int index);
    void RemoveContentAt(int index);
    void RemoveRepeatAt(int index);
    void RemoveSubComponentAt(int index);
    IField Repeat(int index);
    int RepeatCount { get; }
    System.Collections.ObjectModel.ReadOnlyCollection<IField> RepeatList { get; }
    void Set(int index, IContent item);
    void Set(int index, ISubComponent item);
    ISubComponent SubComponent(int index);
    int SubComponetCount { get; }
    string ToString();
  }
}

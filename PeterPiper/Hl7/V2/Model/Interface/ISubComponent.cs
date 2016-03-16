using System;

namespace PeterPiper.Hl7.V2.Model.Interface
{
  public interface ISubComponent : IContentBase
  {
    void Add(IContent item);
    string AsString { get; set; }
    string AsStringRaw { get; set; }
    void ClearAll();
    ISubComponent Clone();
    IContent Content(int index);
    int ContentCount { get; }
    System.Collections.ObjectModel.ReadOnlyCollection<IContent> ContentList { get; }
    void Insert(int index, IContent item);
    bool IsEmpty { get; }
    bool IsHL7Null { get; }
    IMessageDelimiters MessageDelimiters { get; }
    void RemoveContentAt(int index);
    void Set(int index, IContent item);
    string ToString();
  }
}

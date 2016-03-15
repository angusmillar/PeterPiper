using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface ISegment : IModelBase
  {
    void Add(IElement item);
    void Add(IField item);
    string AsString { get; set; }
    string AsStringRaw { get; set; }
    void ClearAll();
    ISegment Clone();
    string Code { get; }
    IElement Element(int index);
    int ElementCount { get; }
    System.Collections.ObjectModel.ReadOnlyCollection<IElement> ElementList { get; }
    IField Field(int index);
    int FieldCount { get; }
    void Insert(int index, IElement item);
    void Insert(int index, IField item);
    bool IsEmpty { get; }
    PeterPiper.Hl7.V2.Support.MessageDelimiters MessageDelimiters { get; }
    void RemoveElementAt(int index);
    void RemoveFieldAt(int index);
    string ToString();
  }
}

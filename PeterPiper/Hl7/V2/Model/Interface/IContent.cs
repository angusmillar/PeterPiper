using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IContent : IContentBase
  {
    string AsString { get; set; }
    string AsStringRaw { get; set; }
    void ClearAll();
    IContent Clone();
    PeterPiper.Hl7.V2.Support.Content.ContentType ContentType { get; set; }
    PeterPiper.Hl7.V2.Support.Content.EscapeData EscapeMetaData { get; }
    bool IsEmpty { get; }
    bool IsHL7Null { get; }
    PeterPiper.Hl7.V2.Support.MessageDelimiters MessageDelimiters { get; }
    string ToString();
  }
}

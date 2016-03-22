using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IEscapeData
  {
    PeterPiper.Hl7.V2.Support.Standard.EscapeType EscapeType { get; }
    string EscapeTypeCharater { get; }
    bool IsFormattingCommand { get; }
    string MetaData { get; }
  }
}

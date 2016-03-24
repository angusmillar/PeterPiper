using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IEscapeData
  {
    /// <summary>
    /// Returns an enum indicating the type of escape found 
    /// </summary>
    PeterPiper.Hl7.V2.Support.Standard.EscapeType EscapeType { get; }
    /// <summary>
    /// Returns the first character of the escape sequence 
    /// </summary>
    string EscapeTypeCharater { get; }
    /// <summary>
    /// Some escapes are purely for formatting text, e.g (HighlightOn, NewLine, WordWrapOn)
    /// Return true when these the escape is one of these. 
    /// </summary>
    bool IsFormattingCommand { get; }
    /// <summary>
    /// Returns the escape sequence minus the first character, used in multi-byte character set escape sequences
    /// </summary>
    string MetaData { get; }
  }
}

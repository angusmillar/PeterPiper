using System;

namespace PeterPiper.Hl7.V2.Model
{
  public interface IContent : IContentBase
  {
    /// <summary>
    /// Get the content as an decoded string (human readable).     
    /// </summary>
    string AsString { get; set; }
    /// <summary>
    /// Get the content as an encoded string (as seen in the message).
    /// </summary>
    string AsStringRaw { get; set; }
    /// <summary>
    /// Will clear the whole content.
    /// </summary>
    void ClearAll();
    /// <summary>
    /// Clone the current content into a new instance.
    /// </summary>
    /// <returns></returns>
    IContent Clone();
    /// <summary>
    /// Return the Content Type of (Escape or Text). 
    /// </summary>
    PeterPiper.Hl7.V2.Support.Content.ContentType ContentType { get; set; }
    /// <summary>
    /// Returns the Meta Data about the escape, useful for custom escapes.
    /// Will return null is content is of ContentType = Text 
    /// </summary>
    IEscapeData EscapeMetaData { get; }
    /// <summary>
    /// True if the content contains no content.
    /// False if the content contains content.
    /// </summary>
    bool IsEmpty { get; }
    /// <summary>
    /// True if the content contains HL7 Null only |""|
    /// False if the content does not contain HL7 Null only |""|
    /// </summary>
    bool IsHL7Null { get; }
    /// <summary>
    /// Return the IMessageDelimiters instance containing the Message Delimiters in use for the content. 
    /// </summary>
    IMessageDelimiters MessageDelimiters { get; }
    /// <summary>
    /// Get the content as an decoded string (human readable).     
    /// </summary>
    /// <returns></returns>
    string ToString();
  }
}

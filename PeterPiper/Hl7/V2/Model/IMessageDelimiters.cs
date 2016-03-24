using System;
namespace PeterPiper.Hl7.V2.Model
{
  public interface IMessageDelimiters
  {
    /// <summary>
    /// Returns the character used as the Component delimiter 
    /// </summary>
    char Component { get; }
    /// <summary>
    /// Returns the character used as the Escape delimiter 
    /// </summary>
    char Escape { get; }
    /// <summary>
    /// Returns the character used as the Field delimiter 
    /// </summary>
    char Field { get; }
    /// <summary>
    /// Returns the character used as the Repeat delimiter 
    /// </summary>
    char Repeat { get; }
    /// <summary>
    /// Returns the character used as the SubComponent delimiter 
    /// </summary>
    char SubComponent { get; }
  }
}

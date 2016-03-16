using System;
namespace PeterPiper.Hl7.V2.Model.Interface
{
  public interface IMessageDelimiters
  {
    char Component { get; }
    char Escape { get; }
    char Field { get; }
    char Repeat { get; }
    char SubComponent { get; }
  }
}

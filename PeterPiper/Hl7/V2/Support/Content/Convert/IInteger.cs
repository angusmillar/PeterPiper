using System;
namespace PeterPiper.Hl7.V2.Support.Content.Convert
{
  public interface IInteger
  {
    int Int { get; }
    Int16 Int16 { get; }
    Int32 Int32 { get; }
    Int64 Int64 { get; }
    bool IsNumeric { get; }
    string AsString();
  }
}
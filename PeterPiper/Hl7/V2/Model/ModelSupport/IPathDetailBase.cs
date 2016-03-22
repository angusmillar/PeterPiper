using System;
namespace PeterPiper.Hl7.V2.Model.ModelSupport
{
  public interface IPathDetailBase
  {
    string ComponentCount { get; }
    string ComponentPosition { get; }
    string ContentPosition { get; }
    string FieldPosition { get; }
    string MessageEvent { get; }
    string MessageType { get; }
    string MessageVersion { get; }
    global::PeterPiper.Hl7.V2.Schema.Model.VersionsSupported MessageVersionSupported { get; }
    string PathBrief { get; }
    string PathVerbos { get; }
    string RepeatCount { get; }
    int RepeatCountInteger { get; }
    string RepeatPosition { get; }
    string SegmentCode { get; }
    string SegmentPosition { get; }
    string SegmentTypePosition { get; }
    string SubComponentCount { get; }
    string SubComponentPosition { get; }
  }
}

using System;
namespace PeterPiper.Hl7.V2.Model.ModelSupport
{
  public interface IPathDetailBase
  {
    /// <summary>
    /// How many components in this instance
    /// </summary>
    string ComponentCount { get; }
    /// <summary>
    /// The position of Component in this instance 
    /// </summary>
    string ComponentPosition { get; }
    /// <summary>
    /// The position of Content in this instance 
    /// </summary>
    string ContentPosition { get; }
    /// <summary>
    /// The position of Field in this instance 
    /// </summary>
    string FieldPosition { get; }
    /// <summary>
    /// The Message event for this instance from (MSH-9.2)
    /// </summary>
    string MessageEvent { get; }
    /// <summary>
    /// The Message type for this instance from (MSH-9.1)
    /// </summary>
    string MessageType { get; }
    /// <summary>
    /// The Message Version for this instance from (MSH-12.1)
    /// </summary>
    string MessageVersion { get; }
    /// <summary>
    /// An enum of the HL7 V2.x schema's that are supported by Peter Piper 
    /// </summary>
    global::PeterPiper.Hl7.V2.Schema.Model.VersionsSupported MessageVersionSupported { get; }
    /// <summary>
    /// Returns a short string syntax that expresses the location of this instance in a message.
    /// e.g "OBX-11.3.2 {Rpt: 2}"
    /// </summary>
    string PathBrief { get; }
    /// <summary>
    /// Returns a long string syntax that expresses the location of this instance in a message.
    /// e.g "Segment: OBX, Field: 11, {Repeat: 2, Component: 3, SubComponent: 2 }"
    /// </summary>
    string PathVerbos { get; }
    /// <summary>
    /// How many Field repeats in this instance, as a string 
    /// </summary>
    string RepeatCount { get; }
    /// <summary>
    /// How many Field repeats in this instance as a integer
    /// </summary>
    int RepeatCountInteger { get; }
    /// <summary>
    /// The position of Field Repeat in this instance 
    /// </summary>
    string RepeatPosition { get; }
    /// <summary>
    /// The Segment Code of this instance
    /// </summary>
    string SegmentCode { get; }
    /// <summary>
    /// The index position of Segment for this instance 
    /// </summary>
    string SegmentPosition { get; }
    /// <summary>
    /// ?? Forgotten what this is 
    /// </summary>
    string SegmentTypePosition { get; }
    /// <summary>
    /// How count of how many SubComponents in this instance.
    /// </summary>
    string SubComponentCount { get; }
    /// <summary>
    /// The position of the SubComponents in this instance 
    /// </summary>
    string SubComponentPosition { get; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model.Implementation;

namespace PeterPiper.Hl7.V2.Model
{
  public static class Creator
  {
    public static IMessage Message(ISegment Segment)
    {
      return new Message(Segment);
    }

    public static IMessage Message(String StringRaw, bool ParseMSHSegmentOnly = false)
    {
      return new Message(StringRaw, ParseMSHSegmentOnly);
    }

    public static IMessage Message(List<String> Collection, bool ParseMSHSegmentOnly = false)
    {
      return new Message(Collection, ParseMSHSegmentOnly);
    }

    public static IMessage Message(string MessageVersion, string MessageType, string MessageTrigger, string MessageControlID = "<GUID>", string MessageStructure = "")
    {
      return new Message(MessageVersion, MessageType, MessageTrigger, MessageControlID, MessageStructure);
    }

    public static ISegment Segment(String StringRaw)
    {
      return new Segment(StringRaw);
    }

    public static ISegment Segment(String StringRaw, Support.MessageDelimiters CustomDelimiters)
    {
      return new Segment(StringRaw, CustomDelimiters);
    }

    public static IElement Element()
    {
      return new Element();
    }

    public static IElement Element(String StringRaw)
    {
      return new Element(StringRaw);
    }

    public static IElement Element(Support.MessageDelimiters CustomDelimiters)
    {
      return new Element(CustomDelimiters);
    }

    public static IElement Element(String StringRaw, Support.MessageDelimiters CustomDelimiters)
    {
      return new Element(StringRaw, CustomDelimiters);
    }

    public static IField Field()
    {
      return new Field();
    }

    public static IField Field(string StringRaw)
    {
      return new Field(StringRaw);
    }

    public static IField Field(Support.MessageDelimiters CustomDelimiters)
    {
      return new Field(CustomDelimiters);
    }

    public static IField Field(string StringRaw, Support.MessageDelimiters CustomDelimiters)
    {
      return new Field(StringRaw, CustomDelimiters);
    }

    public static IComponent Component()
    {
      return new Component();
    }

    public static IComponent Component(string StringRaw)
    {
      return new Component(StringRaw);
    }

    public static IComponent Component(Support.MessageDelimiters CustomDelimiters)
    {
      return new Component(CustomDelimiters);
    }

    public static IComponent Component(string StringRaw, Support.MessageDelimiters CustomDelimiters)
    {
      return new Component(StringRaw, CustomDelimiters);
    }

    public static ISubComponent SubComponent()
    {
      return new SubComponent();
    }

    public static ISubComponent SubComponent(string StringRaw)
    {
      return new SubComponent(StringRaw);
    }

    public static ISubComponent SubComponent(Support.MessageDelimiters CustomDelimiters)
    {
      return new SubComponent(CustomDelimiters);
    }

    public static ISubComponent SubComponent(string StringRaw, Support.MessageDelimiters CustomDelimiters)
    {
      return new SubComponent(StringRaw, CustomDelimiters);
    }

    public static IContent Content(string String)
    {
      return new Content(String);
    }

    public static IContent Content(string String, Support.Content.ContentType ContentType)
    {
      return new Content(String, ContentType);
    }

    public static IContent Content(string String, Support.Content.ContentType ContentType, Support.MessageDelimiters CustomDelimiters)
    {
      return new Content(String, ContentType, CustomDelimiters);
    }

    public static IContent Content(string String, Support.MessageDelimiters CustomDelimiters)
    {
      return new Content(String, CustomDelimiters);
    }

    public static IContent Content(Support.Standard.EscapeType EscapeType)
    {
      return new Content(EscapeType);
    }

    public static IContent Content(Support.Content.EscapeData EscapeMetaData)
    {
      return new Content(EscapeMetaData);
    }

    public static IContent Content(Support.Standard.EscapeType EscapeType, Support.MessageDelimiters CustomDelimiters)
    {
      return new Content(EscapeType, CustomDelimiters);
    }

  }
}

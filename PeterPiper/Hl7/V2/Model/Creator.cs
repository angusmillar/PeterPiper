using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model.Implementation;
using PeterPiper.Hl7.V2.Model;

namespace PeterPiper.Hl7.V2.Model
{
  public static class Creator
  {
    
    public static IFile File(ISegment FileHeaderSegment, List<IBatch> BatchList, ISegment FileTrailerSegment)
    {
      return new File(FileHeaderSegment, BatchList, FileTrailerSegment);
    }
    
    public static IFile File(ISegment FileHeaderSegment, List<IBatch> BatchList)
    {
      return new File(FileHeaderSegment, BatchList);
    }
    
    public static IFile File(ISegment FileHeaderSegment)
    {
      return new File(FileHeaderSegment);
    }
    
    public static IFile File(string StringRaw)
    {
      return new File(StringRaw);
    }
    
    public static IFile File()
    {
      return new File();
    }
    
    public static IBatch Batch(ISegment BatchHeaderSegment, List<IMessage> MessageList, ISegment BatchTrailerSegment)
    {
      return new Batch(BatchHeaderSegment, MessageList, BatchTrailerSegment);
    }
    
    public static IBatch Batch(ISegment BatchHeaderSegment, List<IMessage> MessageList)
    {
      return new Batch(BatchHeaderSegment, MessageList);
    }
    
    public static IBatch Batch(ISegment BatchHeaderSegment)
    {
      return new Batch(BatchHeaderSegment);
    }
    
    public static IBatch Batch(string StringRaw)
    {
      return new Batch(StringRaw);
    }
    
    public static IBatch Batch()
    {
      return new Batch();
    }
    
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

    public static ISegment Segment(String StringRaw, IMessageDelimiters CustomDelimiters)
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

    public static IElement Element(IMessageDelimiters CustomDelimiters)
    {
      return new Element(CustomDelimiters);
    }

    public static IElement Element(String StringRaw, IMessageDelimiters CustomDelimiters)
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

    public static IField Field(IMessageDelimiters CustomDelimiters)
    {
      return new Field(CustomDelimiters);
    }

    public static IField Field(string StringRaw, IMessageDelimiters CustomDelimiters)
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

    public static IComponent Component(IMessageDelimiters CustomDelimiters)
    {
      return new Component(CustomDelimiters);
    }

    public static IComponent Component(string StringRaw, IMessageDelimiters CustomDelimiters)
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

    public static ISubComponent SubComponent(IMessageDelimiters CustomDelimiters)
    {
      return new SubComponent(CustomDelimiters);
    }

    public static ISubComponent SubComponent(string StringRaw, IMessageDelimiters CustomDelimiters)
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

    public static IContent Content(string String, Support.Content.ContentType ContentType, IMessageDelimiters CustomDelimiters)
    {
      return new Content(String, ContentType, CustomDelimiters);
    }

    public static IContent Content(string String, IMessageDelimiters CustomDelimiters)
    {
      return new Content(String, CustomDelimiters);
    }

    public static IContent Content(Support.Standard.EscapeType EscapeType)
    {
      return new Content(EscapeType);
    }

    public static IContent Content(IEscapeData EscapeMetaData)
    {
      return new Content(EscapeMetaData);
    }

    public static IContent Content(Support.Standard.EscapeType EscapeType, IMessageDelimiters CustomDelimiters)
    {
      return new Content(EscapeType, CustomDelimiters);
    }

    public static IMessageDelimiters MessageDelimiters()
    {
      return new MessageDelimiters();
    }

    public static IMessageDelimiters MessageDelimiters(char Field, char Repeat, char Component, char SubComponent, char Escape)
    {
      return new MessageDelimiters(Field, Repeat, Component, SubComponent, Escape);
    }

    public static IEscapeData EscapeData(string ContentEscapeString)
    {
      return new EscapeData(ContentEscapeString);
    }

    public static IEscapeData EscapeData(PeterPiper.Hl7.V2.Support.Standard.EscapeType EscapeType, string MetaData)
    {
      return new EscapeData(EscapeType, MetaData);
    }


  }
}

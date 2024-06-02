using System;
using System.Collections.Generic;
using PeterPiper.Hl7.V2.Model.Implementation;

namespace PeterPiper.Hl7.V2.Model;

public static class Creator
{
    public static IFile File(ISegment fileHeaderSegment, List<IBatch> batchList, ISegment fileTrailerSegment)
    {
        return new File(fileHeaderSegment, batchList, fileTrailerSegment);
    }

    public static IFile File(ISegment fileHeaderSegment, List<IBatch> batchList)
    {
        return new File(fileHeaderSegment, batchList);
    }

    public static IFile File(ISegment fileHeaderSegment)
    {
        return new File(fileHeaderSegment);
    }

    public static IFile File(string stringRaw)
    {
        return new File(stringRaw);
    }

    public static IFile File()
    {
        return new File();
    }

    public static IBatch Batch(ISegment batchHeaderSegment, List<IMessage> messageList, ISegment batchTrailerSegment)
    {
        return new Batch(batchHeaderSegment, messageList, batchTrailerSegment);
    }

    public static IBatch Batch(ISegment batchHeaderSegment, List<IMessage> messageList)
    {
        return new Batch(batchHeaderSegment, messageList);
    }

    public static IBatch Batch(ISegment batchHeaderSegment)
    {
        return new Batch(batchHeaderSegment);
    }

    public static IBatch Batch(string stringRaw)
    {
        return new Batch(stringRaw);
    }

    public static IBatch Batch()
    {
        return new Batch();
    }

    public static IMessage Message(ISegment segment)
    {
        return new Message(segment);
    }

    public static IMessage Message(String stringRaw, bool parseMSHSegmentOnly = false)
    {
        return new Message(stringRaw, parseMSHSegmentOnly);
    }

    public static IMessage Message(List<String> collection, bool parseMSHSegmentOnly = false)
    {
        return new Message(collection, parseMSHSegmentOnly);
    }

    public static IMessage Message(string messageVersion, string messageType, string messageTrigger,
        string messageControlID = "<GUID>", string messageStructure = "")
    {
        return new Message(messageVersion, messageType, messageTrigger, messageControlID, messageStructure);
    }

    public static ISegment Segment(String stringRaw)
    {
        return new Segment(stringRaw);
    }

    public static ISegment Segment(String stringRaw, IMessageDelimiters customDelimiters)
    {
        return new Segment(stringRaw, customDelimiters);
    }

    public static IElement Element()
    {
        return new Element();
    }

    public static IElement Element(String stringRaw)
    {
        return new Element(stringRaw);
    }

    public static IElement Element(IMessageDelimiters customDelimiters)
    {
        return new Element(customDelimiters);
    }

    public static IElement Element(String stringRaw, IMessageDelimiters customDelimiters)
    {
        return new Element(stringRaw, customDelimiters);
    }

    public static IField Field()
    {
        return new Field();
    }

    public static IField Field(string stringRaw)
    {
        return new Field(stringRaw);
    }

    public static IField Field(IMessageDelimiters customDelimiters)
    {
        return new Field(customDelimiters);
    }

    public static IField Field(string stringRaw, IMessageDelimiters customDelimiters)
    {
        return new Field(stringRaw, customDelimiters);
    }

    public static IComponent Component()
    {
        return new Component();
    }

    public static IComponent Component(string stringRaw)
    {
        return new Component(stringRaw);
    }

    public static IComponent Component(IMessageDelimiters customDelimiters)
    {
        return new Component(customDelimiters);
    }

    public static IComponent Component(string stringRaw, IMessageDelimiters customDelimiters)
    {
        return new Component(stringRaw, customDelimiters);
    }

    public static ISubComponent SubComponent()
    {
        return new SubComponent();
    }

    public static ISubComponent SubComponent(string stringRaw)
    {
        return new SubComponent(stringRaw);
    }

    public static ISubComponent SubComponent(IMessageDelimiters customDelimiters)
    {
        return new SubComponent(customDelimiters);
    }

    public static ISubComponent SubComponent(string stringRaw, IMessageDelimiters customDelimiters)
    {
        return new SubComponent(stringRaw, customDelimiters);
    }

    public static IContent Content(string @string)
    {
        return new Content(@string);
    }

    public static IContent Content(string @string, Support.Content.ContentType contentType)
    {
        return new Content(@string, contentType);
    }

    public static IContent Content(string @string, Support.Content.ContentType contentType,
        IMessageDelimiters customDelimiters)
    {
        return new Content(@string, contentType, customDelimiters);
    }

    public static IContent Content(string @string, IMessageDelimiters customDelimiters)
    {
        return new Content(@string, customDelimiters);
    }

    public static IContent Content(Support.Standard.EscapeType escapeType)
    {
        return new Content(escapeType);
    }

    public static IContent Content(IEscapeData escapeMetaData)
    {
        return new Content(escapeMetaData);
    }

    public static IContent Content(Support.Standard.EscapeType escapeType, IMessageDelimiters customDelimiters)
    {
        return new Content(escapeType, customDelimiters);
    }

    public static IMessageDelimiters MessageDelimiters()
    {
        return new MessageDelimiters();
    }

    public static IMessageDelimiters MessageDelimiters(char field, char repeat, char component, char subComponent,
        char escape)
    {
        return new MessageDelimiters(field, repeat, component, subComponent, escape);
    }

    public static IEscapeData EscapeData(string contentEscapeString)
    {
        return new EscapeData(contentEscapeString);
    }

    public static IEscapeData EscapeData(PeterPiper.Hl7.V2.Support.Standard.EscapeType escapeType, string metaData)
    {
        return new EscapeData(escapeType, metaData);
    }
}
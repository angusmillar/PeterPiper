using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Model.Interface;
using PeterPiper.Hl7.V2.Support.Standard;
using PeterPiper.Hl7.V2.Support;
using PeterPiper.Hl7.V2.Support.Content;

namespace TestHl7V2
{
  [TestFixture]
  public class TestContent
  {
    public IMessageDelimiters CustomDelimiters;

    [SetUp]
    public void Setup()
    {
      CustomDelimiters = Creator.MessageDelimiters('#', '@', '*', '!', '%'); 
    }

    [Test]
    public void ContentConstructorTest1()
    {
      var EscapeType = PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine;
      var target = Creator.Content(EscapeType, CustomDelimiters);
      Assert.AreEqual(".br", target.AsStringRaw, "Custom Delimiters returned incorrectly.");      
    }


    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [Test]
    public void ContentConstructorTest2()
    {
      var EscapeMetaData = Creator.EscapeData(EscapeType.SkipVerticalSpaces, "+10");
      var target = Creator.Content(EscapeMetaData);
      Assert.AreEqual(".sp+10", target.AsStringRaw, "Custom Delimiters returned incorrectly.");      
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [Test]
    public void ContentConstructorTest3()
    {
      string String = "Q5555";
      ContentType ContentType = PeterPiper.Hl7.V2.Support.Content.ContentType.Escape;
      var target = Creator.Content(String, ContentType, CustomDelimiters);
      Assert.AreEqual("Q5555", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual("5555", target.EscapeMetaData.MetaData, "Content Constructor test failed");
      Assert.AreEqual("Q", target.EscapeMetaData.EscapeTypeCharater, "Content Constructor test failed");
      Assert.AreEqual(EscapeType.Unknown, target.EscapeMetaData.EscapeType, "Content Constructor test failed");
      Assert.AreEqual(false, target.EscapeMetaData.IsFormattingCommand, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [Test]
    public void ContentConstructorTest4()
    {
      string String = "Hello World";
      ContentType ContentType = PeterPiper.Hl7.V2.Support.Content.ContentType.Text;
      var target = Creator.Content(String, ContentType);
      Assert.AreEqual("Hello World", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual("Hello World", target.AsString, "Content Constructor test failed");
      Assert.AreEqual(ContentType.Text, target.ContentType, "Content Constructor test failed");
      Assert.AreEqual(null, target.EscapeMetaData, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [Test]
    public void ContentConstructorTest5()
    {
      string String = "Hello World";
      var target = Creator.Content(String);
      Assert.AreEqual("Hello World", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual("Hello World", target.AsString, "Content Constructor test failed");
      Assert.AreEqual(ContentType.Text, target.ContentType, "Content Constructor test failed");
      Assert.AreEqual(null, target.EscapeMetaData, "Content Constructor test failed");

    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [Test]
    public void ContentConstructorTest6()
    {
      EscapeType EscapeType = PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine;
      var target = Creator.Content(EscapeType);
      Assert.AreEqual("", target.AsString, "Content Constructor test failed");
      Assert.AreEqual(".br", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual(true, target.EscapeMetaData.IsFormattingCommand, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [Test]
    public void ContentConstructorTest7()
    {
      string String = "Hello World";
      var target = Creator.Content(String, CustomDelimiters);
      Assert.AreEqual("Hello World", target.AsString, "Content Constructor test failed");
      Assert.AreEqual("Hello World", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual(null, target.EscapeMetaData, "Content Constructor test failed");

    }

    /// <summary>
    ///A test for ClearAll
    ///</summary>
    [Test]
    public void ClearAllTest()
    {
      EscapeType EscapeType = PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine;
      var target = Creator.Content(EscapeType);
      Assert.AreSame(".br", target.AsStringRaw, "Content Constructor test failed");
      target.ClearAll();
      Assert.AreSame("", target.AsStringRaw, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Clone
    ///</summary>
    [Test]
    public void CloneTest()
    {
      EscapeType EscapeType = PeterPiper.Hl7.V2.Support.Standard.EscapeType.HighlightOff;
      var target = Creator.Content(EscapeType);
      var expected = target;      
      var actual = target.Clone();
      Assert.AreEqual(expected.AsString, actual.AsString);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw);
      Assert.AreEqual(expected.ContentType, actual.ContentType);
      Assert.AreEqual(expected.IsEmpty, actual.IsEmpty);
      Assert.AreEqual(expected.IsHL7Null, actual.IsHL7Null);
      Assert.AreEqual(expected.EscapeMetaData.EscapeType, actual.EscapeMetaData.EscapeType);
    }

    /// <summary>
    ///A test for MessageDelimiters
    ///</summary>
    [Test]
    public void DelimterAccessTest()
    {
      var target = Creator.Content("Test", ContentType.Text, CustomDelimiters);            
      Assert.AreEqual('#', target.MessageDelimiters.Field, "A test for MessageDelimiters");
      Assert.AreEqual('@', target.MessageDelimiters.Repeat, "A test for MessageDelimiters");
      Assert.AreEqual('*', target.MessageDelimiters.Component, "A test for MessageDelimiters");
      Assert.AreEqual('!', target.MessageDelimiters.SubComponent, "A test for MessageDelimiters");
      Assert.AreEqual('%', target.MessageDelimiters.Escape, "A test for MessageDelimiters");
    }
  }
}

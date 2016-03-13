using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using NUnit.Framework;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support;

namespace TestHl7V2
{
    [TestFixture]
  public class TestSubComponent
  {
      public MessageDelimiters CustomDelimiters;

      [SetUp]
      public void MyTestInitialize()
      {
        CustomDelimiters = new MessageDelimiters('#', '@', '*', '!', '%');
      }

      [Test]
      public void SubComponentConstructorTest()
      {
        string StringRaw = "Content1%T%Content1";
        SubComponent target = new SubComponent(StringRaw, CustomDelimiters);
        Assert.AreEqual("Content1%T%Content1", target.AsStringRaw, "SubComponentConstructorTest()");
        Assert.AreEqual("Content1!Content1", target.AsString, "SubComponentConstructorTest()");
      }

      /// <summary>
      ///A test for SubComponent Constructor
      ///</summary>
      [Test]
      public void SubComponentConstructorTest1()
      {
        SubComponent target = new SubComponent();
        Assert.AreEqual("", target.AsString, "SubComponentConstructorTest1()");
        Assert.AreEqual(true, target.IsEmpty, "SubComponentConstructorTest1()");
      }

      /// <summary>
      ///A test for SubComponent Constructor
      ///</summary>
      [Test]
      public void SubComponentConstructorTest2()
      {
        string StringRaw = "Content1\\H\\Content2Bold\\N\\Content3";
        SubComponent target = new SubComponent(StringRaw);
        Assert.AreEqual(5, target.ContentCount, "SubComponentConstructorTest3()");
        Assert.AreEqual(PeterPiper.Hl7.V2.Support.Content.ContentType.Escape, target.Content(1).ContentType, "SubComponentConstructorTest3()");
        Assert.AreEqual(PeterPiper.Hl7.V2.Support.Standard.Escapes.HighlightStart.ToString(), target.Content(1).EscapeMetaData.EscapeTypeCharater, "SubComponentConstructorTest3()");
      }

      /// <summary>
      ///A test for Add
      ///</summary>
      [Test]
      public void AddTest()
      {
        SubComponent target = new SubComponent();
        Content item1 = new Content("Hello World");
        Content item2 = new Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine);
        target.Add(item1);
        target.Add(item2);
        Assert.AreEqual(".br", target.Content(1).EscapeMetaData.EscapeTypeCharater, "AddTest() 1");
        Assert.AreEqual("Hello World", target.Content(0).AsString, "AddTest() 1");
      }

      /// <summary>
      ///A test for ClearAll
      ///</summary>
      [Test]
      public void ClearAllTest()
      {
        SubComponent target = new SubComponent("Hello \\E\\ World");
        target.ClearAll();
        Assert.AreEqual(true, target.IsEmpty, "AddTest() 1");
        Assert.AreEqual(String.Empty, target.AsStringRaw, "AddTest() 1");
      }

      /// <summary>
      ///A test for Clone
      ///</summary>
      [Test]
      public void CloneTest()
      {
        SubComponent target = new SubComponent("Hello World");
        SubComponent expected = new SubComponent("Hello World");
        SubComponent actual;
        actual = target.Clone();
        Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw);
        //Assert.AreEqual(String.Empty, target.AsStringRaw, "AddTest() 1");      
      }

      /// <summary>
      ///A test for Content
      ///</summary>
      [Test]
      public void ContentTest()
      {
        SubComponent target = new SubComponent("Hello \\T\\ World");
        int index = 2;
        Content expected = new Content(" World");
        Content actual;
        actual = target.Content(index);
        Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Content");
      }

      /// <summary>
      ///A test for Insert
      ///</summary>
      [Test]
      public void InsertTest1()
      {
        SubComponent target = new SubComponent("Hello "); // TODO: Initialize to an appropriate value
        int index = 1; // TODO: Initialize to an appropriate value
        Content item = new Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.SubComponent);
        target.Insert(index, item);
        Assert.AreEqual("&", target.Content(1).AsString, "A test for Insert");
      }

      /// <summary>
      ///A test for RemoveContentAt
      ///</summary>
      [Test]
      public void RemoveContentAtTest()
      {
        SubComponent target = new SubComponent("Hello \\T\\ World");
        int index = 1;
        target.RemoveContentAt(index);
        Assert.AreEqual("Hello  World", target.AsString, "A test for RemoveContentAt");
      }

      /// <summary>
      ///A test for Set
      ///</summary>
      [Test]
      public void SetTest()
      {
        SubComponent target = new SubComponent("Hello \\F\\ World");
        int index = 1;
        Content item = new Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.SubComponent);
        target.Set(index, item);
        Assert.AreEqual("Hello & World", target.AsString, "A test for Set");
      }

      /// <summary>
      ///A test for ToString
      ///</summary>
      [Test]
      public void ToStringTest()
      {
        SubComponent target = new SubComponent("Hello \\F\\ World");
        string expected = "Hello | World";
        string actual;
        actual = target.ToString();
        Assert.AreEqual(expected, actual);

      }

      /// <summary>
      ///A test for AsString
      ///</summary>
      [Test]
      public void AsStringTest()
      {
        SubComponent target = new SubComponent("Hello \\F\\ World");
        string expected = "Hello | World";
        string actual;
        actual = target.AsString;
        Assert.AreEqual(expected, actual);
      }

      /// <summary>
      ///A test for AsStringRaw
      ///</summary>
      [Test]
      public void AsStringRawTest()
      {
        SubComponent target = new SubComponent("Hello \\F\\ World");
        string expected = "Hello \\F\\ World";
        string actual;
        target.AsStringRaw = expected;
        actual = target.AsStringRaw;
        Assert.AreEqual(expected, actual);

      }

      /// <summary>
      ///A test for ContentCount
      ///</summary>
      [Test]
      public void ContentCountTest()
      {
        SubComponent target = new SubComponent("Hello \\F\\ World");
        int actual;
        actual = target.ContentCount;
        Assert.AreEqual(3, actual);
      }

      /// <summary>
      ///A test for ContentList
      ///</summary>
      [Test]
      public void ContentListTest()
      {
        SubComponent target = new SubComponent("Hello \\F\\ World");
        ReadOnlyCollection<Content> actual;
        actual = target.ContentList;
        Assert.AreEqual(3, actual.Count);
        Assert.AreEqual("Hello ", actual[0].AsString);
        Assert.AreEqual("|", actual[1].AsString);
        Assert.AreEqual(" World", actual[2].AsString);
      }

      /// <summary>
      ///A test for IsEmpty
      ///</summary>
      [Test]
      public void IsEmptyTest()
      {
        SubComponent target = new SubComponent();
        bool actual;
        actual = target.IsEmpty;
        Assert.AreEqual(true, actual);
        target.AsStringRaw = "Hello";
        actual = target.IsEmpty;
        Assert.AreEqual(false, actual);

      }

      /// <summary>
      ///A test for IsHL7Null
      ///</summary>
      [Test]
      public void IsHL7NullTest()
      {
        SubComponent target = new SubComponent(PeterPiper.Hl7.V2.Support.Standard.Null.HL7Null); // TODO: Initialize to an appropriate value
        bool actual;
        actual = target.IsHL7Null;
        Assert.AreEqual(true, actual);
        target = new SubComponent("\"\""); // TODO: Initialize to an appropriate value      
        actual = target.IsHL7Null;
        Assert.AreEqual(true, actual);
      }

      /// <summary>
      ///A test for PathInformation
      ///</summary>
      [Test]
      public void PathInformationTest()
      {
        SubComponent target = new SubComponent("Hello \\F\\ World");
        Assert.AreEqual("<unk>-?", target.PathDetail.PathBrief);
        Assert.AreEqual("<unk>-? [0]", target.Content(0).PathDetail.PathBrief);
        Assert.AreEqual("<unk>-? [1]", target.Content(1).PathDetail.PathBrief);
        Assert.AreEqual("<unk>-? [2]", target.Content(2).PathDetail.PathBrief);
        Assert.AreEqual("<unk>-? [3]", target.Content(3).PathDetail.PathBrief);
        Assert.AreEqual("<unk>-? [3]", target.Content(4).PathDetail.PathBrief);
      }

      /// <summary>
      ///A test for MessageDelimiters
      ///</summary>
      [Test]
      public void DelimterAccessTest()
      {
        string StringRaw = "Content1%T%Content1";
        SubComponent target = new SubComponent(StringRaw, CustomDelimiters);
        Assert.AreEqual('#', target.MessageDelimiters.Field, "A test for MessageDelimiters");
        Assert.AreEqual('@', target.MessageDelimiters.Repeat, "A test for MessageDelimiters");
        Assert.AreEqual('*', target.MessageDelimiters.Component, "A test for MessageDelimiters");
        Assert.AreEqual('!', target.MessageDelimiters.SubComponent, "A test for MessageDelimiters");
        Assert.AreEqual('%', target.MessageDelimiters.Escape, "A test for MessageDelimiters");
      }

  }
}

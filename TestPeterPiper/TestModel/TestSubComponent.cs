using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support;

namespace TestHl7V2
{
    [TestClass]
  public class TestSubComponent
  {
      public IMessageDelimiters CustomDelimiters;

      [TestInitialize]
      public void MyTestInitialize()
      {
        CustomDelimiters = Creator.MessageDelimiters('#', '@', '*', '!', '%');
      }

      [TestMethod]
      public void SubComponentConstructorTest()
      {
        string StringRaw = "Content1%T%Content1";
        var target = Creator.SubComponent(StringRaw, CustomDelimiters);
        Assert.AreEqual("Content1%T%Content1", target.AsStringRaw, "SubComponentConstructorTest()");
        Assert.AreEqual("Content1!Content1", target.AsString, "SubComponentConstructorTest()");
      }

      /// <summary>
      ///A test for SubComponent Constructor
      ///</summary>
      [TestMethod]
      public void SubComponentConstructorTest1()
      {
        var target = Creator.SubComponent();
        Assert.AreEqual("", target.AsString, "SubComponentConstructorTest1()");
        Assert.AreEqual(true, target.IsEmpty, "SubComponentConstructorTest1()");
      }

      /// <summary>
      ///A test for SubComponent Constructor
      ///</summary>
      [TestMethod]
      public void SubComponentConstructorTest2()
      {
        string StringRaw = "Content1\\H\\Content2Bold\\N\\Content3";
        var target = Creator.SubComponent(StringRaw);
        Assert.AreEqual(5, target.ContentCount, "SubComponentConstructorTest3()");
        Assert.AreEqual(PeterPiper.Hl7.V2.Support.Content.ContentType.Escape, target.Content(1).ContentType, "SubComponentConstructorTest3()");
        Assert.AreEqual(PeterPiper.Hl7.V2.Support.Standard.Escapes.HighlightStart.ToString(), target.Content(1).EscapeMetaData.EscapeTypeCharater, "SubComponentConstructorTest3()");
      }

      /// <summary>
      ///A test for Add
      ///</summary>
      [TestMethod]
      public void AddTest()
      {
        var target = Creator.SubComponent();
        var item1 = Creator.Content("Hello World");
        var item2 = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine);
        target.Add(item1);
        target.Add(item2);
        Assert.AreEqual(".br", target.Content(1).EscapeMetaData.EscapeTypeCharater, "AddTest() 1");
        Assert.AreEqual("Hello World", target.Content(0).AsString, "AddTest() 1");
      }

      /// <summary>
      ///A test for ClearAll
      ///</summary>
      [TestMethod]
      public void ClearAllTest()
      {
        var target = Creator.SubComponent("Hello \\E\\ World");
        target.ClearAll();
        Assert.AreEqual(true, target.IsEmpty, "AddTest() 1");
        Assert.AreEqual(String.Empty, target.AsStringRaw, "AddTest() 1");
      }

      /// <summary>
      ///A test for Clone
      ///</summary>
      [TestMethod]
      public void CloneTest()
      {
        var target = Creator.SubComponent("Hello World");
        var expected = Creator.SubComponent("Hello World");        
        var actual = target.Clone();
        Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw);
        //Assert.AreEqual(String.Empty, target.AsStringRaw, "AddTest() 1");      
      }

      /// <summary>
      ///A test for Content
      ///</summary>
      [TestMethod]
      public void ContentTest()
      {
        var target = Creator.SubComponent("Hello \\T\\ World");
        int index = 2;
        var expected = Creator.Content(" World");        
        var actual = target.Content(index);
        Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Content");
      }

      /// <summary>
      ///A test for Insert
      ///</summary>
      [TestMethod]
      public void InsertTest1()
      {
        var target = Creator.SubComponent("Hello "); // TODO: Initialize to an appropriate value
        int index = 1; // TODO: Initialize to an appropriate value
        var item = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.SubComponent);
        target.Insert(index, item);
        Assert.AreEqual("&", target.Content(1).AsString, "A test for Insert");
      }

      /// <summary>
      ///A test for RemoveContentAt
      ///</summary>
      [TestMethod]
      public void RemoveContentAtTest()
      {
        var target = Creator.SubComponent("Hello \\T\\ World");
        int index = 1;
        target.RemoveContentAt(index);
        Assert.AreEqual("Hello  World", target.AsString, "A test for RemoveContentAt");
      }

      /// <summary>
      ///A test for Set
      ///</summary>
      [TestMethod]
      public void SetTest()
      {
        var target = Creator.SubComponent("Hello \\F\\ World");
        int index = 1;
        var item = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.SubComponent);
        target.Set(index, item);
        Assert.AreEqual("Hello & World", target.AsString, "A test for Set");
      }

      /// <summary>
      ///A test for ToString
      ///</summary>
      [TestMethod]
      public void ToStringTest()
      {
        var target = Creator.SubComponent("Hello \\F\\ World");
        string expected = "Hello | World";
        string actual;
        actual = target.ToString();
        Assert.AreEqual(expected, actual);

      }

      /// <summary>
      ///A test for AsString
      ///</summary>
      [TestMethod]
      public void AsStringTest()
      {
        var target = Creator.SubComponent("Hello \\F\\ World");
        string expected = "Hello | World";
        string actual;
        actual = target.AsString;
        Assert.AreEqual(expected, actual);
      }

      /// <summary>
      ///A test for AsStringRaw
      ///</summary>
      [TestMethod]
      public void AsStringRawTest()
      {
        var target = Creator.SubComponent("Hello \\F\\ World");
        string expected = "Hello \\F\\ World";
        string actual;
        target.AsStringRaw = expected;
        actual = target.AsStringRaw;
        Assert.AreEqual(expected, actual);

      }

      /// <summary>
      ///A test for ContentCount
      ///</summary>
      [TestMethod]
      public void ContentCountTest()
      {
        var target = Creator.SubComponent("Hello \\F\\ World");
        int actual;
        actual = target.ContentCount;
        Assert.AreEqual(3, actual);
      }

    /// <summary>
    ///A test for HasContent
    ///</summary>
    [TestMethod]
    public void Content_HasContent_True()    {
      var target = Creator.Component("Hello \\F\\ World&Two");
      bool actual;
      actual = target.SubComponent(1).HasContents;
      Assert.IsTrue(actual, "HasContent should be True for SubComponent");
    }

    /// <summary>
    ///A test for HasContent
    ///</summary>
    [TestMethod]
    public void Content_HasContent_False()
    {
      var target = Creator.Component("&Two");
      bool actual;
      actual = target.SubComponent(1).HasContents;
      Assert.IsFalse(actual, "HasContent should be False for SubComponent");
    }

    /// <summary>
    ///A test for ContentList
    ///</summary>
    [TestMethod]
      public void ContentListTest()
      {
        var target = Creator.SubComponent("Hello \\F\\ World");
        ReadOnlyCollection<IContent> actual;
        actual = target.ContentList;
        Assert.AreEqual(3, actual.Count);
        Assert.AreEqual("Hello ", actual[0].AsString);
        Assert.AreEqual("|", actual[1].AsString);
        Assert.AreEqual(" World", actual[2].AsString);
      }

      /// <summary>
      ///A test for IsEmpty
      ///</summary>
      [TestMethod]
      public void IsEmptyTest()
      {
        var target = Creator.SubComponent();
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
      [TestMethod]
      public void IsHL7NullTest()
      {
        var target = Creator.SubComponent(PeterPiper.Hl7.V2.Support.Standard.Null.HL7Null); // TODO: Initialize to an appropriate value
        bool actual;
        actual = target.IsHL7Null;
        Assert.AreEqual(true, actual);
        target = Creator.SubComponent("\"\""); // TODO: Initialize to an appropriate value      
        actual = target.IsHL7Null;
        Assert.AreEqual(true, actual);
      }

      /// <summary>
      ///A test for PathInformation
      ///</summary>
      [TestMethod]
      public void PathInformationTest()
      {
        var target = Creator.SubComponent("Hello \\F\\ World");
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
      [TestMethod]
      public void DelimterAccessTest()
      {
        string StringRaw = "Content1%T%Content1";
        var target = Creator.SubComponent(StringRaw, CustomDelimiters);
        Assert.AreEqual('#', target.MessageDelimiters.Field, "A test for MessageDelimiters");
        Assert.AreEqual('@', target.MessageDelimiters.Repeat, "A test for MessageDelimiters");
        Assert.AreEqual('*', target.MessageDelimiters.Component, "A test for MessageDelimiters");
        Assert.AreEqual('!', target.MessageDelimiters.SubComponent, "A test for MessageDelimiters");
        Assert.AreEqual('%', target.MessageDelimiters.Escape, "A test for MessageDelimiters");
      }

  }
}

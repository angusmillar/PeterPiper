using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support;
using System.Collections.ObjectModel;

namespace TestHl7V2
{
  [TestFixture]
  public class TestElement
  {

    public MessageDelimiters CustomDelimiters;

    [SetUp]
    public void MyTestInitialize()
    {
      CustomDelimiters = new MessageDelimiters('#', '@', '*', '!', '%');
    }


    /// <summary>
    ///A test for Element Constructor
    ///</summary>
    [Test]
    public void ElementConstructorTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      Assert.AreEqual(StringRaw, target.AsStringRaw, "A test for Element Constructor");
      Assert.AreEqual(4, target.RepeatCount, "A test for Element Constructor");
    }

    /// <summary>
    ///A test for Element Constructor
    ///</summary>
    [Test]
    public void ElementConstructorTest1()
    {
      string StringRaw = "Hello %T% World %.br%Earth!Sub2!Sub3*Comp2*Comp3@R2Hello %T% World %.br%Earth!R2Sub2!R2Sub3*R2Comp2*R2Comp3@R3@R4";
      var target = Creator.Element(StringRaw, CustomDelimiters);
      Assert.AreEqual(StringRaw, target.AsStringRaw, "A test for Element Constructor 1");
      Assert.AreEqual(4, target.RepeatCount, "A test for Element Constructor 1");
    }

    /// <summary>
    ///A test for Element Constructor
    ///</summary>
    [Test]
    public void ElementConstructorTest2()
    {
      var target = Creator.Element();
      Assert.AreEqual("", target.AsStringRaw, "A test for Element Constructor 2");
      Assert.AreEqual(0, target.RepeatCount, "A test for Element Constructor 2");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [Test]
    public void AddTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      var item = Creator.Field("R5");
      target.Add(item);
      Assert.AreEqual(StringRaw + "~R5", target.AsStringRaw, "A test for Add");
      Assert.AreEqual(5, target.RepeatCount, "A test for Add");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [Test]
    public void AddTest1()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      var item = Creator.Component("CompAdded");
      target.Add(item);
      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3^CompAdded~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4", target.AsStringRaw, "A test for Add");
      Assert.AreEqual(4, target.ComponentCount, "A test for Element Constructor 2");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [Test]
    public void AddTest2()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      var item = Creator.SubComponent("SubAdded");
      target.Add(item);
      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3&SubAdded^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4", target.AsStringRaw, "A test for Add 2");
      Assert.AreEqual(4, target.SubComponetCount, "A test for Add 2");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [Test]
    public void AddTest3()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      var item = Creator.Content("ContAdded");
      target.Add(item);
      Assert.AreEqual("Hello \\T\\ World \\.br\\EarthContAdded&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4", target.AsStringRaw, "A test for Add 2");
      Assert.AreEqual(6, target.ContentCount, "A test for Add 2");
    }

    /// <summary>
    ///A test for ClearAll
    ///</summary>
    [Test]
    public void ClearAllTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      target.ClearAll();
      Assert.AreEqual(true, target.IsEmpty, "A test for ClearAll");
      Assert.AreEqual(String.Empty, target.AsStringRaw, "A test for ClearAll");
    }

    /// <summary>
    ///A test for Clone
    ///</summary>
    [Test]
    public void CloneTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      var expected = Creator.Element(StringRaw);      
      var actual = target.Clone();
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Clone");
    }

    /// <summary>
    ///A test for Component
    ///</summary>
    [Test]
    public void ComponentTest()
    {      
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 2;
      var expected = Creator.Component("Comp2");      
      var actual = target.Component(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Component");
    }

    /// <summary>
    ///A test for Content
    ///</summary>
    [Test]
    public void ContentTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 3;
      var expected = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine);      
      var actual = target.Content(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Content");
      Assert.AreEqual(expected.ContentType, actual.ContentType, "A test for Content");
      Assert.AreEqual(expected.EscapeMetaData.EscapeType, actual.EscapeMetaData.EscapeType, "A test for Content");
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [Test]
    public void InsertTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 2;
      var item = Creator.SubComponent("SubInserted");
      target.Insert(index, item);
      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&SubInserted&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4", target.AsStringRaw, "A test for Insert");
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [Test]
    public void InsertTest1()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 3;
      var item = Creator.Content("ContentInserted");
      target.Insert(index, item);
      Assert.AreEqual("Hello \\T\\ World ContentInserted\\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4", target.AsStringRaw, "A test for Insert 1");
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [Test]
    public void InsertTest2()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 2; // 
      var item = Creator.Component("CompInserted");
      target.Insert(index, item);
      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^CompInserted^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4", target.AsStringRaw, "A test for Insert 2");
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [Test]
    public void InsertTest4()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 3;
      var item = Creator.Field("FieldInseted");
      target.Insert(index, item);
      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~FieldInseted~R3~R4", target.AsStringRaw, "A test for Insert 4");
    }

    /// <summary>
    ///A test for RemoveComponentAt
    ///</summary>
    [Test]
    public void RemoveComponentAtTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 2;
      target.RemoveComponentAt(index);
      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4", target.AsStringRaw, "A test for RemoveComponentAt");
    }

    /// <summary>
    ///A test for RemoveContentAt
    ///</summary>
    [Test]
    public void RemoveContentAtTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 3;
      target.RemoveContentAt(index);
      Assert.AreEqual("Hello \\T\\ World Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4", target.AsStringRaw, "A test for RemoveComponentAt");
    }

    /// <summary>
    ///A test for RemoveRepeatAt
    ///</summary>
    [Test]
    public void RemoveRepeatAtTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 2;
      target.RemoveRepeatAt(index);
      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R3~R4", target.AsStringRaw, "A test for RemoveComponentAt");
    }

    /// <summary>
    ///A test for RemoveSubComponentAt
    ///</summary>
    [Test]
    public void RemoveSubComponentAtTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 2;
      target.RemoveSubComponentAt(index);
      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4", target.AsStringRaw, "A test for RemoveComponentAt");
    }

    /// <summary>
    ///A test for Repeat
    ///</summary>
    [Test]
    public void RepeatTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 2;
      var expected = Creator.Field("R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3");      
      var actual = target.Repeat(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Repeat");

    }

    /// <summary>
    ///A test for Set
    ///</summary>
    [Test]
    public void SetTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 3;
      var item = Creator.Content("ContNew");
      target.Set(index, item);
      Assert.AreEqual("Hello \\T\\ World ContNewEarth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4", target.AsStringRaw, "A test for Set");
    }

    /// <summary>
    ///A test for SubComponent
    ///</summary>
    [Test]
    public void SubComponentTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int index = 2; // TODO: Initialize to an appropriate value
      var expected = Creator.SubComponent("Sub2");      
      var actual = target.SubComponent(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for SubComponent");

    }

    /// <summary>
    ///A test for ToString
    ///</summary>
    [Test]
    public void ToStringTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      string expected = "Hello & World Earth&Sub2&Sub3^Comp2^Comp3~R2Hello & World Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      string actual;
      actual = target.ToString();
      Assert.AreEqual(expected, actual, "A test for ToString");
    }

    /// <summary>
    ///A test for AsString
    ///</summary>
    [Test]
    public void AsStringTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      string expected = "Hello & World Earth&Sub2&Sub3^Comp2^Comp3~R2Hello & World Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      string actual;
      target.AsString = expected;
      actual = target.AsString;
      Assert.AreEqual(expected, actual, "A test for AsString");
    }

    /// <summary>
    ///A test for AsStringRaw
    ///</summary>
    [Test]
    public void AsStringRawTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      string expected = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      string actual;
      target.AsStringRaw = expected;
      actual = target.AsStringRaw;
      Assert.AreEqual(expected, actual, "A test for AsStringRaw");

    }

    /// <summary>
    ///A test for ComponentCount
    ///</summary>
    [Test]
    public void ComponentCountTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int actual;
      actual = target.ComponentCount;
      Assert.AreEqual(3, actual, "A test for ComponentCount");
    }

    /// <summary>
    ///A test for ContentCount
    ///</summary>
    [Test]
    public void ContentCountTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int actual;
      actual = target.ContentCount;
      Assert.AreEqual(5, actual, "A test for ContentCount");
    }

    /// <summary>
    ///A test for IsEmpty
    ///</summary>
    [Test]
    public void IsEmptyTest()
    {
      string StringRaw = "";
      var target = Creator.Element(StringRaw);
      bool actual;
      actual = target.IsEmpty;
      Assert.AreEqual(true, actual, "A test for ContentCount");
      target.AsStringRaw = "Hello";
      actual = target.IsEmpty;
      Assert.AreEqual(false, actual, "A test for ContentCount");

    }

    /// <summary>
    ///A test for IsHL7Null
    ///</summary>
    [Test]
    public void IsHL7NullTest()
    {
      var target = Creator.Element(); // TODO: Initialize to an appropriate value
      bool actual;
      actual = target.IsHL7Null;
      Assert.AreEqual(false, actual, "A test for IsHL7Null");
      target.AsStringRaw = "\"\"";
      actual = target.IsHL7Null;
      Assert.AreEqual(true, actual, "A test for IsHL7Null");
    }

    /// <summary>
    ///A test for RepeatCount
    ///</summary>
    [Test]
    public void RepeatCountTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int actual;
      actual = target.RepeatCount;
      Assert.AreEqual(4, actual, "A test for RepeatCount");
    }

    /// <summary>
    ///A test for RepeatList
    ///</summary>
    [Test]
    public void RepeatListTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      ReadOnlyCollection<IField> actual;
      actual = target.RepeatList;
      Assert.AreEqual(4, actual.Count, "A test for RepeatList");

      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3", actual[0].AsStringRaw, "A test for RepeatList");
      Assert.AreEqual("R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3", actual[1].AsStringRaw, "A test for RepeatList");
      Assert.AreEqual("R3", actual[2].AsStringRaw, "A test for RepeatList");
      Assert.AreEqual("R4", actual[3].AsStringRaw, "A test for RepeatList");
    }

    /// <summary>
    ///A test for SubComponetCount
    ///</summary>
    [Test]
    public void SubComponetCountTest()
    {
      string StringRaw = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3~R2Hello \\T\\ World \\.br\\Earth&R2Sub2&R2Sub3^R2Comp2^R2Comp3~R3~R4";
      var target = Creator.Element(StringRaw);
      int actual;
      actual = target.SubComponetCount;
      Assert.AreEqual(3, actual, "A test for SubComponetCount");
    }

    /// <summary>
    ///A test for MessageDelimiters
    ///</summary>
    [Test]
    public void DelimterAccessTest()
    {
      var target = Creator.Element("Test", CustomDelimiters);
      Assert.AreEqual('#', target.MessageDelimiters.Field, "A test for MessageDelimiters");
      Assert.AreEqual('@', target.MessageDelimiters.Repeat, "A test for MessageDelimiters");
      Assert.AreEqual('*', target.MessageDelimiters.Component, "A test for MessageDelimiters");
      Assert.AreEqual('!', target.MessageDelimiters.SubComponent, "A test for MessageDelimiters");
      Assert.AreEqual('%', target.MessageDelimiters.Escape, "A test for MessageDelimiters");
    }
  }
}

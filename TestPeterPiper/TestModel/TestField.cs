using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TestHl7V2
{
  [TestClass]
  public class TestField
  {
    public IMessageDelimiters CustomDelimiters;

    [TestInitialize]
    public void MyTestInitialize()
    {
      CustomDelimiters = Creator.MessageDelimiters('#', '@', '*', '!', '%');
    }

    /// <summary>
    ///A test for Field Constructor
    ///</summary>
    [TestMethod]
    public void FieldConstructorTest()
    {
      string StringRaw = "Field1^Field2^Field3^Field4";
      var target  = Creator.Field(StringRaw);
      Assert.AreEqual("Field1^Field2^Field3^Field4", target.AsString, "A test for Field Constructor");
      Assert.AreEqual(4, target.ComponentCount, "A test for Field Constructor");
    }

    /// <summary>
    ///A test for Field Constructor
    ///</summary>
    [TestMethod]
    public void FieldConstructorTest1()
    {
      string StringRaw = "Field1*Field2*Field3*Field4";
      var target  = Creator.Field(StringRaw, CustomDelimiters);
      Assert.AreEqual("Field1*Field2*Field3*Field4", target.AsString, "A test for Field Constructor 1");
      Assert.AreEqual(4, target.ComponentCount, "A test for Field Constructor 1");
    }

    /// <summary>
    ///A test for Field Constructor
    ///</summary>
    [TestMethod]
    public void FieldConstructorTest2()
    {
      var target  = Creator.Field();
      Assert.AreEqual(true, target.IsEmpty, "A test for Field Constructor 2");
      Assert.AreEqual(String.Empty, target.AsString, "A test for Field Constructor 2");
      Assert.AreEqual(String.Empty, target.AsStringRaw, "A test for Field Constructor 2");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [TestMethod]
    public void AddTest()
    {
      var target  = Creator.Field("Field1^Field2^Field3^Field4");
      var item = Creator.Component("Field5");
      target.Add(item);
      Assert.AreEqual("Field1^Field2^Field3^Field4^Field5", target.AsString, "A test for Add");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [TestMethod]
    public void AddTest2()
    {
      var target  = Creator.Field("Field1.1\\.br\\Field1.2\\.br\\Field1.3^Field2^Field3^Field4");
      var item = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine);
      target.Add(item);
      Assert.AreEqual("Field1.1\\.br\\Field1.2\\.br\\Field1.3\\.br\\^Field2^Field3^Field4", target.AsStringRaw, "A test for Add 2");
      Assert.AreEqual(6, target.Component(1).ContentCount, "A test for Add 2");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [TestMethod]
    public void AddTest3()
    {
      var target  = Creator.Field("Sub1&Sub2&Sub3");
      var item = Creator.SubComponent("Sub4");
      target.Add(item);
      Assert.AreEqual("Sub1&Sub2&Sub3&Sub4", target.AsStringRaw, "A test for Add 3");
      Assert.AreEqual(4, target.SubComponentCount, "A test for Add 3");
    }

    /// <summary>
    ///A test for ClearAll
    ///</summary>
    [TestMethod]
    public void ClearAllTest()
    {
      var target  = Creator.Field("Sub1&Sub2&Sub3^Comp2\\H\\");
      target.ClearAll();
      Assert.AreEqual(String.Empty, target.AsStringRaw, "A test for ClearAll");
    }

    /// <summary>
    ///A test for Clone
    ///</summary>
    [TestMethod]
    public void CloneTest()
    {
      var target  = Creator.Field("Sub1&Sub2&Sub3^Comp2\\H\\Comp2.2&Sub");
      var expected = Creator.Field("Sub1&Sub2&Sub3^Comp2\\H\\Comp2.2&Sub");      
      var actual = target.Clone();
      Assert.AreEqual(expected.AsStringRaw, target.AsStringRaw, "A test for Clone");
      Assert.AreEqual(expected.Component(2).AsStringRaw, expected.Component(2).AsStringRaw, "A test for Clone");
      Assert.AreEqual(expected.Component(2).SubComponent(2).AsStringRaw, expected.Component(2).SubComponent(2).AsStringRaw, "A test for Clone");
    }

    /// <summary>
    ///A test for Component
    ///</summary>
    [TestMethod]
    public void ComponentTest()
    {
      var Target = Creator.Field("Sub1&Sub2&Sub3^Comp2\\H\\Comp2.2&Sub");      
      int index = 2; // TODO: Initialize to an appropriate value
      var expected = Creator.Component("Comp2\\H\\Comp2.2&Sub");      
      var actual = Target.Component(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Component");

    }

    /// <summary>
    ///A test for Content
    ///</summary>
    [TestMethod]
    public void ContentTest()
    {
      var target  = Creator.Field("Comp1\\H\\Comp1.2&Sub^Sub1&Sub2&Sub3^");
      int index = 1;
      var expected = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.HighlightOn);
      var actual = target.Content(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Content");
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [TestMethod]
    public void InsertTest()
    {
      var target  = Creator.Field("Sub1&Sub2&Sub4");
      int index = 3; // TODO: Initialize to an appropriate value
      var item = Creator.SubComponent("Sub3");
      target.Insert(index, item);
      Assert.AreEqual("Sub1&Sub2&Sub3&Sub4", target.AsStringRaw, "A test for Insert");
      Assert.AreEqual(4, target.SubComponentCount, "A test for Insert");
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [TestMethod]
    public void InsertTest1()
    {
      var target  = Creator.Field("one\\T\\two");
      int index = 1;
      var item = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine);
      target.Insert(index, item);
      Assert.AreEqual("one\\.br\\\\T\\two", target.AsStringRaw, "A test for Insert 1");
      target = Creator.Field();
      var item2 = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine);
      target.Insert(5, item2);
      Assert.AreEqual("\\.br\\", target.AsStringRaw, "A test for Insert 1");

    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [TestMethod]
    public void InsertTest3()
    {
      var target  = Creator.Field("one^two^Four");
      int index = 3; // TODO: Initialize to an appropriate value
      var item = Creator.Component("three");
      target.Insert(index, item);
      Assert.AreEqual("one^two^three^Four", target.AsStringRaw, "A test for Insert 3");
      Assert.AreEqual(4, target.ComponentCount, "A test for Insert 3");
      target = Creator.Field();
      var item2 = Creator.Component("three");
      target.Insert(index, item2);
      Assert.AreEqual("^^three", target.AsStringRaw, "A test for Insert 3");
      Assert.AreEqual(3, target.ComponentCount, "A test for Insert 3");

    }

    /// <summary>
    ///A test for RemoveComponentAt
    ///</summary>
    [TestMethod]
    public void RemoveComponentAtTest()
    {
      var target  = Creator.Field("Comp1^Comp2^Comp3^Comp4");
      int index = 3;
      target.RemoveComponentAt(index);
      Assert.AreEqual(3, target.ComponentCount, "A test for RemoveComponentAt");
      Assert.AreEqual("Comp1^Comp2^Comp4", target.AsStringRaw, "A test for RemoveComponentAt");
    }

    /// <summary>
    ///A test for RemoveContentAt
    ///</summary>
    [TestMethod]
    public void RemoveContentAtTest()
    {
      var target  = Creator.Field();
      int index = 3;
      target.RemoveContentAt(index);
      Assert.AreEqual(0, target.ContentCount, "A test for RemoveContentAt");
      Assert.AreEqual("", target.AsStringRaw, "A test for RemoveContentAt");

      target = Creator.Field("cont0\\H\\Cont2\\N\\Cont4");
      index = 3;
      target.RemoveContentAt(index);
      Assert.AreEqual(4, target.ContentCount, "A test for RemoveContentAt");
      Assert.AreEqual("cont0\\H\\Cont2Cont4", target.AsStringRaw, "A test for RemoveContentAt");
    }

    /// <summary>
    ///A test for RemoveSubComponentAt
    ///</summary>
    [TestMethod]
    public void RemoveSubComponentAtTest()
    {
      var target  = Creator.Field("Sub1&Sub2&Sub3&Sub4^Comp2");
      int index = 3;
      target.RemoveSubComponentAt(index);
      Assert.AreEqual(3, target.SubComponentCount, "A test for RemoveSubComponentAt");
      Assert.AreEqual("Sub1&Sub2&Sub4^Comp2", target.AsStringRaw, "A test for RemoveSubComponentAt");
      target.RemoveSubComponentAt(20);
    }

    /// <summary>
    ///A test for Set
    ///</summary>
    [TestMethod]
    public void SetTest()
    {
      var target  = Creator.Field("\\H\\Cont1\\N\\Cont3\\.br\\newline&Sub1^Comp2");
      int index = 3;
      var item = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.CenterNextLine);
      target.Set(index, item);
      Assert.AreEqual(6, target.ContentCount, "A test for Set");
      Assert.AreEqual("\\H\\Cont1\\N\\\\.ce\\\\.br\\newline&Sub1^Comp2", target.AsStringRaw, "A test for Set");
    }

    /// <summary>
    ///A test for Set
    ///</summary>
    [TestMethod]
    public void SetTest1()
    {
      var target  = Creator.Field("\\H\\Cont1\\N\\Cont3\\.br\\newline&Sub2&Sub3&Sub4^Comp2");
      int index = 3;
      var item = Creator.SubComponent("NewSub");
      target.Set(index, item);
      Assert.AreEqual("\\H\\Cont1\\N\\Cont3\\.br\\newline&Sub2&NewSub&Sub4^Comp2", target.AsStringRaw, "A test for Set 1");
    }


    /// <summary>
    ///A test for Set
    ///</summary>
    [TestMethod]
    public void SetTest2()
    {
      var target  = Creator.Field("\\H\\Cont1\\N\\Cont3\\.br\\newline&Sub2&Sub3&Sub4^Comp2^Comp3^Comp4");
      int index = 3;
      var item = Creator.Component("NewComp");
      target.Set(index, item);
      Assert.AreEqual("\\H\\Cont1\\N\\Cont3\\.br\\newline&Sub2&Sub3&Sub4^Comp2^NewComp^Comp4", target.AsStringRaw, "A test for Set 2");
    }

    /// <summary>
    ///A test for SubComponent
    ///</summary>
    [TestMethod]
    public void SubComponentTest()
    {
      var target  = Creator.Field("Sub1&Sub2&Sub3&Sub4");
      int index = 3;
      var expected = Creator.SubComponent("Sub3");      
      var actual = target.SubComponent(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for SubComponent");
    }

    /// <summary>
    ///A test for ToString
    ///</summary>
    [TestMethod]
    public void ToStringTest()
    {
      var target  = Creator.Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      string expected = "Hello & World Earth&Sub2&Sub3^Comp2^Comp3";
      string actual;
      actual = target.ToString();
      Assert.AreEqual(expected, actual, "A test for ToString");
    }

    /// <summary>
    ///A test for AsString
    ///</summary>
    [TestMethod]
    public void AsStringTest()
    {
      var target  = Creator.Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      string expected = "Hello & World Earth&Sub2&Sub3^Comp2^Comp3";
      string actual;
      target.AsString = expected;
      actual = target.AsString;
      Assert.AreEqual(expected, actual, "A test for AsString");
    }

    /// <summary>
    ///A test for AsStringRaw
    ///</summary>
    [TestMethod]
    public void AsStringRawTest()
    {
      var target  = Creator.Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      string expected = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3";
      string actual;
      target.AsStringRaw = expected;
      actual = target.AsStringRaw;
      Assert.AreEqual(expected, actual, "A test for AsStringRaw");
    }

    /// <summary>
    ///A test for ComponentCount
    ///</summary>
    [TestMethod]
    public void ComponentCountTest()
    {
      var target  = Creator.Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      int actual;
      actual = target.ComponentCount;
      Assert.AreEqual(3, actual, "A test for AsStringRaw");
    }

    /// <summary>
    ///A test for ComponentCount
    ///</summary>
    [TestMethod]
    public void Component_HasComponents_Many()
    {
      var target = Creator.Field("Comp1^Comp2^Comp3");
      bool actual;
      actual = target.HasComponents;
      Assert.IsTrue(actual, "HasComponents should be True");
    }

    /// <summary>
    ///A test for ComponentCount
    ///</summary>
    [TestMethod]
    public void Component_HasComponents_One()
    {
      var target = Creator.Field("Comp1");
      bool actual;
      actual = target.HasComponents;
      Assert.IsFalse(actual, "HasComponents should be False");
    }

    /// <summary>
    ///A test for ComponentCount
    ///</summary>
    [TestMethod]
    public void Component_HasComponents_None()
    {
      var target = Creator.Field("");
      bool actual;
      actual = target.HasComponents;
      Assert.IsFalse(actual, "HasComponents should be False");
    }

    /// <summary>
    ///A test for ComponentList
    ///</summary>
    [TestMethod]
    public void ComponentListTest()
    {
      var target  = Creator.Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      ReadOnlyCollection<IComponent> actual;
      actual = target.ComponentList;
      Assert.AreEqual(3, actual.Count, "A test for AsStringRaw");
      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3", actual[0].AsStringRaw, "A test for AsStringRaw");
      Assert.AreEqual("Comp2", actual[1].AsStringRaw, "A test for AsStringRaw");
      Assert.AreEqual("Comp3", actual[2].AsStringRaw, "A test for AsStringRaw");
    }

    /// <summary>
    ///A test for ContentCount
    ///</summary>
    [TestMethod]
    public void ContentCountTest()
    {
      var target  = Creator.Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      int actual;
      actual = target.ContentCount;
      Assert.AreEqual(5, actual, "A test for AsStringRaw");
    }

    /// <summary>
    ///A test for HasContets
    ///</summary>
    [TestMethod]
    public void Content_HasContents_Many()
    {
      var target = Creator.Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      bool actual;
      actual = target.Component(1).HasContents;
      Assert.IsTrue(actual, "A test for HasContents should be True");
    }

    /// <summary>
    ///A test for HasContets
    ///</summary>
    [TestMethod]
    public void Content_HasContets_One()
    {
      var target = Creator.Field("Hello&Sub2&Sub3^Comp2^Comp3");
      bool actual;
      actual = target.Component(1).HasContents;
      Assert.IsFalse(actual, "A test for HasContents should be False");
    }

    /// <summary>
    ///A test for HasContets
    ///</summary>
    [TestMethod]
    public void Content_HasContets_None()
    {
      var target = Creator.Field("^Comp2^Comp3");
      bool actual;
      actual = target.Component(1).HasContents;
      Assert.IsFalse(actual, "A test for HasContents should be False");
    }

    /// <summary>
    ///A test for IsEmpty
    ///</summary>
    [TestMethod]
    public void IsEmptyTest()
    {
      var target  = Creator.Field();
      bool actual;
      actual = target.IsEmpty;
      Assert.AreEqual(true, actual, "A test for AsStringRaw");
      target.AsStringRaw = "\\.br\\";
      actual = target.IsEmpty;
      Assert.AreEqual(false, actual, "A test for AsStringRaw");

    }

    /// <summary>
    ///A test for IsHL7Null
    ///</summary>
    [TestMethod]
    public void IsHL7NullTest()
    {
      var target  = Creator.Field("\"\"");
      bool actual;
      actual = target.IsHL7Null;
      Assert.AreEqual(true, actual, "A test for IsHL7Null");
      target.AsString = "Hello";
      actual = target.IsHL7Null;
      Assert.AreEqual(false, actual, "A test for IsHL7Null");
    }

    /// <summary>
    ///A test for SubComponentCount
    ///</summary>
    [TestMethod]
    public void SubComponentCountTest()
    {
      var target  = Creator.Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      int actual;
      actual = target.SubComponentCount;
      Assert.AreEqual(3, actual, "A test for SubComponentCount");
    }

    /// <summary>
    ///A test for HasSubComponents
    ///</summary>
    [TestMethod]
    public void SubComponent_HasSubComponets_Many()
    {
      var target = Creator.Field("SubOne&SubTwo&SubThree^Comp2");
      bool actual;
      actual = target.HasSubComponents;
      Assert.IsTrue(actual, "A test for HasSubComponents True");
    }

    /// <summary>
    ///A test for HasSubComponents
    ///</summary>
    [TestMethod]
    public void SubComponent_HasSubComponets_One()
    {
      var target = Creator.Field("SubOne^Comp2");
      bool actual;
      actual = target.HasSubComponents;
      Assert.IsFalse(actual, "A test for HasSubComponents False");
    }

    /// <summary>
    ///A test for HasSubComponents
    ///</summary>
    [TestMethod]
    public void SubComponent_HasSubComponets_None()
    {
      var target = Creator.Field("^Comp2");
      bool actual;
      actual = target.HasSubComponents;
      Assert.IsFalse(actual, "A test for HasSubComponents False");
    }

    /// <summary>
    ///A test for MessageDelimiters
    ///</summary>
    [TestMethod]
    public void DelimterAccessTest()
    {
      var target  = Creator.Field("Test", CustomDelimiters);
      Assert.AreEqual('#', target.MessageDelimiters.Field, "A test for MessageDelimiters");
      Assert.AreEqual('@', target.MessageDelimiters.Repeat, "A test for MessageDelimiters");
      Assert.AreEqual('*', target.MessageDelimiters.Component, "A test for MessageDelimiters");
      Assert.AreEqual('!', target.MessageDelimiters.SubComponent, "A test for MessageDelimiters");
      Assert.AreEqual('%', target.MessageDelimiters.Escape, "A test for MessageDelimiters");
    }
  }
}

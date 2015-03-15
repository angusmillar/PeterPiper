using Glib.Hl7.V2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Glib.Hl7.V2.Support;
using System.Collections.ObjectModel;

namespace TestProjectGlib
{
    
    
    /// <summary>
    ///This is a test class for FieldTest and is intended
    ///to contain all FieldTest Unit Tests
    ///</summary>
  [TestClass()]
  public class FieldTest
  {


    private TestContext testContextInstance;
    public MessageDelimiters CustomDelimiters;
    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    [TestInitialize()]
    public void MyTestInitialize()
    {
      CustomDelimiters = new MessageDelimiters('#', '@', '*', '!', '%'); 
    }
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for Field Constructor
    ///</summary>
    [TestMethod()]
    public void FieldConstructorTest()
    {
      string StringRaw = "Field1^Field2^Field3^Field4";
      Field target = new Field(StringRaw);
      Assert.AreEqual("Field1^Field2^Field3^Field4", target.AsString, "A test for Field Constructor");
      Assert.AreEqual(4, target.ComponentCount, "A test for Field Constructor");
    }

    /// <summary>
    ///A test for Field Constructor
    ///</summary>
    [TestMethod()]
    public void FieldConstructorTest1()
    {
      string StringRaw = "Field1*Field2*Field3*Field4";
      Field target = new Field(StringRaw, CustomDelimiters);
      Assert.AreEqual("Field1*Field2*Field3*Field4", target.AsString, "A test for Field Constructor 1");
      Assert.AreEqual(4, target.ComponentCount, "A test for Field Constructor 1");
    }

    /// <summary>
    ///A test for Field Constructor
    ///</summary>
    [TestMethod()]
    public void FieldConstructorTest2()
    {
      Field target = new Field();
      Assert.AreEqual(true, target.IsEmpty, "A test for Field Constructor 2");
      Assert.AreEqual(String.Empty, target.AsString, "A test for Field Constructor 2");
      Assert.AreEqual(String.Empty, target.AsStringRaw, "A test for Field Constructor 2");
    }

    /// <summary>
    ///A test for Field Constructor
    ///</summary>
    [TestMethod()]
    public void FieldConstructorTest3()
    {      
      Field target = new Field(CustomDelimiters);
      Assert.AreEqual('#', target.Delimiters.Field, "A test for Field Constructor 3");
      Assert.AreEqual('@', target.Delimiters.Repeat, "A test for Field Constructor 3");
      Assert.AreEqual('*', target.Delimiters.Component, "A test for Field Constructor 3");
      Assert.AreEqual('!', target.Delimiters.SubComponent, "A test for Field Constructor 3");
      Assert.AreEqual('%', target.Delimiters.Escape, "A test for Field Constructor 3");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [TestMethod()]
    public void AddTest()
    {
      Field target = new Field("Field1^Field2^Field3^Field4");
      Component item = new Component("Field5");
      target.Add(item);
      Assert.AreEqual("Field1^Field2^Field3^Field4^Field5", target.AsString, "A test for Add");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [TestMethod()]
    public void AddTest2()
    {
      Field target = new Field("Field1.1\\.br\\Field1.2\\.br\\Field1.3^Field2^Field3^Field4");
      Content item = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.NewLine);
      target.Add(item);
      Assert.AreEqual("Field1.1\\.br\\Field1.2\\.br\\Field1.3\\.br\\^Field2^Field3^Field4", target.AsStringRaw, "A test for Add 2");
      Assert.AreEqual(6, target.Component(1).ContentCount, "A test for Add 2");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [TestMethod()]
    public void AddTest3()
    {
      Field target = new Field("Sub1&Sub2&Sub3");
      SubComponent item = new SubComponent("Sub4");
      target.Add(item);
      Assert.AreEqual("Sub1&Sub2&Sub3&Sub4", target.AsStringRaw, "A test for Add 3");
      Assert.AreEqual(4, target.SubComponentCount, "A test for Add 3");
    }

    /// <summary>
    ///A test for ClearAll
    ///</summary>
    [TestMethod()]
    public void ClearAllTest()
    {
      Field target = new Field("Sub1&Sub2&Sub3^Comp2\\H\\");
      target.ClearAll();
      Assert.AreEqual(String.Empty, target.AsStringRaw, "A test for ClearAll");
    }

    /// <summary>
    ///A test for Clone
    ///</summary>
    [TestMethod()]
    public void CloneTest()
    {
      Field target = new Field("Sub1&Sub2&Sub3^Comp2\\H\\Comp2.2&Sub");
      Field expected = new Field("Sub1&Sub2&Sub3^Comp2\\H\\Comp2.2&Sub");
      Field actual;
      actual = target.Clone();
      Assert.AreEqual(expected.AsStringRaw, target.AsStringRaw, "A test for Clone");
      Assert.AreEqual(expected.Component(2).AsStringRaw, expected.Component(2).AsStringRaw, "A test for Clone");
      Assert.AreEqual(expected.Component(2).SubComponent(2).AsStringRaw, expected.Component(2).SubComponent(2).AsStringRaw, "A test for Clone");
      Assert.AreEqual(null, target._Index, "A test for Clone");
      Assert.AreEqual(null, target._Parent, "A test for Clone");
      Assert.AreEqual(true, target._Temporary, "A test for Clone");
    }

    /// <summary>
    ///A test for Component
    ///</summary>
    [TestMethod()]
    public void ComponentTest()
    {
      Field target = new Field("Sub1&Sub2&Sub3^Comp2\\H\\Comp2.2&Sub");
      int index = 2; // TODO: Initialize to an appropriate value
      Component expected = new Component("Comp2\\H\\Comp2.2&Sub");
      Component actual;
      actual = target.Component(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Component");
      
    }

    /// <summary>
    ///A test for Content
    ///</summary>
    [TestMethod()]
    public void ContentTest()
    {
      Field target = new Field("Comp1\\H\\Comp1.2&Sub^Sub1&Sub2&Sub3^");
      int index = 1;
      Content expected = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.HighlightOn);
      Content actual;
      actual = target.Content(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Content");      
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [TestMethod()]
    public void InsertTest()
    {
      Field target = new Field("Sub1&Sub2&Sub4");
      int index = 3; // TODO: Initialize to an appropriate value
      SubComponent item = new SubComponent("Sub3");
      target.Insert(index, item);
      Assert.AreEqual("Sub1&Sub2&Sub3&Sub4", target.AsStringRaw, "A test for Insert");
      Assert.AreEqual(4, target.SubComponentCount, "A test for Insert");      
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [TestMethod()]
    public void InsertTest1()
    {
      Field target = new Field("one\\T\\two"); 
      int index = 1; 
      Content item = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.NewLine);
      target.Insert(index, item);
      Assert.AreEqual("one\\.br\\\\T\\two", target.AsStringRaw, "A test for Insert 1");
      target = new Field();
      Content item2 = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.NewLine);
      target.Insert(5, item2);
      Assert.AreEqual("\\.br\\", target.AsStringRaw, "A test for Insert 1");      

    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [TestMethod()]
    public void InsertTest3()
    {
      Field target = new Field("one^two^Four"); 
      int index = 3; // TODO: Initialize to an appropriate value
      Component item = new Component("three");
      target.Insert(index, item);
      Assert.AreEqual("one^two^three^Four", target.AsStringRaw, "A test for Insert 3");
      Assert.AreEqual(4, target.ComponentCount, "A test for Insert 3");
      target = new Field();
      Component item2 = new Component("three");
      target.Insert(index, item2);
      Assert.AreEqual("^^three", target.AsStringRaw, "A test for Insert 3");
      Assert.AreEqual(3, target.ComponentCount, "A test for Insert 3");

    }

    /// <summary>
    ///A test for RemoveComponentAt
    ///</summary>
    [TestMethod()]
    public void RemoveComponentAtTest()
    {
      Field target = new Field("Comp1^Comp2^Comp3^Comp4");
      int index = 3; 
      target.RemoveComponentAt(index);
      Assert.AreEqual(3, target.ComponentCount, "A test for RemoveComponentAt");
      Assert.AreEqual("Comp1^Comp2^Comp4", target.AsStringRaw, "A test for RemoveComponentAt");      
    }

    /// <summary>
    ///A test for RemoveContentAt
    ///</summary>
    [TestMethod()]
    public void RemoveContentAtTest()
    {
      Field target = new Field();
      int index = 3;
      target.RemoveContentAt(index);
      Assert.AreEqual(0, target.ContentCount, "A test for RemoveContentAt");
      Assert.AreEqual("", target.AsStringRaw, "A test for RemoveContentAt");      

      target = new Field("cont0\\H\\Cont2\\N\\Cont4"); 
      index = 3;
      target.RemoveContentAt(index);
      Assert.AreEqual(4, target.ContentCount, "A test for RemoveContentAt");
      Assert.AreEqual("cont0\\H\\Cont2Cont4", target.AsStringRaw, "A test for RemoveContentAt");      
    }

    /// <summary>
    ///A test for RemoveSubComponentAt
    ///</summary>
    [TestMethod()]
    public void RemoveSubComponentAtTest()
    {
      Field target = new Field("Sub1&Sub2&Sub3&Sub4^Comp2"); 
      int index = 3; 
      target.RemoveSubComponentAt(index);
      Assert.AreEqual(3, target.SubComponentCount, "A test for RemoveSubComponentAt");
      Assert.AreEqual("Sub1&Sub2&Sub4^Comp2", target.AsStringRaw, "A test for RemoveSubComponentAt");
      target.RemoveSubComponentAt(20);
    }

    /// <summary>
    ///A test for Set
    ///</summary>
    [TestMethod()]
    public void SetTest()
    {
      Field target = new Field("\\H\\Cont1\\N\\Cont3\\.br\\newline&Sub1^Comp2");
      int index = 3;
      Content item = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.CenterNextLine); 
      target.Set(index, item);
      Assert.AreEqual(6, target.ContentCount, "A test for Set");
      Assert.AreEqual("\\H\\Cont1\\N\\\\.ce\\\\.br\\newline&Sub1^Comp2", target.AsStringRaw, "A test for Set");            
    }

    /// <summary>
    ///A test for Set
    ///</summary>
    [TestMethod()]
    public void SetTest1()
    {
      Field target = new Field("\\H\\Cont1\\N\\Cont3\\.br\\newline&Sub2&Sub3&Sub4^Comp2");
      int index = 3;
      SubComponent item = new SubComponent("NewSub");
      target.Set(index, item);
      Assert.AreEqual("\\H\\Cont1\\N\\Cont3\\.br\\newline&Sub2&NewSub&Sub4^Comp2", target.AsStringRaw, "A test for Set 1");            
    }


    /// <summary>
    ///A test for Set
    ///</summary>
    [TestMethod()]
    public void SetTest2()
    {
      Field target = new Field("\\H\\Cont1\\N\\Cont3\\.br\\newline&Sub2&Sub3&Sub4^Comp2^Comp3^Comp4");
      int index = 3;
      Component item = new Component("NewComp");
      target.Set(index, item);
      Assert.AreEqual("\\H\\Cont1\\N\\Cont3\\.br\\newline&Sub2&Sub3&Sub4^Comp2^NewComp^Comp4", target.AsStringRaw, "A test for Set 2");            
    }

    /// <summary>
    ///A test for SubComponent
    ///</summary>
    [TestMethod()]
    public void SubComponentTest()
    {
      Field target = new Field("Sub1&Sub2&Sub3&Sub4");
      int index = 3;
      SubComponent expected = new SubComponent("Sub3");
      SubComponent actual;
      actual = target.SubComponent(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for SubComponent");      
    }

    /// <summary>
    ///A test for ToString
    ///</summary>
    [TestMethod()]
    public void ToStringTest()
    {
      Field target = new Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      string expected = "Hello & World Earth&Sub2&Sub3^Comp2^Comp3";
      string actual; 
      actual = target.ToString();
      Assert.AreEqual(expected, actual, "A test for ToString");      
    }

    /// <summary>
    ///A test for AsString
    ///</summary>
    [TestMethod()]
    public void AsStringTest()
    {
      Field target = new Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      string expected = "Hello & World Earth&Sub2&Sub3^Comp2^Comp3";
      string actual;
      target.AsString = expected;
      actual = target.AsString;
      Assert.AreEqual(expected, actual, "A test for AsString");      
    }

    /// <summary>
    ///A test for AsStringRaw
    ///</summary>
    [TestMethod()]
    public void AsStringRawTest()
    {
      Field target = new Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      string expected = "Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3";
      string actual;
      target.AsStringRaw = expected;
      actual = target.AsStringRaw;
      Assert.AreEqual(expected, actual, "A test for AsStringRaw");      
    }

    /// <summary>
    ///A test for ComponentCount
    ///</summary>
    [TestMethod()]
    public void ComponentCountTest()
    {
      Field target = new Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      int actual;
      actual = target.ComponentCount;
      Assert.AreEqual(3, actual, "A test for AsStringRaw");      
    }

    /// <summary>
    ///A test for ComponentList
    ///</summary>
    [TestMethod()]
    public void ComponentListTest()
    {
      Field target = new Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      ReadOnlyCollection<Component> actual;
      actual = target.ComponentList;
      Assert.AreEqual(3, actual.Count, "A test for AsStringRaw");
      Assert.AreEqual("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3", actual[0].AsStringRaw, "A test for AsStringRaw");
      Assert.AreEqual("Comp2", actual[1].AsStringRaw, "A test for AsStringRaw");
      Assert.AreEqual("Comp3", actual[2].AsStringRaw, "A test for AsStringRaw");      
    }

    /// <summary>
    ///A test for ContentCount
    ///</summary>
    [TestMethod()]
    public void ContentCountTest()
    {
      Field target = new Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      int actual;
      actual = target.ContentCount;
      Assert.AreEqual(5, actual, "A test for AsStringRaw");      
    }

    /// <summary>
    ///A test for IsEmpty
    ///</summary>
    [TestMethod()]
    public void IsEmptyTest()
    {
      Field target = new Field(); 
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
    [TestMethod()]
    public void IsHL7NullTest()
    {
      Field target = new Field("\"\""); 
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
    [TestMethod()]
    public void SubComponentCountTest()
    {
      Field target = new Field("Hello \\T\\ World \\.br\\Earth&Sub2&Sub3^Comp2^Comp3");
      int actual;
      actual = target.SubComponentCount;
      Assert.AreEqual(3, actual, "A test for SubComponentCount");      
    }

  }
}

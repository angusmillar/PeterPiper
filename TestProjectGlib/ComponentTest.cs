using Glib.Hl7.V2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Glib.Hl7.V2.Support;
using System.Collections.ObjectModel;

namespace TestProjectGlib
{
    
    
    /// <summary>
    ///This is a test class for ComponentTest and is intended
    ///to contain all ComponentTest Unit Tests
    ///</summary>
  [TestClass()]
  public class ComponentTest
  {

    public MessageDelimiters CustomDelimiters;
    string ComponentTestString = "Cat\\T\\Mice&Dogs\\T\\Cats";
    private TestContext testContextInstance;

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
    ///A test for Component Constructor
    ///</summary>
    [TestMethod()]
    public void ComponentConstructorTest()
    {      
      Component target = new Component(ComponentTestString);
      Assert.AreEqual("Cat&Mice&Dogs&Cats", target.AsString, "A test for Component Constructor");
      Assert.AreEqual("Cat\\T\\Mice&Dogs\\T\\Cats", target.AsStringRaw, "A test for Component Constructor");
    }

    /// <summary>
    ///A test for Component Constructor
    ///</summary>
    [TestMethod()]
    public void ComponentConstructorTest1()
    {
      string StringRaw = "Cat%T%Mice!Dogs%T%Cats";      
      Component target = new Component(StringRaw, CustomDelimiters);
      Assert.AreEqual("Cat!Mice!Dogs!Cats", target.AsString, "A test for Component Constructor 1");
      Assert.AreEqual("Cat%T%Mice!Dogs%T%Cats", target.AsStringRaw, "A test for Component Constructor 1");
    }

    /// <summary>
    ///A test for Component Constructor
    ///</summary>
    [TestMethod()]
    public void ComponentConstructorTest2()
    {
      Component target = new Component();
      Assert.AreEqual(String.Empty, target.AsString, "A test for Component Constructor 2");
      Assert.AreEqual(true, target.IsEmpty, "A test for Component Constructor 2");
      Assert.AreEqual(String.Empty, target.SubComponent(1).AsStringRaw, "A test for Component Constructor 2");
      Assert.AreEqual(true, target.SubComponent(1).IsEmpty, "A test for Component Constructor 2");
      
    }

    /// <summary>
    ///A test for Component Constructor
    ///</summary>
    [TestMethod()]
    public void ComponentConstructorTest3()
    {      
      Component target = new Component(CustomDelimiters);
      Assert.AreSame(CustomDelimiters, target.Delimiters, "A test for Component Constructor 3");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [TestMethod()]
    public void AddTest1()
    {
      Component target = new Component("Comp1"); 
      SubComponent item = new SubComponent("Sub1");
      target.Add(item);
      Assert.AreEqual("Comp1&Sub1", target.AsStringRaw, "A test for Add 1");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [TestMethod()]
    public void AddTest2()
    {
      Component target = new Component("Comp1"); 
      Content item = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.NewLine);
      target.Add(item);
      Assert.AreEqual("Comp1\\.br\\", target.AsStringRaw, "A test for Add 2");
    }

    /// <summary>
    ///A test for ClearAll
    ///</summary>
    [TestMethod()]
    public void ClearAllTest()
    {
      Component target = new Component("Comp1&Sub1&Sub2\\T\\2");
      target.ClearAll();
      Assert.AreEqual(String.Empty, target.AsStringRaw, "A test for ClearAll");
      Assert.AreEqual(true, target.IsEmpty, "A test for ClearAll");
    }

    /// <summary>
    ///A test for Clone
    ///</summary>
    [TestMethod()]
    public void CloneTest()
    {
      Component target = new Component("Comp1&Sub1&Sub2\\T\\2"); 
      Component expected = new Component("Comp1&Sub1&Sub2\\T\\2"); 
      Component actual;
      actual = target.Clone();
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Clone");      
    }

    /// <summary>
    ///A test for Content
    ///</summary>
    [TestMethod()]
    public void ContentTest()
    {
      Component target = new Component("Hello \\T\\ World \\F\\ Bye");
      int index = 3;
      Content expected = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.Field);
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
      Component target = new Component("Hello&World&Bye"); 
      int index = 2; 
      SubComponent item = new SubComponent("Earth");
      target.Insert(index, item);
      Assert.AreEqual("Hello&Earth&World&Bye", target.AsStringRaw, "A test for Insert");
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [TestMethod()]
    public void InsertTest1()
    {
      Component target = new Component("Hello\\R\\World\\R\\Bye"); 
      int index = 3;
      Content item = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.Field);
      target.Insert(index, item);
      Assert.AreEqual("Hello~World|~Bye", target.AsString, "A test for Insert");
    }

    /// <summary>
    ///A test for RemoveContentAt
    ///</summary>
    [TestMethod()]
    public void RemoveContentAtTest()
    {
      Component target = new Component("Hello\\R\\World\\R\\Bye"); 
      int index = 3; // TODO: Initialize to an appropriate value
      target.RemoveContentAt(index);
      Assert.AreEqual("Hello~WorldBye", target.AsString, "A test for RemoveContentAt");
    }

    /// <summary>
    ///A test for RemoveSubComponentAt
    ///</summary>
    [TestMethod()]
    public void RemoveSubComponentAtTest()
    {
      Component target = new Component("Hello!World!Bye", CustomDelimiters); 
      int index = 2; // TODO: Initialize to an appropriate value
      target.RemoveSubComponentAt(index);
      Assert.AreEqual("Hello!Bye", target.AsString, "A test for RemoveSubComponentAt");
    }

    /// <summary>
    ///A test for Set
    ///</summary>
    [TestMethod()]
    public void SetTest()
    {
      Component target = new Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      int index = 1; 
      Content item1 = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.NewLine);
      Content item2 = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.Indent);
      target.Set(index, item1);
      Assert.AreEqual("HelloWorld~Bye&SecondComponent", target.AsString, "A test for Set");
      Assert.AreEqual(".br", target.Content(1).AsStringRaw, "A test for Set");
      target.Set(10, item2);
      Assert.AreEqual("Hello\\.br\\World\\R\\Bye\\.in\\&SecondComponent", target.AsStringRaw, "A test for Set");
    }

    /// <summary>
    ///A test for Set
    ///</summary>
    [TestMethod()]
    public void SetTest1()
    {
      Component target = new Component("Hello\\R\\World\\R\\Bye&SecondComponent&ThirdComponent");
      int index = 2; 
      SubComponent item = new SubComponent("NewSub");
      SubComponent item2 = new SubComponent("NewSub2");
      target.Set(index, item);
      Assert.AreEqual("NewSub", target.SubComponent(2).AsStringRaw, "A test for Set 2");
      target.Set(10, item2);
      Assert.AreEqual("NewSub2", target.SubComponent(10).AsStringRaw, "A test for Set 2");

    }

    /// <summary>
    ///A test for SubComponent
    ///</summary>
    [TestMethod()]
    public void SubComponentTest()
    {
      Component target = new Component("Hello\\R\\World\\R\\Bye&SecondComponent"); 
      int index = 2;
      SubComponent expected = new SubComponent("SecondComponent");
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
      Component target = new Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      string expected = "Hello~World~Bye&SecondComponent"; 
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
      Component target = new Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      string expected = "Hello~World~Bye&SecondComponent"; // TODO: Initialize to an appropriate value
      string actual;
      target.AsString = expected;
      actual = target.AsString;
      Assert.AreEqual(expected, actual,"A test for ToString");      
    }

    /// <summary>
    ///A test for AsStringRaw
    ///</summary>
    [TestMethod()]
    public void AsStringRawTest()
    {
      Component target = new Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      string expected = "Hello\\R\\World\\R\\Bye&SecondComponent";
      string actual;
      target.AsStringRaw = expected;
      actual = target.AsStringRaw;
      Assert.AreEqual(expected, actual, "A test for ToStringRaw");
      
    }

    /// <summary>
    ///A test for ContentCount
    ///</summary>
    [TestMethod()]
    public void ContentCountTest()
    {
      Component target = new Component("Hello\\R\\World\\R\\Bye&SecondComponent");
      int actual;
      actual = target.ContentCount;
      Assert.AreEqual(5, target.ContentCount, "A test for ContentCount");
    }

    /// <summary>
    ///A test for IsEmpty
    ///</summary>
    [TestMethod()]
    public void IsEmptyTest()
    {
      Component target = new Component(); // TODO: Initialize to an appropriate value
      bool actual;
      actual = target.IsEmpty;
      Assert.AreEqual(true, target.IsEmpty, "A test for IsEmpty");
      target.AsString = "Hello";
      Assert.AreEqual(false, target.IsEmpty, "A test for IsEmpty");
    }

    /// <summary>
    ///A test for IsHL7Null
    ///</summary>
    [TestMethod()]
    public void IsHL7NullTest()
    {
      Component target = new Component("\"\""); 
      bool actual;
      actual = target.IsHL7Null;
      Assert.AreEqual(true, target.IsHL7Null, "A test for IsHL7Null");
      Assert.AreEqual(Glib.Hl7.V2.Support.Standard.Null.HL7Null, target.AsString, "A test for IsHL7Null");
      target.AsString = "Hello";
      Assert.AreEqual(false, target.IsHL7Null, "A test for IsHL7Null");

    }

    /// <summary>
    ///A test for SubComponentCount
    ///</summary>
    [TestMethod()]
    public void SubComponentCountTest()
    {
      Component target = new Component("one&two&three&four"); 
      int actual;
      actual = target.SubComponentCount;
      Assert.AreEqual(4, actual, "A test for SubComponentCount");
    }

    /// <summary>
    ///A test for SubComponentList
    ///</summary>
    [TestMethod()]
    public void SubComponentListTest()
    {
      Component target = new Component("one&two&three&four"); 
      ReadOnlyCollection<SubComponent> actual;
      actual = target.SubComponentList;
      Assert.AreEqual(4, actual.Count, "A test for SubComponentList");
      Assert.AreEqual("one", actual[0].AsString, "A test for SubComponentList");
      Assert.AreEqual("two", actual[1].AsString, "A test for SubComponentList");
      Assert.AreEqual("three", actual[2].AsString, "A test for SubComponentList");
      Assert.AreEqual("four", actual[3].AsString, "A test for SubComponentList");
    }

    /// <summary>
    ///A test for PathInformation
    ///</summary>
    [TestMethod()]
    public void PathInformationTest()
    {
      Component target = new Component("one&two&three&four");
      Assert.AreEqual("<unk>-?", target.PathInformation.PathBrief, "A test for SubComponentList");
      Assert.AreEqual("<unk>-?.?.2", target.SubComponent(2).PathInformation.PathBrief, "A test for SubComponentList");
      Assert.AreEqual("<unk>-?.?.2 [0]", target.SubComponent(2).Content(0).PathInformation.PathBrief, "A test for SubComponentList");
    }
  }
}

using Glib.Hl7.V2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Glib.Hl7.V2.Support;
using System.Collections.ObjectModel;

namespace TestProjectGlib
{
    
    
    /// <summary>
    ///This is a test class for SubComponentTest and is intended
    ///to contain all SubComponentTest Unit Tests
    ///</summary>
  [TestClass()]
  public class SubComponentTest
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
    
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for SubComponent Constructor
    ///</summary>
    [TestMethod()]
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
    [TestMethod()]
    public void SubComponentConstructorTest1()
    {
      SubComponent target = new SubComponent();
      Assert.AreEqual("", target.AsString, "SubComponentConstructorTest1()");
      Assert.AreEqual(true, target.IsEmpty, "SubComponentConstructorTest1()");      
    }

    /// <summary>
    ///A test for SubComponent Constructor
    ///</summary>
    [TestMethod()]
    public void SubComponentConstructorTest2()
    {      
      SubComponent target = new SubComponent(CustomDelimiters);
      Assert.AreSame(CustomDelimiters, target.Delimiters, "SubComponentConstructorTest2()");
      Assert.AreSame(CustomDelimiters, target.Content(1).Delimiters, "SubComponentConstructorTest2()");      
    }

    /// <summary>
    ///A test for SubComponent Constructor
    ///</summary>
    [TestMethod()]
    public void SubComponentConstructorTest3()
    {
      string StringRaw = "Content1\\H\\Content2Bold\\N\\Content3";
      SubComponent target = new SubComponent(StringRaw);
      Assert.AreEqual(5, target.ContentCount, "SubComponentConstructorTest3()");
      Assert.AreEqual(Glib.Hl7.V2.Support.Content.ContentType.Escape, target.Content(1).ContentType, "SubComponentConstructorTest3()");
      Assert.AreEqual(Glib.Hl7.V2.Support.Standard.Escapes.HighlightStart.ToString(), target.Content(1).EscapeMetaData.EscapeTypeCharater, "SubComponentConstructorTest3()");      
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [TestMethod()]
    public void AddTest()
    {
      SubComponent target = new SubComponent(); 
      Content item1 = new Content("Hello World");
      Content item2 = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.NewLine);
      target.Add(item1);
      target.Add(item2);
      Assert.AreEqual(".br" , target.Content(1).EscapeMetaData.EscapeTypeCharater, "AddTest() 1");
      Assert.AreEqual("Hello World", target.Content(0).AsString, "AddTest() 1");      
    }

    /// <summary>
    ///A test for ClearAll
    ///</summary>
    [TestMethod()]
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
    [TestMethod()]
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
    [TestMethod()]
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
    [TestMethod()]
    public void InsertTest1()
    {
      SubComponent target = new SubComponent("Hello "); // TODO: Initialize to an appropriate value
      int index = 1; // TODO: Initialize to an appropriate value
      Content item = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.SubComponent);
      target.Insert(index, item);
      Assert.AreEqual("&", target.Content(1).AsString, "A test for Insert");      
    }

    /// <summary>
    ///A test for RemoveContentAt
    ///</summary>
    [TestMethod()]
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
    [TestMethod()]
    public void SetTest()
    {
      SubComponent target = new SubComponent("Hello \\F\\ World"); 
      int index = 1; 
      Content item = new Content(Glib.Hl7.V2.Support.Standard.EscapeType.SubComponent);
      target.Set(index, item);
      Assert.AreEqual("Hello & World", target.AsString, "A test for Set");      
    }

    /// <summary>
    ///A test for ToString
    ///</summary>
    [TestMethod()]
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
    [TestMethod()]
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
    [TestMethod()]
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
    [TestMethod()]
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
    [TestMethod()]
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
    [TestMethod()]
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
    [TestMethod()]
    public void IsHL7NullTest()
    {
      SubComponent target = new SubComponent(Glib.Hl7.V2.Support.Standard.Null.HL7Null); // TODO: Initialize to an appropriate value
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
    [TestMethod()]
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

  }
}

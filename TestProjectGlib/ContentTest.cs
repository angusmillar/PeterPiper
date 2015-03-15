using Glib.Hl7.V2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Glib.Hl7.V2.Support.Standard;
using Glib.Hl7.V2.Support;
using Glib.Hl7.V2.Support.Content;

namespace TestProjectGlib
{
    
    
    /// <summary>
    ///This is a test class for ContentTest and is intended
    ///to contain all ContentTest Unit Tests
    ///</summary>
  [TestClass()]
  public class ContentTest
  {

    public MessageDelimiters CustomDelimiters;
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
    ///A test for Content Constructor
    ///</summary>
    [TestMethod()]
    public void ContentConstructorTest()
    {
      EscapeType EscapeType = Glib.Hl7.V2.Support.Standard.EscapeType.NewLine;       
      Content target = new Content(EscapeType, CustomDelimiters);
      Assert.AreEqual(".br", target.AsStringRaw, "Custom Delimiters reterned incorrectly.");
      Assert.AreEqual("%", target.Delimiters.Escape.ToString(), "Custom Delimiters reterned incorrectly.");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [TestMethod()]
    public void ContentConstructorTest1()
    {
      EscapeData EscapeMetaData = new EscapeData(EscapeType.SkipVerticalSpaces, "+10");
      Content target = new Content(EscapeMetaData);
      Assert.AreEqual(".sp+10", target.AsStringRaw, "Custom Delimiters reterned incorrectly.");
      Assert.AreEqual("|", target.Delimiters.Field.ToString(), "Custom Delimiters reterned incorrectly.");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [TestMethod()]
    public void ContentConstructorTest2()
    {
      string String = "Q4556";
      Glib.Hl7.V2.Model.ModelSupport.ContentTypeInternal ContentTypeInternal = Glib.Hl7.V2.Model.ModelSupport.ContentTypeInternal.Escape;       
      bool Temporary = true; 
      Nullable<int> Index = 10; 
      ContentBase Parent = null; 
      Content target = new Content(String, ContentTypeInternal, CustomDelimiters, Temporary, Index, Parent);
      Assert.AreEqual("Q4556", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual("Q", target.EscapeMetaData.EscapeTypeCharater, "Content Constructor test failed");
      Assert.AreEqual("4556", target.EscapeMetaData.MetaData, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [TestMethod()]
    public void ContentConstructorTest3()
    {
      EscapeType EscapeType = Glib.Hl7.V2.Support.Standard.EscapeType.WordWrapOn;      
      bool Temporary = true; 
      Nullable<int> Index = new Nullable<int>(); 
      ContentBase Parent = null; 
      Content target = new Content(EscapeType, CustomDelimiters, Temporary, Index, Parent);
      Assert.AreSame(".fi", target.AsStringRaw, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [TestMethod()]
    public void ContentConstructorTest4()
    {
      string String = "Test Content";      
      bool Temporary = true; // TODO: Initialize to an appropriate value
      Nullable<int> Index = 20; // TODO: Initialize to an appropriate value
      ContentBase Parent = null; // TODO: Initia;ize to an appropriate value
      Content target = new Content(String, CustomDelimiters, Temporary, Index, Parent);
      Assert.AreSame("Test Content", target.AsStringRaw, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [TestMethod()]
    public void ContentConstructorTest5()
    {
      string String = "Q5555"; // TODO: Initialize to an appropriate value
      ContentType ContentType = Glib.Hl7.V2.Support.Content.ContentType.Escape;      
      Content target = new Content(String, ContentType, CustomDelimiters);
      Assert.AreEqual("Q5555", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual("5555", target.EscapeMetaData.MetaData, "Content Constructor test failed");
      Assert.AreEqual("Q", target.EscapeMetaData.EscapeTypeCharater, "Content Constructor test failed");
      Assert.AreEqual(EscapeType.Unknown, target.EscapeMetaData.EscapeType, "Content Constructor test failed");
      Assert.AreEqual(false, target.EscapeMetaData.IsFormattingCommand, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [TestMethod()]
    public void ContentConstructorTest6()
    {
      string String = "Hello World"; 
      ContentType ContentType = Glib.Hl7.V2.Support.Content.ContentType.Text;
      Content target = new Content(String, ContentType);
      Assert.AreEqual("Hello World", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual("Hello World", target.AsString, "Content Constructor test failed");
      Assert.AreEqual(ContentType.Text, target.ContentType, "Content Constructor test failed");
      Assert.AreEqual(null, target.EscapeMetaData, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [TestMethod()]
    public void ContentConstructorTest7()
    {
      string String = "Hello World";
      Content target = new Content(String);
      Assert.AreEqual("Hello World", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual("Hello World", target.AsString, "Content Constructor test failed");
      Assert.AreEqual(ContentType.Text, target.ContentType, "Content Constructor test failed");
      Assert.AreEqual(null, target.EscapeMetaData, "Content Constructor test failed");

    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [TestMethod()]
    public void ContentConstructorTest8()
    {
      EscapeType EscapeType = Glib.Hl7.V2.Support.Standard.EscapeType.NewLine;
      Content target = new Content(EscapeType);
      Assert.AreEqual("", target.AsString, "Content Constructor test failed");
      Assert.AreEqual(".br", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual(true, target.EscapeMetaData.IsFormattingCommand, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Content Constructor
    ///</summary>
    [TestMethod()]
    public void ContentConstructorTest9()
    {
      string String = "Hello World";      
      Content target = new Content(String, CustomDelimiters);
      Assert.AreEqual("Hello World", target.AsString, "Content Constructor test failed");
      Assert.AreEqual("Hello World", target.AsStringRaw, "Content Constructor test failed");
      Assert.AreEqual(null, target.EscapeMetaData, "Content Constructor test failed");
      
    }

    /// <summary>
    ///A test for ClearAll
    ///</summary>
    [TestMethod()]
    public void ClearAllTest()
    {
      EscapeType EscapeType = Glib.Hl7.V2.Support.Standard.EscapeType.NewLine;
      Content target = new Content(EscapeType);
      Assert.AreSame(".br", target.AsStringRaw, "Content Constructor test failed");
      target.ClearAll();
      Assert.AreSame("", target.AsStringRaw, "Content Constructor test failed");
    }

    /// <summary>
    ///A test for Clone
    ///</summary>
    [TestMethod()]
    public void CloneTest()
    {
      EscapeType EscapeType = Glib.Hl7.V2.Support.Standard.EscapeType.HighlightOff;
      Content target = new Content(EscapeType); // TODO: Initialize to an appropriate value
      Content expected = target; // TODO: Initialize to an appropriate value
      Content actual;
      actual = target.Clone();
      Assert.AreEqual(expected._Index, actual._Index);
      Assert.AreEqual(expected._Parent, actual._Parent);
      Assert.AreEqual(expected._Temporary, actual._Temporary);
      Assert.AreEqual(expected.AsString, actual.AsString);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw);
      Assert.AreEqual(expected.ContentType, actual.ContentType);
      Assert.AreEqual(expected.IsEmpty, actual.IsEmpty);
      Assert.AreEqual(expected.IsHL7Null, actual.IsHL7Null);
      Assert.AreEqual(expected.EscapeMetaData.EscapeType, actual.EscapeMetaData.EscapeType);      
    }

    /// <summary>
    ///A test for RemoveFromParent
    ///</summary>
    [TestMethod()]
    [DeploymentItem("Glib.dll")]
    public void RemoveFromParentTest()
    {
      //PrivateObject param0 = null; // TODO: Initialize to an appropriate value
      //Content_Accessor target = new Content_Accessor(param0); // TODO: Initialize to an appropriate value
      //target.RemoveFromParent();
      //Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test for SetDataToEncodingCharacters
    ///</summary>
    [TestMethod()]
    [DeploymentItem("Glib.dll")]
    public void SetDataToEncodingCharactersTest()
    {
      //PrivateObject param0 = null; // TODO: Initialize to an appropriate value
      //Content_Accessor target = new Content_Accessor(param0); // TODO: Initialize to an appropriate value
      //target.SetDataToEncodingCharacters();
      //Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test for SetDataToMainSeparator
    ///</summary>
    [TestMethod()]
    [DeploymentItem("Glib.dll")]
    public void SetDataToMainSeparatorTest()
    {
      //PrivateObject param0 = null; // TODO: Initialize to an appropriate value
      //Content_Accessor target = new Content_Accessor(param0); // TODO: Initialize to an appropriate value
      //target.SetDataToMainSeparator();
      //Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test for SetParent
    ///</summary>
    [TestMethod()]
    [DeploymentItem("Glib.dll")]
    public void SetParentTest()
    {
      //PrivateObject param0 = null; // TODO: Initialize to an appropriate value
      //Content_Accessor target = new Content_Accessor(param0); // TODO: Initialize to an appropriate value
      //target.SetParent();
      //Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test for ToString
    ///</summary>
    [TestMethod()]
    public void ToStringTest()
    {
      EscapeType EscapeType = Glib.Hl7.V2.Support.Standard.EscapeType.Field;
      Content target = new Content(EscapeType); 
      string expected = "|"; 
      string actual;
      actual = target.ToString();
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for ValidateData
    ///</summary>
    [TestMethod()]
    [DeploymentItem("Glib.dll")]
    public void ValidateDataTest()
    {
      //PrivateObject param0 = null; // TODO: Initialize to an appropriate value
      //Content_Accessor target = new Content_Accessor(param0); // TODO: Initialize to an appropriate value
      //string String = string.Empty; // TODO: Initialize to an appropriate value
      //bool expected = false; // TODO: Initialize to an appropriate value
      //bool actual;
      //actual = target.ValidateData(String);
      //Assert.AreEqual(expected, actual);
      //Assert.Inconclusive("Verify the correctness of this test method.");
    }

    /// <summary>
    ///A test for AsString
    ///</summary>
    [TestMethod()]
    public void AsStringTest()
    {      
      Content target = new Content("Hellow World"); // TODO: Initialize to an appropriate value
      string expected = "Hellow World";
      string actual;
      target.AsString = expected;
      actual = target.AsString;
      Assert.AreEqual(expected, actual);

      target = new Content(EscapeType.NewLine);
      expected = "";            
      actual = target.AsString;
      Assert.AreEqual(expected, actual);
      Assert.AreEqual(false, target.IsEmpty);      

    }

    /// <summary>
    ///A test for AsStringRaw
    ///</summary>
    [TestMethod()]
    public void AsStringRawTest()
    {
      EscapeType EscapeType = Glib.Hl7.V2.Support.Standard.EscapeType.Indent;
      Content target = new Content(EscapeType); // TODO: Initialize to an appropriate value
      string expected = ".in";
      string actual;
      target.AsStringRaw = expected;
      actual = target.AsStringRaw;
      Assert.AreEqual(expected, actual);
      
    }

    /// <summary>
    ///A test for ContentType
    ///</summary>
    [TestMethod()]
    public void ContentTypeTest()
    {      
      Content target = new Content(new EscapeData("B00"));
      ContentType expected = ContentType.Escape;
      ContentType actual;
      target.ContentType = expected;
      actual = target.ContentType;
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for EscapeMetaData
    ///</summary>
    [TestMethod()]
    public void EscapeMetaDataTest()
    {
      EscapeData oEscapeData = new EscapeData("B00");
      Content target = new Content(oEscapeData);
      EscapeData actual;
      actual = target.EscapeMetaData;
      Assert.AreSame(oEscapeData, actual);
    }

    /// <summary>
    ///A test for IsEmpty
    ///</summary>
    [TestMethod()]
    public void IsEmptyTest()
    {      
      Content target = new Content("");
      bool actual;
      actual = target.IsEmpty;
      Assert.AreEqual(true, target.IsEmpty);
      target.AsString = "hello world";
      Assert.AreEqual(false, target.IsEmpty);
      target = new Content(EscapeType.NewLine);
      Assert.AreEqual(false, target.IsEmpty);
    }

    /// <summary>
    ///A test for IsHL7Null
    ///</summary>
    [TestMethod()]
    public void IsHL7NullTest()
    {      
      Content target = new Content(Glib.Hl7.V2.Support.Standard.Null.HL7Null);
      bool actual = true;
      actual = target.IsHL7Null;
      Assert.AreEqual("\"\"", target.AsString, "IsHL7NullTest() test failed");
      Assert.AreEqual(actual, target.IsHL7Null, "IsHL7NullTest() test failed");
    }

    /// <summary>
    ///A test for PathInformation
    ///</summary>
    [TestMethod()]
    public void PathInformationTest()
    {
      Content target = new Content("one");
      Assert.AreEqual("<unk>-?", target.PathInformation.PathBrief);     
    }
  }
}

using Glib.Hl7.V2.Support.Content;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using System.Linq;

namespace TestProjectGlib
{
    //Test GIT
    
    /// <summary>
    ///This is a test class for Base64Test and is intended
    ///to contain all Base64Test Unit Tests
    ///</summary>
  [TestClass()]
  public class Base64Test
  {

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
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    public static string AssemblyDirectory
    {
      get
      {
        string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        string path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path);
      }
    }

    /// <summary>
    ///A test for Decoder
    ///</summary>
    [TestMethod()]
    public void DecoderTest()
    {
      string path = AssemblyDirectory + "\\TestResource\\ED Data Test.zip";
      byte[] item = File.ReadAllBytes(path);
      byte[] expected = null;
      string temp;      
      temp = Base64.Encoder(item);
      expected = Base64.Decoder(temp);
      //Below is just for debugging, it writes file back out to system
      //File.WriteAllBytes(path + ".new", expected);

      bool same = item.SequenceEqual(expected);
      if (!same)
        Assert.Fail("Base64 encoding failed to encode and decode to the same byte array.");       
    }

    /// <summary>
    ///A test for Encoder
    ///</summary>
    [TestMethod()]
    public void EncoderTest()
    {
      //See decodetTest above which tests both encode and decode in one.
    }
  }
}

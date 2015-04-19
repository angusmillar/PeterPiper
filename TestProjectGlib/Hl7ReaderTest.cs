using Glib.Hl7.V2.Support.TextFile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestProjectGlib
{
    
    
    /// <summary>
    ///This is a test class for Hl7ReaderTest and is intended
    ///to contain all Hl7ReaderTest Unit Tests
    ///</summary>
  [TestClass()]
  public class Hl7ReaderTest
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
    ///A test for Hl7Reader Constructor
    ///</summary>
    [TestMethod()]
    public void Hl7ReaderConstructorTest()
    {
      string path = AssemblyDirectory;

      //This is to work around VS2013 Community not running test cases like VS2010 Pro did.
      if (path.Contains(@"\TestResults\"))
      {
        path = path.Substring(0, path.IndexOf(@"\TestResults\")) + @"\TestProjectGlib\TestResource\TestSetOfMsg.dat";        
      }

      Hl7StreamReader target = new Hl7StreamReader(path);      
    }

    /// <summary>
    ///A test for Hl7Reader Constructor
    ///</summary>
    [TestMethod()]
    public void Hl7ReaderConstructorTest1()
    {
      //Stream stream = null; // TODO: Initialize to an appropriate value
      //Hl7Reader target = new Hl7Reader(stream);
      
    }

    /// <summary>
    ///A test for Close
    ///</summary>
    [TestMethod()]
    public void CloseTest()
    {
      //Stream stream = null; // TODO: Initialize to an appropriate value
      //Hl7Reader target = new Hl7Reader(stream); // TODO: Initialize to an appropriate value
      //target.Close();
      //Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test for Read
    ///</summary>
    [TestMethod()]
    public void ReadTest()
    {
      string path = AssemblyDirectory;      
      //This is to work around VS2013 Community not running test cases like VS2010 Pro did.
      if (path.Contains(@"\TestResults\"))
      {
        path = path.Substring(0, path.IndexOf(@"\TestResults\")) + @"\TestProjectGlib";
      }
      string readpath = path + "\\TestResource\\TestSetOfMsg.dat";
      string writepath = path + "\\TestResource\\TestSetOfMsg2.dat";

      Hl7StreamReader reader = new Hl7StreamReader(readpath);
      HL7StreamWriter writer = new HL7StreamWriter(writepath, true);
      List<Glib.Hl7.V2.Model.Message> oMessageList = new List<Glib.Hl7.V2.Model.Message>();
      string actual;
      while ((actual = reader.Read()) != null)
      {
        Glib.Hl7.V2.Model.Message oHl7 = new Glib.Hl7.V2.Model.Message(actual);        
        Assert.IsTrue(oHl7.SegmentCount() > 1);
        oMessageList.Add(oHl7);
        writer.Write(oHl7, HL7StreamWriter.HL7OutputStyles.InterfaceReadable);
      }
      reader.Close();

      reader = new Hl7StreamReader(writepath);
      int counter = 0;
      while ((actual = reader.Read()) != null)
      {
        Glib.Hl7.V2.Model.Message oHl7 = new Glib.Hl7.V2.Model.Message(actual);
        if (!oMessageList[counter].AsStringRaw.Equals(oHl7.AsStringRaw))
        {
          Assert.Fail("The Hl7 Message read in does not match the hl7 message writen out and back in again bu the Hl7StreamWriter and Hl7StreamReader.");
        }
        counter++;
      }
      reader.Close();
      File.Delete(writepath);            
    }
  }
}

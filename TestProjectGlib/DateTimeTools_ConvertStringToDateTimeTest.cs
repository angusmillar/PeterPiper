using Glib.Hl7.V2.Support.Content;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProjectGlib
{
    
    
    /// <summary>
    ///This is a test class for DateTimeTools_ConvertStringToDateTimeTest and is intended
    ///to contain all DateTimeTools_ConvertStringToDateTimeTest Unit Tests
    ///</summary>
  [TestClass()]
  public class DateTimeTools_ConvertStringToDateTimeTest
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


    /// <summary>
    ///A test for AsDateTime
    ///</summary>
    [TestMethod()]
    public void AsDateTimeTest()
    {
      string HL7StandardDateTimeString = "20140225083022.5190+08:00";
      DateTime expected = new DateTime(2014,02,25,10,30,22,519,DateTimeKind.Local);
      DateTime actual;
      actual = DateTimeTools.ConvertStringToDateTime.AsDateTime(HL7StandardDateTimeString);
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for HasMilliseconds
    ///</summary>
    [TestMethod()]
    [DeploymentItem("Glib.dll")]
    public void HasMillisecondsTest()
    {
      string String = "20140225083022.5190"; ; 
      string expected = "yyyyMMddHHmmss.ffff";
      string actual;
      actual = DateTimeTools_Accessor.ConvertStringToDateTime.HasMilliseconds(String);
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for HasNoMillisecondsOrTimeZone
    ///</summary>
    [TestMethod()]
    [DeploymentItem("Glib.dll")]
    public void HasNoMillisecondsOrTimeZoneTest()
    {
      string String = "20140225083022"; ; 
      string expected = "yyyyMMddHHmmss";
      string actual;
      actual = DateTimeTools_Accessor.ConvertStringToDateTime.HasNoMillisecondsOrTimeZone(String);
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for HasTimeMillisecondsAndTimeZone
    ///</summary>
    [TestMethod()]
    [DeploymentItem("Glib.dll")]
    public void HasTimeMillisecondsAndTimeZoneTest()
    {
      string String = "20140225083022.5190+08:00"; ; 
      string expected = "yyyyMMddHHmmss.ffffzzzzz"; 
      string actual;
      actual = DateTimeTools_Accessor.ConvertStringToDateTime.HasTimeMillisecondsAndTimeZone(String);
      Assert.AreEqual(expected, actual);

    }

    /// <summary>
    ///A test for HasTimeZone
    ///</summary>
    [TestMethod()]
    [DeploymentItem("Glib.dll")]
    public void HasTimeZoneTest()
    {
      string String = "20140225083022+08:00"; ; 
      string expected = "yyyyMMddHHmmsszzzzz"; 
      string actual;
      actual = DateTimeTools_Accessor.ConvertStringToDateTime.HasTimeZone(String);
      Assert.AreEqual(expected, actual);      
    }
  }
}

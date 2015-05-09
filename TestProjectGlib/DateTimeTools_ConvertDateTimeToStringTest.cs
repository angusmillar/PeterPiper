using Glib.Hl7.V2.Support.Content;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProjectGlib
{
    
    
    /// <summary>
    ///This is a test class for DateTimeTools_ConvertDateTimeToStringTest and is intended
    ///to contain all DateTimeTools_ConvertDateTimeToStringTest Unit Tests
    ///</summary>
  [TestClass()]
  public class DateTimeTools_ConvertDateTimeToStringTest
  {


    private TestContext testContextInstance;
    DateTime oDateTime = new DateTime(2014, 02, 25, 10, 30, 22, 519, DateTimeKind.Local);
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
    ///A test for AsDate
    ///</summary>
    [TestMethod()]
    public void AsDateTest()
    {      
      string expected = "20140225"; // TODO: Initialize to an appropriate value
      string actual;
      actual = DateTimeTools.ConvertDateTimeToString.AsDate(oDateTime);
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for AsDate
    ///</summary>
    [TestMethod()]
    public void AsDateTest1()
    {      
      TimeSpan OffSetTime = new TimeSpan(+8,0,0);
      string expected = "20140225+08:00";
      string actual;
      actual = DateTimeTools.ConvertDateTimeToString.AsDate(oDateTime, OffSetTime);
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for AsDateHourMin
    ///</summary>
    [TestMethod()]
    public void AsDateHourMinTest()
    {      
      string expected = "201402251030";
      string actual;
      actual = DateTimeTools.ConvertDateTimeToString.AsDateHourMin(oDateTime);
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for AsDateHourMin
    ///</summary>
    [TestMethod()]
    public void AsDateHourMinTest1()
    {      
      TimeSpan OffSetTime = new TimeSpan(+8,0,0); 
      string expected = "201402250830+0800";
      string actual;
      actual = DateTimeTools.ConvertDateTimeToString.AsDateHourMin(oDateTime, OffSetTime);
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for AsDateHourMinSec
    ///</summary>
    [TestMethod()]
    public void AsDateHourMinSecTest()
    {      
      string expected = "20140225103022";
      string actual;
      actual = DateTimeTools.ConvertDateTimeToString.AsDateHourMinSec(oDateTime);
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for AsDateHourMinSec
    ///</summary>
    [TestMethod()]
    public void AsDateHourMinSecTest1()
    {
      
      TimeSpan OffSetTime = new TimeSpan(+8,0,0); 
      string expected = "20140225083022+0800";
      string actual;
      actual = DateTimeTools.ConvertDateTimeToString.AsDateHourMinSec(oDateTime, OffSetTime);
      Assert.AreEqual(expected, actual);    
    }

    /// <summary>
    ///A test for AsDateHourMinSecMilli
    ///</summary>
    [TestMethod()]
    public void AsDateHourMinSecMilliTest()
    {      
      string expected = "20140225103022.5190";
      string actual;
      actual = DateTimeTools.ConvertDateTimeToString.AsDateHourMinSecMilli(oDateTime);
      Assert.AreEqual(expected, actual);      
    }

    /// <summary>
    ///A test for AsDateHourMinSecMilli
    ///</summary>
    [TestMethod()]
    public void AsDateHourMinSecMilliTest1()
    {      
      TimeSpan OffSetTime = new TimeSpan(+8,0,0); 
      string expected = "20140225083022.5190+0800";
      string actual;
      actual = DateTimeTools.ConvertDateTimeToString.AsDateHourMinSecMilli(oDateTime, OffSetTime);
      Assert.AreEqual(expected, actual);      
    }
  }
}

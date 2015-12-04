using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Glib.Hl7.V2.Model;
using Glib.Hl7.V2.Support;
using Glib.Hl7.V2.Support.Content;

namespace TestHl7V2.TestModel
{
  [TestFixture]
  public class TestDateTimeTools_ConvertDateTimeToString
  {
    DateTimeOffset oDateTimeOffSet = new DateTimeOffset(2014, 02, 25, 10, 30, 22, 519, new TimeSpan(+10, 0, 0));     
    
    [SetUp]
    public void MyTestInitialize()
    {     
    }

    /// <summary>
    ///A test for AsDate
    ///</summary>
    [Test]
    public void AsDateTest()
    {
      string expected = "20140225"; 
      string actual;
      actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDate(oDateTimeOffSet);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDate
    ///</summary>
    [Test]
    public void AsDateTest1()
    {
      
      string expected = "20140225+08:00";
      string actual;
      actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDate(oDateTimeOffSet.ToOffset(new TimeSpan(+8, 0, 0)), true);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMin
    ///</summary>
    [Test]
    public void AsDateHourMinTest()
    {
      string expected = "201402251030";
      string actual;
      actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMin(oDateTimeOffSet);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMin
    ///</summary>
    [Test]
    public void AsDateHourMinTest1()
    {         
      string expected = "201402250830+0800";
      string actual;
      actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMin(oDateTimeOffSet.ToOffset(new TimeSpan(+8, 0, 0)), true);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMinSec
    ///</summary>
    [Test]
    public void AsDateHourMinSecTest()
    {
      string expected = "20140225103022";
      string actual;
      actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSec(oDateTimeOffSet);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMinSec
    ///</summary>
    [Test]
    public void AsDateHourMinSecTest1()
    {      
      string expected = "20140225083022+0800";
      string actual;
      actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSec(oDateTimeOffSet.ToOffset(new TimeSpan(+8,0,0)), true);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMinSecMilli
    ///</summary>
    [Test]
    public void AsDateHourMinSecMilliTest()
    {
      string expected = "20140225103022.5190";
      string actual;
      actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSecMilli(oDateTimeOffSet);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMinSecMilli
    ///</summary>
    [Test]
    public void AsDateHourMinSecMilliTest1()
    {      
      string expected = "20140225083022.5190+0800";
      string actual;
      actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSecMilli(oDateTimeOffSet.ToOffset(new TimeSpan(+8,0,0)),true);
      Assert.AreEqual(expected, actual);
    }


  }
}

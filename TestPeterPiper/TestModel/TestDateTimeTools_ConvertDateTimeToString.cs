﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support;
using PeterPiper.Hl7.V2.Support.Tools;

namespace TestPeterPiper.TestModel
{
  [TestClass]
  public class TestDateTimeTools_ConvertDateTimeToString
  {
    DateTimeOffset oDateTimeOffSet = new DateTimeOffset(2014, 02, 25, 10, 30, 22, 519, new TimeSpan(+10, 0, 0));     
    
    [TestInitialize]
    public void MyTestInitialize()
    {     
    }

    /// <summary>
    ///A test for AsDate
    ///</summary>
    [TestMethod]
    public void AsDateTest()
    {
      string expected = "20140225"; 
      string actual;
      actual = DateTimeSupportTools.AsString(oDateTimeOffSet, false, DateTimeSupportTools.DateTimePrecision.Date);
      //actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDate(oDateTimeOffSet);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDate
    ///</summary>
    [TestMethod]
    public void AsDateTest1()
    {
      
      string expected = "20140225+0800";
      string actual;
      //actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDate(oDateTimeOffSet.ToOffset(new TimeSpan(+8, 0, 0)), true);
      actual = DateTimeSupportTools.AsString(oDateTimeOffSet.ToOffset(new TimeSpan(+8, 0, 0)), true, DateTimeSupportTools.DateTimePrecision.Date);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMin
    ///</summary>
    [TestMethod]
    public void AsDateHourMinTest()
    {
      string expected = "201402251030";
      string actual;
      //actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMin(oDateTimeOffSet);
      actual = DateTimeSupportTools.AsString(oDateTimeOffSet, false, DateTimeSupportTools.DateTimePrecision.DateHourMin);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMin
    ///</summary>
    [TestMethod]
    public void AsDateHourMinTest1()
    {         
      string expected = "201402250830+0800";
      string actual;
      //actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMin(oDateTimeOffSet.ToOffset(new TimeSpan(+8, 0, 0)), true);
      actual = DateTimeSupportTools.AsString(oDateTimeOffSet.ToOffset(new TimeSpan(+8, 0, 0)), true, DateTimeSupportTools.DateTimePrecision.DateHourMin);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMinSec
    ///</summary>
    [TestMethod]
    public void AsDateHourMinSecTest()
    {
      string expected = "20140225103022";
      string actual;
      //actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSec(oDateTimeOffSet);
      actual = DateTimeSupportTools.AsString(oDateTimeOffSet, false, DateTimeSupportTools.DateTimePrecision.DateHourMinSec);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMinSec
    ///</summary>
    [TestMethod]
    public void AsDateHourMinSecTest1()
    {      
      string expected = "20140225083022+0800";
      string actual;
      //actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSec(oDateTimeOffSet.ToOffset(new TimeSpan(+8,0,0)), true);
      actual = DateTimeSupportTools.AsString(oDateTimeOffSet.ToOffset(new TimeSpan(+8, 0, 0)), true, DateTimeSupportTools.DateTimePrecision.DateHourMinSec);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMinSecMilli
    ///</summary>
    [TestMethod]
    public void AsDateHourMinSecMilliTest()
    {
      string expected = "20140225103022.5190";
      string actual;
      //actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSecMilli(oDateTimeOffSet);
      actual = DateTimeSupportTools.AsString(oDateTimeOffSet, false, DateTimeSupportTools.DateTimePrecision.DateHourMinSecMilli);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for AsDateHourMinSecMilli
    ///</summary>
    [TestMethod]
    public void AsDateHourMinSecMilliTest1()
    {      
      string expected = "20140225083022.5190+0800";
      string actual;
      //actual = DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSecMilli(oDateTimeOffSet.ToOffset(new TimeSpan(+8,0,0)),true);
      actual = DateTimeSupportTools.AsString(oDateTimeOffSet.ToOffset(new TimeSpan(+8, 0, 0)), true, DateTimeSupportTools.DateTimePrecision.DateHourMinSecMilli);
      Assert.AreEqual(expected, actual);
    }


  }
}

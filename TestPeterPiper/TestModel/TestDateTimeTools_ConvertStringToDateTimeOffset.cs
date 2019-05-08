using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeterPiper.Hl7.V2.Support.Tools;
using PeterPiper.Hl7.V2.Model;

namespace TestPeterPiper.TestModel
{
  [TestClass]
  public class TestDateTimeTools_ConvertStringToDateTimeOffset
  {


    /// <summary>
    ///A test for AsDateTime
    ///</summary>
    [TestMethod]
    public void AsDateTimeTest()
    {
      string HL7StandardDateTimeString = "20140225083022.5190+0800";
      DateTimeOffset expected = new DateTimeOffset(2014, 02, 25, 08, 30, 22, 519, new TimeSpan(+8, 0, 0));
      DateTimeOffset actual;
      //actual = DateTimeTools.ConvertStringToDateTime.AsDateTimeOffset(HL7StandardDateTimeString);
      actual = DateTimeSupportTools.AsDateTimeOffSet(HL7StandardDateTimeString);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for HasMilliseconds
    ///</summary>
    [TestMethod]
    public void HasMillisecondsTest()
    {
      string String = "20140225083022.519"; ;
      var oField = Creator.Field(String);
      DateTimeOffset actual = oField.Convert.DateTime.GetDateTimeOffset();      
      DateTimeOffset expected = new DateTimeOffset(2014, 02, 25, 08, 30, 22, 519, new TimeSpan(0,0,0));     
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for HasNoMillisecondsOrTimeZone
    ///</summary>
    [TestMethod]
    public void HasNoMillisecondsOrTimeZoneTest()
    {
      string String = "20140225083022";
      var oField = Creator.Field(String);
      DateTimeOffset actual = oField.Convert.DateTime.GetDateTimeOffset();      
      DateTimeOffset expected = new DateTimeOffset(2014, 02, 25, 08, 30, 22, new TimeSpan(0,0,0));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for HasTimeMillisecondsAndTimeZone
    ///</summary>
    [TestMethod]    
    public void HasTimeMillisecondsAndTimeZoneTest()
    {
      string String = "20140225083022.519+0800";
      var oField = Creator.Field(String);
      DateTimeOffset actual = oField.Convert.DateTime.GetDateTimeOffset();
      
      DateTimeOffset expected = new DateTimeOffset(2014, 02, 25, 08, 30, 22,519, new TimeSpan(+8,0,0));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for HasTimeZone
    ///</summary>
    [TestMethod]    
    public void HasTimeZoneTest()
    {
      string String = "20140225083022+0800";
      var oField = Creator.Field(String);
      DateTimeOffset actual = oField.Convert.DateTime.GetDateTimeOffset();
      
      DateTimeOffset expected = new DateTimeOffset(2014, 02, 25, 08, 30, 22, new TimeSpan(+8, 0, 0));      Assert.AreEqual(expected, actual);

    }

    /// <summary>
    ///New Date Time Support
    ///</summary>
    [TestMethod]
    public void Hl7DateTimeTest()
    {
      string String = "20140225083022"; ;
      var oField = Creator.Field(String);
      string actual = string.Empty;
      if (oField.Convert.DateTime.CanParseToDateTimeOffset)
        actual = oField.Convert.DateTime.AsString(false, DateTimeSupportTools.DateTimePrecision.Year);
            
      string expected = "2014";
      Assert.AreEqual(expected, actual);
    }



  }
}

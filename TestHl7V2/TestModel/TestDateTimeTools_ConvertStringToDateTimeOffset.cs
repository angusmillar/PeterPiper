using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PeterPiper.Hl7.V2.Support.Content;
using PeterPiper.Hl7.V2.Model;

namespace TestHl7V2.TestModel
{
  [TestFixture]
  public class TestDateTimeTools_ConvertStringToDateTimeOffset
  {


    /// <summary>
    ///A test for AsDateTime
    ///</summary>
    [Test]
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
    [Test]
    public void HasMillisecondsTest()
    {
      string String = "20140225083022.519"; ;
      var oField = Creator.Field(String);
      DateTimeOffset actual = oField.DateTimeSupport.GetDateTimeOffset();      
      DateTimeOffset expected = new DateTimeOffset(2014, 02, 25, 08, 30, 22, 519, new TimeSpan(0,0,0));     
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for HasNoMillisecondsOrTimeZone
    ///</summary>
    [Test]
    public void HasNoMillisecondsOrTimeZoneTest()
    {
      string String = "20140225083022";
      var oField = Creator.Field(String);
      DateTimeOffset actual = oField.DateTimeSupport.GetDateTimeOffset();      
      DateTimeOffset expected = new DateTimeOffset(2014, 02, 25, 08, 30, 22, new TimeSpan(0,0,0));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for HasTimeMillisecondsAndTimeZone
    ///</summary>
    [Test]    
    public void HasTimeMillisecondsAndTimeZoneTest()
    {
      string String = "20140225083022.519+0800";
      var oField = Creator.Field(String);
      DateTimeOffset actual = oField.DateTimeSupport.GetDateTimeOffset();
      var zone = TimeZone.CurrentTimeZone;
      DateTimeOffset expected = new DateTimeOffset(2014, 02, 25, 08, 30, 22,519, new TimeSpan(+8,0,0));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for HasTimeZone
    ///</summary>
    [Test]    
    public void HasTimeZoneTest()
    {
      string String = "20140225083022+0800";
      var oField = Creator.Field(String);
      DateTimeOffset actual = oField.DateTimeSupport.GetDateTimeOffset();
      var zone = TimeZone.CurrentTimeZone;
      DateTimeOffset expected = new DateTimeOffset(2014, 02, 25, 08, 30, 22, new TimeSpan(+8, 0, 0));      Assert.AreEqual(expected, actual);

    }

    /// <summary>
    ///New Date Time Support
    ///</summary>
    [Test]
    public void Hl7DateTimeTest()
    {
      string String = "20140225083022"; ;
      var oField = Creator.Field(String);
      string actual = string.Empty;
      if (oField.DateTimeSupport.CanParseToDateTimeOffset)
        actual = oField.DateTimeSupport.AsString(false, DateTimeSupportTools.DateTimePrecision.Year);
      var zone = TimeZone.CurrentTimeZone;
      
      string expected = "2014";
      Assert.AreEqual(expected, actual);
    }



  }
}

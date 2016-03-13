using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PeterPiper.Hl7.V2.Model;

namespace TestHl7V2.TestModel
{
  [TestFixture]
  public class TestHl7DateTimeSupport
  {
    [Test]
    public void TestHasPrecision()
    {
      string HL7StandardDateTimeString = "20140225083022.5190+0800";
      Field oField = new Field(HL7StandardDateTimeString);
      //Year
      string actual = oField.DateTimeSupport.AsString(false, PeterPiper.Hl7.V2.Support.Content.DateTimeSupportTools.DateTimePrecision.Year);
      string expected = "2014";
      Assert.AreEqual(expected, actual);

      //YearMonth
      actual = oField.DateTimeSupport.AsString(false, PeterPiper.Hl7.V2.Support.Content.DateTimeSupportTools.DateTimePrecision.YearMonth);
      expected = "201402";
      Assert.AreEqual(expected, actual);

      //Date
      actual = oField.DateTimeSupport.AsString(false, PeterPiper.Hl7.V2.Support.Content.DateTimeSupportTools.DateTimePrecision.Date);
      expected = "20140225";
      Assert.AreEqual(expected, actual);

      //Date Hour Min
      actual = oField.DateTimeSupport.AsString(false, PeterPiper.Hl7.V2.Support.Content.DateTimeSupportTools.DateTimePrecision.DateHourMin);
      expected = "201402250830";
      Assert.AreEqual(expected, actual);

      //Date Hour Min Sec
      actual = oField.DateTimeSupport.AsString(false, PeterPiper.Hl7.V2.Support.Content.DateTimeSupportTools.DateTimePrecision.DateHourMinSec);
      expected = "20140225083022";
      Assert.AreEqual(expected, actual);

      //Date Hour Min Sec Milli
      actual = oField.DateTimeSupport.AsString(false, PeterPiper.Hl7.V2.Support.Content.DateTimeSupportTools.DateTimePrecision.DateHourMinSecMilli);
      expected = "20140225083022.5190";
      Assert.AreEqual(expected, actual);

      //Date Hour Min Sec Milli +timezone
      actual = oField.DateTimeSupport.AsString(true, PeterPiper.Hl7.V2.Support.Content.DateTimeSupportTools.DateTimePrecision.DateHourMinSecMilli);
      expected = "20140225083022.5190+0800";
      Assert.AreEqual(expected, actual);

      //Original Format
      actual = oField.DateTimeSupport.AsString();
      expected = "20140225083022.5190+0800";
      Assert.AreEqual(expected, actual);

      //Check Precision
      actual = oField.DateTimeSupport.AsString();
      var expectedPrecision = new PeterPiper.Hl7.V2.Support.Content.DateTimeSupportTools.DateTimePrecision();
      expectedPrecision = PeterPiper.Hl7.V2.Support.Content.DateTimeSupportTools.DateTimePrecision.DateHourMinSecMilli;
      var actualPrecision = oField.DateTimeSupport.GetPrecision();
      Assert.AreEqual(expectedPrecision, actualPrecision);
    }

    [Test]
    public void TestHasTimezone()
    {
      //with timezone
      string HL7StandardDateTimeString = "20140225083022.5190+0800";
      Field oField = new Field(HL7StandardDateTimeString);
      bool expected = true;
      bool actual = oField.DateTimeSupport.HasTimezone;
      Assert.AreEqual(expected, actual);

      //Without timezone
      HL7StandardDateTimeString = "20140225083022.5190+0700";
      oField = new Field(HL7StandardDateTimeString);
      expected = true;
      actual = oField.DateTimeSupport.HasTimezone;
      Assert.AreEqual(expected, actual);

      oField.DateTimeSupport.SetTimezone(new TimeSpan(+6, 01, 0));
      
      
      actual = oField.DateTimeSupport.HasTimezone;
      expected = true;
      Assert.AreEqual(expected, actual);
      
      //Set as new DateTime with new timezone and set the string to now have a timezone and only sec precision
      HL7StandardDateTimeString = "20140224084022+0800";
      DateTimeOffset expectedDateTime = new DateTimeOffset(2014, 02, 24, 08, 40, 22, new TimeSpan(+8, 0, 0));
      //Tell the content to use new settings for timezone and precision
      oField.AsString = oField.DateTimeSupport.AsString(true, PeterPiper.Hl7.V2.Support.Content.DateTimeSupportTools.DateTimePrecision.DateHourMinSec);
      //Set the new datetime 
      oField.DateTimeSupport.SetDateTimeOffset(expectedDateTime);
      Assert.AreEqual(HL7StandardDateTimeString, oField.DateTimeSupport.AsString());

    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.Hl7.V2.Model;

namespace Glib.Hl7.V2.Support.Content
{
  public class DateTimeSupport
  {
    private ContentBase _ContentBase;

    internal DateTimeSupport(ContentBase ContentBase)
    {
      _ContentBase = ContentBase;
    }

    /// <summary>
    /// Returns True is the HL7 DateTime has a timezone present.     
    /// </summary>
    public bool HasTimezone
    {
      get
      {
        return DateTimeSupportTools.HasTimezone(_ContentBase.AsString);
      }
    }

    /// <summary>
    /// Returns a TimeSpan equal to the timezone found in the HL7 DateTime string.
    /// Throws a formatException if no timezone is present, please test with 'HasTimezone' before calling.    
    /// </summary>
    /// <returns>HL7 string representation of the timezone</returns>
    public TimeSpan GetTimezone()
    {
      return DateTimeSupportTools.GetTimezone(_ContentBase.AsString);
    }

    /// <summary>
    /// Set the timezone for a give HL7 DateTime. This will convert the datetime from the timezone present to the new timezone. 
    /// If no timezone is present in the HL7 datetime string then it will assume that this datetime is from the new timezone and not convert. 
    /// </summary>
    /// <param name="Timespan"></param>
    public void SetTimezone(TimeSpan Timespan)
    {
      _ContentBase.AsString = DateTimeSupportTools.SetTimezone(_ContentBase.AsString, Timespan);
    }

    /// <summary>
    /// Get the precision of the HL7 Datetime string
    /// Returns None if the string is not a HL7 datetime string of empty 
    /// </summary>
    /// <returns></returns>
    public DateTimeSupportTools.DateTimePrecision GetPrecision()
    {
      return DateTimeSupportTools.GetPrecision(_ContentBase.AsString);
    }

    /// <summary>
    /// Returns true if the given string can be parsed to a DateTimeOffset data type
    /// </summary>
    public bool CanParseToDateTimeOffset
    {
      get
      {
        return DateTimeSupportTools.CanParseToDateTimeOffset(_ContentBase.AsString);
      }
    }

    /// <summary>
    /// Returns a DateTimeOffset if the HL7 datetime string can be parsed as a DateTimeOffset.
    /// Throws a FormatException if the HL7 datetime string can not be parsed.
    /// </summary>
    /// <returns></returns>
    public DateTimeOffset GetDateTimeOffset()
    {
      return DateTimeSupportTools.AsDateTimeOffSet(_ContentBase.AsString);
    }

    /// <summary>
    /// Sets the HL7 DateTime string from the given DateTimeOffset.
    /// Will also set the HL7 dateTime timezone if 'HasTimezone' is set to true [Defaults to True.
    /// Will set the HL7 dateTime string Precision to the 'DateTimePrecision' given [Defaults to DateHourMinSec].
    /// </summary>
    /// <param name="DateTimeOffset"></param>
    /// <param name="HasTimezone"></param>
    /// <param name="DateTimePrecision"></param>
    public void SetDateTimeOffset(DateTimeOffset DateTimeOffset, bool HasTimezone = true, DateTimeSupportTools.DateTimePrecision DateTimePrecision = DateTimeSupportTools.DateTimePrecision.DateHourMinSec)
    {
      _ContentBase.AsString = DateTimeSupportTools.AsString(DateTimeOffset, HasTimezone, DateTimePrecision);
    }

    /// <summary>
    /// Returns the HL7 DateTime string set with the timezone and Precision given 
    /// </summary>
    /// <param name="WithTimezone"></param>
    /// <param name="WithPrecision"></param>
    /// <returns></returns>
    public string AsString(bool WithTimezone, DateTimeSupportTools.DateTimePrecision WithPrecision)
    {
      return DateTimeSupportTools.AsString(DateTimeSupportTools.AsDateTimeOffSet(_ContentBase.AsString), WithTimezone, WithPrecision);
    }

    /// <summary>
    /// Returns the string that is currently set.
    /// </summary>
    /// <returns></returns>
    public string AsString()
    {
      return _ContentBase.AsString;
      //return Hl7DateTimeSupport.AsString(Hl7DateTimeSupport.AsDateTimeOffSet(_ContentBase.AsString), Hl7DateTimeSupport.HasTimezone(_ContentBase.AsString), Hl7DateTimeSupport.GetPrecision(_ContentBase.AsString));
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model;
using System.Globalization;

namespace PeterPiper.Hl7.V2.Support.Content
{
  public static class  DateTimeSupportTools
  {
    #region Private Properties
        
    private static string FormatExceptionMessage = "The Content does not match the allowed HL7 Standard datetime formats. Found: '{0}',  Allowed Format: YYYY[MM[DD[HH[MM[SS[.S[S[S[S]]]]]]]]][+/-ZZZZ].";    
    private static string[] formats = new string[] { "yyyy", "yyyyMM", "yyyyMMdd", "yyyyMMddHH", "yyyyMMddHHmm", "yyyyMMddHHmmss",
                                              "yyyyzzzzz", "yyyyMMzzzzz", "yyyyMMddzzzzz", "yyyyMMddHHzzzzz", "yyyyMMddHHmmzzzzz", "yyyyMMddHHmmsszzzzz",
                                              "yyyyMMddHHmmss.f", "yyyyMMddHHmmss.ff", "yyyyMMddHHmmss.fff", "yyyyMMddHHmmss.ffff", "yyyyMMddHHmmss.ffffzzzzz",
                                              "yyyyMMddHHmmss.fzzzzz", "yyyyMMddHHmmss.ffzzzzz", "yyyyMMddHHmmss.fffzzzzz", "yyyyMMddHHmmss.ffffzzzzz"};
    #endregion

    #region Public Properties

    public enum DateTimePrecision { None, Year, YearMonth, Date, DateHourMin, DateHourMinSec, DateHourMinSecMilli };
    
    #endregion

    #region Constructor
    
    #endregion

    #region Public Methods

    /// <summary>
    /// Returns a DateTimeOffset for the Hl7 DateTime string passed in.
    /// If the string passed in can not be parsed as a dateTime then a FormatException is thrown.
    /// </summary>
    /// <param name="Hl7DateTimeString"></param>
    /// <returns></returns>
    public static DateTimeOffset AsDateTimeOffSet(string Hl7DateTimeString)
    {
      DateTimeOffset result;
      if (TryParseDateTimeString(Hl7DateTimeString, out result))
        return result;
      else
        throw new FormatException(String.Format(FormatExceptionMessage, Hl7DateTimeString));
    }
    
    /// <summary>
    /// Returns True if a Timezone element is found in the Hl7 DateTime string.
    /// If the string passed in cannot be parsed as a dateTime then a FormatException is thrown.
    /// </summary>
    /// <param name="Hl7DateTimeString"></param>
    /// <returns></returns>
    public static bool HasTimezone(string Hl7DateTimeString)
    {
      if (DateTimeSupportTools.CanParseToDateTimeOffset(Hl7DateTimeString))
        return CheckForTimezone(Hl7DateTimeString);
      else
        throw new FormatException(String.Format(FormatExceptionMessage, Hl7DateTimeString));           
    }

    /// <summary>
    /// Returns a Timespan that represents the timezone found in the HL7 DateTime string.
    /// Throws a FormatException if no timezone present or if the content is empty
    /// </summary>
    /// <param name="Hl7DateTimeString"></param>
    /// <returns></returns>
    public static TimeSpan GetTimezone(string Hl7DateTimeString)
    {
      if (DateTimeSupportTools.HasTimezone(Hl7DateTimeString))
      {
        char[] TimeZomeDelimiter = { '+', '-' };
        try
        {
          var TimeZoneString = Hl7DateTimeString.Substring(Hl7DateTimeString.LastIndexOfAny(TimeZomeDelimiter), Hl7DateTimeString.Length - Hl7DateTimeString.LastIndexOfAny(TimeZomeDelimiter));
          int Hours = Convert.ToInt32(TimeZoneString.Substring(0, 3));
          int min = Convert.ToInt32(TimeZoneString.Substring(3, 2));
          return new TimeSpan(Hours, min, 0);
        }
        catch(Exception Exec)
        {
          throw new FormatException(String.Format("Unable to parse timezone from HL7 Datetime string of: {0}", Hl7DateTimeString), Exec);
        }
      }
      else
      {
        throw new FormatException(String.Format("No timezone present in given content. Try testing for timezone by calling 'HasTimezone' before calling 'GetTimezone'."));
      }
    }

    /// <summary>
    /// Set the timezone for a give HL7 DateTime string. This will convert the datetime from the timezone present to the new timezone. 
    /// If no timezone is present in the HL7 datetime string then it will assume that this datetime is from the new timezone and not convert.
    /// Will throw a FormatException if the HL7 DateTime string is no able to be parsed as a DateTimeOffset.
    /// </summary>
    /// <param name="Hl7DateTimeString"></param>
    /// <param name="Timespan"></param>
    /// <returns></returns>
    public static string SetTimezone(string Hl7DateTimeString, TimeSpan Timespan)
    {
      if (DateTimeSupportTools.HasTimezone(Hl7DateTimeString))
      {
        var DateTime = DateTimeSupportTools.AsDateTimeOffSet(Hl7DateTimeString);
        DateTime = DateTime.ToOffset(Timespan);
        return DateTimeSupportTools.AsString(DateTime, true, DateTimeSupportTools.GetPrecision(Hl7DateTimeString));
      }
      else
      {
        return Hl7DateTimeString + String.Format("{0:+00;-00}{1:00}", Timespan.Hours, Timespan.Minutes);
      }
    }

    /// <summary>
    /// Returns the Precision found in the Hl7 DateTime string passed in. 
    /// If the string passed in cannot be parsed as a HL7 dateTime string then a FormatException is thrown.
    /// </summary>
    /// <param name="Hl7DateTimeString"></param>
    /// <returns></returns>
    public static DateTimePrecision GetPrecision(string Hl7DateTimeString)
    {
      if (DateTimeSupportTools.CanParseToDateTimeOffset(Hl7DateTimeString))
        return CalculateDateTimePrecision(Hl7DateTimeString);
      else
        return DateTimePrecision.None;
    }

    /// <summary>
    /// Returns True if the Hl7 DateTime string can be parsed to a DateTimeOffset, or False if unable;
    /// </summary>
    /// <param name="Hl7DateTimeString"></param>
    /// <returns></returns>
    public static bool CanParseToDateTimeOffset(string Hl7DateTimeString)
    {
      DateTimeOffset result;
      return TryParseDateTimeString(Hl7DateTimeString, out result);   
    }    
   
    /// <summary>
    /// Returns the Hl7 DateTime string with or with out a timezone and to the precision given;
    /// </summary>
    /// <param name="WithTimezone"></param>
    /// <param name="WithPrecision"></param>
    /// <returns></returns>
    public static string AsString(DateTimeOffset DateTimeOffSet, bool WithTimezone, DateTimePrecision WithPrecision)
    {
      return GetDateTimeOffSetAsHl7String(DateTimeOffSet, WithTimezone, WithPrecision);      
    }

    #endregion

    #region Private Methods

    private static bool TryParseDateTimeString(string DateTimeString, out DateTimeOffset result)
    {
      if (DateTimeString.Length < 4)
      {
        result = DateTimeOffset.MinValue;
        return false;
      }
      IFormatProvider provider = CultureInfo.InvariantCulture.DateTimeFormat;
      if (DateTimeOffset.TryParseExact(DateTimeString, formats, provider, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out result))
        return true;
      else
        return false;
    }

    private static bool CheckForTimezone(string DateTimeString)
    {
      char[] TimeZomeDelimiter = { '+', '-' };
      if (DateTimeString.IndexOfAny(TimeZomeDelimiter) > 0)
        return true;
      else
        return false;
    }
    
    private static DateTimePrecision CalculateDateTimePrecision(string DateTimeString)
    {
      char[] MilliSecondsDelimiter = { '.' };
      char[] TimeZomeDelimiter = { '+', '-' };
      string TempDateTimeString = DateTimeString;
      if ((TempDateTimeString.IndexOfAny(TimeZomeDelimiter) > 0))
      {
        TempDateTimeString = TempDateTimeString.Remove(TempDateTimeString.LastIndexOfAny(TimeZomeDelimiter));
      }

      if (TempDateTimeString.IndexOfAny(MilliSecondsDelimiter) > 0)
      {
        return DateTimePrecision.DateHourMinSecMilli;
      }
      else
      {
        switch (TempDateTimeString.Length)
        {
          case 4:
            return DateTimePrecision.Year;            
          case 6:
            return DateTimePrecision.YearMonth;            
          case 8:
            return DateTimePrecision.Date;            
          case 12:
            return DateTimePrecision.DateHourMin;            
          case 14:
            return DateTimePrecision.DateHourMinSec;                     
          default:
            return DateTimePrecision.None;
        }                  
      }          
    }

    private static string GetDateTimeOffSetAsHl7String(DateTimeOffset TargetDateTimeOffset,  bool WithTimezone, DateTimePrecision WithPrecision)
    {      
      string fYear = "yyyy";
      string fYearMonth = "yyyyMM";
      string fDate = "yyyyMMdd";
      string fHourMin = "HHmm";
      string fSec = "ss";
      string fMilliSec = "ffff";
      string fTimeZone = "zzzz";

      switch (WithPrecision)
      {
        case DateTimePrecision.None:
          break;
        case DateTimePrecision.Year:
          if (WithTimezone)
            return TargetDateTimeOffset.ToString(String.Format("{0}{1}", fYear, fTimeZone)).Replace(":", "");
          else
            return TargetDateTimeOffset.ToString(String.Format("{0}", fYear));

        case DateTimePrecision.YearMonth:
          if (WithTimezone)
            return TargetDateTimeOffset.ToString(String.Format("{0}{1}", fYearMonth, fTimeZone)).Replace(":", "");
          else
            return TargetDateTimeOffset.ToString(String.Format("{0}", fYearMonth));

        case DateTimePrecision.Date:
          if (WithTimezone)
            return TargetDateTimeOffset.ToString(String.Format("{0}{1}", fDate, fTimeZone)).Replace(":", "");
          else
            return TargetDateTimeOffset.ToString(String.Format("{0}", fDate));

        case DateTimePrecision.DateHourMin:
          if (WithTimezone)
            return TargetDateTimeOffset.ToString(String.Format("{0}{1}{2}", fDate, fHourMin, fTimeZone)).Replace(":", "");
          else
            return TargetDateTimeOffset.ToString(String.Format("{0}{1}", fDate, fHourMin));

        case DateTimePrecision.DateHourMinSec:
          if (WithTimezone)
            return TargetDateTimeOffset.ToString(String.Format("{0}{1}{2}{3}", fDate, fHourMin, fSec, fTimeZone)).Replace(":", "");
          else
            return TargetDateTimeOffset.ToString(String.Format("{0}{1}{2}", fDate, fHourMin, fSec));

        case DateTimePrecision.DateHourMinSecMilli:
          if (WithTimezone)
            return TargetDateTimeOffset.ToString(String.Format("{0}{1}{2}.{3}{4}", fDate, fHourMin, fSec, fMilliSec, fTimeZone)).Replace(":", "");
          else
            return TargetDateTimeOffset.ToString(String.Format("{0}{1}{2}.{3}", fDate, fHourMin, fSec, fMilliSec));
        default:
          throw new ApplicationException("Internal error: Unsupported DateTimeprecision value of " + WithPrecision.ToString());
      }
      return "";
    }

    #endregion
    
  }

}

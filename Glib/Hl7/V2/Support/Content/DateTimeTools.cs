using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Glib.Hl7.V2.Support.Content
{
  /// <summary>
  /// A set of tools / methods to work with Hl7 datetimes 
  /// </summary>
  public static class DateTimeTools
  {
    /// <summary>
    /// A set of static functions to convert a DateTime type to its HL7 datetime string eqivilants format.   
    /// </summary>
    public static class ConvertDateTimeToString
    {
      private static string fDate = "yyyyMMdd";
      private static string fHourMin = "HHmm";
      private static string fSec = "ss";
      private static string fMilliSec = "ffff";
      private static string fTimeZone = "zzzz";

      /// <summary>
      /// Returns a Datetime as a string representation of YYYYMMDDHHMMSSMMMM (i.e 201501251030251234)
      /// </summary>
      /// <param name="DateTime"></param>
      /// <returns></returns>
      public static string AsDateHourMinSecMilli(DateTime DateTime)
      {
        return DateTime.ToString(String.Format("{0}{1}{2}.{3}", fDate, fHourMin, fSec, fMilliSec));
      }
      /// <summary>
      /// Returns a Datetime and Timespan as a string representation with Timezone  component of YYYYMMDDHHMMSSMMMM+HHMM (i.e 201501251030251234+1000)
      /// </summary>
      /// <param name="oDateTime"></param>
      /// <param name="OffSetTime"></param>
      /// <returns></returns>
      public static string AsDateHourMinSecMilli(DateTime oDateTime, TimeSpan OffSetTime)
      {
        DateTimeOffset NewTime = new DateTimeOffset(oDateTime);
        return NewTime.ToOffset(OffSetTime).ToString(String.Format("{0}{1}{2}.{3}{4}", fDate, fHourMin, fSec, fMilliSec, fTimeZone));
      }
      /// <summary>
      /// Returns a Datetime as a string representation of YYYYMMDDHHMMSS (i.e 20150125103025)
      /// </summary>
      /// <param name="DateTime"></param>
      /// <returns></returns>
      public static string AsDateHourMinSec(DateTime DateTime)
      {
        return DateTime.ToString(String.Format("{0}{1}{2}", fDate, fHourMin, fSec));
      }
      /// <summary>
      /// Returns a Datetime and Timespan as a string representation with Timezone component of YYYYMMDDHHMMSS+HHMM (i.e 20150125103025+1000)
      /// </summary>
      /// <param name="oDateTime"></param>
      /// <param name="OffSetTime"></param>
      /// <returns></returns>
      public static string AsDateHourMinSec(DateTime oDateTime, TimeSpan OffSetTime)
      {
        DateTimeOffset NewTime = new DateTimeOffset(oDateTime);
        return NewTime.ToOffset(OffSetTime).ToString(String.Format("{0}{1}{2}{3}", fDate, fHourMin, fSec, fTimeZone));
      }
      /// <summary>
      /// Returns a Datetime as a string representation of YYYYMMDDHHMM (i.e 201501251030)
      /// </summary>
      /// <param name="DateTime"></param>
      /// <returns></returns>
      public static string AsDateHourMin(DateTime DateTime)
      {
        return DateTime.ToString(String.Format("{0}{1}", fDate, fHourMin));
      }
      /// <summary>
      /// Returns a Datetime and Timespan as a string representation with Timezone component of YYYYMMDDHHMM+HHMM (i.e 201501251030+1000)
      /// </summary>
      /// <param name="oDateTime"></param>
      /// <param name="OffSetTime"></param>
      /// <returns></returns>
      public static string AsDateHourMin(DateTime oDateTime, TimeSpan OffSetTime)
      {
        DateTimeOffset NewTime = new DateTimeOffset(oDateTime);
        return NewTime.ToOffset(OffSetTime).ToString(String.Format("{0}{1}{2}", fDate, fHourMin, fTimeZone));
      }
      /// <summary>
      /// Returns a Datetime as a string representation of YYYYMMDD (i.e 20150125)
      /// </summary>
      /// <param name="DateTime"></param>
      /// <returns></returns>
      public static string AsDate(DateTime DateTime)
      {
        return DateTime.ToString(String.Format("{0}", fDate));
      }
      /// <summary>
      /// Returns a Datetime and Timespan as a string representation with Timezone component of YYYYMMDD+HHMM (i.e 20150125+1000)
      /// </summary>
      /// <param name="oDateTime"></param>
      /// <param name="OffSetTime"></param>
      /// <returns></returns>
      public static string AsDate(DateTime oDateTime, TimeSpan OffSetTime)
      {
        DateTimeOffset NewTime = new DateTimeOffset(oDateTime);
        return NewTime.ToOffset(OffSetTime).ToString(String.Format("{0}{1}", fDate, fTimeZone));
      }
    }

    /// <summary>
    /// A static function to parse HL7 Datetime strings to their equivilent datetime type.
    /// </summary>
    public static class ConvertStringToDateTime
    {
      private static string LengthExceptionMessage = "The Content length does not match the allowed lengths for HL7 Statndard datetime conversions. Found: '{0}',  Allowed Format: YYYY[MM[DD[HH[MM[SS[.S[S[S[S]]]]]]]]][+/-ZZZZ].";
      private static string FormatExceptionMessage = "The Content does not match the allowed HL7 Statndard datetime formats. Found: '{0}',  Allowed Format: YYYY[MM[DD[HH[MM[SS[.S[S[S[S]]]]]]]]][+/-ZZZZ].";
      /// <summary>
      /// Parse any string datetime which is in a HL7 datetime format into a Datetime type. Can handle timezones. 
      ///  Exceptions:
      ///   System.FormatException:
      ///     The string parsed does not meet the datatime formats as specified within the Hl7 Standard.   
      /// </summary>
      /// <param name="HL7StandardDateTimeString"></param>
      /// <returns></returns>
      public static DateTime AsDateTime(string HL7StandardDateTimeString)
      {
        char[] MilliSecondsDelimiter = { '.' };
        char[] TimeZomeDelimiter = { '+', '-' };        
        CultureInfo provider = CultureInfo.InvariantCulture;
        try
        {
          if (HL7StandardDateTimeString.IndexOfAny(MilliSecondsDelimiter) > 0)
          {
            if (HL7StandardDateTimeString.IndexOfAny(TimeZomeDelimiter) > 0)
            {
              return DateTime.ParseExact(HL7StandardDateTimeString, HasTimeMillisecondsAndTimeZone(HL7StandardDateTimeString), provider);              
            }
            else
            {
              return DateTime.ParseExact(HL7StandardDateTimeString, HasMilliseconds(HL7StandardDateTimeString), provider);                  
            }
          }
          else if (HL7StandardDateTimeString.IndexOfAny(TimeZomeDelimiter) > 0)
          {
            return DateTime.ParseExact(HL7StandardDateTimeString, HasTimeZone(HL7StandardDateTimeString), provider);               
          }
          else
          {
            return DateTime.ParseExact(HL7StandardDateTimeString, HasNoMillisecondsOrTimeZone(HL7StandardDateTimeString), provider);              
          }
        }
        catch (FormatException FormatExec)
        {          
          throw new FormatException(String.Format(FormatExceptionMessage, HL7StandardDateTimeString), FormatExec);
        }
      }

      private static string HasNoMillisecondsOrTimeZone(string String)
      {
        switch (String.Length)
        {
          case 4:
            return "yyyy";
          case 6:
            return "yyyyMM";
          case 8:
            return "yyyyMMdd";
          case 10:
            return "yyyyMMddHH";
          case 12:
            return "yyyyMMddHHmm";
          case 14:
            return "yyyyMMddHHmmss";
          default:
            throw new FormatException(String.Format(LengthExceptionMessage, String));
            
        }
      }
      private static String HasTimeZone(string String)
      {
        switch (String.Length)
        {
          case 10:
            return "yyyyzzzzz";
          case 12:
            return "yyyyMMzzzzz";
          case 14:
            return "yyyyMMddzzzzz";
          case 16:
            return "yyyyMMddHHzzzzz";
          case 18:
            return "yyyyMMddHHmmzzzzz";
          case 20:
            return "yyyyMMddHHmmsszzzzz";
          default:
            throw new FormatException(String.Format(LengthExceptionMessage, String));
        }
      }
      private static String HasMilliseconds(string String)
      {
        switch (String.Length)
        {
          case 16:
            return "yyyyMMddHHmmss.f";
          case 17:
            return "yyyyMMddHHmmss.ff";
          case 18:
            return "yyyyMMddHHmmss.fff";
          case 19:
            return "yyyyMMddHHmmss.ffff";
          case 25:
            return "yyyyMMddHHmmss.ffffzzzzz";
          default:
            throw new FormatException(String.Format(LengthExceptionMessage, String));
        }
      }
      private static String HasTimeMillisecondsAndTimeZone(string String)
      {
        switch (String.Length)
        {
          case 22:
            return "yyyyMMddHHmmss.fzzzzz";
          case 23:
            return "yyyyMMddHHmmss.ffzzzzz";
          case 24:
            return "yyyyMMddHHmmss.fffzzzzz";
          case 25:
            return "yyyyMMddHHmmss.ffffzzzzz";
          default:
            throw new FormatException(String.Format(LengthExceptionMessage, String));
        }
      }
    }
  }
}

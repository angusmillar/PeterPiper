using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Glib.Hl7.V2.Support.Content
{
  /// <summary>
  /// A set of tools / methods to work with Hl7 DatetimeOffset 
  /// </summary>
  public static class DateTimeTools
  {
    /// <summary>
    /// A set of static functions to convert a DateTime type to its HL7 DatetimeOffset string equivalents format.   
    /// </summary>
    public static class ConvertDateTimeOffsetToString
    {
      private static string fDate = "yyyyMMdd";
      private static string fHourMin = "HHmm";
      private static string fSec = "ss";
      private static string fMilliSec = "ffff";
      private static string fTimeZone = "zzzz";

      /// <summary>
      /// Returns a DatetimeOffset as a HL7 Datetime string representation with Timezone if WithTimeZone = true. YYYYMMDDHHMMSSMMMM+HHMM (i.e 201501251030251234+1000)
      /// </summary>
      /// <param name="DateTimeOffset"></param>
      /// <param name="WithTimeZone"></param>
      /// <returns></returns>
      public static string AsDateHourMinSecMilli(DateTimeOffset DateTimeOffset, bool WithTimeZone = false)
      {
        if (WithTimeZone)
          return DateTimeOffset.ToString(String.Format("{0}{1}{2}.{3}{4}", fDate, fHourMin, fSec, fMilliSec, fTimeZone)).Replace(":", "");        
        else
          return DateTimeOffset.ToString(String.Format("{0}{1}{2}.{3}", fDate, fHourMin, fSec, fMilliSec));
      }

      /// <summary>
      /// Returns a DatetimeOffset as a HL7 string representation with Timezone if WithTimeZone = true. YYYYMMDDHHMMSS+HHMM (i.e 20150125103025+1000)
      /// </summary>
      /// <param name="oDateTimeOffset"></param>
      /// <param name="WithTimeZone"></param>
      /// <returns></returns>
      public static string AsDateHourMinSec(DateTimeOffset oDateTimeOffset, bool WithTimeZone = false)
      {
        if (WithTimeZone)
          return oDateTimeOffset.ToString(String.Format("{0}{1}{2}{3}", fDate, fHourMin, fSec, fTimeZone)).Replace(":", "");
        else
          return oDateTimeOffset.ToString(String.Format("{0}{1}{2}", fDate, fHourMin, fSec));
      }
      
      /// <summary>
      /// Returns a DatetimeOffset as a HL7 string representation with Timezone if WithTimeZone = true. YYYYMMDDHHMM+HHMM (i.e 201501251030+1000)
      /// </summary>
      /// <param name="DateTimeOffset"></param>
      /// <param name="WithTimeZone"></param>
      /// <returns></returns>
      public static string AsDateHourMin(DateTimeOffset DateTimeOffset, bool WithTimeZone = false)
      {
        if (WithTimeZone)
          return DateTimeOffset.ToString(String.Format("{0}{1}{2}", fDate, fHourMin, fTimeZone)).Replace(":", "");
        else
          return DateTimeOffset.ToString(String.Format("{0}{1}", fDate, fHourMin));
      }
      
      /// <summary>
      /// Returns a DatetimeOffset as a HL7 string representation with Timezone if WithTimeZone = true. YYYYMMDD+HHMM (i.e 20150125+1000)
      /// </summary>
      /// <param name="DateTimeOffset"></param>
      /// <param name="WithTimeZone"></param>
      /// <returns></returns>
      public static string AsDate(DateTimeOffset DateTimeOffset, bool WithTimeZone = false)
      {        
        if (WithTimeZone)
          return DateTimeOffset.ToString(String.Format("{0}{1}", fDate, fTimeZone)).Replace(":", "");
        else
          return DateTimeOffset.ToString(String.Format("{0}", fDate));
      }
    }

    /// <summary>
    /// A static function to parse HL7 Datetime strings to their equivalent datetime type.
    /// </summary>
    public static class ConvertStringToDateTime
    {
      private static string LengthExceptionMessage = "The Content length does not match the allowed lengths for HL7 Standard datetime conversions. Found: '{0}',  Allowed Format: YYYY[MM[DD[HH[MM[SS[.S[S[S[S]]]]]]]]][+/-ZZZZ].";
      private static string FormatExceptionMessage = "The Content does not match the allowed HL7 Standard datetime formats. Found: '{0}',  Allowed Format: YYYY[MM[DD[HH[MM[SS[.S[S[S[S]]]]]]]]][+/-ZZZZ].";
      /// <summary>
      /// Parse any string datetime which is in a HL7 datetime format into a DateTimeOffset type. Can handle time zones. 
      ///  Exceptions:
      ///   System.FormatException:
      ///     The string parsed does not meet the datatime formats as specified within the Hl7 Standard.   
      /// </summary>
      /// <param name="HL7StandardDateTimeString"></param>
      /// <returns></returns>
      public static DateTimeOffset AsDateTimeOffset(string HL7StandardDateTimeString)
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
              return DateTimeOffset.ParseExact(HL7StandardDateTimeString, HasTimeMillisecondsAndTimeZone(HL7StandardDateTimeString), provider);              
            }
            else
            {
              return DateTimeOffset.ParseExact(HL7StandardDateTimeString, HasMilliseconds(HL7StandardDateTimeString), provider);                  
            }
          }
          else if (HL7StandardDateTimeString.IndexOfAny(TimeZomeDelimiter) > 0)
          {
            return DateTimeOffset.ParseExact(HL7StandardDateTimeString, HasTimeZone(HL7StandardDateTimeString), provider);               
          }
          else
          {
            return DateTimeOffset.ParseExact(HL7StandardDateTimeString, HasNoMillisecondsOrTimeZone(HL7StandardDateTimeString), provider);              
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
          case 9:
            return "yyyyzzzzz";
          case 11:
            return "yyyyMMzzzzz";
          case 13:
            return "yyyyMMddzzzzz";
          case 15:
            return "yyyyMMddHHzzzzz";
          case 17:
            return "yyyyMMddHHmmzzzzz";
          case 19:
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
          case 21:
            return "yyyyMMddHHmmss.fzzzzz";
          case 22:
            return "yyyyMMddHHmmss.ffzzzzz";
          case 23:
            return "yyyyMMddHHmmss.fffzzzzz";
          case 24:
            return "yyyyMMddHHmmss.ffffzzzzz";
          default:
            throw new FormatException(String.Format(LengthExceptionMessage, String));
        }
      }
    }
  }
}

using System;
using System.Globalization;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Support.Tools
{
    public static class DateTimeSupportTools
    {
        private const string FormatExceptionMessage = "The Content does not match the allowed HL7 Standard datetime formats. Found: '{0}',  Allowed Format: YYYY[MM[DD[HH[MM[SS[.S[S[S[S]]]]]]]]][+/-ZZZZ].";

        private static readonly string[] Formats =
        {
            "yyyy", "yyyyMM", "yyyyMMdd", "yyyyMMddHH", "yyyyMMddHHmm", "yyyyMMddHHmmss",
            "yyyyzzzzz", "yyyyMMzzzzz", "yyyyMMddzzzzz", "yyyyMMddHHzzzzz", "yyyyMMddHHmmzzzzz", "yyyyMMddHHmmsszzzzz",
            "yyyyMMddHHmmss.f", "yyyyMMddHHmmss.ff", "yyyyMMddHHmmss.fff", "yyyyMMddHHmmss.ffff",
            "yyyyMMddHHmmss.ffffzzzzz",
            "yyyyMMddHHmmss.fzzzzz", "yyyyMMddHHmmss.ffzzzzz", "yyyyMMddHHmmss.fffzzzzz", "yyyyMMddHHmmss.ffffzzzzz"
        };

        public enum DateTimePrecision
        {
            None,
            Year,
            YearMonth,
            Date,
            DateHourMin,
            DateHourMinSec,
            DateHourMinSecMilli
        };

        /// <summary>
        /// Returns a DateTimeOffset for the Hl7 DateTime string passed in.
        /// If the string passed in can not be parsed as a dateTime then a FormatException is thrown.
        /// </summary>
        /// <param name="Hl7DateTimeString"></param>
        /// <returns></returns>
        public static DateTimeOffset AsDateTimeOffSet(string Hl7DateTimeString)
        {
            if (TryParseDateTimeString(Hl7DateTimeString, out var result))
            {
                return result;
            }

            throw new PeterPiperException(string.Format(FormatExceptionMessage, Hl7DateTimeString));
        }

        /// <summary>
        /// Returns True if a Time-zone element is found in the Hl7 DateTime string.
        /// If the string passed in cannot be parsed as a dateTime then a FormatException is thrown.
        /// </summary>
        /// <param name="Hl7DateTimeString"></param>
        /// <returns></returns>
        public static bool HasTimezone(string Hl7DateTimeString)
        {
            if (CanParseToDateTimeOffset(Hl7DateTimeString))
            {
                return CheckForTimezone(Hl7DateTimeString);
            }

            throw new PeterPiperException(string.Format(FormatExceptionMessage, Hl7DateTimeString));
        }

        /// <summary>
        /// Returns a Timespan that represents the time zone found in the HL7 DateTime string.
        /// Throws a FormatException if no time zone present or if the content is empty
        /// </summary>
        /// <param name="Hl7DateTimeString"></param>
        /// <returns></returns>
        public static TimeSpan GetTimezone(string Hl7DateTimeString)
        {
            if (HasTimezone(Hl7DateTimeString))
            {
                char[] TimeZoneDelimiter = {'+', '-'};
                try
                {
                    var TimeZoneString = Hl7DateTimeString.Substring(
                        Hl7DateTimeString.LastIndexOfAny(TimeZoneDelimiter),
                        Hl7DateTimeString.Length - Hl7DateTimeString.LastIndexOfAny(TimeZoneDelimiter));
                    int Hours = System.Convert.ToInt32(TimeZoneString.Substring(0, 3));
                    int min = System.Convert.ToInt32(TimeZoneString.Substring(3, 2));
                    return new TimeSpan(Hours, min, 0);
                }
                catch (Exception Exec)
                {
                    throw new PeterPiperException(
                        $"Unable to parse time-zone from HL7 date time string of: {Hl7DateTimeString}",
                        Exec);
                }
            }

            throw new PeterPiperException(
                "No time-zone present in given content. Try testing for time-zone by calling 'HasTimezone' before calling 'GetTimezone'.");
        }

        /// <summary>
        /// Set the time-zone for a give HL7 DateTime string. This will convert the date time from the time-zone present to the new time zone. 
        /// If no time-zone is present in the HL7 date time string then it will assume that this date time is from the new time zone and not convert.
        /// Will throw a FormatException if the HL7 DateTime string is no able to be parsed as a DateTimeOffset.
        /// </summary>
        /// <param name="Hl7DateTimeString"></param>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public static string SetTimezone(string Hl7DateTimeString, TimeSpan timespan)
        {
            if (HasTimezone(Hl7DateTimeString))
            {
                var DateTime = AsDateTimeOffSet(Hl7DateTimeString);
                DateTime = DateTime.ToOffset(timespan);
                return AsString(DateTime, true, GetPrecision(Hl7DateTimeString));
            }

            return Hl7DateTimeString + $"{timespan.Hours:+00;-00}{timespan.Minutes:00}";
        }

        /// <summary>
        /// Returns the Precision found in the Hl7 DateTime string passed in. 
        /// If the string passed in cannot be parsed as a HL7 dateTime string then a FormatException is thrown.
        /// </summary>
        /// <param name="Hl7DateTimeString"></param>
        /// <returns></returns>
        public static DateTimePrecision GetPrecision(string Hl7DateTimeString)
        {
            if (CanParseToDateTimeOffset(Hl7DateTimeString))
            {
                return CalculateDateTimePrecision(Hl7DateTimeString);
            }

            return DateTimePrecision.None;
        }

        /// <summary>
        /// Returns True if the Hl7 DateTime string can be parsed to a DateTimeOffset, or False if unable;
        /// </summary>
        /// <param name="Hl7DateTimeString"></param>
        /// <returns></returns>
        public static bool CanParseToDateTimeOffset(string Hl7DateTimeString)
        {
            return TryParseDateTimeString(Hl7DateTimeString, out _);
        }

        /// <summary>
        /// Returns the Hl7 DateTime string with or with out a time zone and to the precision given;
        /// </summary>
        /// <param name="dateTimeOffSet"></param>
        /// <param name="withTimezone"></param>
        /// <param name="withPrecision"></param>
        /// <returns></returns>
        public static string AsString(DateTimeOffset dateTimeOffSet, bool withTimezone, DateTimePrecision withPrecision)
        {
            return GetDateTimeOffSetAsHl7String(dateTimeOffSet, withTimezone, withPrecision);
        }

        private static bool TryParseDateTimeString(string dateTimeString, out DateTimeOffset result)
        {
            if (dateTimeString.Length < 4)
            {
                result = DateTimeOffset.MinValue;
                return false;
            }

            IFormatProvider provider = CultureInfo.InvariantCulture.DateTimeFormat;
            if (DateTimeOffset.TryParseExact(dateTimeString, Formats, provider,
                    DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out result))
            {
                return true;
            }


            return false;
        }

        private static bool CheckForTimezone(string dateTimeString)
        {
            char[] TimeZomeDelimiter = {'+', '-'};
            if (dateTimeString.IndexOfAny(TimeZomeDelimiter) > 0)
            {
                return true;
            }

            return false;
        }

        private static DateTimePrecision CalculateDateTimePrecision(string dateTimeString)
        {
            char[] milliSecondsDelimiter = {'.'};
            char[] timeZoneDelimiter = {'+', '-'};
            string tempDateTimeString = dateTimeString;
            if ((tempDateTimeString.IndexOfAny(timeZoneDelimiter) > 0))
            {
                tempDateTimeString = tempDateTimeString.Remove(tempDateTimeString.LastIndexOfAny(timeZoneDelimiter));
            }

            if (tempDateTimeString.IndexOfAny(milliSecondsDelimiter) > 0)
            {
                return DateTimePrecision.DateHourMinSecMilli;
            }

            switch (tempDateTimeString.Length)
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

        private static string GetDateTimeOffSetAsHl7String(DateTimeOffset targetDateTimeOffset, bool withTimezone,
            DateTimePrecision withPrecision)
        {
            const string fYear = "yyyy";
            const string fYearMonth = "yyyyMM";
            const string fDate = "yyyyMMdd";
            const string fHourMin = "HHmm";
            const string fSec = "ss";
            const string fMilliSec = "ffff";
            const string fTimeZone = "zzzz";

            switch (withPrecision)
            {
                case DateTimePrecision.None:
                    break;
                case DateTimePrecision.Year:
                    if (withTimezone)
                    {
                        return targetDateTimeOffset.ToString($"{fYear}{fTimeZone}").Replace(":", "");
                    }

                    return targetDateTimeOffset.ToString(fYear);

                case DateTimePrecision.YearMonth:
                    if (withTimezone)
                    {
                        return targetDateTimeOffset.ToString($"{fYearMonth}{fTimeZone}")
                            .Replace(":", "");
                    }

                    return targetDateTimeOffset.ToString(fYearMonth);

                case DateTimePrecision.Date:
                    if (withTimezone)
                    {
                        return targetDateTimeOffset.ToString($"{fDate}{fTimeZone}").Replace(":", "");
                    }

                    return targetDateTimeOffset.ToString(fDate);

                case DateTimePrecision.DateHourMin:
                    if (withTimezone)
                    {
                        return targetDateTimeOffset.ToString($"{fDate}{fHourMin}{fTimeZone}").Replace(":", "");
                    }

                    return targetDateTimeOffset.ToString($"{fDate}{fHourMin}");

                case DateTimePrecision.DateHourMinSec:
                    if (withTimezone)
                    {
                        return targetDateTimeOffset
                            .ToString($"{fDate}{fHourMin}{fSec}{fTimeZone}").Replace(":", "");
                    }

                    return targetDateTimeOffset.ToString($"{fDate}{fHourMin}{fSec}");

                case DateTimePrecision.DateHourMinSecMilli:
                    if (withTimezone)
                    {
                        return targetDateTimeOffset
                            .ToString($"{fDate}{fHourMin}{fSec}.{fMilliSec}{fTimeZone}").Replace(":", "");
                    }

                    return targetDateTimeOffset.ToString($"{fDate}{fHourMin}{fSec}.{fMilliSec}");
                
                default:
                    throw new PeterPiperException("Internal error: Unsupported DateTimePrecision value of " +
                                                  withPrecision);
            }

            return "";
        }
    }
}
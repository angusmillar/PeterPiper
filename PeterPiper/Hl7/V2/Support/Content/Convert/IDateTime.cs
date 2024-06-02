using System;
using PeterPiper.Hl7.V2.Support.Tools;
namespace PeterPiper.Hl7.V2.Support.Content.Convert;

public interface IDateTime
{
  string AsString();
  string AsString(bool withTimezone, DateTimeSupportTools.DateTimePrecision withPrecision);
  bool CanParseToDateTimeOffset { get; }
  DateTimeOffset GetDateTimeOffset();
  DateTimeSupportTools.DateTimePrecision GetPrecision();
  TimeSpan GetTimezone();
  bool HasTimezone { get; }
  void SetDateTimeOffset(DateTimeOffset dateTimeOffset, bool hasTimezone = true, DateTimeSupportTools.DateTimePrecision dateTimePrecision = DateTimeSupportTools.DateTimePrecision.DateHourMinSec);
  void SetTimezone(TimeSpan timespan);
}
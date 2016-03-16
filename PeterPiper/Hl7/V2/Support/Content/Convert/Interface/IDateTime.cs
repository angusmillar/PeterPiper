using System;
using PeterPiper.Hl7.V2.Support.Content.Convert.Tools;
namespace PeterPiper.Hl7.V2.Support.Content.Convert.Interface
{
  public interface IDateTime
  {
    string AsString();
    string AsString(bool WithTimezone, DateTimeSupportTools.DateTimePrecision WithPrecision);
    bool CanParseToDateTimeOffset { get; }
    DateTimeOffset GetDateTimeOffset();
    DateTimeSupportTools.DateTimePrecision GetPrecision();
    TimeSpan GetTimezone();
    bool HasTimezone { get; }
    void SetDateTimeOffset(DateTimeOffset DateTimeOffset, bool HasTimezone = true, DateTimeSupportTools.DateTimePrecision DateTimePrecision = DateTimeSupportTools.DateTimePrecision.DateHourMinSec);
    void SetTimezone(TimeSpan Timespan);
  }
}

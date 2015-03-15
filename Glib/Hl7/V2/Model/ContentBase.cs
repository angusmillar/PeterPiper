using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Model
{
  public abstract class ContentBase : ModelBase
  {
    protected ContentBase(Support.MessageDelimiters Delimiters) 
      :base(Delimiters)
    {         
    }
    protected ContentBase()
    {        
    }    

    public void AsDate(DateTime DateTime)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeToString.AsDate(DateTime);
    }
    public void AsDate(DateTime oDateTime, TimeSpan TimeZoneOffSet)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeToString.AsDate(oDateTime, TimeZoneOffSet);
    }
    public DateTime AsDate()
    {
      return Support.Content.DateTimeTools.ConvertStringToDateTime.AsDateTime(this.AsStringRaw);
    }

    public void AsDateHourMin(DateTime DateTime)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeToString.AsDateHourMin(DateTime);
    }
    public void AsDateHourMin(DateTime oDateTime, TimeSpan TimeZoneOffSet)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeToString.AsDateHourMin(oDateTime, TimeZoneOffSet);
    }
    public DateTime AsDateHourMin()
    {
      return Support.Content.DateTimeTools.ConvertStringToDateTime.AsDateTime(this.AsStringRaw);
    }

    public void AsDateHourMinSec(DateTime DateTime)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeToString.AsDateHourMinSec(DateTime);
    }
    public void AsDateHourMinSec(DateTime oDateTime, TimeSpan TimeZoneOffSet)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeToString.AsDateHourMinSec(oDateTime, TimeZoneOffSet);
    }
    public DateTime AsDateHourMinSec()
    {
      return Support.Content.DateTimeTools.ConvertStringToDateTime.AsDateTime(this.AsStringRaw);
    }

    public void AsDateHourMinSecMilli(DateTime DateTime)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeToString.AsDateHourMinSecMilli(DateTime);
    }
    public void AsDateHourMinSecMilli(DateTime oDateTime, TimeSpan TimeZoneOffSet)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeToString.AsDateHourMinSecMilli(oDateTime, TimeZoneOffSet);
    }
    public DateTime AsDateHourMinSecMilli()
    {
      return Support.Content.DateTimeTools.ConvertStringToDateTime.AsDateTime(this.AsStringRaw);
    }

    public void ToBase64(byte[] item)
    {
      this.AsStringRaw = Support.Content.Base64.Encoder(item);
    }
    public byte[] FromBase64()
    {
      return Support.Content.Base64.Decoder(this.AsStringRaw);
    }
  }
}

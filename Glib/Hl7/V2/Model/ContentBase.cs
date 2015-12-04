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

    public void AsDate(DateTimeOffset DateTimeOffset, bool WithTimeZone)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeOffsetToString.AsDate(DateTimeOffset, WithTimeZone);
    }
    public DateTimeOffset AsDate()
    {
      return Support.Content.DateTimeTools.ConvertStringToDateTime.AsDateTimeOffset(this.AsStringRaw);
    }

    //----------------------------------------------------------------------------------------------------
    public void AsDateHourMin(DateTime DateTime)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMin(DateTime);
    }

    public void AsDateHourMin(DateTimeOffset DateTimeOffset, bool WithTimeZone = false)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMin(DateTimeOffset, WithTimeZone);
    }

    //----------------------------------------------------------------------------------------------------

    public DateTimeOffset AsDateHourMin()
    {
      return Support.Content.DateTimeTools.ConvertStringToDateTime.AsDateTimeOffset(this.AsStringRaw);
    }

    //----------------------------------------------------------------------------------------------------

    public void AsDateHourMinSec(DateTime DateTime)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSec(DateTime);
    }

    public void AsDateHourMinSec(DateTimeOffset DateTimeOffset, bool WithTimeZone = false)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSec(DateTimeOffset, WithTimeZone);
    }


    public DateTimeOffset AsDateHourMinSec()
    {
      return Support.Content.DateTimeTools.ConvertStringToDateTime.AsDateTimeOffset(this.AsStringRaw);
    }

    //----------------------------------------------------------------------------------------------------

    public void AsDateHourMinSecMilli(DateTimeOffset oDateTimeOffset, bool WithTimeZone = false)
    {
      this.AsStringRaw = Support.Content.DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSecMilli(oDateTimeOffset, WithTimeZone);
    }
    public DateTimeOffset AsDateHourMinSecMilli()
    {
      return Support.Content.DateTimeTools.ConvertStringToDateTime.AsDateTimeOffset(this.AsStringRaw);
    }

    //----------------------------------------------------------------------------------------------------


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

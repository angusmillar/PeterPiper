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

    public Glib.Hl7.V2.Support.Content.DateTimeSupport DateTimeSupport
    {
      get
      {
        return new Support.Content.DateTimeSupport(this);
      }
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

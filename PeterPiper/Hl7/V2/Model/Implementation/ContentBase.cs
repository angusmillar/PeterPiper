using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Model.Implementation;

namespace PeterPiper.Hl7.V2.Model.Implementation
{
  public abstract class ContentBase : ModelBase, IContentBase
  {
    protected ContentBase(Support.MessageDelimiters Delimiters) 
      :base(Delimiters)
    {         
    }
    protected ContentBase()
    {        
    }    

    public PeterPiper.Hl7.V2.Support.Content.DateTimeSupport DateTimeSupport
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

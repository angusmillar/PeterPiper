using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Model.Implementation;
using PeterPiper.Hl7.V2.Support.Content.Convert.Interface;

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

    public IConvert Convert
    {
      get
      {
        return new PeterPiper.Hl7.V2.Support.Content.Convert.Implementation.Convert(this);
      }
    }

    //public PeterPiper.Hl7.V2.Support.Content.Convert.Implementation.DateTime DateTimeSupport
    //{
    //  get
    //  {
    //    return new Support.Content.Convert.Implementation.DateTime(this);
    //  }
    //}
    //public void ToBase64(byte[] item)
    //{
    //  this.AsStringRaw = Support.Content.Convert.Tools.Base64Tools.Encoder(item);
    //}
    //public byte[] FromBase64()
    //{
    //  return Support.Content.Convert.Tools.Base64Tools.Decoder(this.AsStringRaw);
    //}
  }
}

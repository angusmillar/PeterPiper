using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model.Implementation;

namespace PeterPiper.Hl7.V2.Support.Content.Convert.Implementation
{
  public class Base64 : PeterPiper.Hl7.V2.Support.Content.Convert.IBase64
  {
    private ContentBase _ContentBase;

    internal Base64(ContentBase ContentBase)
    {
      _ContentBase = ContentBase;
    }

    public void Encode(byte[] item)
    {
      _ContentBase.AsStringRaw = Support.Tools.Base64Tools.Encoder(item);
    }
    public byte[] Decode()
    {
      return Support.Tools.Base64Tools.Decoder(_ContentBase.AsStringRaw);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model.Implementation;
using PeterPiper.Hl7.V2.Support.Content.Convert;

namespace PeterPiper.Hl7.V2.Support.Content.Convert.Implementation
{
  public class Convert : PeterPiper.Hl7.V2.Support.Content.Convert.IConvert
  {
    private ContentBase _ContentBase;

    internal Convert(ContentBase ContentBase)
    {
      _ContentBase = ContentBase;
    }

    public IBase64 Base64
    {
      get
      {
        return new Base64(_ContentBase);
      }
    }
    
    public IDateTime DateTime
    {
      get
      {
        return new DateTime(_ContentBase);
      }
    }
    
  }
}

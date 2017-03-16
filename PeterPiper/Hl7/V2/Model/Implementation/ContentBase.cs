using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Model.Implementation;
using PeterPiper.Hl7.V2.Support.Content.Convert;

namespace PeterPiper.Hl7.V2.Model.Implementation
{
  internal abstract class ContentBase : ModelBase, IContentBase
  {
    protected ContentBase(IMessageDelimiters Delimiters)
      : base(Delimiters)
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

  }
}

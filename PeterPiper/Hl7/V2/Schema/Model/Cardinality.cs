using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterPiper.Hl7.V2.Schema.Model
{
  public abstract class Cardinality
  {
    private bool _CanRepeat;
    public bool CanRepeat
    {
      get { return _CanRepeat; }
      set { _CanRepeat = value; }
    }

    private bool _IsMandatory;
    public bool IsMandatory
    {
      get { return _IsMandatory; }
      set { _IsMandatory = value; }
    }

    public string CardinalityMask
    {
      get
      {
        char min = '0';
        char max = '1';
        if (_IsMandatory)
        {
          min = '1';
        }
        if (_CanRepeat)
          max = '*';
        return String.Format("{0}..{1}", min, max);
      }
    }

  }
}

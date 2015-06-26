using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public abstract class IVL<T> : QSET<T> where T : QTY
  {
    private T _low;
    public T low
    {
      get { return _low; }
      set { _low = value; }
    }

    private T _high;
    public T high
    {
      get { return _high; }
      set { _high = value; }
    }

    private Boolean _lowClosed;
    public Boolean lowClosed
    {
      get { return _lowClosed; }
      set { _lowClosed = value; }
    }

    private Boolean _highClosed;
    public Boolean highClosed
    {
      get { return _highClosed; }
      set { _highClosed = value; }
    }
    
  }
}

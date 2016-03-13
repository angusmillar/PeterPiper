using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterPiper.Hl7.V2.Schema.Model
{
  public class CompositeItem
  {
    private string _Description;
    public string Description
    {
      get { return _Description; }
      set { _Description = value; }
    }

    private int _Hl7TableIndex;
    public int Hl7TableIndex
    {
      get { return _Hl7TableIndex; }
      set { _Hl7TableIndex = value; }
    }

    private DataTypeBase _Type;
    public DataTypeBase Type
    {
      get { return _Type; }
      set { _Type = value; }
    }
  }
}

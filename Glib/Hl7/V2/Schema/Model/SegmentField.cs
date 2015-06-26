using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Schema.Model
{
  public class SegmentField : Cardinality
  {
    private int _ItemNumber;
    public int ItemNumber
    {
      get { return _ItemNumber; }
      set { _ItemNumber = value; }
    }

    private string _Description;
    public string Description
    {
      get { return _Description; }
      set { _Description = value; }
    }

    private DataTypeBase _DataType;
    public DataTypeBase DataType
    {
      get { return _DataType; }
      set { _DataType = value; }
    }

    private int _Hl7TableIndex;
    public int Hl7TableIndex
    {
      get { return _Hl7TableIndex; }
      set { _Hl7TableIndex = value; }
    }
     
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public abstract class QTY : Any
  {
    private QTY _uncertainty;
    public QTY uncertainty
    {
      get { return _uncertainty; }
      set { _uncertainty = value; }
    }

    private Enum.UncertaintyType _uncertaintyType;
    public Enum.UncertaintyType uncertaintyType
    {
      get { return _uncertaintyType; }
      set { _uncertaintyType = value; }
    }

    protected Guid? _UniqueId;
    //UniqueId property for QTY object
    public Guid? UniqueId
    {
      get
      {
        return _UniqueId;
      }
      set
      {
        _UniqueId = value;
      }
    }

    //Default constructor
    public QTY()
    {
        //create a new unique id for this QTY object
        _UniqueId = Guid.NewGuid();
    }

  }
}

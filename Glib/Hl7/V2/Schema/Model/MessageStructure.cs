using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Schema.Model
{
  public class MessageStructure
  {
    private string _MessageType;
    public string MessageType
    {
      get { return _MessageType; }
      set { _MessageType = value; }
    }

    private string  _MessageEvent;
    public string  MessageEvent
    {
      get { return _MessageEvent; }
      set { _MessageEvent = value; }
    }

    private string _MessageTypeDescription;
    public string MessageTypeDescription
    {
      get { return _MessageTypeDescription; }
      set { _MessageTypeDescription = value; }
    }

    private string _Notes;
    public string Notes
    {
      get { return _Notes; }
      set { _Notes = value; }
    }

    private List<MessageItemBase> _MessageItemList = new List<MessageItemBase>();
    public List<MessageItemBase> MessageItemList
    {
      get { return _MessageItemList; }
      set { _MessageItemList = value; }
    }    
  }
}

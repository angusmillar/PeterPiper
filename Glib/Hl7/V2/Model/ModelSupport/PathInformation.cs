using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Model.ModelSupport
{
  public class PathInformation
  {
    internal string _MessageType = string.Empty;
    public string MessageType
    {
      get
      {
        return _MessageType;
      }
    }

    internal string _MessageEvent = string.Empty;
    public string MessageEvent
    {
      get
      {
        return _MessageEvent;
      }
    }

    internal string _SegmentCode = string.Empty;
    public string SegmentCode
    {
      get
      {
        return _SegmentCode;
      }
    }

    internal int? _SegmentPosition = null;
    public string SegmentPosition
    {
      get
      {
        return _SegmentPosition.ToString();
      }
    }

    internal int? _SegmentTypePosition = null;
    public string SegmentTypePosition
    {
      get
      {
        return _SegmentTypePosition.ToString();
      }
    }

    internal int? _RepeatCount = null;
    public string RepeatCount
    {
      get
      {
        return _RepeatCount.ToString();
      }
    }
    public int RepeatCountInteger
    {
      get
      {
        if (_RepeatCount == null)
          return 0;
        else
          return Convert.ToInt32(_RepeatCount);
      }
    }

    internal int? _RepeatPosition = null;
    public string RepeatPosition
    {
      get
      {
        return _RepeatPosition.ToString();
      }
    }

    internal int? _FieldPosition = null;
    public string FieldPosition
    {
      get
      {
        return _FieldPosition.ToString();
      }
    }

    internal int? _ComponentPosition = null;
    public string ComponentPosition
    {
      get
      {
        return _ComponentPosition.ToString();
      }
    }

    internal int? _ComponentCount = null;    

    internal int? _SubComponentPosition = null;
    public string SubComponentPosition
    {
      get
      {
        return _SubComponentPosition.ToString();
      }
    }

    internal int? _SubComponentCount = null;

    internal int? _ContentPosition = null;
    public string ContentPosition
    {
      get
      {
        return _ContentPosition.ToString();
      }
    }

    public string PathBrief
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        sb.Append(SegmentCode);

        if (FieldPosition != string.Empty)
        {
          sb.Append("-");
          sb.Append(FieldPosition);
        }
        if (RepeatPosition != string.Empty && _RepeatCount > 1)
        {
          sb.Append("{");
          sb.Append(RepeatPosition);
          sb.Append("}");
        }
        if (ComponentPosition != string.Empty && _ComponentCount > 1)
        {
          if (sb.Length == 0)
            sb.Append("<unk>-?");
          sb.Append(".");
          sb.Append(ComponentPosition);
        }
        if (SubComponentPosition != string.Empty && _SubComponentCount > 1)
        {
          if (sb.Length == 0)
            sb.Append("<unk>-?.?");
          sb.Append(".");
          sb.Append(SubComponentPosition);
        }
        if (ContentPosition != string.Empty)
        {
          if (sb.Length == 0)
            sb.Append("<unk>-?");
          sb.Append(" [");
          sb.Append(ContentPosition);
          sb.Append("]");
        }
        if (sb.Length == 0)
          sb.Append("<unk>-?");
        return sb.ToString();
      }
    }
    public string PathVerbos
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        if (SegmentCode != string.Empty)
        {
          sb.Append("Segment: ");
          sb.Append(SegmentCode);
        }
        if (FieldPosition != string.Empty)
        {
          sb.Append(", Field: ");
          sb.Append(FieldPosition);
        }

        if (RepeatPosition != string.Empty && _RepeatCount > 1)
        {
          sb.Append(", {Repeat: ");
          sb.Append(RepeatPosition);
        }

        if (ComponentPosition != string.Empty && _ComponentCount > 1)
        {
          sb.Append(", Component: ");
          sb.Append(ComponentPosition);
        }
        if (SubComponentPosition != string.Empty && _SubComponentCount > 1)
        {
          sb.Append(", SubComponent: ");
          sb.Append(SubComponentPosition);
        }
        if (ContentPosition != string.Empty)
        {
          sb.Append(", [Content: ");
          sb.Append(ContentPosition);
          sb.Append("]");
        }

        if (RepeatPosition != string.Empty && _RepeatCount > 1)
        {
          sb.Append(" }");
        }

        return sb.ToString();
      }
    }
  }
}

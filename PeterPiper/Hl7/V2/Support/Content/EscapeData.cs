using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Support.Standard;

namespace PeterPiper.Hl7.V2.Support.Content
{
  public class EscapeData
  {
    public EscapeData(EscapeType EscapeType, string MetaData)
    {
      if (EscapeType == Standard.EscapeType.NotAnEscape)
        throw new ArgumentException(String.Format("EscapeMetaData's EscapeType argument can not be set to '{0}', maybe you should choose '{1}'", Standard.EscapeType.NotAnEscape.ToString(), Standard.EscapeType.Unknown.ToString()));
      this._EscapeType = EscapeType;
      this._EscapeTypeCharater = Escapes.ResolveEscapeChararter(_EscapeType);
      this._MetaData = MetaData;
    }
    public EscapeData(string ContentEscapeString)
    {
      if (ContentEscapeString == String.Empty)
        throw new ArgumentException("EscapeMetaData supplied string was empty, this is not allowed.");

      this._EscapeType = Escapes.ResolveEscapeType(ContentEscapeString);
      if (_EscapeType != Standard.EscapeType.Unknown)
      {
        this._EscapeTypeCharater = Escapes.ResolveEscapeChararter(_EscapeType);
      }
      else
      {
        this._EscapeTypeCharater = ContentEscapeString.ToCharArray()[0].ToString();
      }

      if (ContentEscapeString.Length > 1)
        if (ContentEscapeString.StartsWith("."))
        {
          if (ContentEscapeString.Length > 3)
            this._MetaData = ContentEscapeString.Substring(3, ContentEscapeString.Length - 3);
          else
            this._MetaData = String.Empty;
        }
        else
          this._MetaData = ContentEscapeString.Substring(1, ContentEscapeString.Length - 1);
      else
        this._MetaData = String.Empty;
    }

    private EscapeType _EscapeType;
    public EscapeType EscapeType
    {
      get
      {
        return _EscapeType;
      }
    }

    private string _EscapeTypeCharater;
    public string EscapeTypeCharater
    {
      get
      {
        return _EscapeTypeCharater;
      }
    }

    private string _MetaData;
    public string MetaData
    {
      get
      {
        return _MetaData;
      }
    }

    public bool IsFormattingCommand
    {
      get
      {
        switch (this._EscapeType)
        {
          case EscapeType.Field:
            return false;
          case EscapeType.Repeat:
            return false;
          case EscapeType.Component:
            return false;
          case EscapeType.SubComponent:
            return false;
          case EscapeType.Escape:
            return false;
          case EscapeType.HighlightOn:
            return true;
          case EscapeType.HighlightOff:
            return true;
          case EscapeType.LocallyDefined:
            return false;
          case EscapeType.HexadecimalData:
            return false;
          case EscapeType.NewLine:
            return true;
          case EscapeType.SkipVerticalSpaces:
            return true;
          case EscapeType.WordWrapOn:
            return true;
          case EscapeType.WordWrapOff:
            return true;
          case EscapeType.Indent:
            return true;
          case EscapeType.TempIndent:
            return true;
          case EscapeType.SkipSpacesToRight:
            return true;
          case EscapeType.CenterNextLine:
            return true;
          case EscapeType.Unknown:
            return false;
          case EscapeType.NotAnEscape:
            return false;
          default:
            throw new ApplicationException(String.Format("Unknown EscapeType passed into EscapeMetaData, Escapetype was: {0}", this._EscapeType.ToString()));
        }
      }
    }

  }
}

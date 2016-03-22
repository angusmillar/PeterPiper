using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.Model.Implementation;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.CustomException;
namespace PeterPiper.Hl7.V2.Support.Standard
{
  public static class Escapes
  {
    /// <summary>
    /// Field separator
    /// </summary>
    public const string Field = "F";
    /// <summary>
    /// repetition separator
    /// </summary>
    public const string Repeat = "R";
    /// <summary>
    /// component separator
    /// </summary>
    public const string Component = "S";
    /// <summary>
    /// subcomponent separator
    /// </summary>
    public const string SubComponent = "T";
    /// <summary>
    /// escape character
    /// </summary>
    public const string Escape = "E";
    /// <summary>
    /// start highlighting
    /// </summary>
    public const string HighlightStart = "H";
    /// <summary>
    /// normal text (end highlighting)
    /// </summary>
    public const string HighlightEnd = "N";
    /// <summary>
    /// hexadecimal data, will be if form "\Xdddd...\"
    /// </summary>
    public const string Hexadecimal = "X";
    /// <summary>
    /// locally defined escape sequence, will be if form "\Zdddd...\"    
    /// </summary>
    public const string LocallyDefined = "Z";
    /// <summary>
    /// Unknowen is a non HL7 Standard Escape i.e "\Qx54\", We will treat it as being 'Z' the LocallyDefined char
    /// </summary>
    public const string Unknown = "Z";
    /// <summary>
    /// NotAnEscape is not an Escape Seqence at all but 
    /// a Content Type of TEXT within this library
    /// </summary>
    public const string NotAnEscape = "?";

    ////////// Formatting Escapes //////////////////////////////////

    //Begin new output line. Set the horizontal position to the current left margin and increment the vertical
    //position by 1.
    public const string NewLine = ".br";
    /// <summary>
    /// End current output line and skip [number] vertical spaces
    /// [number] is absent, skip one space. The horizontal charac
    /// purposes of compatibility with previous versions of HL7, "^\.sp\" is equivalent to "\.br\."
    /// </summary>
    public const string SkipVerticalSpaces = ".sp";
    /// <summary>
    /// Begin word wrap or fill mode. This is the default state. It can be changed to a no-wrap mode using the .nf
    /// command.
    /// </summary>
    public const string WordWrapOn = ".fi";
    /// <summary>
    /// Begin no-wrap mode.
    /// </summary>
    public const string WordWrapOff = ".nf";
    /// <summary>
    /// Indent [number] of spaces, where [number] is a positive or negative integer. This command cannot
    /// appear after the first printable character of a line.
    /// </summary>
    public const string Indent = ".in";
    /// <summary>
    /// Temporarily indent [number] of spaces where number is a positive or negative integer. This command
    /// cannot appear after the first printable character of a line.
    /// </summary>
    public const string TempIndent = ".ti";
    /// <summary>
    /// Skip [number] spaces to the right.
    /// </summary>
    public const string SkipSpacesToRight = ".sk";
    /// <summary>
    /// End current output line and center the next line.
    /// </summary>
    public const string CenterNextLine = ".ce";

    public static string ResolveEscapeChararter(EscapeType EscapeType)
    {
      switch (EscapeType)
      {
        case EscapeType.Field:
          return Escapes.Field;
        case EscapeType.Component:
          return Escapes.Component;
        case EscapeType.SubComponent:
          return Escapes.SubComponent;
        case EscapeType.Repeat:
          return Escapes.Repeat;
        case EscapeType.Escape:
          return Escapes.Escape;
        case EscapeType.HighlightOn:
          return Escapes.HighlightStart;
        case EscapeType.HighlightOff:
          return Escapes.HighlightEnd;
        case EscapeType.LocallyDefined:
          return Escapes.Hexadecimal;
        case EscapeType.HexadecimalData:
          return Escapes.LocallyDefined;
        case EscapeType.NewLine:
          return Escapes.NewLine;
        case EscapeType.SkipVerticalSpaces:
          return Escapes.SkipVerticalSpaces;
        case EscapeType.WordWrapOn:
          return Escapes.WordWrapOn;
        case EscapeType.WordWrapOff:
          return Escapes.WordWrapOff;
        case EscapeType.Indent:
          return Escapes.Indent;
        case EscapeType.TempIndent:
          return Escapes.TempIndent;
        case EscapeType.SkipSpacesToRight:
          return Escapes.SkipSpacesToRight;
        case EscapeType.CenterNextLine:
          return Escapes.CenterNextLine;
        case EscapeType.NotAnEscape:
          return Escapes.NotAnEscape;
        case EscapeType.Unknown:
          return Escapes.Unknown;
        default:
          return Escapes.Unknown;
      }
    }
    public static EscapeType ResolveEscapeType(string EscapeString)
    {
      if (EscapeString.Trim().Length == 1)
      {
        try
        {
          string EscapeChar = EscapeString.Trim().ToUpper();
          if (EscapeChar == Support.Standard.Escapes.Field)
          {
            return Support.Standard.EscapeType.Field;
          }
          else if (EscapeChar == Support.Standard.Escapes.Component)
          {
            return Support.Standard.EscapeType.Component;
          }
          else if (EscapeChar == Support.Standard.Escapes.SubComponent)
          {
            return Support.Standard.EscapeType.SubComponent;
          }
          else if (EscapeChar == Support.Standard.Escapes.Repeat)
          {
            return Support.Standard.EscapeType.Repeat;
          }
          else if (EscapeChar == Support.Standard.Escapes.Escape)
          {
            return Support.Standard.EscapeType.Escape;
          }
          else if (EscapeChar == Support.Standard.Escapes.HighlightStart)
          {
            return Support.Standard.EscapeType.HighlightOn;
          }
          else if (EscapeChar == Support.Standard.Escapes.HighlightEnd)
          {
            return Support.Standard.EscapeType.HighlightOff;
          }
          else
          {
            return Support.Standard.EscapeType.Unknown;
          }
        }
        catch
        {
          return Support.Standard.EscapeType.Unknown;
        }
      }
      else
      {
        if (EscapeString.ToUpper().StartsWith(Support.Standard.Escapes.Hexadecimal.ToString()))
        {
          return Support.Standard.EscapeType.LocallyDefined;
        }
        else if (EscapeString.ToUpper().StartsWith(Support.Standard.Escapes.LocallyDefined.ToString()))
        {
          return Support.Standard.EscapeType.HexadecimalData;
        }
        else if (EscapeString.StartsWith(Support.Standard.Escapes.NewLine))
        {
          return Support.Standard.EscapeType.NewLine;
        }
        else if (EscapeString.StartsWith(Support.Standard.Escapes.SkipVerticalSpaces))
        {
          return Support.Standard.EscapeType.SkipVerticalSpaces;
        }
        else if (EscapeString.StartsWith(Support.Standard.Escapes.WordWrapOn))
        {
          return Support.Standard.EscapeType.WordWrapOn;
        }
        else if (EscapeString.StartsWith(Support.Standard.Escapes.WordWrapOff))
        {
          return Support.Standard.EscapeType.WordWrapOff;
        }
        else if (EscapeString.StartsWith(Support.Standard.Escapes.Indent))
        {
          return Support.Standard.EscapeType.Indent;
        }
        else if (EscapeString.StartsWith(Support.Standard.Escapes.TempIndent))
        {
          return Support.Standard.EscapeType.TempIndent;
        }
        else if (EscapeString.StartsWith(Support.Standard.Escapes.SkipSpacesToRight))
        {
          return Support.Standard.EscapeType.SkipSpacesToRight;
        }
        else if (EscapeString.StartsWith(Support.Standard.Escapes.SkipSpacesToRight))
        {
          return Support.Standard.EscapeType.SkipSpacesToRight;
        }
        else if (EscapeString.StartsWith(Support.Standard.Escapes.CenterNextLine))
        {
          return Support.Standard.EscapeType.CenterNextLine;
        }        
        else
        {
          return Support.Standard.EscapeType.Unknown;
        }
      }
    }

    /// <summary>
    /// Removes all escapes from a string
    /// </summary>
    /// <param name="StringRaw"></param>
    /// <returns></returns>

    public static string Decode(PeterPiper.Hl7.V2.Model.IContent oContent)
    {
      if (oContent.ContentType == Content.ContentType.Text)
        return oContent.AsStringRaw;
      if (oContent.ContentType == Content.ContentType.Escape)
      {
        switch (oContent.EscapeMetaData.EscapeType)
        {
          case EscapeType.Field:
            return oContent.MessageDelimiters.Field.ToString();
          case EscapeType.Repeat:
            return oContent.MessageDelimiters.Repeat.ToString();
          case EscapeType.Component:
            return oContent.MessageDelimiters.Component.ToString();
          case EscapeType.SubComponent:
            return oContent.MessageDelimiters.SubComponent.ToString();
          case EscapeType.Escape:
            return oContent.MessageDelimiters.Escape.ToString();
          case EscapeType.HighlightOn:
            return String.Empty;
          case EscapeType.HighlightOff:
            return String.Empty;
          case EscapeType.LocallyDefined:
            return String.Empty;
          case EscapeType.HexadecimalData:
            return String.Empty;
          case EscapeType.NewLine:
            return String.Empty;
          case EscapeType.SkipVerticalSpaces:
            return String.Empty;
          case EscapeType.WordWrapOn:
            return String.Empty;
          case EscapeType.WordWrapOff:
            return String.Empty;
          case EscapeType.Indent:
            return String.Empty;
          case EscapeType.TempIndent:
            return String.Empty;
          case EscapeType.SkipSpacesToRight:
            return String.Empty;
          case EscapeType.CenterNextLine:
            return String.Empty;
          case EscapeType.Unknown:
            return String.Empty;
          case EscapeType.NotAnEscape:
            return String.Empty;
          default:
            throw new PeterPiperException(String.Format("Unknown EscapeType passed into Decoder, Escapetype was: {0}", oContent.EscapeMetaData.EscapeType.ToString()));
        }
      }
      else
      {
        throw new PeterPiperException(String.Format("Unknown ContentType passed into Decoder, ContentType was: {0}", oContent.ContentType.ToString()));
      }
    }

    public static string Encode(string StringRaw, IMessageDelimiters CustomDelimiters)
    {
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.HighlightStart.ToLower()), String.Empty);
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.HighlightStart), String.Empty);

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.HighlightStart.ToLower()), String.Empty);
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.HighlightStart), String.Empty);

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.HighlightEnd.ToLower()), String.Empty);
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.HighlightEnd), String.Empty);

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Field.ToLower()), " ");
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Field), " ");

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Component.ToLower()), " ");
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Component), " ");

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.SubComponent.ToLower()), " ");
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.SubComponent), " ");

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Repeat.ToLower()), " ");
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Repeat), " ");

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Escape.ToLower()), " ");
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Escape), " ");

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.NewLine.ToUpper()), " ");
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.NewLine), " ");

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.WordWrapOn.ToUpper()), "");
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.WordWrapOn), "");

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.WordWrapOff.ToUpper()), "");
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.WordWrapOff), "");

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.CenterNextLine.ToUpper()), " ");
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.CenterNextLine), " ");

      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.CenterNextLine.ToUpper()), " ");
      StringRaw = StringRaw.Replace(String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.CenterNextLine), " ");

      // Have not catered for  .sp<numnber>, .in<number, .ti<number>, .sk<number> as <number> part is variable in length, may attempt to do later.

      StringRaw = StringRaw.Replace(CustomDelimiters.Escape.ToString(), String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Escape));
      StringRaw = StringRaw.Replace(CustomDelimiters.Repeat.ToString(), String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Repeat));
      StringRaw = StringRaw.Replace(CustomDelimiters.SubComponent.ToString(), String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.SubComponent));
      StringRaw = StringRaw.Replace(CustomDelimiters.Component.ToString(), String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Component));
      StringRaw = StringRaw.Replace(CustomDelimiters.Field.ToString(), String.Format("{0}{1}{0}", CustomDelimiters.Escape, Support.Standard.Escapes.Field));
      return StringRaw;
    }
  }
  
  public enum EscapeType
  {
    Field, Repeat, Component, SubComponent, Escape, HighlightOn, HighlightOff, LocallyDefined,
    HexadecimalData, NewLine, SkipVerticalSpaces, WordWrapOn, WordWrapOff, Indent, TempIndent,
    SkipSpacesToRight, CenterNextLine, Unknown, NotAnEscape
  };

}

using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.CustomException;
namespace PeterPiper.Hl7.V2.Support.Standard;

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

  public static string ResolveEscapeCharacter(EscapeType escapeType)
  {
    return escapeType switch
    {
      EscapeType.Field => Field,
      EscapeType.Component => Component,
      EscapeType.SubComponent => SubComponent,
      EscapeType.Repeat => Repeat,
      EscapeType.Escape => Escape,
      EscapeType.HighlightOn => HighlightStart,
      EscapeType.HighlightOff => HighlightEnd,
      EscapeType.LocallyDefined => Hexadecimal,
      EscapeType.HexadecimalData => LocallyDefined,
      EscapeType.NewLine => NewLine,
      EscapeType.SkipVerticalSpaces => SkipVerticalSpaces,
      EscapeType.WordWrapOn => WordWrapOn,
      EscapeType.WordWrapOff => WordWrapOff,
      EscapeType.Indent => Indent,
      EscapeType.TempIndent => TempIndent,
      EscapeType.SkipSpacesToRight => SkipSpacesToRight,
      EscapeType.CenterNextLine => CenterNextLine,
      EscapeType.NotAnEscape => NotAnEscape,
      EscapeType.Unknown => Unknown,
      _ => Unknown
    };
  }
  public static EscapeType ResolveEscapeType(string escapeString)
  {
    if (escapeString.Trim().Length == 1)
    {
      try
      {
        string EscapeChar = escapeString.Trim().ToUpper();
        if (EscapeChar == Field)
        {
          return EscapeType.Field;
        }
        else if (EscapeChar == Component)
        {
          return EscapeType.Component;
        }
        else if (EscapeChar == SubComponent)
        {
          return EscapeType.SubComponent;
        }
        else if (EscapeChar == Repeat)
        {
          return EscapeType.Repeat;
        }
        else if (EscapeChar == Escape)
        {
          return EscapeType.Escape;
        }
        else if (EscapeChar == HighlightStart)
        {
          return EscapeType.HighlightOn;
        }
        else if (EscapeChar == HighlightEnd)
        {
          return EscapeType.HighlightOff;
        }
        else
        {
          return EscapeType.Unknown;
        }
      }
      catch
      {
        return EscapeType.Unknown;
      }
    }
    else
    {
      if (escapeString.ToUpper().StartsWith(Hexadecimal))
      {
        return EscapeType.LocallyDefined;
      }
      else if (escapeString.ToUpper().StartsWith(LocallyDefined.ToString()))
      {
        return EscapeType.HexadecimalData;
      }
      else if (escapeString.StartsWith(NewLine))
      {
        return EscapeType.NewLine;
      }
      else if (escapeString.StartsWith(SkipVerticalSpaces))
      {
        return EscapeType.SkipVerticalSpaces;
      }
      else if (escapeString.StartsWith(WordWrapOn))
      {
        return EscapeType.WordWrapOn;
      }
      else if (escapeString.StartsWith(WordWrapOff))
      {
        return EscapeType.WordWrapOff;
      }
      else if (escapeString.StartsWith(Indent))
      {
        return EscapeType.Indent;
      }
      else if (escapeString.StartsWith(TempIndent))
      {
        return EscapeType.TempIndent;
      }
      else if (escapeString.StartsWith(SkipSpacesToRight))
      {
        return EscapeType.SkipSpacesToRight;
      }
      else if (escapeString.StartsWith(SkipSpacesToRight))
      {
        return EscapeType.SkipSpacesToRight;
      }
      else if (escapeString.StartsWith(CenterNextLine))
      {
        return EscapeType.CenterNextLine;
      }        
      else
      
      {
        return EscapeType.Unknown;
      }
    }
  }

  /// <summary>
  /// Removes all escapes from a string
  /// </summary>
  /// <param name="content"></param>
  /// <returns></returns>
  public static string Decode(IContent content)
  {
    if (content.ContentType == Content.ContentType.Text)
      return content.AsStringRaw;
    if (content.ContentType == Content.ContentType.Escape)
    {
      switch (content.EscapeMetaData.EscapeType)
      {
        case EscapeType.Field:
          return content.MessageDelimiters.Field.ToString();
        case EscapeType.Repeat:
          return content.MessageDelimiters.Repeat.ToString();
        case EscapeType.Component:
          return content.MessageDelimiters.Component.ToString();
        case EscapeType.SubComponent:
          return content.MessageDelimiters.SubComponent.ToString();
        case EscapeType.Escape:
          return content.MessageDelimiters.Escape.ToString();
        case EscapeType.HighlightOn:
          return string.Empty;
        case EscapeType.HighlightOff:
          return string.Empty;
        case EscapeType.LocallyDefined:
          return string.Empty;
        case EscapeType.HexadecimalData:
          return string.Empty;
        case EscapeType.NewLine:
          return string.Empty;
        case EscapeType.SkipVerticalSpaces:
          return string.Empty;
        case EscapeType.WordWrapOn:
          return string.Empty;
        case EscapeType.WordWrapOff:
          return string.Empty;
        case EscapeType.Indent:
          return string.Empty;
        case EscapeType.TempIndent:
          return string.Empty;
        case EscapeType.SkipSpacesToRight:
          return string.Empty;
        case EscapeType.CenterNextLine:
          return string.Empty;
        case EscapeType.Unknown:
          return string.Empty;
        case EscapeType.NotAnEscape:
          return string.Empty;
        default:
          throw new PeterPiperException(
            $"Unknown EscapeType passed into Decoder, EscapeType was: {content.EscapeMetaData.EscapeType.ToString()}");
      }
    }
    else
    {
      throw new PeterPiperException(
        $"Unknown ContentType passed into Decoder, ContentType was: {content.ContentType.ToString()}");
    }
  }

  public static string Encode(string stringRaw, IMessageDelimiters customDelimiters)
  {
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{HighlightStart.ToLower()}{customDelimiters.Escape}", string.Empty);
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{HighlightStart}{customDelimiters.Escape}", string.Empty);

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{HighlightStart.ToLower()}{customDelimiters.Escape}", string.Empty);
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{HighlightStart}{customDelimiters.Escape}", string.Empty);

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{HighlightEnd.ToLower()}{customDelimiters.Escape}", string.Empty);
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{HighlightEnd}{customDelimiters.Escape}", string.Empty);

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{Field.ToLower()}{customDelimiters.Escape}", " ");
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{Field}{customDelimiters.Escape}", " ");

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{Component.ToLower()}{customDelimiters.Escape}", " ");
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{Component}{customDelimiters.Escape}", " ");

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{SubComponent.ToLower()}{customDelimiters.Escape}", " ");
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{SubComponent}{customDelimiters.Escape}", " ");

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{Repeat.ToLower()}{customDelimiters.Escape}", " ");
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{Repeat}{customDelimiters.Escape}", " ");

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{Escape.ToLower()}{customDelimiters.Escape}", " ");
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{Escape}{customDelimiters.Escape}", " ");

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{NewLine.ToUpper()}{customDelimiters.Escape}", " ");
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{NewLine}{customDelimiters.Escape}", " ");

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{WordWrapOn.ToUpper()}{customDelimiters.Escape}", "");
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{WordWrapOn}{customDelimiters.Escape}", "");

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{WordWrapOff.ToUpper()}{customDelimiters.Escape}", "");
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{WordWrapOff}{customDelimiters.Escape}", "");

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{CenterNextLine.ToUpper()}{customDelimiters.Escape}", " ");
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{CenterNextLine}{customDelimiters.Escape}", " ");

    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{CenterNextLine.ToUpper()}{customDelimiters.Escape}", " ");
    stringRaw = stringRaw.Replace($"{customDelimiters.Escape}{CenterNextLine}{customDelimiters.Escape}", " ");

    // Have not catered for  .sp<numnber>, .in<number, .ti<number>, .sk<number> as <number> part is variable in length, may attempt to do later.

    stringRaw = stringRaw.Replace(customDelimiters.Escape.ToString(), $"{customDelimiters.Escape}{Escape}{customDelimiters.Escape}");
    stringRaw = stringRaw.Replace(customDelimiters.Repeat.ToString(), $"{customDelimiters.Escape}{Repeat}{customDelimiters.Escape}");
    stringRaw = stringRaw.Replace(customDelimiters.SubComponent.ToString(), $"{customDelimiters.Escape}{SubComponent}{customDelimiters.Escape}");
    stringRaw = stringRaw.Replace(customDelimiters.Component.ToString(), $"{customDelimiters.Escape}{Component}{customDelimiters.Escape}");
    stringRaw = stringRaw.Replace(customDelimiters.Field.ToString(), $"{customDelimiters.Escape}{Field}{customDelimiters.Escape}");
    return stringRaw;
  }
}
  
public enum EscapeType
{
  Field, Repeat, Component, SubComponent, Escape, HighlightOn, HighlightOff, LocallyDefined,
  HexadecimalData, NewLine, SkipVerticalSpaces, WordWrapOn, WordWrapOff, Indent, TempIndent,
  SkipSpacesToRight, CenterNextLine, Unknown, NotAnEscape
};
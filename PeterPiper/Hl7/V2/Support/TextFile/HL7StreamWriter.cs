using System.Collections.Generic;
using System.IO;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Support.TextFile;

public class Hl7StreamWriter 
{
  public enum Hl7OutputStyles { HumanReadable, InterfaceReadable };

  private string _Path;
  private bool _Append;
    
  public Hl7StreamWriter(string path, bool append)
  {
    _Path = path;
    _Append = append;
  }
  public Hl7StreamWriter(string path)
  {
    _Path = path;
    _Append = false;
  }

  private void _Write(string oneMessage, Hl7OutputStyles eHl7OutputStyle)
  {
    var Mode = FileMode.Create;
    if (_Append)
    {
      Mode = FileMode.Append;
    }
      
    using (var stream = new FileStream(_Path, Mode, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan))
    {
      using (var streamWriter = new StreamWriter(stream, System.Text.Encoding.UTF8, 1024, false))
      {
        if (eHl7OutputStyle == Hl7OutputStyles.HumanReadable)
        {
          string[] SpltMessageSegments = oneMessage.Split(PeterPiper.Hl7.V2.Support.Standard.Delimiters.SegmentTerminator);
          for (int i = 0; i < SpltMessageSegments.Length; i++)
          {
            streamWriter.Write($"{SpltMessageSegments[i]}{System.Environment.NewLine}");
          }
        }
        else if (eHl7OutputStyle == Hl7OutputStyles.InterfaceReadable)
        {
          streamWriter.Write(oneMessage);
          streamWriter.Write(System.Environment.NewLine);
        }
        else
        {
          throw new PeterPiperException("Unknown HL7OutputStyles of '" + eHl7OutputStyle.ToString() + "' Found");
        }
      }
    }
  }
    
  public void Write(string oneMessage, Hl7OutputStyles eHl7OutputStyle)
  {
    _Write(oneMessage, eHl7OutputStyle);
  }
  public void Write(PeterPiper.Hl7.V2.Model.IMessage oHl7, Hl7OutputStyles eHl7OutputStyle)
  {
    _Write(oHl7.AsStringRaw, eHl7OutputStyle);
  }
  public void Write(List<PeterPiper.Hl7.V2.Model.IMessage> oMessageList, Hl7OutputStyles eHl7OutputStyle)
  {
    foreach (var oHl7 in oMessageList)
    {
      _Write(oHl7.AsStringRaw, eHl7OutputStyle);
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Support.TextFile
{
  public class HL7StreamWriter 
  {
    public enum HL7OutputStyles { HumanReadable, InterfaceReadable };

    private string _Path;
    private bool _Append;
    
    public HL7StreamWriter(string path, bool Append)
    {
      _Path = path;
      _Append = Append;
    }
    public HL7StreamWriter(string path)
    {
      _Path = path;
      _Append = false;
    }

    private void _Write(string OneMessage, HL7OutputStyles eHL7OutputStyle)
    {
      var Mode = FileMode.Create;
      if (_Append)
        Mode = FileMode.Append;

      using (var stream = new FileStream(_Path, Mode, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan))
      {
        using (var streamWriter = new StreamWriter(stream, System.Text.Encoding.UTF8, 1024, false))
        {
          if (eHL7OutputStyle == HL7OutputStyles.HumanReadable)
          {
            string[] SpltMessagSegments = OneMessage.Split(PeterPiper.Hl7.V2.Support.Standard.Delimiters.SegmentTerminator);
            for (int i = 0; i < SpltMessagSegments.Length; i++)
            {
              streamWriter.Write(String.Format("{0}{1}", SpltMessagSegments[i], System.Environment.NewLine));
            }
          }
          else if (eHL7OutputStyle == HL7OutputStyles.InterfaceReadable)
          {
            streamWriter.Write(OneMessage);
            streamWriter.Write(System.Environment.NewLine);
          }
          else
          {
            throw new PeterPiperException("Unknown HL7OutputStyles of '" + eHL7OutputStyle.ToString() + "' Found");
          }
        }
      }
    }
    
    public void Write(string OneMessage, HL7OutputStyles eHL7OutputStyle)
    {
      _Write(OneMessage, eHL7OutputStyle);
    }
    public void Write(PeterPiper.Hl7.V2.Model.IMessage oHL7, HL7OutputStyles eHL7OutputStyle)
    {
      _Write(oHL7.AsStringRaw, eHL7OutputStyle);
    }
    public void Write(List<PeterPiper.Hl7.V2.Model.IMessage> oMessageList, HL7OutputStyles eHL7OutputStyle)
    {
      foreach (var oHL7 in oMessageList)
      {
        _Write(oHL7.AsStringRaw, eHL7OutputStyle);
      }
    }
  }
}

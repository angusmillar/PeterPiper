using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Glib.Hl7.V2.Support.TextFile
{
  public class Hl7StreamReader
  {    
    private StreamReader _Reader;
    private StringBuilder _Message;
    private readonly string CommentMarker = @"//";
    private string line;

    public string Read()
    {
      _Message.Clear();
      if (!String.IsNullOrWhiteSpace(line))
      {
        if (!line.TrimStart().StartsWith(CommentMarker))
        {
          _Message.Append(line);
          _Message.Append(Glib.Hl7.V2.Support.Standard.Delimiters.SegmentTerminator);
        }
      }
      while ((line = _Reader.ReadLine()) != null)
      {
        if (!String.IsNullOrWhiteSpace(line))
        {
          // '//' used for comments in file
          if (!line.TrimStart().StartsWith(CommentMarker))
          {
            if (line.StartsWith(Glib.Hl7.V2.Support.Standard.Segments.Msh.Code))
            {
              if (_Message.Length > 0)
              {                
                return _Message.ToString();
              }
              else
              {
                _Message.Clear();
              }
            }
            _Message.Append(line);
            _Message.Append(Glib.Hl7.V2.Support.Standard.Delimiters.SegmentTerminator);
          }
        }
      }
      // Flush the last message in the string list out
      if (_Message.Length > 0)
      {
        return _Message.ToString();
      }
      return null;
    }

    public void Close()
    {
      _Reader.Close();
    }

    public Hl7StreamReader(string path)
    {
      _Reader = new StreamReader(path);      
      _Message = new StringBuilder();
    }

    public Hl7StreamReader(Stream stream)
    {
      _Reader = new StreamReader(stream);
      _Message = new StringBuilder();
    }


  }
}

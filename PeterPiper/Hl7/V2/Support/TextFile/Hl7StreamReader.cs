using System;
using System.IO;
using System.Text;


namespace PeterPiper.Hl7.V2.Support.TextFile
{
  public class Hl7StreamReader
  {
    private Stream _Stream;
    private StreamReader _Reader;
    private StringBuilder _Message;
    private readonly string CommentMarker = @"//";
    private string line;
    //private string _Path;

    public Hl7StreamReader(string path)
    {
      _Stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.SequentialScan);
      _Reader = new StreamReader(_Stream);
      
    }

    public Hl7StreamReader(Stream stream)
    {
      _Stream = stream;

      _Reader = new StreamReader(stream);
      
    }


    public string Read()
    {
      _Message = new StringBuilder();
      if (!String.IsNullOrWhiteSpace(line))
      {
        if (!line.TrimStart().StartsWith(CommentMarker))
        {
          _Message.Append(line);
          _Message.Append(PeterPiper.Hl7.V2.Support.Standard.Delimiters.SegmentTerminator);
        }
      }

      
      
        while ((line = _Reader.ReadLine()) != null)
      {
        if (!String.IsNullOrWhiteSpace(line))
        {
          // '//' used for comments in file
          if (!line.TrimStart().StartsWith(CommentMarker))
          {
            if (line.StartsWith(PeterPiper.Hl7.V2.Support.Standard.Segments.Msh.Code))
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
            _Message.Append(PeterPiper.Hl7.V2.Support.Standard.Delimiters.SegmentTerminator);
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
      _Stream.Dispose();
      _Reader.Dispose();
    }



  }
}

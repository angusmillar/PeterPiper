using System;
using System.IO;
using System.Text;


namespace PeterPiper.Hl7.V2.Support.TextFile;

public class Hl7StreamReader
{
    private readonly Stream _Stream;
    private readonly StreamReader _Reader;
    private StringBuilder _Message;
    private const string CommentMarker = @"//";
    private string _Line;

    public Hl7StreamReader(string path)
    {
        _Stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096,
            FileOptions.SequentialScan);
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
        if (!string.IsNullOrWhiteSpace(_Line))
        {
            if (!_Line.TrimStart().StartsWith(CommentMarker))
            {
                _Message.Append(_Line);
                _Message.Append(Standard.Delimiters.SegmentTerminator);
            }
        }


        while ((_Line = _Reader.ReadLine()) != null)
        {
            if (!String.IsNullOrWhiteSpace(_Line))
            {
                // '//' used for comments in file
                if (!_Line.TrimStart().StartsWith(CommentMarker))
                {
                    if (_Line.StartsWith(Standard.Segments.Msh.Code))
                    {
                        if (_Message.Length > 0)
                        {
                            return _Message.ToString();
                        }

                        _Message.Clear();
                    }

                    _Message.Append(_Line);
                    _Message.Append(Standard.Delimiters.SegmentTerminator);
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
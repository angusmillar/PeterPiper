using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation
{
  public class File : IFile
  {
    private MessageDelimiters _Delimiters;
    private List<Batch> _BatchList = new List<Batch>();
    private ISegment _FileHeader;
    private ISegment _FileTrailer;

    internal File(string StringRaw)
    {
      List<string> FileSegmentList = StringRaw.Split(Support.Standard.Delimiters.SegmentTerminator).ToList();
      if (!FileSegmentList.Any())
      {
        throw new PeterPiperException(String.Format("The passed file must begin with the File Header Segment and code: '{0}'", Support.Standard.Segments.Fhs.Code));
      }

      string FHSSegmentStringRaw = FileSegmentList.First();
      if (Message.IsSegmentCode(FHSSegmentStringRaw, Support.Standard.Segments.Fhs.Code))
      {
        this.Delimiters = Implementation.Message.ExtractDelimitersFromStringRaw(FHSSegmentStringRaw);
        _FileHeader = new Segment(FHSSegmentStringRaw, this._Delimiters);
        FileSegmentList.Remove(FHSSegmentStringRaw);

      }
      else
      {
        throw new PeterPiperException(String.Format("The passed message must begin with the File Header Segment and code: '{0}'", Support.Standard.Segments.Fhs.Code));
      }

      if (Message.IsSegmentCode(FileSegmentList.Last(), Support.Standard.Segments.Fts.Code))
      {
        _FileTrailer = new Segment(FileSegmentList.Last(), FileHeader.MessageDelimiters);
        FileSegmentList.Remove(FileSegmentList.Last());
      }

      List<List<string>> BatchSegmentList = GetBatchSegmentList(FileSegmentList);
      for (int i = 0; i < BatchSegmentList.Count; i++)
      {
        try
        {
          Batch Batch = new Batch(BatchSegmentList[i], this._Delimiters);
          try
          {
            this.AddBatch(Batch);
          }
          catch (Exception e)
          {
            throw new PeterPiperException(String.Format("Batch {i} in the File is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.", i), e);
          }
        }
        catch (PeterPiperException peterPiperException)
        {
          throw new PeterPiperException(String.Format("Batch {i} in the File was unable to be parsed.", i), peterPiperException);
        }
      }
    }
    internal File(ISegment FileHeaderSegment, List<IBatch> BatchList, ISegment FileTrailerSegment)
    {
      if (!Message.IsSegmentCode(FileHeaderSegment.Code, Support.Standard.Segments.Fhs.Code))
      {
        throw new PeterPiperException(String.Format("The provided Batch Header Segment (BHS) has the incorrect code of {0}", FileHeaderSegment.Code));
      }

      if (!Message.IsSegmentCode(FileTrailerSegment.Code, Support.Standard.Segments.Fts.Code))
      {
        throw new PeterPiperException(String.Format("The provided File Trailer Segment (FTS) has the incorrect code of {0}", FileTrailerSegment.Code));
      }

      _Delimiters = FileHeaderSegment.MessageDelimiters as MessageDelimiters;
      _FileHeader = FileHeaderSegment;

      if (!ValidateDelimiters(FileTrailerSegment.MessageDelimiters))
      {
        throw new PeterPiperException("The provided File Trailer Segment (FTS) has different HL7 Delimiters than used by the File Header Segment (FHS), this is not allowed.");
      }
      _FileTrailer = FileTrailerSegment;

      for (int i = 0; i < BatchList.Count; i++)
      {
        try
        {
          this.AddBatch(BatchList[i]);
        }
        catch (Exception e)
        {
          throw new PeterPiperException(String.Format("Batch {i} in the File is using different HL7 message delimiters to its parent FHS Segment, this is not allowed.", i), e);
        }
      }
    }
    internal File(ISegment FileHeaderSegment, List<IBatch> BatchList)
    {
      if (!Message.IsSegmentCode(FileHeaderSegment.Code, Support.Standard.Segments.Fhs.Code))
      {
        throw new PeterPiperException(String.Format("The provided File Header Segment (FHS) has the incorrect code of {0}", FileHeaderSegment.Code));
      }

      _Delimiters = FileHeaderSegment.MessageDelimiters as MessageDelimiters;
      _FileHeader = FileHeaderSegment;

      for (int i = 0; i < BatchList.Count; i++)
      {
        try
        {
          this.AddBatch(BatchList[i]);
        }
        catch (Exception e)
        {
          throw new PeterPiperException(String.Format("Batch {i} in the File is using different HL7 message delimiters to its parent FHS Segment, this is not allowed.", i), e);
        }
      }
    }
    internal File(ISegment FileHeaderSegment)
    {
      if (!Message.IsSegmentCode(FileHeaderSegment.Code, Support.Standard.Segments.Fhs.Code))
      {
        throw new PeterPiperException(String.Format("The provided File Header Segment (FHS) has the incorrect code of {0}", FileHeaderSegment.Code));
      }

      _Delimiters = FileHeaderSegment.MessageDelimiters as MessageDelimiters;
      FileHeader = FileHeaderSegment;
    }

    internal File()
    {
      _FileHeader = new Segment(Support.Standard.Segments.Fhs.Code + Support.Standard.Delimiters.Field);
      _Delimiters = _FileHeader.MessageDelimiters as MessageDelimiters;
    }


    public ISegment FileHeader
    {
      get => _FileHeader;
      set
      {
        if (!Message.IsSegmentCode(value.Code, Support.Standard.Segments.Fhs.Code))
        {
          throw new PeterPiperException(String.Format("The provided File Header Segment (FHS) has the incorrect code of {0}", value.Code));
        }

        if (!ValidateDelimiters(value.MessageDelimiters))
        {
          throw new PeterPiperException("The provided File Header Segment (FHS) has different HL7 Delimiters than used to first construct this File object, this is not allowed.");
        }

        _FileHeader = value;
      }
    }

    public ISegment FileTrailer
    {
      get => _FileTrailer;
      set
      {
        if (!ValidateDelimiters(value.MessageDelimiters))
        {
          throw new PeterPiperException("The provided File Trailer Segment (FTS) has different HL7 Delimiters than used by the File Header Segment (FHS), this is not allowed.");
        }

        _FileTrailer = value;
      }
    }

    private bool ValidateDelimiters(IMessageDelimiters DelimitersToCompaire)
    {
      if (_Delimiters.Field != DelimitersToCompaire.Field)
        return false;
      if (_Delimiters.Component != DelimitersToCompaire.Component)
        return false;
      if (_Delimiters.SubComponent != DelimitersToCompaire.SubComponent)
        return false;
      if (_Delimiters.Repeat != DelimitersToCompaire.Repeat)
        return false;
      if (_Delimiters.Escape != DelimitersToCompaire.Escape)
        return false;
      return true;
      ;
    }
    private static List<List<string>> GetBatchSegmentList(List<string> FileSegmentList)
    {
      var BatchSegmentList = new List<List<string>>();
      List<string> SegmentList = null;
      foreach (var Segment in FileSegmentList)
      {
        if (SegmentList is null)
        {
          SegmentList = new List<string>();
          if (Message.IsSegmentCode(Segment, Support.Standard.Segments.Bhs.Code))
          {
            SegmentList.Add(Segment);
          }
          else
          {
            throw new PeterPiperException(String.Format("The second Segment of a File passed must begin with the Batch Header Segment and code: '{0}'",
                                                        Support.Standard.Segments.Bhs.Code));
          }
        }
        else if (Message.IsSegmentCode(Segment, Support.Standard.Segments.Bhs.Code))
        {
          BatchSegmentList.Add(SegmentList);
          SegmentList = new List<string>();
          SegmentList.Add(Segment);
        }
        else
        {
          SegmentList.Add(Segment);
        }
      }

      BatchSegmentList.Add(SegmentList);
      return BatchSegmentList;
    }
    internal MessageDelimiters Delimiters
    {
      get { return _Delimiters; }
      set { _Delimiters = value; }
    }
    public string AsString
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        sb.Append(FileHeader.AsString);
        sb.Append(Support.Standard.Delimiters.SegmentTerminator);
        _BatchList.ForEach(Msg => sb.Append(Msg.AsString));
        if (FileTrailer != null)
        {
          sb.AppendLine(FileTrailer.AsString);
          sb.Append(Support.Standard.Delimiters.SegmentTerminator);
        }

        return sb.ToString();
      }
    }

    public override string ToString()
    {
      return AsString;
    }

    public string AsStringRaw
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        sb.Append(FileHeader.AsStringRaw);
        sb.Append(Support.Standard.Delimiters.SegmentTerminator);
        _BatchList.ForEach(Msg => sb.Append(Msg.AsStringRaw));
        if (FileTrailer != null)
        {
          sb.AppendLine(FileTrailer.AsStringRaw);
          sb.Append(Support.Standard.Delimiters.SegmentTerminator);
        }

        return sb.ToString();
      }
    }

    public void ClearAll()
    {
      _BatchList.Clear();
      _FileTrailer = null;
      _FileHeader.ClearAll();
    }

    public IFile Clone()
    {
      List<IBatch> ClonedBatchList = new List<IBatch>();
      _BatchList.ForEach(x => ClonedBatchList.Add(x.Clone()));
      return new File(FileHeaderSegment: FileHeader.Clone(), BatchList: ClonedBatchList, FileTrailerSegment: FileTrailer.Clone());
    }

    public string EscapeSequence
    {
      get { return String.Format("{0}{1}{2}{3}", this.Delimiters.Component, this.Delimiters.Repeat, this.Delimiters.Escape, this.Delimiters.SubComponent); }
    }

    public string MainSeparator
    {
      get { return String.Format("{0}", this.Delimiters.Field); }
    }

    public IMessageDelimiters MessageDelimiters
    {
      get { return _Delimiters; }
    }
    public void AddBatch(IBatch item)
    {
      if (ValidateDelimiters(item.MessageDelimiters))
      {
        _BatchList.Add(item as Batch);
      }
      else
      {
        throw new PeterPiperException("The Batch being added to the File is using different HL7 message delimiters to its parent FHS Segment, this is not allowed.");
      }
    }

    public void InsertBatch(int index, IBatch item)
    {
      _BatchList.Insert(index, item as Batch);
    }

    public void RemoveBatchAt(int index)
    {
      _BatchList.RemoveAt(index);
    }

    public IBatch Batch(int index)
    {
      return _BatchList[index];
    }

    public int BatchCount()
    {
      return _BatchList.Count;
    }

    public ReadOnlyCollection<IBatch> BatchList()
    {
      return _BatchList.Select(i => i as IBatch).ToList().AsReadOnly();
    }
    
  }
}

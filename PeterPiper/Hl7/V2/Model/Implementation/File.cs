using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Model.Implementation;

public class File : IFile
{
  private MessageDelimiters _Delimiters;
  private readonly List<Batch> _BatchList = new ();
  private ISegment _FileHeader;
  private ISegment _FileTrailer;

  internal File(string stringRaw)
  {
    List<string> FileSegmentList = stringRaw.Split(Support.Standard.Delimiters.SegmentTerminator).ToList();
    if (string.IsNullOrWhiteSpace(FileSegmentList.Last()))
    {
      FileSegmentList.Remove(FileSegmentList.Last());
    }
      
    if (!FileSegmentList.Any())
    {
      throw new PeterPiperException(
        $"The passed file must begin with the File Header Segment and code: '{Support.Standard.Segments.Fhs.Code}'");
    }

    string fhsSegmentStringRaw = FileSegmentList.First();
    if (Message.IsSegmentCode(fhsSegmentStringRaw, Support.Standard.Segments.Fhs.Code))
    {
      _Delimiters = Message.ExtractDelimitersFromStringRaw(fhsSegmentStringRaw);
      _FileHeader = new Segment(fhsSegmentStringRaw, _Delimiters, true, null, null);
      FileSegmentList.Remove(fhsSegmentStringRaw);

    }
    else
    {
      throw new PeterPiperException(
        $"The passed message must begin with the File Header Segment and code: '{Support.Standard.Segments.Fhs.Code}'");
    }

    if (Message.IsSegmentCode(FileSegmentList.Last(), Support.Standard.Segments.Fts.Code))
    {
      _FileTrailer = new Segment(FileSegmentList.Last(), _Delimiters, true, null, null);
      FileSegmentList.Remove(FileSegmentList.Last());
    }

    List<List<string>> BatchSegmentList = GetBatchSegmentList(FileSegmentList);
    for (int i = 0; i < BatchSegmentList.Count; i++)
    {
      try
      {
        Batch Batch = new Batch(BatchSegmentList[i], _Delimiters);
        try
        {
          AddBatch(Batch);
        }
        catch (Exception exception)
        {
          throw new PeterPiperException($"Batch {i} in the File is using different HL7 message delimiters to its parent BHS Segment, this is not allowed.", exception);
        }
      }
      catch (PeterPiperException peterPiperException)
      {
        throw new PeterPiperException($"Batch {i} in the File was unable to be parsed.", peterPiperException);
      }
    }
  }
  internal File(ISegment fileHeaderSegment, List<IBatch> batchList, ISegment fileTrailerSegment)
  {
    if (!Message.IsSegmentCode(fileHeaderSegment.Code, Support.Standard.Segments.Fhs.Code))
    {
      throw new PeterPiperException(
        $"The provided Batch Header Segment (BHS) has the incorrect code of {fileHeaderSegment.Code}");
    }

    if (!Message.IsSegmentCode(fileTrailerSegment.Code, Support.Standard.Segments.Fts.Code))
    {
      throw new PeterPiperException(
        $"The provided File Trailer Segment (FTS) has the incorrect code of {fileTrailerSegment.Code}");
    }

    _Delimiters = fileHeaderSegment.MessageDelimiters as MessageDelimiters;
    _FileHeader = fileHeaderSegment;

    if (!ValidateDelimiters(fileTrailerSegment.MessageDelimiters))
    {
      throw new PeterPiperException("The provided File Trailer Segment (FTS) has different HL7 Delimiters than used by the File Header Segment (FHS), this is not allowed.");
    }
    _FileTrailer = fileTrailerSegment;

    for (int i = 0; i < batchList.Count; i++)
    {
      try
      {
        AddBatch(batchList[i]);
      }
      catch (Exception exception)
      {
        throw new PeterPiperException($"Batch {i} in the File is using different HL7 message delimiters to its parent FHS Segment, this is not allowed.", exception);
      }
    }
  }
  internal File(ISegment fileHeaderSegment, List<IBatch> batchList)
  {
    if (!Message.IsSegmentCode(fileHeaderSegment.Code, Support.Standard.Segments.Fhs.Code))
    {
      throw new PeterPiperException(
        $"The provided File Header Segment (FHS) has the incorrect code of {fileHeaderSegment.Code}");
    }

    _Delimiters = fileHeaderSegment.MessageDelimiters as MessageDelimiters;
    _FileHeader = fileHeaderSegment;

    for (int i = 0; i < batchList.Count; i++)
    {
      try
      {
        AddBatch(batchList[i]);
      }
      catch (Exception exception)
      {
        throw new PeterPiperException($"Batch {i} in the File is using different HL7 message delimiters to its parent FHS Segment, this is not allowed.", exception);
      }
    }
  }
  internal File(ISegment fileHeaderSegment)
  {
    if (!Message.IsSegmentCode(fileHeaderSegment.Code, Support.Standard.Segments.Fhs.Code))
    {
      throw new PeterPiperException(
        $"The provided File Header Segment (FHS) has the incorrect code of {fileHeaderSegment.Code}");
    }

    _Delimiters = fileHeaderSegment.MessageDelimiters as MessageDelimiters;
    FileHeader = fileHeaderSegment;
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
        throw new PeterPiperException($"The provided File Header Segment (FHS) has the incorrect code of {value.Code}");
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

  private bool ValidateDelimiters(IMessageDelimiters delimitersToCompare)
  {
    if (_Delimiters.Field != delimitersToCompare.Field)
      return false;
    if (_Delimiters.Component != delimitersToCompare.Component)
      return false;
    if (_Delimiters.SubComponent != delimitersToCompare.SubComponent)
      return false;
    if (_Delimiters.Repeat != delimitersToCompare.Repeat)
      return false;
    if (_Delimiters.Escape != delimitersToCompare.Escape)
      return false;
    return true;
    ;
  }
  private static List<List<string>> GetBatchSegmentList(List<string> fileSegmentList)
  {
    var BatchSegmentList = new List<List<string>>();
    List<string> SegmentList = null;
    foreach (var Segment in fileSegmentList)
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
          throw new PeterPiperException(
            $"The second Segment of a File passed must begin with the Batch Header Segment and code: '{Support.Standard.Segments.Bhs.Code}'");
        }
      }
      else if (Message.IsSegmentCode(Segment, Support.Standard.Segments.Bhs.Code))
      {
        BatchSegmentList.Add(SegmentList);
        SegmentList = new List<string> { Segment };
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
    get => _Delimiters;
    set => _Delimiters = value;
  }
  public string AsString
  {
    get
    {
      StringBuilder sb = new StringBuilder();
      sb.Append(FileHeader.AsString);
      sb.Append(Support.Standard.Delimiters.SegmentTerminator);
      _BatchList.ForEach(msg => sb.Append(msg.AsString));
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
      _BatchList.ForEach(batch => sb.Append(batch.AsStringRaw));
      if (FileTrailer != null)
      {
        sb.Append(FileTrailer.AsStringRaw);
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
    return new File(fileHeaderSegment: FileHeader.Clone(), batchList: ClonedBatchList, fileTrailerSegment: FileTrailer.Clone());
  }

  public string EscapeSequence => $"{Delimiters.Component}{Delimiters.Repeat}{Delimiters.Escape}{Delimiters.SubComponent}";

  public string MainSeparator => Delimiters.Field.ToString();

  public IMessageDelimiters MessageDelimiters => _Delimiters;

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
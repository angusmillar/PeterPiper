using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Glib.Hl7.Nehta.CDA.DataType
{
  public class EncapsulatedData
  {
    private string _value;
    public string value
    {
      get { return _value; }
      set { _value = value; }
    }

    private byte[] _data;
    public byte[] data
    {
      get { return _data; }
      set { _data = value; }
    }

    private XElement _xml;
    public XElement xml
    {
      get { return _xml; }
      set { _xml = value; }
    }

    private Link _reference;
    public Link reference
    {
      get { return _reference; }
      set { _reference = value; }
    }

    private CodedText _mediaType;
    public CodedText mediaType
    {
      get { return _mediaType; }
      set { _mediaType = value; }
    }

    private CodedText _charset;
    public CodedText charset
    {
      get { return _charset; }
      set { _charset = value; }
    }

    private string _language;
    public string language
    {
      get { return _language; }
      set { _language = value; }
    }

    private Enum.CompressionAlgorithm _compression;
    public Enum.CompressionAlgorithm compression
    {
      get { return _compression; }
      set { _compression = value; }
    }

    private byte[] _integrityCheck;
    public byte[] integrityCheck
    {
      get { return _integrityCheck; }
      set { _integrityCheck = value; }
    }

    private Enum.IntegrityCheckAlgorithm _integrityCheckAlgorithm;
    public Enum.IntegrityCheckAlgorithm integrityCheckAlgorithm
    {
      get { return _integrityCheckAlgorithm; }
      set { _integrityCheckAlgorithm = value; }
    }

    private Text _description;
    public Text description
    {
      get { return _description; }
      set { _description = value; }
    }

    private EncapsulatedData _thumbnail;
    public EncapsulatedData thumbnail
    {
      get { return _thumbnail; }
      set { _thumbnail = value; }
    }

    private EncapsulatedData[] _translation;
    public EncapsulatedData[] translation
    {
      get { return _translation; }
      set { _translation = value; }
    }
    
  }
}

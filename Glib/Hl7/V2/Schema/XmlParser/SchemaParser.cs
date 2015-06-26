using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;


namespace Glib.Hl7.V2.Schema.XmlParser
{
  public class SchemaParser
  {
    private string RootDirectoryAllVersions = @"C:\Users\angusm\Documents\Dropbox\Sun_HL7v2xsd\hl7v2xsd";
    private Dictionary<string, FileInfo> XmlFileDictionary;
    
    private List<Model.DataTypeBase> oDataTypeList;
    private Dictionary<string, Schema.Model.SegmentStructure> oSegmentDictionary;

    public void Run()
    {
      Dictionary<string, List<Schema.Model.MessageStructure>> VersionSchemaDictionary = new Dictionary<string, List<Model.MessageStructure>>(); 
      foreach (var Dir in Directory.EnumerateDirectories(RootDirectoryAllVersions))
      {
        string[] SupportedVersions = new string[] { "2.3", "2.3.1", "2.4", "2.5", "2.5.1" };
        var DicertoryInfo = new DirectoryInfo(Dir);
        if (SupportedVersions.Contains(DicertoryInfo.Name))
          VersionSchemaDictionary.Add(DicertoryInfo.Name, LoadVersion(Dir, DicertoryInfo.Name));
      }      
    }

    public List<Schema.Model.MessageStructure> LoadVersion(string Dir, string Version)
    {
        GetFilePaths(Dir);
        LoadDataType(Version);
        //Must load segments first then Fields
        LoadSegment();
        LoadField();
        return LoadMessageList();
    }

    private void LoadDataType(string Version)
    {
      DataTypeParser oDataTypeParser = new DataTypeParser(Version);
      oDataTypeList = oDataTypeParser.Run(LoadXMLDocument(HL7v2Xsd.Filename.DataTypes));
    }

    private void LoadField()
    {
      //The field.xsd also contains the Segment codes so we produce the SegmentDictionary here and 
      //then enhance it with cardinality information for each field when we load the Segment.xsd
      FieldParser oFieldParser = new FieldParser();
      oSegmentDictionary = oFieldParser.Run(LoadXMLDocument(HL7v2Xsd.Filename.Segments), oSegmentDictionary);
    }

    private void LoadSegment()
    {
      //The field.xsd also contains the Segment codes so we produce the SegmentDictionary here and 
      //then enhance it with cardinality information for each field when we load the Segment.xsd
      SegmentParser oSegmentParser = new SegmentParser();
      oSegmentDictionary = oSegmentParser.Run(LoadXMLDocument(HL7v2Xsd.Filename.Fields), oDataTypeList);      
    }

    private List<Schema.Model.MessageStructure> LoadMessageList()
    {
      List<Schema.Model.MessageStructure> MessageStructureList = new List<Model.MessageStructure>(); 
      MessageParser oMessageParser = new MessageParser();
      List<string> FilenameList = oMessageParser.Run(LoadXMLDocument(HL7v2Xsd.Filename.Messages));
      foreach (var Filename in FilenameList)
      {
        MessageTypeParser oMessageTypeParser = new MessageTypeParser();
        MessageStructureList.Add(oMessageTypeParser.Run(LoadXMLDocument(Filename), oSegmentDictionary));
      }
      return MessageStructureList;
    }

    private XDocument LoadXMLDocument(string FileName)
    {
      XDocument xDoc = null;
      try
      {
        xDoc = XDocument.Load(XmlFileDictionary[FileName].FullName);
      }
      catch (Exception Exec)
      {
        throw new Exception("Could not load and or parse the .xsd file: " + XmlFileDictionary[FileName].FullName, Exec);
      }
      return xDoc;
    }
    private void GetFilePaths(string Directory)
    {
      DirectoryInfo MessageInputDirectory = new DirectoryInfo(Directory);
      XmlFileDictionary = MessageInputDirectory.GetFiles("*.xsd", SearchOption.TopDirectoryOnly).ToDictionary(f => f.Name, f => f);
    }
  }
}

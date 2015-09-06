using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;


namespace Glib.Hl7.V2.Schema.XmlParser
{
  public static class SchemaParser
  {
    #region Private Properties
    
    private static string EmbededResourseMask = "Glib.Hl7.V2.Schema.XmlParser.XMLSchemaFiles.{0}.";    
    private static Model.VersionsSupported _VersionCurrent;        
    private static List<Model.DataTypeBase> oDataTypeList;
    private static Dictionary<string, Schema.Model.SegmentStructure> oSegmentDictionary;
    
    #endregion

    #region Public Methods

    public static Dictionary<Model.VersionsSupported, Schema.Model.VersionSchema> LoadAll()
    {
      var VersionSchemaDictionary = new Dictionary<Model.VersionsSupported, Schema.Model.VersionSchema>();
      foreach (Model.VersionsSupported version in Enum.GetValues(typeof(Model.VersionsSupported)))
      {
        if (version != Model.VersionsSupported.NotSupported)
        {
          _VersionCurrent = version;
          VersionSchemaDictionary.Add(_VersionCurrent, LoadVersion());
        }
      }
      return VersionSchemaDictionary;
    }
    public static Schema.Model.VersionSchema LoadSingleMessage(Model.VersionsSupported version, string MessageType, string MessageEvent)
    {
      var VersionSchemaDictionary = new Dictionary<Model.VersionsSupported, Schema.Model.VersionSchema>();
      _VersionCurrent = version;
      VersionSchemaDictionary.Add(_VersionCurrent, LoadVersion(MessageType, MessageEvent));
      if (VersionSchemaDictionary.Count != 0)
        return VersionSchemaDictionary[version];
      else
        return null;
    }

    public static void LoadAnotherMessage(Schema.Model.VersionSchema VersionSchema, string MessageType, string MessageEvent)
    {      
      _VersionCurrent = VersionSchema.Version;
      LoadAnotherMessageForSameVersion(VersionSchema, MessageType, MessageEvent);
    }


    #endregion

    #region Private Methods
    
    private static Schema.Model.VersionSchema LoadVersion()
    {        
        LoadDataType();
        //Must load segments first then Fields
        LoadSegment();
        LoadField();
        var oVersionSchema = new Schema.Model.VersionSchema();
        oVersionSchema.Version = _VersionCurrent;
        oVersionSchema.MessageStructureList = LoadMessageList();

        oVersionSchema.CompositeList = oDataTypeList.OfType<Schema.Model.Composite>();
        oVersionSchema.PrimitiveList = oDataTypeList.OfType<Schema.Model.Primitive>();
        oVersionSchema.SegmentDictionary = oSegmentDictionary;

        return oVersionSchema;

    }
    private static Schema.Model.VersionSchema LoadVersion(string MessageType, string MessageEvent)
    {      
      LoadDataType();
      //Must load segments first then Fields
      LoadSegment();
      LoadField();
      var oVersionSchema = new Schema.Model.VersionSchema();
      oVersionSchema.Version = _VersionCurrent;
      oVersionSchema.CompositeList = oDataTypeList.OfType<Schema.Model.Composite>();
      oVersionSchema.PrimitiveList = oDataTypeList.OfType<Schema.Model.Primitive>();
      oVersionSchema.SegmentDictionary = oSegmentDictionary;
      oVersionSchema.MessageStructureList = LoadMessageList(MessageType, MessageEvent);
    
      return oVersionSchema;
    }

    private static Schema.Model.VersionSchema LoadAnotherMessageForSameVersion(Schema.Model.VersionSchema VersionSchema, string MessageType, string MessageEvent)
    {      
      if (null == VersionSchema.MessageStructureList.SingleOrDefault(x => x.MessageType == MessageType && x.MessageEvent == MessageEvent))
      {
        //LoadDataType();
        oDataTypeList.Clear();
        oDataTypeList.AddRange(VersionSchema.PrimitiveList);
        oDataTypeList.AddRange(VersionSchema.CompositeList);

        //Must load segments first then Fields
        //LoadSegment();
        //oSegmentDictionary.Clear();
        oSegmentDictionary = VersionSchema.SegmentDictionary;

        //LoadField();
       // No need to load fields as they are already loaed from the first call

        VersionSchema.MessageStructureList.AddRange(LoadMessageList(MessageType, MessageEvent));
      }
      return VersionSchema;
    }
    private static void LoadDataType()
    {
      DataTypeParser oDataTypeParser = new DataTypeParser(_VersionCurrent);
      oDataTypeList = oDataTypeParser.Run(LoadXMLDocument(HL7v2Xsd.Filename.DataTypes));
    }
    private static void LoadField()
    {
      //The field.xsd also contains the Segment codes so we produce the SegmentDictionary here and 
      //then enhance it with cardinality information for each field when we load the Segment.xsd
      FieldParser oFieldParser = new FieldParser();
      oSegmentDictionary = oFieldParser.Run(LoadXMLDocument(HL7v2Xsd.Filename.Segments), oSegmentDictionary);
    }
    private static void LoadSegment()
    {
      //The field.xsd also contains the Segment codes so we produce the SegmentDictionary here and 
      //then enhance it with cardinality information for each field when we load the Segment.xsd
      SegmentParser oSegmentParser = new SegmentParser();
      oSegmentDictionary = oSegmentParser.Run(LoadXMLDocument(HL7v2Xsd.Filename.Fields), oDataTypeList);      
    }
    private static List<Schema.Model.MessageStructure> LoadMessageList(string MessageType, string MessageEvent)
    {
      List<Schema.Model.MessageStructure> MessageStructureList = new List<Model.MessageStructure>(); 
      MessageParser oMessageParser = new MessageParser();
      List<string> FilenameList = oMessageParser.Run(LoadXMLDocument(HL7v2Xsd.Filename.Messages));
      var FileName = FilenameList.SingleOrDefault(x => x == String.Format("{0}_{1}.xsd",MessageType, MessageEvent));
      if (FileName != null)
      {
        MessageTypeParser oMessageTypeParser = new MessageTypeParser();
        MessageStructureList.Add(oMessageTypeParser.Run(LoadXMLDocument(FileName), oSegmentDictionary));
      }
      return MessageStructureList;
    }
    private static List<Schema.Model.MessageStructure> LoadMessageList()
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
    private static XDocument LoadXMLDocument(string FileName)
    {
      XDocument xDoc = null;
      string ResourseToLoad = String.Format(EmbededResourseMask, _VersionCurrent.ToString()) + FileName;
      try
      {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();        
        Stream oStream = assembly.GetManifestResourceStream(ResourseToLoad);
        xDoc = XDocument.Load(oStream);        
      }
      catch (Exception Exec)
      {
        throw new Exception("Could not load and or parse the .xsd file: " + ResourseToLoad, Exec);
      }
      return xDoc;
    }

    #endregion
  }
}

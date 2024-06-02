using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace PeterPiper.Hl7.V2.Schema.XmlParser;

public static class SchemaParser
{
    private const string EmbeddedResourceMask = "PeterPiper.Hl7.V2.Schema.XmlParser.XMLSchemaFiles.{0}.";
    private static Model.VersionsSupported _versionCurrent;
    private static List<Model.DataTypeBase> _dataTypeList;
    private static Dictionary<string, Model.SegmentStructure> _segmentDictionary;

    public static Dictionary<Model.VersionsSupported, Model.VersionSchema> LoadAll()
    {
        var VersionSchemaDictionary = new Dictionary<Model.VersionsSupported, Model.VersionSchema>();
        foreach (Model.VersionsSupported version in Enum.GetValues(typeof(Model.VersionsSupported)))
        {
            if (version != Model.VersionsSupported.NotSupported)
            {
                _versionCurrent = version;
                VersionSchemaDictionary.Add(_versionCurrent, LoadVersion());
            }
        }

        return VersionSchemaDictionary;
    }

    public static Model.VersionSchema LoadSingleMessage(Model.VersionsSupported version, string messageType,
        string messageEvent)
    {
        var VersionSchemaDictionary = new Dictionary<Model.VersionsSupported, Model.VersionSchema>();
        _versionCurrent = version;
        VersionSchemaDictionary.Add(_versionCurrent, LoadVersion(messageType, messageEvent));
        if (VersionSchemaDictionary.Count != 0)
        {
            return VersionSchemaDictionary[version];
        }

        return null;
    }

    public static void LoadAnotherMessage(Model.VersionSchema versionSchema, string messageType, string messageEvent)
    {
        _versionCurrent = versionSchema.Version;
        LoadAnotherMessageForSameVersion(versionSchema, messageType, messageEvent);
    }


    private static Model.VersionSchema LoadVersion()
    {
        LoadDataType();
        //Must load segments first then Fields
        LoadSegment();
        LoadField();
        
        var oVersionSchema = new Model.VersionSchema();
        oVersionSchema.Version = _versionCurrent;
        oVersionSchema.MessageStructureList = LoadMessageList();

        oVersionSchema.CompositeList = _dataTypeList.OfType<Model.Composite>();
        oVersionSchema.PrimitiveList = _dataTypeList.OfType<Model.Primitive>();
        oVersionSchema.SegmentDictionary = _segmentDictionary;

        return oVersionSchema;
    }

    private static Model.VersionSchema LoadVersion(string messageType, string messageEvent)
    {
        LoadDataType();
        //Must load segments first then Fields
        LoadSegment();
        LoadField();
        
        var oVersionSchema = new Model.VersionSchema();
        oVersionSchema.Version = _versionCurrent;
        oVersionSchema.CompositeList = _dataTypeList.OfType<Model.Composite>();
        oVersionSchema.PrimitiveList = _dataTypeList.OfType<Model.Primitive>();
        oVersionSchema.SegmentDictionary = _segmentDictionary;
        oVersionSchema.MessageStructureList = LoadMessageList(messageType, messageEvent);

        return oVersionSchema;
    }

    private static Model.VersionSchema LoadAnotherMessageForSameVersion(Model.VersionSchema versionSchema,
        string messageType, string messageEvent)
    {
        if (null == versionSchema.MessageStructureList.SingleOrDefault(x =>
                x.MessageType == messageType && 
                x.MessageEvent == messageEvent))
        {
            
            _dataTypeList.Clear();
            _dataTypeList.AddRange(versionSchema.PrimitiveList);
            _dataTypeList.AddRange(versionSchema.CompositeList);

            //Must load segments first then Fields
            _segmentDictionary = versionSchema.SegmentDictionary;
            
            // No need to load fields as they are already loaed from the first call
            versionSchema.MessageStructureList.AddRange(LoadMessageList(messageType, messageEvent));
        }

        return versionSchema;
    }

    private static void LoadDataType()
    {
        DataTypeParser oDataTypeParser = new DataTypeParser(_versionCurrent);
        _dataTypeList = oDataTypeParser.Run(LoadXmlDocument(HL7v2Xsd.Filename.DataTypes));
    }

    private static void LoadField()
    {
        //The field.xsd also contains the Segment codes so we produce the SegmentDictionary here and 
        //then enhance it with cardinality information for each field when we load the Segment.xsd
        FieldParser oFieldParser = new FieldParser();
        _segmentDictionary = oFieldParser.Run(LoadXmlDocument(HL7v2Xsd.Filename.Segments), _segmentDictionary);
    }

    private static void LoadSegment()
    {
        //The field.xsd also contains the Segment codes so we produce the SegmentDictionary here and 
        //then enhance it with cardinality information for each field when we load the Segment.xsd
        SegmentParser oSegmentParser = new SegmentParser();
        _segmentDictionary = oSegmentParser.Run(LoadXmlDocument(HL7v2Xsd.Filename.Fields), _dataTypeList);
    }

    private static List<Model.MessageStructure> LoadMessageList(string messageType, string messageEvent)
    {
        List<Model.MessageStructure> MessageStructureList = new List<Model.MessageStructure>();
        MessageParser oMessageParser = new MessageParser();
        List<string> FilenameList = oMessageParser.Run(LoadXmlDocument(HL7v2Xsd.Filename.Messages));
        var FileName = FilenameList.SingleOrDefault(x => x == $"{messageType}_{messageEvent}.xsd");
        if (FileName != null)
        {
            MessageTypeParser oMessageTypeParser = new MessageTypeParser();
            MessageStructureList.Add(oMessageTypeParser.Run(LoadXmlDocument(FileName), _segmentDictionary));
        }

        return MessageStructureList;
    }

    private static List<Model.MessageStructure> LoadMessageList()
    {
        List<Model.MessageStructure> MessageStructureList = new List<Model.MessageStructure>();
        MessageParser oMessageParser = new MessageParser();
        List<string> FilenameList = oMessageParser.Run(LoadXmlDocument(HL7v2Xsd.Filename.Messages));
        foreach (var Filename in FilenameList)
        {
            MessageTypeParser oMessageTypeParser = new MessageTypeParser();
            MessageStructureList.Add(oMessageTypeParser.Run(LoadXmlDocument(Filename), _segmentDictionary));
        }

        return MessageStructureList;
    }

    private static XDocument LoadXmlDocument(string fileName)
    {
        XDocument xDocument;
        string resourceToLoad = string.Format(EmbeddedResourceMask, _versionCurrent.ToString()) + fileName;
        try
        {
            var assembly = typeof(SchemaParser).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourceToLoad);
            ArgumentNullException.ThrowIfNull(stream);
            xDocument = XDocument.Load(stream);
        }
        catch (Exception Exec)
        {
            throw new Exception("Could not load and or parse the .xsd file: " + resourceToLoad, Exec);
        }

        return xDocument;
    }
}
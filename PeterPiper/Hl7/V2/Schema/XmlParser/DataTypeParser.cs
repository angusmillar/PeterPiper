using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PeterPiper.Hl7.V2.Schema.XmlParser;

class DataTypeParser
{
    //private string SchemaParser.XMLSchemaNameSpace = "http://www.w3.org/2001/XMLSchema";
    //private string SchemaParser.Hl7NameSpace = "urn:com.sun:encoder-hl7-1.0";
    private XDocument _XDocument;
    private readonly Model.VersionsSupported _Version;
    private List<Model.Composite> _CompositeList;
    private List<Model.DataTypeBase> _DataTypeList;
    private Dictionary<string, List<int>> _CompositeIndexList;
    private IEnumerable<XElement> _CompositeElementList;

    public DataTypeParser(Model.VersionsSupported version)
    {
        _Version = version;
    }

    public List<Model.DataTypeBase> Run(XDocument xDocument)
    {
        _XDocument = xDocument;
        _DataTypeList = new List<Model.DataTypeBase>();
        IEnumerable<XElement> PrimitivesDataTypeElementList = GetElementList(HL7v2Xsd.Elements.SimpleType);
        GetPrimitiveList(PrimitivesDataTypeElementList);
        _CompositeElementList = GetElementList(HL7v2Xsd.Elements.ComplexType);
        _CompositeIndexList = GetCompositeIndexList();
        _CompositeList = new List<Model.Composite>();
        BuildCompositeStructureList();

        return _DataTypeList;
    }

    private IEnumerable<XElement> GetElementList(XName xElementName)
    {
        var documentRoot = _XDocument.Root;
        ArgumentNullException.ThrowIfNull(documentRoot);

        return documentRoot
            .DescendantsAndSelf()
            .Elements()
            .Where(d => d.Name == xElementName);
    }

    private void GetPrimitiveList(IEnumerable<XElement> primitiveElementList)
    {
        foreach (var x in primitiveElementList)
        {
            XElement RestrictionElement = x.Element(HL7v2Xsd.Elements.Restriction);
            ArgumentNullException.ThrowIfNull(RestrictionElement);

            XAttribute baseAttribute = RestrictionElement.Attribute(HL7v2Xsd.Attributes.Base);
            ArgumentNullException.ThrowIfNull(baseAttribute);

            XAttribute nameAttribute = x.Attribute(HL7v2Xsd.Attributes.Name);
            ArgumentNullException.ThrowIfNull(nameAttribute);

            if (baseAttribute.Value == "xsd:string")
            {
                if (_Version == Model.VersionsSupported.V2_3)
                {
                    //Version 2.3 talks of CM - composite as a data type that should not be used anymore although it is
                    //used throughout this version's .xsd's. The xsd's also states the following primitives codes yet never 
                    //later references them ("CM_CCP", "CM_CD_ELECTRODE", "CM_CSU", "CM_MDV", "CM_OSD"). I believe that all of these
                    // are in fact to be CM primitives. That is what I have done, dropped this set and only used CM and the parse works.  
                    // See the HL7 V2.3.1 standard chapter 2.8.6 for more info. 
                    Model.Primitive oNew = new Model.Primitive();
                    oNew.Code = nameAttribute.Value;

                    bool AddPrimitive = !(oNew.Code == "CM_CCP" || oNew.Code == "CM_CD_ELECTRODE" ||
                                          oNew.Code == "CM_CSU" || oNew.Code == "CM_MDV" || oNew.Code == "CM_OSD");
                    if (AddPrimitive)
                    {
                        oNew.Name = Model.PrimitiveSupport.GetNameForCode(oNew.Code);
                        _DataTypeList.Add(oNew);
                    }
                }
                else
                {
                    Model.Primitive oNew = new Model.Primitive();
                    oNew.Code = nameAttribute.Value;
                    oNew.Name = Model.PrimitiveSupport.GetNameForCode(oNew.Code);
                    _DataTypeList.Add(oNew);
                }
            }
            else
            {
                throw new Exception("Primitive base not found to be xsd:string");
            }
        }

        if (_Version == Model.VersionsSupported.V2_3)
        {
            Model.Primitive cm = new Model.Primitive();
            cm.Code = "CM";
            cm.Name = Model.PrimitiveSupport.GetNameForCode(cm.Code);
            _DataTypeList.Add(cm);
        }
        //Text data and formated text are strange primitives because they can contain a primitive data type of 'escape'.
        //In this sense both are really composites. Yet this library does not treat the escape as a data type but rather
        //as a construct much the same way segment, element, field, component, sub-component are constructs. This library
        //treats Escape as a sub construct of the Sub-Component and refers to it as 'content'. For this reason I am 
        //treating both Text data and formated text as pure primitives.
        //I have also hard coded them in here as the XSD interpretation sees then as composites for the reason explained.
        //All HL7 V2.* have these two data types and I can not foresee them ever being removed.

        Model.Primitive oFormatedText = new Model.Primitive();
        oFormatedText.Code = "FT";
        oFormatedText.Name = Model.PrimitiveSupport.GetNameForCode(oFormatedText.Code);
        _DataTypeList.Add(oFormatedText);

        Model.Primitive oTextData = new Model.Primitive();
        oTextData.Code = "TX";
        oTextData.Name = Model.PrimitiveSupport.GetNameForCode(oTextData.Code);
        _DataTypeList.Add(oTextData);

        //The Varies primitive data type is also odd in the XSD as it is named 'varies' yet later the code used for it
        // is '*' with in the fields.xsd. Then to make matters worse in Version 2.5 they begin to use 'var' instead of '*'.
        Model.Primitive oVariesData = new Model.Primitive();
        oVariesData.Code = "*";
        oVariesData.Name = Model.PrimitiveSupport.GetNameForCode(oVariesData.Code);
        _DataTypeList.Add(oVariesData);
    }

    private Dictionary<string, List<int>> GetCompositeIndexList()
    {
        Dictionary<string, List<int>> Dic = new Dictionary<string, List<int>>();

        foreach (var x in _CompositeElementList.Where(x => x.Descendants(HL7v2Xsd.Elements.Sequence).Any()))
        {
            XAttribute nameAttribute = x.Attribute(HL7v2Xsd.Attributes.Name);
            ArgumentNullException.ThrowIfNull(nameAttribute);

            string Name = nameAttribute.Value;

            XElement sequenceElement = x.Element(HL7v2Xsd.Elements.Sequence);
            ArgumentNullException.ThrowIfNull(sequenceElement);

            IEnumerable<XElement> ElementElementList = sequenceElement.Elements(HL7v2Xsd.Elements.Element);
            List<int> IntegerList = new List<int>();
            bool HasRef = false;
            foreach (var Seq in ElementElementList)
            {
                if (Seq.Attribute(HL7v2Xsd.Attributes.Ref) != null)
                {
                    HasRef = true;

                    XAttribute refAttribute = Seq.Attribute(HL7v2Xsd.Attributes.Ref);
                    ArgumentNullException.ThrowIfNull(refAttribute);

                    string sIndex = refAttribute.Value;
                    sIndex = sIndex.Split('.')[1];
                    int iIndex;
                    try
                    {
                        iIndex = Convert.ToInt32(sIndex);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(
                            $"The index of a Composite data type was not a valid integer, found: {sIndex} in data type ref: {sIndex}",
                            exception);
                    }

                    IntegerList.Add(iIndex);
                }
            }

            if (HasRef)
            {
                Dic.Add(Name, IntegerList);
            }
        }

        return Dic;
    }

    private List<Model.Composite> BuildCompositeStructureList()
    {
        foreach (var CompElement in _CompositeIndexList)
        {
            if (!_DataTypeList.Exists(x => x.Code == CompElement.Key))
            {
                _DataTypeList.Add(GetComposite(CompElement));
            }
        }

        return _CompositeList;
    }

    private Model.Composite GetComposite(KeyValuePair<string, List<int>> item)
    {
        Model.Composite oComposite = new Model.Composite();
        oComposite.Code = item.Key;
        foreach (var index in item.Value)
        {
            XName target = $"{item.Key}.{index.ToString()}.CONTENT";
            XElement CompDetail = (from x in _CompositeElementList
                where (string)x.Attribute(HL7v2Xsd.Attributes.Name) == target
                select x).Single();
            Model.CompositeItem oSchemaBase = BuildSingleComposite(CompDetail);
            oComposite.CompositeItem.Add(index, oSchemaBase);
        }

        return oComposite;
    }


    private Model.CompositeItem BuildSingleComposite(XElement compDetail)
    {
        XElement annotationElement = compDetail.Element(HL7v2Xsd.Elements.Annotation);
        ArgumentNullException.ThrowIfNull(annotationElement);

        XElement AppInfoElement = annotationElement.Element(HL7v2Xsd.Elements.Appinfo);
        ArgumentNullException.ThrowIfNull(AppInfoElement);

        XElement typeElement = AppInfoElement.Element(HL7v2Xsd.Elements.Type);
        ArgumentNullException.ThrowIfNull(typeElement);

        XElement longNameElement = AppInfoElement.Element(HL7v2Xsd.Elements.LongName);
        ArgumentNullException.ThrowIfNull(longNameElement);

        XElement tableElement = AppInfoElement.Element(HL7v2Xsd.Elements.Table);

        string DataTypeCode = typeElement.Value;
        string DataTypeDescription = longNameElement.Value;
        string DataTypeTable = string.Empty;
        if (tableElement != null)
        {
            DataTypeTable = tableElement.Value;
        }

        Model.DataTypeBase oDataTypeBase = _DataTypeList.SingleOrDefault(x => x.Code == DataTypeCode);
        Model.CompositeItem oComponent;
        
        if (oDataTypeBase != null)
        {
            oComponent = new Model.CompositeItem();
            oComponent.Description = DataTypeDescription;
            oComponent.Type = oDataTypeBase;
            oComponent.Hl7TableIndex = 0;
            SetHl7Table(DataTypeTable, oComponent);
            return oComponent;
        }

        oDataTypeBase = _DataTypeList.SingleOrDefault(x => x.Code == DataTypeCode);
        if (oDataTypeBase != null)
        {
            oComponent = new Model.CompositeItem();
            oComponent.Description = DataTypeDescription;
            oComponent.Type = oDataTypeBase;
            oComponent.Hl7TableIndex = 0;
            SetHl7Table(DataTypeTable, oComponent);
            return oComponent;
        }

        //if the sub data type is not in the primitives or the current composite lists then it has not been resolved as yet 
        //and must be a composite as all primitives are resolved at this point. There for we lookup this new composite
        //and resolve it then continue on as planed.
        KeyValuePair<string, List<int>> oSingleCompositeIndexList =
            _CompositeIndexList.Single(x => x.Key == DataTypeCode);
        _DataTypeList.Add(GetComposite(oSingleCompositeIndexList));
        oDataTypeBase = _DataTypeList.SingleOrDefault(x => x.Code == DataTypeCode);

        oComponent = new Model.CompositeItem();
        oComponent.Description = DataTypeDescription;
        oComponent.Type = oDataTypeBase;
        oComponent.Hl7TableIndex = 0;
        SetHl7Table(DataTypeTable, oComponent);
        return oComponent;
    }

    private static void SetHl7Table(string dataTypeTable, Model.CompositeItem oComponent)
    {
        if (!string.IsNullOrWhiteSpace(dataTypeTable))
        {
            try
            {
                if (dataTypeTable.Substring(0, 3) != "HL7")
                {
                    throw new Exception(
                        "A data type references a table that is not prefixed with HL7, what is this table. Ref found was: " +
                        dataTypeTable);
                }
                oComponent.Hl7TableIndex = Convert.ToInt32(dataTypeTable.Substring(3, (dataTypeTable.Length - 3)));
            }
            catch (Exception exec)
            {
                throw new Exception(
                    "A HL7 Table reference was not an integer in the XML schema files. Found: " + dataTypeTable, exec);
            }
        }
    }
}
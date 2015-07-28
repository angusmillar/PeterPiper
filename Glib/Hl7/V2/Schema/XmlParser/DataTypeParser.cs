using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Glib.Hl7.V2.Schema.XmlParser
{
  class DataTypeParser
  {
    //private string SchemaParser.XMLSchemaNameSpace = "http://www.w3.org/2001/XMLSchema";
    //private string SchemaParser.Hl7NameSpace = "urn:com.sun:encoder-hl7-1.0";
    private XDocument xDoc;
    private Model.VersionsSupported Version;
    private List<Model.Composite> CompositeList;
    private List<Model.DataTypeBase> DataTypeList;
    private Dictionary<string, List<int>> CompositeIndexList;
    private IEnumerable<XElement> CompositeElementList;


    public DataTypeParser(Model.VersionsSupported version)
    {
      Version = version;
    }

    public List<Model.DataTypeBase> Run(XDocument xDocument)
    {
      xDoc = xDocument;
      
      DataTypeList = new List<Model.DataTypeBase>();

      IEnumerable<XElement> PrimitivesDataTypeElementList = GetElementList(HL7v2Xsd.Elements.SimpleType);
      GetPrimitiveList(PrimitivesDataTypeElementList);

      CompositeElementList = GetElementList(HL7v2Xsd.Elements.ComplexType);
      CompositeIndexList = GetCompositeIndexList();
      CompositeList = new List<Model.Composite>();
      BuildCompositeStructureList();

      return DataTypeList;
    }

    private IEnumerable<XElement> GetElementList(XName XElementName)
    {
      return xDoc.Root
                 .DescendantsAndSelf()
                 .Elements()
                 .Where(d => d.Name == XElementName);
    }

    private void GetPrimitiveList(IEnumerable<XElement> PrimitiveElementList)
    {
      foreach (var x in PrimitiveElementList)
      {
        if (x.Element(HL7v2Xsd.Elements.Restriction).Attribute(HL7v2Xsd.Attributes.Base).Value == "xsd:string")
        {
          if (Version == Model.VersionsSupported.V2_3)
          {  
            //Version 2.3 talks of CM - composite as a data type that should not be used anymore although it is
            //used throughout this version's .xsd's. The xsd's also states the following primitives codes yet never 
            //later references them ("CM_CCP", "CM_CD_ELECTRODE", "CM_CSU", "CM_MDV", "CM_OSD"). I believe that all of these
            // are in fact to be CM primitives. That is what I have done, dropped this set and only used CM and the parse works.  
            // See the HL7 V2.3.1 standard chapter 2.8.6 for more info. 
            Model.Primitive oNew = new Model.Primitive();
            oNew.Code = x.Attribute(HL7v2Xsd.Attributes.Name).Value;          
            bool AddPrimitive = true;
            if (oNew.Code == "CM_CCP" || oNew.Code == "CM_CD_ELECTRODE" ||
                oNew.Code == "CM_CSU" || oNew.Code == "CM_MDV" || oNew.Code == "CM_OSD")
              AddPrimitive = false;
            if (AddPrimitive)
            {
              oNew.Name = Model.PrimitiveSupport.GetNameForCode(oNew.Code);
              DataTypeList.Add(oNew);
            }
          }
          else
          {
            Model.Primitive oNew = new Model.Primitive();
            oNew.Code = x.Attribute(HL7v2Xsd.Attributes.Name).Value;          
            oNew.Name = Model.PrimitiveSupport.GetNameForCode(oNew.Code);
            DataTypeList.Add(oNew);
          }
        }
        else
        {
          throw new Exception("Primitive base not found to be xsd:string");
        }
      }
      if (Version == Model.VersionsSupported.V2_3)
      {
        Model.Primitive oCM = new Model.Primitive();
        oCM.Code = "CM";
        oCM.Name = Model.PrimitiveSupport.GetNameForCode(oCM.Code);
        DataTypeList.Add(oCM);
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
      DataTypeList.Add(oFormatedText);

      Model.Primitive oTextData = new Model.Primitive();
      oTextData.Code = "TX";
      oTextData.Name = Model.PrimitiveSupport.GetNameForCode(oTextData.Code);
      DataTypeList.Add(oTextData);

      //The Varies primitive data type is also odd in the XSD as it is named 'varies' yet later the code used for it
      // is '*' with in the fields.xsd. Then to make matters worse in Version 2.5 they begin to use 'var' instead of '*'.
      Model.Primitive oVariesData = new Model.Primitive();
      oVariesData.Code = "*";
      oVariesData.Name = Model.PrimitiveSupport.GetNameForCode(oVariesData.Code);
      DataTypeList.Add(oVariesData);
    }

    private Dictionary<string, List<int>> GetCompositeIndexList()
    {                 
      Dictionary<string, List<int>> Dic = new Dictionary<string, List<int>>();

      foreach (var x in CompositeElementList.Where(x => x.Descendants(HL7v2Xsd.Elements.Sequence).Any()))
      {
        string Name = x.Attribute(HL7v2Xsd.Attributes.Name).Value;
        IEnumerable<XElement> ElementElementList = x.Element(HL7v2Xsd.Elements.Sequence).Elements(HL7v2Xsd.Elements.Element);
        List<int> IntegerList = new List<int>();
        bool HasRef = false;
        foreach (var Seq in ElementElementList)
        {
          if (Seq.Attribute(HL7v2Xsd.Attributes.Ref) != null)
          {
            HasRef = true;
            string sIndex = Seq.Attribute(HL7v2Xsd.Attributes.Ref).Value;
            sIndex = sIndex.Split('.')[1];
            int iIndex = 0;
            try
            {
              iIndex = Convert.ToInt32(sIndex);
            }
            catch (Exception Exec)
            {
              throw new Exception("The index of a Composite data type was not a valid integer, found: " + sIndex + " in data type ref: " + sIndex, Exec);
            }
            IntegerList.Add(iIndex);
          }
        }
        if (HasRef)
          Dic.Add(Name, IntegerList);
      }
      return Dic;
    }

    private List<Model.Composite> BuildCompositeStructureList()
    {
      foreach (var CompElement in CompositeIndexList)
      {
        if (!DataTypeList.Exists(x => x.Code == CompElement.Key))
        {
          DataTypeList.Add(GetComposite(CompElement));
        }
      }
      return CompositeList;
    }

    private Model.Composite GetComposite(KeyValuePair<string, List<int>> item)
    {
      Model.Composite oComposite = new Model.Composite();
      oComposite.Code = item.Key;
      foreach (var index in item.Value)
      {
        XName target = String.Format("{0}.{1}.{2}", item.Key, index.ToString(), "CONTENT");
        XElement CompDetail = (from x in CompositeElementList where (string)x.Attribute(HL7v2Xsd.Attributes.Name) == target select x).Single();
        Model.CompositeItem oSchemaBase = BuildSingleComposite(CompDetail);
        oComposite.CompositeItem.Add(index, oSchemaBase);
      }
      return oComposite;
    }


    private Model.CompositeItem BuildSingleComposite(XElement CompDetail)
    {
      string DataTypeCode = CompDetail.Element(HL7v2Xsd.Elements.Annotation).Element(HL7v2Xsd.Elements.Appinfo).Element(HL7v2Xsd.Elements.Type).Value;
      string DataTypeDescription = CompDetail.Element(HL7v2Xsd.Elements.Annotation).Element(HL7v2Xsd.Elements.Appinfo).Element(HL7v2Xsd.Elements.LongName).Value;
      string DataTypeTable = string.Empty;
      if (CompDetail.Element(HL7v2Xsd.Elements.Annotation).Element(HL7v2Xsd.Elements.Appinfo).Element(HL7v2Xsd.Elements.Table) != null)
        DataTypeTable = CompDetail.Element(HL7v2Xsd.Elements.Annotation).Element(HL7v2Xsd.Elements.Appinfo).Element(HL7v2Xsd.Elements.Table).Value;

      Model.DataTypeBase oDataTypeBase = DataTypeList.SingleOrDefault(x => x.Code == DataTypeCode);
      if (oDataTypeBase != null)
      {
        Model.CompositeItem oComponent = new Model.CompositeItem();
        oComponent.Description = DataTypeDescription;
        oComponent.Type = oDataTypeBase;
        oComponent.Hl7TableIndex = 0;
        SetHl7Table(DataTypeTable, oComponent);
        return oComponent;
      }
      else
      {
        oDataTypeBase = DataTypeList.SingleOrDefault(x => x.Code == DataTypeCode);
        if (oDataTypeBase != null)
        {
          Model.CompositeItem oComponent = new Model.CompositeItem();
          oComponent.Description = DataTypeDescription;
          oComponent.Type = oDataTypeBase;
          oComponent.Hl7TableIndex = 0;
          SetHl7Table(DataTypeTable, oComponent);
          return oComponent;
        }
        else
        {
          //if the sub data type is not in the primitives or the current composite lists then it has not been resolved as yet 
          //and must be a composite as all primitives are resolved at this point. There for we lookup this new composite
          //and resolve it then continue on as planed.
          KeyValuePair<string, List<int>> oSingleCompositeIndexList = CompositeIndexList.Single(x => x.Key == DataTypeCode);
          DataTypeList.Add(GetComposite(oSingleCompositeIndexList));
          oDataTypeBase = DataTypeList.SingleOrDefault(x => x.Code == DataTypeCode);

          Model.CompositeItem oComponent = new Model.CompositeItem();
          oComponent.Description = DataTypeDescription;
          oComponent.Type = oDataTypeBase;
          oComponent.Hl7TableIndex = 0;
          SetHl7Table(DataTypeTable, oComponent);
          return oComponent;
        }
      }
    }

    private static void SetHl7Table(string DataTypeTable, Model.CompositeItem oComponent)
    {
      if (DataTypeTable != String.Empty)
      {
        try
        {
          if (DataTypeTable.Substring(0, 3) != "HL7")
            throw new Exception("A data type references a table that is not prefixed with HL7, what is this table. Ref found was: " + DataTypeTable);
          oComponent.Hl7TableIndex = Convert.ToInt32(DataTypeTable.Substring(3, (DataTypeTable.Length - 3)));
        }
        catch (Exception exec)
        {
          throw new Exception("A HL7 Table reference was not an integer in the XML schema files. Found: " + DataTypeTable, exec);
        }
      }
    }

  }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Glib.Hl7.V2.Schema.XmlParser;



namespace TestHl7V2.TestSchema
{
  [TestFixture]
  public class TestSchemaParser
  {
    /// <summary>
    ///A test to load a single message & verson schema ORU^R01
    ///</summary>
    [Test]
    public void TestLoadSingleMessage()
    {

      var SingleMessage = SchemaParser.LoadSingleMessage(Glib.Hl7.V2.Schema.Model.VersionsSupported.V2_4, "ORU", "R01");      

      Assert.AreEqual(SingleMessage.MessageStructureList[0].MessageType, "ORU");
      Assert.AreEqual(SingleMessage.MessageStructureList[0].MessageEvent, "R01");
      var PatientResult = SingleMessage.MessageStructureList[0].MessageItemList[1] as Glib.Hl7.V2.Schema.Model.MessageSegmentGroup;
      var OrderObservation = PatientResult.SegmentGroupItemList[1] as Glib.Hl7.V2.Schema.Model.MessageSegmentGroup;
      var Observation = OrderObservation.SegmentGroupItemList[4] as Glib.Hl7.V2.Schema.Model.MessageSegmentGroup;
      var OBX = Observation.SegmentGroupItemList[0] as Glib.Hl7.V2.Schema.Model.MessageSegment;
      Assert.AreEqual(OBX.Segment.Code, "OBX");
      Assert.AreEqual(OBX.Segment.SegmentFieldList[5].Description, "Observation Value");

      Glib.Hl7.V2.Model.Message oMsg = new Glib.Hl7.V2.Model.Message("2.4", "ORU", "R01");
      string test = oMsg.Segment("MSH").Field(10).PathDetail.PathVerbos;

    }

    /// <summary>
    ///A test to load all 5 schemas
    ///</summary>
    [Test]
    public void TestLoadAll()
    {

      var AllVersions = SchemaParser.LoadAll();      
      Assert.AreEqual(AllVersions.Count, 5, "Should have support for 5 version found support for: " + AllVersions.Count);
    }

    [Test]
    public void TestLoadSingleMessage2()
    {

      Glib.Hl7.V2.Model.Message oMsg = new Glib.Hl7.V2.Model.Message("2.4", "ORU", "R01");
      oMsg.Add(new Glib.Hl7.V2.Model.Segment("PID"));
      oMsg.Add(new Glib.Hl7.V2.Model.Segment("PV1"));
      oMsg.Add(new Glib.Hl7.V2.Model.Segment("ORC"));
      oMsg.Add(new Glib.Hl7.V2.Model.Segment("OBR"));      
      oMsg.Add(new Glib.Hl7.V2.Model.Segment("OBX"));

      oMsg.Segment("PID").Element(3).Add(new Glib.Hl7.V2.Model.Field("100^^CODE^CODE2"));
      oMsg.Segment("PID").Element(3).Add(new Glib.Hl7.V2.Model.Field("1003^^CODE^CODE3"));
      int test = oMsg.Segment("PID").Element(3).RepeatCount;

      oMsg.Segment("PID").Field(5).Component(1).AsString = "Millar";
      oMsg.Segment("PID").Field(5).Component(2).AsString = "Angus";



      var SingleMessage = SchemaParser.LoadSingleMessage(oMsg.PathDetail.MessageVersionSupported, oMsg.PathDetail.MessageType, oMsg.PathDetail.MessageEvent);

      Assert.AreEqual(SingleMessage.MessageStructureList[0].MessageType, "ORU");
      Assert.AreEqual(SingleMessage.MessageStructureList[0].MessageEvent, "R01");
      var PatientResult = SingleMessage.MessageStructureList[0].MessageItemList[1] as Glib.Hl7.V2.Schema.Model.MessageSegmentGroup;
      var OrderObservation = PatientResult.SegmentGroupItemList[1] as Glib.Hl7.V2.Schema.Model.MessageSegmentGroup;
      var Observation = OrderObservation.SegmentGroupItemList[4] as Glib.Hl7.V2.Schema.Model.MessageSegmentGroup;
      var OBX = Observation.SegmentGroupItemList[0] as Glib.Hl7.V2.Schema.Model.MessageSegment;
      Assert.AreEqual(OBX.Segment.Code, "OBX");
      Assert.AreEqual(OBX.Segment.SegmentFieldList[5].Description, "Observation Value");


    }
    

  }
}

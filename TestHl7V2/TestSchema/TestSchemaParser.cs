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

    

  }
}

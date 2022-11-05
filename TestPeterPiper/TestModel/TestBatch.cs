using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TestHl7V2
{
  [TestClass]
  public class TestBatch
  {
    public IMessageDelimiters CustomDelimiters;

    [TestInitialize]
    public void MyTestInitialize()
    {
      CustomDelimiters = Creator.MessageDelimiters('#', '@', '*', '!', '%');
    }

    private IBatch GetTestBatchFromStringConstructor()
    {
      string StringRaw = "BHS|^~\\&|SendingApp|SendingFacility|ReceivingApp|ReceivingFacility|20141208064531|Security|Name/ID|Comment|Control ID|Reference File Control ID\r" +
                         "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1\r" +
                         "PID|1||123456|||Smith^John\r" +
                         "PV1|1|I\r" +
                         "ORC|1|12345|54321|Group1\r" +
                         "OBR|1|12345|54321|test^Testing^L\r" +
                         "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1\r" +
                         "PID|1||123456|||Smith^John\r" +
                         "PV1|1|I\r" +
                         "ORC|1|12345|54321|Group1\r" +
                         "OBR|1|12345|54321|test^Testing^L\r" +
                         "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1\r" +
                         "PID|1||123456|||Smith^John\r" +
                         "PV1|1|I\r" +
                         "ORC|1|12345|54321|Group1\r" +
                         "OBR|1|12345|54321|test^Testing^L\r" +
                         "BTS|BatchMessageCount|BatchComment|BatchTotals";
      
      return Creator.Batch(StringRaw);
    }
    
    private IBatch GetTestBatchFromObjectConstructor()
    {
      ISegment BHS = Creator.Segment("BHS|^~\\&|SendingApp|SendingFacility|ReceivingApp|ReceivingFacility|20141208064531|Security|Name/ID|Comment|Control ID|Reference File Control ID");
      ISegment BTS = Creator.Segment("BTS|BatchMessageCount|BatchComment|BatchTotals");

      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1\r" +
                         "PID|1||123456|||Smith^John\r" +
                         "PV1|1|I\r" +
                         "ORC|1|12345|54321|Group1\r" +
                         "OBR|1|12345|54321|test^Testing^L\r";

      List<IMessage> MessageList = new List<IMessage>()
      {
        Creator.Message(StringRaw),
        Creator.Message(StringRaw),
        Creator.Message(StringRaw)
      };
      
     return Creator.Batch(BHS, MessageList, BTS );
    }
    
    [TestMethod]
    public void BatchConstructorAsStringTest()
    {
      var target = GetTestBatchFromStringConstructor();
      Assert.AreEqual("BHS|^~\\&|SendingApp|SendingFacility|ReceivingApp|ReceivingFacility|20141208064531|Security|Name/ID|Comment|Control ID|Reference File Control ID", target.BatchHeader.AsStringRaw, "A test for Batch Constructor");
      Assert.AreEqual("BTS|BatchMessageCount|BatchComment|BatchTotals", target.BatchTrailer.AsStringRaw, "A test for Batch Constructor");
      Assert.AreEqual(3, target.MessageCount(), "A test for Batch Message count");
    }
    
    [TestMethod]
    public void BatchConstructorAsObjectsTest()
    {
      var target = GetTestBatchFromObjectConstructor();
      Assert.AreEqual("BHS|^~\\&|SendingApp|SendingFacility|ReceivingApp|ReceivingFacility|20141208064531|Security|Name/ID|Comment|Control ID|Reference File Control ID", target.BatchHeader.AsStringRaw, "A test for Batch Constructor");
      Assert.AreEqual("BTS|BatchMessageCount|BatchComment|BatchTotals", target.BatchTrailer.AsStringRaw, "A test for Batch Constructor");
      Assert.AreEqual(3, target.MessageCount(), "A test for Batch Message count");
    }
    
    [TestMethod]
    public void CloneTest()
    {
      var expected = GetTestBatchFromObjectConstructor();
      var target = expected.Clone();
      
      Assert.AreEqual(expected.AsStringRaw, target.AsStringRaw, "A test for Batch Clone");
    }
    
    [TestMethod]
    public void AddMessageToBatchTest()
    {
      var target = GetTestBatchFromObjectConstructor();
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1\r" +
                         "PID|1||123456|||Smith^John\r" +
                         "PV1|1|I\r" +
                         "ORC|1|12345|54321|Group1\r" +
                         "OBR|1|12345|54321|test^Testing^L\r";

      IMessage NewMessage = Creator.Message(StringRaw);
      target.AddMessage(NewMessage);
      
      Assert.AreEqual(4, target.MessageCount(), "A test AddMessage to Batch");
    }
    
    [TestMethod]
    public void RemoveMessageFromBatchTest()
    {
      var target = GetTestBatchFromObjectConstructor();
      
      target.RemoveMessageAt(2);
      
      Assert.AreEqual(2, target.MessageCount(), "A test RemoveMessageAt on Batch");
    }
    
    [TestMethod]
    public void ClearAllBatchTest()
    {
      var target = GetTestBatchFromObjectConstructor();
      
      target.ClearAll();
      
      Assert.AreEqual(0, target.MessageCount(), "A test ClearAll on Batch");
      Assert.IsNull(target.BatchTrailer, "A test ClearAll on Batch");
      Assert.AreEqual(2,target.BatchHeader.ElementCount, "A test ClearAll on Batch");
    }
  }
}

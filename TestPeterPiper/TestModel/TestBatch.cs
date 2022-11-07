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
    
    private const string BhsSegmentString = "BHS|^~\\&|SendingApp|SendingFacility|ReceivingApp|ReceivingFacility|20141208064531|Security|Name/ID|Comment|Control ID|Reference File Control ID";
    private const string BtsSegmentString = "BTS|BatchMessageCount|BatchComment|BatchTotals";

    private const string MessageString = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1\r" +
                                         "PID|1||123456|||Smith^John\r" +
                                         "PV1|1|I\r" +
                                         "ORC|1|12345|54321|Group1\r" +
                                         "OBR|1|12345|54321|test^Testing^L";

    [TestInitialize]
    public void MyTestInitialize()
    {
      CustomDelimiters = Creator.MessageDelimiters('#', '@', '*', '!', '%');
    }

    private IBatch GetTestBatchFromStringConstructor()
    {
      string batchStringRaw = TestBatch.BhsSegmentString + "\r" +
                              TestBatch.MessageString + "\r" + 
                              TestBatch.MessageString + "\r" + 
                              TestBatch.MessageString + "\r" + 
                              TestBatch.BtsSegmentString;
      
      return Creator.Batch(batchStringRaw);
    }
    
    private IBatch GetTestBatchFromObjectConstructorThree()
    {
      ISegment bhs = Creator.Segment(TestBatch.BhsSegmentString);
      
      List<IMessage> messageList = new List<IMessage>()
                                   {
                                     Creator.Message(MessageString),
                                     Creator.Message(MessageString),
                                     Creator.Message(MessageString)
                                   };
      
      ISegment bts = Creator.Segment(TestBatch.BtsSegmentString);
      
     return Creator.Batch(bhs, messageList, bts );
    }
    
    private IBatch GetTestBatchFromObjectConstructorTwo()
    {
      ISegment bhs = Creator.Segment(TestBatch.BhsSegmentString);
      
      List<IMessage> messageList = new List<IMessage>()
                                   {
                                     Creator.Message(MessageString),
                                     Creator.Message(MessageString),
                                     Creator.Message(MessageString)
                                   };
      
      return Creator.Batch(bhs, messageList);
    }
    
    private IBatch GetTestBatchFromObjectConstructorOne()
    {
      ISegment bhs = Creator.Segment(TestBatch.BhsSegmentString);
      
      return Creator.Batch(bhs);
    }
    
    private IBatch GetTestBatchFromObjectConstructorZero()
    { 
      return Creator.Batch();
    }
    
    [TestMethod]
    public void BatchConstructorAsStringTest()
    {
      var target = GetTestBatchFromStringConstructor();
      Assert.AreEqual(TestBatch.BhsSegmentString, target.BatchHeader.AsStringRaw, "A test for Batch Constructor");
      Assert.AreEqual(3, target.MessageCount(), "A test for Batch Message count");
      Assert.AreEqual(TestBatch.BtsSegmentString, target.BatchTrailer.AsStringRaw, "A test for Batch Constructor");
    }
    
    [TestMethod]
    public void BatchConstructorAsObjectsThreeTest()
    {
      var target = GetTestBatchFromObjectConstructorThree();
      Assert.AreEqual(TestBatch.BhsSegmentString, target.BatchHeader.AsStringRaw, "A test for Batch Constructor three");
      Assert.AreEqual(3, target.MessageCount(), "A test for Batch Constructor three");
      Assert.AreEqual(TestBatch.BtsSegmentString, target.BatchTrailer.AsStringRaw, "A test for Batch Constructor three");
    }
    
    [TestMethod]
    public void BatchConstructorAsObjectsTwoTest()
    {
      var target = GetTestBatchFromObjectConstructorTwo();
      Assert.AreEqual(TestBatch.BhsSegmentString, target.BatchHeader.AsStringRaw, "A test for Batch Constructor two");
      Assert.AreEqual(3, target.MessageCount(), "A test for Batch Constructor two");
      Assert.IsNull(target.BatchTrailer, "A test for Batch Constructor two");
    }
    [TestMethod]
    public void BatchConstructorAsObjectsOneTest()
    {
      var target = GetTestBatchFromObjectConstructorOne();
      Assert.AreEqual(TestBatch.BhsSegmentString, target.BatchHeader.AsStringRaw, "A test for Batch Constructor One");
      Assert.AreEqual(0, target.MessageCount(), "A test for Batch Constructor One");
      Assert.IsNull(target.BatchTrailer, "A test for Batch Constructor One");
    }
    [TestMethod]
    public void BatchConstructorAsObjectsZeroTest()
    {
      var target = GetTestBatchFromObjectConstructorZero();
      Assert.AreEqual("BHS|^~\\&|", target.BatchHeader.AsStringRaw, "A test for Batch Constructor Zero");
      Assert.AreEqual(0, target.MessageCount(), "A test for Batch Constructor Zero");
      Assert.IsNull(target.BatchTrailer, "A test for Batch Constructor Zero");
    }
    
    [TestMethod]
    public void BatchConstructorAsStringCustomDiametersTest()
    {
     //"BSH#*@%!#####20141208064531##ORM*O01*ORM_O01#Q54356818T82744882#P#2.3.1"; 
     
     string BhsStringCustom = BhsSegmentString.Replace('|', '#')
                                              .Replace('^', '*')
                                              .Replace('~', '@')
                                              .Replace('\\', '%');
     
     string MessageStringCustom = MessageString.Replace('|', '#')
                                               .Replace('^', '*')
                                               .Replace('~', '@')
                                               .Replace('\\', '%');
     
     string BtsStringCustom = BtsSegmentString.Replace('|', '#')
                                              .Replace('^', '*')
                                              .Replace('~', '@')
                                              .Replace('\\', '%');

     string batchStringRaw = BhsStringCustom + "\r" +
                             MessageStringCustom + "\r" + 
                             MessageStringCustom + "\r" + 
                             MessageStringCustom + "\r" + 
                             BtsStringCustom;
      
      IBatch target = Creator.Batch(batchStringRaw);
      
      Assert.AreEqual(BhsStringCustom, target.BatchHeader.AsStringRaw, "A test for Batch Constructor");
      Assert.AreEqual(3, target.MessageCount(), "A test for Batch Message count");
      Assert.AreEqual(BtsStringCustom, target.BatchTrailer.AsStringRaw, "A test for Batch Constructor");
      Assert.AreEqual("*@%&", target.EscapeSequence, "A test RemoveMessageAt on Batch");
      Assert.AreEqual("#", target.MainSeparator, "A test RemoveMessageAt on Batch");
    }
    
    
    [TestMethod]
    public void CloneTest()
    {
      var expected = GetTestBatchFromObjectConstructorThree();
      var target = expected.Clone();
      
      Assert.AreEqual(expected.AsStringRaw, target.AsStringRaw, "A test for Batch Clone");
    }
    
    [TestMethod]
    public void AddMessageToBatchTest()
    {
      var target = GetTestBatchFromObjectConstructorThree();
      IMessage NewMessage = Creator.Message(MessageString);
      target.AddMessage(NewMessage);
      
      Assert.AreEqual(4, target.MessageCount(), "A test AddMessage to Batch");
    }
    
    [TestMethod]
    public void InsertMessageAtBatchTest()
    {
      var target = GetTestBatchFromObjectConstructorThree();
      
      IMessage NewMessage = Creator.Message(TestBatch.MessageString);
      string MessageInsertSegmentCode = "MSH";
      string MessageInsertMarker = "InsertedAtIndexTwo";
      NewMessage.Segment(MessageInsertSegmentCode).Field(3).AsString = MessageInsertMarker;
      target.InsertMessage(2,NewMessage);

      Assert.AreEqual(4, target.MessageCount(), "A test InsertMessage to Batch");
      Assert.AreEqual(MessageInsertMarker, target.MessageList().ElementAt(2).Segment(MessageInsertSegmentCode).Field(3).AsString, "A test InsertMessage to Batch");
    }
    
    [TestMethod]
    public void RemoveMessageFromBatchTest()
    {
      var target = GetTestBatchFromObjectConstructorThree();
      
      target.RemoveMessageAt(2);
      
      Assert.AreEqual(2, target.MessageCount(), "A test RemoveMessageAt on Batch");
    }
    
    [TestMethod]
    public void ClearAllBatchTest()
    {
      var target = GetTestBatchFromObjectConstructorThree();
      
      target.ClearAll();
      
      Assert.AreEqual(0, target.MessageCount(), "A test ClearAll on Batch");
      Assert.IsNull(target.BatchTrailer, "A test ClearAll on Batch");
      Assert.AreEqual(2,target.BatchHeader.ElementCount, "A test ClearAll on Batch");
    }
    
    [TestMethod]
    public void RemoveMessageFromBatchTestx()
    {
      var target = GetTestBatchFromObjectConstructorThree();
      Assert.AreEqual("^~\\&", target.EscapeSequence, "A test RemoveMessageAt on Batch");
    }
  }
}

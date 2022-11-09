using System.Collections.Generic;
using System.Linq;
using PeterPiper.Hl7.V2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TestHl7V2
{
  [TestClass]
  public class TestFile
  {
    private IMessageDelimiters _CustomDelimiters;
    private const string FhsSegmentString = "FHS|^~\\&|SendingApp|SendingFacility|ReceivingApp|ReceivingFacility|20141208064531|Security|Name/ID|Comment|Control ID|Reference File Control ID";
    private const string FtsSegmentString = "FTS|FileBatchCount|BatchComment";
    
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
      _CustomDelimiters = Creator.MessageDelimiters('#', '@', '*', '!', '%');
    }

    private IFile GetTestFileFromStringConstructor()
    {
      string StringRaw = TestFile.FhsSegmentString + "\r" +
                         TestFile.BhsSegmentString + "\r" +
                         TestFile.MessageString +"\r" +
                         TestFile.MessageString +"\r" +
                         TestFile.MessageString +"\r" +
                         TestFile.BtsSegmentString + "\r" +
                         TestFile.FtsSegmentString;

      return Creator.File(StringRaw);
    }

    private IFile GetTestFileFromObjectConstructorThree()
    {
      ISegment FHS = Creator.Segment(TestFile.FhsSegmentString);
      ISegment BHS = Creator.Segment(TestFile.BhsSegmentString);
      ISegment BTS = Creator.Segment(TestFile.BtsSegmentString);
      ISegment FTS = Creator.Segment(TestFile.FtsSegmentString);

      List<IMessage> MessageList = new List<IMessage>() {
                                                          Creator.Message(TestFile.MessageString),
                                                          Creator.Message(TestFile.MessageString),
                                                          Creator.Message(TestFile.MessageString)
                                                        };

      List<IBatch> BatchList = new List<IBatch>() {
                                                    Creator.Batch(BHS, MessageList, BTS),
                                                    Creator.Batch(BHS, MessageList, BTS),
                                                    Creator.Batch(BHS, MessageList, BTS)
                                                  };


      return Creator.File(FHS, BatchList, FTS);
    }
    
    private IFile GetTestFileFromObjectConstructorTwo()
    {
      ISegment FHS = Creator.Segment(TestFile.FhsSegmentString);
      ISegment BHS = Creator.Segment(TestFile.BhsSegmentString);
      ISegment BTS = Creator.Segment(TestFile.BtsSegmentString);
     
      List<IMessage> MessageList = new List<IMessage>() {
                                                          Creator.Message(TestFile.MessageString),
                                                          Creator.Message(TestFile.MessageString),
                                                          Creator.Message(TestFile.MessageString)
                                                        };

      List<IBatch> BatchList = new List<IBatch>() {
                                                    Creator.Batch(BHS, MessageList, BTS),
                                                    Creator.Batch(BHS, MessageList, BTS),
                                                    Creator.Batch(BHS, MessageList, BTS)
                                                  };


      return Creator.File(FHS, BatchList);
    }
    
    private IFile GetTestFileFromObjectConstructorOne()
    {
      ISegment fhs = Creator.Segment(TestFile.FhsSegmentString);
      return Creator.File(fhs);
    }
    
    private IFile GetTestFileFromObjectConstructorZero()
    {
      return Creator.File();
    }

    [TestMethod]
    public void FileConstructorAsStringCustomDiametersTest()
    {
      //"FSH#*@%!#####20141208064531##ORM*O01*ORM_O01#Q54356818T82744882#P#2.3.1"; 
     
      string FhsStringCustom = FhsSegmentString.Replace('|', '#')
                                               .Replace('^', '*')
                                               .Replace('~', '@')
                                               .Replace('\\', '%');

      
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
      
      string FtsStringCustom = FtsSegmentString.Replace('|', '#')
                                               .Replace('^', '*')
                                               .Replace('~', '@')
                                               .Replace('\\', '%');

      string fileStringRaw =
        FhsStringCustom + "\r" +
        BhsStringCustom + "\r" +
        MessageStringCustom + "\r" +
        MessageStringCustom + "\r" +
        MessageStringCustom + "\r" +
        BtsStringCustom + "\r" +
        FtsStringCustom;
      
      IFile target = Creator.File(fileStringRaw);
      
      Assert.AreEqual(FhsStringCustom, target.FileHeader.AsStringRaw, "A test for Batch Constructor");
      Assert.AreEqual(1, target.BatchCount(), "A test for Batch Message count");
      Assert.AreEqual(FtsStringCustom, target.FileTrailer.AsStringRaw, "A test for Batch Constructor");
      Assert.AreEqual("*@%&", target.EscapeSequence, "A test RemoveMessageAt on Batch");
      Assert.AreEqual("#", target.MainSeparator, "A test RemoveMessageAt on Batch");
    }
    
    [TestMethod]
    public void FileConstructorAsStringTest()
    {
      var target = GetTestFileFromStringConstructor();
      Assert.AreEqual(TestFile.FhsSegmentString, target.FileHeader.AsStringRaw, "A test for File Constructor");
      Assert.AreEqual(1, target.BatchCount(), "A test for File Message count");
      Assert.AreEqual(TestFile.FtsSegmentString, target.FileTrailer.AsStringRaw, "A test for File Constructor");
    }

    [TestMethod]
    public void FileConstructorAsObjectsThree()
    {
      var target = GetTestFileFromObjectConstructorThree();
      Assert.AreEqual(TestFile.FhsSegmentString, target.FileHeader.AsStringRaw, "A test for File Constructor");
      Assert.AreEqual(3, target.BatchCount(), "A test for File Message count");
      Assert.AreEqual(TestFile.FtsSegmentString, target.FileTrailer.AsStringRaw, "A test for File Constructor");
    }
    
    [TestMethod]
    public void FileConstructorAsObjectsTwo()
    {
      var target = GetTestFileFromObjectConstructorTwo();
      Assert.AreEqual(TestFile.FhsSegmentString, target.FileHeader.AsStringRaw, "A test for File Constructor");
      Assert.AreEqual(3, target.BatchCount(), "A test for File Message count");
      Assert.IsNull(target.FileTrailer, "A test for File Constructor");
    }
    
    [TestMethod]
    public void FileConstructorAsObjectsOne()
    {
      var target = GetTestFileFromObjectConstructorOne();
      Assert.AreEqual(TestFile.FhsSegmentString, target.FileHeader.AsStringRaw, "A test for File Constructor");
      Assert.IsNull(target.FileTrailer, "A test for File Constructor");
      Assert.AreEqual(0, target.BatchCount(), "A test for File Message count");
    }
    
    [TestMethod]
    public void FileConstructorAsObjectsZero()
    {
      var target = GetTestFileFromObjectConstructorZero();
      Assert.AreEqual("FHS|^~\\&", target.FileHeader.AsStringRaw, "A test for File Constructor Zero");
      Assert.AreEqual(0, target.BatchCount(), "A test for File Constructor Zero");
      Assert.IsNull(target.FileTrailer, "A test for File Constructor Zero");
    }

    [TestMethod]
    public void CloneTest()
    {
      var expected = GetTestFileFromObjectConstructorThree();
      var target = expected.Clone();

      Assert.AreEqual(expected.AsStringRaw, target.AsStringRaw, "A test for File Clone");
    }

    [TestMethod]
    public void AddBatchToFileTest()
    {
      var target = GetTestFileFromObjectConstructorThree();
      string batchStringRaw = TestFile.BhsSegmentString + "\r" +
                              TestFile.MessageString + "\r" +
                              TestFile.MessageString + "\r" +
                              TestFile.MessageString + "\r" +
                              TestFile.BtsSegmentString;
                         
      IBatch NewBatch = Creator.Batch(batchStringRaw);
      target.AddBatch(NewBatch);

      Assert.AreEqual(4, target.BatchCount(), "A test AddBatch to File");
    }

    [TestMethod]
    public void InsertBatchAtFileTest()
    {
      var target = GetTestFileFromObjectConstructorThree();
      string batchStringRaw = TestFile.BhsSegmentString + "\r" +
                              TestFile.MessageString + "\r" +
                              TestFile.MessageString + "\r" +
                              TestFile.MessageString + "\r" +
                              TestFile.BtsSegmentString;
                         
      IBatch NewBatch = Creator.Batch(batchStringRaw);
      string BatchInsertMarker = "InsertedAtIndexTwo";
      NewBatch.BatchHeader.Field(3).AsString = BatchInsertMarker;
      target.InsertBatch(2,NewBatch);

      Assert.AreEqual(4, target.BatchCount(), "A test AddBatch to File");
      Assert.AreEqual(BatchInsertMarker, target.BatchList().ElementAt(2).BatchHeader.Field(3).AsString, "A test AddBatch to File");
    }
    
    [TestMethod]
    public void RemoveBatchFromFileTest()
    {
      var target = GetTestFileFromObjectConstructorThree();

      target.RemoveBatchAt(2);

      Assert.AreEqual(2, target.BatchCount(), "A test RemoveBatchAt on File");
    }

    [TestMethod]
    public void ClearAllFileTest()
    {
      var target = GetTestFileFromObjectConstructorThree();

      target.ClearAll();

      Assert.AreEqual(0, target.BatchCount(), "A test ClearAll on File");
      Assert.IsNull(target.FileTrailer, "A test ClearAll on File");
      Assert.AreEqual(2, target.FileHeader.ElementCount, "A test ClearAll on File");
    }
    
    [TestMethod]
    public void RemoveMessageFromBatchTest()
    {
      var target = GetTestFileFromObjectConstructorThree();
      Assert.AreEqual("^~\\&", target.EscapeSequence, "A test RemoveMessageAt on Batch");
    }

    [TestMethod]
    public void TestFshWithNoElements()
    {
      //Prepare
      string StringRaw = "FHS|^~\\&" + "\r" +
                         TestFile.BhsSegmentString + "\r" +
                         TestFile.MessageString +"\r" +
                         TestFile.MessageString +"\r" +
                         TestFile.MessageString +"\r" +
                         TestFile.BtsSegmentString + "\r" +
                         TestFile.FtsSegmentString;

      IFile file = Creator.File(StringRaw);
    }
    
    [TestMethod]
    public void TestFshWithNoElements2()
    {
      //Prepare
      var oMsg = new System.Text.StringBuilder();
      oMsg.Append("FHS|^~\\&\r");
      oMsg.Append("BHS|^~\\&\r");
      
      oMsg.Append("MSH|^~\\&|EnsembleHL7|ISC|Mock|GTU00000|202211081829||ACK^O01|bfd263629619489bbf685d08cb77500e|P|2.4\r");
      oMsg.Append("MSA|AA|bfd263629619489bbf685d08cb77500e\r");
      
      oMsg.Append("MSH|^~\\&|EnsembleHL7|ISC|Mock|GTU00000|202211081829||ACK^O01|25d315c94cb9423b9d406c7cda4782f4|P|2.4\r");
      oMsg.Append("MSA|AE|25d315c94cb9423b9d406c7cda4782f4|This is my test error message\r");

      oMsg.Append("MSH|^~\\&|EnsembleHL7|ISC|Mock|GTU00000|202211081829||ACK^O01|90e438772728424080481336d8771053|P|2.4\r");
      oMsg.Append("MSA|AA|90e438772728424080481336d8771053\r");
      
      oMsg.Append("BTS|3\r");
      oMsg.Append("FTS|1\r");

      IFile hl7FileAckMessage = Creator.File(oMsg.ToString());
      string x = hl7FileAckMessage.AsStringRaw;
      
      hl7FileAckMessage = Creator.File(x);
    }
    
    [TestMethod]
    public void TestFshSegmentRoundTrip()
    {
      string fshMinimalSegmentString = "FHS|^~\\&";
      ISegment fshSegment = Creator.Segment(fshMinimalSegmentString);
      string fshSegmentStringTarget = fshSegment.AsStringRaw;
      Assert.AreEqual(fshMinimalSegmentString, fshSegmentStringTarget);

    }
    
  }
}

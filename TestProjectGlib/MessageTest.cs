using Glib.Hl7.V2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Glib.Hl7.V2.Support;

namespace TestProjectGlib
{
    
    
    /// <summary>
    ///This is a test class for MessageTest and is intended
    ///to contain all MessageTest Unit Tests
    ///</summary>
  [TestClass()]
  public class MessageTest
  {


    private TestContext testContextInstance;
    public System.Text.StringBuilder oMsg;
    public string sMSH;
    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    [TestInitialize()]
    public void MyTestInitialize()
    {
      sMSH = "MSH|^~\\&|AUSLAB|TRAIN|EGATE-Atomic^prjAuslabIn|ieMR|20140527095657||ORU^R01|0000000000000000010D|P|2.3.1|||||||en";
      
      oMsg = new System.Text.StringBuilder();
      oMsg.Append("MSH|^~\\&|AUSLAB|TRAIN|EGATE-Atomic^prjAuslabIn|ieMR|20140527095657||ORU^R01^ORU_R01|0000000000000000010D|P|2.3.1|||||||en"); oMsg.Append("\r");
      oMsg.Append("PID|1|PA30000004|PA3000\\R\\0004^^^^MR~22222221111^^^AUSHIC^MC||QHMILLAR^AM201405191155||19730930|M||9^Not Stated|16 ULVA STREET^^BALD HILLS (4036)^^4036||0893412041||||||22222221111"); oMsg.Append("\r");
      oMsg.Append("PV1|1|I|4BT\\R\\PAH&4B Transplant (PAH)&AUSLAB^^^PAH&Princess Alexandra Hospital&AUSLAB||||BONR1\\R\\PAH^Robert Bond^^^^^^^AUSLAB^^^^DN^PAH&Princess Alexandra Hospital~460?PAH^^^^^^^^AUSLAB^^^^DN^PAH&Princess Alexandra Hospital||BONR\\R\\PAH^Bond, Robert (PAH)^^^^^^^AUSLAB^^^^DN^PAH&Princess Alexandra Hospital~460?PAH^^^^^^^^AUSLAB^^^^DN^PAH&Princess Alexandra Hospital~0362233F^^^^^^^^AUSHIC^^^^PRN|MED1\\R\\PAH^Medical 1 (PAH)|||||||||PA2711|G P Eligible|GPE|||||||||||||||||||||||201405191211"); oMsg.Append("\r");
      oMsg.Append("ORC|RE|141470000018|1202^AUSLAB||IP||^^^20140527|||||BONR1\\R\\PAH^Robert Bond^^^^^^^AUSLAB^^^^DN^PAH&Princess Alexandra Hospital~460?PAH^^^^^^^^AUSLAB^^^^DN^PAH&Princess Alexandra Hospital"); oMsg.Append("\r");
      oMsg.Append("OBR|1|141470000018|1202^AUSLAB|URINE^Urine M/C/S^AUSLAB||20140527|201405270900|||4BT\\R\\PAH^4B Transplant (PAH)^^^^^^^AUSLAB^^^^CWARD^PAH&Princess Alexandra Hospital~PAH^Princess Alexandra Hospital^^^^^^^AUSLAB^^^^CCENT^PAH&Princess Alexandra Hospital||||201405270901|URINE&Urine&AUSLAB|BONR1\\R\\PAH^Robert Bond^^^^^^^AUSLAB^^^^DN^PAH&Princess Alexandra Hospital~460?PAH^^^^^^^^AUSLAB^^^^DN^PAH&Princess Alexandra Hospital||||||201405270956||MB|F||^^^20140527"); oMsg.Append("\r");
      oMsg.Append("OBX|1|CE|MSTAT^Micro Report Status^AUSLAB||COM^COMPLETE^AUSLAB|||H|||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|2|CE|UCENT^Urine Centrifuged^AUSLAB||CENT^Centrifuged^AUSLAB||||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|3|CE|UCRY3^Urine Crystals 3^AUSLAB||UNID^Unidentified ^AUSLAB||||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|4|CE|UCRY2^Urine Crystals 2^AUSLAB||UNID^Unidentified ^AUSLAB||||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|5|CE|UCRY1^Urine Crystals 1^AUSLAB||NCRYS^No Crystals Seen^AUSLAB||||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|6|NM|UWBCC^Urine WBC Casts^AUSLAB||0|/lpf|||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|7|NM|UHYC^Urine Hyaline Casts^AUSLAB||0|/lpf|||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|8|NM|UGRANC^Urine Granular Casts^AUSLAB||0|/lpf|||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|9|NM|URBCC^Urine RBC Casts^AUSLAB||0|/lpf|||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|10|CE|UEPI^Urine Micro Epi's^AUSLAB||G50^> 50^AUSLAB|x10\\S\\6/L|||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|11|NM|URBC^Urine Micro RBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|13|CE|CULT1^Culture Comment Line 1^AUSLAB||MSF^Mixed skin flora^AUSLAB||||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|14|TX|MICCOM^Comment ^AUSLAB||this is a ieMR test comment 1~this is a ieMR test commment 2||||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|15|CE|UAMA^Ur Antimicrobial Activity^AUSLAB||ND^Not Detected^AUSLAB||||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|16|CE|ORG^Organism^AUSLAB|1|ESCCES^E. coli (ESBL Producer)^AUSLAB||||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBX|17|ST|QUANT^Quantifier^AUSLAB|1|2+||||||F|||201405270956|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");
      oMsg.Append("OBR|2|141470000018|1202^AUSLAB|SENS^Sensitivity Panel^AUSLAB||20140527|201405270900|||4BT\\R\\PAH^4B Transplant (PAH)^^^^^^^AUSLAB^^^^CWARD^PAH&Princess Alexandra Hospital~PAH^Princess Alexandra Hospital^^^^^^^AUSLAB^^^^CCENT^PAH&Princess Alexandra Hospital||||201405270901|URINE&Urine&AUSLAB|BONR1\\R\\PAH^Robert Bond^^^^^^^AUSLAB^^^^DN^PAH&Princess Alexandra Hospital~460?PAH^^^^^^^^AUSLAB^^^^DN^PAH&Princess Alexandra Hospital||||||201405270954||MB|P||^^^20140527"); oMsg.Append("\r");
      oMsg.Append("OBX|1|TX|CULCOM^Culture Comment^AUSLAB||Sens ieMR test comment 1~Sens ieMR comment line 2||||||R|||201405270954|RB^PATH QLD Central^AUSLAB"); oMsg.Append("\r");






    }
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for Message Constructor
    ///</summary>
    [TestMethod()]
    public void MessageConstructorTest()
    {
      
      string StringRaw = oMsg.ToString();
      bool ParseMSHSegmentOnly = false; 
      Message target = new Message(StringRaw, ParseMSHSegmentOnly);
      Assert.AreEqual(StringRaw, target.AsStringRaw, "A test for Message Constructor");

      ParseMSHSegmentOnly = true; 
      target = new Message(StringRaw, ParseMSHSegmentOnly);
      Assert.AreEqual(oMsg.ToString().Split('\r')[0] + '\r', target.AsStringRaw, "A test for Message Constructor");
      Assert.AreEqual("0000000000000000010D", target.MessageControlID, "A test for Message Constructor");
      Assert.AreEqual("ORU", target.MessageType, "A test for Message Constructor");
      Assert.AreEqual("R01", target.MessageTrigger, "A test for Message Constructor");
      Assert.AreEqual("ORU_R01", target.MessageStructure, "A test for Message Constructor");
      Assert.AreEqual("2.3.1", target.MessageVersion, "A test for Message Constructor");    
      Assert.AreEqual(new DateTime(2014,05,27,9,56,57), target.MessageCreationDateTime, "A test for Message Constructor");
      Assert.AreEqual(1, target.SegmentCount(), "A test for Message Constructor");
      Assert.AreEqual(true, target.IsParseMSHSegmentOnly, "A test for Message Constructor");    
    }

    /// <summary>
    ///A test for Message Constructor
    ///</summary>
    [TestMethod()]
    public void MessageConstructorTest1()
    {
      string[] MsgList = oMsg.ToString().Split('\r');
      List<string> collection = new List<string>();      
      foreach (var Line in MsgList)
      {
        collection.Add(Line);
      }      
      bool ParseMSHSegmentOnly = false; 
      Message target = new Message(collection, ParseMSHSegmentOnly);
      Assert.AreEqual(oMsg.ToString(), target.AsStringRaw, "A test for Message Constructor 1");
      Assert.AreEqual(24, target.SegmentCount(), "A test for Message Constructor 1");
      Assert.AreEqual(false, target.IsParseMSHSegmentOnly, "A test for Message Constructor 1");
    }

    /// <summary>
    ///A test for Message Constructor
    ///</summary>
    [TestMethod()]
    public void MessageConstructorTest2()
    {
      string MessageVersion = "2.7";
      string MessageType = "ORU";
      string MessageTrigger = "R01";
      string MessageControlID = "1234567890";
      string MessageStructure = "ORU_R01";
      Message target = new Message(MessageVersion, MessageType, MessageTrigger, MessageControlID, MessageStructure);
      target.Segment(1).Field(7).ClearAll();
      Assert.AreEqual("MSH|^~\\&|||||||ORU^R01^ORU_R01|1234567890|P|2.7|||AL|NE\r", target.AsStringRaw, "A test for Message Constructor 1");
    }

    /// <summary>
    ///A test for Message Constructor
    ///</summary>
    [TestMethod()]
    public void MessageConstructorTest3()
    {
      Segment item = new Segment("MSH|^~\\&|AUSLAB|TRAIN|EGATE-Atomic^prjAuslabIn|ieMR|20140527095657||ORU^R01^ORU_R01|0000000000000000010D|P|2.3.1|||||||en");
      Message target = new Message(item);
      Assert.AreEqual(oMsg.ToString().Split('\r')[0] + '\r', target.AsStringRaw, "A test for Message Constructor");
      Assert.AreEqual("0000000000000000010D", target.MessageControlID, "A test for Message Constructor");
      Assert.AreEqual("ORU", target.MessageType, "A test for Message Constructor");
      Assert.AreEqual("R01", target.MessageTrigger, "A test for Message Constructor");
      Assert.AreEqual("ORU_R01", target.MessageStructure, "A test for Message Constructor");
      Assert.AreEqual("2.3.1", target.MessageVersion, "A test for Message Constructor");
      Assert.AreEqual(new DateTime(2014, 05, 27, 9, 56, 57), target.MessageCreationDateTime, "A test for Message Constructor");
      Assert.AreEqual(1, target.SegmentCount(), "A test for Message Constructor");
      Assert.AreEqual(false, target.IsParseMSHSegmentOnly, "A test for Message Constructor");    
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [TestMethod()]
    public void AddTest1()
    {
      Segment item = new Segment("MSH|^~\\&|AUSLAB|TRAIN|EGATE-Atomic^prjAuslabIn|ieMR|20140527095657||ORU^R01^ORU_R01|0000000000000000010D|P|2.3.1|||||||en");
      Message target = new Message(item);
      Segment item1 = new Segment("MSH|^~\\&|AUSLAB|TRAIN|EGATE-Atomic^prjAuslabIn|ieMR|20140527095657||ORU^R01^ORU_R01|0000000000000000010D|P|2.3.1|||||||en");
      try
      {
        target.Add(item1);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual("An MSH Segment can not be added to a Message instance, it must be provided on Message instance creation / instantiation", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      item1 = new Segment("OBX|1|TX|CULCOM^Culture Comment^AUSLAB||Sens ieMR test comment 1~Sens ieMR comment line 2||||||R|||201405270954|RB^PATH QLD Central^AUSLAB");
      target.Add(item1);
      Assert.AreEqual("Culture Comment", target.Segment(2).Field(3).Component(2).AsString, "A test for Add");
      Assert.AreEqual(2, target.SegmentCount(), "A test for Add");
    }

    /// <summary>
    ///A test for ClearAll
    ///</summary>
    [TestMethod()]
    public void ClearAllTest()
    {
      Message target;
      Segment itemOBX = new Segment("OBX|1|TX|CULCOM^Culture Comment^AUSLAB||Sens ieMR test comment 1~Sens ieMR comment line 2||||||R|||201405270954|RB^PATH QLD Central^AUSLAB");
      try
      {
        target = new Message(itemOBX); 
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual("The Segment instance passed in is not a MSH Segment, only a MSH Segment can be passed in on creation / instantiation of a Message", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }
      Segment itemMSH = new Segment("MSH|^~\\&|AUSLAB|TRAIN|EGATE-Atomic^prjAuslabIn|ieMR|20140527095657||ORU^R01^ORU_R01|0000000000000000010D|P|2.3.1|||||||en");
      target = new Message(itemMSH); 
      target.ClearAll();
      Assert.AreEqual(2, target.Segment(1).ElementCount, "A test for ClearAll");
      Assert.AreEqual(2, target.Segment(1).FieldCount, "A test for ClearAll");
      Assert.AreEqual("MSH|^~\\&|", target.Segment(1).AsStringRaw, "A test for ClearAll");
      Assert.AreEqual("MSH|^~\\&|", target.Segment(1).AsString, "A test for ClearAll");

      target.Add(itemOBX);
      Assert.AreEqual(2, target.SegmentCount(), "A test for ClearAll");
      Assert.AreEqual("MSH", target.Segment(1).Code, "A test for ClearAll");
      Assert.AreEqual("OBX", target.Segment(2).Code, "A test for ClearAll");
      Assert.AreEqual(2, target.Segment(1).ElementCount, "A test for ClearAll");
      Assert.AreEqual(2, target.Segment(1).FieldCount, "A test for ClearAll");
      Assert.AreEqual("MSH|^~\\&|", target.Segment(1).AsStringRaw, "A test for ClearAll");
      Assert.AreEqual("MSH|^~\\&|", target.Segment(1).AsString, "A test for ClearAll");
      Assert.AreEqual("OBX|1|TX|CULCOM^Culture Comment^AUSLAB||Sens ieMR test comment 1~Sens ieMR comment line 2||||||R|||201405270954|RB^PATH QLD Central^AUSLAB", target.Segment(2).AsStringRaw, "A test for ClearAll");
      
    }

    /// <summary>
    ///A test for Clone
    ///</summary>
    [TestMethod()]
    public void CloneTest()
    {      
      Message target = new Message(oMsg.ToString()); 
      Message expected = new Message(oMsg.ToString()); 
      Message actual;
      actual = target.Clone();
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Clone");      
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [TestMethod()]
    public void InsertTest()
    {
      Message target = new Message(oMsg.ToString()); 
      int index = 2; 
      Segment itemOBX = new Segment("OBX|99|TX|CULCOM^Culture Comment^AUSLAB||Sens ieMR test comment 1~Sens ieMR comment line 2||||||R|||201405270954|RB^PATH QLD Central^AUSLAB");
      target.Insert(index, itemOBX);
      Assert.AreEqual("OBX|99|TX|CULCOM^Culture Comment^AUSLAB||Sens ieMR test comment 1~Sens ieMR comment line 2||||||R|||201405270954|RB^PATH QLD Central^AUSLAB", target.Segment(2).AsStringRaw, "A test for Insert");

      Segment itemMSH = new Segment("MSH|^~\\&|AUSLAB|TRAIN|EGATE-Atomic^prjAuslabIn|ieMR|20140527095657||ORU^R01^ORU_R01|0000000000000000010D|P|2.3.1|||||||en");
      try
      {
        target.Insert(index, itemMSH);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual("An MSH Segment can not be added to a Message instance, it must be provided on Message instance creation / instantiation", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }
    }

    /// <summary>
    ///A test for RemoveSegmentAt
    ///</summary>
    [TestMethod()]
    public void RemoveSegmentAtTest()
    {      
      Message target = new Message(oMsg.ToString());
      int SegCount = target.SegmentCount();
      int index = 10;       
      bool actual;
      actual = target.RemoveSegmentAt(index);
      Assert.AreEqual(SegCount - 1, target.SegmentCount(), "A test for RemoveSegmentAt");
      Assert.AreEqual("4", target.Segment(9).Field(1).AsString, "A test for RemoveSegmentAt");
      Assert.AreEqual("6", target.Segment(10).Field(1).AsString, "A test for RemoveSegmentAt");
    }

    /// <summary>
    ///A test for Segment
    ///</summary>
    [TestMethod()]
    public void SegmentTest()
    {
      Message target = new Message(oMsg.ToString());
      string Code = "OBX";
      Segment expected = new Segment(Code);
      Segment actual;
      actual = target.Segment(Code);
      Assert.AreEqual("OBX|1|CE|MSTAT^Micro Report Status^AUSLAB||COM^COMPLETE^AUSLAB|||H|||F|||201405270956|RB^PATH QLD Central^AUSLAB", actual.AsStringRaw, "A test for Segment");      
    }

    /// <summary>
    ///A test for Segment
    ///</summary>
    [TestMethod()]
    public void SegmentTest1()
    {
      Message target = new Message(oMsg.ToString());
      int index = 6; // TODO: Initialize to an appropriate value
      Segment expected = new Segment("OBX|1|CE|MSTAT^Micro Report Status^AUSLAB||COM^COMPLETE^AUSLAB|||H|||F|||201405270956|RB^PATH QLD Central^AUSLAB");
      Segment actual;
      actual = target.Segment(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Segment");      
    }

    /// <summary>
    ///A test for SegmentList
    ///</summary>
    [TestMethod()]
    public void SegmentListTest()
    {
      Message target = new Message(oMsg.ToString());
      string Code = "OBX";      
      ReadOnlyCollection<Segment> actual;
      actual = target.SegmentList(Code);
      Assert.AreEqual(18, actual.Count, "A test for SegmentList");
      Assert.AreEqual("OBX|5|CE|UCRY1^Urine Crystals 1^AUSLAB||NCRYS^No Crystals Seen^AUSLAB||||||F|||201405270956|RB^PATH QLD Central^AUSLAB", actual[4].AsStringRaw, "A test for SegmentList");      
    }

    /// <summary>
    ///A test for SegmentList
    ///</summary>
    [TestMethod()]
    public void SegmentListTest1()
    {
      Message target = new Message(oMsg.ToString());           
      ReadOnlyCollection<Segment> actual;
      actual = target.SegmentList();
      Assert.AreEqual(24, actual.Count, "A test for SegmentList");
      Assert.AreEqual("OBX|5|CE|UCRY1^Urine Crystals 1^AUSLAB||NCRYS^No Crystals Seen^AUSLAB||||||F|||201405270956|RB^PATH QLD Central^AUSLAB", actual[9].AsStringRaw, "A test for SegmentList 2");    
    }

    /// <summary>
    ///A test for ToString
    ///</summary>
    [TestMethod()]
    public void ToStringTest()
    {
      Message target = new Message(oMsg.ToString());
      string expected = oMsg.ToString();
      string actual;
      actual = target.ToString();
      //Hard to test need to hand craft a message with no escapes?
      //Assert.AreEqual(expected, actual, "A test for ToString");
      
      
    }

    /// <summary>
    ///A test for AsString
    ///</summary>
    [TestMethod()]
    public void AsStringTest()
    {
      //Message target = new Message(oMsg.ToString());
      //string expected = oMsg.ToString();
      //string actual;
      //target.AsString = expected;
      //actual = target.AsString;
      //Hard to test need to hand craft a message with no escapes?
      //Assert.AreEqual(expected, actual);
      
    }

    /// <summary>
    ///A test for AsStringRaw
    ///</summary>
    [TestMethod()]
    public void AsStringRawTest()
    {
      Message target = new Message(oMsg.ToString());
      string expected = oMsg.ToString();
      string actual;
      target.AsStringRaw = expected;
      actual = target.AsStringRaw;
      Assert.AreEqual(expected, actual, "A test for AsStringRaw");      
    }

    /// <summary>
    ///A test for EscapeSequence
    ///</summary>
    [TestMethod()]
    public void EscapeSequenceTest()
    {
      Message target = new Message(oMsg.ToString());      
      string actual;
      actual = target.EscapeSequence;
      Assert.AreEqual("^~\\&", actual, "A test for EscapeSequence");      
    }

    /// <summary>
    ///A test for MainSeparator
    ///</summary>
    [TestMethod()]
    public void MainSeparatorTest()
    {
      Message target = new Message(oMsg.ToString());      
      string actual;
      actual = target.MainSeparator;
      Assert.AreEqual("|", actual, "A test for MainSeparator");      
    }

    /// <summary>
    ///A test for MessageControlID
    ///</summary>
    [TestMethod()]
    public void MessageControlIDTest()
    {
      Message target = new Message(oMsg.ToString());            
      string actual;
      actual = target.MessageControlID;
      Assert.AreEqual("0000000000000000010D", actual, "A test for MessageControlID");   
    }

    /// <summary>
    ///A test for MessageDelimiters
    ///</summary>
    [TestMethod()]
    public void MessageDelimitersTest()
    {
      Message target = new Message(oMsg.ToString());
      MessageDelimiters expected = new MessageDelimiters('|', '~', '^', '&', '\\');
      MessageDelimiters actual;
      actual = target.MessageDelimiters;
      Assert.AreEqual(expected.Field, actual.Field, " test for MessageDelimiters");
      Assert.AreEqual(expected.Repeat, actual.Repeat, "test for MessageDelimiters");
      Assert.AreEqual(expected.Component, actual.Component, " test for MessageDelimiters");
      Assert.AreEqual(expected.SubComponent, actual.SubComponent, " test for MessageDelimiters");
      Assert.AreEqual(expected.Escape, actual.Escape, " test for MessageDelimiters");   
    }

    /// <summary>
    ///A test for MessageStructure
    ///</summary>
    [TestMethod()]
    public void MessageStructureTest()
    {
      Message target = new Message(oMsg.ToString());      
      string actual;
      actual = target.MessageStructure;
      Assert.AreEqual("ORU_R01", actual, "A test for MessageStructure");  
    }

    /// <summary>
    ///A test for MessageTrigger
    ///</summary>
    [TestMethod()]
    public void MessageTriggerTest()
    {
      Message target = new Message(oMsg.ToString());      
      string actual;
      actual = target.MessageTrigger;
      Assert.AreEqual("R01", actual, "A test for MessageTrigger");  
    }

    /// <summary>
    ///A test for MessageType
    ///</summary>
    [TestMethod()]
    public void MessageTypeTest()
    {
      Message target = new Message(oMsg.ToString());            
      string actual;
      actual = target.MessageType;
      Assert.AreEqual("ORU", actual, "A test for MessageType"); 
    }

    /// <summary>
    ///A test for MessageVersion
    ///</summary>
    [TestMethod()]
    public void MessageVersionTest()
    {
      Message target = new Message(oMsg.ToString());            
      string actual;
      actual = target.MessageVersion;
      Assert.AreEqual("2.3.1", actual, "A test for MessageVersion"); 
    }

    /// <summary>
    ///A test for SegmentCount
    ///</summary>
    [TestMethod()]
    public void SegmentCountTest()
    {
      Message target = new Message(oMsg.ToString());
      int actual;
      actual = target.SegmentCount();
      Assert.AreEqual(24, actual, "A test for SegmentCount");      
      
    }

    /// <summary>
    ///A test for PathInformation
    ///</summary>
    [TestMethod()]
    public void PathInformationTest()
    {
      Message target = new Message(oMsg.ToString());
      Glib.Hl7.V2.Model.ModelSupport.PathInformation actual;
      actual = target.PathInformation;

      Assert.AreEqual("ORU", actual.MessageType, "A test for MessageEvent");
      Assert.AreEqual("R01", actual.MessageEvent, "A test for MessageEvent");
      
      Assert.AreEqual("<unk>-?", actual.PathBrief, "A test for MessageEvent");
      Assert.AreEqual("", actual.PathVerbos, "A test for MessageEvent");

    }     

  }
}

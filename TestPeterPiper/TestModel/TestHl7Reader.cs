using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PeterPiper.Hl7.V2.Support.TextFile;
using PeterPiper.Hl7.V2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPeterPiper.TestModel
{
  [TestClass]
  public class TestHl7Reader
  {

    /// <summary>
    ///A test for Hl7Reader Constructor
    ///</summary>
    [TestMethod]
    public void Hl7ReaderConstructorTest()
    {

      string path = Support.PathSupport.AssemblyDirectory + @"\TestResource\TestSetOfMsg.dat";

      Hl7StreamReader target = new Hl7StreamReader(path);
      target.Close();
    }

    /// <summary>
    ///A test for Hl7Reader Constructor
    ///</summary>
    //[TestMethod]
    //public void Hl7ReaderConstructorTest1()
    //{
    //  Stream stream = null; // TODO: Initialize to an appropriate value
    //  Hl7Reader target = new Hl7Reader(stream);

    //}

    /// <summary>
    ///A test for Close
    ///</summary>
    //[TestMethod]
    //public void CloseTest()
    //{
    //  Stream stream = null; // TODO: Initialize to an appropriate value
    //  Hl7Reader target = new Hl7Reader(stream); // TODO: Initialize to an appropriate value
    //  target.Close();
    //  Assert.Inconclusive("A method that does not return a value cannot be verified.");
    //}

    /// <summary>
    ///A test for Read
    ///</summary>
    [TestMethod]
    public void ReadTest()
    {
      
      string readpath = Support.PathSupport.AssemblyDirectory + "\\TestResource\\TestSetOfMsg.dat";
      string writepath = Support.PathSupport.AssemblyDirectory + "\\TestResource\\TestSetOfMsg2.dat";
      if (File.Exists(writepath))
        File.Delete(writepath);

      Hl7StreamReader reader = new Hl7StreamReader(readpath);
      HL7StreamWriter writer = new HL7StreamWriter(writepath, true);
      List<IMessage> oMessageList = new List<IMessage>();
      string actual;
      while ((actual = reader.Read()) != null)
      {
        var oHl7 = Creator.Message(actual);
        Assert.IsTrue(oHl7.SegmentCount() > 1);
        oMessageList.Add(oHl7);
        writer.Write(oHl7, HL7StreamWriter.HL7OutputStyles.InterfaceReadable);
      }
      reader.Close();

      reader = new Hl7StreamReader(writepath);
      int counter = 0;
      while ((actual = reader.Read()) != null)
      {
        var oHl7 = Creator.Message(actual);
        if (!oMessageList[counter].AsStringRaw.Equals(oHl7.AsStringRaw))
        {
          Assert.Fail("The Hl7 Message read in does not match the hl7 message written out and back in again by the Hl7StreamWriter and Hl7StreamReader.");
        }
        counter++;
      }
      reader.Close();
      File.Delete(writepath);
    }


  }
}

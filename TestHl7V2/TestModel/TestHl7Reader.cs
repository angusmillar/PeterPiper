using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Glib.Hl7.V2.Support.TextFile;
using NUnit.Framework;

namespace TestHl7V2.TestModel
{
  [TestFixture]
  public class TestHl7Reader
  {

    public static string AssemblyDirectory
    {
      get
      {
        string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        string path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path);
      }
    }

    /// <summary>
    ///A test for Hl7Reader Constructor
    ///</summary>
    [Test]
    public void Hl7ReaderConstructorTest()
    {
      string path = AssemblyDirectory;

      
      path = AssemblyDirectory + @"\TestResource\TestSetOfMsg.dat";
      

      Hl7StreamReader target = new Hl7StreamReader(path);
    }

    /// <summary>
    ///A test for Hl7Reader Constructor
    ///</summary>
    [Test]
    public void Hl7ReaderConstructorTest1()
    {
      //Stream stream = null; // TODO: Initialize to an appropriate value
      //Hl7Reader target = new Hl7Reader(stream);

    }

    /// <summary>
    ///A test for Close
    ///</summary>
    [Test]
    public void CloseTest()
    {
      //Stream stream = null; // TODO: Initialize to an appropriate value
      //Hl7Reader target = new Hl7Reader(stream); // TODO: Initialize to an appropriate value
      //target.Close();
      //Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }

    /// <summary>
    ///A test for Read
    ///</summary>
    [Test]
    public void ReadTest()
    {
      string path = AssemblyDirectory;      
      string readpath = path + "\\TestResource\\TestSetOfMsg.dat";
      string writepath = path + "\\TestResource\\TestSetOfMsg2.dat";

      Hl7StreamReader reader = new Hl7StreamReader(readpath);
      HL7StreamWriter writer = new HL7StreamWriter(writepath, true);
      List<Glib.Hl7.V2.Model.Message> oMessageList = new List<Glib.Hl7.V2.Model.Message>();
      string actual;
      while ((actual = reader.Read()) != null)
      {
        Glib.Hl7.V2.Model.Message oHl7 = new Glib.Hl7.V2.Model.Message(actual);
        Assert.IsTrue(oHl7.SegmentCount() > 1);
        oMessageList.Add(oHl7);
        writer.Write(oHl7, HL7StreamWriter.HL7OutputStyles.InterfaceReadable);
      }
      reader.Close();

      reader = new Hl7StreamReader(writepath);
      int counter = 0;
      while ((actual = reader.Read()) != null)
      {
        Glib.Hl7.V2.Model.Message oHl7 = new Glib.Hl7.V2.Model.Message(actual);
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

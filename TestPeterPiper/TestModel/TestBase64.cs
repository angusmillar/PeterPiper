using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using PeterPiper.Hl7.V2.Support.Content;

namespace TestPeterPiper.TestModel
{
  [TestClass]
  public class TestBase64
  {

    /// <summary>
    ///A test for Decoder
    ///</summary>
    [TestMethod]
    public void DecoderTest()
    {
      string path = Support.PathSupport.AssemblyDirectory + @"\TestResource\ED Data Test.zip";
      
      string NewPath = String.Empty;
      byte[] item = File.ReadAllBytes(path);
      byte[] expected = null;
      string temp;
      temp = PeterPiper.Hl7.V2.Support.Tools.Base64Tools.Encoder(item);
      expected = PeterPiper.Hl7.V2.Support.Tools.Base64Tools.Decoder(temp);
      //Below is just for debugging, it writes file back out to system
      //File.WriteAllBytes(path + ".new", expected);

      bool same = item.SequenceEqual(expected);
      if (!same)
        Assert.Fail("Base64 encoding failed to encode and decode to the same byte array.");
    }

    /// <summary>
    ///A test for Encoder
    ///</summary>
    [TestMethod]
    public void EncoderTest()
    {
      //See decodetTest above which tests both encode and decode in one.
    }

  }
}

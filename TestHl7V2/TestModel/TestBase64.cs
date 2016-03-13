using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using PeterPiper.Hl7.V2.Support.Content;

namespace TestHl7V2.TestModel
{
  [TestFixture]
  public class TestBase64
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
    ///A test for Decoder
    ///</summary>
    [Test]
    public void DecoderTest()
    {
      string path = AssemblyDirectory + "\\TestResource\\ED Data Test.zip";
      string NewPath = String.Empty;
      byte[] item = File.ReadAllBytes(path);
      byte[] expected = null;
      string temp;
      temp = Base64.Encoder(item);
      expected = Base64.Decoder(temp);
      //Below is just for debugging, it writes file back out to system
      //File.WriteAllBytes(path + ".new", expected);

      bool same = item.SequenceEqual(expected);
      if (!same)
        Assert.Fail("Base64 encoding failed to encode and decode to the same byte array.");
    }

    /// <summary>
    ///A test for Encoder
    ///</summary>
    [Test]
    public void EncoderTest()
    {
      //See decodetTest above which tests both encode and decode in one.
    }

  }
}

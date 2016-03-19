using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support.Standard;
using PeterPiper.Hl7.V2.CustomException;

namespace TestPeterPiper.TestModel
{
  [TestFixture]
  public class TestPeterPiperCustomExceptions
  {
    [Test]
    public void PeterPiperArgumentException_Thrown_On_Failed_Message_Parse()
    {      
      Assert.Throws<PeterPiperArgumentException>(() => Creator.Message("rubbish"));
    }

    [Test]
    public void PeterPiperArgumentException_Thrown_On_Failed_Segment_Parse()
    {
      Assert.Throws<PeterPiperArgumentException>(() => Creator.Segment("rubbish"));
    }

    [Test]
    public void PeterPiperArgumentException_Thrown_On_Failed_Element_Parse()
    {
      Assert.Throws<PeterPiperArgumentException>(() => Creator.Element("sdad|sdad"));
    }

    [Test]
    public void PeterPiperArgumentException_Thrown_On_Failed_Field_Parse()
    {
      Assert.Throws<PeterPiperArgumentException>(() => Creator.Field("sdad|sdad"));
    }

    [Test]
    public void PeterPiperArgumentException_Thrown_On_Failed_Component_Parse()
    {
      Assert.Throws<PeterPiperArgumentException>(() => Creator.Component("s^dad|s^dad"));
    }

    [Test]
    public void PeterPiperArgumentException_Thrown_On_Failed_SubComponent_Parse()
    {
      Assert.Throws<PeterPiperArgumentException>(() => Creator.SubComponent("s^da&d|s^dad"));
    }

    [Test]
    public void PeterPiperArgumentException_Thrown_On_Failed_Content_Parse()
    {
      Assert.Throws<PeterPiperArgumentException>(() => Creator.Content("s^d\\a&\\d|s^dad"));
    }


  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support.Standard;
using PeterPiper.Hl7.V2.CustomException;

namespace TestPeterPiper.TestModel
{
  [TestClass]
  public class TestPeterPiperCustomExceptions
  {
    [TestMethod]
    [ExpectedException(typeof(PeterPiperException))]
    public void PeterPiperException_Thrown_On_Failed_Message_Parse()
    {
      Creator.Message("rubbish");
      //Assert.Throws<PeterPiperException>(() => Creator.Message("rubbish"));
    }

    [TestMethod]
    [ExpectedException(typeof(PeterPiperException))]
    public void PeterPiperException_Thrown_On_Failed_Segment_Parse()
    {
      Creator.Segment("rubbish");
    }

    [TestMethod]
    [ExpectedException(typeof(PeterPiperException))]
    public void PeterPiperException_Thrown_On_Failed_Element_Parse()
    {
      Creator.Element("sdad|sdad");
    }

    [TestMethod]
    [ExpectedException(typeof(PeterPiperException))]
    public void PeterPiperException_Thrown_On_Failed_Field_Parse()
    {
      Creator.Field("sdad|sdad");
    }

    [TestMethod]
    [ExpectedException(typeof(PeterPiperException))]
    public void PeterPiperException_Thrown_On_Failed_Component_Parse()
    {
      Creator.Component("s^dad|s^dad");
    }

    [TestMethod]
    [ExpectedException(typeof(PeterPiperException))]
    public void PeterPiperException_Thrown_On_Failed_SubComponent_Parse()
    {
      Creator.SubComponent("s^da&d|s^dad");
    }

    [TestMethod]
    [ExpectedException(typeof(PeterPiperException))]
    public void PeterPiperException_Thrown_On_Failed_Content_Parse()
    {
      Creator.Content("s^d\\a&\\d|s^dad");
    }


  }
}
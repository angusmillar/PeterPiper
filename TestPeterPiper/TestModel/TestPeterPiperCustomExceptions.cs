using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.CustomException;

namespace TestPeterPiper.TestModel;

[TestClass]
public class TestPeterPiperCustomExceptions
{
    [TestMethod]
    public void PeterPiperException_Thrown_On_Failed_Message_Parse()
    {
        Assert.Throws<PeterPiperException>(() => Creator.Message("rubbish"));
    }

    [TestMethod]
    public void PeterPiperException_Thrown_On_Failed_Segment_Parse2()
    {
        Assert.Throws<PeterPiperException>(() => Creator.Segment("rubbish"));
    }

    [TestMethod]
    public void PeterPiperException_Thrown_On_Failed_Element_Parse()
    {
        Assert.Throws<PeterPiperException>(() => Creator.Element("sdad|sdad"));
    }

    [TestMethod]
    public void PeterPiperException_Thrown_On_Failed_Field_Parse()
    {
        Assert.Throws<PeterPiperException>(() => Creator.Field("sdad|sdad"));
    }

    [TestMethod]
    public void PeterPiperException_Thrown_On_Failed_Component_Parse()
    {
        Assert.Throws<PeterPiperException>(() => Creator.Component("s^dad|s^dad"));
    }

    [TestMethod]
    public void PeterPiperException_Thrown_On_Failed_SubComponent_Parse()
    {
        Assert.Throws<PeterPiperException>(() => Creator.SubComponent("s^da&d|s^dad"));
    }

    [TestMethod]
    public void PeterPiperException_Thrown_On_Failed_Content_Parse()
    {
        Assert.Throws<PeterPiperException>(() => Creator.Content("s^d\\a&\\d|s^dad"));
    }
}
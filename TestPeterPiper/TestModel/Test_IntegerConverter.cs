using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeterPiper.Hl7.V2.Model;

namespace TestPeterPiper.TestModel
{
  [TestClass]
  public class Test_IntegerConverter
  {
    [TestMethod]
    public void Test_Can_Convert_Int32()
    {
      string StringRaw = "MSH|^~\\&|||||||ORM^O01^ORM_O01|2147483647|P|2.3.1";
      var target = Creator.Segment(StringRaw);
      Int32 result = target.Element(10).Convert.Integer.Int32;
      Assert.AreEqual(result, 2147483647);
    }

    [TestMethod]
    public void Test_Can_Convert_Int()
    {
      string StringRaw = "MSH|^~\\&|||||||ORM^O01^ORM_O01|2147483647|P|2.3.1";
      var target = Creator.Segment(StringRaw);
      int result = target.Element(10).Convert.Integer.Int;
      Assert.AreEqual(result, 2147483647);
    }

    [TestMethod]
    public void Test_Can_Convert_Int16()
    {
      string StringRaw = "MSH|^~\\&|||||||ORM^O01^ORM_O01|32767|P|2.3.1";
      var target = Creator.Segment(StringRaw);
      Int16 result = target.Element(10).Convert.Integer.Int16;
      Assert.AreEqual(result, 32767);
    }

    [TestMethod]
    public void Test_Can_Convert_Int64()
    {
      string StringRaw = "MSH|^~\\&|||||||ORM^O01^ORM_O01|9223372036854775807|P|2.3.1";
      var target = Creator.Segment(StringRaw);
      Int64 result = target.Element(10).Convert.Integer.Int64;
      Assert.AreEqual(result, 9223372036854775807);
    }

    [TestMethod]
    public void Test_IsNumeric_True()
    {
      string StringRaw = "MSH|^~\\&|||||||ORM^O01^ORM_O01|9223372036854775807|P|2.3.1";
      var target = Creator.Segment(StringRaw);
      Assert.IsTrue(target.Element(10).Convert.Integer.IsNumeric);
    }

    [TestMethod]
    public void Test_IsNumeric_False()
    {
      string StringRaw = "MSH|^~\\&|||||||ORM^O01^ORM_O01|922abc2036854775807|P|2.3.1";
      var target = Creator.Segment(StringRaw);
      Assert.IsFalse(target.Element(10).Convert.Integer.IsNumeric);
    }

    [TestMethod]    
    public void Test_Cannot_Convert_AlphaStirngToInt32()
    {
      string StringRaw = "MSH|^~\\&|||||||ORM^O01^ORM_O01|21ABC83647|P|2.3.1";
      var target = Creator.Segment(StringRaw);
      try
      {
        Int32 result = target.Element(10).Convert.Integer.Int32;
        Assert.Fail();         
      }
      catch(FormatException exec)
      {
        Assert.IsTrue(exec.Message == "The value '21ABC83647' could not be converted to an int32 integer data type.");
      }     
    }

    [TestMethod]
    public void Test_Cannot_Convert_AlphaStirngToInt16()
    {
      string StringRaw = "MSH|^~\\&|||||||ORM^O01^ORM_O01|21ABC83647|P|2.3.1";
      var target = Creator.Segment(StringRaw);
      try
      {
        Int16 result = target.Element(10).Convert.Integer.Int16;
        Assert.Fail();
      }
      catch (FormatException exec)
      {
        Assert.IsTrue(exec.Message == "The value '21ABC83647' could not be converted to an int16 integer data type.");
      }
    }

    [TestMethod]
    public void Test_Cannot_Convert_AlphaStirngToInt64()
    {
      string StringRaw = "MSH|^~\\&|||||||ORM^O01^ORM_O01|21ABC83647|P|2.3.1";
      var target = Creator.Segment(StringRaw);
      try
      {
        Int64 result = target.Element(10).Convert.Integer.Int64;
        Assert.Fail();
      }
      catch (FormatException exec)
      {
        Assert.IsTrue(exec.Message == "The value '21ABC83647' could not be converted to an int64 integer data type.");
      }
    }

    [TestMethod]
    public void Test_Cannot_Convert_AlphaStirngToInt()
    {
      string StringRaw = "MSH|^~\\&|||||||ORM^O01^ORM_O01|21ABC83647|P|2.3.1";
      var target = Creator.Segment(StringRaw);
      try
      {
        int result = target.Element(10).Convert.Integer.Int;
        Assert.Fail();
      }
      catch (FormatException exec)
      {
        Assert.IsTrue(exec.Message == "The value '21ABC83647' could not be converted to an int32 integer data type.");
      }
    }


  }
}

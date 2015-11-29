using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glib.Hl7.V2.Model;
using Glib.Hl7.V2.Support;
using System.Collections.ObjectModel;
using NUnit.Framework;


namespace TestHl7V2
{
  [TestFixture]
  public class TestSegment
  {
    public MessageDelimiters CustomDelimiters;

    [SetUp]
    public void MyTestInitialize()
    {
      CustomDelimiters = new MessageDelimiters('#', '@', '*', '!', '%');
    }


    [Test]
    public void SegmentConstructorTest()
    {
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);
      Assert.AreEqual("MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1", target.AsStringRaw, "A test for Segment Constructor");
    }

    /// <summary>
    ///A test for Segment Constructor
    ///</summary>
    [Test]
    public void SegmentConstructorTest1()
    {
      //'#', '@', '*', '!', '%'
      string StringRaw = "MSH#*@%!#####20141208064531##ORM*O01*ORM_O01#Q54356818T82744882#P#2.3.1";
      Segment target = new Segment(StringRaw, CustomDelimiters);
      Assert.AreEqual("MSH#*@%!#####20141208064531##ORM*O01*ORM_O01#Q54356818T82744882#P#2.3.1", target.AsStringRaw, "A test for Segment Constructor 1");
      Assert.AreEqual(12, target.ElementCount, "A test for Segment Constructor 1");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [Test]
    public void AddTest1()
    {
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);
      Element item = new Element("Hello World");
      target.Add(item);
      Assert.AreEqual("MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1|Hello World", target.AsStringRaw, "A test for Add");
    }

    /// <summary>
    ///A test for Add
    ///</summary>
    [Test]
    public void AddTest2()
    {
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);
      Field item = new Field("Hello World");
      target.Add(item);
      Assert.AreEqual("MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1|Hello World", target.AsStringRaw, "A test for Add 2");
    }


    /// <summary>
    ///A test for ClearAll
    ///</summary>
    [Test]
    public void ClearAllTest()
    {
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);
      target.ClearAll();
      Assert.AreEqual("MSH|^~\\&|", target.AsStringRaw, "A test for ClearAll");
      target.Add(new Field("TestField"));
      Assert.AreEqual("MSH|^~\\&|TestField", target.AsStringRaw, "A test for ClearAll");
      StringRaw = "ZQT|TestCase^35|Step^2|Message^2";
      target = new Segment(StringRaw);
      Assert.AreEqual("ZQT|TestCase^35|Step^2|Message^2", target.AsStringRaw, "A test for ClearAll");
      target.ClearAll();
      Assert.AreEqual("ZQT|", target.AsStringRaw, "A test for ClearAll");
    }

    /// <summary>
    ///A test for Clone
    ///</summary>
    [Test]
    public void CloneTest()
    {
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);
      Segment expected = new Segment("MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1");
      Segment actual;
      actual = target.Clone();
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Clone");
    }

    /// <summary>
    ///A test for Element
    ///</summary>
    [Test]
    public void ElementTest()
    {
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);
      int index = 9;
      Element expected = new Element("ORM^O01^ORM_O01");
      Element actual;
      actual = target.Element(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Element");
    }

    /// <summary>
    ///A test for Field
    ///</summary>
    [Test]
    public void FieldTest()
    {
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);
      int index = 9;
      Field expected = new Field("ORM^O01^ORM_O01");
      Field actual;
      actual = target.Field(index);
      Assert.AreEqual(expected.AsStringRaw, actual.AsStringRaw, "A test for Field");
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [Test]
    public void InsertTest()
    {
      System.Text.StringBuilder sbMSH1Exception = new System.Text.StringBuilder();
      sbMSH1Exception.Append("MSH-1 contains the 'Main Separator' charater and is not accessible from the Field or Element object instance as it is critical to message construction.");
      sbMSH1Exception.Append(Environment.NewLine);
      sbMSH1Exception.Append("Instead, you can access the read only property call 'MainSeparator' from the Message object instance.");

      System.Text.StringBuilder sbMSH2Exception = new System.Text.StringBuilder();
      sbMSH2Exception.Append("MSH-2 contains the 'Encoding Characters' and is not accessible from the Field or Element object instance as it is critical to message construction.");
      sbMSH2Exception.Append(Environment.NewLine);
      sbMSH2Exception.Append("Instead, you can access the read only property call 'EscapeSequence' from the Message object instance.");

      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);

      Field item = new Field("Hello");

      try
      {
        target.Insert(1, item);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual(sbMSH1Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        target.Insert(2, item);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual(sbMSH2Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      target.Insert(3, item);
      Assert.AreEqual("MSH|^~\\&|Hello|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1", target.AsStringRaw, "A test for Field");
    }

    /// <summary>
    ///A test for Insert
    ///</summary>
    [Test]
    public void InsertTest1()
    {
      System.Text.StringBuilder sbMSH1Exception = new System.Text.StringBuilder();
      sbMSH1Exception.Append("MSH-1 contains the 'Main Separator' charater and is not accessible from the Field or Element object instance as it is critical to message construction.");
      sbMSH1Exception.Append(Environment.NewLine);
      sbMSH1Exception.Append("Instead, you can access the read only property call 'MainSeparator' from the Message object instance.");

      System.Text.StringBuilder sbMSH2Exception = new System.Text.StringBuilder();
      sbMSH2Exception.Append("MSH-2 contains the 'Encoding Characters' and is not accessible from the Field or Element object instance as it is critical to message construction.");
      sbMSH2Exception.Append(Environment.NewLine);
      sbMSH2Exception.Append("Instead, you can access the read only property call 'EscapeSequence' from the Message object instance.");

      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);

      Element item = new Element("Hello");
      try
      {
        target.Insert(1, item);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual(sbMSH1Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        target.Insert(2, item);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual(sbMSH2Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      target.Insert(3, item);
      Assert.AreEqual("MSH|^~\\&|Hello|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1", target.AsStringRaw, "A test for Field");
    }

    /// <summary>
    ///A test for RemoveElementAt
    ///</summary>
    [Test]
    public void RemoveElementAtTest()
    {
      System.Text.StringBuilder sbMSH1Exception = new System.Text.StringBuilder();
      sbMSH1Exception.Append("MSH-1 contains the 'Main Separator' charater and is not accessible from the Field or Element object instance as it is critical to message construction.");
      sbMSH1Exception.Append(Environment.NewLine);
      sbMSH1Exception.Append("Instead, you can access the read only property call 'MainSeparator' from the Message object instance.");

      System.Text.StringBuilder sbMSH2Exception = new System.Text.StringBuilder();
      sbMSH2Exception.Append("MSH-2 contains the 'Encoding Characters' and is not accessible from the Field or Element object instance as it is critical to message construction.");
      sbMSH2Exception.Append(Environment.NewLine);
      sbMSH2Exception.Append("Instead, you can access the read only property call 'EscapeSequence' from the Message object instance.");

      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);

      try
      {
        target.RemoveElementAt(0);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual("Specified argument was out of the range of valid values.\r\nParameter name: Element index is a one based index, zero in not allowed", ae.Message, "A test for RemoveElementAt");
      }

      try
      {
        target.RemoveElementAt(1);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual(sbMSH1Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching", "A test for RemoveElementAt");
      }

      try
      {
        target.RemoveElementAt(2);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual(sbMSH2Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching", "A test for RemoveElementAt");
      }

      target.RemoveElementAt(3);
      Assert.AreEqual("MSH|^~\\&||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1", target.AsStringRaw, "A test for RemoveElementAt");


    }

    /// <summary>
    ///A test for RemoveFieldAt
    ///</summary>
    [Test]
    public void RemoveFieldAtTest()
    {
      System.Text.StringBuilder sbMSH1Exception = new System.Text.StringBuilder();
      sbMSH1Exception.Append("MSH-1 contains the 'Main Separator' charater and is not accessible from the Field or Element object instance as it is critical to message construction.");
      sbMSH1Exception.Append(Environment.NewLine);
      sbMSH1Exception.Append("Instead, you can access the read only property call 'MainSeparator' from the Message object instance.");

      System.Text.StringBuilder sbMSH2Exception = new System.Text.StringBuilder();
      sbMSH2Exception.Append("MSH-2 contains the 'Encoding Characters' and is not accessible from the Field or Element object instance as it is critical to message construction.");
      sbMSH2Exception.Append(Environment.NewLine);
      sbMSH2Exception.Append("Instead, you can access the read only property call 'EscapeSequence' from the Message object instance.");

      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);

      try
      {
        target.RemoveFieldAt(0);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual("Specified argument was out of the range of valid values.\r\nParameter name: Element index is a one based index, zero in not allowed", ae.Message, "A test for RemoveElementAt");
      }

      try
      {
        target.RemoveFieldAt(1);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual(sbMSH1Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching", "A test for RemoveElementAt");
      }

      try
      {
        target.RemoveFieldAt(2);
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual(sbMSH2Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching", "A test for RemoveElementAt");
      }

      target.RemoveElementAt(3);
      Assert.AreEqual("MSH|^~\\&||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1", target.AsStringRaw, "A test for RemoveElementAt");

    }

    /// <summary>
    ///A test for ToString
    ///</summary>
    [Test]
    public void ToStringTest()
    {
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);
      string expected = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      string actual;
      actual = target.ToString();
      Assert.AreEqual(expected, actual, "A test for ToString");
    }

    /// <summary>
    ///A test for AsString
    ///</summary>
    [Test]
    public void AsStringTest()
    {
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);
      string expected = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      string actual;
      try
      {
        target.AsString = expected;
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual("Unable to modify an existing MSH segment instance with the AsString or AsStringRaw properties. /n You need to create a new Segment instance and use it's constructor or selectively edit this segment's parts.", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching", "A test for AsString");
      }

      actual = target.AsString;
      Assert.AreEqual(expected, actual, "A test for AsString");

      StringRaw = "OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB";
      target = new Segment(StringRaw);
      actual = target.AsString;
      expected = "OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10^6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB";
      Assert.AreEqual(expected, actual, "A test for AsString");
    }

    /// <summary>
    ///A test for AsStringRaw
    ///</summary>
    [Test]
    public void AsStringRawTest()
    {
      string StringRaw = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      Segment target = new Segment(StringRaw);
      string expected = "MSH|^~\\&|||||20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1";
      string actual;
      try
      {
        target.AsStringRaw = expected;
        Assert.Fail("An exception should have been thrown");
      }
      catch (ArgumentException ae)
      {
        Assert.AreEqual("Unable to modify an existing MSH segment instance with the AsString or AsStringRaw properties. /n You need to create a new Segment instance and use it's constructor or selectively edit this segment's parts.", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching", "A test for AsString");
      }

      actual = target.AsStringRaw;
      Assert.AreEqual(expected, actual, "A test for AsStringRaw");

      StringRaw = "OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB";
      target = new Segment(StringRaw);
      actual = target.AsStringRaw;
      expected = "OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB";
      Assert.AreEqual(expected, actual, "A test for AsStringRaw");
    }

    /// <summary>
    ///A test for Code
    ///</summary>
    [Test]
    public void CodeTest()
    {
      string StringRaw = "OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB";
      Segment target = new Segment(StringRaw);
      string actual;
      actual = target.Code;
      Assert.AreEqual("OBX", actual, "A test for AsStringRaw");
    }

    /// <summary>
    ///A test for ElementCount
    ///</summary>
    [Test]
    public void ElementCountTest()
    {
      string StringRaw = "OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB";
      Segment target = new Segment(StringRaw);
      int actual;
      actual = target.ElementCount;
      Assert.AreEqual(15, actual, "A test for AsStringRaw");

    }

    /// <summary>
    ///A test for ElementList
    ///</summary>
    [Test]
    public void ElementListTest()
    {
      string StringRaw = "OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB";
      Segment target = new Segment(StringRaw);
      ReadOnlyCollection<Element> actual;
      actual = target.ElementList;
      int counter = 1;
      Assert.AreEqual(15, actual.Count, "A test for ElementList");

      foreach (var item in actual)
      {
        Assert.AreEqual(target.Element(counter).AsStringRaw, item.AsStringRaw, "A test for ElementList");
        item.AsString = counter.ToString();
        counter++;
      }
      Assert.AreEqual(counter, actual.Count + 1, "A test for ElementList");
      Assert.AreEqual("OBX|1|2|3|4|5|6|7|8|9|10|11|12|13|14|15", target.AsStringRaw, "A test for ElementList");
    }

    /// <summary>
    ///A test for FieldCount
    ///</summary>
    [Test]
    public void FieldCountTest()
    {
      string StringRaw = "OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB";
      Segment target = new Segment(StringRaw);
      int actual;
      actual = target.FieldCount;
      Assert.AreEqual(15, actual, "A test for FieldCount");
    }

    /// <summary>
    ///A test for IsEmpty
    ///</summary>
    [Test]
    public void IsEmptyTest()
    {
      string StringRaw = "OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||F|||201405270956|RB^PATH QLD Central^AUSLAB";
      Segment target = new Segment(StringRaw);
      bool actual;
      actual = target.IsEmpty;
      Assert.AreEqual(false, actual, "A test for FieldCount");
      target.ClearAll();
      Assert.AreEqual(true, target.IsEmpty, "A test for FieldCount");
    }

    /// <summary>
    ///A test for PathInformation
    ///</summary>
    [Test]
    public void PathInformationTest()
    {
      string StringRaw = "OBX|12|ST|UWBC^Urine Micro WBC^AUSLAB||<  10|x10\\S\\6/L|< 10|N|||oneC^TwoC^One&Two~one2C^Two2C^One2&Two2|||201405270956|RB^PATH QLD Central^AUSLAB";
      Segment target = new Segment(StringRaw);
      Assert.AreEqual("OBX", target.PathDetail.PathBrief, "A test for PathInformation 1");
      Assert.AreEqual("Segment: OBX", target.PathDetail.PathVerbos, "A test for PathInformation 2");

      Assert.AreEqual("OBX-3", target.Field(3).PathDetail.PathBrief, "A test for PathInformation 3");
      Assert.AreEqual("Segment: OBX, Field: 3", target.Field(3).PathDetail.PathVerbos, "A test for PathInformation 4");

      Assert.AreEqual("OBX-3", target.Element(3).PathDetail.PathBrief, "A test for PathInformation 5");
      Assert.AreEqual("Segment: OBX, Field: 3", target.Element(3).PathDetail.PathVerbos, "A test for PathInformation 6");

      Assert.AreEqual("OBX-3.2", target.Element(3).Component(2).PathDetail.PathBrief, "A test for PathInformation 7");
      Assert.AreEqual("Segment: OBX, Field: 3, Component: 2", target.Element(3).Component(2).PathDetail.PathVerbos, "A test for PathInformation 8");

      Assert.AreEqual("OBX-6 [1]", target.Element(6).Content(1).PathDetail.PathBrief, "A test for PathInformation 9");
      Assert.AreEqual("Segment: OBX, Field: 6, [Content: 1]", target.Element(6).Content(1).PathDetail.PathVerbos, "A test for PathInformation 10");

      Assert.AreEqual("OBX-6", target.Element(6).PathDetail.PathBrief, "A test for PathInformation 11");
      Assert.AreEqual("Segment: OBX, Field: 6", target.Element(6).PathDetail.PathVerbos, "A test for PathInformation 12");

      Assert.AreEqual("OBX-11.3.2 {Rpt: 2}", target.Element(11).Repeat(2).Component(3).SubComponent(2).PathDetail.PathBrief, "A test for PathInformation 11");
      Assert.AreEqual("Segment: OBX, Field: 11, {Repeat: 2, Component: 3, SubComponent: 2 }", target.Element(11).Repeat(2).Component(3).SubComponent(2).PathDetail.PathVerbos, "A test for PathInformation 12");

    }

    /// <summary>
    ///A test for MessageDelimiters
    ///</summary>
    [Test]
    public void DelimterAccessTest()
    {
      //'#', '@', '*', '!', '%'
      string StringRaw = "MSH#*@%!#####20141208064531##ORM*O01*ORM_O01#Q54356818T82744882#P#2.3.1";
      Segment target = new Segment(StringRaw, CustomDelimiters);
      Assert.AreEqual('#', target.MessageDelimiters.Field, "A test for MessageDelimiters");
      Assert.AreEqual('@', target.MessageDelimiters.Repeat, "A test for MessageDelimiters");
      Assert.AreEqual('*', target.MessageDelimiters.Component, "A test for MessageDelimiters");
      Assert.AreEqual('!', target.MessageDelimiters.SubComponent, "A test for MessageDelimiters");
      Assert.AreEqual('%', target.MessageDelimiters.Escape, "A test for MessageDelimiters");
    }
  }
}

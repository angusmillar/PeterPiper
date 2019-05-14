using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPeterPiper.TestModel
{
  [TestClass]
  public class UnitTestContentModel
  {
    [TestMethod]
    [TestCategory("Content")]
    public void TestContentTypeCreate()
    {
      var oContent = Creator.Content("The data for a content", PeterPiper.Hl7.V2.Support.Content.ContentType.Text);
      Assert.AreEqual("The data for a content", oContent.AsString, "AsString Failed on get");
      Assert.AreEqual("The data for a content", oContent.AsStringRaw, "AsStringRaw Failed on get");
      Assert.AreEqual("The data for a content", oContent.ToString(), "ToString Failed on get");
    }

    [TestMethod]
    [TestCategory("Content")]
    public void TestContentTypeExceptions()
    {
      IContent obj = null;
      try
      {
        obj = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.Delimiters.Field.ToString(), PeterPiper.Hl7.V2.Support.Content.ContentType.Text);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Content data cannot contain HL7 V2 Delimiters, maybe you should be using 'AsStringRaw' if you are trying to insert already escaped data.", ae.Message);
      }

      try
      {
        obj = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.Delimiters.Component.ToString(), PeterPiper.Hl7.V2.Support.Content.ContentType.Text);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Content data cannot contain HL7 V2 Delimiters, maybe you should be using 'AsStringRaw' if you are trying to insert already escaped data.", ae.Message);
      }

      try
      {
        obj = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.Delimiters.SubComponent.ToString(), PeterPiper.Hl7.V2.Support.Content.ContentType.Text);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Content data cannot contain HL7 V2 Delimiters, maybe you should be using 'AsStringRaw' if you are trying to insert already escaped data.", ae.Message);
      }

      try
      {
        obj = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.Delimiters.Repeat.ToString(), PeterPiper.Hl7.V2.Support.Content.ContentType.Text);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Content data cannot contain HL7 V2 Delimiters, maybe you should be using 'AsStringRaw' if you are trying to insert already escaped data.", ae.Message);
      }

      try
      {
        obj = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.Delimiters.Escape.ToString(), PeterPiper.Hl7.V2.Support.Content.ContentType.Text);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Content data cannot contain HL7 V2 Delimiters, maybe you should be using 'AsStringRaw' if you are trying to insert already escaped data.", ae.Message);
      }

      try
      {
        obj = Creator.Content("Test Data", PeterPiper.Hl7.V2.Support.Content.ContentType.Text);
        obj.AsString = PeterPiper.Hl7.V2.Support.Standard.Delimiters.Escape.ToString();
        Assert.Fail("An exception should have been thrown on setting AsString");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Content data cannot contain HL7 V2 Delimiters, maybe you should be using 'AsStringRaw' if you are trying to insert already escaped data.", ae.Message);
      }

      try
      {
        obj = Creator.Content("Test Data", PeterPiper.Hl7.V2.Support.Content.ContentType.Text);
        obj.AsStringRaw = PeterPiper.Hl7.V2.Support.Standard.Delimiters.Escape.ToString();
        Assert.Fail("An exception should have been thrown on setting AsString");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Content data cannot contain HL7 V2 Delimiters, maybe you should be using 'AsStringRaw' if you are trying to insert already escaped data.", ae.Message);
      }
    }
  }

  [TestClass]
  public class UnitTestSubComponentModel
  {
    [TestMethod]
    [TestCategory("Content")]
    public void TestSubComponentCreate()
    {
      var oSubComonentWithEscapes = Creator.SubComponent("\\N\\this is not Highlighted\\H\\ This is higlighted \\N\\ This is not, this is hex  \\X0xC2\\ this is local \\Ztesttest\\ this is field \\F\\ this is Compponet \\S\\ this is SubCompoent \\T\\ repeat \\R\\ this is escape \\E\\ done ");
      var oSubComonentPlainOLDText = Creator.SubComponent("This is plan old test");
    }

    [TestMethod]
    [TestCategory("Content")]
    public void TestSubComponentAddingContent()
    {
      string FullTestStringWithEscapes = "not highlighted\\H\\ Highlighted \\N\\highlighted ended \\H\\Added highlight \\N\\Added not highlight \\H\\\\N\\";
      string FullTestStringWithOutEscapes = "not highlighted Highlighted highlighted ended Added highlight Added not highlight ";
      var oSubComonent = Creator.SubComponent("not highlighted\\H\\ Highlighted \\N\\highlighted ended ");
      var oContentEscapeHiglightStart = Creator.Content("H", PeterPiper.Hl7.V2.Support.Content.ContentType.Escape);
      var oContentEscapeHiglightEnd = Creator.Content("N", PeterPiper.Hl7.V2.Support.Content.ContentType.Escape);
      var oContent1 = Creator.Content("Added highlight ", PeterPiper.Hl7.V2.Support.Content.ContentType.Text);
      var oContent2 = Creator.Content("Added not highlight ", PeterPiper.Hl7.V2.Support.Content.ContentType.Text);
      oSubComonent.Add(oContentEscapeHiglightStart);
      oSubComonent.Add(oContent1);
      oSubComonent.Add(oContentEscapeHiglightEnd);
      oSubComonent.Add(oContent2);
      oSubComonent.Add(oContentEscapeHiglightStart.Clone());
      oSubComonent.Add(oContentEscapeHiglightEnd.Clone());
      Assert.AreEqual(oSubComonent.AsStringRaw, FullTestStringWithEscapes);
      Assert.AreEqual(oSubComonent.AsString, FullTestStringWithOutEscapes);
      Assert.AreEqual(FullTestStringWithOutEscapes, oSubComonent.ToString());
      oSubComonent.AsString = FullTestStringWithOutEscapes;
      Assert.AreEqual(oSubComonent.AsStringRaw, FullTestStringWithOutEscapes);
      Assert.AreEqual(oSubComonent.AsString, FullTestStringWithOutEscapes);
      Assert.AreEqual(oSubComonent.ToString(), FullTestStringWithOutEscapes);
      oSubComonent.AsStringRaw = FullTestStringWithEscapes;
      Assert.AreEqual(FullTestStringWithEscapes, oSubComonent.AsStringRaw);
      Assert.AreEqual(oSubComonent.AsString, FullTestStringWithOutEscapes);
      Assert.AreEqual(oSubComonent.ToString(), FullTestStringWithOutEscapes);

      oSubComonent = Creator.SubComponent("\\H\\This is bold text\\N\\");
      Assert.AreEqual(PeterPiper.Hl7.V2.Support.Standard.EscapeType.HighlightOn, oSubComonent.Content(0).EscapeMetaData.EscapeType);
      Assert.AreEqual("This is bold text", oSubComonent.Content(1).AsStringRaw);
      Assert.AreEqual(PeterPiper.Hl7.V2.Support.Standard.EscapeType.HighlightOff, oSubComonent.Content(2).EscapeMetaData.EscapeType);

      Assert.AreEqual(PeterPiper.Hl7.V2.Support.Content.ContentType.Escape, oSubComonent.Content(0).ContentType, "Incorrect ContentType");
      Assert.AreEqual(PeterPiper.Hl7.V2.Support.Content.ContentType.Text, oSubComonent.Content(1).ContentType, "Incorrect ContentType");
      Assert.AreEqual(PeterPiper.Hl7.V2.Support.Content.ContentType.Escape, oSubComonent.Content(2).ContentType, "Incorrect ContentType");

      oSubComonent = Creator.SubComponent("\\H\\This is bold text\\N\\Not bold Text");
      oSubComonent.Insert(10, Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine));
      Assert.AreEqual(oSubComonent.Content(4).EscapeMetaData.EscapeType, PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine, "Incorrect ContentType");
      Assert.AreEqual("\\H\\This is bold text\\N\\Not bold Text\\.br\\", oSubComonent.AsStringRaw, "Incorrect ContentType");
      oSubComonent.RemoveContentAt(2);
      Assert.AreEqual("\\H\\This is bold textNot bold Text\\.br\\", oSubComonent.AsStringRaw, "Incorrect ContentType");
      oSubComonent.Insert(1, Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.NewLine));
      Assert.AreEqual("\\H\\\\.br\\This is bold textNot bold Text\\.br\\", oSubComonent.AsStringRaw, "Incorrect ContentType");
    }

    [TestMethod]
    [TestCategory("Content")]
    public void TestSubComponentSettingContent()
    {
      var oSubComp = Creator.SubComponent();
      if (oSubComp.Content(5).AsString == string.Empty)
      {
        oSubComp.Content(5).AsString = "New Text";
      }
      var oConentHEscape = Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.HighlightOn);
      var oConentText = Creator.Content("This will be highlighted");
      var oConentNEscape = Creator.Content("N", PeterPiper.Hl7.V2.Support.Content.ContentType.Escape);

      oSubComp.ClearAll();
      oSubComp.Add(oConentHEscape);
      oSubComp.Add(oConentText);
      oSubComp.Add(oConentNEscape);
      Assert.AreEqual(oSubComp.Content(0).AsString, String.Empty);
      Assert.AreEqual(oSubComp.Content(0).AsStringRaw, "H");
      Assert.AreEqual(oSubComp.Content(1).AsString, "This will be highlighted");
      Assert.AreEqual(oSubComp.Content(1).AsStringRaw, "This will be highlighted");
      Assert.AreEqual(oSubComp.Content(2).AsString, String.Empty);
      Assert.AreEqual(oSubComp.Content(2).AsStringRaw, "N");
      Assert.AreEqual(oSubComp.AsStringRaw, "\\H\\This will be highlighted\\N\\");
      Assert.AreEqual(oSubComp.AsString, "This will be highlighted");
      oSubComp.Insert(1, Creator.Content(" 2nd "));
      Assert.AreEqual(oSubComp.AsStringRaw, "\\H\\ 2nd This will be highlighted\\N\\", "Testing oSubComp.Add and AsString, ASStringRaw");


      oSubComp.ClearAll();
      oSubComp.Insert(99, Creator.Content("First "));
      oSubComp.Insert(1, Creator.Content("Second "));
      oSubComp.Insert(2, Creator.Content("Fourth "));
      oSubComp.Insert(3, Creator.Content("Fith "));
      oSubComp.Insert(4, Creator.Content("Sixth "));
      oSubComp.Insert(5, Creator.Content("Seven "));
      oSubComp.Insert(2, Creator.Content("Third "));
      Assert.AreEqual(oSubComp.AsStringRaw, "First Second Third Fourth Fith Sixth Seven ", "Testing oSubComp.ContentInsertAfter");

      oSubComp.ClearAll();
      oSubComp.Insert(100, Creator.Content("First "));
      oSubComp.Insert(0, Creator.Content("Second "));
      oSubComp.Insert(0, Creator.Content("Fourth "));
      oSubComp.Insert(0, Creator.Content("Fith "));
      oSubComp.Insert(0, Creator.Content("Sixth "));
      oSubComp.Insert(0, Creator.Content("Seven "));
      oSubComp.Insert(4, Creator.Content("Third "));
      Assert.AreEqual(oSubComp.AsStringRaw, "Seven Sixth Fith Fourth Third Second First ", "Testing oSubComp.ContentInsertBefore");

      oSubComp.RemoveContentAt(3);
      Assert.AreEqual(oSubComp.AsStringRaw, "Seven Sixth Fith Third Second First ", "testing oSubComp.AsStringRaw");

      oSubComp.ClearAll();
      oSubComp.Add(Creator.Content("First "));
      oSubComp.Add(Creator.Content("Third "));
      oSubComp.Add(Creator.Content("Fourth "));
      oSubComp.Add(Creator.Content("Fith "));
      oSubComp.Add(Creator.Content("Seven "));
      oSubComp.Insert(1, Creator.Content("Second "));
      oSubComp.Insert(5, Creator.Content("Sixth "));
      oSubComp.RemoveContentAt(3);
      Assert.AreEqual(oSubComp.AsStringRaw, "First Second Third Fith Sixth Seven ", "Testing oSubComp.Content mixure");

      int Counter = 0;
      foreach (var oContent in oSubComp.ContentList)
      {
        Assert.AreEqual(oContent.AsStringRaw, oSubComp.Content(Counter).AsStringRaw, "oSubComp.ContentList not equal to Content(index)");
        Counter++;
      }

    }

    [TestMethod]
    [TestCategory("Content")]
    public void TestSubComponentContentCount()
    {
      var oSubComp = Creator.SubComponent("First ");
      oSubComp.Add(Creator.Content("Second "));
      oSubComp.Add(Creator.Content("Third "));
      oSubComp.Add(Creator.Content("Fourth "));
      Assert.AreEqual(oSubComp.ContentCount, 4, "Testing oSubComp.ContentCount");
      oSubComp.Content(10).AsStringRaw = "";
      Assert.AreEqual(oSubComp.ContentCount, 4, "Testing oSubComp.ContentCount");
      if (oSubComp.Content(10).AsStringRaw == "Something")
      {
        Assert.Fail("This should not equal anything");
      }
      Assert.AreEqual(oSubComp.ContentCount, 4, "Testing oSubComp.ContentCount with temp Content genertaed");
      oSubComp.Content(10).AsStringRaw = "Ten ";
      Assert.AreEqual(5, oSubComp.ContentCount, "Testing oSubComp.ContentCount with temp Content genertaed");
    }
  }

  [TestClass]
  public class UnitTestComponentModel
  {
    [TestMethod]
    [TestCategory("Content")]
    public void TestComponentCreate()
    {
      var oComponent = Creator.Component("First&Se\\e\\cond&\\H\\Third is Bold\\n\\&forth&fith");
      Assert.AreEqual("First&Se\\e\\cond&\\H\\Third is Bold\\n\\&forth&fith", oComponent.AsStringRaw, "oComonentWithEscapes.AsStringRaw did not match create string");
      oComponent.AsStringRaw = "";
      oComponent.AsStringRaw = "First&Se\\e\\cond&\\H\\Third is Bold\\n\\&forth&fith";
      Assert.AreEqual("First&Se\\e\\cond&\\H\\Third is Bold\\n\\&forth&fith", oComponent.AsStringRaw, "oComonentWithEscapes.AsStringRaw did not match Set StringRaw");
      Assert.AreEqual("First&Se\\cond&Third is Bold&forth&fith", oComponent.AsString, "oComonentWithEscapes.AsString did not match Set StringRaw");

      oComponent.AsString = "First&Men\\E\\Women&\\H\\Third is Bold\\n\\&forth&fith";
      Assert.AreEqual("First\\T\\Men Women\\T\\Third is Bold\\T\\forth\\T\\fith", oComponent.AsStringRaw, "oComonentWithEscapes.AsStringRaw did not match Set StringRaw");
      Assert.AreEqual("First&Men Women&Third is Bold&forth&fith", oComponent.AsString, "oComonentWithEscapes.AsString did not match Set StringRaw");

      oComponent.AsStringRaw = "First&Second&&&&Six&&&&";
      Assert.AreEqual("First&Second&&&&Six", oComponent.AsStringRaw, "Check trailing delimiters are removed");

      Assert.AreEqual("Second", oComponent.SubComponent(2).AsString, ",oComonent.SubComponent failed to return correct data");

      if (oComponent.SubComponent(100).AsString == "")
      {
        Assert.AreEqual(6, oComponent.SubComponentCount, ",oComonent.CountSubComponent is incorrect");
      }

      try
      {
        string test = oComponent.SubComponent(0).AsString;
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("SubComponent is a one based index, zero is not a valid index", ae.Message, "Exception should have been thrown on zero index for SubComponent");
      }

      oComponent.ClearAll();
      Assert.AreEqual(0, oComponent.SubComponentCount, ",oComonent.CountSubComponent is incorrect");
      oComponent.Add(Creator.SubComponent("First"));
      oComponent.Add(Creator.SubComponent("Second"));
      oComponent.Add(Creator.SubComponent("Third"));
      oComponent.Add(Creator.SubComponent("Forth"));
      Assert.AreEqual(4, oComponent.SubComponentCount, ",oComonent.CountSubComponent is incorrect");
      Assert.AreEqual("First", oComponent.SubComponent(1).AsStringRaw, ",oComponent.SubComponent() returned incorrect");
      Assert.AreEqual("Second", oComponent.SubComponent(2).AsString, ",oComponent.SubComponent() returned incorrect");
      Assert.AreEqual("Third", oComponent.SubComponent(3).AsStringRaw, ",oComponent.SubComponent() returned incorrect");
      Assert.AreEqual("Forth", oComponent.SubComponent(4).AsString, ",oComponent.SubComponent() returned incorrect");
      Assert.AreEqual("First&Second&Third&Forth", oComponent.AsString, ",oComponent.AsString returned incorrect");
      oComponent.RemoveSubComponentAt(4);
      Assert.AreEqual("First&Second&Third", oComponent.AsString, ",oComponent.AsString returned incorrect");
      oComponent.RemoveSubComponentAt(2);
      Assert.AreEqual("First&Third", oComponent.AsString, ",oComponent.AsString returned incorrect");

      oComponent.RemoveSubComponentAt(1);
      oComponent.RemoveSubComponentAt(1);
      Assert.AreEqual("", oComponent.AsString, ",oComponent.AsString returned incorrect");

      oComponent.ClearAll();
      oComponent.Insert(10, Creator.SubComponent("Third"));
      oComponent.Insert(1, Creator.SubComponent("First"));
      oComponent.Insert(2, Creator.SubComponent("Second"));
      Assert.AreEqual("First&Second&&&&&&&&&&Third", oComponent.AsString, ",oComponent.AsString returned incorrect");

      oComponent.ClearAll();
      oComponent.Insert(1, Creator.SubComponent("First"));
      oComponent.Insert(2, Creator.SubComponent("Second"));
      oComponent.Add(Creator.SubComponent("Third"));
      Assert.AreEqual("First&Second&Third", oComponent.AsString, ",oComponent.AsString returned incorrect");

      oComponent.ClearAll();
      oComponent.Add(Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.HighlightOn));
      oComponent.Add(Creator.Content("This is Bold"));
      oComponent.Add(Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.HighlightOff));
      oComponent.Add(Creator.Content(Creator.EscapeData(PeterPiper.Hl7.V2.Support.Standard.EscapeType.Unknown, "HAHAHAHAH")));
      Assert.AreEqual("\\H\\This is Bold\\N\\\\ZHAHAHAHAH\\", oComponent.AsStringRaw, ",oComponent.AsString returned incorrect");
      oComponent.Set(3, Creator.Content(Creator.EscapeData(PeterPiper.Hl7.V2.Support.Standard.EscapeType.Unknown, "Boo")));
      Assert.AreEqual("\\H\\This is Bold\\N\\\\ZBoo\\", oComponent.AsStringRaw, ",oComponent.AsString returned incorrect");
      oComponent.Insert(2, Creator.Content("Not BOLD"));
      Assert.AreEqual("\\H\\This is BoldNot BOLD\\N\\\\ZBoo\\", oComponent.AsStringRaw, ",oComponent.AsString returned incorrect");


      //AsString = "Mater Hospital caters for Women \\Zhildren\\ Men";
      oComponent.ClearAll();
      oComponent.Add(Creator.Content("Mater Hospital", PeterPiper.Hl7.V2.Support.Content.ContentType.Text));
      oComponent.Add(Creator.Content("C4568", PeterPiper.Hl7.V2.Support.Content.ContentType.Escape));
      Assert.AreEqual("Mater Hospital", oComponent.AsString, ",oComponent.AsString returned incorrect");
      Assert.AreEqual("Mater Hospital\\C4568\\", oComponent.AsStringRaw, ",oComponent.AsString returned incorrect");

      oComponent.ClearAll();
      oComponent.AsStringRaw = "Mater Hospital\\Q4568\\new line\\.sp+5\\";
      Assert.AreEqual("Mater Hospitalnew line", oComponent.AsString, ",oComponent.AsString returned incorrect");
      Assert.AreEqual("Mater Hospital\\Q4568\\new line\\.sp+5\\", oComponent.AsStringRaw, ",oComponent.AsStringRaw returned incorrect");
      Assert.AreEqual(true, oComponent.Content(3).EscapeMetaData.IsFormattingCommand, ",.Content(3).EscapeMetaData.IsFormattingCommand returned incorrect");
      Assert.AreEqual(PeterPiper.Hl7.V2.Support.Standard.EscapeType.SkipVerticalSpaces, oComponent.Content(3).EscapeMetaData.EscapeType, "oComponent.Content(3).EscapeMetaData.EscapeType returned incorrect");
      Assert.AreEqual(PeterPiper.Hl7.V2.Support.Standard.Escapes.SkipVerticalSpaces, oComponent.Content(3).EscapeMetaData.EscapeTypeCharater, ",oComponent.Content(3).EscapeMetaData.EscapeTypeCharater returned incorrect");
      Assert.AreEqual("+5", oComponent.Content(3).EscapeMetaData.MetaData, ",.Content(3).EscapeMetaData.MetaData returned incorrect");
    }
  }

  [TestClass]
  public class UnitTestFieldModel
  {
    [TestMethod]
    [TestCategory("Content")]
    public void TestFieldCreate()
    {
      //Test setting data through create
      string TestString = "first^Second^ThirdHasSubs1&ThirdHasSubs2^Forth^\\H\\Fith\\N\\";
      var oField = Creator.Field(TestString);
      Assert.AreEqual(TestString, oField.AsStringRaw, "oField.AsStringRaw did not match create string");

      //Test setting data through StringRaw
      oField.AsStringRaw = "";
      oField.AsStringRaw = "First^Second1&Second2^\\H\\Third2 is Bold\\N\\^forth^fith";
      Assert.AreEqual("First^Second1&Second2^\\H\\Third2 is Bold\\N\\^forth^fith", oField.AsStringRaw, "oField.AsStringRaw did not match Set StringRaw");
      Assert.AreEqual("First^Second1&Second2^Third2 is Bold^forth^fith", oField.AsString, "oComonentWithEscapes.AsString did not match Set StringRaw");

      //Test empty dilimeters are not kept
      oField.AsStringRaw = "^^Third^Forth^^^^^^^^^^^^^^^^^^^^";
      Assert.AreEqual("^^Third^Forth", oField.AsStringRaw, "oField.AsStringRaw did not match Set StringRaw");
      Assert.AreEqual("^^Third^Forth", oField.AsString, "oField.AsString did not match Set StringRaw");

      Assert.AreEqual("Forth", oField.Component(4).ToString(), "oField.Component(4).ToString() did not match Set StringRaw");

      //Test inspect Component does not add Components
      if (oField.Component(100).AsString == "")
      {
        Assert.AreEqual(4, oField.ComponentCount, ",oField.ComponentCount is incorrect");
      }

      //Test zero index exception
      try
      {
        string test = oField.Component(0).AsString;
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Component is a one based index, zero is not a valid index", ae.Message, "Exception should have been thrown on zero index for Component");
      }

      //Test Append
      oField.ClearAll();
      Assert.AreEqual(0, oField.ComponentCount, ",oField.ComponentCount is incorrect");
      oField.Add(Creator.Component("First"));
      oField.Add(Creator.Component("Second"));
      oField.Add(Creator.Component("Third"));
      oField.Add(Creator.Component("Forth"));

      //Test Component lookup
      Assert.AreEqual(4, oField.ComponentCount, ",oComonent.CountSubComponent is incorrect");
      Assert.AreEqual("First", oField.Component(1).AsStringRaw, ",oField.Component(1).AsStringRaw returned incorrect");
      Assert.AreEqual("Second", oField.Component(2).AsString, ",oField.Component(2).AsStringRaw returned incorrect");
      Assert.AreEqual("Third", oField.Component(3).AsStringRaw, ",oField.Component(3).AsStringRaw returned incorrect");
      Assert.AreEqual("Forth", oField.Component(4).AsString, ",oField.Component(4).AsStringRaw returned incorrect");
      Assert.AreEqual("First^Second^Third^Forth", oField.AsString, ",oField.AsString");

      //Test RemoveAt
      oField.RemoveComponentAt(4);
      Assert.AreEqual("First^Second^Third", oField.AsString, ",oField.RemoveComponentAt(4) returned incorrect");
      oField.RemoveComponentAt(2);
      Assert.AreEqual("First^Third", oField.AsString, ",oField.RemoveComponentAt(2) returned incorrect");
      oField.RemoveComponentAt(1);
      oField.RemoveComponentAt(1);
      Assert.AreEqual("", oField.AsString, ",oField.RemoveComponentAt() returned incorrect");

      //Test InsertBefore
      oField.ClearAll();
      oField.Insert(10, Creator.Component("Third"));
      oField.Insert(1, Creator.Component("First"));
      oField.Insert(2, Creator.Component("Second"));
      Assert.AreEqual("First^Second^^^^^^^^^^Third", oField.AsString, ",oField.ComponentInsertBefore returned incorrect");

      //Test InsertBefore
      oField.ClearAll();
      oField.Add(Creator.SubComponent("The SubComponent"));
      Assert.AreEqual("The SubComponent", oField.AsString, ",oField.ComponentInsertBefore returned incorrect");
      oField.Add(Creator.Content(" The Content"));
      Assert.AreEqual("The SubComponent The Content", oField.AsString, ",oField.ComponentInsertBefore returned incorrect");


      //Very interesting scenario, add the same Component to two different parent fields. 
      //The Parent property set to that lass parent the object was added to even though it is now contatined in both parent's ComponentList
      //This is not good but as we currently don't use the Parent property for any functional use it works. 
      //Need to review weather we even need the Parent property at all?
      oField.ClearAll();
      var oComponent = Creator.Component("Test");
      oField.Insert(1, oComponent.Clone());
      oField.Insert(2, oComponent.Clone());
      oField.Add(oComponent.Clone());
      oComponent.AsString = "Test2";
      var oField2 = Creator.Field();
      oField2.Add(oComponent.Clone());

      //Test mixture of Append , insert before and after
      oField.ClearAll();
      oField.Insert(1, Creator.Component("First"));
      oField.Insert(2, Creator.Component("Second"));
      oField.Add(Creator.Component("Third"));
      Assert.AreEqual("First^Second^Third", oField.AsString, ",oField Component insert mix returned incorrect");

      //Test Clone
      var FieldClone = oField.Clone();
      oField.Component(1).AsString = FieldClone.Component(3).AsString;
      oField.Component(2).AsString = FieldClone.Component(2).AsString;
      oField.Component(3).AsString = FieldClone.Component(1).AsString;
      Assert.AreEqual("Third^Second^First", oField.AsString, ",oField.Clone test returned incorrect");
      Assert.AreEqual("First^Second^Third", FieldClone.AsString, ",oField.Clone test2 returned incorrect");

      //Test Custom Delimiters
      var oCustomDelimiters = Creator.MessageDelimiters('!', '@', '*', '%', '#');
      oField = Creator.Field("First*#H#Second#N#*Third1%Third2", oCustomDelimiters);
      Assert.AreEqual("Second", oField.Component(2).AsString, ",CustomDelimiters test returned incorrect");
      Assert.AreEqual("#H#Second#N#", oField.Component(2).AsStringRaw, ",CustomDelimiters test returned incorrect");
      Assert.AreEqual("Third2", oField.Component(3).SubComponent(2).AsStringRaw, ",CustomDelimiters test returned incorrect");



    }
  }

  [TestClass]
  public class UnitTestElementModel
  {
    [TestMethod]
    public void TestFieldCreate()
    {
      //Test setting data through create
      string TestStringEscape = "1first^1Second^1ThirdHasSubs1&1ThirdHasSubs2^1Forth^\\H\\1Fith\\N\\~2first^2Second^2ThirdHasSubs1&2ThirdHasSubs2^2Forth^\\H\\2Fith\\N\\~3first^3Second^3ThirdHasSubs1&3ThirdHasSubs2^3Forth^\\H\\3Fith\\N\\";
      string TestString = "1first^1Second^1ThirdHasSubs1&1ThirdHasSubs2^1Forth^1Fith~2first^2Second^2ThirdHasSubs1&2ThirdHasSubs2^2Forth^2Fith~3first^3Second^3ThirdHasSubs1&3ThirdHasSubs2^3Forth^3Fith";
      var oElement = Creator.Element(TestStringEscape);
      Assert.AreEqual(TestStringEscape, oElement.AsStringRaw, "oElement.AsStringRaw did not match create string");

      //Test setting data through StringRaw
      oElement.AsStringRaw = "";
      oElement.AsStringRaw = TestStringEscape;
      Assert.AreEqual(TestStringEscape, oElement.AsStringRaw, "oElement.AsStringRaw did not match Set StringRaw");
      Assert.AreEqual(TestString, oElement.AsString, "oElement.AsString did not match Set StringRaw");

      //Test empty dilimeters are not kept
      oElement.AsStringRaw = "^^1Third^1Forth~~~^^\\H\\2Third\\N\\^2Forth~~~~~~~~~~~~~~~";
      Assert.AreEqual("^^1Third^1Forth~~~^^\\H\\2Third\\N\\^2Forth", oElement.AsStringRaw, "oElement.AsStringRaw did not match Set StringRaw");
      Assert.AreEqual("^^1Third^1Forth~~~^^2Third^2Forth", oElement.AsString, "oElement.AsString did not match Set StringRaw");

      Assert.AreEqual("^^2Third^2Forth", oElement.Repeat(4).ToString(), "oElement.Repeat(4).ToString() did not match Set StringRaw");

      //Test inspect Component does not add Components
      if (oElement.Repeat(100).AsString == "")
      {
        Assert.AreEqual(4, oElement.RepeatCount, ",oElement.RepeatCount is incorrect");
      }

      //Test zero index exception
      try
      {
        string test = oElement.Repeat(0).AsString;
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Repeat is a one based index, zero is not a valid index", ae.Message, "Exception should have been thrown on zero index for Component");
      }

      //Test Append
      oElement.ClearAll();
      Assert.AreEqual(0, oElement.RepeatCount, ",oElement.RepeatCount is incorrect");
      oElement.Add(Creator.Field("First"));
      oElement.Add(Creator.Field("Second"));
      oElement.Add(Creator.Field("Third"));
      oElement.Add(Creator.Field("Forth"));

      //Test Component lookup
      Assert.AreEqual(4, oElement.RepeatCount, ",oElement.RepeatCount is incorrect");
      Assert.AreEqual("First", oElement.Repeat(1).AsStringRaw, ",oElement.Repeat(1).AsStringRaw returned incorrect");
      Assert.AreEqual("Second", oElement.Repeat(2).AsString, ",oElement.Repeat(2).AsStringRaw returned incorrect");
      Assert.AreEqual("Third", oElement.Repeat(3).AsStringRaw, ",oElement.Repeat(3).AsStringRaw returned incorrect");
      Assert.AreEqual("Forth", oElement.Repeat(4).AsString, ",oElement.Repeat(4).AsStringRaw returned incorrect");
      Assert.AreEqual("First~Second~Third~Forth", oElement.AsString, ",oElement.AsString");

      //Test RemoveAt
      oElement.RemoveRepeatAt(4);
      Assert.AreEqual("First~Second~Third", oElement.AsString, ",oElement.RemoveRepeatAt(4) returned incorrect");
      oElement.RemoveRepeatAt(2);
      Assert.AreEqual("First~Third", oElement.AsString, ",oElement.RemoveRepeatAt(2) returned incorrect");
      oElement.RemoveRepeatAt(1);
      oElement.RemoveRepeatAt(1);
      Assert.AreEqual("", oElement.AsString, ",oElement.RemoveRepeatAt() returned incorrect");

      //Test InsertBefore
      oElement.ClearAll();
      oElement.Add(Creator.Component("The Component"));
      Assert.AreEqual("The Component", oElement.AsString, ",oElement.Add(Creator.Component(); returned incorrect");
      oElement.Add(Creator.Component("The Component2"));
      Assert.AreEqual("The Component^The Component2", oElement.AsString, ",oElement.Add(Creator.Component(); returned incorrect");

      oElement.ClearAll();
      oElement.Add(Creator.SubComponent("The SubComponent1"));
      Assert.AreEqual("The SubComponent1", oElement.AsString, ",oElement.Add(new SubComponent(); returned incorrect");

      oElement.ClearAll();
      oElement.Add(Creator.Content("The Content1"));
      Assert.AreEqual("The Content1", oElement.AsString, ",oElement.Add(Creator.Content(); returned incorrect");


      //Test InsertBefore
      oElement.ClearAll();
      oElement.Insert(20, Creator.Field("Third"));
      oElement.Insert(1, Creator.Field("First"));
      oElement.Insert(2, Creator.Field("Second"));
      Assert.AreEqual("First~Second~~~~~~~~~~~~~~~~~~~~Third", oElement.AsString, ",oElement.RepeatInsertBefore returned incorrect");
      oElement.Insert(5, Creator.Component("Forth"));
      Assert.AreEqual("First^^^^Forth~Second~~~~~~~~~~~~~~~~~~~~Third", oElement.AsString, ",oElement.RepeatInsertBefore returned incorrect");
      oElement.Insert(5, Creator.SubComponent("Five"));
      Assert.AreEqual("First&&&&Five^^^^Forth~Second~~~~~~~~~~~~~~~~~~~~Third", oElement.AsString, ",oElement.RepeatInsertBefore returned incorrect");
      oElement.Insert(5, Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.HighlightOn));
      Assert.AreEqual("First\\H\\&&&&Five^^^^Forth~Second~~~~~~~~~~~~~~~~~~~~Third", oElement.AsStringRaw, ",oElement.RepeatInsertBefore returned incorrect");

      //Test mixture of Append , insert before and after
      oElement.ClearAll();
      oElement.Insert(1, Creator.Field("First"));
      oElement.Insert(2, Creator.Field("Second"));
      oElement.Add(Creator.Field("Third"));
      Assert.AreEqual("First~Second~Third", oElement.AsString, ",oElement Repeat insert mix returned incorrect");

      //Test Clone
      var ElementClone = oElement.Clone();
      oElement.Repeat(1).AsString = ElementClone.Repeat(3).AsString;
      oElement.Repeat(2).AsString = ElementClone.Repeat(2).AsString;
      oElement.Repeat(3).AsString = ElementClone.Repeat(1).AsString;
      Assert.AreEqual("Third~Second~First", oElement.AsString, ",oElement.Clone test returned incorrect");
      Assert.AreEqual("First~Second~Third", ElementClone.AsString, ",oElement.Clone test2 returned incorrect");
      if (oElement.Repeat(1).Component(1).AsString != oElement.Repeat(1).Component(1).SubComponent(1).AsString)
      {
        Assert.Fail("the first component should equal the first SubComponent");
      }

      //Test Custom Delimiters
      var oCustomDelimiters = Creator.MessageDelimiters('!', '@', '*', '%', '#'); //Field, Repeat, Component, SubComponet,Escape
      oElement = Creator.Element("First1*#H#Second1#N#*Third11%Third12@First2*#H#Second2#N#*Third21%Third22", oCustomDelimiters);
      Assert.AreEqual("First2*Second2*Third21%Third22", oElement.Repeat(2).AsString, ",CustomDelimiters test returned incorrect");
      Assert.AreEqual("#H#Second2#N#", oElement.Repeat(2).Component(2).AsStringRaw, ",CustomDelimiters test returned incorrect");
      Assert.AreEqual("Third22", oElement.Repeat(2).Component(3).SubComponent(2).AsStringRaw, ",CustomDelimiters test returned incorrect");
    }
  }

  [TestClass]
  public class UnitTestSegmentModel
  {
    [TestMethod]
    public void TestFieldCreate()
    {
      //Test setting data through create
      string TestStringEscape = "PID||PID-2.1^PID-2.2|PID-3.1.1&PID-3.1.2^PID-3.1|PID-{1}4~PID-{2}4|PID-5.1^\\H\\PID-5.2\\N\\";
      string TestString = "PID||PID-2.1^PID-2.2|PID-3.1.1&PID-3.1.2^PID-3.1|PID-{1}4~PID-{2}4|PID-5.1^PID-5.2";
      var oSegment = Creator.Segment(TestStringEscape);
      Assert.AreEqual(TestStringEscape, oSegment.AsStringRaw, "oSegment.AsStringRaw did not match create string");

      //Test setting data through StringRaw
      oSegment.AsStringRaw = "PID|";
      oSegment.AsStringRaw = TestStringEscape;
      Assert.AreEqual(TestStringEscape, oSegment.AsStringRaw, "oSegment.AsStringRaw did not match Set StringRaw");
      Assert.AreEqual(TestString, oSegment.AsString, "oSegment.AsString did not match Set StringRaw");

      //Test empty dilimeters are not kept
      string TestStringEscape1 = "PID|PID-{1}1~PID-{2}1~PID-{3}1.1^PID-{3}1.2.1&PID-{3}1.2.2&PID-{3}1.2.3~PID-{4}1|PID-2.1^PID-2.2|PID-3.1.1&PID-3.1.2^PID-3.1|PID-{1}4~PID-{2}4|PID-5.1^\\H\\PID-5.2\\N\\||||||||||||||||||||||||||||||||||||";
      string TestStringEscape2 = "PID|PID-{1}1~PID-{2}1~PID-{3}1.1^PID-{3}1.2.1&PID-{3}1.2.2&PID-{3}1.2.3~PID-{4}1|PID-2.1^PID-2.2|PID-3.1.1&PID-3.1.2^PID-3.1|PID-{1}4~PID-{2}4|PID-5.1^\\H\\PID-5.2\\N\\";
      TestString = "PID|PID-{1}1~PID-{2}1~PID-{3}1.1^PID-{3}1.2.1&PID-{3}1.2.2&PID-{3}1.2.3~PID-{4}1|PID-2.1^PID-2.2|PID-3.1.1&PID-3.1.2^PID-3.1|PID-{1}4~PID-{2}4|PID-5.1^PID-5.2";

      oSegment.AsStringRaw = TestStringEscape1;
      Assert.AreEqual(TestStringEscape2, oSegment.AsStringRaw, "oSegment.AsStringRaw did not match Set StringRaw");
      Assert.AreEqual(TestString, oSegment.AsString, "oSegment.AsString did not match Set StringRaw");

      Assert.AreEqual("PID-5.1^PID-5.2", oSegment.Field(5).ToString(), "oSegment.Field(3).ToString() did not match Set StringRaw");
      Assert.AreEqual("PID-{2}4", oSegment.Element(4).Repeat(2).ToString(), "oSegment.Field(3).ToString() did not match Set StringRaw");
      Assert.AreEqual("PID-3.1.1&PID-3.1.2^PID-3.1", oSegment.Element(3).AsString, "oSegment.Field(3).ToString() did not match Set StringRaw");
      Assert.AreEqual("PID-3.1.2", oSegment.Field(3).Component(1).SubComponent(2).AsString, "oSegment.Field(3).ToString() did not match Set StringRaw");
      Assert.AreEqual("PID-3.1.1&PID-3.1.2", oSegment.Element(3).Repeat(1).Component(1).AsStringRaw, "oSegment.Field(3).ToString() did not match Set StringRaw");
      Assert.AreEqual("PID-{3}1.2.3", oSegment.Element(1).Repeat(3).Component(2).SubComponent(3).AsStringRaw, "oSegment.Field(3).ToString() did not match Set StringRaw");

      //##Issues## This test fails, but notice the test below it does not!!!
      ////Test inspect Field does not add fields & elements
      //if (oSegment.Field(100).AsString == "")
      //{
      //Assert.AreEqual(5, oSegment.ElementCount, "oSegment.CountElement is incorrect");
      //}

      //Test inspect Element does not add Elements, so if you need to test a field 
      //for emptiness then use Element. 
      if (oSegment.Element(100).AsString == "")
      {
        Assert.AreEqual(5, oSegment.ElementCount, "oSegment.CountElement is incorrect");
      }

      //Test zero index exception
      try
      {
        string test = oSegment.Element(0).AsString;
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Element index is a one based index, zero in not allowed", ae.Message, "Exception should have been thrown on zero index for Element");
      }

      //Test Append
      oSegment.ClearAll();
      Assert.AreEqual(0, oSegment.ElementCount, "oSegment.RepeatCount is incorrect");
      oSegment.Add(Creator.Element("First"));
      oSegment.Add(Creator.Element("Second"));
      oSegment.Add(Creator.Element("Third"));
      oSegment.Add(Creator.Element("Forth"));

      ////Test Component lookup
      Assert.AreEqual(4, oSegment.ElementCount, "oSegment.RepeatCount is incorrect");
      Assert.AreEqual("First", oSegment.Element(1).AsStringRaw, "oSegment.Element(1).AsStringRaw returned incorrect");
      Assert.AreEqual("First", oSegment.Field(1).AsStringRaw, "oSegment.Field(1).AsStringRaw returned incorrect");
      Assert.AreEqual("Second", oSegment.Element(2).AsString, "oSegment.Element(2).AsString returned incorrect");
      Assert.AreEqual("Second", oSegment.Field(2).AsString, "oSegment.Field(2).AsString returned incorrect");
      Assert.AreEqual("Third", oSegment.Element(3).AsStringRaw, "oSegment.Element(3).AsStringRaw returned incorrect");
      Assert.AreEqual("Third", oSegment.Field(3).AsStringRaw, "oSegment.Field(3).AsStringRaw returned incorrect");
      Assert.AreEqual("Forth", oSegment.Element(4).AsString, "oSegment.Element(4).AsString returned incorrect");
      Assert.AreEqual("Forth", oSegment.Field(4).AsString, "oSegment.Field(4).AsString returned incorrect");
      Assert.AreEqual("PID|First|Second|Third|Forth", oSegment.AsString, "oSegment.AsString");

      ////Test RemoveAt
      oSegment.RemoveElementAt(4);
      Assert.AreEqual("PID|First|Second|Third", oSegment.AsString, "oSegment.RemoveRepeatAt(4) returned incorrect");
      oSegment.RemoveElementAt(2);
      Assert.AreEqual("PID|First|Third", oSegment.AsString, "oSegment.RemoveRepeatAt(2) returned incorrect");
      oSegment.RemoveElementAt(1);
      Assert.AreEqual("PID|Third", oSegment.AsString, "oSegment.RemoveRepeatAt() returned incorrect");
      oSegment.RemoveElementAt(1);
      Assert.AreEqual("PID|", oSegment.AsString, "oSegment.RemoveRepeatAt() returned incorrect");

      ////Test InsertAfter
      oSegment.Insert(1, Creator.Element("First1~First2"));
      oSegment.Insert(2, Creator.Element("Third"));
      oSegment.Insert(2, Creator.Element("Second"));
      Assert.AreEqual("PID|First1~First2|Second|Third", oSegment.AsString, "oSegment.RepeatInsertAfter returned incorrect");

      ////Test InsertBefore
      oSegment.ClearAll();
      oSegment.Insert(10, Creator.Element("Third"));
      oSegment.Insert(1, Creator.Element("First1~First2"));
      oSegment.Insert(2, Creator.Element("Second"));
      oSegment.Insert(1, Creator.Element(""));
      oSegment.Insert(1, Creator.Element(""));
      Assert.AreEqual("PID|||First1~First2|Second||||||||||Third", oSegment.AsString, "oSegment.RepeatInsertBefore returned incorrect");

      //Very interesting scenario, add the same Component to two different parent fields. 
      //The Parent property of the Component can only have one parent and is set to the last known even though it is now contained in both parent's ComponentList
      //This was not allowed to happen. I have implemented a base .Clone method which needed to be done anyway and also 
      //now throw an exception for this case as tested below.
      //End story is users must clone object instances to use them in another structure. I think this 
      //is reasonable 

      oSegment.ClearAll();
      var oElement = Creator.Element("Test");
      oSegment.Insert(1, oElement);
      try
      {
        oSegment.Insert(2, oElement);
      }
      catch (ArgumentException e)
      {
        Assert.AreEqual("The object instance passed is in use within another structure. This is not allowed. Have you forgotten to Clone() the instance before reusing.",
                        e.Message, "Exception thrown has wrong text");
      }
      oSegment.Insert(2, oElement.Clone());
      oSegment.Add(oElement.Clone());
      oElement.AsString = "Test2";
      var oSegment2 = Creator.Segment("ROL|");
      oSegment2.Add(oElement.Clone());
      string teeeest = oSegment.Field(1).PathDetail.PathBrief;

      ////Test Adding and removing Fields on segment instance
      oSegment.ClearAll();
      oSegment.AsStringRaw = "PID|1|2|3|4|5|6";
      oSegment.Add(Creator.Field("NewField1"));
      Assert.AreEqual("PID|1|2|3|4|5|6|NewField1", oSegment.AsString, "oSegment.Add() returned incorrect");
      oSegment.Insert(6, Creator.Field("NewField2"));
      Assert.AreEqual("PID|1|2|3|4|5|NewField2|6|NewField1", oSegment.AsString, "oSegment.Add() returned incorrect");


      ////Test mixture of Append , insert before and after
      oSegment.ClearAll();
      oSegment.Insert(1, Creator.Element("First"));
      oSegment.Insert(2, Creator.Element("Second"));
      oSegment.Add(Creator.Element("Third"));
      Assert.AreEqual("PID|First|Second|Third", oSegment.AsString, "oSegment insert mix returned incorrect");

      ////Test Clone
      var SegmentClone = oSegment.Clone();
      oSegment.Element(1).AsString = SegmentClone.Element(3).AsString;
      oSegment.Element(2).AsString = SegmentClone.Element(2).AsString;
      oSegment.Element(3).AsString = SegmentClone.Element(1).AsString;
      Assert.AreEqual("PID|Third|Second|First", oSegment.AsString, "oSegment.Clone test returned incorrect");
      Assert.AreEqual("PID|First|Second|Third", SegmentClone.AsString, "oSegment.Clone test2 returned incorrect");
      Assert.AreEqual(oSegment.Field(1).AsString, oSegment.Element(1).Repeat(1).Component(1).SubComponent(1).AsString, "The first Field / Element should equal the first SubComponent");

      SegmentClone.Element(3).ClearAll();
      Assert.AreEqual("PID|First|Second", SegmentClone.AsString, "oSegment.Clone test2 returned incorrect");

      SegmentClone.Element(2).AsString = String.Empty;
      Assert.AreEqual("PID|First", SegmentClone.AsString, "oSegment.Clone test2 returned incorrect");

      SegmentClone.Element(2).Component(3).SubComponent(10).AsString = "Hi";
      Assert.AreEqual("PID|First|^^&&&&&&&&&Hi", SegmentClone.AsString, "oSegment.Clone test2 returned incorrect");

      SegmentClone.Element(2).Component(3).SubComponent(10).Content(0).ClearAll();
      Assert.AreEqual("PID|First", SegmentClone.AsString, "oSegment.Clone test2 returned incorrect");

      SegmentClone.Element(2).Component(3).SubComponent(10).AsString = "Hi2";
      SegmentClone.Element(2).Component(2).ClearAll();
      Assert.AreEqual("PID|First|^^&&&&&&&&&Hi2", SegmentClone.AsString, "oSegment.Clone test2 returned incorrect");

      SegmentClone.Element(1).Component(1).SubComponent(1).ClearAll();
      Assert.AreEqual("PID||^^&&&&&&&&&Hi2", SegmentClone.AsString, "oSegment.Clone test2 returned incorrect");

      SegmentClone.Field(2).ClearAll();
      Assert.AreEqual("PID|", SegmentClone.AsString, "oSegment.Clone test2 returned incorrect");


      ////Test Custom Delimiters
      var oCustomDelimiters = Creator.MessageDelimiters('!', '@', '*', '%', '#'); //Field, Repeat, Component, SubComponet,Escape
      oSegment = Creator.Segment("PID!First1*#H#Second1#N#*Third11%Third12@First2*#H#Second2#N#*Third21%Third22", oCustomDelimiters);
      Assert.AreEqual("First1*Second1*Third11%Third12", oSegment.Field(1).AsString, ",CustomDelimiters test returned incorrect");
      Assert.AreEqual("#H#Second2#N#", oSegment.Element(1).Repeat(2).Component(2).AsStringRaw, ",CustomDelimiters test returned incorrect");
      Assert.AreEqual("Third22", oSegment.Element(1).Repeat(2).Component(3).SubComponent(2).AsStringRaw, ",CustomDelimiters test returned incorrect");
    }
  }

  [TestClass]
  public class UnitTestMessageModel
  {
    [TestMethod]
    public void TestMessageCreate()
    {
      var oMessage = Creator.Message("2.3.1", "ORU", "R01");

      StringBuilder sbMessageWithTwoMSHSegments = new StringBuilder();
      sbMessageWithTwoMSHSegments.Append("MSH|^~\\&|HNAM^RADNET|PAH^00011|IMPAX-CV|QH|20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1|||AL|NE|AU|8859/1|EN"); sbMessageWithTwoMSHSegments.Append("\r");
      sbMessageWithTwoMSHSegments.Append("PID|1|1038785005^^^QH^PT^CD&A^^\"\"|1038785005^^^QH^PT^CD&A^^\"\"~993171^^^QH^MR^PAH&A^^\"\"~343211^^^QH^MR^LOGH&A^^\"\"~43028819141^^^HIC^MC^^^10/2018~\"\"^^^DVA^VA^^\"\"~420823031C^^^HIC^PEN&9^^^31/07/2015~\"\"^^^HIC^HC^^\"\"~\"\"^^^HIC^SN^^\"\"|993171-PAH^^^^MR^PAH|KABONGO^KABEDI^^^MS^^C||19520725|F||42^Not Aborig. or Torres Strait Is. ,Not a South Sea Islander|24 Holles Street^^WATERFORD WEST^^4133||(046)927-3235^Home|(046)927-3235^Business|78|3^Widowed|2999^Other Christian, nec|1504352845^^^PAH FIN Number Alias Pool^FIN NBR|43028819141||||||0|||||N"); sbMessageWithTwoMSHSegments.Append("\r");
      sbMessageWithTwoMSHSegments.Append("MSH|^~\\&|HNAM^RADNET|PAH^00011|IMPAX-CV|QH|20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1|||AL|NE|AU|8859/1|EN"); sbMessageWithTwoMSHSegments.Append("\r");
      try
      {
        oMessage = Creator.Message(sbMessageWithTwoMSHSegments.ToString());
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Second MSH segment found when parsing new message. Single messages must only have one MSH segment as the first segment.", ae.Message, "Exception should have been thrown in setting oMessage.AsString");
      }

      StringBuilder sbMessage = new StringBuilder();
      sbMessage.Append("MSH|^~\\&|HNAM^RADNET|PAH^00011|IMPAX-CV|QH|20141208064531||ORM^O01^ORM_O01|Q54356818T82744882|P|2.3.1|||AL|NE|AU|8859/1|EN"); sbMessage.Append("\r");
      sbMessage.Append("PID|1|1234567890^^^QH^PT^CD&A^^\"\"|1234567890^^^QH^PT^CD&A^^\"\"~345678^^^QH^MR^PAH&A^^\"\"~010101^^^QH^MR^LOGH&A^^\"\"~00000000001^^^HIC^MC^^^10/2018~\"\"^^^DVA^VA^^\"\"~123456789C^^^HIC^PEN&9^^^31/07/2015~\"\"^^^HIC^HC^^\"\"~\"\"^^^HIC^SN^^\"\"|001122-PAH^^^^MR^PAH|DummySurname^DummyGiven^^^MS^^C||19520725|F||42^Not Aborig. or Torres Strait Is. ,Not a South Sea Islander|24 Holles Street^^WATERFORD WEST^^4133||(046)927-3235^Home|(046)927-3235^Business|78|3^Widowed|2999^Other Christian, nec|1504352845^^^PAH FIN Number Alias Pool^FIN NBR|43028819141||||||0|||||N"); sbMessage.Append("\r");
      sbMessage.Append("PV1|1|I|||||1^SMITH^John^^^^^^PAH External ID Alias Pool^CD:369232^^^External Identifier~1106^SMITH^JOHN^^^^^^PAH Organisation Dr Number Alias Pool^CD:123456^^^ORGANIZATION DOCTOR||1106^SMITH^JOHN^^^^^^PAH External ID Alias Pool^CD:123456^^^External Identifier~1106^SMITH^JOHN^^^^^^PAH Organisation Dr Number Alias Pool^CD:123456^^^ORGANIZATION DOCTOR||||||||||1234567890^^^PAH Visit ID Alias Pool^Visit Id||||||||||||||||||||PAHDHS||Active|||20141127134500"); sbMessage.Append("\r");
      sbMessage.Append("PV2|||^smith|||||||0|||||||||||||^^294882"); sbMessage.Append("\r");
      sbMessage.Append("IN1|1|295388^109|3060132|109||||||||20141127134612|21001231000000|||DummySurname^DummyGiven^^^MS^^C|CD:158|19520725|1 SOMEWHERE STREET^^OVERTHERE WEST^^4133~~~^^^^^9106~9107^^^^^9106|||0|||||||||||||||||||||F"); sbMessage.Append("\r");
      sbMessage.Append("IN2|"); sbMessage.Append("\r");
      sbMessage.Append("ORC|CA|25486076^HNAM_ORDERID|||OC||||20141208064530|123456^DOA^JANE^^^^^^PAH Prsnl ID Alias Pool^CD:369232^^^PRSNLID||1234^SMITH^JOHN^^^^^^PAH External ID Alias Pool^CD:123456^^^External Identifier~1234^SMITH^JOHN^^^^^^PAH Organisation Dr Number Alias Pool^CD:12345^^^ORGANIZATION DOCTOR|PAHDHS||20141208064530|CD:123456^ADMINISTRATION CANCELLATION||CD:2562^Written|123456^DOA^JANE^^^^^^PAH Prsnl ID Alias Pool^CD:123456^^^PRSNLID"); sbMessage.Append("\r");
      sbMessage.Append("OBR|1|25486076^HNAM_ORDERID||CI EP IMPLANT Dv^CI EP Implantable Device|||||||||||Rad Type&Rad Type|1234^SMITH^JOHN^^^^^^PAH External ID Alias Pool^CD:123456^^^External Identifier||CI12345678-PAH|PAH-CI|||||CI|||1^^0^20141205110000^^R"); sbMessage.Append("\r");
      sbMessage.Append("OBX|1|IS|CD:1278500^RADIOLOGY INPATIENT / OUTPATIENT||CD:1"); sbMessage.Append("\r");
      sbMessage.Append("OBX|2|IS|CD:1278500^RADIOLOGY INPATIENT / OUTPATIENT||CD:2"); sbMessage.Append("\r");
      sbMessage.Append("OBX|3|IS|CD:1278500^RADIOLOGY INPATIENT / OUTPATIENT||CD:3"); sbMessage.Append("\r");
      sbMessage.Append("OBX|4|IS|CD:1278500^RADIOLOGY INPATIENT / OUTPATIENT||CD:4"); sbMessage.Append("\r");

      string MessageString = sbMessage.ToString();
      oMessage = Creator.Message(MessageString);
      //Check parsed Message same as Original
      Assert.AreEqual(sbMessage.ToString(), oMessage.AsStringRaw, "Parsed Message is not equal to orginal message.");

      //test IsEmpty property
      Assert.AreEqual(true, oMessage.Segment("IN2").IsEmpty, "oMessage.Segment().IsEmpty returns the incorrect value.");
      Assert.AreEqual(true, oMessage.Segment("PID").Element(50).IsEmpty, "oMessage.Segment().Element(50).IsEmpty returns the incorrect value.");
      Assert.AreEqual(false, oMessage.Segment("PID").Element(1).IsEmpty, "oMessage.Segment().Element(1).IsEmpty returns the incorrect value.");
      Assert.AreEqual(true, oMessage.Segment("PID").Element(1).Component(2).IsEmpty, "oMessage.Segment().Element(1).Component(2).IsEmpty returns the incorrect value.");
      Assert.AreEqual(false, oMessage.Segment("PID").Element(2).Component(4).IsEmpty, "oMessage.Segment().Element(2).Component(4).IsEmpty returns the incorrect value.");
      Assert.AreEqual(false, oMessage.Segment("PID").Element(2).Component(6).SubComponent(2).IsEmpty, "oMessage.Segment().Element(2).Component(6).SubComponent(2).IsEmpty returns the incorrect value.");
      Assert.AreEqual(true, oMessage.Segment("PID").Element(2).Component(6).SubComponent(3).IsEmpty, "oMessage.Segment().Element(2).Component(6).SubComponent(2).IsEmpty returns the incorrect value.");
      Assert.AreEqual(false, oMessage.Segment("PID").Element(2).Component(6).SubComponent(2).Content(0).IsEmpty, "oMessage.Segment().Element(2).Component(6).SubComponent(2).Content(0).IsEmpty returns the incorrect value.");
      Assert.AreEqual(true, oMessage.Segment("PID").Element(2).Component(6).SubComponent(3).Content(2).IsEmpty, "oMessage.Segment().Element(2).Component(6).SubComponent(3).Content(2).IsEmpty returns the incorrect value.");

      //oMessage.
      //Some Time & Date Tests
      DateTimeOffset testDateTime = oMessage.Segment("MSH").Field(7).Convert.DateTime.GetDateTimeOffset();
      oMessage.Segment("MSH").Field(7).Convert.DateTime.SetDateTimeOffset(DateTimeOffset.Now);
      testDateTime = oMessage.Segment("MSH").Field(7).Convert.DateTime.GetDateTimeOffset();
      DateTimeOffset testDateTime3 = oMessage.Segment("MSH").Field(7).Convert.DateTime.GetDateTimeOffset();

      Assert.AreEqual(12, oMessage.SegmentCount(), "The oMessage.CountSegment returns the incorrect value.");
      Assert.AreEqual(4, oMessage.SegmentList("OBX").Count, "The oMessage.SegmentList(\"OBX\").Count returns the incorrect value.");
      Assert.AreEqual("CD:4", oMessage.SegmentList("OBX")[3].Element(5).Repeat(1).AsString, "oMessage.SegmentList(\"OBX\")[3].Element(5).Repeat(1).AsString returns the incorrect value.");
      Assert.AreEqual("PT", oMessage.Segment("PID").Field(3).Component(5).AsString, "oMessage.Segment(\"PID\").Field(3).Component(5).AsString returns the incorrect value.");
      Assert.AreEqual("A", oMessage.Segment("PID").Element(3).Repeat(2).Component(6).SubComponent(2).AsString, "oMessage.Segment(\"PID\").Field(3).Component(5).AsString returns the incorrect value.");
      Assert.AreEqual(2, oMessage.Segment("PID").Element(3).Repeat(2).Component(6).SubComponentCount, "oMessage.Segment(\"PID\").Element(3).Repeat(2).Component(6).CountSubComponent returns the incorrect value.");
      Assert.AreEqual(8, oMessage.Segment("PID").Element(3).RepeatCount, "oMessage.Segment(\"PID\").Element(3).RepeatCount returns the incorrect value.");
      Assert.AreEqual("\"\"", oMessage.Segment("PID").Field(2).Component(8).AsStringRaw, "oMessage.Segment(\"PID\").Field(2).Component(8).AsStringRaw returns the incorrect value.");
      Assert.AreEqual(true, oMessage.Segment("PID").Field(2).Component(8).SubComponent(1).IsHL7Null, "oMessage.Segment(\"PID\").Field(2).Component(8).SubComponent(1).IsHL7Null returns the incorrect value.");
      Assert.AreEqual(true, oMessage.Segment("PID").Field(2).Component(8).IsHL7Null, "oMessage.Segment(\"PID\").Field(2).Component(8).IsHL7Null returns the incorrect value.");

      oMessage.ClearAll();
      Assert.AreEqual(1, oMessage.SegmentCount(), "The oMessage.CountSegment returns the incorrect value.");
      Assert.AreEqual(0, oMessage.SegmentList("OBX").Count, "The oMessage.SegmentList(\"OBX\").Count returns the incorrect value.");
      Assert.AreEqual(1, oMessage.SegmentList("MSH").Count, "The oMessage.SegmentList(\"MSH\").Count returns the incorrect value.");

      oMessage.AsStringRaw = MessageString;
      Assert.AreEqual(12, oMessage.SegmentCount(), "The oMessage.CountSegment returns the incorrect value.");
      Assert.AreEqual(4, oMessage.SegmentList("OBX").Count, "The oMessage.SegmentList(\"OBX\").Count returns the incorrect value.");
      Assert.AreEqual("CD:4", oMessage.SegmentList("OBX")[3].Element(5).Repeat(1).AsString, "oMessage.SegmentList(\"OBX\")[3].Element(5).Repeat(1).AsString returns the incorrect value.");
      Assert.AreEqual("PT", oMessage.Segment("PID").Field(3).Component(5).AsString, "oMessage.Segment(\"PID\").Field(3).Component(5).AsString returns the incorrect value.");
      Assert.AreEqual("A", oMessage.Segment("PID").Element(3).Repeat(2).Component(6).SubComponent(2).AsString, "oMessage.Segment(\"PID\").Field(3).Component(5).AsString returns the incorrect value.");
      Assert.AreEqual(2, oMessage.Segment("PID").Element(3).Repeat(2).Component(6).SubComponentCount, "oMessage.Segment(\"PID\").Element(3).Repeat(2).Component(6).CountSubComponent returns the incorrect value.");
      Assert.AreEqual(8, oMessage.Segment("PID").Element(3).RepeatCount, "oMessage.Segment(\"PID\").Element(3).RepeatCount returns the incorrect value.");
      Assert.AreEqual("\"\"", oMessage.Segment("PID").Field(2).Component(8).AsStringRaw, "oMessage.Segment(\"PID\").Field(2).Component(8).AsStringRaw returns the incorrect value.");
      Assert.AreEqual(true, oMessage.Segment("PID").Field(2).Component(8).SubComponent(1).IsHL7Null, "oMessage.Segment(\"PID\").Field(2).Component(8).SubComponent(1).IsHL7Null returns the incorrect value.");
      Assert.AreEqual(true, oMessage.Segment("PID").Field(2).Component(8).IsHL7Null, "oMessage.Segment(\"PID\").Field(2).Component(8).IsHL7Null returns the incorrect value.");

      oMessage.ClearAll();
      try
      {
        oMessage.AsString = MessageString;
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("While setting a message using AsString() could technically work it would make no sense to have the message's escape characters in MSH-1 & MSH-2 re-escaped. You should be using AsStringRaw()", ae.Message, "Exception should have been thrown in setting oMessage.AsString");
      }
      var oMesage2 = Creator.Message(MessageString);
      oMessage = oMesage2.Clone();
      Assert.AreEqual(12, oMessage.SegmentCount(), "The oMessage.CountSegment returns the incorrect value.");
      Assert.AreEqual(4, oMessage.SegmentList("OBX").Count, "The oMessage.SegmentList(\"OBX\").Count returns the incorrect value.");
      Assert.AreEqual("CD:4", oMessage.SegmentList("OBX")[3].Element(5).Repeat(1).AsString, "oMessage.SegmentList(\"OBX\")[3].Element(5).Repeat(1).AsString returns the incorrect value.");
      Assert.AreEqual("PT", oMessage.Segment("PID").Field(3).Component(5).AsString, "oMessage.Segment(\"PID\").Field(3).Component(5).AsString returns the incorrect value.");
      Assert.AreEqual("A", oMessage.Segment("PID").Element(3).Repeat(2).Component(6).SubComponent(2).AsString, "oMessage.Segment(\"PID\").Field(3).Component(5).AsString returns the incorrect value.");
      Assert.AreEqual(2, oMessage.Segment("PID").Element(3).Repeat(2).Component(6).SubComponentCount, "oMessage.Segment(\"PID\").Element(3).Repeat(2).Component(6).CountSubComponent returns the incorrect value.");
      Assert.AreEqual(8, oMessage.Segment("PID").Element(3).RepeatCount, "oMessage.Segment(\"PID\").Element(3).RepeatCount returns the incorrect value.");
      Assert.AreEqual("\"\"", oMessage.Segment("PID").Field(2).Component(8).AsStringRaw, "oMessage.Segment(\"PID\").Field(2).Component(8).AsStringRaw returns the incorrect value.");
      Assert.AreEqual(true, oMessage.Segment("PID").Field(2).Component(8).SubComponent(1).IsHL7Null, "oMessage.Segment(\"PID\").Field(2).Component(8).SubComponent(1).IsHL7Null returns the incorrect value.");
      Assert.AreEqual(true, oMessage.Segment("PID").Field(2).Component(8).IsHL7Null, "oMessage.Segment(\"PID\").Field(2).Component(8).IsHL7Null returns the incorrect value.");

      int Counter = 1;
      foreach (var OBX in oMessage.SegmentList("OBX"))
      {
        OBX.Field(1).AsString = Counter.ToString();
        Counter++;
      }
      for (int i = 0; i < oMessage.SegmentList("OBX").Count; i++)
      {
        Assert.AreEqual((i + 1).ToString(), oMessage.SegmentList("OBX")[i].Field(1).AsString, "oMessage.SegmentList(\"OBX\")[i].Field(1).AsString returns the incorrect value.");
      }

      var oNewSegment = Creator.Segment("GUS|hello^World");
      oMessage.Add(oNewSegment);
      Assert.AreEqual(13, oMessage.SegmentCount(), "The oMessage.CountSegment returns the incorrect value.");
      Assert.AreEqual("GUS", oMessage.SegmentList()[12].Code, "oMessage.SegmentList()[12].Code");

      var oNewSegment2 = Creator.Segment("NTE|Comment here 2");
      oMessage.Insert(9, oNewSegment2);
      Assert.AreEqual(14, oMessage.SegmentCount(), "The oMessage.CountSegment returns the incorrect value.");
      Assert.AreEqual("NTE", oMessage.Segment(9).Code, "oMessage.Segment(9).Code");
      Assert.AreEqual("Comment here 2", oMessage.Segment(9).Field(1).AsString, "oMessage.Segment(9).Field(1).AsString");

      var oNewSegment3 = Creator.Segment("NTE|Comment here 3");
      oMessage.Insert(9, oNewSegment3);
      Assert.AreEqual(15, oMessage.SegmentCount(), "The oMessage.CountSegment returns the incorrect value.");
      Assert.AreEqual("NTE", oMessage.Segment(9).Code, "oMessage.SegmentList()[12].Code");
      Assert.AreEqual("Comment here 3", oMessage.Segment(9).Field(1).AsString, "oMessage.Segment(9).Field(1).AsString");

      if (oMessage.RemoveSegmentAt(9))
      {
        Assert.AreEqual("Comment here 2", oMessage.Segment(9).Field(1).AsString, "oMessage.Segment(9).Field(1).AsString");
      }
      else
      {
        Assert.Fail("oMessage.RemoveSegmentAt(9) should have returned True");
      }

      var oDelim = Creator.MessageDelimiters('*', '~', '^', '&', '\\');
      var oMSHSeg = Creator.Segment("MSH*^~\\&*SUPERLIS*QHPS*EGATE-Atomic*CITEC*20140804143827**ORU^R01*000000000000005EVT6P*P*2.3.1*", oDelim);
      var oMessage3 = Creator.Message(oMSHSeg);
      var oPIDSeg = Creator.Segment("PID|1|1016826143^^^QH^PT^CD&A^^\"\"|1016826143^^^QH^PT^CD&A^^\"\"~103647^^^QH^MR^TPCH&A^^\"\"~299059^^^QH^MR^PAH&A^^\"\"~165650^^^QH^MR^IPSH&A^^\"\"~297739^^^QH^MR^LOGH&A^^\"\"~B419580^^^QH^MR^RBWH&A^^\"\"~40602113521^^^HIC^MC^^^10/2015~\"\"^^^DVA^VA^^\"\"~NP^^^HIC^PEN&9^^^\"\"~\"\"^^^HIC^HC^^\"\"~\"\"^^^HIC^SN^^\"\"|299059-PAH^^^^MR^PAH|EDDING^WARREN^EVAN^^MR^^C||19520812|M||42^Not Aborig. or Torres Strait Is. ,Not a South Sea Islander|7 Colvin Street^^NORTH IPSWICH^^4305||(042)242-9139^Home|(042)242-9139^Business|CD:301058|4^Divorced|7010^No Religion, NFD|1504350552^^^PAH FIN Number Alias Pool^FIN NBR|40602113521||||||0|||||N");
      try
      {
        oMessage3.Add(oPIDSeg);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("The Segment instance being added to this parent Message instance has custom delimiters that are different than the parent, this is not allowed", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage3.RemoveSegmentAt(1);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Index one is the MSH Segment. This segment can not be removed, it can be modified or a new Message instance can be created", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      oMessage = Creator.Message(MessageString);

      Assert.AreEqual("|", oMessage.MainSeparator, "oMessage.MainSeparator returns the incorrect value");
      Assert.AreEqual("^~\\&", oMessage.EscapeSequence, "oMessage.MessageDelimiters returns the incorrect value");
      //string testr = oMessage.Segment(1).Field(2).AsString;


      StringBuilder sbMSH2Exception = new StringBuilder();
      sbMSH2Exception.Append("MSH-2 contains the 'Encoding Characters' and is not accessible from the Field or Element object instance as it is critical to message construction.");
      sbMSH2Exception.Append(Environment.NewLine);
      sbMSH2Exception.Append("Instead, you can access the read only property call 'EscapeSequence' from the Message object instance.");

      StringBuilder sbMSH1Exception = new StringBuilder();
      sbMSH1Exception.Append("MSH-1 contains the 'Main Separator' character and is not accessible from the Field or Element object instance as it is critical to message construction.");
      sbMSH1Exception.Append(Environment.NewLine);
      sbMSH1Exception.Append("Instead, you can access the read only property call 'MainSeparator' from the Message object instance.");

      try
      {
        oMessage.Segment(1).Element(1).Repeat(1).Component(1).AsString = "TestData";
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual(sbMSH1Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage.Segment(1).Field(2).AsString = "TestData";
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual(sbMSH2Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage.Segment(1).RemoveElementAt(1);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual(sbMSH1Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage.Segment(1).RemoveElementAt(2);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual(sbMSH2Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage.Segment(1).Insert(1, Creator.Element("sffsfsdfAfter"));
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual(sbMSH1Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage.Segment(1).Insert(1, Creator.Element("sffsfsdfBefore"));
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual(sbMSH1Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage.Segment(1).Element(2).Repeat(1).AsString = "Done";
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual(sbMSH2Exception.ToString(), ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage.Segment(1).AsString = "ABC|^~\\&|SUPERLIS|TRAIN|";
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Unable to modify an existing MSH segment instance with the AsString or AsStringRaw properties. /n You need to create a new Segment instance and use it's constructor or selectively edit this segment's parts.", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage.Segment(1).AsStringRaw = "ABC|^~\\&|SUPERLIS|TRAIN|";
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Unable to modify an existing MSH segment instance with the AsString or AsStringRaw properties. /n You need to create a new Segment instance and use it's constructor or selectively edit this segment's parts.", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        var testSeg = Creator.Segment("BAD");
        testSeg.AsStringRaw = "MSH|^~\\&|SUPERLIS|TRAIN|EGATE-Atomic^prjSUPERLISIn|EMR|20140526095519||ORU^R01|000000000000000000ZN|P|2.3.1";
        testSeg.AsStringRaw = "ABC|^~\\&|SUPERLIS|TRAIN|";
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("Unable to modify an existing MSH segment instance with the AsString or AsStringRaw properties. /n You need to create a new Segment instance and use it's constructor or selectively edit this segment's parts.", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      var NewMSH = Creator.Segment("MSH|^~\\&|SUPERLIS|TRAIN|EGATE-Atomic^prjSUPERLISIn|EMR|20140526095519||ORU^R01|000000000000000000ZN|P|2.3.1");
      try
      {
        oMessage.Add(NewMSH);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("An MSH Segment can not be added to a Message instance, it must be provided on Message instance creation / instantiation", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage.Insert(2, NewMSH);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("An MSH Segment can not be added to a Message instance, it must be provided on Message instance creation / instantiation", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      try
      {
        oMessage.Insert(1, NewMSH);
        Assert.Fail("An exception should have been thrown");
      }
      catch (PeterPiper.Hl7.V2.CustomException.PeterPiperException ae)
      {
        Assert.AreEqual("An MSH Segment can not be added to a Message instance, it must be provided on Message instance creation / instantiation", ae.Message, "Exception should have been thrown due to CustomDelimiters not matching");
      }

      //Check Component List works correctly
      oPIDSeg = Creator.Segment("PID|1|1234567890^^^QH^PT^CD&A^^\"\"|1234567890^^^QH^PT^CD&A^^\"\"~123456^^^QH^MR^TPCH&A^^\"\"~123456^^^QH^MR^PAH&A^^\"\"~123456^^^QH^MR^IPSH&A^^\"\"~123456^^^QH^MR^LOGH&A^^\"\"~B123456^^^QH^MR^RBWH&A^^\"\"~12345678901^^^HIC^MC^^^10/2015~\"\"^^^DVA^VA^^\"\"~NP^^^HIC^PEN&9^^^\"\"~\"\"^^^HIC^HC^^\"\"~\"\"^^^HIC^SN^^\"\"|299059-PAH^^^^MR^PAH|DUMMYSURNAME^DUMMYGIVEN^EVAN^^MR^^C||19520812|M||42^Not Aborig. or Torres Strait Is. ,Not a South Sea Islander|7 Where Street^^NORTH HERE^^1234||(042)242-0000^Home|(042)242-0000^Business|CD:301058|4^Divorced|7010^No Religion, NFD|1504350552^^^PAH FIN Number Alias Pool^FIN NBR|40602113521||||||0|||||N");
      Counter = 1;
      foreach (var Com in oPIDSeg.Field(3).ComponentList)
      {
        if (Com.ToString() != String.Empty)
          Com.AsString = Counter.ToString();
        Counter++;
      }
      Assert.AreEqual("1^^^4^5^6^^8", oPIDSeg.Field(3).AsString, "oPIDSeg.Field(3).ComponentList returns the incorrect values");

      string datetest = PeterPiper.Hl7.V2.Support.Tools.DateTimeSupportTools.AsString(DateTimeOffset.Now, true, PeterPiper.Hl7.V2.Support.Tools.DateTimeSupportTools.DateTimePrecision.DateHourMinSec);
      //string datetest = PeterPiper.Hl7.V2.Support.Content.DateTimeTools.ConvertDateTimeOffsetToString.AsDateHourMinSec(DateTimeOffset.Now, true);
      //DateTimeOffset testDateTime2 = PeterPiper.Hl7.V2.Support.Content.DateTimeTools.ConvertStringToDateTime.AsDateTimeOffset("2014+0800");
      DateTimeOffset testDateTime2 = PeterPiper.Hl7.V2.Support.Tools.DateTimeSupportTools.AsDateTimeOffSet("2014+0800");

      oMessage = Creator.Message(sbMessage.ToString());
      var SubCom = Creator.SubComponent("Sub");
      var oContent1 = Creator.Content("Test1");
      var oContent2 = Creator.Content("Test2");
      SubCom.Add(oContent1);
      SubCom.Content(1).AsStringRaw = "";
      SubCom.Content(1).AsStringRaw = "Raw1";
      SubCom.Add(oContent2);
      oContent1.AsString = "Test11";
      oContent2.AsString = "Test22";
      oContent1.AsString = "Test111";
      oContent2.AsString = "Test222";
      Assert.AreEqual("SubRaw1Test222", SubCom.AsStringRaw, "SubCom.AsStringRaw returns the incorrect values");


      var oContent3 = Creator.Content("Test3");
      var oContent4 = Creator.Content("Test4");
      SubCom.Set(1, oContent3);
      SubCom.Set(2, oContent4);
      Assert.AreEqual("SubTest3Test4", SubCom.AsStringRaw, "SubCom.AsStringRaw returns the incorrect values");

      var oField = Creator.Field("Field");
      oField.Component(1).Set(0, oContent1);
      Assert.AreEqual("Test111", oField.AsStringRaw, "SubCom.AsStringRaw returns the incorrect values");
      oField.Component(1).Set(0, oContent2);
      Assert.AreEqual("Test222", oField.AsStringRaw, "SubCom.AsStringRaw returns the incorrect values");
      oContent1.AsStringRaw = "Test1";

      oField = Creator.Field("Field");
      var oComp = Creator.Component();
      var oSubComp = Creator.SubComponent();
      oField.Add(oComp);
      oField.Component(2).Add(oSubComp.Clone());
      oField.Component(2).Add(oSubComp.Clone());
      Assert.AreEqual(false, oField.Component(2).IsEmpty, "SubCom.AsStringRaw returns the incorrect values");
      Assert.AreEqual(true, oField.Component(2).SubComponent(2).IsEmpty, "SubCom.AsStringRaw returns the incorrect values");
      oField.Component(2).SubComponent(1).AsString = "Sub1";
      oField.Component(2).SubComponent(1).AsString = "";
      Assert.AreEqual("Field^&", oField.AsStringRaw, "SubCom.AsStringRaw returns the incorrect values");

      oMessage.Segment("PID").Field(2).ClearAll();
      oMessage.Segment("PID").Field(2).Add(oContent1.Clone());
      oMessage.Segment("PID").Field(2).Component(1).Add(oContent1.Clone());


    }
  }


}

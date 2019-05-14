## Peter Piper is a HL7 V2.x parser for .NET Core & .NET Framework, available on [Nuget.org](https://www.nuget.org/packages/HealthIdentifiers.Identifiers/)

Compatible with .NET Core V1.0 & .NET Framework V4.6 

Below is a quick reference guide of the uses of Peter Piper. The full documentation can be found at [Peter Piper documentation Wiki](https://github.com/angusmillar/PeterPiper/wiki)

## **Parse any HL7 V2.x message**

```C#
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.CustomException;

string MyHL7V2Message = "[Any HL7 V2.x message as a string]";
IMessage oHL7 = null;
try
{
  oHL7 = Creator.Message(MyHL7V2Message);
}
catch(PeterPiperException Exec)
{
  string Message = Exec.Message;
  //boring stuff here
}

string PatientFamilyName = oHL7.Segment("PID").Field(5).Component(1).AsString;
string PatientGivenName =  oHL7.Segment("PID").Field(5).Component(2).AsString;
```

## **Create a new message:**

```C#
using PeterPiper.Hl7.V2.Model;

string MessageVersion = "2.4";
string MessageType = "ADT";
string MessageTrigger = "A01";

IMessage oHL7 = Creator.Message(MessageVersion, MessageType, MessageTrigger);

//Or

IMessage oHL7 = Creator.Message(@"MSH|^~\&|||||||ADT^A01|||2.4|");
```

## **Add a new Segment:**

```C#
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support.Tools;

string MessageVersion = "2.4";
string MessageType = "ADT";
string MessageTrigger = "A01";

IMessage oHL7 = Creator.Message(MessageVersion, MessageType, MessageTrigger);

//Create a new PID Segment
ISegment PIDSeg = Creator.Segment("PID");

//Add the patient name
PIDSeg.Field(5).Component(1).AsString = "Dow";
PIDSeg.Field(5).Component(2).AsString = "John";

//Add the patient Date Of Birth
var DateOfBirth = new DateTimeOffset(1985, 09, 30, 0, 0, 0, new TimeSpan(10, 0, 0));
bool IncludeTimeZone = false;
var DateTimePrecision = DateTimeSupportTools.DateTimePrecision.Date;
PIDSeg.Field(7).Convert.DateTime.SetDateTimeOffset(DateOfBirth, IncludeTimeZone, DateTimePrecision);
// We could have also done this:
// PIDSeg.Field(7).AsString = "20190930";

//Add the patient gender
PIDSeg.Field(8).AsString = "M";

//Add the new Segment to the Message
oHL7.Add(PIDSeg);

//Output the whole message
string CompletedMessage = oHL7.AsStringRaw;

// The CompletedMessage will be as follows:
// MSH|^~\&|||||20190510110902.5055+1000||ADT^A01|5e911c99-5b30-451f-93d3-51baa1958785|P|2.4|||AL|NE
// PID|||||Dow^John||19850930|M

```

## **Accessing Fields:**

```C#
using PeterPiper.Hl7.V2.Model;

// Given a message oHL7 with the example PID Segment:
// PID|1||PA30000004^^^AcmeHealth^MR~22222221111^^^AUSHIC^MC~WA123456B^^^AUSDVA^DVG~8003608833357361^^^AUSHIC^NI||Dow^John||19850930|M

string PatientDateOfBirth = oHL7.Segment("PID").Field(7).AsString;

// or, as this is a date we can do this to get a typed DateTimeOffSet:
if (oHL7.Segment("PID").Field(7).Convert.DateTime.CanParseToDateTimeOffset)
{
  DateTimeOffset DateOfBirth = oHL7.Segment("PID").Field(7).Convert.DateTime.GetDateTimeOffset();
}

```
## **Accessing Element Field repeats:**

```C#
using System.Linq;
using PeterPiper.Hl7.V2.Model;

// Given a message oHL7 with the example PID Segment:
// PID|1||PA30000004^^^AcmeHealth^MR~22222221111^^^AUSHIC^MC~WA123456B^^^AUSDVA^DVG~8003608833357361^^^AUSHIC^NI||Dow^John||19850930|M

IField AcmeMRNRepeat = oHL7.Segment("PID").Element(3).RepeatList.SingleOrDefault(x => x.Component(4).AsString == "AcmeHealth");
if (AcmeMRNRepeat != null)
{
  string AcmeMRNValue = AcmeMRNRepeat.Component(1).AsString;
}
```

## **Accessing Components:**

```C#
using PeterPiper.Hl7.V2.Model;

// Given a message oHL7 with the example PID Segment:
// PID|1||PA30000004^^^AcmeHealth^MR~22222221111^^^AUSHIC^MC~WA123456B^^^AUSDVA^DVG~8003608833357361^^^AUSHIC^NI||Dow^John||19850930|M

string PatientFamilyName = oHL7.Segment("PID").Field(5).Component(1).AsString;
string PatientGivenName =  oHL7.Segment("PID").Field(5).Component(2).AsString;
```

## **Accessing SubComponents:**

```C#
using PeterPiper.Hl7.V2.Model;

// Given a message oHL7 with the example OBR Segment:
// OBR|1|112233|15P000005|26604007^Complete blood count^SCT||||||||||||||||||201504101115+1000||HM|F|||||||DRPRIH&DrSurname&PrincipalResultInterpreterHaem&&&DR

string PrincipalResultInterpreterFamilyName = oHL7.Segment("OBR").Field(32).Component(1).SubComponent(2).AsString;
string PrincipalResultInterpreterGivenName = oHL7.Segment("OBR").Field(5).Component(1).SubComponent(3).AsString;
```

## **Convenience Methods on IMessage**

```C#
using PeterPiper.Hl7.V2.Model;

IMessage oHL7= Creator.Message("[Any HL7 V2.x message as a string]");

string MessageControlID = oHL7.MessageControlID;
DateTimeOffset MessageCreationDateTime = oHL7.MessageCreationDateTime;
string MessageType = oHL7.MessageType;
string MessageTrigger = oHL7.MessageTrigger;
string MessageStructure = oHL7.MessageStructure;      
string MessageVersion = oHL7.MessageVersion;      

```

## **Field, Element, Repeat, Component, SubComponent and Content counts:**

```C#
using PeterPiper.Hl7.V2.Model;

// Given a message oHL7 with the example PID Segment:
// PID|1||PA30000004^^^AcmeHealth^MR~22222221111^^^AUS\T\HIC^MC|123^^^NamespaceID&UniversalID&UniversalIdType|Dow^John||19850930|M

int SegmentCount = oHL7.SegmentCount("PID");                                          //Equals: 1
// ElementCount and FieldCount are alway equal to each other.
// The difference between an Element & a Field is what they contain. Elements contains a list of repeating fields where Field 
// is always the first Field repeat. So that is, oHL7.Segment("PID").Element(3).Repeat(1) is equal to oHL7.Segment("PID").Field(3)   
int ElementCount = oHL7.Segment("PID").ElementCount;                                  //Equals: 8
int FieldCount = oHL7.Segment("PID").FieldCount;                                      //Equals: 8
// RepeatCount is how many Field repeats exist in the Element
int RepeatCount = oHL7.Segment("PID").Element(3).RepeatCount;                         //Equals: 2
int ComponentCount = oHL7.Segment("PID").Field(5).ComponentCount;                     //Equals: 2
int SubComponentCount = oHL7.Segment("PID").Field(4).Component(4).SubComponentCount;  //Equals: 3
//Content is the parts between the HL7 escapes, so below is 1=AUS, 2=T, 3=HIC from the string 'AUS\T\HIC' which when unescaped is AUS&HIC
//See Escaping for more info 
int ContentCount = oHL7.Segment("PID").Element(3).Repeat(2).Component(4).ContentCount;//Equals: 3
```

## **Cloning or Copying items**

```C#
using PeterPiper.Hl7.V2.Model;

// Given two messages oHL71 and oHL72:
// Lets say I wish to copy the PID Segment From message 1 to message 2.

// The following would fail as all Peter Piper message objects instances must be cloned 
// if they are to be copied from one place to another.  

// ISegment MsgOnePIDSegment = oHL71.Segment("PID");
// oHL72.Insert(3, MsgOnePIDSegment);

// However, there are .Clone() methods for use on each Peter Piper item.
// Here are a set of working examples:

//Clone a whole message
IMessage oHL71 = Creator.Message("[An ADT^A08 Message]");
IMessage oHL72 = oHL71.Clone(); 

//Clone a whole Segment
ISegment PIDSegment = oHL71.Segment("PID").Clone();
oHL72.Insert(3, PIDSegment);

//Clone a whole Element
IElement PatientIdentifersElement = oHL71.Segment("PID").Element(3).Clone();
oHL72.Segment("PID").Insert(3, PatientIdentifersElement);

//Clone a whole Field
IField FirstPatientIdentifierField = oHL71.Segment("PID").Element(3).Repeat(1).Clone();
oHL72.Segment("PID").Insert(3, FirstPatientIdentifierField);

//Clone a whole Component
IComponent PatientGivenName = oHL71.Segment("PID").Field(5).Component(2).Clone();
oHL72.Segment("PID").Field(5).Insert(2, PatientGivenName);

//Clone a whole SubComponent
ISubComponent SomeSubComponent = oHL71.Segment("PID").Field(5).Component(2).SubComponent(3).Clone();
oHL72.Segment("PID").Field(5).Component(5).Insert(3, SomeSubComponent);

//Clone Content (Content is the data between HL7 escapes)
IContent SomeContent = oHL71.Segment("PID").Field(5).Content(3).Clone();
oHL72.Segment("PID").Field(5).Insert(3, SomeContent);

```


## **Adding, Inserting and Removing Items**

```C#
using PeterPiper.Hl7.V2.Model;

// Given a message oHL7 with the example PID Segment:
// PID|1||PA30000004^^^AcmeHealth^MR~22222221111^^^AUSHIC^MC~WA123456B^^^AUSDVA^DVG~8003608833357361^^^AUSHIC^NI||Dow^John||19850930|M

//Create a new Field instance for a new identifer
IField NewPyroIdentifier = Creator.Field();
NewPyroIdentifier.Component(1).AsString = "123456";
NewPyroIdentifier.Component(4).AsString = "PyroHealth";
NewPyroIdentifier.Component(5).AsString = "MR";

//Add the new Field as a repeat to the end of the list of repeats in PID-3
//PID-3 = |PA30000004^^^AcmeHealth^MR~22222221111^^^AUSHIC^MC~WA123456B^^^AUSDVA^DVG~8003608833357361^^^AUSHIC^NI~123456^^^PyroHealth^MR|
oHL7.Segment("PID").Element(3).Add(NewPyroIdentifier);

//Remove repeats in position two (one based index) of the list of repeats in PID-3, done twice to remove two
//PID-3 = |PA30000004^^^AcmeHealth^MR~8003608833357361^^^AUSHIC^NI~123456^^^PyroHealth^MR|
oHL7.Segment("PID").Element(3).RemoveRepeatAt(2);
oHL7.Segment("PID").Element(3).RemoveRepeatAt(2);

//Create another new Field instance for another new identifer
IField NewMillarIdentifier = Creator.Field();
NewMillarIdentifier.Component(1).AsString = "654321";
NewMillarIdentifier.Component(4).AsString = "MillarHealth";
NewMillarIdentifier.Component(5).AsString = "MR";

//Insert a repeat into the second position among the repeats in PID-3
//PID-3 = |PA30000004^^^AcmeHealth^MR~654321^^^MillarHealth^MR~8003608833357361^^^AUSHIC^NI~123456^^^PyroHealth^MR|
oHL7.Segment("PID").Element(3).Insert(2, NewMillarIdentifier);

// The same Add, Insert and Remove methods are available for all items, 
// here are a few contrived examples of inserting and removing:

//Segments
oHL7.Insert(3, Creator.Segment("PID"));
oHL7.RemoveSegmentAt(3);

//Elements
oHL7.Segment("PID").Element(5).Repeat(1).Insert(2, Creator.Component("PatientGivenName"));
oHL7.Segment("PID").Element(5).Repeat(1).RemoveComponentAt(2);

//Fields (Using Field just means the first Field/Repeat within an Element)
oHL7.Segment("PID").Field(5).Insert(2, Creator.Component("PatientGivenName"));
oHL7.Segment("PID").Field(5).RemoveComponentAt(2);

//SubComponents
oHL7.Segment("PID").Field(5).Component(15).Insert(2, Creator.SubComponent("Weird Stuff"));
oHL7.Segment("PID").Field(5).Component(15).RemoveSubComponentAt(2);

//But it is also worth mentioning that these inserts are only really required if working with the PeterPiper objects.
//The last three inserts could have just as easily been achieved by string assignment to 'AsString', as follows:
oHL7.Segment("PID").Element(5).Repeat(1)..Component(2).AsString = "PatientGivenName";
oHL7.Segment("PID").Field(5).Component(2).AsString = "PatientGivenName";
oHL7.Segment("PID").Field(5).Component(15).SubComponent(2).AsString = "Weird Stuff";

```


## **Escaping**

```C#
using PeterPiper.Hl7.V2.Model;

// Given a message oHL7 with the example OBR Segment:
// OBR|1|112233|15P000005|RhubarbAndSerum^Serum\T\Rhubarb^L||||||||||||||||||201504101115+1000||HM|F

// In most circumstances you will just assign to, and get from, the '.AsString' property and you will need not worry 
// about escaping as Peter Piper will do the work for you. The technics shown here only come into play for more 
// advanced needs when manipulating escaped data. So don't let theses examples scare you, just use '.AsString' until 
// the day you need to play in detail with escaping.

// Here the string as seen in the raw message 'Serum\T\Rhubarb' is unescaped for us to 'Serum&Rhubarb' by PeterPiper
// when we us the property '.AsString'
// TestDescription1 = "Serum&Rhubarb"
string TestDescription1 = oHL7.Segment("OBR").Field(4).Component(4).AsString; 

// Yet, here as we are using the property '.AsRawString. The string is not unescaped for us, it is provided raw as 
// it would be seen in the raw message
// TestDescription2 = "Serum\T\Rhubarb"
string TestDescription2 = oHL7.Segment("OBR").Field(4).Component(4).AsRawString; 

// You can also set a string value with a reserved HL7 V2 character using 'AsString' and PeterPiper will correctly 
// escape it for you.
// Here we are setting the string 'Serum&Rhubarb' which has a reserved character '&'. Because we have used '.AsString'
// Peter Piper will do the correct escaping for us and the string in the raw message will appear as 'Serum\T\Rhubarb'
oHL7.Segment("OBR").Field(4).Component(4).AsString = "Serum&Rhubarb"

// When you get the string using 'AsString' PeterPiper will unescape it back to 'Serum&Rhubarb' for you
// TestDescription3 = "Serum&Rhubarb"
string TestDescription3 = oHL7.Segment("OBR").Field(4).Component(4).AsString; 

//When you get the string using 'AsRawString' PeterPiper will not unescape it for you
// TestDescription4 = "Serum\T\Rhubarb"
string TestDescription4 = oHL7.Segment("OBR").Field(4).Component(4).AsRawString; 

// If you attempt the below example a PeterPiperException will be thrown with the message: 
// "Component data cannot contain HL7 V2 Delimiters of : | or ^ or ~"
// This is because it treats the `~` as meaning you want to add a repeat to a Component which is not allowed.
// Components can not have repeats only Elements can.  
oHL7.Segment("OBR").Field(4).Component(4).AsStringRaw = "Serum~Rhubarb"

// However if you wanted to add the actual charter '~' to your string then you would use 'AsString' and the raw message
// would contain the correctly escaped string 'Serum\R\Rhubarb'.  
oHL7.Segment("OBR").Field(4).Component(4).AsString = "Serum~Rhubarb";

// And again here the string has been unescaped for us by PeterPiper
// TestDescription3 = "Serum~Rhubarb"
string TestDescription5 = oHL7.Segment("OBR").Field(4).Component(4).AsString; 

// The string is not unescaped for us by PeterPiper, if you use .AsRawString' it is provided 'raw' as seen in the message.
// TestDescription4 = "Serum\R\Rhubarb"
string TestDescription6 = oHL7.Segment("OBR").Field(4).Component(4).AsRawString;

// However, if we were adding the '^' to a Field using the 'AsStringRaw' it will not throw an exception. 
// It would split the Field into two Components e.g: |Serum^Rhubarb|, because Fields are allowed to have Components
oHL7.Segment("OBR").Field(4).AsStringRaw = "Serum^Rhubarb"

//x1 = Serum
string x1 = oHL7.Segment("OBR").Field(3).Component(1).AsString;
//x2 = Rhubarb
string x2 = oHL7.Segment("OBR").Field(3).Component(2).AsString;

// A more advanced way to work with Escapes is to use the Content object
// For instance we could use the HL7 highlight escapes 
// Highlight ON = '/H/' 
// and 
// Highlight OFF = '/N/' 
// as follows

//First lets clear the entire OBR-4 Field to start fresh .
oHL7.Segment("OBR").Field(4).ClearAll();

//Now use the Content type to add the escapes
oHL7.Segment("OBR").Field(4).Add(Creator.Content("Highlight my"));
oHL7.Segment("OBR").Field(4).Add(Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.HighlightOn));
oHL7.Segment("OBR").Field(4).Add(Creator.Content("WORLD"));
oHL7.Segment("OBR").Field(4).Add(Creator.Content(PeterPiper.Hl7.V2.Support.Standard.EscapeType.HighlightOff));

//x3 = Highlight my\H\WORLD\N\
string x3 = oHL7.Segment("OBR").Field(4).AsString;
//x2 = Highlight myWORLD
string x4 = oHL7.Segment("OBR").Field(4).AsString;

// As we said at the beginning, if your new to Peter Piper and just want to ensure your strings are escaped and unescaped
// then just make sure to use '.AsString' and the magic will happen.

// You wil also find that the standard .NET '.ToString()' is also overridden to behave like '.AsString', but I think it is 
// easier to remember '.AsString' and 'AsRawString' than remember '.ToString()' and 'AsRawString'.     

```
## **Custom HL7 V2 Escape Characters**
```C#

using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support.Tools;

// It is legal in HL7 V2.x to use completely different escape sequences in the HL7 V2 message. 
// This is instead of the typical escape sequences of '|^~\&'. 
// To do so you would set the sequences you wish to use in the MSH-1 and MSH-2 Fields.
// PeterPiper can also handle these custom escape sequences characters as per the examples below.
// However, I highly advise you avoid this behavior as very few systems can coupe with it.

//My new custom escape sequences characters
char FieldChar = '!';
char RepeatChar = '%';
char ComponentChar = '*';
char SubComponentChar = '@';
char EscapeChar = '#';
IMessageDelimiters MessageDelimiters = Creator.MessageDelimiters(FieldChar, RepeatChar, ComponentChar, SubComponentChar, EscapeChar);

// A message instance using my custom escape sequences
IMessage oHL7 = Creator.Message("MSH!*%#@!SendApp!SendFacility!RecApp!RecFacility!20140527095657!!ORU*R01!0000000000000000010D!P!2.3.1");
// A new Segment also using the same custom escape sequences
// Notice that when using custom escape sequences you must always provide the IMessageDelimiters object 
// when creating new segments. 
ISegment oSeg = Creator.Segment("PID!!!!!Dow*John!!19850930!M", MessageDelimiters);
oHL7.Add(oSeg);

//Given an already parsed message you can the IMessageDelimiters that is it currently using as follows:
IMessageDelimiters ThisMessagesDelimiters = target.MessageDelimiters;

//As I said earlier, I doubt you will every use this functionality in the real world.

```



## **The Convert extension methods: Integer, DateTimeOffset, Base64**

```C#
using System.Linq;
using PeterPiper.Hl7.V2.Model;
using PeterPiper.Hl7.V2.Support.Tools;

//Given a message oHL7 with the example OBX Segment:
// OBX|7|NM|^^^PLAT^Platelet^Local||198|x10\S\9/L^^ISO+|150-400||||F|||201905110930+1000|""|

//Get the Platelet result value as an integer and its Observation DateTime as a DateTimeOffSet 
ISegment PlateletSegment = oHL71.SegmentList("OBX").SingleOrDefault(x => x.Field(3).Component(4).AsString == "PLAT");
if (PlateletSegment != null)
{
  //Get the result value as an Integer type
  if (PlateletSegment.Field(5).Convert.Integer.IsNumeric)
  {
    int PlateletValueInteger = PlateletSegment.Field(5).Convert.Integer.Int32;
  }

  //Get the Observation DateTime as a DateTimeOffset type 
  if (PlateletSegment.Field(14).Convert.DateTime.CanParseToDateTimeOffset)
  {
    DateTimeOffset PlateletObservationDateTime = PlateletSegment.Field(14).Convert.DateTime.GetDateTimeOffset();
  }

  //Now update the Observation DateTime to DateTime Now, while preserving the original precision & TimeZone
  if (PlateletSegment.Field(14).Convert.DateTime.CanParseToDateTimeOffset)
  {
    //Get the original precision, can be one of (None, Year, YearMonth, Date, DateHourMin, DateHourMinSec, DateHourMinSecMilli)
    //Note: 'None' is only returned if CanParseToDateTimeOffset == false      
    DateTimeSupportTools.DateTimePrecision OriginalPrecision = PlateletSegment.Field(14).Convert.DateTime.GetPrecision();

    //Get a Boolean whether or not the original HL7 DateTime had a TimeZone part e.g '+1000', in this example it does  
    bool OriginalHasTimeZone = PlateletSegment.Field(14).Convert.DateTime.HasTimezone;

    //Get a new DateTime now
    var NewDateTime = DateTimeOffset.Now;
    if (OriginalHasTimeZone)
    {
      //If the Original had a TimeZone then convert the new DateTime to that timezone
      TimeSpan OriginalTimeZone = PlateletSegment.Field(14).Convert.DateTime.GetTimezone();
      NewDateTime = NewDateTime.ToOffset(OriginalTimeZone);
    }
    
    //Update the HL7 DateTime in the message with the new daeTime and the same precision and timezone if the Original had one
    PlateletSegment.Field(14).Convert.DateTime.SetDateTimeOffset(NewDateTime, OriginalHasTimeZone, OriginalPrecision);
  }
}

//Base64 encoding and decoding
//Encode to Base64
byte[] SomeDataAsAByteArray  = System.IO.File.ReadAllBytes(@"C:\temp\SomeData.dat");
oHL71.Segment("OBX").Field(5).Convert.Base64.Encode(SomeDataAsAByteArray);

//Decode from Base64
SomeDataAsAByteArray = oHL71.Segment("OBX").Field(5).Convert.Base64.Decode();

```


Owner:

* Angus Millar: angusbmillar@gmail.com
Version 1.000

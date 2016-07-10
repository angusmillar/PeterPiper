**Peter Piper is an HL7 V2 Parser for .NET Framework 4 implemented in C# and compiles to a single PeterPiper.dll**

[Peter Piper documentation Wiki](https://github.com/angusmillar/PeterPiper/wiki)

```C#
//Simple example:
using PeterPiper.Hl7.V2.Model;

string MyHL7V2Message = "Any HL7 V2 Message as a string";
var oHl7 = Creator.Message(MyHL7V2Message);

string sPatientSurname = oHl7.Segment("PID").Field(5).Component(1).AsString;

```

Having worked with HL7 V2 messaging I certainly know the pain that most implementers face. Of main concern are live interfaces outputting non-standard messages. This is the norm, not the exception. Yet, the leading V2 parsers tend to be written on the premise that all messages will be standards compliant and make working with non-standard messages difficult.

Peter Piper takes a different approach. With the loss of descriptive properties, Peter Piper gains tremendous flexibility and simplicity. This is a design trade-off that has been taken for few reasons:

* Simplicity
* Flexibility
* The majority of messages in the wild are not standard

The HL7 V2 horse has bolted and we are left to deal with what we have; a non-perfect world. If you have spent any time in V2 message land then you will know the phrase:
> "If you have seen one HL7 V2 interface, then you have seen one HL7 V2 interface."

So how does Peter Piper work? It deals with the very basics of HL7 V2 messages, the basic syntax of the message and not the archetypes or data types that are then built on top of the basics. The core parts of a V2 message are:

* ***Message:*** the message as a whole ```MSH|^~\&|SUPER-LIS^2.16.840.1.113883.19.1^ISO|NE...etc```
* ***Segments:*** each line in a message, Segments are terminated by carriage return ```<cr>```
* ***Elements:*** an Element holds many repeating Fields 
```|this is field 1 in Element 1~this is field 2 in Element 1|```  
* ***Repeats:*** a Repeat is a single Field when there are many within an Element, as in the example above where there are 2 repeats. 
* ***Fields:*** a single Field within an Element. Often when we have many Fields in an Element we call them Repeats ```|Ths is a single Field|This is Field Repeat 1~This is Field Repeat 2|``` 
* ***Components:*** a Field can have many Components ```|Compoent1^Compoent2^Compoent3|```
* ***SubCompoents:*** a Component can have many SubComponents ```|Compoent1^SubCompoent1&SubCompoent2&SubCompoent3^Compoent3|``` 
* ***Content:*** A string can have escapes within it, such as the highlighting escape ```\H\```. Content is the strings between the escapes characters ```\```, remember that the escape content is also content. So the ```H``` is a string after an escape character.  ```|This is content 0 not highlighted\H\ this is content 2 highlighted \N\ this is content 4 not highlighted|``` Content 1 is the actual escape content ```H``` and Content 3 is the actual escape content ```N```. 


Owner:

* Angus Millar: angusmillar@iinet.net.au
Version 1.000

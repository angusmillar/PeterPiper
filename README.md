Peter Piper Parser

MSH|^~\&|~Peter Piper picked a peck of pickled patients;
         ~A peck of pickled patients Peter Piper picked;
         ~If Peter Piper picked a peck of pickled patients,
         ~Where's the peck of pickled patients Peter Piper picked?

Peter Piper HL7 V2 Parser for .net

This is HL7 V2 library. It is is a complete state and can parse any valid V2 messages and allow easly manipulation of the message. It also provides support for file writer and reader, base64 support and DateTime support. Sorry no  TCP server/client support planned as yet.    

//Simple example:

string MyHL7V2Message = "Any HL7 V2 Message as a string";

PeterPiper.Hl7.V2.Model.Message oHl7 = new PeterPiper.Hl7.V2.Model.Message(MyHL7V2Message);

string PatientSurname = oHl7.Field(5).AsString;

More Docuemnation will be generated soon.

Owner:

* Angus Millar: angusmillar@iinet.net.au

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterPiper.Hl7.V2.Schema.Model
{

  public class Primitive : DataTypeBase
  {
    private string _Name;
    public string Name
    {
      get { return _Name; }
      set { _Name = value; }
    }
  }

  public static class PrimitiveSupport
  {
    public static string GetNameForCode(string Code)
    {
      switch (Code)
      {
        case "TM":
          return "time";
        case "ST":
          return "string";
        case "TN":
          return "telephone number";
        case "DT":
          return "date";
        //DTM only appeared in HL7 Version 2.5
        case "DTM":
          return "date/time";
        case "ID":
          return "coded value for HL7 defined tables";
        case "IS":
          return "coded value for user-defined tables";
        case "NM":
          return "numeric";
        case "SI":
          return "sequence ID";
        case "FT":
          return "formated text";
        case "TX":
          return "text data";
        //GTS only appeared in HL7 Version 2.5
        case "GTS":
          return "general timing specification";
        //NUL only appeared in HL7 Version 2.5.1 of the .xsd's but is not seen in the 2.6 standard.
        //I currently have no copy of the 2.5.1 standard to check. I assume the meaning is to be Null? 
        case "NUL":
          return "Nul";
        //Version 2.3 talks of CM - composite as a data type that should not be used any more although it is
        //used throughout this version's .xsd's. The xsd's also states the following primitives codes yet never 
        //later references them ("CM_CCP", "CM_CD_ELECTRODE", "CM_CSU", "CM_MDV", "CM_OSD"). I believe that all of these
        // are in fact to be CM primitives. That is what I have done, dropped this set and only used CM and the parse works.  
        // See the HL7 V2.3.1 standard chapter 2.8.6 for more info. 
        case "CM":
          return "composite";          
        case "*":
          return "varies";
        default:
          throw new Exception("Unknown Primitive code found, unable to assign name to code. Code was: " + Code); 
      }
    }
  }


}

using System;

namespace PeterPiper.Hl7.V2.Schema.Model;

public class Primitive : DataTypeBase
{
  public string Name { get; set; }
}

public static class PrimitiveSupport
{
  public static string GetNameForCode(string code)
  {
    return code switch
    {
      "TM" => "time",
      "ST" => "string",
      "TN" => "telephone number",
      "DT" => "date",
      //DTM only appeared in HL7 Version 2.5
      "DTM" => "date/time",
      "ID" => "coded value for HL7 defined tables",
      "IS" => "coded value for user-defined tables",
      "NM" => "numeric",
      "SI" => "sequence ID",
      "FT" => "formated text",
      "TX" => "text data",
      //GTS only appeared in HL7 Version 2.5
      "GTS" => "general timing specification",
      //NUL only appeared in HL7 Version 2.5.1 of the .xsd's but is not seen in the 2.6 standard.
      //I currently have no copy of the 2.5.1 standard to check. I assume the meaning is to be Null? 
      "NUL" => "Nul",
      //Version 2.3 talks of CM - composite as a data type that should not be used any more although it is
      //used throughout this version's .xsd's. The xsd's also states the following primitives codes yet never 
      //later references them ("CM_CCP", "CM_CD_ELECTRODE", "CM_CSU", "CM_MDV", "CM_OSD"). I believe that all of these
      // are in fact to be CM primitives. That is what I have done, dropped this set and only used CM and the parse works.  
      // See the HL7 V2.3.1 standard chapter 2.8.6 for more info. 
      "CM" => "composite",
      "*" => "varies",
      _ => throw new Exception("Unknown Primitive code found, unable to assign name to code. Code was: " + code)
    };
  }
}
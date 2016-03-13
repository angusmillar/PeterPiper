using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterPiper.Hl7.V2.Support.Standard
{
  public static class Hl7Table
  {

    /// <summary>
    /// Acknowledgment code
    /// HL7 Table 0008
    /// </summary>
    public static class Table_0008
    {
      public enum AcknowledgmentCodeType { ApplicationAccept, ApplicationError, ApplicationReject, CommitAccept, CommitError, CommitReject };

      /// <summary>
      /// Original mode: Application Accept - 
      /// </summary>
      public static string ApplicationAccept = "AA";
      /// <summary>
      /// Original mode: Application Error 
      /// </summary>
      public static string ApplicationError = "AE";
      /// <summary>
      /// Original mode: Application Reject
      /// </summary>
      public static string ApplicationReject = "AR";
      /// <summary>
      /// Convert a OriginalModeCodeType to it's code string.
      /// </summary>
      /// <param name="Type"></param>
      /// <returns></returns>
      /// <summary>
      /// Enhanced mode: Accept acknowledgment: Commit Accept 
      /// </summary>
      public static string CommitAccept = "CA";
      /// <summary>
      /// Enhanced mode: Accept acknowledgment: Commit Error 
      /// </summary>
      public static string CommitError = "CE";
      /// <summary>
      /// Enhanced mode: Accept acknowledgment: Commit Reject 
      /// </summary>
      public static string CommitReject = "CR";
      /// <summary>
      /// Convert a AcknowledgmentCodeType to it's code string.
      /// </summary>
      /// <param name="Type"></param>
      /// <returns></returns>
      public static string ConvertTypeToCode(AcknowledgmentCodeType Type)
      {
        switch (Type)
        {
          case AcknowledgmentCodeType.ApplicationAccept:
            return Table_0008.ApplicationAccept;
          case AcknowledgmentCodeType.ApplicationError:
           return Table_0008.ApplicationError;
          case AcknowledgmentCodeType.ApplicationReject:
            return Table_0008.ApplicationReject;
          case AcknowledgmentCodeType.CommitAccept:
            return Table_0008.CommitAccept;
          case AcknowledgmentCodeType.CommitError:
           return Table_0008.CommitError;
          case AcknowledgmentCodeType.CommitReject:
           return Table_0008.CommitReject;
          default:
           throw new ArgumentException(string.Format("Unknown AcknowledgmentCodeType passed in, code was: {0}", Type.ToString()));
        }                 
      }
    }

    /// <summary>
    /// ProcessingID
    /// HL7 Table 0103
    /// </summary>
    public static class Table_0103
    {
      public static char Debugging = 'D';
      public static char Production = 'P';
      public static char Training = 'T';
    }
    /// <summary>
    /// Version ID
    /// HL7 Table 0103
    /// </summary>
    public static class Table_0104
    {
      /// <summary>
      /// Version 2.0
      /// </summary>
      public static string ReleaseTwoPointZero = "2.0";
      /// <summary>
      /// Demo 2.0
      /// </summary>
      public static string DemoTwoPointZero = "2.0";
      /// <summary>
      /// Version 2.1
      /// </summary>
      public static string ReleaseTwoPointOne = "2.1";
      // <summary>
      /// Version 2.2
      /// </summary>
      public static string ReleaseTwoPointTwo = "2.2";
      // <summary>
      /// Version 2.3
      /// </summary>
      public static string ReleaseTwoPointThree = "2.3";
      // <summary>
      /// Version 2.3.1
      /// </summary>
      public static string ReleaseTwoPointThreePointOne = "2.3.1";
      // <summary>
      /// Version 2.4
      /// </summary>
      public static string ReleaseTwoPointFour = "2.4";
      // <summary>
      /// Version 2.5
      /// </summary>
      public static string ReleaseTwoPointFive = "2.5";
      // <summary>
      /// Version 2.6
      /// </summary>
      public static string ReleaseTwoPointSix = "2.6";
      // <summary>
      /// Version 2.7
      /// </summary>
      public static string ReleaseTwoPointSeven = "2.7";
      // <summary>
      /// Version 2.7
      /// </summary>
      public static string ReleaseTwoPointEgith = "2.8";
      // <summary>
      /// Version 2.7
      /// </summary>
      public static string ReleaseTwoPointNine = "2.9";
      // <summary>
      /// Version 2.7
      /// </summary>
      public static string ReleaseTwoPointNinePointOne = "2.9.1";   // Just a guess?
    }


        /// <summary>
    /// ProcessingID
    /// HL7 Table 0364
    /// </summary>
    public static class Table_0364
    {
      /// <summary>
      /// PI	Patient Instructions 
      /// </summary>
      public static string PatientInstructions = "PI";
      /// <summary>
      /// AI	Ancillary Instructions
      /// </summary>
      public static string Production = "AI";
      /// <summary>
      /// GI	General Instructions
      /// </summary>
      public static string GeneralInstructions = "GI";
      /// <summary>
      /// 1R	Primary Reason
      /// </summary>
      public static string PrimaryReason= "1R";
      /// <summary>
      /// 2R	Secondary Reason
      /// </summary>
      public static string SecondaryReason = "2R";
      /// <summary>
      /// GR	General Reason
      /// </summary>
      public static string GeneralReason= "GR";
      /// <summary>
      /// RE	Remark
      /// </summary>
      public static string Remark = "RE";
      /// <summary>
      /// DR	Duplicate/Interaction Reason
      /// </summary>
      public static string Duplicate_InteractionReason= "DR";

    }
  }
}

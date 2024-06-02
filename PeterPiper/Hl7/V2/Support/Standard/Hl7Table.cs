
using PeterPiper.Hl7.V2.CustomException;

namespace PeterPiper.Hl7.V2.Support.Standard;

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
    public const string ApplicationAccept = "AA";

    /// <summary>
    /// Original mode: Application Error 
    /// </summary>
    public const string ApplicationError = "AE";

    /// <summary>
    /// Original mode: Application Reject
    /// </summary>
    public const string ApplicationReject = "AR";

    /// <summary>
    /// Convert a OriginalModeCodeType to it's code string.
    /// </summary>
    /// <param name="Type"></param>
    /// <returns></returns>
    /// <summary>
    /// Enhanced mode: Accept acknowledgment: Commit Accept 
    /// </summary>
    public const string CommitAccept = "CA";

    /// <summary>
    /// Enhanced mode: Accept acknowledgment: Commit Error 
    /// </summary>
    public const string CommitError = "CE";

    /// <summary>
    /// Enhanced mode: Accept acknowledgment: Commit Reject 
    /// </summary>
    public const string CommitReject = "CR";

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
          return ApplicationAccept;
        case AcknowledgmentCodeType.ApplicationError:
          return ApplicationError;
        case AcknowledgmentCodeType.ApplicationReject:
          return ApplicationReject;
        case AcknowledgmentCodeType.CommitAccept:
          return CommitAccept;
        case AcknowledgmentCodeType.CommitError:
          return CommitError;
        case AcknowledgmentCodeType.CommitReject:
          return CommitReject;
        default:
          throw new PeterPiperException($"Unknown AcknowledgmentCodeType passed in, code was: {Type.ToString()}");
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
      
    /// Version 2.0
    public static string ReleaseTwoPointZero = "2.0";
      
    /// Demo 2.0
    public static string DemoTwoPointZero = "2.0";
      
    /// Version 2.1
    public static string ReleaseTwoPointOne = "2.1";
      
    /// Version 2.2
    public static string ReleaseTwoPointTwo = "2.2";
      
    /// Version 2.3
    public static string ReleaseTwoPointThree = "2.3";
      
    /// Version 2.3.1
    public static string ReleaseTwoPointThreePointOne = "2.3.1";
      
    /// Version 2.4
    public static string ReleaseTwoPointFour = "2.4";
      
    /// Version 2.5
    public static string ReleaseTwoPointFive = "2.5";
      
    /// Version 2.6
    public static string ReleaseTwoPointSix = "2.6";
      
    /// Version 2.7
    public static string ReleaseTwoPointSeven = "2.7";
      
    /// Version 2.7
    public static string ReleaseTwoPointEgith = "2.8";
      
    /// Version 2.7
    public static string ReleaseTwoPointNine = "2.9";
      
    /// Version 2.7
    public static string ReleaseTwoPointNinePointOne = "2.9.1";   // Just a guess?
  }
  
  /// ProcessingID
  /// HL7 Table 0364
  /// </summary>
  public static class Table_0364
  {
      
    /// PI	Patient Instructions 
    public static string PatientInstructions = "PI";
      
    /// AI	Ancillary Instructions
    public static string Production = "AI";
      
    /// GI	General Instructions
    public static string GeneralInstructions = "GI";
      
    /// 1R	Primary Reason
    public static string PrimaryReason= "1R";
      
    /// 2R	Secondary Reason
    public static string SecondaryReason = "2R";
      
    /// GR	General Reason
    public static string GeneralReason= "GR";
      
    /// RE	Remark
    public static string Remark = "RE";
      
    /// DR	Duplicate/Interaction Reason
    public static string Duplicate_InteractionReason= "DR";

  }
}
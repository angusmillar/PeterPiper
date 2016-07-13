using System;
using PeterPiper.Hl7.V2.Model.Implementation;

namespace PeterPiper.Hl7.V2.Support.Content.Convert.Implementation
{
  public class Integer : PeterPiper.Hl7.V2.Support.Content.Convert.IInteger
  {
    private ContentBase _ContentBase;

    internal Integer(ContentBase ContentBase)
    {
      _ContentBase = ContentBase;
    }

    public int Int
    {
      get
      {
        return Int32;
      }
    }

    public short Int16
    {
      get
      {
        Int16 Int16Result;
        if (Int16.TryParse(_ContentBase.AsString, out Int16Result))
        {
          return Int16Result;
        }
        else
        {
          throw new FormatException(string.Format("The value '{0}' could not be converted to an int16 integer data type.", _ContentBase.AsString));
        }
      }
    }

    public int Int32
    {
      get
      {
        Int32 Int16Result;
        if (Int32.TryParse(_ContentBase.AsString, out Int16Result))
        {
          return Int16Result;
        }
        else
        {
          throw new FormatException(string.Format("The value '{0}' could not be converted to an int32 integer data type.", _ContentBase.AsString));
        }
      }
    }

    public long Int64
    {
      get
      {
        Int64 Int16Result;
        if (Int64.TryParse(_ContentBase.AsString, out Int16Result))
        {
          return Int16Result;
        }
        else
        {
          throw new FormatException(string.Format("The value '{0}' could not be converted to an int64 integer data type.", _ContentBase.AsString));
        }
      }
    }

    public bool IsNumeric
    {
      get
      {
        return IsDigitsOnly(_ContentBase.AsString);
      }
    }

    /// <summary>
    /// Returns the string that is currently set.
    /// </summary>
    /// <returns></returns>
    public string AsString()
    {
      return _ContentBase.AsString;      
    }

    private static bool IsDigitsOnly(string str)
    {
      foreach (char c in str)
      {
        if (c < '0' || c > '9')
          return false;
      }
      return true;
    }
  }
}

using System;
using System.Linq;
using PeterPiper.Hl7.V2.Model.Implementation;

namespace PeterPiper.Hl7.V2.Support.Content.Convert.Implementation;

public class Integer : PeterPiper.Hl7.V2.Support.Content.Convert.IInteger
{
    private ContentBase _ContentBase;

    internal Integer(ContentBase contentBase)
    {
        _ContentBase = contentBase;
    }

    public int Int => Int32;

    public short Int16
    {
        get
        {
            if (Int16.TryParse(_ContentBase.AsString, out var Int16Result))
            {
                return Int16Result;
            }

            throw new FormatException(
                $"The value '{_ContentBase.AsString}' could not be converted to an int16 integer data type.");
        }
    }

    public int Int32
    {
        get
        {
            if (Int32.TryParse(_ContentBase.AsString, out var Int16Result))
            {
                return Int16Result;
            }

            throw new FormatException(
                $"The value '{_ContentBase.AsString}' could not be converted to an int32 integer data type.");
        }
    }

    public long Int64
    {
        get
        {
            if (Int64.TryParse(_ContentBase.AsString, out var Int16Result))
            {
                return Int16Result;
            }

            throw new FormatException(
                $"The value '{_ContentBase.AsString}' could not be converted to an int64 integer data type.");
        }
    }

    public bool IsNumeric => IsDigitsOnly(_ContentBase.AsString);

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
        return str.All(c => c is >= '0' and <= '9');
    }
}
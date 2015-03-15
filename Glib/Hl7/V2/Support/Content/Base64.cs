using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glib.Hl7.V2.Support.Content
{
  /// <summary>
  /// Class to handle Base64 encoding and decoding. 
  /// </summary>
  public static class Base64
  {
    /// <summary>
    /// Convert a string into a base64 encoded string
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static string Encoder(byte[] item)
    {
      return Convert.ToBase64String(item);
    }

    /// <summary>
    /// Convert a base64 encoded string to a decoded string
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static byte[] Decoder(string item)
    {
      return Convert.FromBase64String(item);        
    }

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterPiper.Hl7.V2.CustomException
{
  public class PeterPiperException : System.Exception
  {
    public PeterPiperException()
    { 
    }
    public PeterPiperException(string message)
      :base(message)
    { }

    public PeterPiperException(string message, Exception inner)
      : base(message, inner)
    { }

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterPiper.Hl7.V2.CustomException
{
  class PeterPiperArgumentException : PeterPiperException
  {
    public PeterPiperArgumentException()
    { 
    }
    public PeterPiperArgumentException(string message)
      :base(message)
    { }

    public PeterPiperArgumentException(string message, Exception inner)
      : base(message, inner)
    { }
  }
}

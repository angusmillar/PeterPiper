using System;

namespace PeterPiper.Hl7.V2.CustomException;

public class PeterPiperException : Exception
{
  public PeterPiperException(string message)
    :base(message) { }

  public PeterPiperException(string message, Exception inner)
    : base(message, inner) { }

}
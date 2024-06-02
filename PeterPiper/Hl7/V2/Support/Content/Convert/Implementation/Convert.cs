using PeterPiper.Hl7.V2.Model.Implementation;

namespace PeterPiper.Hl7.V2.Support.Content.Convert.Implementation;

public class Convert : IConvert
{
  private readonly ContentBase _ContentBase;

  internal Convert(ContentBase contentBase)
  {
    _ContentBase = contentBase;
  }

  public IBase64 Base64 => new Base64(_ContentBase);

  public IDateTime DateTime => new DateTime(_ContentBase);

  public IInteger Integer => new Integer(_ContentBase);
}
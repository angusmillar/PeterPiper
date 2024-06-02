using PeterPiper.Hl7.V2.Model.Implementation;

namespace PeterPiper.Hl7.V2.Support.Content.Convert.Implementation;

public class Base64 : IBase64
{
  private readonly ContentBase _ContentBase;

  internal Base64(ContentBase contentBase)
  {
    _ContentBase = contentBase;
  }

  public void Encode(byte[] item)
  {
    _ContentBase.AsStringRaw = Support.Tools.Base64Tools.Encoder(item);
  }
  public byte[] Decode()
  {
    return Tools.Base64Tools.Decoder(_ContentBase.AsStringRaw);
  }
}
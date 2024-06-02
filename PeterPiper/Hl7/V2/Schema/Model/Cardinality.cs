
namespace PeterPiper.Hl7.V2.Schema.Model;

public abstract class Cardinality
{
  public bool CanRepeat { get; set; }

  public bool IsMandatory { get; set; }

  public string CardinalityMask
  {
    get
    {
      char min = '0';
      char max = '1';
      if (IsMandatory)
      {
        min = '1';
      }

      if (CanRepeat)
      {
        max = '*';
      }
        
      return $"{min}..{max}";
    }
  }

}
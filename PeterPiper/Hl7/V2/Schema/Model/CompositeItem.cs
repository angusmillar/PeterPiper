
namespace PeterPiper.Hl7.V2.Schema.Model;

public class CompositeItem
{
  public string Description { get; set; }

  public int Hl7TableIndex { get; set; }

  public DataTypeBase Type { get; set; }
}
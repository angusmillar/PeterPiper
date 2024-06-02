
namespace PeterPiper.Hl7.V2.Schema.Model;

public class SegmentField : Cardinality
{
  public int ItemNumber { get; set; }
  public string Description { get; set; }
  public DataTypeBase DataType { get; set; }
  public int Hl7TableIndex { get; set; }
}
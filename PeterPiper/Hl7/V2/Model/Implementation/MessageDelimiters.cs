
namespace PeterPiper.Hl7.V2.Model.Implementation;

internal class MessageDelimiters : IMessageDelimiters
{
  public MessageDelimiters()
  {
  }

  public MessageDelimiters(char field, char repeat, char component, char subComponent, char escape)
  {
    Field = field;
    Repeat = repeat;
    Component = component;
    SubComponent = subComponent;
    Escape = escape;
  }

  public char Field { get; } = Support.Standard.Delimiters.Field;
  public char Repeat { get; } = Support.Standard.Delimiters.Repeat;
  public char Component { get; } = Support.Standard.Delimiters.Component;
  public char SubComponent { get; } = Support.Standard.Delimiters.SubComponent;
  public char Escape { get; } = Support.Standard.Delimiters.Escape;
}
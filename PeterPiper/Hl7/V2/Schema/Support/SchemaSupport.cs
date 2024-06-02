namespace PeterPiper.Hl7.V2.Schema.Support;

public class SchemaSupport
{
  public Model.VersionSchema CurrentSchema { get; set; } = null;

  public void LoadSchema(PeterPiper.Hl7.V2.Model.IMessage oMsg)
  {
    LoadSchema(oMsg.MessageVersion, oMsg.MessageType, oMsg.MessageTrigger);
  }
  public void LoadSchema(string messageVersion, string messageType, string messageTrigger)
  {
    var oMessageVersion = Model.Version.GetVersionFromString(messageVersion);
    if (oMessageVersion != Model.VersionsSupported.NotSupported)
    {
      if (CurrentSchema == null)
      {
        CurrentSchema = XmlParser.SchemaParser.LoadSingleMessage(Model.Version.GetVersionFromString(messageVersion), messageType.ToUpper(), messageTrigger.ToUpper());
      }
      else
      {
        if (CurrentSchema.Version == Model.Version.GetVersionFromString(messageVersion))
          XmlParser.SchemaParser.LoadAnotherMessage(CurrentSchema, messageType.ToUpper(), messageTrigger.ToUpper());
        else
          CurrentSchema = XmlParser.SchemaParser.LoadSingleMessage(Model.Version.GetVersionFromString(messageVersion), messageType.ToUpper(), messageTrigger.ToUpper());
      }
    }
  }
}
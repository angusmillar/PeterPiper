using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeterPiper.Hl7.V2;

namespace PeterPiper.Hl7.V2.Schema.Support
{
  public class SchemaSupport
  {
    private  Schema.Model.VersionSchema _CurrentSchema = null;
    public Schema.Model.VersionSchema CurrentSchema
    {
      get { return _CurrentSchema; }
      set { _CurrentSchema = value; }
    }

    public void LoadSchema(PeterPiper.Hl7.V2.Model.IMessage oMsg)
    {
      this.LoadSchema(oMsg.MessageVersion, oMsg.MessageType, oMsg.MessageTrigger);
    }
    public void LoadSchema(string MessageVersion, string MessageType, string MessageTrigger)
    {
      var oMessageVersion = Model.Version.GetVersionFromString(MessageVersion);
      if (oMessageVersion != Model.VersionsSupported.NotSupported)
      {
        if (_CurrentSchema == null)
        {
          _CurrentSchema = V2.Schema.XmlParser.SchemaParser.LoadSingleMessage(V2.Schema.Model.Version.GetVersionFromString(MessageVersion), MessageType.ToUpper(), MessageTrigger.ToUpper());
        }
        else
        {
          if (_CurrentSchema.Version == Model.Version.GetVersionFromString(MessageVersion))
            V2.Schema.XmlParser.SchemaParser.LoadAnotherMessage(_CurrentSchema, MessageType.ToUpper(), MessageTrigger.ToUpper());
          else
            _CurrentSchema = V2.Schema.XmlParser.SchemaParser.LoadSingleMessage(V2.Schema.Model.Version.GetVersionFromString(MessageVersion), MessageType.ToUpper(), MessageTrigger.ToUpper());
        }
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.Hl7.V2;

namespace Glib.Hl7.V2.Schema.Support
{
  public class SchemaSupport
  {
    private  Schema.Model.VersionSchema _CurrentSchema = null;
    public Schema.Model.VersionSchema CurrentSchema
    {
      get { return _CurrentSchema; }
      set { _CurrentSchema = value; }
    }

    public void LoadSchema(V2.Model.Message oMsg)
    {
      this.LoadSchema(oMsg.MessageVersion, oMsg.MessageType, oMsg.MessageTrigger);
    }
    public void LoadSchema(string MessageVersion, string MessageType, string MessageTrigger)
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

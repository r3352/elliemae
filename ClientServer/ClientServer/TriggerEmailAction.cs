// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerEmailAction
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerEmailAction : TriggerAction
  {
    private XmlList<TriggerEmailTemplate> templates = new XmlList<TriggerEmailTemplate>();

    public TriggerEmailAction()
    {
    }

    public TriggerEmailAction(TriggerEmailTemplate[] emailTemplates)
    {
      this.templates.AddRange((IEnumerable<TriggerEmailTemplate>) emailTemplates);
    }

    public TriggerEmailAction(XmlSerializationInfo info)
    {
      this.templates = (XmlList<TriggerEmailTemplate>) info.GetValue(nameof (templates), typeof (XmlList<TriggerEmailTemplate>));
    }

    public List<TriggerEmailTemplate> Templates => (List<TriggerEmailTemplate>) this.templates;

    public override TriggerActionType ActionType => TriggerActionType.Email;

    public override TriggerActivationEvent ActivationEvent => TriggerActivationEvent.LoanSaved;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("templates", (object) this.templates);
    }
  }
}

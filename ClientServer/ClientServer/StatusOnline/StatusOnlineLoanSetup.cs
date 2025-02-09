// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.StatusOnline.StatusOnlineLoanSetup
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.StatusOnline
{
  [Serializable]
  public class StatusOnlineLoanSetup : IXmlSerializable
  {
    private StatusOnlineTriggerCollection triggerList;
    private bool appliedPersonalTriggers;
    private bool showPrompt;

    public StatusOnlineLoanSetup()
    {
      this.triggerList = new StatusOnlineTriggerCollection();
      this.appliedPersonalTriggers = false;
      this.showPrompt = true;
    }

    public StatusOnlineLoanSetup(XmlSerializationInfo info)
    {
      this.triggerList = info.GetValue<StatusOnlineTriggerCollection>(nameof (Triggers));
      this.appliedPersonalTriggers = info.GetBoolean(nameof (AppliedPersonalTriggers), false);
      this.showPrompt = info.GetBoolean(nameof (ShowPrompt), true);
    }

    public StatusOnlineTriggerCollection Triggers => this.triggerList;

    public bool AppliedPersonalTriggers
    {
      get => this.appliedPersonalTriggers;
      set => this.appliedPersonalTriggers = value;
    }

    public bool ShowPrompt
    {
      get => this.showPrompt;
      set => this.showPrompt = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Triggers", (object) this.triggerList);
      info.AddValue("AppliedPersonalTriggers", (object) this.appliedPersonalTriggers);
      info.AddValue("ShowPrompt", (object) this.showPrompt);
    }
  }
}

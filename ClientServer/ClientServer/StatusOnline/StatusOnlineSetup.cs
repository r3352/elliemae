// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.StatusOnline.StatusOnlineSetup
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.StatusOnline
{
  [Serializable]
  public class StatusOnlineSetup : IXmlSerializable
  {
    private StatusOnlineTriggerCollection triggerList;
    private ApplyPersonalTriggersType personalTriggersType;

    public StatusOnlineSetup()
    {
      this.triggerList = new StatusOnlineTriggerCollection();
      this.personalTriggersType = ApplyPersonalTriggersType.FileStarter;
    }

    public StatusOnlineSetup(XmlSerializationInfo info)
    {
      this.triggerList = info.GetValue<StatusOnlineTriggerCollection>(nameof (Triggers));
      this.personalTriggersType = info.GetEnum<ApplyPersonalTriggersType>(nameof (PersonalTriggersType), ApplyPersonalTriggersType.FileStarter);
    }

    public StatusOnlineTriggerCollection Triggers => this.triggerList;

    public ApplyPersonalTriggersType PersonalTriggersType
    {
      get => this.personalTriggersType;
      set => this.personalTriggersType = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Triggers", (object) this.triggerList);
      info.AddValue("PersonalTriggersType", (object) this.personalTriggersType);
    }
  }
}

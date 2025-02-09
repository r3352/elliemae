// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.StatusOnline.LoanStatusTrigger
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.StatusOnline
{
  internal class LoanStatusTrigger : IXmlSerializable
  {
    private LoanStatusTriggerEnum triggerType;
    private string triggerId;

    public LoanStatusTrigger()
    {
      this.triggerType = LoanStatusTriggerEnum.None;
      this.triggerId = string.Empty;
    }

    public LoanStatusTrigger(XmlSerializationInfo info)
    {
      this.triggerType = (LoanStatusTriggerEnum) info.GetValue(nameof (triggerType), typeof (LoanStatusTriggerEnum));
      this.triggerId = info.GetString(nameof (triggerId));
    }

    public LoanStatusTriggerEnum TriggerType => this.triggerType;

    public string TriggerId => this.triggerId;

    public void GetXmlObjectData(XmlSerializationInfo info) => throw new NotSupportedException();
  }
}

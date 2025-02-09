// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerRateLockCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerRateLockCondition : TriggerCondition
  {
    private TriggerLockAction action;

    public TriggerRateLockCondition(TriggerLockAction action) => this.action = action;

    public TriggerRateLockCondition(XmlSerializationInfo info)
      : base(info)
    {
      this.action = (TriggerLockAction) info.GetValue(nameof (action), typeof (TriggerLockAction));
    }

    public TriggerLockAction Action => this.action;

    public override string[] GetActivationFields(ConfigInfoForTriggers activationData)
    {
      if (this.action == TriggerLockAction.Requested)
        return new string[1]{ "LOCKRATE.REQUESTCOUNT" };
      if (this.action == TriggerLockAction.Confirmed)
        return new string[1]{ "LOCKRATE.CONFIRMATIONCOUNT" };
      if (this.action != TriggerLockAction.Denied)
        return new string[0];
      return new string[1]{ "LOCKRATE.DENIALCOUNT" };
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("action", (object) this.action);
    }

    public override TriggerConditionType ConditionType
    {
      get
      {
        if (this.action == TriggerLockAction.Requested)
          return TriggerConditionType.LockRequested;
        return this.action == TriggerLockAction.Confirmed ? TriggerConditionType.LockConfirmed : TriggerConditionType.LockDenied;
      }
    }

    public override string ToString() => "Rate " + (object) this.action;
  }
}

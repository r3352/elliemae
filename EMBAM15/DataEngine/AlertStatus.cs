// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AlertStatus
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class AlertStatus
  {
    public readonly string AlertID;
    public readonly DateTime ActivationTime;
    public readonly string ActivationUser;
    public readonly DateTime DismissalTime;
    public readonly string DismissalUser;

    public AlertStatus(
      string alertId,
      DateTime activationTime,
      string activationUser,
      DateTime dismissalTime,
      string dismissalUser)
    {
      this.AlertID = alertId;
      this.ActivationTime = activationTime;
      this.ActivationUser = activationUser == "" ? (string) null : activationUser;
      this.DismissalTime = dismissalTime;
      this.DismissalUser = dismissalUser == "" ? (string) null : dismissalUser;
    }

    public bool Active => this.Activated && !this.Dismissed;

    public bool Activated => this.ActivationTime != DateTime.MinValue;

    public bool Dismissed => this.DismissalTime != DateTime.MinValue;
  }
}

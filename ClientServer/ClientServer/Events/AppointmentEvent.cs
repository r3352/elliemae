// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Events.AppointmentEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Events
{
  [Serializable]
  public class AppointmentEvent : SessionManagementEvent
  {
    private string dataKey;
    private AppointmentAction action;
    private string ownerID = "";

    public AppointmentEvent(
      SessionInfo sessionContext,
      string dataKey,
      AppointmentAction action,
      string ownerID)
      : base(sessionContext)
    {
      this.dataKey = dataKey;
      this.action = action;
      this.ownerID = ownerID;
    }

    public string DataKey => this.dataKey;

    public AppointmentAction Action => this.action;

    public string OwnerID => this.ownerID;
  }
}

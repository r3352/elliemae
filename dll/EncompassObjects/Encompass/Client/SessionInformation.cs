// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.SessionInformation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  public class SessionInformation : ISessionInformation
  {
    private SessionInfo sessionInfo;

    internal SessionInformation(SessionInfo sessionInfo) => this.sessionInfo = sessionInfo;

    public string SessionID => this.sessionInfo.SessionID;

    public string UserID => this.sessionInfo.UserID;

    public string ClientHostname => this.sessionInfo.Hostname;

    public string ClientIPAddress => this.sessionInfo.HostAddress.ToString();

    public DateTime LoginTime => this.sessionInfo.LoginTime;
  }
}

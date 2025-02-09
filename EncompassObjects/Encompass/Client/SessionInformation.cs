// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.SessionInformation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Provides descriptive information about an Encompass Session.
  /// </summary>
  public class SessionInformation : ISessionInformation
  {
    private SessionInfo sessionInfo;

    internal SessionInformation(SessionInfo sessionInfo) => this.sessionInfo = sessionInfo;

    /// <summary>Gets the unique session ID for the client's session.</summary>
    public string SessionID => this.sessionInfo.SessionID;

    /// <summary>Gets the User ID for the logged in user.</summary>
    public string UserID => this.sessionInfo.UserID;

    /// <summary>
    /// Gets the name of the machine from which the user is logged in.
    /// </summary>
    public string ClientHostname => this.sessionInfo.Hostname;

    /// <summary>
    /// Gets the IP address of the machine from which the user is logged in.
    /// </summary>
    public string ClientIPAddress => this.sessionInfo.HostAddress.ToString();

    /// <summary>Gets the time at which this session was started.</summary>
    public DateTime LoginTime => this.sessionInfo.LoginTime;
  }
}

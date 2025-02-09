// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SessionInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class SessionInfo
  {
    private ServerIdentity server;
    private string sessionId;
    private SessionType sessionType;
    private string userId;
    private string hostname;
    private IPAddress hostAddr;
    private DateTime loginTime;
    private string displayVersion;
    private string serverHostname;
    public SessionInfo.BasicLoanInfo CurrentLoanInfo;

    public SessionInfo(IClientSession session)
    {
      this.server = session.LoginParams.Server;
      this.sessionId = session.SessionID;
      this.sessionType = SessionType.Remoting;
      this.userId = session.LoginParams.UserID;
      this.hostname = session.Hostname;
      this.hostAddr = session.HostAddress;
      this.loginTime = session.LoginTime;
      this.displayVersion = session.DisplayVersion;
      this.serverHostname = "";
    }

    public SessionInfo(
      string serverUri,
      string sessionId,
      SessionType sessionType,
      string userId,
      string hostname,
      IPAddress hostAddr,
      string displayVersion,
      DateTime loginTime,
      string serverHostname = "�")
      : this((ServerIdentity) null, sessionId, sessionType, userId, hostname, hostAddr, displayVersion, loginTime, serverHostname)
    {
      if ((serverUri ?? "").Trim() == "")
        return;
      try
      {
        this.server = ServerIdentity.Parse(serverUri);
      }
      catch
      {
      }
    }

    public SessionInfo(
      ServerIdentity serverIdentity,
      string sessionId,
      SessionType sessionType,
      string userId,
      string hostname,
      IPAddress hostAddr,
      string displayVersion,
      DateTime loginTime,
      string serverHostname = "�")
    {
      this.server = serverIdentity;
      this.sessionId = sessionId;
      this.sessionType = sessionType;
      this.userId = userId;
      this.hostname = hostname;
      this.hostAddr = hostAddr;
      this.loginTime = loginTime;
      this.displayVersion = displayVersion;
      this.serverHostname = serverHostname;
    }

    public string SessionID => this.sessionId;

    public SessionType SessionType => this.sessionType;

    public string UserID => this.userId;

    public ServerIdentity Server => this.server;

    public string Hostname => this.hostname;

    public IPAddress HostAddress => this.hostAddr;

    public string DisplayVersion => this.displayVersion;

    public DateTime LoginTime => this.loginTime;

    public string ServerHostname => this.serverHostname ?? "";

    public int SessionObjectCount { get; set; }

    public override string ToString() => this.UserID + "/" + this.SessionID;

    [Serializable]
    public class BasicLoanInfo
    {
      public readonly string LoanGUID;
      public readonly string BorrowerLastName;
      public readonly string BorrowerFirstName;
      public readonly string LoanNumber;

      public BasicLoanInfo(
        string loanGUID,
        string borrowerLastName,
        string borrowerFirstName,
        string loanNumber)
      {
        this.LoanGUID = loanGUID;
        this.BorrowerLastName = borrowerLastName;
        this.BorrowerFirstName = borrowerFirstName;
        this.LoanNumber = loanNumber;
      }
    }
  }
}

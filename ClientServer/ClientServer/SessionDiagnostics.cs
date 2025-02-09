// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SessionDiagnostics
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class SessionDiagnostics
  {
    private string instanceName;
    private string sessionId;
    private string userId;
    private string appName;
    private string lastApiSignature;
    private DateTime lastApiTimestamp = DateTime.MinValue;
    private int apiCallCount;

    public SessionDiagnostics(
      string instanceName,
      string sessionId,
      string userId,
      string appName)
    {
      this.instanceName = instanceName;
      this.sessionId = sessionId;
      this.userId = userId;
      this.appName = appName;
    }

    public string InstanceName => this.instanceName;

    public string SessionID => this.sessionId;

    public string UserID => this.userId;

    public string ApplicationName => this.appName;

    public string LastAPISignature => this.lastApiSignature;

    public DateTime LastAPITimestamp => this.lastApiTimestamp;

    public int APICount => this.apiCallCount;

    public void RecordAPICall(string signature)
    {
      lock (this)
      {
        this.lastApiSignature = signature;
        this.lastApiTimestamp = DateTime.Now;
        ++this.apiCallCount;
      }
    }

    public SessionDiagnostics Clone()
    {
      lock (this)
        return (SessionDiagnostics) this.MemberwiseClone();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Connection
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class Connection : IComparable<Connection>
  {
    private const string className = "Connection�";
    private string connectionId;
    private string hostname;
    private DateTime lastEcho = DateTime.Now;
    private List<WeakReference> sessions = new List<WeakReference>();

    public Connection(string connectionId, string hostname)
    {
      this.connectionId = connectionId;
      this.hostname = hostname;
    }

    public string ConnectionID => this.connectionId;

    public string HostName => this.hostname;

    public int SessionCount
    {
      get
      {
        lock (this.sessions)
          return this.sessions.Count;
      }
    }

    public void AddSession(IClientSession s)
    {
      lock (this.sessions)
        this.sessions.Add(new WeakReference((object) s));
    }

    public void RemoveSession(IClientSession s)
    {
      lock (this.sessions)
      {
        foreach (WeakReference session in this.sessions)
        {
          IClientSession target = (IClientSession) session.Target;
          if (target != null && target.SessionID == s.SessionID)
          {
            this.sessions.Remove(session);
            break;
          }
        }
      }
    }

    public DateTime LastEchoTime
    {
      get
      {
        lock (this)
          return this.lastEcho;
      }
      set
      {
        lock (this)
          this.lastEcho = value;
      }
    }

    public int TerminateAllSessions()
    {
      lock (this.sessions)
      {
        int num = 0;
        foreach (WeakReference session in this.sessions)
        {
          IClientSession target = (IClientSession) session.Target;
          try
          {
            if (target != null)
            {
              target.Terminate(DisconnectEventArgument.Force, "Connection timed out");
              TraceLog.WriteInfo(nameof (Connection), "Terminated session " + target.SessionID);
              ++num;
            }
          }
          catch (Exception ex)
          {
            TraceLog.WriteError(nameof (Connection), "Error terminating session " + target.SessionID + ": " + (object) ex);
          }
        }
        this.sessions.Clear();
        return num;
      }
    }

    public override string ToString() => this.HostName + "/" + this.ConnectionID;

    public int CompareTo(Connection other) => this.lastEcho.CompareTo(other.lastEcho);
  }
}

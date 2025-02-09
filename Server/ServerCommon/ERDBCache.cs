// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerCommon.ERDBCache
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ReportingDbUtils.Interfaces;

#nullable disable
namespace EllieMae.EMLite.Server.ServerCommon
{
  public class ERDBCache
  {
    private string encInstanceName;
    private ClientContext context;
    private string encSysID;
    private string clientID;
    private string erdbServer;
    private int erdbServerPort = 11099;
    private const string erdbCacheName = "ERDBCache�";
    private IERDBRegistrationMgr _erdbRegMgr;
    private IERDBSession erdbSession;

    public string EncompassSystemID => this.encSysID;

    public string ClientID => this.clientID;

    public string ERDBServer => this.erdbServer;

    public int ERDBServerPort => this.erdbServerPort;

    public IERDBRegistrationMgr ERDBRegistrationMgr
    {
      get
      {
        if (this._erdbRegMgr == null)
        {
          ERDBServerUtil erdbServerUtil = new ERDBServerUtil(this.context);
          this.erdbServer = erdbServerUtil.ERDBServer;
          this.erdbServerPort = erdbServerUtil.ERDBServerPort;
          this._erdbRegMgr = erdbServerUtil.GetRegistrationManager();
          if (this.context != null)
          {
            using (this.context.Cache.Lock(nameof (ERDBCache)))
            {
              this.context.Cache.Remove(nameof (ERDBCache));
              this.context.Cache.Put(nameof (ERDBCache), (object) this);
            }
          }
        }
        return this._erdbRegMgr;
      }
    }

    public IERDBSession ERDBSession
    {
      get
      {
        if (this.erdbSession == null)
          this.erdbSession = this.ERDBRegistrationMgr.GetERDBSession(this.clientID, this.encSysID);
        return this.erdbSession;
      }
    }

    internal void ResetRemotingInterfaces()
    {
      this._erdbRegMgr = (IERDBRegistrationMgr) null;
      this.erdbSession = (IERDBSession) null;
    }

    internal ERDBCache(
      ClientContext context,
      string encInstanceName,
      string clientID,
      string encSysID)
    {
      this.context = context;
      this.encSysID = encSysID;
      this.clientID = clientID;
      this.encInstanceName = (encInstanceName ?? "").Trim();
    }

    public void DisconnectRemotingObjects()
    {
      try
      {
        if (this.erdbSession != null)
          this.erdbSession.Disconnect();
      }
      catch
      {
      }
      this.erdbSession = (IERDBSession) null;
      try
      {
        if (this._erdbRegMgr != null)
          this._erdbRegMgr.Disconnect();
      }
      catch
      {
      }
      this._erdbRegMgr = (IERDBRegistrationMgr) null;
    }
  }
}

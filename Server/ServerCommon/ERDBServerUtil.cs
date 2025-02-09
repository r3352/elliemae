// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerCommon.ERDBServerUtil
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ReportingDbUtils.Basics;
using EllieMae.EMLite.ReportingDbUtils.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerCommon
{
  public class ERDBServerUtil
  {
    private ClientContext context;
    private string erdbServer;
    private int erdbServerPort = -1;

    internal ERDBServerUtil(ClientContext context) => this.context = context;

    private void getERDBServerInfo()
    {
      Dictionary<string, object> registrationInfo = ERDBSession.GetERDBRegistrationInfo((IClientContext) this.context);
      this.erdbServer = (string) registrationInfo["AppServer"];
      this.erdbServerPort = 11099;
      try
      {
        this.erdbServerPort = Convert.ToInt32((string) registrationInfo["Port"]);
      }
      catch
      {
      }
    }

    public string ERDBServer
    {
      get
      {
        if (this.erdbServer == null)
          this.getERDBServerInfo();
        return this.erdbServer;
      }
    }

    public int ERDBServerPort
    {
      get
      {
        if (this.erdbServerPort < 0)
          this.getERDBServerInfo();
        return this.erdbServerPort;
      }
    }

    public IERDBRegistrationMgr GetRegistrationManager()
    {
      if (!RemotingUtil.ERDBClientChannelRegistered)
        RemotingUtil.RegisterERDBClientChannel();
      return (IERDBRegistrationMgr) Activator.GetObject(typeof (IERDBRegistrationMgr), "gtcp://" + this.ERDBServer + ":" + (object) this.ERDBServerPort + "/RegistrationManager.rem", (object) null);
    }
  }
}

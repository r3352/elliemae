// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServerInstanceInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ServerInstanceInfo
  {
    private string instanceName;
    private bool enabled;
    private int sessionCount;
    private int sessionObjCount;

    public ServerInstanceInfo(
      string instanceName,
      bool enabled,
      int sessionCount,
      int sessionObjCount)
    {
      this.instanceName = instanceName;
      this.enabled = enabled;
      this.sessionCount = sessionCount;
      this.sessionObjCount = sessionObjCount;
    }

    public string InstanceName => this.instanceName;

    public bool Enabled => this.enabled;

    public int SessionCount => this.sessionCount;

    public int SessionObjectCount => this.sessionObjCount;

    public override string ToString() => this.instanceName == "" ? "<Default>" : this.instanceName;
  }
}

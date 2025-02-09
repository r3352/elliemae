// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoConnectCustomServiceInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoConnectCustomServiceInfo : IComparable<LoConnectCustomServiceInfo>
  {
    private string customentityId;
    private LoServiceType servicetype;
    private bool access;

    public LoConnectCustomServiceInfo(
      string customentityId,
      LoServiceType serviceType,
      bool access = false)
    {
      this.customentityId = customentityId;
      this.servicetype = serviceType;
      this.access = access;
    }

    public string CustomEntityId => this.customentityId;

    public LoServiceType ServiceType => this.servicetype;

    public bool Access => this.access;

    public int CompareTo(LoConnectCustomServiceInfo obj)
    {
      return this.CustomEntityId.CompareTo(obj.CustomEntityId);
    }
  }
}

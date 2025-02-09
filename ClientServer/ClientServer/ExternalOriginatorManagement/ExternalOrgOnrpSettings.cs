// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.ExternalOrgOnrpSettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  [Serializable]
  public class ExternalOrgOnrpSettings
  {
    public const string TimeZoneString = "ET�";
    public int ONRPID = -1;
    public ONRPEntitySettings Broker = new ONRPEntitySettings();
    public ONRPEntitySettings Correspondent = new ONRPEntitySettings();

    public ExternalOrgOnrpSettings Clone()
    {
      return new ExternalOrgOnrpSettings()
      {
        Broker = this.Broker.Clone(),
        Correspondent = this.Correspondent.Clone()
      };
    }
  }
}

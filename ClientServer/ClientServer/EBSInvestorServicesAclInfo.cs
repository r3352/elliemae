// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EBSInvestorServicesAclInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class EBSInvestorServicesAclInfo
  {
    public AclFeature Feature { get; private set; }

    public InvestorServiceAclInfo.InvestorServicesDefaultSetting DefaultAccess { get; set; }

    public Dictionary<string, bool> AccessToProviders { get; private set; }

    public string Category { get; set; }

    public EBSInvestorServicesAclInfo(AclFeature feature)
    {
      this.Feature = feature;
      this.DefaultAccess = InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified;
      this.AccessToProviders = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    }
  }
}

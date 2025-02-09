// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPC2.ServiceSetupResult
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.EPC2
{
  [Serializable]
  public class ServiceSetupResult
  {
    public ProductInfo ProductInfo = new ProductInfo();

    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public VendorPlatform VendorPlatform { get; set; }

    public string InstanceId { get; set; }

    public OrderTypeEnum OrderType { get; set; }

    public bool IsActive { get; set; }

    public string ProductName { get; set; }

    public string ProviderId { get; set; }

    public string Version { get; set; }

    public string Category { get; set; }

    public string PartnerId { get; set; }

    public int Rank { get; set; }

    public List<AuthorizedUser> AuthorizedUsers { get; set; }

    public List<object> PreConfiguredOptions { get; set; }

    public DateTime Created { get; set; }

    public string CreatedBy { get; set; }

    public ScopeEnum Scope { get; set; }

    public bool IsPreconfigured { get; set; }
  }
}

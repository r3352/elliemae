// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPC2.ProductInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.EPC2
{
  [Serializable]
  public class ProductInfo
  {
    public string ProductName { get; set; }

    public string ListingName { get; set; }

    public VendorPlatform VendorPlatform { get; set; }

    public string ConfigurationName { get; set; }

    public string IsGenerallyAvailable { get; set; }

    public string ActiveVersion { get; set; }

    public string OriginWorkflow { get; set; }
  }
}

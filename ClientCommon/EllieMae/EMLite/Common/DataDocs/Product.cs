// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DataDocs.Product
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.Common.DataDocs
{
  public class Product
  {
    [JsonProperty("entityRef")]
    public EntityRef EntityRef { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("clientEntity")]
    public ClientEntity ClientEntity { get; set; }
  }
}

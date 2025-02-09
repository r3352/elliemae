// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DataDocs.Provider
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.Common.DataDocs
{
  public class Provider
  {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("category")]
    public string Category { get; set; }

    [JsonProperty("credentialStore")]
    public string CredentialStore { get; set; }

    [JsonProperty("product")]
    public string Product { get; set; }

    [JsonProperty("configurationName")]
    public string ConfigurationName { get; set; }

    [JsonProperty("activeVersion")]
    public int ActiveVersion { get; set; }

    [JsonProperty("options")]
    public Options options { get; set; }
  }
}

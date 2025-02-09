// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DataDocs.SystemOptionsValidatorResponse
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.Common.DataDocs
{
  public class SystemOptionsValidatorResponse
  {
    [JsonProperty("category")]
    public string Category { get; set; }

    [JsonProperty("attribute")]
    public string Attribute { get; set; }

    [JsonProperty("exists")]
    public bool Exists { get; set; }
  }
}

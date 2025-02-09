// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DataDocs.CustomDataField
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.Common.DataDocs
{
  public class CustomDataField
  {
    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("jsonPath")]
    public string JsonPath { get; set; }
  }
}

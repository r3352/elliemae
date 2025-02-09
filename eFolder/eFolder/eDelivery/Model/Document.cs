// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.Model.Document
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.Model
{
  public class Document
  {
    public string id { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string mediaId { get; set; }

    public string title { get; set; }

    public string type { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int pages { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int size { get; set; }

    [JsonIgnore]
    public string filePath { get; set; }
  }
}

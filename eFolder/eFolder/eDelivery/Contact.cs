// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.Contact
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class Contact
  {
    public string contactType { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string borrowerId { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string authType { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string authCode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string userId { get; set; }

    [JsonIgnore]
    public string url { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string recipientId { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string name { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string email { get; set; }
  }
}

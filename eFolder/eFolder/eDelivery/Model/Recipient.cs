// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.Model.Recipient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.Model
{
  public class Recipient
  {
    public string id { get; set; }

    [JsonIgnore]
    public string borrowerId { get; set; }

    [JsonIgnore]
    public string url { get; set; }

    [JsonIgnore]
    public string userId { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string role { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string fullName { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Email createdEmail { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string sendESignatureNotification { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string email { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int routingOrder { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<Signingpoint> documents { get; set; }

    [JsonIgnore]
    public string authCode { get; set; }

    [JsonIgnore]
    public string recipientId { get; set; }
  }
}

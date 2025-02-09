// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.Request
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class Request
  {
    [JsonIgnore]
    public Caller caller { get; set; }

    [JsonIgnore]
    public string applicationGroupId { get; set; }

    [JsonIgnore]
    public string packageId { get; set; }

    public User from { get; set; }

    public bool notifyWhenViewed { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public NotViewed notViewed { get; set; }

    public List<Recipient> recipients { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<Document> documents { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Fulfillment fulfillment { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, string> custom { get; set; }

    [JsonIgnore]
    public string consumerConnectSiteID { get; set; }
  }
}

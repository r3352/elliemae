// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.Model.User
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.Model
{
  public class User
  {
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string[] recipientIds { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string name { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string email { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string street { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string city { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string state { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string zipCode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string phone { get; set; }
  }
}

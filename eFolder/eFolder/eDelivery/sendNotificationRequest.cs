// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.sendNotificationRequest
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;
using System;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class sendNotificationRequest
  {
    public string body { get; set; }

    public string contentType { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string createdBy { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string createdDate { get; set; }

    public string[] emails { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string modifiedBy { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string modifiedDate { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string replyTo { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string senderFullName { get; set; }

    public string subject { get; set; }

    [JsonIgnore]
    public Guid loanGuid { get; set; }
  }
}

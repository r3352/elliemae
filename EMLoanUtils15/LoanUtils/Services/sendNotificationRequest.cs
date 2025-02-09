// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.sendNotificationRequest
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using Newtonsoft.Json;
using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Services
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

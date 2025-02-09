// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Util.TrackingDetail
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Util
{
  public class TrackingDetail
  {
    [JsonProperty("packageId")]
    public string PackageID { get; set; }

    [JsonProperty("eSignComplete")]
    public bool eSignComplete { get; set; }

    [JsonProperty("notificationDate")]
    public string notificationDate { get; set; }

    public string NotificationDate => Utils.ConvertToPST(this.notificationDate);

    [JsonProperty("createdInformation")]
    public CreatedInformation CreatedInformation { get; set; }

    [JsonProperty("userDetails")]
    public List<UserDetail> UserDetails { get; set; }

    [JsonProperty("fulfillmentDetails")]
    public FulfillmentDetail FulfillmentDetails { get; set; }

    [JsonProperty("consentPdf")]
    public ConsentPDF ConsentPDF { get; set; }
  }
}

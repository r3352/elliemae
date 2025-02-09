// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.EDelivery.EDeliveryTrackingDetail
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.EDelivery
{
  public class EDeliveryTrackingDetail
  {
    public string packageId;
    public bool eSignComplete;
    public string notificationDate;
    public CreatedInformation createdInformation;
    public List<EDeliveryUserDetail> userDetails;
    public FulfillmentDetail fulfillmentDetails;
    public ConsentPdfOutput consentPdf;

    public string GetNotificationDate() => Utils.ConvertToPST(this.notificationDate);
  }
}

// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.SubmitOrderResult
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [SoapInclude(typeof (ClosingMarketResult))]
  [SoapType("SubmitOrderResult", "http://www.closingmarket.com")]
  public class SubmitOrderResult : SerializableObject
  {
    public int ClosingMarketTransactionID;
    public string ProviderOrderID;
    public string ProviderDetailResponse;
    public string TradingPartnerResponseHtml;
    public string TransactionDescription;
  }
}

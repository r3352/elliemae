// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Classes.ServiceUrls
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Classes
{
  [Serializable]
  public class ServiceUrls
  {
    private Dictionary<string, string> _urls { get; set; } = new Dictionary<string, string>();

    public string UpdateServicesUrl { get; set; }

    public string DataServicesUrl { get; set; }

    public string LoginServicesUrl { get; set; }

    public string JedServicesUrl { get; set; }

    public string LoanCenterServiceUrl { get; set; }

    public string CenterwiseServicesUrl { get; set; }

    public string DataServices2Url { get; set; }

    public string ERDBWebServiceUrl { get; set; }

    public string MaventServiceUrl { get; set; }

    public string HomePageUrl { get; set; }

    public string PPEServiceUrl { get; set; }

    public string ePassAServiceUrl { get; set; }

    public string DocServiceUrl { get; set; }

    public string ImageLibraryServiceUrl { get; set; }

    public string IDRServiceUrl { get; set; }

    public string InboxServiceUrl { get; set; }

    public string FulfillmentServiceUrl { get; set; }

    public string CompanySettingsServiceUrl { get; set; }

    public string EVaultRetrieveServiceUrl { get; set; }

    public string MaventProductDocsUrl { get; set; }

    public string ECSProductsUrl { get; set; }

    public string LogTransactionUrl { get; set; }

    public string EPackageServiceUrl { get; set; }

    public string DownloadServiceUrl { get; set; }

    public string EpassAiUrl { get; set; }

    public string LicenseUrl { get; set; }

    public string CustomerLoyalty { get; set; }

    public string ConsentServiceUrl { get; set; }

    public string EmailNotificationUrl { get; set; }

    public string DocumentClassificationUrl { get; set; }

    public string EppsPricingUrl { get; set; }

    public string EppsWebserviceUrl { get; set; }

    public string EppsCoreContourUrl { get; set; }

    public string LenderCenterLogTransactionUrl { get; set; }

    public string MiCenterUrl { get; set; }

    public string LoanCenterLogTransactionUrl { get; set; }

    public string ePassGetMessageAlertsUrl { get; set; }

    public string EcloseSetupUrl { get; set; }

    public string TqlAvmServiceUrl { get; set; }

    public string TqlFloodServiceUrl { get; set; }

    public string TqlFloodCoreLogicServiceUrl { get; set; }

    public string TqlFloodLpsServiceUrl { get; set; }

    public string TqlFloodVendorServiceUrl { get; set; }

    public string TqlFraudServiceUrl { get; set; }

    public string TqlTaxReturnServiceUrl { get; set; }

    public string TqlDemoVendorServiceUrl { get; set; }

    public string TqlNotificationServiceUrl { get; set; }

    public string TqlPlatformServiceUrl { get; set; }

    public string TqlVendorContentServiceUrl { get; set; }

    public string TqlVendorGatewayServiceUrl { get; set; }

    public string AppraisalCenterBaseUrl { get; set; }

    public string AppraisalCenterServiceUrl { get; set; }

    public string TitleCenterBaseUrl { get; set; }

    public string TitleCenterServiceUrl { get; set; }

    public string MisAppraisalBillingInterfaceServiceUrl { get; set; }

    public string MisTitleBillingInterfaceServiceUrl { get; set; }

    public string MarketPlaceUrl { get; set; }

    public string EncompassCRMUrl { get; set; }

    public string ResourceCenterUrl { get; set; }

    public string EllieMaeResourceCenterUrl { get; set; }

    public string MessageListUrl { get; set; }

    public string CBSMarketWatchUrl { get; set; }

    public string EncompassWebcenterUrl { get; set; }

    public string EducationUrl { get; set; }

    public string LogServicesUrl { get; set; }

    public string SSFHostUrl { get; set; }

    public Dictionary<string, string> ToDictionary()
    {
      Dictionary<string, string> urls = this._urls;
      // ISSUE: explicit non-virtual call
      if ((urls != null ? (__nonvirtual (urls.Count) == 0 ? 1 : 0) : 0) != 0)
      {
        try
        {
          this._urls = ((IEnumerable<PropertyInfo>) this.GetType().GetProperties()).ToDictionary<PropertyInfo, string, string>((Func<PropertyInfo, string>) (p => p.Name), (Func<PropertyInfo, string>) (p => Convert.ToString(p.GetValue((object) this))));
        }
        catch (Exception ex)
        {
        }
      }
      return this._urls;
    }
  }
}

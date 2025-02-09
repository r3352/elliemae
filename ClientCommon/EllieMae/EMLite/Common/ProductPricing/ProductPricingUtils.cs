// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ProductPricing.ProductPricingUtils
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Common.ProductPricing
{
  public class ProductPricingUtils
  {
    private static string className = nameof (ProductPricingUtils);
    private static readonly string sw = Tracing.SwCustomLetters;
    private static bool? isHistoricalPricingEnabledVal = new bool?();

    public static void SynchronizeProductPricingSettingsWithServer()
    {
      ProductPricingUtils.SynchronizeProductPricingSettingsWithServer(Session.DefaultInstance);
    }

    public static async void SynchronizeProductPricingSettingsWithServer(Sessions.Session session)
    {
      try
      {
        string currentSelectedProvider = session.SessionObjects.GetCompanySettingFromCache("POLICIES", "PRICING_PARTNER");
        if (session.StartupInfo.ProductPricingPartner == null && currentSelectedProvider == "")
          return;
        string typeAndToken = new OAuth2(session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(session.SessionObjects)).GetAccessToken(Session.DefaultInstance.ServerIdentity?.InstanceName, session.SessionID, "sc")?.TypeAndToken;
        if (string.IsNullOrEmpty(typeAndToken))
        {
          Tracing.Log(ProductPricingUtils.sw, TraceLevel.Error, ProductPricingUtils.className, "No Access Token was generated when calling SynchronizeProductPricingSettingsWithServer. ");
        }
        else
        {
          Task<List<Epc2Provider>> epcGetProvidersTask = Epc2ServiceClient.GetProviderList(session.SessionObjects, typeAndToken, new string[1]
          {
            "PRODUCTPRICING"
          }, "Encompass Smart Client");
          string clientId = session.CompanyInfo.ClientID;
          string instanceName = session?.ServerIdentity?.InstanceName;
          if (!string.IsNullOrWhiteSpace(instanceName) && (instanceName.StartsWith("TEBE", StringComparison.InvariantCultureIgnoreCase) || instanceName.StartsWith("DEBE", StringComparison.InvariantCultureIgnoreCase)))
            clientId = instanceName;
          Task<Partner[]> emnGetPartnersTask = Task.Run<Partner[]>((Func<Partner[]>) (() => new PPEservice(session?.SessionObjects?.StartupInfo?.ServiceUrls?.PPEServiceUrl).GetPartners(clientId)));
          await Task.WhenAll((Task) epcGetProvidersTask, (Task) emnGetPartnersTask);
          Partner[] result1 = emnGetPartnersTask.Result;
          List<Epc2Provider> result2 = epcGetProvidersTask.Result;
          string settingFromCache = Session.SessionObjects.GetCompanySettingFromCache("POLICIES", "USE.LOCK.REQUEST.FIELDS");
          session.ConfigurationManager.SetCompanySetting("POLICIES", "PRICING_PARTNER", "");
          session.ConfigurationManager.SetCompanySetting("POLICIES", "PRICING_PARTNER_SELL_SIDE_SHOW", "");
          session.ConfigurationManager.SetCompanySetting("POLICIES", "USE.LOCK.REQUEST.FIELDS", "");
          bool requireUpdate = false;
          List<ProductPricingSetting> settings = ProductPricingUtils.MergeProviders(session, result1, result2, settingFromCache, currentSelectedProvider, ref requireUpdate);
          // ISSUE: explicit non-virtual call
          if (requireUpdate && settings != null && __nonvirtual (settings.Count) > 0)
          {
            session.ConfigurationManager.UpdateProductPricingSettings(settings);
            session.StartupInfo.ProductPricingPartner = session.ConfigurationManager.GetActiveProductPricingPartner();
          }
          currentSelectedProvider = (string) null;
          epcGetProvidersTask = (Task<List<Epc2Provider>>) null;
          emnGetPartnersTask = (Task<Partner[]>) null;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ProductPricingUtils.sw, TraceLevel.Error, ProductPricingUtils.className, "Error occurred when calling SynchronizeProductPricingSettingsWithServer. " + ex.StackTrace);
      }
    }

    public static List<ProductPricingSetting> MergeProviders(
      Sessions.Session session,
      Partner[] partnerList,
      List<Epc2Provider> providerList,
      string useLagacy,
      string currentSelectedProvider,
      ref bool requireUpdate)
    {
      List<ProductPricingSetting> productPricingSettings = session.ConfigurationManager.GetProductPricingSettings();
      List<ProductPricingSetting> mergedSetting = new List<ProductPricingSetting>();
      ProductPricingUtils.MergeEpc2ProviderList(providerList, productPricingSettings.Where<ProductPricingSetting>((Func<ProductPricingSetting, bool>) (s => s.VendorPlatform == VendorPlatform.EPC2)).ToList<ProductPricingSetting>(), useLagacy, currentSelectedProvider, ref requireUpdate, ref mergedSetting);
      ProductPricingUtils.MergeEmnPartnerList(session, partnerList, productPricingSettings.Where<ProductPricingSetting>((Func<ProductPricingSetting, bool>) (s => s.VendorPlatform == VendorPlatform.EMN)).ToList<ProductPricingSetting>(), useLagacy, currentSelectedProvider, ref requireUpdate, ref mergedSetting);
      return !requireUpdate ? productPricingSettings : mergedSetting;
    }

    private static void MergeEpc2ProviderList(
      List<Epc2Provider> providerList,
      List<ProductPricingSetting> currentSettings,
      string useLagacy,
      string currentSelectedProvider,
      ref bool requireUpdate,
      ref List<ProductPricingSetting> mergedSetting)
    {
      if (providerList == null)
        return;
      int count1 = providerList.Count;
      int? count2 = currentSettings?.Count;
      int valueOrDefault = count2.GetValueOrDefault();
      if (!(count1 == valueOrDefault & count2.HasValue))
        requireUpdate = true;
      Func<string, string> func = (Func<string, string>) (listingName => listingName.Length <= 60 ? listingName : listingName.Substring(0, 60));
      string str = string.Empty;
      foreach (Epc2Provider provider1 in providerList)
      {
        Epc2Provider provider = provider1;
        ProductPricingSetting productPricingSetting1 = (ProductPricingSetting) null;
        ProductPricingSetting productPricingSetting2 = currentSettings != null ? currentSettings.FirstOrDefault<ProductPricingSetting>((Func<ProductPricingSetting, bool>) (s => s.ProviderID == provider.Id)) : (ProductPricingSetting) null;
        if (productPricingSetting2 != null)
        {
          productPricingSetting1 = productPricingSetting2.Clone();
          productPricingSetting1.PartnerName = func(provider.ListingName);
          productPricingSetting1.VendorPlatform = VendorPlatform.EPC2;
          productPricingSetting1.SupportEnableAutoLockRequest = productPricingSetting1.IsEPPS || productPricingSetting1.SupportEnableAutoLockRequest;
          productPricingSetting1.ShowSellSide = productPricingSetting1.VendorPlatform == VendorPlatform.EPC2;
          if (currentSelectedProvider != "")
          {
            productPricingSetting1.Active = productPricingSetting1.ProviderID == currentSelectedProvider;
            if (productPricingSetting1.ProviderID == currentSelectedProvider && useLagacy != "")
              productPricingSetting1.ImportToLoan = useLagacy == "n";
          }
          requireUpdate |= !productPricingSetting1.Equals((object) productPricingSetting2);
        }
        if (productPricingSetting1 == null)
        {
          productPricingSetting1 = new ProductPricingSetting(provider.Id, provider.PartnerId, VendorPlatform.EPC2, "", func(provider.ListingName), "", "", false, false, false, false, "", "", "", "", "", "", "", "", "");
          if (string.IsNullOrWhiteSpace(str))
            str = Session.ConfigurationManager.GetCompanySetting("POLICIES", "EPPSPartnerID");
          productPricingSetting1.EppsPartnerId = str;
          productPricingSetting1.SupportEnableAutoLockRequest = productPricingSetting1.IsEPPS;
          productPricingSetting1.ShowSellSide = productPricingSetting1.VendorPlatform == VendorPlatform.EPC2;
          productPricingSetting1.SupportImportToLoan = productPricingSetting1.IsEPPS;
          productPricingSetting1.Active = productPricingSetting1.ProviderID == currentSelectedProvider;
          if (productPricingSetting1.ProviderID == currentSelectedProvider && useLagacy != "")
            productPricingSetting1.ImportToLoan = useLagacy == "n";
          requireUpdate = true;
        }
        mergedSetting.Add(productPricingSetting1);
      }
    }

    private static void MergeEmnPartnerList(
      Sessions.Session session,
      Partner[] partnerList,
      List<ProductPricingSetting> currentSettings,
      string useLagacy,
      string currentSelectedProvider,
      ref bool requireUpdate,
      ref List<ProductPricingSetting> mergedSetting)
    {
      if (partnerList == null)
        return;
      int num = ((IEnumerable<Partner>) partnerList).Count<Partner>();
      int? count = currentSettings?.Count;
      int valueOrDefault = count.GetValueOrDefault();
      if (!(num == valueOrDefault & count.HasValue))
        requireUpdate = true;
      string companySetting = session.ConfigurationManager.GetCompanySetting("POLICIES", "EPPS_EPC2_SHIP_DARK");
      bool flag = string.IsNullOrEmpty(companySetting) || string.Equals(companySetting, "true", StringComparison.CurrentCultureIgnoreCase);
      foreach (Partner partner1 in partnerList)
      {
        Partner partner = partner1;
        ProductPricingSetting productPricingSetting1 = (ProductPricingSetting) null;
        ProductPricingSetting productPricingSetting2 = currentSettings != null ? currentSettings.FirstOrDefault<ProductPricingSetting>((Func<ProductPricingSetting, bool>) (s => s.ProviderID == partner.PartnerID)) : (ProductPricingSetting) null;
        if (productPricingSetting2 != null)
        {
          productPricingSetting1 = productPricingSetting2.Clone();
          productPricingSetting1.AdminURL = partner.AdminURL;
          productPricingSetting1.MoreInfoURL = partner.MoreInfoURL;
          productPricingSetting1.PartnerName = partner.Name;
          productPricingSetting1.SettingsSection = partner.SettingsSection;
          productPricingSetting1.ShowSellSide = partner.ShowSellSide;
          productPricingSetting1.SupportImportToLoan = partner.SupportsImportToLoan;
          productPricingSetting1.SupportPartnerLockConfirm = partner.SupportsPartnerLockConfirm;
          productPricingSetting1.SupportPartnerRequestLock = partner.SupportsPartnerRequestLock;
          productPricingSetting1.IsCustomizeInvestorName = partner.IsCustomizeInvestorName;
          productPricingSetting1.VendorPlatform = VendorPlatform.EMN;
          productPricingSetting1.SupportEnableAutoLockRequest = productPricingSetting1.IsEPPS || productPricingSetting1.SupportEnableAutoLockRequest;
          if (currentSelectedProvider != "")
          {
            productPricingSetting1.Active = productPricingSetting1.ProviderID == currentSelectedProvider;
            if (productPricingSetting1.ProviderID == currentSelectedProvider && useLagacy != "")
              productPricingSetting1.ImportToLoan = useLagacy == "n";
          }
          if (productPricingSetting1.IsEPPS)
            productPricingSetting1.MoreInfoURL = "https://www.icemortgagetechnology.com/products/ice-product-and-pricing-engine";
          requireUpdate |= !productPricingSetting1.Equals((object) productPricingSetting2);
          mergedSetting.Add(productPricingSetting1);
        }
        if (productPricingSetting1 == null)
        {
          ProductPricingSetting productPricingSetting3 = new ProductPricingSetting(partner.PartnerID, partner.PartnerID, VendorPlatform.EMN, partner.SettingsSection, partner.Name, partner.AdminURL, partner.MoreInfoURL, partner.SupportsImportToLoan, partner.SupportsPartnerRequestLock, partner.SupportsPartnerLockConfirm, partner.ShowSellSide, "", "", "", "", "", "", "", "", "");
          productPricingSetting3.SupportEnableAutoLockRequest = productPricingSetting3.IsEPPS;
          productPricingSetting3.Active = productPricingSetting3.ProviderID == currentSelectedProvider;
          if (productPricingSetting3.PartnerID == currentSelectedProvider && useLagacy != "")
            productPricingSetting3.ImportToLoan = useLagacy == "n";
          if (productPricingSetting3.IsEPPS)
            productPricingSetting3.MoreInfoURL = "https://www.icemortgagetechnology.com/products/ice-product-and-pricing-engine";
          requireUpdate = true;
          mergedSetting.Add(productPricingSetting3);
        }
      }
    }

    public static string GetPartnerId(ProductPricingSetting setting)
    {
      string partnerId = setting.PartnerID;
      if (setting.IsEPPS)
        partnerId = "MPS";
      return partnerId;
    }

    public static bool IsHistoricalPricingEnabled
    {
      get
      {
        if (ProductPricingUtils.isHistoricalPricingEnabledVal.HasValue)
          return ProductPricingUtils.isHistoricalPricingEnabledVal.Value;
        ProductPricingUtils.isHistoricalPricingEnabledVal = new bool?(ProductPricingUtils.GetHistoricalPricingClientSetting());
        return ProductPricingUtils.isHistoricalPricingEnabledVal.Value;
      }
    }

    public static bool GetHistoricalPricingClientSetting()
    {
      string str = (string) null;
      try
      {
        str = SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", "DisableHistoricalPricing");
      }
      catch (Exception ex)
      {
      }
      return str == null || !(str.Trim() == "1");
    }

    public static bool IsProviderICEPPE(string providerId)
    {
      return !string.IsNullOrWhiteSpace(providerId) && Session.ConfigurationManager.GetProductPricingSettings().Any<ProductPricingSetting>((Func<ProductPricingSetting, bool>) (p => string.Compare(p.ProviderID, providerId, true) == 0 && p.IsEPPS && p.VendorPlatform == VendorPlatform.EPC2));
    }

    public static bool IsEpc2Provider(string providerId)
    {
      return !string.IsNullOrWhiteSpace(providerId) && Session.ConfigurationManager.GetProductPricingSettings().Any<ProductPricingSetting>((Func<ProductPricingSetting, bool>) (p => string.Compare(p.ProviderID, providerId, true) == 0 && p.VendorPlatform == VendorPlatform.EPC2));
    }
  }
}

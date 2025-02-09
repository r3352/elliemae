// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DataDocs.PartnerResponseBody
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#nullable disable
namespace EllieMae.EMLite.Common.DataDocs
{
  public class PartnerResponseBody
  {
    [JsonProperty("partnerId")]
    public string PartnerID { get; set; }

    [JsonProperty("productName")]
    public string ProductName { get; set; }

    [JsonProperty("providerName")]
    public string ProviderName { get; set; }

    [JsonProperty("integrationType")]
    public string IntegrationType { get; set; }

    [JsonProperty("configurationName")]
    public string ConfigurationName { get; set; }

    [JsonProperty("activeVersion")]
    public string ActiveVersion { get; set; }

    [JsonProperty("vendorKey")]
    public string VendorKey { get; set; }

    [JsonProperty("providerCompanyCode")]
    public string ProviderCompanyCode { get; set; }

    [JsonProperty("freddieMacCreditProviderParentCode")]
    public string FreddieMacCreditProviderParentCode { get; set; }

    [JsonProperty("serviceMode")]
    public string ServiceMode { get; set; }

    [JsonProperty("fannieMaeCreditProviderCode")]
    public string FannieMaeCreditProviderCode { get; set; }

    [JsonProperty("freddieMacCreditProviderAffiliateCode")]
    public string FreddieMacCreditProviderAffiliateCode { get; set; }

    [JsonProperty("isGenerallyAvailable")]
    public string IsGenerallyAvailable { get; set; }

    [JsonProperty("partnerWebUIUrl")]
    public string PartnerWebUIUrl { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("category")]
    public string Category { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    public JObject RawResponse { get; set; }
  }
}

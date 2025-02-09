// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ProductPricingSetting
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ProductPricingSetting : IXmlSerializable
  {
    private string providerID;
    private string partnerID;
    private VendorPlatform vendorPlatform;
    private string settingsSection;
    private string partnerName;
    private string adminURL;
    private string moreInfoURL;
    private bool supportImportToLoan;
    private bool supportPartnerRequestLock;
    private bool supportPartnerLockConfirm;
    private bool showSellSide;
    private bool importToLoan;
    private bool partnerRequestLock;
    private bool partnerLockConfirm;
    private bool active;
    private bool partnerRequestLockWhenNoCurrentLock;
    private bool isCustomizeInvestorName;
    private bool useCustomizedInvestorName;
    private bool useOnlyInvestorName;
    private bool useOnlyLenderName;
    private bool useInvestorAndLenderName;
    private bool enableAutoLockRequest;
    private bool supportEnableAutoLockRequest;
    private bool enableAutoLockExtensionRequest;
    private bool enableAutoCancelLockRequest;
    private bool getPricingRelock;
    private bool enableAutoLockRelock;
    private string eppsPartnerId;

    public string ExcludeAutoLockLoanType { get; set; }

    public string ExcludeAutoLockPropertyState { get; set; }

    public string ExcludeAutoLockChannel { get; set; }

    public string ExcludeAutoLockLoanPurpose { get; set; }

    public string ExcludeAutoLockAmortizationType { get; set; }

    public string ExcludeAutoLockPropertyWillBe { get; set; }

    public string ExcludeAutoLockLienPosition { get; set; }

    public string ExcludeAutoLockLoanProgram { get; set; }

    public string ExcludeAutoLockPlanCode { get; set; }

    public bool IsExportUserFinished { get; set; }

    public ProductPricingSetting()
    {
    }

    public ProductPricingSetting(
      string providerID,
      string partnerID,
      VendorPlatform vendorPlatform,
      string settingsSection,
      string partnerName,
      string adminURL,
      string moreInfoURL,
      bool supportImportToLoan,
      bool supportPartnerRequestLock,
      bool supportPartnerLockConfirm,
      bool showSellSide,
      bool importToLoan,
      bool partnerRequestLock,
      bool partnerLockConfirm,
      bool active,
      bool partnerRequestLockWhenNoCurrentLock,
      bool isCustomizeInvestorName,
      bool useCustomizedInvestorName,
      bool useOnlyInvestorName,
      bool useOnlyLenderName,
      bool useInvestorAndLenderName,
      bool enableAutoLockRequest,
      bool enableAutoLockExtensionRequest,
      bool enableAutoCancelLockRequest,
      bool supportEnableAutoLockRequest,
      bool getPricingRelock,
      bool enableAutoLockRelock,
      bool isExportUserFinished,
      string ExcludeAutoLockLoanType,
      string ExcludeAutoLockPropertyState,
      string ExcludeAutoLockChannel,
      string ExcludeAutoLockLoanPurpose,
      string ExcludeAutoLockAmortizationType,
      string ExcludeAutoLockPropertyWillBe,
      string ExcludeAutoLockLienPosition,
      string ExcludeAutoLockLoanProgram,
      string ExcludeAutoLockPlanCode)
    {
      this.providerID = providerID;
      this.partnerID = partnerID;
      this.vendorPlatform = vendorPlatform;
      this.settingsSection = settingsSection;
      this.partnerName = partnerName;
      this.adminURL = adminURL;
      this.moreInfoURL = moreInfoURL;
      this.supportImportToLoan = supportImportToLoan;
      this.supportPartnerRequestLock = supportPartnerRequestLock;
      this.supportPartnerLockConfirm = supportPartnerLockConfirm;
      this.showSellSide = showSellSide;
      this.importToLoan = importToLoan;
      this.partnerRequestLock = partnerRequestLock;
      this.partnerLockConfirm = partnerLockConfirm;
      this.active = active;
      this.partnerRequestLockWhenNoCurrentLock = partnerRequestLockWhenNoCurrentLock;
      this.isCustomizeInvestorName = isCustomizeInvestorName;
      this.useCustomizedInvestorName = useCustomizedInvestorName;
      this.useOnlyInvestorName = useOnlyInvestorName;
      this.useOnlyLenderName = useOnlyLenderName;
      this.useInvestorAndLenderName = useInvestorAndLenderName;
      this.enableAutoLockRequest = enableAutoLockRequest;
      this.enableAutoLockExtensionRequest = enableAutoLockExtensionRequest;
      this.enableAutoCancelLockRequest = enableAutoCancelLockRequest;
      this.supportEnableAutoLockRequest = supportEnableAutoLockRequest;
      this.getPricingRelock = getPricingRelock;
      this.enableAutoLockRelock = enableAutoLockRelock;
      this.IsExportUserFinished = isExportUserFinished;
      this.ExcludeAutoLockLoanType = ExcludeAutoLockLoanType;
      this.ExcludeAutoLockPropertyState = ExcludeAutoLockPropertyState;
      this.ExcludeAutoLockChannel = ExcludeAutoLockChannel;
      this.ExcludeAutoLockLoanPurpose = ExcludeAutoLockLoanPurpose;
      this.ExcludeAutoLockAmortizationType = ExcludeAutoLockAmortizationType;
      this.ExcludeAutoLockPropertyWillBe = ExcludeAutoLockPropertyWillBe;
      this.ExcludeAutoLockLienPosition = ExcludeAutoLockLienPosition;
      this.ExcludeAutoLockLoanProgram = ExcludeAutoLockLoanProgram;
      this.ExcludeAutoLockPlanCode = ExcludeAutoLockPlanCode;
    }

    public ProductPricingSetting(
      string providerID,
      string partnerID,
      VendorPlatform vendorPlatform,
      string settingsSection,
      string partnerName,
      string adminURL,
      string moreInfoURL,
      bool supportImportToLoan,
      bool supportPartnerRequestLock,
      bool supportPartnerLockConfirm,
      bool showSellSide,
      string ExcludeAutoLockLoanType = "�",
      string ExcludeAutoLockPropertyState = "�",
      string ExcludeAutoLockChannel = "�",
      string ExcludeAutoLockLoanPurpose = "�",
      string ExcludeAutoLockAmortizationType = "�",
      string ExcludeAutoLockPropertyWillBe = "�",
      string ExcludeAutoLockLienPosition = "�",
      string ExcludeAutoLockLoanProgram = "�",
      string ExcludeAutoLockPlanCode = "�")
    {
      this.providerID = providerID;
      this.partnerID = partnerID;
      this.vendorPlatform = vendorPlatform;
      this.settingsSection = settingsSection;
      this.partnerName = partnerName;
      this.adminURL = adminURL;
      this.moreInfoURL = moreInfoURL;
      this.supportImportToLoan = supportImportToLoan;
      this.supportPartnerRequestLock = supportPartnerRequestLock;
      this.supportPartnerLockConfirm = supportPartnerLockConfirm;
      this.showSellSide = showSellSide;
      this.ExcludeAutoLockLoanType = ExcludeAutoLockLoanType;
      this.ExcludeAutoLockPropertyState = ExcludeAutoLockPropertyState;
      this.ExcludeAutoLockChannel = ExcludeAutoLockChannel;
      this.ExcludeAutoLockLoanPurpose = ExcludeAutoLockLoanPurpose;
      this.ExcludeAutoLockAmortizationType = ExcludeAutoLockAmortizationType;
      this.ExcludeAutoLockPropertyWillBe = ExcludeAutoLockPropertyWillBe;
      this.ExcludeAutoLockLienPosition = ExcludeAutoLockLienPosition;
      this.ExcludeAutoLockLoanProgram = ExcludeAutoLockLoanProgram;
      this.ExcludeAutoLockPlanCode = ExcludeAutoLockPlanCode;
    }

    public ProductPricingSetting(XmlSerializationInfo info)
    {
      this.providerID = info.GetString(nameof (providerID), "");
      this.partnerID = info.GetString(nameof (partnerID), "");
      this.vendorPlatform = (VendorPlatform) Enum.Parse(typeof (VendorPlatform), info.GetString(nameof (vendorPlatform), ""), true);
      this.settingsSection = info.GetString(nameof (settingsSection), "");
      this.partnerName = info.GetString(nameof (partnerName), "");
      this.adminURL = info.GetString(nameof (adminURL), "");
      this.moreInfoURL = info.GetString(nameof (moreInfoURL), "");
      this.supportImportToLoan = info.GetBoolean(nameof (supportImportToLoan), false);
      this.supportPartnerRequestLock = info.GetBoolean(nameof (supportPartnerRequestLock), false);
      this.supportPartnerLockConfirm = info.GetBoolean(nameof (supportPartnerLockConfirm), false);
      this.showSellSide = info.GetBoolean(nameof (showSellSide), false);
      this.importToLoan = info.GetBoolean(nameof (importToLoan), false);
      this.partnerRequestLock = info.GetBoolean(nameof (partnerRequestLock), false);
      this.partnerLockConfirm = info.GetBoolean(nameof (partnerLockConfirm), false);
      this.active = info.GetBoolean(nameof (active), false);
      this.partnerRequestLockWhenNoCurrentLock = info.GetBoolean(nameof (partnerRequestLockWhenNoCurrentLock), false);
      this.isCustomizeInvestorName = info.GetBoolean(nameof (isCustomizeInvestorName), false);
      this.useCustomizedInvestorName = info.GetBoolean(nameof (useCustomizedInvestorName), false);
      this.useOnlyInvestorName = info.GetBoolean(nameof (useOnlyInvestorName), false);
      this.useOnlyLenderName = info.GetBoolean(nameof (useOnlyLenderName), false);
      this.useInvestorAndLenderName = info.GetBoolean(nameof (useInvestorAndLenderName), false);
      this.enableAutoLockRequest = info.GetBoolean(nameof (enableAutoLockRequest), false);
      this.enableAutoLockExtensionRequest = info.GetBoolean(nameof (enableAutoLockExtensionRequest), false);
      this.enableAutoCancelLockRequest = info.GetBoolean(nameof (enableAutoCancelLockRequest), false);
      this.supportEnableAutoLockRequest = info.GetBoolean(nameof (supportEnableAutoLockRequest), false);
      this.getPricingRelock = info.GetBoolean(nameof (getPricingRelock), true);
      this.enableAutoLockRelock = info.GetBoolean(nameof (enableAutoLockRelock), false);
      this.ExcludeAutoLockLoanType = info.GetString(nameof (ExcludeAutoLockLoanType), "");
      this.ExcludeAutoLockPropertyState = info.GetString(nameof (ExcludeAutoLockPropertyState), "");
      this.ExcludeAutoLockChannel = info.GetString(nameof (ExcludeAutoLockChannel), "");
      this.ExcludeAutoLockLoanPurpose = info.GetString(nameof (ExcludeAutoLockLoanPurpose), "");
      this.ExcludeAutoLockAmortizationType = info.GetString(nameof (ExcludeAutoLockAmortizationType), "");
      this.ExcludeAutoLockPropertyWillBe = info.GetString(nameof (ExcludeAutoLockPropertyWillBe), "");
      this.ExcludeAutoLockLienPosition = info.GetString(nameof (ExcludeAutoLockLienPosition), "");
      this.ExcludeAutoLockLoanProgram = info.GetString(nameof (ExcludeAutoLockLoanProgram), "");
      this.ExcludeAutoLockPlanCode = info.GetString(nameof (ExcludeAutoLockPlanCode), "");
      this.IsExportUserFinished = info.GetBoolean(nameof (IsExportUserFinished), false);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("partnerID", (object) this.partnerID);
      info.AddValue("partnerID", (object) this.partnerID);
      info.AddValue("settingsSection", (object) this.settingsSection);
      info.AddValue("partnerName", (object) this.partnerName);
      info.AddValue("adminURL", (object) this.adminURL);
      info.AddValue("moreInfoURL", (object) this.moreInfoURL);
      info.AddValue("supportImportToLoan", (object) this.supportImportToLoan);
      info.AddValue("supportPartnerRequestLock", (object) this.supportPartnerRequestLock);
      info.AddValue("supportPartnerLockConfirm", (object) this.supportPartnerLockConfirm);
      info.AddValue("showSellSide", (object) this.showSellSide);
      info.AddValue("importToLoan", (object) this.importToLoan);
      info.AddValue("partnerRequestLock", (object) this.partnerRequestLock);
      info.AddValue("partnerLockConfirm", (object) this.partnerLockConfirm);
      info.AddValue("active", (object) this.active);
      info.AddValue("partnerRequestLockWhenNoCurrentLock", (object) this.partnerRequestLockWhenNoCurrentLock);
      info.AddValue("isCustomizeInvestorName", (object) this.isCustomizeInvestorName);
      info.AddValue("useCustomizedInvestorName", (object) this.useCustomizedInvestorName);
      info.AddValue("useOnlyInvestorName", (object) this.useOnlyInvestorName);
      info.AddValue("useOnlyLenderName", (object) this.useOnlyLenderName);
      info.AddValue("useInvestorAndLenderName", (object) this.useInvestorAndLenderName);
      info.AddValue("enableAutoLockRequest", (object) this.enableAutoLockRequest);
      info.AddValue("enableAutoLockExtensionRequest", (object) this.enableAutoLockExtensionRequest);
      info.AddValue("enableAutoCancelLockRequest", (object) this.enableAutoCancelLockRequest);
      info.AddValue("supportEnableLockRequest", (object) this.supportEnableAutoLockRequest);
      info.AddValue("getPricingRelock", (object) this.getPricingRelock);
      info.AddValue("enableAutoLockRelock", (object) this.enableAutoLockRelock);
      info.AddValue("ExcludeAutoLockLoanType", (object) this.ExcludeAutoLockLoanType);
      info.AddValue("ExcludeAutoLockPropertyState", (object) this.ExcludeAutoLockPropertyState);
      info.AddValue("ExcludeAutoLockChannel", (object) this.ExcludeAutoLockChannel);
      info.AddValue("ExcludeAutoLockLoanPurpose", (object) this.ExcludeAutoLockLoanPurpose);
      info.AddValue("ExcludeAutoLockAmortizationType", (object) this.ExcludeAutoLockAmortizationType);
      info.AddValue("ExcludeAutoLockPropertyWillBe", (object) this.ExcludeAutoLockPropertyWillBe);
      info.AddValue("ExcludeAutoLockLienPosition", (object) this.ExcludeAutoLockLienPosition);
      info.AddValue("ExcludeAutoLockLoanProgram", (object) this.ExcludeAutoLockLoanProgram);
      info.AddValue("ExcludeAutoLockPlanCode", (object) this.ExcludeAutoLockPlanCode);
      info.AddValue("IsExportUserFinished", (object) this.IsExportUserFinished);
    }

    public override int GetHashCode() => base.GetHashCode();

    public string ProviderID
    {
      get => this.providerID;
      set => this.providerID = value;
    }

    public string PartnerID
    {
      get => this.partnerID;
      set => this.providerID = value;
    }

    public VendorPlatform VendorPlatform
    {
      get => this.vendorPlatform;
      set => this.vendorPlatform = value;
    }

    public string SettingsSection
    {
      get => this.settingsSection;
      set => this.settingsSection = value;
    }

    public string PartnerName
    {
      get => this.partnerName;
      set => this.partnerName = value;
    }

    public string AdminURL
    {
      get => this.adminURL;
      set => this.adminURL = value;
    }

    public string MoreInfoURL
    {
      get => this.moreInfoURL;
      set => this.moreInfoURL = value;
    }

    public bool SupportImportToLoan
    {
      get => this.supportImportToLoan;
      set => this.supportImportToLoan = value;
    }

    public bool SupportPartnerRequestLock
    {
      get => this.supportPartnerRequestLock;
      set => this.supportPartnerRequestLock = value;
    }

    public bool SupportPartnerLockConfirm
    {
      get => this.supportPartnerLockConfirm;
      set => this.supportPartnerLockConfirm = value;
    }

    public bool ShowSellSide
    {
      get => this.showSellSide;
      set => this.showSellSide = value;
    }

    public bool ImportToLoan
    {
      get => this.importToLoan && this.supportImportToLoan;
      set => this.importToLoan = value;
    }

    public bool PartnerRequestLock
    {
      get => this.partnerRequestLock && this.supportPartnerRequestLock;
      set => this.partnerRequestLock = value;
    }

    public bool PartnerLockConfirm
    {
      get => this.partnerLockConfirm && this.supportPartnerLockConfirm;
      set => this.partnerLockConfirm = value;
    }

    public bool PartnerRequestLockWhenNoCurrentLock
    {
      get => this.supportPartnerRequestLock && this.partnerRequestLockWhenNoCurrentLock;
      set => this.partnerRequestLockWhenNoCurrentLock = value;
    }

    public bool IsCustomizeInvestorName
    {
      get => this.isCustomizeInvestorName;
      set => this.isCustomizeInvestorName = value;
    }

    public bool UseCustomizedInvestorName
    {
      get => this.useCustomizedInvestorName;
      set => this.useCustomizedInvestorName = value;
    }

    public bool UseOnlyInvestorName
    {
      get => this.useOnlyInvestorName;
      set => this.useOnlyInvestorName = value;
    }

    public bool UseOnlyLenderName
    {
      get => this.useOnlyLenderName;
      set => this.useOnlyLenderName = value;
    }

    public bool UseInvestorAndLenderName
    {
      get => this.useInvestorAndLenderName;
      set => this.useInvestorAndLenderName = value;
    }

    public bool EnableAutoLockRequest
    {
      get => this.enableAutoLockRequest;
      set => this.enableAutoLockRequest = value;
    }

    public bool SupportEnableAutoLockRequest
    {
      get => this.supportEnableAutoLockRequest;
      set => this.supportEnableAutoLockRequest = value;
    }

    public bool EnableAutoLockExtensionRequest
    {
      get => this.enableAutoLockExtensionRequest;
      set => this.enableAutoLockExtensionRequest = value;
    }

    public bool EnableAutoCancelRequest
    {
      get => this.enableAutoCancelLockRequest;
      set => this.enableAutoCancelLockRequest = value;
    }

    public bool GetPricingRelock
    {
      get => this.getPricingRelock;
      set => this.getPricingRelock = value;
    }

    public bool EnableAutoLockRelock
    {
      get => this.enableAutoLockRelock;
      set => this.enableAutoLockRelock = value;
    }

    public bool IsEPPS
    {
      get
      {
        return string.Compare(this.partnerID, "mps", true) == 0 || string.Compare(this.partnerID, this.eppsPartnerId, true) == 0;
      }
    }

    public string EppsPartnerId
    {
      get => this.eppsPartnerId;
      set => this.eppsPartnerId = value;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is ProductPricingSetting))
        return false;
      ProductPricingSetting productPricingSetting = (ProductPricingSetting) obj;
      return !(productPricingSetting.providerID != this.providerID) && !(productPricingSetting.partnerID != this.partnerID) && productPricingSetting.vendorPlatform == this.vendorPlatform && !(productPricingSetting.settingsSection != this.settingsSection) && !(productPricingSetting.partnerName != this.partnerName) && !(productPricingSetting.adminURL != this.adminURL) && !(productPricingSetting.moreInfoURL != this.moreInfoURL) && productPricingSetting.supportImportToLoan == this.supportImportToLoan && productPricingSetting.supportPartnerRequestLock == this.supportPartnerRequestLock && productPricingSetting.supportPartnerLockConfirm == this.supportPartnerLockConfirm && productPricingSetting.partnerRequestLockWhenNoCurrentLock == this.partnerRequestLockWhenNoCurrentLock && productPricingSetting.showSellSide == this.showSellSide && productPricingSetting.isCustomizeInvestorName == this.isCustomizeInvestorName && !(productPricingSetting.ExcludeAutoLockAmortizationType != this.ExcludeAutoLockAmortizationType) && !(productPricingSetting.ExcludeAutoLockChannel != this.ExcludeAutoLockChannel) && !(productPricingSetting.ExcludeAutoLockLienPosition != this.ExcludeAutoLockLienPosition) && !(productPricingSetting.ExcludeAutoLockLoanProgram != this.ExcludeAutoLockLoanProgram) && !(productPricingSetting.ExcludeAutoLockLoanPurpose != this.ExcludeAutoLockLoanPurpose) && !(productPricingSetting.ExcludeAutoLockLoanType != this.ExcludeAutoLockLoanType) && !(productPricingSetting.ExcludeAutoLockPlanCode != this.ExcludeAutoLockPlanCode) && !(productPricingSetting.ExcludeAutoLockPropertyState != this.ExcludeAutoLockPropertyState) && !(productPricingSetting.ExcludeAutoLockPropertyWillBe != this.ExcludeAutoLockPropertyWillBe) && productPricingSetting.IsExportUserFinished == this.IsExportUserFinished && !(productPricingSetting.EppsPartnerId != this.EppsPartnerId) && productPricingSetting.SupportEnableAutoLockRequest == this.SupportEnableAutoLockRequest;
    }

    public bool Active
    {
      get => this.active;
      set => this.active = value;
    }

    public ProductPricingSetting Clone()
    {
      return new ProductPricingSetting(this.providerID, this.partnerID, this.vendorPlatform, this.settingsSection, this.partnerName, this.adminURL, this.moreInfoURL, this.supportImportToLoan, this.supportPartnerRequestLock, this.supportPartnerLockConfirm, this.showSellSide, this.importToLoan, this.partnerRequestLock, this.partnerLockConfirm, this.active, this.partnerRequestLockWhenNoCurrentLock, this.isCustomizeInvestorName, this.useCustomizedInvestorName, this.useOnlyInvestorName, this.useOnlyLenderName, this.useInvestorAndLenderName, this.enableAutoLockRequest, this.enableAutoLockExtensionRequest, this.enableAutoCancelLockRequest, this.supportEnableAutoLockRequest, this.getPricingRelock, this.enableAutoLockRelock, this.IsExportUserFinished, this.ExcludeAutoLockLoanType, this.ExcludeAutoLockPropertyState, this.ExcludeAutoLockChannel, this.ExcludeAutoLockLoanPurpose, this.ExcludeAutoLockAmortizationType, this.ExcludeAutoLockPropertyWillBe, this.ExcludeAutoLockLienPosition, this.ExcludeAutoLockLoanProgram, this.ExcludeAutoLockPlanCode)
      {
        eppsPartnerId = this.eppsPartnerId
      };
    }
  }
}

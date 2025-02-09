// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ProductPricing.ProductPricingAdmin
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ProductPricing
{
  public class ProductPricingAdmin : SettingsUserControl
  {
    private PPEservice serviceProvider;
    private bool suspendEvent;
    private Dictionary<string, ProductPricingSetting> settingDictionary = new Dictionary<string, ProductPricingSetting>();
    private List<ProductPricingSetting> settings = new List<ProductPricingSetting>();
    private ProductPricingSetting currentActiveSetting;
    private bool deleteBackKey;
    private bool internalTrigger;
    private Sessions.Session session;
    private List<LockExtensionPriceAdjustment> lockExtPriceAdjs = new List<LockExtensionPriceAdjustment>();
    private List<LockExtensionPriceAdjustment> lockExtPriceAdjsOccur = new List<LockExtensionPriceAdjustment>();
    private int groupContainer1ExpandedHeight;
    private int groupContainer1ContractedHeight;
    private const string ExtensionNumberPrefix = "Extension #";
    private bool tpoSiteExists;
    private Hashtable policySettings;
    private int padding = 20;
    private const string className = "ProductPricingAdmin";
    private static string sw = Tracing.SwOutsideLoan;
    private string errorDetails = string.Empty;
    private string stackTraceDetails = string.Empty;
    private List<Epc2Provider> epcProviderList;
    private ExportCredential exportCredential;
    private IContainer components;
    private BorderPanel borderPanel1;
    private GroupContainer gcElapsedTime;
    private TextBox txtElapsedTime;
    private Label label1;
    private CheckBox chkElapseTime;
    private GroupContainer gcPPConfig;
    private Label label2;
    private System.Windows.Forms.LinkLabel llMoreInfo;
    private System.Windows.Forms.LinkLabel llAdmin;
    private System.Windows.Forms.LinkLabel llSvcMgmt;
    private Label lblSvcMgmtBar;
    private ComboBox cbProvider;
    private CheckBox chkLockConfirm;
    private CheckBox chkRequestLock;
    private CheckBox chkImportToLoanFile;
    private CheckBox checkBox2;
    private CheckBox checkBox1;
    private Panel pnlSettings;
    private Label label3;
    private CheckBox chkTimerEnforceTiming;
    private CheckBox chkRequestLockWOLock;
    private CheckBox chkCustomizeName;
    private RadioButton rdoCustomizeNameAlwaysLender;
    private RadioButton rdoCustomizeNameAlwaysInvestor;
    private RadioButton rdoCustomizeNameDepends;
    private GroupContainer grCustomizeName;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer2;
    private GridView gvExtPriceAdj;
    private TextBox txtDailyPriceAdj;
    private StandardIconButton siBtnDelete;
    private StandardIconButton siBtnAdd;
    private GradientPanel gradientPanel1;
    private Label label6;
    private ComboBox cmbLockExtensionOption;
    private CheckBox chkFixedExtension;
    private Label label5;
    private Panel pnlLockextensionConfig;
    private StandardIconButton siBtnEdit;
    private Panel pnlProviders;
    private Panel pnlProviderSettings;
    private ToolTip toolTip1;
    private Panel pnlGlobalSettings;
    private Label label7;
    private BorderPanel borderPanel2;
    private CheckBox chkApplyPriceAdjOnOffDays;
    private GroupContainer gcLockRequestType;
    private CheckBox chkReLockOnly;
    private CheckBox chkEnableLockExtension;
    private Label label8;
    private TextBox txtExtensionDays;
    private Label label9;
    private RadioButton rbtnCustomDays;
    private RadioButton rbtnLockPeriod;
    private RadioButton rbtnUnlimted;
    private CheckBox chkGetPricingRelock;
    private CheckBox chkRelock;
    private GroupContainer gcLockCancellation;
    private CheckBox chkLockCancellationOnLockReq;
    private CheckBox chkEnableLockCancellation;
    private CheckBox chkAllowRelockOutsideLDHours;
    private CheckBox chkAllowNewLockOutsideLDHours;
    private Panel pnlAdjustPerLockExt;
    private Label label10;
    private GroupContainer groupContainer3;
    private StandardIconButton btnEditAdjustPerLockExt;
    private StandardIconButton btnDeleteAdjustPerLockExt;
    private StandardIconButton btnAddAdjustPerLockExt;
    private GridView gvAdjustPerLockExt;
    private Label label11;
    private CheckBox chkAllowDailyAdj;
    private Label label4;
    private TextBox txtLockExtensionAllowTotalCapDays;
    private Label label12;
    private CheckBox ckLockExtensionAllowTotalCap;
    private Label label16;
    private Label label13;
    private Label labelGetCurrentPricingDays;
    private TextBox txtLockExpDays;
    private CheckBox chkGetCurrentPricing;
    private CheckBox ckLockExtAllowTotalTimesCap;
    private TextBox txtLockExtAllowTotalTimes;
    private Label label17;
    private CheckBox chkEnableRelockForTpoClient;
    private CheckBox ckRelockFeeAllowed;
    private TextBox txtRelockFee;
    private Label labelRelockFeeAllowed;
    private CheckBox ckRelockAllowTotalCap;
    private TextBox txtRelockAllowTotalCapDays;
    private Label labelRelockAllowTotalCap;
    private Label lblElapsedTimeSettings;
    private CheckBox chkDontAllowPriceChange;
    private GroupContainer groupContainerLockVoid;
    private CheckBox chkEnableLockVoid;
    private PictureBox progressSpinner;
    private Panel pnlExportUser;
    private Label label21;
    private System.Windows.Forms.LinkLabel llExportUser;
    private CheckBox chkReLockReLockfees;
    private CheckBox chkReLockPriceConcessions;
    private CheckBox chkReLockCustomPriceAdjustments;
    private CheckBox chkReLockExtensionFees;
    private Label label22;
    private CheckBox chkLockUpdateReLockfees;
    private CheckBox chkLockUpdatePriceConcessions;
    private CheckBox chkLockUpdateCustomPriceAdjustments;
    private CheckBox chkLockUpdateExtensionFees;
    private Label label23;
    private Label label24;
    private TextBox txtWorstCasePrice;
    private CheckBox chkUseWorstCasePrice;
    private CheckBox chkRestrictLockPeriod;
    private CheckBox chkWaiveFeeAfter;
    private TextBox txtWaiveFeeAfter;
    private Label label25;
    private Panel pnlLockUpdates;
    private Panel panel1;
    private Panel panel3;
    private Panel panel2;
    private Panel pnlForReLocks;
    private Panel panel4;
    private Panel panel5;
    private CheckBox chkLockUpdateandLockConfirm;
    private CheckBox chkAutoValidate;
    private CheckBox chkEnableLockVoidRetail;
    private CheckBox chkEnableLockVoidWholesale;
    private GroupContainer groupContainerZeroBasedParPricing;
    private CheckBox chkZeroBasedParPricingRetail;
    private CheckBox chkZeroBasedParPricingWholesale;
    private Label label14;
    private CheckBox chkAllowPPESelection;

    public ProductPricingAdmin(SetUpContainer setupCont, Sessions.Session session)
      : base(setupCont)
    {
      this.InitializeComponent();
      this.adjustPosition();
      TextBoxFormatter.Attach(this.txtExtensionDays, TextBoxContentRule.Integer, "#,##0");
      TextBoxFormatter.Attach(this.txtLockExtensionAllowTotalCapDays, TextBoxContentRule.Integer, "#,##0");
      TextBoxFormatter.Attach(this.txtLockExtAllowTotalTimes, TextBoxContentRule.Integer, "#,##0");
      this.groupContainer1ExpandedHeight = this.groupContainer1.Height - this.pnlLockextensionConfig.Height + this.padding;
      this.groupContainer1ContractedHeight = this.groupContainer1.Height - this.pnlLockextensionConfig.Height - this.pnlAdjustPerLockExt.Height;
      this.session = session;
      this.policySettings = this.session.ConfigurationManager.GetCompanySettings("POLICIES");
      this.serviceProvider = new PPEservice(session?.SessionObjects?.StartupInfo?.ServiceUrls?.PPEServiceUrl);
      this.reSyncServerSettings();
      this.initialPageValue();
      this.AdjustRelockGroupContainerSize(false);
      this.ApplyShipDarkToRelockSettings();
    }

    private async void reSyncServerSettings()
    {
      ProductPricingAdmin productPricingAdmin = this;
      Task<List<Epc2Provider>> epcGetProductTask = Epc2ServiceClient.GetProviderList(productPricingAdmin.session.SessionObjects, new Bam().GetAccessToken("sc"), new string[1]
      {
        "PRODUCTPRICING"
      }, "Encompass Smart Client");
      string clientId = productPricingAdmin.session.CompanyInfo.ClientID;
      string instanceName = productPricingAdmin.session?.ServerIdentity?.InstanceName;
      if (!string.IsNullOrWhiteSpace(instanceName) && (instanceName.StartsWith("TEBE", StringComparison.InvariantCultureIgnoreCase) || instanceName.StartsWith("DEBE", StringComparison.InvariantCultureIgnoreCase)))
        clientId = instanceName;
      Task<Partner[]> emnGetProductTask = Task.Run<Partner[]>((Func<Partner[]>) (() => this.serviceProvider.GetPartners(clientId)));
      Partner[] partnerList = (Partner[]) null;
      try
      {
        productPricingAdmin.progressSpinner.Visible = true;
        await Task.WhenAll((Task) epcGetProductTask, (Task) emnGetProductTask);
        partnerList = emnGetProductTask.Result;
        productPricingAdmin.epcProviderList = epcGetProductTask.Result;
        productPricingAdmin.progressSpinner.Visible = false;
      }
      catch (Exception ex)
      {
        productPricingAdmin.progressSpinner.Image = (Image) new Bitmap((Image) SystemIcons.Error.ToBitmap(), new Size(18, 18));
        Tracing.Log(ProductPricingAdmin.sw, TraceLevel.Error, nameof (ProductPricingAdmin), string.Format("GetPartners Exception (Message {0})", (object) ex.Message));
        productPricingAdmin.errorDetails = ex.Message;
        productPricingAdmin.stackTraceDetails = ex.StackTrace;
      }
      productPricingAdmin.session.ServerManager.UpdateServerSettings((IDictionary) new Dictionary<string, object>()
      {
        {
          "POLICIES.PRICING_PARTNER",
          (object) ""
        },
        {
          "POLICIES.PRICING_PARTNER_SELL_SIDE_SHOW",
          (object) ""
        },
        {
          "POLICIES.USE.LOCK.REQUEST.FIELDS",
          (object) ""
        }
      }, true, false);
      string useLagacy = productPricingAdmin.policySettings.Contains((object) "USE.LOCK.REQUEST.FIELDS") ? productPricingAdmin.policySettings[(object) "USE.LOCK.REQUEST.FIELDS"].ToString() : "";
      string currentSelectedProvider = productPricingAdmin.policySettings.Contains((object) "PRICING_PARTNER") ? productPricingAdmin.policySettings[(object) "PRICING_PARTNER"].ToString() : "";
      bool requireUpdate = false;
      productPricingAdmin.settings = ProductPricingUtils.MergeProviders(productPricingAdmin.session, partnerList, productPricingAdmin.epcProviderList, useLagacy, currentSelectedProvider, ref requireUpdate);
      if (requireUpdate)
        productPricingAdmin.session.ConfigurationManager.UpdateProductPricingSettings(productPricingAdmin.settings);
      productPricingAdmin.settingDictionary.Clear();
      foreach (ProductPricingSetting setting in productPricingAdmin.settings)
        productPricingAdmin.settingDictionary.Add(setting.ProviderID, setting);
      productPricingAdmin.currentActiveSetting = (ProductPricingSetting) null;
      foreach (ProductPricingSetting setting in productPricingAdmin.settings)
      {
        if (setting.Active)
          productPricingAdmin.currentActiveSetting = setting;
      }
      productPricingAdmin.cbProvider.Items.Clear();
      productPricingAdmin.cbProvider.Items.Add((object) "No Provider Selected");
      foreach (KeyValuePair<string, ProductPricingSetting> setting in productPricingAdmin.settingDictionary)
        productPricingAdmin.cbProvider.Items.Add((object) new ComboboxItem()
        {
          Text = setting.Value.PartnerName,
          Value = setting.Value
        });
      if (productPricingAdmin.currentActiveSetting != null)
        productPricingAdmin.cbProvider.SelectedIndex = productPricingAdmin.settingDictionary.Values.ToList<ProductPricingSetting>().IndexOf(productPricingAdmin.currentActiveSetting) + 1;
      else if (productPricingAdmin.cbProvider.Items.Count > 0)
        productPricingAdmin.cbProvider.SelectedIndex = 0;
      if (productPricingAdmin.currentActiveSetting != null && productPricingAdmin.currentActiveSetting.VendorPlatform == VendorPlatform.EPC2)
        productPricingAdmin.updateMoreInfoLink(productPricingAdmin.currentActiveSetting);
      else
        productPricingAdmin.updateUISetting(productPricingAdmin.currentActiveSetting);
      if (productPricingAdmin.currentActiveSetting != null && productPricingAdmin.currentActiveSetting.VendorPlatform == VendorPlatform.EPC2)
      {
        if (productPricingAdmin.currentActiveSetting.IsEPPS)
        {
          productPricingAdmin.chkReLockOnly.Checked = true;
          productPricingAdmin.chkReLockOnly.Enabled = false;
        }
        if (!productPricingAdmin.chkWaiveFeeAfter.Checked && !productPricingAdmin.chkWaiveFeeAfter.Enabled)
          productPricingAdmin.chkWaiveFeeAfter.Enabled = false;
      }
      else
        productPricingAdmin.LockUpdateReLockSettingsChange(false);
      productPricingAdmin.cbProvider.SelectedIndexChanged += new EventHandler(productPricingAdmin.cbProvider_SelectedIndexChanged);
      productPricingAdmin.chkImportToLoanFile.CheckedChanged += new EventHandler(productPricingAdmin.chk_CheckedChanged);
      productPricingAdmin.chkRequestLock.CheckedChanged += new EventHandler(productPricingAdmin.chkRequestLock_CheckedChanged);
      productPricingAdmin.chkRequestLockWOLock.CheckedChanged += new EventHandler(productPricingAdmin.chk_CheckedChanged);
      productPricingAdmin.chkLockConfirm.CheckedChanged += new EventHandler(productPricingAdmin.chk_CheckedChanged);
      productPricingAdmin.rdoCustomizeNameAlwaysInvestor.CheckedChanged += new EventHandler(productPricingAdmin.rdoCustomizeNameAlwaysInvestor_CheckedChanged);
      productPricingAdmin.rdoCustomizeNameAlwaysLender.CheckedChanged += new EventHandler(productPricingAdmin.rdoCustomizeNameAlwaysLender_CheckedChanged);
      productPricingAdmin.rdoCustomizeNameDepends.CheckedChanged += new EventHandler(productPricingAdmin.rdoCustomizeNameDepends_CheckedChanged);
      productPricingAdmin.chkGetPricingRelock.CheckedChanged += new EventHandler(productPricingAdmin.chk_CheckedChanged);
      productPricingAdmin.chkCustomizeName.CheckedChanged += new EventHandler(productPricingAdmin.chkCustomizeName_CheckedChanged);
      productPricingAdmin.chkLockUpdateandLockConfirm.CheckedChanged += new EventHandler(productPricingAdmin.chk_CheckedChanged);
      productPricingAdmin.chkAllowPPESelection.CheckedChanged += new EventHandler(productPricingAdmin.chk_CheckedChanged);
      productPricingAdmin.chkZeroBasedParPricingRetail.CheckedChanged += new EventHandler(productPricingAdmin.chk_CheckedChanged);
      productPricingAdmin.chkZeroBasedParPricingWholesale.CheckedChanged += new EventHandler(productPricingAdmin.chk_CheckedChanged);
      productPricingAdmin.suspendEvent = true;
      productPricingAdmin.SetExtensionSettings();
      productPricingAdmin.suspendEvent = false;
    }

    private void initialPageValue()
    {
      if (this.session != null && this.session.ConfigurationManager != null)
        this.tpoSiteExists = this.session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      this.suspendEvent = true;
      foreach (ProductPricingSetting setting in this.settings)
      {
        if (setting.Active)
          this.currentActiveSetting = setting;
      }
      if (this.currentActiveSetting != null)
        this.cbProvider.SelectedIndex = this.settingDictionary.Values.ToList<ProductPricingSetting>().IndexOf(this.currentActiveSetting) + 1;
      else if (this.cbProvider.Items.Count > 0)
        this.cbProvider.SelectedIndex = 0;
      IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Policies");
      int num = (int) serverSettings[(object) "Policies.Pricing_ElapsedTime"];
      if (num > 0)
      {
        this.chkElapseTime.Checked = true;
        this.txtElapsedTime.Text = num.ToString();
        this.txtElapsedTime.Enabled = true;
        this.chkTimerEnforceTiming.Checked = (bool) serverSettings[(object) "Policies.EnforceOnlyWhenNoCurrentLock"];
      }
      else
      {
        this.chkElapseTime.Checked = false;
        this.txtElapsedTime.Enabled = false;
        this.txtElapsedTime.Text = "0";
      }
      this.initPageSectionCustomizeName();
      this.refreshChkRequestLockWOLockControl();
      if ((bool) serverSettings[(object) "Policies.EnableLockExtension"])
      {
        this.chkEnableLockExtension.Checked = true;
        this.cmbLockExtensionOption.Enabled = true;
      }
      else
      {
        this.chkEnableLockExtension.Checked = false;
        this.cmbLockExtensionOption.Enabled = false;
      }
      this.ckLockExtensionAllowTotalCap.Checked = serverSettings[(object) "Policies.LockExtensionAllowTotalCap"] != null && (bool) serverSettings[(object) "Policies.LockExtensionAllowTotalCap"];
      if (this.ckLockExtensionAllowTotalCap.Checked)
        this.txtLockExtensionAllowTotalCapDays.Text = serverSettings[(object) "Policies.LockExtensionAllowTotalCapDays"] != null ? ((int) serverSettings[(object) "Policies.LockExtensionAllowTotalCapDays"]).ToString() : "";
      this.ckLockExtAllowTotalTimesCap.Checked = serverSettings[(object) "Policies.LockExtAllowTotalTimesCapEnabled"] != null && (bool) serverSettings[(object) "Policies.LockExtAllowTotalTimesCapEnabled"];
      if (this.ckLockExtAllowTotalTimesCap.Checked)
        this.txtLockExtAllowTotalTimes.Text = serverSettings[(object) "Policies.LockExtAllowTotalTimesCap"] != null ? ((int) serverSettings[(object) "Policies.LockExtAllowTotalTimesCap"]).ToString() : "";
      this.ToggleLockExtensionCapFields();
      this.setExtensionLimit(serverSettings);
      this.cmbLockExtensionOption.SelectedIndex = (int) serverSettings[(object) "Policies.LockExtensionCompanyControlled"];
      this.chkFixedExtension.Checked = (bool) serverSettings[(object) "Policies.LockExtensionAllowFixedExt"];
      this.chkAllowDailyAdj.Checked = (bool) serverSettings[(object) "Policies.LockExtensionAllowDailyAdj"];
      this.txtDailyPriceAdj.Text = ((Decimal) serverSettings[(object) "Policies.LockExtensionDailyPriceAdj"]).ToString("N3");
      this.gvExtPriceAdj.Items.Clear();
      this.lockExtPriceAdjs.Clear();
      this.lockExtPriceAdjs.AddRange((IEnumerable<LockExtensionPriceAdjustment>) this.session.ConfigurationManager.GetLockExtensionPriceAdjustments());
      foreach (LockExtensionPriceAdjustment priceAdj in this.lockExtPriceAdjs.ToArray())
        this.gvExtPriceAdj.Items.Add(this.createLockExtPriceAdjustment(priceAdj));
      this.gvAdjustPerLockExt.Items.Clear();
      this.lockExtPriceAdjsOccur.Clear();
      this.lockExtPriceAdjsOccur.AddRange((IEnumerable<LockExtensionPriceAdjustment>) this.session.ConfigurationManager.GetLockExtPriceAdjustPerOccurrence());
      foreach (LockExtensionPriceAdjustment priceAdj in this.lockExtPriceAdjsOccur.ToArray())
        this.gvAdjustPerLockExt.Items.Add(this.createLockExtPriceAdjustPerOccurrence(priceAdj));
      if (this.cmbLockExtensionOption.SelectedIndex == 2)
      {
        this.rbtnCustomDays.Enabled = false;
        this.rbtnCustomDays.Checked = false;
        this.txtExtensionDays.Enabled = false;
      }
      this.chkApplyPriceAdjOnOffDays.Checked = (bool) serverSettings[(object) "Policies.LockExtCalOpt_ApplyPriceAdj"];
      if ((bool) serverSettings[(object) "Policies.EnableLockCancellation"])
      {
        this.chkEnableLockCancellation.Checked = true;
      }
      else
      {
        this.chkEnableLockCancellation.Checked = false;
        this.chkLockCancellationOnLockReq.Enabled = false;
      }
      this.chkLockCancellationOnLockReq.Checked = (bool) serverSettings[(object) "Policies.EnableLockCancellation_ReqForm"];
      this.chkEnableLockVoid.Checked = (bool) serverSettings[(object) "Policies.EnableLockVoid"];
      this.chkEnableLockVoidRetail.Checked = (bool) serverSettings[(object) "Policies.EnableLockVoidRetail"];
      this.chkEnableLockVoidWholesale.Checked = (bool) serverSettings[(object) "Policies.EnableLockVoidWholesale"];
      this.chkZeroBasedParPricingRetail.Checked = (bool) serverSettings[(object) "Policies.EnableZeroParPricingRetail"];
      this.chkZeroBasedParPricingWholesale.Checked = (bool) serverSettings[(object) "Policies.EnableZeroParPricingWholesale"];
      this.chkGetCurrentPricing.Checked = (bool) serverSettings[(object) "Policies.GetCurrentPricing"];
      if (serverSettings[(object) "Policies.LockExpDays"] != null)
        this.txtLockExpDays.Text = serverSettings[(object) "Policies.LockExpDays"].ToString();
      this.chkRelock.Checked = (bool) serverSettings[(object) "Policies.EnableRelock"];
      this.chkAllowNewLockOutsideLDHours.Checked = (bool) serverSettings[(object) "Policies.AllowNewLockOutsideLDHours"];
      if (!this.chkRelock.Checked)
      {
        this.chkAllowRelockOutsideLDHours.Enabled = false;
        this.chkReLockOnly.Checked = false;
        this.chkReLockOnly.Enabled = false;
        this.chkGetPricingRelock.Enabled = false;
        this.chkGetCurrentPricing.Enabled = false;
        this.ckRelockAllowTotalCap.Enabled = false;
        this.ckRelockFeeAllowed.Enabled = false;
        this.chkUseWorstCasePrice.Enabled = false;
        this.chkRestrictLockPeriod.Enabled = false;
        this.chkWaiveFeeAfter.Enabled = false;
      }
      else
        this.chkReLockOnly.Checked = (bool) serverSettings[(object) "Policies.RelockOnly"];
      this.chkEnableRelockForTpoClient.Enabled = this.chkRelock.Checked && this.tpoSiteExists;
      this.chkAllowRelockOutsideLDHours.Checked = (bool) serverSettings[(object) "Policies.EnableRelockOutsideLockDeskHours"];
      this.chkEnableRelockForTpoClient.Checked = serverSettings[(object) "Policies.EnableRelockForTpoClient"] != null && (bool) serverSettings[(object) "Policies.EnableRelockForTpoClient"];
      if (!ProductPricingUtils.IsHistoricalPricingEnabled)
        this.chkGetPricingRelock.Visible = false;
      if (!this.chkEnableLockExtension.Checked)
      {
        this.ckLockExtAllowTotalTimesCap.Enabled = false;
        this.txtLockExtAllowTotalTimes.Enabled = false;
        this.label17.Enabled = false;
        this.siBtnEdit.Enabled = this.siBtnAdd.Enabled = this.siBtnDelete.Enabled = this.txtDailyPriceAdj.Enabled = this.chkApplyPriceAdjOnOffDays.Enabled = this.gvExtPriceAdj.Enabled = this.rbtnUnlimted.Enabled = this.rbtnLockPeriod.Enabled = this.rbtnCustomDays.Enabled = this.txtExtensionDays.Enabled = this.gvAdjustPerLockExt.Enabled = this.btnAddAdjustPerLockExt.Enabled = this.btnEditAdjustPerLockExt.Enabled = this.btnDeleteAdjustPerLockExt.Enabled = false;
      }
      this.ckRelockAllowTotalCap.Checked = serverSettings[(object) "Policies.RelockAllowTotalCap"] != null && (bool) serverSettings[(object) "Policies.RelockAllowTotalCap"];
      if (this.ckRelockAllowTotalCap.Checked)
        this.txtRelockAllowTotalCapDays.Text = serverSettings[(object) "Policies.RelockAllowTotalCapTimes"] != null ? serverSettings[(object) "Policies.RelockAllowTotalCapTimes"].ToString() : "";
      this.ckRelockFeeAllowed.Checked = serverSettings[(object) "Policies.RelockFeeAllowed"] != null && (bool) serverSettings[(object) "Policies.RelockFeeAllowed"];
      if (this.ckRelockFeeAllowed.Checked)
        this.txtRelockFee.Text = serverSettings[(object) "Policies.RelockFee"] != null ? Utils.ParseDecimal((object) serverSettings[(object) "Policies.RelockFee"].ToString(), 0M, 3).ToString("N3") : "";
      else
        this.chkWaiveFeeAfter.Enabled = false;
      this.chkDontAllowPriceChange.Checked = serverSettings[(object) "Policies.NotAllowPricingChange"] != null && (bool) serverSettings[(object) "Policies.NotAllowPricingChange"];
      this.chkLockUpdateExtensionFees.Checked = serverSettings[(object) "Policies.LockUpdateExtensionFees"] != null && (bool) serverSettings[(object) "Policies.LockUpdateExtensionFees"];
      this.chkLockUpdateCustomPriceAdjustments.Checked = serverSettings[(object) "Policies.LockUpdateCustomPriceAdjustments"] != null && (bool) serverSettings[(object) "Policies.LockUpdateCustomPriceAdjustments"];
      this.chkLockUpdatePriceConcessions.Checked = serverSettings[(object) "Policies.LockUpdatePriceConcessions"] != null && (bool) serverSettings[(object) "Policies.LockUpdatePriceConcessions"];
      this.chkLockUpdateReLockfees.Checked = serverSettings[(object) "Policies.LockUpdateReLockfees"] != null && (bool) serverSettings[(object) "Policies.LockUpdateReLockfees"];
      this.chkUseWorstCasePrice.Checked = serverSettings[(object) "Policies.WorstCasePrice"] != null && (bool) serverSettings[(object) "Policies.WorstCasePrice"];
      if (this.chkUseWorstCasePrice.Checked)
      {
        this.txtWorstCasePrice.Text = serverSettings[(object) "Policies.WorstCasePriceEqualDays"] != null ? serverSettings[(object) "Policies.WorstCasePriceEqualDays"].ToString() : "";
        this.chkReLockExtensionFees.Checked = serverSettings[(object) "Policies.ReLockExtensionFees"] != null && (bool) serverSettings[(object) "Policies.ReLockExtensionFees"];
        this.chkReLockCustomPriceAdjustments.Checked = serverSettings[(object) "Policies.ReLockCustomPriceAdjustments"] != null && (bool) serverSettings[(object) "Policies.ReLockCustomPriceAdjustments"];
        this.chkReLockPriceConcessions.Checked = serverSettings[(object) "Policies.ReLockPriceConcessions"] != null && (bool) serverSettings[(object) "Policies.ReLockPriceConcessions"];
        this.chkReLockReLockfees.Checked = serverSettings[(object) "Policies.ReLockReLockfees"] != null && (bool) serverSettings[(object) "Policies.ReLockReLockfees"];
      }
      else
        this.chkUseWorstCasePrice_CheckedChanged((object) this, (EventArgs) null);
      this.chkWaiveFeeAfter.Checked = serverSettings[(object) "Policies.WaiveFee"] != null && (bool) serverSettings[(object) "Policies.WaiveFee"];
      if (this.chkWaiveFeeAfter.Checked)
        this.txtWaiveFeeAfter.Text = serverSettings[(object) "Policies.WaiveFeeAfterDays"] != null ? serverSettings[(object) "Policies.WaiveFeeAfterDays"].ToString() : "";
      this.chkRestrictLockPeriod.Checked = serverSettings[(object) "Policies.RestrictLockPeriod"] != null && (bool) serverSettings[(object) "Policies.RestrictLockPeriod"];
      this.setDirtyFlag(false);
      this.suspendEvent = false;
    }

    private void setExtensionLimit(IDictionary settings)
    {
      switch ((int) settings[(object) "Policies.LOCKEXTENSION_CAP_TYPE"])
      {
        case 0:
          this.rbtnUnlimted.Checked = true;
          break;
        case 1:
          this.rbtnLockPeriod.Checked = true;
          break;
        case 2:
          this.rbtnCustomDays.Checked = true;
          this.txtExtensionDays.Text = ((int) settings[(object) "Policies.LOCKEXTENSION_CAP_DAYS"]).ToString();
          break;
        default:
          this.rbtnUnlimted.Checked = true;
          break;
      }
    }

    private void adjustPosition()
    {
      if (this.currentActiveSetting != null)
        this.gcPPConfig.Height = this.pnlProviderSettings.Height + this.pnlProviders.Height + this.padding;
      else
        this.gcPPConfig.Height = this.pnlProviders.Height + this.padding + this.padding;
      this.pnlGlobalSettings.Top = this.gcPPConfig.Height + this.padding;
    }

    private void refreshChkRequestLockWOLockControl()
    {
      if (!this.chkRequestLock.Enabled)
      {
        this.chkRequestLockWOLock.Enabled = false;
        this.chkRequestLockWOLock.Checked = false;
      }
      else if (!this.chkRequestLock.Checked)
      {
        this.chkRequestLockWOLock.Enabled = true;
        this.chkRequestLockWOLock.Checked = false;
      }
      else if (this.session.ConfigurationManager.GetCompanySetting("POLICIES", "ReqeustLockFromPPEOnlyWOCurrentLock") == "True")
        this.chkRequestLockWOLock.Checked = true;
      else
        this.chkRequestLockWOLock.Checked = false;
    }

    private void initPageSectionCustomizeName()
    {
      this.chkCustomizeName.Enabled = false;
      this.rdoCustomizeNameAlwaysInvestor.Enabled = false;
      this.rdoCustomizeNameAlwaysLender.Enabled = false;
      this.rdoCustomizeNameDepends.Enabled = false;
      if (this.currentActiveSetting != null && this.currentActiveSetting.IsCustomizeInvestorName)
        this.chkCustomizeName.Enabled = true;
      if (this.currentActiveSetting != null && this.currentActiveSetting.UseCustomizedInvestorName)
      {
        this.chkCustomizeName.Checked = true;
        this.rdoCustomizeNameAlwaysInvestor.Checked = this.currentActiveSetting.UseOnlyInvestorName;
        this.rdoCustomizeNameAlwaysLender.Checked = this.currentActiveSetting.UseOnlyLenderName;
        this.rdoCustomizeNameDepends.Checked = this.currentActiveSetting.UseInvestorAndLenderName;
        this.rdoCustomizeNameAlwaysInvestor.Enabled = true;
        this.rdoCustomizeNameAlwaysLender.Enabled = true;
        this.rdoCustomizeNameDepends.Enabled = true;
      }
      else
        this.chkCustomizeName.Checked = false;
    }

    private GVItem createLockExtPriceAdjustment(LockExtensionPriceAdjustment priceAdj)
    {
      return new GVItem((object) priceAdj.DaysToExtend)
      {
        SubItems = {
          (object) priceAdj.PriceAdjustment
        },
        Tag = (object) priceAdj
      };
    }

    private void llAdmin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      if (this.currentActiveSetting.AdminURL == "")
        return;
      int num = (int) new ProductPricingBrowser(this.currentActiveSetting.AdminURL, this.currentActiveSetting.PartnerName + " Administration").ShowDialog((IWin32Window) this);
    }

    private void llMoreInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      if (this.currentActiveSetting.MoreInfoURL == "")
        return;
      int num = (int) new ProductPricingBrowser(this.currentActiveSetting.MoreInfoURL, this.currentActiveSetting.PartnerName + " Information").ShowDialog((IWin32Window) this);
    }

    private void llSvcMgmt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      using (PerformanceMeter.StartNew(nameof (llSvcMgmt_LinkClicked), "Launch Services Management app", false, 525, nameof (llSvcMgmt_LinkClicked), "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\ProductPricing\\ProductPricingAdmin.cs"))
      {
        using (ServicesManagementBrowser managementBrowser = new ServicesManagementBrowser())
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE ServiceManagement.ShowDialog", 530, nameof (llSvcMgmt_LinkClicked), "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\ProductPricing\\ProductPricingAdmin.cs");
          PerformanceMeter.Current.AddCheckpoint("AFTER ServiceManagement.ShowDialog: " + managementBrowser.ShowDialog((IWin32Window) Form.ActiveForm).ToString(), 532, nameof (llSvcMgmt_LinkClicked), "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\ProductPricing\\ProductPricingAdmin.cs");
        }
      }
    }

    public override void Reset()
    {
      this.settings = this.session.ConfigurationManager.GetProductPricingSettings();
      this.initialPageValue();
      this.SetExtensionSettings();
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      if (!this.validateData())
        return;
      foreach (ProductPricingSetting setting in this.settings)
      {
        if (this.currentActiveSetting == null || setting.ProviderID != this.currentActiveSetting.ProviderID)
        {
          setting.Active = false;
        }
        else
        {
          setting.ImportToLoan = this.chkImportToLoanFile.Checked;
          setting.PartnerRequestLock = this.chkRequestLock.Checked;
          setting.PartnerLockConfirm = this.chkLockConfirm.Checked;
          setting.PartnerRequestLockWhenNoCurrentLock = this.chkRequestLockWOLock.Checked;
          setting.GetPricingRelock = this.chkGetPricingRelock.Checked;
          if (this.chkCustomizeName.Checked)
          {
            setting.UseCustomizedInvestorName = true;
            setting.UseOnlyInvestorName = this.rdoCustomizeNameAlwaysInvestor.Checked;
            setting.UseOnlyLenderName = this.rdoCustomizeNameAlwaysLender.Checked;
            setting.UseInvestorAndLenderName = this.rdoCustomizeNameDepends.Checked;
          }
          else
          {
            setting.UseCustomizedInvestorName = false;
            setting.UseOnlyInvestorName = false;
            setting.UseOnlyLenderName = false;
            setting.UseInvestorAndLenderName = false;
          }
          setting.IsExportUserFinished = this.currentActiveSetting.IsExportUserFinished;
          setting.Active = true;
        }
      }
      this.settings = this.session.ConfigurationManager.UpdateProductPricingSettings(this.settings);
      this.settingDictionary.Clear();
      foreach (ProductPricingSetting setting in this.settings)
      {
        if (!this.settingDictionary.ContainsKey(setting.ProviderID))
          this.settingDictionary.Add(setting.ProviderID, setting);
      }
      Dictionary<string, object> settings = new Dictionary<string, object>();
      if (this.chkElapseTime.Checked)
      {
        settings.Add("POLICIES.Pricing_ElapsedTime", (object) Utils.ParseInt((object) this.txtElapsedTime.Text, 1));
        this.session.StartupInfo.PolicySettings[(object) "Policies.Pricing_ElapsedTime"] = (object) Utils.ParseInt((object) this.txtElapsedTime.Text, 1);
      }
      else
      {
        settings.Add("POLICIES.Pricing_ElapsedTime", (object) Utils.ParseInt((object) this.txtElapsedTime.Text, 0));
        this.session.StartupInfo.PolicySettings[(object) "Policies.Pricing_ElapsedTime"] = (object) Utils.ParseInt((object) this.txtElapsedTime.Text, 0);
      }
      settings.Add("POLICIES.EnforceOnlyWhenNoCurrentLock", (object) this.chkTimerEnforceTiming.Checked);
      settings.Add("POLICIES.ReqeustLockFromPPEOnlyWOCurrentLock", (object) this.chkRequestLockWOLock.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnforceOnlyWhenNoCurrentLock"] = (object) this.chkTimerEnforceTiming.Checked;
      this.session.StartupInfo.PolicySettings[(object) "POLICIES.ReqeustLockFromPPEOnlyWOCurrentLock"] = (object) this.chkRequestLockWOLock.Checked;
      settings.Add("POLICIES.EnableLockExtension", (object) this.chkEnableLockExtension.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnableLockExtension"] = (object) this.chkEnableLockExtension.Checked;
      settings.Add("POLICIES.GetCurrentPricing", (object) this.chkGetCurrentPricing.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.GetCurrentPricing"] = (object) this.chkGetCurrentPricing.Checked;
      if (this.rbtnUnlimted.Checked)
      {
        settings.Add("POLICIES.LOCKEXTENSION_CAP_TYPE", (object) 0);
        this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtension_Cap_Type"] = (object) 0;
        settings.Add("POLICIES.LOCKEXTENSION_CAP_DAYS", (object) 0);
        this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtension_Cap_Days"] = (object) 0;
      }
      if (this.rbtnLockPeriod.Checked)
      {
        settings.Add("POLICIES.LOCKEXTENSION_CAP_TYPE", (object) 1);
        this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtension_Cap_Type"] = (object) 1;
        settings.Add("POLICIES.LOCKEXTENSION_CAP_DAYS", (object) 0);
        this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtension_Cap_Days"] = (object) 0;
      }
      if (this.rbtnCustomDays.Checked)
      {
        settings.Add("POLICIES.LOCKEXTENSION_CAP_TYPE", (object) 2);
        this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtension_Cap_Type"] = (object) 2;
        settings.Add("POLICIES.LOCKEXTENSION_CAP_DAYS", (object) Utils.ParseInt((object) this.txtExtensionDays.Text));
        this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtension_Cap_Days"] = (object) Utils.ParseInt((object) this.txtExtensionDays.Text);
      }
      settings.Add("POLICIES.LockExtensionAllowTotalCap", (object) this.ckLockExtensionAllowTotalCap.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtensionAllowTotalCap"] = (object) this.ckLockExtensionAllowTotalCap.Checked;
      int num1 = string.IsNullOrEmpty(this.txtLockExtensionAllowTotalCapDays.Text) ? 0 : Utils.ParseInt((object) this.txtLockExtensionAllowTotalCapDays.Text);
      settings.Add("POLICIES.LockExtensionAllowTotalCapDays", (object) num1);
      this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtensionAllowTotalCapDays"] = (object) num1;
      settings.Add("POLICIES.LockExtAllowTotalTimesCapEnabled", (object) this.ckLockExtAllowTotalTimesCap.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtAllowTotalTimesCapEnabled"] = (object) this.ckLockExtAllowTotalTimesCap.Checked;
      int num2 = string.IsNullOrEmpty(this.txtLockExtAllowTotalTimes.Text.Trim()) ? 0 : Utils.ParseInt((object) this.txtLockExtAllowTotalTimes.Text.Trim());
      settings.Add("POLICIES.LockExtAllowTotalTimesCap", (object) num2);
      this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtAllowTotalTimesCap"] = (object) num2;
      int num3 = string.IsNullOrEmpty(this.txtLockExpDays.Text) ? 60 : Utils.ParseInt((object) this.txtLockExpDays.Text);
      settings.Add("POLICIES.LOCKEXPDAYS", (object) num3);
      this.session.StartupInfo.PolicySettings[(object) "Policies.LockExpDays"] = (object) num3;
      settings.Add("POLICIES.LockExtensionCompanyControlled", (object) this.cmbLockExtensionOption.SelectedIndex);
      this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtensionCompanyControlled"] = (object) this.cmbLockExtensionOption.SelectedIndex;
      settings.Add("POLICIES.LockExtensionAllowDailyAdj", (object) this.chkAllowDailyAdj.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtensionAllowDailyAdj"] = (object) this.chkAllowDailyAdj.Checked;
      settings.Add("POLICIES.LockExtensionAllowFixedExt", (object) this.chkFixedExtension.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtensionAllowFixedExt"] = (object) this.chkFixedExtension.Checked;
      Decimal num4 = 0M;
      if (this.txtDailyPriceAdj.Text.Trim() != "")
        num4 = Utils.ParseDecimal((object) this.txtDailyPriceAdj.Text, 0M, 3);
      this.txtDailyPriceAdj.Text = num4.ToString("N3");
      settings.Add("POLICIES.LockExtensionDailyPriceAdj", (object) num4);
      this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtensionDailyPriceAdj"] = (object) num4;
      this.lockExtPriceAdjs.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvExtPriceAdj.Items)
        this.lockExtPriceAdjs.Add((LockExtensionPriceAdjustment) gvItem.Tag);
      this.session.ConfigurationManager.UpdateLockExtensionPriceAdjustments(this.lockExtPriceAdjs.ToArray());
      this.lockExtPriceAdjsOccur.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAdjustPerLockExt.Items)
        this.lockExtPriceAdjsOccur.Add((LockExtensionPriceAdjustment) gvItem.Tag);
      this.session.ConfigurationManager.UpdateLockExtPriceAdjustPerOccurrence(this.lockExtPriceAdjsOccur.ToArray());
      settings.Add("POLICIES.LockExtCalOpt_ApplyPriceAdj", (object) this.chkApplyPriceAdjOnOffDays.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.LockExtCalOpt_ApplyPriceAdj"] = (object) this.chkApplyPriceAdjOnOffDays.Checked;
      settings.Add("POLICIES.EnableLockCancellation", (object) this.chkEnableLockCancellation.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnableLockCancellation"] = (object) this.chkEnableLockCancellation.Checked;
      settings.Add("POLICIES.EnableLockCancellation_ReqForm", (object) this.chkLockCancellationOnLockReq.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnableLockCancellation_ReqForm"] = (object) this.chkLockCancellationOnLockReq.Checked;
      settings.Add("POLICIES.EnableLockVoid", (object) this.chkEnableLockVoid.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnableLockVoid"] = (object) this.chkEnableLockVoid.Checked;
      settings.Add("POLICIES.EnableLockVoidRetail", (object) this.chkEnableLockVoidRetail.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnableLockVoidRetail"] = (object) this.chkEnableLockVoidRetail.Checked;
      settings.Add("POLICIES.EnableLockVoidWholesale", (object) this.chkEnableLockVoidWholesale.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnableLockVoidWholesale"] = (object) this.chkEnableLockVoidWholesale.Checked;
      settings.Add("POLICIES.EnableZeroParPricingRetail", (object) this.chkZeroBasedParPricingRetail.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"] = (object) this.chkZeroBasedParPricingRetail.Checked;
      settings.Add("POLICIES.EnableZeroParPricingWholesale", (object) this.chkZeroBasedParPricingWholesale.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"] = (object) this.chkZeroBasedParPricingWholesale.Checked;
      settings.Add("POLICIES.RelockOnly", (object) this.chkReLockOnly.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.RelockOnly"] = (object) this.chkReLockOnly.Checked;
      settings.Add("POLICIES.EnableRelockForTpoClient", (object) this.chkEnableRelockForTpoClient.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnableRelockForTpoClient"] = (object) this.chkEnableRelockForTpoClient.Checked;
      settings.Add("POLICIES.EnableRelockOutsideLockDeskHours", (object) this.chkAllowRelockOutsideLDHours.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.EnableRelockOutsideLockDeskHours"] = (object) this.chkAllowRelockOutsideLDHours.Checked;
      settings.Add("POLICIES.EnableRelock", (object) this.chkRelock.Checked);
      this.session.StartupInfo.PolicySettings[(object) "POLICIES.EnableRelock"] = (object) this.chkRelock.Checked;
      if (this.currentActiveSetting != null && !this.currentActiveSetting.IsEPPS)
        settings.Add("Trade.EPPSLoanProgEliPricing", (object) "False");
      settings.Add("POLICIES.RelockAllowTotalCap", (object) this.ckRelockAllowTotalCap.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.RelockAllowTotalCap"] = (object) this.ckRelockAllowTotalCap.Checked;
      int num5 = string.IsNullOrEmpty(this.txtRelockAllowTotalCapDays.Text) ? 0 : Utils.ParseInt((object) this.txtRelockAllowTotalCapDays.Text);
      settings.Add("POLICIES.RelockAllowTotalCapTimes", (object) num5);
      this.session.StartupInfo.PolicySettings[(object) "Policies.RelockAllowTotalCapTimes"] = (object) num5;
      settings.Add("POLICIES.RelockFeeAllowed", (object) this.ckRelockFeeAllowed.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.RelockFeeAllowed"] = (object) this.ckRelockFeeAllowed.Checked;
      Decimal num6 = string.IsNullOrEmpty(this.txtRelockFee.Text) ? 0M : Utils.ParseDecimal((object) this.txtRelockFee.Text);
      settings.Add("POLICIES.RelockFee", (object) num6);
      settings.Add("POLICIES.AllowNewLockOutsideLDHours", (object) this.chkAllowNewLockOutsideLDHours.Checked);
      this.session.StartupInfo.PolicySettings[(object) "Policies.RelockFee"] = (object) num6;
      settings.Add("POLICIES.NotAllowPricingChange", (object) this.chkDontAllowPriceChange.Checked);
      this.session.StartupInfo.PolicySettings[(object) "POLICIES.NotAllowPricingChange"] = (object) this.chkDontAllowPriceChange.Checked;
      settings.Add("POLICIES.LockUpdateExtensionFees", (object) this.chkLockUpdateExtensionFees.Checked);
      this.session.StartupInfo.PolicySettings[(object) "POLICIES.LockUpdateExtensionFees"] = (object) this.chkLockUpdateExtensionFees.Checked;
      settings.Add("POLICIES.LockUpdateCustomPriceAdjustments", (object) this.chkLockUpdateCustomPriceAdjustments.Checked);
      this.session.StartupInfo.PolicySettings[(object) "POLICIES.LockUpdateCustomPriceAdjustments"] = (object) this.chkLockUpdateCustomPriceAdjustments.Checked;
      settings.Add("POLICIES.LockUpdatePriceConcessions", (object) this.chkLockUpdatePriceConcessions.Checked);
      this.session.StartupInfo.PolicySettings[(object) "POLICIES.LockUpdatePriceConcessions"] = (object) this.chkLockUpdatePriceConcessions.Checked;
      settings.Add("POLICIES.LockUpdateReLockfees", (object) this.chkLockUpdateReLockfees.Checked);
      this.session.StartupInfo.PolicySettings[(object) "POLICIES.LockUpdateReLockfees"] = (object) this.chkLockUpdateReLockfees.Checked;
      if (this.chkUseWorstCasePrice.Enabled)
      {
        settings.Add("POLICIES.ReLockExtensionFees", (object) this.chkReLockExtensionFees.Checked);
        this.session.StartupInfo.PolicySettings[(object) "POLICIES.ReLockExtensionFees"] = (object) this.chkReLockExtensionFees.Checked;
        settings.Add("POLICIES.ReLockCustomPriceAdjustments", (object) this.chkReLockCustomPriceAdjustments.Checked);
        this.session.StartupInfo.PolicySettings[(object) "POLICIES.ReLockCustomPriceAdjustments"] = (object) this.chkReLockCustomPriceAdjustments.Checked;
        settings.Add("POLICIES.ReLockPriceConcessions", (object) this.chkReLockPriceConcessions.Checked);
        this.session.StartupInfo.PolicySettings[(object) "POLICIES.ReLockPriceConcessions"] = (object) this.chkReLockPriceConcessions.Checked;
        settings.Add("POLICIES.ReLockReLockfees", (object) this.chkReLockReLockfees.Checked);
        this.session.StartupInfo.PolicySettings[(object) "POLICIES.ReLockReLockfees"] = (object) this.chkReLockReLockfees.Checked;
        string str = string.IsNullOrEmpty(this.txtWorstCasePrice.Text) ? string.Empty : this.txtWorstCasePrice.Text.Trim();
        settings.Add("POLICIES.WorstCasePriceEqualDays", (object) str);
        this.session.StartupInfo.PolicySettings[(object) "Policies.WorstCasePriceEqualDays"] = (object) str;
      }
      if (this.currentActiveSetting != null && this.currentActiveSetting.VendorPlatform == VendorPlatform.EPC2)
      {
        settings.Add("POLICIES.WorstCasePrice", (object) this.chkUseWorstCasePrice.Checked);
        this.session.StartupInfo.PolicySettings[(object) "POLICIES.WorstCasePrice"] = (object) this.chkUseWorstCasePrice.Checked;
      }
      settings.Add("POLICIES.WaiveFee", (object) this.chkWaiveFeeAfter.Checked);
      this.session.StartupInfo.PolicySettings[(object) "POLICIES.WaiveFee"] = (object) this.chkWaiveFeeAfter.Checked;
      string str1 = string.IsNullOrEmpty(this.txtWaiveFeeAfter.Text) ? string.Empty : this.txtWaiveFeeAfter.Text.Trim();
      settings.Add("POLICIES.WaiveFeeAfterDays", (object) str1);
      this.session.StartupInfo.PolicySettings[(object) "Policies.WaiveFeeAfterDays"] = (object) str1;
      settings.Add("POLICIES.RestrictLockPeriod", (object) this.chkRestrictLockPeriod.Checked);
      this.session.StartupInfo.PolicySettings[(object) "POLICIES.RestrictLockPeriod"] = (object) this.chkRestrictLockPeriod.Checked;
      settings.Add("POLICIES.AllowPPESelection", (object) this.chkAllowPPESelection.Checked);
      this.session.StartupInfo.PolicySettings[(object) "POLICIES.AllowPPESelection"] = (object) this.chkAllowPPESelection.Checked;
      if (!this.GetShipDarkFlag())
      {
        ProductPricingSetting currentActiveSetting1 = this.currentActiveSetting;
        if ((currentActiveSetting1 != null ? (currentActiveSetting1.IsEPPS ? 1 : 0) : 0) != 0)
        {
          ProductPricingSetting currentActiveSetting2 = this.currentActiveSetting;
          if ((currentActiveSetting2 != null ? (currentActiveSetting2.VendorPlatform == VendorPlatform.EPC2 ? 1 : 0) : 0) != 0)
          {
            settings.Add("POLICIES.ENABLEALLCHANNEL", (object) false);
            settings.Add("POLICIES.ENABLEONRPRET", (object) false);
            this.session.StartupInfo.PolicySettings[(object) "POLICIES.ENABLEALLCHANNEL"] = (object) false;
            this.session.StartupInfo.PolicySettings[(object) "POLICIES.ENABLEONRPRET"] = (object) false;
            if (this.session.StartupInfo.ProductPricingPartner == null || !this.session.StartupInfo.ProductPricingPartner.IsEPPS && this.session.StartupInfo.ProductPricingPartner.VendorPlatform != VendorPlatform.EPC2)
              WorkflowServiceClient.SetupWorkflowProcessAction(this.session.SessionObjects, new Bam().GetAccessToken("sc"), Session.Connection?.Server?.InstanceName);
          }
        }
      }
      this.session.ServerManager.UpdateServerSettings((IDictionary) settings, true, false);
      this.setDirtyFlag(false);
      this.session.StartupInfo.ProductPricingPartner = this.currentActiveSetting;
    }

    private bool validateData()
    {
      if (this.chkElapseTime.Checked && this.txtElapsedTime.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a value in Elapsed Time Setting.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtElapsedTime.Text = "15";
        return false;
      }
      if (this.chkCustomizeName.Checked && !this.rdoCustomizeNameAlwaysInvestor.Checked && !this.rdoCustomizeNameAlwaysLender.Checked && !this.rdoCustomizeNameDepends.Checked)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a Customize Investor Name Import Setting.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.chkFixedExtension.Checked && this.gvExtPriceAdj.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide fixed price adjustments.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.chkAllowDailyAdj.Checked && this.txtDailyPriceAdj.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide daily adjustment.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.cmbLockExtensionOption.SelectedIndex == 1)
      {
        bool flag1 = this.chkFixedExtension.Checked && this.gvExtPriceAdj.Items.Count > 0;
        bool flag2 = this.chkAllowDailyAdj.Checked && this.txtDailyPriceAdj.Text.Trim() != "";
        if (!flag1 && !flag2)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please provide company specific lock extension price adjustments.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvExtPriceAdj.Items)
        {
          if (((LockExtensionPriceAdjustment) gvItem.Tag).DaysToExtend == 0)
          {
            this.gvExtPriceAdj.Focus();
            return false;
          }
        }
      }
      if (this.cmbLockExtensionOption.SelectedIndex == 2)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAdjustPerLockExt.Items)
        {
          if (((LockExtensionPriceAdjustment) gvItem.Tag).DaysToExtend == 0)
          {
            this.gvAdjustPerLockExt.Focus();
            return false;
          }
        }
      }
      if (this.chkEnableLockExtension.Checked && this.cmbLockExtensionOption.SelectedIndex != 2 && this.cmbLockExtensionOption.SelectedIndex != -1)
      {
        if (!this.rbtnUnlimted.Checked && !this.rbtnLockPeriod.Checked && !this.rbtnCustomDays.Checked)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must select How many days should lock extension be limited to.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (this.rbtnCustomDays.Checked && string.IsNullOrEmpty(this.txtExtensionDays.Text.Trim()))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please provide how many days lock extension is limted to.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtExtensionDays.Text = "30";
          return false;
        }
        if (this.ckLockExtAllowTotalTimesCap.Checked && string.IsNullOrEmpty(this.txtLockExtAllowTotalTimes.Text.Trim()))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please provide how many times a lock extension should be limited to.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtLockExtAllowTotalTimes.Text = "3";
          return false;
        }
        if (!string.IsNullOrEmpty(this.txtLockExtAllowTotalTimes.Text.Trim()) && (Convert.ToInt32(this.txtLockExtAllowTotalTimes.Text.Trim()) < 1 || Convert.ToInt32(this.txtLockExtAllowTotalTimes.Text.Trim()) > 10))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Data out of range: 1 - 10", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtLockExtAllowTotalTimes.Text = "";
          this.txtLockExtAllowTotalTimes.Focus();
          return false;
        }
      }
      if (this.ckLockExtensionAllowTotalCap.Checked && string.IsNullOrEmpty(this.txtLockExtensionAllowTotalCapDays.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide how many lock days an extended lock should be limited to.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtLockExtensionAllowTotalCapDays.Text = "90";
        return false;
      }
      if (this.chkGetCurrentPricing.Checked && string.IsNullOrEmpty(this.txtLockExpDays.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide number of days", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtLockExpDays.Text = "60";
        return false;
      }
      if (!string.IsNullOrEmpty(this.txtLockExpDays.Text.Trim()) && (Convert.ToInt32(this.txtLockExpDays.Text) < 1 || Convert.ToInt32(this.txtLockExpDays.Text) > 999))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Data out of range: 1 - 999", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtLockExpDays.Text = "";
        this.txtLockExpDays.Focus();
        return false;
      }
      if (this.ckLockExtensionAllowTotalCap.Checked && string.IsNullOrEmpty(this.txtLockExtensionAllowTotalCapDays.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide number of days", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (!string.IsNullOrEmpty(this.txtLockExtensionAllowTotalCapDays.Text.Trim()) && (Convert.ToInt32(this.txtLockExtensionAllowTotalCapDays.Text) < 1 || Convert.ToInt32(this.txtLockExtensionAllowTotalCapDays.Text) > 999))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Data out of range: 1 - 999", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtLockExtensionAllowTotalCapDays.Text = "";
        this.txtLockExtensionAllowTotalCapDays.Focus();
        return false;
      }
      if (this.ckRelockAllowTotalCap.Checked && string.IsNullOrEmpty(this.txtRelockAllowTotalCapDays.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide how many times a Re-Lock should be limited to.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtRelockAllowTotalCapDays.Text = "2";
        this.txtRelockAllowTotalCapDays.Focus();
        return false;
      }
      if (this.ckRelockFeeAllowed.Checked && string.IsNullOrEmpty(this.txtRelockFee.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide Re-Lock fee.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtRelockFee.Focus();
        return false;
      }
      if (!string.IsNullOrEmpty(this.txtRelockAllowTotalCapDays.Text.Trim()) && (Convert.ToInt32(this.txtRelockAllowTotalCapDays.Text) < 1 || Convert.ToInt32(this.txtRelockAllowTotalCapDays.Text) > 10))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Data out of range: 1 - 10", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtRelockAllowTotalCapDays.Text = "2";
        this.txtRelockAllowTotalCapDays.Focus();
        return false;
      }
      if (this.chkWaiveFeeAfter.Checked && string.IsNullOrEmpty(this.txtWaiveFeeAfter.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide number of days for Waive fees", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtWaiveFeeAfter.Text = "";
        this.txtWaiveFeeAfter.Focus();
        return false;
      }
      if (this.exportCredential != null && this.exportCredential.IsExportEnabled && this.exportCredential.ActiveServiceSetupForProvider == null)
      {
        string[] customButtonList = new string[2]
        {
          "Export",
          "Save without Export"
        };
        if (Utils.Dialog((IWin32Window) this, "No services can be ordered until the Service Setup is established", customButtonList, MessageBoxIcon.Hand) != DialogResult.OK)
          return false;
        if (Utils.Result == customButtonList[0])
          this.llExportUser_LinkClicked((object) this.llExportUser, (LinkLabelLinkClickedEventArgs) null);
      }
      return true;
    }

    private void updateUISetting(ProductPricingSetting setting)
    {
      this.pnlProviderSettings.Visible = true;
      this.currentActiveSetting = setting;
      this.exportCredential = new ExportCredential(this.session.SessionObjects, this.currentActiveSetting, new Bam().GetAccessToken("sc"));
      this.pnlExportUser.Visible = this.exportCredential.IsExportEnabled;
      if (setting == null)
      {
        this.llAdmin.Enabled = false;
        this.llMoreInfo.Enabled = false;
        this.llSvcMgmt.Visible = false;
        this.lblSvcMgmtBar.Visible = false;
        this.chkImportToLoanFile.Enabled = false;
        this.chkRequestLock.Enabled = false;
        this.chkLockConfirm.Enabled = false;
        this.chkRequestLockWOLock.Enabled = false;
        this.chkGetPricingRelock.Enabled = false;
        if (this.chkRelock.Checked)
          this.chkAllowNewLockOutsideLDHours.Enabled = true;
        else
          this.chkAllowNewLockOutsideLDHours.Enabled = false;
        this.chkDontAllowPriceChange.Enabled = false;
        this.chkDontAllowPriceChange.Checked = false;
        this.chkAllowPPESelection.Checked = false;
        this.adjustPosition();
        this.LockUpdateReLockSettingsChange(true);
      }
      else
      {
        this.pnlProviderSettings.Visible = true;
        if (setting.AdminURL == "")
          this.llAdmin.Enabled = false;
        else
          this.llAdmin.Enabled = true;
        if (setting.MoreInfoURL == "")
          this.llMoreInfo.Enabled = false;
        else
          this.llMoreInfo.Enabled = true;
        this.llSvcMgmt.Visible = setting.IsEPPS && setting.VendorPlatform == VendorPlatform.EPC2;
        this.lblSvcMgmtBar.Visible = setting.IsEPPS && setting.VendorPlatform == VendorPlatform.EPC2;
        if (setting.SupportImportToLoan)
          this.chkImportToLoanFile.Enabled = true;
        else
          this.chkImportToLoanFile.Enabled = false;
        this.chkImportToLoanFile.Checked = setting.ImportToLoan;
        if (setting.SupportPartnerLockConfirm)
          this.chkLockConfirm.Enabled = true;
        else
          this.chkLockConfirm.Enabled = false;
        this.chkLockConfirm.Checked = setting.PartnerLockConfirm;
        if (setting.SupportPartnerRequestLock)
          this.chkRequestLock.Enabled = true;
        else
          this.chkRequestLock.Enabled = false;
        this.chkRequestLock.Checked = setting.PartnerRequestLock;
        if (setting.IsEPPS && this.chkRelock.Checked)
          this.chkGetPricingRelock.Enabled = true;
        else
          this.chkGetPricingRelock.Enabled = this.chkGetPricingRelock.Checked = false;
        this.chkGetPricingRelock.Checked = setting.GetPricingRelock;
        if (setting != null)
        {
          if (setting.VendorPlatform == VendorPlatform.EPC2)
          {
            this.chkDontAllowPriceChange.Enabled = false;
            this.chkDontAllowPriceChange.Checked = false;
          }
          else
          {
            this.chkDontAllowPriceChange.Enabled = true;
            this.chkDontAllowPriceChange.Checked = this.session.StartupInfo.PolicySettings[(object) "POLICIES.NotAllowPricingChange"] != null && (bool) this.session.StartupInfo.PolicySettings[(object) "POLICIES.NotAllowPricingChange"];
          }
        }
        this.chkAllowNewLockOutsideLDHours.Enabled = false;
        this.chkAllowNewLockOutsideLDHours.Checked = false;
        this.chkAllowPPESelection.Checked = this.session.StartupInfo.PolicySettings[(object) "POLICIES.AllowPPESelection"] != null && (bool) this.session.StartupInfo.PolicySettings[(object) "POLICIES.AllowPPESelection"];
        this.initPageSectionCustomizeName();
        this.refreshChkRequestLockWOLockControl();
        this.adjustPosition();
      }
    }

    private async void updateMoreInfoLink(ProductPricingSetting selectedSetting)
    {
      Epc2Provider epc2Provider = await Task.Run<Epc2Provider>((Func<Task<Epc2Provider>>) (() => Epc2ServiceClient.GetProviderById(this.session.SessionObjects, new Bam().GetAccessToken("sc"), selectedSetting.ProviderID)));
      selectedSetting.MoreInfoURL = epc2Provider == null || epc2Provider.AdditionalLinks == null ? "" : epc2Provider.AdditionalLinks.SingleOrDefault<AdditionalLinks>((Func<AdditionalLinks, bool>) (item => item.Type == "PartnerAdminGuide"))?.Url ?? "";
      this.updateUISetting(selectedSetting);
    }

    private void cbProvider_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      string key = this.cbProvider.SelectedIndex != 0 ? ((ComboboxItem) this.cbProvider.SelectedItem).Value.ProviderID ?? "" : "";
      ProductPricingSetting setting = this.settingDictionary.ContainsKey(key) ? this.settingDictionary[key] : (ProductPricingSetting) null;
      if (setting != null && this.currentActiveSetting != null && this.currentActiveSetting.PartnerName == setting.PartnerName)
        return;
      if (setting != null && setting.VendorPlatform == VendorPlatform.EPC2)
        this.updateMoreInfoLink(setting);
      else
        this.updateUISetting(setting);
      this.LockUpdateReLockSettingsChange(true);
      if (this.currentActiveSetting != null && !this.currentActiveSetting.UseCustomizedInvestorName && this.cbProvider.SelectedItem != null && this.cbProvider.SelectedItem is ComboboxItem && ((ComboboxItem) this.cbProvider.SelectedItem).Value.IsEPPS)
      {
        this.chkCustomizeName.Enabled = true;
        this.chkCustomizeName.Checked = false;
        this.currentActiveSetting.IsCustomizeInvestorName = true;
      }
      this.updateDefaultSettingsForRelockFee();
      this.retainWaiveFeeForEPCProvider();
      this.SetExtensionSettings();
      this.UpdateRelockOnlySetting();
      this.setDirtyFlag(true);
    }

    private void UpdateRelockOnlySetting()
    {
      if (!this.chkRelock.Checked)
        return;
      ProductPricingSetting currentActiveSetting1 = this.currentActiveSetting;
      if ((currentActiveSetting1 != null ? (currentActiveSetting1.IsEPPS ? 1 : 0) : 0) != 0)
      {
        ProductPricingSetting currentActiveSetting2 = this.currentActiveSetting;
        if ((currentActiveSetting2 != null ? (currentActiveSetting2.VendorPlatform == VendorPlatform.EPC2 ? 1 : 0) : 0) != 0)
        {
          this.chkReLockOnly.Checked = true;
          this.chkReLockOnly.Enabled = false;
          return;
        }
      }
      this.chkReLockOnly.Enabled = true;
      this.chkReLockOnly.Checked = false;
    }

    private void SetExtensionSettings()
    {
      IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Policies");
      int num = (int) serverSettings[(object) "Policies.LockExtensionCompanyControlled"];
      this.rbtnUnlimted.Enabled = this.chkEnableLockExtension.Checked;
      this.rbtnLockPeriod.Enabled = this.chkEnableLockExtension.Checked;
      this.rbtnCustomDays.Enabled = this.chkEnableLockExtension.Checked;
      this.txtExtensionDays.Enabled = this.chkEnableLockExtension.Checked && this.rbtnCustomDays.Checked;
      this.cmbLockExtensionOption.Enabled = this.ckLockExtensionAllowTotalCap.Enabled = this.label12.Enabled = this.chkEnableLockExtension.Checked;
      if (this.cmbLockExtensionOption.SelectedIndex < 0)
        this.cmbLockExtensionOption.SelectedIndex = num >= 0 ? num : 0;
      if (this.cmbLockExtensionOption.SelectedIndex <= 0)
        this.groupContainer1.Height = this.groupContainer1ContractedHeight;
      else
        this.groupContainer1.Height = this.groupContainer1ExpandedHeight;
      this.ckLockExtensionAllowTotalCap.Enabled = this.chkEnableLockExtension.Checked;
      this.txtLockExtensionAllowTotalCapDays.Enabled = this.chkEnableLockExtension.Checked && this.ckLockExtensionAllowTotalCap.Checked;
      this.setExtensionLimit(serverSettings);
      if (this.chkEnableLockExtension.Checked && !this.rbtnLockPeriod.Checked && !this.rbtnUnlimted.Checked && !this.rbtnCustomDays.Checked)
        this.rbtnUnlimted.Checked = true;
      this.chkRelock_CheckedChanged((object) null, (EventArgs) null);
    }

    private void SetLockRelockSetting()
    {
      this.labelGetCurrentPricingDays.Enabled = this.chkGetCurrentPricing.Enabled;
      this.labelRelockFeeAllowed.Enabled = this.ckRelockFeeAllowed.Enabled;
      this.labelRelockAllowTotalCap.Enabled = this.ckRelockAllowTotalCap.Enabled;
    }

    private void updateDefaultSettingsForRelockFee()
    {
      if ((this.cbProvider.SelectedItem == null || !(this.cbProvider.SelectedItem.ToString().ToLower() == "no provider selected")) && (this.currentActiveSetting == null || this.currentActiveSetting.VendorPlatform != VendorPlatform.EPC2))
        return;
      if (this.ckRelockFeeAllowed.Checked)
      {
        this.chkWaiveFeeAfter.Enabled = true;
        this.chkWaiveFeeAfter.Checked = false;
      }
      else
      {
        this.chkWaiveFeeAfter.Checked = false;
        this.chkWaiveFeeAfter.Enabled = false;
      }
    }

    private void retainWaiveFeeForEPCProvider()
    {
      if (this.cbProvider.SelectedItem == null || this.currentActiveSetting == null || this.currentActiveSetting.VendorPlatform != VendorPlatform.EPC2)
        return;
      this.chkWaiveFeeAfter.Checked = this.session.StartupInfo.PolicySettings[(object) "Policies.WaiveFee"] != null && (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.WaiveFee"];
      if (!this.chkWaiveFeeAfter.Checked)
        return;
      this.txtWaiveFeeAfter.Text = this.session.StartupInfo.PolicySettings[(object) "Policies.WaiveFeeAfterDays"] != null ? this.session.StartupInfo.PolicySettings[(object) "Policies.WaiveFeeAfterDays"].ToString() : "";
    }

    private void chk_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || sender == null)
        return;
      this.setDirtyFlag(true);
    }

    private void txtLockExpDays_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar) || e.KeyChar.Equals((object) Keys.Space))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        if (this.txtLockExpDays.Text.Length > 3)
          e.Handled = true;
        this.setDirtyFlag(true);
      }
    }

    private void chkGetCurrentPricing_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkGetCurrentPricing.Checked)
      {
        this.txtLockExpDays.Enabled = true;
        this.txtLockExpDays.Text = this.session.StartupInfo.PolicySettings[(object) "Policies.LockExpDays"] != null ? this.session.StartupInfo.PolicySettings[(object) "Policies.LockExpDays"].ToString() : "60";
      }
      else
      {
        this.txtLockExpDays.Enabled = false;
        this.txtLockExpDays.Text = string.Empty;
      }
      this.setDirtyFlag(true);
    }

    private void txtElapsedTime_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.internalTrigger)
        return;
      this.formatText((object) this.txtElapsedTime, e);
      if (this.txtElapsedTime.Text.Trim() == "")
      {
        this.setDirtyFlag(true);
      }
      else
      {
        int num1 = Utils.ParseInt((object) this.txtElapsedTime.Text, 0);
        if (num1 < 1 || num1 > 9999)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Data out of range: 1 - 9999", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.internalTrigger = true;
          this.txtElapsedTime.Text = "15";
          this.internalTrigger = false;
        }
        this.setDirtyFlag(true);
      }
    }

    public bool formatText(object sender, EventArgs e)
    {
      if (this.internalTrigger)
        return false;
      if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
        return false;
      }
      if (sender != this.txtElapsedTime)
        return false;
      FieldFormat dataFormat = FieldFormat.INTEGER;
      TextBox textBox = (TextBox) sender;
      string text = textBox.Text;
      bool needsUpdate = false;
      string newVal = Utils.FormatInput(text, dataFormat, ref needsUpdate);
      if (!(newVal != textBox.Text))
        return false;
      this.internalTrigger = true;
      int selectionStart = textBox.SelectionStart;
      int newCursorPosition = Utils.GetNewCursorPosition(textBox.Text, newVal, textBox.SelectionStart, new string[2]
      {
        "-",
        ","
      });
      textBox.Text = newVal;
      textBox.SelectionStart = newCursorPosition;
      this.internalTrigger = false;
      return true;
    }

    private void txt_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void chkElapseTime_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkElapseTime.Checked)
      {
        this.txtElapsedTime.Enabled = true;
        this.txtElapsedTime.Text = "15";
        this.chkTimerEnforceTiming.Enabled = true;
      }
      else
      {
        this.internalTrigger = true;
        this.txtElapsedTime.Text = "0";
        this.txtElapsedTime.Enabled = false;
        this.internalTrigger = false;
        this.chkTimerEnforceTiming.Checked = false;
        this.chkTimerEnforceTiming.Enabled = false;
      }
      this.chk_CheckedChanged(sender, e);
    }

    private void chkRequestLock_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkRequestLock.Checked)
      {
        this.chkRequestLockWOLock.Enabled = true;
        this.refreshChkRequestLockWOLockControl();
      }
      else
      {
        this.chkRequestLockWOLock.Enabled = false;
        this.chkRequestLockWOLock.Checked = false;
      }
      this.chk_CheckedChanged(sender, e);
    }

    private void chkCustomizeName_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkCustomizeName.Checked)
      {
        this.rdoCustomizeNameAlwaysInvestor.Enabled = true;
        this.rdoCustomizeNameAlwaysLender.Enabled = true;
        this.rdoCustomizeNameDepends.Enabled = true;
        if (this.session.EncompassEdition == EncompassEdition.Banker)
          this.rdoCustomizeNameAlwaysInvestor.Checked = true;
      }
      else
      {
        this.rdoCustomizeNameAlwaysInvestor.Checked = false;
        this.rdoCustomizeNameAlwaysLender.Checked = false;
        this.rdoCustomizeNameDepends.Checked = false;
        this.rdoCustomizeNameAlwaysInvestor.Enabled = false;
        this.rdoCustomizeNameAlwaysLender.Enabled = false;
        this.rdoCustomizeNameDepends.Enabled = false;
      }
      this.chk_CheckedChanged(sender, e);
    }

    private void rdoCustomizeNameAlwaysInvestor_CheckedChanged(object sender, EventArgs e)
    {
      this.chk_CheckedChanged(sender, e);
    }

    private void rdoCustomizeNameAlwaysLender_CheckedChanged(object sender, EventArgs e)
    {
      this.chk_CheckedChanged(sender, e);
    }

    private void rdoCustomizeNameDepends_CheckedChanged(object sender, EventArgs e)
    {
      this.chk_CheckedChanged(sender, e);
    }

    private void chkEnableLockExtension_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkEnableLockExtension.Checked)
      {
        this.cmbLockExtensionOption.Enabled = true;
        this.ToggleLockExtensionCapFields();
        this.toggleLockExtensionForOccurrence();
      }
      else
      {
        this.cmbLockExtensionOption.Enabled = this.gvExtPriceAdj.Enabled = this.chkFixedExtension.Enabled = this.siBtnEdit.Enabled = this.siBtnAdd.Enabled = this.siBtnDelete.Enabled = this.chkAllowDailyAdj.Enabled = this.txtDailyPriceAdj.Enabled = this.chkApplyPriceAdjOnOffDays.Enabled = this.gvAdjustPerLockExt.Enabled = this.btnAddAdjustPerLockExt.Enabled = this.btnEditAdjustPerLockExt.Enabled = this.btnDeleteAdjustPerLockExt.Enabled = false;
        this.ToggleLockExtensionCapFields();
      }
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void toggleLockExtensionForOccurrence()
    {
      if (this.cmbLockExtensionOption.SelectedIndex != 2 && this.cmbLockExtensionOption.SelectedIndex != -1)
        return;
      this.rbtnUnlimted.Enabled = false;
      this.rbtnLockPeriod.Enabled = false;
      this.rbtnCustomDays.Enabled = false;
      this.txtExtensionDays.Enabled = false;
      this.ckLockExtAllowTotalTimesCap.Enabled = false;
      this.txtLockExtAllowTotalTimes.Enabled = false;
      this.label17.Enabled = false;
      this.label9.Enabled = false;
    }

    private void chkAllowDailyAdj_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkAllowDailyAdj.Checked)
      {
        this.txtDailyPriceAdj.Enabled = this.chkApplyPriceAdjOnOffDays.Enabled = true;
      }
      else
      {
        this.txtDailyPriceAdj.Enabled = this.chkApplyPriceAdjOnOffDays.Enabled = false;
        this.chkApplyPriceAdjOnOffDays.Checked = false;
      }
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void txtDailyPriceAdj_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void siBtnAdd_Click(object sender, EventArgs e)
    {
      LockExtensionPriceAdjustment priceAdj = new LockExtensionPriceAdjustment(0, 0M);
      GVItem extPriceAdjustment = this.createLockExtPriceAdjustment(priceAdj);
      this.gvExtPriceAdj.Items.Add(extPriceAdjustment);
      this.gvExtPriceAdj.StartEditing(extPriceAdjustment.SubItems[0]);
      this.lockExtPriceAdjs.Add(priceAdj);
      this.lockExtPriceAdjs.Sort();
      this.gvExtPriceAdj.SelectedIndexChanged -= new EventHandler(this.gvExtPriceAdj_SelectedIndexChanged);
      foreach (GVItem selectedItem in this.gvExtPriceAdj.SelectedItems)
        selectedItem.Selected = false;
      extPriceAdjustment.Selected = true;
      this.gvExtPriceAdj.SelectedIndexChanged += new EventHandler(this.gvExtPriceAdj_SelectedIndexChanged);
    }

    private void siBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvExtPriceAdj.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a Price Adjustment to delete.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        foreach (GVItem selectedItem in this.gvExtPriceAdj.SelectedItems)
        {
          this.lockExtPriceAdjs.Remove((LockExtensionPriceAdjustment) selectedItem.Tag);
          this.gvExtPriceAdj.Items.Remove(selectedItem);
        }
        this.setDirtyFlag(true);
      }
    }

    private void gvExtPriceAdj_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Index == 0)
      {
        if (!Utils.IsInt((object) e.EditorControl.Text))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please provide a valid data for Days to Extend", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.EditorControl.Text = e.SubItem.Text;
          e.Cancel = true;
          return;
        }
        if (Utils.ParseInt((object) e.EditorControl.Text) <= 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please provide a valid data for Days to Extend", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.EditorControl.Text = e.SubItem.Text;
          e.Cancel = true;
          return;
        }
        bool flag = false;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvExtPriceAdj.Items)
        {
          if (gvItem.Index != e.SubItem.Item.Index && gvItem.SubItems[0].Text == e.EditorControl.Text)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "There already exists a price adjustment with the same Days to Extend.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.EditorControl.Text = e.SubItem.Text;
          e.Cancel = true;
          return;
        }
        ((LockExtensionPriceAdjustment) e.SubItem.Item.Tag).DaysToExtend = Utils.ParseInt((object) e.EditorControl.Text);
      }
      else
      {
        if (!Utils.IsDecimal((object) e.EditorControl.Text))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please provide a valid data for Price Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.Cancel = true;
          return;
        }
        if (Utils.ParseDecimal((object) e.EditorControl.Text, 0M, 3) > 0M && Utils.Dialog((IWin32Window) this, "Are you sure you want to implement a positive Price Adjustment", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        {
          e.EditorControl.Text = e.SubItem.Text;
          e.Cancel = true;
          return;
        }
        e.EditorControl.Text = Utils.ParseDecimal((object) e.EditorControl.Text, 0M, 3).ToString("N3");
        ((LockExtensionPriceAdjustment) e.SubItem.Item.Tag).PriceAdjustment = Utils.ParseDecimal((object) e.EditorControl.Text, 0M, 3);
      }
      this.setExtPriceAdjustmentBtns(true);
      this.setDirtyFlag(true);
    }

    private void cmbLockExtensionOption_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbLockExtensionOption.SelectedIndex == 0)
      {
        this.pnlLockextensionConfig.Visible = false;
        this.pnlAdjustPerLockExt.Visible = false;
        this.groupContainer1.Height = this.groupContainer1ContractedHeight;
        this.rbtnLockPeriod.Enabled = true;
        this.rbtnUnlimted.Enabled = true;
        this.rbtnCustomDays.Enabled = true;
        this.txtExtensionDays.Enabled = this.rbtnCustomDays.Checked && this.rbtnCustomDays.Enabled;
        this.label9.Enabled = true;
        this.ckLockExtAllowTotalTimesCap.Enabled = true;
        this.txtLockExtAllowTotalTimes.Enabled = this.ckLockExtAllowTotalTimesCap.Checked;
        this.label17.Enabled = true;
      }
      else if (this.cmbLockExtensionOption.SelectedIndex == 1)
      {
        this.pnlLockextensionConfig.Visible = true;
        this.pnlAdjustPerLockExt.Visible = false;
        this.chkAllowDailyAdj.Enabled = this.chkFixedExtension.Enabled = this.chkApplyPriceAdjOnOffDays.Enabled = true;
        this.chkAllowDailyAdj_CheckedChanged((object) null, (EventArgs) null);
        this.chkFixedExtension_CheckedChanged((object) null, (EventArgs) null);
        this.groupContainer1.Height = this.groupContainer1ExpandedHeight;
        this.rbtnLockPeriod.Enabled = true;
        this.rbtnUnlimted.Enabled = true;
        this.rbtnCustomDays.Enabled = true;
        this.txtExtensionDays.Enabled = this.rbtnCustomDays.Checked && this.rbtnCustomDays.Enabled;
        this.label9.Enabled = true;
        this.ckLockExtAllowTotalTimesCap.Enabled = true;
        this.txtLockExtAllowTotalTimes.Enabled = this.ckLockExtAllowTotalTimesCap.Checked;
        this.label17.Enabled = true;
        this.chkFixedExtension.Enabled = this.chkAllowDailyAdj.Enabled = this.chkEnableLockExtension.Checked;
      }
      else if (this.cmbLockExtensionOption.SelectedIndex == -1)
      {
        this.pnlLockextensionConfig.Visible = false;
        this.pnlAdjustPerLockExt.Visible = false;
        this.groupContainer1.Height = this.groupContainer1ContractedHeight;
      }
      else
      {
        this.pnlLockextensionConfig.Visible = false;
        this.pnlAdjustPerLockExt.Visible = true;
        this.groupContainer1.Height = this.groupContainer1ExpandedHeight;
        this.rbtnLockPeriod.Enabled = false;
        this.rbtnLockPeriod.Checked = false;
        this.rbtnUnlimted.Enabled = false;
        this.rbtnUnlimted.Checked = false;
        this.rbtnCustomDays.Enabled = false;
        this.rbtnCustomDays.Checked = false;
        this.txtExtensionDays.Enabled = this.rbtnCustomDays.Checked && this.rbtnCustomDays.Enabled;
        this.txtExtensionDays.Text = "";
        this.label9.Enabled = false;
        this.ckLockExtAllowTotalTimesCap.Enabled = false;
        this.ckLockExtAllowTotalTimesCap.Checked = false;
        this.txtLockExtAllowTotalTimes.Enabled = this.ckLockExtAllowTotalTimesCap.Checked;
        this.txtLockExtAllowTotalTimes.Text = "";
        this.label17.Enabled = false;
        this.gvAdjustPerLockExt.Enabled = this.btnAddAdjustPerLockExt.Enabled = true;
      }
      this.setDirtyFlag(true);
    }

    private void chkFixedExtension_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkFixedExtension.Checked)
        this.siBtnAdd.Enabled = this.gvExtPriceAdj.Enabled = true;
      else
        this.siBtnAdd.Enabled = this.gvExtPriceAdj.Enabled = this.siBtnDelete.Enabled = this.siBtnEdit.Enabled = false;
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void siBtnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvExtPriceAdj.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a price adjustment to edit.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.gvExtPriceAdj.SelectedItems.Count > 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select only one price adjustment for modification.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.gvExtPriceAdj.StartEditing(this.gvExtPriceAdj.SelectedItems[0].SubItems[0]);
    }

    private void gvExtPriceAdj_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.siBtnDelete.Enabled = this.siBtnEdit.Enabled = false;
      if (this.gvExtPriceAdj.SelectedItems.Count == 1)
      {
        this.siBtnDelete.Enabled = this.siBtnEdit.Enabled = true;
      }
      else
      {
        if (this.gvExtPriceAdj.SelectedItems.Count <= 1)
          return;
        this.siBtnDelete.Enabled = true;
      }
    }

    private void chkEnableLockCancellation_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkEnableLockCancellation.Checked)
      {
        this.chkLockCancellationOnLockReq.Enabled = true;
      }
      else
      {
        this.chkLockCancellationOnLockReq.Checked = false;
        this.chkLockCancellationOnLockReq.Enabled = false;
      }
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void chkLockCancellationOnLockReq_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void chkReLockOnly_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void ToggleLockExtensionCapFields()
    {
      this.rbtnUnlimted.Enabled = this.chkEnableLockExtension.Checked;
      this.rbtnLockPeriod.Enabled = this.chkEnableLockExtension.Checked;
      this.rbtnCustomDays.Enabled = this.chkEnableLockExtension.Checked;
      this.txtExtensionDays.Enabled = this.chkEnableLockExtension.Checked && this.rbtnCustomDays.Checked;
      this.ckLockExtensionAllowTotalCap.Enabled = this.chkEnableLockExtension.Checked;
      this.txtLockExtensionAllowTotalCapDays.Enabled = this.chkEnableLockExtension.Checked && this.ckLockExtensionAllowTotalCap.Checked;
      this.label9.Enabled = this.chkEnableLockExtension.Checked;
      this.label12.Enabled = this.chkEnableLockExtension.Checked;
      this.ckLockExtAllowTotalTimesCap.Enabled = this.chkEnableLockExtension.Checked;
      this.txtLockExtAllowTotalTimes.Enabled = this.chkEnableLockExtension.Checked && this.ckLockExtAllowTotalTimesCap.Checked;
      this.label17.Enabled = this.chkEnableLockExtension.Checked;
      this.chkFixedExtension.Enabled = this.chkAllowDailyAdj.Enabled = this.gvAdjustPerLockExt.Enabled = this.btnAddAdjustPerLockExt.Enabled = this.chkEnableLockExtension.Checked;
      if (this.chkAllowDailyAdj.Checked && this.chkAllowDailyAdj.Enabled)
        this.txtDailyPriceAdj.Enabled = this.chkApplyPriceAdjOnOffDays.Enabled = true;
      if (!this.chkFixedExtension.Checked || !this.chkFixedExtension.Enabled)
        return;
      this.siBtnAdd.Enabled = this.gvExtPriceAdj.Enabled = true;
    }

    private void txtExtensionDays_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.internalTrigger)
        return;
      if (this.txtExtensionDays.Text.Trim() == string.Empty)
      {
        this.setDirtyFlag(true);
      }
      else
      {
        this.formatText((object) this.txtExtensionDays, e);
        if (this.txtElapsedTime.Text.Trim() == "")
        {
          this.setDirtyFlag(true);
        }
        else
        {
          int num1 = Utils.ParseInt((object) this.txtExtensionDays.Text, 0);
          if (num1 < 1 || num1 > 999)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "Data out of range: 1 - 999", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.txtExtensionDays.Text = "";
          }
          this.setDirtyFlag(true);
        }
      }
    }

    private void ExtensionCapType_CheckedChanged(object sender, EventArgs e)
    {
      this.txtExtensionDays.Enabled = this.rbtnCustomDays.Checked && this.rbtnCustomDays.Enabled;
      if (this.rbtnCustomDays.Checked)
      {
        IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Policies");
        this.txtExtensionDays.Text = (int) serverSettings[(object) "Policies.LOCKEXTENSION_CAP_DAYS"] != 0 ? ((int) serverSettings[(object) "Policies.LOCKEXTENSION_CAP_DAYS"]).ToString() : "30";
      }
      else
        this.txtExtensionDays.Text = string.Empty;
      this.chk_CheckedChanged(sender, e);
    }

    private void chkRelock_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkRelock.Checked)
      {
        this.chkEnableRelockForTpoClient.Enabled = this.tpoSiteExists;
        this.chkAllowRelockOutsideLDHours.Enabled = this.chkGetCurrentPricing.Enabled = true;
        ProductPricingSetting currentActiveSetting1 = this.currentActiveSetting;
        if ((currentActiveSetting1 != null ? (currentActiveSetting1.IsEPPS ? 1 : 0) : 0) != 0)
        {
          ProductPricingSetting currentActiveSetting2 = this.currentActiveSetting;
          if ((currentActiveSetting2 != null ? (currentActiveSetting2.VendorPlatform == VendorPlatform.EPC2 ? 1 : 0) : 0) != 0)
            goto label_4;
        }
        this.chkReLockOnly.Enabled = true;
label_4:
        this.ckRelockAllowTotalCap.Enabled = true;
        this.labelRelockAllowTotalCap.Enabled = true;
        this.ckRelockFeeAllowed.Enabled = true;
        this.labelRelockFeeAllowed.Enabled = true;
        this.chkAllowNewLockOutsideLDHours.Enabled = Convert.ToString(this.cbProvider.SelectedItem) == "No Provider Selected";
        if (this.cbProvider.SelectedItem != null && this.cbProvider.SelectedItem is ComboboxItem && ((ComboboxItem) this.cbProvider.SelectedItem).Value.IsEPPS)
          this.chkGetPricingRelock.Enabled = true;
        else
          this.chkGetPricingRelock.Enabled = this.chkGetPricingRelock.Checked = false;
        this.LockUpdateReLockSettingsChange(false);
      }
      else
      {
        this.chkAllowNewLockOutsideLDHours.Enabled = false;
        this.chkAllowNewLockOutsideLDHours.Checked = false;
        this.chkEnableRelockForTpoClient.Enabled = false;
        this.chkEnableRelockForTpoClient.Checked = false;
        this.chkReLockOnly.Enabled = this.chkReLockOnly.Checked = this.chkGetPricingRelock.Enabled = this.chkGetPricingRelock.Checked = this.chkAllowRelockOutsideLDHours.Enabled = this.chkAllowRelockOutsideLDHours.Checked = false;
        this.chkGetCurrentPricing.Enabled = this.chkGetCurrentPricing.Checked = this.txtLockExpDays.Enabled = false;
        this.txtLockExpDays.Text = string.Empty;
        this.chkUseWorstCasePrice.Enabled = this.chkUseWorstCasePrice.Checked = false;
        this.chkRestrictLockPeriod.Enabled = this.chkRestrictLockPeriod.Checked = false;
        this.chkWaiveFeeAfter.Enabled = this.chkWaiveFeeAfter.Checked = false;
        this.ckRelockAllowTotalCap.Checked = false;
        this.ckRelockFeeAllowed.Checked = false;
        this.ckRelockAllowTotalCap.Enabled = false;
        this.txtRelockAllowTotalCapDays.Enabled = false;
        this.txtRelockAllowTotalCapDays.Text = string.Empty;
        this.ckRelockFeeAllowed.Enabled = false;
        this.txtRelockFee.Enabled = false;
        this.txtRelockFee.Text = string.Empty;
        this.chkAllowNewLockOutsideLDHours.Enabled = false;
        this.labelRelockAllowTotalCap.Enabled = false;
        this.labelRelockFeeAllowed.Enabled = false;
        this.LockUpdateReLockSettingsChange(false);
      }
      this.SetLockRelockSetting();
      if (sender != null)
        this.UpdateRelockOnlySetting();
      this.chk_CheckedChanged(sender, e);
    }

    private void chkAllowRelockOutsideLDHours_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void btnAddAdjustPerLockExt_Click(object sender, EventArgs e)
    {
      if (this.lockExtPriceAdjsOccur.Count == 10)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Failed to create lock extension: Lock extension limit has been reached.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        LockExtensionPriceAdjustment priceAdj = new LockExtensionPriceAdjustment("Extension #" + (object) (this.lockExtPriceAdjsOccur.Count + 1), 0, 0M);
        GVItem adjustPerOccurrence = this.createLockExtPriceAdjustPerOccurrence(priceAdj);
        this.gvAdjustPerLockExt.Items.Add(adjustPerOccurrence);
        this.gvAdjustPerLockExt.StartEditing(adjustPerOccurrence.SubItems[1]);
        this.lockExtPriceAdjsOccur.Add(priceAdj);
        this.lockExtPriceAdjsOccur.Sort();
        this.gvAdjustPerLockExt.SelectedIndexChanged -= new EventHandler(this.gvAdjustPerLockExt_SelectedIndexChanged);
        foreach (GVItem selectedItem in this.gvAdjustPerLockExt.SelectedItems)
          selectedItem.Selected = false;
        adjustPerOccurrence.Selected = true;
        this.gvAdjustPerLockExt.SelectedIndexChanged += new EventHandler(this.gvAdjustPerLockExt_SelectedIndexChanged);
      }
    }

    private void btnEditAdjustPerLockExt_Click(object sender, EventArgs e)
    {
      if (this.gvAdjustPerLockExt.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a price adjustment to edit.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.gvAdjustPerLockExt.SelectedItems.Count > 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select only one price adjustment for modification.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.gvAdjustPerLockExt.StartEditing(this.gvAdjustPerLockExt.SelectedItems[0].SubItems[1]);
    }

    private void btnDeleteAdjustPerLockExt_Click(object sender, EventArgs e)
    {
      if (this.gvAdjustPerLockExt.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a Price Adjustment to delete.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        foreach (GVItem selectedItem in this.gvAdjustPerLockExt.SelectedItems)
        {
          this.lockExtPriceAdjsOccur.Remove((LockExtensionPriceAdjustment) selectedItem.Tag);
          this.gvAdjustPerLockExt.Items.Remove(selectedItem);
        }
        int num2 = 1;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAdjustPerLockExt.Items)
        {
          ((LockExtensionPriceAdjustment) gvItem.Tag).ExtensionNumber = "Extension #" + (object) num2;
          gvItem.SubItems[0].Text = "Extension #" + (object) num2;
          ++num2;
        }
        this.setDirtyFlag(true);
      }
    }

    private void gvAdjustPerLockExt_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnDeleteAdjustPerLockExt.Enabled = this.btnEditAdjustPerLockExt.Enabled = false;
      if (this.gvAdjustPerLockExt.SelectedItems.Count == 1)
      {
        this.btnDeleteAdjustPerLockExt.Enabled = this.btnEditAdjustPerLockExt.Enabled = true;
      }
      else
      {
        if (this.gvAdjustPerLockExt.SelectedItems.Count <= 1)
          return;
        this.btnDeleteAdjustPerLockExt.Enabled = true;
      }
    }

    private void gvAdjustPerLockExt_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Index == 1)
      {
        if (!Utils.IsInt((object) e.EditorControl.Text))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please provide a valid data for Days To Extend", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.EditorControl.Text = e.SubItem.Text;
          e.Cancel = true;
          return;
        }
        if (Utils.ParseInt((object) e.EditorControl.Text) <= 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please provide a valid data for Days To Extend", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.EditorControl.Text = e.SubItem.Text;
          e.Cancel = true;
          return;
        }
        if (this.cmbLockExtensionOption.SelectedIndex != 2)
        {
          bool flag = false;
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAdjustPerLockExt.Items)
          {
            if (gvItem.Index != e.SubItem.Item.Index && gvItem.SubItems[1].Text == e.EditorControl.Text)
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "There already exists a price adjustment with the same Days to Extend.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            e.EditorControl.Text = e.SubItem.Text;
            e.Cancel = true;
            return;
          }
        }
        ((LockExtensionPriceAdjustment) e.SubItem.Item.Tag).DaysToExtend = Utils.ParseInt((object) e.EditorControl.Text);
      }
      else
      {
        if (!Utils.IsDecimal((object) e.EditorControl.Text))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please provide a valid data for Price Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.Cancel = true;
          return;
        }
        if (Utils.ParseDecimal((object) e.EditorControl.Text, 0M, 3) > 0M && Utils.Dialog((IWin32Window) this, "Are you sure you want to implement a positive Price Adjustment", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        {
          e.EditorControl.Text = e.SubItem.Text;
          e.Cancel = true;
          return;
        }
        e.EditorControl.Text = Utils.ParseDecimal((object) e.EditorControl.Text, 0M, 3).ToString("N3");
        ((LockExtensionPriceAdjustment) e.SubItem.Item.Tag).PriceAdjustment = Utils.ParseDecimal((object) e.EditorControl.Text, 0M, 3);
      }
      this.setAdjustPerLockBtns(true);
      this.setDirtyFlag(true);
    }

    private GVItem createLockExtPriceAdjustPerOccurrence(LockExtensionPriceAdjustment priceAdj)
    {
      return new GVItem()
      {
        SubItems = {
          (object) priceAdj.ExtensionNumber,
          (object) priceAdj.DaysToExtend,
          (object) priceAdj.PriceAdjustment
        },
        Tag = (object) priceAdj
      };
    }

    private void gvAdjustPerLockExt_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      this.setAdjustPerLockBtns(false);
    }

    private void setAdjustPerLockBtns(bool enabled)
    {
      this.btnAddAdjustPerLockExt.Enabled = enabled;
      this.btnDeleteAdjustPerLockExt.Enabled = enabled;
      this.btnEditAdjustPerLockExt.Enabled = enabled;
    }

    private void setExtPriceAdjustmentBtns(bool enabled)
    {
      this.siBtnAdd.Enabled = enabled;
      this.siBtnEdit.Enabled = enabled;
      this.siBtnDelete.Enabled = enabled;
    }

    private void txtLockExtensionAllowTotalCapDays_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.internalTrigger)
        return;
      this.formatText((object) this.txtLockExtensionAllowTotalCapDays, e);
      if (this.txtLockExtensionAllowTotalCapDays.Text.Trim() == "")
      {
        this.setDirtyFlag(true);
      }
      else
      {
        int num1 = Utils.ParseInt((object) this.txtLockExtensionAllowTotalCapDays.Text.Trim(), 0);
        if (num1 < 1 || num1 > 999)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Data out of range: 1 - 999", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.internalTrigger = true;
          this.txtLockExtensionAllowTotalCapDays.Text = "90";
          this.internalTrigger = false;
        }
        this.setDirtyFlag(true);
      }
    }

    private void ckLockExtensionAllowTotalCap_CheckedChanged(object sender, EventArgs e)
    {
      this.txtLockExtensionAllowTotalCapDays.Enabled = this.ckLockExtensionAllowTotalCap.Checked;
      this.txtLockExtensionAllowTotalCapDays.Text = this.ckLockExtensionAllowTotalCap.Checked ? "90" : "";
      this.setDirtyFlag(true);
    }

    private void ckLockExtAllowTotalTimesCap_CheckedChanged(object sender, EventArgs e)
    {
      this.txtLockExtAllowTotalTimes.Enabled = this.ckLockExtAllowTotalTimesCap.Checked;
      if (this.ckLockExtAllowTotalTimesCap.Checked && this.txtLockExtAllowTotalTimes.Text.Trim().Length == 0)
        this.txtLockExtAllowTotalTimes.Text = "3";
      if (!this.ckLockExtAllowTotalTimesCap.Checked)
        this.txtLockExtAllowTotalTimes.Text = "";
      this.setDirtyFlag(true);
    }

    private void txtLockExtAllowTotalTimes_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.internalTrigger)
        return;
      if (this.txtLockExtAllowTotalTimes.Text.Trim() == "")
      {
        this.setDirtyFlag(true);
      }
      else
      {
        int num1 = Utils.ParseInt((object) this.txtLockExtAllowTotalTimes.Text.Trim(), 0);
        if (this.ckLockExtAllowTotalTimesCap.Checked && this.cmbLockExtensionOption.SelectedIndex != 2 && (num1 < 1 || num1 > 10))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Data out of range: 1 - 10", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.internalTrigger = true;
          this.txtLockExtAllowTotalTimes.Text = "3";
          this.internalTrigger = false;
        }
        else
          this.setDirtyFlag(true);
      }
    }

    private void chkEnableRelockForTpoClient_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void txtRelockAllowTotalCapDays_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent || this.internalTrigger)
        return;
      this.formatText((object) this.txtRelockAllowTotalCapDays, e);
      if (this.txtRelockAllowTotalCapDays.Text.Trim() == "")
      {
        this.setDirtyFlag(true);
      }
      else
      {
        int num1 = Utils.ParseInt((object) this.txtRelockAllowTotalCapDays.Text.Trim(), 0);
        if (num1 < 1 || num1 > 10)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Data out of range: 1 - 10", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.internalTrigger = true;
          this.txtRelockAllowTotalCapDays.Text = "2";
          this.internalTrigger = false;
        }
        this.setDirtyFlag(true);
      }
    }

    private void ckRelockAllowTotalCap_CheckedChanged(object sender, EventArgs e)
    {
      if (this.ckRelockAllowTotalCap.Checked)
      {
        this.txtRelockAllowTotalCapDays.Enabled = true;
        this.txtRelockAllowTotalCapDays.Text = "2";
      }
      else
      {
        this.txtRelockAllowTotalCapDays.Enabled = false;
        this.txtRelockAllowTotalCapDays.Text = string.Empty;
      }
      this.setDirtyFlag(true);
    }

    private void ckRelockFeeAllowed_CheckedChanged(object sender, EventArgs e)
    {
      if (this.ckRelockFeeAllowed.Checked)
      {
        this.txtRelockFee.Enabled = true;
      }
      else
      {
        this.txtRelockFee.Enabled = false;
        this.txtRelockFee.Text = string.Empty;
      }
      this.updateDefaultSettingsForRelockFee();
      this.setDirtyFlag(true);
    }

    private void txtRelockFee_TextChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void txtRelockFee_Leave(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtRelockFee.Text))
        return;
      this.txtRelockFee.Text = Utils.ParseDecimal((object) this.txtRelockFee.Text, 0M, 3).ToString("N3");
    }

    private void txtRelockAllowTotalCapDays_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar) || e.KeyChar.Equals((object) Keys.Space))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        if (this.txtRelockAllowTotalCapDays.Text.Length > 2)
          e.Handled = true;
        this.setDirtyFlag(true);
      }
    }

    private void txtRelockFee_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar) || e.KeyChar.Equals((object) Keys.Space))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        char keyChar;
        if (!char.IsNumber(e.KeyChar))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('.'))
          {
            keyChar = e.KeyChar;
            if (!keyChar.Equals('-'))
              e.Handled = true;
          }
        }
        TextBox textBox = sender as TextBox;
        int selectionStart = textBox.SelectionStart;
        int startIndex = textBox.SelectionStart + textBox.SelectionLength;
        keyChar = e.KeyChar;
        if (keyChar.Equals('-') && selectionStart != 0)
          e.Handled = true;
        if (!char.IsNumber(e.KeyChar))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('.'))
            goto label_12;
        }
        string str1 = textBox.Text.Substring(0, selectionStart);
        keyChar = e.KeyChar;
        string str2 = keyChar.ToString();
        string str3 = textBox.Text.Substring(startIndex);
        string[] strArray = (str1 + str2 + str3).Split('.');
        if (strArray.Length > 1 && (strArray[1].Length > 3 || strArray.Length > 2))
          e.Handled = true;
label_12:
        this.setDirtyFlag(true);
      }
    }

    private void gvExtPriceAdj_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      this.setExtPriceAdjustmentBtns(false);
    }

    private void chkAllowNewLockOutsideLDHours_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void progressSpinner_Click(object sender, EventArgs e)
    {
      int num = (int) new DiagnosticDetailForm(this.errorDetails, this.stackTraceDetails).ShowDialog();
    }

    private void llExportUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.ExportUser();
    }

    private void ExportUser()
    {
      try
      {
        if (MessageBox.Show((IWin32Window) this, "Export Services Password Manager credentials to Services Management", "Export Users", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
          return;
        new Bam().GetAccessToken("sc");
        Epc2Provider provider = this.epcProviderList.Where<Epc2Provider>((Func<Epc2Provider, bool>) (p => p.Id == this.currentActiveSetting.ProviderID)).FirstOrDefault<Epc2Provider>();
        bool isNewServicerSetupActive = true;
        this.exportCredential.ServiceSetups = (List<ServiceSetupResult>) null;
        if (this.exportCredential.HasDuplicateServiceSetup)
        {
          MessageBox.Show((IWin32Window) this, string.Format("The service setup name '{0}' for selected provider already exists. The service setup must be deleted from Services Management before export is allowed.", (object) this.exportCredential.ServiceSetupName), "Duplicate Service Setup Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          ServiceSetupResult activeServiceSetup = this.exportCredential.ActiveServiceSetupForProvider;
          if (activeServiceSetup != null)
            isNewServicerSetupActive = MessageBox.Show((IWin32Window) this, string.Format("A service setup for selected provider already exists. Do you want to make the new service setup '{0}' Active?", (object) this.exportCredential.ServiceSetupName), "Active Service Setup Found", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
          ProgressDialog progressBar = new ProgressDialog(this.exportCredential.TotlaCount);
          bool isProcessRunning = true;
          Thread thread1 = new Thread((ThreadStart) (() =>
          {
            while (isProcessRunning)
            {
              Thread.Sleep(500);
              progressBar.UpdateProgress(this.exportCredential.ExportIndex);
            }
            Thread.Sleep(500);
            if (progressBar.InvokeRequired)
              progressBar.BeginInvoke((Delegate) (() => progressBar.Close()));
            isProcessRunning = false;
          }));
          Thread thread2 = new Thread((ThreadStart) (() =>
          {
            try
            {
              ServiceSetupResult serviceSetup = Task.Run<ServiceSetupResult>((Func<Task<ServiceSetupResult>>) (async () => await this.exportCredential.ExportUsers(provider, activeServiceSetup, isNewServicerSetupActive))).Result;
              if (this.InvokeRequired)
              {
                this.Invoke((Delegate) (() =>
                {
                  int num = (int) MessageBox.Show((IWin32Window) this, string.Format("{0} users have been exported successfully for '{1}' Service Setup.", (object) serviceSetup.AuthorizedUsers.Count, (object) serviceSetup.Name), "Users Exported Successfully", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                  this.currentActiveSetting.IsExportUserFinished = true;
                  this.pnlExportUser.Visible = false;
                  isProcessRunning = false;
                }));
              }
              else
              {
                int num = (int) MessageBox.Show((IWin32Window) this, string.Format("{0} users have been exported successfully for '{1}' Service Setup.", (object) serviceSetup.AuthorizedUsers.Count, (object) serviceSetup.Name), "Users Exported Successfully", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.currentActiveSetting.IsExportUserFinished = true;
                this.pnlExportUser.Visible = false;
                isProcessRunning = false;
              }
            }
            catch (Exception ex)
            {
              if (this.InvokeRequired)
              {
                this.Invoke((Delegate) (() =>
                {
                  isProcessRunning = false;
                  this.currentActiveSetting.IsExportUserFinished = false;
                  this.pnlExportUser.Visible = true;
                  int num = (int) Utils.Dialog((IWin32Window) this, "Export users operation failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  Tracing.Log(ProductPricingAdmin.sw, TraceLevel.Error, nameof (ProductPricingAdmin), string.Format("Export User Exception (Message {0})", (object) ex.Message));
                }));
              }
              else
              {
                isProcessRunning = false;
                this.currentActiveSetting.IsExportUserFinished = false;
                this.pnlExportUser.Visible = true;
                int num = (int) Utils.Dialog((IWin32Window) this, "Export users operation failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Tracing.Log(ProductPricingAdmin.sw, TraceLevel.Error, nameof (ProductPricingAdmin), string.Format("Export User Exception (Message {0})", (object) ex.Message));
              }
            }
          }));
          thread1.Start();
          thread2.Start();
          int num1 = (int) progressBar.ShowDialog();
        }
      }
      catch (Exception ex)
      {
        this.currentActiveSetting.IsExportUserFinished = false;
        this.pnlExportUser.Visible = true;
        int num = (int) Utils.Dialog((IWin32Window) this, "Export users operation failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(ProductPricingAdmin.sw, TraceLevel.Error, nameof (ProductPricingAdmin), string.Format("Export User Exception (Message {0})", (object) ex.Message));
      }
    }

    private void chkUseWorstCasePrice_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkUseWorstCasePrice.Checked)
      {
        this.txtWorstCasePrice.Enabled = true;
        this.txtWorstCasePrice.Text = "60";
        this.chkReLockCustomPriceAdjustments.Checked = true;
        this.chkReLockExtensionFees.Checked = true;
        this.chkReLockPriceConcessions.Checked = true;
        this.chkReLockReLockfees.Checked = true;
        this.chkReLockCustomPriceAdjustments.Enabled = this.chkReLockExtensionFees.Enabled = this.chkReLockPriceConcessions.Enabled = this.chkReLockReLockfees.Enabled = true;
      }
      else if (!this.chkUseWorstCasePrice.Checked)
      {
        this.txtWorstCasePrice.Enabled = false;
        this.txtWorstCasePrice.Text = string.Empty;
        this.chkReLockCustomPriceAdjustments.Checked = false;
        this.chkReLockExtensionFees.Checked = false;
        this.chkReLockPriceConcessions.Checked = false;
        this.chkReLockReLockfees.Checked = false;
        this.chkReLockCustomPriceAdjustments.Enabled = this.chkReLockExtensionFees.Enabled = this.chkReLockPriceConcessions.Enabled = this.chkReLockReLockfees.Enabled = false;
      }
      this.setDirtyFlag(true);
    }

    private void chkWaiveFeeAfter_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkWaiveFeeAfter.Checked)
      {
        this.txtWaiveFeeAfter.Enabled = true;
      }
      else
      {
        this.txtWaiveFeeAfter.Enabled = false;
        this.txtWaiveFeeAfter.Text = string.Empty;
      }
      this.setDirtyFlag(true);
    }

    private void txtWorstCasePrice_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar) || e.KeyChar.Equals((object) Keys.Space))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        this.setDirtyFlag(true);
      }
    }

    private void txtWaiveFeeAfter_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar) || e.KeyChar.Equals((object) Keys.Space))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        this.setDirtyFlag(true);
      }
    }

    private void chkLockUpdateExtensionFees_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void chkLockUpdateCustomPriceAdjustments_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void chkLockUpdatePriceConcessions_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void chkLockUpdateReLockfees_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void chkReLockExtensionFees_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void chkReLockCustomPriceAdjustments_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void chkReLockPriceConcessions_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void chkReLockReLockfees_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void chkRestrictLockPeriod_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void LockUpdateReLockSettingsChange(bool flagEnableDisable)
    {
      if (this.cbProvider.Items.Count == 0)
        return;
      string str = string.Empty;
      if (this.cbProvider.SelectedItem.ToString().ToLower() == "no provider selected")
        str = "null";
      else if (this.currentActiveSetting.VendorPlatform == VendorPlatform.EPC2)
        str = "EPC2";
      this.AdjustRelockGroupContainerSize(true);
      this.ApplyShipDarkToRelockSettings();
      if (this.chkRelock.Checked)
      {
        switch (str)
        {
          case "EPC2":
            this.chkUseWorstCasePrice.Enabled = true;
            this.chkUseWorstCasePrice.Checked = this.session.StartupInfo.PolicySettings[(object) "Policies.WorstCasePrice"] == null || string.IsNullOrEmpty(Convert.ToString(this.session.StartupInfo.PolicySettings[(object) "Policies.WorstCasePrice"])) || (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.WorstCasePrice"];
            this.chkWaiveFeeAfter.Enabled = true;
            this.chkRestrictLockPeriod.Enabled = true;
            this.chkWaiveFeeAfter.Checked = this.session.StartupInfo.PolicySettings[(object) "Policies.WaiveFee"] != null && (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.WaiveFee"];
            if (this.chkWaiveFeeAfter.Checked)
              this.txtWaiveFeeAfter.Text = this.session.StartupInfo.PolicySettings[(object) "Policies.WaiveFeeAfterDays"] != null ? this.session.StartupInfo.PolicySettings[(object) "Policies.WaiveFeeAfterDays"].ToString() : "";
            if (!this.ckRelockFeeAllowed.Checked)
              this.chkWaiveFeeAfter.Enabled = false;
            this.chkLockUpdateExtensionFees.Enabled = this.chkLockUpdateCustomPriceAdjustments.Enabled = this.chkLockUpdatePriceConcessions.Enabled = this.chkLockUpdateReLockfees.Enabled = true;
            this.label22.Enabled = true;
            this.label23.Enabled = true;
            this.label24.Enabled = true;
            this.label25.Enabled = true;
            this.SetWorstCasePriceSettings();
            break;
          case "null":
            this.chkUseWorstCasePrice.Enabled = false;
            this.chkUseWorstCasePrice.Checked = false;
            this.label22.Enabled = false;
            this.label24.Enabled = false;
            this.label23.Enabled = true;
            this.label25.Enabled = true;
            this.chkWaiveFeeAfter.Enabled = true;
            if (!this.ckRelockFeeAllowed.Checked)
              this.chkWaiveFeeAfter.Enabled = false;
            this.chkRestrictLockPeriod.Enabled = true;
            this.chkLockUpdateExtensionFees.Enabled = this.chkLockUpdateCustomPriceAdjustments.Enabled = this.chkLockUpdatePriceConcessions.Enabled = this.chkLockUpdateReLockfees.Enabled = true;
            this.chkReLockExtensionFees.Checked = this.chkReLockCustomPriceAdjustments.Checked = this.chkReLockPriceConcessions.Checked = this.chkReLockReLockfees.Checked = false;
            this.chkReLockExtensionFees.Enabled = this.chkReLockCustomPriceAdjustments.Enabled = this.chkReLockPriceConcessions.Enabled = this.chkReLockReLockfees.Enabled = false;
            if (flagEnableDisable)
              break;
            this.setDirtyFlag(false);
            this.suspendEvent = false;
            break;
          default:
            this.DisableLockUpdateRelockSettings();
            if (flagEnableDisable)
              break;
            this.setDirtyFlag(false);
            this.suspendEvent = false;
            break;
        }
      }
      else
      {
        this.DisableLockUpdateRelockSettings();
        if (flagEnableDisable)
          return;
        this.setDirtyFlag(false);
        this.suspendEvent = false;
      }
    }

    private void SetWorstCasePriceSettings()
    {
      if (!this.chkUseWorstCasePrice.Checked)
        return;
      IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Policies");
      this.txtWorstCasePrice.Text = serverSettings[(object) "Policies.WorstCasePriceEqualDays"] != null ? serverSettings[(object) "Policies.WorstCasePriceEqualDays"].ToString() : "60";
      this.chkReLockExtensionFees.Checked = serverSettings[(object) "Policies.ReLockExtensionFees"] != null && (bool) serverSettings[(object) "Policies.ReLockExtensionFees"];
      this.chkReLockCustomPriceAdjustments.Checked = serverSettings[(object) "Policies.ReLockCustomPriceAdjustments"] != null && (bool) serverSettings[(object) "Policies.ReLockCustomPriceAdjustments"];
      this.chkReLockPriceConcessions.Checked = serverSettings[(object) "Policies.ReLockPriceConcessions"] != null && (bool) serverSettings[(object) "Policies.ReLockPriceConcessions"];
      this.chkReLockReLockfees.Checked = serverSettings[(object) "Policies.ReLockReLockfees"] != null && (bool) serverSettings[(object) "Policies.ReLockReLockfees"];
    }

    private void DisableLockUpdateRelockSettings()
    {
      this.chkUseWorstCasePrice.Enabled = this.chkUseWorstCasePrice.Checked = this.txtWorstCasePrice.Enabled = false;
      this.txtWorstCasePrice.Text = string.Empty;
      this.chkWaiveFeeAfter.Enabled = this.chkWaiveFeeAfter.Checked = this.txtWaiveFeeAfter.Enabled = false;
      this.txtWaiveFeeAfter.Text = string.Empty;
      this.chkRestrictLockPeriod.Enabled = this.chkRestrictLockPeriod.Checked = false;
      this.label22.Enabled = false;
      this.label23.Enabled = false;
      this.label24.Enabled = false;
      this.label25.Enabled = false;
      this.chkLockUpdateExtensionFees.Enabled = this.chkLockUpdateCustomPriceAdjustments.Enabled = this.chkLockUpdatePriceConcessions.Enabled = this.chkLockUpdateReLockfees.Enabled = false;
      this.chkLockUpdateExtensionFees.Checked = this.chkLockUpdateCustomPriceAdjustments.Checked = this.chkLockUpdatePriceConcessions.Checked = this.chkLockUpdateReLockfees.Checked = false;
      this.chkReLockExtensionFees.Checked = this.chkReLockCustomPriceAdjustments.Checked = this.chkReLockPriceConcessions.Checked = this.chkReLockReLockfees.Checked = false;
      this.chkReLockExtensionFees.Enabled = this.chkReLockCustomPriceAdjustments.Enabled = this.chkReLockPriceConcessions.Enabled = this.chkReLockReLockfees.Enabled = false;
    }

    private void ApplyShipDarkToRelockSettings()
    {
      this.panel1.Dock = DockStyle.Top;
      this.panel2.Dock = DockStyle.Top;
      this.panel3.Dock = DockStyle.Top;
      this.panel4.Dock = DockStyle.Top;
      this.panel5.Dock = DockStyle.Top;
      this.panel1.Visible = this.panel2.Visible = this.panel3.Visible = false;
      this.chkUseWorstCasePrice.Checked = this.chkReLockExtensionFees.Checked = false;
      this.chkReLockCustomPriceAdjustments.Checked = this.chkReLockPriceConcessions.Checked = false;
      this.chkReLockReLockfees.Checked = this.chkWaiveFeeAfter.Checked = false;
      this.chkRestrictLockPeriod.Checked = false;
      this.txtWaiveFeeAfter.Text = this.txtWorstCasePrice.Text = string.Empty;
    }

    private bool GetShipDarkFlag()
    {
      string a = Convert.ToString(this.session.StartupInfo.PolicySettings[(object) "Policies.EPPS_EPC2_SHIP_DARK_SR"]);
      return string.IsNullOrEmpty(a) || string.Equals(a, "true", StringComparison.CurrentCultureIgnoreCase);
    }

    private void AdjustRelockGroupContainerSize(bool flag)
    {
      if (flag)
        this.gcLockRequestType.Height = 305;
      else
        this.gcLockRequestType.Height = 285;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProductPricingAdmin));
      this.toolTip1 = new ToolTip(this.components);
      this.siBtnEdit = new StandardIconButton();
      this.siBtnDelete = new StandardIconButton();
      this.siBtnAdd = new StandardIconButton();
      this.btnEditAdjustPerLockExt = new StandardIconButton();
      this.btnDeleteAdjustPerLockExt = new StandardIconButton();
      this.btnAddAdjustPerLockExt = new StandardIconButton();
      this.borderPanel2 = new BorderPanel();
      this.borderPanel1 = new BorderPanel();
      this.pnlGlobalSettings = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.gcLockCancellation = new GroupContainer();
      this.chkLockCancellationOnLockReq = new CheckBox();
      this.chkEnableLockCancellation = new CheckBox();
      this.pnlLockextensionConfig = new Panel();
      this.chkApplyPriceAdjOnOffDays = new CheckBox();
      this.label7 = new Label();
      this.groupContainer2 = new GroupContainer();
      this.chkFixedExtension = new CheckBox();
      this.gvExtPriceAdj = new GridView();
      this.label5 = new Label();
      this.chkAllowDailyAdj = new CheckBox();
      this.txtDailyPriceAdj = new TextBox();
      this.label4 = new Label();
      this.pnlAdjustPerLockExt = new Panel();
      this.label10 = new Label();
      this.groupContainer3 = new GroupContainer();
      this.gvAdjustPerLockExt = new GridView();
      this.label11 = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.ckLockExtAllowTotalTimesCap = new CheckBox();
      this.txtLockExtAllowTotalTimes = new TextBox();
      this.label17 = new Label();
      this.ckLockExtensionAllowTotalCap = new CheckBox();
      this.txtLockExtensionAllowTotalCapDays = new TextBox();
      this.label12 = new Label();
      this.txtExtensionDays = new TextBox();
      this.label9 = new Label();
      this.rbtnCustomDays = new RadioButton();
      this.rbtnLockPeriod = new RadioButton();
      this.rbtnUnlimted = new RadioButton();
      this.label8 = new Label();
      this.chkEnableLockExtension = new CheckBox();
      this.cmbLockExtensionOption = new ComboBox();
      this.label6 = new Label();
      this.gcLockRequestType = new GroupContainer();
      this.pnlForReLocks = new Panel();
      this.panel5 = new Panel();
      this.ckRelockAllowTotalCap = new CheckBox();
      this.txtRelockAllowTotalCapDays = new TextBox();
      this.labelRelockAllowTotalCap = new Label();
      this.panel4 = new Panel();
      this.ckRelockFeeAllowed = new CheckBox();
      this.labelRelockFeeAllowed = new Label();
      this.txtRelockFee = new TextBox();
      this.panel1 = new Panel();
      this.chkReLockReLockfees = new CheckBox();
      this.label22 = new Label();
      this.chkReLockExtensionFees = new CheckBox();
      this.chkReLockCustomPriceAdjustments = new CheckBox();
      this.chkReLockPriceConcessions = new CheckBox();
      this.chkUseWorstCasePrice = new CheckBox();
      this.label24 = new Label();
      this.txtWorstCasePrice = new TextBox();
      this.panel3 = new Panel();
      this.chkRestrictLockPeriod = new CheckBox();
      this.panel2 = new Panel();
      this.chkWaiveFeeAfter = new CheckBox();
      this.label25 = new Label();
      this.txtWaiveFeeAfter = new TextBox();
      this.pnlLockUpdates = new Panel();
      this.label23 = new Label();
      this.chkLockUpdateExtensionFees = new CheckBox();
      this.chkLockUpdateCustomPriceAdjustments = new CheckBox();
      this.chkLockUpdatePriceConcessions = new CheckBox();
      this.chkLockUpdateReLockfees = new CheckBox();
      this.chkEnableRelockForTpoClient = new CheckBox();
      this.labelGetCurrentPricingDays = new Label();
      this.txtLockExpDays = new TextBox();
      this.chkGetCurrentPricing = new CheckBox();
      this.label13 = new Label();
      this.label16 = new Label();
      this.chkAllowRelockOutsideLDHours = new CheckBox();
      this.chkAllowNewLockOutsideLDHours = new CheckBox();
      this.chkGetPricingRelock = new CheckBox();
      this.chkRelock = new CheckBox();
      this.chkReLockOnly = new CheckBox();
      this.gcElapsedTime = new GroupContainer();
      this.chkDontAllowPriceChange = new CheckBox();
      this.lblElapsedTimeSettings = new Label();
      this.chkTimerEnforceTiming = new CheckBox();
      this.txtElapsedTime = new TextBox();
      this.label1 = new Label();
      this.chkElapseTime = new CheckBox();
      this.gcPPConfig = new GroupContainer();
      this.pnlProviderSettings = new Panel();
      this.grCustomizeName = new GroupContainer();
      this.rdoCustomizeNameDepends = new RadioButton();
      this.rdoCustomizeNameAlwaysLender = new RadioButton();
      this.chkCustomizeName = new CheckBox();
      this.rdoCustomizeNameAlwaysInvestor = new RadioButton();
      this.pnlSettings = new Panel();
      this.chkLockUpdateandLockConfirm = new CheckBox();
      this.chkAutoValidate = new CheckBox();
      this.chkRequestLockWOLock = new CheckBox();
      this.checkBox1 = new CheckBox();
      this.chkLockConfirm = new CheckBox();
      this.checkBox2 = new CheckBox();
      this.chkRequestLock = new CheckBox();
      this.chkImportToLoanFile = new CheckBox();
      this.pnlProviders = new Panel();
      this.pnlExportUser = new Panel();
      this.label21 = new Label();
      this.llExportUser = new System.Windows.Forms.LinkLabel();
      this.progressSpinner = new PictureBox();
      this.label3 = new Label();
      this.cbProvider = new ComboBox();
      this.llAdmin = new System.Windows.Forms.LinkLabel();
      this.label2 = new Label();
      this.llMoreInfo = new System.Windows.Forms.LinkLabel();
      this.lblSvcMgmtBar = new Label();
      this.llSvcMgmt = new System.Windows.Forms.LinkLabel();
      this.groupContainerLockVoid = new GroupContainer();
      this.chkEnableLockVoidWholesale = new CheckBox();
      this.chkEnableLockVoidRetail = new CheckBox();
      this.chkEnableLockVoid = new CheckBox();
      this.groupContainerZeroBasedParPricing = new GroupContainer();
      this.label14 = new Label();
      this.chkZeroBasedParPricingRetail = new CheckBox();
      this.chkZeroBasedParPricingWholesale = new CheckBox();
      this.chkAllowPPESelection = new CheckBox();
      ((ISupportInitialize) this.siBtnEdit).BeginInit();
      ((ISupportInitialize) this.siBtnDelete).BeginInit();
      ((ISupportInitialize) this.siBtnAdd).BeginInit();
      ((ISupportInitialize) this.btnEditAdjustPerLockExt).BeginInit();
      ((ISupportInitialize) this.btnDeleteAdjustPerLockExt).BeginInit();
      ((ISupportInitialize) this.btnAddAdjustPerLockExt).BeginInit();
      this.borderPanel2.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.pnlGlobalSettings.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.gcLockCancellation.SuspendLayout();
      this.pnlLockextensionConfig.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.pnlAdjustPerLockExt.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.gcLockRequestType.SuspendLayout();
      this.pnlForReLocks.SuspendLayout();
      this.panel5.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel2.SuspendLayout();
      this.pnlLockUpdates.SuspendLayout();
      this.gcElapsedTime.SuspendLayout();
      this.gcPPConfig.SuspendLayout();
      this.pnlProviderSettings.SuspendLayout();
      this.grCustomizeName.SuspendLayout();
      this.pnlSettings.SuspendLayout();
      this.pnlProviders.SuspendLayout();
      this.pnlExportUser.SuspendLayout();
      ((ISupportInitialize) this.progressSpinner).BeginInit();
      this.groupContainerLockVoid.SuspendLayout();
      this.groupContainerZeroBasedParPricing.SuspendLayout();
      this.SuspendLayout();
      this.siBtnEdit.BackColor = Color.Transparent;
      this.siBtnEdit.Enabled = false;
      this.siBtnEdit.Location = new Point(409, 5);
      this.siBtnEdit.Margin = new Padding(4, 4, 4, 4);
      this.siBtnEdit.MouseDownImage = (Image) null;
      this.siBtnEdit.Name = "siBtnEdit";
      this.siBtnEdit.Size = new Size(21, 20);
      this.siBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.siBtnEdit.TabIndex = 8;
      this.siBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnEdit, "Edit Price Adjustment");
      this.siBtnEdit.Click += new EventHandler(this.siBtnEdit_Click);
      this.siBtnDelete.BackColor = Color.Transparent;
      this.siBtnDelete.Enabled = false;
      this.siBtnDelete.Location = new Point(439, 5);
      this.siBtnDelete.Margin = new Padding(4, 4, 4, 4);
      this.siBtnDelete.MouseDownImage = (Image) null;
      this.siBtnDelete.Name = "siBtnDelete";
      this.siBtnDelete.Size = new Size(21, 20);
      this.siBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDelete.TabIndex = 3;
      this.siBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnDelete, "Delete Price Adjustment");
      this.siBtnDelete.Click += new EventHandler(this.siBtnDelete_Click);
      this.siBtnAdd.BackColor = Color.Transparent;
      this.siBtnAdd.Enabled = false;
      this.siBtnAdd.Location = new Point(380, 5);
      this.siBtnAdd.Margin = new Padding(4, 4, 4, 4);
      this.siBtnAdd.MouseDownImage = (Image) null;
      this.siBtnAdd.Name = "siBtnAdd";
      this.siBtnAdd.Size = new Size(21, 20);
      this.siBtnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnAdd.TabIndex = 1;
      this.siBtnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnAdd, "New Price Adjustment");
      this.siBtnAdd.Click += new EventHandler(this.siBtnAdd_Click);
      this.btnEditAdjustPerLockExt.BackColor = Color.Transparent;
      this.btnEditAdjustPerLockExt.Enabled = false;
      this.btnEditAdjustPerLockExt.Location = new Point(445, 5);
      this.btnEditAdjustPerLockExt.Margin = new Padding(4, 4, 4, 4);
      this.btnEditAdjustPerLockExt.MouseDownImage = (Image) null;
      this.btnEditAdjustPerLockExt.Name = "btnEditAdjustPerLockExt";
      this.btnEditAdjustPerLockExt.Size = new Size(21, 20);
      this.btnEditAdjustPerLockExt.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditAdjustPerLockExt.TabIndex = 8;
      this.btnEditAdjustPerLockExt.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditAdjustPerLockExt, "Edit Price Adjustment");
      this.btnEditAdjustPerLockExt.Click += new EventHandler(this.btnEditAdjustPerLockExt_Click);
      this.btnDeleteAdjustPerLockExt.BackColor = Color.Transparent;
      this.btnDeleteAdjustPerLockExt.Enabled = false;
      this.btnDeleteAdjustPerLockExt.Location = new Point(475, 5);
      this.btnDeleteAdjustPerLockExt.Margin = new Padding(4, 4, 4, 4);
      this.btnDeleteAdjustPerLockExt.MouseDownImage = (Image) null;
      this.btnDeleteAdjustPerLockExt.Name = "btnDeleteAdjustPerLockExt";
      this.btnDeleteAdjustPerLockExt.Size = new Size(21, 20);
      this.btnDeleteAdjustPerLockExt.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteAdjustPerLockExt.TabIndex = 3;
      this.btnDeleteAdjustPerLockExt.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteAdjustPerLockExt, "Delete Price Adjustment");
      this.btnDeleteAdjustPerLockExt.Click += new EventHandler(this.btnDeleteAdjustPerLockExt_Click);
      this.btnAddAdjustPerLockExt.BackColor = Color.Transparent;
      this.btnAddAdjustPerLockExt.Location = new Point(416, 5);
      this.btnAddAdjustPerLockExt.Margin = new Padding(4, 4, 4, 4);
      this.btnAddAdjustPerLockExt.MouseDownImage = (Image) null;
      this.btnAddAdjustPerLockExt.Name = "btnAddAdjustPerLockExt";
      this.btnAddAdjustPerLockExt.Size = new Size(21, 20);
      this.btnAddAdjustPerLockExt.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddAdjustPerLockExt.TabIndex = 1;
      this.btnAddAdjustPerLockExt.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddAdjustPerLockExt, "New Price Adjustment");
      this.btnAddAdjustPerLockExt.Click += new EventHandler(this.btnAddAdjustPerLockExt_Click);
      this.borderPanel2.AutoSize = true;
      this.borderPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.borderPanel2.Controls.Add((Control) this.borderPanel1);
      this.borderPanel2.Dock = DockStyle.Top;
      this.borderPanel2.Location = new Point(0, 0);
      this.borderPanel2.Margin = new Padding(4, 4, 4, 4);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(279, 2007);
      this.borderPanel2.TabIndex = 4;
      this.borderPanel1.AutoScroll = true;
      this.borderPanel1.AutoSize = true;
      this.borderPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.borderPanel1.BackColor = Color.WhiteSmoke;
      this.borderPanel1.Borders = AnchorStyles.None;
      this.borderPanel1.Controls.Add((Control) this.pnlGlobalSettings);
      this.borderPanel1.Controls.Add((Control) this.gcPPConfig);
      this.borderPanel1.Dock = DockStyle.Top;
      this.borderPanel1.Location = new Point(1, 1);
      this.borderPanel1.Margin = new Padding(4, 4, 4, 4);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(277, 2007);
      this.borderPanel1.TabIndex = 2;
      this.pnlGlobalSettings.AutoSize = true;
      this.pnlGlobalSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlGlobalSettings.Controls.Add((Control) this.groupContainer1);
      this.pnlGlobalSettings.Controls.Add((Control) this.gcLockRequestType);
      this.pnlGlobalSettings.Controls.Add((Control) this.gcElapsedTime);
      this.pnlGlobalSettings.Dock = DockStyle.Top;
      this.pnlGlobalSettings.Location = new Point(0, 485);
      this.pnlGlobalSettings.Margin = new Padding(4, 4, 4, 4);
      this.pnlGlobalSettings.Name = "pnlGlobalSettings";
      this.pnlGlobalSettings.Size = new Size(277, 1522);
      this.pnlGlobalSettings.TabIndex = 3;
      this.groupContainer1.Borders = AnchorStyles.Top;
      this.groupContainer1.Controls.Add((Control) this.gcLockCancellation);
      this.groupContainer1.Controls.Add((Control) this.pnlLockextensionConfig);
      this.groupContainer1.Controls.Add((Control) this.pnlAdjustPerLockExt);
      this.groupContainer1.Controls.Add((Control) this.gradientPanel1);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 752);
      this.groupContainer1.Margin = new Padding(4, 4, 4, 4);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(277, 770);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Text = "Lock Extension Price Adjustment";
      this.gcLockCancellation.Borders = AnchorStyles.Top;
      this.gcLockCancellation.Controls.Add((Control) this.chkLockCancellationOnLockReq);
      this.gcLockCancellation.Controls.Add((Control) this.chkEnableLockCancellation);
      this.gcLockCancellation.Dock = DockStyle.Top;
      this.gcLockCancellation.HeaderForeColor = SystemColors.ControlText;
      this.gcLockCancellation.Location = new Point(0, 628);
      this.gcLockCancellation.Margin = new Padding(4, 4, 4, 4);
      this.gcLockCancellation.Name = "gcLockCancellation";
      this.gcLockCancellation.Size = new Size(277, 128);
      this.gcLockCancellation.TabIndex = 9;
      this.gcLockCancellation.Text = "Lock Cancellation";
      this.chkLockCancellationOnLockReq.AutoSize = true;
      this.chkLockCancellationOnLockReq.Location = new Point(37, 71);
      this.chkLockCancellationOnLockReq.Margin = new Padding(4, 4, 4, 4);
      this.chkLockCancellationOnLockReq.Name = "chkLockCancellationOnLockReq";
      this.chkLockCancellationOnLockReq.Size = new Size(368, 20);
      this.chkLockCancellationOnLockReq.TabIndex = 1;
      this.chkLockCancellationOnLockReq.Text = "Enable lock cancellation requests on Lock Request Form";
      this.chkLockCancellationOnLockReq.UseVisualStyleBackColor = true;
      this.chkLockCancellationOnLockReq.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkEnableLockCancellation.AutoSize = true;
      this.chkEnableLockCancellation.Location = new Point(12, 43);
      this.chkEnableLockCancellation.Margin = new Padding(4, 4, 4, 4);
      this.chkEnableLockCancellation.Name = "chkEnableLockCancellation";
      this.chkEnableLockCancellation.Size = new Size(370, 20);
      this.chkEnableLockCancellation.TabIndex = 0;
      this.chkEnableLockCancellation.Text = "Enable lock cancellations in Secondary Registration Tool";
      this.chkEnableLockCancellation.UseVisualStyleBackColor = true;
      this.chkEnableLockCancellation.CheckedChanged += new EventHandler(this.chkEnableLockCancellation_CheckedChanged);
      this.pnlLockextensionConfig.Controls.Add((Control) this.chkApplyPriceAdjOnOffDays);
      this.pnlLockextensionConfig.Controls.Add((Control) this.label7);
      this.pnlLockextensionConfig.Controls.Add((Control) this.groupContainer2);
      this.pnlLockextensionConfig.Controls.Add((Control) this.label5);
      this.pnlLockextensionConfig.Controls.Add((Control) this.chkAllowDailyAdj);
      this.pnlLockextensionConfig.Controls.Add((Control) this.txtDailyPriceAdj);
      this.pnlLockextensionConfig.Controls.Add((Control) this.label4);
      this.pnlLockextensionConfig.Dock = DockStyle.Top;
      this.pnlLockextensionConfig.Location = new Point(0, 454);
      this.pnlLockextensionConfig.Margin = new Padding(4, 4, 4, 4);
      this.pnlLockextensionConfig.Name = "pnlLockextensionConfig";
      this.pnlLockextensionConfig.Size = new Size(277, 174);
      this.pnlLockextensionConfig.TabIndex = 8;
      this.chkApplyPriceAdjOnOffDays.AutoSize = true;
      this.chkApplyPriceAdjOnOffDays.Location = new Point(526, 69);
      this.chkApplyPriceAdjOnOffDays.Margin = new Padding(4, 4, 4, 4);
      this.chkApplyPriceAdjOnOffDays.Name = "chkApplyPriceAdjOnOffDays";
      this.chkApplyPriceAdjOnOffDays.Size = new Size(412, 20);
      this.chkApplyPriceAdjOnOffDays.TabIndex = 9;
      this.chkApplyPriceAdjOnOffDays.Text = "Apply lock extension price adjustment to weekends and holidays";
      this.chkApplyPriceAdjOnOffDays.UseVisualStyleBackColor = true;
      this.chkApplyPriceAdjOnOffDays.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(8, 151);
      this.label7.Margin = new Padding(4, 0, 4, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(42, 17);
      this.label7.TabIndex = 8;
      this.label7.Text = "Note";
      this.groupContainer2.Controls.Add((Control) this.siBtnEdit);
      this.groupContainer2.Controls.Add((Control) this.chkFixedExtension);
      this.groupContainer2.Controls.Add((Control) this.siBtnDelete);
      this.groupContainer2.Controls.Add((Control) this.siBtnAdd);
      this.groupContainer2.Controls.Add((Control) this.gvExtPriceAdj);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(12, 10);
      this.groupContainer2.Margin = new Padding(4, 4, 4, 4);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(466, 133);
      this.groupContainer2.TabIndex = 5;
      this.chkFixedExtension.AutoSize = true;
      this.chkFixedExtension.BackColor = Color.Transparent;
      this.chkFixedExtension.Location = new Point(5, 4);
      this.chkFixedExtension.Margin = new Padding(4, 4, 4, 4);
      this.chkFixedExtension.Name = "chkFixedExtension";
      this.chkFixedExtension.Size = new Size(249, 20);
      this.chkFixedExtension.TabIndex = 7;
      this.chkFixedExtension.Text = "Fixed extension days and adjustment";
      this.chkFixedExtension.UseVisualStyleBackColor = false;
      this.chkFixedExtension.CheckedChanged += new EventHandler(this.chkFixedExtension_CheckedChanged);
      this.gvExtPriceAdj.BorderStyle = BorderStyle.None;
      gvColumn1.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "Days to Extend";
      gvColumn1.Width = 120;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Price Adjustment";
      gvColumn2.Width = 140;
      this.gvExtPriceAdj.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvExtPriceAdj.Dock = DockStyle.Fill;
      this.gvExtPriceAdj.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvExtPriceAdj.Location = new Point(1, 26);
      this.gvExtPriceAdj.Margin = new Padding(4, 4, 4, 4);
      this.gvExtPriceAdj.Name = "gvExtPriceAdj";
      this.gvExtPriceAdj.Size = new Size(464, 106);
      this.gvExtPriceAdj.TabIndex = 0;
      this.gvExtPriceAdj.SelectedIndexChanged += new EventHandler(this.gvExtPriceAdj_SelectedIndexChanged);
      this.gvExtPriceAdj.EditorOpening += new GVSubItemEditingEventHandler(this.gvExtPriceAdj_EditorOpening);
      this.gvExtPriceAdj.EditorClosing += new GVSubItemEditingEventHandler(this.gvExtPriceAdj_EditorClosing);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(48, 151);
      this.label5.Margin = new Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(573, 16);
      this.label5.TabIndex = 7;
      this.label5.Text = ": Use negative price adjustments (for example, -0.125) to indicate a penalty for a given extension.";
      this.chkAllowDailyAdj.AutoSize = true;
      this.chkAllowDailyAdj.Location = new Point(504, 18);
      this.chkAllowDailyAdj.Margin = new Padding(4, 4, 4, 4);
      this.chkAllowDailyAdj.Name = "chkAllowDailyAdj";
      this.chkAllowDailyAdj.Size = new Size(161, 20);
      this.chkAllowDailyAdj.TabIndex = 1;
      this.chkAllowDailyAdj.Text = "Allow daily adjustment";
      this.chkAllowDailyAdj.UseVisualStyleBackColor = true;
      this.chkAllowDailyAdj.CheckedChanged += new EventHandler(this.chkAllowDailyAdj_CheckedChanged);
      this.txtDailyPriceAdj.Enabled = false;
      this.txtDailyPriceAdj.Location = new Point(700, 42);
      this.txtDailyPriceAdj.Margin = new Padding(4, 4, 4, 4);
      this.txtDailyPriceAdj.Name = "txtDailyPriceAdj";
      this.txtDailyPriceAdj.Size = new Size(92, 22);
      this.txtDailyPriceAdj.TabIndex = 4;
      this.txtDailyPriceAdj.TextChanged += new EventHandler(this.txtDailyPriceAdj_TextChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(523, 46);
      this.label4.Margin = new Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(155, 16);
      this.label4.TabIndex = 2;
      this.label4.Text = "Price adjustment per day";
      this.pnlAdjustPerLockExt.Controls.Add((Control) this.label10);
      this.pnlAdjustPerLockExt.Controls.Add((Control) this.groupContainer3);
      this.pnlAdjustPerLockExt.Controls.Add((Control) this.label11);
      this.pnlAdjustPerLockExt.Dock = DockStyle.Top;
      this.pnlAdjustPerLockExt.Location = new Point(0, 280);
      this.pnlAdjustPerLockExt.Margin = new Padding(4, 4, 4, 4);
      this.pnlAdjustPerLockExt.Name = "pnlAdjustPerLockExt";
      this.pnlAdjustPerLockExt.Size = new Size(277, 174);
      this.pnlAdjustPerLockExt.TabIndex = 10;
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(8, 153);
      this.label10.Margin = new Padding(4, 0, 4, 0);
      this.label10.Name = "label10";
      this.label10.Size = new Size(42, 17);
      this.label10.TabIndex = 8;
      this.label10.Text = "Note";
      this.groupContainer3.Controls.Add((Control) this.btnEditAdjustPerLockExt);
      this.groupContainer3.Controls.Add((Control) this.btnDeleteAdjustPerLockExt);
      this.groupContainer3.Controls.Add((Control) this.btnAddAdjustPerLockExt);
      this.groupContainer3.Controls.Add((Control) this.gvAdjustPerLockExt);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(12, 11);
      this.groupContainer3.Margin = new Padding(4, 4, 4, 4);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(508, 133);
      this.groupContainer3.TabIndex = 5;
      this.groupContainer3.Text = "Adjustment per Lock Extension";
      this.gvAdjustPerLockExt.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "Extension Number";
      gvColumn3.Width = 120;
      gvColumn4.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Days To Extend";
      gvColumn4.Width = 140;
      gvColumn5.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column3";
      gvColumn5.Text = "Price Adjustment";
      gvColumn5.Width = 100;
      this.gvAdjustPerLockExt.Columns.AddRange(new GVColumn[3]
      {
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvAdjustPerLockExt.Dock = DockStyle.Fill;
      this.gvAdjustPerLockExt.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAdjustPerLockExt.Location = new Point(1, 26);
      this.gvAdjustPerLockExt.Margin = new Padding(4, 4, 4, 4);
      this.gvAdjustPerLockExt.Name = "gvAdjustPerLockExt";
      this.gvAdjustPerLockExt.Size = new Size(506, 106);
      this.gvAdjustPerLockExt.TabIndex = 0;
      this.gvAdjustPerLockExt.SelectedIndexChanged += new EventHandler(this.gvAdjustPerLockExt_SelectedIndexChanged);
      this.gvAdjustPerLockExt.EditorOpening += new GVSubItemEditingEventHandler(this.gvAdjustPerLockExt_EditorOpening);
      this.gvAdjustPerLockExt.EditorClosing += new GVSubItemEditingEventHandler(this.gvAdjustPerLockExt_EditorClosing);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(48, 153);
      this.label11.Margin = new Padding(4, 0, 4, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(573, 16);
      this.label11.TabIndex = 7;
      this.label11.Text = ": Use negative price adjustments (for example, -0.125) to indicate a penalty for a given extension.";
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.ckLockExtAllowTotalTimesCap);
      this.gradientPanel1.Controls.Add((Control) this.txtLockExtAllowTotalTimes);
      this.gradientPanel1.Controls.Add((Control) this.label17);
      this.gradientPanel1.Controls.Add((Control) this.ckLockExtensionAllowTotalCap);
      this.gradientPanel1.Controls.Add((Control) this.txtLockExtensionAllowTotalCapDays);
      this.gradientPanel1.Controls.Add((Control) this.label12);
      this.gradientPanel1.Controls.Add((Control) this.txtExtensionDays);
      this.gradientPanel1.Controls.Add((Control) this.label9);
      this.gradientPanel1.Controls.Add((Control) this.rbtnCustomDays);
      this.gradientPanel1.Controls.Add((Control) this.rbtnLockPeriod);
      this.gradientPanel1.Controls.Add((Control) this.rbtnUnlimted);
      this.gradientPanel1.Controls.Add((Control) this.label8);
      this.gradientPanel1.Controls.Add((Control) this.chkEnableLockExtension);
      this.gradientPanel1.Controls.Add((Control) this.cmbLockExtensionOption);
      this.gradientPanel1.Controls.Add((Control) this.label6);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 26);
      this.gradientPanel1.Margin = new Padding(4, 4, 4, 4);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(277, 254);
      this.gradientPanel1.TabIndex = 6;
      this.ckLockExtAllowTotalTimesCap.BackColor = Color.Transparent;
      this.ckLockExtAllowTotalTimesCap.Location = new Point(41, 183);
      this.ckLockExtAllowTotalTimesCap.Margin = new Padding(4, 4, 4, 4);
      this.ckLockExtAllowTotalTimesCap.Name = "ckLockExtAllowTotalTimesCap";
      this.ckLockExtAllowTotalTimesCap.Size = new Size(192, 21);
      this.ckLockExtAllowTotalTimesCap.TabIndex = 16;
      this.ckLockExtAllowTotalTimesCap.Text = "Limit locks to not exceed";
      this.ckLockExtAllowTotalTimesCap.UseVisualStyleBackColor = false;
      this.ckLockExtAllowTotalTimesCap.CheckedChanged += new EventHandler(this.ckLockExtAllowTotalTimesCap_CheckedChanged);
      this.txtLockExtAllowTotalTimes.Location = new Point(242, 181);
      this.txtLockExtAllowTotalTimes.Margin = new Padding(4, 4, 4, 4);
      this.txtLockExtAllowTotalTimes.MaxLength = 3;
      this.txtLockExtAllowTotalTimes.Name = "txtLockExtAllowTotalTimes";
      this.txtLockExtAllowTotalTimes.Size = new Size(130, 22);
      this.txtLockExtAllowTotalTimes.TabIndex = 15;
      this.txtLockExtAllowTotalTimes.TextChanged += new EventHandler(this.txtLockExtAllowTotalTimes_TextChanged);
      this.label17.AutoSize = true;
      this.label17.BackColor = Color.Transparent;
      this.label17.Location = new Point(381, 185);
      this.label17.Margin = new Padding(4, 0, 4, 0);
      this.label17.Name = "label17";
      this.label17.Size = new Size(99, 16);
      this.label17.TabIndex = 14;
      this.label17.Text = "total extensions";
      this.ckLockExtensionAllowTotalCap.BackColor = Color.Transparent;
      this.ckLockExtensionAllowTotalCap.Location = new Point(41, 148);
      this.ckLockExtensionAllowTotalCap.Margin = new Padding(4, 4, 4, 4);
      this.ckLockExtensionAllowTotalCap.Name = "ckLockExtensionAllowTotalCap";
      this.ckLockExtensionAllowTotalCap.Size = new Size(258, 21);
      this.ckLockExtensionAllowTotalCap.TabIndex = 13;
      this.ckLockExtensionAllowTotalCap.Text = "Limit extended locks to not exceed";
      this.ckLockExtensionAllowTotalCap.UseVisualStyleBackColor = false;
      this.ckLockExtensionAllowTotalCap.CheckedChanged += new EventHandler(this.ckLockExtensionAllowTotalCap_CheckedChanged);
      this.txtLockExtensionAllowTotalCapDays.Location = new Point(308, 146);
      this.txtLockExtensionAllowTotalCapDays.Margin = new Padding(4, 4, 4, 4);
      this.txtLockExtensionAllowTotalCapDays.MaxLength = 3;
      this.txtLockExtensionAllowTotalCapDays.Name = "txtLockExtensionAllowTotalCapDays";
      this.txtLockExtensionAllowTotalCapDays.Size = new Size(132, 22);
      this.txtLockExtensionAllowTotalCapDays.TabIndex = 12;
      this.txtLockExtensionAllowTotalCapDays.TextChanged += new EventHandler(this.txtLockExtensionAllowTotalCapDays_TextChanged);
      this.label12.AutoSize = true;
      this.label12.BackColor = Color.Transparent;
      this.label12.Location = new Point(450, 149);
      this.label12.Margin = new Padding(4, 0, 4, 0);
      this.label12.Name = "label12";
      this.label12.Size = new Size(93, 16);
      this.label12.TabIndex = 11;
      this.label12.Text = "total lock days";
      this.txtExtensionDays.Location = new Point(308, 105);
      this.txtExtensionDays.Margin = new Padding(4, 4, 4, 4);
      this.txtExtensionDays.MaxLength = 3;
      this.txtExtensionDays.Name = "txtExtensionDays";
      this.txtExtensionDays.Size = new Size(132, 22);
      this.txtExtensionDays.TabIndex = 9;
      this.txtExtensionDays.TextChanged += new EventHandler(this.txtExtensionDays_TextChanged);
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Location = new Point(446, 108);
      this.label9.Margin = new Padding(4, 0, 4, 0);
      this.label9.Name = "label9";
      this.label9.Size = new Size(37, 16);
      this.label9.TabIndex = 8;
      this.label9.Text = "days";
      this.rbtnCustomDays.AutoSize = true;
      this.rbtnCustomDays.BackColor = Color.Transparent;
      this.rbtnCustomDays.Location = new Point(67, 106);
      this.rbtnCustomDays.Margin = new Padding(4, 4, 4, 4);
      this.rbtnCustomDays.Name = "rbtnCustomDays";
      this.rbtnCustomDays.Size = new Size(205, 20);
      this.rbtnCustomDays.TabIndex = 6;
      this.rbtnCustomDays.TabStop = true;
      this.rbtnCustomDays.Text = "Limit extensions to not exceed";
      this.rbtnCustomDays.UseVisualStyleBackColor = false;
      this.rbtnCustomDays.CheckedChanged += new EventHandler(this.ExtensionCapType_CheckedChanged);
      this.rbtnLockPeriod.AutoSize = true;
      this.rbtnLockPeriod.BackColor = Color.Transparent;
      this.rbtnLockPeriod.Location = new Point(66, 82);
      this.rbtnLockPeriod.Margin = new Padding(4, 4, 4, 4);
      this.rbtnLockPeriod.Name = "rbtnLockPeriod";
      this.rbtnLockPeriod.Size = new Size(182, 20);
      this.rbtnLockPeriod.TabIndex = 5;
      this.rbtnLockPeriod.TabStop = true;
      this.rbtnLockPeriod.Text = "Original Lock Period days";
      this.rbtnLockPeriod.UseVisualStyleBackColor = false;
      this.rbtnLockPeriod.CheckedChanged += new EventHandler(this.ExtensionCapType_CheckedChanged);
      this.rbtnUnlimted.AutoSize = true;
      this.rbtnUnlimted.BackColor = Color.Transparent;
      this.rbtnUnlimted.Location = new Point(66, 58);
      this.rbtnUnlimted.Margin = new Padding(4, 4, 4, 4);
      this.rbtnUnlimted.Name = "rbtnUnlimted";
      this.rbtnUnlimted.Size = new Size(177, 20);
      this.rbtnUnlimted.TabIndex = 4;
      this.rbtnUnlimted.TabStop = true;
      this.rbtnUnlimted.Text = "Unlimited extension days";
      this.rbtnUnlimted.UseVisualStyleBackColor = false;
      this.rbtnUnlimted.CheckedChanged += new EventHandler(this.ExtensionCapType_CheckedChanged);
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.Location = new Point(36, 38);
      this.label8.Margin = new Padding(4, 0, 4, 0);
      this.label8.Name = "label8";
      this.label8.Size = new Size(295, 16);
      this.label8.TabIndex = 3;
      this.label8.Text = "How many days should extensions be limited to?";
      this.chkEnableLockExtension.BackColor = Color.Transparent;
      this.chkEnableLockExtension.Location = new Point(14, 9);
      this.chkEnableLockExtension.Margin = new Padding(4, 4, 4, 4);
      this.chkEnableLockExtension.Name = "chkEnableLockExtension";
      this.chkEnableLockExtension.Size = new Size(391, 21);
      this.chkEnableLockExtension.TabIndex = 2;
      this.chkEnableLockExtension.Text = "Enable lock extension requests";
      this.chkEnableLockExtension.UseVisualStyleBackColor = false;
      this.chkEnableLockExtension.CheckedChanged += new EventHandler(this.chkEnableLockExtension_CheckedChanged);
      this.cmbLockExtensionOption.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLockExtensionOption.FormattingEnabled = true;
      this.cmbLockExtensionOption.Items.AddRange(new object[3]
      {
        (object) "No company control – user can request any extension days and adjustment",
        (object) "Company controls extension days and price adjustment",
        (object) "Company controls extension days and price adjustment by extension occurrence"
      });
      this.cmbLockExtensionOption.Location = new Point(116, 213);
      this.cmbLockExtensionOption.Margin = new Padding(4, 4, 4, 4);
      this.cmbLockExtensionOption.Name = "cmbLockExtensionOption";
      this.cmbLockExtensionOption.Size = new Size(562, 24);
      this.cmbLockExtensionOption.TabIndex = 1;
      this.cmbLockExtensionOption.SelectedIndexChanged += new EventHandler(this.cmbLockExtensionOption_SelectedIndexChanged);
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(11, 218);
      this.label6.Margin = new Padding(4, 0, 4, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(89, 16);
      this.label6.TabIndex = 0;
      this.label6.Text = "Control option";
      this.gcLockRequestType.Borders = AnchorStyles.Top;
      this.gcLockRequestType.Controls.Add((Control) this.pnlForReLocks);
      this.gcLockRequestType.Controls.Add((Control) this.pnlLockUpdates);
      this.gcLockRequestType.Controls.Add((Control) this.chkEnableRelockForTpoClient);
      this.gcLockRequestType.Controls.Add((Control) this.labelGetCurrentPricingDays);
      this.gcLockRequestType.Controls.Add((Control) this.txtLockExpDays);
      this.gcLockRequestType.Controls.Add((Control) this.chkGetCurrentPricing);
      this.gcLockRequestType.Controls.Add((Control) this.label13);
      this.gcLockRequestType.Controls.Add((Control) this.label16);
      this.gcLockRequestType.Controls.Add((Control) this.chkAllowRelockOutsideLDHours);
      this.gcLockRequestType.Controls.Add((Control) this.chkAllowNewLockOutsideLDHours);
      this.gcLockRequestType.Controls.Add((Control) this.chkGetPricingRelock);
      this.gcLockRequestType.Controls.Add((Control) this.chkRelock);
      this.gcLockRequestType.Controls.Add((Control) this.chkReLockOnly);
      this.gcLockRequestType.Dock = DockStyle.Top;
      this.gcLockRequestType.HeaderForeColor = SystemColors.ControlText;
      this.gcLockRequestType.Location = new Point(0, 174);
      this.gcLockRequestType.Margin = new Padding(4, 4, 4, 4);
      this.gcLockRequestType.Name = "gcLockRequestType";
      this.gcLockRequestType.Size = new Size(277, 578);
      this.gcLockRequestType.TabIndex = 4;
      this.gcLockRequestType.Text = "Lock Update and Re-Lock Enablement";
      this.pnlForReLocks.Controls.Add((Control) this.panel5);
      this.pnlForReLocks.Controls.Add((Control) this.panel4);
      this.pnlForReLocks.Controls.Add((Control) this.panel1);
      this.pnlForReLocks.Controls.Add((Control) this.panel3);
      this.pnlForReLocks.Controls.Add((Control) this.panel2);
      this.pnlForReLocks.Location = new Point(36, 295);
      this.pnlForReLocks.Margin = new Padding(4, 4, 4, 4);
      this.pnlForReLocks.Name = "pnlForReLocks";
      this.pnlForReLocks.Size = new Size(675, 281);
      this.pnlForReLocks.TabIndex = 39;
      this.panel5.Controls.Add((Control) this.ckRelockAllowTotalCap);
      this.panel5.Controls.Add((Control) this.txtRelockAllowTotalCapDays);
      this.panel5.Controls.Add((Control) this.labelRelockAllowTotalCap);
      this.panel5.Location = new Point(3, 218);
      this.panel5.Margin = new Padding(4, 4, 4, 4);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(483, 32);
      this.panel5.TabIndex = 33;
      this.ckRelockAllowTotalCap.BackColor = Color.Transparent;
      this.ckRelockAllowTotalCap.Location = new Point(4, 4);
      this.ckRelockAllowTotalCap.Margin = new Padding(4, 4, 4, 4);
      this.ckRelockAllowTotalCap.Name = "ckRelockAllowTotalCap";
      this.ckRelockAllowTotalCap.Size = new Size(220, 22);
      this.ckRelockAllowTotalCap.TabIndex = 16;
      this.ckRelockAllowTotalCap.Text = "Limit Re-Locks to not exceed";
      this.ckRelockAllowTotalCap.UseVisualStyleBackColor = false;
      this.ckRelockAllowTotalCap.CheckedChanged += new EventHandler(this.ckRelockAllowTotalCap_CheckedChanged);
      this.txtRelockAllowTotalCapDays.Enabled = false;
      this.txtRelockAllowTotalCapDays.Location = new Point(232, 2);
      this.txtRelockAllowTotalCapDays.Margin = new Padding(4, 4, 4, 4);
      this.txtRelockAllowTotalCapDays.MaxLength = 2;
      this.txtRelockAllowTotalCapDays.Name = "txtRelockAllowTotalCapDays";
      this.txtRelockAllowTotalCapDays.Size = new Size(132, 22);
      this.txtRelockAllowTotalCapDays.TabIndex = 15;
      this.txtRelockAllowTotalCapDays.TextChanged += new EventHandler(this.txtRelockAllowTotalCapDays_TextChanged);
      this.txtRelockAllowTotalCapDays.KeyPress += new KeyPressEventHandler(this.txtRelockAllowTotalCapDays_KeyPress);
      this.labelRelockAllowTotalCap.AutoSize = true;
      this.labelRelockAllowTotalCap.BackColor = Color.Transparent;
      this.labelRelockAllowTotalCap.Location = new Point(373, 5);
      this.labelRelockAllowTotalCap.Margin = new Padding(4, 0, 4, 0);
      this.labelRelockAllowTotalCap.Name = "labelRelockAllowTotalCap";
      this.labelRelockAllowTotalCap.Size = new Size(93, 16);
      this.labelRelockAllowTotalCap.TabIndex = 14;
      this.labelRelockAllowTotalCap.Text = "total Re-Locks";
      this.panel4.Controls.Add((Control) this.ckRelockFeeAllowed);
      this.panel4.Controls.Add((Control) this.labelRelockFeeAllowed);
      this.panel4.Controls.Add((Control) this.txtRelockFee);
      this.panel4.Location = new Point(0, 162);
      this.panel4.Margin = new Padding(4, 4, 4, 4);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(594, 25);
      this.panel4.TabIndex = 39;
      this.ckRelockFeeAllowed.BackColor = Color.Transparent;
      this.ckRelockFeeAllowed.Location = new Point(4, 2);
      this.ckRelockFeeAllowed.Margin = new Padding(4, 4, 4, 4);
      this.ckRelockFeeAllowed.Name = "ckRelockFeeAllowed";
      this.ckRelockFeeAllowed.Size = new Size(189, 22);
      this.ckRelockFeeAllowed.TabIndex = 19;
      this.ckRelockFeeAllowed.Text = "Apply a Re-Lock fee of ";
      this.ckRelockFeeAllowed.UseVisualStyleBackColor = false;
      this.ckRelockFeeAllowed.CheckedChanged += new EventHandler(this.ckRelockFeeAllowed_CheckedChanged);
      this.labelRelockFeeAllowed.AutoSize = true;
      this.labelRelockFeeAllowed.BackColor = Color.Transparent;
      this.labelRelockFeeAllowed.Location = new Point(347, 4);
      this.labelRelockFeeAllowed.Margin = new Padding(4, 0, 4, 0);
      this.labelRelockFeeAllowed.Name = "labelRelockFeeAllowed";
      this.labelRelockFeeAllowed.Size = new Size(109, 16);
      this.labelRelockFeeAllowed.TabIndex = 17;
      this.labelRelockFeeAllowed.Text = "for each Re-Lock";
      this.txtRelockFee.Enabled = false;
      this.txtRelockFee.Location = new Point(201, 0);
      this.txtRelockFee.Margin = new Padding(4, 4, 4, 4);
      this.txtRelockFee.Name = "txtRelockFee";
      this.txtRelockFee.Size = new Size(132, 22);
      this.txtRelockFee.TabIndex = 18;
      this.txtRelockFee.KeyPress += new KeyPressEventHandler(this.txtRelockFee_KeyPress);
      this.txtRelockFee.Leave += new EventHandler(this.txtRelockFee_Leave);
      this.panel1.Controls.Add((Control) this.chkReLockReLockfees);
      this.panel1.Controls.Add((Control) this.label22);
      this.panel1.Controls.Add((Control) this.chkReLockExtensionFees);
      this.panel1.Controls.Add((Control) this.chkReLockCustomPriceAdjustments);
      this.panel1.Controls.Add((Control) this.chkReLockPriceConcessions);
      this.panel1.Controls.Add((Control) this.chkUseWorstCasePrice);
      this.panel1.Controls.Add((Control) this.label24);
      this.panel1.Controls.Add((Control) this.txtWorstCasePrice);
      this.panel1.Location = new Point(2, 0);
      this.panel1.Margin = new Padding(4, 4, 4, 4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(643, 162);
      this.panel1.TabIndex = 38;
      this.chkReLockReLockfees.BackColor = Color.Transparent;
      this.chkReLockReLockfees.Location = new Point(25, 137);
      this.chkReLockReLockfees.Margin = new Padding(4, 4, 4, 4);
      this.chkReLockReLockfees.Name = "chkReLockReLockfees";
      this.chkReLockReLockfees.Size = new Size(124, 22);
      this.chkReLockReLockfees.TabIndex = 24;
      this.chkReLockReLockfees.Text = "Re-Lock fees";
      this.chkReLockReLockfees.UseVisualStyleBackColor = false;
      this.chkReLockReLockfees.CheckedChanged += new EventHandler(this.chkReLockReLockfees_CheckedChanged);
      this.label22.AutoSize = true;
      this.label22.Location = new Point(-3, 26);
      this.label22.Margin = new Padding(4, 0, 4, 0);
      this.label22.Name = "label22";
      this.label22.Size = new Size(473, 16);
      this.label22.TabIndex = 20;
      this.label22.Text = "For Re-Locks using Worst Case Price, include the following accumulated items:";
      this.chkReLockExtensionFees.BackColor = Color.Transparent;
      this.chkReLockExtensionFees.Location = new Point(25, 52);
      this.chkReLockExtensionFees.Margin = new Padding(4, 4, 4, 4);
      this.chkReLockExtensionFees.Name = "chkReLockExtensionFees";
      this.chkReLockExtensionFees.Size = new Size(189, 21);
      this.chkReLockExtensionFees.TabIndex = 21;
      this.chkReLockExtensionFees.Text = "Extension Fees";
      this.chkReLockExtensionFees.UseVisualStyleBackColor = false;
      this.chkReLockExtensionFees.CheckedChanged += new EventHandler(this.chkReLockExtensionFees_CheckedChanged);
      this.chkReLockCustomPriceAdjustments.BackColor = Color.Transparent;
      this.chkReLockCustomPriceAdjustments.Location = new Point(25, 80);
      this.chkReLockCustomPriceAdjustments.Margin = new Padding(4, 4, 4, 4);
      this.chkReLockCustomPriceAdjustments.Name = "chkReLockCustomPriceAdjustments";
      this.chkReLockCustomPriceAdjustments.Size = new Size(221, 21);
      this.chkReLockCustomPriceAdjustments.TabIndex = 22;
      this.chkReLockCustomPriceAdjustments.Text = "Custom Price Adjustments";
      this.chkReLockCustomPriceAdjustments.UseVisualStyleBackColor = false;
      this.chkReLockCustomPriceAdjustments.CheckedChanged += new EventHandler(this.chkReLockCustomPriceAdjustments_CheckedChanged);
      this.chkReLockPriceConcessions.BackColor = Color.Transparent;
      this.chkReLockPriceConcessions.Location = new Point(25, 108);
      this.chkReLockPriceConcessions.Margin = new Padding(4, 4, 4, 4);
      this.chkReLockPriceConcessions.Name = "chkReLockPriceConcessions";
      this.chkReLockPriceConcessions.Size = new Size(189, 21);
      this.chkReLockPriceConcessions.TabIndex = 23;
      this.chkReLockPriceConcessions.Text = "Price Concessions";
      this.chkReLockPriceConcessions.UseVisualStyleBackColor = false;
      this.chkReLockPriceConcessions.CheckedChanged += new EventHandler(this.chkReLockPriceConcessions_CheckedChanged);
      this.chkUseWorstCasePrice.BackColor = Color.Transparent;
      this.chkUseWorstCasePrice.Location = new Point(2, 2);
      this.chkUseWorstCasePrice.Margin = new Padding(4, 4, 4, 4);
      this.chkUseWorstCasePrice.Name = "chkUseWorstCasePrice";
      this.chkUseWorstCasePrice.Size = new Size(281, 22);
      this.chkUseWorstCasePrice.TabIndex = 30;
      this.chkUseWorstCasePrice.Text = "Use Worst Case Price prior or equal to";
      this.chkUseWorstCasePrice.UseVisualStyleBackColor = false;
      this.chkUseWorstCasePrice.CheckedChanged += new EventHandler(this.chkUseWorstCasePrice_CheckedChanged);
      this.label24.AutoSize = true;
      this.label24.Location = new Point(349, 4);
      this.label24.Margin = new Padding(4, 0, 4, 0);
      this.label24.Name = "label24";
      this.label24.Size = new Size(269, 16);
      this.label24.TabIndex = 32;
      this.label24.Text = "days of Lock Expiration or Cancellation Date";
      this.txtWorstCasePrice.Enabled = false;
      this.txtWorstCasePrice.Location = new Point(285, 0);
      this.txtWorstCasePrice.Margin = new Padding(4, 4, 4, 4);
      this.txtWorstCasePrice.Name = "txtWorstCasePrice";
      this.txtWorstCasePrice.Size = new Size(57, 22);
      this.txtWorstCasePrice.TabIndex = 31;
      this.txtWorstCasePrice.KeyPress += new KeyPressEventHandler(this.txtWorstCasePrice_KeyPress);
      this.panel3.Controls.Add((Control) this.chkRestrictLockPeriod);
      this.panel3.Location = new Point(3, 249);
      this.panel3.Margin = new Padding(4, 4, 4, 4);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(683, 23);
      this.panel3.TabIndex = 33;
      this.chkRestrictLockPeriod.BackColor = Color.Transparent;
      this.chkRestrictLockPeriod.Location = new Point(4, 0);
      this.chkRestrictLockPeriod.Margin = new Padding(4, 4, 4, 4);
      this.chkRestrictLockPeriod.Name = "chkRestrictLockPeriod";
      this.chkRestrictLockPeriod.Size = new Size(588, 23);
      this.chkRestrictLockPeriod.TabIndex = 36;
      this.chkRestrictLockPeriod.Text = "Restrict Lock Period of Re-Lock to not exceed the Original Lock period of Initial Lock";
      this.chkRestrictLockPeriod.UseVisualStyleBackColor = false;
      this.chkRestrictLockPeriod.CheckedChanged += new EventHandler(this.chkRestrictLockPeriod_CheckedChanged);
      this.panel2.Controls.Add((Control) this.chkWaiveFeeAfter);
      this.panel2.Controls.Add((Control) this.label25);
      this.panel2.Controls.Add((Control) this.txtWaiveFeeAfter);
      this.panel2.Location = new Point(4, 192);
      this.panel2.Margin = new Padding(4, 4, 4, 4);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(584, 25);
      this.panel2.TabIndex = 33;
      this.chkWaiveFeeAfter.BackColor = Color.Transparent;
      this.chkWaiveFeeAfter.Location = new Point(23, 2);
      this.chkWaiveFeeAfter.Margin = new Padding(4, 4, 4, 4);
      this.chkWaiveFeeAfter.Name = "chkWaiveFeeAfter";
      this.chkWaiveFeeAfter.Size = new Size(148, 22);
      this.chkWaiveFeeAfter.TabIndex = 35;
      this.chkWaiveFeeAfter.Text = "Waive fee after ";
      this.chkWaiveFeeAfter.UseVisualStyleBackColor = false;
      this.chkWaiveFeeAfter.CheckedChanged += new EventHandler(this.chkWaiveFeeAfter_CheckedChanged);
      this.label25.AutoSize = true;
      this.label25.BackColor = Color.Transparent;
      this.label25.Location = new Point(236, 4);
      this.label25.Margin = new Padding(4, 0, 4, 0);
      this.label25.Name = "label25";
      this.label25.Size = new Size(269, 16);
      this.label25.TabIndex = 33;
      this.label25.Text = "days of Lock Expiration or Cancellation Date";
      this.txtWaiveFeeAfter.Enabled = false;
      this.txtWaiveFeeAfter.Location = new Point(171, 0);
      this.txtWaiveFeeAfter.Margin = new Padding(4, 4, 4, 4);
      this.txtWaiveFeeAfter.Name = "txtWaiveFeeAfter";
      this.txtWaiveFeeAfter.Size = new Size(56, 22);
      this.txtWaiveFeeAfter.TabIndex = 34;
      this.txtWaiveFeeAfter.KeyPress += new KeyPressEventHandler(this.txtWaiveFeeAfter_KeyPress);
      this.pnlLockUpdates.Controls.Add((Control) this.label23);
      this.pnlLockUpdates.Controls.Add((Control) this.chkLockUpdateExtensionFees);
      this.pnlLockUpdates.Controls.Add((Control) this.chkLockUpdateCustomPriceAdjustments);
      this.pnlLockUpdates.Controls.Add((Control) this.chkLockUpdatePriceConcessions);
      this.pnlLockUpdates.Controls.Add((Control) this.chkLockUpdateReLockfees);
      this.pnlLockUpdates.Location = new Point(386, 176);
      this.pnlLockUpdates.Margin = new Padding(4, 4, 4, 4);
      this.pnlLockUpdates.Name = "pnlLockUpdates";
      this.pnlLockUpdates.Size = new Size(126, 52);
      this.pnlLockUpdates.TabIndex = 37;
      this.pnlLockUpdates.Visible = false;
      this.label23.AutoSize = true;
      this.label23.Location = new Point(4, 9);
      this.label23.Margin = new Padding(4, 0, 4, 0);
      this.label23.Name = "label23";
      this.label23.Size = new Size(357, 16);
      this.label23.TabIndex = 25;
      this.label23.Text = "For Lock Updates, include the following accumulated items:";
      this.label23.Visible = false;
      this.chkLockUpdateExtensionFees.BackColor = Color.Transparent;
      this.chkLockUpdateExtensionFees.Location = new Point(32, 34);
      this.chkLockUpdateExtensionFees.Margin = new Padding(4, 4, 4, 4);
      this.chkLockUpdateExtensionFees.Name = "chkLockUpdateExtensionFees";
      this.chkLockUpdateExtensionFees.Size = new Size(189, 21);
      this.chkLockUpdateExtensionFees.TabIndex = 26;
      this.chkLockUpdateExtensionFees.Text = "Extension Fees";
      this.chkLockUpdateExtensionFees.UseVisualStyleBackColor = false;
      this.chkLockUpdateExtensionFees.Visible = false;
      this.chkLockUpdateExtensionFees.CheckedChanged += new EventHandler(this.chkLockUpdateExtensionFees_CheckedChanged);
      this.chkLockUpdateCustomPriceAdjustments.BackColor = Color.Transparent;
      this.chkLockUpdateCustomPriceAdjustments.Location = new Point(32, 62);
      this.chkLockUpdateCustomPriceAdjustments.Margin = new Padding(4, 4, 4, 4);
      this.chkLockUpdateCustomPriceAdjustments.Name = "chkLockUpdateCustomPriceAdjustments";
      this.chkLockUpdateCustomPriceAdjustments.Size = new Size(221, 21);
      this.chkLockUpdateCustomPriceAdjustments.TabIndex = 27;
      this.chkLockUpdateCustomPriceAdjustments.Text = "Custom Price Adjustments";
      this.chkLockUpdateCustomPriceAdjustments.UseVisualStyleBackColor = false;
      this.chkLockUpdateCustomPriceAdjustments.Visible = false;
      this.chkLockUpdateCustomPriceAdjustments.CheckedChanged += new EventHandler(this.chkLockUpdateCustomPriceAdjustments_CheckedChanged);
      this.chkLockUpdatePriceConcessions.BackColor = Color.Transparent;
      this.chkLockUpdatePriceConcessions.Location = new Point(32, 91);
      this.chkLockUpdatePriceConcessions.Margin = new Padding(4, 4, 4, 4);
      this.chkLockUpdatePriceConcessions.Name = "chkLockUpdatePriceConcessions";
      this.chkLockUpdatePriceConcessions.Size = new Size(189, 21);
      this.chkLockUpdatePriceConcessions.TabIndex = 28;
      this.chkLockUpdatePriceConcessions.Text = "Price Concessions";
      this.chkLockUpdatePriceConcessions.UseVisualStyleBackColor = false;
      this.chkLockUpdatePriceConcessions.Visible = false;
      this.chkLockUpdatePriceConcessions.CheckedChanged += new EventHandler(this.chkLockUpdatePriceConcessions_CheckedChanged);
      this.chkLockUpdateReLockfees.BackColor = Color.Transparent;
      this.chkLockUpdateReLockfees.Location = new Point(32, 119);
      this.chkLockUpdateReLockfees.Margin = new Padding(4, 4, 4, 4);
      this.chkLockUpdateReLockfees.Name = "chkLockUpdateReLockfees";
      this.chkLockUpdateReLockfees.Size = new Size(149, 21);
      this.chkLockUpdateReLockfees.TabIndex = 29;
      this.chkLockUpdateReLockfees.Text = "Re-Lock fees";
      this.chkLockUpdateReLockfees.UseVisualStyleBackColor = false;
      this.chkLockUpdateReLockfees.Visible = false;
      this.chkLockUpdateReLockfees.CheckedChanged += new EventHandler(this.chkLockUpdateReLockfees_CheckedChanged);
      this.chkEnableRelockForTpoClient.AutoSize = true;
      this.chkEnableRelockForTpoClient.Location = new Point(37, 69);
      this.chkEnableRelockForTpoClient.Margin = new Padding(4, 4, 4, 4);
      this.chkEnableRelockForTpoClient.Name = "chkEnableRelockForTpoClient";
      this.chkEnableRelockForTpoClient.Size = new Size(336, 20);
      this.chkEnableRelockForTpoClient.TabIndex = 13;
      this.chkEnableRelockForTpoClient.Text = "Enable Lock Updates and Re-Locks for TPO clients";
      this.chkEnableRelockForTpoClient.UseVisualStyleBackColor = true;
      this.chkEnableRelockForTpoClient.CheckedChanged += new EventHandler(this.chkEnableRelockForTpoClient_CheckedChanged);
      this.labelGetCurrentPricingDays.AutoSize = true;
      this.labelGetCurrentPricingDays.Location = new Point(302, 274);
      this.labelGetCurrentPricingDays.Margin = new Padding(4, 0, 4, 0);
      this.labelGetCurrentPricingDays.Name = "labelGetCurrentPricingDays";
      this.labelGetCurrentPricingDays.Size = new Size(269, 16);
      this.labelGetCurrentPricingDays.TabIndex = 12;
      this.labelGetCurrentPricingDays.Text = "days of Lock Expiration or Cancellation Date";
      this.txtLockExpDays.Enabled = false;
      this.txtLockExpDays.Location = new Point(232, 270);
      this.txtLockExpDays.Margin = new Padding(4, 4, 4, 4);
      this.txtLockExpDays.Name = "txtLockExpDays";
      this.txtLockExpDays.Size = new Size(57, 22);
      this.txtLockExpDays.TabIndex = 11;
      this.txtLockExpDays.KeyPress += new KeyPressEventHandler(this.txtLockExpDays_KeyPress);
      this.chkGetCurrentPricing.AutoSize = true;
      this.chkGetCurrentPricing.Location = new Point(39, 274);
      this.chkGetCurrentPricing.Margin = new Padding(4, 4, 4, 4);
      this.chkGetCurrentPricing.Name = "chkGetCurrentPricing";
      this.chkGetCurrentPricing.Size = new Size(165, 20);
      this.chkGetCurrentPricing.TabIndex = 9;
      this.chkGetCurrentPricing.Text = "Get current pricing after";
      this.chkGetCurrentPricing.UseVisualStyleBackColor = true;
      this.chkGetCurrentPricing.CheckedChanged += new EventHandler(this.chkGetCurrentPricing_CheckedChanged);
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
      this.label13.Location = new Point(34, 245);
      this.label13.Margin = new Padding(4, 0, 4, 0);
      this.label13.Name = "label13";
      this.label13.Size = new Size(380, 17);
      this.label13.TabIndex = 8;
      this.label13.Text = "For Re-Locks (Inactive Cancelled or Expired Locks)";
      this.label16.AutoSize = true;
      this.label16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
      this.label16.Location = new Point(34, 103);
      this.label16.Margin = new Padding(4, 0, 4, 0);
      this.label16.Name = "label16";
      this.label16.Size = new Size(244, 17);
      this.label16.TabIndex = 7;
      this.label16.Text = "For Lock Updates (Active Locks)";
      this.chkAllowRelockOutsideLDHours.AutoSize = true;
      this.chkAllowRelockOutsideLDHours.Location = new Point(37, 182);
      this.chkAllowRelockOutsideLDHours.Margin = new Padding(4, 4, 4, 4);
      this.chkAllowRelockOutsideLDHours.Name = "chkAllowRelockOutsideLDHours";
      this.chkAllowRelockOutsideLDHours.Size = new Size(315, 20);
      this.chkAllowRelockOutsideLDHours.TabIndex = 6;
      this.chkAllowRelockOutsideLDHours.Text = "Allow Lock Updates outside of Lock Desk Hours";
      this.chkAllowRelockOutsideLDHours.UseVisualStyleBackColor = true;
      this.chkAllowRelockOutsideLDHours.CheckedChanged += new EventHandler(this.chkAllowRelockOutsideLDHours_CheckedChanged);
      this.chkAllowNewLockOutsideLDHours.AutoSize = true;
      this.chkAllowNewLockOutsideLDHours.Enabled = false;
      this.chkAllowNewLockOutsideLDHours.Location = new Point(37, 208);
      this.chkAllowNewLockOutsideLDHours.Margin = new Padding(4, 4, 4, 4);
      this.chkAllowNewLockOutsideLDHours.Name = "chkAllowNewLockOutsideLDHours";
      this.chkAllowNewLockOutsideLDHours.Size = new Size(297, 20);
      this.chkAllowNewLockOutsideLDHours.TabIndex = 6;
      this.chkAllowNewLockOutsideLDHours.Text = "Allow New Locks outside of Lock Desk Hours";
      this.chkAllowNewLockOutsideLDHours.UseVisualStyleBackColor = true;
      this.chkAllowNewLockOutsideLDHours.CheckedChanged += new EventHandler(this.chkAllowNewLockOutsideLDHours_CheckedChanged);
      this.chkGetPricingRelock.AutoSize = true;
      this.chkGetPricingRelock.Location = new Point(37, 155);
      this.chkGetPricingRelock.Margin = new Padding(4, 4, 4, 4);
      this.chkGetPricingRelock.Name = "chkGetPricingRelock";
      this.chkGetPricingRelock.Size = new Size(355, 20);
      this.chkGetPricingRelock.TabIndex = 5;
      this.chkGetPricingRelock.Text = "Allow Get Pricing (Historical) Request for Lock Updates";
      this.chkGetPricingRelock.UseVisualStyleBackColor = true;
      this.chkRelock.AutoSize = true;
      this.chkRelock.Location = new Point(13, 41);
      this.chkRelock.Margin = new Padding(4, 4, 4, 4);
      this.chkRelock.Name = "chkRelock";
      this.chkRelock.Size = new Size(246, 20);
      this.chkRelock.TabIndex = 4;
      this.chkRelock.Text = "Enable Lock Updates and Re-Locks";
      this.chkRelock.UseVisualStyleBackColor = true;
      this.chkRelock.CheckedChanged += new EventHandler(this.chkRelock_CheckedChanged);
      this.chkReLockOnly.AutoSize = true;
      this.chkReLockOnly.Location = new Point(37, 128);
      this.chkReLockOnly.Margin = new Padding(4, 4, 4, 4);
      this.chkReLockOnly.Name = "chkReLockOnly";
      this.chkReLockOnly.Size = new Size(441, 20);
      this.chkReLockOnly.TabIndex = 3;
      this.chkReLockOnly.Text = "Only allow Lock Updates (Historical Pricing) when a current lock exists";
      this.chkReLockOnly.UseVisualStyleBackColor = true;
      this.chkReLockOnly.CheckedChanged += new EventHandler(this.chkReLockOnly_CheckedChanged);
      this.gcElapsedTime.Borders = AnchorStyles.Top;
      this.gcElapsedTime.Controls.Add((Control) this.chkDontAllowPriceChange);
      this.gcElapsedTime.Controls.Add((Control) this.lblElapsedTimeSettings);
      this.gcElapsedTime.Controls.Add((Control) this.chkTimerEnforceTiming);
      this.gcElapsedTime.Controls.Add((Control) this.txtElapsedTime);
      this.gcElapsedTime.Controls.Add((Control) this.label1);
      this.gcElapsedTime.Controls.Add((Control) this.chkElapseTime);
      this.gcElapsedTime.Dock = DockStyle.Top;
      this.gcElapsedTime.HeaderForeColor = SystemColors.ControlText;
      this.gcElapsedTime.Location = new Point(0, 0);
      this.gcElapsedTime.Margin = new Padding(4, 4, 4, 4);
      this.gcElapsedTime.Name = "gcElapsedTime";
      this.gcElapsedTime.Size = new Size(277, 174);
      this.gcElapsedTime.TabIndex = 0;
      this.gcElapsedTime.Text = "Lock Request Submission After Pricing has been Retrieved";
      this.chkDontAllowPriceChange.AutoSize = true;
      this.chkDontAllowPriceChange.Location = new Point(16, 41);
      this.chkDontAllowPriceChange.Margin = new Padding(4, 4, 4, 4);
      this.chkDontAllowPriceChange.Name = "chkDontAllowPriceChange";
      this.chkDontAllowPriceChange.Size = new Size(347, 20);
      this.chkDontAllowPriceChange.TabIndex = 5;
      this.chkDontAllowPriceChange.Text = "Do not allow changes after pricing has been retrieved ";
      this.chkDontAllowPriceChange.UseVisualStyleBackColor = true;
      this.chkDontAllowPriceChange.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.lblElapsedTimeSettings.AutoSize = true;
      this.lblElapsedTimeSettings.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblElapsedTimeSettings.Location = new Point(9, 76);
      this.lblElapsedTimeSettings.Margin = new Padding(4, 0, 4, 0);
      this.lblElapsedTimeSettings.Name = "lblElapsedTimeSettings";
      this.lblElapsedTimeSettings.Size = new Size(170, 17);
      this.lblElapsedTimeSettings.TabIndex = 4;
      this.lblElapsedTimeSettings.Text = "Elapsed Time Settings";
      this.chkTimerEnforceTiming.AutoSize = true;
      this.chkTimerEnforceTiming.Enabled = false;
      this.chkTimerEnforceTiming.Location = new Point(39, 137);
      this.chkTimerEnforceTiming.Margin = new Padding(4, 4, 4, 4);
      this.chkTimerEnforceTiming.Name = "chkTimerEnforceTiming";
      this.chkTimerEnforceTiming.Size = new Size(360, 20);
      this.chkTimerEnforceTiming.TabIndex = 3;
      this.chkTimerEnforceTiming.Text = "Apply the pricing timer only when there is no current lock.";
      this.chkTimerEnforceTiming.UseVisualStyleBackColor = true;
      this.chkTimerEnforceTiming.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.txtElapsedTime.Location = new Point(468, 106);
      this.txtElapsedTime.Margin = new Padding(4, 4, 4, 4);
      this.txtElapsedTime.MaxLength = 4;
      this.txtElapsedTime.Name = "txtElapsedTime";
      this.txtElapsedTime.Size = new Size(132, 22);
      this.txtElapsedTime.TabIndex = 2;
      this.txtElapsedTime.TextChanged += new EventHandler(this.txtElapsedTime_TextChanged);
      this.txtElapsedTime.KeyDown += new KeyEventHandler(this.txt_KeyDown);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(622, 110);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(53, 16);
      this.label1.TabIndex = 1;
      this.label1.Text = "minutes";
      this.chkElapseTime.AutoSize = true;
      this.chkElapseTime.Location = new Point(13, 108);
      this.chkElapseTime.Margin = new Padding(4, 4, 4, 4);
      this.chkElapseTime.Name = "chkElapseTime";
      this.chkElapseTime.Size = new Size(407, 20);
      this.chkElapseTime.TabIndex = 0;
      this.chkElapseTime.Text = "Allow lock request only if user has retrieved pricing within the last";
      this.chkElapseTime.UseVisualStyleBackColor = true;
      this.chkElapseTime.CheckedChanged += new EventHandler(this.chkElapseTime_CheckedChanged);
      this.gcPPConfig.Borders = AnchorStyles.None;
      this.gcPPConfig.Controls.Add((Control) this.pnlProviderSettings);
      this.gcPPConfig.Controls.Add((Control) this.pnlProviders);
      this.gcPPConfig.Dock = DockStyle.Top;
      this.gcPPConfig.HeaderForeColor = SystemColors.ControlText;
      this.gcPPConfig.Location = new Point(0, 0);
      this.gcPPConfig.Margin = new Padding(4, 4, 4, 4);
      this.gcPPConfig.Name = "gcPPConfig";
      this.gcPPConfig.Size = new Size(277, 485);
      this.gcPPConfig.TabIndex = 1;
      this.gcPPConfig.Text = "Product and Pricing Provider Integration";
      this.pnlProviderSettings.Controls.Add((Control) this.grCustomizeName);
      this.pnlProviderSettings.Controls.Add((Control) this.pnlSettings);
      this.pnlProviderSettings.Dock = DockStyle.Top;
      this.pnlProviderSettings.Location = new Point(0, 93);
      this.pnlProviderSettings.Margin = new Padding(4, 4, 4, 4);
      this.pnlProviderSettings.Name = "pnlProviderSettings";
      this.pnlProviderSettings.Size = new Size(277, 386);
      this.pnlProviderSettings.TabIndex = 12;
      this.grCustomizeName.Borders = AnchorStyles.None;
      this.grCustomizeName.Controls.Add((Control) this.rdoCustomizeNameDepends);
      this.grCustomizeName.Controls.Add((Control) this.rdoCustomizeNameAlwaysLender);
      this.grCustomizeName.Controls.Add((Control) this.chkCustomizeName);
      this.grCustomizeName.Controls.Add((Control) this.rdoCustomizeNameAlwaysInvestor);
      this.grCustomizeName.Dock = DockStyle.Top;
      this.grCustomizeName.HeaderForeColor = SystemColors.ControlText;
      this.grCustomizeName.Location = new Point(0, 254);
      this.grCustomizeName.Margin = new Padding(4, 4, 4, 4);
      this.grCustomizeName.Name = "grCustomizeName";
      this.grCustomizeName.Size = new Size(277, 162);
      this.grCustomizeName.TabIndex = 11;
      this.grCustomizeName.Text = "Customize Investor Name Import";
      this.rdoCustomizeNameDepends.AutoSize = true;
      this.rdoCustomizeNameDepends.Location = new Point(37, 123);
      this.rdoCustomizeNameDepends.Margin = new Padding(4, 4, 4, 4);
      this.rdoCustomizeNameDepends.Name = "rdoCustomizeNameDepends";
      this.rdoCustomizeNameDepends.Size = new Size(555, 20);
      this.rdoCustomizeNameDepends.TabIndex = 12;
      this.rdoCustomizeNameDepends.TabStop = true;
      this.rdoCustomizeNameDepends.Text = "Save the investor name to the Encompass lender name field only for wholesale programs";
      this.rdoCustomizeNameDepends.UseVisualStyleBackColor = true;
      this.rdoCustomizeNameAlwaysLender.AutoSize = true;
      this.rdoCustomizeNameAlwaysLender.Location = new Point(37, 94);
      this.rdoCustomizeNameAlwaysLender.Margin = new Padding(4, 4, 4, 4);
      this.rdoCustomizeNameAlwaysLender.Name = "rdoCustomizeNameAlwaysLender";
      this.rdoCustomizeNameAlwaysLender.Size = new Size(428, 20);
      this.rdoCustomizeNameAlwaysLender.TabIndex = 11;
      this.rdoCustomizeNameAlwaysLender.TabStop = true;
      this.rdoCustomizeNameAlwaysLender.Text = "Always save the investor name to the Encompass lender name field";
      this.rdoCustomizeNameAlwaysLender.UseVisualStyleBackColor = true;
      this.chkCustomizeName.AutoSize = true;
      this.chkCustomizeName.Location = new Point(16, 38);
      this.chkCustomizeName.Margin = new Padding(4, 4, 4, 4);
      this.chkCustomizeName.Name = "chkCustomizeName";
      this.chkCustomizeName.Size = new Size(218, 20);
      this.chkCustomizeName.TabIndex = 9;
      this.chkCustomizeName.Text = "Customize investor name import";
      this.chkCustomizeName.UseVisualStyleBackColor = true;
      this.rdoCustomizeNameAlwaysInvestor.AutoSize = true;
      this.rdoCustomizeNameAlwaysInvestor.Location = new Point(37, 66);
      this.rdoCustomizeNameAlwaysInvestor.Margin = new Padding(4, 4, 4, 4);
      this.rdoCustomizeNameAlwaysInvestor.Name = "rdoCustomizeNameAlwaysInvestor";
      this.rdoCustomizeNameAlwaysInvestor.Size = new Size(437, 20);
      this.rdoCustomizeNameAlwaysInvestor.TabIndex = 10;
      this.rdoCustomizeNameAlwaysInvestor.TabStop = true;
      this.rdoCustomizeNameAlwaysInvestor.Text = "Always save the investor name to the Encompass investor name field";
      this.rdoCustomizeNameAlwaysInvestor.UseVisualStyleBackColor = true;
      this.pnlSettings.Controls.Add((Control) this.chkAllowPPESelection);
      this.pnlSettings.Controls.Add((Control) this.chkRequestLockWOLock);
      this.pnlSettings.Controls.Add((Control) this.checkBox1);
      this.pnlSettings.Controls.Add((Control) this.chkLockConfirm);
      this.pnlSettings.Controls.Add((Control) this.checkBox2);
      this.pnlSettings.Controls.Add((Control) this.chkRequestLock);
      this.pnlSettings.Controls.Add((Control) this.chkImportToLoanFile);
      this.pnlSettings.Dock = DockStyle.Top;
      this.pnlSettings.Location = new Point(0, 0);
      this.pnlSettings.Margin = new Padding(4, 4, 4, 4);
      this.pnlSettings.Name = "pnlSettings";
      this.pnlSettings.Size = new Size(277, 210);
      this.pnlSettings.TabIndex = 9;
      this.chkLockUpdateandLockConfirm.AutoSize = true;
      this.chkLockUpdateandLockConfirm.Enabled = false;
      this.chkLockUpdateandLockConfirm.Location = new Point(27, 119);
      this.chkLockUpdateandLockConfirm.Margin = new Padding(4, 4, 4, 4);
      this.chkLockUpdateandLockConfirm.Name = "chkLockUpdateandLockConfirm";
      this.chkLockUpdateandLockConfirm.Size = new Size(792, 20);
      this.chkLockUpdateandLockConfirm.TabIndex = 11;
      this.chkLockUpdateandLockConfirm.Text = "Request Lock Update and auto lock and confirm when lock request validation status returns 'No pricing change, loan still qualifies'";
      this.chkLockUpdateandLockConfirm.UseVisualStyleBackColor = true;
      this.chkAutoValidate.AutoSize = true;
      this.chkAutoValidate.Enabled = false;
      this.chkAutoValidate.Location = new Point(4, 96);
      this.chkAutoValidate.Margin = new Padding(4, 4, 4, 4);
      this.chkAutoValidate.Name = "chkAutoValidate";
      this.chkAutoValidate.Size = new Size(256, 20);
      this.chkAutoValidate.TabIndex = 10;
      this.chkAutoValidate.Text = "Auto Validate changes for active locks";
      this.chkAutoValidate.UseVisualStyleBackColor = true;
      this.chkRequestLockWOLock.AutoSize = true;
      this.chkRequestLockWOLock.Enabled = false;
      this.chkRequestLockWOLock.Location = new Point(29, 122);
      this.chkRequestLockWOLock.Margin = new Padding(4, 4, 4, 4);
      this.chkRequestLockWOLock.Name = "chkRequestLockWOLock";
      this.chkRequestLockWOLock.Size = new Size(263, 20);
      this.chkRequestLockWOLock.TabIndex = 9;
      this.chkRequestLockWOLock.Text = "Enforce only when no current lock exists";
      this.chkRequestLockWOLock.UseVisualStyleBackColor = true;
      this.checkBox1.AutoSize = true;
      this.checkBox1.Checked = true;
      this.checkBox1.CheckState = CheckState.Checked;
      this.checkBox1.Enabled = false;
      this.checkBox1.Location = new Point(4, 18);
      this.checkBox1.Margin = new Padding(4, 4, 4, 4);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new Size(579, 20);
      this.checkBox1.TabIndex = 4;
      this.checkBox1.Text = "Get Pricing (link to the selected provider to search for loan products, rates, and pricing details)";
      this.checkBox1.UseVisualStyleBackColor = true;
      this.chkLockConfirm.AutoSize = true;
      this.chkLockConfirm.Location = new Point(4, 150);
      this.chkLockConfirm.Margin = new Padding(4, 4, 4, 4);
      this.chkLockConfirm.Name = "chkLockConfirm";
      this.chkLockConfirm.Size = new Size(744, 20);
      this.chkLockConfirm.TabIndex = 8;
      this.chkLockConfirm.Text = "Lock and Confirm (link to the selected provider to confirm a lock request; generate a matching confirmation in Encompass)";
      this.chkLockConfirm.UseVisualStyleBackColor = true;
      this.checkBox2.AutoSize = true;
      this.checkBox2.Checked = true;
      this.checkBox2.CheckState = CheckState.Checked;
      this.checkBox2.Enabled = false;
      this.checkBox2.Location = new Point(27, 42);
      this.checkBox2.Margin = new Padding(4, 4, 4, 4);
      this.checkBox2.Name = "checkBox2";
      this.checkBox2.Size = new Size(232, 20);
      this.checkBox2.TabIndex = 5;
      this.checkBox2.Text = "Import Data to Lock Request Form";
      this.checkBox2.UseVisualStyleBackColor = true;
      this.chkRequestLock.AutoSize = true;
      this.chkRequestLock.Location = new Point(4, 98);
      this.chkRequestLock.Margin = new Padding(4, 4, 4, 4);
      this.chkRequestLock.Name = "chkRequestLock";
      this.chkRequestLock.Size = new Size(692, 20);
      this.chkRequestLock.TabIndex = 7;
      this.chkRequestLock.Text = "Request Lock (link to the selected provider to create a lock request; generate a matching request in Encompass)";
      this.chkRequestLock.UseVisualStyleBackColor = true;
      this.chkImportToLoanFile.AutoSize = true;
      this.chkImportToLoanFile.Location = new Point(27, 70);
      this.chkImportToLoanFile.Margin = new Padding(4, 4, 4, 4);
      this.chkImportToLoanFile.Name = "chkImportToLoanFile";
      this.chkImportToLoanFile.Size = new Size(170, 20);
      this.chkImportToLoanFile.TabIndex = 6;
      this.chkImportToLoanFile.Text = "Import Data to Loan File";
      this.chkImportToLoanFile.UseVisualStyleBackColor = true;
      this.pnlProviders.Controls.Add((Control) this.pnlExportUser);
      this.pnlProviders.Controls.Add((Control) this.progressSpinner);
      this.pnlProviders.Controls.Add((Control) this.label3);
      this.pnlProviders.Controls.Add((Control) this.cbProvider);
      this.pnlProviders.Controls.Add((Control) this.llAdmin);
      this.pnlProviders.Controls.Add((Control) this.label2);
      this.pnlProviders.Controls.Add((Control) this.llMoreInfo);
      this.pnlProviders.Controls.Add((Control) this.lblSvcMgmtBar);
      this.pnlProviders.Controls.Add((Control) this.llSvcMgmt);
      this.pnlProviders.Dock = DockStyle.Top;
      this.pnlProviders.Location = new Point(0, 25);
      this.pnlProviders.Margin = new Padding(4, 4, 4, 4);
      this.pnlProviders.Name = "pnlProviders";
      this.pnlProviders.Size = new Size(277, 68);
      this.pnlProviders.TabIndex = 11;
      this.pnlExportUser.Controls.Add((Control) this.label21);
      this.pnlExportUser.Controls.Add((Control) this.llExportUser);
      this.pnlExportUser.Location = new Point(633, 26);
      this.pnlExportUser.Margin = new Padding(4, 4, 4, 4);
      this.pnlExportUser.Name = "pnlExportUser";
      this.pnlExportUser.Size = new Size(115, 27);
      this.pnlExportUser.TabIndex = 16;
      this.pnlExportUser.Visible = false;
      this.label21.AutoSize = true;
      this.label21.Location = new Point(2, 6);
      this.label21.Margin = new Padding(4, 0, 4, 0);
      this.label21.Name = "label21";
      this.label21.Size = new Size(10, 16);
      this.label21.TabIndex = 17;
      this.label21.Text = "|";
      this.llExportUser.AutoSize = true;
      this.llExportUser.Location = new Point(12, 6);
      this.llExportUser.Margin = new Padding(4, 0, 4, 0);
      this.llExportUser.Name = "llExportUser";
      this.llExportUser.Size = new Size(84, 16);
      this.llExportUser.TabIndex = 16;
      this.llExportUser.TabStop = true;
      this.llExportUser.Text = "Export Users";
      this.llExportUser.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llExportUser_LinkClicked);
      this.progressSpinner.Image = (Image) componentResourceManager.GetObject("progressSpinner.Image");
      this.progressSpinner.Location = new Point(315, 34);
      this.progressSpinner.Margin = new Padding(4, 4, 4, 4);
      this.progressSpinner.Name = "progressSpinner";
      this.progressSpinner.Size = new Size(24, 24);
      this.progressSpinner.SizeMode = PictureBoxSizeMode.AutoSize;
      this.progressSpinner.TabIndex = 13;
      this.progressSpinner.TabStop = false;
      this.progressSpinner.Visible = false;
      this.progressSpinner.Click += new EventHandler(this.progressSpinner_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(7, 10);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(515, 16);
      this.label3.TabIndex = 10;
      this.label3.Text = "Select a provider. Note that some integration options are not supported by all vendors.";
      this.cbProvider.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbProvider.FormattingEnabled = true;
      this.cbProvider.Location = new Point(12, 34);
      this.cbProvider.Margin = new Padding(4, 4, 4, 4);
      this.cbProvider.Name = "cbProvider";
      this.cbProvider.Size = new Size(294, 24);
      this.cbProvider.TabIndex = 0;
      this.llAdmin.AutoSize = true;
      this.llAdmin.Location = new Point(352, 32);
      this.llAdmin.Margin = new Padding(4, 0, 4, 0);
      this.llAdmin.Name = "llAdmin";
      this.llAdmin.Size = new Size(45, 16);
      this.llAdmin.TabIndex = 1;
      this.llAdmin.TabStop = true;
      this.llAdmin.Text = "Admin";
      this.llAdmin.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llAdmin_LinkClicked);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(397, 32);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(10, 16);
      this.label2.TabIndex = 3;
      this.label2.Text = "|";
      this.llMoreInfo.AutoSize = true;
      this.llMoreInfo.Enabled = false;
      this.llMoreInfo.Location = new Point(407, 32);
      this.llMoreInfo.Margin = new Padding(4, 0, 4, 0);
      this.llMoreInfo.Name = "llMoreInfo";
      this.llMoreInfo.Size = new Size(62, 16);
      this.llMoreInfo.TabIndex = 2;
      this.llMoreInfo.TabStop = true;
      this.llMoreInfo.Text = "More Info";
      this.llMoreInfo.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llMoreInfo_LinkClicked);
      this.lblSvcMgmtBar.AutoSize = true;
      this.lblSvcMgmtBar.Location = new Point(475, 32);
      this.lblSvcMgmtBar.Margin = new Padding(4, 0, 4, 0);
      this.lblSvcMgmtBar.Name = "lblSvcMgmtBar";
      this.lblSvcMgmtBar.Size = new Size(10, 16);
      this.lblSvcMgmtBar.TabIndex = 3;
      this.lblSvcMgmtBar.Text = "|";
      this.lblSvcMgmtBar.Visible = false;
      this.llSvcMgmt.AutoSize = true;
      this.llSvcMgmt.Location = new Point(485, 32);
      this.llSvcMgmt.Margin = new Padding(4, 0, 4, 0);
      this.llSvcMgmt.Name = "llSvcMgmt";
      this.llSvcMgmt.Size = new Size(142, 16);
      this.llSvcMgmt.TabIndex = 2;
      this.llSvcMgmt.TabStop = true;
      this.llSvcMgmt.Text = "Services Management";
      this.llSvcMgmt.Visible = false;
      this.llSvcMgmt.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llSvcMgmt_LinkClicked);
      this.groupContainerLockVoid.Borders = AnchorStyles.Top;
      this.groupContainerLockVoid.Controls.Add((Control) this.chkEnableLockVoidWholesale);
      this.groupContainerLockVoid.Controls.Add((Control) this.chkEnableLockVoidRetail);
      this.groupContainerLockVoid.Controls.Add((Control) this.chkEnableLockVoid);
      this.groupContainerLockVoid.Dock = DockStyle.Top;
      this.groupContainerLockVoid.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerLockVoid.Location = new Point(0, 2007);
      this.groupContainerLockVoid.Margin = new Padding(4, 4, 4, 4);
      this.groupContainerLockVoid.Name = "groupContainerLockVoid";
      this.groupContainerLockVoid.Size = new Size(279, 131);
      this.groupContainerLockVoid.TabIndex = 10;
      this.groupContainerLockVoid.Text = "Lock Void";
      this.chkEnableLockVoidWholesale.AutoSize = true;
      this.chkEnableLockVoidWholesale.Location = new Point(9, 74);
      this.chkEnableLockVoidWholesale.Margin = new Padding(4, 4, 4, 4);
      this.chkEnableLockVoidWholesale.Name = "chkEnableLockVoidWholesale";
      this.chkEnableLockVoidWholesale.Size = new Size(393, 20);
      this.chkEnableLockVoidWholesale.TabIndex = 2;
      this.chkEnableLockVoidWholesale.Text = "Enable lock void in Secondary Registration Tool (Wholesale)";
      this.chkEnableLockVoidWholesale.UseVisualStyleBackColor = true;
      this.chkEnableLockVoidWholesale.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkEnableLockVoidRetail.AutoSize = true;
      this.chkEnableLockVoidRetail.Location = new Point(9, 46);
      this.chkEnableLockVoidRetail.Margin = new Padding(4, 4, 4, 4);
      this.chkEnableLockVoidRetail.Name = "chkEnableLockVoidRetail";
      this.chkEnableLockVoidRetail.Size = new Size(363, 20);
      this.chkEnableLockVoidRetail.TabIndex = 1;
      this.chkEnableLockVoidRetail.Text = "Enable lock void in Secondary Registration Tool (Retail)";
      this.chkEnableLockVoidRetail.UseVisualStyleBackColor = true;
      this.chkEnableLockVoidRetail.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkEnableLockVoid.AutoSize = true;
      this.chkEnableLockVoid.Location = new Point(10, 101);
      this.chkEnableLockVoid.Margin = new Padding(4, 4, 4, 4);
      this.chkEnableLockVoid.Name = "chkEnableLockVoid";
      this.chkEnableLockVoid.Size = new Size(578, 20);
      this.chkEnableLockVoid.TabIndex = 0;
      this.chkEnableLockVoid.Text = "Enable lock void in Secondary Registration Tool (Correspondent Individual Best-Efforts Only)";
      this.chkEnableLockVoid.UseVisualStyleBackColor = true;
      this.chkEnableLockVoid.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.groupContainerZeroBasedParPricing.Borders = AnchorStyles.Top;
      this.groupContainerZeroBasedParPricing.Controls.Add((Control) this.label14);
      this.groupContainerZeroBasedParPricing.Controls.Add((Control) this.chkZeroBasedParPricingRetail);
      this.groupContainerZeroBasedParPricing.Controls.Add((Control) this.chkZeroBasedParPricingWholesale);
      this.groupContainerZeroBasedParPricing.Dock = DockStyle.Top;
      this.groupContainerZeroBasedParPricing.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerZeroBasedParPricing.Location = new Point(0, 2138);
      this.groupContainerZeroBasedParPricing.Margin = new Padding(4, 4, 4, 4);
      this.groupContainerZeroBasedParPricing.Name = "groupContainerZeroBasedParPricing";
      this.groupContainerZeroBasedParPricing.Size = new Size(279, 131);
      this.groupContainerZeroBasedParPricing.TabIndex = 11;
      this.groupContainerZeroBasedParPricing.Text = "Zero Based Par Pricing";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(6, 45);
      this.label14.Margin = new Padding(4, 0, 4, 0);
      this.label14.Name = "label14";
      this.label14.Size = new Size(551, 16);
      this.label14.TabIndex = 11;
      this.label14.Text = "Configure Par Pricing for Lock Request Form and Secondary Lock Tool (Par Pricing is 0.000)";
      this.chkZeroBasedParPricingRetail.AutoSize = true;
      this.chkZeroBasedParPricingRetail.Location = new Point(9, 73);
      this.chkZeroBasedParPricingRetail.Margin = new Padding(4, 4, 4, 4);
      this.chkZeroBasedParPricingRetail.Name = "chkZeroBasedParPricingRetail";
      this.chkZeroBasedParPricingRetail.Size = new Size(224, 20);
      this.chkZeroBasedParPricingRetail.TabIndex = 0;
      this.chkZeroBasedParPricingRetail.Text = "Zero Based Par Pricing for Retail";
      this.chkZeroBasedParPricingRetail.UseVisualStyleBackColor = true;
      this.chkZeroBasedParPricingWholesale.AutoSize = true;
      this.chkZeroBasedParPricingWholesale.Location = new Point(9, 100);
      this.chkZeroBasedParPricingWholesale.Margin = new Padding(4, 4, 4, 4);
      this.chkZeroBasedParPricingWholesale.Name = "chkZeroBasedParPricingWholesale";
      this.chkZeroBasedParPricingWholesale.Size = new Size(254, 20);
      this.chkZeroBasedParPricingWholesale.TabIndex = 1;
      this.chkZeroBasedParPricingWholesale.Text = "Zero Based Par Pricing for Wholesale";
      this.chkZeroBasedParPricingWholesale.UseVisualStyleBackColor = true;
      this.chkAllowPPESelection.AutoSize = true;
      this.chkAllowPPESelection.Location = new Point(4, 178);
      this.chkAllowPPESelection.Margin = new Padding(4);
      this.chkAllowPPESelection.Name = "chkAllowPPESelection";
      this.chkAllowPPESelection.Size = new Size(598, 20);
      this.chkAllowPPESelection.TabIndex = 12;
      this.chkAllowPPESelection.Text = "Allow Product and Pricing Provider selection from Lock Request Form and Secondary Lock Tool";
      this.chkAllowPPESelection.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.groupContainerZeroBasedParPricing);
      this.Controls.Add((Control) this.groupContainerLockVoid);
      this.Controls.Add((Control) this.borderPanel2);
      this.Margin = new Padding(4, 4, 4, 4);
      this.Name = nameof (ProductPricingAdmin);
      this.Size = new Size(279, 1747);
      ((ISupportInitialize) this.siBtnEdit).EndInit();
      ((ISupportInitialize) this.siBtnDelete).EndInit();
      ((ISupportInitialize) this.siBtnAdd).EndInit();
      ((ISupportInitialize) this.btnEditAdjustPerLockExt).EndInit();
      ((ISupportInitialize) this.btnDeleteAdjustPerLockExt).EndInit();
      ((ISupportInitialize) this.btnAddAdjustPerLockExt).EndInit();
      this.borderPanel2.ResumeLayout(false);
      this.borderPanel2.PerformLayout();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.pnlGlobalSettings.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.gcLockCancellation.ResumeLayout(false);
      this.gcLockCancellation.PerformLayout();
      this.pnlLockextensionConfig.ResumeLayout(false);
      this.pnlLockextensionConfig.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.pnlAdjustPerLockExt.ResumeLayout(false);
      this.pnlAdjustPerLockExt.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.gcLockRequestType.ResumeLayout(false);
      this.gcLockRequestType.PerformLayout();
      this.pnlForReLocks.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.pnlLockUpdates.ResumeLayout(false);
      this.pnlLockUpdates.PerformLayout();
      this.gcElapsedTime.ResumeLayout(false);
      this.gcElapsedTime.PerformLayout();
      this.gcPPConfig.ResumeLayout(false);
      this.pnlProviderSettings.ResumeLayout(false);
      this.grCustomizeName.ResumeLayout(false);
      this.grCustomizeName.PerformLayout();
      this.pnlSettings.ResumeLayout(false);
      this.pnlSettings.PerformLayout();
      this.pnlProviders.ResumeLayout(false);
      this.pnlProviders.PerformLayout();
      this.pnlExportUser.ResumeLayout(false);
      this.pnlExportUser.PerformLayout();
      ((ISupportInitialize) this.progressSpinner).EndInit();
      this.groupContainerLockVoid.ResumeLayout(false);
      this.groupContainerLockVoid.PerformLayout();
      this.groupContainerZeroBasedParPricing.ResumeLayout(false);
      this.groupContainerZeroBasedParPricing.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

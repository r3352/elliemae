// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ProductPricingLightAdmin
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Export;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ProductPricingLightAdmin : Form
  {
    private PPEservice serviceProvider;
    public List<ProductPricingSetting> settings = new List<ProductPricingSetting>();
    private List<ProductPricingSetting> displayList = new List<ProductPricingSetting>();
    private Sessions.Session session;
    public ServiceSetupEvaluatorResponse svcSetupResponse;
    private const string className = "ProductPricingLightAdmin";
    private static string sw = Tracing.SwOutsideLoan;
    private string errorDetails = string.Empty;
    private string stackTraceDetails = string.Empty;
    private bool isReadOnly;
    private IContainer components;
    private ComboBox cbProvider;
    private Label label1;
    private DialogButtons dialogButtons1;
    private PictureBox progressSpinner;

    public ProductPricingLightAdmin(Sessions.Session session, bool isReadOnly = false)
    {
      this.isReadOnly = isReadOnly;
      this.InitializeComponent();
      this.adjustPositionAndLabel();
      this.session = session;
      this.serviceProvider = new PPEservice(session?.SessionObjects?.StartupInfo?.ServiceUrls?.PPEServiceUrl);
      this.reSyncServerSettings();
      if (this.session.UserInfo.IsTopLevelAdministrator() || this.isReadOnly)
        return;
      this.dialogButtons1.OKButton.Enabled = false;
    }

    private void adjustPositionAndLabel()
    {
      if (!this.isReadOnly)
        return;
      this.ClientSize = new Size(384, 114);
      this.label1.Size = new Size(335, 16);
      this.cbProvider.Location = new Point(16, 36);
      this.progressSpinner.Location = new Point(348, 36);
      this.dialogButtons1.OKButton.Text = "Ok";
      this.label1.Text = "Provider";
      this.Text = "Select a Pricing Provider";
      this.dialogButtons1.CancelButton.Visible = false;
    }

    private async void reSyncServerSettings()
    {
      ProductPricingLightAdmin pricingLightAdmin1 = this;
      Partner[] partnerList = (Partner[]) null;
      List<Epc2Provider> temoproviderList = (List<Epc2Provider>) null;
      try
      {
        ProductPricingLightAdmin pricingLightAdmin = pricingLightAdmin1;
        string accessToken = new Bam(pricingLightAdmin1.session.LoanData).GetAccessToken("sc");
        Task<List<Epc2Provider>> ecpGetProductTask = Epc2ServiceClient.GetProviderList(pricingLightAdmin1.session.SessionObjects, accessToken, new string[1]
        {
          "PRODUCTPRICING"
        }, "Encompass Smart Client");
        Task<ServiceSetupEvaluatorResponse> ecpServiceSetupTask = Epc2ServiceClient.GetServiceSetupEvaluatorResponse(pricingLightAdmin1.session.SessionObjects, accessToken, pricingLightAdmin1.session.LoanData.GUID, "");
        string clientId = pricingLightAdmin1.session.CompanyInfo.ClientID;
        string instanceName = pricingLightAdmin1.session?.ServerIdentity?.InstanceName;
        if (!string.IsNullOrWhiteSpace(instanceName) && (instanceName.StartsWith("TEBE", StringComparison.InvariantCultureIgnoreCase) || instanceName.StartsWith("DEBE", StringComparison.InvariantCultureIgnoreCase)))
          clientId = instanceName;
        Task<Partner[]> emnGetProductTask = Task.Run<Partner[]>((Func<Partner[]>) (() => pricingLightAdmin.serviceProvider.GetPartners(clientId)));
        pricingLightAdmin1.progressSpinner.Visible = true;
        await Task.WhenAll((Task) ecpGetProductTask, (Task) emnGetProductTask, (Task) ecpServiceSetupTask);
        partnerList = emnGetProductTask.Result;
        temoproviderList = ecpGetProductTask.Result;
        pricingLightAdmin1.svcSetupResponse = ecpServiceSetupTask.Result;
        pricingLightAdmin1.progressSpinner.Visible = false;
        ecpGetProductTask = (Task<List<Epc2Provider>>) null;
        ecpServiceSetupTask = (Task<ServiceSetupEvaluatorResponse>) null;
        emnGetProductTask = (Task<Partner[]>) null;
      }
      catch (Exception ex)
      {
        pricingLightAdmin1.progressSpinner.Image = (Image) new Bitmap((Image) SystemIcons.Error.ToBitmap(), new Size(18, 18));
        Tracing.Log(ProductPricingLightAdmin.sw, TraceLevel.Error, nameof (ProductPricingLightAdmin), string.Format("GetPartners Exception (Message {0})", (object) ex.Message));
        pricingLightAdmin1.errorDetails = ex.Message;
        pricingLightAdmin1.stackTraceDetails = ex.StackTrace;
      }
      List<ProductPricingSetting> productPricingSettingList = new List<ProductPricingSetting>();
      string companySetting = pricingLightAdmin1.session.ConfigurationManager.GetCompanySetting("POLICIES", "PRICING_PARTNER");
      pricingLightAdmin1.session.ConfigurationManager.SetCompanySetting("POLICIES", "PRICING_PARTNER", "");
      pricingLightAdmin1.session.ConfigurationManager.SetCompanySetting("POLICIES", "PRICING_PARTNER_SELL_SIDE_SHOW", "");
      bool requireUpdate = false;
      pricingLightAdmin1.settings = ProductPricingUtils.MergeProviders(pricingLightAdmin1.session, partnerList, temoproviderList, "", companySetting, ref requireUpdate);
      foreach (ProductPricingSetting setting1 in pricingLightAdmin1.settings)
      {
        ProductPricingSetting setting = setting1;
        if (setting != null)
        {
          if (setting.VendorPlatform == VendorPlatform.EPC2)
          {
            if (pricingLightAdmin1.svcSetupResponse != null && pricingLightAdmin1.svcSetupResponse.MatchingResults.Where<MatchingResult>((Func<MatchingResult, bool>) (x => x.ServiceSetup.ProviderId == setting.ProviderID)).FirstOrDefault<MatchingResult>() != null)
              pricingLightAdmin1.displayList.Add(setting);
          }
          else if (setting.Active)
            pricingLightAdmin1.displayList.Add(setting);
        }
      }
      pricingLightAdmin1.initialPageValue();
    }

    private void initialPageValue()
    {
      this.cbProvider.Items.Clear();
      if (this.isReadOnly)
      {
        this.displayList.Insert(0, new ProductPricingSetting()
        {
          PartnerName = "Select Provider"
        });
        this.dialogButtons1.OKButton.Enabled = true;
      }
      this.cbProvider.DataSource = (object) this.displayList;
      this.cbProvider.DisplayMember = "partnerName";
      if (this.cbProvider.Items.Count > 0)
        this.cbProvider.SelectedIndex = 0;
      else
        this.dialogButtons1.OKButton.Enabled = false;
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      ProductPricingSetting selectedItem = (ProductPricingSetting) this.cbProvider.SelectedItem;
      foreach (ProductPricingSetting setting in this.settings)
        setting.Active = !(setting.ProviderID != selectedItem.ProviderID);
      if (!this.isReadOnly)
        this.settings = this.session.ConfigurationManager.UpdateProductPricingSettings(this.settings);
      this.DialogResult = DialogResult.OK;
    }

    private void dialogButtons1_Cancel(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void progressSpinner_Click(object sender, EventArgs e)
    {
      int num = (int) new DiagnosticDetailForm(this.errorDetails, this.stackTraceDetails).ShowDialog();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProductPricingLightAdmin));
      this.cbProvider = new ComboBox();
      this.label1 = new Label();
      this.dialogButtons1 = new DialogButtons();
      this.progressSpinner = new PictureBox();
      ((ISupportInitialize) this.progressSpinner).BeginInit();
      this.SuspendLayout();
      this.cbProvider.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbProvider.FormattingEnabled = true;
      this.cbProvider.Location = new Point(16, 62);
      this.cbProvider.Name = "cbProvider";
      this.cbProvider.Size = new Size(320, 21);
      this.cbProvider.TabIndex = 1;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(335, 46);
      this.label1.TabIndex = 2;
      this.label1.Text = "You have not setup a company provider yet. Select a company provider and save. For additional setup, go to Product and Pricing in Secondary Setup in Settings. ";
      this.dialogButtons1.DialogResult = DialogResult.Cancel;
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 94);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.OKText = "&Save";
      this.dialogButtons1.Size = new Size(384, 44);
      this.dialogButtons1.TabIndex = 1;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.dialogButtons1.Cancel += new EventHandler(this.dialogButtons1_Cancel);
      this.dialogButtons1.CancelButton.Visible = true;
      this.progressSpinner.Image = (Image) componentResourceManager.GetObject("progressSpinner.Image");
      this.progressSpinner.Location = new Point(348, 62);
      this.progressSpinner.Name = "progressSpinner";
      this.progressSpinner.Size = new Size(24, 24);
      this.progressSpinner.SizeMode = PictureBoxSizeMode.AutoSize;
      this.progressSpinner.TabIndex = 12;
      this.progressSpinner.TabStop = false;
      this.progressSpinner.Visible = false;
      this.progressSpinner.Click += new EventHandler(this.progressSpinner_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(384, 138);
      this.Controls.Add((Control) this.progressSpinner);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cbProvider);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ProductPricingLightAdmin);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Product and Pricing";
      ((ISupportInitialize) this.progressSpinner).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

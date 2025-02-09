// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.EncompassAIQSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientCommon.AIQCapsilon;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.CapsilonAIQ;
using EllieMae.EMLite.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class EncompassAIQSetupControl : SettingsUserControl
  {
    private TabPage[] dynamicTabPages = new TabPage[4];
    private bool isAIQLicenseExists;
    private Sessions.Session session;
    private const string className = "EncompassAIQSetupControl";
    private static readonly string sw = Tracing.SwEFolder;
    private const string scope = "sc";
    private static HttpClient client = new HttpClient();
    private AIQDecryptor decryptor = new AIQDecryptor();
    private AIQServerClient AIQAuthentication;
    private IContainer components;
    private StandardIconButton btnSave;
    private GroupContainer gcDays;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private Label label2;
    private TextBox txtAPIClientSecret;
    private Label lblAPIClientSecret;
    private TextBox txtAPIClientID;
    private Label lblAPIClientID;
    private TextBox txtDevConnectAPIUserID;
    private Label lblDevConnectAPIUserID;
    private TextBox txtEncompassClientInstance;
    private Label lblEncompassClientInstance;
    private TextBox txtEncompassClientID;
    private Label lblEncompassClientID;
    private Label lblDevConnectAccess;
    private Label label3;
    private TextBox txtAIQSiteAddress;
    private Label lblAIQSiteAddress;
    private StandardIconButton btnEdit;
    private Button btnVerifyAddress;
    private Label lblVerifyAddress;
    private ImageList imgListTv;

    private Dictionary<string, string> GetAPIConfigDetails()
    {
      return new Dictionary<string, string>()
      {
        {
          "LaunchURL_Desktop",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "LaunchURL_Desktop")
        },
        {
          "LaunchURl_WEB",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "LaunchURl_WEB")
        },
        {
          "AppName",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.AppName")
        },
        {
          "AppSecret",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.AppSecret")
        },
        {
          "AIQcredential",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.credential")
        },
        {
          "AIQ.ApplicationDataMap",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.ApplicationDataMap")
        },
        {
          "AIQ.ApplicationFlags",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.ApplicationFlags")
        },
        {
          "AIQ.GetSiteID.URL",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.GetSiteID.URL")
        },
        {
          "AIQ.LOS.URL",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.LOS.URL")
        },
        {
          "secretKeyId",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "secretKeyId")
        },
        {
          "secretKey",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "secretKey")
        },
        {
          "regionName",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "regionName")
        },
        {
          "serviceName_LOS",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "serviceName_LOS")
        },
        {
          "serviceName_SiteID",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "serviceName_SiteID")
        },
        {
          "TERMINATOR",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "TERMINATOR")
        }
      };
    }

    public EncompassAIQSetupControl(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.session = session;
      this.AIQAuthentication = new AIQServerClient(this.GetAPIConfigDetails());
      this.InitializeComponent();
      this.txtAIQSiteAddress.Enabled = true;
      this.SetControlState(false);
      this.SetAIQDetails();
    }

    private void SetAIQDetails()
    {
      string empty = string.Empty;
      string str1 = string.Empty;
      try
      {
        string str2 = this.CheckAIQLicense(ref this.isAIQLicenseExists);
        if (!this.isAIQLicenseExists)
          return;
        if (!string.IsNullOrWhiteSpace(this.session.ConfigurationManager.GetCompanySetting("AIQSetup", "AIQAddress")))
        {
          this.txtAIQSiteAddress.Text = this.session.ConfigurationManager.GetCompanySetting("AIQSetup", "AIQAddress");
          this.txtEncompassClientID.Text = this.session.ConfigurationManager.GetCompanySetting("AIQSetup", "EncompassClientID");
          this.txtEncompassClientInstance.Text = this.session.ConfigurationManager.GetCompanySetting("AIQSetup", "EncompassInstanceID");
          this.txtDevConnectAPIUserID.Text = this.session.ConfigurationManager.GetCompanySetting("AIQSetup", "APIUserID");
          this.txtAPIClientID.Text = this.session.ConfigurationManager.GetCompanySetting("AIQSetup", "APIClientID");
          string companySetting = this.session.ConfigurationManager.GetCompanySetting("AIQSetup", "APIClientSecret");
          if (!string.IsNullOrWhiteSpace(companySetting))
            str1 = this.decryptor.RsaDecryptWithPrivate(companySetting);
          this.txtAPIClientSecret.Text = str1;
        }
        else
        {
          this.InitDynamicTabs();
          this.SetControlState(true);
          this.SetControlFont((Control) this.txtAIQSiteAddress, 1);
          this.txtAIQSiteAddress.Text = str2;
        }
      }
      catch (FormatException ex)
      {
        Tracing.Log(EncompassAIQSetupControl.sw, TraceLevel.Error, nameof (EncompassAIQSetupControl), string.Format("Error loading Data & Document Automation and Mortgage Analyzers details. Exception: {0}", (object) ex.Message));
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading Data & Document Automation and Mortgage Analyzers details.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        Tracing.Log(EncompassAIQSetupControl.sw, TraceLevel.Error, nameof (EncompassAIQSetupControl), string.Format("Error loading Data & Document Automation and Mortgage Analyzers details. Exception: {0}", (object) ex.Message));
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading Data & Document Automation and Mortgage Analyzers details.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    public void InitDynamicTabs()
    {
      foreach (Control control in (ArrangedElementCollection) this.tabControl1.SelectedTab.Controls)
      {
        if (control.GetType().Name.ToString() == "TextBox" && control.Name != "txtAIQSiteAddress")
          this.SetControlFont(control, 0);
      }
      this.txtEncompassClientID.Text = "Encompass Client ID";
      this.txtAIQSiteAddress.Text = "https://doc-mgmt.com";
      this.txtEncompassClientInstance.Text = "Encompass Instance ID";
      this.txtDevConnectAPIUserID.Text = "Connect API User ID";
      this.txtAPIClientID.Text = "API Client ID";
      this.txtAPIClientSecret.Text = "API Client Secret";
    }

    private void SetControlFont(Control c, int flag)
    {
      if (flag == 0)
      {
        c.ForeColor = SystemColors.GrayText;
        c.Font = new Font(this.Font, FontStyle.Italic);
      }
      else
      {
        c.Text = "";
        c.ForeColor = SystemColors.WindowText;
        c.Font = new Font(this.Font, FontStyle.Regular);
      }
    }

    private bool validateData()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.txtAIQSiteAddress.Text.Trim() == "" || this.txtAIQSiteAddress.Text.Trim() == "https://doc-mgmt.com")
        stringBuilder.Append("{field} is required.".Replace("{field}", "Data & Document Automation and Mortgage Analyzers Address") + "\n");
      if (this.txtEncompassClientID.Text.Trim() == "" || this.txtEncompassClientID.Text.Trim() == "Encompass Client ID")
        stringBuilder.Append("{field} is required.".Replace("{field}", "Encompass Client ID") + "\n");
      if (this.txtEncompassClientInstance.Text.Trim() == "" || this.txtEncompassClientInstance.Text.Trim() == "Encompass Instance ID")
        stringBuilder.Append("{field} is required.".Replace("{field}", "Encompass Instance ID") + "\n");
      if (this.txtDevConnectAPIUserID.Text.Trim() == "" || this.txtDevConnectAPIUserID.Text.Trim() == "Connect API User ID")
        stringBuilder.Append("{field} is required.".Replace("{field}", "Connect API User ID") + "\n");
      if (this.txtAPIClientID.Text.Trim() == "" || this.txtAPIClientID.Text.Trim() == "API Client ID")
        stringBuilder.Append("{field} is required.".Replace("{field}", "API Client ID") + "\n");
      if (this.txtAPIClientSecret.Text.Trim() == "" || this.txtAPIClientSecret.Text.Trim() == "API Client Secret")
        stringBuilder.Append("{field} is required.".Replace("{field}", "API Client Secret") + "\n");
      if (stringBuilder.Length <= 0)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private bool validateURL()
    {
      if (new Regex("^((((H|h)(T|t)|(F|f))(T|t)(P|p)((S|s)?))\\://)?(www.|[a-zA-Z0-9].)[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z]{2,6}(\\:[0-9]{1,5})*(/($|[a-zA-Z0-9\\.\\,\\;\\?\\'\\\\\\+&amp;%\\$#\\=~_\\-]+))*$").IsMatch(this.txtAIQSiteAddress.Text))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "      Data & Document Automation and Mortgage Analyzers Site Address is incorrect.Please enter a valid address.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void tabPage1_Click(object sender, EventArgs e)
    {
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.validateData() || !this.SaveToDatabase())
        return;
      this.SetControlState(false);
      this.btnSave.Enabled = false;
      this.btnEdit.Enabled = true;
      this.lblVerifyAddress.Text = string.Empty;
      this.lblVerifyAddress.ImageIndex = 0;
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "You are attempting to edit Data & Document Automation and Mortgage Analyzers Connectivity Settings. This could result in Data & Document Automation and Mortgage Analyzers Integration not working.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      this.SetControlState(true);
      this.btnSave.Enabled = true;
      this.btnEdit.Enabled = false;
    }

    private void SetControlState(bool flag)
    {
      this.txtAPIClientID.Enabled = flag;
      this.txtAPIClientSecret.Enabled = flag;
      this.txtDevConnectAPIUserID.Enabled = flag;
      this.txtEncompassClientID.Enabled = flag;
      this.txtEncompassClientInstance.Enabled = flag;
      this.btnSave.Enabled = flag;
    }

    private void ResetControl()
    {
      this.txtAPIClientID.Text = string.Empty;
      this.txtAPIClientSecret.Text = string.Empty;
      this.txtDevConnectAPIUserID.Text = string.Empty;
      this.txtEncompassClientID.Text = string.Empty;
      this.txtEncompassClientInstance.Text = string.Empty;
    }

    private string CheckAIQLicense(ref bool isLicenseExists)
    {
      isLicenseExists = true;
      return "https://www.demo.com";
    }

    private bool SaveToDatabase()
    {
      bool database = false;
      string empty = string.Empty;
      AIQServerClient.EncompassConfigRequest encompassConfigRequest = new AIQServerClient.EncompassConfigRequest();
      AIQServerClient.EncompassConnConfig encompassConnConfig = new AIQServerClient.EncompassConnConfig();
      try
      {
        string companySetting = this.session.ConfigurationManager.GetCompanySetting("AIQSetup", "SiteID");
        encompassConfigRequest.clientId = this.txtEncompassClientID.Text;
        encompassConfigRequest.instanceId = this.txtEncompassClientInstance.Text;
        encompassConfigRequest.devConnectApiUserId = this.txtDevConnectAPIUserID.Text;
        encompassConfigRequest.apiClientId = this.txtAPIClientID.Text;
        encompassConfigRequest.apiClientSecret = this.decryptor.RsaEncryptWithPrivate(this.txtAPIClientSecret.Text);
        encompassConfigRequest.environment = this.session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.Environment");
        var data = new
        {
          encompassConfigurationRequest = encompassConfigRequest
        };
        Task<AIQServerClient.EncompassConnConfig> task = this.AIQAuthentication.UpdateAIQEncompassInfo(companySetting, JsonConvert.SerializeObject((object) data));
        Task.WaitAll((Task) task);
        AIQServerClient.EncompassConnConfig result = task.Result;
        if (!(result.status.ToLower() == "success"))
          throw new Exception(result.errorMessage);
        if (result.siteId != string.Empty)
        {
          this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "SiteID", result.siteId);
          this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "EncompassClientID", result.clientId);
          this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "EncompassInstanceID", result.instanceId);
          this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "APIUserID", result.devConnectApiUserId);
          this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "APIClientID", result.apiClientId);
          this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "APIClientSecret", result.apiClientSecret);
        }
        database = true;
      }
      catch (FormatException ex)
      {
        Tracing.Log(EncompassAIQSetupControl.sw, TraceLevel.Error, nameof (EncompassAIQSetupControl), string.Format("Error saving Data & Document Automation detail. Exception: {0}", (object) ex.Message));
        int num = (int) Utils.Dialog((IWin32Window) this, "Encompass ran into issues configuring Data & Document Automation and Mortgage Analyzers. Please retry. If error persists, please contact Customer Support", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        Tracing.Log(EncompassAIQSetupControl.sw, TraceLevel.Error, nameof (EncompassAIQSetupControl), string.Format("Error saving Data & Document Automation details. Exception: {0}", (object) ex.Message));
        int num = (int) Utils.Dialog((IWin32Window) this, "Encompass ran into issues configuring Data & Document Automation and Mortgage Analyzers. Please retry. If error persists, please contact Customer Support", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return database;
    }

    private void VerifyAddressStatus(string siteAddress)
    {
      string empty = string.Empty;
      try
      {
        Task<AIQServerClient.SiteVerifyOutput> task = this.AIQAuthentication.VerifySiteAddress(siteAddress);
        Task.WaitAll((Task) task);
        if (!(task.Result.status.ToLower() == "success") || string.IsNullOrWhiteSpace(task.Result.deploymentStatus))
          throw new Exception(task.Result.errorMessage);
        switch (task.Result.deploymentStatus)
        {
          case "NOT_VERIFIED":
            this.lblVerifyAddress.Text = "      Data & Document Automation and Mortgage Analyzers Site could not be verified. Please contact Customer Support.";
            this.lblVerifyAddress.ImageIndex = 1;
            this.ClearDBInfo(task.Result.siteId);
            this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "AIQAddress", this.txtAIQSiteAddress.Text);
            this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "SiteID", task.Result.siteId);
            this.SetControlState(true);
            break;
          case "DEPLOYED":
            this.lblVerifyAddress.Text = "      Data & Document Automation and Mortgage Analyzers site is verified";
            this.lblVerifyAddress.ImageIndex = 0;
            this.ClearDBInfo(task.Result.siteId);
            this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "AIQAddress", this.txtAIQSiteAddress.Text);
            this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "SiteID", task.Result.siteId);
            this.SetControlState(true);
            break;
          case "AIQ_SITE_REQUESTED":
            this.lblVerifyAddress.Text = "      Data & Document Automation and Mortgage Analyzers Site could not be verified. Please contact Customer Support.";
            this.lblVerifyAddress.ImageIndex = 1;
            this.ClearDBInfo(task.Result.siteId);
            this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "AIQAddress", this.txtAIQSiteAddress.Text);
            this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "SiteID", task.Result.siteId);
            this.SetControlState(true);
            break;
          case "AIQ_SITE_NOT_REQUESTED":
            this.lblVerifyAddress.Text = "      Data & Document Automation and Mortgage Analyzers Site could not be verified. Please contact Customer Support.";
            this.lblVerifyAddress.ImageIndex = 1;
            this.ClearDBInfo(task.Result.siteId);
            this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "AIQAddress", this.txtAIQSiteAddress.Text);
            this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "SiteID", task.Result.siteId);
            this.SetControlState(true);
            break;
          case "AIQ_SITE_NOT_CONFIGURED":
            this.lblVerifyAddress.Text = "      Data & Document Automation and Mortgage Analyzers Site is not configured. Please contact Customer Support.";
            this.lblVerifyAddress.ImageIndex = 1;
            this.ClearDBInfo(task.Result.siteId);
            this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "AIQAddress", this.txtAIQSiteAddress.Text);
            this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "SiteID", task.Result.siteId);
            this.SetControlState(true);
            break;
          case "AIQ_SITE_NOT_FOUND":
            this.lblVerifyAddress.Text = "      Data & Document Automation and Mortgage Analyzers Site could not be found. Please contact Customer Support.";
            this.lblVerifyAddress.ImageIndex = 1;
            this.SetControlState(false);
            break;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(EncompassAIQSetupControl.sw, TraceLevel.Error, nameof (EncompassAIQSetupControl), string.Format("Error verify Data & Document Automation and Mortgage Analyzers site address. Exception: {0}", (object) ex.Message));
        int num = (int) Utils.Dialog((IWin32Window) this, "Something went wrong. Please retry. If error persists, contact Customer Support.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void btnVerifyAddress_Click(object sender, EventArgs e)
    {
      string text = this.txtAIQSiteAddress.Text;
      if (this.txtAIQSiteAddress.Text.Trim() == "" || this.txtAIQSiteAddress.Text.Trim() == "https://doc-mgmt.com")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "{field} is required.".Replace("{field}", "Data & Document Automation and Mortgage Analyzers Address"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.VerifyAddressStatus(text);
    }

    private void ClearDBInfo(string siteIdIn)
    {
      string empty = string.Empty;
      string companySetting = this.session.ConfigurationManager.GetCompanySetting("AIQSetup", "SiteID");
      if (!(companySetting != string.Empty) || !(companySetting != siteIdIn))
        return;
      this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "EncompassClientID", string.Empty);
      this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "EncompassInstanceID", string.Empty);
      this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "APIUserID", string.Empty);
      this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "APIClientID", string.Empty);
      this.session.ConfigurationManager.SetCompanySetting("AIQSetup", "APIClientSecret", string.Empty);
      this.ResetControl();
    }

    private void txtEncompassClientID_Enter(object sender, EventArgs e)
    {
      if (!(this.txtEncompassClientID.Text == "Encompass Client ID"))
        return;
      this.SetControlFont((Control) this.txtEncompassClientID, 1);
    }

    private void txtEncompassClientID_Leave(object sender, EventArgs e)
    {
      if (this.txtEncompassClientID.Text.Length != 0)
        return;
      this.txtEncompassClientID.Text = "Encompass Client ID";
      this.SetControlFont((Control) this.txtEncompassClientID, 0);
    }

    private void txtEncompassClientInstance_Enter(object sender, EventArgs e)
    {
      if (!(this.txtEncompassClientInstance.Text == "Encompass Instance ID"))
        return;
      this.SetControlFont((Control) this.txtEncompassClientInstance, 1);
    }

    private void txtEncompassClientInstance_Leave(object sender, EventArgs e)
    {
      if (this.txtEncompassClientInstance.Text.Length != 0)
        return;
      this.txtEncompassClientInstance.Text = "Encompass Instance ID";
      this.SetControlFont((Control) this.txtEncompassClientInstance, 0);
    }

    private void txtAIQSiteAddress_Leave(object sender, EventArgs e)
    {
      if (this.txtAIQSiteAddress.Text.Length != 0)
        return;
      this.txtAIQSiteAddress.Text = "https://doc-mgmt.com";
      this.SetControlFont((Control) this.txtAIQSiteAddress, 0);
    }

    private void txtAIQSiteAddress_Enter(object sender, EventArgs e)
    {
      if (!(this.txtAIQSiteAddress.Text == "https://doc-mgmt.com"))
        return;
      this.SetControlFont((Control) this.txtAIQSiteAddress, 1);
    }

    private void txtDevConnectAPIUserID_Enter(object sender, EventArgs e)
    {
      if (!(this.txtDevConnectAPIUserID.Text == "Connect API User ID"))
        return;
      this.SetControlFont((Control) this.txtDevConnectAPIUserID, 1);
    }

    private void txtDevConnectAPIUserID_Leave(object sender, EventArgs e)
    {
      if (this.txtDevConnectAPIUserID.Text.Length != 0)
        return;
      this.txtDevConnectAPIUserID.Text = "Connect API User ID";
      this.SetControlFont((Control) this.txtDevConnectAPIUserID, 0);
    }

    private void txtAPIClientID_Enter(object sender, EventArgs e)
    {
      if (!(this.txtAPIClientID.Text == "API Client ID"))
        return;
      this.SetControlFont((Control) this.txtAPIClientID, 1);
    }

    private void txtAPIClientID_Leave(object sender, EventArgs e)
    {
      if (this.txtAPIClientID.Text.Length != 0)
        return;
      this.txtAPIClientID.Text = "API Client ID";
      this.SetControlFont((Control) this.txtAPIClientID, 0);
    }

    private void txtAPIClientSecret_Enter(object sender, EventArgs e)
    {
      if (!(this.txtAPIClientSecret.Text == "API Client Secret"))
        return;
      this.SetControlFont((Control) this.txtAPIClientSecret, 1);
    }

    private void txtAPIClientSecret_Leave(object sender, EventArgs e)
    {
      if (this.txtAPIClientSecret.Text.Length != 0)
        return;
      this.txtAPIClientSecret.Text = "API Client Secret";
      this.SetControlFont((Control) this.txtAPIClientSecret, 0);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EncompassAIQSetupControl));
      this.gcDays = new GroupContainer();
      this.btnEdit = new StandardIconButton();
      this.tabControl1 = new TabControl();
      this.tabPage1 = new TabPage();
      this.lblVerifyAddress = new Label();
      this.imgListTv = new ImageList(this.components);
      this.btnVerifyAddress = new Button();
      this.label2 = new Label();
      this.txtAPIClientSecret = new TextBox();
      this.lblAPIClientSecret = new Label();
      this.txtAPIClientID = new TextBox();
      this.lblAPIClientID = new Label();
      this.txtDevConnectAPIUserID = new TextBox();
      this.lblDevConnectAPIUserID = new Label();
      this.txtEncompassClientInstance = new TextBox();
      this.lblEncompassClientInstance = new Label();
      this.txtEncompassClientID = new TextBox();
      this.lblEncompassClientID = new Label();
      this.lblDevConnectAccess = new Label();
      this.label3 = new Label();
      this.txtAIQSiteAddress = new TextBox();
      this.lblAIQSiteAddress = new Label();
      this.btnSave = new StandardIconButton();
      this.gcDays.SuspendLayout();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.SuspendLayout();
      this.gcDays.Controls.Add((Control) this.btnEdit);
      this.gcDays.Controls.Add((Control) this.tabControl1);
      this.gcDays.Controls.Add((Control) this.btnSave);
      this.gcDays.Dock = DockStyle.Fill;
      this.gcDays.HeaderForeColor = SystemColors.ControlText;
      this.gcDays.Location = new Point(0, 0);
      this.gcDays.Name = "gcDays";
      this.gcDays.Size = new Size(705, 431);
      this.gcDays.TabIndex = 12;
      this.gcDays.Text = "Set up Data & Document Automation and Mortgage Analyzers features.";
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(684, 4);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnEdit.TabIndex = 11;
      this.btnEdit.TabStop = false;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.tabControl1.Controls.Add((Control) this.tabPage1);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(1, 26);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(703, 404);
      this.tabControl1.TabIndex = 10;
      this.tabPage1.AutoScroll = true;
      this.tabPage1.Controls.Add((Control) this.lblVerifyAddress);
      this.tabPage1.Controls.Add((Control) this.btnVerifyAddress);
      this.tabPage1.Controls.Add((Control) this.label2);
      this.tabPage1.Controls.Add((Control) this.txtAPIClientSecret);
      this.tabPage1.Controls.Add((Control) this.lblAPIClientSecret);
      this.tabPage1.Controls.Add((Control) this.txtAPIClientID);
      this.tabPage1.Controls.Add((Control) this.lblAPIClientID);
      this.tabPage1.Controls.Add((Control) this.txtDevConnectAPIUserID);
      this.tabPage1.Controls.Add((Control) this.lblDevConnectAPIUserID);
      this.tabPage1.Controls.Add((Control) this.txtEncompassClientInstance);
      this.tabPage1.Controls.Add((Control) this.lblEncompassClientInstance);
      this.tabPage1.Controls.Add((Control) this.txtEncompassClientID);
      this.tabPage1.Controls.Add((Control) this.lblEncompassClientID);
      this.tabPage1.Controls.Add((Control) this.lblDevConnectAccess);
      this.tabPage1.Controls.Add((Control) this.label3);
      this.tabPage1.Controls.Add((Control) this.txtAIQSiteAddress);
      this.tabPage1.Controls.Add((Control) this.lblAIQSiteAddress);
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(695, 378);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Connectivity Settings";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.lblVerifyAddress.AutoSize = true;
      this.lblVerifyAddress.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblVerifyAddress.ImageList = this.imgListTv;
      this.lblVerifyAddress.Location = new Point(577, 32);
      this.lblVerifyAddress.Name = "lblVerifyAddress";
      this.lblVerifyAddress.Size = new Size(0, 13);
      this.lblVerifyAddress.TabIndex = 22;
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "member-group.png");
      this.imgListTv.Images.SetKeyName(1, "Warning.gif");
      this.imgListTv.Images.SetKeyName(2, "");
      this.imgListTv.Images.SetKeyName(3, "");
      this.btnVerifyAddress.Location = new Point(463, 26);
      this.btnVerifyAddress.Name = "btnVerifyAddress";
      this.btnVerifyAddress.Size = new Size(108, 23);
      this.btnVerifyAddress.TabIndex = 21;
      this.btnVerifyAddress.Text = "Verify Address";
      this.btnVerifyAddress.UseVisualStyleBackColor = true;
      this.btnVerifyAddress.Click += new EventHandler(this.btnVerifyAddress_Click);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 6.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.ForeColor = SystemColors.Highlight;
      this.label2.Location = new Point(28, 205);
      this.label2.Name = "label2";
      this.label2.Size = new Size(186, 12);
      this.label2.TabIndex = 20;
      this.label2.Text = "API User must have Super Admin privileges";
      this.txtAPIClientSecret.Location = new Point(253, 246);
      this.txtAPIClientSecret.Name = "txtAPIClientSecret";
      this.txtAPIClientSecret.PasswordChar = '*';
      this.txtAPIClientSecret.Size = new Size(204, 20);
      this.txtAPIClientSecret.TabIndex = 7;
      this.txtAPIClientSecret.Enter += new EventHandler(this.txtAPIClientSecret_Enter);
      this.txtAPIClientSecret.Leave += new EventHandler(this.txtAPIClientSecret_Leave);
      this.lblAPIClientSecret.AutoSize = true;
      this.lblAPIClientSecret.Location = new Point(250, 230);
      this.lblAPIClientSecret.Name = "lblAPIClientSecret";
      this.lblAPIClientSecret.Size = new Size(87, 13);
      this.lblAPIClientSecret.TabIndex = 15;
      this.lblAPIClientSecret.Text = "API Client Secret";
      this.txtAPIClientID.Location = new Point(27, 246);
      this.txtAPIClientID.Name = "txtAPIClientID";
      this.txtAPIClientID.Size = new Size(204, 20);
      this.txtAPIClientID.TabIndex = 6;
      this.txtAPIClientID.Enter += new EventHandler(this.txtAPIClientID_Enter);
      this.txtAPIClientID.Leave += new EventHandler(this.txtAPIClientID_Leave);
      this.lblAPIClientID.AutoSize = true;
      this.lblAPIClientID.Location = new Point(24, 230);
      this.lblAPIClientID.Name = "lblAPIClientID";
      this.lblAPIClientID.Size = new Size(67, 13);
      this.lblAPIClientID.TabIndex = 11;
      this.lblAPIClientID.Text = "API Client ID";
      this.txtDevConnectAPIUserID.Location = new Point(27, 182);
      this.txtDevConnectAPIUserID.Name = "txtDevConnectAPIUserID";
      this.txtDevConnectAPIUserID.Size = new Size(204, 20);
      this.txtDevConnectAPIUserID.TabIndex = 5;
      this.txtDevConnectAPIUserID.Enter += new EventHandler(this.txtDevConnectAPIUserID_Enter);
      this.txtDevConnectAPIUserID.Leave += new EventHandler(this.txtDevConnectAPIUserID_Leave);
      this.lblDevConnectAPIUserID.AutoSize = true;
      this.lblDevConnectAPIUserID.Location = new Point(24, 166);
      this.lblDevConnectAPIUserID.Name = "lblDevConnectAPIUserID";
      this.lblDevConnectAPIUserID.Size = new Size(129, 13);
      this.lblDevConnectAPIUserID.TabIndex = 9;
      this.lblDevConnectAPIUserID.Text = "Dev Connect API User ID";
      this.txtEncompassClientInstance.Location = new Point(253, 131);
      this.txtEncompassClientInstance.Name = "txtEncompassClientInstance";
      this.txtEncompassClientInstance.Size = new Size(204, 20);
      this.txtEncompassClientInstance.TabIndex = 3;
      this.txtEncompassClientInstance.Enter += new EventHandler(this.txtEncompassClientInstance_Enter);
      this.txtEncompassClientInstance.Leave += new EventHandler(this.txtEncompassClientInstance_Leave);
      this.lblEncompassClientInstance.AutoSize = true;
      this.lblEncompassClientInstance.Location = new Point(250, 115);
      this.lblEncompassClientInstance.Name = "lblEncompassClientInstance";
      this.lblEncompassClientInstance.Size = new Size(120, 13);
      this.lblEncompassClientInstance.TabIndex = 6;
      this.lblEncompassClientInstance.Text = "Encompass Instance ID";
      this.txtEncompassClientID.Location = new Point(27, 131);
      this.txtEncompassClientID.Name = "txtEncompassClientID";
      this.txtEncompassClientID.Size = new Size(204, 20);
      this.txtEncompassClientID.TabIndex = 2;
      this.txtEncompassClientID.Enter += new EventHandler(this.txtEncompassClientID_Enter);
      this.txtEncompassClientID.Leave += new EventHandler(this.txtEncompassClientID_Leave);
      this.lblEncompassClientID.AutoSize = true;
      this.lblEncompassClientID.Location = new Point(24, 115);
      this.lblEncompassClientID.Name = "lblEncompassClientID";
      this.lblEncompassClientID.Size = new Size(105, 13);
      this.lblEncompassClientID.TabIndex = 4;
      this.lblEncompassClientID.Text = "Encompass Client ID";
      this.lblDevConnectAccess.AutoSize = true;
      this.lblDevConnectAccess.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblDevConnectAccess.Location = new Point(24, 74);
      this.lblDevConnectAccess.Name = "lblDevConnectAccess";
      this.lblDevConnectAccess.Size = new Size(126, 13);
      this.lblDevConnectAccess.TabIndex = 3;
      this.lblDevConnectAccess.Text = "Dev Connect Access";
      this.label3.AutoSize = true;
      this.label3.ForeColor = SystemColors.GrayText;
      this.label3.Location = new Point(24, 52);
      this.label3.Name = "label3";
      this.label3.Size = new Size(1117, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "_________________________________________________________________________________________________________________________________________________________________________________________";
      this.txtAIQSiteAddress.Location = new Point(27, 29);
      this.txtAIQSiteAddress.Name = "txtAIQSiteAddress";
      this.txtAIQSiteAddress.Size = new Size(430, 20);
      this.txtAIQSiteAddress.TabIndex = 1;
      this.txtAIQSiteAddress.Enter += new EventHandler(this.txtAIQSiteAddress_Enter);
      this.txtAIQSiteAddress.Leave += new EventHandler(this.txtAIQSiteAddress_Leave);
      this.lblAIQSiteAddress.AutoSize = true;
      this.lblAIQSiteAddress.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblAIQSiteAddress.Location = new Point(24, 13);
      this.lblAIQSiteAddress.Name = "lblAIQSiteAddress";
      this.lblAIQSiteAddress.Size = new Size(87, 13);
      this.lblAIQSiteAddress.TabIndex = 0;
      this.lblAIQSiteAddress.Text = "Data && Document Automation and Mortgage Analyzers Site Address";
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(662, 4);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 9;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcDays);
      this.Name = nameof (EncompassAIQSetupControl);
      this.Size = new Size(705, 431);
      this.gcDays.ResumeLayout(false);
      ((ISupportInitialize) this.btnEdit).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.ResumeLayout(false);
    }

    private struct PersonaAccessRight
    {
      public int UserSpecificAccessRight { get; set; }

      public int PersonaSetting { get; set; }
    }
  }
}

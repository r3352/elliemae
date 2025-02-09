// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.CenterwiseActivationDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class CenterwiseActivationDialog : Form
  {
    private const string className = "CenterwiseActivationDialog";
    private static readonly string sw = Tracing.SwEpass;
    private const string Passcode = "General7792";
    private CenterwiseLOSelectionDialog loDialog;
    private string[] allowedWebCenterUsers;
    private UserInfo[] selectedWebCenterUsers;
    private UserInfo[] selectedEDMUsers;
    private IContainer components;
    private Label label1;
    private Label label2;
    private Label label3;
    private Panel panel1;
    private Panel pnlEDM;
    private Label label7;
    private Label label6;
    private Label label4;
    private Label label5;
    private Panel pnlWebCenter;
    private Label label9;
    private Label label10;
    private Label label11;
    private LinkLabel lnkSelectWebCenterLOs;
    private Button btnActivate;
    private Button btnCancel;
    private CheckBox chkNotAgain;
    private Panel pnlAdminStart;
    private Panel pnlAdminSummary;
    private Panel panel3;
    private Label label12;
    private Panel panel4;
    private PictureBox pictureBox2;
    private Label label16;
    private Button btnDone;
    private Panel panel5;
    private Label label18;
    private Label label19;
    private LinkLabel lnkWebCenterSetup;
    private Panel pnlUserSummary;
    private Panel pnlLOWebCenter;
    private LinkLabel lnkLOWebCenterSetup;
    private Label label8;
    private Panel pnlLOEDM;
    private PictureBox pictureBox3;
    private Label label13;
    private Button btnOK;
    private Panel panel12;
    private Label label14;
    private Label label15;
    private Label label17;
    private Label label20;
    private PictureBox pictureBox1;
    private PictureBox pictureBox5;
    private PictureBox pictureBox4;
    private PictureBox pictureBox6;
    private CheckBox chkLONotAgain;

    public CenterwiseActivationDialog()
    {
      this.InitializeComponent();
      if (Session.UserInfo.Userid == "admin")
      {
        this.showPanel(this.pnlAdminStart);
      }
      else
      {
        if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, false))
          this.pnlLOEDM.Visible = false;
        this.showPanel(this.pnlUserSummary);
      }
    }

    private void showPanel(Panel p)
    {
      this.pnlAdminStart.Visible = false;
      this.pnlAdminSummary.Visible = false;
      this.pnlUserSummary.Visible = false;
      p.Dock = DockStyle.Fill;
      p.Visible = true;
    }

    private void lnkSelectWebCenterLOs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (this.loDialog == null)
          this.loDialog = new CenterwiseLOSelectionDialog(this.getAllowedWebCenterUsers());
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
      if (this.loDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.selectedWebCenterUsers = this.loDialog.SelectedUsers;
    }

    private void btnActivate_Click(object sender, EventArgs e)
    {
      if (!this.activateCenterwise())
        return;
      this.showPanel(this.pnlAdminSummary);
    }

    private bool activateCenterwise()
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.selectedWebCenterUsers == null)
        this.selectedWebCenterUsers = CenterwiseLOSelectionDialog.GetFilteredLOList(this.getAllowedWebCenterUsers());
      if (this.selectedEDMUsers == null)
        this.selectedEDMUsers = Session.OrganizationManager.GetAllUsers();
      TimeSpan timeSpan = TimeSpan.Zero + TimeSpan.FromSeconds((double) (this.selectedWebCenterUsers.Length * 4)) + TimeSpan.FromSeconds((double) this.selectedEDMUsers.Length);
      Cursor.Current = Cursors.Default;
      if (timeSpan.TotalSeconds > 300.0 && Utils.Dialog((IWin32Window) this, "The activation process will take approximately " + timeSpan.TotalMinutes.ToString("#") + " minutes to complete. Would you like to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
        return false;
      using (ProgressDialog progressDialog = new ProgressDialog("CenterWise Activation Wizard", new AsynchronousProcess(this.activateCenterwiseAsync), (object) null, true))
        return progressDialog.ShowDialog((IWin32Window) this) == DialogResult.OK;
    }

    private DialogResult activateCenterwiseAsync(object state, IProgressFeedback feedback)
    {
      try
      {
        using (EMSiteWebService emSiteWebService = new EMSiteWebService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.CenterwiseServicesUrl))
        {
          feedback.Status = "Activating Corporate WebCenter...";
          if (!emSiteWebService.CreateCenterWiseCorporateSite(Session.CompanyInfo.ClientID, "General7792"))
            Tracing.Log(CenterwiseActivationDialog.sw, nameof (CenterwiseActivationDialog), TraceLevel.Warning, "Call to CreateCenterWiseCorporateSite failed -- skipping this step.");
          this.preventWizardFromDisplaying();
          feedback.Status = "Activating Loan Officer WebCenter Sites...";
          feedback.ResetCounter(this.selectedWebCenterUsers.Length);
          for (int index = 0; index < this.selectedWebCenterUsers.Length; ++index)
          {
            if (feedback.Cancel && this.verifyCancel(feedback))
              return DialogResult.Cancel;
            if (feedback.Cancel)
              feedback.Cancel = false;
            feedback.Details = "Creating web site for '" + this.selectedWebCenterUsers[index].FullName + "'";
            if (!emSiteWebService.HasUnconfiguredCenterWiseSite(Session.CompanyInfo.ClientID, this.selectedWebCenterUsers[index].Userid, "General7792"))
              emSiteWebService.CreateCenterWiseLOSite(Session.CompanyInfo.ClientID, this.selectedWebCenterUsers[index].Userid, "General7792");
            feedback.Increment(1);
          }
        }
        feedback.Status = "Activating EDM...";
        feedback.ResetCounter(this.selectedEDMUsers.Length);
        for (int index = 0; index < this.selectedEDMUsers.Length; ++index)
        {
          feedback.Details = "Updating user '" + this.selectedEDMUsers[index].FullName + "'";
          if (feedback.Cancel && this.verifyCancel(feedback))
            return DialogResult.Cancel;
          if (feedback.Cancel)
            feedback.Cancel = false;
          try
          {
            Modules.EnableModuleUser(EncompassModule.EDM, this.selectedEDMUsers[index].Userid);
          }
          catch (Exception ex)
          {
            MetricsFactory.IncrementErrorCounter(ex, "Error activating EDM for user", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\CenterwiseActivationDialog.cs", nameof (activateCenterwiseAsync), 179);
            Tracing.Log(CenterwiseActivationDialog.sw, nameof (CenterwiseActivationDialog), TraceLevel.Error, "Error activating EDM for user '" + (object) this.selectedEDMUsers[index] + "': " + (object) ex);
            int num = (int) Utils.Dialog((IWin32Window) feedback, "An error occurred while attempting to enable your users for EDM access. Contact ICE Mortgage Technology Customer Support for additional assistance at 800-777-1718.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
          }
          feedback.Increment(1);
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) feedback, "Activation process failed: " + (object) ex);
        return DialogResult.Abort;
      }
    }

    private bool verifyCancel(IProgressFeedback feedback)
    {
      return Utils.Dialog((IWin32Window) feedback, "If this process is cancelled you will be required to complete the CenterWise setup manually. Are you sure you wish to cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
    }

    public static void StartActivationCheck()
    {
      ThreadPool.QueueUserWorkItem(new WaitCallback(CenterwiseActivationDialog.executeActivationCheck));
    }

    private static void executeActivationCheck(object notUsed)
    {
      try
      {
        if (!CenterwiseActivationDialog.isDisplayRequired())
          return;
        Session.Application.Invoke((Delegate) new MethodInvoker(CenterwiseActivationDialog.Display), (object[]) null);
      }
      catch
      {
      }
    }

    private static bool isDisplayRequired()
    {
      return CenterwiseActivationDialog.forceActivationDialogEnabled() || Session.UserID == "admin" && CenterwiseActivationDialog.RequiresWebCenterActivation() || CenterwiseActivationDialog.isLO() && CenterwiseActivationDialog.RequiresLOActivationNotification();
    }

    public static void DisplayIfRequired()
    {
      if (!CenterwiseActivationDialog.isDisplayRequired())
        return;
      CenterwiseActivationDialog.Display();
    }

    public static void Display()
    {
      using (CenterwiseActivationDialog activationDialog = new CenterwiseActivationDialog())
      {
        int num = (int) activationDialog.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private static bool isLO()
    {
      foreach (RolesMappingInfo roleMapping in Session.StartupInfo.RoleMappings)
      {
        if (roleMapping.RealWorldRoleID == RealWorldRoleID.LoanOfficer)
        {
          if (roleMapping.RoleIDList.Length != 0)
          {
            foreach (RoleSummaryInfo allowedRole in Session.StartupInfo.AllowedRoles)
            {
              if (allowedRole.RoleID == roleMapping.RoleIDList[0])
                return true;
            }
            break;
          }
          break;
        }
      }
      return false;
    }

    public static bool RequiresWebCenterActivation()
    {
      try
      {
        if (Session.GetPrivateProfileString("CENTERWISE", "ActivationWizard") == "0")
          return false;
        bool flag = false;
        using (EMSiteWebService emSiteWebService = new EMSiteWebService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.CenterwiseServicesUrl))
        {
          emSiteWebService.Timeout = 5000;
          flag = emSiteWebService.CenterWiseNotConfigured(Session.CompanyInfo.ClientID, "General7792");
        }
        return flag;
      }
      catch (Exception ex)
      {
        Tracing.Log(CenterwiseActivationDialog.sw, nameof (CenterwiseActivationDialog), TraceLevel.Error, ex.ToString());
        return false;
      }
    }

    public static bool RequiresLOActivationNotification()
    {
      try
      {
        if (Session.GetPrivateProfileString("CENTERWISE", "ActivationWizard") == "0")
          return false;
        bool flag = false;
        using (EMSiteWebService emSiteWebService = new EMSiteWebService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.CenterwiseServicesUrl))
        {
          emSiteWebService.Timeout = 5000;
          flag = emSiteWebService.HasUnconfiguredCenterWiseSite(Session.CompanyInfo.ClientID, Session.UserID, "General7792");
        }
        return flag;
      }
      catch (Exception ex)
      {
        Tracing.Log(CenterwiseActivationDialog.sw, nameof (CenterwiseActivationDialog), TraceLevel.Error, ex.ToString());
        return false;
      }
    }

    private static bool forceActivationDialogEnabled()
    {
      return string.Concat(EnConfigurationSettings.GlobalSettings["CWActivation", (object) "0"]) == "1";
    }

    private void CenterwiseActivationDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.chkNotAgain.Checked && !this.chkLONotAgain.Checked)
        return;
      this.preventWizardFromDisplaying();
    }

    private void preventWizardFromDisplaying()
    {
      Session.WritePrivateProfileString("CENTERWISE", "ActivationWizard", "0");
    }

    private string[] getAllowedWebCenterUsers()
    {
      if (this.allowedWebCenterUsers != null)
        return this.allowedWebCenterUsers;
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        NameValueCollection data = new NameValueCollection();
        data.Add("ClientID", Session.CompanyInfo.ClientID);
        using (WebClient webClient = new WebClient())
        {
          webClient.UseDefaultCredentials = true;
          byte[] buffer = webClient.UploadValues("https://www.epassbusinesscenter.com/epassutils/portalgetusers.asp", "POST", data);
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.Load((Stream) new MemoryStream(buffer));
          List<string> stringList = new List<string>();
          foreach (XmlElement selectNode in xmlDocument.SelectNodes("//USER"))
            stringList.Add(selectNode.GetAttribute("ID"));
          this.allowedWebCenterUsers = stringList.ToArray();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CenterwiseActivationDialog.sw, nameof (CenterwiseActivationDialog), TraceLevel.Error, "Error retrieving ePASS user list: " + (object) ex);
        this.allowedWebCenterUsers = new string[0];
      }
      return this.allowedWebCenterUsers;
    }

    private void lnkLOWebCenterSetup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      string homePageUrl = Session.SessionObjects?.StartupInfo?.ServiceUrls?.HomePageUrl;
      Session.MainScreen.NavigateHome(string.IsNullOrWhiteSpace(homePageUrl) || !Uri.IsWellFormedUriString(homePageUrl, UriKind.Absolute) ? "https://encompass.elliemae.com/homepage/LoginUser.aspx" : homePageUrl + "homepage/LoginUser.aspx");
      this.DialogResult = DialogResult.OK;
    }

    private void btnDone_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CenterwiseActivationDialog));
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.panel1 = new Panel();
      this.pnlEDM = new Panel();
      this.pictureBox1 = new PictureBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.pnlWebCenter = new Panel();
      this.pictureBox5 = new PictureBox();
      this.lnkSelectWebCenterLOs = new LinkLabel();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label11 = new Label();
      this.btnActivate = new Button();
      this.btnCancel = new Button();
      this.chkNotAgain = new CheckBox();
      this.pnlAdminStart = new Panel();
      this.pnlAdminSummary = new Panel();
      this.panel3 = new Panel();
      this.pictureBox4 = new PictureBox();
      this.lnkWebCenterSetup = new LinkLabel();
      this.label12 = new Label();
      this.panel4 = new Panel();
      this.pictureBox2 = new PictureBox();
      this.label16 = new Label();
      this.btnDone = new Button();
      this.panel5 = new Panel();
      this.label18 = new Label();
      this.label19 = new Label();
      this.pnlUserSummary = new Panel();
      this.chkLONotAgain = new CheckBox();
      this.pnlLOWebCenter = new Panel();
      this.pictureBox6 = new PictureBox();
      this.label20 = new Label();
      this.lnkLOWebCenterSetup = new LinkLabel();
      this.label8 = new Label();
      this.pnlLOEDM = new Panel();
      this.label17 = new Label();
      this.pictureBox3 = new PictureBox();
      this.label13 = new Label();
      this.btnOK = new Button();
      this.panel12 = new Panel();
      this.label14 = new Label();
      this.label15 = new Label();
      this.panel1.SuspendLayout();
      this.pnlEDM.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.pnlWebCenter.SuspendLayout();
      ((ISupportInitialize) this.pictureBox5).BeginInit();
      this.pnlAdminStart.SuspendLayout();
      this.pnlAdminSummary.SuspendLayout();
      this.panel3.SuspendLayout();
      ((ISupportInitialize) this.pictureBox4).BeginInit();
      this.panel4.SuspendLayout();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      this.panel5.SuspendLayout();
      this.pnlUserSummary.SuspendLayout();
      this.pnlLOWebCenter.SuspendLayout();
      ((ISupportInitialize) this.pictureBox6).BeginInit();
      this.pnlLOEDM.SuspendLayout();
      ((ISupportInitialize) this.pictureBox3).BeginInit();
      this.panel12.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(98, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Congratulations!";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(0, 16);
      this.label2.Name = "label2";
      this.label2.Size = new Size(332, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Your purchase of the Encompass® CenterWise™ is now complete.";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(0, 46);
      this.label3.Name = "label3";
      this.label3.Size = new Size(561, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Please take a moment to activate your subscription and start using Encompass CenterWise today!";
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(633, 80);
      this.panel1.TabIndex = 3;
      this.pnlEDM.Controls.Add((Control) this.pictureBox1);
      this.pnlEDM.Controls.Add((Control) this.label7);
      this.pnlEDM.Controls.Add((Control) this.label6);
      this.pnlEDM.Controls.Add((Control) this.label4);
      this.pnlEDM.Controls.Add((Control) this.label5);
      this.pnlEDM.Dock = DockStyle.Top;
      this.pnlEDM.Location = new Point(0, 80);
      this.pnlEDM.Name = "pnlEDM";
      this.pnlEDM.Size = new Size(633, 94);
      this.pnlEDM.TabIndex = 4;
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(2, 7);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(16, 16);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 10;
      this.pictureBox1.TabStop = false;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(63, 60);
      this.label7.Name = "label7";
      this.label7.Size = new Size(477, 12);
      this.label7.TabIndex = 5;
      this.label7.Text = "To disable users from using EDM go to Settings > System Administartion > Licensed Add-Ons > E-Document Mgmt.";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(29, 60);
      this.label6.Name = "label6";
      this.label6.Size = new Size(33, 12);
      this.label6.TabIndex = 4;
      this.label6.Text = "Note:";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(29, 8);
      this.label4.Name = "label4";
      this.label4.Size = new Size(87, 14);
      this.label4.TabIndex = 2;
      this.label4.Text = "EDM Activation";
      this.label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label5.Location = new Point(29, 24);
      this.label5.Name = "label5";
      this.label5.Size = new Size(598, 34);
      this.label5.TabIndex = 3;
      this.label5.Text = "EDM will automatically be activated for all of your users. Enabling your users will allow them to take advantage of the tools available in the eFolder.";
      this.pnlWebCenter.Controls.Add((Control) this.pictureBox5);
      this.pnlWebCenter.Controls.Add((Control) this.lnkSelectWebCenterLOs);
      this.pnlWebCenter.Controls.Add((Control) this.label9);
      this.pnlWebCenter.Controls.Add((Control) this.label10);
      this.pnlWebCenter.Controls.Add((Control) this.label11);
      this.pnlWebCenter.Dock = DockStyle.Top;
      this.pnlWebCenter.Location = new Point(0, 174);
      this.pnlWebCenter.Name = "pnlWebCenter";
      this.pnlWebCenter.Size = new Size(633, 92);
      this.pnlWebCenter.TabIndex = 5;
      this.pictureBox5.Image = (Image) componentResourceManager.GetObject("pictureBox5.Image");
      this.pictureBox5.Location = new Point(2, 7);
      this.pictureBox5.Name = "pictureBox5";
      this.pictureBox5.Size = new Size(16, 16);
      this.pictureBox5.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox5.TabIndex = 10;
      this.pictureBox5.TabStop = false;
      this.lnkSelectWebCenterLOs.AutoSize = true;
      this.lnkSelectWebCenterLOs.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lnkSelectWebCenterLOs.LinkArea = new LinkArea(0, 10);
      this.lnkSelectWebCenterLOs.Location = new Point(63, 71);
      this.lnkSelectWebCenterLOs.Name = "lnkSelectWebCenterLOs";
      this.lnkSelectWebCenterLOs.Size = new Size(394, 15);
      this.lnkSelectWebCenterLOs.TabIndex = 5;
      this.lnkSelectWebCenterLOs.TabStop = true;
      this.lnkSelectWebCenterLOs.Text = "Click Here to deselect individual Loan Officers that shouldn't receive a Loan Officer WebCenter.";
      this.lnkSelectWebCenterLOs.UseCompatibleTextRendering = true;
      this.lnkSelectWebCenterLOs.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkSelectWebCenterLOs_LinkClicked);
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(29, 70);
      this.label9.Name = "label9";
      this.label9.Size = new Size(33, 12);
      this.label9.TabIndex = 4;
      this.label9.Text = "Note:";
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(29, 8);
      this.label10.Name = "label10";
      this.label10.Size = new Size(126, 14);
      this.label10.TabIndex = 2;
      this.label10.Text = "WebCenter Activation";
      this.label11.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label11.Location = new Point(29, 24);
      this.label11.Name = "label11";
      this.label11.Size = new Size(600, 45);
      this.label11.TabIndex = 3;
      this.label11.Text = componentResourceManager.GetString("label11.Text");
      this.btnActivate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnActivate.Location = new Point(478, 413);
      this.btnActivate.Name = "btnActivate";
      this.btnActivate.Size = new Size(75, 23);
      this.btnActivate.TabIndex = 6;
      this.btnActivate.Text = "&Activate";
      this.btnActivate.UseVisualStyleBackColor = true;
      this.btnActivate.Click += new EventHandler(this.btnActivate_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(556, 413);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.chkNotAgain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkNotAgain.AutoSize = true;
      this.chkNotAgain.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkNotAgain.Location = new Point(0, 418);
      this.chkNotAgain.Name = "chkNotAgain";
      this.chkNotAgain.Size = new Size(184, 18);
      this.chkNotAgain.TabIndex = 8;
      this.chkNotAgain.Text = "Do not show this message again";
      this.chkNotAgain.UseVisualStyleBackColor = true;
      this.pnlAdminStart.Controls.Add((Control) this.btnActivate);
      this.pnlAdminStart.Controls.Add((Control) this.pnlWebCenter);
      this.pnlAdminStart.Controls.Add((Control) this.chkNotAgain);
      this.pnlAdminStart.Controls.Add((Control) this.pnlEDM);
      this.pnlAdminStart.Controls.Add((Control) this.btnCancel);
      this.pnlAdminStart.Controls.Add((Control) this.panel1);
      this.pnlAdminStart.Dock = DockStyle.Fill;
      this.pnlAdminStart.Location = new Point(10, 10);
      this.pnlAdminStart.Name = "pnlAdminStart";
      this.pnlAdminStart.Size = new Size(633, 437);
      this.pnlAdminStart.TabIndex = 9;
      this.pnlAdminSummary.Controls.Add((Control) this.panel3);
      this.pnlAdminSummary.Controls.Add((Control) this.panel4);
      this.pnlAdminSummary.Controls.Add((Control) this.btnDone);
      this.pnlAdminSummary.Controls.Add((Control) this.panel5);
      this.pnlAdminSummary.Dock = DockStyle.Fill;
      this.pnlAdminSummary.Location = new Point(10, 10);
      this.pnlAdminSummary.Name = "pnlAdminSummary";
      this.pnlAdminSummary.Size = new Size(633, 437);
      this.pnlAdminSummary.TabIndex = 10;
      this.panel3.Controls.Add((Control) this.pictureBox4);
      this.panel3.Controls.Add((Control) this.lnkWebCenterSetup);
      this.panel3.Controls.Add((Control) this.label12);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(0, 144);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(633, 92);
      this.panel3.TabIndex = 5;
      this.pictureBox4.Image = (Image) componentResourceManager.GetObject("pictureBox4.Image");
      this.pictureBox4.Location = new Point(2, 7);
      this.pictureBox4.Name = "pictureBox4";
      this.pictureBox4.Size = new Size(16, 16);
      this.pictureBox4.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox4.TabIndex = 15;
      this.pictureBox4.TabStop = false;
      this.lnkWebCenterSetup.AutoSize = true;
      this.lnkWebCenterSetup.Location = new Point(30, 33);
      this.lnkWebCenterSetup.Name = "lnkWebCenterSetup";
      this.lnkWebCenterSetup.Size = new Size(168, 14);
      this.lnkWebCenterSetup.TabIndex = 12;
      this.lnkWebCenterSetup.TabStop = true;
      this.lnkWebCenterSetup.Text = "Setup your Corporate WebCenter";
      this.lnkWebCenterSetup.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkLOWebCenterSetup_LinkClicked);
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(30, 9);
      this.label12.Name = "label12";
      this.label12.Size = new Size(136, 14);
      this.label12.TabIndex = 2;
      this.label12.Text = "WebCenter is Activated";
      this.panel4.Controls.Add((Control) this.pictureBox2);
      this.panel4.Controls.Add((Control) this.label16);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(0, 58);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(633, 86);
      this.panel4.TabIndex = 4;
      this.pictureBox2.Image = (Image) componentResourceManager.GetObject("pictureBox2.Image");
      this.pictureBox2.Location = new Point(2, 7);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(16, 16);
      this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox2.TabIndex = 9;
      this.pictureBox2.TabStop = false;
      this.label16.AutoSize = true;
      this.label16.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label16.Location = new Point(30, 9);
      this.label16.Name = "label16";
      this.label16.Size = new Size(97, 14);
      this.label16.TabIndex = 2;
      this.label16.Text = "EDM is Activated";
      this.btnDone.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnDone.DialogResult = DialogResult.OK;
      this.btnDone.Location = new Point(556, 414);
      this.btnDone.Name = "btnDone";
      this.btnDone.Size = new Size(75, 22);
      this.btnDone.TabIndex = 7;
      this.btnDone.Text = "&Done";
      this.btnDone.UseVisualStyleBackColor = true;
      this.btnDone.Click += new EventHandler(this.btnDone_Click);
      this.panel5.Controls.Add((Control) this.label18);
      this.panel5.Controls.Add((Control) this.label19);
      this.panel5.Dock = DockStyle.Top;
      this.panel5.Location = new Point(0, 0);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(633, 58);
      this.panel5.TabIndex = 3;
      this.label18.AutoSize = true;
      this.label18.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label18.Location = new Point(0, 0);
      this.label18.Name = "label18";
      this.label18.Size = new Size(98, 14);
      this.label18.TabIndex = 0;
      this.label18.Text = "Congratulations!";
      this.label19.AutoSize = true;
      this.label19.Location = new Point(0, 17);
      this.label19.Name = "label19";
      this.label19.Size = new Size(331, 14);
      this.label19.TabIndex = 1;
      this.label19.Text = "Your subscription of the Encompass® CenterWise™ is now active.";
      this.pnlUserSummary.Controls.Add((Control) this.chkLONotAgain);
      this.pnlUserSummary.Controls.Add((Control) this.pnlLOWebCenter);
      this.pnlUserSummary.Controls.Add((Control) this.pnlLOEDM);
      this.pnlUserSummary.Controls.Add((Control) this.btnOK);
      this.pnlUserSummary.Controls.Add((Control) this.panel12);
      this.pnlUserSummary.Dock = DockStyle.Fill;
      this.pnlUserSummary.Location = new Point(10, 10);
      this.pnlUserSummary.Name = "pnlUserSummary";
      this.pnlUserSummary.Size = new Size(633, 437);
      this.pnlUserSummary.TabIndex = 11;
      this.chkLONotAgain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkLONotAgain.AutoSize = true;
      this.chkLONotAgain.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkLONotAgain.Location = new Point(2, 417);
      this.chkLONotAgain.Name = "chkLONotAgain";
      this.chkLONotAgain.Size = new Size(184, 18);
      this.chkLONotAgain.TabIndex = 9;
      this.chkLONotAgain.Text = "Do not show this message again";
      this.chkLONotAgain.UseVisualStyleBackColor = true;
      this.pnlLOWebCenter.Controls.Add((Control) this.pictureBox6);
      this.pnlLOWebCenter.Controls.Add((Control) this.label20);
      this.pnlLOWebCenter.Controls.Add((Control) this.lnkLOWebCenterSetup);
      this.pnlLOWebCenter.Controls.Add((Control) this.label8);
      this.pnlLOWebCenter.Dock = DockStyle.Top;
      this.pnlLOWebCenter.Location = new Point(0, 204);
      this.pnlLOWebCenter.Name = "pnlLOWebCenter";
      this.pnlLOWebCenter.Size = new Size(633, 150);
      this.pnlLOWebCenter.TabIndex = 5;
      this.pictureBox6.Image = (Image) componentResourceManager.GetObject("pictureBox6.Image");
      this.pictureBox6.Location = new Point(2, 7);
      this.pictureBox6.Name = "pictureBox6";
      this.pictureBox6.Size = new Size(16, 16);
      this.pictureBox6.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox6.TabIndex = 16;
      this.pictureBox6.TabStop = false;
      this.label20.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label20.Location = new Point(28, 22);
      this.label20.Name = "label20";
      this.label20.Size = new Size(599, 48);
      this.label20.TabIndex = 15;
      this.label20.Text = componentResourceManager.GetString("label20.Text");
      this.lnkLOWebCenterSetup.AutoSize = true;
      this.lnkLOWebCenterSetup.Location = new Point(28, 72);
      this.lnkLOWebCenterSetup.Name = "lnkLOWebCenterSetup";
      this.lnkLOWebCenterSetup.Size = new Size(181, 14);
      this.lnkLOWebCenterSetup.TabIndex = 12;
      this.lnkLOWebCenterSetup.TabStop = true;
      this.lnkLOWebCenterSetup.Text = "Setup your Loan Officer WebCenter";
      this.lnkLOWebCenterSetup.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkLOWebCenterSetup_LinkClicked);
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(28, 6);
      this.label8.Name = "label8";
      this.label8.Size = new Size(136, 14);
      this.label8.TabIndex = 2;
      this.label8.Text = "WebCenter is Activated";
      this.pnlLOEDM.Controls.Add((Control) this.label17);
      this.pnlLOEDM.Controls.Add((Control) this.pictureBox3);
      this.pnlLOEDM.Controls.Add((Control) this.label13);
      this.pnlLOEDM.Dock = DockStyle.Top;
      this.pnlLOEDM.Location = new Point(0, 76);
      this.pnlLOEDM.Name = "pnlLOEDM";
      this.pnlLOEDM.Size = new Size(633, 128);
      this.pnlLOEDM.TabIndex = 4;
      this.label17.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label17.Location = new Point(28, 24);
      this.label17.Name = "label17";
      this.label17.Size = new Size(599, 48);
      this.label17.TabIndex = 12;
      this.label17.Text = componentResourceManager.GetString("label17.Text");
      this.pictureBox3.Image = (Image) componentResourceManager.GetObject("pictureBox3.Image");
      this.pictureBox3.Location = new Point(2, 7);
      this.pictureBox3.Name = "pictureBox3";
      this.pictureBox3.Size = new Size(16, 16);
      this.pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox3.TabIndex = 9;
      this.pictureBox3.TabStop = false;
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(28, 8);
      this.label13.Name = "label13";
      this.label13.Size = new Size(97, 14);
      this.label13.TabIndex = 2;
      this.label13.Text = "EDM is Activated";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(556, 413);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.panel12.Controls.Add((Control) this.label14);
      this.panel12.Controls.Add((Control) this.label15);
      this.panel12.Dock = DockStyle.Top;
      this.panel12.Location = new Point(0, 0);
      this.panel12.Name = "panel12";
      this.panel12.Size = new Size(633, 76);
      this.panel12.TabIndex = 3;
      this.label14.AutoSize = true;
      this.label14.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(0, 0);
      this.label14.Name = "label14";
      this.label14.Size = new Size(98, 14);
      this.label14.TabIndex = 0;
      this.label14.Text = "Congratulations!";
      this.label15.Location = new Point(0, 17);
      this.label15.Name = "label15";
      this.label15.Size = new Size(574, 34);
      this.label15.TabIndex = 1;
      this.label15.Text = "Your company has activated a subscription for the Encompass® CenterWise™. Below are the tools that are now available to you.";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(653, 457);
      this.Controls.Add((Control) this.pnlAdminSummary);
      this.Controls.Add((Control) this.pnlAdminStart);
      this.Controls.Add((Control) this.pnlUserSummary);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CenterwiseActivationDialog);
      this.Padding = new Padding(10);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Activation Setup Wizard";
      this.FormClosing += new FormClosingEventHandler(this.CenterwiseActivationDialog_FormClosing);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.pnlEDM.ResumeLayout(false);
      this.pnlEDM.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.pnlWebCenter.ResumeLayout(false);
      this.pnlWebCenter.PerformLayout();
      ((ISupportInitialize) this.pictureBox5).EndInit();
      this.pnlAdminStart.ResumeLayout(false);
      this.pnlAdminStart.PerformLayout();
      this.pnlAdminSummary.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      ((ISupportInitialize) this.pictureBox4).EndInit();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.pnlUserSummary.ResumeLayout(false);
      this.pnlUserSummary.PerformLayout();
      this.pnlLOWebCenter.ResumeLayout(false);
      this.pnlLOWebCenter.PerformLayout();
      ((ISupportInitialize) this.pictureBox6).EndInit();
      this.pnlLOEDM.ResumeLayout(false);
      this.pnlLOEDM.PerformLayout();
      ((ISupportInitialize) this.pictureBox3).EndInit();
      this.panel12.ResumeLayout(false);
      this.panel12.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.VersionManagerSC
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.AdminTools.Properties;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class VersionManagerSC : Form
  {
    private const string serverUri = "/EncompassSCWS/SmartClientService.asmx";
    private const string SCTestCIDSuffix = "-Test";
    private const string className = "VersionManager";
    private static readonly string sw = Tracing.SwCommon;
    private VersionManagerSC.SCPackage currentPackage;
    private ReturnResult clientInfo;
    private Hashtable versionmgrSettings;
    private IContainer components;
    private RadioButton radManual;
    private RadioButton radAuto;
    private TableContainer grpHotfixes;
    private FlowLayoutPanel flpButtons;
    private Button btnActivate;
    private GridView gvVersions;
    private Label label3;
    private Button btnClose;
    private BorderPanel pnlTestClientID;
    private Label label1;
    private PictureBox pictureBox2;
    private System.Windows.Forms.LinkLabel lnkHelp;
    private Label lblTestClientID;
    private BorderPanel pnlTestMessage;
    private System.Windows.Forms.LinkLabel lnkHelp2;
    private PictureBox pictureBox1;
    private Label label4;
    private HelpLink helpLink1;
    private System.Windows.Forms.LinkLabel lnkReleaseNotes;

    public VersionManagerSC()
    {
      this.InitializeComponent();
      this.loadVersionSettings();
      this.lblTestClientID.Text = this.getSmartClientCID() + "-Test";
      this.Text = this.Text + " - " + this.getSmartClientCID();
    }

    private string getSmartClientCID() => AssemblyResolver.SCClientID;

    private bool isTestClientID()
    {
      return this.getSmartClientCID().EndsWith("-Test", StringComparison.CurrentCultureIgnoreCase);
    }

    private void loadVersionSettings()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        List<VersionManagerSC.SCPackage> scPackageList = new List<VersionManagerSC.SCPackage>();
        this.currentPackage = (VersionManagerSC.SCPackage) null;
        this.clientInfo = (ReturnResult) null;
        int num1 = 0;
        using (SmartClientService smartClientService = new SmartClientService(AssemblyResolver.AuthServerURL + "/EncompassSCWS/SmartClientService.asmx"))
        {
          this.clientInfo = smartClientService.GetClientInfo(this.getSmartClientCID(), Session.CompanyInfo.Password);
          if (this.clientInfo.ReturnCode == ReturnCode.AuthenticationFailed)
            throw new Exception("Authentication request failed. Contact ICE Mortgage Technology Customer Support at 1-800-777-1718 for assistance.");
          if (this.clientInfo.ReturnCode != ReturnCode.Success)
            throw new Exception("Unexpected error returned by server");
          foreach (SCPackageInfo settings in smartClientService.GetSCPackageInfo(this.getSmartClientCID(), VersionInformation.CurrentVersion.Version.FullVersion))
            scPackageList.Add(new VersionManagerSC.SCPackage(settings, num1++));
        }
        if (scPackageList.Count == 0)
          throw new Exception("No package information received");
        if (this.clientInfo.UpdateByEM)
        {
          this.currentPackage = scPackageList[scPackageList.Count - 1];
        }
        else
        {
          foreach (VersionManagerSC.SCPackage scPackage in scPackageList)
          {
            if (scPackage.Settings.InstallUrlID == this.clientInfo.InstallUrlID)
            {
              this.currentPackage = scPackage;
              break;
            }
          }
        }
        this.gvVersions.Items.Clear();
        for (int index = scPackageList.Count - 1; index >= 0; --index)
          this.gvVersions.Items.Add(this.createPackageItem(scPackageList[index]));
        if (this.clientInfo.UpdateByEM)
          this.radAuto.Checked = true;
        else
          this.radManual.Checked = true;
        if (this.currentPackage == null && !this.clientInfo.UpdateByEM)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You are currently configured to run a customized Encompass package. You will need to contact ICE Mortgage Technology Customer Support in order to make changes to your settings.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.disableControls();
        }
        else
        {
          this.refreshPackageStatuses();
          this.updateControlStates();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(VersionManagerSC.sw, "VersionManager", TraceLevel.Error, "Error loading version settings: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to retrieve your version update settings: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.disableControls();
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void disableControls()
    {
      this.radAuto.Enabled = false;
      this.radManual.Enabled = false;
      this.grpHotfixes.Enabled = false;
    }

    private GVItem createPackageItem(VersionManagerSC.SCPackage pkg)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) pkg.Settings.EncVersion
          },
          [2] = {
            Value = (object) pkg.Settings.Description
          },
          [3] = {
            Value = (object) pkg.Settings.ReleaseDate.ToString("MM/dd/yyyy")
          }
        },
        Tag = (object) pkg
      };
    }

    private void refreshPackageStatuses()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvVersions.Items)
      {
        VersionManagerSC.SCPackageStatus scPackageStatus = this.getSCPackageStatus(gvItem.Tag as VersionManagerSC.SCPackage);
        gvItem.SubItems[1].Text = this.getVersionStatusDescription(scPackageStatus);
        gvItem.SubItems[1].ForeColor = this.getVersionStatusColor(scPackageStatus);
      }
      this.gvVersions_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private VersionManagerSC.SCPackageStatus getSCPackageStatus(VersionManagerSC.SCPackage pkg)
    {
      if (this.currentPackage == null)
        return VersionManagerSC.SCPackageStatus.Unknown;
      if (this.currentPackage.Index == pkg.Index)
        return VersionManagerSC.SCPackageStatus.Current;
      return this.currentPackage.Index > pkg.Index ? VersionManagerSC.SCPackageStatus.Included : VersionManagerSC.SCPackageStatus.New;
    }

    private Color getVersionStatusColor(VersionManagerSC.SCPackageStatus status)
    {
      switch (status)
      {
        case VersionManagerSC.SCPackageStatus.New:
          return EncompassColors.Alert2;
        case VersionManagerSC.SCPackageStatus.Current:
          return Color.Green;
        case VersionManagerSC.SCPackageStatus.Included:
          return Color.Green;
        default:
          return Color.Black;
      }
    }

    private string getVersionStatusDescription(VersionManagerSC.SCPackageStatus status)
    {
      switch (status)
      {
        case VersionManagerSC.SCPackageStatus.New:
          return "New";
        case VersionManagerSC.SCPackageStatus.Current:
          return "Approved";
        case VersionManagerSC.SCPackageStatus.Included:
          return "Approved";
        default:
          return "";
      }
    }

    private void onVersionLinkClick(object sender, EventArgs e)
    {
      Process.Start(((Element) sender).Tag.ToString());
    }

    private void onUpdateMethodChanged(object sender, EventArgs e)
    {
      if (this.clientInfo.UpdateByEM && this.radManual.Checked)
      {
        this.switchToManualUpdates();
      }
      else
      {
        if (this.clientInfo.UpdateByEM || !this.radAuto.Checked)
          return;
        this.switchToAutoUpdates();
      }
    }

    private void switchToManualUpdates()
    {
      if (Utils.ShowChekboxDialog("WARNING: Should you elect to manually accept Service Pack and Critical Patch updates you are accepting the risk associated. By accepting to manually update your Encompass instance, you acknowledge that you understand that this could cause the data between Encompass and any application, including ICE Mortgage Technology Connect products, built on the Encompass Lending Platform APIs to be out of sync. The ICE Mortgage Technology Platform APIs support the most current minor version(s) of Encompass. It is Highly Recommended that you accept updates automatically to ensure accurate data and calculations.", "I understand and accept the risk", "Encompass") != DialogResult.OK)
      {
        this.radAuto.Checked = true;
      }
      else
      {
        this.versionmgrSettings = CollectionsUtil.CreateCaseInsensitiveHashtable();
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsg", (object) "Accepted");
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsgDT", (object) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsgUser", (object) Session.UserID);
        Session.ServerManager.UpdateServerSettings((IDictionary) this.versionmgrSettings);
        VersionManagerSC.SCPackage tag = (VersionManagerSC.SCPackage) this.gvVersions.Items[0].Tag;
        try
        {
          ReturnResult returnResult = (ReturnResult) null;
          using (SmartClientService smartClientService = new SmartClientService(AssemblyResolver.AuthServerURL + "/EncompassSCWS/SmartClientService.asmx"))
            returnResult = smartClientService.DoUpdatesByClient(this.getSmartClientCID(), Session.CompanyInfo.Password, tag.Settings.InstallUrlID);
          if (returnResult.ReturnCode == ReturnCode.AuthenticationFailed)
          {
            int num1 = (int) Utils.Dialog((IWin32Window) this, "An authentication error occurred while saving your changes. Ensure that your ICE Mortgage Technology Network password is correct and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else if (returnResult.ReturnCode != ReturnCode.Success)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while saving your changes: " + returnResult.Description + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          this.loadVersionSettings();
        }
        catch (Exception ex)
        {
          Tracing.Log(VersionManagerSC.sw, "VersionManager", TraceLevel.Error, "Error saving settings: " + (object) ex);
          int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to save your changes: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private Persona[] getPersonas()
    {
      Persona[] allPersonas = Session.PersonaManager.GetAllPersonas();
      ArrayList arrayList = new ArrayList();
      foreach (Persona persona in allPersonas)
      {
        if (persona.Name != "Administrator" && persona.ID != 1 && persona.Name != "Super Administrator" && persona.ID != 0)
          arrayList.Add((object) persona);
      }
      return (Persona[]) arrayList.ToArray(typeof (Persona));
    }

    private void switchToAutoUpdates()
    {
      if (Utils.Dialog((IWin32Window) this, "By switching to the automatic approval option, all Encompass hot updates will be applied to your users' computers as soon as they are available. This setting does not affect major product upgrades, which you must still perform manually." + Environment.NewLine + Environment.NewLine + "Are you sure you want to switch to the automatic approval option for Encompass hot updates?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
      {
        this.radManual.Checked = true;
      }
      else
      {
        this.versionmgrSettings = CollectionsUtil.CreateCaseInsensitiveHashtable();
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsg", (object) null);
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsgDT", (object) null);
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsgUser", (object) null);
        Session.ServerManager.UpdateServerSettings((IDictionary) this.versionmgrSettings);
        VersionManagerSC.SCPackage tag = (VersionManagerSC.SCPackage) this.gvVersions.Items[0].Tag;
        try
        {
          ReturnResult returnResult = (ReturnResult) null;
          using (SmartClientService smartClientService = new SmartClientService(AssemblyResolver.AuthServerURL + "/EncompassSCWS/SmartClientService.asmx"))
            returnResult = smartClientService.LetEMDoUupdates(this.getSmartClientCID(), Session.CompanyInfo.Password, VersionInformation.CurrentVersion.Version.FullVersion);
          if (returnResult.ReturnCode == ReturnCode.AuthenticationFailed)
          {
            int num1 = (int) Utils.Dialog((IWin32Window) this, "An authentication error occurred while saving your changes. Ensure that your ICE Mortgage Technology Network password is correct and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else if (returnResult.ReturnCode != ReturnCode.Success)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while saving your changes: " + returnResult.Description + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          this.loadVersionSettings();
        }
        catch (Exception ex)
        {
          Tracing.Log(VersionManagerSC.sw, "VersionManager", TraceLevel.Error, "Error saving settings: " + (object) ex);
          int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to save your changes: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private void updateControlStates()
    {
      this.flpButtons.Enabled = this.radManual.Checked;
      this.gvVersions.Selectable = this.radManual.Checked;
      this.pnlTestMessage.Visible = false;
      if (this.isTestClientID())
      {
        this.pnlTestClientID.Visible = false;
        this.pnlTestMessage.Visible = true;
      }
      else if (this.radAuto.Checked && this.pnlTestClientID.Enabled)
      {
        this.Height -= this.pnlTestClientID.Height;
        this.pnlTestClientID.Visible = false;
        this.pnlTestClientID.Enabled = false;
      }
      else
      {
        if (!this.radManual.Checked || this.pnlTestClientID.Enabled)
          return;
        this.Height += this.pnlTestClientID.Height;
        this.pnlTestClientID.Visible = true;
        this.pnlTestClientID.Enabled = true;
      }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void gvVersions_SelectedIndexChanged(object sender, EventArgs e)
    {
      VersionManagerSC.SCPackage scPackage = (VersionManagerSC.SCPackage) null;
      if (this.gvVersions.SelectedItems.Count > 0)
        scPackage = this.gvVersions.SelectedItems[0].Tag as VersionManagerSC.SCPackage;
      if (scPackage == null || scPackage.Index == this.currentPackage.Index)
      {
        this.btnActivate.Text = "Approve";
        this.btnActivate.Enabled = false;
      }
      else if (scPackage.Index > this.currentPackage.Index)
      {
        this.btnActivate.Text = "Approve";
        this.btnActivate.Enabled = true;
      }
      else
      {
        this.btnActivate.Text = "Rollback";
        this.btnActivate.Enabled = true;
      }
    }

    private void btnActivate_Click(object sender, EventArgs e)
    {
      VersionManagerSC.SCPackage tag = this.gvVersions.SelectedItems[0].Tag as VersionManagerSC.SCPackage;
      string text;
      if (tag.Index < this.currentPackage.Index)
        text = "Rolling back to an earlier version will cause you to lose all fixes and new functionality included in subsequent updates." + Environment.NewLine + Environment.NewLine + "Are you sure you want to roll back to version '" + tag.Settings.EncVersion + "' of Encompass?";
      else
        text = "Once an update is approved, it will be applied to your users' workstations the next time they start Encompass." + Environment.NewLine + Environment.NewLine + "Are you sure you want to approve the update '" + tag.Settings.EncVersion + "' and all updates prior to it?";
      if (Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      try
      {
        ReturnResult returnResult = (ReturnResult) null;
        using (SmartClientService smartClientService = new SmartClientService(AssemblyResolver.AuthServerURL + "/EncompassSCWS/SmartClientService.asmx"))
        {
          returnResult = smartClientService.DoUpdatesByClient(this.getSmartClientCID(), Session.CompanyInfo.Password, tag.Settings.InstallUrlID);
          this.currentPackage = tag;
        }
        if (returnResult.ReturnCode == ReturnCode.AuthenticationFailed)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "An authentication error occurred while saving your changes. Ensure that your ICE Mortgage Technology Network password is correct and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (returnResult.ReturnCode != ReturnCode.Success)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while saving your changes: " + returnResult.Description + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.refreshPackageStatuses();
      }
      catch (Exception ex)
      {
        Tracing.Log(VersionManagerSC.sw, "VersionManager", TraceLevel.Error, "Error saving settings: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to save your changes: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void lnkReleaseNotes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.openReleaseNotes();
    }

    private void openReleaseNotes()
    {
      Process.Start("https://help.icemortgagetechnology.com/documentation/encompass/Content/encompass/release_notes/release-notes.htm");
    }

    private void onDisplayTestCIDHelp(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.lnkHelp_Help((object) null, (EventArgs) null);
    }

    private void lnkHelp_Help(object sender, EventArgs e) => JedHelp.ShowHelp("VersionManager");

    private void VersionManagerSC_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.lnkHelp_Help((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (VersionManagerSC));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.radManual = new RadioButton();
      this.radAuto = new RadioButton();
      this.label3 = new Label();
      this.btnClose = new Button();
      this.helpLink1 = new HelpLink();
      this.pnlTestClientID = new BorderPanel();
      this.lnkHelp = new System.Windows.Forms.LinkLabel();
      this.pnlTestMessage = new BorderPanel();
      this.lnkHelp2 = new System.Windows.Forms.LinkLabel();
      this.pictureBox1 = new PictureBox();
      this.label4 = new Label();
      this.lblTestClientID = new Label();
      this.pictureBox2 = new PictureBox();
      this.label1 = new Label();
      this.grpHotfixes = new TableContainer();
      this.lnkReleaseNotes = new System.Windows.Forms.LinkLabel();
      this.flpButtons = new FlowLayoutPanel();
      this.btnActivate = new Button();
      this.gvVersions = new GridView();
      this.pnlTestClientID.SuspendLayout();
      this.pnlTestMessage.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      this.grpHotfixes.SuspendLayout();
      this.flpButtons.SuspendLayout();
      this.SuspendLayout();
      this.radManual.Location = new Point(93, 143);
      this.radManual.Name = "radManual";
      this.radManual.Size = new Size(621, 44);
      this.radManual.TabIndex = 14;
      this.radManual.TabStop = true;
      this.radManual.Text = componentResourceManager.GetString("radManual.Text");
      this.radManual.TextAlign = ContentAlignment.TopLeft;
      this.radManual.UseCompatibleTextRendering = true;
      this.radManual.UseMnemonic = false;
      this.radManual.UseVisualStyleBackColor = true;
      this.radManual.CheckedChanged += new EventHandler(this.onUpdateMethodChanged);
      this.radAuto.AutoSize = true;
      this.radAuto.Location = new Point(93, 117);
      this.radAuto.Name = "radAuto";
      this.radAuto.Size = new Size(464, 18);
      this.radAuto.TabIndex = 13;
      this.radAuto.TabStop = true;
      this.radAuto.Text = "Automatically apply all Encompass service packs and critical patches as they are released.";
      this.radAuto.UseVisualStyleBackColor = true;
      this.label3.Location = new Point(9, 12);
      this.label3.Name = "label3";
      this.label3.Size = new Size(761, 99);
      this.label3.TabIndex = 17;
      this.label3.Text = componentResourceManager.GetString("label3.Text");
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(695, 465);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 16;
      this.btnClose.Text = "&Close";
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.helpLink1.BackColor = Color.Transparent;
      this.helpLink1.Cursor = Cursors.Hand;
      this.helpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink1.Location = new Point(25, 470);
      this.helpLink1.Name = "helpLink1";
      this.helpLink1.Size = new Size(111, 17);
      this.helpLink1.TabIndex = 20;
      this.helpLink1.Help += new EventHandler(this.lnkHelp_Help);
      this.pnlTestClientID.BackColor = Color.FromArgb(233, 242, (int) byte.MaxValue);
      this.pnlTestClientID.Controls.Add((Control) this.lnkHelp);
      this.pnlTestClientID.Controls.Add((Control) this.pnlTestMessage);
      this.pnlTestClientID.Controls.Add((Control) this.lblTestClientID);
      this.pnlTestClientID.Controls.Add((Control) this.pictureBox2);
      this.pnlTestClientID.Controls.Add((Control) this.label1);
      this.pnlTestClientID.Location = new Point(93, 393);
      this.pnlTestClientID.Name = "pnlTestClientID";
      this.pnlTestClientID.Size = new Size(629, 49);
      this.pnlTestClientID.TabIndex = 18;
      this.lnkHelp.AutoSize = true;
      this.lnkHelp.LinkArea = new LinkArea(60, 24);
      this.lnkHelp.Location = new Point(24, 26);
      this.lnkHelp.Name = "lnkHelp";
      this.lnkHelp.Size = new Size(422, 18);
      this.lnkHelp.TabIndex = 11;
      this.lnkHelp.TabStop = true;
      this.lnkHelp.Text = "For information on how to use your Test Client ID, view the Encompass online help.";
      this.lnkHelp.UseCompatibleTextRendering = true;
      this.lnkHelp.LinkClicked += new LinkLabelLinkClickedEventHandler(this.onDisplayTestCIDHelp);
      this.pnlTestMessage.BackColor = Color.FromArgb(252, 220, 223);
      this.pnlTestMessage.Controls.Add((Control) this.lnkHelp2);
      this.pnlTestMessage.Controls.Add((Control) this.pictureBox1);
      this.pnlTestMessage.Controls.Add((Control) this.label4);
      this.pnlTestMessage.Location = new Point(1, 0);
      this.pnlTestMessage.Name = "pnlTestMessage";
      this.pnlTestMessage.Size = new Size(629, 49);
      this.pnlTestMessage.TabIndex = 19;
      this.lnkHelp2.AutoSize = true;
      this.lnkHelp2.LinkArea = new LinkArea(60, 24);
      this.lnkHelp2.Location = new Point(24, 26);
      this.lnkHelp2.Name = "lnkHelp2";
      this.lnkHelp2.Size = new Size(422, 18);
      this.lnkHelp2.TabIndex = 11;
      this.lnkHelp2.TabStop = true;
      this.lnkHelp2.Text = "For information on how to use your Test Client ID, view the Encompass online help.";
      this.lnkHelp2.UseCompatibleTextRendering = true;
      this.lnkHelp2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.onDisplayTestCIDHelp);
      this.pictureBox1.Image = (Image) Resources.bullet;
      this.pictureBox1.Location = new Point(10, 10);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(7, 5);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 9;
      this.pictureBox1.TabStop = false;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(23, 6);
      this.label4.Name = "label4";
      this.label4.Size = new Size(426, 14);
      this.label4.TabIndex = 0;
      this.label4.Text = "Note: Any changes made on this screen will only apply to your Test Client ID.";
      this.lblTestClientID.AutoSize = true;
      this.lblTestClientID.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTestClientID.Location = new Point(213, 6);
      this.lblTestClientID.Name = "lblTestClientID";
      this.lblTestClientID.Size = new Size(110, 14);
      this.lblTestClientID.TabIndex = 10;
      this.lblTestClientID.Text = "ENCBE11111111-Test";
      this.pictureBox2.Image = (Image) Resources.bullet;
      this.pictureBox2.Location = new Point(10, 10);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(7, 5);
      this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox2.TabIndex = 9;
      this.pictureBox2.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(24, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(170, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Your Encompass Test Client ID is: ";
      this.grpHotfixes.Controls.Add((Control) this.lnkReleaseNotes);
      this.grpHotfixes.Controls.Add((Control) this.flpButtons);
      this.grpHotfixes.Controls.Add((Control) this.gvVersions);
      this.grpHotfixes.Location = new Point(94, 198);
      this.grpHotfixes.Name = "grpHotfixes";
      this.grpHotfixes.Size = new Size(629, 176);
      this.grpHotfixes.TabIndex = 15;
      this.grpHotfixes.Text = "Service Packs & Critical Patches";
      this.lnkReleaseNotes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lnkReleaseNotes.AutoSize = true;
      this.lnkReleaseNotes.BackColor = Color.Transparent;
      this.lnkReleaseNotes.LinkArea = new LinkArea(63, 14);
      this.lnkReleaseNotes.Location = new Point(4, 156);
      this.lnkReleaseNotes.Name = "lnkReleaseNotes";
      this.lnkReleaseNotes.Size = new Size(388, 18);
      this.lnkReleaseNotes.TabIndex = 3;
      this.lnkReleaseNotes.TabStop = true;
      this.lnkReleaseNotes.Text = "For information about the features in each update, refer to the release notes.";
      this.lnkReleaseNotes.UseCompatibleTextRendering = true;
      this.lnkReleaseNotes.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkReleaseNotes_LinkClicked);
      this.flpButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flpButtons.BackColor = Color.Transparent;
      this.flpButtons.Controls.Add((Control) this.btnActivate);
      this.flpButtons.Enabled = false;
      this.flpButtons.FlowDirection = FlowDirection.RightToLeft;
      this.flpButtons.Location = new Point(496, 2);
      this.flpButtons.Name = "flpButtons";
      this.flpButtons.Size = new Size(128, 22);
      this.flpButtons.TabIndex = 1;
      this.btnActivate.Enabled = false;
      this.btnActivate.Location = new Point(44, 0);
      this.btnActivate.Margin = new Padding(0);
      this.btnActivate.Name = "btnActivate";
      this.btnActivate.Size = new Size(84, 22);
      this.btnActivate.TabIndex = 2;
      this.btnActivate.Text = "Approve";
      this.btnActivate.UseVisualStyleBackColor = true;
      this.btnActivate.Click += new EventHandler(this.btnActivate_Click);
      this.gvVersions.AllowMultiselect = false;
      this.gvVersions.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Version";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column4";
      gvColumn2.Text = "Status";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.Text = "Description";
      gvColumn3.Width = 280;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "Release Date";
      gvColumn4.Width = 80;
      this.gvVersions.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvVersions.Dock = DockStyle.Fill;
      this.gvVersions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvVersions.Location = new Point(1, 26);
      this.gvVersions.Name = "gvVersions";
      this.gvVersions.Selectable = false;
      this.gvVersions.Size = new Size(627, 124);
      this.gvVersions.SortIconVisible = false;
      this.gvVersions.SortOption = GVSortOption.None;
      this.gvVersions.TabIndex = 0;
      this.gvVersions.SelectedIndexChanged += new EventHandler(this.gvVersions_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(832, 502);
      this.Controls.Add((Control) this.helpLink1);
      this.Controls.Add((Control) this.pnlTestClientID);
      this.Controls.Add((Control) this.radManual);
      this.Controls.Add((Control) this.radAuto);
      this.Controls.Add((Control) this.grpHotfixes);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.btnClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.Name = nameof (VersionManagerSC);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass Version Manager";
      this.KeyDown += new KeyEventHandler(this.VersionManagerSC_KeyDown);
      this.pnlTestClientID.ResumeLayout(false);
      this.pnlTestClientID.PerformLayout();
      this.pnlTestMessage.ResumeLayout(false);
      this.pnlTestMessage.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      this.grpHotfixes.ResumeLayout(false);
      this.grpHotfixes.PerformLayout();
      this.flpButtons.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum SCPackageStatus
    {
      Unknown,
      New,
      Current,
      Included,
    }

    private class SCPackage
    {
      public readonly SCPackageInfo Settings;
      public readonly int Index;

      public SCPackage(SCPackageInfo settings, int index)
      {
        this.Settings = settings;
        this.Index = index;
      }
    }
  }
}

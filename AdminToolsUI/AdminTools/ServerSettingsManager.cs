// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.ServerSettingsManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class ServerSettingsManager : Form
  {
    private Label label1;
    private ComboBox cboCategory;
    private Label label2;
    private ServerSettingsEditor settingsEditor;
    private Label label3;
    private Button btnCancel;
    private Button btnOK;
    private Button btnApply;
    private System.ComponentModel.Container components;
    private Button btnReRegisterERDB;
    private HelpLink helpLink;
    internal SettingTargetSystem targetSystem;
    private static readonly string sw = Tracing.SwOutsideLoan;

    public ServerSettingsManager()
    {
      this.InitializeComponent();
      this.targetSystem = !Session.Connection.IsServerInProcess ? SettingTargetSystem.ServerOnly : SettingTargetSystem.OfflineOnly;
      if (Session.EncompassEdition == EncompassEdition.Banker)
        this.targetSystem |= SettingTargetSystem.BankerOnly;
      else
        this.targetSystem |= SettingTargetSystem.BrokerOnly;
      if (Session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        this.targetSystem |= SettingTargetSystem.HostedOnly;
      else
        this.targetSystem |= SettingTargetSystem.SelfHostedOnly;
      this.cboCategory.Items.Clear();
      this.cboCategory.Items.AddRange((object[]) SettingDefinitions.GetDisplayCategories(this.targetSystem));
      this.settingsEditor.ServerManager = Session.ServerManager;
      this.settingsEditor.SettingChanged += new SettingChangedEventHandler(this.onSettingChanged);
      this.cboCategory.SelectedIndex = 0;
      for (int index = 0; index < this.cboCategory.Items.Count; ++index)
      {
        if (this.cboCategory.Items[index].ToString().ToUpper() == "COMPONENTS")
        {
          this.cboCategory.SelectedIndex = index;
          break;
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ServerSettingsManager));
      this.label1 = new Label();
      this.cboCategory = new ComboBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.btnApply = new Button();
      this.btnReRegisterERDB = new Button();
      this.helpLink = new HelpLink();
      this.settingsEditor = new ServerSettingsEditor();
      this.SuspendLayout();
      this.label1.Location = new Point(10, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(396, 44);
      this.label1.TabIndex = 0;
      this.label1.Text = "Use this screen to modify the settings that control the behavior of the Encompass Server.";
      this.cboCategory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCategory.Location = new Point(415, 60);
      this.cboCategory.Name = "cboCategory";
      this.cboCategory.Size = new Size(146, 22);
      this.cboCategory.TabIndex = 1;
      this.cboCategory.SelectedIndexChanged += new EventHandler(this.cboCategory_SelectedIndexChanged);
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label2.FlatStyle = FlatStyle.Flat;
      this.label2.Location = new Point(315, 60);
      this.label2.Name = "label2";
      this.label2.Size = new Size(100, 23);
      this.label2.TabIndex = 2;
      this.label2.Text = "Category:";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label3.Location = new Point(10, 474);
      this.label3.Name = "label3";
      this.label3.Size = new Size(552, 33);
      this.label3.TabIndex = 4;
      this.label3.Text = "Double-click a value to edit it. Changes to items marked with an asterisk (*) will not take effect until the server is restarted.";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(490, 517);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(412, 517);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnApply.Enabled = false;
      this.btnApply.Location = new Point(334, 517);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(75, 23);
      this.btnApply.TabIndex = 7;
      this.btnApply.Text = "&Apply";
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.btnReRegisterERDB.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnReRegisterERDB.Location = new Point(225, 517);
      this.btnReRegisterERDB.Name = "btnReRegisterERDB";
      this.btnReRegisterERDB.Size = new Size(106, 23);
      this.btnReRegisterERDB.TabIndex = 8;
      this.btnReRegisterERDB.Text = "Re-register ERDB";
      this.btnReRegisterERDB.UseVisualStyleBackColor = true;
      this.btnReRegisterERDB.Visible = false;
      this.btnReRegisterERDB.Click += new EventHandler(this.btnReRegisterERDB_Click);
      this.helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.DisplayOption = HelpLinkDisplayOption.IconOnly;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.Location = new Point(10, 517);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(20, 19);
      this.helpLink.TabIndex = 9;
      this.helpLink.Help += new EventHandler(this.helpLink_Help);
      this.settingsEditor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.settingsEditor.AutoScroll = true;
      this.settingsEditor.Location = new Point(10, 87);
      this.settingsEditor.Name = "settingsEditor";
      this.settingsEditor.ServerManager = (IServerManager) null;
      this.settingsEditor.Size = new Size(553, 385);
      this.settingsEditor.TabIndex = 3;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(574, 550);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.btnReRegisterERDB);
      this.Controls.Add((Control) this.btnApply);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.cboCategory);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.settingsEditor);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.Name = nameof (ServerSettingsManager);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Server Settings Manager";
      this.KeyDown += new KeyEventHandler(this.ServerSettingsManager_KeyDown);
      this.ResumeLayout(false);
    }

    private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setBtnReRegisterERDBVisibilityAndText();
      bool flag = false;
      if (this.btnApply.Enabled)
      {
        if (Utils.Dialog((IWin32Window) this, "Save the changes for the current set of settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          flag = this.saveChanges();
        else
          this.btnApply.Enabled = false;
      }
      if (flag)
        this.btnApply.Enabled = false;
      this.settingsEditor.LoadSettings((ICollection) SettingDefinitions.GetOrderedDefinitions(this.cboCategory.Text, this.targetSystem));
    }

    private void setBtnReRegisterERDBVisibilityAndText()
    {
      object selectedItem = this.cboCategory.SelectedItem;
      string str = (string) null;
      if (selectedItem != null)
        str = selectedItem.ToString().ToUpper();
      this.btnReRegisterERDB.Visible = this.settingsEditor.UseIPRestriction && str == "POLICIES" || str == "PASSWORD";
      this.btnReRegisterERDB.Text = str == "PASSWORD" ? "Enable MFA Login" : "Set Allowed IPs";
    }

    private bool saveChanges()
    {
      bool flag = false;
      if (this.btnApply.Enabled)
      {
        flag = this.settingsEditor.SaveChanges();
        if (flag)
        {
          this.btnApply.Enabled = false;
          this.setBtnReRegisterERDBVisibilityAndText();
        }
      }
      return flag;
    }

    private void settingsChanged(string path) => this.btnApply.Enabled = true;

    private void btnApply_Click(object sender, EventArgs e) => this.saveChanges();

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.saveChanges();
      if (!string.IsNullOrEmpty(this.settingsEditor.errSetting))
        return;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.btnApply.Enabled && Utils.Dialog((IWin32Window) this, "All unsaved changes to the server settings will be lost.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        return;
      this.Close();
    }

    private void onSettingChanged(string path) => this.btnApply.Enabled = true;

    internal bool BtnApplyEnabled => this.btnApply.Enabled;

    internal bool BtnReRegisterERDBVisible
    {
      set => this.btnReRegisterERDB.Visible = value;
    }

    internal string BtnReRegisterERDBText
    {
      set => this.btnReRegisterERDB.Text = value;
    }

    private void btnReRegisterERDB_Click(object sender, EventArgs e)
    {
      if (this.btnReRegisterERDB.Text.IndexOf("Allowed IPs") >= 0)
        this.settingsEditor.SetAllowedIPs();
      else
        this.settingsEditor.SetMFAenabled();
    }

    private void helpLink_Help(object sender, EventArgs e) => JedHelp.ShowHelp("SettingsManager");

    private void ServerSettingsManager_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.helpLink_Help((object) null, (EventArgs) null);
    }
  }
}

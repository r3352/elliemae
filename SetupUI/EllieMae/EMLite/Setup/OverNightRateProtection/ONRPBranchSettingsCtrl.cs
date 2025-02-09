// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.OverNightRateProtection.ONRPBranchSettingsCtrl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.OverNightRateProtection
{
  public class ONRPBranchSettingsCtrl : UserControl
  {
    public ONRPEntitySettings settings;
    public ONRPEntitySettings originalSettings;
    private Sessions.Session session;
    private int currentOID;
    private int parentId;
    private bool readOnly;
    private bool modified;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Button btnViewEditSettings;
    private Label label1;
    private CheckBox parentBox;

    public ONRPBranchSettingsCtrl(Sessions.Session session, int uId)
    {
      this.session = session;
      this.currentOID = uId;
      this.InitializeComponent();
      this.parentBox.Enabled = this.currentOID != 0;
    }

    public void RefreshData(ONRPEntitySettings settings, int parentId)
    {
      this.parentId = parentId;
      this.settings = settings;
      this.parentBox.Enabled = this.parentId != 0;
      IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Policies");
      if (this.currentOID == -1)
      {
        this.originalSettings = ONRPSettingFactory.GetRetailBranchUISetting(serverSettings);
        this.settings = this.originalSettings;
      }
      else
      {
        this.settings.SetRules((IONRPRuleHandler) null, new ONRPBaseRule(), new LockDeskGlobalSettings(serverSettings, LoanChannel.BankedRetail));
        this.originalSettings = settings.Clone((IONRPRuleHandler) null, (ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(serverSettings, LoanChannel.BankedRetail));
      }
    }

    private void btnViewEditSettings_Click(object sender, EventArgs e)
    {
      ONRPToleranceSetting toleranceSetting = new ONRPToleranceSetting(this.readOnly || this.currentOID == 0);
      if (this.currentOID == 0)
      {
        this.settings.SetRules((IONRPRuleHandler) null, (ONRPBaseRule) new ONRPSettingRules(), (LockDeskGlobalSettings) null);
      }
      else
      {
        this.settings.SetRules((IONRPRuleHandler) null, (ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(this.session.ServerManager.GetServerSettings("Policies"), LoanChannel.BankedRetail));
        toleranceSetting.RefreshData(this.settings);
      }
      if (toleranceSetting.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
      {
        this.settings = toleranceSetting.Settings;
        this.modified = true;
      }
      else
        this.settings = toleranceSetting.Settings;
    }

    private void parentBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.parentBox.Checked)
        this.ReloadParentONRPInfo(this.parentId);
      this.settings.UseParentInfo = this.parentBox.Checked;
      this.SetReadOnly(this.parentBox.Checked);
    }

    public void ReloadParentONRPInfo(int uId)
    {
      OrgInfo organizationWithOnrp = this.session.OrganizationManager.GetFirstOrganizationWithONRP(uId);
      if (organizationWithOnrp == null)
        return;
      this.settings = organizationWithOnrp.ONRPRetailBranchSettings;
      this.parentBox.Checked = true;
    }

    public void SetUseParentInfo(bool useParentInfo)
    {
      this.parentBox.Checked = useParentInfo;
      this.SetReadOnly(useParentInfo);
    }

    public bool GetUseParentInfo() => this.parentBox.Checked;

    public bool IsModfied() => this.modified;

    public void SetReadOnly(bool readOnly) => this.readOnly = readOnly;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.parentBox = new CheckBox();
      this.label1 = new Label();
      this.btnViewEditSettings = new Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.parentBox);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.btnViewEditSettings);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(512, 77);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Branch Settings for Overnight Rate Protection";
      this.parentBox.AutoSize = true;
      this.parentBox.BackColor = Color.Transparent;
      this.parentBox.Enabled = false;
      this.parentBox.Location = new Point(408, 6);
      this.parentBox.Name = "parentBox";
      this.parentBox.Size = new Size(100, 17);
      this.parentBox.TabIndex = 20;
      this.parentBox.Text = "Use Parent Info";
      this.parentBox.UseVisualStyleBackColor = false;
      this.parentBox.CheckedChanged += new EventHandler(this.parentBox_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(20, 44);
      this.label1.Name = "label1";
      this.label1.Size = new Size(307, 13);
      this.label1.TabIndex = 19;
      this.label1.Text = "Click View / Edit Settings button to setup ONRP for this branch.";
      this.btnViewEditSettings.Location = new Point(363, 38);
      this.btnViewEditSettings.Name = "btnViewEditSettings";
      this.btnViewEditSettings.Size = new Size(121, 23);
      this.btnViewEditSettings.TabIndex = 18;
      this.btnViewEditSettings.Text = "View / Edit Settings";
      this.btnViewEditSettings.UseVisualStyleBackColor = true;
      this.btnViewEditSettings.Click += new EventHandler(this.btnViewEditSettings_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (ONRPBranchSettingsCtrl);
      this.Size = new Size(512, 77);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

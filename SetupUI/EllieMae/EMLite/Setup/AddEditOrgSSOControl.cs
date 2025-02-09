// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddEditOrgSSOControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddEditOrgSSOControl : UserControl
  {
    private Sessions.Session session;
    public SSOInfo ssoSettings;
    private IOrganizationManager rOrg;
    private int parentID = -1;
    private int oid = -1;
    private bool newOrg;
    private OrgInfo orgInfo;
    private IContainer components;
    private GroupContainer groupContainer1;
    private CheckBox chkUseParentInfo;
    private RadioButton rbFullAccess;
    private RadioButton rbRestricted;
    private Label label1;
    private Label lblwarningmessage;

    public AddEditOrgSSOControl(Sessions.Session session, OrgInfo orgInfo, bool newOrg = false)
    {
      this.session = session;
      this.rOrg = this.session.OrganizationManager;
      this.orgInfo = orgInfo;
      this.newOrg = newOrg;
      this.InitializeComponent();
      if (!newOrg)
        this.ssoSettings = orgInfo.SSOSettings;
      this.parentID = orgInfo.Parent;
      this.oid = orgInfo.Oid;
      if (this.ssoSettings != null)
      {
        this.setSSOInfo();
      }
      else
      {
        this.ssoSettings = new SSOInfo();
        this.setSSOInfo();
      }
      if (this.oid == this.parentID && !newOrg)
        this.chkUseParentInfo.Enabled = false;
      this.SetWarningmsg();
    }

    private void SetWarningmsg()
    {
      this.lblwarningmessage.Visible = !this.ssoSettings.UseParentInfo;
    }

    public AddEditOrgSSOControl(Sessions.Session session, UserInfo userInfo, int oid = -1)
    {
      this.session = session;
      this.rOrg = this.session.OrganizationManager;
      this.InitializeComponent();
      this.oid = oid;
      this.orgInfo = this.rOrg.GetOrganization(oid);
      if (this.ssoSettings != null)
        this.setSSOInfo();
      else if (oid != -1)
      {
        this.ssoSettings = new SSOInfo();
        if (this.orgInfo.SSOSettings != null)
        {
          this.ssoSettings = this.orgInfo.SSOSettings;
        }
        else
        {
          this.ssoSettings.UseParentInfo = true;
          this.ssoSettings.LoginAccess = false;
        }
      }
      this.chkUseParentInfo.Checked = this.ssoSettings.UseParentInfo;
      if (this.ssoSettings.LoginAccess)
        this.rbRestricted.Checked = true;
      else
        this.rbFullAccess.Checked = true;
      this.SetWarningmsg();
    }

    private void setSSOInfo()
    {
      OrgInfo orgInfo = this.ssoSettings.UseParentInfo ? this.rOrg.GetFirstOrganizationForSSO(this.orgInfo.Oid) : this.orgInfo;
      this.chkUseParentInfo.Checked = this.ssoSettings.UseParentInfo ? this.ssoSettings.UseParentInfo : orgInfo.SSOSettings.UseParentInfo;
      if (orgInfo.SSOSettings.LoginAccess)
        this.rbRestricted.Checked = true;
      else
        this.rbFullAccess.Checked = true;
    }

    private void chkUseParentInfo_CheckedChanged(object sender, EventArgs e)
    {
      this.rbRestricted.Enabled = this.rbFullAccess.Enabled = !this.chkUseParentInfo.Checked;
      if (this.chkUseParentInfo.Checked)
      {
        OrgInfo organizationForSso = this.rOrg.GetFirstOrganizationForSSO(this.newOrg ? this.orgInfo.Oid : this.orgInfo.Parent);
        this.ssoSettings.LoginAccess = organizationForSso.SSOSettings.LoginAccess;
        if (organizationForSso.SSOSettings.LoginAccess)
          this.rbRestricted.Checked = true;
        else
          this.rbFullAccess.Checked = true;
      }
      this.ssoSettings.UseParentInfo = this.chkUseParentInfo.Checked;
      this.SetWarningmsg();
    }

    private void rbFullAccess_CheckedChanged(object sender, EventArgs e)
    {
      RadioButton radioButton = (RadioButton) sender;
      if (radioButton.Name == "rbRestricted")
      {
        if (radioButton.Checked && !this.IsRestrictedSSOEnabled())
        {
          this.rbFullAccess.Checked = true;
          int num = (int) Utils.Dialog((IWin32Window) this, "Your organization is not set up for SSO, configure your SSO settings to allow Restricted Access.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
          this.ssoSettings.LoginAccess = radioButton.Checked;
      }
      else
        this.ssoSettings.LoginAccess = !radioButton.Checked;
    }

    private bool IsRestrictedSSOEnabled()
    {
      string ssoEnabledResponse = new OAuth2(this.session.StartupInfo.OAPIGatewayBaseUri).GetRestrictedSSOEnabledResponse(this.session.ServerIdentity.InstanceName, this.session.SessionID, "sc");
      if (string.IsNullOrWhiteSpace(ssoEnabledResponse))
        return false;
      object obj = JsonConvert.DeserializeObject<object>(ssoEnabledResponse);
      return obj != null && ((JToken) obj).HasValues;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.lblwarningmessage = new Label();
      this.rbFullAccess = new RadioButton();
      this.rbRestricted = new RadioButton();
      this.label1 = new Label();
      this.chkUseParentInfo = new CheckBox();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.lblwarningmessage);
      this.groupContainer1.Controls.Add((Control) this.rbFullAccess);
      this.groupContainer1.Controls.Add((Control) this.rbRestricted);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.chkUseParentInfo);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(548, 94);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Login Access";
      this.lblwarningmessage.AutoSize = true;
      this.lblwarningmessage.ForeColor = Color.Red;
      this.lblwarningmessage.Location = new Point(72, 76);
      this.lblwarningmessage.Name = "lblwarningmessage";
      this.lblwarningmessage.Size = new Size(470, 13);
      this.lblwarningmessage.TabIndex = 13;
      this.lblwarningmessage.Text = "Updates made to SSO Access settings will only be applied to users that have not been customized";
      this.rbFullAccess.AutoSize = true;
      this.rbFullAccess.Location = new Point(78, 56);
      this.rbFullAccess.Name = "rbFullAccess";
      this.rbFullAccess.Size = new Size(308, 17);
      this.rbFullAccess.TabIndex = 12;
      this.rbFullAccess.TabStop = true;
      this.rbFullAccess.Text = "Full Access (Login supported via SSO or User ID/Password)";
      this.rbFullAccess.UseVisualStyleBackColor = true;
      this.rbFullAccess.Click += new EventHandler(this.rbFullAccess_CheckedChanged);
      this.rbRestricted.AutoSize = true;
      this.rbRestricted.Location = new Point(78, 33);
      this.rbRestricted.Name = "rbRestricted";
      this.rbRestricted.Size = new Size(282, 17);
      this.rbRestricted.TabIndex = 11;
      this.rbRestricted.TabStop = true;
      this.rbRestricted.Text = "Restricted Access (Login supported only through SSO)";
      this.rbRestricted.UseVisualStyleBackColor = true;
      this.rbRestricted.Click += new EventHandler(this.rbFullAccess_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 35);
      this.label1.Name = "label1";
      this.label1.Size = new Size(63, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "Select One:";
      this.chkUseParentInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfo.AutoSize = true;
      this.chkUseParentInfo.BackColor = Color.Transparent;
      this.chkUseParentInfo.Location = new Point(442, 5);
      this.chkUseParentInfo.Name = "chkUseParentInfo";
      this.chkUseParentInfo.Size = new Size(100, 17);
      this.chkUseParentInfo.TabIndex = 9;
      this.chkUseParentInfo.Text = "Use Parent Info";
      this.chkUseParentInfo.UseVisualStyleBackColor = false;
      this.chkUseParentInfo.CheckedChanged += new EventHandler(this.chkUseParentInfo_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (AddEditOrgSSOControl);
      this.Size = new Size(548, 94);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

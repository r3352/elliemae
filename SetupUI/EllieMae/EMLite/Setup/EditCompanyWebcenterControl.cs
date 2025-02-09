// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditCompanyWebcenterControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditCompanyWebcenterControl : UserControl
  {
    private Sessions.Session session;
    private int orgID = -1;
    private ExternalOriginatorManagementData externalOriginator;
    public AddWebcenterURLControl addURLControl;
    private IContainer components;
    private Panel panelAll;
    private Label label1;
    private GroupContainer grpAll;
    private Label label33;
    private BorderPanel borderPanelHeader;
    private StandardIconButton standardIconButton1;
    private CheckBox chkTestAccount;

    public bool IsDirty { get; set; }

    public EditCompanyWebcenterControl(Sessions.Session session, int orgID, int parent)
    {
      this.session = session;
      this.orgID = orgID;
      this.IsDirty = false;
      this.InitializeComponent();
      this.standardIconButton1.Enabled = false;
      this.Dock = DockStyle.Fill;
      this.externalOriginator = this.session.ConfigurationManager.GetExternalOrganization(false, orgID);
      if (this.externalOriginator != null && this.externalOriginator.IsTestAccount)
        this.chkTestAccount.Checked = true;
      if (this.externalOriginator != null && this.externalOriginator.Parent != 0)
      {
        ExternalOriginatorManagementData rootOrganisation = this.session.ConfigurationManager.GetRootOrganisation(false, orgID);
        if (rootOrganisation != null && rootOrganisation.IsTestAccount)
          this.chkTestAccount.Checked = true;
        this.chkTestAccount.Visible = false;
      }
      this.addURLControl = new AddWebcenterURLControl(true, true, this.session, parent, this.externalOriginator, btnSave: this.standardIconButton1, caller: (object) this);
      this.panelAll.Controls.Add((Control) this.addURLControl);
      this.IsDirty = this.standardIconButton1.Enabled;
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    private void chkUseParentInfo_CheckedChanged(object sender, EventArgs e)
    {
      if (this.externalOriginator == null)
      {
        this.externalOriginator = this.session.ConfigurationManager.GetExternalOrganization(false, this.orgID);
      }
      else
      {
        this.externalOriginator.InheritWebCenterSetup = false;
        this.session.ConfigurationManager.UpdateInheritWebCenterSetupFlag(this.orgID, false);
        this.addURLControl.SetWebCenterUrlControlState(true);
      }
    }

    public void standardIconButton1_Click(object sender, EventArgs e)
    {
      bool flag = this.addURLControl.AddOrUpdateExternalOrgURLS();
      this.session.ConfigurationManager.UpdateExternalOrgIsTestAccount(this.orgID, this.chkTestAccount.Checked);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
      if (!flag)
        return;
      this.IsDirty = false;
    }

    private void chkTestAccount_CheckedChanged(object sender, EventArgs e)
    {
      this.IsDirty = true;
      this.standardIconButton1.Enabled = true;
    }

    private void grpAll_Paint(object sender, PaintEventArgs e)
    {
    }

    public void DisableControls()
    {
      this.standardIconButton1.Visible = false;
      this.disableControl(this.Controls);
    }

    private void disableControl(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case TextBox _:
          case CheckBox _:
          case ComboBox _:
          case DatePicker _:
            control.Enabled = false;
            break;
        }
        if (control.Controls != null && control.Controls.Count > 0)
          this.disableControl(control.Controls);
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
      this.panelAll = new Panel();
      this.label1 = new Label();
      this.grpAll = new GroupContainer();
      this.chkTestAccount = new CheckBox();
      this.standardIconButton1 = new StandardIconButton();
      this.borderPanelHeader = new BorderPanel();
      this.label33 = new Label();
      this.grpAll.SuspendLayout();
      ((ISupportInitialize) this.standardIconButton1).BeginInit();
      this.borderPanelHeader.SuspendLayout();
      this.SuspendLayout();
      this.panelAll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelAll.Location = new Point(6, 80);
      this.panelAll.Name = "panelAll";
      this.panelAll.Size = new Size(852, 526);
      this.panelAll.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 59);
      this.label1.Name = "label1";
      this.label1.Size = new Size(183, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Select your TPO Connect site’s URL.";
      this.grpAll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpAll.Controls.Add((Control) this.chkTestAccount);
      this.grpAll.Controls.Add((Control) this.standardIconButton1);
      this.grpAll.Controls.Add((Control) this.borderPanelHeader);
      this.grpAll.Controls.Add((Control) this.panelAll);
      this.grpAll.Controls.Add((Control) this.label1);
      this.grpAll.HeaderForeColor = SystemColors.ControlText;
      this.grpAll.Location = new Point(5, 5);
      this.grpAll.Margin = new Padding(0);
      this.grpAll.Name = "grpAll";
      this.grpAll.Size = new Size(862, 610);
      this.grpAll.TabIndex = 2;
      this.grpAll.Text = "TPO Connect Setup";
      this.grpAll.Paint += new PaintEventHandler(this.grpAll_Paint);
      this.chkTestAccount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkTestAccount.AutoSize = true;
      this.chkTestAccount.BackColor = Color.Transparent;
      this.chkTestAccount.Location = new Point(727, 4);
      this.chkTestAccount.Name = "chkTestAccount";
      this.chkTestAccount.Size = new Size(90, 17);
      this.chkTestAccount.TabIndex = 39;
      this.chkTestAccount.Text = "Test Account";
      this.chkTestAccount.UseVisualStyleBackColor = false;
      this.chkTestAccount.CheckedChanged += new EventHandler(this.chkTestAccount_CheckedChanged);
      this.standardIconButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.standardIconButton1.BackColor = Color.Transparent;
      this.standardIconButton1.Location = new Point(842, 4);
      this.standardIconButton1.MouseDownImage = (Image) null;
      this.standardIconButton1.Name = "standardIconButton1";
      this.standardIconButton1.Size = new Size(16, 16);
      this.standardIconButton1.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.standardIconButton1.TabIndex = 38;
      this.standardIconButton1.TabStop = false;
      this.standardIconButton1.Click += new EventHandler(this.standardIconButton1_Click);
      this.borderPanelHeader.Borders = AnchorStyles.Bottom;
      this.borderPanelHeader.Controls.Add((Control) this.label33);
      this.borderPanelHeader.Dock = DockStyle.Top;
      this.borderPanelHeader.Location = new Point(1, 26);
      this.borderPanelHeader.Name = "borderPanelHeader";
      this.borderPanelHeader.Size = new Size(860, 26);
      this.borderPanelHeader.TabIndex = 0;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(5, 5);
      this.label33.Name = "label33";
      this.label33.Size = new Size(417, 13);
      this.label33.TabIndex = 36;
      this.label33.Text = "Select the TPO Connect site that the Third Party Originator company or branch will use.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAll);
      this.Margin = new Padding(0);
      this.Name = nameof (EditCompanyWebcenterControl);
      this.Padding = new Padding(5);
      this.Size = new Size(872, 620);
      this.grpAll.ResumeLayout(false);
      this.grpAll.PerformLayout();
      ((ISupportInitialize) this.standardIconButton1).EndInit();
      this.borderPanelHeader.ResumeLayout(false);
      this.borderPanelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

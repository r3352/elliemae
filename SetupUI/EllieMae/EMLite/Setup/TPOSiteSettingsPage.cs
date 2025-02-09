// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TPOSiteSettingsPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TPOSiteSettingsPage : PersonaTreePageBase
  {
    private int personaID = -1;
    private Persona[] personaList;
    private string userID = "";
    private IContainer components;
    private Panel panelTPOAdministratorPage;
    private PanelEx panelExTPOAdministrator;
    private CheckBox chkAccessTPOAdminSite;

    public int PersonaID
    {
      get => this.personaID;
      set
      {
        if (this.personaID != value)
          this.Reset();
        this.personaID = value;
      }
    }

    public TPOSiteSettingsPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "TPO Connect Site Settings";
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.CheckedChangedEvent += new PersonaTreeNodeCheckedChanged(this.CheckedChanged);
      this.personaID = personaId;
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new TPOSiteSettingsSecurityHelper(this.session, personaId);
      this.init();
    }

    public TPOSiteSettingsPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "TPO Connect Site Settings";
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.bIsUserSetup = true;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new TPOSiteSettingsSecurityHelper(this.session, userId, personas);
      this.userID = userId;
      this.personaList = personas;
      this.init();
    }

    private void NodeMouseUp(TreeNode node)
    {
    }

    private bool CheckedChanged(TreeNode node)
    {
      if (node.Text == "Access Wholesale" || node.Text == "Access Correspondent")
      {
        this.bInit = true;
        this.securityHelper.SetNodeUpdateStatus(node, true);
        if (!node.Checked && node.Nodes != null && node.Nodes.Count > 0)
        {
          foreach (TreeNode node1 in node.Nodes)
          {
            if (node1.Checked)
            {
              node1.Checked = false;
              this.securityHelper.SetNodeUpdateStatus(node1, true);
            }
          }
        }
        this.setDirtyFlag(true);
        this.bInit = false;
        return false;
      }
      if (!(node.Text == "Register Wholesale Loan") && !(node.Text == "Register Correspondent Loan"))
        return true;
      this.bInit = true;
      if (node.Parent.Checked)
        this.securityHelper.SetNodeUpdateStatus(node, true);
      else if (node.Checked)
      {
        node.Checked = false;
        this.securityHelper.SetNodeUpdateStatus(node, true);
      }
      this.setDirtyFlag(true);
      this.bInit = false;
      return false;
    }

    private void NodeChecked(TreeNode node)
    {
      if (this.bInit)
        return;
      this.NodeMouseUp(node);
    }

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      this.UpdatePermissions();
    }

    public void Reset() => this.ResetTree();

    public bool NeedToSaveData() => this.hasBeenModified();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelTPOAdministratorPage = new Panel();
      this.panelExTPOAdministrator = new PanelEx();
      this.chkAccessTPOAdminSite = new CheckBox();
      this.panelTPOAdministratorPage.SuspendLayout();
      this.panelExTPOAdministrator.SuspendLayout();
      this.SuspendLayout();
      this.panelTPOAdministratorPage.Controls.Add((Control) this.panelExTPOAdministrator);
      this.panelTPOAdministratorPage.Dock = DockStyle.Fill;
      this.panelTPOAdministratorPage.Location = new Point(0, 0);
      this.panelTPOAdministratorPage.Name = "panelTPOAdministratorPage";
      this.panelTPOAdministratorPage.Size = new Size(284, 262);
      this.panelTPOAdministratorPage.TabIndex = 0;
      this.panelExTPOAdministrator.Controls.Add((Control) this.chkAccessTPOAdminSite);
      this.panelExTPOAdministrator.Location = new Point(53, 77);
      this.panelExTPOAdministrator.Name = "panelExTPOAdministrator";
      this.panelExTPOAdministrator.Size = new Size(131, 30);
      this.panelExTPOAdministrator.TabIndex = 0;
      this.chkAccessTPOAdminSite.AutoSize = true;
      this.chkAccessTPOAdminSite.Location = new Point(12, 12);
      this.chkAccessTPOAdminSite.Name = "chkAccessTPOAdminSite";
      this.chkAccessTPOAdminSite.Size = new Size(139, 17);
      this.chkAccessTPOAdminSite.TabIndex = 0;
      this.chkAccessTPOAdminSite.Text = "Access TPO Connect Admin Site";
      this.chkAccessTPOAdminSite.UseVisualStyleBackColor = true;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.AutoScroll = true;
      this.ClientSize = new Size(284, 262);
      this.Controls.Add((Control) this.panelTPOAdministratorPage);
      this.Name = "TPOAdministrationPage";
      this.Text = "TPO Site Settings";
      this.panelTPOAdministratorPage.ResumeLayout(false);
      this.panelExTPOAdministrator.ResumeLayout(false);
      this.panelExTPOAdministrator.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

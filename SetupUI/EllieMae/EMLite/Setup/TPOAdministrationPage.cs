// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TPOAdministrationPage
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
  public class TPOAdministrationPage : PersonaTreePageBase
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

    public TPOAdministrationPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "TPO Connect Administration Settings";
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.personaID = personaId;
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new TPOAdministrationSecurityHelper(this.session, personaId);
      this.init();
    }

    public TPOAdministrationPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "TPO Connect Administration Settings";
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.bIsUserSetup = true;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new TPOAdministrationSecurityHelper(this.session, userId, personas);
      this.userID = userId;
      this.personaList = personas;
      this.init();
    }

    private void NodeMouseUp(TreeNode node)
    {
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
      this.Name = nameof (TPOAdministrationPage);
      this.Text = nameof (TPOAdministrationPage);
      this.panelTPOAdministratorPage.ResumeLayout(false);
      this.panelExTPOAdministrator.ResumeLayout(false);
      this.panelExTPOAdministrator.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

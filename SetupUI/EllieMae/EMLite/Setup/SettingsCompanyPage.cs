// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsCompanyPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SettingsCompanyPage : PersonaTreePageBase
  {
    private System.ComponentModel.Container components;
    private int personaID = -1;
    private Persona[] personaList;
    private string userID = "";

    public SettingsCompanyPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "Company Settings";
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.personaID = personaId;
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new SettingsCompanySecurityHelper(this.session, personaId);
      this.init();
    }

    public SettingsCompanyPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "Company Settings";
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.bIsUserSetup = true;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new SettingsCompanySecurityHelper(this.session, userId, personas);
      this.userID = userId;
      this.personaList = personas;
      this.init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleBaseSize = new Size(5, 13);
      this.AutoScroll = true;
      this.ClientSize = new Size(528, 361);
      this.Name = nameof (SettingsCompanyPage);
      this.ResumeLayout(false);
    }

    private void NodeMouseUp(TreeNode node)
    {
    }

    private void NodeChecked(TreeNode node)
    {
      if (!this.bInit)
        this.NodeMouseUp(node);
      if (!(node.Text == "Add LEI") && !(node.Text == "Edit LEI") || !node.Checked)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Please ensure 'HMDA Profiles' option under 'Loan Setup' setting is selected for the Persona to access the HMDA Profiles maintained.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      foreach (TreeNode node1 in node.Parent.Nodes)
        node1.Checked = true;
    }

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

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      this.UpdatePermissions();
    }

    public void Reset() => this.ResetTree();

    public bool NeedToSaveData() => this.hasBeenModified();
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EVaultSettingsPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EVaultSettingsPage : PersonaTreePageBase
  {
    private IContainer components;

    public EVaultSettingsPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "eVault Settings";
      this.CheckedChangedEvent += new PersonaTreeNodeCheckedChanged(this.CheckedChanged);
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new EVaultSecurityHelper(this.session, personaId);
      this.bInit = true;
      this.init();
    }

    public EVaultSettingsPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "eVault Settings";
      this.CheckedChangedEvent += new PersonaTreeNodeCheckedChanged(this.CheckedChanged);
      this.bIsUserSetup = true;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new EVaultSecurityHelper(this.session, userId, personas);
      this.bInit = true;
      this.init();
    }

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      this.UpdatePermissions();
    }

    public void Reset() => this.ResetTree();

    public bool NeedToSaveData() => this.hasBeenModified();

    public override void SetPersona(int personaId) => base.SetPersona(personaId);

    public override void MakeReadOnly(bool makeReadOnly)
    {
      base.MakeReadOnly(makeReadOnly);
      if (!makeReadOnly)
        return;
      this.Reset();
    }

    private bool CheckedChanged(TreeNode node)
    {
      if (node.Tag == null || (AclFeature) node.Tag != AclFeature.eVaultTab_eVaultPortal)
        return true;
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(528, 361);
      this.Name = nameof (EVaultSettingsPage);
      this.Text = "eVaultPage";
      this.ResumeLayout(false);
    }
  }
}

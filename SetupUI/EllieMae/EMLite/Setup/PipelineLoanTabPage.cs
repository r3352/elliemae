// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PipelineLoanTabPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PipelineLoanTabPage : PersonaTreePageBase
  {
    private IContainer components;
    private string pipelineLoanTabNodePath = "Access to Pipeline/Loan Tab";

    public event PipelineLoanTabPage.FeatureStatusChanged FeatureStateChanged;

    public event EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaAccess HasContactOriginateLoanAccessEvent;

    public PipelineLoanTabPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.bIsUserSetup = false;
      this.securityHelper = (IFeatureSecurityHelper) new PipelineLoanTabSecurityHelper(this.session, personaId);
      this.myInit();
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
    }

    private bool PipelineLoanTabPage_BeforeCheckedEvent(TreeNode node)
    {
      return this.securityHelper.NodeToFeature(node) != AclFeature.GlobalTab_Pipeline || this.HasContactOriginateLoanAccessEvent == null || !node.Checked || !this.HasContactOriginateLoanAccessEvent() || Utils.Dialog((IWin32Window) this, "If you clear this check box, the persona will lose their access to all the options on the Pipeline, Loan, and Forms/Tools tabs. Their access to the Originate Loan, Order Credit, and Product and Pricing buttons on the Contacts tab will be lost as well. Do you still want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel;
    }

    private void PipelineLoanTabPage_AfterCheckedEvent(TreeNode node)
    {
      if (this.securityHelper.NodeToFeature(node) != AclFeature.GlobalTab_Pipeline || this.FeatureStateChanged == null || this.HasContactOriginateLoanAccessEvent == null)
        return;
      if (!node.Checked && this.HasContactOriginateLoanAccessEvent())
        this.FeatureStateChanged(AclFeature.GlobalTab_Pipeline, node.Checked ? AclTriState.True : AclTriState.False, true);
      else
        this.FeatureStateChanged(AclFeature.GlobalTab_Pipeline, node.Checked ? AclTriState.True : AclTriState.False, false);
    }

    public PipelineLoanTabPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.bIsUserSetup = true;
      this.securityHelper = (IFeatureSecurityHelper) new PipelineLoanTabSecurityHelper(this.session, userId, personas);
      this.myInit();
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
    }

    private void myInit()
    {
      this.InitializeComponent();
      this.Title = "Pipeline/Loan Tabs";
      this.treeViewTabs.ShowLines = this.treeViewTabs.ShowRootLines = false;
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.PipelineLoanTabPage_AfterCheckedEvent);
      this.BeforeCheckedEvent += new PersonaTreeNodeBeforeChecked(this.PipelineLoanTabPage_BeforeCheckedEvent);
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
      this.BackColor = Color.White;
      this.ClientSize = new Size(528, 361);
      this.Name = nameof (PipelineLoanTabPage);
      this.ResumeLayout(false);
    }

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      this.UpdatePermissions();
    }

    public void Reset()
    {
      this.ResetTree();
      TreeNode treeNode = this.findTreeNode(this.pipelineLoanTabNodePath);
      if (treeNode == null)
        return;
      this.PipelineLoanTabPage_AfterCheckedEvent(treeNode);
    }

    public bool NeedToSaveData() => this.hasBeenModified();

    public void GrandAccessToPipelineLoanTab()
    {
      TreeNode treeNode = this.findTreeNode(this.pipelineLoanTabNodePath);
      if (treeNode == null || treeNode.Checked)
        return;
      treeNode.Checked = true;
    }

    public bool HasAccessToPipelineLoanTab()
    {
      TreeNode treeNode = this.findTreeNode(this.pipelineLoanTabNodePath);
      return treeNode != null && treeNode.Checked;
    }

    public delegate void FeatureStatusChanged(
      AclFeature feature,
      AclTriState state,
      bool gotoContactTab);
  }
}

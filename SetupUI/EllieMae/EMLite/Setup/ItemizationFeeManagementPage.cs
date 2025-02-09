// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ItemizationFeeManagementPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ItemizationFeeManagementPage : PersonaTreePageBase
  {
    private PipelineConfiguration pipelineConfiguration;

    public ItemizationFeeManagementPage(
      Sessions.Session session,
      int personaId,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.pipelineConfiguration = pipelineConfiguration;
      this.bIsUserSetup = false;
      this.securityHelper = (IFeatureSecurityHelper) new ItemizationFeeSecurityHelper(session, personaId);
      this.myInit();
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.init();
    }

    public ItemizationFeeManagementPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.pipelineConfiguration = pipelineConfiguration;
      this.bIsUserSetup = true;
      this.securityHelper = (IFeatureSecurityHelper) new ItemizationFeeSecurityHelper(session, userId, personas);
      this.myInit();
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.init();
    }

    private void myInit()
    {
      this.Title = "Itemization Fee Management";
      this.lblDescription.Text = "Select which fee attributes can be modified";
      this.lblDescription.Visible = true;
      this.treeViewTabs.ShowLines = true;
      this.treeViewTabs.ShowRootLines = true;
      this.pipelineConfiguration.FeatureStateChanged += new PipelineConfiguration.PipelineConfigChanged(this.pipelineConfiguration_FeatureStateChanged);
      this.pipelineConfiguration_FeatureStateChanged(AclFeature.GlobalTab_Pipeline, this.pipelineConfiguration.HasPipelineLoanTabAccess() ? AclTriState.True : AclTriState.False, false);
    }

    private void pipelineConfiguration_FeatureStateChanged(
      AclFeature feature,
      AclTriState state,
      bool gotoContact)
    {
      if (feature != AclFeature.GlobalTab_Pipeline)
        return;
      if (state == AclTriState.True)
      {
        this.treeViewTabs.Visible = true;
        this.setNoAccessLabel(false, AclFeature.GlobalTab_Pipeline);
      }
      else
      {
        this.treeViewTabs.Visible = false;
        this.setNoAccessLabel(true, AclFeature.GlobalTab_Pipeline);
      }
    }

    public override void setPermission(TreeNode node)
    {
      if (this.securityHelper.IsDependentOnChildren(node))
        return;
      this.securityHelper.SetNodeUpdateStatus(node, true);
      if (!(node.Text == "Seller amount, Seller Credit, Seller Obligated") && !(node.Text == "Broker, Lender, Other amounts") || !node.Checked)
        return;
      TreeNode treeNode = this.findTreeNode("Fee Amounts\\Borrower amount only");
      treeNode.Checked = true;
      this.securityHelper.SetNodeUpdateStatus(treeNode, true);
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

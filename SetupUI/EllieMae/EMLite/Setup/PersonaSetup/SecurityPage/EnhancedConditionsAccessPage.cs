// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.EnhancedConditionsAccessPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.SecurityPage
{
  public class EnhancedConditionsAccessPage : PersonaTreePageBase
  {
    private PipelineConfiguration pipelineConfiguration;
    public Guid conditionTypeID;
    private int personaID = -1;
    private string userID = "";
    private IContainer components;

    public EnhancedConditionsAccessPage(
      Sessions.Session session,
      int personaId,
      Guid enhancedConditionType,
      EventHandler dirtyFlagChanged,
      PipelineConfiguration pipelineConfiguration,
      bool isInvestorDelivery = false)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.pipelineConfiguration = pipelineConfiguration;
      this.Title = "Enhanced Conditions";
      this.bIsUserSetup = false;
      this.personaID = personaId;
      this.conditionTypeID = enhancedConditionType;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new EnhancedConditionSecurityHelper(this.session, personaId, enhancedConditionType, isInvestorDelivery);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.EnhancedConditionsAccessPage_AfterCheckedEvent);
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.init();
    }

    public EnhancedConditionsAccessPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      Guid enhancedConditionType,
      EventHandler dirtyFlagChanged,
      PipelineConfiguration pipelineConfiguration,
      bool isInvestorDelivery = false)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.pipelineConfiguration = pipelineConfiguration;
      this.Title = "Enhanced Conditions";
      this.bIsUserSetup = true;
      this.userID = userId;
      this.conditionTypeID = enhancedConditionType;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new EnhancedConditionSecurityHelper(this.session, userId, personas, enhancedConditionType, isInvestorDelivery);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.EnhancedConditionsAccessPage_AfterCheckedEvent);
      this.init();
    }

    public void ResetData() => this.ResetTree();

    public void ResetPersona(int personaID) => this.personaID = personaID;

    public void SaveData()
    {
      if (!this.NeedToSaveData())
        return;
      this.UpdatePermissions();
    }

    public Hashtable GetPersonaPermissions()
    {
      return ((IEnhancedConditionsAclManager) this.session.ACL.GetAclManager(AclCategory.EnhancedConditions)).GetPersonaPermissions(this.conditionTypeID, this.personaID);
    }

    protected override void UpdatePermissions()
    {
      Hashtable updatedFeatures = this.securityHelper.GetUpdatedFeatures(true);
      IEnumerator enumerator = updatedFeatures.Keys.GetEnumerator();
      while (enumerator.MoveNext())
      {
        AclEnhancedConditionType current = (AclEnhancedConditionType) enumerator.Current;
        AclTriState access = (AclTriState) updatedFeatures[enumerator.Current];
        if (!this.bIsUserSetup)
          ((EnhancedConditionsAclManager) this.session.ACL.GetAclManager(AclCategory.EnhancedConditions)).SetPermissions(current, this.personaID, access, this.conditionTypeID);
        else
          ((EnhancedConditionsAclManager) this.session.ACL.GetAclManager(AclCategory.EnhancedConditions)).SetPermissions(current, this.userID, access, this.conditionTypeID);
      }
      this.setDirtyFlag(false);
      this.setDirtyFlag(false);
    }

    public bool NeedToSaveData() => this.hasBeenModified();

    public void resetDirtyFlag(bool val) => this.setDirtyFlag(val);

    private void EnhancedConditionsAccessPage_AfterCheckedEvent(TreeNode node)
    {
      this.securityHelper.SetNodeUpdateStatus(node, node.Checked);
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
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(800, 450);
      this.Name = nameof (EnhancedConditionsAccessPage);
      this.Text = nameof (EnhancedConditionsAccessPage);
      this.ResumeLayout(false);
    }
  }
}

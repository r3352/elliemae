// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.ePass.UnassignedFilesPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity.ePass;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.ePass
{
  public class UnassignedFilesPage : PersonaTreePageBase
  {
    private PipelineConfiguration pipelineConfiguration;
    private AccessToDocumentTabPage accessToTabPage;
    private IContainer components;

    public UnassignedFilesPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged,
      PipelineConfiguration pipelineConfiguration,
      AccessToDocumentTabPage accessToTabPage)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.pipelineConfiguration = pipelineConfiguration;
      this.accessToTabPage = accessToTabPage;
      this.Title = "Unassigned Files (File Manager)";
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new ePassUnassignedFilesHelper(this.session, personaId);
      this.init();
      this.myInit();
    }

    public UnassignedFilesPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged,
      PipelineConfiguration pipelineConfiguration,
      AccessToDocumentTabPage accessToTabPage)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.pipelineConfiguration = pipelineConfiguration;
      this.accessToTabPage = accessToTabPage;
      this.Title = "Unassigned Files (File Manager)";
      this.bIsUserSetup = true;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new ePassUnassignedFilesHelper(this.session, userId, personas);
      this.init();
      this.myInit();
    }

    private void myInit()
    {
      if (this.pipelineConfiguration != null)
      {
        this.pipelineConfiguration.FeatureStateChanged += new PipelineConfiguration.PipelineConfigChanged(this.pipelineConfiguration_FeatureStateChanged);
        this.pipelineConfiguration_FeatureStateChanged(AclFeature.GlobalTab_Pipeline, this.pipelineConfiguration.HasPipelineLoanTabAccess() ? AclTriState.True : AclTriState.False, false);
      }
      if (this.accessToTabPage == null)
        return;
      this.accessToTabPage.FeatureStateChanged += new AccessToDocumentTabPage.FeatureStatusChanged(this.accessToTabPage_FeatureStateChanged);
      this.accessToTabPage_FeatureStateChanged(AclFeature.eFolder_AccessToDocumentTab, this.accessToTabPage.HasAccessToDocumentTab() ? AclTriState.True : AclTriState.False);
    }

    private void accessToTabPage_FeatureStateChanged(AclFeature feature, AclTriState state)
    {
      if (this.pipelineConfiguration == null || !this.pipelineConfiguration.HasPipelineLoanTabAccess() || feature != AclFeature.eFolder_AccessToDocumentTab)
        return;
      if (state == AclTriState.True)
      {
        this.treeViewTabs.Visible = true;
        this.setNoAccessLabel(false, AclFeature.eFolder_AccessToDocumentTab);
      }
      else
      {
        this.treeViewTabs.Visible = false;
        this.setNoAccessLabel(true, AclFeature.eFolder_AccessToDocumentTab);
      }
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
        if (!this.accessToTabPage.HasAccessToDocumentTab())
        {
          this.accessToTabPage_FeatureStateChanged(AclFeature.eFolder_AccessToDocumentTab, AclTriState.False);
        }
        else
        {
          this.treeViewTabs.Visible = true;
          this.setNoAccessLabel(false, AclFeature.GlobalTab_Pipeline);
        }
      }
      else
      {
        this.treeViewTabs.Visible = false;
        this.setNoAccessLabel(true, AclFeature.GlobalTab_Pipeline);
      }
    }

    public void SaveData()
    {
      if (!this.NeedToSaveData())
        return;
      this.UpdatePermissions();
    }

    public void ResetData() => this.ResetTree();

    public bool NeedToSaveData() => this.hasBeenModified();

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
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(490, 364);
      this.Name = nameof (UnassignedFilesPage);
      this.ResumeLayout(false);
    }
  }
}

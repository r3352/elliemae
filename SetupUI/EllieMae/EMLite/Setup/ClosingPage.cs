// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ClosingPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ClosingPage : PersonaTreePageBase
  {
    private ePassServices serviceDlg;
    private int selectOption = 2;
    private Hashtable cachedData = new Hashtable();
    private int personaID;
    private string userID;
    private Persona[] personaList;
    private PipelineConfiguration pipelineConfiguration;
    private IContainer components;

    public ClosingPage(
      Sessions.Session session,
      int personaId,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.pipelineConfiguration = pipelineConfiguration;
      this.personaID = personaId;
      this.bIsUserSetup = false;
      this.securityHelper = (IFeatureSecurityHelper) new ClosingSecurityHelper(this.session, personaId);
      this.myInit();
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.init();
    }

    public ClosingPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.pipelineConfiguration = pipelineConfiguration;
      this.bIsUserSetup = true;
      this.userID = userId;
      this.personaList = personas;
      this.securityHelper = (IFeatureSecurityHelper) new ClosingSecurityHelper(this.session, userId, personas);
      this.myInit();
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.init();
    }

    private void myInit()
    {
      this.Title = "Other";
      this.treeViewTabs.ShowLines = false;
      this.treeViewTabs.ShowRootLines = false;
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.InitialSpecialDepNodes();
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

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      if (this.serviceDlg != null && this.serviceDlg.HasBeenModified)
        this.serviceDlg.SaveData();
      this.UpdatePermissions();
    }

    public void Reset()
    {
      this.InitialSpecialDepNodes();
      this.ResetTree();
      this.serviceDlg = (ePassServices) null;
      this.cachedData = new Hashtable();
    }

    public int PersonaID
    {
      get => this.personaID;
      set
      {
        if (this.personaID != value)
        {
          this.personaID = value;
          this.Reset();
        }
        else
          this.personaID = value;
      }
    }

    public bool NeedToSaveData()
    {
      return this.serviceDlg != null && this.serviceDlg.HasBeenModified || this.hasBeenModified();
    }

    private void NodeMouseUp(TreeNode node)
    {
      if (!this.pipelineConfiguration.HasPipelineLoanTabAccess() || node.Tag == null || (AclFeature) node.Tag != AclFeature.LoanTab_Other_ePASS)
        return;
      if (!this.bIsUserSetup)
      {
        if (this.serviceDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanTab_Other_ePASS))
          {
            this.serviceDlg = new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.personaID, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanTab_Other_ePASS] = (object) this.serviceDlg.DataView;
          }
          this.serviceDlg = new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.personaID, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.serviceDlg.IsReadOnly = this.readOnly;
      }
      else if (this.serviceDlg == null || this.selectOption < 2)
      {
        if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanTab_Other_ePASS))
        {
          this.serviceDlg = new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.userID, this.personaList, this.readOnly, 2);
          this.cachedData[(object) AclFeature.LoanTab_Other_ePASS] = (object) this.serviceDlg.DataView;
        }
        this.serviceDlg = new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.userID, this.personaList, this.readOnly, this.selectOption);
        this.selectOption = 2;
      }
      else
        this.serviceDlg.IsReadOnly = this.readOnly;
      if (this.cachedData[(object) AclFeature.LoanTab_Other_ePASS] != null)
        this.serviceDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanTab_Other_ePASS];
      if (DialogResult.OK == this.serviceDlg.ShowDialog((IWin32Window) this))
      {
        this.cachedData[(object) AclFeature.LoanTab_Other_ePASS] = (object) this.serviceDlg.DataView;
        if (this.serviceDlg.HasBeenModified)
          this.setDirtyFlag(true);
      }
      if (this.serviceDlg.HasSomethingChecked() && !node.Checked)
      {
        this.bInit = true;
        node.Checked = true;
        this.bInit = false;
      }
      else if (!this.serviceDlg.HasSomethingChecked() && node.Checked)
      {
        this.bInit = true;
        node.Checked = false;
        this.bInit = false;
      }
      if (!this.bIsUserSetup)
        return;
      node.ImageIndex = this.serviceDlg.GetImageIndex();
      node.SelectedImageIndex = this.serviceDlg.GetImageIndex();
    }

    private void NodeChecked(TreeNode node)
    {
      if (this.bInit || node.Tag == null || (AclFeature) node.Tag != AclFeature.LoanTab_Other_ePASS)
        return;
      this.selectOption = !node.Checked ? 0 : 1;
      this.NodeMouseUp(node);
    }

    private void InitialSpecialDepNodes()
    {
      this.serviceDlg = this.bIsUserSetup ? new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.userID, this.personaList, this.readOnly, this.selectOption) : new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.personaID, this.readOnly, this.selectOption);
      Hashtable specialDepTreeNodes1 = new Hashtable();
      Hashtable specialDepTreeNodes2 = new Hashtable();
      if (this.serviceDlg.HasSomethingChecked())
        specialDepTreeNodes1.Add((object) "Manage Service Providers List", (object) true);
      else
        specialDepTreeNodes1.Add((object) "Manage Service Providers List", (object) false);
      specialDepTreeNodes2.Add((object) "Manage Service Providers List", (object) this.serviceDlg.GetImageIndex());
      this.securityHelper.setSpecialDepTreeNodes(specialDepTreeNodes1);
      this.securityHelper.setSpecialDepTreeNodesImg(specialDepTreeNodes2);
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
      this.Name = nameof (ClosingPage);
      this.Text = "ContactsMgrPage";
      this.ResumeLayout(false);
    }
  }
}

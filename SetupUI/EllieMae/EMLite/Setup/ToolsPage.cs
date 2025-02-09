// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ToolsPage
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
  public class ToolsPage : PersonaTreePageBase
  {
    private IContainer components;
    private int personaID = -1;
    private int selectOption = 2;
    private string userID = "";
    private Persona[] personaList;
    private bool isPersonal;
    private GrantWriteAccessDlg popUp;
    private Hashtable cachedData;
    private PipelineConfiguration pipelineConfiguration;

    public ToolsPage(
      Sessions.Session session,
      int personaId,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.pipelineConfiguration = pipelineConfiguration;
      this.Title = "Tools";
      this.cachedData = new Hashtable();
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new ToolsSecurityHelper(this.session, personaId);
      this.personaID = personaId;
      this.bInit = true;
      this.InitialSpecialDepNodes();
      this.init();
      this.myInit();
    }

    public ToolsPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.pipelineConfiguration = pipelineConfiguration;
      this.Title = "Tools";
      this.cachedData = new Hashtable();
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.bIsUserSetup = true;
      this.isPersonal = true;
      this.userID = userId;
      this.personaList = personas;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new ToolsSecurityHelper(this.session, userId, personas);
      this.bInit = true;
      this.InitialSpecialDepNodes();
      this.init();
      this.myInit();
    }

    private void myInit()
    {
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ToolsPage));
      this.gcTreeView.SuspendLayout();
      this.SuspendLayout();
      this.treeViewTabs.LineColor = Color.Black;
      this.treeViewTabs.Size = new Size(526, 296);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.gcTreeView.Size = new Size(528, 323);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(528, 360);
      this.Name = nameof (ToolsPage);
      this.gcTreeView.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override void SetPersona(int personaId)
    {
      base.SetPersona(personaId);
      this.PersonaID = personaId;
    }

    private void NodeMouseUp(TreeNode node)
    {
      if (!this.pipelineConfiguration.HasPipelineLoanTabAccess() || node.Tag == null || (AclFeature) node.Tag != AclFeature.ToolsTab_GrantWriteAccess)
        return;
      if (!this.isPersonal)
      {
        if (this.popUp == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.ToolsTab_GrantWriteAccess))
          {
            this.popUp = new GrantWriteAccessDlg(this.session, this.personaID, this.readOnly, 2);
            this.cachedData[(object) AclFeature.ToolsTab_GrantWriteAccess] = (object) this.popUp.DataView;
          }
          this.popUp = new GrantWriteAccessDlg(this.session, this.personaID, this.readOnly, this.selectOption);
          this.cachedData[(object) AclFeature.ToolsTab_GrantWriteAccess] = (object) this.popUp.DataView;
          this.selectOption = 2;
        }
        else
          this.popUp.IsReadOnly = this.readOnly;
      }
      else if (this.popUp == null || this.selectOption < 2)
      {
        if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.ToolsTab_GrantWriteAccess))
        {
          this.popUp = new GrantWriteAccessDlg(this.session, this.userID, this.personaList, this.readOnly, 2);
          this.cachedData[(object) AclFeature.ToolsTab_GrantWriteAccess] = (object) this.popUp.DataView;
        }
        this.popUp = new GrantWriteAccessDlg(this.session, this.userID, this.personaList, this.readOnly, this.selectOption);
        this.cachedData[(object) AclFeature.ToolsTab_GrantWriteAccess] = (object) this.popUp.DataView;
        this.selectOption = 2;
      }
      else
        this.popUp.IsReadOnly = this.readOnly;
      if (this.cachedData[(object) AclFeature.ToolsTab_GrantWriteAccess] != null)
        this.popUp.DataView = (ArrayList) this.cachedData[(object) AclFeature.ToolsTab_GrantWriteAccess];
      if (DialogResult.OK == this.popUp.ShowDialog((IWin32Window) this))
      {
        this.cachedData[(object) AclFeature.ToolsTab_GrantWriteAccess] = (object) this.popUp.DataView;
        this.raiseDirtyFlagChangedEvent((object) this, (EventArgs) null);
      }
      if (this.popUp.HasSomethingChecked() && !node.Checked)
      {
        this.bInit = true;
        node.Checked = true;
        this.bInit = false;
      }
      else if (!this.popUp.HasSomethingChecked() && node.Checked)
      {
        this.bInit = true;
        node.Checked = false;
        this.bInit = false;
      }
      if (!this.isPersonal)
        return;
      node.ImageIndex = this.popUp.GetImageIndex();
      node.SelectedImageIndex = this.popUp.GetImageIndex();
    }

    private void InitialSpecialDepNodes()
    {
      if (!this.isPersonal)
      {
        this.popUp = new GrantWriteAccessDlg(this.session, this.personaID, this.readOnly, this.selectOption);
        this.popUp.setView();
      }
      else
      {
        this.popUp = new GrantWriteAccessDlg(this.session, this.userID, this.personaList, this.readOnly, this.selectOption);
        this.popUp.setView();
      }
      Hashtable specialDepTreeNodes1 = new Hashtable();
      Hashtable specialDepTreeNodes2 = new Hashtable();
      if (this.popUp.HasSomethingChecked())
        specialDepTreeNodes1.Add((object) "Grant Write Access to Loan Team Members", (object) true);
      else
        specialDepTreeNodes1.Add((object) "Grant Write Access to Loan Team Members", (object) false);
      specialDepTreeNodes2.Add((object) "Grant Write Access to Loan Team Members", (object) this.popUp.GetImageIndex());
      this.securityHelper.setSpecialDepTreeNodes(specialDepTreeNodes1);
      this.securityHelper.setSpecialDepTreeNodesImg(specialDepTreeNodes2);
    }

    private bool makeCheck(TreeNode node, string nodeName)
    {
      bool flag = false;
      if (node.Text == nodeName)
      {
        node.Checked = true;
        return true;
      }
      foreach (TreeNode node1 in node.Nodes)
      {
        if (this.makeCheck(node1, nodeName))
        {
          node.Checked = true;
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void NodeChecked(TreeNode node)
    {
      if (this.bInit)
        return;
      if (node.Tag != null && (AclFeature) node.Tag == AclFeature.ToolsTab_GrantWriteAccess)
      {
        this.selectOption = !node.Checked ? 0 : 1;
        this.NodeMouseUp(node);
      }
      else
      {
        if (!(node.Text == "File Contacts") && !(node.Text == "Access to the Tools Tab"))
          return;
        this.selectOption = !node.Checked ? 0 : 1;
        this.popUp = this.isPersonal ? new GrantWriteAccessDlg(this.session, this.userID, this.personaList, this.readOnly, this.selectOption) : new GrantWriteAccessDlg(this.session, this.personaID, this.readOnly, this.selectOption);
        this.popUp.setView();
        this.cachedData[(object) AclFeature.ToolsTab_GrantWriteAccess] = (object) this.popUp.DataView;
        this.selectOption = 2;
      }
    }

    public void SaveData()
    {
      if (!this.NeedToSaveData())
        return;
      if (this.popUp != null && this.popUp.HasBeenModified())
        this.popUp.SaveData();
      this.UpdatePermissions();
    }

    public void ResetData()
    {
      this.InitialSpecialDepNodes();
      this.ResetTree();
      this.popUp = (GrantWriteAccessDlg) null;
      this.cachedData.Clear();
    }

    public bool NeedToSaveData()
    {
      return this.popUp != null && this.popUp.HasBeenModified() || this.hasBeenModified();
    }

    public int PersonaID
    {
      get => this.personaID;
      set
      {
        if (this.personaID != value)
        {
          this.personaID = value;
          this.ResetData();
        }
        else
          this.personaID = value;
      }
    }

    public override void MakeReadOnly(bool makeReadOnly)
    {
      base.MakeReadOnly(makeReadOnly);
      if (!makeReadOnly)
        return;
      this.ResetData();
    }

    public void ResetDirtyFlag() => this.popUp = (GrantWriteAccessDlg) null;
  }
}

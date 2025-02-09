// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOStandardFeaturesPage
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
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LOStandardFeaturesPage : PersonaTreePageBase
  {
    private IContainer components;
    private int personaID = -1;
    private string userID = "";
    private Persona[] personaList;
    private GrantWriteAccessDlg popUp;
    private Hashtable cachedData;

    public LOStandardFeaturesPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "Standard Features";
      this.cachedData = new Hashtable();
      this.CheckedChangedEvent += new PersonaTreeNodeCheckedChanged(this.CheckedChanged);
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new LOStandardFeaturesSecurityHelper(this.session, personaId);
      this.personaID = personaId;
      this.bInit = true;
      this.init();
    }

    public LOStandardFeaturesPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "Standard Features";
      this.cachedData = new Hashtable();
      this.CheckedChangedEvent += new PersonaTreeNodeCheckedChanged(this.CheckedChanged);
      this.bIsUserSetup = true;
      this.userID = userId;
      this.personaList = personas;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new LOStandardFeaturesSecurityHelper(this.session, userId, personas);
      this.bInit = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LOStandardFeaturesPage));
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
      this.Name = nameof (LOStandardFeaturesPage);
      this.gcTreeView.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override void SetPersona(int personaId)
    {
      base.SetPersona(personaId);
      this.PersonaID = personaId;
    }

    private bool CheckedChanged(TreeNode node)
    {
      if (node.Tag == null || (AclFeature) node.Tag != AclFeature.LOConnectTab_WorkflowTasksTool && (AclFeature) node.Tag != AclFeature.LOConnectTab_DeleteAllTasks && (AclFeature) node.Tag != AclFeature.LOConnectTab_UnderwritingCenter && (AclFeature) node.Tag != AclFeature.LOConnectTab_AllServices && (AclFeature) node.Tag != AclFeature.LOConnectTab_PipelineDefault && (AclFeature) node.Tag != AclFeature.LOConnectTab_LoanPipelineOption && (AclFeature) node.Tag != AclFeature.LOConnectTab_TaskPipelineOption && (AclFeature) node.Tag != AclFeature.LOConnectTab_MiCenter)
        return true;
      this.securityHelper.SetNodeUpdateStatus(node, true);
      if ((AclFeature) node.Tag == AclFeature.LOConnectTab_DeleteAllTasks)
      {
        if (!node.Checked)
          return true;
        this.bInit = false;
        node.PrevNode.Checked = true;
        this.securityHelper.SetNodeUpdateStatus(node.PrevNode, true);
      }
      if ((AclFeature) node.Tag == AclFeature.LOConnectTab_AllServices)
      {
        if (!node.Checked && this.isAllChildCheckBoxesUnChecked(node.Parent))
          this.setNode(node, false);
        return true;
      }
      if ((AclFeature) node.Tag == AclFeature.LOConnectTab_UnderwritingCenter)
      {
        if (node.Checked)
        {
          this.setNode(node, true);
          return false;
        }
        if (this.isAllChildCheckBoxesUnChecked(node.Parent))
          this.setNode(node, false);
        return true;
      }
      if ((AclFeature) node.Tag == AclFeature.LOConnectTab_PipelineDefault)
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
        this.bInit = false;
        return true;
      }
      if ((AclFeature) node.Tag == AclFeature.LOConnectTab_LoanPipelineOption)
      {
        if (node.Checked)
        {
          node.NextNode.Checked = false;
          this.securityHelper.SetNodeUpdateStatus(node.NextNode, true);
          this.securityHelper.SetNodeUpdateStatus(node, true);
        }
        else if (this.isAllChildCheckBoxesUnChecked(node.Parent))
          this.setNode(node, false);
        return true;
      }
      if ((AclFeature) node.Tag == AclFeature.LOConnectTab_TaskPipelineOption)
        return true;
      if ((AclFeature) node.Tag == AclFeature.LOConnectTab_MiCenter)
      {
        if (node.Checked)
        {
          this.setNode(node, true);
          return false;
        }
        if (this.isAllChildCheckBoxesUnChecked(node.Parent))
          this.setNode(node, false);
        return true;
      }
      this.setDirtyFlag(true);
      return false;
    }

    private bool isAllChildCheckBoxesUnChecked(TreeNode node)
    {
      return node.Nodes.OfType<TreeNode>().Where<TreeNode>((Func<TreeNode, bool>) (nd => nd.Checked)).Count<TreeNode>() <= 0;
    }

    private void setNode(TreeNode node, bool checkedValue)
    {
      node.Parent.Checked = checkedValue;
      this.securityHelper.SetNodeUpdateStatus(node.Parent, true);
      this.setDirtyFlag(true);
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

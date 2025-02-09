// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaTreePageBase
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.SecurityPage
{
  public class PersonaTreePageBase : Form, IPersonaSecurityPage
  {
    private bool dirty;
    protected bool bInit;
    protected bool bIsUserSetup;
    protected bool readOnly;
    protected IFeatureSecurityHelper securityHelper;
    private IContainer components;
    protected TreeView treeViewTabs;
    protected ImageList imgListTv;
    protected ContextMenu contextMenu1;
    protected MenuItem menuItemCheckAll;
    protected MenuItem menuItemUncheckAll;
    private MenuItem menuItemExpandAll;
    private MenuItem menuItemCollapseAll;
    private MenuItem menuItem1;
    protected MenuItem menuItemLinkWithPersona;
    protected MenuItem menuItemDisconnectFromPersona;
    private Label lblNoAccess;
    protected GroupContainer gcTreeView;
    protected Label lblDescription;
    protected Sessions.Session session;

    public event EventHandler DirtyFlagChanged;

    public event PersonaTreeNodeAfterChecked AfterCheckedEvent;

    public event PersonaTreeNodeBeforeChecked BeforeCheckedEvent;

    public event PersonaTreeNodeCheckedChanged CheckedChangedEvent;

    public event PersonaTreeNodeMouseUp MouseUpEvent;

    public PersonaTreePageBase()
    {
    }

    public PersonaTreePageBase(Sessions.Session session, EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.session = session;
      this.treeViewTabs.BackColor = this.gcTreeView.BackColor;
      this.DirtyFlagChanged += dirtyFlagChanged;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PersonaTreePageBase));
      this.treeViewTabs = new TreeView();
      this.contextMenu1 = new ContextMenu();
      this.menuItemCheckAll = new MenuItem();
      this.menuItemUncheckAll = new MenuItem();
      this.menuItemExpandAll = new MenuItem();
      this.menuItemCollapseAll = new MenuItem();
      this.menuItem1 = new MenuItem();
      this.menuItemLinkWithPersona = new MenuItem();
      this.menuItemDisconnectFromPersona = new MenuItem();
      this.imgListTv = new ImageList(this.components);
      this.gcTreeView = new GroupContainer();
      this.lblNoAccess = new Label();
      this.lblDescription = new Label();
      this.gcTreeView.SuspendLayout();
      this.SuspendLayout();
      this.treeViewTabs.BorderStyle = BorderStyle.None;
      this.treeViewTabs.CheckBoxes = true;
      this.treeViewTabs.ContextMenu = this.contextMenu1;
      this.treeViewTabs.Dock = DockStyle.Fill;
      this.treeViewTabs.Location = new Point(1, 73);
      this.treeViewTabs.Name = "treeViewTabs";
      this.treeViewTabs.Size = new Size(526, 287);
      this.treeViewTabs.TabIndex = 0;
      this.treeViewTabs.BeforeCheck += new TreeViewCancelEventHandler(this.treeViewTabs_BeforeCheck);
      this.treeViewTabs.AfterCheck += new TreeViewEventHandler(this.treeViewTabs_AfterCheck);
      this.treeViewTabs.MouseDown += new MouseEventHandler(this.treeViewTabs_MouseDown);
      this.treeViewTabs.MouseUp += new MouseEventHandler(this.treeViewTabs_MouseUp);
      this.contextMenu1.MenuItems.AddRange(new MenuItem[7]
      {
        this.menuItemCheckAll,
        this.menuItemUncheckAll,
        this.menuItemExpandAll,
        this.menuItemCollapseAll,
        this.menuItem1,
        this.menuItemLinkWithPersona,
        this.menuItemDisconnectFromPersona
      });
      this.menuItemCheckAll.Index = 0;
      this.menuItemCheckAll.Text = "Check All";
      this.menuItemCheckAll.Click += new EventHandler(this.menuItemCheckAll_Click);
      this.menuItemUncheckAll.Index = 1;
      this.menuItemUncheckAll.Text = "Uncheck All";
      this.menuItemUncheckAll.Click += new EventHandler(this.menuItemUncheckAll_Click);
      this.menuItemExpandAll.Index = 2;
      this.menuItemExpandAll.Text = "Expand All";
      this.menuItemExpandAll.Click += new EventHandler(this.menuItemExpandAll_Click);
      this.menuItemCollapseAll.Index = 3;
      this.menuItemCollapseAll.Text = "Collapse All";
      this.menuItemCollapseAll.Click += new EventHandler(this.menuItemCollapseAll_Click);
      this.menuItem1.Index = 4;
      this.menuItem1.Text = "-";
      this.menuItemLinkWithPersona.Index = 5;
      this.menuItemLinkWithPersona.Text = "Link with Persona Rights";
      this.menuItemLinkWithPersona.Click += new EventHandler(this.menuItemLinkWithPersona_Click);
      this.menuItemDisconnectFromPersona.Index = 6;
      this.menuItemDisconnectFromPersona.Text = "Disconnect from Persona Rights";
      this.menuItemDisconnectFromPersona.Click += new EventHandler(this.menuItemDisconnectFromPersona_Click);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.gcTreeView.Controls.Add((Control) this.treeViewTabs);
      this.gcTreeView.Controls.Add((Control) this.lblNoAccess);
      this.gcTreeView.Controls.Add((Control) this.lblDescription);
      this.gcTreeView.Dock = DockStyle.Fill;
      this.gcTreeView.HeaderForeColor = SystemColors.ControlText;
      this.gcTreeView.Location = new Point(0, 0);
      this.gcTreeView.Name = "gcTreeView";
      this.gcTreeView.Size = new Size(528, 361);
      this.gcTreeView.TabIndex = 1;
      this.gcTreeView.Text = "<title>";
      this.lblNoAccess.BackColor = Color.Transparent;
      this.lblNoAccess.Dock = DockStyle.Top;
      this.lblNoAccess.Location = new Point(1, 39);
      this.lblNoAccess.Name = "lblNoAccess";
      this.lblNoAccess.Size = new Size(526, 34);
      this.lblNoAccess.TabIndex = 1;
      this.lblNoAccess.Text = "The persona does not have access to the Pipeline, Loan, Forms and Tools, ePass tabs.";
      this.lblNoAccess.Visible = false;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Dock = DockStyle.Top;
      this.lblDescription.Location = new Point(1, 26);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(0, 13);
      this.lblDescription.TabIndex = 2;
      this.lblDescription.Visible = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(528, 361);
      this.Controls.Add((Control) this.gcTreeView);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (PersonaTreePageBase);
      this.Text = "PersonaSettingsPageBase";
      this.gcTreeView.ResumeLayout(false);
      this.gcTreeView.PerformLayout();
      this.ResumeLayout(false);
    }

    public bool ShowGroupContainer
    {
      get => this.gcTreeView.Visible;
      set
      {
        this.gcTreeView.Visible = value;
        if (value)
        {
          this.gcTreeView.Controls.Remove((Control) this.treeViewTabs);
          this.Controls.Remove((Control) this.treeViewTabs);
          this.Controls.Remove((Control) this.gcTreeView);
          this.Controls.Add((Control) this.gcTreeView);
          this.gcTreeView.Controls.Add((Control) this.treeViewTabs);
        }
        else
        {
          this.gcTreeView.Controls.Remove((Control) this.treeViewTabs);
          this.Controls.Remove((Control) this.treeViewTabs);
          this.Controls.Remove((Control) this.gcTreeView);
          this.Controls.Add((Control) this.treeViewTabs);
        }
      }
    }

    public string Title
    {
      set => this.gcTreeView.Text = value;
    }

    protected void setDirtyFlag(bool val)
    {
      this.dirty = val;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, val ? new EventArgs() : (EventArgs) null);
    }

    protected void setNoAccessLabel(bool display, AclFeature feature)
    {
      if (!display)
      {
        this.lblNoAccess.Visible = false;
      }
      else
      {
        this.lblNoAccess.Visible = true;
        if (feature != AclFeature.GlobalTab_Pipeline)
        {
          if (feature != AclFeature.eFolder_AccessToDocumentTab)
            return;
          this.lblNoAccess.Text = "The persona does not have access to the Document Tab.";
        }
        else
          this.lblNoAccess.Text = "The persona does not have access to the Pipeline, Loan, eFolder, Forms and Tools tabs.";
      }
    }

    protected void init()
    {
      this.bInit = true;
      this.securityHelper.BuildNodes(this.treeViewTabs);
      this.SetNodeStates();
      this.bInit = false;
      this.treeViewTabs.ShowLines = true;
      this.treeViewTabs.ShowRootLines = true;
    }

    public virtual void SetPersona(int personaId)
    {
      this.securityHelper.SetPersonaId(personaId);
      this.SetNodeStates();
      this.setDirtyFlag(false);
    }

    public void SetNodeStates()
    {
      this.bInit = true;
      this.securityHelper.UncheckAllDependentNodes();
      this.securityHelper.SetNodeStates();
      this.bInit = false;
    }

    protected void treeViewTabs_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (this.bInit)
        return;
      this.bInit = true;
      if (this.AfterCheckedEvent != null)
        this.AfterCheckedEvent(e.Node);
      this.treeNodeCheckChanged(e.Node);
      this.setDirtyFlag(true);
      this.bInit = false;
    }

    private void treeNodeCheckChanged(TreeNode node)
    {
      if (this.bIsUserSetup && node.SelectedImageIndex == 0)
        this.securityHelper.SetNodeImageIndex(node, 1);
      if (this.CheckedChangedEvent != null && !this.CheckedChangedEvent(node))
        return;
      if (node.Nodes.Count > 0)
      {
        if (node.Checked)
        {
          foreach (TreeNode node1 in node.Nodes)
            this.checkNodeAndItsChildren(node1);
        }
        else
        {
          bool flag1 = false;
          bool flag2 = false;
          foreach (TreeNode node2 in node.Nodes)
          {
            if (node2.ForeColor == SystemColors.GrayText)
            {
              flag1 = true;
              flag2 = node2.Checked;
            }
            this.uncheckNodeAndItsChildren(node2);
            if (flag1 & flag2)
              node.Checked = flag2;
          }
        }
      }
      this.setPermission(node);
      if (string.Compare(node.Text, "LO Comp Tool", true) == 0 && !node.Checked)
        this.securityHelper.SetNodeUpdateStatus(node, true);
      bool flag = false;
      if (node.Tag != null)
      {
        switch ((AclFeature) node.Tag)
        {
          case AclFeature.eFolder_Other_ManageAccessToDocs:
          case AclFeature.eFolder_AttachDoc:
          case AclFeature.eFolder_DeleteDoc:
          case AclFeature.eFolder_RequestDoc:
          case AclFeature.eFolder_SendDoc:
          case AclFeature.eFolder_AssignAs:
            flag = true;
            break;
        }
      }
      if (flag)
        return;
      if (node.Checked)
        this.checkNodeAndItsParent(node.Parent);
      else
        this.uncheckDependentParent(node.Parent);
    }

    private void uncheckDependentParent(TreeNode node)
    {
      if (node == null || !this.securityHelper.IsDependentOnChildren(node))
        return;
      bool flag = true;
      foreach (TreeNode node1 in node.Nodes)
      {
        if (node1.Checked)
          flag = false;
      }
      if (!flag)
        return;
      if (node.Checked)
      {
        node.Checked = false;
        if (this.bIsUserSetup && node.SelectedImageIndex == 0)
          this.securityHelper.SetNodeImageIndex(node, 1);
      }
      this.uncheckDependentParent(node.Parent);
    }

    public void checkNodeAndItsParent(TreeNode node)
    {
      if (node == null || node.Checked)
        return;
      if (this.bIsUserSetup && node.SelectedImageIndex == 0)
        this.securityHelper.SetNodeImageIndex(node, 1);
      node.Checked = true;
      this.setPermission(node);
      if (node.Tag != null && (AclFeature) node.Tag == AclFeature.eFolder_Other_ManageAccessToDocs)
        return;
      this.checkNodeAndItsParent(node.Parent);
    }

    private void checkNodeAndItsChildren(TreeNode node)
    {
      if (node.ForeColor == SystemColors.GrayText)
        return;
      if (this.bIsUserSetup && node.SelectedImageIndex == 0)
        this.securityHelper.SetNodeImageIndex(node, 1);
      if (node.Text != "Allow loan files to be opened for data (slower performance)" && node.Text != "Auto Exclusive Lock")
        node.Checked = true;
      if (node.Nodes.Count > 0)
      {
        foreach (TreeNode node1 in node.Nodes)
        {
          if (!(node1.ForeColor == SystemColors.GrayText))
            this.checkNodeAndItsChildren(node1);
        }
      }
      this.setPermission(node);
    }

    private void uncheckNodeAndItsChildren(TreeNode node)
    {
      if (node.ForeColor == SystemColors.GrayText)
        return;
      if (this.bIsUserSetup && node.SelectedImageIndex == 0)
        this.securityHelper.SetNodeImageIndex(node, 1);
      node.Checked = false;
      if (node.Nodes.Count > 0)
      {
        foreach (TreeNode node1 in node.Nodes)
        {
          if (!(node1.ForeColor == SystemColors.GrayText))
            this.uncheckNodeAndItsChildren(node1);
        }
      }
      this.setPermission(node);
    }

    public virtual void setPermission(TreeNode node)
    {
      if (this.securityHelper.IsDependentOnChildren(node))
        return;
      this.securityHelper.SetNodeUpdateStatus(node, true);
    }

    protected virtual void UpdatePermissions()
    {
      Hashtable updatedFeatures = this.securityHelper.GetUpdatedFeatures(this.bIsUserSetup);
      IEnumerator enumerator = updatedFeatures.Keys.GetEnumerator();
      while (enumerator.MoveNext())
        this.securityHelper.SetPermission((AclFeature) enumerator.Current, (AclTriState) updatedFeatures[enumerator.Current]);
      this.setDirtyFlag(false);
    }

    protected bool hasBeenModified() => this.dirty;

    protected void menuItemLinkWithPersona_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.treeViewTabs.SelectedNode;
      AclFeature feature = this.securityHelper.NodeToFeature(selectedNode);
      bool personaPermission = (bool) this.securityHelper.GetPersonaPermissions()[(object) feature];
      if (selectedNode.Checked != personaPermission)
        selectedNode.Checked = personaPermission;
      this.securityHelper.SetNodeImageIndex(selectedNode, 0);
      this.securityHelper.SetNodeUpdateStatus(selectedNode, true);
      this.setDirtyFlag(true);
    }

    protected void menuItemDisconnectFromPersona_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.treeViewTabs.SelectedNode;
      this.securityHelper.SetNodeImageIndex(selectedNode, 1);
      this.securityHelper.SetNodeUpdateStatus(selectedNode, true);
      this.setDirtyFlag(true);
    }

    protected void treeViewTabs_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      TreeNode nodeAt = this.treeViewTabs.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.menuItemCheckAll.Enabled = true;
      this.menuItemUncheckAll.Enabled = true;
      if (this.readOnly)
      {
        this.menuItemDisconnectFromPersona.Enabled = false;
        this.menuItemLinkWithPersona.Enabled = false;
      }
      else if (nodeAt.ForeColor == SystemColors.GrayText)
      {
        this.menuItemCheckAll.Enabled = false;
        this.menuItemUncheckAll.Enabled = false;
        this.menuItemDisconnectFromPersona.Enabled = false;
        this.menuItemLinkWithPersona.Enabled = false;
      }
      else if (nodeAt.Tag != null)
      {
        switch ((AclFeature) nodeAt.Tag)
        {
          case AclFeature.eFolder_Other_ManageAccessToDocs:
          case AclFeature.eFolder_AttachDoc:
          case AclFeature.eFolder_DeleteDoc:
          case AclFeature.eFolder_RequestDoc:
          case AclFeature.eFolder_SendDoc:
          case AclFeature.eFolder_AssignAs:
          case AclFeature.LOConnectTab_DeleteAllTasks:
          case AclFeature.eVaultTab_eVaultPortal:
          case AclFeature.DeveloperConnectTab_SubscribetoWebhooks:
            this.menuItemDisconnectFromPersona.Enabled = true;
            this.menuItemLinkWithPersona.Enabled = true;
            break;
          default:
            this.menuItemDisconnectFromPersona.Enabled = false;
            this.menuItemLinkWithPersona.Enabled = false;
            break;
        }
      }
      else
      {
        this.menuItemDisconnectFromPersona.Enabled = true;
        this.menuItemLinkWithPersona.Enabled = true;
      }
      switch (nodeAt.Text)
      {
        case "Access to Trash Folder":
        case "Closing":
        case "File Access Management":
        case "My Profile":
        case "New Loans* (Contacts)":
        case "Originate Loan/Order Credit/Product and Pricing":
        case "Personal Loan Templates":
        case "Personal Resources":
        case "Reports":
        case "Trash Folder Tasks":
          this.menuItemDisconnectFromPersona.Enabled = false;
          this.menuItemLinkWithPersona.Enabled = false;
          break;
      }
      this.treeViewTabs.SelectedNode = nodeAt;
    }

    protected void ResetTree()
    {
      this.SetNodeStates();
      this.setDirtyFlag(false);
      this.securityHelper.ResetNodeUpdateStatus();
    }

    public virtual void MakeReadOnly(bool makeReadOnly) => this.MakeReadOnly(makeReadOnly, true);

    public virtual void MakeReadOnly(bool makeReadOnly, bool resetData)
    {
      this.readOnly = makeReadOnly;
      this.menuItemDisconnectFromPersona.Enabled = !makeReadOnly;
      this.menuItemLinkWithPersona.Enabled = !makeReadOnly;
      if (!(makeReadOnly & resetData))
        return;
      this.ResetTree();
    }

    private void menuItemCheckAll_Click(object sender, EventArgs e)
    {
      foreach (TreeNode node in this.treeViewTabs.Nodes)
        this.checkNodeAndItsChildren(node);
    }

    private void menuItemUncheckAll_Click(object sender, EventArgs e)
    {
      foreach (TreeNode node in this.treeViewTabs.Nodes)
        this.uncheckNodeAndItsChildren(node);
    }

    private void menuItemExpandAll_Click(object sender, EventArgs e)
    {
      this.treeViewTabs.ExpandAll();
    }

    private void menuItemCollapseAll_Click(object sender, EventArgs e)
    {
      this.treeViewTabs.CollapseAll();
    }

    private void treeViewTabs_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      TreeNode nodeAt = this.treeViewTabs.GetNodeAt(e.X, e.Y);
      if (this.MouseUpEvent == null || nodeAt == null)
        return;
      this.MouseUpEvent(nodeAt);
    }

    private void treeViewTabs_BeforeCheck(object sender, TreeViewCancelEventArgs e)
    {
      if (this.bInit)
        return;
      if (this.readOnly)
        e.Cancel = true;
      else if (e.Node.ForeColor == SystemColors.GrayText)
      {
        e.Cancel = true;
      }
      else
      {
        if (this.BeforeCheckedEvent == null || this.BeforeCheckedEvent(e.Node))
          return;
        e.Cancel = true;
      }
    }

    protected TreeNode findTreeNode(string treeNodePath)
    {
      foreach (TreeNode node in this.treeViewTabs.Nodes)
      {
        if (node.FullPath == treeNodePath)
          return node;
        TreeNode treeNode = this.findTreeNode(node, treeNodePath);
        if (treeNode != null)
          return treeNode;
      }
      return (TreeNode) null;
    }

    private TreeNode findTreeNode(TreeNode node, string treeNodePath)
    {
      foreach (TreeNode node1 in node.Nodes)
      {
        if (node1.FullPath == treeNodePath)
          return node1;
        TreeNode treeNode = this.findTreeNode(node1, treeNodePath);
        if (treeNode != null)
          return treeNode;
      }
      return (TreeNode) null;
    }

    protected void raiseDirtyFlagChangedEvent(object sender, EventArgs e)
    {
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged(sender, e);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.OrganizationDialog
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class OrganizationDialog : Form
  {
    private const string DUMMY_NODE = "<DUMMY_NODE>";
    private Sessions.Session session;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private TreeView tvwOrganization;
    private ImageList imgList;
    private CheckBox chkIncludeChildren;

    public OrgInfo SelectedOrganization
    {
      get => this.getSelectedOrganization();
      set => this.setSelectedOrganization(value);
    }

    public bool IncludeChildren
    {
      get => this.chkIncludeChildren.Checked;
      set => this.chkIncludeChildren.Checked = value;
    }

    public OrganizationDialog()
      : this(Session.DefaultInstance)
    {
    }

    public OrganizationDialog(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.initializeDialog();
    }

    private void initializeDialog()
    {
      Cursor.Current = Cursors.WaitCursor;
      this.tvwOrganization.BeginUpdate();
      IOrganizationManager organizationManager = this.session.OrganizationManager;
      List<int> intList = new List<int>();
      OrgInfo organization = organizationManager.GetOrganization(this.session.UserInfo.OrgId);
      intList.Add(organization.Oid);
      if (UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas))
      {
        while (organization.Parent != organization.Oid)
        {
          organization = organizationManager.GetOrganization(organization.Parent);
          intList.Add(organization.Oid);
        }
      }
      TreeNode treeNode = new TreeNode(organization.OrgName, 0, 1);
      treeNode.ToolTipText = organization.Description;
      treeNode.Tag = (object) organization;
      this.tvwOrganization.Nodes.Add(treeNode);
      for (int index = intList.Count - 2; index >= 0; --index)
      {
        this.addBranch(treeNode);
        foreach (TreeNode node in treeNode.Nodes)
        {
          if (((OrgInfo) node.Tag).Oid == intList[index])
          {
            treeNode = node;
            break;
          }
        }
      }
      if (1 == intList.Count && organization.Children.Length != 0)
        treeNode.Nodes.Add(new TreeNode("<DUMMY_NODE>", 0, 1));
      this.tvwOrganization.SelectedNode = treeNode;
      this.tvwOrganization.EndUpdate();
      Cursor.Current = Cursors.Default;
    }

    private void addBranch(TreeNode tvwNode)
    {
      if (1 < tvwNode.Nodes.Count || 1 == tvwNode.Nodes.Count && "<DUMMY_NODE>" != tvwNode.Nodes[0].Text)
        return;
      tvwNode.Nodes.Clear();
      IOrganizationManager organizationManager = this.session.OrganizationManager;
      OrgInfo tag = (OrgInfo) tvwNode.Tag;
      if (tag.Children.Length == 0)
        return;
      foreach (OrgInfo organization in organizationManager.GetOrganizations(tag.Children))
      {
        TreeNode node = new TreeNode(organization.OrgName, 0, 1);
        node.ToolTipText = organization.Description;
        node.Tag = (object) organization;
        tvwNode.Nodes.Add(node);
        if (organization.Children.Length != 0)
          node.Nodes.Add(new TreeNode("<DUMMY_NODE>", 0, 1));
      }
    }

    private OrgInfo getSelectedOrganization()
    {
      OrgInfo selectedOrganization = (OrgInfo) null;
      if (this.tvwOrganization.SelectedNode != null && this.tvwOrganization.SelectedNode.Tag != null)
        selectedOrganization = (OrgInfo) this.tvwOrganization.SelectedNode.Tag;
      return selectedOrganization;
    }

    private void setSelectedOrganization(OrgInfo selectedOrganization)
    {
      if (this.tvwOrganization.Nodes == null || this.tvwOrganization.Nodes.Count == 0)
        return;
      TreeNode treeNode = this.findTreeNode(selectedOrganization.Oid, this.tvwOrganization.Nodes) ?? this.tvwOrganization.Nodes[0];
      this.tvwOrganization.SelectedNode = treeNode;
      treeNode.EnsureVisible();
    }

    private TreeNode findTreeNode(int orgId, TreeNodeCollection tvwChildNodes)
    {
      foreach (TreeNode tvwChildNode in tvwChildNodes)
      {
        if (tvwChildNode.Tag != null)
        {
          if (orgId == ((OrgInfo) tvwChildNode.Tag).Oid)
            return tvwChildNode;
          this.addBranch(tvwChildNode);
          TreeNode treeNode = this.findTreeNode(orgId, tvwChildNode.Nodes);
          if (treeNode != null)
            return treeNode;
        }
      }
      return (TreeNode) null;
    }

    private void tvwOrganization_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.btnOK.Enabled = this.tvwOrganization.SelectedNode != null;
    }

    private void tvwOrganization_MouseUp(object sender, MouseEventArgs e)
    {
      this.btnOK.Enabled = this.tvwOrganization.SelectedNode != null;
    }

    private void tvwOrganization_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
      this.tvwOrganization.BeginUpdate();
      this.addBranch(e.Node);
      this.tvwOrganization.EndUpdate();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = this.tvwOrganization.SelectedNode != null ? DialogResult.OK : DialogResult.Cancel;
      this.Close();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OrganizationDialog));
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.tvwOrganization = new TreeView();
      this.imgList = new ImageList(this.components);
      this.chkIncludeChildren = new CheckBox();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(124, 418);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(205, 418);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.tvwOrganization.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tvwOrganization.HideSelection = false;
      this.tvwOrganization.ImageIndex = 0;
      this.tvwOrganization.ImageList = this.imgList;
      this.tvwOrganization.Location = new Point(13, 13);
      this.tvwOrganization.Name = "tvwOrganization";
      this.tvwOrganization.SelectedImageIndex = 1;
      this.tvwOrganization.ShowNodeToolTips = true;
      this.tvwOrganization.Size = new Size(267, 376);
      this.tvwOrganization.TabIndex = 2;
      this.tvwOrganization.BeforeExpand += new TreeViewCancelEventHandler(this.tvwOrganization_BeforeExpand);
      this.tvwOrganization.MouseUp += new MouseEventHandler(this.tvwOrganization_MouseUp);
      this.tvwOrganization.AfterSelect += new TreeViewEventHandler(this.tvwOrganization_AfterSelect);
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "folder.bmp");
      this.imgList.Images.SetKeyName(1, "folder-open.bmp");
      this.chkIncludeChildren.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkIncludeChildren.AutoSize = true;
      this.chkIncludeChildren.Location = new Point(13, 395);
      this.chkIncludeChildren.Name = "chkIncludeChildren";
      this.chkIncludeChildren.Size = new Size(212, 17);
      this.chkIncludeChildren.TabIndex = 3;
      this.chkIncludeChildren.Text = "Include children of this level and below.";
      this.chkIncludeChildren.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(292, 453);
      this.Controls.Add((Control) this.chkIncludeChildren);
      this.Controls.Add((Control) this.tvwOrganization);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (OrganizationDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select an Organization";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

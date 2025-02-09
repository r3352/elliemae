// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.TPOOrgDialog
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class TPOOrgDialog : Form
  {
    private List<ExternalOriginatorManagementData> externalOrgsList;
    private bool isAdmin;
    private TreeNode tvwNode = new TreeNode("Third Party Originators", 0, 1);
    private const string tpoCompanyType = "Tpo";
    private Sessions.Session session;
    private IContainer components;
    private Button button1;
    private Button button2;
    private ExternalHierarchyTree tvwOrganization;
    private ImageList imgList;

    public TPOOrgDialog(
      List<ExternalOriginatorManagementData> externalOrgsList,
      Sessions.Session session)
    {
      this.session = session;
      this.externalOrgsList = externalOrgsList;
      this.tvwOrganization = new ExternalHierarchyTree(this.session);
      this.InitializeComponent();
      this.initializeDialog();
    }

    public TPOOrgDialog(
      List<ExternalOriginatorManagementData> externalOrgsList)
      : this(externalOrgsList, Session.DefaultInstance)
    {
    }

    public ExternalOriginatorManagementData SelectedTPOOrg
    {
      get => this.getSelectedTPOOrg();
      set => this.setSelectedTPOOrg(value);
    }

    public bool IncludeChildren
    {
      get => true;
      set
      {
      }
    }

    private void initializeDialog()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      if (this.session.UserInfo.IsAdministrator() || !aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_ContactSalesRep) && aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_OrganizationSettings))
        this.isAdmin = true;
      List<HierarchySummary>[] allHierarchy = this.session.ConfigurationManager.GetAllHierarchy();
      List<HierarchySummary> list = allHierarchy == null || allHierarchy.Length != 3 ? (List<HierarchySummary>) null : allHierarchy[1];
      this.tvwOrganization.ExternalCompanyList = allHierarchy;
      Cursor.Current = Cursors.WaitCursor;
      this.tvwOrganization.BeginUpdate();
      this.tvwNode.ToolTipText = "Third Party Originators";
      ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
      originatorManagementData.ExternalID = "-1";
      List<string> stringList = new List<string>();
      foreach (ExternalOriginatorManagementData externalOrgs in this.externalOrgsList)
        stringList.Add(externalOrgs.ExternalID);
      originatorManagementData.RootOrgBySalesRep = stringList;
      originatorManagementData.OrganizationName = "Third Party Originators";
      this.tvwNode.Tag = (object) originatorManagementData;
      this.tvwOrganization.Nodes.Add(this.tvwNode);
      if (list != null)
        this.buildHierarchyTree(list);
      this.tvwOrganization.SelectedNode = this.tvwNode;
      this.tvwOrganization.EndUpdate();
      this.tvwOrganization.Sort();
      Cursor.Current = Cursors.Default;
    }

    private void buildHierarchyTree(List<HierarchySummary> list)
    {
      if (list.Count == 0)
        return;
      this.buildHierarchyTree(this.tvwNode, list.Where<HierarchySummary>((Func<HierarchySummary, bool>) (x => x.Depth == 1)).ToList<HierarchySummary>());
    }

    private void buildHierarchyTree(TreeNode parentNode, List<HierarchySummary> list)
    {
      if (list.Count == 0)
        return;
      TreeNode newTreeNode = (TreeNode) null;
      list.ForEach((Action<HierarchySummary>) (company =>
      {
        if (this.externalOrgsList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (e1 => e1.ExternalID.Equals(company.ExternalID))) == null && company.Parent == 0 && !this.isAdmin)
          return;
        newTreeNode = new TreeNode(company.OrganizationName, 0, 1);
        newTreeNode.Tag = (object) new ExtNodeTag(company.oid, company.OrganizationName, "Tpo");
        parentNode.Nodes.Add(newTreeNode);
        if (this.tvwOrganization.BrokerOrganizationList.FirstOrDefault<HierarchySummary>((Func<HierarchySummary, bool>) (x => x.Parent == company.oid)) == null)
          return;
        newTreeNode.Nodes.Add("<DUMMY NODE>");
      }));
    }

    private ExternalOriginatorManagementData getSelectedTPOOrg()
    {
      ExternalOriginatorManagementData selectedTpoOrg = (ExternalOriginatorManagementData) null;
      if (this.tvwOrganization.SelectedNode != null && this.tvwOrganization.SelectedNode.Tag != null)
        selectedTpoOrg = this.session.ConfigurationManager.GetExternalOrganization(false, ((ExtNodeTag) this.tvwOrganization.SelectedNode.Tag).Oid);
      return selectedTpoOrg;
    }

    private void setSelectedTPOOrg(ExternalOriginatorManagementData selectedTPOOrg)
    {
      if (this.tvwOrganization.Nodes == null || this.tvwOrganization.Nodes.Count == 0)
        return;
      this.locateUserInOrganization(selectedTPOOrg.oid);
    }

    private void locateUserInOrganization(int orgId)
    {
      List<int> organizationParents = this.session.ConfigurationManager.GetExternalOrganizationParents(orgId);
      this.selectNodeForOrg(this.tvwNode, orgId, organizationParents);
    }

    private void selectNodeForOrg(TreeNode root, int oid, List<int> parents)
    {
      if (root == null)
        return;
      for (int index = 0; index < root.Nodes.Count && (ExtNodeTag) root.Nodes[index].Tag != null; ++index)
      {
        if (((ExtNodeTag) root.Nodes[index].Tag).Oid == oid)
        {
          this.tvwOrganization.SelectedNode = root.Nodes[index];
          root.Nodes[index].EnsureVisible();
          break;
        }
        if (parents.Contains(((ExtNodeTag) root.Nodes[index].Tag).Oid))
        {
          root.Expand();
          this.tvwOrganization.CallBeforeExpand(new TreeViewCancelEventArgs(root.Nodes[index], false, TreeViewAction.Expand));
        }
        this.selectNodeForOrg(root.Nodes[index], oid, parents);
      }
    }

    private void tvwOrganization_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.button1.Enabled = this.tvwOrganization.SelectedNode != null;
    }

    private void tvwOrganization_MouseUp(object sender, MouseEventArgs e)
    {
      this.button1.Enabled = this.tvwOrganization.SelectedNode != null;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.DialogResult = this.tvwOrganization.SelectedNode != null ? DialogResult.OK : DialogResult.Cancel;
      this.Close();
    }

    private void button2_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TPOOrgDialog));
      this.button1 = new Button();
      this.button2 = new Button();
      this.imgList = new ImageList(this.components);
      this.SuspendLayout();
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.Location = new Point(92, 331);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.Location = new Point(184, 331);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.tvwOrganization.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tvwOrganization.HideSelection = false;
      this.tvwOrganization.ImageIndex = 0;
      this.tvwOrganization.ImageList = this.imgList;
      this.tvwOrganization.Location = new Point(12, 12);
      this.tvwOrganization.Name = "tvwOrganization";
      this.tvwOrganization.SelectedImageIndex = 1;
      this.tvwOrganization.Size = new Size(247, 290);
      this.tvwOrganization.TabIndex = 4;
      this.tvwOrganization.Sorted = true;
      this.tvwOrganization.AfterSelect += new TreeViewEventHandler(this.tvwOrganization_AfterSelect);
      this.tvwOrganization.MouseUp += new MouseEventHandler(this.tvwOrganization_MouseUp);
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "folder.bmp");
      this.imgList.Images.SetKeyName(1, "folder-open.bmp");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(271, 370);
      this.Controls.Add((Control) this.tvwOrganization);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Name = nameof (TPOOrgDialog);
      this.Text = "Select an Organization";
      this.ResumeLayout(false);
    }
  }
}

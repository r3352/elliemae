// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.HierarchyTree
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class HierarchyTree : TreeView
  {
    public const int IDXFolderAndBelowSelected = 0;
    public const int IDXFolderSelected = 1;
    public const int IDXUserSelected = 2;
    public const int IDXFolderIncluded = 3;
    public const int IDXUserIncluded = 4;
    public const int IDXFolderNotInGroup = 5;
    public const int IDXUserNotInGroup = 6;
    public const string DUMMY_NODE = "<DUMMY NODE>";
    private Sessions.Session session;

    public Sessions.Session CurrentSession => this.session;

    public int ParentOrgId { get; set; }

    public HierarchyTree()
      : this(Session.DefaultInstance)
    {
    }

    public HierarchyTree(Sessions.Session session) => this.session = session;

    public void SetSession(Sessions.Session session) => this.session = session;

    protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
    {
      base.OnBeforeExpand(e);
      this.BeginUpdate();
      this.addBranch(e.Node);
      this.EndUpdate();
    }

    public void RootNodes(int orgId)
    {
      IOrganizationManager organizationManager = this.session.OrganizationManager;
      this.Nodes.Clear();
      this.BeginUpdate();
      OrgInfo organization = organizationManager.GetOrganization(orgId);
      int oid = organization.Oid;
      ArrayList arrayList = new ArrayList();
      arrayList.Add((object) oid);
      if (this.session.UserInfo.IsTopLevelAdministrator())
      {
        while (organization.Parent != organization.Oid)
        {
          organization = organizationManager.GetOrganization(organization.Parent);
          arrayList.Add((object) organization.Oid);
        }
      }
      else
      {
        while (this.ParentOrgId != organization.Oid)
        {
          organization = organizationManager.GetOrganization(organization.Parent);
          arrayList.Add((object) organization.Oid);
        }
      }
      TreeNode treeNode = new TreeNode(organization.OrgName, 0, 1);
      treeNode.Tag = (object) new OrgNodeTag(organization.Oid, organization.Description);
      this.Nodes.Add(treeNode);
      this.EndUpdate();
      for (int index = arrayList.Count - 2; index >= 0; --index)
      {
        this.addBranch(treeNode);
        foreach (TreeNode node in treeNode.Nodes)
        {
          if (((OrgNodeTag) node.Tag).Oid == (int) arrayList[index])
          {
            treeNode = node;
            break;
          }
        }
      }
      if (arrayList.Count == 1 && organization.Children.Length != 0)
        treeNode.Nodes.Add(new TreeNode("<DUMMY NODE>", 0, 1));
      this.SelectedNode = treeNode;
    }

    private void addBranch(TreeNode tn)
    {
      if (tn.Nodes.Count > 1 || tn.Nodes.Count == 1 && tn.Nodes[0].Text != "<DUMMY NODE>")
        return;
      tn.Nodes.Clear();
      IOrganizationManager organizationManager = this.session.OrganizationManager;
      OrgNodeTag tag = (OrgNodeTag) tn.Tag;
      OrgInfo organization1 = organizationManager.GetOrganization(tag.Oid);
      if (organization1.Children.Length == 0)
        return;
      foreach (OrgInfo organization2 in organizationManager.GetOrganizations(organization1.Children))
      {
        string orgName = organization2.OrgName;
        string description = organization2.Description;
        int oid = organization2.Oid;
        TreeNode node = new TreeNode(orgName, 0, 1);
        node.Tag = (object) new OrgNodeTag(oid, description);
        tn.Nodes.Add(node);
        if (organization2.Children.Length != 0)
          node.Nodes.Add(new TreeNode("<DUMMY NODE>", 0, 1));
      }
    }
  }
}

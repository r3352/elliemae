// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TreeViewer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TreeViewer : TreeView
  {
    public object resourceToSelect;
    private IResourceItemProvider resourceCntProvider;
    private OrgInfo[] allOrgList;
    private UserInfo[] allUserList;
    private Sessions.Session session;

    public TreeViewer(Sessions.Session session)
    {
      this.session = session;
      try
      {
        if (this.session.ConfigurationManager.CheckIfAnyTPOSiteExists())
        {
          this.allOrgList = this.session.OrganizationManager.GetAllIntAndExtOrganizations();
          this.allUserList = this.session.OrganizationManager.GetAllIntAndExtUsers();
        }
        else
        {
          this.allOrgList = this.session.OrganizationManager.GetAllOrganizations();
          this.allUserList = this.session.OrganizationManager.GetAllUsers();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
      }
    }

    public void ShowTree(object resourceSet)
    {
      TreeNode node = (TreeNode) null;
      if (resourceSet is FileSystemResourceSet)
      {
        FileSystemResourceSet systemResourceSet = (FileSystemResourceSet) resourceSet;
        this.resourceCntProvider = (IResourceItemProvider) new FileSystemItemProvider(this.session, (FileSystemResourceSet) resourceSet, (object) null);
        ArrayList arrayList = new ArrayList((ICollection) systemResourceSet.Folders);
        FileSystemEntry fileSystemEntry = (FileSystemEntry) null;
        switch (systemResourceSet.FileType)
        {
          case AclFileType.LoanProgram:
            node = new TreeNode("Public Loan Programs");
            fileSystemEntry = new FileSystemEntry("\\", "Public Loan Programs", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.ClosingCost:
            node = new TreeNode("Public Closing Cost Templates");
            fileSystemEntry = new FileSystemEntry("\\", "Public Closing Cost Templates", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.MiscData:
            node = new TreeNode("Public Data Templates");
            fileSystemEntry = new FileSystemEntry("\\", "Public Data Templates", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.FormList:
            node = new TreeNode("Public Form Lists");
            fileSystemEntry = new FileSystemEntry("\\", "Public Form Lists", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.DocumentSet:
            node = new TreeNode("Public Document Sets");
            fileSystemEntry = new FileSystemEntry("\\", "Public Document Sets", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.LoanTemplate:
            node = new TreeNode("Public Loan Templates");
            fileSystemEntry = new FileSystemEntry("\\", "Public Loan Templates", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.CustomPrintForms:
            node = new TreeNode("Public Custom Forms");
            fileSystemEntry = new FileSystemEntry("\\", "Public Custom Forms", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.PrintGroups:
            node = new TreeNode("Public Forms Groups");
            fileSystemEntry = new FileSystemEntry("\\", "Public Forms Groups", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.Reports:
            node = new TreeNode("Public Reports");
            fileSystemEntry = new FileSystemEntry("\\", "Public Reports", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.BorrowerCustomLetters:
            node = new TreeNode("Public Custom Letters");
            fileSystemEntry = new FileSystemEntry("\\", "Public Custom Letters", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.BizCustomLetters:
            node = new TreeNode("Public Custom Letters");
            fileSystemEntry = new FileSystemEntry("\\", "Public Custom Letters", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.CampaignTemplate:
            node = new TreeNode("Public Campaign Templates");
            fileSystemEntry = new FileSystemEntry("\\", "Public Campaign Templates", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.DashboardTemplate:
            node = new TreeNode("Public Dashboard Templates");
            fileSystemEntry = new FileSystemEntry("\\", "Public Dashboard Templates", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.DashboardViewTemplate:
            node = new TreeNode("Public DashboardView Templates");
            fileSystemEntry = new FileSystemEntry("\\", "Public DashboardView Templates", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.TaskSet:
            node = new TreeNode("Public Task Sets");
            fileSystemEntry = new FileSystemEntry("\\", "Public Task Sets", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.SettlementServiceProviders:
            node = new TreeNode("Public Settlement Service Providers");
            fileSystemEntry = new FileSystemEntry("\\", "Public Settlement Service Providers", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
          case AclFileType.AffiliatedBusinessArrangements:
            node = new TreeNode("Public Affiliates");
            fileSystemEntry = new FileSystemEntry("\\", "Public Affiliates", FileSystemEntry.Types.Folder, (string) null);
            node.Tag = (object) fileSystemEntry;
            if (arrayList.Contains((object) fileSystemEntry))
            {
              node.ImageIndex = 0;
              node.SelectedImageIndex = 0;
              break;
            }
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
            break;
        }
        node.Tag = (object) fileSystemEntry;
        if (arrayList.Contains((object) fileSystemEntry))
        {
          node.ImageIndex = 0;
          node.SelectedImageIndex = 0;
        }
        else
        {
          node.ImageIndex = 5;
          node.SelectedImageIndex = 5;
        }
      }
      else if ((object) (resourceSet as AclGroup) != null)
      {
        this.resourceCntProvider = (IResourceItemProvider) new AclGroupItemProvider(this.session, (AclGroup) resourceSet, this.resourceToSelect, this.allOrgList, this.allUserList);
      }
      else
      {
        switch (resourceSet)
        {
          case AclGroupLoanMembers _:
            this.resourceCntProvider = (IResourceItemProvider) new UserGroupItemProvider(this.session, (AclGroupLoanMembers) resourceSet, this.resourceToSelect, this.allOrgList, this.allUserList);
            break;
          case AclGroupRoleMembers _:
            AclGroupRoleMembers members = (AclGroupRoleMembers) resourceSet;
            this.allUserList = this.session.OrganizationManager.GetUsersWithRole(members.RoleID);
            this.resourceCntProvider = (IResourceItemProvider) new UserGroupItemProvider(members, this.resourceToSelect, this.allOrgList, this.allUserList, this.session);
            break;
          case AclGroupContactMembers _:
            this.resourceCntProvider = (IResourceItemProvider) new UserGroupItemProvider(this.session, (AclGroupContactMembers) resourceSet, this.resourceToSelect, this.allOrgList, this.allUserList);
            break;
        }
      }
      this.resourceCntProvider.SetHeadNode(node);
      TreeNode[] initialNodes = this.resourceCntProvider.getInitialNodes();
      this.BeginUpdate();
      if (initialNodes.Length != 0)
      {
        this.Nodes.AddRange(initialNodes);
        TreeNode nodeToSelect = this.resourceCntProvider.getNodeToSelect();
        if (nodeToSelect != null)
          this.SelectedNode = nodeToSelect;
        else
          this.SelectedNode = this.Nodes[0];
      }
      this.ExpandTree(this.Nodes[0]);
      if (this.Nodes.Count > 1)
        this.ExpandTree(this.Nodes[1]);
      this.resourceToSelect = (object) null;
      this.EndUpdate();
    }

    public void ExpandTree(TreeNode node)
    {
      this.OnBeforeExpand(new TreeViewCancelEventArgs(node, false, TreeViewAction.Expand));
      foreach (TreeNode node1 in node.Nodes)
        this.ExpandTree(node1);
    }

    protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
    {
      base.OnBeforeExpand(e);
      this.BeginUpdate();
      this.resourceCntProvider.onNodeExpand(e.Node);
      this.EndUpdate();
    }
  }
}

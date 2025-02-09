// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FileSystemItemProvider
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FileSystemItemProvider : IResourceItemProvider
  {
    private Sessions.Session session;
    private FolderItemProvider nodeCntProvider;
    private FileItemProvider leafCntProvider;
    private FileSystemResourceSet resourceSet;
    private FileSystemEntry[] allSystemEntry;
    private TreeNode headNode;

    public FileSystemItemProvider(
      Sessions.Session session,
      FileSystemResourceSet resourceSet,
      object resourceToSelect)
    {
      this.session = session;
      this.resourceSet = resourceSet;
      this.initialAllSystemEntry();
      this.nodeCntProvider = new FolderItemProvider(new ArrayList((ICollection) resourceSet.Folders), (object) null, this.allSystemEntry);
      this.leafCntProvider = new FileItemProvider(new ArrayList((ICollection) resourceSet.Files), (object) null);
    }

    private void initialAllSystemEntry()
    {
      int num = -1;
      TemplateSettingsType type1 = TemplateSettingsType.LoanProgram;
      CustomLetterType type2 = CustomLetterType.Generic;
      switch (this.resourceSet.FileType)
      {
        case AclFileType.LoanProgram:
          type1 = TemplateSettingsType.LoanProgram;
          num = 0;
          break;
        case AclFileType.ClosingCost:
          type1 = TemplateSettingsType.ClosingCost;
          num = 0;
          break;
        case AclFileType.MiscData:
          type1 = TemplateSettingsType.MiscData;
          num = 0;
          break;
        case AclFileType.FormList:
          type1 = TemplateSettingsType.FormList;
          num = 0;
          break;
        case AclFileType.DocumentSet:
          type1 = TemplateSettingsType.DocumentSet;
          num = 0;
          break;
        case AclFileType.LoanTemplate:
          type1 = TemplateSettingsType.LoanTemplate;
          num = 0;
          break;
        case AclFileType.CustomPrintForms:
          type2 = CustomLetterType.Generic;
          num = 1;
          break;
        case AclFileType.PrintGroups:
          this.allSystemEntry = this.session.ConfigurationManager.GetAllPublicFormGroupDirEntries(false);
          break;
        case AclFileType.Reports:
          this.allSystemEntry = this.session.ReportManager.GetAllPublicReportDirEntries();
          break;
        case AclFileType.BorrowerCustomLetters:
          type2 = CustomLetterType.Borrower;
          num = 1;
          break;
        case AclFileType.BizCustomLetters:
          type2 = CustomLetterType.BizPartner;
          num = 1;
          break;
        case AclFileType.CampaignTemplate:
          type1 = TemplateSettingsType.Campaign;
          num = 0;
          break;
        case AclFileType.DashboardTemplate:
          type1 = TemplateSettingsType.DashboardTemplate;
          num = 0;
          break;
        case AclFileType.DashboardViewTemplate:
          type1 = TemplateSettingsType.DashboardViewTemplate;
          num = 0;
          break;
        case AclFileType.TaskSet:
          type1 = TemplateSettingsType.TaskSet;
          num = 0;
          break;
        case AclFileType.SettlementServiceProviders:
          type1 = TemplateSettingsType.SettlementServiceProviders;
          num = 0;
          break;
        case AclFileType.AffiliatedBusinessArrangements:
          type1 = TemplateSettingsType.AffiliatedBusinessArrangements;
          num = 0;
          break;
      }
      if (num == 0)
      {
        this.allSystemEntry = this.session.ConfigurationManager.GetAllPublicTemplateSettingsSystemEntries(type1, false);
      }
      else
      {
        if (num != 1)
          return;
        this.allSystemEntry = this.session.ConfigurationManager.GetAllPublicCustomLetterFileEntries(type2);
      }
    }

    private FileSystemEntry[] getChildren(FileSystemEntry parentEntry)
    {
      List<FileSystemEntry> fileSystemEntryList = new List<FileSystemEntry>();
      if (this.allSystemEntry != null && this.allSystemEntry.Length != 0)
      {
        foreach (FileSystemEntry fileSystemEntry in this.allSystemEntry)
        {
          if (fileSystemEntry.ParentFolder.Path == parentEntry.Path)
            fileSystemEntryList.Add(fileSystemEntry);
        }
      }
      return fileSystemEntryList.ToArray();
    }

    public TreeNode[] getInitialNodes()
    {
      FileSystemEntry[] children = this.getChildren(FileSystemEntry.PublicRoot);
      ArrayList arrayList = new ArrayList();
      if (this.headNode != null)
        arrayList.Add((object) this.headNode);
      for (int index = 0; index < children.Length; ++index)
      {
        TreeNode treeNode = children[index].Type != FileSystemEntry.Types.File ? this.nodeCntProvider.addNodeToParent(children[index], this.headNode) : this.leafCntProvider.addNodeToParent(children[index], this.headNode);
        if (treeNode != null && this.headNode == null)
          arrayList.Add((object) treeNode);
      }
      return (TreeNode[]) arrayList.ToArray(typeof (TreeNode));
    }

    public void onNodeExpand(TreeNode tn)
    {
      if (!this.isNodeExpandable(tn) || this.nodeIsExpanded(tn))
        return;
      tn.Nodes.Clear();
      FileSystemEntry[] children = this.getChildren((FileSystemEntry) tn.Tag);
      if (children == null)
        return;
      for (int index = 0; index < children.Length; ++index)
      {
        FileSystemEntry entry = children[index];
        if (entry.Type == FileSystemEntry.Types.Folder)
          this.nodeCntProvider.addNodeToParent(entry, tn);
        else
          this.leafCntProvider.addNodeToParent(entry, tn);
      }
    }

    public void SetHeadNode(TreeNode node) => this.headNode = node;

    private bool isNodeExpandable(TreeNode node)
    {
      return ((FileSystemEntry) node.Tag).Type == FileSystemEntry.Types.Folder;
    }

    private bool nodeIsExpanded(TreeNode node)
    {
      bool flag = false;
      if (node.Nodes.Count > 1)
        flag = true;
      if (node.Nodes.Count == 1 && node.Nodes[0].Text != "<DUMMY NODE>")
        flag = true;
      FileSystemEntry tag = (FileSystemEntry) node.Tag;
      if (tag.IsPublic && tag.ParentFolder.Path == "\\")
      {
        switch (tag.Name)
        {
          case "Public Affiliates":
          case "Public Campaign Templates":
          case "Public Closing Cost Templates":
          case "Public Custom Forms":
          case "Public Custom Letters":
          case "Public Dashboard Templates":
          case "Public Data Templates":
          case "Public Document Sets":
          case "Public Form Lists":
          case "Public Forms Groups":
          case "Public Loan Programs":
          case "Public Loan Templates":
          case "Public Reports":
          case "Public Settlement Service Providers":
            flag = true;
            break;
        }
      }
      return flag;
    }

    public TreeNode getNodeToSelect() => (TreeNode) null;
  }
}

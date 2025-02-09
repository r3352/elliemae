// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FolderItemProvider
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FolderItemProvider
  {
    private ArrayList _CheckedIds;
    private FileSystemEntry[] allSystemEntry;

    public TreeNode NodeToSelect => (TreeNode) null;

    public FolderItemProvider(
      ArrayList checkedEntryIds,
      object entryToSelect,
      FileSystemEntry[] allSystemEntry)
    {
      this._CheckedIds = checkedEntryIds;
      this.allSystemEntry = allSystemEntry;
    }

    public TreeNode addNodeToParent(FileSystemEntry entry, TreeNode parentNode)
    {
      TreeNode node = new TreeNode(entry.Name);
      node.Tag = (object) entry;
      parentNode?.Nodes.Add(node);
      this.setNodeImageIndex(node);
      this.addDummyNodeIfNecessary(node);
      return node;
    }

    public void setNodeImageIndex(TreeNode node)
    {
      if (this._CheckedIds.Contains((object) (FileSystemEntry) node.Tag))
        this.setNodeImageIndex(node, 0);
      else if (this.existsInclusiveParent(node))
        this.setNodeImageIndex(node, 3);
      else
        this.setNodeImageIndex(node, 5);
    }

    private void setNodeImageIndex(TreeNode node, int index)
    {
      node.ImageIndex = index;
      node.SelectedImageIndex = index;
    }

    private bool existsInclusiveParent(TreeNode node)
    {
      TreeNode parent = node.Parent;
      if (parent == null)
        return false;
      if (parent.ImageIndex == 0 || parent.ImageIndex == 3)
        return true;
      if (parent.ImageIndex == 5)
        return false;
      return parent.ImageIndex == 1 && (object) (node.Tag as UserInfo) != null || this.existsInclusiveParent(parent);
    }

    private void addDummyNodeIfNecessary(TreeNode node)
    {
      if ((this.allSystemEntry == null ? new LoanProgramIFSExplorer(TemplateSettingsType.LoanProgram, true, Session.DefaultInstance).GetFileSystemEntries(FileSystemEntry.PublicRoot) : this.allSystemEntry).Length == 0)
        return;
      node.Nodes.Clear();
      node.Nodes.Add(new TreeNode("<DUMMY NODE>", 0, 1));
    }
  }
}

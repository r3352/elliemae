// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MilestoneWorkflowTaskTree
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TreeViewSearchControl;
using TreeViewSearchProvider;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MilestoneWorkflowTaskTree : UserControl
  {
    private TaskGroupTemplate[] taskGroupTemplates;
    private TaskTemplate[] taskTemplates;
    private TreeView treeDummyGroupsTasks = new TreeView();
    private TreeView treeDummyStandaloneTasks = new TreeView();
    private TreeViewSearcher tvs;
    private IContainer components;
    private PanelEx pnlExTreeView;
    private TreeView treeWorkflowTasks;
    private Label lblMessage;
    private PanelEx pnlExTreeViewSearch;

    public TaskTemplate[] SelectedTasks => this.getSelectedTaskTemplates();

    public MilestoneWorkflowTaskTree(
      TaskGroupTemplate[] taskGroupTemplates,
      TaskTemplate[] taskTemplates)
    {
      this.InitializeComponent();
      this.taskGroupTemplates = taskGroupTemplates;
      this.taskTemplates = taskTemplates;
      this.initForm();
    }

    private void initForm()
    {
      if (this.updateUIForNoRecords(1))
        return;
      this.populateTaskGroupTemplates();
      this.populateTaskTemplates();
      if (this.updateUIForNoRecords(2))
        return;
      this.sortTree();
    }

    private void initTreeViewSearcher()
    {
      if (!this.pnlExTreeViewSearch.Visible)
        return;
      UIController everything = UIController.Everything;
      everything.ClearButtonImage = false;
      everything.SettingsButton = false;
      everything.MessageMedium = MessageMedium.Label;
      everything.AssignFunctionKey(this.ParentForm);
      everything.RealTime = true;
      this.tvs = new TreeViewSearcher(everything);
      this.tvs.AddTrees(new List<TreeView>()
      {
        this.treeWorkflowTasks
      });
      this.tvs.SearchProvider.SearchParams.Occurances = SearchParameters.Occurance.All;
      this.tvs.SearchProvider.NodeFormat.Highlighting = NodeFormatSettings.Highlight.NextOne;
      this.pnlExTreeViewSearch.Controls.Add((Control) this.tvs);
    }

    private bool updateUIForNoRecords(int switch_on)
    {
      bool flag = false;
      switch (switch_on)
      {
        case 1:
          if (this.taskTemplates.Length == 0)
          {
            flag = true;
            break;
          }
          break;
        case 2:
          if (this.treeDummyGroupsTasks.Nodes.Count == 0 && this.treeDummyStandaloneTasks.Nodes.Count == 0)
          {
            flag = true;
            break;
          }
          break;
      }
      if (flag)
      {
        this.lblMessage.Text = "There are no Workflow Tasks defined";
        this.treeWorkflowTasks.Visible = false;
        this.pnlExTreeViewSearch.Visible = false;
      }
      return flag;
    }

    private void sortTree()
    {
      this.treeDummyGroupsTasks.Sort();
      this.treeDummyStandaloneTasks.Sort();
      foreach (TreeNode node in this.treeDummyGroupsTasks.Nodes)
        this.treeWorkflowTasks.Nodes.Add((TreeNode) node.Clone());
      foreach (TreeNode node in this.treeDummyStandaloneTasks.Nodes)
        this.treeWorkflowTasks.Nodes.Add((TreeNode) node.Clone());
      this.treeWorkflowTasks.ExpandAll();
    }

    private bool populateTaskGroupTemplates(string parentId = null, TreeNode parentNode = null)
    {
      bool flag1 = false;
      foreach (TaskGroupTemplate taskGroupTemplate in ((IEnumerable<TaskGroupTemplate>) this.taskGroupTemplates).Where<TaskGroupTemplate>((Func<TaskGroupTemplate, bool>) (parent => parent.ParentTaskGroupTemplateId == parentId)))
      {
        TreeNode treeNode = new TreeNode(taskGroupTemplate.Name)
        {
          Tag = (object) taskGroupTemplate
        };
        bool flag2 = this.populateTaskTemplates(taskGroupTemplate.ID, treeNode);
        flag1 = this.populateTaskGroupTemplates(taskGroupTemplate.ID, treeNode);
        if (flag2 || flag1)
        {
          if (parentNode == null)
            this.treeDummyGroupsTasks.Nodes.Add(treeNode);
          else
            parentNode.Nodes.Add(treeNode);
          flag1 = true;
        }
      }
      return flag1;
    }

    private bool populateTaskTemplates(string parentGroupId = null, TreeNode parentNode = null)
    {
      bool flag = false;
      foreach (TaskTemplate taskTemplate in ((IEnumerable<TaskTemplate>) this.taskTemplates).Where<TaskTemplate>((Func<TaskTemplate, bool>) (child => child.TaskGroupTemplateId == parentGroupId)))
      {
        TreeNode node = new TreeNode(taskTemplate.Name);
        node.Tag = (object) taskTemplate;
        if (parentNode == null)
          this.treeDummyStandaloneTasks.Nodes.Add(node);
        else
          parentNode.Nodes.Add(node);
        flag = true;
      }
      return flag;
    }

    private void MilestoneWorkflowTaskTree_Load(object sender, EventArgs e)
    {
      this.initTreeViewSearcher();
    }

    private void treeWorkflowTasks_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (e.Action == TreeViewAction.Unknown)
        return;
      this.updateChildNodesCheckedUncheckedStatus(e.Node);
      if (e.Node.Checked)
        this.updateParentNodesCheckedStatus(e.Node);
      else
        this.updateParentNodesUncheckedStatus(e.Node);
    }

    private void updateParentNodesUncheckedStatus(TreeNode node)
    {
      if (node.Parent == null)
        return;
      bool flag = true;
      foreach (TreeNode node1 in node.Parent.Nodes)
      {
        if (node != node1 && node1.Checked)
        {
          flag = false;
          break;
        }
      }
      if (!flag)
        return;
      node.Parent.Checked = false;
      this.updateParentNodesUncheckedStatus(node.Parent);
    }

    private void updateParentNodesCheckedStatus(TreeNode node)
    {
      if (node.Parent == null || node.Parent.Checked)
        return;
      node.Parent.Checked = true;
      this.updateParentNodesCheckedStatus(node.Parent);
    }

    private void updateChildNodesCheckedUncheckedStatus(TreeNode node)
    {
      if (node.Nodes.Count <= 0)
        return;
      foreach (TreeNode node1 in node.Nodes)
      {
        node1.Checked = node.Checked;
        this.updateChildNodesCheckedUncheckedStatus(node1);
      }
    }

    private void getCheckedTaskTemplates(
      ref List<TaskTemplate> selectedTasks,
      TreeNodeCollection nodes = null)
    {
      if (nodes == null)
        nodes = this.treeWorkflowTasks.Nodes;
      foreach (TreeNode node in nodes)
      {
        if (node.Checked)
        {
          if (node.Tag is TaskTemplate)
            selectedTasks.Add((TaskTemplate) node.Tag);
          else
            this.getCheckedTaskTemplates(ref selectedTasks, node.Nodes);
        }
      }
    }

    private TaskTemplate[] getSelectedTaskTemplates()
    {
      List<TaskTemplate> selectedTasks = new List<TaskTemplate>();
      this.getCheckedTaskTemplates(ref selectedTasks, this.treeWorkflowTasks.Nodes);
      return selectedTasks.ToArray();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlExTreeView = new PanelEx();
      this.treeWorkflowTasks = new TreeView();
      this.lblMessage = new Label();
      this.pnlExTreeViewSearch = new PanelEx();
      this.pnlExTreeView.SuspendLayout();
      this.SuspendLayout();
      this.pnlExTreeView.Controls.Add((Control) this.treeWorkflowTasks);
      this.pnlExTreeView.Controls.Add((Control) this.lblMessage);
      this.pnlExTreeView.Dock = DockStyle.Fill;
      this.pnlExTreeView.Location = new Point(0, 26);
      this.pnlExTreeView.Name = "pnlExTreeView";
      this.pnlExTreeView.Size = new Size(150, 124);
      this.pnlExTreeView.TabIndex = 0;
      this.treeWorkflowTasks.CheckBoxes = true;
      this.treeWorkflowTasks.Dock = DockStyle.Fill;
      this.treeWorkflowTasks.Location = new Point(0, 0);
      this.treeWorkflowTasks.Name = "treeWorkflowTasks";
      this.treeWorkflowTasks.Size = new Size(150, 124);
      this.treeWorkflowTasks.TabIndex = 3;
      this.treeWorkflowTasks.AfterCheck += new TreeViewEventHandler(this.treeWorkflowTasks_AfterCheck);
      this.lblMessage.AutoSize = true;
      this.lblMessage.Location = new Point(4, 4);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(0, 13);
      this.lblMessage.TabIndex = 4;
      this.pnlExTreeViewSearch.Dock = DockStyle.Top;
      this.pnlExTreeViewSearch.Location = new Point(0, 0);
      this.pnlExTreeViewSearch.Name = "pnlExTreeViewSearch";
      this.pnlExTreeViewSearch.Size = new Size(150, 26);
      this.pnlExTreeViewSearch.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlExTreeView);
      this.Controls.Add((Control) this.pnlExTreeViewSearch);
      this.Name = nameof (MilestoneWorkflowTaskTree);
      this.Load += new System.EventHandler(this.MilestoneWorkflowTaskTree_Load);
      this.pnlExTreeView.ResumeLayout(false);
      this.pnlExTreeView.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

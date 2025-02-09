// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddTasks
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddTasks : Form
  {
    private Sessions.Session session;
    private MilestoneTaskListPanel taskList;
    private MilestoneWorkflowTaskTree workflowTaskTree;
    private ConditionRulePanel container;
    private TaskMilestonePair[] tasksForRule;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private ComboBox cboMilestone;
    private Label label1;
    private Panel panelForListView;
    private TabControl tabMilestoneTasks;
    private TabPage tpTask;
    private TabPage tpWorkflowTask;

    public AddTasks(Sessions.Session session, string[] milestoneList, ConditionRulePanel container)
    {
      this.InitializeComponent();
      this.session = session;
      this.cboMilestone.Items.AddRange((object[]) milestoneList);
      this.container = container;
      this.initForm();
    }

    private void initForm()
    {
      this.taskList = new MilestoneTaskListPanel(this.session, true);
      this.taskList.Dock = DockStyle.Fill;
      this.tpTask.Controls.Add((Control) this.taskList);
      if (this.container.IsWorkflowTaskAccessible)
        return;
      this.tabMilestoneTasks.TabPages.Remove(this.tpWorkflowTask);
    }

    public TaskMilestonePair[] TasksForRule => this.tasksForRule;

    public string SelectedMilestone => this.cboMilestone.Text;

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      TaskTemplate[] taskTemplateArray = this.workflowTaskTree != null ? this.workflowTaskTree.SelectedTasks : new TaskTemplate[0];
      MilestoneTaskDefinition[] selectedTasks = this.taskList.SelectedTasks;
      if (selectedTasks.Length == 0 && taskTemplateArray.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a task first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string milestoneID = string.IsNullOrWhiteSpace(this.cboMilestone.Text) ? "Processing" : this.cboMilestone.Text;
        this.tasksForRule = new TaskMilestonePair[selectedTasks.Length + taskTemplateArray.Length];
        int index1 = 0;
        int index2 = 0;
        while (index2 < selectedTasks.Length)
        {
          this.tasksForRule[index1] = new TaskMilestonePair(selectedTasks[index2].TaskGUID, milestoneID, true);
          ++index2;
          ++index1;
        }
        int index3 = 0;
        while (index3 < taskTemplateArray.Length)
        {
          this.tasksForRule[index1] = new TaskMilestonePair(taskTemplateArray[index3].TypeID, milestoneID, true, MilestoneTaskType.MilestoneWorkflow);
          ++index3;
          ++index1;
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void tabMilestoneTasks_Selected(object sender, TabControlEventArgs e)
    {
      if (e.TabPage != this.tpWorkflowTask || this.workflowTaskTree != null)
        return;
      int num1 = 0;
      TaskGroupTemplate[] taskGroupTemplates = new TaskGroupTemplate[this.container.WorkflowTaskGroupTemplatesInfoInSettings.Value.Count];
      TaskTemplate[] taskTemplates = new TaskTemplate[this.container.WorkflowTaskTemplatesInfoInSettings.Value.Count];
      foreach (DictionaryEntry dictionaryEntry in this.container.WorkflowTaskGroupTemplatesInfoInSettings.Value)
        taskGroupTemplates[num1++] = (TaskGroupTemplate) dictionaryEntry.Value;
      int num2 = 0;
      foreach (DictionaryEntry dictionaryEntry in this.container.WorkflowTaskTemplatesInfoInSettings.Value)
        taskTemplates[num2++] = (TaskTemplate) dictionaryEntry.Value;
      this.workflowTaskTree = new MilestoneWorkflowTaskTree(taskGroupTemplates, taskTemplates);
      this.workflowTaskTree.Dock = DockStyle.Fill;
      this.workflowTaskTree.Parent = (Control) this;
      this.tpWorkflowTask.Controls.Add((Control) this.workflowTaskTree);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dialogButtons1 = new DialogButtons();
      this.cboMilestone = new ComboBox();
      this.label1 = new Label();
      this.panelForListView = new Panel();
      this.tabMilestoneTasks = new TabControl();
      this.tpTask = new TabPage();
      this.tpWorkflowTask = new TabPage();
      this.panelForListView.SuspendLayout();
      this.tabMilestoneTasks.SuspendLayout();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 455);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(562, 44);
      this.dialogButtons1.TabIndex = 0;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.cboMilestone.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cboMilestone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboMilestone.FormattingEnabled = true;
      this.cboMilestone.Location = new Point(89, 429);
      this.cboMilestone.Name = "cboMilestone";
      this.cboMilestone.Size = new Size(121, 21);
      this.cboMilestone.TabIndex = 3;
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 433);
      this.label1.Name = "label1";
      this.label1.Size = new Size(73, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "For Milestone:";
      this.panelForListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelForListView.Controls.Add((Control) this.tabMilestoneTasks);
      this.panelForListView.Location = new Point(13, 12);
      this.panelForListView.Name = "panelForListView";
      this.panelForListView.Size = new Size(537, 411);
      this.panelForListView.TabIndex = 5;
      this.tabMilestoneTasks.Controls.Add((Control) this.tpTask);
      this.tabMilestoneTasks.Controls.Add((Control) this.tpWorkflowTask);
      this.tabMilestoneTasks.Dock = DockStyle.Fill;
      this.tabMilestoneTasks.Location = new Point(0, 0);
      this.tabMilestoneTasks.Name = "tabMilestoneTasks";
      this.tabMilestoneTasks.SelectedIndex = 0;
      this.tabMilestoneTasks.Size = new Size(537, 411);
      this.tabMilestoneTasks.TabIndex = 0;
      this.tabMilestoneTasks.Selected += new TabControlEventHandler(this.tabMilestoneTasks_Selected);
      this.tpTask.Location = new Point(4, 22);
      this.tpTask.Name = "tpTask";
      this.tpTask.Padding = new Padding(3);
      this.tpTask.Size = new Size(529, 385);
      this.tpTask.TabIndex = 0;
      this.tpTask.Text = "Tasks";
      this.tpTask.UseVisualStyleBackColor = true;
      this.tpWorkflowTask.Location = new Point(4, 22);
      this.tpWorkflowTask.Name = "tpWorkflowTask";
      this.tpWorkflowTask.Padding = new Padding(3);
      this.tpWorkflowTask.Size = new Size(529, 385);
      this.tpWorkflowTask.TabIndex = 1;
      this.tpWorkflowTask.Text = "Workflow Tasks";
      this.tpWorkflowTask.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(562, 499);
      this.Controls.Add((Control) this.panelForListView);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboMilestone);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (AddTasks);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Tasks";
      this.panelForListView.ResumeLayout(false);
      this.tabMilestoneTasks.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

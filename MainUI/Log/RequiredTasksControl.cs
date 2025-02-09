// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.RequiredTasksControl
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class RequiredTasksControl : UserControl
  {
    private const string className = "RequiredTasksControl";
    private static readonly string sw = Tracing.SwDataEngine;
    private LogList logList;
    private MilestoneLog selectedMilestone;
    private LoanData loan;
    private TaskMilestonePair[] reqTasks;
    private Hashtable taskSetup;
    private bool inChecked;
    private Hashtable loanMilestones;
    private bool isServiceError;
    private string msgWorkflowTaskRequired = "{0} is not yet created within the Loan.{1}Create & complete the Task before completing the Milestone.";
    internal Lazy<Hashtable> workflowTaskTemplates = new Lazy<Hashtable>((Func<Hashtable>) (() => Session.SessionObjects.CachedWorkflowTaskTemplates));
    private bool hasWorkFlowTaskItem;
    private bool editable = true;
    private IContainer components;
    private Button btnList;
    private GroupContainer groupContainer1;
    private GridView gridViewTasks;
    private ImageList imageList1;
    private ToolTip toolTip1;
    private Panel panel1;
    private Label lblMsg1;

    private bool isWorkflowTaskAccessible
    {
      get
      {
        return Session.DefaultInstance.EncompassEdition == EncompassEdition.Banker && Session.StartupInfo.EnableWorkFlowTasks;
      }
    }

    public bool Editable
    {
      set => this.btnList.Enabled = this.editable = value;
    }

    public RequiredTasksControl()
    {
      this.loan = Session.LoanData;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.initControls();
    }

    public RequiredTasksControl(
      LogList logList,
      MilestoneLog selectedMilestone,
      Hashtable loanMilestones)
    {
      this.logList = logList;
      this.selectedMilestone = selectedMilestone;
      this.loan = Session.LoanData;
      this.editable = true;
      this.loanMilestones = loanMilestones;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      try
      {
        this.taskSetup = Session.LoanDataMgr == null ? Session.ConfigurationManager.GetMilestoneTasks() : Session.LoanDataMgr.SystemConfiguration.TasksSetup;
      }
      catch (Exception ex)
      {
        Tracing.Log(RequiredTasksControl.sw, TraceLevel.Error, nameof (RequiredTasksControl), "RequiredTasksControl: can't load task setup. Error: " + ex.Message);
      }
      this.initControls();
    }

    private void initControls()
    {
      this.panel1.Visible = this.isWorkflowTaskAccessible && this.hasWorkFlowTaskItem;
    }

    public void RefreshHistoryTaskList(List<MilestoneTaskLog> tasks)
    {
      this.gridViewTasks.SubItemCheck -= new GVSubItemEventHandler(this.gridViewTasks_SubItemCheck);
      this.gridViewTasks.Items.Clear();
      this.gridViewTasks.BeginUpdate();
      for (int index = 0; index < tasks.Count; ++index)
      {
        GVItem gvItem = new GVItem(tasks[index].IsRequired ? "1" : "");
        gvItem.SubItems.Add((object) this.buildTaskStatusString(tasks[index]));
        gvItem.Tag = (object) tasks[index];
        if (tasks[index].CompletedDate != DateTime.MinValue)
          gvItem.SubItems[1].Checked = true;
        if (tasks[index].IsRequired)
        {
          gvItem.ImageIndex = 0;
          gvItem.SubItems[0].ImageAlignment = HorizontalAlignment.Left;
        }
        this.gridViewTasks.Items.Add(gvItem);
      }
      this.gridViewTasks.EndUpdate();
      this.gridViewTasks.SubItemCheck += new GVSubItemEventHandler(this.gridViewTasks_SubItemCheck);
    }

    public void RefreshTaskList(TaskMilestonePair[] reqTasks, bool includePrevious)
    {
      if (reqTasks != null)
        this.reqTasks = reqTasks;
      MilestoneLog milestoneLog = (MilestoneLog) null;
      MilestoneLog[] allMilestones = this.logList.GetAllMilestones();
      for (int index = 0; index < allMilestones.Length; ++index)
      {
        if (!allMilestones[index].Done)
        {
          milestoneLog = allMilestones[index];
          break;
        }
      }
      Hashtable insensitiveHashtable1 = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (this.reqTasks != null)
      {
        string empty = string.Empty;
        for (int index = 0; index < this.reqTasks.Length; ++index)
        {
          string milestoneId = this.reqTasks[index].MilestoneID;
          if (this.loanMilestones.ContainsKey((object) milestoneId))
          {
            string str = this.loanMilestones[(object) milestoneId].ToString();
            if (!insensitiveHashtable1.ContainsKey((object) (this.reqTasks[index].TaskGuid + "|" + str)))
              insensitiveHashtable1.Add((object) (this.reqTasks[index].TaskGuid + "|" + str), (object) this.reqTasks[index]);
          }
        }
      }
      MilestoneTaskLog[] milestoneTaskLogs1 = this.logList.GetAllMilestoneTaskLogs(this.selectedMilestone.Stage);
      MilestoneTaskLog[] milestoneTaskLogs2 = this.logList.GetAllMilestoneTaskLogs((string) null);
      foreach (MilestoneTaskLog milestoneTaskLog in milestoneTaskLogs2)
      {
        if (milestoneLog != null && string.Compare(milestoneTaskLog.Stage, milestoneLog.Stage, true) == 0 && milestoneTaskLog.DaysToComplete == 0 && milestoneTaskLog.DaysToCompleteFromSetting > 0)
        {
          milestoneTaskLog.DaysToComplete = milestoneTaskLog.DaysToCompleteFromSetting;
          milestoneTaskLog.DaysToCompleteFromSetting = -1;
        }
      }
      this.gridViewTasks.Items.Clear();
      this.gridViewTasks.BeginUpdate();
      this.gridViewTasks.SubItemCheck -= new GVSubItemEventHandler(this.gridViewTasks_SubItemCheck);
      Hashtable insensitiveHashtable2 = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index = 0; index < milestoneTaskLogs1.Length; ++index)
      {
        if (insensitiveHashtable1 != null && insensitiveHashtable1.ContainsKey((object) (milestoneTaskLogs1[index].TaskGUID + "|" + milestoneTaskLogs1[index].Stage)))
        {
          TaskMilestonePair taskMilestonePair = (TaskMilestonePair) insensitiveHashtable1[(object) (milestoneTaskLogs1[index].TaskGUID + "|" + milestoneTaskLogs1[index].Stage)];
          milestoneTaskLogs1[index].IsRequired = taskMilestonePair.isRequired;
        }
        else
          milestoneTaskLogs1[index].IsRequired = false;
        GVItem gvItem = new GVItem(milestoneTaskLogs1[index].IsRequired ? "1" : "");
        gvItem.SubItems.Add((object) this.buildTaskStatusString(milestoneTaskLogs1[index]));
        gvItem.Tag = (object) milestoneTaskLogs1[index];
        if (milestoneTaskLogs1[index].CompletedDate != DateTime.MinValue)
          gvItem.SubItems[1].Checked = true;
        if (milestoneTaskLogs1[index].IsRequired)
        {
          gvItem.ImageIndex = 0;
          gvItem.SubItems[0].ImageAlignment = HorizontalAlignment.Left;
        }
        this.gridViewTasks.Items.Add(gvItem);
        if (!insensitiveHashtable2.ContainsKey((object) (milestoneTaskLogs1[index].TaskGUID + "|" + milestoneTaskLogs1[index].Stage)))
          insensitiveHashtable2.Add((object) (milestoneTaskLogs1[index].TaskGUID + "|" + milestoneTaskLogs1[index].Stage), (object) milestoneTaskLogs1[index]);
      }
      if (insensitiveHashtable1 != null && insensitiveHashtable1.Count > 0)
      {
        foreach (DictionaryEntry dictionaryEntry in insensitiveHashtable1)
        {
          TaskMilestonePair taskMilestonePair = (TaskMilestonePair) dictionaryEntry.Value;
          string key = taskMilestonePair.MilestoneID.ToString();
          if (this.loanMilestones.ContainsKey((object) key))
          {
            string strB = this.loanMilestones[(object) key].ToString();
            if ((!(this.selectedMilestone.Stage != strB) || includePrevious) && !insensitiveHashtable2.ContainsKey((object) (taskMilestonePair.TaskGuid + "|" + strB)) && this.taskSetup != null && this.taskSetup.ContainsKey((object) taskMilestonePair.TaskGuid))
            {
              MilestoneTaskDefinition milestoneTaskDefinition = (MilestoneTaskDefinition) this.taskSetup[(object) taskMilestonePair.TaskGuid];
              MilestoneTaskLog milestoneTaskLog = (MilestoneTaskLog) null;
              for (int index = 0; index < milestoneTaskLogs2.Length; ++index)
              {
                if (milestoneTaskLogs2[index].TaskGUID == milestoneTaskDefinition.TaskGUID && string.Compare(milestoneTaskLogs2[index].Stage, strB, true) == 0)
                {
                  milestoneTaskLog = milestoneTaskLogs2[index];
                  break;
                }
              }
              if (milestoneTaskLog == null)
              {
                milestoneTaskLog = new MilestoneTaskLog(Session.UserInfo, milestoneTaskDefinition.TaskName, milestoneTaskDefinition.TaskDescription);
                milestoneTaskLog.TaskGUID = milestoneTaskDefinition.TaskGUID;
                milestoneTaskLog.Stage = this.selectedMilestone.Stage;
                milestoneTaskLog.IsRequired = taskMilestonePair.isRequired;
                milestoneTaskLog.TaskPriority = milestoneTaskDefinition.TaskPriority.ToString();
                milestoneTaskLog.DaysToComplete = milestoneTaskDefinition.DaysToComplete;
                if (milestoneLog != null && milestoneLog.Guid == this.selectedMilestone.Guid)
                  Session.LoanData.GetLogList().AddRecord((LogRecordBase) milestoneTaskLog);
                else
                  milestoneTaskLog.AddedToLog = false;
              }
              if (!milestoneTaskLog.Completed || string.Compare(milestoneTaskLog.Stage, this.selectedMilestone.Stage, true) == 0)
              {
                GVItem gvItem = new GVItem(milestoneTaskLog.IsRequired ? "1" : "");
                gvItem.SubItems.Add((object) this.buildTaskStatusString(milestoneTaskLog));
                if (milestoneTaskLog.CompletedDate != DateTime.MinValue)
                  gvItem.SubItems[1].Checked = true;
                gvItem.ImageIndex = milestoneTaskLog.IsRequired ? 0 : -1;
                gvItem.Tag = (object) milestoneTaskLog;
                this.gridViewTasks.Items.Add(gvItem);
              }
            }
          }
        }
      }
      this.hasWorkFlowTaskItem = false;
      this.RefreshWorkflowTaskList(includePrevious);
      this.gridViewTasks.EndUpdate();
      this.gridViewTasks.SubItemCheck += new GVSubItemEventHandler(this.gridViewTasks_SubItemCheck);
      this.initControls();
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
    }

    public void RefreshWorkflowTaskList(bool includePrevious)
    {
      if (!this.isWorkflowTaskAccessible || this.reqTasks == null)
        return;
      IEnumerable<TaskMilestonePair> source1 = ((IEnumerable<TaskMilestonePair>) this.reqTasks).Where<TaskMilestonePair>((Func<TaskMilestonePair, bool>) (rt => rt.TaskType == MilestoneTaskType.MilestoneWorkflow && this.loanMilestones.Contains((object) rt.MilestoneID) && this.loanMilestones[(object) rt.MilestoneID].ToString() == this.selectedMilestone.Stage | includePrevious && this.workflowTaskTemplates.Value.ContainsKey((object) rt.TaskGuid)));
      if (source1 == null || !source1.Any<TaskMilestonePair>())
        return;
      string loanId = new Regex("[{}]").Replace(this.loan.GUID, string.Empty);
      IEnumerable<string> source2 = source1.Select<TaskMilestonePair, string>((Func<TaskMilestonePair, string>) (rt => rt.TaskGuid));
      WorkflowTask[] tasksBy;
      try
      {
        tasksBy = MilestoneWorkFlowTaskApiHelper.GetTasksBy(loanId, source2.ToArray<string>());
      }
      catch (Exception ex)
      {
        foreach (TaskMilestonePair taskMilestonePair in source1)
          this.addWorkflowTaskToGrid(taskMilestonePair.TaskGuid, true);
        if (ex.InnerException == null || !(ex.InnerException is HttpException) || (ex.InnerException as HttpException).GetHttpCode() < 500)
          return;
        this.isServiceError = true;
        return;
      }
      List<string> wtTypeIDs = new List<string>();
      if (tasksBy != null && tasksBy.Length != 0)
      {
        foreach (WorkflowTask workflowTask in tasksBy)
        {
          wtTypeIDs.Add(workflowTask.TypeID);
          if (workflowTask.Status.ToUpper() != "COMPLETED")
            this.addWorkflowTaskToGrid(workflowTask.Name);
        }
      }
      foreach (object key in source2.Where<string>((Func<string, bool>) (rt => !wtTypeIDs.Contains(rt))))
        this.addWorkflowTaskToGrid(((TaskTemplate) this.workflowTaskTemplates.Value[key]).Name, true);
    }

    public void RefreshTask(MilestoneTaskLog taskLog)
    {
      this.gridViewTasks.SubItemCheck -= new GVSubItemEventHandler(this.gridViewTasks_SubItemCheck);
      for (int nItemIndex = 0; nItemIndex < this.gridViewTasks.Items.Count; ++nItemIndex)
      {
        if (!(this.gridViewTasks.Items[nItemIndex].Tag is MilestoneTaskType) && ((LogRecordBase) this.gridViewTasks.Items[nItemIndex].Tag).Guid == taskLog.Guid)
        {
          this.gridViewTasks.Items[nItemIndex].Text = taskLog.IsRequired ? "1" : "";
          this.gridViewTasks.Items[nItemIndex].SubItems[1].Text = this.buildTaskStatusString(taskLog);
          this.gridViewTasks.Items[nItemIndex].SubItems[1].Checked = taskLog.Completed;
          this.gridViewTasks.Items[nItemIndex].Tag = (object) taskLog;
          break;
        }
      }
      this.gridViewTasks.SubItemCheck += new GVSubItemEventHandler(this.gridViewTasks_SubItemCheck);
    }

    private string buildTaskStatusString(MilestoneTaskLog taskLog)
    {
      return taskLog.AddedToLog ? taskLog.TaskName + (taskLog.CompletedDate != DateTime.MinValue ? " completed on " + taskLog.CompletedDate.ToString("MM/dd/yyyy") : (taskLog.DaysToComplete > 0 ? " expected on " + taskLog.ExpectedDate.ToString("MM/dd/yyyy") : "")) : taskLog.TaskName;
    }

    private void gridViewTasks_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.gridViewTasks.SubItemCheck -= new GVSubItemEventHandler(this.gridViewTasks_SubItemCheck);
      if (!this.editable)
      {
        e.SubItem.Checked = !e.SubItem.Checked;
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to edit task.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.gridViewTasks.SubItemCheck += new GVSubItemEventHandler(this.gridViewTasks_SubItemCheck);
      }
      else
      {
        this.inChecked = true;
        MilestoneTaskLog tag = (MilestoneTaskLog) this.gridViewTasks.Items[e.SubItem.Item.Index].Tag;
        if (!tag.AddedToLog)
        {
          tag.Date = DateTime.Now;
          Session.LoanData.GetLogList().AddRecord((LogRecordBase) tag);
          tag.AddedToLog = true;
        }
        if (e.SubItem.Checked)
          tag.MarkAsCompleted(DateTime.Now, Session.UserInfo);
        else
          tag.UnmarkAsCompleted();
        this.RefreshTask(tag);
        Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        this.gridViewTasks.SubItemCheck += new GVSubItemEventHandler(this.gridViewTasks_SubItemCheck);
      }
    }

    private void btnList_Click(object sender, EventArgs e)
    {
      using (MilestoneTaskListContainer taskListContainer = new MilestoneTaskListContainer(Session.LoanDataMgr))
      {
        int num = (int) taskListContainer.ShowDialog((IWin32Window) this);
        this.RefreshTaskList((TaskMilestonePair[]) null, false);
      }
    }

    private void gridViewTasks_SubItemEnter(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index == 1)
        return;
      if (e.SubItem.ImageIndex == 0)
      {
        this.toolTip1.Show("Task is required!", (IWin32Window) this, new Point(20, e.SubItem.IconRegion.Y + 50));
      }
      else
      {
        if (e.SubItem.ImageIndex != 2)
          return;
        if (this.isServiceError)
        {
          this.toolTip1.Show("Error retrieving Task Details.", (IWin32Window) this, e.SubItem.IconRegion.X, e.SubItem.IconRegion.Y + 70);
        }
        else
        {
          ToolTip toolTip1 = this.toolTip1;
          string text = string.Format(this.msgWorkflowTaskRequired, (object) e.SubItem.Item.SubItems[1].Text, (object) Environment.NewLine);
          Rectangle iconRegion = e.SubItem.IconRegion;
          int x = iconRegion.X;
          iconRegion = e.SubItem.IconRegion;
          int y = iconRegion.Y + 70;
          toolTip1.Show(text, (IWin32Window) this, x, y);
        }
      }
    }

    private void gridViewTasks_SubItemLeave(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index == 1)
        return;
      this.toolTip1.RemoveAll();
    }

    private void gridViewTasks_SubItemClick(object source, GVSubItemMouseEventArgs e)
    {
      if (e.SubItem.Item.Tag is MilestoneTaskType)
        return;
      if (this.inChecked)
      {
        this.inChecked = false;
      }
      else
      {
        this.inChecked = false;
        if (e.SubItem.Index != 1)
          return;
        MilestoneTaskLog tag = (MilestoneTaskLog) this.gridViewTasks.Items[e.SubItem.Item.Index].Tag;
        if (tag == null)
          return;
        using (MilestoneTaskWorksheetContainer worksheetContainer = new MilestoneTaskWorksheetContainer(tag, this.editable))
        {
          if (worksheetContainer.ShowDialog() != DialogResult.OK)
            return;
          if (!tag.AddedToLog)
          {
            tag.Date = DateTime.Now;
            Session.LoanData.GetLogList().AddRecord((LogRecordBase) tag);
            tag.AddedToLog = true;
          }
          this.RefreshTask(tag);
          Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        }
      }
    }

    public bool VerifyRequiredTasks()
    {
      if (this.reqTasks == null || this.reqTasks.Length == 0)
        return true;
      for (int nItemIndex = 0; nItemIndex < this.gridViewTasks.Items.Count; ++nItemIndex)
      {
        if (this.gridViewTasks.Items[nItemIndex].Tag is MilestoneTaskLog && ((MilestoneTaskLog) this.gridViewTasks.Items[nItemIndex].Tag).IsRequired && !this.gridViewTasks.Items[nItemIndex].SubItems[1].Checked)
          return false;
      }
      return this.verifyRequiredWorkflowTasks();
    }

    private bool verifyRequiredWorkflowTasks()
    {
      if (!this.isWorkflowTaskAccessible)
        return true;
      for (int nItemIndex = 0; nItemIndex < this.gridViewTasks.Items.Count; ++nItemIndex)
      {
        if (this.gridViewTasks.Items[nItemIndex].Tag is MilestoneTaskType)
          return false;
      }
      return true;
    }

    private void addWorkflowTaskToGrid(string name, bool isError = false)
    {
      GVItem gvItem = new GVItem();
      gvItem.ImageIndex = 0;
      gvItem.SubItems.Add((object) name);
      gvItem.SubItems[1].ImageIndex = 1;
      gvItem.SubItems[1].CheckBoxEnabled = false;
      if (isError)
        gvItem.SubItems[2].ImageIndex = 2;
      gvItem.Tag = (object) MilestoneTaskType.MilestoneWorkflow;
      this.gridViewTasks.Items.Add(gvItem);
      this.hasWorkFlowTaskItem = true;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RequiredTasksControl));
      this.btnList = new Button();
      this.groupContainer1 = new GroupContainer();
      this.gridViewTasks = new GridView();
      this.imageList1 = new ImageList(this.components);
      this.panel1 = new Panel();
      this.lblMsg1 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.btnList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnList.Location = new Point(391, 2);
      this.btnList.Name = "btnList";
      this.btnList.Size = new Size(73, 22);
      this.btnList.TabIndex = 3;
      this.btnList.Text = "Task List";
      this.btnList.UseVisualStyleBackColor = true;
      this.btnList.Click += new EventHandler(this.btnList_Click);
      this.groupContainer1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.gridViewTasks);
      this.groupContainer1.Controls.Add((Control) this.panel1);
      this.groupContainer1.Controls.Add((Control) this.btnList);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(469, 274);
      this.groupContainer1.TabIndex = 8;
      this.groupContainer1.Text = "Tasks";
      this.gridViewTasks.AllowColumnResize = false;
      this.gridViewTasks.AllowMultiselect = false;
      this.gridViewTasks.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Task";
      gvColumn1.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn1.Width = 20;
      gvColumn2.CheckBoxes = true;
      gvColumn2.Cursor = Cursors.Hand;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Column";
      gvColumn2.Width = 397;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "";
      gvColumn3.Width = 50;
      this.gridViewTasks.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewTasks.Dock = DockStyle.Fill;
      this.gridViewTasks.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gridViewTasks.HeaderHeight = 0;
      this.gridViewTasks.HeaderVisible = false;
      this.gridViewTasks.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewTasks.ImageList = this.imageList1;
      this.gridViewTasks.Location = new Point(1, 50);
      this.gridViewTasks.Name = "gridViewTasks";
      this.gridViewTasks.Selectable = false;
      this.gridViewTasks.Size = new Size(467, 224);
      this.gridViewTasks.TabIndex = 8;
      this.gridViewTasks.SubItemEnter += new GVSubItemEventHandler(this.gridViewTasks_SubItemEnter);
      this.gridViewTasks.SubItemLeave += new GVSubItemEventHandler(this.gridViewTasks_SubItemLeave);
      this.gridViewTasks.SubItemClick += new GVSubItemMouseEventHandler(this.gridViewTasks_SubItemClick);
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "required.png");
      this.imageList1.Images.SetKeyName(1, "tasks.png");
      this.imageList1.Images.SetKeyName(2, "warning-32x32.png");
      this.panel1.BackColor = Color.White;
      this.panel1.Controls.Add((Control) this.lblMsg1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(467, 24);
      this.panel1.TabIndex = 11;
      this.lblMsg1.AutoSize = true;
      this.lblMsg1.BackColor = Color.White;
      this.lblMsg1.Location = new Point(18, 5);
      this.lblMsg1.Name = "lblMsg1";
      this.lblMsg1.Size = new Size(285, 13);
      this.lblMsg1.TabIndex = 9;
      this.lblMsg1.Text = "Review and complete Workflow Tasks in Encompass Web";
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (RequiredTasksControl);
      this.Size = new Size(469, 274);
      this.groupContainer1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

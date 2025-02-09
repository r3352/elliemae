// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.TaskList.TaskListForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.TaskList;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.TaskList
{
  public class TaskListForm : Form, ITaskList
  {
    public static readonly DateTime DateTimeMinValue = new DateTime(1900, 1, 1);
    public static readonly DateTime DateTimeMaxValue = new DateTime(2079, 6, 6);
    private GroupContainer gcTask;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnExport;
    private StandardIconButton btnDelete;
    private StandardIconButton btnNew;
    private ToolTip toolTip1;
    private StandardIconButton btnEdit;
    private ContextMenuStrip mnuTasks;
    private ToolStripMenuItem menuItemNew;
    private ToolStripMenuItem menuItemOpen;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem menuItemDelete;
    private ToolStripMenuItem statusToolStripMenuItem;
    private ToolStripMenuItem notStartedToolStripMenuItem;
    private ToolStripMenuItem inProgressToolStripMenuItem;
    private ToolStripMenuItem completedToolStripMenuItem;
    private ToolStripMenuItem waitingOnSomeoneElseToolStripMenuItem;
    private ToolStripMenuItem deferredToolStripMenuItem;
    private bool isLoaded;
    private GridView gvTasks;
    private IContainer components;

    public TaskListForm()
    {
      this.InitializeComponent();
      Session.Application.RegisterService((object) this, typeof (ITaskList));
      this.disableMenuOption();
    }

    public void LoadTasks()
    {
      TaskInfo[] allTasksForUser = Session.ContactManager.GetAllTasksForUser(Session.UserID);
      if (allTasksForUser != null)
      {
        this.gvTasks.Items.Clear();
        this.gvTasks.BeginUpdate();
        for (int index = 0; index < allTasksForUser.Length; ++index)
        {
          string[] items = new string[5]
          {
            TaskInfoUtils.TaskStatusStrings[(int) allTasksForUser[index].TaskStatus],
            allTasksForUser[index].Subject,
            null,
            null,
            null
          };
          DateTime dueDate = allTasksForUser[index].DueDate;
          items[2] = dueDate == DateTime.MaxValue ? "None" : dueDate.ToString("M/d/yyyy");
          items[3] = allTasksForUser[index].CreationTime.ToString("M/d/yyyy H:mm tt");
          items[4] = allTasksForUser[index].CampaignInfo;
          this.gvTasks.Items.Add(new GVItem(items)
          {
            Checked = allTasksForUser[index].TaskStatus == TaskStatus.Completed,
            Tag = (object) allTasksForUser[index]
          });
        }
        this.gvTasks.EndUpdate();
      }
      this.btnDelete.Enabled = false;
      this.btnEdit.Enabled = false;
      this.btnExport.Enabled = this.gvTasks.SelectedItems.Count > 0;
      this.isLoaded = true;
      this.gcTask.Text = "Tasks (" + (object) this.gvTasks.Items.Count + ")";
    }

    public bool IsMenuItemEnabled(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Task_NewTask:
          flag = this.btnNew.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Task_EditTask:
          flag = this.btnEdit.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Task_DeleteTask:
          flag = this.btnDelete.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Task_ExportExcel:
          flag = this.btnExport.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Task_Status:
          flag = this.gvTasks.SelectedItems.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Task_Status_NotStarted:
          flag = this.gvTasks.SelectedItems.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Task_Status_InProgress:
          flag = this.gvTasks.SelectedItems.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Task_Status_Completed:
          flag = this.gvTasks.SelectedItems.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Task_Status_WaitOnSomeoneElse:
          flag = this.gvTasks.SelectedItems.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Task_Status_Deferred:
          flag = this.gvTasks.SelectedItems.Count > 0;
          break;
      }
      return flag;
    }

    public bool IsMenuItemVisible(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Task_NewTask:
          flag = this.btnNew.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Task_EditTask:
          flag = this.btnEdit.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Task_DeleteTask:
          flag = this.btnDelete.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Task_ExportExcel:
          flag = this.btnExport.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Task_Status:
        case ContactMainForm.ContactsActionEnum.Task_Status_NotStarted:
        case ContactMainForm.ContactsActionEnum.Task_Status_InProgress:
        case ContactMainForm.ContactsActionEnum.Task_Status_Completed:
        case ContactMainForm.ContactsActionEnum.Task_Status_WaitOnSomeoneElse:
        case ContactMainForm.ContactsActionEnum.Task_Status_Deferred:
          flag = true;
          break;
      }
      return flag;
    }

    public void TriggerContactAction(ContactMainForm.ContactsActionEnum action)
    {
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Task_NewTask:
          this.btnNew_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Task_EditTask:
          this.btnEdit_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Task_DeleteTask:
          this.btnDelete_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Task_ExportExcel:
          this.btnExport_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Task_Status_NotStarted:
          using (IEnumerator<GVItem> enumerator = this.gvTasks.SelectedItems.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.changeStatus(enumerator.Current.Index, TaskStatus.NotStarted);
            break;
          }
        case ContactMainForm.ContactsActionEnum.Task_Status_InProgress:
          using (IEnumerator<GVItem> enumerator = this.gvTasks.SelectedItems.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.changeStatus(enumerator.Current.Index, TaskStatus.InProgress);
            break;
          }
        case ContactMainForm.ContactsActionEnum.Task_Status_Completed:
          using (IEnumerator<GVItem> enumerator = this.gvTasks.SelectedItems.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.changeStatus(enumerator.Current.Index, TaskStatus.Completed);
            break;
          }
        case ContactMainForm.ContactsActionEnum.Task_Status_WaitOnSomeoneElse:
          using (IEnumerator<GVItem> enumerator = this.gvTasks.SelectedItems.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.changeStatus(enumerator.Current.Index, TaskStatus.WaitingOnSomeoneElse);
            break;
          }
        case ContactMainForm.ContactsActionEnum.Task_Status_Deferred:
          using (IEnumerator<GVItem> enumerator = this.gvTasks.SelectedItems.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.changeStatus(enumerator.Current.Index, TaskStatus.Deferred);
            break;
          }
      }
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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.gvTasks = new GridView();
      this.mnuTasks = new ContextMenuStrip(this.components);
      this.menuItemNew = new ToolStripMenuItem();
      this.menuItemOpen = new ToolStripMenuItem();
      this.menuItemDelete = new ToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripSeparator();
      this.statusToolStripMenuItem = new ToolStripMenuItem();
      this.notStartedToolStripMenuItem = new ToolStripMenuItem();
      this.inProgressToolStripMenuItem = new ToolStripMenuItem();
      this.completedToolStripMenuItem = new ToolStripMenuItem();
      this.waitingOnSomeoneElseToolStripMenuItem = new ToolStripMenuItem();
      this.deferredToolStripMenuItem = new ToolStripMenuItem();
      this.gcTask = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnExport = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.mnuTasks.SuspendLayout();
      this.gcTask.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnExport).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      this.SuspendLayout();
      this.gvTasks.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Status";
      gvColumn1.Width = 110;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Subject";
      gvColumn2.Width = 369;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Date;
      gvColumn3.Text = "Due Date";
      gvColumn3.Width = 90;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.DateTime;
      gvColumn4.Text = "Creation Time";
      gvColumn4.Width = 125;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Campaign";
      gvColumn5.Width = 120;
      this.gvTasks.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvTasks.ContextMenuStrip = this.mnuTasks;
      this.gvTasks.Dock = DockStyle.Fill;
      this.gvTasks.Location = new Point(1, 26);
      this.gvTasks.Name = "gvTasks";
      this.gvTasks.Size = new Size(814, 419);
      this.gvTasks.TabIndex = 3;
      this.gvTasks.SelectedIndexChanged += new EventHandler(this.gvTasks_SelectedIndexChanged);
      this.gvTasks.DoubleClick += new EventHandler(this.gvTasks_DoubleClick);
      this.mnuTasks.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.menuItemNew,
        (ToolStripItem) this.menuItemOpen,
        (ToolStripItem) this.menuItemDelete,
        (ToolStripItem) this.toolStripMenuItem2,
        (ToolStripItem) this.statusToolStripMenuItem
      });
      this.mnuTasks.Name = "mnuTasks";
      this.mnuTasks.ShowImageMargin = false;
      this.mnuTasks.Size = new Size(117, 98);
      this.menuItemNew.Name = "menuItemNew";
      this.menuItemNew.Size = new Size(116, 22);
      this.menuItemNew.Text = "New Task";
      this.menuItemNew.Click += new EventHandler(this.menuItemNew_Click);
      this.menuItemOpen.Name = "menuItemOpen";
      this.menuItemOpen.Size = new Size(116, 22);
      this.menuItemOpen.Text = "Edit Task";
      this.menuItemOpen.Click += new EventHandler(this.menuItemOpen_Click);
      this.menuItemDelete.Name = "menuItemDelete";
      this.menuItemDelete.Size = new Size(116, 22);
      this.menuItemDelete.Text = "Delete Task";
      this.menuItemDelete.Click += new EventHandler(this.menuItemDelete_Click);
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new Size(113, 6);
      this.statusToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.notStartedToolStripMenuItem,
        (ToolStripItem) this.inProgressToolStripMenuItem,
        (ToolStripItem) this.completedToolStripMenuItem,
        (ToolStripItem) this.waitingOnSomeoneElseToolStripMenuItem,
        (ToolStripItem) this.deferredToolStripMenuItem
      });
      this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
      this.statusToolStripMenuItem.Size = new Size(116, 22);
      this.statusToolStripMenuItem.Text = "Status";
      this.notStartedToolStripMenuItem.Name = "notStartedToolStripMenuItem";
      this.notStartedToolStripMenuItem.Size = new Size(205, 22);
      this.notStartedToolStripMenuItem.Text = "Not Started";
      this.notStartedToolStripMenuItem.Click += new EventHandler(this.StatusToolStripMenuItem_Click);
      this.inProgressToolStripMenuItem.Name = "inProgressToolStripMenuItem";
      this.inProgressToolStripMenuItem.Size = new Size(205, 22);
      this.inProgressToolStripMenuItem.Text = "In Progress";
      this.inProgressToolStripMenuItem.Click += new EventHandler(this.StatusToolStripMenuItem_Click);
      this.completedToolStripMenuItem.Name = "completedToolStripMenuItem";
      this.completedToolStripMenuItem.Size = new Size(205, 22);
      this.completedToolStripMenuItem.Text = "Completed";
      this.completedToolStripMenuItem.Click += new EventHandler(this.StatusToolStripMenuItem_Click);
      this.waitingOnSomeoneElseToolStripMenuItem.Name = "waitingOnSomeoneElseToolStripMenuItem";
      this.waitingOnSomeoneElseToolStripMenuItem.Size = new Size(205, 22);
      this.waitingOnSomeoneElseToolStripMenuItem.Text = "Waiting on Someone Else";
      this.waitingOnSomeoneElseToolStripMenuItem.Click += new EventHandler(this.StatusToolStripMenuItem_Click);
      this.deferredToolStripMenuItem.Name = "deferredToolStripMenuItem";
      this.deferredToolStripMenuItem.Size = new Size(205, 22);
      this.deferredToolStripMenuItem.Text = "Deferred";
      this.deferredToolStripMenuItem.Click += new EventHandler(this.StatusToolStripMenuItem_Click);
      this.gcTask.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcTask.Controls.Add((Control) this.gvTasks);
      this.gcTask.Dock = DockStyle.Fill;
      this.gcTask.Location = new Point(0, 0);
      this.gcTask.Name = "gcTask";
      this.gcTask.Size = new Size(816, 446);
      this.gcTask.TabIndex = 11;
      this.gcTask.Text = "Tasks";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnExport);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnEdit);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNew);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(192, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(620, 22);
      this.flowLayoutPanel1.TabIndex = 4;
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Location = new Point(602, 3);
      this.btnExport.Margin = new Padding(3, 3, 2, 3);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(16, 16);
      this.btnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExport.TabIndex = 0;
      this.btnExport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnExport, "Export to Excel");
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(581, 3);
      this.btnDelete.Margin = new Padding(3, 3, 2, 3);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 1;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Task");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(560, 3);
      this.btnEdit.Margin = new Padding(3, 3, 2, 3);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 3;
      this.btnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEdit, "Edit Task");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(539, 3);
      this.btnNew.Margin = new Padding(3, 3, 2, 3);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 2;
      this.btnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNew, "New Task");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(816, 446);
      this.Controls.Add((Control) this.gcTask);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (TaskListForm);
      this.Text = nameof (TaskListForm);
      this.mnuTasks.ResumeLayout(false);
      this.gcTask.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnExport).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      this.ResumeLayout(false);
    }

    private void displayTaskDetails(int idx)
    {
      TaskInfo task = (TaskInfo) null;
      if (idx >= 0)
        task = (TaskInfo) this.gvTasks.Items[idx].Tag;
      this.DisplayTask(task);
    }

    public void CreateTask() => this.DisplayTask((TaskInfo) null);

    public void DisplayTask(TaskInfo task)
    {
      using (TaskDetailsForm taskDetailsForm = new TaskDetailsForm(task))
      {
        if (task != null && task.TaskType == TaskType.CampaignTask)
          taskDetailsForm.Behavior = TaskDetailsForm.FormBehavior.DisableContactsButton | TaskDetailsForm.FormBehavior.DisableDeleteButton | TaskDetailsForm.FormBehavior.EnableUserSelection | TaskDetailsForm.FormBehavior.EnableMultiSelection;
        taskDetailsForm.TaskUpdatedEvent += new TaskDetailsForm.TaskUpdatedEventHandler(this.frmTaskDetails_TaskUpdatedEvent);
        int num = (int) taskDetailsForm.ShowDialog();
        taskDetailsForm.TaskUpdatedEvent -= new TaskDetailsForm.TaskUpdatedEventHandler(this.frmTaskDetails_TaskUpdatedEvent);
      }
    }

    public bool DisplayTask(int taskId)
    {
      if (!this.isLoaded)
        this.LoadTasks();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTasks.Items)
      {
        if (((TaskInfo) gvItem.Tag).TaskID == taskId)
        {
          this.DisplayTask(gvItem.Tag as TaskInfo);
          return true;
        }
      }
      return false;
    }

    private void btnNew_Click(object sender, EventArgs e) => this.displayTaskDetails(-1);

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvTasks.SelectedItems.Count <= 0 || this.gvTasks.SelectedItems.Count > 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a task to edit.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
      }
      else
        this.displayTaskDetails(this.gvTasks.SelectedItems[0].Index);
    }

    private void btnDelete_Click(object sender, EventArgs e) => this.deleteSelectedTasks();

    private void btnExport_Click(object sender, EventArgs e) => this.exportTasks();

    private void exportTasks()
    {
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddHeaderColumn("Status");
      excelHandler.AddHeaderColumn("Subject");
      excelHandler.AddHeaderColumn("Due Date", "m/d/yyyy");
      excelHandler.AddHeaderColumn("Creation Date", "m/d/yyyy");
      excelHandler.AddHeaderColumn("Priority");
      excelHandler.AddHeaderColumn("Comments");
      excelHandler.AddHeaderColumn("Campaign");
      excelHandler.AddHeaderColumn("Contacts", "0");
      foreach (GVItem selectedItem in this.gvTasks.SelectedItems)
        excelHandler.AddDataRow(this.getTaskInfo((TaskInfo) selectedItem.Tag));
      excelHandler.CreateExcel();
    }

    private string[] getTaskInfo(TaskInfo task)
    {
      List<string> stringList = new List<string>();
      if (task.TaskStatus > TaskStatus.Invalid && (TaskStatus) TaskInfoUtils.TaskStatusStrings.Length > task.TaskStatus)
        stringList.Add(TaskInfoUtils.TaskStatusStrings[(int) task.TaskStatus]);
      else
        stringList.Add("");
      stringList.Add(task.Subject);
      stringList.Add(task.DueDate.ToString("MM/dd/yyyy"));
      stringList.Add(task.CreationTime.ToString("MM/dd/yyyy"));
      if (task.Priority > TaskPriority.Invalid && (TaskPriority) TaskInfoUtils.TaskPriorityStrings.Length > task.Priority)
        stringList.Add(TaskInfoUtils.TaskPriorityStrings[(int) task.Priority]);
      else
        stringList.Add("");
      stringList.Add(task.Notes);
      stringList.Add(task.CampaignInfo);
      string str = "";
      if (task.Contacts != null)
      {
        foreach (ContactInfo contact in task.Contacts)
          str = !(str == "") ? str + ", " + contact.ContactName : contact.ContactName;
      }
      stringList.Add(str);
      return stringList.ToArray();
    }

    private void contextMenu1_Popup(object sender, EventArgs e)
    {
      if (this.gvTasks.SelectedItems.Count == 0)
      {
        this.menuItemOpen.Enabled = false;
        this.menuItemDelete.Enabled = false;
        this.statusToolStripMenuItem.Enabled = false;
      }
      else
      {
        if (this.gvTasks.SelectedItems.Count == 1)
          this.menuItemOpen.Enabled = true;
        else
          this.menuItemOpen.Enabled = false;
        this.menuItemDelete.Enabled = true;
        this.statusToolStripMenuItem.Enabled = false;
      }
    }

    private void menuItemCompleted_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.gvTasks.SelectedItems.Count; ++index)
        this.changeStatus(this.gvTasks.SelectedItems[index].Index, true);
    }

    private void menuItemNotCompleted_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.gvTasks.SelectedItems.Count; ++index)
        this.changeStatus(this.gvTasks.SelectedItems[index].Index, false);
    }

    private void menuItemNew_Click(object sender, EventArgs e) => this.displayTaskDetails(-1);

    private void menuItemOpen_Click(object sender, EventArgs e)
    {
      this.displayTaskDetails(this.gvTasks.SelectedItems[0].Index);
    }

    private void gvTasks_DoubleClick(object sender, EventArgs e)
    {
      if (this.gvTasks.SelectedItems == null || this.gvTasks.SelectedItems.Count == 0)
        return;
      this.displayTaskDetails(this.gvTasks.SelectedItems[0].Index);
    }

    private void frmTaskDetails_TaskUpdatedEvent(object sender, EventArgs e) => this.LoadTasks();

    private void menuItemDelete_Click(object sender, EventArgs e) => this.deleteSelectedTasks();

    private void changeStatus(int idx, bool completed)
    {
      TaskInfo tag = (TaskInfo) this.gvTasks.Items[idx].Tag;
      bool flag = false;
      if (completed)
      {
        if (tag.TaskStatus != TaskStatus.Completed)
        {
          flag = true;
          tag.TaskStatus = TaskStatus.Completed;
        }
      }
      else if (tag.TaskStatus == TaskStatus.Completed)
      {
        flag = true;
        tag.TaskStatus = tag.PrevNonCompletedStatus != TaskStatus.Invalid ? tag.PrevNonCompletedStatus : TaskStatus.NotStarted;
      }
      if (!flag)
        return;
      Session.ContactManager.InsertOrUpdateTask(tag);
      this.gvTasks.Items[idx].SubItems[0].Text = TaskInfoUtils.TaskStatusStrings[(int) tag.TaskStatus];
    }

    private void changeStatus(int idx, TaskStatus newStatus)
    {
      TaskInfo tag = (TaskInfo) this.gvTasks.Items[idx].Tag;
      bool flag = true;
      tag.TaskStatus = newStatus;
      if (!flag)
        return;
      Session.ContactManager.InsertOrUpdateTask(tag);
      this.gvTasks.Items[idx].SubItems[0].Text = TaskInfoUtils.TaskStatusStrings[(int) tag.TaskStatus];
    }

    private void deleteSelectedTasks()
    {
      if (this.gvTasks.SelectedItems.Count <= 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more tasks to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the " + (object) this.gvTasks.SelectedItems.Count + " selected task(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        int[] taskIDs = new int[this.gvTasks.SelectedItems.Count];
        for (int index = 0; index < taskIDs.Length; ++index)
        {
          TaskInfo tag = (TaskInfo) this.gvTasks.SelectedItems[index].Tag;
          taskIDs[index] = tag.TaskID;
        }
        Session.ContactManager.DeleteTasks(taskIDs);
        this.LoadTasks();
      }
    }

    private void gvTasks_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.disableMenuOption();
      this.btnEdit.Enabled = this.gvTasks.SelectedItems.Count == 1;
      this.menuItemOpen.Enabled = this.gvTasks.SelectedItems.Count == 1;
      this.btnDelete.Enabled = this.gvTasks.SelectedItems.Count > 0;
      this.statusToolStripMenuItem.Enabled = this.gvTasks.SelectedItems.Count > 0;
      this.menuItemDelete.Enabled = this.gvTasks.SelectedItems.Count > 0;
      this.btnExport.Enabled = this.gvTasks.SelectedItems.Count > 0;
    }

    private void disableMenuOption()
    {
      this.menuItemOpen.Enabled = false;
      this.menuItemDelete.Enabled = false;
      this.statusToolStripMenuItem.Enabled = false;
    }

    private void StatusToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.gvTasks.SelectedItems.Count == 0)
        return;
      TaskStatus newStatus = TaskStatus.Completed;
      if (sender == this.notStartedToolStripMenuItem)
        newStatus = TaskStatus.NotStarted;
      else if (sender == this.inProgressToolStripMenuItem)
        newStatus = TaskStatus.InProgress;
      else if (sender == this.completedToolStripMenuItem)
        newStatus = TaskStatus.Completed;
      else if (sender == this.waitingOnSomeoneElseToolStripMenuItem)
        newStatus = TaskStatus.WaitingOnSomeoneElse;
      else if (sender == this.deferredToolStripMenuItem)
        newStatus = TaskStatus.Deferred;
      foreach (GVItem selectedItem in this.gvTasks.SelectedItems)
        this.changeStatus(selectedItem.Index, newStatus);
    }
  }
}

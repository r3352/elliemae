// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.MilestoneTaskListControl
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class MilestoneTaskListControl : UserControl, IRefreshContents, IOnlineHelpTarget
  {
    private const string className = "MilestoneTaskListControl";
    private static readonly string sw = Tracing.SwDataEngine;
    private LoanDataMgr loanMgr;
    private LoanData loan;
    private LogList logList;
    private bool editable = true;
    private bool canAdd = true;
    private bool canEdit = true;
    private bool canDelete = true;
    private bool printable = true;
    private bool inPopupWindow;
    private IContainer components;
    private ImageList ilIcons;
    private StandardIconButton btnAdd;
    private StandardIconButton btnDelete;
    private StandardIconButton btnEdit;
    private GridView gridViewTasks;
    private GroupContainer groupContainer1;
    private StandardIconButton btnPrint;
    private StandardIconButton btnExport;
    private ToolTip toolTip1;

    public MilestoneTaskListControl(LoanDataMgr loanMgr, bool inPopupWindow)
    {
      this.loanMgr = loanMgr;
      this.loan = loanMgr.LoanData;
      this.inPopupWindow = inPopupWindow;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.RefreshContents();
      try
      {
        UserInfo userInfo = Session.UserInfo;
        FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
        bool flag = true;
        if (UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas))
          flag = false;
        if (flag && !aclManager.GetUserApplicationRight(AclFeature.ToolsTab_Task) || (this.loan.ContentAccess & LoanContentAccess.Task) != LoanContentAccess.Task && this.loan.ContentAccess != LoanContentAccess.FullAccess || !aclManager.GetUserApplicationRight(AclFeature.ToolsTab_Task_Add) && !aclManager.GetUserApplicationRight(AclFeature.ToolsTab_Task_Edit) && !aclManager.GetUserApplicationRight(AclFeature.ToolsTab_Task_Delete))
          this.editable = false;
        this.canAdd = this.canEdit = this.canDelete = true;
        if (flag)
        {
          if (!aclManager.GetUserApplicationRight(AclFeature.ToolsTab_Task_Add))
            this.canAdd = false;
          if (!aclManager.GetUserApplicationRight(AclFeature.ToolsTab_Task_Edit))
            this.canEdit = false;
          if (!aclManager.GetUserApplicationRight(AclFeature.ToolsTab_Task_Delete))
            this.canDelete = false;
        }
        this.printable = true;
        if (flag)
        {
          this.printable = false;
          string[] usersStdPrintForms = Session.AclGroupManager.GetUsersStdPrintForms();
          if (usersStdPrintForms != null)
          {
            foreach (string str in usersStdPrintForms)
            {
              if (str == "Task Detail")
              {
                this.printable = aclManager.GetUserApplicationRight(AclFeature.LoansTab_Print_PrintButton) && aclManager.GetUserApplicationRight(AclFeature.LoansTab_Print_StandardForms);
                break;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(MilestoneTaskListControl.sw, TraceLevel.Error, nameof (MilestoneTaskListControl), "Cannot check access rights: Error: " + ex.Message);
      }
      this.gridViewTasks_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    public void RefreshContents(MilestoneTaskLog taskLog)
    {
      for (int nItemIndex1 = 0; nItemIndex1 < this.gridViewTasks.Items.Count; ++nItemIndex1)
      {
        if (((LogRecordBase) this.gridViewTasks.Items[nItemIndex1].Tag).Guid == taskLog.Guid)
        {
          GVItem gvItem = this.buildListViewItem(taskLog, false);
          for (int nItemIndex2 = 0; nItemIndex2 < this.gridViewTasks.Columns.Count; ++nItemIndex2)
          {
            if (nItemIndex2 == 0)
              this.gridViewTasks.Items[nItemIndex1].Text = gvItem.Text;
            else
              this.gridViewTasks.Items[nItemIndex1].SubItems[nItemIndex2].Text = gvItem.SubItems[nItemIndex2].Text;
          }
          this.gridViewTasks.Items[nItemIndex1].Tag = (object) taskLog;
          this.gridViewTasks.Items[nItemIndex1].Selected = true;
          break;
        }
      }
    }

    public void RefreshContents()
    {
      MilestoneTaskLog milestoneTaskLog = (MilestoneTaskLog) null;
      if (this.gridViewTasks.SelectedItems.Count > 0)
        milestoneTaskLog = (MilestoneTaskLog) this.gridViewTasks.SelectedItems[0].Tag;
      this.gridViewTasks.Items.Clear();
      this.logList = this.loan.GetLogList();
      if (this.logList == null)
        return;
      MilestoneTaskLog[] milestoneTaskLogs = this.logList.GetAllMilestoneTaskLogs((string) null);
      if (milestoneTaskLogs == null || milestoneTaskLogs.Length == 0)
        return;
      this.gridViewTasks.BeginUpdate();
      for (int index = 0; index < milestoneTaskLogs.Length; ++index)
        this.gridViewTasks.Items.Add(this.buildListViewItem(milestoneTaskLogs[index], milestoneTaskLog != null && milestoneTaskLog.TaskGUID == milestoneTaskLogs[index].TaskGUID));
      this.gridViewTasks.EndUpdate();
    }

    public void RefreshLoanContents() => this.RefreshContents();

    private GVItem buildListViewItem(MilestoneTaskLog task, bool selected)
    {
      GVItem gvItem = new GVItem(task.TaskName);
      gvItem.SubItems.Add((object) task.Stage);
      if (task.ContactCount > 0)
        gvItem.SubItems.Add((object) task.GetFirstContactName());
      else
        gvItem.SubItems.Add((object) "");
      if (task.CompletedDate != DateTime.MinValue)
      {
        gvItem.SubItems.Add((object) "Completed");
        gvItem.SubItems.Add((object) task.CompletedDate.ToString("MM/dd/yyyy"));
        gvItem.SubItems.Add((object) task.CompletedByFullName);
      }
      else if (task.ExpectedDate != DateTime.MinValue)
      {
        gvItem.SubItems.Add((object) "Expected");
        gvItem.SubItems.Add((object) task.ExpectedDate.ToString("MM/dd/yyyy"));
        gvItem.SubItems.Add((object) task.AddedByFullName);
      }
      else
      {
        gvItem.SubItems.Add((object) "Added");
        gvItem.SubItems.Add((object) task.Date.ToString("MM/dd/yyyy"));
        gvItem.SubItems.Add((object) task.AddedByFullName);
      }
      string str = "No";
      if (task.AlertList != null)
      {
        for (int index = 0; index < task.AlertList.Count; ++index)
        {
          if (task.AlertList[index].RoleId != 0)
          {
            str = "Yes";
            break;
          }
        }
      }
      gvItem.SubItems.Add((object) str);
      gvItem.SubItems.Add((object) task.TaskPriority);
      gvItem.Tag = (object) task;
      gvItem.Selected = selected;
      return gvItem;
    }

    private void iconBtDelete_Click(object sender, EventArgs e)
    {
      if (this.gridViewTasks.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a task first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected task(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        int index1 = this.gridViewTasks.SelectedItems[0].Index;
        List<GVItem> gvItemList = new List<GVItem>();
        for (int index2 = 0; index2 < this.gridViewTasks.SelectedItems.Count; ++index2)
          gvItemList.Add(this.gridViewTasks.SelectedItems[index2]);
        string str = string.Empty;
        for (int index3 = 0; index3 < gvItemList.Count; ++index3)
        {
          MilestoneTaskLog tag = (MilestoneTaskLog) gvItemList[index3].Tag;
          if (tag.IsRequired)
          {
            str = str + "\r\n" + tag.TaskName;
          }
          else
          {
            this.logList.RemoveRecord((LogRecordBase) tag);
            this.gridViewTasks.Items.Remove(gvItemList[index3]);
          }
        }
        Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        if (str != string.Empty)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The following milestone required task(s) can't be deleted:" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        if (this.gridViewTasks.Items.Count == 0)
          return;
        if (index1 + 1 >= this.gridViewTasks.Items.Count)
          this.gridViewTasks.Items[this.gridViewTasks.Items.Count - 1].Selected = true;
        else
          this.gridViewTasks.Items[index1].Selected = true;
      }
    }

    private void iconBtEdit_Click(object sender, EventArgs e)
    {
      if (this.gridViewTasks.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a task first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        MilestoneTaskLog tag = (MilestoneTaskLog) this.gridViewTasks.SelectedItems[0].Tag;
        using (MilestoneTaskWorksheetContainer worksheetContainer = new MilestoneTaskWorksheetContainer(tag, this.editable))
        {
          if (worksheetContainer.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.gridViewTasks.SelectedItems[0].Text = tag.TaskName;
          this.gridViewTasks.SelectedItems[0].SubItems[1].Text = tag.Stage;
          this.gridViewTasks.SelectedItems[0].SubItems[2].Text = tag.GetFirstContactName();
          if (tag.CompletedDate != DateTime.MinValue)
          {
            this.gridViewTasks.SelectedItems[0].SubItems[3].Text = "Completed";
            this.gridViewTasks.SelectedItems[0].SubItems[4].Text = tag.CompletedDate.ToString("MM/dd/yyyy");
            this.gridViewTasks.SelectedItems[0].SubItems[5].Text = tag.CompletedByFullName;
          }
          else if (tag.ExpectedDate != DateTime.MinValue)
          {
            this.gridViewTasks.SelectedItems[0].SubItems[3].Text = "Expected";
            this.gridViewTasks.SelectedItems[0].SubItems[4].Text = tag.ExpectedDate.ToString("MM/dd/yyyy");
            this.gridViewTasks.SelectedItems[0].SubItems[5].Text = tag.AddedByFullName;
          }
          else
          {
            this.gridViewTasks.SelectedItems[0].SubItems[3].Text = "Added";
            this.gridViewTasks.SelectedItems[0].SubItems[4].Text = tag.Date.ToString("MM/dd/yyyy");
            this.gridViewTasks.SelectedItems[0].SubItems[5].Text = tag.AddedByFullName;
          }
          this.gridViewTasks.SelectedItems[0].SubItems[6].Text = "No";
          if (tag.AlertList != null && tag.AlertList.Count > 0)
          {
            for (int index = 0; index < tag.AlertList.Count; ++index)
            {
              if (tag.AlertList[index].RoleId != 0)
              {
                this.gridViewTasks.SelectedItems[0].SubItems[6].Text = "Yes";
                break;
              }
            }
          }
          this.gridViewTasks.SelectedItems[0].SubItems[7].Text = tag.TaskPriority;
          Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        }
      }
    }

    public void AddNewTask()
    {
      if (!this.editable)
        return;
      this.iconBtAdd_Click((object) null, (EventArgs) null);
    }

    private void iconBtAdd_Click(object sender, EventArgs e)
    {
      this.gridViewTasks.SelectedItems.Clear();
      MilestoneTaskLog milestoneTaskLog = new MilestoneTaskLog(Session.UserInfo, "", "");
      milestoneTaskLog.IsRequired = false;
      milestoneTaskLog.TaskPriority = MilestoneTaskLog.TASKPRIORITYNORMAL;
      milestoneTaskLog.Stage = Session.LoanData.GetLogList().NextStage;
      using (MilestoneTaskWorksheetContainer worksheetContainer = new MilestoneTaskWorksheetContainer(milestoneTaskLog, this.editable))
      {
        if (worksheetContainer.ShowDialog((IWin32Window) this) != DialogResult.OK || !this.inPopupWindow)
          return;
        this.gridViewTasks.Items.Add(this.buildListViewItem(milestoneTaskLog, true));
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      if (this.gridViewTasks.Items.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You don't have any task to print.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        MilestoneTaskLog[] milestoneTaskLogArray = new MilestoneTaskLog[this.gridViewTasks.SelectedItems.Count];
        for (int index = 0; index < this.gridViewTasks.SelectedItems.Count; ++index)
          milestoneTaskLogArray[index] = (MilestoneTaskLog) this.gridViewTasks.SelectedItems[index].Tag;
        List<FormItemInfo> formItemInfoList = new List<FormItemInfo>();
        MilestoneTaskLog[] milestoneTaskLogs = this.loan.GetLogList().GetAllMilestoneTaskLogs((string) null);
        foreach (MilestoneTaskLog milestoneTaskLog in milestoneTaskLogArray)
        {
          int num2 = Array.IndexOf<MilestoneTaskLog>(milestoneTaskLogs, milestoneTaskLog);
          if (num2 >= 0)
            formItemInfoList.Add(new FormItemInfo()
            {
              FormType = OutputFormType.Task,
              BlockNum = num2,
              FormName = milestoneTaskLog.TaskName
            });
        }
        try
        {
          new PdfFormFacade(this.loanMgr, Form.ActiveForm).ProcessForms(formItemInfoList.ToArray(), PdfFormPrintOptions.WithData, "Preview");
        }
        catch (Exception ex)
        {
          MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\MilestoneTasks\\MilestoneTaskListControl.cs", nameof (btnPrint_Click), 387);
          int num3 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to generate the Task print form:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        finally
        {
          Cursor.Current = Cursors.Default;
        }
      }
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        foreach (GVColumn column in this.gridViewTasks.Columns.DisplaySequence)
          excelHandler.AddHeaderColumn(column);
        foreach (GVItem selectedItem in this.gridViewTasks.SelectedItems)
        {
          string[] data = new string[this.gridViewTasks.Columns.Count];
          for (int index1 = 0; index1 < data.Length; ++index1)
          {
            int index2 = this.gridViewTasks.Columns.DisplaySequence[index1].Index;
            if (index2 == 2)
            {
              MilestoneTaskLog tag = (MilestoneTaskLog) selectedItem.Tag;
              data[index1] = tag.GetAllContactNames();
            }
            else
              data[index1] = selectedItem.SubItems[index2].Text;
          }
          excelHandler.AddDataRow(data);
        }
        excelHandler.CreateExcel();
      }
    }

    public string GetHelpTargetName() => "Milestone Tasks";

    private void gridViewTasks_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = this.editable && this.canAdd;
      this.btnEdit.Enabled = this.gridViewTasks.SelectedItems.Count == 1 && this.editable && this.canEdit;
      this.btnDelete.Enabled = this.gridViewTasks.SelectedItems.Count > 0 && this.editable && this.canDelete;
      this.btnPrint.Enabled = this.gridViewTasks.SelectedItems.Count > 0 && this.printable;
      this.btnExport.Enabled = this.gridViewTasks.SelectedItems.Count > 0;
    }

    private void gridViewTasks_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!this.editable || !this.canEdit)
        return;
      this.iconBtEdit_Click((object) null, (EventArgs) null);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MilestoneTaskListControl));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      this.ilIcons = new ImageList(this.components);
      this.btnAdd = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.gridViewTasks = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.btnPrint = new StandardIconButton();
      this.btnExport = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.btnAdd).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnExport).BeginInit();
      this.SuspendLayout();
      this.ilIcons.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilIcons.ImageStream");
      this.ilIcons.TransparentColor = Color.Transparent;
      this.ilIcons.Images.SetKeyName(0, "");
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(684, 5);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 5;
      this.btnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAdd, "Add task");
      this.btnAdd.Click += new EventHandler(this.iconBtAdd_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(706, 5);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 6;
      this.btnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEdit, "Edit task");
      this.btnEdit.Click += new EventHandler(this.iconBtEdit_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(728, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 4;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete task");
      this.btnDelete.Click += new EventHandler(this.iconBtDelete_Click);
      this.gridViewTasks.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 133;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SortMethod = GVSortMethod.Custom;
      gvColumn2.Text = "For Milestone";
      gvColumn2.Width = 110;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Contacts";
      gvColumn3.Width = (int) sbyte.MaxValue;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Current Status";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.Date;
      gvColumn5.Text = "Date";
      gvColumn5.Width = 70;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "By";
      gvColumn6.Width = 120;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "Follow Up Needed";
      gvColumn7.Width = 120;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Text = "Priority";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 60;
      this.gridViewTasks.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gridViewTasks.Dock = DockStyle.Fill;
      this.gridViewTasks.Location = new Point(1, 26);
      this.gridViewTasks.Name = "gridViewTasks";
      this.gridViewTasks.Size = new Size(793, 355);
      this.gridViewTasks.TabIndex = 13;
      this.gridViewTasks.SelectedIndexChanged += new EventHandler(this.gridViewTasks_SelectedIndexChanged);
      this.gridViewTasks.ItemDoubleClick += new GVItemEventHandler(this.gridViewTasks_ItemDoubleClick);
      this.groupContainer1.Controls.Add((Control) this.btnPrint);
      this.groupContainer1.Controls.Add((Control) this.btnExport);
      this.groupContainer1.Controls.Add((Control) this.btnAdd);
      this.groupContainer1.Controls.Add((Control) this.gridViewTasks);
      this.groupContainer1.Controls.Add((Control) this.btnEdit);
      this.groupContainer1.Controls.Add((Control) this.btnDelete);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(795, 382);
      this.groupContainer1.TabIndex = 14;
      this.groupContainer1.Text = "Tasks";
      this.btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(772, 5);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(16, 16);
      this.btnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.btnPrint.TabIndex = 15;
      this.btnPrint.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnPrint, "Print Task Detail");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Location = new Point(750, 5);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(16, 16);
      this.btnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExport.TabIndex = 14;
      this.btnExport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnExport, "Export tasks");
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (MilestoneTaskListControl);
      this.Size = new Size(795, 382);
      ((ISupportInitialize) this.btnAdd).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnExport).EndInit();
      this.ResumeLayout(false);
    }
  }
}

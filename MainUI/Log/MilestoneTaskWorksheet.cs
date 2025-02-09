// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.MilestoneTaskWorksheet
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class MilestoneTaskWorksheet : UserControl
  {
    private MilestoneTaskLog taskLog;
    private string taskGUID = string.Empty;
    private bool editable = true;
    private bool completedByCurrentUser;
    private IContainer components;
    private GradientPanel gradientPanelTop;
    private Panel panelDetail;
    private ComboBox cboPriority;
    private Label label4;
    private Label label3;
    private TextBox txtDescription;
    private Label label2;
    private TextBox txtName;
    private Label label1;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer3;
    private GroupContainer groupContainer2;
    private TextBox txtCompleteBy;
    private TextBox txtAddBy;
    private CheckBox chkComplete;
    private CheckBox chkAdd;
    private GroupContainer groupContainer4;
    private Button btnDateStamp;
    private TextBox txtComments;
    private Label labelTaskName;
    private Label labelTaskStatus;
    private StandardIconButton btnAdd;
    private StandardIconButton btnDelete;
    private TextBox txtAddDate;
    private TextBox txtCompleteDate;
    private StandardIconButton btnFind;
    private LogAlertEditControl ctlAlert2;
    private LogAlertEditControl ctlAlert1;
    private LogAlertEditControl ctlAlert3;
    private StandardIconButton btnEdit;
    private GridView gridViewContacts;
    private ToolTip toolTip1;
    private ComboBoxEx cboExMilestone;
    private TextBox txtExpectCompleteDate;
    private TextBox txtDays;
    private Label label5;
    private DateTimePicker datePickerComplete;

    public MilestoneTaskWorksheet(MilestoneTaskLog taskLog, bool hideTitle, bool editable)
    {
      this.taskLog = taskLog;
      this.editable = editable;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      TextBoxFormatter.Attach(this.txtDays, TextBoxContentRule.NonNegativeInteger, "##0");
      this.txtCompleteDate.Top = this.datePickerComplete.Top;
      this.txtCompleteDate.Left = this.datePickerComplete.Left;
      if (hideTitle)
        this.gradientPanelTop.Visible = false;
      this.initForm();
      if (!this.editable)
      {
        this.btnDateStamp.Enabled = false;
        this.btnDelete.Enabled = false;
        this.btnFind.Enabled = false;
        this.btnAdd.Enabled = false;
        this.txtName.ReadOnly = true;
        this.txtDescription.ReadOnly = true;
        this.cboExMilestone.Enabled = false;
        this.cboPriority.Enabled = false;
        this.groupContainer1.Enabled = false;
        this.groupContainer2.Enabled = false;
        this.groupContainer3.Enabled = false;
        this.groupContainer4.Enabled = false;
      }
      else if (taskLog.IsRequired)
        this.cboExMilestone.Enabled = false;
      this.gridViewContacts_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void initForm()
    {
      RoleInfo[] allRoles = Session.LoanDataMgr.SystemConfiguration.AllRoles;
      this.ctlAlert1.Initialize(allRoles);
      this.ctlAlert2.Initialize(allRoles);
      this.ctlAlert3.Initialize(allRoles);
      LogList logList = Session.LoanData.GetLogList();
      this.initMilestoneField();
      this.completedByCurrentUser = false;
      this.taskGUID = this.taskLog.TaskGUID;
      this.txtName.Text = this.taskLog.TaskName;
      this.txtDescription.Text = this.taskLog.TaskDescription;
      this.txtComments.Text = this.taskLog.Comments;
      this.chkAdd.Checked = this.taskLog.AddedToLog;
      this.txtAddBy.Text = this.taskLog.AddedToLog ? this.taskLog.AddedByFullName : "";
      TextBox txtAddDate = this.txtAddDate;
      DateTime dateTime;
      string str1;
      if (!this.taskLog.AddedToLog)
      {
        str1 = "";
      }
      else
      {
        dateTime = this.taskLog.AddDate;
        str1 = dateTime.ToString("MM/dd/yyyy hh:mm tt");
      }
      txtAddDate.Text = str1;
      this.cboPriority.Text = this.taskLog.TaskPriority;
      this.txtDays.Text = this.taskLog.AddedToLog ? (this.taskLog.DaysToComplete <= 0 ? "" : string.Concat((object) this.taskLog.DaysToComplete)) : "";
      if (this.taskLog.DaysToComplete <= 0 || !this.taskLog.AddedToLog)
      {
        this.txtExpectCompleteDate.Text = string.Empty;
      }
      else
      {
        TextBox expectCompleteDate = this.txtExpectCompleteDate;
        string str2;
        if (!(this.taskLog.ExpectedDate == DateTime.MinValue))
        {
          dateTime = this.taskLog.ExpectedDate;
          str2 = dateTime.ToString("MM/dd/yyyy hh:mm tt");
        }
        else
          str2 = "";
        expectCompleteDate.Text = str2;
      }
      Hashtable hashtable = (Hashtable) null;
      if (this.taskLog.ContactCount > 0)
      {
        List<int> intList = new List<int>();
        for (int i = 0; i < this.taskLog.ContactCount; ++i)
        {
          MilestoneTaskLog.TaskContact taskContactAt = this.taskLog.GetTaskContactAt(i);
          if (taskContactAt.ContactRole != "Borrower" && taskContactAt.ContactID != -1)
            intList.Add(Utils.ParseInt((object) taskContactAt.ContactID));
        }
        if (intList.Count > 0)
        {
          BizPartnerInfo[] bizPartners = Session.ContactManager.GetBizPartners(intList.ToArray());
          if (bizPartners != null && bizPartners.Length != 0)
          {
            hashtable = new Hashtable();
            for (int index = 0; index < bizPartners.Length; ++index)
            {
              if (bizPartners[index] != null && !hashtable.ContainsKey((object) bizPartners[index].ContactID))
                hashtable.Add((object) bizPartners[index].ContactID, (object) bizPartners[index]);
            }
          }
        }
      }
      this.gridViewContacts.BeginUpdate();
      for (int i = 0; i < this.taskLog.ContactCount; ++i)
      {
        MilestoneTaskLog.TaskContact taskContactAt = this.taskLog.GetTaskContactAt(i);
        if (taskContactAt != null)
        {
          if (taskContactAt.ContactID != -1 && hashtable != null && hashtable.ContainsKey((object) taskContactAt.ContactID))
          {
            BizPartnerInfo bizPartnerInfo = (BizPartnerInfo) hashtable[(object) taskContactAt.ContactID];
            if (bizPartnerInfo.FullName.Trim() != string.Empty)
              taskContactAt.ContactName = bizPartnerInfo.FullName;
            if (bizPartnerInfo.WorkPhone.Trim() != string.Empty)
              taskContactAt.ContactPhone = bizPartnerInfo.WorkPhone;
            if (bizPartnerInfo.BizEmail.Trim() != string.Empty)
              taskContactAt.ContactEmail = bizPartnerInfo.BizEmail;
            string str3 = bizPartnerInfo.BizAddress.Street1 + " " + bizPartnerInfo.BizAddress.Street2;
            if (str3.Trim() != string.Empty)
              taskContactAt.ContactAddress = str3;
            if (bizPartnerInfo.BizAddress.City.Trim() != string.Empty)
              taskContactAt.ContactCity = bizPartnerInfo.BizAddress.City.Trim();
            if (bizPartnerInfo.BizAddress.State.Trim() != string.Empty)
              taskContactAt.ContactState = bizPartnerInfo.BizAddress.State.Trim();
            if (bizPartnerInfo.BizAddress.Zip.Trim() != string.Empty)
              taskContactAt.ContactZip = bizPartnerInfo.BizAddress.Zip.Trim();
          }
          this.gridViewContacts.Items.Add(new GVItem(taskContactAt.ContactName)
          {
            SubItems = {
              (object) taskContactAt.ContactRole,
              (object) taskContactAt.ContactPhone,
              (object) taskContactAt.ContactEmail,
              (object) taskContactAt.ContactAddress,
              (object) taskContactAt.ContactCity,
              (object) taskContactAt.ContactState
            },
            Tag = (object) taskContactAt
          });
        }
      }
      this.gridViewContacts.EndUpdate();
      this.txtCompleteBy.Text = this.taskLog.CompletedByFullName;
      if (this.taskLog.CompletedDate != DateTime.MinValue)
      {
        this.chkComplete.Checked = true;
        this.datePickerComplete.Value = this.taskLog.CompletedDate;
      }
      else
        this.chkComplete.Checked = false;
      this.showHideCompletionFields();
      if (this.taskLog.Stage != string.Empty)
      {
        MilestoneLog milestone = logList.GetMilestone(this.taskLog.Stage);
        if (milestone == null && this.taskLog.Stage.ToLower() == "submittal")
          milestone = logList.GetMilestone(this.taskLog.Stage.ToLower());
        this.loadMilestoneField(milestone);
      }
      this.ctlAlert1.Populate((LogAlert) null);
      this.ctlAlert2.Populate((LogAlert) null);
      this.ctlAlert3.Populate((LogAlert) null);
      int num = 0;
      for (int index = 0; index < this.taskLog.AlertList.Count; ++index)
      {
        if (this.taskLog.AlertList[index].RoleId != 0 || !(this.taskLog.AlertList[index].FollowedUpDate == DateTime.MinValue))
        {
          ++num;
          switch (num)
          {
            case 1:
              this.ctlAlert1.Populate(this.taskLog.AlertList[index]);
              continue;
            case 2:
              this.ctlAlert2.Populate(this.taskLog.AlertList[index]);
              continue;
            case 3:
              this.ctlAlert3.Populate(this.taskLog.AlertList[index]);
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void initMilestoneField()
    {
      foreach (MilestoneLog allMilestone in Session.LoanData.GetLogList().GetAllMilestones())
      {
        if (string.Compare(allMilestone.Stage, "Started", true) != 0)
          this.cboExMilestone.Items.Add((object) new MilestoneLabel(((MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(allMilestone.MilestoneID, allMilestone.Stage, false, allMilestone.Days, allMilestone.DoneText, allMilestone.ExpText, allMilestone.RoleRequired == "Y", allMilestone.RoleID)));
      }
    }

    private void loadMilestoneField(MilestoneLog msLog)
    {
      this.cboExMilestone.SelectedItem = (object) null;
      string str = !(msLog.Stage == "submittal") ? msLog.Stage : "Submittal";
      foreach (MilestoneLabel milestoneLabel in this.cboExMilestone.Items)
      {
        if (milestoneLabel.MilestoneName == str)
          this.cboExMilestone.SelectedItem = (object) milestoneLabel;
      }
    }

    private string getCurrentMilestoneField()
    {
      MilestoneLabel milestoneLabel = (MilestoneLabel) this.cboExMilestone.Items[this.cboExMilestone.SelectedIndex];
      return milestoneLabel == null ? string.Empty : milestoneLabel.MilestoneName;
    }

    private void ctlAlert_LogAlertChanged(object sender, EventArgs e)
    {
      this.ctlAlert1.SetEditMode(EditMode.None);
      this.ctlAlert2.SetEditMode(EditMode.None);
      this.ctlAlert3.SetEditMode(EditMode.None);
    }

    private void iconBtAdd_Click(object sender, EventArgs e)
    {
      using (AddContactForm addContactForm = new AddContactForm((MilestoneTaskLog.TaskContact) null))
      {
        if (addContactForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.gridViewContacts.Items.Add(new GVItem(addContactForm.ContactName)
        {
          SubItems = {
            (object) addContactForm.ContactRole,
            (object) addContactForm.ContactPhone,
            (object) addContactForm.ContactEmail,
            (object) addContactForm.ContactAddress,
            (object) addContactForm.ContactCity,
            (object) addContactForm.ContactState
          },
          Tag = (object) new MilestoneTaskLog.TaskContact(addContactForm.ContactID, addContactForm.ContactName, addContactForm.ContactRole, addContactForm.ContactPhone, addContactForm.ContactEmail, addContactForm.ContactAddress, addContactForm.ContactCity, addContactForm.ContactState, addContactForm.ContactZip, "")
        });
      }
    }

    public bool ValidateMilestoneTask()
    {
      if (this.txtName.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a task name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtName.Focus();
        return false;
      }
      if (this.cboPriority.Text.Trim() == MilestoneTaskLog.TASKREQUIRED && this.getCurrentMilestoneField() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a milestone for required task.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cboExMilestone.Focus();
        return false;
      }
      this.taskLog.TaskGUID = this.taskGUID;
      this.taskLog.Date = Utils.ParseDate((object) this.txtAddDate.Text.Trim());
      this.taskLog.TaskName = this.txtName.Text.Trim();
      this.taskLog.TaskDescription = this.txtDescription.Text.Trim();
      this.taskLog.Comments = this.txtComments.Text;
      if (this.taskLog.AddedToLog || this.txtDays.Text.Trim() != string.Empty)
        this.taskLog.DaysToComplete = Utils.ParseInt((object) this.txtDays.Text.Trim());
      else
        this.txtDays.Text = this.taskLog.DaysToComplete >= 0 ? string.Concat((object) this.taskLog.DaysToComplete) : "";
      if (!this.txtCompleteDate.Visible)
      {
        this.taskLog.CompletedDate = this.datePickerComplete.Value;
        if (this.completedByCurrentUser)
          this.taskLog.SetCompletedByUser(Session.UserInfo);
      }
      else
        this.taskLog.UnmarkAsCompleted();
      this.taskLog.ClearContactList();
      for (int nItemIndex = 0; nItemIndex < this.gridViewContacts.Items.Count; ++nItemIndex)
        this.taskLog.AddContact((MilestoneTaskLog.TaskContact) this.gridViewContacts.Items[nItemIndex].Tag);
      this.taskLog.TaskPriority = this.cboPriority.Text.Trim();
      string currentMilestoneField = this.getCurrentMilestoneField();
      this.taskLog.Stage = !(currentMilestoneField != string.Empty) ? this.getCurrentMilestoneField() : currentMilestoneField;
      this.taskLog.AlertList.Clear();
      LogAlert alert1 = this.ctlAlert1.GetAlert();
      if (alert1 != null)
        this.taskLog.AlertList.Add(alert1);
      LogAlert alert2 = this.ctlAlert2.GetAlert();
      if (alert2 != null)
        this.taskLog.AlertList.Add(alert2);
      LogAlert alert3 = this.ctlAlert3.GetAlert();
      if (alert3 != null)
        this.taskLog.AlertList.Add(alert3);
      return true;
    }

    private void iconBtDelete_Click(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a contact first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete " + (this.gridViewContacts.SelectedItems.Count > 1 ? "these contacts" : "this contact") + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        int index = this.gridViewContacts.SelectedItems[0].Index;
        for (int nItemIndex = this.gridViewContacts.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
        {
          if (this.gridViewContacts.Items[nItemIndex].Selected)
            this.gridViewContacts.Items.RemoveAt(nItemIndex);
        }
        if (this.gridViewContacts.Items.Count == 0 || index >= this.gridViewContacts.Items.Count)
          return;
        this.gridViewContacts.Items[index].Selected = true;
      }
    }

    private void btnDateStamp_Click(object sender, EventArgs e)
    {
      this.txtComments.TextChanged -= new EventHandler(this.txtComments_TextChanged);
      if (this.taskLog.Comments != string.Empty && !this.taskLog.Comments.EndsWith("\r\n"))
        this.taskLog.Comments += "\r\n";
      MilestoneTaskLog taskLog = this.taskLog;
      taskLog.Comments = taskLog.Comments + DateTime.Now.ToString("MM/dd/yy h:mm tt ") + "(" + Utils.CurrentTimeZoneName + ") " + Session.UserInfo.FullName + " > ";
      this.txtComments.Text = this.taskLog.Comments;
      this.txtComments.TextChanged += new EventHandler(this.txtComments_TextChanged);
      this.txtComments.Focus();
    }

    private void txtComments_TextChanged(object sender, EventArgs e)
    {
      this.taskLog.Comments = this.txtComments.Text;
    }

    private void chkComplete_Click(object sender, EventArgs e)
    {
      if (!this.chkComplete.Checked)
      {
        this.txtCompleteBy.Text = string.Empty;
      }
      else
      {
        this.txtCompleteBy.Text = Session.UserInfo.FullName;
        if (this.datePickerComplete.Value == DateTime.MinValue)
          this.datePickerComplete.Value = DateTime.Now;
        this.completedByCurrentUser = true;
      }
      this.showHideCompletionFields();
    }

    private void showHideCompletionFields()
    {
      this.datePickerComplete.Visible = this.chkComplete.Checked;
      this.txtCompleteDate.Visible = !this.chkComplete.Checked;
    }

    private void btnFind_Click(object sender, EventArgs e)
    {
      using (SelectTaskForm selectTaskForm = new SelectTaskForm(true))
      {
        if (selectTaskForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.taskGUID = selectTaskForm.SelectedTask.TaskGUID;
        this.txtName.Text = selectTaskForm.SelectedTask.TaskName;
        this.txtDescription.Text = selectTaskForm.SelectedTask.TaskDescription;
        this.txtDays.Text = selectTaskForm.SelectedTask.DaysToComplete.ToString();
        this.cboPriority.SelectedIndex = (int) selectTaskForm.SelectedTask.TaskPriority;
        this.txtDays_TextChanged((object) null, (EventArgs) null);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a contact to edit.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        MilestoneTaskLog.TaskContact tag = (MilestoneTaskLog.TaskContact) this.gridViewContacts.SelectedItems[0].Tag;
        using (AddContactForm addContactForm = new AddContactForm(tag))
        {
          if (addContactForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.gridViewContacts.SelectedItems[0].Text = addContactForm.ContactName;
          this.gridViewContacts.SelectedItems[0].SubItems[1].Text = addContactForm.ContactRole;
          this.gridViewContacts.SelectedItems[0].SubItems[2].Text = addContactForm.ContactPhone;
          this.gridViewContacts.SelectedItems[0].SubItems[3].Text = addContactForm.ContactEmail;
          this.gridViewContacts.SelectedItems[0].SubItems[4].Text = addContactForm.ContactAddress;
          this.gridViewContacts.SelectedItems[0].SubItems[5].Text = addContactForm.ContactCity;
          this.gridViewContacts.SelectedItems[0].SubItems[6].Text = addContactForm.ContactState;
          this.gridViewContacts.SelectedItems[0].Tag = (object) new MilestoneTaskLog.TaskContact(addContactForm.ContactID, addContactForm.ContactName, addContactForm.ContactRole, addContactForm.ContactPhone, addContactForm.ContactEmail, addContactForm.ContactAddress, addContactForm.ContactCity, addContactForm.ContactState, addContactForm.ContactZip, tag.ContactGUID);
        }
      }
    }

    private void datePickerComplete_KeyUp(object sender, KeyEventArgs e)
    {
    }

    private void gridViewContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEdit.Enabled = this.gridViewContacts.SelectedItems.Count == 1;
      this.btnDelete.Enabled = this.gridViewContacts.SelectedItems.Count > 0;
    }

    private void txtDays_TextChanged(object sender, EventArgs e)
    {
      int num = Utils.ParseInt((object) this.txtDays.Text.Trim());
      if (num <= 0 || this.txtAddDate.Text.Trim() == string.Empty && this.taskLog.AddedToLog)
      {
        this.txtExpectCompleteDate.Text = string.Empty;
      }
      else
      {
        DateTime dateTime1 = this.taskLog.AddedToLog ? Utils.ParseDate((object) this.txtAddDate.Text) : DateTime.Now;
        dateTime1 = dateTime1.AddDays((double) num);
        DateTime dateTime2 = DateTime.Parse(dateTime1.ToString("MM/dd/yyyy") + " 12:00:00 pm");
        this.txtExpectCompleteDate.Text = dateTime2 == DateTime.MinValue ? "" : dateTime2.ToString("MM/dd/yyyy hh:mm tt");
      }
    }

    private void gridViewContacts_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEdit_Click((object) null, (EventArgs) null);
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
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.panelDetail = new Panel();
      this.cboExMilestone = new ComboBoxEx();
      this.btnFind = new StandardIconButton();
      this.groupContainer4 = new GroupContainer();
      this.btnDateStamp = new Button();
      this.txtComments = new TextBox();
      this.groupContainer3 = new GroupContainer();
      this.ctlAlert3 = new LogAlertEditControl();
      this.ctlAlert2 = new LogAlertEditControl();
      this.ctlAlert1 = new LogAlertEditControl();
      this.groupContainer2 = new GroupContainer();
      this.txtExpectCompleteDate = new TextBox();
      this.txtDays = new TextBox();
      this.label5 = new Label();
      this.txtCompleteDate = new TextBox();
      this.txtAddDate = new TextBox();
      this.txtCompleteBy = new TextBox();
      this.txtAddBy = new TextBox();
      this.chkComplete = new CheckBox();
      this.chkAdd = new CheckBox();
      this.cboPriority = new ComboBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.txtDescription = new TextBox();
      this.label2 = new Label();
      this.txtName = new TextBox();
      this.label1 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.gridViewContacts = new GridView();
      this.btnEdit = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.gradientPanelTop = new GradientPanel();
      this.labelTaskStatus = new Label();
      this.labelTaskName = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.datePickerComplete = new DateTimePicker();
      this.panelDetail.SuspendLayout();
      ((ISupportInitialize) this.btnFind).BeginInit();
      this.groupContainer4.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.gradientPanelTop.SuspendLayout();
      this.SuspendLayout();
      this.panelDetail.Controls.Add((Control) this.cboExMilestone);
      this.panelDetail.Controls.Add((Control) this.btnFind);
      this.panelDetail.Controls.Add((Control) this.groupContainer4);
      this.panelDetail.Controls.Add((Control) this.groupContainer3);
      this.panelDetail.Controls.Add((Control) this.groupContainer2);
      this.panelDetail.Controls.Add((Control) this.cboPriority);
      this.panelDetail.Controls.Add((Control) this.label4);
      this.panelDetail.Controls.Add((Control) this.label3);
      this.panelDetail.Controls.Add((Control) this.txtDescription);
      this.panelDetail.Controls.Add((Control) this.label2);
      this.panelDetail.Controls.Add((Control) this.txtName);
      this.panelDetail.Controls.Add((Control) this.label1);
      this.panelDetail.Controls.Add((Control) this.groupContainer1);
      this.panelDetail.Dock = DockStyle.Fill;
      this.panelDetail.Location = new Point(0, 27);
      this.panelDetail.Name = "panelDetail";
      this.panelDetail.Size = new Size(805, 592);
      this.panelDetail.TabIndex = 0;
      this.cboExMilestone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboExMilestone.FormattingEnabled = true;
      this.cboExMilestone.ItemHeight = 16;
      this.cboExMilestone.Location = new Point(90, 86);
      this.cboExMilestone.Name = "cboExMilestone";
      this.cboExMilestone.SelectedBGColor = SystemColors.Highlight;
      this.cboExMilestone.Size = new Size(309, 22);
      this.cboExMilestone.TabIndex = 24;
      this.btnFind.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFind.BackColor = Color.Transparent;
      this.btnFind.Location = new Point(770, 13);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = new Size(16, 16);
      this.btnFind.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFind.TabIndex = 23;
      this.btnFind.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnFind, "Find a predefined task");
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.groupContainer4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer4.Controls.Add((Control) this.btnDateStamp);
      this.groupContainer4.Controls.Add((Control) this.txtComments);
      this.groupContainer4.Location = new Point(447, 267);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(342, 322);
      this.groupContainer4.TabIndex = 15;
      this.groupContainer4.Text = "Comments";
      this.btnDateStamp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDateStamp.BackColor = SystemColors.Control;
      this.btnDateStamp.Location = new Point(263, 2);
      this.btnDateStamp.Name = "btnDateStamp";
      this.btnDateStamp.Size = new Size(75, 22);
      this.btnDateStamp.TabIndex = 16;
      this.btnDateStamp.Text = "Date Stamp";
      this.btnDateStamp.UseVisualStyleBackColor = true;
      this.btnDateStamp.Click += new EventHandler(this.btnDateStamp_Click);
      this.txtComments.BorderStyle = BorderStyle.None;
      this.txtComments.Dock = DockStyle.Fill;
      this.txtComments.Location = new Point(1, 26);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.ScrollBars = ScrollBars.Both;
      this.txtComments.Size = new Size(340, 295);
      this.txtComments.TabIndex = 17;
      this.txtComments.TextChanged += new EventHandler(this.txtComments_TextChanged);
      this.groupContainer3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupContainer3.Controls.Add((Control) this.ctlAlert3);
      this.groupContainer3.Controls.Add((Control) this.ctlAlert2);
      this.groupContainer3.Controls.Add((Control) this.ctlAlert1);
      this.groupContainer3.Location = new Point(18, 388);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(423, 200);
      this.groupContainer3.TabIndex = 11;
      this.groupContainer3.Text = "Follow Ups";
      this.ctlAlert3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlAlert3.DisplayTwoLines = true;
      this.ctlAlert3.Location = new Point(12, 144);
      this.ctlAlert3.Name = "ctlAlert3";
      this.ctlAlert3.Size = new Size(394, 49);
      this.ctlAlert3.TabIndex = 14;
      this.ctlAlert2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlAlert2.DisplayTwoLines = true;
      this.ctlAlert2.Location = new Point(12, 89);
      this.ctlAlert2.Name = "ctlAlert2";
      this.ctlAlert2.Size = new Size(394, 49);
      this.ctlAlert2.TabIndex = 13;
      this.ctlAlert1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlAlert1.DisplayTwoLines = true;
      this.ctlAlert1.Location = new Point(12, 34);
      this.ctlAlert1.Name = "ctlAlert1";
      this.ctlAlert1.Size = new Size(394, 49);
      this.ctlAlert1.TabIndex = 12;
      this.groupContainer2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupContainer2.Controls.Add((Control) this.datePickerComplete);
      this.groupContainer2.Controls.Add((Control) this.txtExpectCompleteDate);
      this.groupContainer2.Controls.Add((Control) this.txtDays);
      this.groupContainer2.Controls.Add((Control) this.label5);
      this.groupContainer2.Controls.Add((Control) this.txtCompleteDate);
      this.groupContainer2.Controls.Add((Control) this.txtAddDate);
      this.groupContainer2.Controls.Add((Control) this.txtCompleteBy);
      this.groupContainer2.Controls.Add((Control) this.txtAddBy);
      this.groupContainer2.Controls.Add((Control) this.chkComplete);
      this.groupContainer2.Controls.Add((Control) this.chkAdd);
      this.groupContainer2.Location = new Point(18, 267);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(423, 115);
      this.groupContainer2.TabIndex = 6;
      this.groupContainer2.Text = "Status";
      this.txtExpectCompleteDate.Location = new Point(192, 31);
      this.txtExpectCompleteDate.Name = "txtExpectCompleteDate";
      this.txtExpectCompleteDate.ReadOnly = true;
      this.txtExpectCompleteDate.Size = new Size(140, 20);
      this.txtExpectCompleteDate.TabIndex = 18;
      this.txtExpectCompleteDate.TabStop = false;
      this.txtDays.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDays.Location = new Point(105, 31);
      this.txtDays.MaxLength = 3;
      this.txtDays.Name = "txtDays";
      this.txtDays.Size = new Size(81, 20);
      this.txtDays.TabIndex = 7;
      this.txtDays.TextAlign = HorizontalAlignment.Right;
      this.txtDays.TextChanged += new EventHandler(this.txtDays_TextChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(13, 34);
      this.label5.Name = "label5";
      this.label5.Size = new Size(90, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "Days to Complete";
      this.txtCompleteDate.Location = new Point(113, 90);
      this.txtCompleteDate.Name = "txtCompleteDate";
      this.txtCompleteDate.ReadOnly = true;
      this.txtCompleteDate.Size = new Size(158, 20);
      this.txtCompleteDate.TabIndex = 14;
      this.txtCompleteDate.TabStop = false;
      this.txtAddDate.Location = new Point(105, 57);
      this.txtAddDate.Name = "txtAddDate";
      this.txtAddDate.ReadOnly = true;
      this.txtAddDate.Size = new Size(158, 20);
      this.txtAddDate.TabIndex = 13;
      this.txtAddDate.TabStop = false;
      this.txtCompleteBy.Location = new Point(274, 83);
      this.txtCompleteBy.Name = "txtCompleteBy";
      this.txtCompleteBy.ReadOnly = true;
      this.txtCompleteBy.Size = new Size(140, 20);
      this.txtCompleteBy.TabIndex = 12;
      this.txtCompleteBy.TabStop = false;
      this.txtAddBy.Location = new Point(274, 57);
      this.txtAddBy.Name = "txtAddBy";
      this.txtAddBy.ReadOnly = true;
      this.txtAddBy.Size = new Size(140, 20);
      this.txtAddBy.TabIndex = 9;
      this.txtAddBy.TabStop = false;
      this.chkComplete.AutoSize = true;
      this.chkComplete.Location = new Point(16, 87);
      this.chkComplete.Name = "chkComplete";
      this.chkComplete.Size = new Size(76, 17);
      this.chkComplete.TabIndex = 9;
      this.chkComplete.Text = "Completed";
      this.chkComplete.UseVisualStyleBackColor = true;
      this.chkComplete.Click += new EventHandler(this.chkComplete_Click);
      this.chkAdd.AutoSize = true;
      this.chkAdd.Enabled = false;
      this.chkAdd.Location = new Point(16, 60);
      this.chkAdd.Name = "chkAdd";
      this.chkAdd.Size = new Size(57, 17);
      this.chkAdd.TabIndex = 8;
      this.chkAdd.Text = "Added";
      this.chkAdd.UseVisualStyleBackColor = true;
      this.cboPriority.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboPriority.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPriority.FormattingEnabled = true;
      this.cboPriority.Items.AddRange(new object[3]
      {
        (object) "Low",
        (object) "Normal",
        (object) "High"
      });
      this.cboPriority.Location = new Point(599, 86);
      this.cboPriority.Name = "cboPriority";
      this.cboPriority.Size = new Size(190, 21);
      this.cboPriority.TabIndex = 4;
      this.label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(555, 89);
      this.label4.Name = "label4";
      this.label4.Size = new Size(38, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Priority";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(14, 89);
      this.label3.Name = "label3";
      this.label3.Size = new Size(70, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "For Milestone";
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(90, 35);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(699, 47);
      this.txtDescription.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(14, 40);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Description";
      this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtName.Location = new Point(90, 11);
      this.txtName.MaxLength = 256;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(674, 20);
      this.txtName.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(14, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(35, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Name";
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.gridViewContacts);
      this.groupContainer1.Controls.Add((Control) this.btnEdit);
      this.groupContainer1.Controls.Add((Control) this.btnAdd);
      this.groupContainer1.Controls.Add((Control) this.btnDelete);
      this.groupContainer1.Location = new Point(17, 113);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(772, 148);
      this.groupContainer1.TabIndex = 5;
      this.groupContainer1.Text = "Contacts";
      this.gridViewContacts.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 130;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Role";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Home Phone";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Personal Email";
      gvColumn4.Width = 120;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Address";
      gvColumn5.Width = 150;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "City";
      gvColumn6.Width = 110;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "State";
      gvColumn7.Width = 40;
      this.gridViewContacts.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gridViewContacts.Dock = DockStyle.Fill;
      this.gridViewContacts.Location = new Point(1, 26);
      this.gridViewContacts.Name = "gridViewContacts";
      this.gridViewContacts.Size = new Size(770, 121);
      this.gridViewContacts.TabIndex = 19;
      this.gridViewContacts.SelectedIndexChanged += new EventHandler(this.gridViewContacts_SelectedIndexChanged);
      this.gridViewContacts.ItemDoubleClick += new GVItemEventHandler(this.gridViewContacts_ItemDoubleClick);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(726, 4);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 18;
      this.btnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEdit, "Edit Contact");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(704, 4);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 17;
      this.btnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAdd, "Add Contact");
      this.btnAdd.Click += new EventHandler(this.iconBtAdd_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(748, 4);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 16;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Contact");
      this.btnDelete.Click += new EventHandler(this.iconBtDelete_Click);
      this.gradientPanelTop.Controls.Add((Control) this.labelTaskStatus);
      this.gradientPanelTop.Controls.Add((Control) this.labelTaskName);
      this.gradientPanelTop.Dock = DockStyle.Top;
      this.gradientPanelTop.Location = new Point(0, 0);
      this.gradientPanelTop.Name = "gradientPanelTop";
      this.gradientPanelTop.Size = new Size(805, 27);
      this.gradientPanelTop.TabIndex = 0;
      this.labelTaskStatus.AutoSize = true;
      this.labelTaskStatus.BackColor = Color.Transparent;
      this.labelTaskStatus.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelTaskStatus.Location = new Point(89, 8);
      this.labelTaskStatus.Name = "labelTaskStatus";
      this.labelTaskStatus.Size = new Size(43, 13);
      this.labelTaskStatus.TabIndex = 2;
      this.labelTaskStatus.Text = "(Status)";
      this.labelTaskName.AutoSize = true;
      this.labelTaskName.BackColor = Color.Transparent;
      this.labelTaskName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelTaskName.Location = new Point(4, 8);
      this.labelTaskName.Name = "labelTaskName";
      this.labelTaskName.Size = new Size(79, 13);
      this.labelTaskName.TabIndex = 1;
      this.labelTaskName.Text = "(Task Name)";
      this.datePickerComplete.CalendarMonthBackground = Color.WhiteSmoke;
      this.datePickerComplete.CalendarTitleBackColor = Color.SteelBlue;
      this.datePickerComplete.CustomFormat = "MM/dd/yyyy hh:mm tt";
      this.datePickerComplete.Format = DateTimePickerFormat.Custom;
      this.datePickerComplete.Location = new Point(105, 83);
      this.datePickerComplete.Name = "datePickerComplete";
      this.datePickerComplete.Size = new Size(158, 20);
      this.datePickerComplete.TabIndex = 19;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelDetail);
      this.Controls.Add((Control) this.gradientPanelTop);
      this.Name = nameof (MilestoneTaskWorksheet);
      this.Size = new Size(805, 619);
      this.panelDetail.ResumeLayout(false);
      this.panelDetail.PerformLayout();
      ((ISupportInitialize) this.btnFind).EndInit();
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.gradientPanelTop.ResumeLayout(false);
      this.gradientPanelTop.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

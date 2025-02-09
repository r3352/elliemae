// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.TaskList.TaskDetailsForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Calendar;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.TaskList;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.TaskList
{
  public class TaskDetailsForm : Form, IHelp
  {
    private const string className = "TaskDetailsForm";
    private static string sw = Tracing.SwContact;
    private TaskInfo _task;
    private UserInfo[] _userInfos;
    private TaskDetailsForm.FormBehavior _behavior;
    private Point _btnAddLocation;
    private Point _btnAddAllLocation;
    private Point _btnCancelLocation;
    private Point _btnCancelAllLocation;
    private bool _saved;
    private bool _processingBizPartners;
    private bool _addAllSelected;
    private GroupContainer grpContacts;
    private FlowLayoutPanel flowLayoutPanel3;
    private StandardIconButton btnDelete;
    private StandardIconButton btnContacts;
    private GridView gvContacts;
    private Label label6;
    private BorderPanel pnlComments;
    private ToolTip toolTip1;
    private Panel panel1;
    private Label lblCampaign;
    private Panel pnlCampaignInfo;
    private TextBox txtCampaign;
    private bool _cancelAllSelected;
    private IContainer components;
    private Panel pnlTaskDetails;
    private DatePicker dtPickerDueDate;
    private DatePicker dtPickerStartDate;
    private TextBox txtBoxSubject;
    private RichTextBox rtBoxNotes;
    private ComboBox cmbBoxPriority;
    private ComboBox cmbBoxStatus;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private Panel pnlButtons;
    private Button btnCancel;
    private Button btnAdd;
    private Panel pnlUserSelection;
    private ComboBox cboSelectedUser;
    private RadioButton rbSelectedUser;
    private RadioButton rbContactOwner;
    private RadioButton rbCurrentUser;
    private Label lblUserSelection;
    private Button btnCancelAll;
    private Button btnAddAll;

    public TaskDetailsForm.FormBehavior Behavior
    {
      get => this._behavior;
      set
      {
        this._behavior = value;
        this.btnContacts.Enabled = this.btnContacts.Enabled && TaskDetailsForm.FormBehavior.DisableContactsButton != (TaskDetailsForm.FormBehavior.DisableContactsButton & this._behavior);
        this.btnDelete.Enabled = this.btnDelete.Enabled && TaskDetailsForm.FormBehavior.DisableDeleteButton != (TaskDetailsForm.FormBehavior.DisableDeleteButton & this._behavior);
        if (TaskDetailsForm.FormBehavior.EnableUserSelection == (TaskDetailsForm.FormBehavior.EnableUserSelection & this._behavior))
        {
          if (this.btnContacts.Enabled || this.btnDelete.Enabled || this._task == null || this._task.Contacts == null || 1 != this._task.Contacts.Length)
            return;
          this.pnlUserSelection.Visible = true;
          this.pnlComments.Height = this.pnlUserSelection.Top - this.pnlComments.Top;
          if (this.rbSelectedUser.Enabled)
          {
            this._userInfos = this.getScopedUsersWithContactAccess();
            if (this._userInfos != null && this._userInfos.Length != 0)
            {
              foreach (object userInfo in this._userInfos)
                this.cboSelectedUser.Items.Add(userInfo);
              this.cboSelectedUser.SelectedIndex = 0;
            }
            else
              this.rbSelectedUser.Enabled = false;
          }
        }
        else
        {
          this.pnlUserSelection.Visible = false;
          this.pnlComments.Height = this.grpContacts.Bottom - this.pnlComments.Top;
        }
        if (TaskDetailsForm.FormBehavior.EnableMultiSelection == (TaskDetailsForm.FormBehavior.EnableMultiSelection & this._behavior))
        {
          this.btnAdd.Location = this._btnAddLocation;
          this.btnCancel.Location = this._btnCancelLocation;
          this.btnAddAll.Visible = true;
          this.btnCancelAll.Visible = true;
        }
        else
        {
          this.btnAdd.Location = this._btnCancelLocation;
          this.btnCancel.Location = this._btnCancelAllLocation;
          this.btnAddAll.Visible = false;
          this.btnCancelAll.Visible = false;
        }
      }
    }

    public UserSelectionMethod UserSelectionMethod
    {
      get
      {
        if (this.rbCurrentUser.Checked)
          return UserSelectionMethod.CurrentUser;
        return !this.rbContactOwner.Checked ? UserSelectionMethod.SelectedUser : UserSelectionMethod.ContactOwner;
      }
    }

    public string SelectedUser
    {
      get
      {
        return !this.rbSelectedUser.Checked || this.cboSelectedUser.SelectedItem == null ? string.Empty : ((UserInfo) this.cboSelectedUser.SelectedItem).Userid;
      }
    }

    public bool AddAllSelected => this._addAllSelected;

    public bool CancelAllSelected => this._cancelAllSelected;

    public TaskInfo Task
    {
      get
      {
        int taskId = this._task.TaskID;
        string userid = this._task.UserID;
        if (TaskDetailsForm.FormBehavior.EnableUserSelection == (TaskDetailsForm.FormBehavior.EnableUserSelection & this._behavior))
        {
          if (this.rbCurrentUser.Checked)
            userid = Session.UserID;
          else if (this.rbContactOwner.Checked)
          {
            BorrowerInfo borrower = Session.ContactManager.GetBorrower(int.Parse(this._contacts[0].ContactID));
            if (borrower != null)
              userid = borrower.OwnerID;
          }
          else
            userid = this._userInfos[this.cboSelectedUser.SelectedIndex].Userid;
        }
        TaskType taskType = this._task.TaskType;
        DateTime creationTime = this._task.CreationTime == DateTime.MaxValue ? DateTime.Now : this._task.CreationTime;
        DateTime now = DateTime.Now;
        TaskStatus selectedIndex1 = (TaskStatus) this.cmbBoxStatus.SelectedIndex;
        string text1 = this.txtBoxSubject.Text;
        string text2 = this.rtBoxNotes.Text;
        DateTime dueDate = this.dtPickerDueDate.Value == DateTime.MinValue ? DateTime.MaxValue : this.dtPickerDueDate.Value;
        DateTime startDate = this.dtPickerStartDate.Value;
        TaskPriority selectedIndex2 = (TaskPriority) this.cmbBoxPriority.SelectedIndex;
        return new TaskInfo(taskId, userid, taskType, selectedIndex1, text1, text2, dueDate, startDate, selectedIndex2, creationTime, now, this._contacts, this._task.CampaignStepID);
      }
    }

    public TaskDetailsForm(TaskInfo task)
    {
      this._task = task;
      this.InitializeComponent();
      this.setDefaultFormBehavior();
      this.cmbBoxStatus.Items.AddRange((object[]) TaskInfoUtils.TaskStatusStrings);
      this.cmbBoxStatus.SelectedIndex = 0;
      this.cmbBoxPriority.SelectedIndex = 1;
      if (this._task != null)
      {
        this.btnAdd.Text = "&Save";
        this.btnAddAll.Text = "Save All";
        this.txtBoxSubject.Text = this._task.Subject;
        this.rtBoxNotes.Text = this._task.Notes;
        this.cmbBoxStatus.SelectedItem = (object) TaskInfoUtils.TaskStatusStrings[(int) this._task.TaskStatus];
        this.cmbBoxPriority.SelectedItem = (object) TaskInfoUtils.TaskPriorityStrings[(int) this._task.Priority];
        this.dtPickerStartDate.Value = this._task.StartDate;
        this.dtPickerDueDate.Value = this._task.DueDate == DateTime.MaxValue ? DateTime.MinValue : this._task.DueDate;
        this.gvContacts.BeginUpdate();
        this.addContactsToListView(this._task.Contacts, false);
        this.gvContacts.EndUpdate();
      }
      else
      {
        this.dtPickerDueDate.Value = DateTime.MinValue;
        this.dtPickerStartDate.Value = DateTime.MinValue;
        this._task = new TaskInfo(0, Session.UserID, TaskType.NormalTask, TaskStatus.NotStarted, "", "", DateTime.MaxValue, DateTime.MinValue, TaskPriority.Normal, DateTime.MaxValue, DateTime.MaxValue, new ContactInfo[0], -1);
      }
      this.markSaved();
      if (this._task.TaskType != TaskType.CampaignTask || task.CampaignStepID == -1)
      {
        this.pnlCampaignInfo.Visible = false;
      }
      else
      {
        this.pnlCampaignInfo.Visible = true;
        this.txtCampaign.Text = this._task.CampaignInfo;
      }
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (TaskDetailsForm));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private ContactInfo[] _contacts
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        for (int nItemIndex = 0; nItemIndex < this.gvContacts.Items.Count; ++nItemIndex)
        {
          ContactInfo tag = (ContactInfo) this.gvContacts.Items[nItemIndex].Tag;
          arrayList.Add((object) tag);
        }
        return (ContactInfo[]) arrayList.ToArray(typeof (ContactInfo));
      }
    }

    private void setDefaultFormBehavior()
    {
      Point location1 = this.btnAdd.Location;
      int x1 = location1.X;
      location1 = this.btnAdd.Location;
      int y1 = location1.Y;
      this._btnAddLocation = new Point(x1, y1);
      Point location2 = this.btnAddAll.Location;
      int x2 = location2.X;
      location2 = this.btnAddAll.Location;
      int y2 = location2.Y;
      this._btnAddAllLocation = new Point(x2, y2);
      Point location3 = this.btnCancel.Location;
      int x3 = location3.X;
      location3 = this.btnCancel.Location;
      int y3 = location3.Y;
      this._btnCancelLocation = new Point(x3, y3);
      Point location4 = this.btnCancelAll.Location;
      int x4 = location4.X;
      location4 = this.btnCancelAll.Location;
      int y4 = location4.Y;
      this._btnCancelAllLocation = new Point(x4, y4);
      if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.Cnt_Campaign_AssignTaskToOther))
      {
        this.rbContactOwner.Enabled = false;
        this.rbSelectedUser.Enabled = false;
      }
      this.Behavior = (TaskDetailsForm.FormBehavior) 0;
    }

    private void markDirty() => this._saved = false;

    private void markSaved() => this._saved = true;

    private void addContactsToListView(ContactInfo[] contacts, bool checkDuplicate)
    {
      if (contacts == null || contacts.Length == 0)
        return;
      for (int index = 0; index < contacts.Length; ++index)
      {
        bool flag = false;
        if (checkDuplicate)
        {
          for (int nItemIndex = 0; nItemIndex < this.gvContacts.Items.Count; ++nItemIndex)
          {
            ContactInfo tag = (ContactInfo) this.gvContacts.Items[nItemIndex].Tag;
            if (tag.ContactType == contacts[index].ContactType && tag.ContactID == contacts[index].ContactID)
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
        {
          this.gvContacts.Items.Add(new GVItem(new string[2]
          {
            contacts[index].ContactName,
            contacts[index].ContactType == CategoryType.Borrower ? "Borrower" : "Business"
          })
          {
            Tag = (object) contacts[index]
          });
          if (CategoryType.BizPartner == contacts[index].ContactType)
            this._processingBizPartners = true;
        }
      }
      this.grpContacts.Text = "Contacts (" + (object) this.gvContacts.Items.Count + ")";
      if (!this._processingBizPartners)
        return;
      this.rbContactOwner.Enabled = false;
    }

    private bool addTask(TaskInfo taskInfo)
    {
      return 0 <= Session.ContactManager.InsertOrUpdateTask(taskInfo);
    }

    private UserInfo[] getScopedUsersWithContactAccess()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) aclManager.GetPersonaListByFeature(new AclFeature[1]
      {
        AclFeature.GlobalTab_Contacts
      }, AclTriState.True));
      RoleInfo[] allRoleFunctions = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      Dictionary<string, UserInfo> dictionary1 = new Dictionary<string, UserInfo>();
      Dictionary<string, UserInfo> dictionary2 = new Dictionary<string, UserInfo>();
      foreach (RoleInfo roleInfo in allRoleFunctions)
      {
        bool flag = false;
        foreach (int personaId in roleInfo.PersonaIDs)
        {
          if (stringList.Contains(personaId.ToString()))
          {
            flag = true;
            break;
          }
        }
        foreach (UserInfo userInfo in Session.OrganizationManager.GetScopedUsersWithRole(roleInfo.RoleID))
        {
          if (flag && !dictionary1.ContainsKey(userInfo.Userid))
            dictionary1.Add(userInfo.Userid, userInfo);
          else if (!dictionary2.ContainsKey(userInfo.Userid))
            dictionary2.Add(userInfo.Userid, userInfo);
        }
      }
      FeaturesAclManager featuresAclManager1 = aclManager;
      AclFeature[] features1 = new AclFeature[1]
      {
        AclFeature.GlobalTab_Contacts
      };
      foreach (string str in featuresAclManager1.GetUserListByFeature(features1, AclTriState.True))
      {
        if (dictionary2.ContainsKey(str) && !dictionary1.ContainsKey(str))
        {
          UserInfo user = Session.OrganizationManager.GetUser(str);
          dictionary1.Add(str, user);
        }
      }
      FeaturesAclManager featuresAclManager2 = aclManager;
      AclFeature[] features2 = new AclFeature[1]
      {
        AclFeature.GlobalTab_Contacts
      };
      foreach (string key in featuresAclManager2.GetUserListByFeature(features2, AclTriState.False))
      {
        if (dictionary1.ContainsKey(key))
          dictionary1.Remove(key);
      }
      UserInfo[] array = new UserInfo[dictionary1.Count];
      dictionary1.Values.CopyTo(array, 0);
      return array;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (this.txtBoxSubject.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Subject cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.DialogResult = DialogResult.None;
      }
      else
      {
        if ("btnAddAll" == ((Control) sender).Name)
          this._addAllSelected = true;
        DialogResult dialogResult = DialogResult.OK;
        if (TaskDetailsForm.FormBehavior.DisableAddTask != (TaskDetailsForm.FormBehavior.DisableAddTask & this._behavior))
        {
          if (this.addTask(this.Task))
          {
            this.OnTaskUpdatedEvent(EventArgs.Empty);
            this.markSaved();
          }
          else
            dialogResult = DialogResult.Cancel;
        }
        this.DialogResult = dialogResult;
        this.Close();
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      if ("btnCancelAll" == ((Control) sender).Name)
        this._cancelAllSelected = true;
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void TaskDetailsForm_Closing(object sender, CancelEventArgs e)
    {
      if (this._saved || TaskDetailsForm.FormBehavior.DisableAddTask == (TaskDetailsForm.FormBehavior.DisableAddTask & this._behavior))
        return;
      TaskInfo task = this.Task;
      if (!(task.Subject != this._task.Subject) && !(task.Notes != this._task.Notes) && !(task.DueDate != this._task.DueDate) && !(task.StartDate != this._task.StartDate) && task.TaskStatus == this._task.TaskStatus && task.Priority == this._task.Priority)
        return;
      this.markDirty();
      DialogResult dialogResult = MessageBox.Show((IWin32Window) this, "Do you want to save changes?", "Task", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
      if (DialogResult.Yes == dialogResult)
      {
        if (!this.addTask(task))
          return;
        this.OnTaskUpdatedEvent(EventArgs.Empty);
        this.markSaved();
      }
      else
      {
        if (DialogResult.Cancel != dialogResult)
          return;
        e.Cancel = true;
      }
    }

    private void btnContacts_Click(object sender, EventArgs e)
    {
      frmContacts frmContacts = new frmContacts(nameof (TaskDetailsForm), Session.UserID);
      if (frmContacts.ShowDialog() != DialogResult.OK)
        return;
      this.markDirty();
      ContactInfo[] selectedContacts = frmContacts.GetSelectedContacts();
      this.gvContacts.BeginUpdate();
      this.addContactsToListView(selectedContacts, true);
      this.gvContacts.EndUpdate();
    }

    private void dtPickerDueDate_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dtPickerDueDate.Value != DateTime.MinValue))
        return;
      if (this.dtPickerDueDate.Value < TaskListForm.DateTimeMinValue || this.dtPickerDueDate.Value > TaskListForm.DateTimeMaxValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified due date must be between " + TaskListForm.DateTimeMinValue.ToShortDateString() + " and " + TaskListForm.DateTimeMaxValue.ToShortDateString());
        this.dtPickerDueDate.Value = DateTime.Now;
      }
      else
      {
        if (!(this.dtPickerStartDate.Value != DateTime.MinValue) || !(this.dtPickerDueDate.Value < this.dtPickerStartDate.Value))
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "The due date of a task cannot occur before its start date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.dtPickerDueDate.Value = this.dtPickerStartDate.Value;
      }
    }

    private void dtPickerStartDate_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dtPickerStartDate.Value != DateTime.MinValue))
        return;
      if (this.dtPickerStartDate.Value < TaskListForm.DateTimeMinValue || this.dtPickerStartDate.Value > TaskListForm.DateTimeMaxValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified start date must be between " + TaskListForm.DateTimeMinValue.ToShortDateString() + " and " + TaskListForm.DateTimeMaxValue.ToShortDateString());
        this.dtPickerDueDate.Value = DateTime.Now;
      }
      else
      {
        if (!(this.dtPickerDueDate.Value != DateTime.MinValue) || !(this.dtPickerDueDate.Value < this.dtPickerStartDate.Value))
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "The start date of a task cannot occur after its due date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.dtPickerStartDate.Value = this.dtPickerDueDate.Value;
      }
    }

    private void gvContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnDelete.Enabled = false;
      if (this.gvContacts.SelectedItems.Count <= 0)
        return;
      this.btnDelete.Enabled = TaskDetailsForm.FormBehavior.DisableDeleteButton != (TaskDetailsForm.FormBehavior.DisableDeleteButton & this._behavior);
    }

    private void gvContacts_DoubleClick(object sender, EventArgs e)
    {
      if (this.gvContacts.SelectedItems.Count == 0 || TaskDetailsForm.FormBehavior.DisableViewButton == (TaskDetailsForm.FormBehavior.DisableViewButton & this._behavior))
        return;
      ContactInfo tag = (ContactInfo) this.gvContacts.SelectedItems[0].Tag;
      AttendeeInfo attendeeInfo = new AttendeeInfo();
      attendeeInfo.AssignInfo(tag);
      int num = (int) attendeeInfo.ShowDialog();
      if (!attendeeInfo.GoToContact)
        return;
      Session.MainScreen.NavigateToContact(attendeeInfo.SelectedContact);
      this.btnCancel.PerformClick();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvContacts.SelectedItems == null || this.gvContacts.SelectedItems.Count == 0)
        return;
      this.markDirty();
      foreach (GVItem selectedItem in this.gvContacts.SelectedItems)
        this.gvContacts.Items.Remove(selectedItem);
      this.grpContacts.Text = "Contacts (" + (object) this.gvContacts.Items.Count + ")";
    }

    private void txtBoxSubject_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.txtBoxSubject.Text.Length < 250 || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void rtBoxNotes_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.rtBoxNotes.Text.Length < 1024 || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    private void rbUserSelection_CheckedChanged(object sender, EventArgs e)
    {
      this.cboSelectedUser.Visible = this.rbSelectedUser.Checked;
    }

    public event TaskDetailsForm.TaskUpdatedEventHandler TaskUpdatedEvent;

    protected virtual void OnTaskUpdatedEvent(EventArgs e)
    {
      if (this.TaskUpdatedEvent == null)
        return;
      this.TaskUpdatedEvent((object) this, e);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.pnlTaskDetails = new Panel();
      this.panel1 = new Panel();
      this.txtBoxSubject = new TextBox();
      this.pnlCampaignInfo = new Panel();
      this.txtCampaign = new TextBox();
      this.lblCampaign = new Label();
      this.pnlComments = new BorderPanel();
      this.rtBoxNotes = new RichTextBox();
      this.label6 = new Label();
      this.pnlUserSelection = new Panel();
      this.lblUserSelection = new Label();
      this.cboSelectedUser = new ComboBox();
      this.rbSelectedUser = new RadioButton();
      this.rbContactOwner = new RadioButton();
      this.rbCurrentUser = new RadioButton();
      this.grpContacts = new GroupContainer();
      this.flowLayoutPanel3 = new FlowLayoutPanel();
      this.btnDelete = new StandardIconButton();
      this.btnContacts = new StandardIconButton();
      this.gvContacts = new GridView();
      this.dtPickerDueDate = new DatePicker();
      this.dtPickerStartDate = new DatePicker();
      this.cmbBoxPriority = new ComboBox();
      this.cmbBoxStatus = new ComboBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.pnlButtons = new Panel();
      this.btnCancelAll = new Button();
      this.btnAddAll = new Button();
      this.btnCancel = new Button();
      this.btnAdd = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.pnlTaskDetails.SuspendLayout();
      this.panel1.SuspendLayout();
      this.pnlCampaignInfo.SuspendLayout();
      this.pnlComments.SuspendLayout();
      this.pnlUserSelection.SuspendLayout();
      this.grpContacts.SuspendLayout();
      this.flowLayoutPanel3.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnContacts).BeginInit();
      this.pnlButtons.SuspendLayout();
      this.SuspendLayout();
      this.pnlTaskDetails.Controls.Add((Control) this.panel1);
      this.pnlTaskDetails.Controls.Add((Control) this.pnlComments);
      this.pnlTaskDetails.Controls.Add((Control) this.label6);
      this.pnlTaskDetails.Controls.Add((Control) this.pnlUserSelection);
      this.pnlTaskDetails.Controls.Add((Control) this.grpContacts);
      this.pnlTaskDetails.Controls.Add((Control) this.dtPickerDueDate);
      this.pnlTaskDetails.Controls.Add((Control) this.dtPickerStartDate);
      this.pnlTaskDetails.Controls.Add((Control) this.cmbBoxPriority);
      this.pnlTaskDetails.Controls.Add((Control) this.cmbBoxStatus);
      this.pnlTaskDetails.Controls.Add((Control) this.label5);
      this.pnlTaskDetails.Controls.Add((Control) this.label4);
      this.pnlTaskDetails.Controls.Add((Control) this.label3);
      this.pnlTaskDetails.Controls.Add((Control) this.label2);
      this.pnlTaskDetails.Controls.Add((Control) this.label1);
      this.pnlTaskDetails.Dock = DockStyle.Fill;
      this.pnlTaskDetails.Location = new Point(0, 0);
      this.pnlTaskDetails.Name = "pnlTaskDetails";
      this.pnlTaskDetails.Size = new Size(753, 384);
      this.pnlTaskDetails.TabIndex = 23;
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.txtBoxSubject);
      this.panel1.Controls.Add((Control) this.pnlCampaignInfo);
      this.panel1.Location = new Point(63, 10);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(680, 20);
      this.panel1.TabIndex = 43;
      this.txtBoxSubject.Dock = DockStyle.Fill;
      this.txtBoxSubject.Location = new Point(0, 0);
      this.txtBoxSubject.MaxLength = 250;
      this.txtBoxSubject.Name = "txtBoxSubject";
      this.txtBoxSubject.Size = new Size(321, 20);
      this.txtBoxSubject.TabIndex = 25;
      this.pnlCampaignInfo.BackColor = Color.Transparent;
      this.pnlCampaignInfo.Controls.Add((Control) this.txtCampaign);
      this.pnlCampaignInfo.Controls.Add((Control) this.lblCampaign);
      this.pnlCampaignInfo.Dock = DockStyle.Right;
      this.pnlCampaignInfo.Location = new Point(321, 0);
      this.pnlCampaignInfo.Name = "pnlCampaignInfo";
      this.pnlCampaignInfo.Padding = new Padding(7, 0, 0, 0);
      this.pnlCampaignInfo.Size = new Size(359, 20);
      this.pnlCampaignInfo.TabIndex = 27;
      this.txtCampaign.Dock = DockStyle.Fill;
      this.txtCampaign.Location = new Point(70, 0);
      this.txtCampaign.Name = "txtCampaign";
      this.txtCampaign.ReadOnly = true;
      this.txtCampaign.Size = new Size(289, 20);
      this.txtCampaign.TabIndex = 27;
      this.lblCampaign.AutoEllipsis = true;
      this.lblCampaign.Dock = DockStyle.Left;
      this.lblCampaign.Location = new Point(7, 0);
      this.lblCampaign.Name = "lblCampaign";
      this.lblCampaign.Size = new Size(63, 20);
      this.lblCampaign.TabIndex = 26;
      this.lblCampaign.Text = "Campaign";
      this.lblCampaign.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlComments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlComments.Controls.Add((Control) this.rtBoxNotes);
      this.pnlComments.Location = new Point(10, 98);
      this.pnlComments.Name = "pnlComments";
      this.pnlComments.Size = new Size(375, 182);
      this.pnlComments.TabIndex = 42;
      this.rtBoxNotes.AcceptsTab = true;
      this.rtBoxNotes.BorderStyle = BorderStyle.None;
      this.rtBoxNotes.Dock = DockStyle.Fill;
      this.rtBoxNotes.Location = new Point(1, 1);
      this.rtBoxNotes.Name = "rtBoxNotes";
      this.rtBoxNotes.Size = new Size(373, 180);
      this.rtBoxNotes.TabIndex = 33;
      this.rtBoxNotes.Text = "";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 84);
      this.label6.Name = "label6";
      this.label6.Size = new Size(57, 14);
      this.label6.TabIndex = 41;
      this.label6.Text = "Comments";
      this.pnlUserSelection.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlUserSelection.Controls.Add((Control) this.lblUserSelection);
      this.pnlUserSelection.Controls.Add((Control) this.cboSelectedUser);
      this.pnlUserSelection.Controls.Add((Control) this.rbSelectedUser);
      this.pnlUserSelection.Controls.Add((Control) this.rbContactOwner);
      this.pnlUserSelection.Controls.Add((Control) this.rbCurrentUser);
      this.pnlUserSelection.Location = new Point(0, 287);
      this.pnlUserSelection.Name = "pnlUserSelection";
      this.pnlUserSelection.Size = new Size(384, 95);
      this.pnlUserSelection.TabIndex = 25;
      this.lblUserSelection.AutoSize = true;
      this.lblUserSelection.Location = new Point(8, 11);
      this.lblUserSelection.Name = "lblUserSelection";
      this.lblUserSelection.Size = new Size(154, 14);
      this.lblUserSelection.TabIndex = 26;
      this.lblUserSelection.Text = "Select Task List to add task to:";
      this.cboSelectedUser.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSelectedUser.Location = new Point(162, 70);
      this.cboSelectedUser.Name = "cboSelectedUser";
      this.cboSelectedUser.Size = new Size(220, 22);
      this.cboSelectedUser.Sorted = true;
      this.cboSelectedUser.TabIndex = 25;
      this.cboSelectedUser.Visible = false;
      this.rbSelectedUser.AutoSize = true;
      this.rbSelectedUser.Location = new Point(10, 72);
      this.rbSelectedUser.Name = "rbSelectedUser";
      this.rbSelectedUser.Size = new Size(150, 18);
      this.rbSelectedUser.TabIndex = 24;
      this.rbSelectedUser.Text = "Selected User's Task List:";
      this.rbSelectedUser.UseVisualStyleBackColor = true;
      this.rbSelectedUser.CheckedChanged += new EventHandler(this.rbUserSelection_CheckedChanged);
      this.rbContactOwner.AutoSize = true;
      this.rbContactOwner.Location = new Point(10, 52);
      this.rbContactOwner.Name = "rbContactOwner";
      this.rbContactOwner.Size = new Size(153, 18);
      this.rbContactOwner.TabIndex = 23;
      this.rbContactOwner.Text = "Contact Owner's Task List";
      this.rbContactOwner.UseVisualStyleBackColor = true;
      this.rbCurrentUser.AutoSize = true;
      this.rbCurrentUser.Checked = true;
      this.rbCurrentUser.Location = new Point(10, 31);
      this.rbCurrentUser.Name = "rbCurrentUser";
      this.rbCurrentUser.Size = new Size(85, 18);
      this.rbCurrentUser.TabIndex = 22;
      this.rbCurrentUser.TabStop = true;
      this.rbCurrentUser.Text = "My Task List";
      this.rbCurrentUser.UseVisualStyleBackColor = true;
      this.rbCurrentUser.CheckedChanged += new EventHandler(this.rbUserSelection_CheckedChanged);
      this.grpContacts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpContacts.Controls.Add((Control) this.flowLayoutPanel3);
      this.grpContacts.Controls.Add((Control) this.gvContacts);
      this.grpContacts.Location = new Point(394, 32);
      this.grpContacts.Name = "grpContacts";
      this.grpContacts.Size = new Size(348, 352);
      this.grpContacts.TabIndex = 40;
      this.grpContacts.Text = "Contacts";
      this.flowLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel3.BackColor = Color.Transparent;
      this.flowLayoutPanel3.Controls.Add((Control) this.btnDelete);
      this.flowLayoutPanel3.Controls.Add((Control) this.btnContacts);
      this.flowLayoutPanel3.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel3.Location = new Point(292, 2);
      this.flowLayoutPanel3.Name = "flowLayoutPanel3";
      this.flowLayoutPanel3.Size = new Size(50, 22);
      this.flowLayoutPanel3.TabIndex = 1;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(31, 3);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 3;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Remove Contacts");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnContacts.BackColor = Color.Transparent;
      this.btnContacts.Location = new Point(9, 3);
      this.btnContacts.Name = "btnContacts";
      this.btnContacts.Size = new Size(16, 16);
      this.btnContacts.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnContacts.TabIndex = 5;
      this.btnContacts.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnContacts, "Add Contacts");
      this.btnContacts.Click += new EventHandler(this.btnContacts_Click);
      this.gvContacts.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Contact Name";
      gvColumn1.Width = 246;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Type";
      gvColumn2.Width = 100;
      this.gvContacts.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvContacts.Dock = DockStyle.Fill;
      this.gvContacts.Location = new Point(1, 26);
      this.gvContacts.Name = "gvContacts";
      this.gvContacts.Size = new Size(346, 325);
      this.gvContacts.TabIndex = 0;
      this.gvContacts.SelectedIndexChanged += new EventHandler(this.gvContacts_SelectedIndexChanged);
      this.gvContacts.DoubleClick += new EventHandler(this.gvContacts_DoubleClick);
      this.dtPickerDueDate.BackColor = SystemColors.Window;
      this.dtPickerDueDate.Location = new Point(256, 56);
      this.dtPickerDueDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtPickerDueDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtPickerDueDate.Name = "dtPickerDueDate";
      this.dtPickerDueDate.Size = new Size(129, 22);
      this.dtPickerDueDate.TabIndex = 27;
      this.dtPickerDueDate.Value = new DateTime(0L);
      this.dtPickerDueDate.ValueChanged += new EventHandler(this.dtPickerDueDate_ValueChanged);
      this.dtPickerStartDate.BackColor = SystemColors.Window;
      this.dtPickerStartDate.Location = new Point(256, 32);
      this.dtPickerStartDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtPickerStartDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtPickerStartDate.Name = "dtPickerStartDate";
      this.dtPickerStartDate.Size = new Size(129, 22);
      this.dtPickerStartDate.TabIndex = 29;
      this.dtPickerStartDate.Value = new DateTime(0L);
      this.dtPickerStartDate.ValueChanged += new EventHandler(this.dtPickerStartDate_ValueChanged);
      this.cmbBoxPriority.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxPriority.Items.AddRange(new object[3]
      {
        (object) "Low",
        (object) "Normal",
        (object) "High"
      });
      this.cmbBoxPriority.Location = new Point(63, 56);
      this.cmbBoxPriority.Name = "cmbBoxPriority";
      this.cmbBoxPriority.Size = new Size(129, 22);
      this.cmbBoxPriority.TabIndex = 31;
      this.cmbBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxStatus.Location = new Point(63, 32);
      this.cmbBoxStatus.Name = "cmbBoxStatus";
      this.cmbBoxStatus.Size = new Size(129, 22);
      this.cmbBoxStatus.TabIndex = 30;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 60);
      this.label5.Name = "label5";
      this.label5.Size = new Size(40, 14);
      this.label5.TabIndex = 34;
      this.label5.Text = "Priority";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 37);
      this.label4.Name = "label4";
      this.label4.Size = new Size(38, 14);
      this.label4.TabIndex = 32;
      this.label4.Text = "Status";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(198, 37);
      this.label3.Name = "label3";
      this.label3.Size = new Size(55, 14);
      this.label3.TabIndex = 28;
      this.label3.Text = "Start Date";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(198, 60);
      this.label2.Name = "label2";
      this.label2.Size = new Size(51, 14);
      this.label2.TabIndex = 26;
      this.label2.Text = "Due Date";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(43, 14);
      this.label1.TabIndex = 24;
      this.label1.Text = "Subject";
      this.pnlButtons.Controls.Add((Control) this.btnCancelAll);
      this.pnlButtons.Controls.Add((Control) this.btnAddAll);
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Controls.Add((Control) this.btnAdd);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 384);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(753, 44);
      this.pnlButtons.TabIndex = 24;
      this.btnCancelAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancelAll.BackColor = SystemColors.Control;
      this.btnCancelAll.Location = new Point(668, 12);
      this.btnCancelAll.Name = "btnCancelAll";
      this.btnCancelAll.Size = new Size(75, 22);
      this.btnCancelAll.TabIndex = 28;
      this.btnCancelAll.Text = "Cancel All";
      this.btnCancelAll.UseVisualStyleBackColor = true;
      this.btnCancelAll.Click += new EventHandler(this.btnClose_Click);
      this.btnAddAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAddAll.BackColor = SystemColors.Control;
      this.btnAddAll.Location = new Point(502, 12);
      this.btnAddAll.Name = "btnAddAll";
      this.btnAddAll.Size = new Size(75, 22);
      this.btnAddAll.TabIndex = 30;
      this.btnAddAll.Text = "Add All";
      this.btnAddAll.UseVisualStyleBackColor = true;
      this.btnAddAll.Click += new EventHandler(this.btnAdd_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(585, 12);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 15;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnClose_Click);
      this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAdd.BackColor = SystemColors.Control;
      this.btnAdd.DialogResult = DialogResult.OK;
      this.btnAdd.Location = new Point(419, 12);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 22);
      this.btnAdd.TabIndex = 19;
      this.btnAdd.Text = "&Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(753, 428);
      this.Controls.Add((Control) this.pnlTaskDetails);
      this.Controls.Add((Control) this.pnlButtons);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TaskDetailsForm);
      this.ShowInTaskbar = false;
      this.Text = "Task Details";
      this.Closing += new CancelEventHandler(this.TaskDetailsForm_Closing);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.pnlTaskDetails.ResumeLayout(false);
      this.pnlTaskDetails.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.pnlCampaignInfo.ResumeLayout(false);
      this.pnlCampaignInfo.PerformLayout();
      this.pnlComments.ResumeLayout(false);
      this.pnlUserSelection.ResumeLayout(false);
      this.pnlUserSelection.PerformLayout();
      this.grpContacts.ResumeLayout(false);
      this.flowLayoutPanel3.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnContacts).EndInit();
      this.pnlButtons.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public delegate void TaskUpdatedEventHandler(object sender, EventArgs e);

    public enum FormBehavior
    {
      DisableContactsButton = 1,
      DisableDeleteButton = 2,
      DisableViewButton = 4,
      DisableAddTask = 8,
      EnableUserSelection = 16, // 0x00000010
      EnableMultiSelection = 32, // 0x00000020
    }
  }
}

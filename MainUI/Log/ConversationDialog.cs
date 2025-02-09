// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.ConversationDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class ConversationDialog : UserControl
  {
    private const int MAX_ALERTS = 3;
    private ConversationLog convLog;
    private GVItem listViewItem;
    private RoleInfo[] roles;
    private GroupContainer groupContainerBottom;
    private StandardIconButton btnRolodex;
    private GroupContainer groupContainerTop;
    private ToolTip toolTip1;
    private Label lblPreviousComments;
    private EditMode editMode = EditMode.None;
    private bool deleteBackKey = true;
    private static Font bFont = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    private IContainer components;
    private Panel pnlDialogHeader;
    private Panel pnlFollowUpDetails;
    private Panel pnlFollowUpHeader;
    private Label lblFollowUpHeader;
    private Panel pnlPreviousComments;
    private TextBox txtPreviousComments;
    private Panel pnlNewComments;
    private Label lblNewComments;
    private TextBox txtNewComments;
    private Panel pnlDialogDetails;
    private TextBox txtName;
    private RadioButton rbEmail;
    private RadioButton rbPhone;
    private TextBox txtEmail;
    private TextBox txtPhone;
    private Label lblDateString;
    private Label lblDate;
    private TextBox txtCompany;
    private Label lblCompany;
    private Label lblName;
    private CheckBox chkShowInLog;
    private CheckBox chkEditLogEntry;
    private LogAlertEditControl ctlAlert1;
    private LogAlertEditControl ctlAlert2;
    private LogAlertEditControl ctlAlert3;

    public ConversationDialog(ConversationLog convLog, GVItem listViewItem)
    {
      this.convLog = convLog;
      this.listViewItem = listViewItem;
      this.InitializeComponent();
      if (this.convLog != null)
      {
        convLog.SessionUserName = Session.UserInfo.FullName;
        this.editMode = convLog.IsNew ? EditMode.AddMode : EditMode.EditModeLocked;
      }
      this.initializeControls();
      this.populateControls();
      this.setEditMode();
      if (this.convLog == null)
      {
        this.Enabled = false;
      }
      else
      {
        this.chkShowInLog.CheckedChanged += new EventHandler(this.chkShowInLog_CheckedChanged);
        this.txtName.Leave += new EventHandler(this.txtName_Leave);
        this.txtCompany.Leave += new EventHandler(this.txtCompany_Leave);
        this.rbPhone.CheckedChanged += new EventHandler(this.rbPhone_CheckedChanged);
        this.rbEmail.CheckedChanged += new EventHandler(this.rbEmail_CheckedChanged);
        this.txtPhone.TextChanged += new EventHandler(this.txtPhone_TextChanged);
        this.txtPhone.Leave += new EventHandler(this.txtPhone_Leave);
        this.txtEmail.Leave += new EventHandler(this.txtEmail_Leave);
        this.txtNewComments.Leave += new EventHandler(this.txtNewComments_Leave);
        this.txtPreviousComments.Leave += new EventHandler(this.txtPreviousComments_Leave);
        this.ctlAlert1.LogAlertChanged += new LogAlertChangedEventHandler(this.ctlAlert_LogAlertChanged);
        this.ctlAlert2.LogAlertChanged += new LogAlertChangedEventHandler(this.ctlAlert_LogAlertChanged);
        this.ctlAlert3.LogAlertChanged += new LogAlertChangedEventHandler(this.ctlAlert_LogAlertChanged);
      }
    }

    private void initializeControls()
    {
      this.Dock = DockStyle.Fill;
      if (this.listViewItem == null)
      {
        this.pnlDialogDetails.Controls.Remove((Control) this.lblDate);
        this.pnlDialogDetails.Controls.Remove((Control) this.lblDateString);
      }
      if (EditMode.EditModeLocked == this.editMode && (Session.UserInfo.IsSuperAdministrator() || !this.isConversationLogLocked()))
        this.chkEditLogEntry.Visible = true;
      this.roles = Session.LoanDataMgr.SystemConfiguration.AllRoles;
      this.ctlAlert1.Initialize(this.roles);
      this.ctlAlert2.Initialize(this.roles);
      this.ctlAlert3.Initialize(this.roles);
    }

    private void populateControls()
    {
      if (this.convLog == null)
        return;
      this.chkShowInLog.Checked = this.convLog.DisplayInLog;
      if (this.listViewItem != null)
      {
        this.lblDateString.Text = this.convLog.Date.ToString("ddd, MM/dd/yy h:mm tt");
        if (string.Empty != this.convLog.UserId)
        {
          UserInfo user = Session.OrganizationManager.GetUser(this.convLog.UserId);
          if ((UserInfo) null != user)
          {
            Label lblDateString = this.lblDateString;
            lblDateString.Text = lblDateString.Text + " by " + user.FullName + " (" + user.Userid + ")";
          }
        }
      }
      this.txtName.Text = this.convLog.Name;
      this.txtCompany.Text = this.convLog.Company;
      this.rbPhone.Checked = !this.convLog.IsEmail;
      this.rbEmail.Checked = this.convLog.IsEmail;
      this.txtPhone.Text = this.convLog.Phone;
      this.txtEmail.Text = this.convLog.Email;
      this.txtNewComments.Text = this.convLog.NewComments;
      this.txtPreviousComments.Text = this.convLog.Comments;
      int count1 = this.convLog.AlertList == null ? 0 : this.convLog.AlertList.Count;
      if (this.convLog.AlertList != null)
      {
        for (int index = count1 - 1; index >= 0; --index)
        {
          if (this.convLog.AlertList[index].RoleId == 0)
            this.convLog.AlertList.RemoveAt(index);
        }
      }
      int count2 = this.convLog.AlertList == null ? 0 : this.convLog.AlertList.Count;
      for (int index = 0; index < count2; ++index)
        this.populateAlertControl(this.convLog.AlertList[index], index);
      for (int alertIndex = count2; alertIndex < 3; ++alertIndex)
        this.populateAlertControl((LogAlert) null, alertIndex);
    }

    private void populateAlertControl(LogAlert alert, int alertIndex)
    {
      switch (alertIndex)
      {
        case 0:
          this.ctlAlert1.Populate(alert);
          this.ctlAlert1.LogAlertCreated += new LogAlertCreatedEventHandler(this.ctlAlert_LogAlertCreated);
          break;
        case 1:
          this.ctlAlert2.Populate(alert);
          this.ctlAlert2.LogAlertCreated += new LogAlertCreatedEventHandler(this.ctlAlert_LogAlertCreated);
          break;
        case 2:
          this.ctlAlert3.Populate(alert);
          this.ctlAlert3.LogAlertCreated += new LogAlertCreatedEventHandler(this.ctlAlert_LogAlertCreated);
          break;
      }
    }

    private void setEditMode()
    {
      bool flag = EditMode.EditModeLocked == this.editMode;
      this.btnRolodex.Enabled = !flag;
      this.txtName.ReadOnly = flag;
      this.txtCompany.ReadOnly = flag;
      this.rbPhone.Enabled = !flag;
      this.rbEmail.Enabled = !flag;
      this.txtPhone.ReadOnly = flag;
      this.txtEmail.ReadOnly = flag;
      this.txtNewComments.ReadOnly = false;
      this.txtPreviousComments.ReadOnly = flag;
      if (this.editMode == EditMode.AddMode)
      {
        this.lblNewComments.Text = "Comments";
        this.pnlNewComments.Height += this.pnlPreviousComments.Height;
        this.pnlPreviousComments.Visible = false;
        this.pnlNewComments.Dock = DockStyle.Fill;
      }
      else
        this.pnlNewComments.Dock = DockStyle.Top;
      this.ctlAlert1.SetEditMode(this.editMode);
      this.ctlAlert2.SetEditMode(this.editMode);
      this.ctlAlert3.SetEditMode(this.editMode);
      this.OnLogLockEvent(new LogLockEventArgs(EditMode.EditModeLocked == this.editMode));
    }

    private bool isConversationLogLocked()
    {
      return (bool) Session.StartupInfo.PolicySettings[(object) "Policies.ConversationLogLock"];
    }

    private void refreshDisplay(bool updateLogPanel, bool updateDialogHeader)
    {
      if (updateLogPanel && this.convLog.DisplayInLog)
        Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      if (updateDialogHeader)
        this.pnlDialogHeader.Invalidate();
      Session.LoanData.Dirty = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public void SetName(string name)
    {
      if (!(this.txtName.Text != name))
        return;
      this.txtName.Text = name;
      this.txtName_Leave((object) this.txtName, EventArgs.Empty);
    }

    public void SetCompany(string company)
    {
      if (!(this.txtCompany.Text != company))
        return;
      this.txtCompany.Text = company;
      this.txtCompany_Leave((object) this.txtCompany, EventArgs.Empty);
    }

    public void SetPhone(string phone) => this.SetPhone(phone, false);

    public void SetPhone(string phone, bool formatValue)
    {
      if (!(this.txtPhone.Text != phone))
        return;
      if (!formatValue)
        this.txtPhone.TextChanged -= new EventHandler(this.txtPhone_TextChanged);
      this.txtPhone.Text = phone;
      this.txtPhone_Leave((object) this.txtPhone, EventArgs.Empty);
      if (formatValue)
        return;
      this.txtPhone.TextChanged += new EventHandler(this.txtPhone_TextChanged);
    }

    public void SetEmail(string email)
    {
      if (!(this.txtEmail.Text != email))
        return;
      this.txtEmail.Text = email;
      this.txtEmail_Leave((object) this.txtEmail, EventArgs.Empty);
    }

    private void chkShowInLog_CheckedChanged(object sender, EventArgs e)
    {
      this.convLog.DisplayInLog = this.chkShowInLog.Checked;
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
    }

    private void btnFileContacts_Click(object sender, EventArgs e)
    {
      using (SelectionDialog selectionDialog = new SelectionDialog(this, Session.LoanData))
      {
        int num = (int) selectionDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void txtName_Leave(object sender, EventArgs e)
    {
      if (!(this.convLog.Name != this.txtName.Text))
        return;
      this.convLog.Name = this.txtName.Text;
      if (this.listViewItem != null)
        this.listViewItem.SubItems[2].Text = this.convLog.Name;
      this.refreshDisplay(true, true);
    }

    private void txtCompany_Leave(object sender, EventArgs e)
    {
      if (!(this.convLog.Company != this.txtCompany.Text))
        return;
      this.convLog.Company = this.txtCompany.Text;
      if (this.listViewItem != null)
        this.listViewItem.SubItems[3].Text = this.convLog.Company;
      this.refreshDisplay(false, true);
    }

    private void rbPhone_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.convLog.IsEmail && !this.rbPhone.Checked)
        SystemUtil.ShellExecute("mailto:" + AppSecurity.EncodeCommand(this.txtEmail.Text));
      this.convLog.IsEmail = !this.rbPhone.Checked;
      this.refreshDisplay(true, false);
    }

    private void rbEmail_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.convLog.IsEmail && this.rbEmail.Checked)
        SystemUtil.ShellExecute("mailto:" + AppSecurity.EncodeCommand(this.txtEmail.Text));
      this.convLog.IsEmail = this.rbEmail.Checked;
      this.refreshDisplay(true, false);
    }

    private void txtPhone_TextChanged(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        int newCursorPos = 0;
        bool needsUpdate = false;
        string str = Utils.FormatInput(textBox.Text, FieldFormat.PHONE, ref needsUpdate, textBox.SelectionStart, ref newCursorPos);
        if (needsUpdate)
        {
          textBox.Text = str;
          textBox.SelectionStart = newCursorPos;
        }
      }
      this.convLog.Phone = textBox.Text;
      this.refreshDisplay(false, false);
    }

    private void txtPhone_KeyDown(object sender, KeyEventArgs e)
    {
      if (Keys.Back != e.KeyCode && Keys.Delete != e.KeyCode)
        return;
      this.deleteBackKey = true;
    }

    private void txtPhone_Leave(object sender, EventArgs e)
    {
      if (!(this.convLog.Phone != this.txtPhone.Text))
        return;
      this.convLog.Phone = this.txtPhone.Text;
      this.refreshDisplay(false, false);
    }

    private void txtEmail_Leave(object sender, EventArgs e)
    {
      if (!(this.convLog.Email != this.txtEmail.Text))
        return;
      this.convLog.Email = this.txtEmail.Text;
      this.refreshDisplay(false, false);
    }

    private void txtNewComments_Leave(object sender, EventArgs e)
    {
      if (!(this.convLog.NewComments != this.txtNewComments.Text))
        return;
      this.convLog.NewComments = this.txtNewComments.Text;
      this.refreshDisplay(false, false);
    }

    private void txtPreviousComments_Leave(object sender, EventArgs e)
    {
      if (!(this.convLog.Comments != this.txtPreviousComments.Text))
        return;
      this.convLog.Comments = this.txtPreviousComments.Text;
      this.refreshDisplay(false, false);
    }

    private void chkEditLogEntry_CheckedChanged(object sender, EventArgs e)
    {
      this.editMode = this.chkEditLogEntry.Checked ? EditMode.EditModeUnlocked : EditMode.EditModeLocked;
      this.setEditMode();
    }

    private void ctlAlert_LogAlertChanged(object sender, EventArgs e)
    {
      if (this.listViewItem != null)
        this.listViewItem.SubItems[4].Text = this.convLog.AlertList.IsFollowUpRequired() ? "Yes" : "No";
      this.refreshDisplay(true, true);
    }

    private void ctlAlert_LogAlertCreated(object sender, LogAlertEventArgs e)
    {
      if (e.Alert == null)
        return;
      this.convLog.AlertList.Add(e.Alert);
    }

    private void pnlDialogHeader_Paint(object sender, PaintEventArgs e)
    {
      if (this.convLog == null)
        return;
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = stringBuilder1;
      DateTime dateTime;
      string str1;
      if (!this.convLog.IsEmail)
      {
        dateTime = this.convLog.Date;
        str1 = "Called on " + dateTime.ToString("MM/dd/yy");
      }
      else
        str1 = "Emailed ";
      stringBuilder2.Append(str1);
      float x1 = 4f;
      Brush brush = Brushes.Black;
      Graphics graphics = e.Graphics;
      graphics.DrawString(stringBuilder1.ToString(), ConversationDialog.bFont, brush, new PointF(x1, 4f));
      SizeF sizeF = graphics.MeasureString(stringBuilder1.ToString(), ConversationDialog.bFont);
      float x2 = x1 + (sizeF.Width + 4f);
      LogAlert mostCriticalAlert = this.convLog.AlertList.GetMostCriticalAlert();
      if (mostCriticalAlert == null)
        return;
      stringBuilder1.Length = 0;
      if (DateTime.MinValue == mostCriticalAlert.FollowedUpDate)
      {
        stringBuilder1.Append("Follow-up needed ");
        DateTime today = DateTime.Today;
        dateTime = mostCriticalAlert.DueDate;
        DateTime date = dateTime.Date;
        int days = (today - date).Days;
        if (days == 0)
        {
          stringBuilder1.Append("today!");
        }
        else
        {
          StringBuilder stringBuilder3 = stringBuilder1;
          dateTime = mostCriticalAlert.DueDate;
          string str2 = "on " + dateTime.ToString("MM/dd/yy");
          stringBuilder3.Append(str2);
          if (0 > days)
          {
            stringBuilder1.Append(" (in " + (object) Math.Abs(days) + " day(s))");
          }
          else
          {
            stringBuilder1.Append(" (" + (object) days + " day(s) ago!)");
            brush = (Brush) new SolidBrush(AppColors.AlertRed);
          }
        }
      }
      else
      {
        StringBuilder stringBuilder4 = stringBuilder1;
        string str3;
        if (!(DateTime.Today == mostCriticalAlert.FollowedUpDate))
        {
          dateTime = mostCriticalAlert.FollowedUpDate;
          str3 = "on " + dateTime.ToString("MM/dd/yy");
        }
        else
          str3 = "today";
        string str4 = "Followed up " + str3;
        stringBuilder4.Append(str4);
      }
      graphics.DrawString(stringBuilder1.ToString(), ConversationDialog.bFont, brush, new PointF(x2, 4f));
    }

    public event LogLockEventHandler LogLockEvent;

    protected virtual void OnLogLockEvent(LogLockEventArgs e)
    {
      if (this.LogLockEvent == null)
        return;
      this.LogLockEvent((object) this, e);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.chkShowInLog = new CheckBox();
      this.pnlDialogHeader = new Panel();
      this.chkEditLogEntry = new CheckBox();
      this.pnlFollowUpDetails = new Panel();
      this.ctlAlert3 = new LogAlertEditControl();
      this.ctlAlert2 = new LogAlertEditControl();
      this.ctlAlert1 = new LogAlertEditControl();
      this.pnlFollowUpHeader = new Panel();
      this.lblFollowUpHeader = new Label();
      this.pnlPreviousComments = new Panel();
      this.txtPreviousComments = new TextBox();
      this.pnlNewComments = new Panel();
      this.lblNewComments = new Label();
      this.txtNewComments = new TextBox();
      this.pnlDialogDetails = new Panel();
      this.btnRolodex = new StandardIconButton();
      this.txtName = new TextBox();
      this.rbEmail = new RadioButton();
      this.rbPhone = new RadioButton();
      this.txtEmail = new TextBox();
      this.txtPhone = new TextBox();
      this.lblDateString = new Label();
      this.lblDate = new Label();
      this.txtCompany = new TextBox();
      this.lblCompany = new Label();
      this.lblName = new Label();
      this.groupContainerBottom = new GroupContainer();
      this.groupContainerTop = new GroupContainer();
      this.toolTip1 = new ToolTip(this.components);
      this.lblPreviousComments = new Label();
      this.pnlDialogHeader.SuspendLayout();
      this.pnlFollowUpDetails.SuspendLayout();
      this.pnlFollowUpHeader.SuspendLayout();
      this.pnlPreviousComments.SuspendLayout();
      this.pnlNewComments.SuspendLayout();
      this.pnlDialogDetails.SuspendLayout();
      ((ISupportInitialize) this.btnRolodex).BeginInit();
      this.groupContainerBottom.SuspendLayout();
      this.groupContainerTop.SuspendLayout();
      this.SuspendLayout();
      this.chkShowInLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkShowInLog.Location = new Point(556, 4);
      this.chkShowInLog.Name = "chkShowInLog";
      this.chkShowInLog.Size = new Size(166, 18);
      this.chkShowInLog.TabIndex = 0;
      this.chkShowInLog.Text = "Show entry in the loan Log";
      this.pnlDialogHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlDialogHeader.BackColor = Color.Transparent;
      this.pnlDialogHeader.Controls.Add((Control) this.chkEditLogEntry);
      this.pnlDialogHeader.Controls.Add((Control) this.chkShowInLog);
      this.pnlDialogHeader.Location = new Point(2, 0);
      this.pnlDialogHeader.Name = "pnlDialogHeader";
      this.pnlDialogHeader.Size = new Size(709, 24);
      this.pnlDialogHeader.TabIndex = 0;
      this.pnlDialogHeader.Paint += new PaintEventHandler(this.pnlDialogHeader_Paint);
      this.chkEditLogEntry.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkEditLogEntry.Location = new Point(489, 4);
      this.chkEditLogEntry.Name = "chkEditLogEntry";
      this.chkEditLogEntry.Size = new Size(62, 18);
      this.chkEditLogEntry.TabIndex = 1;
      this.chkEditLogEntry.Text = "Edit log entry";
      this.chkEditLogEntry.Visible = false;
      this.chkEditLogEntry.CheckedChanged += new EventHandler(this.chkEditLogEntry_CheckedChanged);
      this.pnlFollowUpDetails.Controls.Add((Control) this.ctlAlert3);
      this.pnlFollowUpDetails.Controls.Add((Control) this.ctlAlert2);
      this.pnlFollowUpDetails.Controls.Add((Control) this.ctlAlert1);
      this.pnlFollowUpDetails.Dock = DockStyle.Top;
      this.pnlFollowUpDetails.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlFollowUpDetails.Location = new Point(1, 26);
      this.pnlFollowUpDetails.Name = "pnlFollowUpDetails";
      this.pnlFollowUpDetails.Size = new Size(713, 74);
      this.pnlFollowUpDetails.TabIndex = 3;
      this.ctlAlert3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlAlert3.Location = new Point(0, 52);
      this.ctlAlert3.Name = "ctlAlert3";
      this.ctlAlert3.Size = new Size(713, 22);
      this.ctlAlert3.TabIndex = 2;
      this.ctlAlert2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlAlert2.Location = new Point(0, 28);
      this.ctlAlert2.Name = "ctlAlert2";
      this.ctlAlert2.Size = new Size(713, 22);
      this.ctlAlert2.TabIndex = 1;
      this.ctlAlert1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlAlert1.Location = new Point(0, 4);
      this.ctlAlert1.Name = "ctlAlert1";
      this.ctlAlert1.Size = new Size(713, 22);
      this.ctlAlert1.TabIndex = 0;
      this.pnlFollowUpHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlFollowUpHeader.BackColor = Color.Transparent;
      this.pnlFollowUpHeader.Controls.Add((Control) this.lblFollowUpHeader);
      this.pnlFollowUpHeader.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlFollowUpHeader.Location = new Point(4, 2);
      this.pnlFollowUpHeader.Name = "pnlFollowUpHeader";
      this.pnlFollowUpHeader.Size = new Size(707, 22);
      this.pnlFollowUpHeader.TabIndex = 46;
      this.lblFollowUpHeader.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblFollowUpHeader.Location = new Point(4, 0);
      this.lblFollowUpHeader.Name = "lblFollowUpHeader";
      this.lblFollowUpHeader.Size = new Size(100, 22);
      this.lblFollowUpHeader.TabIndex = 0;
      this.lblFollowUpHeader.Text = "Follow Up";
      this.lblFollowUpHeader.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlPreviousComments.Controls.Add((Control) this.lblPreviousComments);
      this.pnlPreviousComments.Controls.Add((Control) this.txtPreviousComments);
      this.pnlPreviousComments.Dock = DockStyle.Fill;
      this.pnlPreviousComments.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlPreviousComments.Location = new Point(1, 187);
      this.pnlPreviousComments.Name = "pnlPreviousComments";
      this.pnlPreviousComments.Size = new Size(713, 196);
      this.pnlPreviousComments.TabIndex = 2;
      this.txtPreviousComments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPreviousComments.Location = new Point(4, 22);
      this.txtPreviousComments.Multiline = true;
      this.txtPreviousComments.Name = "txtPreviousComments";
      this.txtPreviousComments.ScrollBars = ScrollBars.Both;
      this.txtPreviousComments.Size = new Size(703, 169);
      this.txtPreviousComments.TabIndex = 1;
      this.pnlNewComments.Controls.Add((Control) this.lblNewComments);
      this.pnlNewComments.Controls.Add((Control) this.txtNewComments);
      this.pnlNewComments.Dock = DockStyle.Top;
      this.pnlNewComments.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlNewComments.Location = new Point(1, 91);
      this.pnlNewComments.Name = "pnlNewComments";
      this.pnlNewComments.Size = new Size(713, 96);
      this.pnlNewComments.TabIndex = 1;
      this.lblNewComments.AutoSize = true;
      this.lblNewComments.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblNewComments.Location = new Point(4, 6);
      this.lblNewComments.Name = "lblNewComments";
      this.lblNewComments.Size = new Size(83, 14);
      this.lblNewComments.TabIndex = 0;
      this.lblNewComments.Text = "New Comments";
      this.lblNewComments.TextAlign = ContentAlignment.MiddleLeft;
      this.txtNewComments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtNewComments.Location = new Point(4, 22);
      this.txtNewComments.Multiline = true;
      this.txtNewComments.Name = "txtNewComments";
      this.txtNewComments.ScrollBars = ScrollBars.Both;
      this.txtNewComments.Size = new Size(703, 69);
      this.txtNewComments.TabIndex = 1;
      this.pnlDialogDetails.Controls.Add((Control) this.btnRolodex);
      this.pnlDialogDetails.Controls.Add((Control) this.txtName);
      this.pnlDialogDetails.Controls.Add((Control) this.rbEmail);
      this.pnlDialogDetails.Controls.Add((Control) this.rbPhone);
      this.pnlDialogDetails.Controls.Add((Control) this.txtEmail);
      this.pnlDialogDetails.Controls.Add((Control) this.txtPhone);
      this.pnlDialogDetails.Controls.Add((Control) this.lblDateString);
      this.pnlDialogDetails.Controls.Add((Control) this.lblDate);
      this.pnlDialogDetails.Controls.Add((Control) this.txtCompany);
      this.pnlDialogDetails.Controls.Add((Control) this.lblCompany);
      this.pnlDialogDetails.Controls.Add((Control) this.lblName);
      this.pnlDialogDetails.Dock = DockStyle.Top;
      this.pnlDialogDetails.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlDialogDetails.Location = new Point(1, 25);
      this.pnlDialogDetails.Name = "pnlDialogDetails";
      this.pnlDialogDetails.Size = new Size(713, 66);
      this.pnlDialogDetails.TabIndex = 0;
      this.btnRolodex.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRolodex.BackColor = Color.Transparent;
      this.btnRolodex.Location = new Point(435, 23);
      this.btnRolodex.Name = "btnRolodex";
      this.btnRolodex.Size = new Size(16, 16);
      this.btnRolodex.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnRolodex.TabIndex = 11;
      this.btnRolodex.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRolodex, "Find a contact");
      this.btnRolodex.Click += new EventHandler(this.btnFileContacts_Click);
      this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtName.Location = new Point(83, 21);
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(346, 20);
      this.txtName.TabIndex = 4;
      this.txtName.Leave += new EventHandler(this.txtName_Leave);
      this.rbEmail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.rbEmail.Location = new Point(458, 45);
      this.rbEmail.Name = "rbEmail";
      this.rbEmail.Size = new Size(60, 16);
      this.rbEmail.TabIndex = 8;
      this.rbEmail.Text = "Email";
      this.rbPhone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.rbPhone.Location = new Point(458, 24);
      this.rbPhone.Name = "rbPhone";
      this.rbPhone.Size = new Size(61, 16);
      this.rbPhone.TabIndex = 7;
      this.rbPhone.Text = "Phone";
      this.txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtEmail.Location = new Point(519, 43);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(188, 20);
      this.txtEmail.TabIndex = 10;
      this.txtPhone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtPhone.Location = new Point(519, 21);
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(188, 20);
      this.txtPhone.TabIndex = 9;
      this.txtPhone.KeyDown += new KeyEventHandler(this.txtPhone_KeyDown);
      this.lblDateString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblDateString.Location = new Point(80, 4);
      this.lblDateString.Name = "lblDateString";
      this.lblDateString.Size = new Size(540, 16);
      this.lblDateString.TabIndex = 1;
      this.lblDateString.Text = "ddd, MM/dd/yy hh:mm tt";
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(4, 4);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(29, 14);
      this.lblDate.TabIndex = 0;
      this.lblDate.Text = "Date";
      this.lblDate.TextAlign = ContentAlignment.MiddleLeft;
      this.txtCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCompany.Location = new Point(83, 43);
      this.txtCompany.Name = "txtCompany";
      this.txtCompany.Size = new Size(368, 20);
      this.txtCompany.TabIndex = 6;
      this.txtCompany.Leave += new EventHandler(this.txtCompany_Leave);
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(4, 45);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(52, 14);
      this.lblCompany.TabIndex = 5;
      this.lblCompany.Text = "Company";
      this.lblCompany.TextAlign = ContentAlignment.MiddleLeft;
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(4, 24);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(34, 14);
      this.lblName.TabIndex = 3;
      this.lblName.Text = "Name";
      this.lblName.TextAlign = ContentAlignment.MiddleLeft;
      this.groupContainerBottom.Controls.Add((Control) this.pnlFollowUpDetails);
      this.groupContainerBottom.Controls.Add((Control) this.pnlFollowUpHeader);
      this.groupContainerBottom.Dock = DockStyle.Bottom;
      this.groupContainerBottom.Location = new Point(0, 383);
      this.groupContainerBottom.Name = "groupContainerBottom";
      this.groupContainerBottom.Size = new Size(715, 105);
      this.groupContainerBottom.TabIndex = 3;
      this.groupContainerTop.Borders = AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainerTop.Controls.Add((Control) this.pnlPreviousComments);
      this.groupContainerTop.Controls.Add((Control) this.pnlDialogHeader);
      this.groupContainerTop.Controls.Add((Control) this.pnlNewComments);
      this.groupContainerTop.Controls.Add((Control) this.pnlDialogDetails);
      this.groupContainerTop.Dock = DockStyle.Fill;
      this.groupContainerTop.Location = new Point(0, 0);
      this.groupContainerTop.Name = "groupContainerTop";
      this.groupContainerTop.Size = new Size(715, 383);
      this.groupContainerTop.TabIndex = 4;
      this.lblPreviousComments.AutoSize = true;
      this.lblPreviousComments.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPreviousComments.Location = new Point(4, 6);
      this.lblPreviousComments.Name = "lblPreviousComments";
      this.lblPreviousComments.Size = new Size(102, 14);
      this.lblPreviousComments.TabIndex = 0;
      this.lblPreviousComments.Text = "Previous Comments";
      this.lblPreviousComments.TextAlign = ContentAlignment.MiddleLeft;
      this.Controls.Add((Control) this.groupContainerTop);
      this.Controls.Add((Control) this.groupContainerBottom);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ConversationDialog);
      this.Size = new Size(715, 488);
      this.pnlDialogHeader.ResumeLayout(false);
      this.pnlFollowUpDetails.ResumeLayout(false);
      this.pnlFollowUpHeader.ResumeLayout(false);
      this.pnlPreviousComments.ResumeLayout(false);
      this.pnlPreviousComments.PerformLayout();
      this.pnlNewComments.ResumeLayout(false);
      this.pnlNewComments.PerformLayout();
      this.pnlDialogDetails.ResumeLayout(false);
      this.pnlDialogDetails.PerformLayout();
      ((ISupportInitialize) this.btnRolodex).EndInit();
      this.groupContainerBottom.ResumeLayout(false);
      this.groupContainerTop.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LogEntryWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LogEntryWS : UserControl, IOnlineHelpTarget
  {
    private const string className = "LogEntryWS";
    private const int MAX_ALERTS = 3;
    private const bool UPDATE_LOG_PANEL = true;
    private const bool UPDATE_DIALOG_HEADER = true;
    private LogEntryLog generalLog;
    private RoleInfo[] roles;
    private EditMode editMode = EditMode.None;
    private static Font bFont = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    private System.ComponentModel.Container components;
    private Button btnClose;
    private Button btnDelete;
    private Panel pnlAutoScroll;
    private Panel pnlDialogHeader;
    private Panel pnlDialogDetails;
    private Label lblDateString;
    private Label lblDate;
    private Label lblSubject;
    private TextBox txtSubject;
    private Panel pnlNewComments;
    private Label lblNewComments;
    private TextBox txtNewComments;
    private Panel pnlPreviousComments;
    private Label lblPreviousComments;
    private TextBox txtPreviousComments;
    private Panel pnlFollowUpHeader;
    private CheckBox chkEditLogEntry;
    private Label lblFollowUpHeader;
    private Panel pnlFollowUpDetails;
    private LogAlertEditControl ctlAlert1;
    private LogAlertEditControl ctlAlert2;
    private LogAlertEditControl ctlAlert3;

    public LogEntryWS(LogEntryLog generalLog)
    {
      this.generalLog = generalLog;
      this.InitializeComponent();
      generalLog.SessionUserName = Session.UserInfo.FullName;
      this.editMode = generalLog.IsNew ? EditMode.AddMode : EditMode.EditModeLocked;
      this.initializeControls();
      this.populateControls();
      this.setEditMode();
      this.txtSubject.Leave += new EventHandler(this.txtSubject_Leave);
      this.txtNewComments.Leave += new EventHandler(this.txtNewComments_Leave);
      this.txtPreviousComments.Leave += new EventHandler(this.txtPreviousComments_Leave);
      this.ctlAlert1.LogAlertChanged += new LogAlertChangedEventHandler(this.ctlAlert_LogAlertChanged);
      this.ctlAlert2.LogAlertChanged += new LogAlertChangedEventHandler(this.ctlAlert_LogAlertChanged);
      this.ctlAlert3.LogAlertChanged += new LogAlertChangedEventHandler(this.ctlAlert_LogAlertChanged);
    }

    public string GetHelpTargetName() => nameof (LogEntryWS);

    private void initializeControls()
    {
      this.Dock = DockStyle.Fill;
      this.pnlDialogHeader.BackColor = AppColors.SandySky;
      this.pnlFollowUpHeader.BackColor = AppColors.SandySky;
      this.btnDelete.BackColor = AppColors.BeachSurf;
      this.btnClose.BackColor = AppColors.BeachSurf;
      if (EditMode.EditModeLocked == this.editMode && (Session.UserInfo.IsSuperAdministrator() || !this.isGeneralLogLocked()))
        this.chkEditLogEntry.Visible = true;
      this.roles = Session.LoanDataMgr.SystemConfiguration.AllRoles;
      this.ctlAlert1.Initialize(this.roles);
      this.ctlAlert2.Initialize(this.roles);
      this.ctlAlert3.Initialize(this.roles);
    }

    private void populateControls()
    {
      this.lblDateString.Text = this.generalLog.Date.ToString("ddd, MM/dd/yy h:mm tt");
      if (string.Empty != this.generalLog.UserId)
      {
        UserInfo user = Session.OrganizationManager.GetUser(this.generalLog.UserId);
        if ((UserInfo) null != user)
        {
          Label lblDateString = this.lblDateString;
          lblDateString.Text = lblDateString.Text + " by " + user.FullName + " (" + user.Userid + ")";
        }
      }
      this.txtSubject.Text = this.generalLog.Description;
      this.txtNewComments.Text = this.generalLog.NewComments;
      this.txtPreviousComments.Text = this.generalLog.Comments;
      int count = this.generalLog.AlertList == null ? 0 : this.generalLog.AlertList.Count;
      if (count == 0)
      {
        this.ctlAlert1.Populate((LogAlert) null);
        this.ctlAlert2.Populate((LogAlert) null);
        this.ctlAlert3.Populate((LogAlert) null);
        this.ctlAlert1.LogAlertCreated += new LogAlertCreatedEventHandler(this.ctlAlert_LogAlertCreated);
        this.ctlAlert2.LogAlertCreated += new LogAlertCreatedEventHandler(this.ctlAlert_LogAlertCreated);
        this.ctlAlert3.LogAlertCreated += new LogAlertCreatedEventHandler(this.ctlAlert_LogAlertCreated);
      }
      else if (1 == count)
      {
        this.ctlAlert1.Populate(this.generalLog.AlertList[0]);
        this.ctlAlert2.Populate((LogAlert) null);
        this.ctlAlert3.Populate((LogAlert) null);
        this.ctlAlert2.LogAlertCreated += new LogAlertCreatedEventHandler(this.ctlAlert_LogAlertCreated);
        this.ctlAlert3.LogAlertCreated += new LogAlertCreatedEventHandler(this.ctlAlert_LogAlertCreated);
      }
      else if (2 == count)
      {
        this.ctlAlert1.Populate(this.generalLog.AlertList[0]);
        this.ctlAlert2.Populate(this.generalLog.AlertList[1]);
        this.ctlAlert3.Populate((LogAlert) null);
        this.ctlAlert3.LogAlertCreated += new LogAlertCreatedEventHandler(this.ctlAlert_LogAlertCreated);
      }
      else
      {
        this.ctlAlert1.Populate(this.generalLog.AlertList[0]);
        this.ctlAlert2.Populate(this.generalLog.AlertList[1]);
        this.ctlAlert3.Populate(this.generalLog.AlertList[2]);
      }
    }

    private void setEditMode()
    {
      bool flag = EditMode.EditModeLocked == this.editMode;
      this.btnDelete.Enabled = !flag;
      this.txtSubject.ReadOnly = flag;
      this.txtNewComments.ReadOnly = false;
      this.txtPreviousComments.ReadOnly = flag;
      if (this.editMode == EditMode.AddMode)
      {
        this.lblNewComments.Text = "Comments";
        this.pnlNewComments.Height += this.pnlPreviousComments.Height;
        this.pnlPreviousComments.Visible = false;
      }
      this.ctlAlert1.SetEditMode(this.editMode);
      this.ctlAlert2.SetEditMode(this.editMode);
      this.ctlAlert3.SetEditMode(this.editMode);
    }

    private bool isGeneralLogLocked()
    {
      return (bool) Session.StartupInfo.PolicySettings[(object) "Policies.GeneralLogLock"];
    }

    private void refreshDisplay(bool updateLogPanel, bool updateDialogHeader)
    {
      if (updateLogPanel)
        Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      if (!updateDialogHeader)
        return;
      this.pnlDialogHeader.Invalidate();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().RemoveFromWorkArea();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (DialogResult.Yes != Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the entry?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1))
        return;
      Session.Application.GetService<ILoanEditor>().RemoveFromWorkArea();
      Session.LoanData.GetLogList().RemoveRecord((LogRecordBase) this.generalLog);
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
    }

    private void txtSubject_Leave(object sender, EventArgs e)
    {
      if (!(this.generalLog.Description != this.txtSubject.Text))
        return;
      this.generalLog.Description = this.txtSubject.Text;
      this.refreshDisplay(true, false);
    }

    private void txtNewComments_Leave(object sender, EventArgs e)
    {
      if (!(this.generalLog.NewComments != this.txtNewComments.Text))
        return;
      this.generalLog.NewComments = this.txtNewComments.Text;
    }

    private void txtPreviousComments_Leave(object sender, EventArgs e)
    {
      if (!(this.generalLog.Comments != this.txtPreviousComments.Text))
        return;
      this.generalLog.Comments = this.txtPreviousComments.Text;
    }

    private void chkEditLogEntry_CheckedChanged(object sender, EventArgs e)
    {
      this.editMode = this.chkEditLogEntry.Checked ? EditMode.EditModeUnlocked : EditMode.EditModeLocked;
      this.setEditMode();
    }

    private void ctlAlert_LogAlertChanged(object sender, EventArgs e)
    {
      this.refreshDisplay(true, true);
    }

    private void ctlAlert_LogAlertCreated(object sender, LogAlertEventArgs e)
    {
      if (e.Alert == null)
        return;
      this.generalLog.AlertList.Add(e.Alert);
    }

    private void topPanel_Paint(object sender, PaintEventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Created on " + this.generalLog.Date.ToString("MM/dd/yy"));
      float x1 = 4f;
      Brush brush = Brushes.Black;
      Graphics graphics = e.Graphics;
      graphics.DrawString(stringBuilder.ToString(), LogEntryWS.bFont, brush, new PointF(x1, 4f));
      SizeF sizeF = graphics.MeasureString(stringBuilder.ToString(), LogEntryWS.bFont);
      float x2 = x1 + (sizeF.Width + 4f);
      LogAlert mostCriticalAlert = this.generalLog.AlertList.GetMostCriticalAlert();
      if (mostCriticalAlert == null)
        return;
      stringBuilder.Length = 0;
      if (DateTime.MinValue == mostCriticalAlert.FollowedUpDate)
      {
        stringBuilder.Append("Follow-up needed ");
        int days = (DateTime.Today - mostCriticalAlert.DueDate.Date).Days;
        if (days == 0)
        {
          stringBuilder.Append("today!");
        }
        else
        {
          stringBuilder.Append("on " + mostCriticalAlert.DueDate.ToString("MM/dd/yy"));
          if (0 > days)
          {
            stringBuilder.Append(" (in " + (object) Math.Abs(days) + " day(s))");
          }
          else
          {
            stringBuilder.Append(" (" + (object) days + " day(s) ago!)");
            brush = (Brush) new SolidBrush(AppColors.AlertRed);
          }
        }
      }
      else
        stringBuilder.Append("Followed up " + (DateTime.Today == mostCriticalAlert.FollowedUpDate ? "today" : "on " + mostCriticalAlert.FollowedUpDate.ToString("MM/dd/yy")));
      graphics.DrawString(stringBuilder.ToString(), LogEntryWS.bFont, brush, new PointF(x2, 4f));
    }

    private void InitializeComponent()
    {
      this.pnlDialogHeader = new Panel();
      this.btnClose = new Button();
      this.btnDelete = new Button();
      this.pnlAutoScroll = new Panel();
      this.pnlDialogDetails = new Panel();
      this.txtSubject = new TextBox();
      this.lblSubject = new Label();
      this.lblDateString = new Label();
      this.lblDate = new Label();
      this.pnlNewComments = new Panel();
      this.lblNewComments = new Label();
      this.txtNewComments = new TextBox();
      this.pnlPreviousComments = new Panel();
      this.lblPreviousComments = new Label();
      this.txtPreviousComments = new TextBox();
      this.pnlFollowUpHeader = new Panel();
      this.chkEditLogEntry = new CheckBox();
      this.lblFollowUpHeader = new Label();
      this.pnlFollowUpDetails = new Panel();
      this.ctlAlert3 = new LogAlertEditControl();
      this.ctlAlert2 = new LogAlertEditControl();
      this.ctlAlert1 = new LogAlertEditControl();
      this.pnlDialogHeader.SuspendLayout();
      this.pnlAutoScroll.SuspendLayout();
      this.pnlDialogDetails.SuspendLayout();
      this.pnlNewComments.SuspendLayout();
      this.pnlPreviousComments.SuspendLayout();
      this.pnlFollowUpHeader.SuspendLayout();
      this.pnlFollowUpDetails.SuspendLayout();
      this.SuspendLayout();
      this.pnlDialogHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlDialogHeader.BackColor = Color.Wheat;
      this.pnlDialogHeader.Controls.Add((Control) this.btnClose);
      this.pnlDialogHeader.Controls.Add((Control) this.btnDelete);
      this.pnlDialogHeader.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlDialogHeader.ForeColor = SystemColors.ControlText;
      this.pnlDialogHeader.Location = new Point(0, 0);
      this.pnlDialogHeader.Name = "pnlDialogHeader";
      this.pnlDialogHeader.Size = new Size(510, 22);
      this.pnlDialogHeader.TabIndex = 0;
      this.pnlDialogHeader.Paint += new PaintEventHandler(this.topPanel_Paint);
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.BackColor = Color.LightYellow;
      this.btnClose.ForeColor = Color.Black;
      this.btnClose.Location = new Point(394, 1);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(56, 20);
      this.btnClose.TabIndex = 2;
      this.btnClose.TabStop = false;
      this.btnClose.Text = "&Close";
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.LightYellow;
      this.btnDelete.ForeColor = Color.Black;
      this.btnDelete.Location = new Point(450, 1);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(56, 20);
      this.btnDelete.TabIndex = 1;
      this.btnDelete.TabStop = false;
      this.btnDelete.Text = "&Delete";
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.pnlAutoScroll.AutoScroll = true;
      this.pnlAutoScroll.AutoScrollMinSize = new Size(510, 420);
      this.pnlAutoScroll.Controls.Add((Control) this.pnlDialogHeader);
      this.pnlAutoScroll.Controls.Add((Control) this.pnlDialogDetails);
      this.pnlAutoScroll.Controls.Add((Control) this.pnlNewComments);
      this.pnlAutoScroll.Controls.Add((Control) this.pnlPreviousComments);
      this.pnlAutoScroll.Controls.Add((Control) this.pnlFollowUpHeader);
      this.pnlAutoScroll.Controls.Add((Control) this.pnlFollowUpDetails);
      this.pnlAutoScroll.Dock = DockStyle.Fill;
      this.pnlAutoScroll.Font = new Font("Arial", 8.25f);
      this.pnlAutoScroll.Location = new Point(0, 0);
      this.pnlAutoScroll.Name = "pnlAutoScroll";
      this.pnlAutoScroll.Size = new Size(510, 428);
      this.pnlAutoScroll.TabIndex = 1;
      this.pnlDialogDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlDialogDetails.Controls.Add((Control) this.txtSubject);
      this.pnlDialogDetails.Controls.Add((Control) this.lblSubject);
      this.pnlDialogDetails.Controls.Add((Control) this.lblDateString);
      this.pnlDialogDetails.Controls.Add((Control) this.lblDate);
      this.pnlDialogDetails.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlDialogDetails.Location = new Point(0, 22);
      this.pnlDialogDetails.Name = "pnlDialogDetails";
      this.pnlDialogDetails.Size = new Size(510, 44);
      this.pnlDialogDetails.TabIndex = 0;
      this.txtSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSubject.Location = new Point(49, 22);
      this.txtSubject.Name = "txtSubject";
      this.txtSubject.Size = new Size(172, 20);
      this.txtSubject.TabIndex = 51;
      this.txtSubject.Text = "";
      this.lblSubject.AutoSize = true;
      this.lblSubject.Location = new Point(4, 24);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(45, 16);
      this.lblSubject.TabIndex = 52;
      this.lblSubject.Text = "Subject:";
      this.lblSubject.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDateString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblDateString.Location = new Point(49, 4);
      this.lblDateString.Name = "lblDateString";
      this.lblDateString.Size = new Size(341, 16);
      this.lblDateString.TabIndex = 50;
      this.lblDateString.Text = "ddd, MM/dd/yy hh:mm tt  by wwwwwmmmmmwwwwwm";
      this.lblDateString.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(4, 4);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(31, 16);
      this.lblDate.TabIndex = 49;
      this.lblDate.Text = "Date:";
      this.lblDate.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlNewComments.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlNewComments.Controls.Add((Control) this.lblNewComments);
      this.pnlNewComments.Controls.Add((Control) this.txtNewComments);
      this.pnlNewComments.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlNewComments.Location = new Point(0, 66);
      this.pnlNewComments.Name = "pnlNewComments";
      this.pnlNewComments.Size = new Size(510, 96);
      this.pnlNewComments.TabIndex = 44;
      this.lblNewComments.AutoSize = true;
      this.lblNewComments.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNewComments.Location = new Point(4, 2);
      this.lblNewComments.Name = "lblNewComments";
      this.lblNewComments.Size = new Size(93, 16);
      this.lblNewComments.TabIndex = 26;
      this.lblNewComments.Text = "New Comments:";
      this.lblNewComments.TextAlign = ContentAlignment.MiddleLeft;
      this.txtNewComments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtNewComments.Location = new Point(4, 22);
      this.txtNewComments.Multiline = true;
      this.txtNewComments.Name = "txtNewComments";
      this.txtNewComments.ScrollBars = ScrollBars.Both;
      this.txtNewComments.Size = new Size(500, 72);
      this.txtNewComments.TabIndex = 25;
      this.txtNewComments.Text = "";
      this.pnlPreviousComments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlPreviousComments.Controls.Add((Control) this.lblPreviousComments);
      this.pnlPreviousComments.Controls.Add((Control) this.txtPreviousComments);
      this.pnlPreviousComments.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlPreviousComments.Location = new Point(0, 162);
      this.pnlPreviousComments.Name = "pnlPreviousComments";
      this.pnlPreviousComments.Size = new Size(510, 167);
      this.pnlPreviousComments.TabIndex = 45;
      this.lblPreviousComments.AutoSize = true;
      this.lblPreviousComments.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblPreviousComments.Location = new Point(4, 2);
      this.lblPreviousComments.Name = "lblPreviousComments";
      this.lblPreviousComments.Size = new Size(117, 16);
      this.lblPreviousComments.TabIndex = 25;
      this.lblPreviousComments.Text = "Previous Comments:";
      this.lblPreviousComments.TextAlign = ContentAlignment.MiddleLeft;
      this.txtPreviousComments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPreviousComments.Location = new Point(4, 22);
      this.txtPreviousComments.Multiline = true;
      this.txtPreviousComments.Name = "txtPreviousComments";
      this.txtPreviousComments.ScrollBars = ScrollBars.Both;
      this.txtPreviousComments.Size = new Size(500, 143);
      this.txtPreviousComments.TabIndex = 24;
      this.txtPreviousComments.Text = "";
      this.pnlFollowUpHeader.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlFollowUpHeader.BackColor = Color.Wheat;
      this.pnlFollowUpHeader.Controls.Add((Control) this.chkEditLogEntry);
      this.pnlFollowUpHeader.Controls.Add((Control) this.lblFollowUpHeader);
      this.pnlFollowUpHeader.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlFollowUpHeader.Location = new Point(0, 329);
      this.pnlFollowUpHeader.Name = "pnlFollowUpHeader";
      this.pnlFollowUpHeader.Size = new Size(510, 22);
      this.pnlFollowUpHeader.TabIndex = 47;
      this.chkEditLogEntry.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkEditLogEntry.Location = new Point(410, 0);
      this.chkEditLogEntry.Name = "chkEditLogEntry";
      this.chkEditLogEntry.Size = new Size(108, 22);
      this.chkEditLogEntry.TabIndex = 1;
      this.chkEditLogEntry.Text = "Edit log entry";
      this.chkEditLogEntry.Visible = false;
      this.chkEditLogEntry.CheckedChanged += new EventHandler(this.chkEditLogEntry_CheckedChanged);
      this.lblFollowUpHeader.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblFollowUpHeader.Location = new Point(4, 0);
      this.lblFollowUpHeader.Name = "lblFollowUpHeader";
      this.lblFollowUpHeader.Size = new Size(100, 22);
      this.lblFollowUpHeader.TabIndex = 0;
      this.lblFollowUpHeader.Text = "Follow Up";
      this.lblFollowUpHeader.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlFollowUpDetails.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlFollowUpDetails.Controls.Add((Control) this.ctlAlert3);
      this.pnlFollowUpDetails.Controls.Add((Control) this.ctlAlert2);
      this.pnlFollowUpDetails.Controls.Add((Control) this.ctlAlert1);
      this.pnlFollowUpDetails.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlFollowUpDetails.Location = new Point(0, 351);
      this.pnlFollowUpDetails.Name = "pnlFollowUpDetails";
      this.pnlFollowUpDetails.Size = new Size(510, 74);
      this.pnlFollowUpDetails.TabIndex = 48;
      this.ctlAlert3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlAlert3.Location = new Point(0, 52);
      this.ctlAlert3.Name = "ctlAlert3";
      this.ctlAlert3.Size = new Size(510, 22);
      this.ctlAlert3.TabIndex = 2;
      this.ctlAlert2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlAlert2.Location = new Point(0, 28);
      this.ctlAlert2.Name = "ctlAlert2";
      this.ctlAlert2.Size = new Size(510, 22);
      this.ctlAlert2.TabIndex = 1;
      this.ctlAlert1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlAlert1.Location = new Point(0, 4);
      this.ctlAlert1.Name = "ctlAlert1";
      this.ctlAlert1.Size = new Size(510, 22);
      this.ctlAlert1.TabIndex = 0;
      this.Controls.Add((Control) this.pnlAutoScroll);
      this.Font = new Font("Arial", 8.25f);
      this.Name = nameof (LogEntryWS);
      this.Size = new Size(510, 428);
      this.Leave += new EventHandler(this.txtSubject_Leave);
      this.pnlDialogHeader.ResumeLayout(false);
      this.pnlAutoScroll.ResumeLayout(false);
      this.pnlDialogDetails.ResumeLayout(false);
      this.pnlNewComments.ResumeLayout(false);
      this.pnlPreviousComments.ResumeLayout(false);
      this.pnlFollowUpHeader.ResumeLayout(false);
      this.pnlFollowUpDetails.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

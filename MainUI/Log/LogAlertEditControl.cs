// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LogAlertEditControl
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LogAlertEditControl : UserControl
  {
    private bool isAlertComplete;
    private EditMode editMode = EditMode.None;
    private RoleInfo[] roles;
    private bool displayTwoLines;
    private LogAlert alert;
    private System.ComponentModel.Container components;
    private Panel pnlAlertControl;
    private Label lblAlert;
    private ComboBox cboRoleName;
    private Label lblFollowUp;
    private DateTimePicker dtpFollowUp;
    private Label lblFollowedUp;
    private DateTimePicker dtpFollowedUp;

    [Category("Appearance")]
    [Description("Defines the images which will be displayed all controls in one line or two lines.")]
    [DefaultValue(typeof (bool), "false")]
    public bool DisplayTwoLines
    {
      get => this.displayTwoLines;
      set
      {
        this.displayTwoLines = value;
        if (this.displayTwoLines)
        {
          this.dtpFollowUp.Width = this.dtpFollowedUp.Width;
          this.dtpFollowUp.Left = this.Width - this.dtpFollowUp.Width - 5;
          this.lblFollowUp.Left = this.dtpFollowUp.Left - this.lblFollowUp.Width - 5;
          this.cboRoleName.Width = this.lblFollowUp.Left - this.cboRoleName.Left - 5;
          this.lblFollowedUp.Left = this.lblFollowUp.Left;
          this.dtpFollowedUp.Left = this.dtpFollowUp.Left;
          this.dtpFollowedUp.Top = this.dtpFollowUp.Top + this.dtpFollowUp.Height + 2;
          this.lblFollowedUp.Top = this.dtpFollowedUp.Top + 2;
        }
        else
        {
          this.dtpFollowedUp.Top = this.dtpFollowUp.Top;
          this.lblFollowedUp.Top = this.lblFollowUp.Top;
          this.dtpFollowedUp.Left = this.Width - this.dtpFollowedUp.Width - 5;
          this.lblFollowedUp.Left = this.dtpFollowedUp.Left - this.lblFollowedUp.Width - 5;
          this.dtpFollowUp.Left = this.lblFollowedUp.Left - this.dtpFollowUp.Width - 18;
          this.lblFollowUp.Left = this.dtpFollowUp.Left - this.lblFollowUp.Width;
          this.cboRoleName.Width = this.lblFollowUp.Left - this.cboRoleName.Left - 2;
        }
      }
    }

    public event LogAlertChangedEventHandler LogAlertChanged;

    public event LogAlertCreatedEventHandler LogAlertCreated;

    public LogAlert GetAlert() => this.alert;

    public LogAlertEditControl()
    {
      this.InitializeComponent();
      this.dtpFollowUp.Format = DateTimePickerFormat.Custom;
      this.dtpFollowUp.CustomFormat = " ";
      this.dtpFollowedUp.Checked = false;
      this.dtpFollowedUp.Format = DateTimePickerFormat.Custom;
      this.dtpFollowedUp.CustomFormat = " ";
      this.dtpFollowUp.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dtpFollowedUp.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
    }

    public void Initialize(RoleInfo[] roles)
    {
      this.roles = roles;
      this.cboRoleName.Items.Add((object) string.Empty);
      foreach (RoleSummaryInfo role in roles)
        this.cboRoleName.Items.Add((object) role.RoleName);
      this.cboRoleName.SelectedIndex = 0;
    }

    public void Populate(LogAlert alert)
    {
      this.disableEvents();
      this.alert = alert;
      if (this.alert == null)
      {
        this.cboRoleName.SelectedIndex = 0;
        this.dtpFollowUp.Format = DateTimePickerFormat.Custom;
        this.dtpFollowedUp.Checked = false;
        this.dtpFollowedUp.Format = DateTimePickerFormat.Custom;
      }
      else
      {
        string roleName = this.getRoleName(this.alert.RoleId);
        if (string.Empty == roleName)
          this.alert = new LogAlert(Session.UserID);
        this.cboRoleName.Text = this.getRoleName(this.alert.RoleId);
        if (DateTime.MinValue != this.alert.DueDate)
        {
          this.dtpFollowUp.Format = DateTimePickerFormat.Short;
          this.dtpFollowUp.Value = this.alert.DueDate;
        }
        else
          this.dtpFollowUp.Format = DateTimePickerFormat.Custom;
        if (DateTime.MinValue != this.alert.FollowedUpDate)
        {
          this.dtpFollowedUp.Checked = true;
          this.dtpFollowedUp.Format = DateTimePickerFormat.Short;
          this.dtpFollowedUp.Value = this.alert.FollowedUpDate;
        }
        else
        {
          this.dtpFollowedUp.Checked = false;
          this.dtpFollowedUp.Format = DateTimePickerFormat.Custom;
        }
      }
      this.isAlertComplete = this.dtpFollowedUp.Checked;
      this.cboRoleName_SelectedIndexChanged((object) null, (EventArgs) null);
      this.enableEvents();
    }

    public void SetEditMode(EditMode editMode)
    {
      this.editMode = editMode;
      this.cboRoleName.Enabled = false;
      this.dtpFollowUp.Enabled = false;
      this.dtpFollowedUp.Enabled = false;
      if (this.cboRoleName.SelectedIndex == 0)
      {
        this.cboRoleName.Enabled = this.editMode != EditMode.EditModeLocked;
      }
      else
      {
        if (this.alert.IsNew)
        {
          this.cboRoleName.Enabled = true;
          this.dtpFollowUp.Enabled = true;
        }
        if (Session.UserInfo.IsAdministrator() && EditMode.EditModeUnlocked == editMode)
        {
          this.cboRoleName.Enabled = true;
          this.dtpFollowUp.Enabled = true;
          this.dtpFollowedUp.Enabled = true;
        }
        else
        {
          if (Session.UserID == this.alert.UserId && (!this.isAlertComplete || EditMode.EditModeUnlocked == editMode))
          {
            this.cboRoleName.Enabled = true;
            this.dtpFollowUp.Enabled = true;
          }
          if (!Session.LoanData.GetLogList().IsUserInAssignedRole(Session.UserID, this.alert.RoleId) || this.isAlertComplete && EditMode.EditModeUnlocked != editMode)
            return;
          this.dtpFollowUp.Enabled = true;
          this.dtpFollowedUp.Enabled = true;
        }
      }
    }

    private void enableEvents()
    {
      this.cboRoleName.SelectedIndexChanged += new EventHandler(this.cboRoleName_SelectedIndexChanged);
      this.dtpFollowUp.ValueChanged += new EventHandler(this.dtpFollowUp_ValueChanged);
      this.dtpFollowedUp.ValueChanged += new EventHandler(this.dtpFollowedUp_ValueChanged);
    }

    private void disableEvents()
    {
      this.cboRoleName.SelectedIndexChanged -= new EventHandler(this.cboRoleName_SelectedIndexChanged);
      this.dtpFollowUp.ValueChanged -= new EventHandler(this.dtpFollowUp_ValueChanged);
      this.dtpFollowedUp.ValueChanged -= new EventHandler(this.dtpFollowedUp_ValueChanged);
    }

    private int getRoleId(string roleName)
    {
      foreach (RoleInfo role in this.roles)
      {
        if (roleName == role.RoleName)
          return role.RoleID;
      }
      return -1;
    }

    private string getRoleName(int roleId)
    {
      foreach (RoleInfo role in this.roles)
      {
        if (roleId == role.RoleID)
          return role.RoleName;
      }
      return string.Empty;
    }

    private string getRoleAbbr(int roleId)
    {
      foreach (RoleInfo role in this.roles)
      {
        if (roleId == role.RoleID)
          return role.RoleAbbr;
      }
      return string.Empty;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void cboRoleName_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (sender != null)
        this.disableEvents();
      if (this.alert == null && sender != null)
      {
        this.alert = new LogAlert(Session.UserID);
        this.OnLogAlertCreated(new LogAlertEventArgs(this.alert));
      }
      bool flag = false;
      if (this.cboRoleName.SelectedIndex > 0)
      {
        flag = true;
        this.dtpFollowUp.Enabled = true;
        int roleId = this.getRoleId(this.cboRoleName.SelectedItem.ToString());
        this.dtpFollowedUp.Enabled = Session.LoanData.GetLogList().IsUserInAssignedRole(Session.UserID, roleId);
      }
      if (this.alert != null)
      {
        if (!flag)
        {
          this.alert.RoleId = 0;
          this.alert.DueDate = DateTime.MinValue;
        }
        else
        {
          this.alert.RoleId = this.getRoleId(this.cboRoleName.SelectedItem.ToString());
          if (DateTime.MinValue == this.alert.DueDate)
            this.alert.DueDate = DateTime.Now;
        }
      }
      if (this.alert != null && (!flag || !this.dtpFollowedUp.Checked))
        this.alert.FollowedUpDate = DateTime.MinValue;
      if (!flag)
      {
        this.dtpFollowUp.Enabled = false;
        this.dtpFollowedUp.Enabled = false;
      }
      if (this.alert == null || DateTime.MinValue == this.alert.DueDate)
      {
        this.dtpFollowUp.Format = DateTimePickerFormat.Custom;
      }
      else
      {
        this.dtpFollowUp.Format = DateTimePickerFormat.Short;
        if (sender == null)
          this.dtpFollowUp.Value = this.alert.DueDate;
      }
      if (this.alert == null || DateTime.MinValue == this.alert.FollowedUpDate)
      {
        this.dtpFollowedUp.Checked = false;
        this.dtpFollowedUp.Format = DateTimePickerFormat.Custom;
      }
      else
      {
        this.dtpFollowedUp.Checked = true;
        this.dtpFollowedUp.Format = DateTimePickerFormat.Short;
        if (sender == null)
          this.dtpFollowedUp.Value = this.alert.FollowedUpDate;
      }
      if (sender != null)
        this.OnLogAlertChanged(EventArgs.Empty);
      if (sender == null)
        return;
      this.enableEvents();
    }

    private void dtpFollowUp_ValueChanged(object sender, EventArgs e)
    {
      if (this.alert == null || this.alert.RoleId == 0)
        return;
      this.alert.DueDate = this.dtpFollowUp.Value;
      this.OnLogAlertChanged(EventArgs.Empty);
    }

    private void dtpFollowedUp_ValueChanged(object sender, EventArgs e)
    {
      if (this.dtpFollowedUp.Checked)
      {
        if (this.alert == null || this.alert.RoleId == 0)
        {
          this.dtpFollowedUp.Checked = !this.dtpFollowedUp.Checked;
          return;
        }
        this.alert.FollowedUpDate = this.dtpFollowedUp.Value;
        this.dtpFollowedUp.Format = DateTimePickerFormat.Short;
      }
      else
      {
        this.alert.FollowedUpDate = DateTime.MinValue;
        this.dtpFollowedUp.Format = DateTimePickerFormat.Custom;
      }
      this.OnLogAlertChanged(EventArgs.Empty);
    }

    protected virtual void OnLogAlertChanged(EventArgs e)
    {
      if (this.LogAlertChanged == null)
        return;
      this.LogAlertChanged((object) this, e);
    }

    protected virtual void OnLogAlertCreated(LogAlertEventArgs e)
    {
      if (this.LogAlertCreated == null)
        return;
      this.LogAlertCreated((object) this, e);
    }

    private void InitializeComponent()
    {
      this.pnlAlertControl = new Panel();
      this.dtpFollowedUp = new DateTimePicker();
      this.lblFollowedUp = new Label();
      this.dtpFollowUp = new DateTimePicker();
      this.lblFollowUp = new Label();
      this.cboRoleName = new ComboBox();
      this.lblAlert = new Label();
      this.pnlAlertControl.SuspendLayout();
      this.SuspendLayout();
      this.pnlAlertControl.Controls.Add((Control) this.dtpFollowedUp);
      this.pnlAlertControl.Controls.Add((Control) this.lblFollowedUp);
      this.pnlAlertControl.Controls.Add((Control) this.dtpFollowUp);
      this.pnlAlertControl.Controls.Add((Control) this.lblFollowUp);
      this.pnlAlertControl.Controls.Add((Control) this.cboRoleName);
      this.pnlAlertControl.Controls.Add((Control) this.lblAlert);
      this.pnlAlertControl.Dock = DockStyle.Fill;
      this.pnlAlertControl.Location = new Point(0, 0);
      this.pnlAlertControl.Name = "pnlAlertControl";
      this.pnlAlertControl.Size = new Size(506, 22);
      this.pnlAlertControl.TabIndex = 0;
      this.dtpFollowedUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.dtpFollowedUp.Font = new Font("Arial", 8.25f);
      this.dtpFollowedUp.Format = DateTimePickerFormat.Short;
      this.dtpFollowedUp.Location = new Point(405, 1);
      this.dtpFollowedUp.Name = "dtpFollowedUp";
      this.dtpFollowedUp.ShowCheckBox = true;
      this.dtpFollowedUp.Size = new Size(96, 20);
      this.dtpFollowedUp.TabIndex = 5;
      this.lblFollowedUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblFollowedUp.AutoSize = true;
      this.lblFollowedUp.Font = new Font("Arial", 8.25f);
      this.lblFollowedUp.Location = new Point(323, 3);
      this.lblFollowedUp.Name = "lblFollowedUp";
      this.lblFollowedUp.Size = new Size(81, 14);
      this.lblFollowedUp.TabIndex = 4;
      this.lblFollowedUp.Text = "Followed up on";
      this.lblFollowedUp.TextAlign = ContentAlignment.MiddleLeft;
      this.dtpFollowUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.dtpFollowUp.Font = new Font("Arial", 8.25f);
      this.dtpFollowUp.Format = DateTimePickerFormat.Short;
      this.dtpFollowUp.Location = new Point(225, 1);
      this.dtpFollowUp.Name = "dtpFollowUp";
      this.dtpFollowUp.Size = new Size(80, 20);
      this.dtpFollowUp.TabIndex = 3;
      this.lblFollowUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblFollowUp.AutoSize = true;
      this.lblFollowUp.Font = new Font("Arial", 8.25f);
      this.lblFollowUp.Location = new Point(146, 3);
      this.lblFollowUp.Name = "lblFollowUp";
      this.lblFollowUp.Size = new Size(79, 14);
      this.lblFollowUp.TabIndex = 2;
      this.lblFollowUp.Text = "to follow up on";
      this.lblFollowUp.TextAlign = ContentAlignment.MiddleLeft;
      this.cboRoleName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboRoleName.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRoleName.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboRoleName.Location = new Point(34, 0);
      this.cboRoleName.Name = "cboRoleName";
      this.cboRoleName.Size = new Size(110, 22);
      this.cboRoleName.TabIndex = 1;
      this.lblAlert.AutoSize = true;
      this.lblAlert.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblAlert.Location = new Point(4, 3);
      this.lblAlert.Name = "lblAlert";
      this.lblAlert.Size = new Size(30, 14);
      this.lblAlert.TabIndex = 0;
      this.lblAlert.Text = "Alert";
      this.lblAlert.TextAlign = ContentAlignment.MiddleLeft;
      this.Controls.Add((Control) this.pnlAlertControl);
      this.Name = nameof (LogAlertEditControl);
      this.Size = new Size(506, 22);
      this.pnlAlertControl.ResumeLayout(false);
      this.pnlAlertControl.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

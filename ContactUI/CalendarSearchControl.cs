// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CalendarSearchControl
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class CalendarSearchControl : UserControl
  {
    private string currentCalendarUserID = "";
    private CSMessage.AccessLevel currentCalendarUserAccessLevel;
    private IContainer components;
    private Panel pnlBackGround;
    private ComboBox cbxCalendarControl;
    private TextBox txtDisplayName;
    private Label lblAccessLevel;
    private PictureBox picDisallow;
    private PictureBox picAllow;

    public CalendarSearchControl()
    {
      this.InitializeComponent();
      this.EnforceSecurity();
    }

    private void cbxCalendarControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbxCalendarControl.SelectedIndex == 0)
      {
        this.picDisallow.BringToFront();
        this.SetCurrentCalendarUser(Session.UserID, CSMessage.AccessLevel.Full);
      }
      else
      {
        this.picAllow.BringToFront();
        this.picAllow_Click((object) null, (EventArgs) null);
      }
    }

    public event CalendarSearchControl.CalendarSearchEventHandler CalendarSearchEvent;

    protected virtual void OnCalendarSearchEvent(CalendarSearchControl.CalendarSearchEventArgs e)
    {
      if (this.CalendarSearchEvent == null)
        return;
      this.CalendarSearchEvent((object) this, e);
    }

    public void SetCurrentCalendarUser(string userID, CSMessage.AccessLevel accessLevel)
    {
      this.currentCalendarUserID = userID;
      this.currentCalendarUserAccessLevel = accessLevel;
      UserInfo userInfo = !(this.currentCalendarUserID == Session.UserID) ? Session.OrganizationManager.GetUser(this.currentCalendarUserID) : Session.UserInfo;
      this.txtDisplayName.Text = userInfo.FirstName + " " + userInfo.LastName;
      switch (this.currentCalendarUserAccessLevel)
      {
        case CSMessage.AccessLevel.ReadOnly:
          this.lblAccessLevel.Text = "Access Level: Read Only";
          break;
        case CSMessage.AccessLevel.Partial:
          this.lblAccessLevel.Text = "Access Level: Partial";
          break;
        case CSMessage.AccessLevel.Full:
          this.lblAccessLevel.Text = "Access Level: Full";
          break;
      }
      if (this.currentCalendarUserID != Session.UserID)
      {
        this.cbxCalendarControl.SelectedIndex = 1;
      }
      else
      {
        this.cbxCalendarControl.SelectedIndex = 0;
        this.lblAccessLevel.Text = "";
      }
      this.OnCalendarSearchEvent(new CalendarSearchControl.CalendarSearchEventArgs(userInfo, this.currentCalendarUserAccessLevel));
    }

    public string CurrentCalendarUserID => this.currentCalendarUserID;

    private void EnforceSecurity()
    {
      if (Session.MainScreen.AllowCalendarSharing())
        return;
      this.picDisallow.BringToFront();
      this.cbxCalendarControl.Enabled = false;
    }

    private void picAllow_Click(object sender, EventArgs e)
    {
      if (!(this.cbxCalendarControl.Text == "Team Member Calendar"))
        return;
      CSManagementDialog.Start();
    }

    private void picAllow_MouseEnter(object sender, EventArgs e) => this.Cursor = Cursors.Hand;

    private void picAllow_MouseLeave(object sender, EventArgs e) => this.Cursor = Cursors.Default;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CalendarSearchControl));
      this.pnlBackGround = new Panel();
      this.picAllow = new PictureBox();
      this.picDisallow = new PictureBox();
      this.lblAccessLevel = new Label();
      this.cbxCalendarControl = new ComboBox();
      this.txtDisplayName = new TextBox();
      this.pnlBackGround.SuspendLayout();
      ((ISupportInitialize) this.picAllow).BeginInit();
      ((ISupportInitialize) this.picDisallow).BeginInit();
      this.SuspendLayout();
      this.pnlBackGround.BackColor = Color.Transparent;
      this.pnlBackGround.Controls.Add((Control) this.picAllow);
      this.pnlBackGround.Controls.Add((Control) this.picDisallow);
      this.pnlBackGround.Controls.Add((Control) this.lblAccessLevel);
      this.pnlBackGround.Controls.Add((Control) this.cbxCalendarControl);
      this.pnlBackGround.Controls.Add((Control) this.txtDisplayName);
      this.pnlBackGround.Location = new Point(0, 0);
      this.pnlBackGround.Name = "pnlBackGround";
      this.pnlBackGround.Size = new Size(487, 24);
      this.pnlBackGround.TabIndex = 0;
      this.picAllow.BackColor = Color.Transparent;
      this.picAllow.Image = (Image) componentResourceManager.GetObject("picAllow.Image");
      this.picAllow.Location = new Point(302, 0);
      this.picAllow.Name = "picAllow";
      this.picAllow.Size = new Size(18, 19);
      this.picAllow.TabIndex = 7;
      this.picAllow.TabStop = false;
      this.picAllow.MouseLeave += new EventHandler(this.picAllow_MouseLeave);
      this.picAllow.Click += new EventHandler(this.picAllow_Click);
      this.picAllow.MouseEnter += new EventHandler(this.picAllow_MouseEnter);
      this.picDisallow.BackColor = Color.Transparent;
      this.picDisallow.Image = (Image) componentResourceManager.GetObject("picDisallow.Image");
      this.picDisallow.InitialImage = (Image) null;
      this.picDisallow.Location = new Point(302, 0);
      this.picDisallow.Name = "picDisallow";
      this.picDisallow.Size = new Size(18, 19);
      this.picDisallow.TabIndex = 6;
      this.picDisallow.TabStop = false;
      this.lblAccessLevel.AutoSize = true;
      this.lblAccessLevel.Location = new Point(326, 0);
      this.lblAccessLevel.Name = "lblAccessLevel";
      this.lblAccessLevel.Size = new Size(77, 13);
      this.lblAccessLevel.TabIndex = 5;
      this.lblAccessLevel.Text = "Access Level: ";
      this.cbxCalendarControl.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbxCalendarControl.FormattingEnabled = true;
      this.cbxCalendarControl.Items.AddRange(new object[2]
      {
        (object) "My Calendar",
        (object) "Team Member Calendar"
      });
      this.cbxCalendarControl.Location = new Point(3, 0);
      this.cbxCalendarControl.Name = "cbxCalendarControl";
      this.cbxCalendarControl.Size = new Size(144, 21);
      this.cbxCalendarControl.TabIndex = 1;
      this.cbxCalendarControl.SelectedIndexChanged += new EventHandler(this.cbxCalendarControl_SelectedIndexChanged);
      this.txtDisplayName.Location = new Point(153, 0);
      this.txtDisplayName.Name = "txtDisplayName";
      this.txtDisplayName.ReadOnly = true;
      this.txtDisplayName.Size = new Size(143, 20);
      this.txtDisplayName.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.pnlBackGround);
      this.Name = nameof (CalendarSearchControl);
      this.Size = new Size(394, 103);
      this.pnlBackGround.ResumeLayout(false);
      this.pnlBackGround.PerformLayout();
      ((ISupportInitialize) this.picAllow).EndInit();
      ((ISupportInitialize) this.picDisallow).EndInit();
      this.ResumeLayout(false);
    }

    public delegate void CalendarSearchEventHandler(
      object sender,
      CalendarSearchControl.CalendarSearchEventArgs e);

    public class CalendarSearchEventArgs : EventArgs
    {
      private UserInfo userInfo;
      private CSMessage.AccessLevel accessLevel;

      public UserInfo UserInfo => this.userInfo;

      public CSMessage.AccessLevel AccessLevel => this.accessLevel;

      public CalendarSearchEventArgs(UserInfo userInfo, CSMessage.AccessLevel accessLevel)
      {
        this.userInfo = userInfo;
        this.accessLevel = accessLevel;
      }
    }
  }
}

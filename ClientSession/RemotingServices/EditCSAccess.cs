// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.EditCSAccess
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class EditCSAccess : Form
  {
    private CSContactInfo contact;
    private bool removeContact;
    private IContainer components;
    private TextBox txtMsg;
    private RadioButton rbtFull;
    private RadioButton rbtPartial;
    private Label lblSub;
    private RadioButton rbtReadOnly;
    private Label lblQuestion;
    private Button btnCancel;
    private Button btnOK;
    private Label lblCurentAL;
    private Label lblUser;
    private Panel panel1;
    private Panel pnlAccessLevel;
    private Panel pnlDesc;
    private Label label1;
    private Label label2;

    public EditCSAccess(CSContactInfo contact, bool removeContact)
    {
      this.InitializeComponent();
      this.contact = contact;
      this.removeContact = removeContact;
      this.InitialPage();
    }

    private void InitialPage()
    {
      this.lblUser.Text = this.contact.UserFirstName + " " + this.contact.UserLastName;
      if (this.removeContact)
      {
        this.pnlAccessLevel.Visible = false;
        this.lblQuestion.Text = "Type a message explaining why you are removing this user's access to your calendar (optional):";
      }
      else
        this.lblQuestion.Text = "Type a message explaining why you are modifying this user's access to your calendar (optional):";
      switch (this.contact.AccessLevel)
      {
        case CSMessage.AccessLevel.ReadOnly:
          this.lblCurentAL.Text = " Read Only";
          this.rbtReadOnly.Enabled = false;
          break;
        case CSMessage.AccessLevel.Partial:
          this.lblCurentAL.Text = " Partial";
          this.rbtPartial.Enabled = false;
          break;
        case CSMessage.AccessLevel.Full:
          this.lblCurentAL.Text = " Full";
          this.rbtFull.Enabled = false;
          break;
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.checkExist())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected record has been deleted.  No action has been performed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        if (this.removeContact)
        {
          CSControlMessage csControlMessage = new CSControlMessage(Session.UserID, this.contact.UserID, CSMessage.MessageType.DeleteAccess, this.txtMsg.Text.Trim());
          Session.CalendarManager.DeleteContact(this.contact.OwnerID, this.contact.UserID);
          Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) csControlMessage);
        }
        else
        {
          CSControlMessage ackMsg = new CSControlMessage(Session.UserID, this.contact.UserID, CSMessage.MessageType.ModifyAccess, this.txtMsg.Text.Trim());
          CSMessage.AccessLevel accessLevel = CSMessage.AccessLevel.Pending;
          if (this.rbtFull.Checked)
            accessLevel = CSMessage.AccessLevel.Full;
          else if (this.rbtPartial.Checked)
            accessLevel = CSMessage.AccessLevel.Partial;
          else if (this.rbtReadOnly.Checked)
            accessLevel = CSMessage.AccessLevel.ReadOnly;
          if (accessLevel == CSMessage.AccessLevel.Pending)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Please select a new access level", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          Session.CalendarManager.UpdateContact(this.contact.OwnerID, this.contact.UserID, accessLevel, ackMsg);
          Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) ackMsg);
        }
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private bool checkExist()
    {
      return Session.CalendarManager.GetCalendarContactInfo(this.contact.OwnerID, this.contact.UserID) != null;
    }

    private void EditCSAccess_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtMsg = new TextBox();
      this.rbtFull = new RadioButton();
      this.rbtPartial = new RadioButton();
      this.lblSub = new Label();
      this.rbtReadOnly = new RadioButton();
      this.lblQuestion = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.lblCurentAL = new Label();
      this.lblUser = new Label();
      this.panel1 = new Panel();
      this.pnlDesc = new Panel();
      this.pnlAccessLevel = new Panel();
      this.label1 = new Label();
      this.label2 = new Label();
      this.panel1.SuspendLayout();
      this.pnlDesc.SuspendLayout();
      this.pnlAccessLevel.SuspendLayout();
      this.SuspendLayout();
      this.txtMsg.Dock = DockStyle.Fill;
      this.txtMsg.Location = new Point(0, 34);
      this.txtMsg.MaxLength = 512;
      this.txtMsg.Multiline = true;
      this.txtMsg.Name = "txtMsg";
      this.txtMsg.Size = new Size(424, 156);
      this.txtMsg.TabIndex = 14;
      this.rbtFull.AutoSize = true;
      this.rbtFull.Location = new Point(175, 20);
      this.rbtFull.Name = "rbtFull";
      this.rbtFull.Size = new Size(92, 17);
      this.rbtFull.TabIndex = 18;
      this.rbtFull.TabStop = true;
      this.rbtFull.Text = "Full (Add/Edit)";
      this.rbtFull.UseVisualStyleBackColor = true;
      this.rbtPartial.AutoSize = true;
      this.rbtPartial.Location = new Point(86, 20);
      this.rbtPartial.Name = "rbtPartial";
      this.rbtPartial.Size = new Size(82, 17);
      this.rbtPartial.TabIndex = 17;
      this.rbtPartial.TabStop = true;
      this.rbtPartial.Text = "Partial (Add)";
      this.rbtPartial.UseVisualStyleBackColor = true;
      this.lblSub.AutoSize = true;
      this.lblSub.Location = new Point(1, 4);
      this.lblSub.Name = "lblSub";
      this.lblSub.Size = new Size(96, 13);
      this.lblSub.TabIndex = 16;
      this.lblSub.Text = "New Access Level";
      this.rbtReadOnly.AutoSize = true;
      this.rbtReadOnly.Location = new Point(4, 20);
      this.rbtReadOnly.Name = "rbtReadOnly";
      this.rbtReadOnly.Size = new Size(75, 17);
      this.rbtReadOnly.TabIndex = 15;
      this.rbtReadOnly.TabStop = true;
      this.rbtReadOnly.Text = "Read Only";
      this.rbtReadOnly.UseVisualStyleBackColor = true;
      this.lblQuestion.Dock = DockStyle.Top;
      this.lblQuestion.Location = new Point(0, 0);
      this.lblQuestion.Name = "lblQuestion";
      this.lblQuestion.Size = new Size(424, 34);
      this.lblQuestion.TabIndex = 19;
      this.lblQuestion.Text = "Type a message explaining why you are modifying this user's access to your calendar (optional):";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(361, 284);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 13;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(280, 284);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 12;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.lblCurentAL.AutoSize = true;
      this.lblCurentAL.ForeColor = Color.Red;
      this.lblCurentAL.Location = new Point(132, 29);
      this.lblCurentAL.Name = "lblCurentAL";
      this.lblCurentAL.Size = new Size(53, 13);
      this.lblCurentAL.TabIndex = 20;
      this.lblCurentAL.Text = "currentAL";
      this.lblUser.AutoSize = true;
      this.lblUser.ForeColor = Color.Red;
      this.lblUser.Location = new Point(132, 9);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new Size(57, 13);
      this.lblUser.TabIndex = 21;
      this.lblUser.Text = "UserName";
      this.panel1.Controls.Add((Control) this.pnlDesc);
      this.panel1.Controls.Add((Control) this.pnlAccessLevel);
      this.panel1.Location = new Point(12, 48);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(424, 230);
      this.panel1.TabIndex = 22;
      this.pnlDesc.Controls.Add((Control) this.txtMsg);
      this.pnlDesc.Controls.Add((Control) this.lblQuestion);
      this.pnlDesc.Dock = DockStyle.Fill;
      this.pnlDesc.Location = new Point(0, 40);
      this.pnlDesc.Name = "pnlDesc";
      this.pnlDesc.Size = new Size(424, 190);
      this.pnlDesc.TabIndex = 1;
      this.pnlAccessLevel.Controls.Add((Control) this.rbtReadOnly);
      this.pnlAccessLevel.Controls.Add((Control) this.lblSub);
      this.pnlAccessLevel.Controls.Add((Control) this.rbtPartial);
      this.pnlAccessLevel.Controls.Add((Control) this.rbtFull);
      this.pnlAccessLevel.Dock = DockStyle.Top;
      this.pnlAccessLevel.Location = new Point(0, 0);
      this.pnlAccessLevel.Name = "pnlAccessLevel";
      this.pnlAccessLevel.Size = new Size(424, 40);
      this.pnlAccessLevel.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(63, 13);
      this.label1.TabIndex = 23;
      this.label1.Text = "User Name:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 29);
      this.label2.Name = "label2";
      this.label2.Size = new Size(111, 13);
      this.label2.TabIndex = 24;
      this.label2.Text = "Current Access Level:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(448, 319);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.lblUser);
      this.Controls.Add((Control) this.lblCurentAL);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EditCSAccess);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Calendar sharing access";
      this.KeyUp += new KeyEventHandler(this.EditCSAccess_KeyUp);
      this.panel1.ResumeLayout(false);
      this.pnlDesc.ResumeLayout(false);
      this.pnlDesc.PerformLayout();
      this.pnlAccessLevel.ResumeLayout(false);
      this.pnlAccessLevel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

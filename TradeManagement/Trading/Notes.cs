// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.Notes
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class Notes : Form
  {
    private IContainer components;
    private Label lblDataStamp;
    private Label lblDeatils;
    private Label lblUser;
    private TextBox txtDataStamp;
    private TextBox txtUser;
    private TextBox txtDetails;
    private Button btnOK;
    private Button btnCancel;

    public string noteDetails { get; set; }

    public string userName { get; set; }

    public string notesOpreation { get; set; }

    public string notesDateTime { get; set; }

    public Notes()
    {
      this.noteDetails = string.Empty;
      this.InitializeComponent();
    }

    public Notes(
      string NotesDetails,
      string UserName,
      string NotesOpreation,
      string NotesDateTime)
      : this()
    {
      this.noteDetails = NotesDetails;
      this.userName = UserName;
      this.notesOpreation = NotesOpreation;
      this.notesDateTime = NotesDateTime;
    }

    private void Notes_Load(object sender, EventArgs e)
    {
      this.txtDataStamp.Text = this.notesDateTime == null ? Utils.CreateTimestamp(true) : this.notesDateTime;
      this.txtUser.Text = Session.UserInfo.FullName;
      this.txtDetails.Text = this.noteDetails;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      IWin32Window owner = (IWin32Window) null;
      if (string.IsNullOrWhiteSpace(this.txtDetails.Text.Trim()))
      {
        int num = (int) Utils.Dialog(owner, "The Notes Details cannot be blank.");
      }
      else
      {
        this.noteDetails = this.txtDetails.Text.Trim();
        this.notesDateTime = this.txtDataStamp.Text;
        this.notesOpreation = "ok";
        this.Close();
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.notesOpreation = "Cancel";
      this.Close();
    }

    private void Notes_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!(this.notesOpreation != "ok"))
        return;
      this.notesOpreation = "Cancel";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblDataStamp = new Label();
      this.lblDeatils = new Label();
      this.lblUser = new Label();
      this.txtDataStamp = new TextBox();
      this.txtUser = new TextBox();
      this.txtDetails = new TextBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.lblDataStamp.AutoSize = true;
      this.lblDataStamp.Location = new Point(31, 26);
      this.lblDataStamp.Name = "lblDataStamp";
      this.lblDataStamp.Size = new Size(56, 13);
      this.lblDataStamp.TabIndex = 0;
      this.lblDataStamp.Text = "Date Time";
      this.lblDeatils.AutoSize = true;
      this.lblDeatils.Location = new Point(48, 53);
      this.lblDeatils.Name = "lblDeatils";
      this.lblDeatils.Size = new Size(39, 13);
      this.lblDeatils.TabIndex = 1;
      this.lblDeatils.Text = "Details";
      this.lblUser.AutoSize = true;
      this.lblUser.Location = new Point(58, 115);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new Size(29, 13);
      this.lblUser.TabIndex = 2;
      this.lblUser.Text = "User";
      this.txtDataStamp.Enabled = false;
      this.txtDataStamp.Location = new Point(96, 23);
      this.txtDataStamp.Name = "txtDataStamp";
      this.txtDataStamp.Size = new Size(147, 20);
      this.txtDataStamp.TabIndex = 3;
      this.txtUser.Enabled = false;
      this.txtUser.Location = new Point(96, 112);
      this.txtUser.Name = "txtUser";
      this.txtUser.Size = new Size(147, 20);
      this.txtUser.TabIndex = 4;
      this.txtDetails.Location = new Point(96, 50);
      this.txtDetails.Multiline = true;
      this.txtDetails.Name = "txtDetails";
      this.txtDetails.Size = new Size(391, 56);
      this.txtDetails.TabIndex = 5;
      this.btnOK.BackColor = SystemColors.Control;
      this.btnOK.Location = new Point(96, 145);
      this.btnOK.Margin = new Padding(0);
      this.btnOK.Name = "btnOK";
      this.btnOK.Padding = new Padding(2, 0, 0, 0);
      this.btnOK.Size = new Size(44, 22);
      this.btnOK.TabIndex = 131;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.Location = new Point(167, 145);
      this.btnCancel.Margin = new Padding(0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Padding = new Padding(2, 0, 0, 0);
      this.btnCancel.Size = new Size(50, 22);
      this.btnCancel.TabIndex = 132;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(499, 185);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtDetails);
      this.Controls.Add((Control) this.txtUser);
      this.Controls.Add((Control) this.txtDataStamp);
      this.Controls.Add((Control) this.lblUser);
      this.Controls.Add((Control) this.lblDeatils);
      this.Controls.Add((Control) this.lblDataStamp);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = nameof (Notes);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (Notes);
      this.FormClosing += new FormClosingEventHandler(this.Notes_FormClosing);
      this.Load += new EventHandler(this.Notes_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

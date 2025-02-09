// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.LoanReassignmentForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class LoanReassignmentForm : Form
  {
    private Sessions.Session session;
    private ExternalUserInfo deleteExtUser;
    private ExternalUserInfo selectedUser;
    private List<ExternalUserInfo> allLOLPUsers;
    private List<int> personaIds;
    private IContainer components;
    private Label label1;
    private Button button1;
    private Button button2;
    private RadioButton radioButton1;
    private RadioButton radioButton2;
    private StandardIconButton standardIconButton1;
    private Panel panel1;
    private Panel panel2;
    private Panel panel3;
    private Panel panel4;
    private TextBox textBox1;

    public LoanReassignmentForm(
      Sessions.Session session,
      ExternalUserInfo deleteExtUser,
      List<ExternalUserInfo> allLOLPUsers)
      : this(session, deleteExtUser, allLOLPUsers, (List<int>) null)
    {
    }

    public LoanReassignmentForm(
      Sessions.Session session,
      ExternalUserInfo deleteExtUser,
      List<ExternalUserInfo> allLOLPUsers,
      List<int> personaIds)
    {
      this.session = session;
      this.deleteExtUser = deleteExtUser;
      this.allLOLPUsers = allLOLPUsers;
      this.InitializeComponent();
      this.standardIconButton1.Enabled = false;
      this.button1.Enabled = false;
      this.personaIds = personaIds;
    }

    private void standardIconButton1_Click(object sender, EventArgs e)
    {
      using (ExternalUsersList externalUsersList = new ExternalUsersList(this.session, this.deleteExtUser, this.allLOLPUsers, this.personaIds))
      {
        if (externalUsersList.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.selectedUser = externalUsersList.getSelectedUser();
        this.textBox1.Text = this.selectedUser.FirstName + " " + this.selectedUser.LastName;
      }
    }

    public ExternalUserInfo getSelectedUser() => this.selectedUser;

    private void button1_Click(object sender, EventArgs e)
    {
      if (!this.radioButton1.Checked && !this.radioButton2.Checked)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select one option before proceeding.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.radioButton1.Checked && (UserInfo) this.selectedUser == (UserInfo) null)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select a user to reassign the loans to.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void radioButton1_CheckedChanged(object sender, EventArgs e)
    {
      if (this.radioButton1.Checked)
        this.radioButton2.Checked = false;
      this.standardIconButton1.Enabled = true;
      this.button1.Enabled = true;
    }

    private void radioButton2_CheckedChanged(object sender, EventArgs e)
    {
      if (this.radioButton2.Checked)
        this.radioButton1.Checked = false;
      this.standardIconButton1.Enabled = false;
      this.button1.Enabled = true;
      this.selectedUser = (ExternalUserInfo) null;
      this.textBox1.Text = "";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.button1 = new Button();
      this.button2 = new Button();
      this.radioButton1 = new RadioButton();
      this.radioButton2 = new RadioButton();
      this.standardIconButton1 = new StandardIconButton();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.panel3 = new Panel();
      this.panel4 = new Panel();
      this.textBox1 = new TextBox();
      ((ISupportInitialize) this.standardIconButton1).BeginInit();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel4.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 19);
      this.label1.Name = "label1";
      this.label1.Size = new Size(340, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Before you proceed, reassign the loans that were assigned to this user.";
      this.button1.Location = new Point(196, 96);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 4;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(277, 96);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 5;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.radioButton1.AutoSize = true;
      this.radioButton1.Dock = DockStyle.Left;
      this.radioButton1.Location = new Point(0, 0);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new Size(84, 20);
      this.radioButton1.TabIndex = 7;
      this.radioButton1.TabStop = true;
      this.radioButton1.Text = "Reassign to:";
      this.radioButton1.UseVisualStyleBackColor = true;
      this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
      this.radioButton2.AutoSize = true;
      this.radioButton2.Location = new Point(0, 26);
      this.radioButton2.Name = "radioButton2";
      this.radioButton2.Size = new Size(92, 17);
      this.radioButton2.TabIndex = 8;
      this.radioButton2.TabStop = true;
      this.radioButton2.Text = "Don't reassign";
      this.radioButton2.UseVisualStyleBackColor = true;
      this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
      this.standardIconButton1.BackColor = Color.Transparent;
      this.standardIconButton1.Location = new Point(240, 0);
      this.standardIconButton1.MouseDownImage = (Image) null;
      this.standardIconButton1.Name = "standardIconButton1";
      this.standardIconButton1.Padding = new Padding(0, 2, 0, 0);
      this.standardIconButton1.Size = new Size(16, 20);
      this.standardIconButton1.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.standardIconButton1.TabIndex = 6;
      this.standardIconButton1.TabStop = false;
      this.standardIconButton1.Click += new EventHandler(this.standardIconButton1_Click);
      this.panel1.AutoSize = true;
      this.panel1.Dock = DockStyle.Left;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(0, 55);
      this.panel1.TabIndex = 9;
      this.panel2.Controls.Add((Control) this.panel3);
      this.panel2.Controls.Add((Control) this.panel1);
      this.panel2.Location = new Point(15, 35);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(337, 55);
      this.panel2.TabIndex = 10;
      this.panel3.Controls.Add((Control) this.panel4);
      this.panel3.Controls.Add((Control) this.radioButton2);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(337, 55);
      this.panel3.TabIndex = 10;
      this.panel4.Controls.Add((Control) this.textBox1);
      this.panel4.Controls.Add((Control) this.standardIconButton1);
      this.panel4.Controls.Add((Control) this.radioButton1);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(0, 0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(337, 20);
      this.panel4.TabIndex = 7;
      this.textBox1.Location = new Point(84, 0);
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new Size(144, 20);
      this.textBox1.TabIndex = 8;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.button2;
      this.ClientSize = new Size(370, 135);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanReassignmentForm);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loans Reassignment Form";
      ((ISupportInitialize) this.standardIconButton1).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

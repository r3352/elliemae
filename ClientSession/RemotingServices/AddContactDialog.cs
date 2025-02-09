// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.AddContactDialog
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class AddContactDialog : Form
  {
    private static int _maxMsgLength = 512;
    private Button btnCancel;
    private Button btnOK;
    private RichTextBox rtBoxMsg;
    private Label label6;
    private TextBox txtBoxLastName;
    private TextBox txtBoxFirstName;
    private Label label5;
    private Label label4;
    private Label label3;
    private TextBox txtBoxUserid;
    private Label label2;
    private Label label1;
    private Label label7;
    private ComboBox cmbBoxToGroup;
    private System.ComponentModel.Container components;

    public AddContactDialog(string[] groups, int selectedIdx)
      : this((string) null, groups, selectedIdx)
    {
    }

    public AddContactDialog(string contactUserid, string[] groups, int selectedIdx)
    {
      this.InitializeComponent();
      if (groups != null && groups.Length != 0)
      {
        this.cmbBoxToGroup.Items.AddRange((object[]) groups);
        if (selectedIdx >= 0 && selectedIdx < groups.Length)
          this.cmbBoxToGroup.SelectedIndex = selectedIdx;
        else
          this.cmbBoxToGroup.SelectedIndex = -1;
      }
      if (contactUserid != null)
      {
        this.txtBoxUserid.Text = contactUserid;
        this.txtBoxUserid.ReadOnly = true;
        this.cmbBoxToGroup.Select();
        this.cmbBoxToGroup.Focus();
      }
      else
        this.btnOK.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.rtBoxMsg = new RichTextBox();
      this.label6 = new Label();
      this.txtBoxLastName = new TextBox();
      this.txtBoxFirstName = new TextBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.txtBoxUserid = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.label7 = new Label();
      this.cmbBoxToGroup = new ComboBox();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(261, 321);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(345, 321);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.rtBoxMsg.Location = new Point(16, 232);
      this.rtBoxMsg.Name = "rtBoxMsg";
      this.rtBoxMsg.Size = new Size(404, 76);
      this.rtBoxMsg.TabIndex = 5;
      this.rtBoxMsg.Text = "";
      this.rtBoxMsg.TextChanged += new EventHandler(this.rtBoxMsg_TextChanged);
      this.label6.Location = new Point(16, 188);
      this.label6.Name = "label6";
      this.label6.Size = new Size(416, 36);
      this.label6.TabIndex = 104;
      this.label6.Text = "A message will be sent asking this user to approve your request to add him/her to your Messenger List (optional)";
      this.txtBoxLastName.Location = new Point(288, 144);
      this.txtBoxLastName.Name = "txtBoxLastName";
      this.txtBoxLastName.Size = new Size(132, 20);
      this.txtBoxLastName.TabIndex = 4;
      this.txtBoxFirstName.Location = new Point(80, 144);
      this.txtBoxFirstName.Name = "txtBoxFirstName";
      this.txtBoxFirstName.Size = new Size(132, 20);
      this.txtBoxFirstName.TabIndex = 3;
      this.label5.Location = new Point(224, 144);
      this.label5.Name = "label5";
      this.label5.Size = new Size(68, 23);
      this.label5.TabIndex = 103;
      this.label5.Text = "Last Name";
      this.label4.Location = new Point(16, 144);
      this.label4.Name = "label4";
      this.label4.Size = new Size(64, 23);
      this.label4.TabIndex = 102;
      this.label4.Text = "First Name";
      this.label3.Location = new Point(16, 96);
      this.label3.Name = "label3";
      this.label3.Size = new Size(416, 40);
      this.label3.TabIndex = 101;
      this.label3.Text = "You can also display the contact's first and/or last name in your Messenger List (optional)";
      this.txtBoxUserid.CharacterCasing = CharacterCasing.Lower;
      this.txtBoxUserid.Location = new Point(96, 32);
      this.txtBoxUserid.Name = "txtBoxUserid";
      this.txtBoxUserid.Size = new Size(324, 20);
      this.txtBoxUserid.TabIndex = 1;
      this.txtBoxUserid.TextChanged += new EventHandler(this.txtBoxUserid_TextChanged);
      this.label2.Location = new Point(16, 32);
      this.label2.Name = "label2";
      this.label2.Size = new Size(80, 23);
      this.label2.TabIndex = 11;
      this.label2.Text = "User ID";
      this.label1.Location = new Point(16, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(416, 23);
      this.label1.TabIndex = 100;
      this.label1.Text = "To add a contact to your Messenger List, enter the following information";
      this.label7.Location = new Point(16, 56);
      this.label7.Name = "label7";
      this.label7.Size = new Size(80, 23);
      this.label7.TabIndex = 105;
      this.label7.Text = "Add to group";
      this.cmbBoxToGroup.Location = new Point(96, 56);
      this.cmbBoxToGroup.Name = "cmbBoxToGroup";
      this.cmbBoxToGroup.Size = new Size(328, 21);
      this.cmbBoxToGroup.TabIndex = 2;
      this.cmbBoxToGroup.KeyPress += new KeyPressEventHandler(this.cmbBoxToGroup_KeyPress);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(440, 356);
      this.Controls.Add((Control) this.cmbBoxToGroup);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.rtBoxMsg);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.txtBoxLastName);
      this.Controls.Add((Control) this.txtBoxFirstName);
      this.Controls.Add((Control) this.txtBoxUserid);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddContactDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add to Messenger List";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void txtBoxUserid_TextChanged(object sender, EventArgs e)
    {
      if (this.txtBoxUserid.Text.Trim() == "")
        this.btnOK.Enabled = false;
      else
        this.btnOK.Enabled = true;
    }

    private void rtBoxMsg_TextChanged(object sender, EventArgs e)
    {
      if (this.rtBoxMsg.Text.Length <= AddContactDialog._maxMsgLength)
        return;
      this.rtBoxMsg.Text = this.rtBoxMsg.Text.Substring(0, AddContactDialog._maxMsgLength);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.cmbBoxToGroup.SelectedIndex < 0 && this.cmbBoxToGroup.Text.Trim() == "")
      {
        this.DialogResult = DialogResult.None;
        int num = (int) Utils.Dialog((IWin32Window) this, "Group name cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void cmbBoxToGroup_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.cmbBoxToGroup.Text.Length < 64 || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    public string Userid => this.txtBoxUserid.Text.Trim();

    public string FirstName => this.txtBoxFirstName.Text.Trim();

    public string LastName => this.txtBoxLastName.Text.Trim();

    public string Message => this.rtBoxMsg.Text;

    public string NewGroupName
    {
      get
      {
        if (this.cmbBoxToGroup.SelectedIndex >= 0)
          return (string) null;
        for (int index = 0; index < this.cmbBoxToGroup.Items.Count; ++index)
        {
          if (string.Compare(this.cmbBoxToGroup.Text.Trim(), this.cmbBoxToGroup.Items[index].ToString().Trim(), StringComparison.OrdinalIgnoreCase) == 0)
            return (string) null;
        }
        return this.cmbBoxToGroup.Text.Trim();
      }
    }

    public string SelectedGroupName
    {
      get
      {
        if (this.cmbBoxToGroup.SelectedIndex >= 0)
          return this.cmbBoxToGroup.SelectedItem.ToString().Trim();
        for (int index = 0; index < this.cmbBoxToGroup.Items.Count; ++index)
        {
          if (string.Compare(this.cmbBoxToGroup.Text.Trim(), this.cmbBoxToGroup.Items[index].ToString().Trim(), StringComparison.OrdinalIgnoreCase) == 0)
            return this.cmbBoxToGroup.Items[index].ToString().Trim();
        }
        return (string) null;
      }
    }
  }
}

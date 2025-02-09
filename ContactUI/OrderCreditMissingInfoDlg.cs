// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.OrderCreditMissingInfoDlg
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class OrderCreditMissingInfoDlg : Form
  {
    private BorrowerInfo contact;
    private bool initialLoad;
    private bool intermidiateData;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label12;
    private Button btnContinue;
    private Button btnCancel;
    private Label label5;
    private TextBox txtBoxFirstName;
    private TextBox txtBoxLastName;
    private TextBox txtBoxSSN;
    private System.ComponentModel.Container components;

    public OrderCreditMissingInfoDlg(BorrowerInfo contact)
    {
      this.InitializeComponent();
      this.initialLoad = true;
      this.contact = contact;
      this.txtBoxFirstName.Text = contact.FirstName;
      this.txtBoxLastName.Text = contact.LastName;
      this.txtBoxSSN.Text = contact.SSN;
      this.initialLoad = false;
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
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.txtBoxFirstName = new TextBox();
      this.txtBoxLastName = new TextBox();
      this.label12 = new Label();
      this.btnContinue = new Button();
      this.btnCancel = new Button();
      this.txtBoxSSN = new TextBox();
      this.label5 = new Label();
      this.SuspendLayout();
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(120, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Co-Borrower Contact";
      this.label2.Location = new Point(12, 32);
      this.label2.Name = "label2";
      this.label2.Size = new Size(336, 32);
      this.label2.TabIndex = 1;
      this.label2.Text = "In order to order a credit report, you need to fill out the contact's first name, last name and social security number. ";
      this.label3.Location = new Point(12, 72);
      this.label3.Name = "label3";
      this.label3.Size = new Size(72, 16);
      this.label3.TabIndex = 2;
      this.label3.Text = "First Name:";
      this.label4.Location = new Point(12, 96);
      this.label4.Name = "label4";
      this.label4.Size = new Size(72, 16);
      this.label4.TabIndex = 3;
      this.label4.Text = "Last Name:";
      this.txtBoxFirstName.Location = new Point(84, 68);
      this.txtBoxFirstName.MaxLength = 50;
      this.txtBoxFirstName.Name = "txtBoxFirstName";
      this.txtBoxFirstName.Size = new Size(216, 20);
      this.txtBoxFirstName.TabIndex = 4;
      this.txtBoxFirstName.Text = "";
      this.txtBoxLastName.Location = new Point(84, 96);
      this.txtBoxLastName.MaxLength = 50;
      this.txtBoxLastName.Name = "txtBoxLastName";
      this.txtBoxLastName.Size = new Size(216, 20);
      this.txtBoxLastName.TabIndex = 5;
      this.txtBoxLastName.Text = "";
      this.label12.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label12.BorderStyle = BorderStyle.Fixed3D;
      this.label12.Location = new Point(8, 160);
      this.label12.Name = "label12";
      this.label12.Size = new Size(328, 2);
      this.label12.TabIndex = 16;
      this.btnContinue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnContinue.DialogResult = DialogResult.OK;
      this.btnContinue.Location = new Point(192, 176);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(68, 22);
      this.btnContinue.TabIndex = 19;
      this.btnContinue.Text = "OK";
      this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(268, 176);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(68, 22);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.txtBoxSSN.Location = new Point(84, 124);
      this.txtBoxSSN.MaxLength = 12;
      this.txtBoxSSN.Name = "txtBoxSSN";
      this.txtBoxSSN.Size = new Size(216, 20);
      this.txtBoxSSN.TabIndex = 22;
      this.txtBoxSSN.Text = "";
      this.txtBoxSSN.TextChanged += new EventHandler(this.controlChanged);
      this.label5.Location = new Point(12, 124);
      this.label5.Name = "label5";
      this.label5.Size = new Size(72, 16);
      this.label5.TabIndex = 21;
      this.label5.Text = "SSN:";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(354, 211);
      this.Controls.Add((Control) this.txtBoxSSN);
      this.Controls.Add((Control) this.txtBoxLastName);
      this.Controls.Add((Control) this.txtBoxFirstName);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (OrderCreditMissingInfoDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Order Credit";
      this.ResumeLayout(false);
    }

    private void btnContinue_Click(object sender, EventArgs e)
    {
      if (this.txtBoxFirstName.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The contact's first name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.txtBoxFirstName.Focus();
      }
      else if (this.txtBoxLastName.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The contact's last name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.txtBoxLastName.Focus();
      }
      else if (this.txtBoxSSN.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The contact's social security number cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.txtBoxSSN.Focus();
      }
      else
      {
        this.contact.FirstName = this.txtBoxFirstName.Text.Trim();
        this.contact.LastName = this.txtBoxLastName.Text.Trim();
        this.contact.SSN = this.txtBoxSSN.Text.Trim();
        Session.ContactManager.UpdateBorrower(this.contact);
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void controlChanged(object sender, EventArgs e)
    {
      if (this.initialLoad)
        return;
      this.formatText(sender, e);
    }

    public void formatText(object sender, EventArgs e)
    {
      if (this.intermidiateData)
      {
        this.intermidiateData = false;
      }
      else
      {
        FieldFormat dataFormat = FieldFormat.SSN;
        TextBox textBox = (TextBox) sender;
        bool needsUpdate = false;
        int newCursorPos = 0;
        string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate, textBox.SelectionStart, ref newCursorPos);
        if (!needsUpdate)
          return;
        this.intermidiateData = true;
        textBox.Text = str;
        textBox.SelectionStart = newCursorPos;
      }
    }
  }
}

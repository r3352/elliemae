// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.LoanFolderSelectionForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class LoanFolderSelectionForm : Form
  {
    private ListBox listBoxLoanFolders;
    private Label label1;
    private Button btnOK;
    private Button btnCancel;
    private System.ComponentModel.Container components;
    private string _SelectedLoanFolder = string.Empty;

    public LoanFolderSelectionForm() => this.InitializeComponent();

    public LoanFolderSelectionForm(string[] loanFolders)
    {
      this.InitializeComponent();
      this.listBoxLoanFolders.Items.Clear();
      this.listBoxLoanFolders.Items.AddRange((object[]) loanFolders);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.listBoxLoanFolders = new ListBox();
      this.label1 = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.listBoxLoanFolders.Location = new Point(12, 60);
      this.listBoxLoanFolders.Name = "listBoxLoanFolders";
      this.listBoxLoanFolders.Size = new Size(216, 173);
      this.listBoxLoanFolders.TabIndex = 1;
      this.label1.Location = new Point(16, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(212, 32);
      this.label1.TabIndex = 0;
      this.label1.Text = "You must select a destination folder for the loan.";
      this.btnOK.Location = new Point(28, 244);
      this.btnOK.Name = "btnOK";
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(132, 244);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(248, 273);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.listBoxLoanFolders);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanFolderSelectionForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select a Loan Folder";
      this.ResumeLayout(false);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.listBoxLoanFolders.SelectedItem == null)
      {
        int num = (int) MessageBox.Show("Please choose a loan folder in which you would like to put the originated loan.");
      }
      else
      {
        this._SelectedLoanFolder = this.listBoxLoanFolders.SelectedItem.ToString();
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    public string SelectedLoanFolder => this._SelectedLoanFolder;
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.MatchedLoansForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class MatchedLoansForm : Form
  {
    public static string[] RequiredFields = new string[5]
    {
      "Loan.Guid",
      "Loan.BorrowerFirstName",
      "Loan.BorrowerLastName",
      "Loan.DateFileOpened",
      "Loan.LoanFolder"
    };
    private ListView listView1;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private Label label1;
    private Button btnSelect;
    private Button btnCancel;
    private Button btnCreateNew;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private System.ComponentModel.Container components;
    private LoanIdentity loanIdentity;

    public MatchedLoansForm(PipelineInfo[] pinfos)
    {
      this.InitializeComponent();
      this.listView1.Items.Clear();
      for (int index = 0; index < pinfos.Length; ++index)
        this.listView1.Items.Add(new ListViewItem(new string[4]
        {
          string.Concat(pinfos[index].GetField("BorrowerFirstName")),
          string.Concat(pinfos[index].GetField("BorrowerLastName")),
          Utils.ParseDate(pinfos[index].GetField("DateFileOpened")).ToString("MM/dd/yyyy"),
          string.Concat(pinfos[index].GetField("LoanFolder"))
        })
        {
          Tag = (object) pinfos[index].Identity
        });
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.listView1 = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.label1 = new Label();
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.btnCreateNew = new Button();
      this.SuspendLayout();
      this.listView1.Columns.AddRange(new ColumnHeader[4]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4
      });
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.Location = new Point(12, 44);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.Size = new Size(532, 172);
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.columnHeader1.Text = "Borrower First Name";
      this.columnHeader1.Width = 146;
      this.columnHeader2.Text = "Borrower Last Name";
      this.columnHeader2.Width = 147;
      this.columnHeader3.Text = "Date Loan Created";
      this.columnHeader3.Width = 115;
      this.columnHeader4.Text = "Loan Folder";
      this.columnHeader4.Width = 120;
      this.label1.Location = new Point(12, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(532, 32);
      this.label1.TabIndex = 1;
      this.label1.Text = "The contact might already have the following loans created. Please select one of the loans listed below or create a new loan.";
      this.btnSelect.Location = new Point(388, 228);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 24);
      this.btnSelect.TabIndex = 2;
      this.btnSelect.Text = "Select";
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(469, 228);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCreateNew.DialogResult = DialogResult.OK;
      this.btnCreateNew.Location = new Point(307, 228);
      this.btnCreateNew.Name = "btnCreateNew";
      this.btnCreateNew.Size = new Size(75, 24);
      this.btnCreateNew.TabIndex = 4;
      this.btnCreateNew.Text = "Create New";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(554, 264);
      this.Controls.Add((Control) this.btnCreateNew);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.listView1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MatchedLoansForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Existing Loans";
      this.KeyUp += new KeyEventHandler(this.MatchedLoansForm_KeyUp);
      this.ResumeLayout(false);
    }

    public LoanIdentity LoanID => this.loanIdentity;

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.listView1.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a loan file from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.loanIdentity = (LoanIdentity) this.listView1.SelectedItems[0].Tag;
        this.DialogResult = DialogResult.Yes;
        this.Close();
      }
    }

    private void MatchedLoansForm_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }
  }
}

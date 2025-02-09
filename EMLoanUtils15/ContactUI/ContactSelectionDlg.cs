// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactSelectionDlg
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactSelectionDlg : Form
  {
    private System.ComponentModel.Container components;
    private Button btnNext;
    private Button btnCancel;
    private Panel panel2;
    private ListView listViewCoBor;
    private RadioButton rbtnUpdateCoBor;
    private RadioButton rbtnCreateCoBor;
    private ColumnHeader columnHeader6;
    private ColumnHeader columnHeader7;
    private ColumnHeader columnHeader8;
    private Panel panel1;
    private ListView listViewBor;
    private RadioButton rbtnCreateBor;
    private RadioButton rbtnUpdateBor;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private BorrowerInfo[] borrowers;
    private CheckBox chkBoxUpdateAddress;
    private BorrowerInfo[] coborrowers;
    private bool hasCoborrower = true;
    public bool CreateBorrower;
    public bool CreateCoBorrower;
    public BorrowerInfo SelectedBorrower;
    public BorrowerInfo SelectedCoBorrower;
    public bool UpdateAddress;

    public ContactSelectionDlg(
      BorrowerInfo[] borrowers,
      BorrowerInfo[] coborrowers,
      bool allowAddressUpdate,
      bool showCreateNew,
      bool hasCoborrower,
      bool allowUpdateOptOut)
    {
      this.InitializeComponent();
      this.borrowers = borrowers;
      this.coborrowers = coborrowers;
      this.hasCoborrower = hasCoborrower;
      if (allowAddressUpdate)
      {
        this.chkBoxUpdateAddress.Checked = true;
        this.chkBoxUpdateAddress.Enabled = true;
      }
      else
      {
        this.chkBoxUpdateAddress.Checked = false;
        this.chkBoxUpdateAddress.Enabled = false;
      }
      ListViewItem[] items1 = new ListViewItem[borrowers.Length];
      for (int index = 0; index < borrowers.Length; ++index)
        items1[index] = this.getViewItemFromContact(borrowers[index]);
      this.listViewBor.Items.AddRange(items1);
      ListViewItem[] items2 = new ListViewItem[coborrowers.Length];
      for (int index = 0; index < coborrowers.Length; ++index)
        items2[index] = this.getViewItemFromContact(coborrowers[index]);
      this.listViewCoBor.Items.AddRange(items2);
      if (borrowers.Length != 0)
      {
        this.rbtnUpdateBor.Checked = true;
        this.rbtnUpdateBor.Enabled = true;
        this.listViewBor.Enabled = true;
        this.listViewBor.Items[0].Selected = true;
      }
      else
      {
        this.rbtnCreateBor.Checked = true;
        this.rbtnUpdateBor.Enabled = false;
        this.listViewBor.Enabled = false;
      }
      if (coborrowers.Length != 0)
      {
        this.rbtnUpdateCoBor.Checked = true;
        this.rbtnUpdateCoBor.Enabled = true;
        this.listViewCoBor.Enabled = true;
        this.listViewCoBor.Items[0].Selected = true;
      }
      else
      {
        this.rbtnCreateCoBor.Checked = true;
        this.rbtnUpdateCoBor.Enabled = false;
        this.listViewCoBor.Enabled = false;
      }
      if (!hasCoborrower)
      {
        this.rbtnCreateCoBor.Checked = false;
        this.rbtnUpdateCoBor.Checked = false;
        this.rbtnCreateCoBor.Enabled = false;
        this.rbtnUpdateCoBor.Enabled = false;
        this.listViewCoBor.Enabled = false;
      }
      if (!showCreateNew)
      {
        this.rbtnCreateCoBor.Enabled = false;
        this.rbtnCreateCoBor.Checked = false;
        this.rbtnCreateBor.Enabled = false;
        this.rbtnCreateBor.Checked = false;
      }
      if (!this.rbtnCreateBor.Enabled && !this.rbtnCreateCoBor.Enabled && !this.rbtnUpdateBor.Enabled && !this.rbtnUpdateCoBor.Enabled)
        this.btnNext.Enabled = false;
      else
        this.btnNext.Enabled = true;
      if (allowUpdateOptOut)
        return;
      this.btnCancel.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnNext = new Button();
      this.btnCancel = new Button();
      this.panel2 = new Panel();
      this.listViewCoBor = new ListView();
      this.columnHeader6 = new ColumnHeader();
      this.columnHeader7 = new ColumnHeader();
      this.columnHeader8 = new ColumnHeader();
      this.rbtnUpdateCoBor = new RadioButton();
      this.rbtnCreateCoBor = new RadioButton();
      this.panel1 = new Panel();
      this.listViewBor = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.rbtnCreateBor = new RadioButton();
      this.rbtnUpdateBor = new RadioButton();
      this.chkBoxUpdateAddress = new CheckBox();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnNext.Location = new Point(428, 372);
      this.btnNext.Name = "btnNext";
      this.btnNext.TabIndex = 6;
      this.btnNext.Text = "OK";
      this.btnNext.Click += new EventHandler(this.btnNext_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(516, 372);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.panel2.BorderStyle = BorderStyle.Fixed3D;
      this.panel2.Controls.Add((Control) this.listViewCoBor);
      this.panel2.Controls.Add((Control) this.rbtnUpdateCoBor);
      this.panel2.Controls.Add((Control) this.rbtnCreateCoBor);
      this.panel2.Location = new Point(3, 185);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(604, 172);
      this.panel2.TabIndex = 11;
      this.listViewCoBor.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader6,
        this.columnHeader7,
        this.columnHeader8
      });
      this.listViewCoBor.FullRowSelect = true;
      this.listViewCoBor.GridLines = true;
      this.listViewCoBor.HideSelection = false;
      this.listViewCoBor.Location = new Point(8, 52);
      this.listViewCoBor.MultiSelect = false;
      this.listViewCoBor.Name = "listViewCoBor";
      this.listViewCoBor.Size = new Size(584, 112);
      this.listViewCoBor.TabIndex = 3;
      this.listViewCoBor.View = View.Details;
      this.columnHeader6.Text = "Last Name";
      this.columnHeader6.Width = 119;
      this.columnHeader7.Text = "First Name";
      this.columnHeader7.Width = 95;
      this.columnHeader8.Text = "Current Home Address";
      this.columnHeader8.Width = 365;
      this.rbtnUpdateCoBor.Location = new Point(8, 28);
      this.rbtnUpdateCoBor.Name = "rbtnUpdateCoBor";
      this.rbtnUpdateCoBor.Size = new Size(580, 24);
      this.rbtnUpdateCoBor.TabIndex = 5;
      this.rbtnUpdateCoBor.Text = "Update the contact history for this co-borrower.";
      this.rbtnUpdateCoBor.CheckedChanged += new EventHandler(this.rbtnUpdateCoBor_CheckedChanged);
      this.rbtnCreateCoBor.Checked = true;
      this.rbtnCreateCoBor.Location = new Point(8, 4);
      this.rbtnCreateCoBor.Name = "rbtnCreateCoBor";
      this.rbtnCreateCoBor.Size = new Size(580, 24);
      this.rbtnCreateCoBor.TabIndex = 4;
      this.rbtnCreateCoBor.TabStop = true;
      this.rbtnCreateCoBor.Text = "Create a new Borrower Contact for this co-borrower.";
      this.panel1.BorderStyle = BorderStyle.Fixed3D;
      this.panel1.Controls.Add((Control) this.listViewBor);
      this.panel1.Controls.Add((Control) this.rbtnCreateBor);
      this.panel1.Controls.Add((Control) this.rbtnUpdateBor);
      this.panel1.Location = new Point(3, 1);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(604, 172);
      this.panel1.TabIndex = 10;
      this.listViewBor.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3
      });
      this.listViewBor.FullRowSelect = true;
      this.listViewBor.GridLines = true;
      this.listViewBor.HideSelection = false;
      this.listViewBor.Location = new Point(8, 52);
      this.listViewBor.MultiSelect = false;
      this.listViewBor.Name = "listViewBor";
      this.listViewBor.Size = new Size(584, 112);
      this.listViewBor.TabIndex = 0;
      this.listViewBor.View = View.Details;
      this.columnHeader1.Text = "Last Name";
      this.columnHeader1.Width = 119;
      this.columnHeader2.Text = "First Name";
      this.columnHeader2.Width = 95;
      this.columnHeader3.Text = "Current Home Address";
      this.columnHeader3.Width = 364;
      this.rbtnCreateBor.Checked = true;
      this.rbtnCreateBor.Location = new Point(8, 4);
      this.rbtnCreateBor.Name = "rbtnCreateBor";
      this.rbtnCreateBor.Size = new Size(580, 24);
      this.rbtnCreateBor.TabIndex = 1;
      this.rbtnCreateBor.TabStop = true;
      this.rbtnCreateBor.Text = "Create a new Borrower Contact for this borrower.";
      this.rbtnUpdateBor.Location = new Point(8, 28);
      this.rbtnUpdateBor.Name = "rbtnUpdateBor";
      this.rbtnUpdateBor.Size = new Size(580, 24);
      this.rbtnUpdateBor.TabIndex = 2;
      this.rbtnUpdateBor.Text = "Update the contact history for this borrower.";
      this.rbtnUpdateBor.CheckedChanged += new EventHandler(this.rbtnUpdateBor_CheckedChanged);
      this.chkBoxUpdateAddress.Location = new Point(20, 368);
      this.chkBoxUpdateAddress.Name = "chkBoxUpdateAddress";
      this.chkBoxUpdateAddress.Size = new Size(376, 20);
      this.chkBoxUpdateAddress.TabIndex = 12;
      this.chkBoxUpdateAddress.Text = "Update the contact's home address with the subject property address.";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(610, 407);
      this.Controls.Add((Control) this.chkBoxUpdateAddress);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnNext);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactSelectionDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Update Borrower Contacts";
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public ListViewItem getViewItemFromContact(BorrowerInfo contact)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      string str3 = string.Empty;
      if (contact.HomeAddress.Street2.Trim() != string.Empty)
        str1 = ", " + contact.HomeAddress.Street2;
      if (contact.HomeAddress.City.Trim() != string.Empty)
        str2 = ", " + contact.HomeAddress.City;
      if (contact.HomeAddress.State.Trim() != string.Empty)
        str3 = ", " + contact.HomeAddress.State;
      string str4 = contact.HomeAddress.Street1 + str1 + str2 + str3 + " " + contact.HomeAddress.Zip;
      return new ListViewItem(new string[3]
      {
        contact.LastName,
        contact.FirstName,
        str4
      })
      {
        Tag = (object) contact
      };
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
      this.getSelectedBorrower();
      this.getSelectedCoBorrower();
      if (!this.rbtnCreateBor.Enabled && !this.rbtnCreateCoBor.Enabled && !this.rbtnUpdateBor.Enabled && !this.rbtnUpdateCoBor.Enabled)
        this.btnCancel.PerformClick();
      if (this.rbtnUpdateBor.Checked & this.SelectedBorrower == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a contact for the borrower.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.rbtnUpdateCoBor.Checked & this.SelectedCoBorrower == null)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select a contact for the coborrower.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.CreateBorrower = this.rbtnCreateBor.Checked;
        this.CreateCoBorrower = this.rbtnCreateCoBor.Checked;
        this.UpdateAddress = this.chkBoxUpdateAddress.Checked;
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void getSelectedBorrower()
    {
      if (!this.rbtnUpdateBor.Checked)
        return;
      if (this.listViewBor.SelectedItems.Count > 0)
        this.SelectedBorrower = (BorrowerInfo) this.listViewBor.SelectedItems[0].Tag;
      else
        this.SelectedBorrower = (BorrowerInfo) null;
    }

    private void getSelectedCoBorrower()
    {
      if (!this.rbtnUpdateCoBor.Checked)
        return;
      if (this.listViewCoBor.SelectedItems.Count > 0)
        this.SelectedCoBorrower = (BorrowerInfo) this.listViewCoBor.SelectedItems[0].Tag;
      else
        this.SelectedCoBorrower = (BorrowerInfo) null;
    }

    private void rbtnUpdateCoBor_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rbtnUpdateCoBor.Checked)
      {
        this.listViewCoBor.Enabled = true;
        this.listViewCoBor.Focus();
      }
      else
        this.listViewCoBor.Enabled = false;
    }

    private void rbtnUpdateBor_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rbtnUpdateBor.Checked)
      {
        this.listViewBor.Enabled = true;
        this.listViewBor.Focus();
      }
      else
        this.listViewBor.Enabled = false;
    }
  }
}

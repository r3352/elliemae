// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactsWithNoEmailForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.UI;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactsWithNoEmailForm : Form
  {
    private Label label1;
    private ColumnHeader columnHeaderFirstName;
    private ColumnHeader columnHeaderLastName;
    private Button btnContinue;
    private Button btnAbort;
    private ListViewEx lvwContacts;
    private ColumnHeader columnHeader1;
    private ComboBox cmbSelect;
    private System.ComponentModel.Container components;

    public ContactsWithNoEmailForm(object[] contacts, ContactType contactType, bool hasContinued)
    {
      this.InitializeComponent();
      if (contactType == ContactType.Borrower)
      {
        for (int index = 0; index < contacts.Length; ++index)
        {
          BorrowerInfo contact = (BorrowerInfo) contacts[index];
          this.lvwContacts.Items.Add(new ListViewItem(new string[3]
          {
            "",
            contact.FirstName,
            contact.LastName
          }));
        }
      }
      else
      {
        for (int index = 0; index < contacts.Length; ++index)
        {
          BizPartnerInfo contact = (BizPartnerInfo) contacts[index];
          this.lvwContacts.Items.Add(new ListViewItem(new string[3]
          {
            "",
            contact.FirstName,
            contact.LastName
          }));
        }
      }
      if (hasContinued)
        return;
      this.btnContinue.Visible = false;
      this.label1.Text = "Warning: The following contacts you selected to email to have empty email addresses. The Mail Merge function will be aborted.";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnContinue = new Button();
      this.lvwContacts = new ListViewEx();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeaderFirstName = new ColumnHeader();
      this.columnHeaderLastName = new ColumnHeader();
      this.btnAbort = new Button();
      this.label1 = new Label();
      this.cmbSelect = new ComboBox();
      this.SuspendLayout();
      this.btnContinue.DialogResult = DialogResult.OK;
      this.btnContinue.Location = new Point(124, 224);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(75, 23);
      this.btnContinue.TabIndex = 0;
      this.btnContinue.Text = "Continue";
      this.lvwContacts.AllowColumnReorder = true;
      this.lvwContacts.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader1,
        this.columnHeaderFirstName,
        this.columnHeaderLastName
      });
      this.lvwContacts.DoubleClickActivation = false;
      this.lvwContacts.FullRowSelect = true;
      this.lvwContacts.GridLines = true;
      this.lvwContacts.Location = new Point(8, 64);
      this.lvwContacts.MultiSelect = false;
      this.lvwContacts.Name = "lvwContacts";
      this.lvwContacts.Size = new Size(272, 152);
      this.lvwContacts.TabIndex = 1;
      this.lvwContacts.UseCompatibleStateImageBehavior = false;
      this.lvwContacts.View = View.Details;
      this.lvwContacts.SubItemClicked += new SubItemEventHandler(this.lvwContacts_SubItemClicked);
      this.columnHeader1.Text = "Select";
      this.columnHeader1.Width = 56;
      this.columnHeaderFirstName.Text = "First Name";
      this.columnHeaderFirstName.Width = 120;
      this.columnHeaderLastName.Text = "Last Name";
      this.columnHeaderLastName.Width = 91;
      this.btnAbort.DialogResult = DialogResult.Abort;
      this.btnAbort.Location = new Point(206, 224);
      this.btnAbort.Name = "btnAbort";
      this.btnAbort.Size = new Size(75, 23);
      this.btnAbort.TabIndex = 2;
      this.btnAbort.Text = "Abort";
      this.label1.Location = new Point(12, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(268, 52);
      this.label1.TabIndex = 3;
      this.label1.Text = "Warning: The following contacts you selected to email to have empty email addresses. Do you wish to continue?";
      this.cmbSelect.FormattingEnabled = true;
      this.cmbSelect.Items.AddRange(new object[2]
      {
        (object) "Home",
        (object) "Work"
      });
      this.cmbSelect.Location = new Point(8, 222);
      this.cmbSelect.Name = "cmbSelect";
      this.cmbSelect.Size = new Size(90, 21);
      this.cmbSelect.TabIndex = 4;
      this.cmbSelect.Visible = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnAbort;
      this.ClientSize = new Size(292, 259);
      this.Controls.Add((Control) this.cmbSelect);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnAbort);
      this.Controls.Add((Control) this.lvwContacts);
      this.Controls.Add((Control) this.btnContinue);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactsWithNoEmailForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Contacts With No Email Address";
      this.ResumeLayout(false);
    }

    private void lvwContacts_SubItemClicked(object sender, SubItemEventArgs e)
    {
      this.lvwContacts.StartEditing((Control) this.cmbSelect, e.Item, e.SubItem);
    }
  }
}

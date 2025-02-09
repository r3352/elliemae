// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactsEmailSelection
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactsEmailSelection : Form
  {
    private ContactType contactType;
    private int personalEmailCount;
    private int bizEmailCount;
    private List<int> selectedContactIds = new List<int>();
    private List<string> emailOptions = new List<string>();
    private IContainer components;
    private ListViewEx lvwContacts;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeaderFirstName;
    private ColumnHeader columnHeaderLastName;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private Button btnOkay;
    private Button btnCancel;
    private ComboBox cmbOption;
    private Label label1;
    private CheckBox chbSelectAll;

    public int[] SelectedContactIds => this.selectedContactIds.ToArray();

    public string[] EmailOptions => this.emailOptions.ToArray();

    public ContactsEmailSelection(object[] contacts, ContactType contactType, bool hasContinued)
    {
      this.InitializeComponent();
      this.contactType = contactType;
      if (this.contactType == ContactType.Borrower)
      {
        this.chbSelectAll.Text = "Use business emails";
        for (int index = 0; index < contacts.Length; ++index)
        {
          BorrowerInfo contact = (BorrowerInfo) contacts[index];
          string str = string.Empty;
          if (string.Empty != contact.PersonalEmail)
          {
            str = ContactUtils.emailAddressOption_Home;
            ++this.personalEmailCount;
          }
          this.lvwContacts.Items.Add(new ListViewItem(new string[5]
          {
            str,
            contact.FirstName,
            contact.LastName,
            contact.PersonalEmail,
            contact.BizEmail
          })
          {
            Tag = (object) contact
          });
        }
      }
      else
      {
        this.chbSelectAll.Text = "Use home emails";
        for (int index = 0; index < contacts.Length; ++index)
        {
          BizPartnerInfo contact = (BizPartnerInfo) contacts[index];
          string str = string.Empty;
          if (string.Empty != contact.BizEmail)
          {
            str = ContactUtils.emailAddressOption_Work;
            ++this.bizEmailCount;
          }
          this.lvwContacts.Items.Add(new ListViewItem(new string[5]
          {
            str,
            contact.FirstName,
            contact.LastName,
            contact.PersonalEmail,
            contact.BizEmail
          })
          {
            Tag = (object) contact
          });
        }
      }
      this.btnOkay.Enabled = 0 < this.personalEmailCount + this.bizEmailCount;
    }

    private void lvwContacts_SubItemClicked(object sender, SubItemEventArgs e)
    {
      if (e.SubItem != 0)
        return;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      ListViewItem listViewItem = e.Item;
      string personalEmail;
      string bizEmail;
      if (this.contactType == ContactType.Borrower)
      {
        BorrowerInfo tag = (BorrowerInfo) listViewItem.Tag;
        personalEmail = tag.PersonalEmail;
        bizEmail = tag.BizEmail;
      }
      else
      {
        BizPartnerInfo tag = (BizPartnerInfo) listViewItem.Tag;
        personalEmail = tag.PersonalEmail;
        bizEmail = tag.BizEmail;
      }
      this.cmbOption.Items.Clear();
      this.cmbOption.Items.Add((object) string.Empty);
      if (string.Empty != personalEmail)
        this.cmbOption.Items.Add((object) ContactUtils.emailAddressOption_Home);
      if (string.Empty != bizEmail)
        this.cmbOption.Items.Add((object) ContactUtils.emailAddressOption_Work);
      if (ContactUtils.emailAddressOption_Home == listViewItem.SubItems[0].Text)
        --this.personalEmailCount;
      else if (ContactUtils.emailAddressOption_Work == listViewItem.SubItems[0].Text)
        --this.bizEmailCount;
      this.lvwContacts.StartEditing((Control) this.cmbOption, e.Item, e.SubItem);
    }

    private void cmbOption_SelectedIndexChanged(object sender, EventArgs e)
    {
      int personalEmailCount = this.personalEmailCount;
      int bizEmailCount = this.bizEmailCount;
      if (ContactUtils.emailAddressOption_Home == this.cmbOption.SelectedItem.ToString())
        ++personalEmailCount;
      else if (ContactUtils.emailAddressOption_Work == this.cmbOption.SelectedItem.ToString())
        ++bizEmailCount;
      this.btnOkay.Enabled = 0 < personalEmailCount + bizEmailCount;
    }

    private void lvwContacts_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
    {
      if (ContactUtils.emailAddressOption_Home == e.DisplayText)
        ++this.personalEmailCount;
      else if (ContactUtils.emailAddressOption_Work == e.DisplayText)
        ++this.bizEmailCount;
      this.btnOkay.Enabled = 0 < this.personalEmailCount + this.bizEmailCount;
      if (this.contactType == ContactType.Borrower)
      {
        if (!(e.Item.Text == ContactUtils.emailAddressOption_Home) || !this.chbSelectAll.Checked)
          return;
        this.chbSelectAll.CheckedChanged -= new EventHandler(this.chbSelectAll_CheckedChanged);
        this.chbSelectAll.Checked = false;
        this.chbSelectAll.CheckedChanged += new EventHandler(this.chbSelectAll_CheckedChanged);
      }
      else
      {
        if (!(e.Item.Text == ContactUtils.emailAddressOption_Work) || !this.chbSelectAll.Checked)
          return;
        this.chbSelectAll.CheckedChanged -= new EventHandler(this.chbSelectAll_CheckedChanged);
        this.chbSelectAll.Checked = false;
        this.chbSelectAll.CheckedChanged += new EventHandler(this.chbSelectAll_CheckedChanged);
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btnOkay_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Email Selection Summary:\n");
      if (0 < this.personalEmailCount)
        stringBuilder.AppendLine(string.Format("{0} {1} Home email selected.", (object) this.personalEmailCount.ToString(), 1 == this.personalEmailCount ? (object) "contact has" : (object) "contacts have"));
      if (0 < this.bizEmailCount)
        stringBuilder.AppendLine(string.Format("{0} {1} Work email selected.", (object) this.bizEmailCount.ToString(), 1 == this.bizEmailCount ? (object) "contact has" : (object) "contacts have"));
      int num = this.lvwContacts.Items.Count - this.personalEmailCount - this.bizEmailCount;
      if (0 < num)
        stringBuilder.AppendLine(string.Format("{0} {1} no email selected.", (object) num.ToString(), 1 == num ? (object) "contact has" : (object) "contacts have"));
      if (DialogResult.Cancel == Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk))
        return;
      foreach (ListViewItem listViewItem in this.lvwContacts.Items)
      {
        if (string.Empty != listViewItem.SubItems[0].Text)
        {
          this.selectedContactIds.Add(this.contactType != ContactType.Borrower ? ((BizPartnerInfo) listViewItem.Tag).ContactID : ((BorrowerInfo) listViewItem.Tag).ContactID);
          this.emailOptions.Add(listViewItem.SubItems[0].Text);
        }
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void chbSelectAll_CheckedChanged(object sender, EventArgs e)
    {
      this.personalEmailCount = 0;
      this.bizEmailCount = 0;
      foreach (ListViewItem listViewItem in this.lvwContacts.Items)
      {
        string str = string.Empty;
        if (this.contactType == ContactType.Borrower)
        {
          BorrowerInfo tag = (BorrowerInfo) listViewItem.Tag;
          if (this.chbSelectAll.Checked)
          {
            if (string.Empty != tag.BizEmail)
            {
              str = ContactUtils.emailAddressOption_Work;
              ++this.bizEmailCount;
            }
          }
          else if (string.Empty != tag.PersonalEmail)
          {
            str = ContactUtils.emailAddressOption_Home;
            ++this.personalEmailCount;
          }
        }
        else
        {
          BizPartnerInfo tag = (BizPartnerInfo) listViewItem.Tag;
          if (this.chbSelectAll.Checked)
          {
            if (string.Empty != tag.PersonalEmail)
            {
              str = ContactUtils.emailAddressOption_Home;
              ++this.personalEmailCount;
            }
          }
          else if (string.Empty != tag.BizEmail)
          {
            str = ContactUtils.emailAddressOption_Work;
            ++this.bizEmailCount;
          }
        }
        listViewItem.SubItems[0].Text = str;
      }
      this.btnOkay.Enabled = 0 < this.personalEmailCount + this.bizEmailCount;
    }

    private void lvwContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.contactType == ContactType.Borrower && this.chbSelectAll.Checked)
      {
        foreach (ListViewItem listViewItem in this.lvwContacts.Items)
        {
          if (listViewItem.SubItems[0].Text == ContactUtils.emailAddressOption_Home)
          {
            this.chbSelectAll.CheckedChanged -= new EventHandler(this.chbSelectAll_CheckedChanged);
            this.chbSelectAll.Checked = false;
            this.chbSelectAll.CheckedChanged += new EventHandler(this.chbSelectAll_CheckedChanged);
            break;
          }
        }
      }
      else
      {
        if (!this.chbSelectAll.Checked)
          return;
        foreach (ListViewItem listViewItem in this.lvwContacts.Items)
        {
          if (listViewItem.SubItems[0].Text == ContactUtils.emailAddressOption_Work)
          {
            this.chbSelectAll.CheckedChanged -= new EventHandler(this.chbSelectAll_CheckedChanged);
            this.chbSelectAll.Checked = false;
            this.chbSelectAll.CheckedChanged += new EventHandler(this.chbSelectAll_CheckedChanged);
          }
        }
      }
    }

    private void ContactsEmailSelection_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel_Click((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOkay = new Button();
      this.btnCancel = new Button();
      this.cmbOption = new ComboBox();
      this.label1 = new Label();
      this.lvwContacts = new ListViewEx();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeaderFirstName = new ColumnHeader();
      this.columnHeaderLastName = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.chbSelectAll = new CheckBox();
      this.SuspendLayout();
      this.btnOkay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOkay.Location = new Point(473, 337);
      this.btnOkay.Name = "btnOkay";
      this.btnOkay.Size = new Size(75, 23);
      this.btnOkay.TabIndex = 8;
      this.btnOkay.Text = "OK";
      this.btnOkay.UseVisualStyleBackColor = true;
      this.btnOkay.Click += new EventHandler(this.btnOkay_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(554, 337);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.cmbOption.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cmbOption.FormattingEnabled = true;
      this.cmbOption.Location = new Point(15, 337);
      this.cmbOption.Name = "cmbOption";
      this.cmbOption.Size = new Size(121, 21);
      this.cmbOption.TabIndex = 10;
      this.cmbOption.Visible = false;
      this.cmbOption.SelectedIndexChanged += new EventHandler(this.cmbOption_SelectedIndexChanged);
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(344, 16);
      this.label1.TabIndex = 4;
      this.label1.Text = "Please select which email address to use for each contact";
      this.lvwContacts.AllowColumnReorder = true;
      this.lvwContacts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwContacts.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeader1,
        this.columnHeaderFirstName,
        this.columnHeaderLastName,
        this.columnHeader2,
        this.columnHeader3
      });
      this.lvwContacts.DoubleClickActivation = false;
      this.lvwContacts.FullRowSelect = true;
      this.lvwContacts.GridLines = true;
      this.lvwContacts.Location = new Point(15, 32);
      this.lvwContacts.MultiSelect = false;
      this.lvwContacts.Name = "lvwContacts";
      this.lvwContacts.Size = new Size(614, 280);
      this.lvwContacts.TabIndex = 5;
      this.lvwContacts.UseCompatibleStateImageBehavior = false;
      this.lvwContacts.View = View.Details;
      this.lvwContacts.SubItemClicked += new SubItemEventHandler(this.lvwContacts_SubItemClicked);
      this.lvwContacts.SubItemEndEditing += new SubItemEndEditingEventHandler(this.lvwContacts_SubItemEndEditing);
      this.lvwContacts.SelectedIndexChanged += new EventHandler(this.lvwContacts_SelectedIndexChanged);
      this.columnHeader1.Text = "Select";
      this.columnHeader1.Width = 68;
      this.columnHeaderFirstName.Text = "First Name";
      this.columnHeaderFirstName.Width = 110;
      this.columnHeaderLastName.Text = "Last Name";
      this.columnHeaderLastName.Width = 110;
      this.columnHeader2.Text = "Home Email";
      this.columnHeader2.Width = 150;
      this.columnHeader3.Text = "Work Email";
      this.columnHeader3.Width = 150;
      this.chbSelectAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chbSelectAll.AutoSize = true;
      this.chbSelectAll.Location = new Point(15, 319);
      this.chbSelectAll.Name = "chbSelectAll";
      this.chbSelectAll.Size = new Size(70, 17);
      this.chbSelectAll.TabIndex = 11;
      this.chbSelectAll.Text = "Select All";
      this.chbSelectAll.UseVisualStyleBackColor = true;
      this.chbSelectAll.CheckedChanged += new EventHandler(this.chbSelectAll_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(644, 368);
      this.Controls.Add((Control) this.chbSelectAll);
      this.Controls.Add((Control) this.cmbOption);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOkay);
      this.Controls.Add((Control) this.lvwContacts);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (ContactsEmailSelection);
      this.Text = "Contacts Email Selection";
      this.KeyUp += new KeyEventHandler(this.ContactsEmailSelection_KeyUp);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

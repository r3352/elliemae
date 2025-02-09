// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.ePass.EmailListDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.eFolder.eDelivery.HelperMethods;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.ePass
{
  [ComVisible(false)]
  public class EmailListDialog : Form, IHelp
  {
    private const string className = "EmailListDialog";
    private static readonly string sw = Tracing.SwEpass;
    private ColumnHeader contactHdr;
    private ColumnHeader emailHdr;
    private ColumnHeader categoryHdr;
    private Button okBtn;
    private Button cancelBtn;
    protected ListView emailLvw;
    private System.ComponentModel.Container components;
    private StandardIconButton btnAddContact;
    private ListViewSortManager sortMngr;
    protected LoanData loanData;
    protected FileContacts fileContacts;
    public List<string> selectedContacts;
    public List<ContactDetails> loanContacts;
    public List<string> contacts = new List<string>();
    public List<NonBorrowingOwner> selectedNbo = new List<NonBorrowingOwner>();
    private bool includeAllPairs;

    public EmailListDialog(
      LoanData loanData,
      List<ContactDetails> loanContacts,
      bool includeAllPairs = false)
    {
      this.includeAllPairs = includeAllPairs;
      this.InitializeComponent();
      this.loanData = loanData;
      this.loanContacts = loanContacts == null ? new List<ContactDetails>() : loanContacts;
      this.fileContacts = SendPackageFactory.CreateFileContact(loanData);
      this.initializeEmailList();
      this.sortMngr = new ListViewSortManager(this.emailLvw, new System.Type[3]
      {
        typeof (ListViewTextSort),
        typeof (ListViewTextSort),
        typeof (ListViewTextSort)
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
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.emailLvw = new ListView();
      this.contactHdr = new ColumnHeader();
      this.emailHdr = new ColumnHeader();
      this.categoryHdr = new ColumnHeader();
      this.btnAddContact = new StandardIconButton();
      ((ISupportInitialize) this.btnAddContact).BeginInit();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom;
      this.okBtn.Location = new Point(237, 347);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(118, 29);
      this.okBtn.TabIndex = 2;
      this.okBtn.Text = "&Select";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(366, 347);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(117, 29);
      this.cancelBtn.TabIndex = 3;
      this.cancelBtn.Text = "&Cancel";
      this.emailLvw.Columns.AddRange(new ColumnHeader[3]
      {
        this.contactHdr,
        this.emailHdr,
        this.categoryHdr
      });
      this.emailLvw.FullRowSelect = true;
      this.emailLvw.HideSelection = false;
      this.emailLvw.Location = new Point(17, 15);
      this.emailLvw.Name = "emailLvw";
      this.emailLvw.Size = new Size(700, 315);
      this.emailLvw.Sorting = SortOrder.Ascending;
      this.emailLvw.TabIndex = 1;
      this.emailLvw.UseCompatibleStateImageBehavior = false;
      this.emailLvw.View = View.Details;
      this.emailLvw.DoubleClick += new EventHandler(this.emailLvw_DoubleClick);
      this.contactHdr.Text = "Contact Name";
      this.contactHdr.Width = 120;
      this.emailHdr.Text = "Email Address";
      this.emailHdr.Width = 260;
      this.categoryHdr.Text = "Category";
      this.categoryHdr.Width = 100;
      this.btnAddContact.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddContact.BackColor = Color.Transparent;
      this.btnAddContact.Location = new Point(670, 22);
      this.btnAddContact.Margin = new Padding(4, 3, 0, 3);
      this.btnAddContact.MouseDownImage = (Image) null;
      this.btnAddContact.Name = "btnAddContact";
      this.btnAddContact.Size = new Size(16, 16);
      this.btnAddContact.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddContact.TabIndex = 48;
      this.btnAddContact.TabStop = false;
      this.btnAddContact.Click += new EventHandler(this.btnAddContact_Click);
      this.AutoScaleBaseSize = new Size(7, 16);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(735, 400);
      this.Controls.Add((Control) this.btnAddContact);
      this.Controls.Add((Control) this.emailLvw);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EmailListDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "File Contacts";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      ((ISupportInitialize) this.btnAddContact).EndInit();
      this.ResumeLayout(false);
    }

    private void initializeEmailList()
    {
      this.emailLvw.Items.Clear();
      if (this.includeAllPairs)
      {
        int pairIndex1 = this.loanData.GetPairIndex(this.loanData.PairId);
        int pairIndex2 = -1;
        BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
        try
        {
          foreach (BorrowerPair borrowerPair in borrowerPairs)
          {
            pairIndex2 = this.loanData.GetPairIndex(borrowerPair.Id);
            this.loanData.SetBorrowerPair(pairIndex2);
            foreach (string str in this.fileContacts.ContactTypes.Keys.ToList<string>().FindAll((Predicate<string>) (x => x.ToLower().Contains("borrower"))))
            {
              Dictionary<string, string> contactDetails = this.fileContacts.GetContactDetails(str);
              if (!string.IsNullOrEmpty(contactDetails["Email"]) || !string.IsNullOrEmpty(contactDetails["Name"]))
              {
                ListViewItem listViewItem = this.AddListViewItem(contactDetails, str);
                listViewItem.Tag = str.ToLower() == "borrower" ? (object) this.loanData.CurrentBorrowerPair.Borrower.Id : (object) this.loanData.CurrentBorrowerPair.CoBorrower.Id;
                this.emailLvw.Items.Add(listViewItem);
                this.fileContacts.ContactTypes[str] = 1;
                this.contacts.Add(string.Join(";", str, contactDetails["Name"], contactDetails["Email"], string.Empty, "1", "0"));
              }
            }
          }
        }
        finally
        {
          if (pairIndex1 != pairIndex2)
            this.loanData.SetBorrowerPair(pairIndex1);
        }
      }
      else
      {
        foreach (string str in this.fileContacts.ContactTypes.Keys.ToList<string>().FindAll((Predicate<string>) (x => x.ToLower().Contains("borrower"))))
        {
          Dictionary<string, string> contactDetails = this.fileContacts.GetContactDetails(str);
          if (!string.IsNullOrEmpty(contactDetails["Email"]) || !string.IsNullOrEmpty(contactDetails["Name"]))
          {
            ListViewItem listViewItem = this.AddListViewItem(contactDetails, str);
            listViewItem.Tag = str.ToLower() == "borrower" ? (object) this.loanData.CurrentBorrowerPair.Borrower.Id : (object) this.loanData.CurrentBorrowerPair.CoBorrower.Id;
            this.emailLvw.Items.Add(listViewItem);
            this.fileContacts.ContactTypes[str] = 1;
            this.contacts.Add(string.Join(";", str, contactDetails["Name"], contactDetails["Email"], string.Empty, "1", "0"));
          }
        }
      }
      foreach (string str in this.fileContacts.ContactTypes.Keys.ToList<string>().FindAll((Predicate<string>) (x => !x.ToLower().Contains("borrower"))))
      {
        string contactType = str;
        if (!this.loanContacts.Exists((Predicate<ContactDetails>) (x => x.contactType == contactType && !string.IsNullOrEmpty(x.email) && !string.IsNullOrEmpty(x.name))))
        {
          Dictionary<string, string> contactDetails = this.fileContacts.GetContactDetails(contactType);
          if (!string.IsNullOrEmpty(contactDetails["Email"]) || !string.IsNullOrEmpty(contactDetails["Name"]))
          {
            this.emailLvw.Items.Add(new ListViewItem(contactDetails["Name"])
            {
              SubItems = {
                contactDetails["Email"],
                contactType
              }
            });
            this.fileContacts.ContactTypes[contactType] = 1;
            this.contacts.Add(string.Join(";", contactType, contactDetails["Name"], contactDetails["Email"], string.Empty, "1", "0"));
          }
        }
        else
        {
          foreach (ContactDetails contactDetails in this.loanContacts.FindAll((Predicate<ContactDetails>) (x => x.contactType == contactType && !string.IsNullOrEmpty(x.email) && !string.IsNullOrEmpty(x.name))))
          {
            this.emailLvw.Items.Add(new ListViewItem(contactDetails.name)
            {
              SubItems = {
                contactDetails.email,
                contactType
              }
            });
            this.contacts.Add(string.Join(";", contactType, contactDetails.name, contactDetails.email, string.Empty, "0", "0"));
          }
          Dictionary<string, string> contactDetails1 = this.fileContacts.GetContactDetails(contactType);
          if (!string.IsNullOrEmpty(contactDetails1["Email"]) || !string.IsNullOrEmpty(contactDetails1["Name"]))
          {
            this.fileContacts.ContactTypes[contactType] = 1;
            this.contacts.Add(string.Join(";", contactType, contactDetails1["Name"], contactDetails1["Email"], string.Empty, "1", "0"));
          }
        }
      }
      foreach (NonBorrowingOwner nonBorrowingOwner in this.includeAllPairs ? this.loanData.GetNboByBorrowerPairId("") : this.loanData.GetNboByBorrowerPairId(this.loanData.CurrentBorrowerPair.Id))
      {
        string text = nonBorrowingOwner.FirstName + " " + nonBorrowingOwner.LastName;
        this.emailLvw.Items.Add(new ListViewItem(text)
        {
          SubItems = {
            nonBorrowingOwner.EmailAddress,
            "Non-Borrowing Owner"
          },
          Tag = (object) nonBorrowingOwner
        });
        this.contacts.Add(string.Join(";", "NonBorrowingOwner", text, nonBorrowingOwner.EmailAddress, string.Empty, "1", "0"));
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.emailLvw.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select at least one e-mail.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        List<string> stringList = new List<string>();
        foreach (ListViewItem selectedItem in this.emailLvw.SelectedItems)
          stringList.Add(selectedItem.SubItems[2].Text.ToString());
        foreach (ListViewItem selectedItem in this.emailLvw.SelectedItems)
        {
          if (this.selectedContacts == null)
            this.selectedContacts = new List<string>();
          string fileContact = string.Empty;
          foreach (ListViewItem.ListViewSubItem subItem in selectedItem.SubItems)
            fileContact = fileContact + subItem.Text + ";";
          if (!fileContact.Split(';')[2].ToLower().Contains("borrower"))
          {
            if (!fileContact.Split(';')[2].ToLower().Contains("non-borrowing") && stringList.FindAll((Predicate<string>) (x => x.Equals(fileContact.Split(';')[2]))).Count == 1)
            {
              if (this.fileContacts.ContactTypes.Where<KeyValuePair<string, int>>((Func<KeyValuePair<string, int>, bool>) (x => x.Value == 1)).Select<KeyValuePair<string, int>, string>((Func<KeyValuePair<string, int>, string>) (pair => pair.Key)).ToList<string>().Exists((Predicate<string>) (x => x.Equals(fileContact.Split(';')[2]))))
              {
                if (string.IsNullOrEmpty(this.contacts.Find((Predicate<string>) (x =>
                {
                  if (x.Contains(fileContact.Split(';')[2]))
                  {
                    if (x.Split(';')[1] == fileContact.Split(';')[0])
                    {
                      if (x.Split(';')[2] == fileContact.Split(';')[1])
                        return x.Split(';')[4] == "1";
                    }
                  }
                  return false;
                }))))
                {
                  if (Utils.Dialog((IWin32Window) this, "You are updating the existing " + fileContact.Split(';')[2] + " details.  Do you want to save?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                  {
                    if (!this.fileContacts.UpdateContactDetails(new Dictionary<string, string>()
                    {
                      {
                        "ContactType",
                        fileContact.Split(';')[2]
                      },
                      {
                        "Name",
                        fileContact.Split(';')[0]
                      },
                      {
                        "Email",
                        fileContact.Split(';')[1]
                      }
                    }))
                    {
                      int num2 = (int) Utils.Dialog((IWin32Window) this, "Error occured while updating File contact. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                      return;
                    }
                    string str = this.contacts.Find((Predicate<string>) (x =>
                    {
                      if (!x.Contains(fileContact.Split(';')[2]))
                        return false;
                      return x.Split(';')[4] == "1";
                    }));
                    this.contacts.Add(string.Join(";", fileContact.Split(';')[2], fileContact.Split(';')[0], fileContact.Split(';')[1], string.Empty, "1", "0"));
                    if (string.IsNullOrEmpty(str))
                      this.contacts.Add(string.Join(";", str.Split(';')[2], str.Split(';')[0], str.Split(';')[1], string.Empty, "0", "0"));
                    this.contacts.Remove(str);
                  }
                }
              }
              else
              {
                if (!this.fileContacts.UpdateContactDetails(new Dictionary<string, string>()
                {
                  {
                    "ContactType",
                    fileContact.Split(';')[2]
                  },
                  {
                    "Name",
                    fileContact.Split(';')[0]
                  },
                  {
                    "Email",
                    fileContact.Split(';')[1]
                  }
                }))
                {
                  int num3 = (int) Utils.Dialog((IWin32Window) this, "Error occured while updating File contact. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  return;
                }
                this.contacts.Add(string.Join(";", fileContact.Split(';')[2], fileContact.Split(';')[0], fileContact.Split(';')[1], string.Empty, "1", "0"));
              }
            }
          }
          if (selectedItem.Tag is NonBorrowingOwner)
          {
            this.selectedNbo.Add(selectedItem.Tag as NonBorrowingOwner);
          }
          else
          {
            fileContact = fileContact + selectedItem.Tag + ";";
            this.selectedContacts.Add(fileContact);
          }
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void emailLvw_DoubleClick(object sender, EventArgs e)
    {
      if (this.emailLvw.SelectedItems.Count == 0)
        return;
      if (this.selectedContacts == null)
        this.selectedContacts = new List<string>();
      string fileContact = string.Empty;
      foreach (ListViewItem.ListViewSubItem subItem in this.emailLvw.SelectedItems[0].SubItems)
        fileContact = fileContact + subItem.Text + ";";
      if (!fileContact.Split(';')[2].ToLower().Contains("borrower"))
      {
        if (!fileContact.Split(';')[2].ToLower().Contains("non-borrowing"))
        {
          if (this.fileContacts.ContactTypes.Where<KeyValuePair<string, int>>((Func<KeyValuePair<string, int>, bool>) (x => x.Value == 1)).Select<KeyValuePair<string, int>, string>((Func<KeyValuePair<string, int>, string>) (pair => pair.Key)).ToList<string>().Exists((Predicate<string>) (x => x.Equals(fileContact.Split(';')[2]))))
          {
            if (string.IsNullOrEmpty(this.contacts.Find((Predicate<string>) (x =>
            {
              if (x.Contains(fileContact.Split(';')[2]))
              {
                if (x.Split(';')[1] == fileContact.Split(';')[0])
                {
                  if (x.Split(';')[2] == fileContact.Split(';')[1])
                    return x.Split(';')[4] == "1";
                }
              }
              return false;
            }))))
            {
              if (Utils.Dialog((IWin32Window) this, "You are updating the existing " + fileContact.Split(';')[2] + " details.  Do you want to save?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
              {
                if (!this.fileContacts.UpdateContactDetails(new Dictionary<string, string>()
                {
                  {
                    "ContactType",
                    fileContact.Split(';')[2]
                  },
                  {
                    "Name",
                    fileContact.Split(';')[0]
                  },
                  {
                    "Email",
                    fileContact.Split(';')[1]
                  }
                }))
                {
                  int num = (int) Utils.Dialog((IWin32Window) this, "Error occured while updating File contact. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  return;
                }
                string str = this.contacts.Find((Predicate<string>) (x =>
                {
                  if (!x.Contains(fileContact.Split(';')[2]))
                    return false;
                  return x.Split(';')[4] == "1";
                }));
                this.contacts.Add(string.Join(";", fileContact.Split(';')[2], fileContact.Split(';')[0], fileContact.Split(';')[1], string.Empty, "1", "0"));
                if (string.IsNullOrEmpty(str))
                  this.contacts.Add(string.Join(";", str.Split(';')[2], str.Split(';')[0], str.Split(';')[1], string.Empty, "0", "0"));
                this.contacts.Remove(str);
              }
            }
          }
          else
          {
            if (!this.fileContacts.UpdateContactDetails(new Dictionary<string, string>()
            {
              {
                "ContactType",
                fileContact.Split(';')[2]
              },
              {
                "Name",
                fileContact.Split(';')[0]
              },
              {
                "Email",
                fileContact.Split(';')[1]
              }
            }))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "Error occured while updating File contact. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            this.contacts.Add(string.Join(";", fileContact.Split(';')[2], fileContact.Split(';')[0], fileContact.Split(';')[1], string.Empty, "1", "0"));
          }
        }
      }
      if (!fileContact.Split(';')[2].ToLower().Contains("non-borrowing"))
      {
        fileContact = fileContact + this.emailLvw.SelectedItems[0].Tag + ";";
        this.selectedContacts.Add(fileContact);
      }
      else
        this.selectedNbo.Add(this.emailLvw.SelectedItems[0].Tag as NonBorrowingOwner);
      this.DialogResult = DialogResult.OK;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (EmailListDialog));
    }

    private void btnAddContact_Click(object sender, EventArgs e) => this.AddContactDetails();

    protected virtual void AddContactDetails()
    {
      using (AddFileContactDialog dialog = new AddFileContactDialog(this.fileContacts.ContactTypes.Where<KeyValuePair<string, int>>((Func<KeyValuePair<string, int>, bool>) (x => x.Value != 1 || !x.Key.ToLower().Contains("borrower"))).Select<KeyValuePair<string, int>, string>((Func<KeyValuePair<string, int>, string>) (pair => pair.Key)).ToList<string>()))
      {
        if (dialog.ShowDialog() != DialogResult.OK)
          return;
        if (this.fileContacts.ContactTypes.Where<KeyValuePair<string, int>>((Func<KeyValuePair<string, int>, bool>) (x => x.Value == 1)).Select<KeyValuePair<string, int>, string>((Func<KeyValuePair<string, int>, string>) (pair => pair.Key)).ToList<string>().Exists((Predicate<string>) (x => x.Equals(dialog.contactDetails["ContactType"].ToString()))))
        {
          if (string.IsNullOrEmpty(this.contacts.Find((Predicate<string>) (x =>
          {
            if (x.Contains(dialog.contactDetails["ContactType"]))
            {
              if (x.Split(';')[1] == dialog.contactDetails["Name"])
              {
                if (x.Split(';')[2] == dialog.contactDetails["Email"])
                  return x.Split(';')[4] == "1";
              }
            }
            return false;
          }))))
          {
            if (Utils.Dialog((IWin32Window) this, "You are updating the existing " + dialog.contactDetails["ContactType"].ToString() + " details.  Do you want to save?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
              if (!this.fileContacts.UpdateContactDetails(dialog.contactDetails))
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "Error occured while updating File contact. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
              string str = this.contacts.Find((Predicate<string>) (x =>
              {
                if (!x.Contains(dialog.contactDetails["ContactType"]))
                  return false;
                return x.Split(';')[4] == "1";
              }));
              this.contacts.Add(string.Join(";", dialog.contactDetails["ContactType"], dialog.contactDetails["Name"], dialog.contactDetails["Email"], dialog.contactDetails["AuthCode"], "1", "1"));
              if (string.IsNullOrEmpty(str))
                this.contacts.Add(string.Join(";", str.Split(';')[2], str.Split(';')[0], str.Split(';')[1], string.Empty, "0", "0"));
            }
            else
              this.contacts.Add(string.Join(";", dialog.contactDetails["ContactType"], dialog.contactDetails["Name"], dialog.contactDetails["Email"], dialog.contactDetails["AuthCode"], "0", "1"));
          }
        }
        else
        {
          if (!this.fileContacts.UpdateContactDetails(dialog.contactDetails))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Error occured while updating File contact. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          this.contacts.Add(string.Join(";", dialog.contactDetails["ContactType"], dialog.contactDetails["Name"], dialog.contactDetails["Email"], dialog.contactDetails["AuthCode"], "1", "1"));
        }
        Task.WaitAll((Task) new EBSServiceClient().GetRecipientURL(new GetRecipientURLRequest()
        {
          loanId = this.loanData.GetField("GUID").Replace("{", string.Empty).Replace("}", string.Empty),
          siteId = !string.IsNullOrEmpty(this.loanData.GetField("ConsumerConnectSiteID")) ? this.loanData.GetField("ConsumerConnectSiteID") : "1234567890",
          contacts = new List<Contact>()
          {
            new Contact()
            {
              contactType = dialog.contactDetails["ContactType"].ToLower().Contains("borrower") ? "Borrower" : dialog.contactDetails["ContactType"],
              name = dialog.contactDetails["Name"],
              email = dialog.contactDetails["Email"],
              authCode = dialog.contactDetails["AuthCode"],
              authType = "AuthCode",
              recipientId = Guid.NewGuid().ToString(),
              borrowerId = dialog.contactDetails["ContactType"].ToLower().Contains("borrower") ? (dialog.contactDetails["ContactType"].ToLower().StartsWith("borrower") ? this.loanData.CurrentBorrowerPair.Borrower.Id : this.loanData.CurrentBorrowerPair.CoBorrower.Id) : (string) null
            }
          }.ToArray()
        }));
        this.emailLvw.Items.Add(new ListViewItem(dialog.contactDetails["Name"])
        {
          SubItems = {
            dialog.contactDetails["Email"],
            dialog.contactDetails["ContactType"]
          }
        });
        this.fileContacts.ContactTypes[dialog.contactDetails["ContactType"]] = 1;
      }
    }

    protected virtual ListViewItem AddListViewItem(
      Dictionary<string, string> contactDetails,
      string ContactType)
    {
      return new ListViewItem(contactDetails["Name"])
      {
        SubItems = {
          contactDetails["Email"],
          ContactType
        }
      };
    }
  }
}

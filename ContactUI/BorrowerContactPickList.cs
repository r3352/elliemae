// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BorrowerContactPickList
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BorrowerContactPickList : Form, IHelp
  {
    private const string className = "BorrowerContactPickList";
    private static string sw = Tracing.SwContact;
    private Hashtable userNameCache = new Hashtable();
    private string helpID = string.Empty;
    private bool forOrderCredit;
    private int contactIdToExclude = -1;
    private Button btnDone;
    private Button Cancel;
    private GridView gvContacts;
    private System.ComponentModel.Container components;

    public BorrowerContactPickList(int contactIdToExclude, string helpID, bool forOrderCredit)
    {
      this.InitializeComponent();
      this.helpID = helpID;
      this.forOrderCredit = forOrderCredit;
      this.contactIdToExclude = contactIdToExclude;
      this.Init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.btnDone = new Button();
      this.Cancel = new Button();
      this.gvContacts = new GridView();
      this.SuspendLayout();
      this.btnDone.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnDone.Location = new Point(430, 422);
      this.btnDone.Name = "btnDone";
      this.btnDone.Size = new Size(88, 24);
      this.btnDone.TabIndex = 1;
      this.btnDone.Text = "&Done";
      this.btnDone.Click += new EventHandler(this.btnDone_Click);
      this.Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.Cancel.DialogResult = DialogResult.Cancel;
      this.Cancel.Location = new Point(526, 422);
      this.Cancel.Name = "Cancel";
      this.Cancel.Size = new Size(88, 24);
      this.Cancel.TabIndex = 2;
      this.Cancel.Text = "&Cancel";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Owner";
      gvColumn1.Width = 74;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 90;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Home Phone";
      gvColumn4.Width = 80;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Personal Email";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Type";
      gvColumn6.Width = 40;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "Status";
      gvColumn7.Width = 60;
      this.gvContacts.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gvContacts.Location = new Point(12, 12);
      this.gvContacts.Name = "gvContacts";
      this.gvContacts.Size = new Size(602, 404);
      this.gvContacts.TabIndex = 10;
      this.gvContacts.ItemDoubleClick += new GVItemEventHandler(this.gvContacts_ItemDoubleClick);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.Cancel;
      this.ClientSize = new Size(624, 454);
      this.Controls.Add((Control) this.Cancel);
      this.Controls.Add((Control) this.btnDone);
      this.Controls.Add((Control) this.gvContacts);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (BorrowerContactPickList);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Co-Borrower Contact";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
    }

    private void Init() => this.getContacts();

    private void getContacts()
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        QueryCriterion queryCriterion = (QueryCriterion) new OrdinalValueCriterion("Contact.ContactID", (object) this.contactIdToExclude, OrdinalMatchType.NotEquals);
        SortField sortField = new SortField("Contact.LastName", FieldSortOrder.Descending);
        List<string> stringList = new List<string>()
        {
          "Contact.ContactID",
          "Contact.FirstName",
          "Contact.LastName",
          "Contact.HomePhone",
          "Contact.PersonalEmail",
          "Contact.ContactType",
          "Contact.Status",
          "Contact.SSN",
          "Contact.OwnerID"
        };
        this.gvContacts.DataProvider = (IGVDataProvider) new CursorGVDataProvider(Session.ContactManager.OpenBorrowerCursor(new QueryCriterion[1]
        {
          queryCriterion
        }, RelatedLoanMatchType.None, new SortField[1]
        {
          sortField
        }, (string[]) null, false), new PopulateGVItemEventHandler(this.contactProvider_PopulateGVItem));
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void contactProvider_PopulateGVItem(object sender, PopulateGVItemEventArgs e)
    {
      if (e.DataItem == null)
        return;
      BorrowerInfo dataItem = (BorrowerInfo) e.DataItem;
      e.ListItem.SubItems[0].Text = dataItem.OwnerID;
      e.ListItem.SubItems[1].Text = dataItem.LastName;
      e.ListItem.SubItems[2].Text = dataItem.FirstName;
      e.ListItem.SubItems[3].Text = dataItem.HomePhone;
      e.ListItem.SubItems[4].Text = dataItem.PersonalEmail;
      e.ListItem.SubItems[5].Text = Enum.GetName(typeof (BorrowerType), (object) dataItem.ContactType);
      e.ListItem.SubItems[6].Text = dataItem.Status;
      e.ListItem.Tag = (object) dataItem;
    }

    public BorrowerInfo GetSelectedBorrowerContact()
    {
      return this.gvContacts.SelectedItems.Count == 0 ? (BorrowerInfo) null : (BorrowerInfo) this.gvContacts.SelectedItems[0].Tag;
    }

    private void btnDone_Click(object sender, EventArgs e)
    {
      if (this.gvContacts.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You haven't selected any contact as the co-borrower.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.forOrderCredit)
        {
          BorrowerInfo tag = (BorrowerInfo) this.gvContacts.SelectedItems[0].Tag;
          if ((tag.FirstName.Trim() == string.Empty || tag.LastName.Trim() == string.Empty || string.Concat(tag["Contact.SSN"]) == string.Empty) && DialogResult.Cancel == new OrderCreditMissingInfoDlg(tag).ShowDialog((IWin32Window) this))
            return;
        }
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.helpID);
    }

    private void gvContacts_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (this.gvContacts.SelectedItems.Count <= 0)
        return;
      this.btnDone.PerformClick();
    }
  }
}

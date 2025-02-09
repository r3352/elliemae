// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactQueryForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactQueryForm : Form
  {
    private Button btnDeleteQuery;
    private Button btnRename;
    private Button btnLoad;
    protected ListBox listBoxSavedQueries;
    private Button btnCancel;
    private System.ComponentModel.Container components;
    private ContactType _ContactType;
    private ContactProvider contactProvider;
    private ContactQuery _QueryToLoad;

    public ContactQueryForm(ContactType contactType)
    {
      this.InitializeComponent();
      this._ContactType = contactType;
      this.contactProvider = this._ContactType != ContactType.Borrower ? (ContactProvider) new BizPartnerProvider() : (ContactProvider) new BorrowerProvider();
      this.loadSavedQueries();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnDeleteQuery = new Button();
      this.btnRename = new Button();
      this.btnLoad = new Button();
      this.listBoxSavedQueries = new ListBox();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.btnDeleteQuery.Location = new Point(272, 56);
      this.btnDeleteQuery.Name = "btnDeleteQuery";
      this.btnDeleteQuery.TabIndex = 22;
      this.btnDeleteQuery.Text = "Delete";
      this.btnDeleteQuery.Click += new EventHandler(this.btnDeleteQuery_Click);
      this.btnRename.Location = new Point(272, 20);
      this.btnRename.Name = "btnRename";
      this.btnRename.TabIndex = 21;
      this.btnRename.Text = "Rename";
      this.btnRename.Click += new EventHandler(this.btnRename_Click);
      this.btnLoad.Location = new Point(92, 228);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.TabIndex = 20;
      this.btnLoad.Text = "Load";
      this.btnLoad.Click += new EventHandler(this.btnLoad_Click);
      this.listBoxSavedQueries.Location = new Point(12, 11);
      this.listBoxSavedQueries.Name = "listBoxSavedQueries";
      this.listBoxSavedQueries.Size = new Size(252, 199);
      this.listBoxSavedQueries.TabIndex = 19;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(184, 228);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 23;
      this.btnCancel.Text = "Cancel";
      this.AcceptButton = (IButtonControl) this.btnLoad;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(358, 263);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnDeleteQuery);
      this.Controls.Add((Control) this.btnRename);
      this.Controls.Add((Control) this.btnLoad);
      this.Controls.Add((Control) this.listBoxSavedQueries);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactQueryForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Searches";
      this.ResumeLayout(false);
    }

    protected void loadSavedQueries()
    {
      ContactQueries contactQueries = this.contactProvider.GetContactQueries(Session.UserID);
      this.listBoxSavedQueries.Items.Clear();
      this.listBoxSavedQueries.Items.AddRange((object[]) contactQueries.Items);
    }

    private void btnDeleteQuery_Click(object sender, EventArgs e)
    {
      if (this.listBoxSavedQueries.SelectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a saved query in the list to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (DialogResult.OK != Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected saved search?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
          return;
        this.contactProvider.DeleteContactQueries(Session.UserID, (ContactQuery) this.listBoxSavedQueries.SelectedItem);
        this.loadSavedQueries();
      }
    }

    private void btnRename_Click(object sender, EventArgs e)
    {
      if (this.listBoxSavedQueries.SelectedItem == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a saved query in the list to rename.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        NewPhraseForm newPhraseForm = new NewPhraseForm();
        newPhraseForm.WindowTitle = "New Query Name";
        newPhraseForm.Description = "Please enter your new query name.";
        newPhraseForm.TopMost = true;
        if (newPhraseForm.ShowDialog() == DialogResult.Cancel)
          return;
        string str = newPhraseForm.NewPhrase.Trim();
        if (str == string.Empty)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Please enter a name for the query.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          for (int index = 0; index < this.listBoxSavedQueries.Items.Count; ++index)
          {
            if (index != this.listBoxSavedQueries.SelectedIndex)
            {
              ContactQuery contactQuery = (ContactQuery) this.listBoxSavedQueries.Items[index];
              if (str == contactQuery.Name.Trim())
              {
                int num3 = (int) Utils.Dialog((IWin32Window) this, "There already exist a query with this name.  Please enter another name of your new query.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
          }
          ContactQuery selectedItem = (ContactQuery) this.listBoxSavedQueries.SelectedItem;
          this.contactProvider.DeleteContactQueries(Session.UserID, selectedItem);
          selectedItem.Name = newPhraseForm.NewPhrase.Trim();
          this.contactProvider.AddContactQuery(Session.UserID, selectedItem);
          this.loadSavedQueries();
        }
      }
    }

    public ContactQuery QueryToLoad => this._QueryToLoad;

    private void btnLoad_Click(object sender, EventArgs e)
    {
      if (this.listBoxSavedQueries.SelectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a saved query in the list to load.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this._QueryToLoad = (ContactQuery) this.listBoxSavedQueries.SelectedItem;
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }
  }
}

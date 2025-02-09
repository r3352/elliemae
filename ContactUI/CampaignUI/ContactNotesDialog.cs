// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.ContactNotesDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class ContactNotesDialog : ContactNotesForm
  {
    private ContactType contactType;
    private int contactId;
    private IContainer components;

    public ContactNotesDialog(ContactType contactType, int contactId)
    {
      this.contactType = contactType;
      this.contactId = contactId;
      this.InitializeComponent();
      this.initializeDialog();
    }

    private void initializeDialog()
    {
      this.Text = this.contactType == ContactType.Borrower ? "Borrower Contact Notes" : "Business Contact Notes";
      this.IsReadOnly = false;
      this.CurrentContactID = this.contactId;
      this.ContactDeleted += new ContactDeletedEventHandler(this.ContactNotesDialog_ContactDeleted);
    }

    private void ContactNotesDialog_ContactDeleted(int contactId)
    {
      int num = (int) Utils.Dialog((IWin32Window) this, "The " + (this.contactType == ContactType.Borrower ? "borrower contact" : "business contact") + " associated with this task has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    protected override ContactNote[] fetchNotes(int contactId)
    {
      return Session.ContactManager.GetNotesForContact(contactId, this.contactType);
    }

    protected override ContactNote fetchNote(int contactId, int noteId)
    {
      return Session.ContactManager.GetNoteForContact(contactId, this.contactType, noteId);
    }

    protected override int addNote(int contactId, ContactNote note)
    {
      return Session.ContactManager.AddNoteForContact(contactId, this.contactType, note);
    }

    protected override void updateNote(int contactId, ContactNote note)
    {
      Session.ContactManager.UpdateNoteForContact(contactId, this.contactType, note);
    }

    protected override void deleteNote(int contactId, int noteId)
    {
      Session.ContactManager.DeleteNoteForContact(contactId, this.contactType, noteId);
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.BackColor = SystemColors.Control;
      this.ClientSize = new Size(707, 396);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactNotesDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Contact Notes";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

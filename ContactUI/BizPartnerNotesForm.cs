// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizPartnerNotesForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.RemotingServices;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizPartnerNotesForm : ContactNotesForm
  {
    private System.ComponentModel.Container components;

    public BizPartnerNotesForm() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(500, 268);
      this.Name = nameof (BizPartnerNotesForm);
    }

    protected override ContactNote[] fetchNotes(int contactId)
    {
      return Session.ContactManager.GetNotesForContact(contactId, ContactType.BizPartner);
    }

    protected override ContactNote fetchNote(int contactId, int noteId)
    {
      return Session.ContactManager.GetNoteForContact(contactId, ContactType.BizPartner, noteId);
    }

    protected override int addNote(int contactId, ContactNote note)
    {
      return Session.ContactManager.AddNoteForContact(contactId, ContactType.BizPartner, note);
    }

    protected override void updateNote(int contactId, ContactNote note)
    {
      Session.ContactManager.UpdateNoteForContact(contactId, ContactType.BizPartner, note);
    }

    protected override void deleteNote(int contactId, int noteId)
    {
      Session.ContactManager.DeleteNoteForContact(contactId, ContactType.BizPartner, noteId);
    }
  }
}

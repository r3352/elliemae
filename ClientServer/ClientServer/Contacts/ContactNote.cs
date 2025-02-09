// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.ContactNote
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class ContactNote
  {
    private int noteId = -1;
    private string subject;
    private DateTime timestamp;
    private string details;

    public ContactNote(int noteId, string subject, DateTime timestamp, string details)
    {
      this.noteId = noteId;
      this.subject = subject;
      this.timestamp = timestamp;
      this.details = details;
    }

    public ContactNote(string subject, string details)
      : this(-1, subject, DateTime.Now, details)
    {
    }

    public ContactNote(int noteId, ContactNote source)
      : this(noteId, source.subject, source.timestamp, source.details)
    {
    }

    public ContactNote()
      : this("", "")
    {
    }

    public int NoteID => this.noteId;

    public string Subject
    {
      get => this.subject;
      set => this.subject = value;
    }

    public DateTime Timestamp
    {
      get => this.timestamp;
      set => this.timestamp = value;
    }

    public string Details
    {
      get => this.details;
      set => this.details = value;
    }
  }
}

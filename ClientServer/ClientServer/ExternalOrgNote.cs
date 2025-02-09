// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOrgNote
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ExternalOrgNote
  {
    private int noteID = -1;
    private int externalCompanyID = -1;
    private string whoAdded = string.Empty;
    private DateTime addedDateTime = DateTime.MinValue;
    private string notesDetails = string.Empty;

    public ExternalOrgNote()
    {
    }

    public ExternalOrgNote(string whoAdded, string notesDetails)
    {
      this.whoAdded = whoAdded;
      this.notesDetails = notesDetails;
    }

    public int NoteID
    {
      get => this.noteID;
      set => this.noteID = value;
    }

    public int ExternalCompanyID
    {
      get => this.externalCompanyID;
      set => this.externalCompanyID = value;
    }

    public string WhoAdded
    {
      get => this.whoAdded;
      set => this.whoAdded = value;
    }

    public DateTime AddedDateTime
    {
      get => this.addedDateTime;
      set => this.addedDateTime = value;
    }

    public string NotesDetails
    {
      get => this.notesDetails;
      set => this.notesDetails = value;
    }
  }
}

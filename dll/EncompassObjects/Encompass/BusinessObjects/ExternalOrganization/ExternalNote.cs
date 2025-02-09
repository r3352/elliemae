// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalNote
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class ExternalNote : IExternalNote
  {
    private ExternalOrgNote externalOrgNote;

    internal ExternalNote(ExternalOrgNote externalOrgNote)
    {
      this.externalOrgNote = externalOrgNote;
    }

    public int NoteID => this.externalOrgNote.NoteID;

    public int ExternalCompanyID
    {
      get => this.externalOrgNote.ExternalCompanyID;
      set => this.externalOrgNote.ExternalCompanyID = value;
    }

    public string WhoAdded => this.externalOrgNote.WhoAdded;

    public DateTime AddedDateTime => this.externalOrgNote.AddedDateTime;

    public string NotesDetails
    {
      get => this.externalOrgNote.NotesDetails;
      set => this.externalOrgNote.NotesDetails = value;
    }

    internal static ExternalNotesList ToList(ExternalOrgNotes notes)
    {
      ExternalNotesList list = new ExternalNotesList();
      for (int index = 0; index < notes.Count; ++index)
        list.Add(new ExternalNote(notes.GetNotesAt(index)));
      return list;
    }
  }
}

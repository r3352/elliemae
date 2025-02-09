// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalNote
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents a single External organization Note.</summary>
  public class ExternalNote : IExternalNote
  {
    private ExternalOrgNote externalOrgNote;

    internal ExternalNote(ExternalOrgNote externalOrgNote)
    {
      this.externalOrgNote = externalOrgNote;
    }

    /// <summary>Gets note id</summary>
    public int NoteID => this.externalOrgNote.NoteID;

    /// <summary>Gets or sets external organizaiton id</summary>
    public int ExternalCompanyID
    {
      get => this.externalOrgNote.ExternalCompanyID;
      set => this.externalOrgNote.ExternalCompanyID = value;
    }

    /// <summary>Gets userid of Encompass user who added the note</summary>
    public string WhoAdded => this.externalOrgNote.WhoAdded;

    /// <summary>Gets Date added</summary>
    public DateTime AddedDateTime => this.externalOrgNote.AddedDateTime;

    /// <summary>Gets or sets note details</summary>
    public string NotesDetails
    {
      get => this.externalOrgNote.NotesDetails;
      set => this.externalOrgNote.NotesDetails = value;
    }

    internal static ExternalNotesList ToList(ExternalOrgNotes notes)
    {
      ExternalNotesList list = new ExternalNotesList();
      for (int i = 0; i < notes.Count; ++i)
        list.Add(new ExternalNote(notes.GetNotesAt(i)));
      return list;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PersonaPipelineView
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PersonaPipelineView : PipelineViewBase
  {
    public const string DefaultViewName = "Default View�";
    private int personaID;
    private string personaName;
    private int sortIndex = -1;
    private PersonaPipelineViewColumnCollection columns = new PersonaPipelineViewColumnCollection();

    public PersonaPipelineView(int personaID, string name = "New View�", string personaName = null, int viewID = -1)
    {
      this.viewID = viewID;
      this.personaID = personaID;
      this.personaName = personaName;
      this.name = name;
    }

    public PersonaPipelineView(
      int viewID,
      int personaID,
      string personaName,
      string name,
      int sortIndex,
      string loanFolders,
      PipelineViewLoanOwnership ownership,
      PersonaPipelineViewColumn[] columns,
      string filterXml)
    {
      this.viewID = viewID;
      this.personaID = personaID;
      this.personaName = personaName;
      this.name = name;
      this.sortIndex = sortIndex;
      this.loanFolders = loanFolders;
      this.ownership = ownership;
      this.columns.AddRange(columns);
      this.filter = FieldFilterList.Parse(filterXml);
    }

    public int ViewID => this.viewID;

    public int PersonaID => this.personaID;

    public string PersonaName => this.personaName;

    public int SortIndex => this.sortIndex;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string LoanFolders
    {
      get => this.loanFolders;
      set => this.loanFolders = value;
    }

    public PipelineViewLoanOwnership Ownership
    {
      get => this.ownership;
      set => this.ownership = value;
    }

    public FieldFilterList Filter
    {
      get => this.filter;
      set => this.filter = value;
    }

    public PersonaPipelineViewColumnCollection Columns => this.columns;

    public PersonaPipelineView Duplicate(int copyToPersonaID)
    {
      PersonaPipelineView personaPipelineView = new PersonaPipelineView(copyToPersonaID, "New View");
      personaPipelineView.loanFolders = this.loanFolders;
      personaPipelineView.filter = this.filter;
      personaPipelineView.name = this.name;
      personaPipelineView.ownership = this.ownership;
      foreach (PersonaPipelineViewColumn column in this.columns)
        personaPipelineView.columns.Add(column.Clone());
      return personaPipelineView;
    }

    public override string ToString() => this.name;

    public override bool Equals(object obj)
    {
      return obj is PersonaPipelineView personaPipelineView && personaPipelineView.personaID == this.personaID && string.Compare(personaPipelineView.name, this.name, true) == 0;
    }

    public override int GetHashCode()
    {
      return this.personaID.GetHashCode() ^ this.name.ToLower().GetHashCode();
    }

    public int CompareTo(PersonaPipelineView other)
    {
      if (other == null)
        return 1;
      if (this.personaName != other.personaName)
        return string.Compare(this.personaName, other.personaName, true);
      return this.sortIndex != other.sortIndex ? this.sortIndex - other.sortIndex : string.Compare(this.name, other.name, true);
    }
  }
}

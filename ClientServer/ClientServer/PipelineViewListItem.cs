// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PipelineViewListItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class PipelineViewListItem : IComparable
  {
    private const string className = "PipelineViewListItem�";
    private PersonaPipelineView personaView;
    private FileSystemEntry fsEntry;
    private UserPipelineView userView;

    public PipelineViewListItem(PersonaPipelineView p) => this.personaView = p;

    public PipelineViewListItem(FileSystemEntry fsEntry) => this.fsEntry = fsEntry;

    public PipelineViewListItem(UserPipelineView userView) => this.userView = userView;

    public PersonaPipelineView PersonaView => this.personaView;

    public FileSystemEntry FileSystemEntry => this.fsEntry;

    public UserPipelineView UserView => this.userView;

    public bool Static => this.personaView != null;

    public bool IsUserView => this.userView != null;

    public override string ToString()
    {
      if (this.personaView != null)
        return this.personaView.PersonaName + " - " + this.personaView.Name;
      return this.fsEntry == null ? this.userView.Name : this.fsEntry.Name;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is PipelineViewListItem pipelineViewListItem))
        return false;
      if (this.personaView != null)
        return object.Equals((object) this.personaView, (object) pipelineViewListItem.personaView);
      return this.FileSystemEntry == null ? object.Equals((object) this.UserView, (object) pipelineViewListItem.UserView) : object.Equals((object) this.FileSystemEntry, (object) pipelineViewListItem.FileSystemEntry);
    }

    public override int GetHashCode()
    {
      if (this.personaView != null)
        return this.personaView.GetHashCode();
      return this.fsEntry == null ? this.userView.GetHashCode() : this.fsEntry.GetHashCode();
    }

    public int CompareTo(object obj)
    {
      PipelineViewListItem pipelineViewListItem = obj as PipelineViewListItem;
      if (obj == null)
        throw new Exception("Invalid value for comparison");
      if (this.Static && pipelineViewListItem.Static)
        return this.personaView.CompareTo(pipelineViewListItem.personaView);
      return !this.Static && !pipelineViewListItem.Static ? (this.fsEntry == null ? string.Compare(this.userView.Name, pipelineViewListItem.userView.Name, true) : string.Compare(this.fsEntry.Name, pipelineViewListItem.fsEntry.Name, true)) : (this.Static ? 1 : -1);
    }

    public static PipelineView GetPipelineViewForPersonaPipelineView(
      PersonaPipelineView personaView,
      ReportFieldDefContainer fieldDefs,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      PipelineView personaPipelineView = new PipelineView(personaView.ToString());
      personaPipelineView.LoanFolder = personaView.LoanFolders;
      personaPipelineView.LoanOwnership = personaView.Ownership;
      personaPipelineView.Filter = personaView.Filter;
      personaPipelineView.PersonaName = personaView.PersonaName;
      personaPipelineView.Name = personaView.PersonaName + " - " + personaView.Name;
      TableLayout tableLayout = new TableLayout();
      PipelineViewListItem pipelineViewListItem = new PipelineViewListItem(personaView);
      foreach (PersonaPipelineViewColumn column in personaView.Columns)
      {
        ReportFieldDef fieldByCriterionName = fieldDefs.GetFieldByCriterionName(column.ColumnDBName);
        if (fieldByCriterionName != null)
        {
          TableLayout.Column tableLayoutColumn = fieldByCriterionName.ToTableLayoutColumn(action);
          tableLayoutColumn.SortOrder = column.SortOrder;
          if (column.Width >= 0)
            tableLayoutColumn.Width = column.Width;
          tableLayout.AddColumn(tableLayoutColumn);
        }
        else
          Tracing.Log(Tracing.SwOutsideLoan, nameof (PipelineViewListItem), TraceLevel.Warning, "Can not find Loan Report Definition for " + column.ColumnDBName + " from Persona View (" + pipelineViewListItem.ToString() + ")");
      }
      personaPipelineView.Layout = tableLayout;
      return personaPipelineView;
    }

    public static List<PipelineView> GetPipelineViewForPersonaPipelineView(
      List<PersonaPipelineView> personaView,
      ReportFieldDefContainer fieldDefs,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      List<PipelineView> personaPipelineView = new List<PipelineView>();
      foreach (PersonaPipelineView personaView1 in personaView)
        personaPipelineView.Add(PipelineViewListItem.GetPipelineViewForPersonaPipelineView(personaView1, fieldDefs, action));
      return personaPipelineView;
    }

    public static List<PipelineView> GetPipelineViewForUserPipelineView(
      List<UserPipelineView> userViews,
      ReportFieldDefContainer fieldDefs,
      ReportFieldCommonExtension.GetFieldDefWidth action)
    {
      List<PipelineView> userPipelineView = new List<PipelineView>();
      foreach (UserPipelineView userView in userViews)
      {
        PipelineView pipelineView = new PipelineView(userView.ToString());
        pipelineView.ViewID = userView.viewID;
        pipelineView.LoanFolder = userView.LoanFolders;
        pipelineView.LoanOwnership = userView.Ownership;
        pipelineView.Filter = userView.Filter;
        pipelineView.Name = userView.Name;
        pipelineView.ExternalOrgId = userView.ExternalOrgId;
        pipelineView.LoanOrgType = userView.OrgType;
        pipelineView.IsUserView = true;
        TableLayout tableLayout = new TableLayout();
        PipelineViewListItem pipelineViewListItem = new PipelineViewListItem(userView);
        foreach (UserPipelineViewColumn column in userView.Columns)
        {
          ReportFieldDef fieldByCriterionName = fieldDefs.GetFieldByCriterionName(column.ColumnDBName);
          if (fieldByCriterionName != null)
          {
            TableLayout.Column tableLayoutColumn = fieldByCriterionName.ToTableLayoutColumn(action);
            tableLayoutColumn.SortOrder = column.SortOrder;
            tableLayoutColumn.SortPriority = column.SortPriority;
            if (column.Width >= 0)
              tableLayoutColumn.Width = column.Width;
            tableLayout.AddColumn(tableLayoutColumn);
          }
          else
            Tracing.Log(Tracing.SwOutsideLoan, nameof (PipelineViewListItem), TraceLevel.Warning, "Can not find Loan Report Definition for " + column.ColumnDBName + " from User View (" + pipelineViewListItem.ToString() + ")");
        }
        pipelineView.Layout = tableLayout;
        userPipelineView.Add(pipelineView);
      }
      return userPipelineView;
    }
  }
}

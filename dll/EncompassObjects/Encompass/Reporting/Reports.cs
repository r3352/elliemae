// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.Reports
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  public class Reports : SessionBoundObject, IReports
  {
    public const string ReportingDatabaseCanonicalPrefix = "Fields.�";
    private Session session;

    internal Reports(Session session)
      : base(session)
    {
      this.session = session;
    }

    public LoanReportCursor OpenReportCursor(
      StringList fieldsToRetrieve,
      QueryCriterion filter,
      SortCriterionList sortOrder)
    {
      return this.OpenReportCursor(false, fieldsToRetrieve, filter, sortOrder);
    }

    public LoanReportCursor OpenReportCursor(
      bool fromERDB,
      StringList fieldsToRetrieve,
      QueryCriterion filter,
      SortCriterionList sortOrder)
    {
      DataQuery dataQuery = new DataQuery((IEnumerable) fieldsToRetrieve);
      DataField dataField = new DataField("Loan.Guid");
      if (!((List<IQueryTerm>) dataQuery.Selections).Contains((IQueryTerm) dataField))
        ((List<IQueryTerm>) dataQuery.Selections).Add((IQueryTerm) dataField);
      if (filter != null)
        dataQuery.Filter = filter.Unwrap();
      if (sortOrder != null)
      {
        foreach (SortCriterion sortCriterion in (CollectionBase) sortOrder)
          ((List<SortField>) dataQuery.SortFields).Add(sortCriterion.Unwrap());
      }
      string fileName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
      return new LoanReportCursor(this.session, this.session.SessionObjects.LoanManager.OpenQuery(dataQuery, fromERDB, fileName, false));
    }

    public LoanReportCursor OpenReportCursor(StringList fieldsToRetrieve, QueryCriterion filter)
    {
      return this.OpenReportCursor(fieldsToRetrieve, filter, (SortCriterionList) null);
    }

    public LoanReportDataList SelectReportingFieldsForLoans(
      StringList loanGuids,
      StringList fieldsToRetrieve)
    {
      DataQuery dataQuery = new DataQuery((IEnumerable) fieldsToRetrieve);
      DataField dataField = new DataField("Loan.Guid");
      if (!((List<IQueryTerm>) dataQuery.Selections).Contains((IQueryTerm) dataField))
        ((List<IQueryTerm>) dataQuery.Selections).Add((IQueryTerm) dataField);
      dataQuery.Filter = (QueryCriterion) new ListValueCriterion("Loan.Guid", (Array) loanGuids.ToArray());
      QueryResult queryResult = this.session.SessionObjects.LoanManager.QueryPipeline(dataQuery, false);
      Dictionary<string, List<LoanReportData>> dictionary = new Dictionary<string, List<LoanReportData>>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      for (int index = 0; index < queryResult.RecordCount; ++index)
      {
        string key = string.Concat(queryResult[index, "Loan.Guid"]);
        if (!dictionary.ContainsKey(key))
          dictionary[key] = new List<LoanReportData>();
        dictionary[key].Add(new LoanReportData(queryResult.Columns, queryResult.GetRow(index)));
      }
      LoanReportDataList loanReportDataList = new LoanReportDataList();
      for (int index = 0; index < loanGuids.Count; ++index)
      {
        if (dictionary.ContainsKey(loanGuids[index]))
          loanReportDataList.AddRange((ICollection) dictionary[loanGuids[index]]);
      }
      return loanReportDataList;
    }

    public LoanReportData SelectReportingFieldsForLoan(string loanGuid, StringList fieldsToRetrieve)
    {
      LoanReportDataList loanReportDataList = this.SelectReportingFieldsForLoans(new StringList()
      {
        loanGuid
      }, fieldsToRetrieve);
      return loanReportDataList.Count > 0 ? loanReportDataList[0] : (LoanReportData) null;
    }

    public ReportingFieldDescriptorList GetReportingDatabaseFields()
    {
      return this.GetReportingDatabaseFields(false);
    }

    public ReportingFieldDescriptorList GetReportingDatabaseFields(bool useERDB)
    {
      LoanXDBTableList loanXdbTableList = this.Session.SessionObjects.LoanManager.GetLoanXDBTableList(useERDB);
      ReportingFieldDescriptorList reportingDatabaseFields = new ReportingFieldDescriptorList();
      foreach (LoanXDBField allField in loanXdbTableList.GetAllFields())
        reportingDatabaseFields.Add(new ReportingFieldDescriptor(allField));
      return reportingDatabaseFields;
    }
  }
}

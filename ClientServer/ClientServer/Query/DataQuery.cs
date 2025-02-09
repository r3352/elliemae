// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.DataQuery
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class DataQuery
  {
    private DataQuery.DataTermCollection selections = new DataQuery.DataTermCollection();
    private QueryCriterion filter;
    private DataQuery.SortFieldCollection sortFields = new DataQuery.SortFieldCollection();
    private DataQuery.DataTermCollection groupingTerms = new DataQuery.DataTermCollection();
    private int maxResults;
    private bool distinctRecords;
    private bool usePrefiltering = true;
    private bool excludeArchivedLoans = true;

    public DataQuery()
    {
    }

    public DataQuery(IEnumerable fields)
      : this(fields, (QueryCriterion) null)
    {
    }

    public DataQuery(IEnumerable fields, QueryCriterion filter)
    {
      foreach (object field in fields)
      {
        if (field is string)
          this.selections.Add((IQueryTerm) new DataField((string) field));
        else if (field is IQueryTerm)
          this.selections.Add((IQueryTerm) field);
      }
      this.filter = filter;
    }

    public DataQuery.DataTermCollection Selections => this.selections;

    public QueryCriterion Filter
    {
      get => this.filter;
      set => this.filter = value;
    }

    public DataQuery.SortFieldCollection SortFields => this.sortFields;

    public DataQuery.DataTermCollection GroupingTerms => this.groupingTerms;

    public int MaxNumberOfResults
    {
      get => this.maxResults;
      set => this.maxResults = value;
    }

    public bool DistinctRecordsOnly
    {
      get => this.distinctRecords;
      set => this.distinctRecords = value;
    }

    public bool UsePrefiltering
    {
      get => this.usePrefiltering;
      set => this.usePrefiltering = value;
    }

    public bool ExcludeArchivedLoans
    {
      get => this.excludeArchivedLoans;
      set => this.excludeArchivedLoans = value;
    }

    [Serializable]
    public class DataTermCollection : List<IQueryTerm>
    {
      public void AddField(string fieldName) => this.Add((IQueryTerm) new DataField(fieldName));

      public void AddExpression(string name, string expression, params string[] fields)
      {
        this.Add((IQueryTerm) new DataExpression(name, expression, fields));
      }
    }

    [Serializable]
    public class SortFieldCollection : List<SortField>
    {
      public void AddField(string fieldName, FieldSortOrder sortOrder)
      {
        this.Add(new SortField(fieldName, sortOrder));
      }

      public void AddField(string fieldName, FieldSortOrder sortOrder, DataConversion conversion)
      {
        this.Add(new SortField(fieldName, sortOrder, conversion));
      }

      public void AddExpression(
        string fieldName,
        FieldSortOrder sortOrder,
        string expression,
        params string[] fields)
      {
        this.Add(new SortField((IQueryTerm) new DataExpression(fieldName, expression, fields), sortOrder));
      }
    }
  }
}

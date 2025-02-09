// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.ReportParameters
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class ReportParameters
  {
    private List<ColumnInfo> fields = new List<ColumnInfo>();
    private FieldFilterList fieldFilters = new FieldFilterList();
    private QueryCriterion customFilter;

    public List<ColumnInfo> Fields => this.fields;

    public FieldFilterList FieldFilters
    {
      get => this.fieldFilters;
      set => this.fieldFilters = value;
    }

    public QueryCriterion CustomFilter
    {
      get => this.customFilter;
      set => this.customFilter = value;
    }

    public string[] GetSelectionFieldIDs()
    {
      List<string> stringList = new List<string>();
      foreach (ColumnInfo field in this.fields)
        stringList.Add(field.FieldID);
      return stringList.ToArray();
    }

    public string[] GetSelectionFieldCriterionNames()
    {
      List<string> stringList = new List<string>();
      foreach (ColumnInfo field in this.fields)
        stringList.Add(field.CriterionName);
      return stringList.ToArray();
    }

    public QueryCriterion CreateCombinedFilter()
    {
      QueryCriterion queryCriterion = this.fieldFilters.CreateEvaluator().ToQueryCriterion();
      if (queryCriterion == null)
        return this.customFilter;
      return this.customFilter == null ? queryCriterion : queryCriterion.And(this.customFilter);
    }
  }
}

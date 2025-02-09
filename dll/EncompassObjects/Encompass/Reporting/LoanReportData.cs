// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.LoanReportData
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  public class LoanReportData : ILoanReportData
  {
    private Dictionary<string, object> loanData = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    internal LoanReportData(QueryResult.ColumnNameCollection columns, object[] data)
    {
      for (int index = 0; index < columns.Count; ++index)
        this.loanData[columns[index]] = data[index];
    }

    internal LoanReportData(PipelineInfo pinfo)
    {
      foreach (DictionaryEntry dictionaryEntry in pinfo.Info)
      {
        string key = dictionaryEntry.Key.ToString();
        if (!key.Contains("."))
          key = "Loan." + key;
        this.loanData[key] = dictionaryEntry.Value;
      }
    }

    public string Guid => string.Concat(this.loanData["Loan.Guid"]);

    public object this[string fieldName]
    {
      get
      {
        return this.loanData.ContainsKey(fieldName) ? this.loanData[fieldName] : throw new ArgumentException("The specified field is not included in the result set.");
      }
    }

    string ILoanReportData.this[string fieldName] => string.Concat(this[fieldName]);

    public StringList GetFieldNames()
    {
      StringList fieldNames = new StringList();
      foreach (string key in this.loanData.Keys)
        fieldNames.Add(key);
      return fieldNames;
    }

    internal static LoanReportDataList ToList(
      QueryResult.ColumnNameCollection columns,
      object[] data)
    {
      LoanReportDataList list = new LoanReportDataList();
      for (int index = 0; index < data.Length; ++index)
        list.Add(new LoanReportData(columns, (object[]) data[index]));
      return list;
    }

    internal static LoanReportDataList ToList(PipelineInfo[] pinfos)
    {
      LoanReportDataList list = new LoanReportDataList();
      for (int index = 0; index < pinfos.Length; ++index)
        list.Add(new LoanReportData(pinfos[index]));
      return list;
    }
  }
}

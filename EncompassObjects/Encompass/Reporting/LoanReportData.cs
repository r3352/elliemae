// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.LoanReportData
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  /// <summary>
  /// Provides access to the data for a single loan returned by running a report.
  /// </summary>
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

    /// <summary>
    /// Gets the GUID of the loan associated with this data object.
    /// </summary>
    public string Guid => string.Concat(this.loanData["Loan.Guid"]);

    /// <summary>
    /// Retrieves the value for a field included in the report
    /// </summary>
    /// <param name="fieldName">The name of the field to be retrieved.</param>
    /// <returns>The value of the field.</returns>
    public object this[string fieldName]
    {
      get
      {
        return this.loanData.ContainsKey(fieldName) ? this.loanData[fieldName] : throw new ArgumentException("The specified field is not included in the result set.");
      }
    }

    /// <summary>
    /// Provides an implementation of the ILoanReportData.this method
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    /// <exclude />
    string ILoanReportData.this[string fieldName] => string.Concat(this[fieldName]);

    /// <summary>
    /// Provides a list of all of the available fields in the data object.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.StringList" /> containing all of the field names
    /// for which this data object contains a value.</returns>
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

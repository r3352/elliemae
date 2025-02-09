// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.AnalyzersCleanupFromLoanExternalTableTask
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Server.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  public class AnalyzersCleanupFromLoanExternalTableTask : ITaskHandler
  {
    private const string className = "AnalyzersCleanupFromLoanExternalTableTask�";

    public void ProcessTask(ServerTask taskInfo)
    {
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        bool flag = true;
        if (taskInfo != null && !string.IsNullOrEmpty(taskInfo.Data))
        {
          string[] strArray = taskInfo.Data.Split('|');
          flag = this.IsTimeOfDayBetween(DateTime.Now, new TimeSpan(Convert.ToInt32(strArray[0]), 0, 0), new TimeSpan(Convert.ToInt32(strArray[1]), 59, 59));
        }
        if (!flag)
          return;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("(select distinct columns.COLUMN_NAME, columns.TABLE_NAME, DATA_TYPE as datatype from (select TableName, FieldID from LoanXDBFieldList where FieldID like '%Analyzer%') tbl inner join INFORMATION_SCHEMA.COLUMNS columns on tbl.TableName = columns.TABLE_NAME and (COLUMN_NAME='_analyzer_x1' OR COLUMN_NAME='_analyzer_x2' OR COLUMN_NAME='_analyzer_x3' OR COLUMN_NAME='_analyzer_x4' OR COLUMN_NAME='_analyzer_x5' OR COLUMN_NAME='_analyzer_x6'))");
        DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable1 == null || dataTable1.Rows.Count <= 0)
          return;
        dbQueryBuilder.Reset();
        Dictionary<object, IEnumerable<\u003C\u003Ef__AnonymousType1<object, object>>> dictionary = dataTable1.Rows.Cast<DataRow>().GroupBy<DataRow, object>((System.Func<DataRow, object>) (w => w["TABLE_NAME"])).ToDictionary<IGrouping<object, DataRow>, object, IEnumerable<\u003C\u003Ef__AnonymousType1<object, object>>>((System.Func<IGrouping<object, DataRow>, object>) (key => key.Key), value => value.Select(s => new
        {
          Column = s["COLUMN_NAME"],
          DataType = s["datatype"]
        }));
        int count = dictionary.Count;
        dbQueryBuilder.AppendLine("DECLARE @tempTable TABLE ( ID INT );");
        dbQueryBuilder.AppendLine("update lef set EXT_CreditAnalyzerEligible = null, EXT_CreditAnalyzerExceptions=null, \r\n                            EXT_CreditAnalyzerStatus = null, EXT_IncomeAnalyzerEligible = null,\r\n                            EXT_IncomeAnalyzerExceptions = null, EXT_IncomeAnalyzerStatus = null\r\n                            OUTPUT inserted.xrefid into @tempTable\r\n                            from LoanExternalFieldsStringValues lef inner\r\n                            join (");
        foreach (KeyValuePair<object, IEnumerable<\u003C\u003Ef__AnonymousType1<object, object>>> keyValuePair in dictionary)
        {
          dbQueryBuilder.AppendLine("select XRefId from " + keyValuePair.Key + " where ");
          int num = keyValuePair.Value.Count();
          foreach (var data in keyValuePair.Value)
          {
            if (data.DataType.ToString() == "decimal")
              dbQueryBuilder.Append(data.Column.ToString() + " > 0");
            else if (data.DataType.ToString() == "varchar")
              dbQueryBuilder.Append("(" + data.Column.ToString() + " is not null and " + data.Column.ToString() + " != '')");
            if (num-- > 1)
              dbQueryBuilder.Append(" or ");
            else
              dbQueryBuilder.AppendLine("");
          }
          if (count-- > 1)
            dbQueryBuilder.AppendLine(" union all ");
        }
        dbQueryBuilder.AppendLine(") as tbl on tbl.XrefId = lef.XrefId where lef.IsMigrated=0;");
        dbQueryBuilder.AppendLine("select ls.guid, ls.LoanNumber, ls.LoanFolder, ls.BorrowerLastName, ls.BorrowerFirstName, ls.Address1, ls.LoanSource from LoanSummary ls inner join @tempTable temp on ls.xrefid=temp.id");
        DataTable dataTable2 = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable2 != null && dataTable2.Rows.Count > 0)
        {
          List<LoanFileAuditRecord> loanFileAuditRecordList = new List<LoanFileAuditRecord>(dataTable2.Rows.Count);
          foreach (DataRow row in (InternalDataCollectionBase) dataTable2.Rows)
          {
            string loanGuid = row["Guid"].ToString();
            string loanNumber = row["LoanNumber"].ToString();
            string loanFolder = row["LoanFolder"].ToString();
            string borrowerLastName = row["BorrowerLastName"].ToString();
            string borrowerFirstName = row["BorrowerFirstName"].ToString();
            string address = row["Address1"].ToString();
            string fileSource = row["LoanSource"].ToString();
            loanFileAuditRecordList.Add(new LoanFileAuditRecord("<system>", "<system>", ActionType.LoanExternalFieldsDeleted, DateTime.Now, loanGuid, loanFolder, loanNumber, borrowerLastName, borrowerFirstName, address, fileSource, "Encompass SmartClient"));
          }
          SystemAuditTrailAccessor.InsertAuditRecords((SystemAuditRecord[]) loanFileAuditRecordList.ToArray());
        }
        current.TraceLog.WriteWarning(nameof (AnalyzersCleanupFromLoanExternalTableTask), "AnalyzersCleanupFromLoanExternalTableTask: completed the migration, rows affected: " + (object) (dataTable2 != null ? dataTable2.Rows.Count : 0));
      }
      catch (Exception ex)
      {
        current.TraceLog.WriteError(nameof (AnalyzersCleanupFromLoanExternalTableTask), "Error during AnalyzersCleanupFromLoanExternalTableTask : Stack Trace : " + ex.ToString());
      }
    }

    private bool IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)
    {
      if (endTime == startTime)
        return true;
      return endTime < startTime ? time.TimeOfDay <= endTime || time.TimeOfDay >= startTime : time.TimeOfDay >= startTime && time.TimeOfDay <= endTime;
    }
  }
}

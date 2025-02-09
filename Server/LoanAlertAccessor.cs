// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanAlertAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanAlertAccessor
  {
    private const string LoanSummaryTableName = "LoanSummary�";
    private const string LoanAlertsTableName = "LoanAlerts�";

    public List<PipelineInfo.Alert> GetLoanAlerts(Guid loanId)
    {
      int? loanXrefId = this.GetLoanXrefId(loanId);
      if (!loanXrefId.HasValue)
        return (List<PipelineInfo.Alert>) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT * FROM LoanAlerts WHERE LoanXRefId = " + SQL.Encode((object) loanXrefId);
      dbQueryBuilder.AppendLine(text);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      return dataTable == null || dataTable.Rows.Count < 1 ? (List<PipelineInfo.Alert>) null : this.MapDataRowCollectionToAlertObject(dataTable.Rows);
    }

    public int? GetLoanXrefId(Guid loanID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT XRefID FROM LoanSummary WHERE Guid = " + SQL.Encode((object) loanID.ToString("B")));
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      return dataTable == null || dataTable.Rows.Count < 1 ? new int?() : new int?(Convert.ToInt32(dataTable.Rows[0]["XRefID"]));
    }

    private List<PipelineInfo.Alert> MapDataRowCollectionToAlertObject(DataRowCollection Rows)
    {
      List<PipelineInfo.Alert> alertObject = new List<PipelineInfo.Alert>();
      foreach (DataRow row in (InternalDataCollectionBase) Rows)
        alertObject.Add(new PipelineInfo.Alert()
        {
          AlertTargetID = (string) SQL.Decode(row["UniqueID"]),
          AlertID = SQL.DecodeInt(row["AlertType"]),
          Event = string.Concat(row["Event"]),
          Status = string.Concat(row["status"]),
          Date = SQL.DecodeDateTime(row["AlertDate"]),
          GroupID = SQL.DecodeInt(row["GroupID"], -1),
          DisplayStatus = SQL.DecodeInt(row["DisplayStatus"], 1),
          SnoozeStartDTTM = SQL.DecodeDateTime(row["SnoozeStartDTTM"], DateTime.MinValue),
          SnoozeDuration = SQL.DecodeInt(row["SnoozeDuration"], 0),
          LoanAlertID = string.Concat(row["LoanAlertId"]),
          UserID = SQL.DecodeString(row["UserID"])
        });
      return alertObject;
    }
  }
}

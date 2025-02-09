// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.EnhancedConditionReportingAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class EnhancedConditionReportingAccessor
  {
    private const string EnhancedConditionTable = "EnhancedCondition�";
    private const string EnhancedConditionDocumentTable = "EnhancedConditionDocument�";
    private const string EnhancedConditionTrackingTable = "EnhancedConditionTracking�";
    private static readonly string _className = "EnhancedConditionListAccessor";

    public static List<string> GetLoanEnhancedConditionIds(string guid)
    {
      try
      {
        List<string> enhancedConditionIds = new List<string>();
        if (string.IsNullOrWhiteSpace(guid))
          return enhancedConditionIds;
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT ec.id AS Id FROM EnhancedCondition ec ");
        dbQueryBuilder.AppendLine("INNER JOIN LoanSummary ls ON ls.XRefId = ec.LoanXRefId ");
        dbQueryBuilder.AppendLine("WHERE ls.Guid = " + SQL.Encode((object) guid));
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          enhancedConditionIds.Add(dataRow["Id"].ToString());
        return enhancedConditionIds;
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionReportingAccessor._className, ex);
      }
      return (List<string>) null;
    }

    public static void GetAddUpdateEnhancedConditionsSQL(
      ref EllieMae.EMLite.Server.DbQueryBuilder sql,
      EnhancedConditionLog[] enhancedConditions,
      out string errorMessage,
      int loanXRefId)
    {
      sql = sql ?? new EllieMae.EMLite.Server.DbQueryBuilder();
      errorMessage = string.Empty;
      if (enhancedConditions == null || enhancedConditions.Length == 0)
        return;
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("EnhancedCondition");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("EnhancedConditionDocument");
      DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("EnhancedConditionTracking");
      StringBuilder errorList = new StringBuilder();
      foreach (EnhancedConditionLog enhancedCondition in enhancedConditions)
      {
        string guid = enhancedCondition.Guid;
        DbValueList key = new DbValueList();
        key.Add("LoanXRefId", (object) loanXRefId);
        key.Add("Id", (object) guid);
        DbValueList values1 = EnhancedConditionReportingAccessor.MapEnhancedConditionDBValueList(enhancedCondition, ref errorList);
        sql.Upsert(table1, values1, key);
        DbValueList keys = new DbValueList();
        keys.Add("LoanXRef", (object) loanXRefId);
        keys.Add("EnhancedConditionId", (object) guid);
        sql.DeleteFrom(table2, keys);
        foreach (DocumentLog linkedDocument in enhancedCondition.GetLinkedDocuments(false))
        {
          DbValueList values2 = EnhancedConditionReportingAccessor.MapEnhancedConditionDocumentDBValueList(guid, linkedDocument.Guid, loanXRefId);
          sql.InsertInto(table2, values2, true, false);
        }
        sql.DeleteFrom(table3, keys);
        foreach (StatusTrackingEntry statusTrackingEntry in enhancedCondition.Trackings.GetStatusTrackingEntries())
        {
          DbValueList values3 = EnhancedConditionReportingAccessor.MapEnhancedConditionTrackingDBValueList(guid, statusTrackingEntry, ref errorList, loanXRefId);
          sql.InsertInto(table3, values3, true, false);
        }
        if (errorList != null && errorList.Length > 0)
          errorMessage = errorMessage + "Enhanced condition '" + enhancedCondition.Guid.ToString() + "' has warnings - " + errorList.ToString();
      }
      if (!string.IsNullOrEmpty(errorMessage))
        return;
      errorMessage = (string) null;
    }

    private static DbValueList MapEnhancedConditionDBValueList(
      EnhancedConditionLog enhancedCondition,
      ref StringBuilder errorList,
      int loanXRefId = -1,
      bool forCreate = false)
    {
      DbValueList dbValueList1 = new DbValueList();
      if (forCreate)
      {
        dbValueList1.Add("LoanXRefId", (object) loanXRefId, (IDbEncoder) DbEncoding.None);
        dbValueList1.Add("Id", (object) enhancedCondition.Guid.ToString());
      }
      dbValueList1.Add("ConditionType", (object) enhancedCondition.EnhancedConditionType);
      dbValueList1.Add("Title", (object) enhancedCondition.Title);
      dbValueList1.Add("InternalId", (object) enhancedCondition.InternalId);
      dbValueList1.Add("InternalDescription", (object) enhancedCondition.InternalDescription);
      dbValueList1.Add("InternalPrint", (object) enhancedCondition.InternalPrint, (IDbEncoder) DbEncoding.YesNo);
      dbValueList1.Add("ExternalId", (object) enhancedCondition.ExternalId);
      dbValueList1.Add("ExternalDescription", (object) enhancedCondition.ExternalDescription);
      dbValueList1.Add("ExternalPrint", (object) enhancedCondition.ExternalPrint, (IDbEncoder) DbEncoding.YesNo);
      DateTime? nullable = enhancedCondition.ExternalPrintDate;
      if (nullable.HasValue)
      {
        nullable = enhancedCondition.ExternalPrintDate;
        DateTime minValue = DateTime.MinValue;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0)
        {
          nullable = enhancedCondition.ExternalPrintDate;
          DateTime maxValue = DateTime.MaxValue;
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() < maxValue ? 1 : 0) : 0) != 0)
          {
            errorList.AppendLine(EnhancedConditionReportingAccessor.ValidateDateKind(enhancedCondition.ExternalPrintDate, "ExternalPrintDate"));
            DbValueList dbValueList2 = dbValueList1;
            nullable = enhancedCondition.ExternalPrintDate;
            string str = nullable.ToString();
            dbValueList2.Add("ExternalPrintDate", (object) str);
          }
        }
      }
      dbValueList1.Add("Source", (object) enhancedCondition.Source);
      dbValueList1.Add("ApplicationId", (object) enhancedCondition.PairId);
      dbValueList1.Add("Category", (object) enhancedCondition.Category);
      dbValueList1.Add("PriorTo", (object) enhancedCondition.PriorTo);
      dbValueList1.Add("Recipient", (object) enhancedCondition.Recipient);
      dbValueList1.Add("SourceOfCondition", (object) enhancedCondition.SourceOfCondition.ToString());
      nullable = enhancedCondition.StartDate;
      if (nullable.HasValue)
      {
        nullable = enhancedCondition.StartDate;
        DateTime minValue = DateTime.MinValue;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0)
        {
          nullable = enhancedCondition.StartDate;
          DateTime maxValue = DateTime.MaxValue;
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() < maxValue ? 1 : 0) : 0) != 0)
          {
            errorList.AppendLine(EnhancedConditionReportingAccessor.ValidateDateKind(enhancedCondition.StartDate, "StartDate"));
            DbValueList dbValueList3 = dbValueList1;
            nullable = enhancedCondition.StartDate;
            string str = nullable.ToString();
            dbValueList3.Add("StartDate", (object) str);
          }
        }
      }
      nullable = enhancedCondition.EndDate;
      if (nullable.HasValue)
      {
        nullable = enhancedCondition.EndDate;
        DateTime minValue = DateTime.MinValue;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0)
        {
          nullable = enhancedCondition.EndDate;
          DateTime maxValue = DateTime.MaxValue;
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() < maxValue ? 1 : 0) : 0) != 0)
          {
            errorList.AppendLine(EnhancedConditionReportingAccessor.ValidateDateKind(enhancedCondition.EndDate, "EndDate"));
            DbValueList dbValueList4 = dbValueList1;
            nullable = enhancedCondition.EndDate;
            string str = nullable.ToString();
            dbValueList4.Add("EndDate", (object) str);
          }
        }
      }
      dbValueList1.Add("RequestedFrom", (object) enhancedCondition.RequestedFrom);
      dbValueList1.Add("DaysToRecieve", (object) enhancedCondition.DaysToReceive);
      dbValueList1.Add("Owner", (object) enhancedCondition.Owner);
      dbValueList1.Add("Partner", (object) enhancedCondition.Partner);
      nullable = enhancedCondition.PublishedDate;
      if (nullable.HasValue)
      {
        nullable = enhancedCondition.PublishedDate;
        DateTime minValue = DateTime.MinValue;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0)
        {
          nullable = enhancedCondition.PublishedDate;
          DateTime maxValue = DateTime.MaxValue;
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() < maxValue ? 1 : 0) : 0) != 0)
          {
            errorList.AppendLine(EnhancedConditionReportingAccessor.ValidateDateKind(enhancedCondition.PublishedDate, "PublishedDate"));
            DbValueList dbValueList5 = dbValueList1;
            nullable = enhancedCondition.PublishedDate;
            string str = nullable.ToString();
            dbValueList5.Add("PublishedDate", (object) str);
          }
        }
      }
      dbValueList1.Add("Status", (object) enhancedCondition.Status);
      nullable = enhancedCondition.StatusDate;
      if (nullable.HasValue)
      {
        nullable = enhancedCondition.StatusDate;
        DateTime minValue = DateTime.MinValue;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0)
        {
          nullable = enhancedCondition.StatusDate;
          DateTime maxValue = DateTime.MaxValue;
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() < maxValue ? 1 : 0) : 0) != 0)
          {
            errorList.AppendLine(EnhancedConditionReportingAccessor.ValidateDateKind(enhancedCondition.StatusDate, "StatusDate"));
            DbValueList dbValueList6 = dbValueList1;
            nullable = enhancedCondition.StatusDate;
            string str = nullable.ToString();
            dbValueList6.Add("StatusDate", (object) str);
          }
        }
      }
      dbValueList1.Add("StatusOpen", (object) enhancedCondition.StatusOpen, (IDbEncoder) DbEncoding.YesNo);
      if (enhancedCondition.Comments != null)
        dbValueList1.Add("CommentsCount", (object) enhancedCondition.Comments.Count);
      errorList.AppendLine(EnhancedConditionReportingAccessor.ValidateDateKind(new DateTime?(enhancedCondition.DateAdded), "CreatedDate"));
      dbValueList1.Add("CreatedDate", (object) enhancedCondition.DateAdded.ToString());
      dbValueList1.Add("CreatedBy", (object) enhancedCondition.AddedBy);
      dbValueList1.Add("LastModifiedDate", (object) DateTimeOffset.UtcNow.ToString());
      dbValueList1.Add("LastModifiedBy", (object) enhancedCondition.UpdatedBy);
      dbValueList1.Add("IsRemoved", (object) enhancedCondition.IsRemoved, (IDbEncoder) DbEncoding.YesNo);
      nullable = enhancedCondition.AgeStartDate;
      if (nullable.HasValue)
      {
        nullable = enhancedCondition.AgeStartDate;
        DateTime minValue = DateTime.MinValue;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0)
        {
          nullable = enhancedCondition.AgeStartDate;
          DateTime maxValue = DateTime.MaxValue;
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() < maxValue ? 1 : 0) : 0) != 0)
          {
            errorList.AppendLine(EnhancedConditionReportingAccessor.ValidateDateKind(enhancedCondition.AgeStartDate, "AgeStartDate"));
            DbValueList dbValueList7 = dbValueList1;
            nullable = enhancedCondition.AgeStartDate;
            string str = nullable.ToString();
            dbValueList7.Add("AgeStartDate", (object) str);
          }
        }
      }
      nullable = enhancedCondition.AgeClosedDate;
      if (nullable.HasValue)
      {
        nullable = enhancedCondition.AgeClosedDate;
        DateTime minValue = DateTime.MinValue;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0)
        {
          nullable = enhancedCondition.AgeClosedDate;
          DateTime maxValue = DateTime.MaxValue;
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() < maxValue ? 1 : 0) : 0) != 0)
          {
            errorList.AppendLine(EnhancedConditionReportingAccessor.ValidateDateKind(enhancedCondition.AgeClosedDate, "AgeClosedDate"));
            DbValueList dbValueList8 = dbValueList1;
            nullable = enhancedCondition.AgeClosedDate;
            string str = nullable.ToString();
            dbValueList8.Add("AgeClosedDate", (object) str);
          }
        }
      }
      nullable = enhancedCondition.DocumentReceiptDate;
      if (nullable.HasValue)
      {
        nullable = enhancedCondition.DocumentReceiptDate;
        DateTime minValue = DateTime.MinValue;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0)
        {
          nullable = enhancedCondition.DocumentReceiptDate;
          DateTime maxValue = DateTime.MaxValue;
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() < maxValue ? 1 : 0) : 0) != 0)
          {
            errorList.AppendLine(EnhancedConditionReportingAccessor.ValidateDateKind(enhancedCondition.DocumentReceiptDate, "DocumentReceiptDate"));
            DbValueList dbValueList9 = dbValueList1;
            nullable = enhancedCondition.DocumentReceiptDate;
            string str = nullable.ToString();
            dbValueList9.Add("DocumentReceiptDate", (object) str);
          }
        }
      }
      return dbValueList1;
    }

    private static string ValidateDateKind(DateTime? date, string fieldName)
    {
      return (date.HasValue ? (date.GetValueOrDefault().Kind != DateTimeKind.Utc ? 1 : 0) : 1) != 0 ? fieldName + " '" + date.ToString() + "' does not have kind UTC." : string.Empty;
    }

    private static DbValueList MapEnhancedConditionDocumentDBValueList(
      string enhancedConditionId,
      string documentId,
      int loanXRefId = -1)
    {
      return new DbValueList()
      {
        {
          "LoanXRef",
          (object) loanXRefId,
          (IDbEncoder) DbEncoding.None
        },
        {
          "EnhancedConditionId",
          (object) enhancedConditionId
        },
        {
          "DocumentId",
          (object) documentId
        }
      };
    }

    private static DbValueList MapEnhancedConditionTrackingDBValueList(
      string enhancedConditionId,
      StatusTrackingEntry statusTracking,
      ref StringBuilder errorList,
      int loanXRefId = -1)
    {
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("LoanXRef", (object) loanXRefId, (IDbEncoder) DbEncoding.None);
      dbValueList.Add("EnhancedConditionId", (object) enhancedConditionId);
      dbValueList.Add("UserId", (object) statusTracking.UserId);
      dbValueList.Add("Status", (object) statusTracking.Status);
      errorList.AppendLine(EnhancedConditionReportingAccessor.ValidateDateKind(new DateTime?(statusTracking.Date), "StatusDate"));
      dbValueList.Add("StatusDate", (object) statusTracking.Date.ToString());
      return dbValueList;
    }

    public static void GetDeleteEnhancedConditionForLoanSQL(ref EllieMae.EMLite.Server.DbQueryBuilder sql, int loanXRefId)
    {
      sql = sql ?? new EllieMae.EMLite.Server.DbQueryBuilder();
      if (loanXRefId <= 0)
        return;
      string str = SQL.Encode((object) loanXRefId);
      sql.AppendLine("DECLARE @enhancedcondition_guids TABLE(GUID VARCHAR(38) PRIMARY KEY)");
      sql.AppendLine("INSERT INTO @enhancedcondition_guids SELECT Id FROM [EnhancedCondition] WHERE LoanXrefID = " + str);
      sql.AppendLine("DELETE FROM [EnhancedConditionTracking] WHERE LoanXref = " + str + " AND EnhancedConditionId IN (SELECT Guid FROM @enhancedCondition_guids)");
      sql.AppendLine("DELETE FROM [EnhancedConditionDocument] WHERE LoanXref = " + str + " AND EnhancedConditionId IN (SELECT Guid FROM @enhancedCondition_guids)");
      sql.AppendLine("DELETE FROM [EnhancedCondition] WHERE LoanXrefID = " + str);
    }
  }
}

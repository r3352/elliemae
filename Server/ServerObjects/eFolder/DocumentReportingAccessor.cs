// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.DocumentReportingAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class DocumentReportingAccessor
  {
    private const string DocumentTable = "Document�";
    private static readonly string _className = "DocumentListAccessor";

    public static List<string> GetLoanDocumentIds(string guid)
    {
      try
      {
        List<string> loanDocumentIds = new List<string>();
        if (string.IsNullOrWhiteSpace(guid))
          return loanDocumentIds;
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT doc.id AS Id FROM Document doc ");
        dbQueryBuilder.AppendLine("INNER JOIN LoanSummary ls ON ls.XRefId = doc.LoanXRefId ");
        dbQueryBuilder.AppendLine("WHERE ls.Guid = " + SQL.Encode((object) guid));
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          loanDocumentIds.Add(dataRow["Id"].ToString());
        return loanDocumentIds;
      }
      catch (Exception ex)
      {
        Err.Reraise(DocumentReportingAccessor._className, ex);
      }
      return (List<string>) null;
    }

    public static void GetAddUpdateDocumentsSQL(
      ref EllieMae.EMLite.Server.DbQueryBuilder sql,
      DocumentLog[] documents,
      int loanXRefId)
    {
      sql = sql ?? new EllieMae.EMLite.Server.DbQueryBuilder();
      if (documents == null || documents.Length == 0)
        return;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("Document");
      foreach (DocumentLog document in documents)
      {
        if (document != null)
        {
          DbValueList key = new DbValueList();
          key.Add("LoanXRefId", (object) loanXRefId);
          key.Add("Id", (object) document.Guid.ToString());
          DbValueList values = DocumentReportingAccessor.MapDocumentDBValueList(document);
          sql.Upsert(table, values, key);
        }
      }
    }

    private static DbValueList MapDocumentDBValueList(
      DocumentLog document,
      int loanXRefId = -1,
      bool forCreate = false)
    {
      DbValueList dbValueList = new DbValueList();
      if (forCreate)
      {
        dbValueList.Add("LoanXRefId", (object) loanXRefId, (IDbEncoder) DbEncoding.None);
        dbValueList.Add("Id", (object) document.Guid.ToString());
      }
      dbValueList.Add("Title", (object) document.Title);
      dbValueList.Add("Description", (object) document.Description);
      dbValueList.Add("ApplicationId", (object) document.PairId);
      dbValueList.Add("CreatedBy", (object) document.AddedBy);
      dbValueList.Add("CreatedDate", (object) document.DateAdded, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("RequestedBy", (object) document.RequestedBy);
      dbValueList.Add("RequestedDate", (object) document.DateRequested, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("RerequestedBy", (object) document.RerequestedBy);
      dbValueList.Add("RerequestedDate", (object) document.DateRerequested, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("DaysDue", (object) document.DaysDue);
      dbValueList.Add("ExpectedDate", (object) document.DateExpected, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("ReceivedBy", (object) document.ReceivedBy);
      dbValueList.Add("ReceivedDate", (object) document.DateReceived, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("DaysTillExpire", (object) document.DaysTillExpire);
      dbValueList.Add("ExpirationDate", (object) document.DateExpires, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("Status", (object) document.Status);
      dbValueList.Add("StatusDate", (object) document.Date, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("ArchivedBy", (object) document.ArchivedBy);
      dbValueList.Add("ArchivedDate", (object) document.DateArchived, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("ReviewedBy", (object) document.ReviewedBy);
      dbValueList.Add("ReviewedDate", (object) document.DateReviewed, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("UnderwritingReadyBy", (object) document.UnderwritingReadyBy);
      dbValueList.Add("UnderwritingReadyDate", (object) document.DateUnderwritingReady, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("ShippingReadyBy", (object) document.ShippingReadyBy);
      dbValueList.Add("ShippingReadyDate", (object) document.DateShippingReady, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("AccessedBy", (object) document.AccessedBy);
      dbValueList.Add("AccessedDate", (object) document.DateAccessed, (IDbEncoder) DbEncoding.DateTime);
      dbValueList.Add("IsRemoved", (object) document.IsRemoved, (IDbEncoder) DbEncoding.YesNo);
      return dbValueList;
    }

    public static void GetDeleteDocumentForLoanSQL(ref EllieMae.EMLite.Server.DbQueryBuilder sql, int loanXRefId)
    {
      sql = sql ?? new EllieMae.EMLite.Server.DbQueryBuilder();
      if (loanXRefId <= 0)
        return;
      sql.AppendLine("DELETE FROM [Document] WHERE LoanXrefID = " + SQL.Encode((object) loanXRefId));
    }
  }
}

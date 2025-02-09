// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TrusteeQueryStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class TrusteeQueryStore
  {
    private const string className = "TrusteeQueryStore�";

    public static int AddTrusteeRecord(TrusteeRecord trusteeRecord)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@recID", "int");
      TraceLog.WriteInfo(nameof (TrusteeQueryStore), "TrusteeQueryStore.AddTrusteeRecord: Creating SQL commands for table 'TrusteeList'.");
      dbQueryBuilder.AppendLine("INSERT INTO TrusteeList ([ContactName], [Address], [City], [State], [ZipCode], [County], [PhoneNumber], [TrustDate], [OrgState], [OrgType]) VALUES (" + SQL.Encode((object) trusteeRecord.ContactName) + "," + SQL.Encode((object) trusteeRecord.Address) + "," + SQL.Encode((object) trusteeRecord.City) + "," + SQL.Encode((object) trusteeRecord.State) + "," + SQL.Encode((object) trusteeRecord.ZipCode) + "," + SQL.Encode((object) trusteeRecord.County) + "," + SQL.Encode((object) trusteeRecord.Phone) + ",");
      if (trusteeRecord.TrustDate != DateTime.MinValue)
        dbQueryBuilder.AppendLine("'" + trusteeRecord.TrustDate.ToString("MM/dd/yyyy") + "',");
      else
        dbQueryBuilder.AppendLine("Null,");
      dbQueryBuilder.AppendLine(SQL.Encode((object) trusteeRecord.OrgState) + "," + SQL.Encode((object) trusteeRecord.OrgType) + ")");
      dbQueryBuilder.SelectIdentity("@recID");
      dbQueryBuilder.Select("@recID");
      try
      {
        TraceLog.WriteInfo(nameof (TrusteeQueryStore), dbQueryBuilder.ToString());
        return Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (TrusteeQueryStore), ex);
        throw new Exception("Cannot update TrusteeList table Due to the following problem:\r\n" + ex.Message);
      }
    }

    public static void DeleteTrusteeRecord(int[] ids)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        string text = "DELETE FROM TrusteeList Where [id] = " + (object) ids[0];
        if (ids.Length > 1)
        {
          for (int index = 1; index < ids.Length; ++index)
            text = text + " Or [id] = " + (object) ids[index];
        }
        dbQueryBuilder.AppendLine(text);
        TraceLog.WriteInfo(nameof (TrusteeQueryStore), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (TrusteeQueryStore), ex);
        throw new Exception("Cannot delete Trustee record from TrusteeList.\r\n" + ex.Message);
      }
    }

    public static bool HasDuplicateTrusteeRecord(int id, string contactName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        string text = "Select id from TrusteeList Where [ContactName] = " + SQL.Encode((object) contactName);
        if (id > -1)
          text = text + " And [id] <> " + (object) id;
        dbQueryBuilder.AppendLine(text);
        if (dbQueryBuilder.ExecuteScalar() is int num)
        {
          if (num > 0)
            return true;
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (TrusteeQueryStore), ex);
        throw new Exception("Cannot access TrusteeList table.\r\n" + ex.Message);
      }
      return false;
    }

    public static void UpdateTrusteeRecord(TrusteeRecord trusteeRecord)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        string str = "UPDATE TrusteeList SET [ContactName] = " + SQL.Encode((object) trusteeRecord.ContactName) + ",[Address] = " + SQL.Encode((object) trusteeRecord.Address) + ",[City] = " + SQL.Encode((object) trusteeRecord.City) + ",[State] = " + SQL.Encode((object) trusteeRecord.State) + ",[ZipCode] = " + SQL.Encode((object) trusteeRecord.ZipCode) + ",[County] = " + SQL.Encode((object) trusteeRecord.County) + ",[PhoneNumber] = " + SQL.Encode((object) trusteeRecord.Phone) + ",";
        string text = (!(trusteeRecord.TrustDate != DateTime.MinValue) ? str + "[TrustDate] = Null," : str + "[TrustDate] = '" + trusteeRecord.TrustDate.ToString("") + "',") + "[OrgState] = " + SQL.Encode((object) trusteeRecord.OrgState) + ",[OrgType] = " + SQL.Encode((object) trusteeRecord.OrgType) + " WHERE id = " + (object) trusteeRecord.Id;
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (TrusteeQueryStore), ex);
        throw new Exception("Cannot update Trustee record in TrusteeList table.\r\n" + ex.Message);
      }
    }

    public static TrusteeRecord[] GetTrusteeRecords()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT * FROM TrusteeList";
      dbQueryBuilder.AppendLine(text);
      ArrayList arrayList = new ArrayList();
      try
      {
        TraceLog.WriteInfo(nameof (TrusteeQueryStore), dbQueryBuilder.ToString());
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        for (int index = 0; index < dataTable.Rows.Count; ++index)
        {
          DataRow row = dataTable.Rows[index];
          arrayList.Add((object) new TrusteeRecord(row));
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (TrusteeQueryStore), ex);
        throw new Exception("Cannot read records from TrusteeList table.\r\n" + ex.Message);
      }
      return arrayList.Count == 0 ? (TrusteeRecord[]) null : (TrusteeRecord[]) arrayList.ToArray(typeof (TrusteeRecord));
    }
  }
}

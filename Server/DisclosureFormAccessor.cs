// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DisclosureFormAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class DisclosureFormAccessor
  {
    private static string tableName = "DisclosureForms";

    public static void UpdateDisclosureForms(
      Dictionary<string, DisclosureTrackingFormItem.FormType> formList)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete from " + DisclosureFormAccessor.tableName + " where DisclosureType = '2010'");
      foreach (string key in formList.Keys)
        dbQueryBuilder.AppendLine("Insert into " + DisclosureFormAccessor.tableName + " (formName, formType, DisclosureType) Values (" + SQL.Encode((object) key) + ", " + (object) (int) formList[key] + ", " + SQL.Encode((object) "2010") + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateDisclosure2015Forms(
      Dictionary<string, DisclosureTrackingFormItem.FormType> formList)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete from " + DisclosureFormAccessor.tableName + " where DisclosureType = '2015'");
      foreach (string key in formList.Keys)
        dbQueryBuilder.AppendLine("Insert into " + DisclosureFormAccessor.tableName + " (formName, formType, DisclosureType) Values (" + SQL.Encode((object) key) + ", " + (object) (int) formList[key] + ", " + SQL.Encode((object) "2015") + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    [PgReady]
    public static Dictionary<string, DisclosureTrackingFormItem.FormType> GetDisclosureFroms()
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("Select * from " + DisclosureFormAccessor.tableName + " where DisclosureType = '2010';");
        DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery();
        Dictionary<string, DisclosureTrackingFormItem.FormType> disclosureFroms = new Dictionary<string, DisclosureTrackingFormItem.FormType>();
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          disclosureFroms.Add(SQL.DecodeString(row["formName"]), SQL.DecodeEnum<DisclosureTrackingFormItem.FormType>(row["formType"]));
        return disclosureFroms;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from " + DisclosureFormAccessor.tableName + " where DisclosureType = '2010'");
      DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
      Dictionary<string, DisclosureTrackingFormItem.FormType> disclosureFroms1 = new Dictionary<string, DisclosureTrackingFormItem.FormType>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
        disclosureFroms1.Add(string.Concat(row["formName"]), (DisclosureTrackingFormItem.FormType) Enum.Parse(typeof (DisclosureTrackingFormItem.FormType), string.Concat(row["formType"])));
      return disclosureFroms1;
    }

    [PgReady]
    public static Dictionary<string, DisclosureTrackingFormItem.FormType> GetDisclosure2015Forms()
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("Select * from " + DisclosureFormAccessor.tableName + " where DisclosureType = '2015';");
        DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery();
        Dictionary<string, DisclosureTrackingFormItem.FormType> disclosure2015Forms = new Dictionary<string, DisclosureTrackingFormItem.FormType>();
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          disclosure2015Forms.Add(SQL.DecodeString(row["formName"]) ?? "", SQL.DecodeEnum<DisclosureTrackingFormItem.FormType>(row["formType"]));
        return disclosure2015Forms;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from " + DisclosureFormAccessor.tableName + " where DisclosureType = '2015'");
      DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
      Dictionary<string, DisclosureTrackingFormItem.FormType> disclosure2015Forms1 = new Dictionary<string, DisclosureTrackingFormItem.FormType>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
        disclosure2015Forms1.Add(string.Concat(row["formName"]), (DisclosureTrackingFormItem.FormType) Enum.Parse(typeof (DisclosureTrackingFormItem.FormType), string.Concat(row["formType"])));
      return disclosure2015Forms1;
    }
  }
}

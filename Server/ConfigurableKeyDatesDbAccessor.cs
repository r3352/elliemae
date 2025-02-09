// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ConfigurableKeyDatesDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class ConfigurableKeyDatesDbAccessor
  {
    public static ConfigurableKeyDate GetConfigurableKeyDate(string fieldId)
    {
      ConfigurableKeyDate configurableKeyDate = new ConfigurableKeyDate(string.Empty, string.Empty, string.Empty);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("Select * from ConfigurableKeyDates where FieldId = '" + fieldId + "'");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
      {
        DataRow row = dataSet.Tables[0].Rows[0];
        row["FieldId"].ToString();
        string DefaultValue = row["DefaultValue"].ToString();
        string interpretation = row["Interpretation"].ToString();
        configurableKeyDate = new ConfigurableKeyDate(fieldId, DefaultValue, interpretation);
        configurableKeyDate.FieldLabel = row["FieldLabel"].ToString();
        configurableKeyDate.ViewInTpoConnect = SQL.DecodeBoolean(row["ViewInTpoConnect"]);
      }
      return configurableKeyDate;
    }

    public static List<ConfigurableKeyDate> GetConfigurableKeyDates()
    {
      List<ConfigurableKeyDate> configurableKeyDates = new List<ConfigurableKeyDate>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("Select * from ConfigurableKeyDates");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          configurableKeyDates.Add(new ConfigurableKeyDate(row["FieldId"].ToString(), row["DefaultValue"].ToString(), row["Interpretation"].ToString())
          {
            FieldLabel = row["FieldLabel"].ToString(),
            ViewInTpoConnect = SQL.DecodeBoolean(row["ViewInTpoConnect"])
          });
      }
      return configurableKeyDates;
    }

    public static void UpdateConfigurableDate(ConfigurableKeyDate configurableKeyDates)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      int num = configurableKeyDates.ViewInTpoConnect ? 1 : 0;
      try
      {
        string str = SQL.EncodeString(configurableKeyDates.FieldLabel, false);
        stringBuilder.AppendLine("UPDATE ConfigurableKeyDates");
        stringBuilder.AppendLine("     SET FieldLabel = '" + str + "', ");
        stringBuilder.AppendLine("     ViewInTpoConnect = '" + (object) num + "'");
        stringBuilder.AppendLine("     WHERE FieldId = '" + configurableKeyDates.FieldId + "'");
        stringBuilder.AppendLine(" select @@rowcount");
        dbQueryBuilder.Append(stringBuilder.ToString());
        if (Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) <= 0)
          throw new Exception(string.Format("No entry for Field, \"{0}\"", (object) configurableKeyDates.FieldId));
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("No entry for Field, \"{0}\"", (object) configurableKeyDates.FieldId));
      }
    }
  }
}

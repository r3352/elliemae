// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanPropertySettingsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanPropertySettingsAccessor
  {
    private const string className = "LoanPropertiesAccessor�";
    private const string tableName = "LoanProperties�";

    private LoanPropertySettingsAccessor()
    {
    }

    [PgReady]
    public static LoanProperty[] GetLoanPropertySettings(string guid)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        string text = "SELECT  category, attribute, value FROM  [LoanProperties] WHERE guid='" + guid.ToString() + "'";
        pgDbQueryBuilder.AppendLine(text);
        return LoanPropertySettingsAccessor.createLoanPropertySettings(pgDbQueryBuilder.Execute());
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text1 = "SELECT  category, attribute, value FROM  [LoanProperties] WHERE guid='" + guid.ToString() + "'";
      dbQueryBuilder.AppendLine(text1);
      return LoanPropertySettingsAccessor.createLoanPropertySettings(dbQueryBuilder.Execute());
    }

    public static LoanProperty[] GetLoanPropertySettings(string guid, string category)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT  category, attribute, value FROM  [LoanProperties] WHERE guid='" + guid.ToString() + "' AND category='" + category + "'";
      dbQueryBuilder.AppendLine(text);
      return LoanPropertySettingsAccessor.createLoanPropertySettings(dbQueryBuilder.Execute());
    }

    public static void DeleteLoanPropertySettings(Guid guid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE FROM [LoanProperties] WHERE guid='" + guid.ToString() + "'");
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static DbValueList createLoanPropertySettingsDbValueList(
      string guid,
      string category,
      string attribute,
      string value)
    {
      return new DbValueList()
      {
        {
          nameof (guid),
          (object) guid.ToString()
        },
        {
          nameof (category),
          (object) category
        },
        {
          nameof (attribute),
          (object) attribute
        },
        {
          nameof (value),
          (object) value
        }
      };
    }

    private static LoanProperty[] createLoanPropertySettings(DataRowCollection rows)
    {
      List<LoanProperty> loanPropertyList = new List<LoanProperty>();
      foreach (DataRow row in (InternalDataCollectionBase) rows)
      {
        LoanProperty loanProperty = new LoanProperty(row["category"].ToString(), row["attribute"].ToString(), row["value"].ToString());
        loanPropertyList.Add(loanProperty);
      }
      return loanPropertyList.ToArray();
    }

    public static bool Exists(string guid, LoanProperty lp)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT  category, attribute, value FROM  [LoanProperties] WHERE guid='" + guid.ToString() + "' and category='" + lp.Category + "' and attribute='" + lp.Attribute + "'";
      dbQueryBuilder.AppendLine(text);
      return dbQueryBuilder.Execute().Count > 0;
    }

    public static void AppendUpdateRecord(string guid, LoanProperty lp)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("category", (object) lp.Category);
      dbValueList.Add("attribute", (object) lp.Attribute);
      dbValueList.Add(nameof (guid), (object) guid);
      DbTableInfo table = DbAccessManager.GetTable("LoanProperties");
      dbQueryBuilder.DeleteFrom(table, dbValueList);
      dbValueList.Add("value", (object) lp.Value);
      dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      dbQueryBuilder.Execute();
    }
  }
}

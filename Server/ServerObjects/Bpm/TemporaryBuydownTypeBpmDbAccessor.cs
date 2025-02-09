// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.TemporaryBuydownTypeBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class TemporaryBuydownTypeBpmDbAccessor
  {
    private const string buydownTable = "TemporaryBuydownType�";

    public static List<TemporaryBuydown> GetAllTemporaryBuydowns()
    {
      List<TemporaryBuydown> temporaryBuydowns = new List<TemporaryBuydown>();
      string text = "select * from TemporaryBuydownType order by LastModifiedDateTime desc";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(text);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable == null)
        return temporaryBuydowns;
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        TemporaryBuydown buydown = TemporaryBuydownTypeBpmDbAccessor.convertDataRowToBuydown(dataTable.Rows[index]);
        temporaryBuydowns.Add(buydown);
      }
      return temporaryBuydowns;
    }

    public static void CreateTemporaryBuydownType(TemporaryBuydown buydown)
    {
      DbValueList values = new DbValueList()
      {
        {
          "BuydownType",
          (object) buydown.BuydownType
        },
        {
          "Description",
          (object) buydown.Description
        },
        {
          "Rate1",
          (object) buydown.Rate1
        },
        {
          "Term1",
          (object) buydown.Term1
        },
        {
          "Rate2",
          (object) buydown.Rate2
        },
        {
          "Term2",
          (object) buydown.Term2
        },
        {
          "Rate3",
          (object) buydown.Rate3
        },
        {
          "Term3",
          (object) buydown.Term3
        },
        {
          "Rate4",
          (object) buydown.Rate4
        },
        {
          "Term4",
          (object) buydown.Term4
        },
        {
          "Rate5",
          (object) buydown.Rate5
        },
        {
          "Term5",
          (object) buydown.Term5
        },
        {
          "Rate6",
          (object) buydown.Rate6
        },
        {
          "Term6",
          (object) buydown.Term6
        },
        {
          "LastModifiedBy",
          (object) buydown.lastModifiedBy
        },
        {
          "LastModifiedDateTime",
          (object) Convert.ToDateTime(buydown.lastModifiedDateTime)
        }
      };
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("TemporaryBuydownType");
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateTemporaryBuydownType(TemporaryBuydown buydown)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("TemporaryBuydownType");
      DbValue key = new DbValue("id", (object) buydown.ID);
      DbValueList values = new DbValueList()
      {
        {
          "BuydownType",
          (object) buydown.BuydownType
        },
        {
          "Description",
          (object) buydown.Description
        },
        {
          "Rate1",
          (object) buydown.Rate1
        },
        {
          "Term1",
          (object) buydown.Term1
        },
        {
          "Rate2",
          (object) buydown.Rate2
        },
        {
          "Term2",
          (object) buydown.Term2
        },
        {
          "Rate3",
          (object) buydown.Rate3
        },
        {
          "Term3",
          (object) buydown.Term3
        },
        {
          "Rate4",
          (object) buydown.Rate4
        },
        {
          "Term4",
          (object) buydown.Term4
        },
        {
          "Rate5",
          (object) buydown.Rate5
        },
        {
          "Term5",
          (object) buydown.Term5
        },
        {
          "Rate6",
          (object) buydown.Rate6
        },
        {
          "Term6",
          (object) buydown.Term6
        },
        {
          "LastModifiedBy",
          (object) buydown.lastModifiedBy
        },
        {
          "LastModifiedDateTime",
          (object) Convert.ToDateTime(buydown.lastModifiedDateTime)
        }
      };
      dbQueryBuilder.Update(table, values, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteTemporaryBuydownType(TemporaryBuydown buydown)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("TemporaryBuydownType");
      DbValue key = new DbValue("id", (object) buydown.ID);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static TemporaryBuydown convertDataRowToBuydown(DataRow datarow)
    {
      return new TemporaryBuydown()
      {
        ID = Convert.ToInt32(datarow["id"]),
        BuydownType = datarow["buydowntype"] == DBNull.Value ? "" : (string) datarow["buydowntype"],
        Description = datarow["description"] == DBNull.Value ? "" : (string) datarow["description"],
        Rate1 = datarow["rate1"] == DBNull.Value ? "" : (string) datarow["rate1"],
        Term1 = datarow["term1"] == DBNull.Value ? "" : (string) datarow["term1"],
        Rate2 = datarow["rate2"] == DBNull.Value ? "" : (string) datarow["rate2"],
        Term2 = datarow["term2"] == DBNull.Value ? "" : (string) datarow["term2"],
        Rate3 = datarow["rate3"] == DBNull.Value ? "" : (string) datarow["rate3"],
        Term3 = datarow["term3"] == DBNull.Value ? "" : (string) datarow["term3"],
        Rate4 = datarow["rate4"] == DBNull.Value ? "" : (string) datarow["rate4"],
        Term4 = datarow["term4"] == DBNull.Value ? "" : (string) datarow["term4"],
        Rate5 = datarow["rate5"] == DBNull.Value ? "" : (string) datarow["rate5"],
        Term5 = datarow["term5"] == DBNull.Value ? "" : (string) datarow["term5"],
        Rate6 = datarow["rate6"] == DBNull.Value ? "" : (string) datarow["rate6"],
        Term6 = datarow["term6"] == DBNull.Value ? "" : (string) datarow["term6"],
        lastModifiedBy = datarow["LastModifiedBy"] == DBNull.Value ? "" : (string) datarow["LastModifiedBy"],
        lastModifiedDateTime = datarow["LastModifiedDateTime"] == DBNull.Value ? "" : ((DateTime) datarow["LastModifiedDateTime"]).ToString("MM/dd/yyyy hh:mm tt")
      };
    }
  }
}

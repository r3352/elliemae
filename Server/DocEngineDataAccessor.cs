// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DocEngineDataAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DocEngine;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class DocEngineDataAccessor
  {
    public const string EDS_SETTINGS_SECTION = "EDS�";
    public const string EDS_SETTINGS_KEY_COMPANYPLANSLASTMOD = "CompanyPlansLastModUTC�";
    public const string EDS_SETTINGS_KEY_CUSTOMPLANSLASTMOD = "CustomPlansLastModUTC�";
    public const string DATEFORMAT = "yyyy-MM-dd HH:mm:ss�";

    public static PlanCodeInfo[] GetCompanyPlanCodes(DocumentOrderType orderType, bool activeOnly)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 31, nameof (GetCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("PlanCodes");
      DbTableInfo table2 = DbAccessManager.GetTable("CustomPlanCodes");
      if (orderType == DocumentOrderType.Both)
      {
        dbQueryBuilder.SelectFrom(table1);
        dbQueryBuilder.SelectFrom(table2);
      }
      else
      {
        dbQueryBuilder.SelectFrom(table1, new DbValue("OrderType", (object) (int) orderType));
        dbQueryBuilder.SelectFrom(table2, new DbValue("OrderType", (object) (int) orderType));
      }
      List<PlanCodeInfo> planCodeInfoList = new List<PlanCodeInfo>();
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      PerformanceMeter.Current.AddCheckpoint("SQL Query", 52, nameof (GetCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        planCodeInfoList.Add(DocEngineDataAccessor.dataRowToPlanCodeInfo(row, false));
      PerformanceMeter.Current.AddCheckpoint("Added Short List", 59, nameof (GetCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
      {
        if ((bool) row["Active"] || !activeOnly)
          planCodeInfoList.Add(DocEngineDataAccessor.dataRowToPlanCodeInfo(row, true));
      }
      PerformanceMeter.Current.AddCheckpoint("Added Custom Plans", 69, nameof (GetCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
      PerformanceMeter.Current.AddCheckpoint("END", 71, nameof (GetCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
      return planCodeInfoList.ToArray();
    }

    public static List<CustomPlanCode> GetCompanyCustomPlanCodes(DocumentOrderType orderType)
    {
      List<CustomPlanCode> companyCustomPlanCodes = new List<CustomPlanCode>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("CustomPlanCodes");
      if (orderType == DocumentOrderType.Both)
        dbQueryBuilder.SelectFrom(table);
      else
        dbQueryBuilder.SelectFrom(table, new DbValue("OrderType", (object) (int) orderType));
      List<string> stringList = new List<string>();
      foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        companyCustomPlanCodes.Add(DocEngineDataAccessor.getCustomPlanCodeFromDatarow(row));
      return companyCustomPlanCodes;
    }

    private static PlanCodeInfo dataRowToPlanCodeInfo(DataRow r, bool isCustom)
    {
      return new PlanCodeInfo(string.Concat(r["PlanCode"]), string.Concat(r["PlanCodeID"]), (DocumentOrderType) r["OrderType"], isCustom);
    }

    private static CustomPlanCode getCustomPlanCodeFromDatarow(DataRow row)
    {
      return new CustomPlanCode()
      {
        Description = string.Concat(row["Description"]),
        ImportInvestorToLoan = string.Concat(row["ImportInvestorToLoan"]) == "True",
        Investor = string.Concat(row["Investor"]),
        IsEMAlias = string.Concat(row["IsEMAlias"]) == "True",
        OrderType = (DocumentOrderType) Enum.Parse(typeof (DocumentOrderType), string.Concat(row["OrderType"])),
        PlanCode = string.Concat(row["PlanCode"]),
        PlanCodeID = string.Concat(row["PlanCodeID"]),
        IsActive = string.Concat(row["Active"]) == "True"
      };
    }

    public static void AddCompanyPlanCodes(DocumentOrderType orderType, PlanCodeInfo[] newCodes)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 132, nameof (AddCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("PlanCodes");
      foreach (PlanCodeInfo newCode in newCodes)
      {
        if (!newCode.IsCustom)
          dbQueryBuilder.DeleteFrom(table, new DbValueList()
          {
            {
              "PlanCodeID",
              (object) newCode.PlanID
            },
            {
              "OrderType",
              (object) (int) orderType
            }
          });
      }
      foreach (PlanCodeInfo newCode in newCodes)
      {
        if (!newCode.IsCustom)
          dbQueryBuilder.InsertInto(table, new DbValueList()
          {
            {
              "PlanCode",
              (object) newCode.PlanCode
            },
            {
              "PlanCodeID",
              (object) newCode.PlanID
            },
            {
              "OrderType",
              (object) (int) orderType
            }
          }, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
      Company.SetCompanySetting("EDS", "CompanyPlansLastModUTC", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
      PerformanceMeter.Current.AddCheckpoint("END", 164, nameof (AddCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
    }

    public static void AddCompanyCustomPlanCodes(CustomPlanCode newPlanCode)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 173, nameof (AddCompanyCustomPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("CustomPlanCodes");
      dbQueryBuilder.AppendLine("delete from CustomPlanCodes where PlanCode = " + SQL.EncodeString(newPlanCode.PlanCode) + " and OrderType = " + (object) (int) newPlanCode.OrderType);
      dbQueryBuilder.InsertInto(table, new DbValueList()
      {
        {
          "PlanCode",
          (object) newPlanCode.PlanCode
        },
        {
          "Description",
          (object) newPlanCode.Description
        },
        {
          "OrderType",
          (object) (int) newPlanCode.OrderType
        },
        {
          "IsEMAlias",
          newPlanCode.IsEMAlias ? (object) "True" : (object) "False"
        },
        {
          "PlanCodeID",
          (object) newPlanCode.PlanCodeID
        },
        {
          "Investor",
          (object) newPlanCode.Investor
        },
        {
          "ImportInvestorToLoan",
          newPlanCode.ImportInvestorToLoan ? (object) "True" : (object) "False"
        },
        {
          "Active",
          newPlanCode.IsActive ? (object) "True" : (object) "False"
        }
      }, true, false);
      dbQueryBuilder.ExecuteNonQuery();
      Company.SetCompanySetting("EDS", "CustomPlansLastModUTC", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
      PerformanceMeter.Current.AddCheckpoint("END", 197, nameof (AddCompanyCustomPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
    }

    public static void RemoveCompanyPlanCodes(DocumentOrderType orderType, string[] codesToRemove)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 206, nameof (RemoveCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbAccessManager.GetTable("PlanCodes");
      dbQueryBuilder.AppendLine("delete from PlanCodes where PlanCodeID in (" + SQL.EncodeArray((Array) codesToRemove) + ") and OrderType = " + (object) (int) orderType);
      dbQueryBuilder.ExecuteNonQuery();
      Company.SetCompanySetting("EDS", "CompanyPlansLastModUTC", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
      PerformanceMeter.Current.AddCheckpoint("END", 219, nameof (RemoveCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
    }

    public static void RemoveCompanyCustomPlanCodes(List<CustomPlanCode> planCodes)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 228, nameof (RemoveCompanyCustomPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbAccessManager.GetTable("CustomPlanCodes");
      foreach (CustomPlanCode customPlanCode in planCodes.ToArray())
        dbQueryBuilder.AppendLine("delete from CustomPlanCodes where PlanCode = " + SQL.EncodeString(customPlanCode.PlanCode) + " and OrderType = " + (object) (int) customPlanCode.OrderType);
      dbQueryBuilder.ExecuteNonQuery();
      Company.SetCompanySetting("EDS", "CustomPlansLastModUTC", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
      PerformanceMeter.Current.AddCheckpoint("END", 242, nameof (RemoveCompanyCustomPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\DocEngineDataAccessor.cs");
    }

    public static DocEngineStackingOrderInfo[] GetDocEngineStackingOrders(
      DocumentOrderType orderType,
      bool includeDocumentsCount = true)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      List<DocEngineStackingOrderInfo> stackingOrderInfoList = new List<DocEngineStackingOrderInfo>();
      DbTableInfo table = DbAccessManager.GetTable("DocEngineStackingOrders");
      if (orderType == DocumentOrderType.Both)
        dbQueryBuilder.SelectFrom(table);
      else
        dbQueryBuilder.SelectFrom(table, new DbValue("OrderType", (object) (int) orderType));
      if (includeDocumentsCount)
      {
        dbQueryBuilder.AppendLine("select OrderID, Count(*) as ElementCount from DocEngineStackingOrderElements group by OrderID");
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          stackingOrderInfoList.Add(DocEngineDataAccessor.dataRowToStackingOrderInfo(row, dataSet.Tables[1]));
        return stackingOrderInfoList.ToArray();
      }
      foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.ExecuteSetQuery().Tables[0].Rows)
        stackingOrderInfoList.Add(new DocEngineStackingOrderInfo(SQL.DecodeString(row["OrderID"]), SQL.DecodeString(row["Name"]), SQL.DecodeEnum<DocumentOrderType>(row["OrderType"]), SQL.DecodeBoolean(row["IsDefault"]), 0));
      return stackingOrderInfoList.ToArray();
    }

    private static DocEngineStackingOrderInfo dataRowToStackingOrderInfo(
      DataRow r,
      DataTable countsTable)
    {
      string guid = string.Concat(r["OrderID"]);
      DataRow[] dataRowArray = countsTable.Select("OrderID = " + SQL.Encode((object) guid));
      int elementCount = 0;
      if (dataRowArray.Length != 0)
        elementCount = SQL.DecodeInt(dataRowArray[0]["ElementCount"], 0);
      return new DocEngineStackingOrderInfo(guid, SQL.DecodeString(r["Name"]), SQL.DecodeEnum<DocumentOrderType>(r["OrderType"]), SQL.DecodeBoolean(r["IsDefault"]), elementCount);
    }

    public static DocEngineStackingOrder GetDocEngineStackingOrder(string orderID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from DocEngineStackingOrders where OrderID = " + SQL.Encode((object) orderID));
      dbQueryBuilder.AppendLine("select * from DocEngineStackingOrderElements where OrderID = " + SQL.Encode((object) orderID) + " order by SeqNo");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      return dataSet.Tables[0].Rows.Count == 0 ? (DocEngineStackingOrder) null : DocEngineDataAccessor.dataRowToStackingOrder(dataSet.Tables[0].Rows[0], dataSet.Tables[1].Rows);
    }

    public static DocEngineStackingOrder GetDocEngineStackingOrder(
      DocumentOrderType orderType,
      string name)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("DocEngineStackingOrders");
      dbQueryBuilder.SelectFrom(table, new string[1]
      {
        "OrderID"
      }, new DbValueList()
      {
        {
          "OrderType",
          (object) (int) orderType
        },
        {
          "Name",
          (object) name
        }
      });
      string orderID = (string) SQL.Decode(dbQueryBuilder.ExecuteScalar());
      return orderID == null ? (DocEngineStackingOrder) null : DocEngineDataAccessor.GetDocEngineStackingOrder(orderID);
    }

    public static bool SetDefaultDocEngineStackingOrder(string orderID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@orderType", "int");
      dbQueryBuilder.AppendLine("select @orderType = OrderType from DocEngineStackingOrders where OrderID = " + SQL.Encode((object) orderID));
      dbQueryBuilder.If("@orderType is not null");
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine("update DocEngineStackingOrders set IsDefault = 0 where OrderType = @orderType");
      dbQueryBuilder.AppendLine("update DocEngineStackingOrders set IsDefault = 1 where OrderID = " + SQL.Encode((object) orderID));
      dbQueryBuilder.End();
      dbQueryBuilder.Select("@orderType");
      return SQL.Decode(dbQueryBuilder.ExecuteScalar(DbTransactionType.Serialized)) != null;
    }

    public static void SaveDocEngineStackingOrder(DocEngineStackingOrder stackingOrder)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("DocEngineStackingOrders");
      DbTableInfo table2 = DbAccessManager.GetTable("DocEngineStackingOrderElements");
      dbQueryBuilder.Declare("@isDefault", "bit");
      dbQueryBuilder.AppendLine("select @isDefault = IsDefault from DocEngineStackingOrders where OrderID = " + SQL.Encode((object) stackingOrder.Guid));
      DbValue key = new DbValue("OrderID", (object) stackingOrder.Guid);
      dbQueryBuilder.DeleteFrom(table1, key);
      dbQueryBuilder.InsertInto(table1, new DbValueList()
      {
        {
          "OrderID",
          (object) stackingOrder.Guid
        },
        {
          "Name",
          (object) stackingOrder.Name
        },
        {
          "OrderType",
          (object) (int) stackingOrder.Type
        },
        {
          "IsDefault",
          (object) "IsNull(@isDefault, 0)",
          (IDbEncoder) DbEncoding.None
        }
      }, true, false);
      for (int index = 0; index < stackingOrder.Elements.Count; ++index)
        dbQueryBuilder.InsertInto(table2, new DbValueList()
        {
          {
            "OrderID",
            (object) stackingOrder.Guid
          },
          {
            "SeqNo",
            (object) index
          },
          {
            "Name",
            (object) stackingOrder.Elements[index].Name
          },
          {
            "Type",
            (object) (int) stackingOrder.Elements[index].Type
          }
        }, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteDocEngineStackingOrder(string orderID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("DocEngineStackingOrders");
      DbValue key = new DbValue("OrderID", (object) orderID);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static DocEngineStackingOrder dataRowToStackingOrder(
      DataRow r,
      DataRowCollection elementRows)
    {
      DocEngineStackingOrder stackingOrder = new DocEngineStackingOrder(SQL.DecodeString(r["OrderID"]), SQL.DecodeString(r["Name"]), SQL.DecodeBoolean(r["IsDefault"]), SQL.DecodeEnum<DocumentOrderType>(r["OrderType"]));
      foreach (DataRow elementRow in (InternalDataCollectionBase) elementRows)
        stackingOrder.Elements.Add(DocEngineDataAccessor.dataRowToStackingElement(elementRow));
      return stackingOrder;
    }

    private static StackingElement dataRowToStackingElement(DataRow r)
    {
      return new StackingElement(SQL.DecodeEnum<StackingElementType>(r["Type"]), SQL.DecodeString(r["Name"]));
    }

    public static void AddKnownDocEngineStackingElements(
      DocumentOrderType orderType,
      StackingElement[] elements)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("DocEngineStackingElements");
      foreach (StackingElement element in elements)
      {
        DbValueList dbValueList = new DbValueList();
        dbValueList.Add("Name", (object) element.Name);
        dbValueList.Add("Type", (object) (int) element.Type);
        dbValueList.Add("OrderType", (object) (int) orderType);
        dbQueryBuilder.IfNotExists(table, dbValueList);
        dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static StackingElement[] GetKnownDocEngineStackingElements(DocumentOrderType orderType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("DocEngineStackingElements");
      DbValue key = new DbValue("OrderType", (object) (int) orderType);
      dbQueryBuilder.SelectFrom(table, key);
      List<StackingElement> stackingElementList = new List<StackingElement>();
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        stackingElementList.Add(DocEngineDataAccessor.dataRowToStackingElement(r));
      return stackingElementList.ToArray();
    }

    public static List<string> GetHighCostStates()
    {
      List<string> highCostStates = new List<string>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("HighCostStates");
      dbQueryBuilder.SelectFrom(table);
      foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.ExecuteTableQuery().Rows)
        highCostStates.Add(string.Concat(row[0]));
      return highCostStates;
    }

    public static void UpdateHighCostStates(List<string> stateList)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("HighCostStates");
      dbQueryBuilder.DeleteFrom(table);
      foreach (string str in stateList.ToArray())
        dbQueryBuilder.InsertInto(table, new DbValueList()
        {
          {
            "state",
            (object) str
          }
        }, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}

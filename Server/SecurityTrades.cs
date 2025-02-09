// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SecurityTrades
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class SecurityTrades
  {
    private static string className = nameof (SecurityTrades);

    public static SecurityTradeInfo[] GetAllTrades() => SecurityTrades.getTradesFromDatabase();

    public static SecurityTradeInfo GetTrade(int tradeId)
    {
      try
      {
        SecurityTradeInfo[] tradesFromDatabase = SecurityTrades.getTradesFromDatabase(string.Concat((object) tradeId));
        return tradesFromDatabase.Length == 0 ? (SecurityTradeInfo) null : tradesFromDatabase[0];
      }
      catch (Exception ex)
      {
        Err.Reraise(SecurityTrades.className, ex);
        return (SecurityTradeInfo) null;
      }
    }

    public static Dictionary<int, SecurityTradeInfo> GetTradeDictionary(int[] tradeIds)
    {
      try
      {
        SecurityTradeInfo[] tradesFromDatabase = SecurityTrades.getTradesFromDatabase(EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) tradeIds));
        Dictionary<int, SecurityTradeInfo> tradeDictionary = new Dictionary<int, SecurityTradeInfo>();
        foreach (SecurityTradeInfo securityTradeInfo in tradesFromDatabase)
        {
          if (securityTradeInfo != null)
            tradeDictionary[securityTradeInfo.TradeID] = securityTradeInfo;
        }
        for (int key = 0; key < tradeIds.Length; ++key)
        {
          if (!tradeDictionary.ContainsKey(tradeIds[key]))
            tradeDictionary[key] = (SecurityTradeInfo) null;
        }
        return tradeDictionary;
      }
      catch (Exception ex)
      {
        Err.Reraise(SecurityTrades.className, ex);
        return (Dictionary<int, SecurityTradeInfo>) null;
      }
    }

    public static SecurityTradeInfo[] GetTrades(int[] tradeIds)
    {
      try
      {
        return SecurityTrades.getTradesFromDatabase(EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) tradeIds));
      }
      catch (Exception ex)
      {
        Err.Reraise(SecurityTrades.className, ex);
        return (SecurityTradeInfo[]) null;
      }
    }

    public static SecurityTradeInfo GetTradeByName(string tradeName)
    {
      try
      {
        SecurityTradeInfo[] tradesFromDatabase = SecurityTrades.getTradesFromDatabase("select TradeID from Trades where Name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(tradeName) + "'");
        return tradesFromDatabase.Length == 0 ? (SecurityTradeInfo) null : tradesFromDatabase[0];
      }
      catch (Exception ex)
      {
        Err.Reraise(SecurityTrades.className, ex);
        return (SecurityTradeInfo) null;
      }
    }

    public static SecurityTradeInfo[] GetTradesByName(string tradeName)
    {
      try
      {
        return SecurityTrades.getTradesFromDatabase("select TradeID from Trades where Name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(tradeName) + "'");
      }
      catch (Exception ex)
      {
        Err.Reraise(SecurityTrades.className, ex);
        return (SecurityTradeInfo[]) null;
      }
    }

    public static SecurityTradeInfo[] GetTradesByName(string tradeName, bool includeHidden)
    {
      try
      {
        return !includeHidden ? SecurityTrades.getTradesFromDatabase("select Trades.TradeID from Trades inner join SecurityTrades on Trades.TradeID = SecurityTrades.TradeID where Trades.Name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(tradeName) + "' and SecurityTrades.IsHidden = 0") : SecurityTrades.GetTradesByName(tradeName);
      }
      catch (Exception ex)
      {
        Err.Reraise(SecurityTrades.className, ex);
        return (SecurityTradeInfo[]) null;
      }
    }

    public static SecurityTradeInfo[] GetActiveTrades()
    {
      try
      {
        SecurityTradeInfo[] tradesFromDatabase = SecurityTrades.getTradesFromDatabase("select TradeID from Trades where Status <> " + (object) 5);
        return tradesFromDatabase.Length == 0 ? (SecurityTradeInfo[]) null : tradesFromDatabase;
      }
      catch (Exception ex)
      {
        Err.Reraise(SecurityTrades.className, ex);
        return (SecurityTradeInfo[]) null;
      }
    }

    public static int CreateTrade(SecurityTradeInfo info, UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (SecurityTrades));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      dbQueryBuilder.Declare("@tradeId", "int");
      dbQueryBuilder.InsertInto(table1, TradeUtils.CreateTradesValueList((TradeInfoObj) info), true, false);
      dbQueryBuilder.SelectIdentity("@tradeId");
      dbQueryBuilder.InsertInto(table2, SecurityTrades.createSecurityTradeInfoValueList(info, true), true, false);
      DbValue dbValue = new DbValue("TradeID", (object) "@tradeId", (IDbEncoder) DbEncoding.None);
      DbValueList objectsValueList = TradeUtils.CreateTradeObjectsValueList((TradeInfoObj) info);
      objectsValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.Select("@tradeId");
      int tradeId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
      info = SecurityTrades.GetTrade(tradeId);
      SecurityTrades.AddTradeHistoryItem(new SecurityTradeHistoryItem(info, TradeHistoryAction.TradeCreated, currentUser));
      if (info.Status != TradeStatus.Open)
        SecurityTrades.AddTradeHistoryItem(new SecurityTradeHistoryItem(info, info.Status, currentUser));
      if (info.Locked)
        SecurityTrades.AddTradeHistoryItem(new SecurityTradeHistoryItem(info, TradeHistoryAction.TradeLocked, currentUser));
      return tradeId;
    }

    public static void UpdateTrade(SecurityTradeInfo info, UserInfo currentUser)
    {
      SecurityTradeInfo trade = SecurityTrades.GetTrade(info.TradeID);
      if (trade == null)
        throw new ObjectNotFoundException("Trade not found", ObjectType.Trade, (object) info.TradeID);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Trades");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (SecurityTrades));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeObjects");
      DbValue key = new DbValue("TradeID", (object) info.TradeID);
      dbQueryBuilder.DeleteFrom(table3, key);
      dbQueryBuilder.Update(table1, TradeUtils.CreateTradesValueList((TradeInfoObj) info), key);
      dbQueryBuilder.Update(table2, SecurityTrades.createSecurityTradeInfoValueList(info, false), key);
      DbValueList objectsValueList = TradeUtils.CreateTradeObjectsValueList((TradeInfoObj) info);
      objectsValueList.Add(key);
      dbQueryBuilder.InsertInto(table3, objectsValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
      if (info.Locked != trade.Locked)
        SecurityTrades.AddTradeHistoryItem(new SecurityTradeHistoryItem(info, info.Locked ? TradeHistoryAction.TradeLocked : TradeHistoryAction.TradeUnlocked, currentUser));
      if (info.Status == trade.Status)
        return;
      SecurityTrades.AddTradeHistoryItem(new SecurityTradeHistoryItem(info, info.Status, currentUser));
    }

    public static void DeleteTrade(int tradeId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("TradeAssignmentByTrade");
      DbTableInfo table2 = DbAccessManager.GetTable(nameof (SecurityTrades));
      DbTableInfo table3 = DbAccessManager.GetTable("TradeHistory");
      DbValue key = new DbValue("TradeID", (object) tradeId);
      dbQueryBuilder.AppendLine("delete TradeAssignmentByTrade where AssigneeTradeID = " + (object) tradeId);
      dbQueryBuilder.DeleteFrom(table1, key);
      dbQueryBuilder.DeleteFrom(table2, key);
      dbQueryBuilder.DeleteFrom(table3, key);
      dbQueryBuilder.ExecuteNonQuery();
      TradeUtils.DeleteTrade(tradeId);
    }

    public static void DeleteHiddenOrphanTrades()
    {
      foreach (TradeBase tradeBase in SecurityTrades.getTradesFromDatabase("Select TradeID from SecurityTrades where IsHidden = 1 and TradeID not in (Select TradeID from TradeAssignmentByTrade)"))
        SecurityTrades.DeleteTrade(tradeBase.TradeID);
    }

    public static void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      TradeUtils.setBasicTradeStatus(tradeIds, status, currentUser);
      Dictionary<int, SecurityTradeInfo> tradeDictionary = SecurityTrades.GetTradeDictionary(tradeIds);
      foreach (int tradeId in tradeIds)
      {
        if (tradeDictionary.ContainsKey(tradeId) && tradeDictionary[tradeId] != null)
          SecurityTrades.AddTradeHistoryItem(new SecurityTradeHistoryItem(tradeDictionary[tradeId], action, currentUser));
      }
    }

    public static void DeleteLoanTradeFromTrading(int loanTradeId)
    {
      throw new InvalidOperationException("Loan trade cannot be removed from the security trade once assigned.");
    }

    public static SecurityTradeHistoryItem[] GetTradeHistory(int tradeId)
    {
      return SecurityTrades.getTradeHistoryFromDatabase("select HistoryID from TradeHistory where TradeID = " + (object) tradeId);
    }

    public static SecurityTradeHistoryItem[] GetTradeHistoryForLoanTrade(int tradeId)
    {
      try
      {
        return SecurityTrades.getTradeHistoryFromDatabase("select HistoryID from TradeHistory inner join TradeAssignmentByTrade on TradeHistory.TradeID =  TradeAssignmentByTrade.AssigneeTradeID where TradeAssignmentByTrade.AssigneeTradeID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) tradeId));
      }
      catch (Exception ex)
      {
        Err.Reraise(SecurityTrades.className, ex);
        return (SecurityTradeHistoryItem[]) null;
      }
    }

    private static SecurityTradeHistoryItem[] getTradeHistoryFromDatabase(string selectionQuery)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select TradeHistory.*,  Trades.Name as TradeName, Trades.DealerName");
      dbQueryBuilder.AppendLine("from TradeHistory inner join Trades on TradeHistory.TradeID = Trades.TradeID");
      if ((selectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("where TradeHistory.HistoryID in (" + selectionQuery + ")");
      return SecurityTrades.dataRowsToTradeHistoryItems((IEnumerable) dbQueryBuilder.Execute());
    }

    private static SecurityTradeHistoryItem[] dataRowsToTradeHistoryItems(IEnumerable rows)
    {
      List<SecurityTradeHistoryItem> tradeHistoryItemList = new List<SecurityTradeHistoryItem>();
      foreach (DataRow row in rows)
        tradeHistoryItemList.Add(SecurityTrades.dataRowToHistoryItem(row));
      return tradeHistoryItemList.ToArray();
    }

    public static int AddTradeHistoryItem(SecurityTradeHistoryItem item)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeHistory");
      dbQueryBuilder.InsertInto(table, new DbValueList()
      {
        {
          "TradeID",
          (object) item.TradeID
        },
        {
          "UserID",
          (object) item.UserID
        },
        {
          "Action",
          (object) (int) item.Action
        },
        {
          "Status",
          (object) item.Status,
          (IDbEncoder) DbEncoding.MinusOneAsNull
        },
        {
          "Timestamp",
          (object) "GetDate()",
          (IDbEncoder) DbEncoding.None
        },
        {
          "DataXml",
          (object) item.Data.ToXml()
        }
      }, true, false);
      dbQueryBuilder.SelectIdentity();
      return EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
    }

    private static SecurityTradeHistoryItem dataRowToHistoryItem(DataRow r)
    {
      return new SecurityTradeHistoryItem(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["HistoryID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), (TradeHistoryAction) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Action"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Timestamp"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["UserID"]), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["DataXml"]));
    }

    private static SecurityTradeInfo[] getTradesFromDatabase()
    {
      return SecurityTrades.getTradesFromDatabase("");
    }

    private static SecurityTradeInfo[] getTradesFromDatabase(string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select TradeID from SecurityTrades";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select SecurityTradeDetails.*, TradeObjects.*, TradeSummary.* ");
      dbQueryBuilder.AppendLine("    from SecurityTradeDetails");
      dbQueryBuilder.AppendLine("    inner join TradeObjects on SecurityTradeDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine("    left outer join");
      dbQueryBuilder.AppendLine("    FN_GetSecurityTradeSummaryInline(" + (object) 1 + ") TradeSummary on SecurityTradeDetails.TradeID = TradeSummary.TradeID");
      dbQueryBuilder.AppendLine("    where SecurityTradeDetails.TradeID in (" + criteria + ")");
      DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
      SecurityTradeInfo[] tradesFromDatabase = new SecurityTradeInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
        tradesFromDatabase[index] = SecurityTrades.dataRowToTradeInfo(table.Rows[index]);
      return tradesFromDatabase;
    }

    private static SecurityTradeInfo dataRowToTradeInfo(DataRow r)
    {
      SecurityTradeInfo tradeInfo = new SecurityTradeInfo();
      tradeInfo.TradeID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"], -1);
      tradeInfo.Status = (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], 0);
      tradeInfo.Guid = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]);
      tradeInfo.Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"]);
      tradeInfo.CommitmentType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CommitmentType"]);
      tradeInfo.TradeDescription = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]);
      tradeInfo.SecurityType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SecurityType"]);
      tradeInfo.ProgramType = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ProgramType"]);
      tradeInfo.Term1 = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Term1"]);
      tradeInfo.Term2 = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Term2"]);
      tradeInfo.Coupon = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Coupon"]);
      tradeInfo.Price = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Price"]);
      tradeInfo.DealerName = string.Concat(r["DealerName"]);
      tradeInfo.TradeAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]);
      tradeInfo.Tolerance = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]);
      tradeInfo.MinAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinAmount"]);
      tradeInfo.MaxAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxAmount"]);
      tradeInfo.CommitmentDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]);
      tradeInfo.ConfirmDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ConfirmDate"]);
      tradeInfo.SettlementDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["SettlementDate"]);
      tradeInfo.NotificationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["NotificationDate"]);
      tradeInfo.Locked = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"], false);
      tradeInfo.IsHidden = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsHidden"], false);
      tradeInfo.PairOffAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffAmount"]);
      tradeInfo.ParseTradeObjects(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Notes"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FilterQueryXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PairOffsXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["PricingXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AdjustmentsXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SRPTableXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DealerXml"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AssigneeXml"]), "", "", "", "");
      tradeInfo.OpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OpenAmount"]);
      tradeInfo.TotalPairOffGainLoss = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PairOffGainLoss"]);
      tradeInfo.OptionPremium = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OptionPremium"]);
      return tradeInfo;
    }

    private static DbValueList createSecurityTradeInfoValueList(
      SecurityTradeInfo info,
      bool useParam)
    {
      DbValueList tradeInfoValueList = new DbValueList();
      if (useParam)
      {
        DbValue dbValue = new DbValue("TradeID", (object) "@tradeId", (IDbEncoder) DbEncoding.None);
        tradeInfoValueList.Add(dbValue);
      }
      tradeInfoValueList.Add("SecurityType", (object) info.SecurityType);
      tradeInfoValueList.Add("ProgramType", (object) info.ProgramType);
      tradeInfoValueList.Add("Term1", (object) info.Term1);
      tradeInfoValueList.Add("Term2", (object) info.Term2);
      tradeInfoValueList.Add("Coupon", (object) info.Coupon);
      tradeInfoValueList.Add("Price", (object) info.Price);
      tradeInfoValueList.Add("ConfirmDate", (object) info.ConfirmDate);
      tradeInfoValueList.Add("SettlementDate", (object) info.SettlementDate);
      tradeInfoValueList.Add("NotificationDate", (object) info.NotificationDate);
      tradeInfoValueList.Add("IsHidden", (object) info.IsHidden, (IDbEncoder) DbEncoding.Flag);
      tradeInfoValueList.Add("OptionPremium", (object) info.OptionPremium);
      return tradeInfoValueList;
    }

    private static SecurityTradeViewModel[] getTradeViewsFromDatabase(string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select TradeID from SecurityTradeDetails";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select SecurityTradeDetails.*, TradeObjects.*, SecurityTradeSummary.* from SecurityTradeDetails");
      dbQueryBuilder.AppendLine("   inner join TradeObjects on SecurityTradeDetails.TradeID = TradeObjects.TradeID");
      dbQueryBuilder.AppendLine("   left outer join");
      dbQueryBuilder.AppendLine("   FN_GetSecurityTradeSummaryInline(" + (object) 1 + ") SecurityTradeSummary on SecurityTradeDetails.TradeID = SecurityTradeSummary.TradeID");
      dbQueryBuilder.AppendLine("   where SecurityTradeDetails.TradeID in (" + criteria + ")");
      DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
      SecurityTradeViewModel[] viewsFromDatabase = new SecurityTradeViewModel[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        SecurityTradeViewModel securityTradeViewInfo = SecurityTrades.dataRowToSecurityTradeViewInfo(table.Rows[index]);
        securityTradeViewInfo.OpenAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(table.Rows[index]["OpenAmount"]);
        securityTradeViewInfo.TotalPairOffGainLoss = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(table.Rows[index]["PairOffGainLoss"]);
        securityTradeViewInfo.CompletionPercent = SecurityTradeCalculation.CalculateCompletionPercentage(securityTradeViewInfo.TradeAmount, securityTradeViewInfo.OpenAmount);
        viewsFromDatabase[index] = securityTradeViewInfo;
      }
      return viewsFromDatabase;
    }

    private static SecurityTradeViewModel dataRowToSecurityTradeViewInfo(DataRow r)
    {
      return new SecurityTradeViewModel(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Guid"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Name"]), (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CommitmentType"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["TradeDescription"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SecurityType"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ProgramType"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Term1"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Term2"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Coupon"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Price"]), string.Concat(r["DealerName"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TradeAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MinAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["MaxAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CommitmentDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ConfirmDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["SettlementDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["NotificationDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Locked"], false), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["LastModified"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsHidden"], false), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalProfit"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["OptionPremium"]));
    }

    public static int[] QueryTradeIds(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      StringBuilder stringBuilder = new StringBuilder(SecurityTrades.getQueryTradeIdsSql(user, criteria, sortOrder, isExternalOrganization));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(stringBuilder.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int[] numArray = new int[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        numArray[index] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[index]["TradeID"]);
      return numArray;
    }

    public static string getQueryTradeIdsSql(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      try
      {
        string fieldList = "SecurityTradeDetails.TradeID";
        SecurityTradeQuery securityTradeQuery = new SecurityTradeQuery(user);
        return SecurityTrades.generateQuerySql(fieldList, user, criteria, sortOrder, (QueryEngine) securityTradeQuery, isExternalOrganization).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(SecurityTrades.className, ex);
        return (string) null;
      }
    }

    private static DbQueryBuilder generateQuerySql(
      string fieldList,
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortFields,
      QueryEngine queryEngine,
      bool isExternalOrganization)
    {
      DbQueryBuilder querySql = new DbQueryBuilder();
      querySql.AppendLine("select " + fieldList + " ");
      IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(fieldList.Replace(" ", "").Split(','));
      QueryCriterion filter = QueryCriterion.Join(criteria, BinaryOperator.And);
      querySql.Append(queryEngine.GetTableSelectionClause(fields, filter, sortFields, true, true, isExternalOrganization));
      querySql.Append(queryEngine.GetOrderByClause(sortFields));
      return querySql;
    }

    public static Dictionary<int, SecurityTradeViewModel> GetTradeViews(int[] tradeIds)
    {
      List<SecurityTradeViewModel> securityTradeViewModelList = new List<SecurityTradeViewModel>();
      foreach (int tradeId in tradeIds)
        securityTradeViewModelList.AddRange((IEnumerable<SecurityTradeViewModel>) SecurityTrades.getTradeViewsFromDatabase(string.Concat((object) tradeId)));
      Dictionary<int, SecurityTradeViewModel> tradeViews = new Dictionary<int, SecurityTradeViewModel>();
      foreach (SecurityTradeViewModel securityTradeViewModel in securityTradeViewModelList.ToArray())
      {
        if (securityTradeViewModel != null)
          tradeViews[securityTradeViewModel.TradeID] = securityTradeViewModel;
      }
      for (int key = 0; key < tradeIds.Length; ++key)
      {
        if (!tradeViews.ContainsKey(tradeIds[key]))
          tradeViews[key] = (SecurityTradeViewModel) null;
      }
      return tradeViews;
    }
  }
}

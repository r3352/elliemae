// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.MasterContracts
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class MasterContracts
  {
    private static string className = nameof (MasterContracts);

    public static int AddTradeHistoryItem(LoanTradeHistoryItem item)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("TradeHistory");
      dbQueryBuilder.InsertInto(table1, new DbValueList()
      {
        {
          "TradeID",
          (object) item.TradeID
        },
        {
          "ContractID",
          (object) item.ContractID,
          (IDbEncoder) DbEncoding.MinusOneAsNull
        },
        {
          "LoanGuid",
          (object) item.LoanGuid,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
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
      if (item.Action == TradeHistoryAction.LoanRejected)
      {
        DbTableInfo table2 = DbAccessManager.GetTable("TradeRejections");
        DbValueList dbValueList = new DbValueList();
        dbValueList.Add("LoanGuid", (object) item.LoanGuid);
        dbValueList.Add("Investor", (object) item.InvestorName);
        dbQueryBuilder.IfNotExists(table2, dbValueList);
        dbValueList.Add("Timestamp", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
        dbQueryBuilder.InsertInto(table2, dbValueList, true, false);
      }
      return EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
    }

    public static MasterContractSummaryInfo[] GetAllContracts(bool includeTradeData)
    {
      return MasterContracts.getContractsFromDatabase("select ContractID from MasterContracts", includeTradeData, true);
    }

    public static MasterContractSummaryInfo[] GetActiveContracts(bool includeTradeData)
    {
      return MasterContracts.getContractsFromDatabase("select ContractID from MasterContracts where Status <> " + (object) 1, includeTradeData, true);
    }

    public static MasterContractSummaryInfo[] GetArchivedContracts(bool includeTradeData)
    {
      return MasterContracts.getContractsFromDatabase("select ContractID from MasterContracts where Status = " + (object) 1, includeTradeData, true);
    }

    public static MasterContractInfo GetContract(int contractId)
    {
      MasterContractSummaryInfo[] contractsFromDatabase = MasterContracts.getContractsFromDatabase(string.Concat((object) contractId), true, false);
      return contractsFromDatabase.Length == 0 ? (MasterContractInfo) null : (MasterContractInfo) contractsFromDatabase[0];
    }

    public static MasterContractInfo[] GetContracts(int[] contractIds)
    {
      string criteria = "";
      foreach (int contractId in contractIds)
        criteria = !(criteria == "") ? criteria + ", " + (object) contractId : string.Concat((object) contractId);
      MasterContractSummaryInfo[] contractsFromDatabase = MasterContracts.getContractsFromDatabase(criteria, true, false);
      List<MasterContractInfo> masterContractInfoList = new List<MasterContractInfo>();
      foreach (MasterContractSummaryInfo contractSummaryInfo in contractsFromDatabase)
      {
        if (contractSummaryInfo is MasterContractInfo)
          masterContractInfoList.Add((MasterContractInfo) contractSummaryInfo);
      }
      return masterContractInfoList.ToArray();
    }

    public static MasterContractSummaryInfo[] GetContractSummaries(int[] contractIds)
    {
      List<MasterContractSummaryInfo> contractSummaryInfoList = new List<MasterContractSummaryInfo>();
      foreach (int contractId in contractIds)
        contractSummaryInfoList.AddRange((IEnumerable<MasterContractSummaryInfo>) MasterContracts.getContractsFromDatabase(string.Concat((object) contractId), true, true));
      return contractSummaryInfoList.ToArray();
    }

    public static MasterContractInfo GetContractForTrade(int tradeId)
    {
      MasterContractSummaryInfo[] contractsFromDatabase = MasterContracts.getContractsFromDatabase("select ContractID from Trades where TradeID = " + (object) tradeId, false, false);
      return contractsFromDatabase.Length == 0 ? (MasterContractInfo) null : (MasterContractInfo) contractsFromDatabase[0];
    }

    public static MasterContractInfo GetContractByContractNumber(string contractNumber)
    {
      MasterContractSummaryInfo[] contractsFromDatabase = MasterContracts.getContractsFromDatabase("Select ContractID from MasterContracts where ContractNumber like '" + EllieMae.EMLite.DataAccess.SQL.Escape(contractNumber) + "'", true, false);
      return contractsFromDatabase.Length == 0 ? (MasterContractInfo) null : (MasterContractInfo) contractsFromDatabase[0];
    }

    public static int CreateContract(MasterContractInfo contract)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable(nameof (MasterContracts));
      DbTableInfo table2 = DbAccessManager.GetTable("MasterContractObjects");
      dbQueryBuilder.Declare("@contractId", "int");
      dbQueryBuilder.InsertInto(table1, MasterContracts.createContractValueList(contract), true, false);
      dbQueryBuilder.SelectIdentity("@contractId");
      DbValue dbValue = new DbValue("ContractId", (object) "@contractId", (IDbEncoder) DbEncoding.None);
      DbValueList objectsValueList = MasterContracts.createContractObjectsValueList(contract);
      objectsValueList.Add(dbValue);
      dbQueryBuilder.InsertInto(table2, objectsValueList, true, false);
      dbQueryBuilder.AppendLine("INSERT INTO Trades (Guid, Name, Status, Locked, TradeType, TradeAmount, OpenAmount, LastModified, Tolerance, PairOffFee, PairOffAmount, CommitmentType, TradeDescription ) ");
      dbQueryBuilder.AppendLine("SELECT NEWID(), ContractNumber, 1, 0, 6, ContractAmount, 0, getdate(), 0, 0.00, 0.00, '', '' FROM MasterContracts WHERE ContractID = @contractId");
      dbQueryBuilder.AppendLine("UPDATE MasterContracts SET TradeId = @@IDENTITY WHERE ContractID = @contractId");
      dbQueryBuilder.Select("@contractId");
      return EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
    }

    public static void UpdateContract(MasterContractInfo contract)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable(nameof (MasterContracts));
      DbTableInfo table2 = DbAccessManager.GetTable("MasterContractObjects");
      DbValue key = new DbValue("ContractID", (object) contract.ContractID);
      dbQueryBuilder.DeleteFrom(table2, key);
      dbQueryBuilder.Update(table1, MasterContracts.createContractValueList(contract), key);
      DbValueList objectsValueList = MasterContracts.createContractObjectsValueList(contract);
      objectsValueList.Add(key);
      dbQueryBuilder.InsertInto(table2, objectsValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteContract(int contractId, UserInfo userInfo)
    {
      MasterContractSummaryInfo contract = (MasterContractSummaryInfo) MasterContracts.GetContract(contractId);
      LoanTradeInfo[] tradesByContractId = LoanTrades.GetLoanTradesByContractID(contractId);
      MbsPools.GetMbsPoolsByContractID(contractId);
      for (int index = 0; index < tradesByContractId.Length; ++index)
        MasterContracts.AddTradeHistoryItem(new LoanTradeHistoryItem(tradesByContractId[index], contract, TradeHistoryAction.ContractUnassigned, userInfo));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update LoanTrades set ContractID = NULL where ContractID = " + (object) contractId + ";");
      dbQueryBuilder.AppendLine("update Trades set ContractID = NULL where ContractID = " + (object) contractId);
      DbTableInfo table = DbAccessManager.GetTable(nameof (MasterContracts));
      DbValue key = new DbValue("ContractID", (object) contractId);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetContractStatus(int[] contractIds, MasterContractStatus status)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update MasterContracts set status = " + (object) (int) status + " where ContractID in (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) contractIds) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static DbValueList createContractValueList(MasterContractInfo contract)
    {
      return new DbValueList()
      {
        {
          "ContractNumber",
          (object) contract.ContractNumber
        },
        {
          "InvestorName",
          (object) contract.InvestorName
        },
        {
          "InvestorContractNum",
          (object) contract.InvestorContractNumber
        },
        {
          "ContractAmount",
          (object) contract.ContractAmount
        },
        {
          "Tolerance",
          (object) contract.Tolerance
        },
        {
          "StartDate",
          (object) contract.StartDate
        },
        {
          "EndDate",
          (object) contract.EndDate
        },
        {
          "Term",
          (object) (int) contract.Term
        },
        {
          "Status",
          (object) contract.Status
        }
      };
    }

    private static DbValueList createContractObjectsValueList(MasterContractInfo contract)
    {
      return new DbValueList()
      {
        {
          "InvestorXml",
          (object) contract.Investor.ToXml()
        }
      };
    }

    private static MasterContractSummaryInfo[] getContractsFromDatabase(
      string criteria,
      bool includeTradeData,
      bool summariesOnly)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (includeTradeData)
      {
        dbQueryBuilder.Append("select *, (case ContractAmount when 0 Then 0 Else AssignedAmount * 100/ContractAmount End) As CompletionPercent from MasterContracts ");
        if (!summariesOnly)
          dbQueryBuilder.AppendLine("  left outer join MasterContractObjects on MasterContractObjects.ContractID = MasterContracts.ContractID");
        dbQueryBuilder.AppendLine("   left outer join FN_GetContractLoanSummaryInline() ContractTrades on MasterContracts.ContractID = ContractTrades.ContractID");
        dbQueryBuilder.AppendLine("   where MasterContracts.ContractID in (" + criteria + ")");
        dbQueryBuilder.AppendLine("   order by MasterContracts.ContractNumber");
      }
      else if (summariesOnly)
        dbQueryBuilder.AppendLine("select MasterContracts.*, -1 As TradeCount, 0 As TotalProfit, 0 As AssignedAmount from MasterContracts where MasterContracts.ContractID in (" + criteria + ") order by MasterContracts.ContractNumber");
      else
        dbQueryBuilder.AppendLine("select MasterContracts.*, MasterContractObjects.*, -1 As TradeCount, 0 As TotalProfit, 0 As AssignedAmount from MasterContracts left outer join MasterContractObjects on MasterContractObjects.ContractID = MasterContracts.ContractID where MasterContracts.ContractID in (" + criteria + ") order by MasterContracts.ContractNumber");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      MasterContractSummaryInfo[] contractsFromDatabase = new MasterContractSummaryInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        contractsFromDatabase[index] = !summariesOnly ? (MasterContractSummaryInfo) MasterContracts.dataRowToMasterContractInfo(dataRowCollection[index]) : MasterContracts.dataRowToMasterContractSummaryInfo(dataRowCollection[index]);
      return contractsFromDatabase;
    }

    private static MasterContractSummaryInfo dataRowToMasterContractSummaryInfo(DataRow r)
    {
      Decimal completionPercent = 0M;
      try
      {
        completionPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]);
      }
      catch
      {
      }
      return new MasterContractSummaryInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorName"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorContractNum"]), (MasterContractTerm) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Term"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["ContractAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["StartDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["EndDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeCount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AssignedAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalProfit"]), completionPercent);
    }

    private static MasterContractInfo dataRowToMasterContractInfo(DataRow r)
    {
      Decimal completionPercent = 0M;
      try
      {
        completionPercent = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CompletionPercent"]);
      }
      catch
      {
      }
      return new MasterContractInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorName"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["InvestorContractNum"]), (MasterContractTerm) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Term"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["ContractAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["StartDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["EndDate"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeCount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AssignedAmount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["TotalProfit"]), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["InvestorXml"]), completionPercent);
    }

    private static LoanTradeHistoryItem dataRowToHistoryItem(DataRow r)
    {
      return new LoanTradeHistoryItem(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["HistoryID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["TradeID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContractID"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LoanGuid"]), (TradeHistoryAction) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Action"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Timestamp"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["UserID"]), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["DataXml"]));
    }

    public static int[] QueryMasterContractIds(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      StringBuilder stringBuilder = new StringBuilder(MasterContracts.getQueryMasterContractIdsSql(user, criteria, sortOrder, isExternalOrganization));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = stringBuilder.ToString().Replace("[ContractTrades].[CompletionPercent]", "ISNULL([ContractTrades].[CompletionPercent], 0)").Replace("[ContractTrades].[TradeCount]", "ISNULL([ContractTrades].[TradeCount], 0)").Replace("[ContractTrades].[AssignedAmount]", "ISNULL([ContractTrades].[AssignedAmount], 0)").Replace("[ContractTrades].[TotalProfit]", "ISNULL([ContractTrades].[TotalProfit], 0)");
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int[] numArray = new int[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        numArray[index] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[index]["ContractID"]);
      return numArray;
    }

    public static string getQueryMasterContractIdsSql(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      try
      {
        string fieldList = "MasterContracts.ContractID";
        MasterContractQuery masterContractQuery = new MasterContractQuery(user);
        return MasterContracts.generateQuerySql(fieldList, user, criteria, sortOrder, (QueryEngine) masterContractQuery, isExternalOrganization).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(MasterContracts.className, ex);
        return (string) null;
      }
    }

    public static string getQueryTradeIdsSql(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      try
      {
        string fieldList = "Trades.TradeID";
        TradeQuery tradeQuery = new TradeQuery(user);
        return MasterContracts.generateQuerySql(fieldList, user, criteria, sortOrder, (QueryEngine) tradeQuery, isExternalOrganization).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(MasterContracts.className, ex);
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
  }
}

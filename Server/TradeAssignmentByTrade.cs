// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TradeAssignmentByTrade
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class TradeAssignmentByTrade
  {
    private static string className = nameof (TradeAssignmentByTrade);

    public static void AssignTradeToTrade(
      TradeType tradeType,
      int tradeId,
      TradeType assigneeTradeType,
      int assigneeTradeId,
      UserInfo currentUser)
    {
      TradeAssignmentByTrade.AssignTradeToTrade(tradeType, tradeId, assigneeTradeType, assigneeTradeId, (object) MbsPoolSecurityTradeStatus.Assigned, DateTime.Now, currentUser);
    }

    public static void AssignTradeToTrade(
      TradeType tradeType,
      int tradeId,
      TradeType assigneeTradeType,
      int assigneeTradeId,
      Decimal assignedAmount,
      UserInfo currentUser)
    {
      TradeAssignmentByTrade.AssignTradeToTrade(tradeType, tradeId, assigneeTradeType, assigneeTradeId, (object) MbsPoolSecurityTradeStatus.Assigned, DateTime.Now, assignedAmount, currentUser);
    }

    public static void AssignTradeToTrade(
      TradeType tradeType,
      int tradeId,
      TradeType assigneeTradeType,
      int assigneeTradeId,
      object status,
      DateTime assignedDate,
      UserInfo currentUser)
    {
      TradeAssignmentByTrade.AssignTradeToTrade(tradeType, tradeId, assigneeTradeType, assigneeTradeId, status, assignedDate, 0M, currentUser);
    }

    public static void AssignTradeToTrade(
      TradeType tradeType,
      int tradeId,
      TradeType assigneeTradeType,
      int assigneeTradeId,
      Decimal assignedAmount,
      UserInfo currentUser,
      TradeHistoryAction tradeHistoryAction)
    {
      TradeAssignmentByTrade.AssignTradeToTrade(tradeType, tradeId, assigneeTradeType, assigneeTradeId, (object) MbsPoolSecurityTradeStatus.Assigned, DateTime.Now, assignedAmount, currentUser, tradeHistoryAction);
    }

    public static void AssignTradeToTrade(
      TradeType tradeType,
      int tradeId,
      TradeType assigneeTradeType,
      int assigneeTradeId,
      object status,
      DateTime assignedDate,
      Decimal assignedAmount,
      UserInfo currentUser,
      TradeHistoryAction tradeHistoryAction = TradeHistoryAction.AssigneeAssigned)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(nameof (TradeAssignmentByTrade));
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("TradeID", (object) tradeId);
      dbValueList.Add("AssigneeTradeID", (object) assigneeTradeId);
      DbValueList values = new DbValueList();
      values.Add("AssignedStatusDate", (object) SQL.EncodeDateTime(assignedDate), (IDbEncoder) DbEncoding.None);
      values.Add("AssignedAmount", (object) assignedAmount);
      dbQueryBuilder1.IfExists(table, dbValueList);
      dbQueryBuilder1.Begin();
      dbQueryBuilder1.Update(table, values, dbValueList);
      dbQueryBuilder1.End();
      values.Add("TradeID", (object) tradeId);
      values.Add("AssigneeTradeID", (object) assigneeTradeId);
      values.Add("AssignedStatus", (object) (int) status);
      dbQueryBuilder1.Else();
      dbQueryBuilder1.Begin();
      if (assigneeTradeType == TradeType.LoanTrade && tradeType == TradeType.SecurityTrade)
      {
        dbQueryBuilder1.IfExists(table, new DbValueList()
        {
          {
            "AssigneeTradeID",
            (object) assigneeTradeId
          }
        });
        dbQueryBuilder1.Begin();
        dbQueryBuilder1.AppendLine("return");
        dbQueryBuilder1.End();
      }
      dbQueryBuilder1.InsertInto(table, values, true, false);
      dbQueryBuilder1.End();
      dbQueryBuilder1.AppendLine("select 1 from TradeAssignmentByTrade where TradeID = " + (object) tradeId + " and AssigneeTradeID = " + (object) assigneeTradeId);
      DataSet dataSet = dbQueryBuilder1.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count == 0)
      {
        DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
        dbQueryBuilder2.AppendLine("select t.Name from TradeAssignmentByTrade tat inner join Trades t on t.TradeID = tat.TradeID where tat.AssigneeTradeID = " + (object) assigneeTradeId);
        throw new Exception("The loan trade has been allocated to security trade \"" + SQL.DecodeString(dbQueryBuilder2.ExecuteScalar()) + "\".");
      }
      TradeAssignmentByTrade.addTradeHistoryItem(tradeType, tradeId, tradeHistoryAction, currentUser);
      TradeAssignmentByTrade.addTradeHistoryItem(assigneeTradeType, assigneeTradeId, tradeHistoryAction, currentUser);
    }

    public static void UnassignTradeToTrade(
      TradeType tradeType,
      int tradeId,
      TradeType assigneeTradeType,
      int assigneeTradeId,
      UserInfo currentUser,
      TradeHistoryAction tradeHistoryAction = TradeHistoryAction.AssigneeUnassigned)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(nameof (TradeAssignmentByTrade));
      dbQueryBuilder.DeleteFrom(table, new DbValueList()
      {
        {
          "TradeID",
          (object) tradeId
        },
        {
          "AssigneeTradeID",
          (object) assigneeTradeId
        }
      });
      dbQueryBuilder.ExecuteNonQuery();
      TradeAssignmentByTrade.addTradeHistoryItem(tradeType, tradeId, tradeHistoryAction, currentUser);
      TradeAssignmentByTrade.addTradeHistoryItem(assigneeTradeType, assigneeTradeId, tradeHistoryAction, currentUser);
    }

    public static void UpdateAssignedAmountToTrade(
      TradeType tradeType,
      int tradeId,
      TradeType assigneeTradeType,
      int assigneeTradeId,
      Decimal assignedAmount,
      UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(nameof (TradeAssignmentByTrade));
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("TradeID", (object) tradeId);
      dbValueList.Add("AssigneeTradeID", (object) assigneeTradeId);
      DbValueList values = new DbValueList();
      values.Add("AssignedAmount", (object) assignedAmount);
      dbQueryBuilder.IfExists(table, dbValueList);
      dbQueryBuilder.Begin();
      dbQueryBuilder.Update(table, values, dbValueList);
      dbQueryBuilder.End();
      dbQueryBuilder.ExecuteNonQuery();
      TradeAssignmentByTrade.addTradeHistoryItem(tradeType, tradeId, TradeHistoryAction.AssignedAmountChanged, currentUser);
    }

    public static SecurityTradeAssignment[] GetSecurityTradeAssignments(int tradeId)
    {
      try
      {
        if (tradeId < 0)
          return (SecurityTradeAssignment[]) null;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from TradeAssignmentByTrade where TradeID = " + SQL.Encode((object) tradeId));
        SecurityTradeInfo trade = SecurityTrades.GetTrade(tradeId);
        if (trade == null)
          throw new ObjectNotFoundException("Trade not found", ObjectType.Trade, (object) tradeId);
        DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
        SecurityTradeAssignment[] tradeAssignments = new SecurityTradeAssignment[table.Rows.Count];
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          DataRow row = table.Rows[index];
          tradeAssignments[index] = new SecurityTradeAssignment(trade, LoanTrades.GetTrade(SQL.DecodeInt(row["AssigneeTradeID"])), (SecurityLoanTradeStatus) SQL.DecodeInt(row["AssignedStatus"], 0), SQL.DecodeDateTime(row["AssignedStatusDate"]));
        }
        return tradeAssignments;
      }
      catch (Exception ex)
      {
        Err.Reraise(TradeAssignmentByTrade.className, ex);
        return (SecurityTradeAssignment[]) null;
      }
    }

    public static MbsPoolAssignment[] GetMbsPoolAssignments(int tradeId)
    {
      try
      {
        if (tradeId < 0)
          return (MbsPoolAssignment[]) null;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from TradeAssignmentByTrade inner join Trades on AssigneeTradeID = Trades.TradeID where TradeAssignmentByTrade.TradeID = " + SQL.Encode((object) tradeId) + " and Trades.TradeType = 1");
        MbsPoolInfo trade = MbsPools.GetTrade(tradeId);
        if (trade == null)
          throw new ObjectNotFoundException("Trade not found", ObjectType.Trade, (object) tradeId);
        DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
        MbsPoolAssignment[] mbsPoolAssignments = new MbsPoolAssignment[table.Rows.Count];
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          DataRow row = table.Rows[index];
          mbsPoolAssignments[index] = new MbsPoolAssignment(trade, (TradeInfoObj) SecurityTrades.GetTrade(SQL.DecodeInt(row["AssigneeTradeID"])), (MbsPoolSecurityTradeStatus) SQL.DecodeInt(row["AssignedStatus"], 0), SQL.DecodeDateTime(row["AssignedStatusDate"]), SQL.DecodeDecimal(row["AssignedAmount"], 0M));
        }
        return mbsPoolAssignments;
      }
      catch (Exception ex)
      {
        Err.Reraise(TradeAssignmentByTrade.className, ex);
        return (MbsPoolAssignment[]) null;
      }
    }

    public static MbsPoolAssignment[] GetUnassignedMbsPoolAssignments(int tradeId)
    {
      try
      {
        if (tradeId < 0)
          return (MbsPoolAssignment[]) null;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select " + SQL.Encode((object) tradeId) + " as 'TradeID', tradeID as 'AssigneeTradeID'," + SQL.Encode((object) 1) + " as 'AssignedStatus', GETDATE() as 'AssignedStatusDate', 0 as 'AssignedAmount'");
        dbQueryBuilder.AppendLine("from SecurityTrades ");
        dbQueryBuilder.AppendLine("where TradeID not in (select TradeID from Trades where Trades.Status = 5) ");
        dbQueryBuilder.AppendLine("and TradeID not in (select AssigneeTradeID from TradeAssignmentByTrade where TradeID = " + tradeId.ToString() + ")");
        MbsPoolInfo trade = MbsPools.GetTrade(tradeId);
        if (trade == null)
          throw new ObjectNotFoundException("Trade not found", ObjectType.Trade, (object) tradeId);
        DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
        MbsPoolAssignment[] mbsPoolAssignments = new MbsPoolAssignment[table.Rows.Count];
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          DataRow row = table.Rows[index];
          mbsPoolAssignments[index] = new MbsPoolAssignment(trade, (TradeInfoObj) SecurityTrades.GetTrade(SQL.DecodeInt(row["AssigneeTradeID"])), (MbsPoolSecurityTradeStatus) SQL.DecodeInt(row["AssignedStatus"], 0), SQL.DecodeDateTime(row["AssignedStatusDate"]), SQL.DecodeDecimal(row["AssignedAmount"], 0M));
        }
        return mbsPoolAssignments;
      }
      catch (Exception ex)
      {
        Err.Reraise(TradeAssignmentByTrade.className, ex);
        return (MbsPoolAssignment[]) null;
      }
    }

    public static MbsPoolAssignment[] GetMbsPoolAssignmentsBySecurityTrade(int tradeId)
    {
      try
      {
        if (tradeId < 0)
          return (MbsPoolAssignment[]) null;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select TradeAssignmentByTrade.* from TradeAssignmentByTrade inner join SecurityTrades on TradeAssignmentByTrade.AssigneeTradeID = SecurityTrades.TradeID inner join MbsPools on TradeAssignmentByTrade.TradeID = MbsPools.TradeID where SecurityTrades.TradeID = " + SQL.Encode((object) tradeId));
        DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
        MbsPoolAssignment[] assignmentsBySecurityTrade = new MbsPoolAssignment[table.Rows.Count];
        SecurityTradeInfo trade = SecurityTrades.GetTrade(tradeId);
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          DataRow row = table.Rows[index];
          assignmentsBySecurityTrade[index] = new MbsPoolAssignment(MbsPools.GetTrade(SQL.DecodeInt(row["TradeID"])), (TradeInfoObj) trade, (MbsPoolSecurityTradeStatus) SQL.DecodeInt(row["AssignedStatus"], 0), SQL.DecodeDateTime(row["AssignedStatusDate"]), SQL.DecodeDecimal(row["AssignedAmount"], 0M));
        }
        return assignmentsBySecurityTrade;
      }
      catch (Exception ex)
      {
        Err.Reraise(TradeAssignmentByTrade.className, ex);
        return (MbsPoolAssignment[]) null;
      }
    }

    public static MbsPoolAssignment[] GetUnassignedMbsPoolAssignmentsBySecurityTrade(int tradeId)
    {
      try
      {
        if (tradeId < 0)
          return (MbsPoolAssignment[]) null;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select TradeID as 'TradeID', " + SQL.Encode((object) tradeId) + " as 'AssigneeTradeID'," + SQL.Encode((object) 1) + " as 'AssignedStatus', GETDATE() as 'AssignedStatusDate', 0 as 'AssignedAmount'");
        dbQueryBuilder.AppendLine("from MbsPools");
        dbQueryBuilder.AppendLine("where TradeID not in (select TradeID from TradeAssignmentByTrade where AssigneeTradeID = " + tradeId.ToString() + ")");
        DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
        MbsPoolAssignment[] assignmentsBySecurityTrade = new MbsPoolAssignment[table.Rows.Count];
        SecurityTradeInfo trade = SecurityTrades.GetTrade(tradeId);
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          DataRow row = table.Rows[index];
          assignmentsBySecurityTrade[index] = new MbsPoolAssignment(MbsPools.GetTrade(SQL.DecodeInt(row["TradeID"])), (TradeInfoObj) trade, (MbsPoolSecurityTradeStatus) SQL.DecodeInt(row["AssignedStatus"], 0), SQL.DecodeDateTime(row["AssignedStatusDate"]), SQL.DecodeDecimal(row["AssignedAmount"], 0M));
        }
        return assignmentsBySecurityTrade;
      }
      catch (Exception ex)
      {
        Err.Reraise(TradeAssignmentByTrade.className, ex);
        return (MbsPoolAssignment[]) null;
      }
    }

    public static MbsPoolAssignment[] GetMbsPoolAssignmentsByGseCommitment(int tradeId)
    {
      try
      {
        if (tradeId < 0)
          return (MbsPoolAssignment[]) null;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select TradeAssignmentByTrade.* from TradeAssignmentByTrade inner join GseCommitments on TradeAssignmentByTrade.AssigneeTradeID = GseCommitments.TradeID inner join MbsPools on TradeAssignmentByTrade.TradeID = MbsPools.TradeID where GseCommitments.TradeID = " + SQL.Encode((object) tradeId));
        DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
        MbsPoolAssignment[] assignmentsByGseCommitment = new MbsPoolAssignment[table.Rows.Count];
        GSECommitmentInfo trade = GseCommitments.GetTrade(tradeId);
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          DataRow row = table.Rows[index];
          assignmentsByGseCommitment[index] = new MbsPoolAssignment(MbsPools.GetTrade(SQL.DecodeInt(row["TradeID"])), (TradeInfoObj) trade, (MbsPoolSecurityTradeStatus) SQL.DecodeInt(row["AssignedStatus"], 0), SQL.DecodeDateTime(row["AssignedStatusDate"]), SQL.DecodeDecimal(row["AssignedAmount"], 0M));
        }
        return assignmentsByGseCommitment;
      }
      catch (Exception ex)
      {
        Err.Reraise(TradeAssignmentByTrade.className, ex);
        return (MbsPoolAssignment[]) null;
      }
    }

    public static MbsPoolAssignment[] GetUnassignedMbsPoolAssignmentsByGseCommitment(int tradeId)
    {
      try
      {
        if (tradeId < 0)
          return (MbsPoolAssignment[]) null;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select TradeID as 'TradeID', " + SQL.Encode((object) tradeId) + " as 'AssigneeTradeID'," + SQL.Encode((object) 1) + " as 'AssignedStatus', GETDATE() as 'AssignedStatusDate', 0 as 'AssignedAmount'");
        dbQueryBuilder.AppendLine("from MbsPools");
        dbQueryBuilder.AppendLine("where TradeID not in (select TradeID from TradeAssignmentByTrade where AssigneeTradeID = " + tradeId.ToString() + ") ");
        dbQueryBuilder.AppendLine("and PoolMortgageType = " + 4.ToString());
        DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
        MbsPoolAssignment[] assignmentsByGseCommitment = new MbsPoolAssignment[table.Rows.Count];
        GSECommitmentInfo trade = GseCommitments.GetTrade(tradeId);
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          DataRow row = table.Rows[index];
          assignmentsByGseCommitment[index] = new MbsPoolAssignment(MbsPools.GetTrade(SQL.DecodeInt(row["TradeID"])), (TradeInfoObj) trade, (MbsPoolSecurityTradeStatus) SQL.DecodeInt(row["AssignedStatus"], 0), SQL.DecodeDateTime(row["AssignedStatusDate"]), SQL.DecodeDecimal(row["AssignedAmount"], 0M));
        }
        return assignmentsByGseCommitment;
      }
      catch (Exception ex)
      {
        Err.Reraise(TradeAssignmentByTrade.className, ex);
        return (MbsPoolAssignment[]) null;
      }
    }

    public static GSECommitmentAssignment[] GetGSECommintmentByMbsPool(int tradeId)
    {
      try
      {
        if (tradeId < 0)
          return (GSECommitmentAssignment[]) null;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select TradeAssignmentByTrade.* from TradeAssignmentByTrade inner join MbsPools on TradeAssignmentByTrade.TradeID = MbsPools.TradeID inner join GseCommitments on TradeAssignmentByTrade.AssigneeTradeID = GseCommitments.TradeID where MbsPools.TradeID = " + SQL.Encode((object) tradeId));
        DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
        GSECommitmentAssignment[] commintmentByMbsPool = new GSECommitmentAssignment[table.Rows.Count];
        MbsPoolInfo trade = MbsPools.GetTrade(tradeId);
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          DataRow row = table.Rows[index];
          commintmentByMbsPool[index] = new GSECommitmentAssignment(GseCommitments.GetTrade(SQL.DecodeInt(row["AssigneeTradeID"])), (TradeInfoObj) trade, (GseCommitmentLoanStatus) SQL.DecodeInt(row["AssignedStatus"], 0), SQL.DecodeDateTime(row["AssignedStatusDate"]), SQL.DecodeDecimal(row["AssignedAmount"], 0M));
        }
        return commintmentByMbsPool;
      }
      catch (Exception ex)
      {
        Err.Reraise(TradeAssignmentByTrade.className, ex);
        return (GSECommitmentAssignment[]) null;
      }
    }

    public static GSECommitmentAssignment[] GetUnassignedGSECommintmentByMbsPool(int tradeId)
    {
      try
      {
        if (tradeId < 0)
          return (GSECommitmentAssignment[]) null;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select TradeID as 'TradeID', " + SQL.Encode((object) tradeId) + " as 'AssigneeTradeID'," + SQL.Encode((object) 1) + " as 'AssignedStatus', GETDATE() as 'AssignedStatusDate', 0 as 'AssignedAmount'");
        dbQueryBuilder.AppendLine("from GseCommitments");
        dbQueryBuilder.AppendLine("where TradeID not in (select AssigneeTradeID from TradeAssignmentByTrade where TradeID = " + tradeId.ToString() + ") ");
        DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
        GSECommitmentAssignment[] commintmentByMbsPool = new GSECommitmentAssignment[table.Rows.Count];
        MbsPoolInfo trade = MbsPools.GetTrade(tradeId);
        for (int index = 0; index < table.Rows.Count; ++index)
        {
          DataRow row = table.Rows[index];
          commintmentByMbsPool[index] = new GSECommitmentAssignment(GseCommitments.GetTrade(SQL.DecodeInt(row["TradeID"])), (TradeInfoObj) trade, (GseCommitmentLoanStatus) SQL.DecodeInt(row["AssignedStatus"], 0), SQL.DecodeDateTime(row["AssignedStatusDate"]), SQL.DecodeDecimal(row["AssignedAmount"], 0M));
        }
        return commintmentByMbsPool;
      }
      catch (Exception ex)
      {
        Err.Reraise(TradeAssignmentByTrade.className, ex);
        return (GSECommitmentAssignment[]) null;
      }
    }

    private static void addTradeHistoryItem(
      TradeType tradeType,
      int tradeId,
      TradeHistoryAction action,
      UserInfo currentUser)
    {
      switch (tradeType)
      {
        case TradeType.SecurityTrade:
          SecurityTrades.AddTradeHistoryItem(new SecurityTradeHistoryItem(tradeId, action, currentUser));
          break;
        case TradeType.MbsPool:
          MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(tradeId, action, currentUser));
          break;
        case TradeType.GSECommitment:
          GseCommitments.AddTradeHistoryItem(new GseCommitmentHistoryItem(tradeId, action, currentUser));
          break;
      }
    }
  }
}

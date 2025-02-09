// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.LoanFolderBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public static class LoanFolderBpmDbAccessor
  {
    private const string ruleTableName = "BR_LoanFolderRules�";

    public static void SetRule(LoanFolderRuleInfo rule)
    {
      ClientContext.GetCurrent().Cache.Put<LoanFolderRuleInfo[]>("BR_LoanFolderRules", (Action) (() => LoanFolderBpmDbAccessor.saveRuleToDatabase(rule)), new Func<LoanFolderRuleInfo[]>(LoanFolderBpmDbAccessor.getRulesFromDatabase), CacheSetting.Low);
      LoanFolderBpmDbAccessor.raiseCacheControlEvent();
    }

    public static LoanFolderRuleInfo[] GetRules()
    {
      return ClientContext.GetCurrent().Cache.Get<LoanFolderRuleInfo[]>("BR_LoanFolderRules", new Func<LoanFolderRuleInfo[]>(LoanFolderBpmDbAccessor.getRulesFromDatabase), CacheSetting.Low);
    }

    public static LoanFolderRuleInfo GetRule(string loanFolder)
    {
      if (ClientContext.GetCurrent().Settings.CacheSetting == CacheSetting.Disabled)
        return LoanFolderBpmDbAccessor.getRuleFromDatabase(loanFolder);
      foreach (LoanFolderRuleInfo rule in LoanFolderBpmDbAccessor.GetRules())
      {
        if (string.Compare(rule.LoanFolder, loanFolder, true) == 0)
          return rule;
      }
      return (LoanFolderRuleInfo) null;
    }

    public static void DeleteRule(string loanFolder)
    {
      ClientContext.GetCurrent().Cache.Remove("BR_LoanFolderRules", (Action) (() => LoanFolderBpmDbAccessor.deleteRuleFromDatabase(loanFolder)), CacheSetting.Low);
      LoanFolderBpmDbAccessor.raiseCacheControlEvent();
    }

    public static string[] GetLoanFolders(bool canOriginateLoans)
    {
      List<string> stringList = new List<string>();
      foreach (LoanFolderRuleInfo rule in LoanFolderBpmDbAccessor.GetRules())
      {
        if (rule.CanOriginateLoans == canOriginateLoans)
          stringList.Add(rule.LoanFolder);
      }
      return stringList.ToArray();
    }

    private static LoanFolderRuleInfo getRuleFromDatabase(string loanFolder)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from BR_LoanFolderRules where loanFolder like '" + SQL.Escape(loanFolder) + "'");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (LoanFolderRuleInfo) null : new LoanFolderRuleInfo(loanFolder, dataRowCollection[0]);
    }

    [PgReady]
    private static LoanFolderRuleInfo[] getRulesFromDatabase()
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_LoanFolderRules"));
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        List<LoanFolderRuleInfo> loanFolderRuleInfoList = new List<LoanFolderRuleInfo>();
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          string str = SQL.DecodeString(row["loanFolder"]);
          if (string.Compare(str, SystemSettings.TrashFolder, true) != 0)
            loanFolderRuleInfoList.Add(new LoanFolderRuleInfo(str, row, current.Settings.DbServerType));
        }
        List<string> stringList = new List<string>((IEnumerable<string>) LoanFolder.GetAllLoanFolderNames(false));
        foreach (LoanFolderRuleInfo loanFolderRuleInfo in loanFolderRuleInfoList.ToArray())
        {
          if (stringList.Contains(loanFolderRuleInfo.LoanFolder))
            stringList.Remove(loanFolderRuleInfo.LoanFolder);
        }
        if (stringList.Count <= 0)
          return loanFolderRuleInfoList.ToArray();
        foreach (string loanFolder in stringList)
          LoanFolderBpmDbAccessor.saveRuleToDatabase(new LoanFolderRuleInfo(loanFolder));
        return LoanFolderBpmDbAccessor.getRulesFromDatabase();
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_LoanFolderRules"));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      List<LoanFolderRuleInfo> loanFolderRuleInfoList1 = new List<LoanFolderRuleInfo>();
      foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection1)
      {
        string str = SQL.DecodeString(row["loanFolder"]);
        if (string.Compare(str, SystemSettings.TrashFolder, true) != 0)
          loanFolderRuleInfoList1.Add(new LoanFolderRuleInfo(str, row));
      }
      List<string> stringList1 = new List<string>((IEnumerable<string>) LoanFolder.GetAllLoanFolderNames(false));
      foreach (LoanFolderRuleInfo loanFolderRuleInfo in loanFolderRuleInfoList1.ToArray())
      {
        if (stringList1.Contains(loanFolderRuleInfo.LoanFolder))
          stringList1.Remove(loanFolderRuleInfo.LoanFolder);
      }
      if (stringList1.Count <= 0)
        return loanFolderRuleInfoList1.ToArray();
      foreach (string loanFolder in stringList1)
        LoanFolderBpmDbAccessor.saveRuleToDatabase(new LoanFolderRuleInfo(loanFolder));
      return LoanFolderBpmDbAccessor.getRulesFromDatabase();
    }

    [PgReady]
    private static void saveRuleToDatabase(LoanFolderRuleInfo rule)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_LoanFolderRules");
        DbValue key = new DbValue("loanFolder", (object) rule.LoanFolder);
        pgDbQueryBuilder.DeleteFrom(table, key);
        DbValueList values = new DbValueList();
        values.Add(key);
        if (!rule.UseDefaultData)
        {
          values.Add("canOriginateLoans", (object) rule.CanOriginateLoans, (IDbEncoder) DbEncoding.Flag);
          values.Add("canDuplicateLoansFrom", (object) rule.CanDuplicateLoansFrom, (IDbEncoder) DbEncoding.Flag);
        }
        values.Add("canDuplicateLoansInto", (object) rule.CanDuplicateLoansInto, (IDbEncoder) DbEncoding.Flag);
        values.Add("canImportLoans", (object) rule.CanImportLoans, (IDbEncoder) DbEncoding.Flag);
        values.Add("canPiggybackLoans", (object) rule.CanPiggybackLoans, (IDbEncoder) DbEncoding.Flag);
        values.Add("moveRuleOption", (object) (int) rule.MoveRuleOption);
        values.Add("milestoneID", (object) (rule.MilestoneID ?? ""));
        for (int index = 0; index < 9; ++index)
          values.Add(BizRule.MoveRuleLoanStatusDbColNames[index], (object) rule.LoanStatusSettings[index], (IDbEncoder) DbEncoding.Flag);
        pgDbQueryBuilder.InsertInto(table, values, true, false);
        pgDbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_LoanFolderRules");
        DbValue key = new DbValue("loanFolder", (object) rule.LoanFolder);
        dbQueryBuilder.DeleteFrom(table, key);
        DbValueList values = new DbValueList();
        values.Add(key);
        if (!rule.UseDefaultData)
        {
          values.Add("canOriginateLoans", (object) rule.CanOriginateLoans, (IDbEncoder) DbEncoding.Flag);
          values.Add("canDuplicateLoansFrom", (object) rule.CanDuplicateLoansFrom, (IDbEncoder) DbEncoding.Flag);
        }
        values.Add("canDuplicateLoansInto", (object) rule.CanDuplicateLoansInto, (IDbEncoder) DbEncoding.Flag);
        values.Add("canImportLoans", (object) rule.CanImportLoans, (IDbEncoder) DbEncoding.Flag);
        values.Add("canPiggybackLoans", (object) rule.CanPiggybackLoans, (IDbEncoder) DbEncoding.Flag);
        values.Add("moveRuleOption", (object) (int) rule.MoveRuleOption);
        values.Add("milestoneID", (object) (rule.MilestoneID ?? ""));
        for (int index = 0; index < 9; ++index)
          values.Add(BizRule.MoveRuleLoanStatusDbColNames[index], (object) rule.LoanStatusSettings[index], (IDbEncoder) DbEncoding.Flag);
        dbQueryBuilder.InsertInto(table, values, true, false);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    private static void deleteRuleFromDatabase(string loanFolder)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_LoanFolderRules");
      DbValue key = new DbValue(nameof (loanFolder), (object) loanFolder);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void raiseCacheControlEvent()
    {
      ClientContext.GetCurrent().Sessions.BroadcastMessage((Message) new CacheControlMessage(ClientSessionCacheID.LoanFolderRule), false);
    }
  }
}

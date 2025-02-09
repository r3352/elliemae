// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.BpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public abstract class BpmDbAccessor
  {
    private const string className = "BpmDbAccessor�";
    private static Dictionary<BizRuleType, BpmDbAccessor> ruleAccessors = new Dictionary<BizRuleType, BpmDbAccessor>();
    private ClientSessionCacheID clientCacheId;

    static BpmDbAccessor()
    {
      BpmDbAccessor.ruleAccessors[BizRuleType.FieldAccess] = (BpmDbAccessor) new FieldAccessBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.FieldRules] = (BpmDbAccessor) new FieldRulesBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.InputForms] = (BpmDbAccessor) new InputFormsBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.LoanAccess] = (BpmDbAccessor) new LoanAccessBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.MilestoneRules] = (BpmDbAccessor) new MilestoneRulesBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.LoanActionCompletionRules] = (BpmDbAccessor) new LoanActionCompletionRulesBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.Triggers] = (BpmDbAccessor) new TriggersBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.PrintForms] = (BpmDbAccessor) new PrintFormsBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.PrintSelection] = (BpmDbAccessor) new PrintSelectionBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.AutomatedConditions] = (BpmDbAccessor) new AutomatedConditionBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.LoanActionAccess] = (BpmDbAccessor) new LoanActionAccessBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.AutoLockExclusionRules] = (BpmDbAccessor) new AutoLockExclusionRulesBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.DDMFeeScenarios] = (BpmDbAccessor) new DDMFeeRuleScenarioRulesBpmAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.DDMFieldScenarios] = (BpmDbAccessor) new DDMFieldRuleScenarioRuleBpmAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.MilestoneTemplateConditions] = (BpmDbAccessor) new AutomatedPurchaseConditionBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.DDMDataTables] = (BpmDbAccessor) new DDMDataTableDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.ServiceWorkflowRules] = (BpmDbAccessor) new ServiceWorkflowRulesBpmDbAccessor();
      BpmDbAccessor.ruleAccessors[BizRuleType.AutomatedEnhancedConditions] = (BpmDbAccessor) new AutomatedEnhancedConditionBpmDbAccessor();
    }

    protected BpmDbAccessor(ClientSessionCacheID cacheId) => this.clientCacheId = cacheId;

    public BizRuleInfo[] GetRules() => this.GetRules(BizRule.EmptyConditions, false);

    public BizRuleInfo[] GetRules(bool activeRulesOnly, bool toAcquireLock = true)
    {
      return this.GetRules(BizRule.EmptyConditions, activeRulesOnly, toAcquireLock);
    }

    public BizRuleInfo[] GetRules(
      bool activeRulesOnly,
      bool toAcquireLock,
      string additionalFilter = null)
    {
      return this.GetRules(BizRule.EmptyConditions, activeRulesOnly, toAcquireLock, additionalFilter);
    }

    public BizRuleInfo[] GetRules(
      BizRule.Condition condition,
      bool activeRulesOnly,
      bool toAcquireLock = true)
    {
      return this.GetRules(new BizRule.Condition[1]
      {
        condition
      }, activeRulesOnly, toAcquireLock);
    }

    public BizRuleInfo[] GetRules(
      BizRule.Condition[] conditions,
      bool activeRulesOnly,
      bool toAcquireLock = true,
      string additionalFilter = null)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (!this.isCachingEnabled() || !activeRulesOnly)
        return this.getRulesFromDatabase(conditions, activeRulesOnly, additionalFilter);
      BizRuleInfo[] o = this.getCachedRules(additionalFilter);
      if (o == null)
      {
        try
        {
          IDisposable disposable = (IDisposable) null;
          if (toAcquireLock)
            disposable = this.acquireCacheLock(timeout: EllieMae.EMLite.Server.ServerGlobals.LockTimeoutDuringGetCache);
          try
          {
            if (EllieMae.EMLite.Server.ServerGlobals.CacheRegetFromCache)
              o = this.getCachedRules(additionalFilter);
            if (o == null)
            {
              o = this.getRulesFromDatabase(BizRule.EmptyConditions, false, additionalFilter);
              current.Cache.Put(this.RuleTableName, (object) o, CacheSetting.Low);
            }
          }
          finally
          {
            disposable?.Dispose();
          }
        }
        catch (TimeoutException ex)
        {
          try
          {
            TraceLog.WriteWarning(nameof (BpmDbAccessor), "Timeout expired while acquiring lock on " + this.RuleTableName);
          }
          catch
          {
          }
          if (EllieMae.EMLite.Server.ServerGlobals.CacheRegetFromDB)
            o = this.getRulesFromDatabase(BizRule.EmptyConditions, false, additionalFilter);
          else
            throw;
        }
        catch (ApplicationException ex)
        {
          if (ex.Message.IndexOf("timeout period expired") > 0 || ex.HResult == -2147023436)
          {
            try
            {
              TraceLog.WriteWarning(nameof (BpmDbAccessor), "Timeout expired while acquiring lock on " + this.RuleTableName);
            }
            catch
            {
            }
            if (EllieMae.EMLite.Server.ServerGlobals.CacheRegetFromDB)
              o = this.getRulesFromDatabase(BizRule.EmptyConditions, false, additionalFilter);
            else
              throw;
          }
          else
            throw;
        }
      }
      List<BizRuleInfo> bizRuleInfoList = new List<BizRuleInfo>();
      if (conditions.Length == 0)
      {
        if (!(o != null & activeRulesOnly))
          return o;
        for (int index = 0; index < o.Length; ++index)
        {
          if (!o[index].Inactive)
            bizRuleInfoList.Add(o[index]);
        }
        return this.toTypedArray(bizRuleInfoList.ToArray());
      }
      List<BizRule.Condition> conditionList = new List<BizRule.Condition>((IEnumerable<BizRule.Condition>) conditions);
      for (int index = 0; index < o.Length; ++index)
      {
        if (conditionList.Contains(o[index].Condition) && (!activeRulesOnly || !o[index].Inactive))
          bizRuleInfoList.Add(o[index]);
      }
      return this.toTypedArray(bizRuleInfoList.ToArray());
    }

    public BizRuleInfo GetRule(int ruleId, bool toAcquireLock = true)
    {
      if (!this.isCachingEnabled())
        return this.getRuleFromDatabase(ruleId);
      foreach (BizRuleInfo rule in this.GetRules(BizRule.EmptyConditions, false, toAcquireLock))
      {
        if (rule.RuleID == ruleId)
          return rule;
      }
      return (BizRuleInfo) null;
    }

    public BizRuleInfo CreateNewRule(BizRuleInfo rule)
    {
      bool toAcquireLock = !SmartClientUtils.LockSlimNoRecursion;
      using (this.acquireCacheLock())
      {
        int ruleInDatabase = this.createRuleInDatabase(rule);
        this.invalidateCache();
        using (ClientContext.GetCurrent().MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?(true)))
          return this.GetRule(ruleInDatabase, toAcquireLock);
      }
    }

    public void InvalidateCache()
    {
      using (this.acquireCacheLock())
        this.invalidateCache();
    }

    public void DeleteRule(int ruleId)
    {
      using (this.acquireCacheLock())
      {
        this.deleteRuleFromDatabase(ruleId);
        this.invalidateCache();
      }
    }

    public void UpdateRule(BizRuleInfo rule)
    {
      using (this.acquireCacheLock())
      {
        this.updateRuleInDatabase(rule);
        this.invalidateCache();
      }
    }

    public BizRule.ActivationReturnCode ActivateRule(int ruleId, string userId, string fullName)
    {
      using (this.acquireCacheLock())
      {
        BizRule.ActivationReturnCode activationReturnCode = this.activateRuleInDatabase(ruleId, userId, fullName);
        if (activationReturnCode == BizRule.ActivationReturnCode.Succeed)
          this.invalidateCache();
        return activationReturnCode;
      }
    }

    public BizRule.ActivationReturnCode DeactivateRule(int ruleId, string userId, string fullName)
    {
      using (this.acquireCacheLock())
      {
        BizRule.ActivationReturnCode activationReturnCode = this.deactivateRuleInDatabase(ruleId, userId, fullName);
        if (activationReturnCode == BizRule.ActivationReturnCode.Succeed)
          this.invalidateCache();
        return activationReturnCode;
      }
    }

    public BizRuleInfo GetActiveRule(
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2)
    {
      if (!this.isCachingEnabled())
        return this.getActiveRuleFromDatabase(condition, condition2, conditionState, conditionState2);
      BizRuleInfo[] rules = this.GetRules();
      for (int index = 0; index < rules.Length; ++index)
      {
        if (!rules[index].Inactive && (rules[index].Condition == condition && condition == BizRule.Condition.Null || rules[index].Condition == condition && rules[index].ConditionState == conditionState && (rules[index].ConditionState2 ?? "") == (conditionState2 ?? "")))
          return rules[index];
      }
      return (BizRuleInfo) null;
    }

    public BizRuleInfo GetActiveRule(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      string conditionState1 = BizRule.ConditionStateAsString(condition, conditionState, milestoneID);
      return this.GetActiveRule(condition, condition2, conditionState1, conditionState2);
    }

    public DateTime GetLastRuleModificationTime()
    {
      if (!this.isCachingEnabled())
        return this.getRuleLastModificationTimeFromDatabase(-1);
      DateTime modificationTime = DateTime.MinValue;
      foreach (BizRuleInfo rule in this.GetRules())
      {
        if (rule.LastModTime > modificationTime)
          modificationTime = rule.LastModTime;
      }
      return modificationTime;
    }

    protected abstract string RuleTableName { get; }

    protected abstract BizRuleInfo[] GetFilteredRulesFromDatabase(string filter);

    protected abstract void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue keyValue);

    protected abstract void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue keyValue);

    protected abstract Type RuleType { get; }

    private bool isCachingEnabled()
    {
      return ClientContext.GetCurrent().Settings.CacheSetting >= CacheSetting.Low;
    }

    private IDisposable acquireCacheLock(EllieMae.EMLite.ClientServer.LockType lockType = EllieMae.EMLite.ClientServer.LockType.ReaderWriter, int timeout = 20000)
    {
      return ClientContext.GetCurrent().Cache.Lock(this.RuleTableName, lockType, timeout);
    }

    private void invalidateCache()
    {
      ClientContext.GetCurrent().Cache.Put<BizRuleInfo[]>(this.RuleTableName, (Func<BizRuleInfo[]>) (() => this.getRulesFromDatabase(BizRule.EmptyConditions, true)), CacheSetting.Low);
      this.raiseCacheControlEvent();
    }

    private BizRuleInfo[] getCachedRules(string additionalFilter = null)
    {
      return this.GetFilteredRulesFromCache((BizRuleInfo[]) ClientContext.GetCurrent().Cache.Get(this.RuleTableName), additionalFilter);
    }

    protected virtual BizRuleInfo[] GetFilteredRulesFromCache(
      BizRuleInfo[] rules,
      string additionalFilter)
    {
      return rules;
    }

    private void raiseCacheControlEvent()
    {
      ClientContext.GetCurrent().Sessions.BroadcastMessage((Message) new CacheControlMessage(this.clientCacheId), false);
    }

    private BizRuleInfo[] toTypedArray(BizRuleInfo[] rules)
    {
      Array instance = Array.CreateInstance(this.RuleType, rules.Length);
      rules.CopyTo(instance, 0);
      return (BizRuleInfo[]) instance;
    }

    protected virtual DbValueList getDbValueList(BizRuleInfo rule)
    {
      return new DbValueList()
      {
        {
          "ruleName",
          (object) rule.RuleName
        },
        {
          "condition",
          (object) (int) rule.Condition
        },
        {
          "condition2",
          (object) rule.Condition2
        },
        {
          "conditionState",
          rule.Condition == BizRule.Condition.AdvancedCoding ? (object) "" : (object) rule.ConditionState
        },
        {
          "conditionState2",
          (object) rule.ConditionState2
        },
        {
          "advancedCode",
          rule.Condition == BizRule.Condition.AdvancedCoding ? (object) rule.ConditionState : (object) (string) null
        },
        {
          "advancedCodeXml",
          rule.Condition == BizRule.Condition.AdvancedCoding ? (object) rule.AdvancedCodeXML : (object) (string) null
        },
        {
          "status",
          (object) (int) rule.Status
        },
        {
          "lastModTime",
          (object) "GetDate()",
          (IDbEncoder) DbEncoding.None
        },
        {
          "lastModifiedByFullName",
          (object) rule.LastModifiedByFullName
        },
        {
          "lastModifiedByUserId",
          (object) rule.LastModifiedByUserId
        },
        {
          "Comments",
          (object) rule.CommentsTxt
        }
      };
    }

    protected virtual DbValueList GetDbKeyValueList(BizRuleInfo rule)
    {
      return new DbValueList()
      {
        {
          "ruleID",
          (object) rule.RuleID
        }
      };
    }

    private static string getBaseConditionFilter(BizRule.Condition condition, bool activeRulesOnly)
    {
      return BpmDbAccessor.getBaseConditionFilter(new BizRule.Condition[1]
      {
        condition
      }, activeRulesOnly);
    }

    private static string getBaseConditionFilter(
      BizRule.Condition[] conditions,
      bool activeRulesOnly,
      string additionalFilter = null)
    {
      List<string> stringList = new List<string>();
      if (conditions.Length != 0)
      {
        int[] data = new int[conditions.Length];
        for (int index = 0; index < conditions.Length; ++index)
          data[index] = (int) conditions[index];
        stringList.Add("(rules.condition in (" + SQL.EncodeArray((Array) data) + "))");
      }
      if (activeRulesOnly)
        stringList.Add("(rules.status = " + (object) 1 + ")");
      if (stringList.Count == 0)
        stringList.Add("(1 = 1)");
      if (additionalFilter != null)
        stringList.Add(additionalFilter);
      return string.Join(" and ", stringList.ToArray());
    }

    private static string getBaseConditionFilter(
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      bool activeRulesOnly)
    {
      List<string> stringList = new List<string>();
      stringList.Add(BpmDbAccessor.getBaseConditionFilter(condition, activeRulesOnly));
      if (condition != BizRule.Condition.Null)
      {
        stringList.Add("(rules.conditionState = " + SQL.Encode((object) (conditionState ?? "")) + ")");
        if ((conditionState2 ?? "") != "")
          stringList.Add("(IsNull(rules.conditionState2, '') = " + SQL.Encode((object) (conditionState2 ?? "")) + ")");
      }
      return string.Join(" and ", stringList.ToArray());
    }

    private BizRuleInfo[] getRulesFromDatabase(
      BizRule.Condition[] conditions,
      bool activeRulesOnly,
      string additionalFilter = null)
    {
      return this.GetFilteredRulesFromDatabase(BpmDbAccessor.getBaseConditionFilter(conditions, activeRulesOnly, additionalFilter));
    }

    private BizRuleInfo getRuleFromDatabase(int ruleId)
    {
      BizRuleInfo[] rulesFromDatabase = this.GetFilteredRulesFromDatabase("rules.ruleID = " + (object) ruleId);
      return rulesFromDatabase.Length == 0 ? (BizRuleInfo) null : rulesFromDatabase[0];
    }

    private DateTime updateRuleInDatabase(BizRuleInfo rule)
    {
      if (rule.RuleID <= 0)
        return DateTime.MinValue;
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(this.RuleTableName);
      DbValue keyValue = new DbValue("ruleId", (object) rule.RuleID);
      sql.Update(table, this.getDbValueList(rule), this.GetDbKeyValueList(rule));
      this.DeleteAuxiliaryDataFromQuery(sql, keyValue);
      this.WriteAuxiliaryDataToQuery(sql, rule, keyValue);
      sql.SelectFrom(table, new string[1]{ "lastModTime" }, new DbValue("ruleId", (object) rule.RuleID));
      return (DateTime) SQL.Decode(sql.ExecuteScalar(), (object) DateTime.MinValue);
    }

    private int createRuleInDatabase(BizRuleInfo rule)
    {
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(this.RuleTableName);
      sql.Declare("@ruleId", "int");
      sql.InsertInto(table, this.getDbValueList(rule), true, false);
      sql.SelectIdentity("@ruleId");
      DbValue keyValue = new DbValue("ruleId", (object) "@ruleId", (IDbEncoder) DbEncoding.None);
      this.WriteAuxiliaryDataToQuery(sql, rule, keyValue);
      sql.Select("@ruleId");
      return (int) SQL.Decode(sql.ExecuteScalar(), (object) -1);
    }

    private void deleteRuleFromDatabase(int ruleId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue dbValue = new DbValue(nameof (ruleId), (object) ruleId);
      this.DeleteAuxiliaryDataFromQuery(sql, dbValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(this.RuleTableName), dbValue);
      sql.ExecuteNonQuery();
    }

    private BizRule.ActivationReturnCode activateRuleInDatabase(
      int ruleId,
      string userId,
      string fullName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @status tinyint");
      dbQueryBuilder.AppendLine("select @status = status from " + this.RuleTableName + " where ruleID = " + (object) ruleId);
      dbQueryBuilder.AppendLine("if @status is NULL");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tselect " + (object) -1);
      dbQueryBuilder.AppendLine("\treturn");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.AppendLine("if @status = " + (object) 1);
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tselect " + (object) 1);
      dbQueryBuilder.AppendLine("\treturn");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.AppendLine("update " + this.RuleTableName + " set status = " + (object) 1 + ", lastModifiedByUserId = " + SQL.Encode((object) userId) + ", lastModifiedByFullName = " + SQL.Encode((object) fullName) + ", lastModTime = GetDate() where ruleID = " + (object) ruleId);
      dbQueryBuilder.AppendLine("select " + (object) 0);
      return (BizRule.ActivationReturnCode) dbQueryBuilder.ExecuteScalar();
    }

    private BizRule.ActivationReturnCode deactivateRuleInDatabase(
      int ruleId,
      string userId,
      string fullName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @status tinyint");
      dbQueryBuilder.AppendLine("select @status = status from " + this.RuleTableName + " where ruleID = " + (object) ruleId);
      dbQueryBuilder.AppendLine("if @status is NULL");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tselect " + (object) -1);
      dbQueryBuilder.AppendLine("\treturn");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.AppendLine("update " + this.RuleTableName + " set status = " + (object) 0 + ", lastModifiedByUserId = " + SQL.Encode((object) userId) + ", lastModifiedByFullName = " + SQL.Encode((object) fullName) + ", lastModTime = GetDate() where ruleID = " + (object) ruleId);
      dbQueryBuilder.AppendLine("select " + (object) 0);
      return (BizRule.ActivationReturnCode) dbQueryBuilder.ExecuteScalar();
    }

    private BizRuleInfo getActiveRuleFromDatabase(
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2)
    {
      BizRuleInfo[] rulesFromDatabase = this.GetFilteredRulesFromDatabase(BpmDbAccessor.getBaseConditionFilter(condition, condition2, conditionState, conditionState2, true));
      return rulesFromDatabase.Length == 0 ? (BizRuleInfo) null : rulesFromDatabase[0];
    }

    [PgReady]
    public DateTime getRuleLastModificationTimeFromDatabase(int ruleId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.Append("select Max(lastModTime) from " + this.RuleTableName);
        if (ruleId > 0)
          pgDbQueryBuilder.Append(" where ruleID = " + (object) ruleId);
        return (DateTime) SQL.Decode(pgDbQueryBuilder.ExecuteScalar(), (object) DateTime.MinValue);
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("select Max(lastModTime) from " + this.RuleTableName);
      if (ruleId > 0)
        dbQueryBuilder.Append(" where ruleID = " + (object) ruleId);
      return (DateTime) SQL.Decode(dbQueryBuilder.ExecuteScalar(), (object) DateTime.MinValue);
    }

    public static BpmDbAccessor GetAccessor(BizRuleType ruleType)
    {
      return !BpmDbAccessor.ruleAccessors.ContainsKey(ruleType) ? (BpmDbAccessor) null : BpmDbAccessor.ruleAccessors[ruleType];
    }

    public static BpmDbAccessor GetAccessorForRuleType(Type ruleType)
    {
      return BpmDbAccessor.GetAccessor(BpmDbAccessor.GetBizRuleType(ruleType));
    }

    public static BizRuleType GetBizRuleType(Type ruleType)
    {
      if (ruleType == typeof (FieldAccessRuleInfo))
        return BizRuleType.FieldAccess;
      if (ruleType == typeof (FieldRuleInfo))
        return BizRuleType.FieldRules;
      if (ruleType == typeof (InputFormRuleInfo))
        return BizRuleType.InputForms;
      if (ruleType == typeof (LoanAccessRuleInfo))
        return BizRuleType.LoanAccess;
      if (ruleType == typeof (MilestoneRuleInfo))
        return BizRuleType.MilestoneRules;
      if (ruleType == typeof (LoanActionCompletionRuleInfo))
        return BizRuleType.LoanActionCompletionRules;
      if (ruleType == typeof (TriggerInfo))
        return BizRuleType.Triggers;
      if (ruleType == typeof (PrintFormRuleInfo))
        return BizRuleType.PrintForms;
      if (ruleType == typeof (PrintSelectionRuleInfo))
        return BizRuleType.PrintSelection;
      if (ruleType == typeof (LoanFolderRuleInfo))
        return BizRuleType.LoanFolderRules;
      if (ruleType == typeof (AutomatedConditionRuleInfo))
        return BizRuleType.AutomatedConditions;
      if (ruleType == typeof (LoanActionAccessRuleInfo))
        return BizRuleType.LoanActionAccess;
      if (ruleType == typeof (AutoLockExclusionRuleInfo))
        return BizRuleType.AutoLockExclusionRules;
      if (ruleType == typeof (DDMFeeRuleScenario))
        return BizRuleType.DDMFeeScenarios;
      if (ruleType == typeof (DDMFieldRuleScenario))
        return BizRuleType.DDMFieldScenarios;
      if (ruleType == typeof (AutomatedPurchaseConditionRuleInfo))
        return BizRuleType.MilestoneTemplateConditions;
      if (ruleType == typeof (ServiceWorkflowRulesBpmDbAccessor))
        return BizRuleType.ServiceWorkflowRules;
      if (ruleType == typeof (AutomatedEnhancedConditionRuleInfo))
        return BizRuleType.AutomatedEnhancedConditions;
      throw new Exception("The rule type " + ruleType.Name + " is unknown or invalid");
    }

    public static BizRuleInfo[] GetRules(BizRuleType[] ruleTypes)
    {
      return BpmDbAccessor.GetRules(ruleTypes, false);
    }

    public static BizRuleInfo[] GetRules(BizRuleType[] ruleTypes, bool activeOnly)
    {
      return BpmDbAccessor.GetRules(ruleTypes, BizRule.EmptyConditions, activeOnly);
    }

    public static BizRuleInfo[] GetRules(
      BizRuleType[] ruleTypes,
      BizRule.Condition condition,
      bool activeOnly)
    {
      return BpmDbAccessor.GetRules(ruleTypes, new BizRule.Condition[1]
      {
        condition
      }, (activeOnly ? 1 : 0) != 0);
    }

    public static BizRuleInfo[] GetRules(
      BizRuleType[] ruleTypes,
      BizRule.Condition[] conditions,
      bool activeOnly)
    {
      List<BizRuleInfo> bizRuleInfoList = new List<BizRuleInfo>();
      foreach (BizRuleType ruleType in ruleTypes)
      {
        BpmDbAccessor accessor = BpmDbAccessor.GetAccessor(ruleType);
        if (accessor != null)
          bizRuleInfoList.AddRange((IEnumerable<BizRuleInfo>) accessor.GetRules(conditions, activeOnly));
      }
      return bizRuleInfoList.ToArray();
    }

    public static DateTime GetLastModificationTime()
    {
      DateTime modificationTime1 = DateTime.MinValue;
      foreach (BizRuleType ruleType in Enum.GetValues(typeof (BizRuleType)))
      {
        BpmDbAccessor accessor = BpmDbAccessor.GetAccessor(ruleType);
        if (accessor != null)
        {
          DateTime modificationTime2 = accessor.GetLastRuleModificationTime();
          if (modificationTime2 > modificationTime1)
            modificationTime1 = modificationTime2;
        }
      }
      return modificationTime1;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.MilestoneValidatorCache
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public static class MilestoneValidatorCache
  {
    private const string className = "MilestoneValidatorCache�";
    private static readonly string sw = Tracing.SwDataEngine;
    private static Hashtable validators = new Hashtable();
    private static object syncRoot = new object();

    public static MilestoneValidators GetMilestoneValidators(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo)
    {
      string key = sessionObjects.StartupInfo.ServerInstanceName + "@" + sessionObjects.StartupInfo.ServerID;
      MilestoneValidatorCache.ServerValidators serverValidators = (MilestoneValidatorCache.ServerValidators) null;
      lock (MilestoneValidatorCache.syncRoot)
      {
        if (!MilestoneValidatorCache.validators.Contains((object) key))
          MilestoneValidatorCache.validators[(object) key] = (object) new MilestoneValidatorCache.ServerValidators();
        serverValidators = (MilestoneValidatorCache.ServerValidators) MilestoneValidatorCache.validators[(object) key];
      }
      return serverValidators.GetMilestoneValidators(sessionObjects, configInfo);
    }

    public static DateTime GetValidatorsModificationTimestamp(SessionObjects sessionObjects)
    {
      string key = sessionObjects.StartupInfo.ServerInstanceName + "@" + sessionObjects.StartupInfo.ServerID;
      MilestoneValidatorCache.ServerValidators serverValidators = (MilestoneValidatorCache.ServerValidators) null;
      lock (MilestoneValidatorCache.syncRoot)
      {
        if (!MilestoneValidatorCache.validators.Contains((object) key))
          return DateTime.MinValue;
        serverValidators = (MilestoneValidatorCache.ServerValidators) MilestoneValidatorCache.validators[(object) key];
      }
      return serverValidators.LastModificationTime;
    }

    private class ServerValidators
    {
      private MilestoneValidators vals;
      private DateTime lastModified = DateTime.MinValue;
      private object syncRoot = new object();

      public DateTime LastModificationTime
      {
        get
        {
          lock (this.syncRoot)
            return this.lastModified;
        }
      }

      public MilestoneValidators GetMilestoneValidators(
        SessionObjects sessionObjects,
        ILoanConfigurationInfo configInfo)
      {
        lock (this.syncRoot)
        {
          if (sessionObjects.UserInfo.IsSuperAdministrator())
          {
            this.vals = new MilestoneValidators(new MilestoneRule[0]);
            this.lastModified = DateTime.MaxValue;
          }
          else if (this.vals == null || configInfo.MilestoneRulesModificationTime > this.lastModified)
          {
            this.vals = new MilestoneValidators(this.generateMilestoneRules((MilestoneRuleInfo[]) sessionObjects.BpmManager.GetRules(BizRuleType.MilestoneRules, true)));
            this.lastModified = configInfo.MilestoneRulesModificationTime;
          }
          return this.vals;
        }
      }

      private MilestoneRule[] generateMilestoneRules(MilestoneRuleInfo[] ruleInfos)
      {
        List<MilestoneRule> milestoneRuleList = new List<MilestoneRule>();
        foreach (MilestoneRuleInfo ruleInfo in ruleInfos)
        {
          foreach (AdvancedCodeMilestonePair advancedCode in ruleInfo.AdvancedCodes)
          {
            RuleCondition ruleCondition = BizRuleTranslator.GetRuleCondition((BizRuleInfo) ruleInfo);
            milestoneRuleList.Add(new MilestoneRule(ruleInfo.RuleName, ruleCondition, advancedCode));
          }
        }
        return milestoneRuleList.ToArray();
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.BpmManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Server.ServerObjects.StatusOnline;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class BpmManager : SessionBoundObject, IBpmManager
  {
    private const string className = "BpmManager";

    public BpmManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (BpmManager).ToLower());
      return this;
    }

    public virtual BizRuleInfo CreateNewRule(BizRuleInfo rule, bool isGlobalSetting = false)
    {
      this.updateLastModifiedInfo(rule);
      this.onApiCalled(nameof (BpmManager), nameof (CreateNewRule), new object[1]
      {
        (object) rule
      });
      try
      {
        BizRuleInfo newRule = BpmDbAccessor.GetAccessorForRuleType(rule.GetType()).CreateNewRule(rule);
        SystemAuditRecord record;
        switch (rule.RuleType)
        {
          case BizRuleType.DDMFeeScenarios:
            record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FeeScenarioCreated, DateTime.Now, Convert.ToString(((DDMFeeRuleScenario) rule).Order), ((DDMFeeRuleScenario) rule).ParentRuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())), rule.RuleName, isGlobalSetting ? "Created through Global Rule Settings." : string.Empty);
            break;
          case BizRuleType.DDMFieldScenarios:
            record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FieldScenarioCreated, DateTime.Now, Convert.ToString(((DDMFieldRuleScenario) rule).Order), ((DDMFieldRuleScenario) rule).ParentRuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())), rule.RuleName, isGlobalSetting ? "Created through Global Rule Settings." : string.Empty);
            break;
          default:
            record = (SystemAuditRecord) new BizRuleAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessRuleCreated, DateTime.Now, string.Concat((object) newRule.RuleID), rule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())));
            break;
        }
        SystemAuditTrailAccessor.InsertAuditRecord(record);
        this.CreateFieldSearchInfo(newRule);
        return newRule;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo) null;
      }
    }

    public virtual BizRuleInfo[] GetRules()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), Array.Empty<object>());
      try
      {
        return BpmDbAccessor.GetRules(BizRule.BizRuleTypes, false);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetAllRules()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllRules), Array.Empty<object>());
      try
      {
        return BpmDbAccessor.GetRules(BizRule.AllBizRuleTypes, false);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(bool activeOnly)
    {
      this.onApiCalled(nameof (BpmManager), "GetAllRules", new object[1]
      {
        (object) activeOnly
      });
      try
      {
        return BpmDbAccessor.GetRules(BizRule.BizRuleTypes, activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(BizRule.Condition condition, bool activeOnly)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), new object[2]
      {
        (object) condition,
        (object) activeOnly
      });
      try
      {
        return BpmDbAccessor.GetRules(BizRule.BizRuleTypes, condition, activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(BizRule.Condition[] conditions, bool activeOnly)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), new object[2]
      {
        (object) conditions,
        (object) activeOnly
      });
      try
      {
        return BpmDbAccessor.GetRules(BizRule.BizRuleTypes, conditions, activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(BizRuleType ruleType)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), new object[1]
      {
        (object) ruleType
      });
      try
      {
        return BpmDbAccessor.GetAccessor(ruleType).GetRules();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(
      BizRuleType ruleType,
      bool activeOnly,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), new object[3]
      {
        (object) ruleType,
        (object) activeOnly,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return BpmDbAccessor.GetAccessor(ruleType).GetRules(activeOnly, true, (string) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(
      BizRuleType ruleType,
      BizRule.Condition condition,
      bool activeOnly)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), new object[3]
      {
        (object) ruleType,
        (object) condition,
        (object) activeOnly
      });
      try
      {
        return BpmDbAccessor.GetAccessor(ruleType).GetRules(condition, activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(
      BizRuleType ruleType,
      BizRule.Condition[] conditions,
      bool activeOnly,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), new object[4]
      {
        (object) ruleType,
        (object) conditions,
        (object) activeOnly,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return BpmDbAccessor.GetAccessor(ruleType).GetRules(conditions, activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(BizRuleType[] ruleTypes)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), new object[1]
      {
        (object) ruleTypes
      });
      try
      {
        return BpmDbAccessor.GetRules(ruleTypes);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(BizRuleType[] ruleTypes, bool activeOnly)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), new object[2]
      {
        (object) ruleTypes,
        (object) activeOnly
      });
      try
      {
        return BpmDbAccessor.GetRules(ruleTypes, activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(
      BizRuleType[] ruleTypes,
      BizRule.Condition condition,
      bool activeOnly)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), new object[3]
      {
        (object) ruleTypes,
        (object) condition,
        (object) activeOnly
      });
      try
      {
        return BpmDbAccessor.GetRules(ruleTypes, condition, activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo[] GetRules(
      BizRuleType[] ruleTypes,
      BizRule.Condition[] conditions,
      bool activeOnly)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRules), new object[3]
      {
        (object) ruleTypes,
        (object) conditions,
        (object) activeOnly
      });
      try
      {
        return BpmDbAccessor.GetRules(ruleTypes, conditions, activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo[]) null;
      }
    }

    public virtual FieldAccessRuleInfo[] GetActiveFieldAccessRulesByPersonas(int[] personaIds)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetActiveFieldAccessRulesByPersonas), new object[1]
      {
        (object) personaIds
      });
      try
      {
        return FieldAccessBpmDbAccessor.GetActiveFieldAccessRulesByPersona(personaIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (FieldAccessRuleInfo[]) null;
      }
    }

    public virtual BizRuleInfo GetRule(BizRuleType ruleType, int ruleID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRule), new object[2]
      {
        (object) ruleType,
        (object) ruleID
      });
      try
      {
        return BpmDbAccessor.GetAccessor(ruleType).GetRule(ruleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo) null;
      }
    }

    public virtual void UpdateRule(BizRuleInfo rule, bool isGlobalSetting = false)
    {
      this.updateLastModifiedInfo(rule);
      this.onApiCalled(nameof (BpmManager), nameof (UpdateRule), new object[1]
      {
        (object) rule
      });
      try
      {
        BpmDbAccessor.GetAccessorForRuleType(rule.GetType()).UpdateRule(rule);
        SystemAuditRecord record;
        switch (rule.RuleType)
        {
          case BizRuleType.DDMFeeScenarios:
            record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FeeScenarioModified, DateTime.Now, Convert.ToString(((DDMFeeRuleScenario) rule).Order), ((DDMFeeRuleScenario) rule).ParentRuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())), rule.RuleName, isGlobalSetting ? "Modified through Global Rule Settings." : string.Empty);
            break;
          case BizRuleType.DDMFieldScenarios:
            record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FieldScenarioModified, DateTime.Now, Convert.ToString(((DDMFieldRuleScenario) rule).Order), ((DDMFieldRuleScenario) rule).ParentRuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())), rule.RuleName, isGlobalSetting ? "Modified through Global Rule Settings." : string.Empty);
            break;
          default:
            record = (SystemAuditRecord) new BizRuleAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessRuleModified, DateTime.Now, string.Concat((object) rule.RuleID), rule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())));
            break;
        }
        SystemAuditTrailAccessor.InsertAuditRecord(record);
        this.UpdateFieldSearchInfo(rule);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void InvalidateCache(BizRuleType ruleType)
    {
      this.onApiCalled(nameof (BpmManager), "InvalidateRule", new object[1]
      {
        (object) ruleType
      });
      try
      {
        BpmDbAccessor.GetAccessor(ruleType).InvalidateCache();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void DeleteRule(
      BizRuleType ruleType,
      int ruleId,
      bool isGlobalSetting = false,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeleteRule), new object[3]
      {
        (object) ruleType,
        (object) ruleId,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          BizRuleInfo rule = BpmDbAccessor.GetAccessor(ruleType).GetRule(ruleId);
          string ruleName = rule.RuleName;
          BpmDbAccessor.GetAccessor(ruleType).DeleteRule(ruleId);
          SystemAuditRecord record;
          switch (rule.RuleType)
          {
            case BizRuleType.DDMFeeScenarios:
              record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FeeScenarioDeleted, DateTime.Now, Convert.ToString(((DDMFeeRuleScenario) rule).Order), ((DDMFeeRuleScenario) rule).ParentRuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())), rule.RuleName, isGlobalSetting ? "Deleted through Global Rule Settings." : string.Empty);
              break;
            case BizRuleType.DDMFieldScenarios:
              record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FieldScenarioDeleted, DateTime.Now, Convert.ToString(((DDMFieldRuleScenario) rule).Order), ((DDMFieldRuleScenario) rule).ParentRuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())), rule.RuleName, isGlobalSetting ? "Deleted through Global Rule Settings." : string.Empty);
              break;
            default:
              record = (SystemAuditRecord) new BizRuleAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessRuleDeleted, DateTime.Now, string.Concat((object) rule.RuleID), rule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())));
              break;
          }
          SystemAuditTrailAccessor.InsertAuditRecord(record);
          this.DeleteFieldSearchInfo(rule);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual DateTime GetLastRuleModificationTime()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetLastRuleModificationTime), Array.Empty<object>());
      try
      {
        return BpmDbAccessor.GetLastModificationTime();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return DateTime.MinValue;
      }
    }

    public virtual DateTime GetLastRuleModificationTime(BizRuleType ruleType)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetLastRuleModificationTime), new object[1]
      {
        (object) ruleType
      });
      try
      {
        return BpmDbAccessor.GetAccessor(ruleType).GetLastRuleModificationTime();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return DateTime.MinValue;
      }
    }

    public virtual BizRule.ActivationReturnCode ActivateRule(
      BizRuleType ruleType,
      int ruleID,
      bool isGlobalSetting = false,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (ActivateRule), new object[5]
      {
        (object) ruleType,
        (object) ruleID,
        (object) this.Session.UserID,
        (object) this.Session.GetUserInfo().FullName,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          BizRule.ActivationReturnCode activationReturnCode = BpmDbAccessor.GetAccessor(ruleType).ActivateRule(ruleID, this.Session.UserID, this.Session.GetUserInfo().FullName);
          if (activationReturnCode != BizRule.ActivationReturnCode.Succeed)
            return activationReturnCode;
          BizRuleInfo rule = BpmDbAccessor.GetAccessor(ruleType).GetRule(ruleID);
          string ruleName = rule.RuleName;
          SystemAuditRecord record;
          switch (rule.RuleType)
          {
            case BizRuleType.DDMFeeScenarios:
              record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FeeScenarioActivated, DateTime.Now, Convert.ToString(((DDMFeeRuleScenario) rule).Order), ((DDMFeeRuleScenario) rule).ParentRuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())), rule.RuleName, isGlobalSetting ? "Activated through Global Rule Settings." : string.Empty);
              break;
            case BizRuleType.DDMFieldScenarios:
              record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FieldScenarioActivated, DateTime.Now, Convert.ToString(((DDMFieldRuleScenario) rule).Order), ((DDMFieldRuleScenario) rule).ParentRuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())), rule.RuleName, isGlobalSetting ? "Activated through Global Rule Settings." : string.Empty);
              break;
            default:
              record = (SystemAuditRecord) new BizRuleAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessRuleActivated, DateTime.Now, string.Concat((object) rule.RuleID), rule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())));
              break;
          }
          SystemAuditTrailAccessor.InsertAuditRecord(record);
          this.ChagneFieldSearchRuleStatus(rule, BizRule.RuleStatus.Active);
          return activationReturnCode;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return BizRule.ActivationReturnCode.NoOp;
      }
    }

    public virtual BizRule.ActivationReturnCode DeactivateRule(
      BizRuleType ruleType,
      int ruleId,
      bool isGlobalSetting = false,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeactivateRule), new object[5]
      {
        (object) ruleType,
        (object) ruleId,
        (object) this.Session.UserID,
        (object) this.Session.GetUserInfo().FullName,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          BizRule.ActivationReturnCode activationReturnCode = BpmDbAccessor.GetAccessor(ruleType).DeactivateRule(ruleId, this.Session.UserID, this.Session.GetUserInfo().FullName);
          if (activationReturnCode != BizRule.ActivationReturnCode.Succeed)
            return activationReturnCode;
          BizRuleInfo rule = BpmDbAccessor.GetAccessor(ruleType).GetRule(ruleId);
          string ruleName = rule.RuleName;
          SystemAuditRecord record;
          switch (rule.RuleType)
          {
            case BizRuleType.DDMFeeScenarios:
              record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FeeScenarioDeactivated, DateTime.Now, Convert.ToString(((DDMFeeRuleScenario) rule).Order), ((DDMFeeRuleScenario) rule).ParentRuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())), rule.RuleName, isGlobalSetting ? "Deactivated through Global Rule Settings." : string.Empty);
              break;
            case BizRuleType.DDMFieldScenarios:
              record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FieldScenarioDeactivated, DateTime.Now, Convert.ToString(((DDMFieldRuleScenario) rule).Order), ((DDMFieldRuleScenario) rule).ParentRuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())), rule.RuleName, isGlobalSetting ? "Deactivated through Global Rule Settings." : string.Empty);
              break;
            default:
              record = (SystemAuditRecord) new BizRuleAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessRuleDeactivated, DateTime.Now, string.Concat((object) rule.RuleID), rule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType())));
              break;
          }
          SystemAuditTrailAccessor.InsertAuditRecord(record);
          this.ChagneFieldSearchRuleStatus(rule, BizRule.RuleStatus.Inactive);
          return activationReturnCode;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return BizRule.ActivationReturnCode.NoOp;
      }
    }

    public virtual BizRuleInfo GetActiveRule(
      BizRuleType ruleType,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2)
    {
      this.onApiCalled(nameof (BpmManager), "GetActiveRuleID", new object[5]
      {
        (object) ruleType,
        (object) condition,
        (object) condition2,
        (object) conditionState,
        (object) conditionState2
      });
      try
      {
        return BpmDbAccessor.GetAccessor(ruleType).GetActiveRule(condition, condition2, conditionState, conditionState2);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (BizRuleInfo) null;
      }
    }

    public virtual FieldRuleFieldIDAndType[] GetInconsistentRuleFields(int ruleId)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetInconsistentRuleFields), new object[1]
      {
        (object) ruleId
      });
      try
      {
        return FieldRulesBpmDbAccessor.GetInconsistentFields(ruleId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (FieldRuleFieldIDAndType[]) null;
      }
    }

    public virtual DocumentDefaultAccessRuleInfo[] GetDocumentDefaultAccessRules()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDocumentDefaultAccessRules), Array.Empty<object>());
      try
      {
        return DocumentAccessRulesDbAccessor.GetDocumentDefaultAccessRules();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DocumentDefaultAccessRuleInfo[]) null;
      }
    }

    public virtual DocumentDefaultAccessRuleInfo GetDocumentDefaultAccessRule(int roleAddedBy)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDocumentDefaultAccessRule), new object[1]
      {
        (object) roleAddedBy
      });
      try
      {
        return DocumentAccessRulesDbAccessor.GetDocumentDefaultAccessRule(roleAddedBy);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DocumentDefaultAccessRuleInfo) null;
      }
    }

    public virtual void SaveDocumentDefaultAccessRules(DocumentDefaultAccessRuleInfo[] rules)
    {
      this.onApiCalled(nameof (BpmManager), nameof (SaveDocumentDefaultAccessRules), (object[]) rules);
      try
      {
        DocumentAccessRulesDbAccessor.SaveDocumentDefaultAccessRules(rules);
        foreach (DocumentDefaultAccessRuleInfo rule in rules)
        {
          string ruleName = "Role Access To Document";
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new BizRuleAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessRuleModified, DateTime.Now, string.Concat((object) rule.RoleAddedBy), ruleName, AuditObjectType.BizRule_DocumentDefaultAccessRule));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void SetLoanFolderAccessRule(LoanFolderRuleInfo rule)
    {
      this.onApiCalled(nameof (BpmManager), nameof (SetLoanFolderAccessRule), new object[1]
      {
        (object) rule
      });
      try
      {
        LoanFolderBpmDbAccessor.SetRule(rule);
        string ruleName = "Loan Folder Rules";
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new BizRuleAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessRuleModified, DateTime.Now, rule.LoanFolder, ruleName, SystemAuditTrailEnums.GetAuditObjectType(BpmDbAccessor.GetBizRuleType(rule.GetType()))));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual LoanFolderRuleInfo[] GetLoanFolderAccessRules()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetLoanFolderAccessRules), Array.Empty<object>());
      try
      {
        return LoanFolderBpmDbAccessor.GetRules();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (LoanFolderRuleInfo[]) null;
      }
    }

    public virtual LoanFolderRuleInfo GetLoanFolderAccessRule(string loanFolder)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetLoanFolderAccessRule), new object[1]
      {
        (object) loanFolder
      });
      try
      {
        return LoanFolderBpmDbAccessor.GetRule(loanFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (LoanFolderRuleInfo) null;
      }
    }

    public virtual void DeleteLoanFolderAccessRule(string loanFolder)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeleteLoanFolderAccessRule), new object[1]
      {
        (object) loanFolder
      });
      try
      {
        LoanFolderBpmDbAccessor.DeleteRule(loanFolder);
        string ruleName = "Loan Folder Rules";
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new BizRuleAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessRuleDeleted, DateTime.Now, loanFolder, ruleName, AuditObjectType.BizRule_LoanFolderRules));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual string[] GetAllowedOriginationLoanFolders()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllowedOriginationLoanFolders), Array.Empty<object>());
      try
      {
        return LoanFolderBpmDbAccessor.GetLoanFolders(true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (string[]) null;
      }
    }

    public virtual int CreateDDMFeeRule(DDMFeeRule ddmFeeRule, bool forceToPrimaryDb = false)
    {
      int ddmFeeRule1 = 0;
      this.onApiCalled(nameof (BpmManager), nameof (CreateDDMFeeRule), new object[2]
      {
        (object) ddmFeeRule,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          ddmFeeRule1 = DDMFeeRuleDbAccessor.CreateDDMFeeRule(ddmFeeRule);
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FeeRuleCreated, DateTime.Now, "", ddmFeeRule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMFeeRules), "", ""));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
      return ddmFeeRule1;
    }

    public virtual int CreateDDMFieldRule(DDMFieldRule ddmFieldRule, bool forceToPrimaryDb = false)
    {
      int ddmFieldRule1 = 0;
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          this.onApiCalled(nameof (BpmManager), nameof (CreateDDMFieldRule), new object[2]
          {
            (object) ddmFieldRule,
            (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
          });
          ddmFieldRule1 = DDMFieldRuleDbAccessor.CreateDDMFieldRule(ddmFieldRule);
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FieldRuleCreated, DateTime.Now, "", ddmFieldRule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMFieldRules), "", ""));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
      return ddmFieldRule1;
    }

    public virtual int UpdateDDMFieldRule(
      DDMFieldRule ddmFieldRule,
      bool statusUpdate = false,
      bool isGlobalSetting = false,
      int activeDeActiveScenarioId = 0)
    {
      int num = 0;
      this.onApiCalled(nameof (BpmManager), nameof (UpdateDDMFieldRule), new object[1]
      {
        (object) ddmFieldRule
      });
      try
      {
        num = DDMFieldRuleDbAccessor.UpdateDDMFieldRule(ddmFieldRule);
        if (statusUpdate | isGlobalSetting)
        {
          string additionalInfo = isGlobalSetting ? "Modified through Global Rule Settings" : string.Empty;
          SystemAuditRecord record;
          if (statusUpdate)
          {
            if (ddmFieldRule.Status == ruleStatus.Active)
            {
              DDMFieldRuleScenario fieldRuleScenario = ddmFieldRule.Scenarios.Where<DDMFieldRuleScenario>((Func<DDMFieldRuleScenario, bool>) (s => s.RuleID == activeDeActiveScenarioId)).FirstOrDefault<DDMFieldRuleScenario>();
              record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FieldRuleActivated, DateTime.Now, "", ddmFieldRule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMFieldRules), fieldRuleScenario != null ? fieldRuleScenario.RuleName : "(No Scenario)", "");
            }
            else
            {
              DDMFieldRuleScenario fieldRuleScenario = ddmFieldRule.Scenarios.Where<DDMFieldRuleScenario>((Func<DDMFieldRuleScenario, bool>) (s => s.RuleID == activeDeActiveScenarioId)).FirstOrDefault<DDMFieldRuleScenario>();
              record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FieldRuleDeactivated, DateTime.Now, "", ddmFieldRule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMFieldRules), fieldRuleScenario != null ? fieldRuleScenario.RuleName : "(No Scenario)", "");
            }
          }
          else
            record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FieldRuleModified, DateTime.Now, "", ddmFieldRule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMFieldRules), "", additionalInfo);
          SystemAuditTrailAccessor.InsertAuditRecord(record);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
      return num;
    }

    public virtual DDMFieldRule[] GetAllDDMFieldRules(bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), "GetAllDDMFieldRule", new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFieldRuleDbAccessor.GetAllDDMFieldRule();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMFieldRule[]) null;
      }
    }

    public virtual DDMFieldRule[] GetAllDDMFieldRulesAndScenarios(
      bool activeOnly,
      List<int> fieldRuleIds = null,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllDDMFieldRulesAndScenarios), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFieldRuleDbAccessor.GetAllDDMFieldRulesAndScenarios(activeOnly, fieldRuleIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMFieldRule[]) null;
      }
    }

    public virtual Dictionary<int, HashSet<string>> GetAllDdmFieldRulesAndFieldsList(
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllDdmFieldRulesAndFieldsList), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFieldRuleDbAccessor.GetAllDdmFieldRulesAndFieldsList();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (Dictionary<int, HashSet<string>>) null;
      }
    }

    public virtual void DeleteDDMFieldRuleByID(int ruleID, bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeleteDDMFieldRuleByID), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          DDMFieldRule ddmFieldRule = DDMFieldRuleDbAccessor.GetDDMFieldRule(ruleID);
          DDMFieldRuleDbAccessor.DeleteDDMFieldRuleByID(ruleID);
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FieldRuleDeleted, DateTime.Now, "", ddmFieldRule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMFieldRules), "", ""));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual bool DDMFieldRuleExist(string ruleName, bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DDMFieldRuleExist), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFieldRuleDbAccessor.DDMFieldRuleExist(ruleName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return false;
      }
    }

    public virtual bool DDMFieldsExistInFieldRules(string fields, bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DDMFieldsExistInFieldRules), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFieldRuleDbAccessor.DDMFieldsExistInFieldRules(fields);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return false;
      }
    }

    public virtual DDMFeeRule[] GetAllDDMFeeRules(bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), "GetAllDDMFeeRule", new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFeeRuleDbAccessor.GetAllDDMFeeRule();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMFeeRule[]) null;
      }
    }

    public virtual DDMFeeRule[] GetAllDDMFeeRulesAndScenarios(
      bool activeOnly,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllDDMFeeRulesAndScenarios), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFeeRuleDbAccessor.GetAllDDMFeeRulesAndScenarios(activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMFeeRule[]) null;
      }
    }

    public virtual DDMFeeRule GetDDMFeeRuleAndScenarioByID(int ruleID, bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDDMFeeRuleAndScenarioByID), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFeeRuleDbAccessor.GetDDMFeeRuleAndScenarioByID(ruleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMFeeRule) null;
      }
    }

    public virtual DDMFeeRule[] GetDDMFeeRuleByDataTable(
      string dataTableName,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDDMFeeRuleByDataTable), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFeeRuleDbAccessor.GetFeeRulesByDataTable(dataTableName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMFeeRule[]) null;
      }
    }

    public virtual DDMFieldRule[] GetDDMFieldRuleByDataTable(
      string dataTableName,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDDMFieldRuleByDataTable), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFieldRuleDbAccessor.GetFieldRulesByDataTable(dataTableName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMFieldRule[]) null;
      }
    }

    public virtual void UpdateDDMFeeRuleByID(
      int ruleID,
      DDMFeeRule feeRule,
      bool statusUpdate = false,
      bool isGlobalSetting = false,
      int activeDeActiveScenarioId = 0)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateDDMFeeRuleByID), Array.Empty<object>());
      try
      {
        DDMFeeRuleDbAccessor.UpdateDDMFeeRuleByID(ruleID, feeRule);
        if (!(statusUpdate | isGlobalSetting))
          return;
        string additionalInfo = isGlobalSetting ? "Modified through Global Rule Settings" : string.Empty;
        SystemAuditRecord record;
        if (statusUpdate)
        {
          if (feeRule.Status == ruleStatus.Active)
          {
            DDMFeeRuleScenario ddmFeeRuleScenario = feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (s => s.RuleID == activeDeActiveScenarioId)).FirstOrDefault<DDMFeeRuleScenario>();
            record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FeeRuleActivated, DateTime.Now, "", feeRule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMFeeRules), ddmFeeRuleScenario != null ? ddmFeeRuleScenario.RuleName : "(No Scenario)", "");
          }
          else
          {
            DDMFeeRuleScenario ddmFeeRuleScenario = feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (s => s.RuleID == activeDeActiveScenarioId)).FirstOrDefault<DDMFeeRuleScenario>();
            record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FeeRuleDeactivated, DateTime.Now, "", feeRule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMFeeRules), ddmFeeRuleScenario != null ? ddmFeeRuleScenario.RuleName : "(No Scenario)", "");
          }
        }
        else
          record = (SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FeeRuleModified, DateTime.Now, "", feeRule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMFeeRules), "", additionalInfo);
        SystemAuditTrailAccessor.InsertAuditRecord(record);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual bool DDMFeeRuleExist(string ruleName, bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DDMFeeRuleExist), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFeeRuleDbAccessor.DDMFeeRuleExist(ruleName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return false;
      }
    }

    public virtual void DeleteDDMFeeRuleByID(int ruleID, bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeleteDDMFeeRuleByID), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          DDMFeeRule ddmFeeRule = DDMFeeRuleDbAccessor.GetDDMFeeRule(ruleID);
          DDMFeeRuleDbAccessor.DeleteDDMFeeRuleByID(ruleID);
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.FeeRuleDeleted, DateTime.Now, "", ddmFeeRule.RuleName, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMFeeRules), "", ""));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual bool DDMFeeLineExist(string feeLine, bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DDMFeeLineExist), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMFeeRuleDbAccessor.DDMFeeLineExist(feeLine);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return false;
      }
    }

    public virtual void UpdateDDMDataPopulationTiming(
      DDMDataPopulationTiming ddmDataPopTiming,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateDDMDataPopulationTiming), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          DDMDataPopulationTimingDbAccessor.UpdateDDMDataPopulationTiming(ddmDataPopTiming);
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.DataPopulationTimingModified, DateTime.Now, "", "Data Population Settings", SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMDataPopTiming), "", ""));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual List<DDMDataPopTimingField> UpdateDDMDataPopulationTimingNumberOfReferences()
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateDDMDataPopulationTimingNumberOfReferences), Array.Empty<object>());
      try
      {
        return DDMDataPopulationTimingDbAccessor.UpdateDDMDataPopulationTimingNumberOfReferences(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (List<DDMDataPopTimingField>) null;
      }
    }

    public virtual int GetNumberReferences(string fieldId, bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetNumberReferences), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataPopulationTimingDbAccessor.GetNumberReferences(fieldId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return 0;
      }
    }

    public virtual DDMDataPopulationTiming GetDDMDataPopulationTiming(bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDDMDataPopulationTiming), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataPopulationTimingDbAccessor.GetDDMDataPopulationTiming();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMDataPopulationTiming) null;
      }
    }

    public virtual bool DDMDataTableExists(string tablename, bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DDMDataTableExists), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.DDMDataTableExists(tablename);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return false;
      }
    }

    public virtual int CreateDDMDataTable(
      DDMDataTable ddmDataTable,
      bool importMode = false,
      bool forceToPrimaryDb = false)
    {
      int ddmDataTable1 = 0;
      this.onApiCalled(nameof (BpmManager), nameof (CreateDDMDataTable), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          ddmDataTable1 = DDMDataTableDbAccessor.CreateDDMDataTable(ddmDataTable);
          string additionalInfo = importMode ? "Data Table created through Import." : string.Empty;
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.DataTableCreated, DateTime.Now, "", ddmDataTable.Name, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMDataTables), "", additionalInfo));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
      return ddmDataTable1;
    }

    public virtual int UpdateDDMDataTable(DDMDataTable ddmDataTable, bool forceToPrimaryDb = false)
    {
      int num = 0;
      this.onApiCalled(nameof (BpmManager), nameof (UpdateDDMDataTable), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          num = DDMDataTableDbAccessor.UpdateDDMDataTable(ddmDataTable);
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.DataTableModified, DateTime.Now, "", ddmDataTable.Name, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMDataTables), "", ""));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
      return num;
    }

    public virtual void DeleteDDMDataTable(DDMDataTable ddmDataTable)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeleteDDMDataTable), Array.Empty<object>());
      try
      {
        DDMDataTableDbAccessor.DeleteDDMDataTable(ddmDataTable);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new DDMAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.DataTableDeleted, DateTime.Now, "", ddmDataTable.Name, SystemAuditTrailEnums.GetAuditObjectType(BizRuleType.DDMDataTables), "", ""));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual bool IsTableUsedByFeeOrFieldRules(DDMDataTable dt, bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (IsTableUsedByFeeOrFieldRules), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.IsTableUsedByFeeOrFieldRules(dt);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return true;
      }
    }

    public virtual void ResetDataTableFeeRuleFieldRuleValue(DDMDataTable dt)
    {
      this.onApiCalled(nameof (BpmManager), "ResetDataTableFieldRuleValue", Array.Empty<object>());
      try
      {
        DDMDataTableDbAccessor.ResetDataTableFeeRuleFieldRuleValue(dt);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual DDMDataTable[] GetAllDDMDataTables(bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllDDMDataTables), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.GetAllDDMDataTables();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMDataTable[]) null;
      }
    }

    public virtual DDMDataTable[] GetAllDDMDataTablesWithFieldValues(bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllDDMDataTablesWithFieldValues), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.GetAllDDMDataTablesAndFieldValues();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMDataTable[]) null;
      }
    }

    public virtual DDMDataTable[] GetAllReferencedDDMDataTablesWithFieldValues(bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllReferencedDDMDataTablesWithFieldValues), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.GetDDMDataTablesAndFieldValuesForDataTableNames(FieldSearchDbAccessor.GetReferencedDataTables(new FieldSearchRuleType[3]
          {
            FieldSearchRuleType.DDMFeeScenarios,
            FieldSearchRuleType.DDMFieldScenarios,
            FieldSearchRuleType.DDMDataTables
          }));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMDataTable[]) null;
      }
    }

    public virtual DDMDataTable[] GetDDMDataTablesAndFieldValuesForDataTableNames(
      List<string> dataTableNames,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDDMDataTablesAndFieldValuesForDataTableNames), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.GetDDMDataTablesAndFieldValuesForDataTableNames(dataTableNames);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMDataTable[]) null;
      }
    }

    public virtual DDMDataTable GetDDMDataTableAndFieldIdsForDataTableName(
      string dataTableName,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDDMDataTableAndFieldIdsForDataTableName), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.GetDDMDataTableAndFieldIdsForDataTableName(dataTableName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMDataTable) null;
      }
    }

    public virtual void AddFieldValuesForDataTable(
      DDMDataTable dataTable,
      Dictionary<string, string> fieldValues,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (AddFieldValuesForDataTable), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          DDMDataTableDbAccessor.AddDDMDataTableFieldValuesForDataTable(dataTable, fieldValues);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual DDMDataTable GetDDMDataTableAndFieldValues(
      int dataTableId,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDDMDataTableAndFieldValues), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.GetDDMDataTableAndFieldValues(dataTableId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMDataTable) null;
      }
    }

    public virtual DDMDataTable GetDDMDataTableAndFieldValuesByName(
      string dataTableName,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDDMDataTableAndFieldValuesByName), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.GetDDMDataTableAndFieldValuesByName(dataTableName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMDataTable) null;
      }
    }

    public virtual int CreateDDMDataTableField(DDMDataTableFieldValue ddmDataTableField)
    {
      this.onApiCalled(nameof (BpmManager), nameof (CreateDDMDataTableField), Array.Empty<object>());
      try
      {
        return DDMDataTableFieldValueDbAccessor.CreateDDMDataTableField(ddmDataTableField);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return 0;
      }
    }

    public virtual int UpdateDDMDataTableField(DDMDataTableFieldValue ddmDataTableField)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateDDMDataTableField), Array.Empty<object>());
      try
      {
        return DDMDataTableFieldValueDbAccessor.UpdateDDMDataTableField(ddmDataTableField);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return 0;
      }
    }

    public virtual void DeleteDDMDataTableField(DDMDataTableFieldValue ddmDataTableField)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeleteDDMDataTableField), Array.Empty<object>());
      try
      {
        DDMDataTableFieldValueDbAccessor.DeleteDDMDataTableField(ddmDataTableField);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual int[] AtomicDataTableChange(
      List<DDMDataTableFieldValue> batchCreationList,
      List<DDMDataTableFieldValue> batchUpdateList,
      List<DDMDataTableFieldValue> batchDeletionList)
    {
      this.onApiCalled(nameof (BpmManager), nameof (AtomicDataTableChange), Array.Empty<object>());
      try
      {
        return DDMDataTableFieldValueDbAccessor.AtomicDataTableChange(batchCreationList, batchUpdateList, batchDeletionList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (int[]) null;
      }
    }

    public virtual List<DDMDataTableReference> GetDDMDataTableReferences(
      string dataTableName,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDDMDataTableReferences), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.GetDDMDataTableReferences(dataTableName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (List<DDMDataTableReference>) null;
      }
    }

    public virtual void SetMsWorksheetInfo(WorksheetInfo worksheetInfo)
    {
      this.onApiCalled(nameof (BpmManager), nameof (SetMsWorksheetInfo), new object[1]
      {
        (object) worksheetInfo
      });
      try
      {
        WorkflowBpmDbAccessor.SetMsWorksheetInfo(worksheetInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void UpdateMsWorksheetAlertMessages(Dictionary<string, string> alertMsgsToUpdate)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateMsWorksheetAlertMessages), Array.Empty<object>());
      try
      {
        WorkflowBpmDbAccessor.UpdateMsWorksheetAlertMessages(alertMsgsToUpdate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void SetOrUpdateMsWorksheetAlertMessages(
      Dictionary<WorksheetInfo, string> alertMsgsToUpdate)
    {
      this.onApiCalled(nameof (BpmManager), "UpdateMsWorksheetAlertMessages", Array.Empty<object>());
      try
      {
        WorkflowBpmDbAccessor.SetOrUpdateMsWorksheetAlertMessages(alertMsgsToUpdate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual WorksheetInfo[] GetMsWorksheetInfos()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMsWorksheetInfos), Array.Empty<object>());
      try
      {
        return WorkflowBpmDbAccessor.GetAllMsWorksheetInfos();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (WorksheetInfo[]) null;
      }
    }

    public virtual WorksheetInfo GetMsWorksheetInfo(int coreMilestoneID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMsWorksheetInfo), new object[1]
      {
        (object) coreMilestoneID
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMsWorksheetInfo(string.Concat((object) coreMilestoneID));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (WorksheetInfo) null;
      }
    }

    public virtual WorksheetInfo GetMsWorksheetInfo(string milestoneID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMsWorksheetInfo), new object[1]
      {
        (object) milestoneID
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMsWorksheetInfo(milestoneID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (WorksheetInfo) null;
      }
    }

    public virtual IEnumerable<EllieMae.EMLite.Workflow.Milestone> GetMilestones(bool activeOnly)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestones), new object[2]
      {
        (object) activeOnly,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return (IEnumerable<EllieMae.EMLite.Workflow.Milestone>) WorkflowBpmDbAccessor.GetMilestones(activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (IEnumerable<EllieMae.EMLite.Workflow.Milestone>) null;
      }
    }

    public virtual void UpdateMilestoneCache()
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateMilestoneCache), Array.Empty<object>());
      try
      {
        WorkflowBpmDbAccessor.UpdateMilestoneCache();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual EllieMae.EMLite.Workflow.Milestone GetMilestone(string milestoneId)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestone), new object[1]
      {
        (object) milestoneId
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMilestone(milestoneId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (EllieMae.EMLite.Workflow.Milestone) null;
      }
    }

    public virtual EllieMae.EMLite.Workflow.Milestone GetMilestoneByName(string milestoneName)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestoneByName), new object[1]
      {
        (object) milestoneName
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMilestoneByName(milestoneName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (EllieMae.EMLite.Workflow.Milestone) null;
      }
    }

    public virtual void CreateMilestone(EllieMae.EMLite.Workflow.Milestone ms)
    {
      this.onApiCalled(nameof (BpmManager), nameof (CreateMilestone), new object[1]
      {
        (object) ms
      });
      try
      {
        WorkflowBpmDbAccessor.CreateMilestone(ms);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void UpdateMilestone(EllieMae.EMLite.Workflow.Milestone ms)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateMilestone), new object[1]
      {
        (object) ms
      });
      try
      {
        WorkflowBpmDbAccessor.UpdateMilestone(ms);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void SetMilestoneArchiveFlag(string milestoneId, bool archived)
    {
      this.onApiCalled(nameof (BpmManager), nameof (SetMilestoneArchiveFlag), new object[2]
      {
        (object) milestoneId,
        (object) archived
      });
      try
      {
        WorkflowBpmDbAccessor.SetMilestoneArchiveFlag(milestoneId, archived);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void DeleteMilestone(string milestoneId)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeleteMilestone), new object[1]
      {
        (object) milestoneId
      });
      try
      {
        WorkflowBpmDbAccessor.DeleteMilestone(milestoneId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void SetMilestoneOrder(string[] milestoneIds)
    {
      this.onApiCalled(nameof (BpmManager), nameof (SetMilestoneOrder), (object[]) milestoneIds);
      try
      {
        WorkflowBpmDbAccessor.SetMilestoneOrder(milestoneIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void ChangeMilestoneSortIndex(string milestoneId, int sortIndex)
    {
      this.onApiCalled(nameof (BpmManager), nameof (ChangeMilestoneSortIndex), new object[2]
      {
        (object) milestoneId,
        (object) sortIndex
      });
      try
      {
        WorkflowBpmDbAccessor.ChangeMilestoneSortIndex(milestoneId, sortIndex);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void ChangeMilestoneSortIndex(EllieMae.EMLite.Workflow.Milestone OldMilestone, EllieMae.EMLite.Workflow.Milestone NewMilestone)
    {
      this.onApiCalled(nameof (BpmManager), nameof (ChangeMilestoneSortIndex), new object[2]
      {
        (object) OldMilestone,
        (object) NewMilestone
      });
      try
      {
        WorkflowBpmDbAccessor.ChangeMilestoneSortIndex(OldMilestone, NewMilestone);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual IEnumerable<MilestoneTemplate> GetMilestoneTemplates(bool activeOnly)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestoneTemplates), new object[1]
      {
        (object) activeOnly
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMilestoneTemplates(activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (IEnumerable<MilestoneTemplate>) null;
      }
    }

    public virtual string GetMilestoneTemplatebyMilestoneID(string milestoneId)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestoneTemplatebyMilestoneID), new object[1]
      {
        (object) milestoneId
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMilestoneTemplatebyMilestoneID(milestoneId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return "";
      }
    }

    public virtual MilestoneTemplate GetMilestoneTemplate(string templateId)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestoneTemplate), new object[1]
      {
        (object) templateId
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMilestoneTemplate(templateId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (MilestoneTemplate) null;
      }
    }

    public virtual MilestoneTemplate GetMilestoneTemplateByGuid(string templateGuid)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestoneTemplateByGuid), new object[1]
      {
        (object) templateGuid
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMilestoneTemplateByGuid(templateGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (MilestoneTemplate) null;
      }
    }

    public virtual List<FieldRuleInfo> GetMilestoneTemplateConditions()
    {
      this.onApiCalled(nameof (BpmManager), "GetMilestoneTemplate", new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMilestoneTemplate();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (List<FieldRuleInfo>) null;
      }
    }

    public virtual MilestoneTemplate GetMilestoneTemplateByName(string templateName)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestoneTemplateByName), new object[1]
      {
        (object) templateName
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMilestoneTemplateByName(templateName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (MilestoneTemplate) null;
      }
    }

    public virtual MilestoneTemplate GetDefaultMilestoneTemplate()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDefaultMilestoneTemplate), Array.Empty<object>());
      try
      {
        return WorkflowBpmDbAccessor.GetDefaultMilestoneTemplate();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (MilestoneTemplate) null;
      }
    }

    public virtual void CreateMilestoneTemplate(MilestoneTemplate template, BizRuleInfo rule)
    {
      this.onApiCalled(nameof (BpmManager), nameof (CreateMilestoneTemplate), new object[2]
      {
        (object) template,
        (object) rule
      });
      try
      {
        WorkflowBpmDbAccessor.CreateMilestoneTemplate(template, rule);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual IEnumerable<MilestoneTemplate> UpdateMilestoneTemplateImpactedAreaSettings(
      Dictionary<MilestoneTemplate, string> newSettings,
      string impactedArea)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateMilestoneTemplateImpactedAreaSettings), new object[2]
      {
        (object) newSettings,
        (object) impactedArea
      });
      try
      {
        WorkflowBpmDbAccessor.UpdateMilestoneTemplateImpactedAreaSettings(newSettings, impactedArea);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
      return this.GetMilestoneTemplates(true);
    }

    public virtual void UpdateMilestoneTemplateEDisclosureExceptions(
      EDisclosureSetup eDisclosureSetup)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateMilestoneTemplateEDisclosureExceptions), new object[1]
      {
        (object) eDisclosureSetup
      });
      try
      {
        WorkflowBpmDbAccessor.UpdateMilestoneTemplateEDisclosureExceptions(eDisclosureSetup);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void UpdateMilestoneTemplateEDisclosureExceptions(
      Dictionary<string, Dictionary<MilestoneTemplate, string>> exceptionsList,
      bool remove)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateMilestoneTemplateEDisclosureExceptions), new object[2]
      {
        (object) exceptionsList,
        (object) remove
      });
      try
      {
        WorkflowBpmDbAccessor.UpdateMilestoneTemplateEDisclosureExceptions(exceptionsList, remove);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void RemoveMilestoneTemplateEDisclosureExceptions(
      Dictionary<string, List<string>> exceptionsList)
    {
      this.onApiCalled(nameof (BpmManager), nameof (RemoveMilestoneTemplateEDisclosureExceptions), new object[1]
      {
        (object) exceptionsList
      });
      try
      {
        WorkflowBpmDbAccessor.RemoveMilestoneTemplateEDisclosureExceptions(exceptionsList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual Dictionary<string, List<string>> GetAllMilestoneTemplateEDisclosureExceptions()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllMilestoneTemplateEDisclosureExceptions), Array.Empty<object>());
      try
      {
        return WorkflowBpmDbAccessor.GetAllMilestoneTemplateEDisclosureExceptions();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (Dictionary<string, List<string>>) null;
      }
    }

    public virtual void UpdateMilestoneTemplate(MilestoneTemplate template, BizRuleInfo rule)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateMilestoneTemplate), new object[2]
      {
        (object) template,
        (object) rule
      });
      try
      {
        WorkflowBpmDbAccessor.UpdateMilestoneTemplate(template, rule);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void SetMilestoneTemplateActiveFlag(string templateId, bool active)
    {
      this.onApiCalled(nameof (BpmManager), nameof (SetMilestoneTemplateActiveFlag), new object[2]
      {
        (object) templateId,
        (object) active
      });
      try
      {
        WorkflowBpmDbAccessor.SetMilestoneTemplateActiveFlag(templateId, active);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void DeleteMilestoneTemplate(string templateId)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeleteMilestoneTemplate), new object[1]
      {
        (object) templateId
      });
      try
      {
        WorkflowBpmDbAccessor.DeleteMilestoneTemplate(templateId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void SetMilestoneTemplateOrder(string[] templateIds)
    {
      this.onApiCalled(nameof (BpmManager), nameof (SetMilestoneTemplateOrder), (object[]) templateIds);
      try
      {
        WorkflowBpmDbAccessor.SetMilestoneTemplateOrder(templateIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void ChangeMilestoneTemplateSortIndex(string templateId, int sortIndex)
    {
      this.onApiCalled(nameof (BpmManager), nameof (ChangeMilestoneTemplateSortIndex), new object[2]
      {
        (object) templateId,
        (object) sortIndex
      });
      try
      {
        WorkflowBpmDbAccessor.ChangeMilestoneTemplateSortIndex(templateId, sortIndex);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void ChangeMilestoneTemplateSortIndex(
      MilestoneTemplate OldTemplate,
      MilestoneTemplate NewTemplate)
    {
      this.onApiCalled(nameof (BpmManager), nameof (ChangeMilestoneTemplateSortIndex), new object[2]
      {
        (object) OldTemplate,
        (object) NewTemplate
      });
      try
      {
        WorkflowBpmDbAccessor.ChangeMilestoneTemplateSortIndex(OldTemplate, NewTemplate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual FieldRuleInfo GetMilestoneTemplateConditions(string templateId)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestoneTemplateConditions), new object[1]
      {
        (object) templateId
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMilestoneTemplateConditions(templateId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (FieldRuleInfo) null;
      }
    }

    public virtual Hashtable GetMilestoneTemplateDefaultSettings()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestoneTemplateDefaultSettings), Array.Empty<object>());
      try
      {
        return WorkflowBpmDbAccessor.GetMilestoneTemplateDefaultSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (Hashtable) null;
      }
    }

    public virtual string[] GetMilestoneIDsByRoleID(int roleID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestoneIDsByRoleID), new object[1]
      {
        (object) roleID
      });
      try
      {
        return WorkflowBpmDbAccessor.GetMilestoneIDsByRoleID(roleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (string[]) null;
      }
    }

    public virtual RoleInfo[] GetAllRoleFunctions()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllRoleFunctions), Array.Empty<object>());
      try
      {
        return WorkflowBpmDbAccessor.GetAllRoleFunctions();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (RoleInfo[]) null;
      }
    }

    public virtual RoleInfo GetRoleFunction(int roleID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRoleFunction), new object[1]
      {
        (object) roleID
      });
      try
      {
        return WorkflowBpmDbAccessor.GetRoleFunction(roleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (RoleInfo) null;
      }
    }

    public virtual RoleInfo[] GetRoleFunctionsByUserID(string userID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRoleFunctionsByUserID), new object[1]
      {
        (object) userID
      });
      try
      {
        return WorkflowBpmDbAccessor.GetRoleFunctionsByUserID(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (RoleInfo[]) null;
      }
    }

    public virtual RoleSummaryInfo[] GetUserEligibleRoles(string userID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetUserEligibleRoles), new object[1]
      {
        (object) userID
      });
      try
      {
        return WorkflowBpmDbAccessor.GetUserEligibleRoles(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (RoleSummaryInfo[]) null;
      }
    }

    public virtual RoleInfo[] GetAllRoleFunctionsByPersonaID(int personaID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllRoleFunctionsByPersonaID), new object[1]
      {
        (object) personaID
      });
      try
      {
        return WorkflowBpmDbAccessor.GetRoleFunctionsByPersonaID(personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (RoleInfo[]) null;
      }
    }

    public virtual int SetRoleFunction(RoleInfo roleInfo)
    {
      this.onApiCalled(nameof (BpmManager), nameof (SetRoleFunction), new object[1]
      {
        (object) roleInfo
      });
      try
      {
        int num = WorkflowBpmDbAccessor.SetRoleFunction((RoleSummaryInfo) roleInfo);
        string ruleName = "Protected Document";
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new BizRuleAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.BusinessRuleModified, DateTime.Now, roleInfo.Name, ruleName, AuditObjectType.BizRule_RoleProtectedDocument));
        return num;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return -1;
      }
    }

    public virtual void DeleteRoleFunction(int roleID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeleteRoleFunction), new object[1]
      {
        (object) roleID
      });
      try
      {
        WorkflowBpmDbAccessor.DeleteRoleFunction(roleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual RolesMappingInfo[] GetAllRoleMappingInfos()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllRoleMappingInfos), Array.Empty<object>());
      try
      {
        return WorkflowBpmDbAccessor.GetAllRoleMappingInfos();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (RolesMappingInfo[]) null;
      }
    }

    public virtual RolesMappingInfo GetRoleMappingInfo(RealWorldRoleID realWorldRoleID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRoleMappingInfo), new object[1]
      {
        (object) realWorldRoleID
      });
      try
      {
        return WorkflowBpmDbAccessor.GetRoleMappingInfo(realWorldRoleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (RolesMappingInfo) null;
      }
    }

    public virtual void UpdateRoleMappingInfos(RolesMappingInfo[] rolesMappingInfos)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateRoleMappingInfos), (object[]) rolesMappingInfos);
      try
      {
        WorkflowBpmDbAccessor.UpdateRoleMappingInfos(rolesMappingInfos);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual RolesMappingInfo[] GetUsersRoleMapping(string userID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetUsersRoleMapping), new object[1]
      {
        (object) userID
      });
      try
      {
        return WorkflowBpmDbAccessor.GetUsersRoleMapping(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (RolesMappingInfo[]) null;
      }
    }

    public virtual RolesMappingInfo GetUsersRoleMapping(
      string userID,
      RealWorldRoleID realWorldRoleID)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetUsersRoleMapping), new object[2]
      {
        (object) userID,
        (object) realWorldRoleID
      });
      try
      {
        return WorkflowBpmDbAccessor.GetUsersRoleMapping(userID, realWorldRoleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (RolesMappingInfo) null;
      }
    }

    public virtual Hashtable GetAllMilestoneAlertMessages()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllMilestoneAlertMessages), Array.Empty<object>());
      try
      {
        return WorkflowBpmDbAccessor.GetAllMilestoneAlertMessages();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (Hashtable) null;
      }
    }

    private void updateLastModifiedInfo(BizRuleInfo rule)
    {
      rule.LastModifiedByUserId = this.Session.UserID;
      rule.LastModifiedByFullName = this.Session.GetUserInfo().FullName;
    }

    public virtual void SaveDataTableExportLog(DDMDataTableExportLog dataTableExportLog)
    {
      this.onApiCalled(nameof (BpmManager), nameof (SaveDataTableExportLog), Array.Empty<object>());
      try
      {
        DDMDataTableDbAccessor.SaveDataTableExportLog(dataTableExportLog);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual DDMDataTableExportLog GetDataTableExportLog(
      string dataTableExportLogID,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetDataTableExportLog), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      try
      {
        using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
          return DDMDataTableDbAccessor.GetDataTableExportLog(dataTableExportLogID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (DDMDataTableExportLog) null;
      }
    }

    public virtual List<TemporaryBuydown> GetAllTemporaryBuydowns()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetAllTemporaryBuydowns), Array.Empty<object>());
      try
      {
        return TemporaryBuydownTypeBpmDbAccessor.GetAllTemporaryBuydowns();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (List<TemporaryBuydown>) null;
      }
    }

    public virtual void CreateTemporaryBuydownType(TemporaryBuydown buydown)
    {
      this.onApiCalled(nameof (BpmManager), nameof (CreateTemporaryBuydownType), Array.Empty<object>());
      try
      {
        TemporaryBuydownTypeBpmDbAccessor.CreateTemporaryBuydownType(buydown);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void UpdateTemporaryBuydownType(TemporaryBuydown buydown)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateTemporaryBuydownType), Array.Empty<object>());
      try
      {
        TemporaryBuydownTypeBpmDbAccessor.UpdateTemporaryBuydownType(buydown);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void DeleteTemporaryBuydownType(TemporaryBuydown buydown)
    {
      this.onApiCalled(nameof (BpmManager), nameof (DeleteTemporaryBuydownType), Array.Empty<object>());
      try
      {
        TemporaryBuydownTypeBpmDbAccessor.DeleteTemporaryBuydownType(buydown);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
      }
    }

    public virtual void CreateFieldSearchInfo(BizRuleInfo[] bizRuleInfos)
    {
      foreach (BizRuleInfo bizRuleInfo in bizRuleInfos)
        this.CreateFieldSearchInfo(bizRuleInfo);
    }

    public virtual List<int> UpdateFieldSearchInfo(BizRuleInfo[] bizRuleInfos)
    {
      List<int> intList = new List<int>();
      foreach (BizRuleInfo bizRuleInfo in bizRuleInfos)
      {
        int num = this.UpdateFieldSearchInfo(bizRuleInfo);
        if (num > 0)
          intList.Add(num);
      }
      return intList;
    }

    public virtual void DeleteFieldSearchInfo(BizRuleInfo[] bizRuleInfos)
    {
      foreach (BizRuleInfo bizRuleInfo in bizRuleInfos)
        this.DeleteFieldSearchInfo(bizRuleInfo);
    }

    public virtual List<int> UpdateFieldSearchInfo(
      List<FieldSearchRuleType> fsRuleTypes,
      bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (BpmManager), nameof (UpdateFieldSearchInfo), new object[2]
      {
        null,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.DDM, forceToPrimaryDb)
      });
      using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
      {
        List<FieldSearchRuleType> fieldSearchRuleTypeList = new List<FieldSearchRuleType>();
        List<BizRuleType> source = new List<BizRuleType>();
        foreach (FieldSearchRuleType fsRuleType in fsRuleTypes)
        {
          BizRuleType result = BizRuleType.None;
          if (Enum.TryParse<BizRuleType>(fsRuleType.ToString(), true, out result) && result != BizRuleType.None)
            source.Add(result);
          else
            fieldSearchRuleTypeList.Add(fsRuleType);
        }
        List<int> FsRuleIds = this.UpdateFieldSearchInfo(this.GetRules(source.Where<BizRuleType>((Func<BizRuleType, bool>) (ruleType => ruleType != BizRuleType.DDMDataTables)).ToArray<BizRuleType>()));
        if (source.Where<BizRuleType>((Func<BizRuleType, bool>) (ruleType => ruleType == BizRuleType.DDMDataTables)).Count<BizRuleType>() > 0)
          FsRuleIds.AddRange((IEnumerable<int>) DDMDataTableDbAccessor.UpdateAllDataTableFieldSearchInfo());
        if (fieldSearchRuleTypeList.Count > 0)
        {
          foreach (FieldSearchRuleType fieldSearchRuleType in fieldSearchRuleTypeList)
          {
            switch (fieldSearchRuleType)
            {
              case FieldSearchRuleType.PiggyBackingFields:
                BinaryObject systemSettings = SystemConfiguration.GetSystemSettings("PiggybackFields");
                if (systemSettings != null)
                {
                  PiggybackFields ruleFields = systemSettings.ToObject<PiggybackFields>();
                  if (ruleFields != null && ruleFields.Count > 0)
                  {
                    FieldSearchRule fieldSearchRule = new FieldSearchRule(ruleFields);
                    FsRuleIds.Add(FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule));
                    continue;
                  }
                  FieldSearchDbAccessor.DeleteFieldSearchInfo(0, FieldSearchRuleType.PiggyBackingFields);
                  continue;
                }
                continue;
              case FieldSearchRuleType.LoanCustomFields:
                FieldSearchDbAccessor.UpdateLoanCustomFieldsFieldSearch(SystemConfiguration.GetLoanCustomFields());
                fsRuleTypes.Remove(FieldSearchRuleType.LoanCustomFields);
                continue;
              case FieldSearchRuleType.BorrowerCustomFields:
                FieldSearchRule fieldSearchRule1 = new FieldSearchRule(BorrowerCustomFields.Get(), FieldSearchRuleType.BorrowerCustomFields);
                if (fieldSearchRule1.FieldSearchFields.Count > 0)
                {
                  FsRuleIds.Add(FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule1));
                  continue;
                }
                FieldSearchDbAccessor.DeleteFieldSearchInfo(0, FieldSearchRuleType.BorrowerCustomFields);
                continue;
              case FieldSearchRuleType.BusinessCustomFields:
                FieldSearchRule fieldSearchRule2 = new FieldSearchRule(BizPartnerCustomFields.Get(), FieldSearchRuleType.BusinessCustomFields);
                string identifier1 = "Page Fields";
                int parentFsRuleId1 = FieldSearchDbAccessor.FindParentFSRuleId(0, FieldSearchRuleType.BusinessCustomFields, identifier1);
                if (parentFsRuleId1 != 0)
                  fieldSearchRule2.ParentFSRuleId = new int?(parentFsRuleId1);
                fieldSearchRule2.Identifier = identifier1;
                if (fieldSearchRule2.FieldSearchFields.Count > 0)
                  FsRuleIds.Add(FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule2));
                else
                  FieldSearchDbAccessor.DeleteFieldSearchInfoWhithDataCheck(0, FieldSearchRuleType.BusinessCustomFields, identifier1);
                CustomFieldsDefinitionInfo[] fieldsDefinitions = BizPartnerContact.GetCustomFieldsDefinitions(CustomFieldsType.BizCategoryCustom);
                if (fieldsDefinitions != null)
                {
                  foreach (CustomFieldsDefinitionInfo fields in fieldsDefinitions)
                  {
                    FieldSearchRule fieldSearchRule3 = new FieldSearchRule(fields);
                    if (fields.CustomFieldDefinitions != null && fields.CustomFieldDefinitions.Length != 0)
                    {
                      string identifier2 = "Category:" + fields.CustomFieldDefinitions[0].CategoryId.ToString();
                      int parentFsRuleId2 = FieldSearchDbAccessor.FindParentFSRuleId(0, FieldSearchRuleType.BusinessCustomFields, identifier2);
                      if (parentFsRuleId2 != 0)
                        fieldSearchRule3.ParentFSRuleId = new int?(parentFsRuleId2);
                      fieldSearchRule3.Identifier = identifier2;
                      if (fieldSearchRule3.FieldSearchFields.Count > 0)
                        FsRuleIds.Add(FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule3));
                      else
                        FieldSearchDbAccessor.DeleteFieldSearchInfoWhithDataCheck(0, FieldSearchRuleType.BusinessCustomFields, identifier2);
                    }
                  }
                  continue;
                }
                continue;
              case FieldSearchRuleType.Alerts:
                foreach (AlertConfig alertConfig in AlertConfigAccessor.GetAlertConfigList())
                {
                  FieldSearchRule fieldSearchRule4 = new FieldSearchRule(alertConfig, alertConfig.AlertID);
                  FsRuleIds.Add(FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule4));
                }
                continue;
              case FieldSearchRuleType.LockRequestAdditionalFields:
                FieldSearchRule fieldSearchRule5 = new FieldSearchRule(SecondaryRegistrationAccessor.GetLRAdditionalFields());
                if (fieldSearchRule5.FieldSearchFields.Count > 0)
                {
                  FsRuleIds.Add(FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule5));
                  continue;
                }
                FieldSearchDbAccessor.DeleteFieldSearchInfo(0, FieldSearchRuleType.LockRequestAdditionalFields);
                continue;
              case FieldSearchRuleType.HtmlEmailTemplate:
                foreach (HtmlEmailTemplateType emailTemplateType in HtmlEmailTemplate.GetValidEmailTemplateTypes())
                {
                  foreach (HtmlEmailTemplate htmlEmailTemplate in SystemConfigurationAccessor.GetHtmlEmailTemplates((string) null, emailTemplateType))
                  {
                    FieldSearchRule fieldSearchRule6 = new FieldSearchRule(htmlEmailTemplate);
                    FsRuleIds.Add(FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule6));
                  }
                }
                continue;
              case FieldSearchRuleType.CompanyStatusOnline:
                StatusOnlineSetup setup = StatusOnlineStore.GetSetup((string) null);
                string[] identifiersToKeep = new string[setup.Triggers.Count];
                for (int index = 0; index < setup.Triggers.Count; ++index)
                {
                  StatusOnlineTrigger trigger = setup.Triggers[index];
                  FieldSearchRule fieldSearchRule7 = new FieldSearchRule(trigger);
                  if (fieldSearchRule7.FieldSearchFields.Count > 0)
                  {
                    FsRuleIds.Add(FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule7));
                    identifiersToKeep[index] = trigger.Guid;
                  }
                }
                FieldSearchDbAccessor.DeleteFieldSearchInfo(identifiersToKeep, FieldSearchRuleType.CompanyStatusOnline);
                continue;
              case FieldSearchRuleType.TPOCustomFields:
                FieldSearchRule fieldSearchRule8 = new FieldSearchRule(ExternalOrgManagementAccessor.GetCustomFieldInfo(), FieldSearchRuleType.TPOCustomFields);
                if (fieldSearchRule8.FieldSearchFields.Count > 0)
                {
                  FsRuleIds.Add(FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule8));
                  continue;
                }
                FieldSearchDbAccessor.DeleteFieldSearchInfo(0, FieldSearchRuleType.TPOCustomFields);
                continue;
              default:
                continue;
            }
          }
        }
        FieldSearchDbAccessor.PurgeDangling(FsRuleIds, fsRuleTypes);
        return FsRuleIds;
      }
    }

    public virtual void CreateFieldSearchInfo()
    {
      this.onApiCalled(nameof (BpmManager), nameof (CreateFieldSearchInfo), (object[]) null);
      FieldSearchDbAccessor.DeleteFieldSearchInfo();
      this.CreateFieldSearchInfo(this.GetAllRules());
    }

    public virtual List<int> UpdateFieldSearchInfo(bool forceToPrimaryDb = false)
    {
      using (this.Session.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
      {
        List<FieldSearchRuleType> fsRuleTypes = new List<FieldSearchRuleType>();
        foreach (SearchableRuleType searchableRuleType in this.GetSearchableRuleTypes())
        {
          if (searchableRuleType.Type != FieldSearchRuleType.None)
            fsRuleTypes.Add(searchableRuleType.Type);
        }
        return this.UpdateFieldSearchInfo(fsRuleTypes, false);
      }
    }

    public virtual void DeleteFieldSearchInfo() => FieldSearchDbAccessor.DeleteFieldSearchInfo();

    private void CreateFieldSearchInfo(BizRuleInfo bizRuleInfo)
    {
      if (!(bizRuleInfo is IFieldSearchable))
        return;
      try
      {
        FieldSearchDbAccessor.CreateFieldSearchInfo(new FieldSearchRule(bizRuleInfo));
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteException(TraceLevel.Error, nameof (BpmManager), ex);
      }
    }

    private int UpdateFieldSearchInfo(BizRuleInfo bizRuleInfo)
    {
      if (!(bizRuleInfo is IFieldSearchable))
        return 0;
      try
      {
        return FieldSearchDbAccessor.UpdateFieldSearchInfo(new FieldSearchRule(bizRuleInfo));
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteException(TraceLevel.Error, nameof (BpmManager), ex);
      }
      return 0;
    }

    private void ChagneFieldSearchRuleStatus(BizRuleInfo bizRuleInfo, BizRule.RuleStatus ruleStatus)
    {
      if (!(bizRuleInfo is IFieldSearchable))
        return;
      try
      {
        FieldSearchDbAccessor.ChangeRuleStatus(bizRuleInfo, ruleStatus);
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteException(TraceLevel.Error, nameof (BpmManager), ex);
      }
    }

    private void DeleteFieldSearchInfo(BizRuleInfo bizRuleInfo)
    {
      if (!(bizRuleInfo is IFieldSearchable))
        return;
      try
      {
        FieldSearchRuleType result;
        if (!Enum.TryParse<FieldSearchRuleType>(bizRuleInfo.RuleType.ToString(), true, out result))
          result = FieldSearchRuleType.None;
        FieldSearchDbAccessor.DeleteFieldSearchInfo(bizRuleInfo.RuleID, result);
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteException(TraceLevel.Error, nameof (BpmManager), ex);
      }
    }

    public virtual List<SearchableRuleType> GetSearchableRuleTypes()
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetSearchableRuleTypes), Array.Empty<object>());
      try
      {
        return FieldSearchUtil.AllSearchableRuleTypes;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (List<SearchableRuleType>) null;
      }
    }

    public virtual FieldSearchRuleType GetRuleType(int fsRuleId)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRuleType), Array.Empty<object>());
      try
      {
        return FieldSearchDbAccessor.GetRuleType(fsRuleId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return FieldSearchRuleType.None;
      }
    }

    public virtual int GetRuleId(int fsRuleId)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRuleId), Array.Empty<object>());
      try
      {
        return FieldSearchDbAccessor.GetRuleId(fsRuleId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return -1;
      }
    }

    public virtual string GetRuleIdentifier(int fsRuleId)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetRuleIdentifier), Array.Empty<object>());
      try
      {
        return FieldSearchDbAccessor.GetRuleIdentifier(fsRuleId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (string) null;
      }
    }

    public virtual string[] GetMilestoneNamesByRuleIds(int[] ruleIDs)
    {
      this.onApiCalled(nameof (BpmManager), nameof (GetMilestoneNamesByRuleIds), Array.Empty<object>());
      try
      {
        return AutomatedConditionBpmDbAccessor.GetMilestoneNamesByRuleIds(ruleIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BpmManager), ex);
        return (string[]) null;
      }
    }
  }
}

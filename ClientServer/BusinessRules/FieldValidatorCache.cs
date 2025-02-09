// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.FieldValidatorCache
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class FieldValidatorCache
  {
    private const string className = "FieldValidatorCache�";
    private static readonly string sw = Tracing.SwDataEngine;
    private static Hashtable validators = new Hashtable();
    private static object syncRoot = new object();

    private FieldValidatorCache()
    {
    }

    public static FieldRuleValidators GetFieldValidators(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo)
    {
      string key = sessionObjects.StartupInfo.ServerInstanceName + "@" + sessionObjects.StartupInfo.ServerID;
      FieldValidatorCache.ServerValidators serverValidators = (FieldValidatorCache.ServerValidators) null;
      lock (FieldValidatorCache.syncRoot)
      {
        if (!FieldValidatorCache.validators.Contains((object) key))
          FieldValidatorCache.validators[(object) key] = (object) new FieldValidatorCache.ServerValidators();
        serverValidators = (FieldValidatorCache.ServerValidators) FieldValidatorCache.validators[(object) key];
      }
      return serverValidators.GetFieldValidators(sessionObjects, configInfo);
    }

    public static DateTime GetRulesModificationTimestamp(SessionObjects sessionObjects)
    {
      string key = sessionObjects.StartupInfo.ServerInstanceName + "@" + sessionObjects.StartupInfo.ServerID;
      FieldValidatorCache.ServerValidators serverValidators = (FieldValidatorCache.ServerValidators) null;
      lock (FieldValidatorCache.syncRoot)
      {
        if (!FieldValidatorCache.validators.Contains((object) key))
          return DateTime.MinValue;
        serverValidators = (FieldValidatorCache.ServerValidators) FieldValidatorCache.validators[(object) key];
      }
      return serverValidators.LastModificationTime;
    }

    private class ServerValidators
    {
      private FieldRuleValidators vals;
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

      public FieldRuleValidators GetFieldValidators(
        SessionObjects sessionObjects,
        ILoanConfigurationInfo configInfo)
      {
        lock (this.syncRoot)
        {
          if (sessionObjects.UserInfo.IsSuperAdministrator())
          {
            this.vals = new FieldRuleValidators(new FieldRule[0]);
            this.lastModified = DateTime.MaxValue;
          }
          else if (this.vals == null || configInfo.FieldRulesModificationTime > this.lastModified)
          {
            this.vals = new FieldRuleValidators(this.generateFieldRules(configInfo), !sessionObjects.Interactive);
            this.lastModified = configInfo.FieldRulesModificationTime;
          }
          return this.vals;
        }
      }

      private FieldRule[] generateFieldRules(ILoanConfigurationInfo configInfo)
      {
        ArrayList arrayList = new ArrayList();
        arrayList.AddRange((ICollection) this.generateFieldRules(configInfo, (BizRuleInfo[]) configInfo.FieldRules));
        return (FieldRule[]) arrayList.ToArray(typeof (FieldRule));
      }

      private ArrayList generateFieldRules(
        ILoanConfigurationInfo configInfo,
        BizRuleInfo[] bizRules)
      {
        ArrayList fieldRules = new ArrayList();
        foreach (FieldRuleInfo bizRule in bizRules)
        {
          if (!bizRule.Inactive)
            fieldRules.AddRange((ICollection) this.generateFieldRules(configInfo, bizRule));
        }
        return fieldRules;
      }

      private ArrayList generateFieldRules(ILoanConfigurationInfo configInfo, FieldRuleInfo bizRule)
      {
        ArrayList fieldRules = new ArrayList();
        Hashtable requiredFields = bizRule.RequiredFields;
        RuleCondition ruleCondition = BizRuleTranslator.GetRuleCondition((BizRuleInfo) bizRule);
        if (ruleCondition == null)
          return fieldRules;
        foreach (DictionaryEntry fieldRule in bizRule.FieldRules)
        {
          string[] prereqFields = (string[]) requiredFields[fieldRule.Key];
          try
          {
            string description = bizRule.RuleName + " - Field " + fieldRule.Key.ToString();
            if (fieldRule.Value is FRRange)
              fieldRules.Add((object) this.createRangeRule(configInfo, description, fieldRule.Key.ToString(), ruleCondition, (FRRange) fieldRule.Value, prereqFields));
            else if (fieldRule.Value is FRList)
              fieldRules.Add((object) this.createListRule(configInfo, description, fieldRule.Key.ToString(), ruleCondition, (FRList) fieldRule.Value, prereqFields));
            else if (fieldRule.Value is string)
              fieldRules.Add((object) this.createCustomRule(configInfo, description, fieldRule.Key.ToString(), ruleCondition, (string) fieldRule.Value, prereqFields));
          }
          catch (Exception ex)
          {
            Tracing.Log(FieldValidatorCache.sw, nameof (FieldValidatorCache), TraceLevel.Error, "Error creating field rule for field " + fieldRule.Key.ToString() + ": " + (object) ex);
          }
        }
        return fieldRules;
      }

      private ValueRangeFieldRule createRangeRule(
        ILoanConfigurationInfo configInfo,
        string description,
        string fieldId,
        RuleCondition condition,
        FRRange range,
        string[] prereqFields)
      {
        return new ValueRangeFieldRule(description, fieldId, condition, (object) range.LowerBound, (object) range.UpperBound, prereqFields, this.getFieldFormat(configInfo, fieldId));
      }

      private ValueListFieldRule createListRule(
        ILoanConfigurationInfo configInfo,
        string description,
        string fieldId,
        RuleCondition condition,
        FRList list,
        string[] prereqFields)
      {
        if (CustomFieldInfo.IsCustomFieldID(fieldId))
        {
          CustomFieldInfo field = configInfo.CustomFields.GetField(fieldId);
          if (field == null)
            throw new Exception("Custom field '" + fieldId + "' does not exist");
          if (field.IsListValued())
            return new ValueListFieldRule(description, fieldId, condition, (object[]) null, prereqFields, this.getFieldFormat(configInfo, fieldId));
        }
        return list.IsLock ? new ValueListFieldRule(description, fieldId, condition, (object[]) list.List, prereqFields, this.getFieldFormat(configInfo, fieldId)) : new ValueListFieldRule(description, fieldId, condition, (object[]) null, prereqFields, this.getFieldFormat(configInfo, fieldId));
      }

      private AdvancedCodeFieldRule createCustomRule(
        ILoanConfigurationInfo configInfo,
        string description,
        string fieldId,
        RuleCondition condition,
        string rule,
        string[] prereqFields)
      {
        return new AdvancedCodeFieldRule(description, fieldId, condition, rule, prereqFields, this.getFieldFormat(configInfo, fieldId));
      }

      private FieldFormat getFieldFormat(ILoanConfigurationInfo configInfo, string fieldId)
      {
        if (!CustomFieldInfo.IsCustomFieldID(fieldId))
          return EncompassFields.GetFormat(fieldId);
        return (configInfo.CustomFields.GetField(fieldId) ?? throw new Exception("Custom field '" + fieldId + "' does not exist")).Format;
      }
    }
  }
}

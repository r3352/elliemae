// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.AdvancedCodeFieldRule
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.BusinessRules.AdvancedCode;
using Elli.Domain.BusinessRule;
using Elli.Domain.Mortgage;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace Elli.BusinessRules
{
  public class AdvancedCodeFieldRule : FieldRule
  {
    private readonly string _ruleDefinition;

    public AdvancedCodeFieldRule(
      string description,
      string fieldId,
      RuleCondition condition,
      string definition,
      string[] prereqFields,
      FieldFormat fieldFormat)
      : base(description, fieldId, condition, prereqFields, fieldFormat)
    {
      this._ruleDefinition = definition;
    }

    public AdvancedCodeFieldRule(
      string ruleId,
      string description,
      string fieldId,
      RuleCondition condition,
      string[] prereqFields,
      string definition,
      FieldFormat fieldFormat)
      : base(ruleId, description, fieldId, condition, prereqFields, fieldFormat)
    {
      this._ruleDefinition = definition;
    }

    public string Definition => this._ruleDefinition;

    protected string GetFieldRuleDefinition() => this._ruleDefinition + Environment.NewLine;

    public override bool Validate(object value) => true;

    public bool Validate(
      object value,
      Loan loan,
      UserInfo user,
      IAdvancedCodeEvaluatorFactory advCodeFactory,
      object fieldValue)
    {
      return string.IsNullOrEmpty(this._ruleDefinition) || advCodeFactory.GetAdvancedCodeEvaluator(AdvancedCodeType.FieldRule, loan, user, (object) "", fieldValue).Evaluate(this._ruleDefinition);
    }
  }
}

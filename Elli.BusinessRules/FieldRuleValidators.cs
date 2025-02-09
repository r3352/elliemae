// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.FieldRuleValidators
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.BusinessRules.AdvancedCode;
using Elli.Common.Fields;
using Elli.Domain.BusinessRule;
using Elli.Domain.Extensions;
using Elli.Domain.ModelFields;
using Elli.Domain.Mortgage;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.BusinessRules
{
  public class FieldRuleValidators
  {
    private readonly FieldRule[] _fieldRules;
    private readonly UserInfo _user;
    private readonly IAdvancedCodeEvaluatorFactory _advCodeFactory;

    public FieldRuleValidators(
      FieldRule[] fieldRules,
      UserInfo user,
      IAdvancedCodeEvaluatorFactory advCodeFactory)
    {
      this._fieldRules = fieldRules;
      this._user = user;
      this._advCodeFactory = advCodeFactory;
    }

    public void ValidateAll(
      Loan loan,
      Action<FieldRule, IEnumerable<string>> validationFailedAction)
    {
      foreach (FieldRule fieldRule in this._fieldRules)
      {
        if (fieldRule.Condition.AppliesTo(loan, this._user))
        {
          object fieldValue = (object) null;
          if (fieldRule.GetType() != typeof (ButtonFieldRule))
          {
            fieldValue = this.GetEncompassFieldValue(loan, fieldRule.FieldID);
            if (fieldValue.IsFieldBlank())
              continue;
          }
          List<string> stringList = new List<string>();
          if (fieldRule.PrerequisiteFields.Length != 0)
          {
            foreach (string prerequisiteField in fieldRule.PrerequisiteFields)
            {
              if (this.GetEncompassFieldValue(loan, prerequisiteField).IsFieldBlank())
                stringList.Add(prerequisiteField);
            }
          }
          if (stringList.Count > 0 && validationFailedAction != null)
          {
            validationFailedAction(fieldRule, (IEnumerable<string>) stringList);
            stringList.Clear();
          }
          if (!(fieldRule.GetType() == typeof (ButtonFieldRule)) && (fieldRule is AdvancedCodeFieldRule advancedCodeFieldRule ? (advancedCodeFieldRule.Validate(fieldValue, loan, this._user, this._advCodeFactory, fieldValue) ? 1 : 0) : (fieldRule.Validate(fieldValue) ? 1 : 0)) == 0 && validationFailedAction != null)
            validationFailedAction(fieldRule, (IEnumerable<string>) stringList);
        }
      }
    }

    private object GetEncompassFieldValue(Loan loan, string fieldId)
    {
      string fullModelPath = EncompassFieldData.Instance.GetFullModelPath(fieldId);
      object encompassFieldValue = (object) null;
      if (fullModelPath != null)
        encompassFieldValue = ModelTraverser.GetMemberValue((object) loan, fullModelPath);
      return encompassFieldValue;
    }
  }
}

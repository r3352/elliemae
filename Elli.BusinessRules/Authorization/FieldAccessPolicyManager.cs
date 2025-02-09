// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Authorization.FieldAccessPolicyManager
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.Common.Fields;
using Elli.Domain;
using Elli.Domain.BusinessRule;
using Elli.Domain.ModelFields;
using Elli.Domain.Mortgage;
using Elli.ElliEnum;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.BusinessRules.Authorization
{
  public class FieldAccessPolicyManager
  {
    private readonly IFieldAccessPolicy _policy;

    public FieldAccessPolicyManager(IFieldAccessPolicy policy) => this._policy = policy;

    public FieldAccessUserPolicy Apply(Loan loan, UserInfo user, bool shouldHide = true)
    {
      IEnumerable<FieldAccessRule> source1 = this._policy.FieldAccessRules.Where<FieldAccessRule>((Func<FieldAccessRule, bool>) (p => p.RuleCondition.AppliesTo(loan, user)));
      if (!(source1 is FieldAccessRule[] fieldAccessRuleArray))
        fieldAccessRuleArray = source1.ToArray<FieldAccessRule>();
      FieldAccessRule[] source2 = fieldAccessRuleArray;
      if (shouldHide)
      {
        foreach (FieldAccessRule fieldAccessRule in ((IEnumerable<FieldAccessRule>) source2).Where<FieldAccessRule>((Func<FieldAccessRule, bool>) (p => p.AccessRight == FieldAccessRight.Hide)))
        {
          if (!FieldAccessPolicyManager.IsButtonField(fieldAccessRule.Name))
          {
            string fullModelPath = EncompassFieldData.Instance.GetFullModelPath(fieldAccessRule.Name);
            if (!string.IsNullOrEmpty(fullModelPath))
            {
              fieldAccessRule.ModelPath = fullModelPath;
              object memberModel;
              string memberName;
              ModelTraverser.TryGetModel((object) loan, fullModelPath, loan.CurrentApplicationIndex, out memberModel, out memberName);
              if (memberModel != null)
              {
                if (memberModel is Entity)
                  ModelTraverser.SetModelProperty(memberModel, memberName, (object) null, false);
                else if (memberModel is IEnumerable<object>)
                {
                  foreach (object model in memberModel as IEnumerable<object>)
                  {
                    if (model is Entity)
                      ModelTraverser.SetModelProperty(model, memberName, (object) null, false);
                  }
                }
              }
            }
          }
        }
      }
      FieldAccessUserPolicy accessUserPolicy = new FieldAccessUserPolicy(this._policy.Identity, this._policy.Name);
      accessUserPolicy.FieldAccessRules.AddRange((IEnumerable<FieldAccessRule>) ((IEnumerable<FieldAccessRule>) source2).ToList<FieldAccessRule>());
      return accessUserPolicy;
    }

    public FieldAccessUserPolicy GetAppliedPolicyConditions(Loan loan, UserInfo user)
    {
      return new FieldAccessUserPolicy(this._policy.Identity, this._policy.Name);
    }

    private void MaskObjectProperty(object member, string name)
    {
      member.GetType().GetProperty(name).SetValue(member, (object) null, (object[]) null);
      member.GetType().GetProperty(name).SetValue(member, (object) null, (object[]) null);
    }

    private static bool IsButtonField(string buttonName)
    {
      return buttonName.ToUpper().StartsWith("BUTTON_");
    }
  }
}

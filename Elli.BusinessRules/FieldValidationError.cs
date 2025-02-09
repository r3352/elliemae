// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.FieldValidationError
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.Common;
using Elli.Common.Fields;
using Elli.Domain.BusinessRule;
using Elli.ElliEnum;
using System.Collections.Generic;

#nullable disable
namespace Elli.BusinessRules
{
  public class FieldValidationError
  {
    private readonly EmList<string> _validValues = new EmList<string>();
    private readonly EmList<string> _prerequisiteFieldsNotCompleted = new EmList<string>();

    protected FieldValidationError()
    {
    }

    public FieldValidationError(
      FieldRule fieldRule,
      IEnumerable<string> prerequisiteFieldsNotFilled)
    {
      switch (fieldRule)
      {
        case ValueRangeFieldRule _:
          this.Type = FieldValidationType.Range;
          break;
        case ValueListFieldRule _:
          this.Type = FieldValidationType.List;
          break;
      }
      this.FieldId = fieldRule.FieldID;
      this.Message = "Some prerequisite fields are not completed";
      foreach (string str in prerequisiteFieldsNotFilled)
        this._prerequisiteFieldsNotCompleted.Add(str);
    }

    public FieldValidationError(ValueRangeFieldRule rangeRule)
    {
      this.Type = FieldValidationType.Range;
      this.FieldId = rangeRule.FieldID;
      this.MinValue = EncompassFieldUtils.ApplyFieldFormatting(rangeRule.MinValue, rangeRule.CurrentFieldFormat);
      this.MaxValue = EncompassFieldUtils.ApplyFieldFormatting(rangeRule.MaxValue, rangeRule.CurrentFieldFormat);
      this.Message = string.Format("The value for field '{0}' must be in the range between {1} and {2}", (object) this.FieldId, (object) this.MinValue, (object) this.MaxValue);
    }

    public FieldValidationError(ValueListFieldRule listRule)
    {
      this.Type = FieldValidationType.List;
      this.FieldId = listRule.FieldID;
      foreach (object validValue in listRule.ValidValues)
        this.ValidValues.Add(validValue.ToString());
      this.Message = string.Format("The value for field '{0}' is invalid", (object) this.FieldId);
    }

    public FieldValidationError(AdvancedCodeFieldRule advRule)
    {
      this.Type = FieldValidationType.AdvancedCode;
      this.FieldId = advRule.FieldID;
      this.Message = string.Format("The advanced code condition for field id '{0}' is not satisfied", (object) this.FieldId);
    }

    public FieldValidationType Type { get; private set; }

    public string FieldId { get; private set; }

    public string Message { get; private set; }

    public string MinValue { get; private set; }

    public string MaxValue { get; private set; }

    public EmList<string> ValidValues => this._validValues;

    public EmList<string> PrerequisiteFieldsNotCompleted => this._prerequisiteFieldsNotCompleted;
  }
}

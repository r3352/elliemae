// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.PrintFormSelectorImplDef
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Compiler;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class PrintFormSelectorImplDef : CodedFormSelector
  {
    private PrintSelectionEvent formSelectorEvent;

    public PrintFormSelectorImplDef(
      string description,
      PrintSelectionEvent formSelectorEvent,
      RuleCondition cond)
      : base(description, formSelectorEvent.GetActivationFields(), cond)
    {
      this.formSelectorEvent = formSelectorEvent;
    }

    public PrintSelectionEvent Event => this.formSelectorEvent;

    protected override string GetRuleDefinition()
    {
      CodeWriter code = new CodeWriter(CodeLanguage.VB);
      code.WriteCommentLine("PrintFormSelector Name = " + this.Description);
      code.WriteLine("Dim Active as Boolean = False");
      this.writeEventConditionCode(code);
      code.WriteLine("ExecutePrintFormSelector = Active");
      code.WriteLine();
      return code.ToString();
    }

    private void writeEventConditionCode(CodeWriter code)
    {
      foreach (PrintSelectionCondition condition in this.formSelectorEvent.Conditions)
      {
        code.StartBlock("if String.Compare(FieldID, \"" + condition.FieldID + "\") = 0 then");
        if (condition.ConditionType == PrintSelectionConditionType.FixedValue)
          this.writeFixedValueCondition(code, (PrintSelectionFixedValueCondition) condition);
        else if (condition.ConditionType == PrintSelectionConditionType.Range)
          this.writeRangeCondition(code, (PrintSelectionRangeCondition) condition);
        else if (condition.ConditionType == PrintSelectionConditionType.ValueList)
          this.writeValueListCondition(code, (PrintSelectionValueListCondition) condition);
        else
          this.writeNonEmptyValueCondition(code, (PrintSelectionNonEmptyValueCondition) condition);
        code.EndBlock("end if");
        code.WriteLine();
      }
    }

    private void writeNonEmptyValueCondition(
      CodeWriter code,
      PrintSelectionNonEmptyValueCondition cond)
    {
      code.StartBlock("if Not IsEmpty(NewValue) then");
      code.WriteLine("active = True");
      code.EndBlock("end if");
    }

    private void writeFixedValueCondition(CodeWriter code, PrintSelectionFixedValueCondition cond)
    {
      code.WriteLine("Dim CondValue as Object = XType(" + code.ToLiteralString(this.translateValue(cond.Value)) + ")");
      code.StartBlock("if Object.Equals(NewValue, CondValue) then");
      code.WriteLine("active = True");
      code.EndBlock("end if");
    }

    private void writeRangeCondition(CodeWriter code, PrintSelectionRangeCondition cond)
    {
      code.WriteLine("Dim MinValue as IComparable = CType(XType(" + code.ToLiteralString(this.translateValue(cond.Minimum)) + "), IComparable)");
      code.WriteLine("Dim MaxValue as IComparable = CType(XType(" + code.ToLiteralString(this.translateValue(cond.Maximum)) + "), IComparable)");
      code.StartBlock("if Not NewValue = Nothing then");
      code.StartBlock("if Not Object.Equals(MinValue, Nothing) And Not Object.Equals(MaxValue, Nothing) then");
      code.StartBlock("if MinValue.CompareTo(NewValue) <= 0 And MaxValue.CompareTo(NewValue) >= 0");
      code.WriteLine("active = True");
      code.EndBlock("end if");
      code.ContinueBlock("elseif Not Object.Equals(MinValue, Nothing) then");
      code.StartBlock("if MinValue.CompareTo(NewValue) <= 0");
      code.WriteLine("active = True");
      code.EndBlock("end if");
      code.ContinueBlock("elseif Not Object.Equals(MaxValue, Nothing) then");
      code.StartBlock("if MaxValue.CompareTo(NewValue) >= 0");
      code.WriteLine("active = True");
      code.EndBlock("end if");
      code.EndBlock("end if");
      code.EndBlock("end if");
    }

    private void writeValueListCondition(CodeWriter code, PrintSelectionValueListCondition cond)
    {
      code.WriteLine("Dim CondValue as Object = Nothing");
      for (int index = 0; index < cond.Values.Length; ++index)
      {
        code.WriteLine("CondValue = XType(" + code.ToLiteralString(this.translateValue(cond.Values[index])) + ")");
        code.StartBlock("if Object.Equals(NewValue, CondValue) then");
        code.WriteLine("active = True");
        code.EndBlock("end if");
      }
    }

    private string translateValue(string val)
    {
      return val.ToLower() == "[today]" ? DateTime.Today.ToString("MM/dd/yyyy") : val;
    }
  }
}

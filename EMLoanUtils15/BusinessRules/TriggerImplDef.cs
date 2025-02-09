// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.TriggerImplDef
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Compiler;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class TriggerImplDef : CodedTrigger
  {
    private TriggerEvent triggerEvent;

    public TriggerImplDef(
      string description,
      TriggerEvent triggerEvent,
      RuleCondition cond,
      ConfigInfoForTriggers configInfo)
      : base(description, triggerEvent.GetActivationFields(configInfo), cond)
    {
      this.triggerEvent = triggerEvent;
    }

    public TriggerEvent Event => this.triggerEvent;

    protected override string GetRuleDefinition()
    {
      CodeWriter code = new CodeWriter(CodeLanguage.VB);
      code.WriteCommentLine("Trigger Name = " + this.Description);
      code.WriteLine("Dim Active as Boolean = False");
      this.writeEventConditionCode(code);
      code.WriteLine("if Active = False then Return False");
      code.WriteLine();
      this.writeEventActionCode(code);
      code.WriteLine();
      code.WriteLine("Return True");
      return code.ToString();
    }

    private void writeEventActionCode(CodeWriter code)
    {
      if (this.triggerEvent.Action.ActionType == TriggerActionType.Copy)
        this.writeCopyActionCode(code, (TriggerCopyAction) this.triggerEvent.Action);
      else if (this.triggerEvent.Action.ActionType == TriggerActionType.Assign)
        this.writeAssignmentActionCode(code, (TriggerAssignmentAction) this.triggerEvent.Action);
      else if (this.triggerEvent.Action.ActionType == TriggerActionType.CompleteTasks)
        this.writeTaskCompletionActionCode(code, (TriggerCompleteTasksAction) this.triggerEvent.Action);
      else if (this.triggerEvent.Action.ActionType == TriggerActionType.AdvancedCode)
      {
        this.writeAdvancedCodeActionCode(code, (TriggerAdvancedCodeAction) this.triggerEvent.Action);
      }
      else
      {
        if (this.triggerEvent.Action.ActionType != TriggerActionType.AddSpecialFeatureCode)
          return;
        this.writeSpecialFutureCode(code, (TriggerSpecialFeatureCodesAction) this.triggerEvent.Action);
      }
    }

    private void writeCopyActionCode(CodeWriter code, TriggerCopyAction action)
    {
      foreach (string targetFieldId in action.TargetFieldIDs)
        code.WriteLine("[" + targetFieldId + "] = NewValue");
    }

    private void writeAssignmentActionCode(CodeWriter code, TriggerAssignmentAction action)
    {
      foreach (TriggerAssignment assignment in action.Assignments)
      {
        if (assignment.Evaluate)
          code.WriteLine("[" + assignment.FieldID + "] = " + assignment.Expression.Replace("\r", " ").Replace("\n", " "));
        else
          code.WriteLine("[" + assignment.FieldID + "] = " + code.ToLiteralString(assignment.Expression));
      }
    }

    private void writeTaskCompletionActionCode(CodeWriter code, TriggerCompleteTasksAction action)
    {
      foreach (string taskName in action.TaskNames)
        code.WriteLine("Tasks.SetComplete(" + code.ToLiteralString(taskName) + ")");
    }

    private void writeAdvancedCodeActionCode(CodeWriter code, TriggerAdvancedCodeAction action)
    {
      code.WriteLine(action.SourceCode);
      code.WriteLine();
    }

    private void writeEventConditionCode(CodeWriter code)
    {
      foreach (TriggerCondition condition in this.triggerEvent.Conditions)
      {
        switch (condition)
        {
          case TriggerFieldCondition _:
            this.writeFieldConditionCode(code, condition as TriggerFieldCondition);
            break;
          case TriggerMilestoneCompletionCondition _:
            this.writeMilestoneConditionCode(code, condition as TriggerMilestoneCompletionCondition);
            break;
          case TriggerRateLockCondition _:
            this.writeRateLockConditionCode(code, condition as TriggerRateLockCondition);
            break;
        }
      }
    }

    private void writeFieldConditionCode(CodeWriter code, TriggerFieldCondition cond)
    {
      code.StartBlock("if String.Compare(FieldID, \"" + cond.FieldID + "\", True) = 0 then");
      if (cond.ConditionType == TriggerConditionType.FixedValue)
        this.writeFixedValueCondition(code, (TriggerFixedValueCondition) cond);
      else if (cond.ConditionType == TriggerConditionType.NonEmptyValue)
        this.writeNonEmptyValueCondition(code, (TriggerNonEmptyValueCondition) cond);
      else if (cond.ConditionType == TriggerConditionType.Range)
        this.writeRangeCondition(code, (TriggerRangeCondition) cond);
      else if (cond.ConditionType == TriggerConditionType.ValueList)
        this.writeValueListCondition(code, (TriggerValueListCondition) cond);
      else
        this.writeValueChangeCondition(code, (TriggerValueChangeCondition) cond);
      code.EndBlock("end if");
      code.WriteLine();
    }

    private void writeMilestoneConditionCode(
      CodeWriter code,
      TriggerMilestoneCompletionCondition cond)
    {
      code.StartBlock("if NewValue = \"Achieved\" then");
      code.WriteLine("active = True");
      code.EndBlock("end if");
      code.WriteLine();
    }

    private void writeRateLockConditionCode(CodeWriter code, TriggerRateLockCondition cond)
    {
      string str = "";
      if (cond.Action == TriggerLockAction.Requested)
        str = "Requested";
      else if (cond.Action == TriggerLockAction.Confirmed)
        str = "Confirmed";
      else if (cond.Action == TriggerLockAction.Denied)
        str = "Denied";
      code.StartBlock("if [LOCKRATE.REQUESTSTATUS] = \"" + str + "\" then");
      code.WriteLine("active = True");
      code.EndBlock("end if");
      code.WriteLine();
    }

    private void writeSpecialFutureCode(CodeWriter code, TriggerSpecialFeatureCodesAction action)
    {
      string str = "";
      if (action.SpecialFeatureCodes != null && action.SpecialFeatureCodes.Count > 0)
      {
        foreach (KeyValuePair<string, string> specialFeatureCode in action.SpecialFeatureCodes)
          str = str + (str != "" ? "@@" : "") + specialFeatureCode.Key + "~~" + specialFeatureCode.Value;
      }
      if (!(str != ""))
        return;
      code.WriteLine("Dim sfcCodeList As String");
      code.WriteLine("Dim sfcCodes() As String");
      code.WriteLine("Dim delimiters() As String = { \"@@\" }");
      code.WriteLine("sfcCodeList = \"" + str + "\"");
      code.WriteLine("sfcCodes = sfcCodeList.Split(delimiters, StringSplitOptions.None)");
      code.WriteLine("LoanActions.ApplySpecialFeatureCodes(sfcCodes, false)");
    }

    private void writeValueChangeCondition(CodeWriter code, TriggerValueChangeCondition cond)
    {
      code.WriteLine("active = True");
    }

    private void writeNonEmptyValueCondition(CodeWriter code, TriggerNonEmptyValueCondition cond)
    {
      code.StartBlock("if IsEmpty(PriorValue) And Not IsEmpty(NewValue) then");
      code.WriteLine("active = True");
      code.EndBlock("end if");
    }

    private void writeFixedValueCondition(CodeWriter code, TriggerFixedValueCondition cond)
    {
      code.WriteLine("Dim CondValue as Object = XType(" + code.ToLiteralString(cond.Value) + ")");
      code.StartBlock("if Object.Equals(NewValue, CondValue) then");
      code.WriteLine("active = True");
      code.EndBlock("end if");
    }

    private void writeRangeCondition(CodeWriter code, TriggerRangeCondition cond)
    {
      code.WriteLine("Dim MinValue as IComparable = CType(XType(" + code.ToLiteralString(cond.Minimum) + "), IComparable)");
      code.WriteLine("Dim MaxValue as IComparable = CType(XType(" + code.ToLiteralString(cond.Maximum) + "), IComparable)");
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

    private void writeValueListCondition(CodeWriter code, TriggerValueListCondition cond)
    {
      code.WriteLine("Dim CondValue as Object = Nothing");
      for (int index = 0; index < cond.Values.Length; ++index)
      {
        code.WriteLine("CondValue = XType(" + code.ToLiteralString(cond.Values[index]) + ")");
        code.StartBlock("if Object.Equals(NewValue, CondValue) then");
        code.WriteLine("active = True");
        code.EndBlock("end if");
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Milestone.MilestoneRule
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.AdvCode.Runtime;
using Elli.BusinessRules.AdvancedCode;
using Elli.Common.Fields;
using Elli.Domain.BusinessRule;
using Elli.Domain.ModelFields;
using Elli.Domain.Mortgage;
using Elli.ElliEnum;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.BusinessRules.Milestone
{
  public class MilestoneRule : Elli.Domain.BusinessRule.BusinessRule
  {
    private readonly string _ruleName;
    private readonly Elli.BusinessRules.Milestone.FieldMilestonePair[] _fields;
    private readonly Elli.BusinessRules.Milestone.TaskMilestonePair[] _tasks;
    private readonly Elli.BusinessRules.Milestone.DocMilestonePair[] _docs;
    private readonly AdvancedCodeMilestonePair[] _advancedCode;
    private readonly string _stage;

    public MilestoneRule(
      string ruleName,
      RuleCondition condition,
      Elli.BusinessRules.Milestone.FieldMilestonePair[] fieldPair,
      Elli.BusinessRules.Milestone.TaskMilestonePair[] taskPair,
      Elli.BusinessRules.Milestone.DocMilestonePair[] docPair,
      AdvancedCodeMilestonePair[] advancedCode,
      string stage)
      : base(ruleName, condition)
    {
      this._ruleName = ruleName;
      this._fields = fieldPair;
      this._tasks = taskPair;
      this._docs = docPair;
      this._advancedCode = advancedCode;
      this._stage = stage;
    }

    public Elli.BusinessRules.Milestone.FieldMilestonePair[] FieldMilestonePair => this._fields;

    public Elli.BusinessRules.Milestone.TaskMilestonePair[] TaskMilestonePair => this._tasks;

    public Elli.BusinessRules.Milestone.DocMilestonePair[] DocMilestonePair => this._docs;

    public AdvancedCodeMilestonePair[] AdvancedCodePair => this._advancedCode;

    public string RuleName => this._ruleName;

    public void Validate(
      MilestoneValidationError milestoneError,
      Loan loan,
      UserInfo user,
      IAdvancedCodeEvaluatorFactory advCodeFactory,
      FieldSettings CustomFieldSettings = null,
      bool ignoreWorkFlowTask = false)
    {
      this.ValidateFields(milestoneError, loan, CustomFieldSettings);
      this.ValidateDocs(milestoneError, loan);
      this.ValidateTasks(milestoneError, loan, ignoreWorkFlowTask);
      if (milestoneError.RequiredDocs.Any<RequiredDocInfo>() || milestoneError.RequiredFields.Any<FieldModelPathInfo>() || milestoneError.RequiredTasks.Any<RequiredTaskInfo>())
        return;
      this.PerformAdvCodeAction(milestoneError, loan, user, advCodeFactory);
    }

    private void PerformAdvCodeAction(
      MilestoneValidationError milestoneError,
      Loan loan,
      UserInfo user,
      IAdvancedCodeEvaluatorFactory advCodeFactory)
    {
      try
      {
        if (this._advancedCode == null || this._advancedCode.Length == 0)
          return;
        IAdvancedCodeEvaluator advancedCodeEvaluator = advCodeFactory.GetAdvancedCodeEvaluator(AdvancedCodeType.FieldRule, loan, user, (object) "", (object) "");
        for (int index = 0; index < this._advancedCode.Length; ++index)
          advancedCodeEvaluator.Evaluate(this._advancedCode[index].SourceCode);
      }
      catch (ScriptAbortException ex)
      {
        milestoneError.AdvanceCodeError.Add(ex.Message);
      }
    }

    private void ValidateFields(
      MilestoneValidationError milestoneError,
      Loan loan,
      FieldSettings CustomFieldSettings)
    {
      if (this._fields == null || this._fields.Length == 0)
        return;
      bool flag = false;
      for (int index = 0; index < this._fields.Length; ++index)
      {
        string fieldId = this._fields[index].FieldId;
        if (!string.IsNullOrEmpty(fieldId))
        {
          object obj = this.GetEncompassFieldValue(loan, fieldId);
          if (CustomFieldInfo.IsCustomFieldID(fieldId) && CustomFieldSettings != null)
          {
            FieldDefinition field = EncompassFields.GetField(fieldId, CustomFieldSettings);
            if (field != null && field.Format.ToString().Equals("X") && obj != null && !obj.Equals((object) "X"))
              obj = (object) null;
          }
          if (obj == null || obj.ToString() == "")
          {
            if (milestoneError.RequiredFields.Any<FieldModelPathInfo>((Func<FieldModelPathInfo, bool>) (requiredField => requiredField.FieldId == fieldId)))
              flag = true;
            if (!flag)
            {
              FieldModelPathInfo fieldModelPathInfo = new FieldModelPathInfo()
              {
                FieldId = fieldId
              };
              try
              {
                string modelPath = EncompassFieldData.Instance.Get(fieldId).ModelPath;
                if (!string.IsNullOrEmpty(modelPath))
                  fieldModelPathInfo.ModelPath = modelPath;
              }
              catch (KeyNotFoundException ex)
              {
                FieldDefinition field1 = EncompassFields.GetField(fieldId);
                if (field1 != null)
                {
                  fieldModelPathInfo.Description = field1.Description;
                  fieldModelPathInfo.Format = field1.Format.ToString();
                  fieldModelPathInfo.ModelPath = EncompassFieldData.Instance.GetFullModelPath(fieldId);
                }
                else if (CustomFieldInfo.IsCustomFieldID(fieldId))
                {
                  if (CustomFieldSettings != null)
                  {
                    FieldDefinition field2 = EncompassFields.GetField(fieldId, CustomFieldSettings);
                    if (field2 != null)
                    {
                      fieldModelPathInfo.Description = field2.Description;
                      fieldModelPathInfo.Format = field2.Format.ToString();
                      fieldModelPathInfo.ModelPath = EncompassFieldData.Instance.GetFullModelPath(fieldId);
                    }
                  }
                }
              }
              EncompassField encompassField = EncompassFieldData.Instance.Find(fieldId);
              if (encompassField != null)
              {
                fieldModelPathInfo.Description = encompassField.Description;
                fieldModelPathInfo.Format = encompassField.Format;
                if (encompassField.Options != null && encompassField.Options.Any<EncompassFieldOption>())
                {
                  fieldModelPathInfo.Options = (IList<Elli.BusinessRules.FieldOption>) new List<Elli.BusinessRules.FieldOption>();
                  foreach (EncompassFieldOption option in (IEnumerable<EncompassFieldOption>) encompassField.Options)
                    fieldModelPathInfo.Options.Add(new Elli.BusinessRules.FieldOption()
                    {
                      Description = option.Description,
                      Value = option.Value
                    });
                }
              }
              milestoneError.RequiredFields.Add(fieldModelPathInfo);
            }
            else
              continue;
          }
          flag = false;
        }
      }
    }

    private object GetEncompassFieldValue(Loan loan, string fieldId)
    {
      string fullModelPath = EncompassFieldData.Instance.GetFullModelPath(fieldId);
      object encompassFieldValue = (object) null;
      if (fullModelPath != null)
        encompassFieldValue = ModelTraverser.GetMemberValue((object) loan, fullModelPath, loan.CurrentApplicationIndex);
      return encompassFieldValue;
    }

    private void ValidateDocs(MilestoneValidationError milestoneError, Loan loan)
    {
      if (this._docs == null || this._docs.Length == 0)
        return;
      for (int count = 0; count < this._docs.Length; count++)
      {
        RequiredDocInfo requiredDocInfo1 = new RequiredDocInfo();
        requiredDocInfo1.DocGuid = this._docs[count].DocGUID;
        requiredDocInfo1.AttachedRequired = this._docs[count].AttachmentRequired;
        requiredDocInfo1.Title = this._docs[count].Title;
        if (!milestoneError.RequiredDocs.Any<RequiredDocInfo>((Func<RequiredDocInfo, bool>) (reqdDocInfo => reqdDocInfo.DocGuid == this._docs[count].DocGUID)))
          milestoneError.RequiredDocs.Add(requiredDocInfo1);
        else if (milestoneError.RequiredDocs.Any<RequiredDocInfo>((Func<RequiredDocInfo, bool>) (reqdDocInfo => reqdDocInfo.DocGuid == this._docs[count].DocGUID)) && this._docs[count].AttachmentRequired)
        {
          RequiredDocInfo requiredDocInfo2 = milestoneError.RequiredDocs.FirstOrDefault<RequiredDocInfo>((Func<RequiredDocInfo, bool>) (reqdDocInfo => reqdDocInfo.DocGuid == this._docs[count].DocGUID));
          if (requiredDocInfo2 != null)
            milestoneError.RequiredDocs.Remove(requiredDocInfo2);
          milestoneError.RequiredDocs.Add(requiredDocInfo1);
        }
        foreach (DocumentLog documentLog in loan.DocumentLogs)
        {
          if (this._docs[count].Title == documentLog.Title)
          {
            if (this._docs[count].AttachmentRequired)
            {
              if (documentLog.ReceiveDateUtc.HasValue && documentLog.FileAttachments.Any<FileAttachmentReference>())
              {
                RequiredDocInfo requiredDocInfo3 = milestoneError.RequiredDocs.FirstOrDefault<RequiredDocInfo>((Func<RequiredDocInfo, bool>) (reqdDocInfo => reqdDocInfo.DocGuid == this._docs[count].DocGUID));
                if (requiredDocInfo3 != null)
                  milestoneError.RequiredDocs.Remove(requiredDocInfo3);
              }
            }
            else if (documentLog.ReceiveDateUtc.HasValue)
              milestoneError.RequiredDocs.Remove(requiredDocInfo1);
          }
        }
      }
    }

    private void ValidateTasks(
      MilestoneValidationError milestoneError,
      Loan loan,
      bool ignoreWorkflowTask)
    {
      if (this._tasks == null || this._tasks.Length == 0)
        return;
      foreach (Elli.BusinessRules.Milestone.TaskMilestonePair task1 in this._tasks)
      {
        Elli.BusinessRules.Milestone.TaskMilestonePair task = task1;
        if (!ignoreWorkflowTask || task.TaskType != MilestoneTaskType.MilestoneWorkflow)
        {
          RequiredTaskInfo requiredTaskInfo = new RequiredTaskInfo();
          requiredTaskInfo.TaskGuid = task.TaskGUID;
          requiredTaskInfo.IsRequired = task.IsRequired;
          requiredTaskInfo.TaskType = task.TaskType.ToString();
          requiredTaskInfo.TaskDescription = task.TaskDescription;
          requiredTaskInfo.TaskName = task.TaskName;
          requiredTaskInfo.DaysToComplete = task.DaysToComplete;
          requiredTaskInfo.ExpectedDate = task.DaysToComplete <= 0 ? new DateTime?() : new DateTime?(DateTime.UtcNow.AddDays((double) task.DaysToComplete));
          if (!milestoneError.RequiredTasks.Any<RequiredTaskInfo>((Func<RequiredTaskInfo, bool>) (reqdTaskInfo => reqdTaskInfo.TaskGuid == task.TaskGUID)))
            milestoneError.RequiredTasks.Add(requiredTaskInfo);
          foreach (MilestoneTaskLog milestoneTaskLog in loan.MilestoneTaskLogs)
          {
            MilestoneTaskLog taskLog = milestoneTaskLog;
            if (task.TaskGUID == taskLog.TaskGuid && taskLog.Stage == this._stage)
            {
              bool? requiredIndicator = taskLog.IsRequiredIndicator;
              bool flag = false;
              if (requiredIndicator.GetValueOrDefault() == flag & requiredIndicator.HasValue)
                milestoneError.RequiredTasks.Remove(milestoneError.RequiredTasks.Find((Predicate<RequiredTaskInfo>) (reqTaskInfo => reqTaskInfo.TaskGuid == taskLog.TaskGuid)));
              if (taskLog.Completed)
                milestoneError.RequiredTasks.Remove(milestoneError.RequiredTasks.Find((Predicate<RequiredTaskInfo>) (reqTaskInfo => reqTaskInfo.TaskGuid == taskLog.TaskGuid)));
            }
          }
        }
      }
    }
  }
}

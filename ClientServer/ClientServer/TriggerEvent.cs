// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerEvent : IXmlSerializable
  {
    private TriggerCondition[] conditions;
    private TriggerAction action;

    public TriggerEvent(TriggerCondition condition, TriggerAction action)
      : this(new TriggerCondition[1]{ condition }, action)
    {
    }

    public TriggerEvent(TriggerCondition[] conditions, TriggerAction action)
    {
      if (conditions == null || action == null)
        throw new ArgumentNullException("Must specify a valid condition and action for trigger event");
      this.conditions = conditions.Length != 0 ? conditions : throw new ArgumentException("Must specify at least one valid condition for trigger event");
      this.action = action;
    }

    public TriggerEvent(XmlSerializationInfo info)
    {
      XmlList<TriggerConditionType> xmlList = (XmlList<TriggerConditionType>) info.GetValue("conditionTypes", typeof (XmlList<TriggerConditionType>));
      List<TriggerCondition> triggerConditionList = new List<TriggerCondition>();
      for (int index = 0; index < xmlList.Count; ++index)
        triggerConditionList.Add((TriggerCondition) info.GetValue("condition" + (object) index, TriggerEvent.getConditionObjectType(xmlList[index])));
      this.conditions = triggerConditionList.ToArray();
      TriggerActionType actionType = (TriggerActionType) Enum.Parse(typeof (TriggerActionType), info.GetString("actionType"));
      this.action = (TriggerAction) info.GetValue(nameof (action), TriggerEvent.getActionObjectType(actionType));
    }

    public TriggerCondition[] Conditions => this.conditions;

    public TriggerAction Action => this.action;

    public string[] GetActivationFields(ConfigInfoForTriggers activationData)
    {
      List<string> stringList = new List<string>();
      foreach (TriggerCondition condition in this.conditions)
      {
        foreach (string activationField in condition.GetActivationFields(activationData))
        {
          if (!stringList.Contains(activationField))
            stringList.Add(activationField);
        }
      }
      return stringList.ToArray();
    }

    public string ToXml() => new XmlSerializer().Serialize((object) this);

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      XmlList<TriggerConditionType> xmlList = new XmlList<TriggerConditionType>();
      for (int index = 0; index < this.conditions.Length; ++index)
      {
        xmlList.Add(this.conditions[index].ConditionType);
        info.AddValue("condition" + (object) index, (object) this.conditions[index]);
      }
      info.AddValue("conditionTypes", (object) xmlList);
      info.AddValue("actionType", (object) this.action.ActionType.ToString());
      info.AddValue("action", (object) this.action);
    }

    private static Type getActionObjectType(TriggerActionType actionType)
    {
      switch (actionType)
      {
        case TriggerActionType.Assign:
          return typeof (TriggerAssignmentAction);
        case TriggerActionType.Copy:
          return typeof (TriggerCopyAction);
        case TriggerActionType.AdvancedCode:
          return typeof (TriggerAdvancedCodeAction);
        case TriggerActionType.CompleteTasks:
          return typeof (TriggerCompleteTasksAction);
        case TriggerActionType.Email:
          return typeof (TriggerEmailAction);
        case TriggerActionType.LoanMove:
          return typeof (TriggerMoveLoanFolderAction);
        case TriggerActionType.ApplyLoanTemplate:
          return typeof (TriggerApplyLoanTemplateAction);
        case TriggerActionType.AddSpecialFeatureCode:
          return typeof (TriggerSpecialFeatureCodesAction);
        default:
          return (Type) null;
      }
    }

    private static Type getConditionObjectType(TriggerConditionType conditionType)
    {
      switch (conditionType)
      {
        case TriggerConditionType.ValueChange:
          return typeof (TriggerValueChangeCondition);
        case TriggerConditionType.FixedValue:
          return typeof (TriggerFixedValueCondition);
        case TriggerConditionType.Range:
          return typeof (TriggerRangeCondition);
        case TriggerConditionType.ValueList:
          return typeof (TriggerValueListCondition);
        case TriggerConditionType.MilestoneCompleted:
          return typeof (TriggerMilestoneCompletionCondition);
        case TriggerConditionType.LockRequested:
        case TriggerConditionType.LockConfirmed:
        case TriggerConditionType.LockDenied:
          return typeof (TriggerRateLockCondition);
        case TriggerConditionType.RegisterLoan:
        case TriggerConditionType.ImportAdditionalData:
        case TriggerConditionType.OrderReissueCredit:
        case TriggerConditionType.Disclosures:
        case TriggerConditionType.SubmitLoan:
        case TriggerConditionType.ChangedCircumstance:
        case TriggerConditionType.LockRequest:
        case TriggerConditionType.RunDUUnderwriting:
        case TriggerConditionType.ReSubmitLoan:
        case TriggerConditionType.ViewPurchaseAdvice:
        case TriggerConditionType.LockExtension:
        case TriggerConditionType.RunLPUnderwriting:
        case TriggerConditionType.SubmitPurchase:
        case TriggerConditionType.FloatLock:
        case TriggerConditionType.CancelLock:
        case TriggerConditionType.RePriceLock:
        case TriggerConditionType.ReLockLock:
        case TriggerConditionType.ChangeRequestOB:
        case TriggerConditionType.Withdrawal:
        case TriggerConditionType.Cancel:
        case TriggerConditionType.RequestLoanEstimate:
        case TriggerConditionType.RequestTitleFees:
        case TriggerConditionType.GenerateLoanEstimateDisclosure:
        case TriggerConditionType.OrderAppraisalRequest:
        case TriggerConditionType.OrderAUS:
        case TriggerConditionType.SaveLoan:
          return typeof (TriggerGenericTPOCondition);
        default:
          return (Type) null;
      }
    }
  }
}

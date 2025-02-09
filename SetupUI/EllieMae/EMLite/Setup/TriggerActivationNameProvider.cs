// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TriggerActivationNameProvider
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TriggerActivationNameProvider : CustomEnumNameProvider
  {
    private static string[] loanActions = new string[18]
    {
      "TPO_RegisterLoan",
      "TPO_ImportAdditionalData",
      "TPO_OrderRe-issueCredit",
      "TPO_Disclosures",
      "TPO_SubmitLoan",
      "TPO_CoCRequested",
      "TPO_LockRequested",
      "TPO_DU",
      "TPO_LP",
      "TPO_Re-submitLoan",
      "TPO_LockExtensionRequested",
      "TPO_WithdrawalLoan",
      "TPO_CancelLoan",
      "TPO_RequestLoanEstimate",
      "TPO_RequestTitleFees",
      "TPO_GenerateLEDisclosures",
      "TPO_OrderAppraisal",
      "TPO_OrderAUS"
    };
    private static Hashtable nameMap = CollectionsUtil.CreateCaseInsensitiveHashtable();
    public const string RATE_LOCK_ACTIONS = "Rate Lock actions";
    public const string TPO_ACTIONS = "TPO actions";

    static TriggerActivationNameProvider()
    {
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.FieldModified, (object) "Field value modified");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.MilestoneCompleted, (object) "Milestone completed");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.LockRequested, (object) "Rate lock requested");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.LockConfirmed, (object) "Rate lock confirmed");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.LockDenied, (object) "Rate lock denied");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.RegisterLoan, (object) "Register Loan");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.ImportAdditionalData, (object) "Import Additional Data");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.OrderReissueCredit, (object) "Order/Reissue Credit");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.RequestTitleFees, (object) "Request Title Fees");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.GenerateLoanEstimateDisclosure, (object) "Generate Loan Estimate/Disclosures");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.OrderAppraisalRequest, (object) "Order Appraisal Request");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.OrderAUS, (object) "Order AUS");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.RequestLoanEstimate, (object) "Request Loan Estimate");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.Disclosures, (object) "Disclosures");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.SubmitLoan, (object) "Submit Loan");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.ChangedCircumstance, (object) "Change of Circumstance");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.LockRequest, (object) "Lock Request");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.RunDUUnderwriting, (object) "Run DU Underwriting");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.RunLPUnderwriting, (object) "Run LP Underwriting");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.ReSubmitLoan, (object) "Re-submit Loan");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.ViewPurchaseAdvice, (object) "View Purchase Advice");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.LockExtension, (object) "Lock Extension");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.Withdrawal, (object) "Withdraw");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.Cancel, (object) "Cancel");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.SubmitPurchase, (object) "Submit for Purchase");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.FloatLock, (object) "Float Lock");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.CancelLock, (object) "Cancel Lock");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.RePriceLock, (object) "RePrice Lock");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.ReLockLock, (object) "ReLock Lock");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.ChangeRequestOB, (object) "Change Request - OB");
      TriggerActivationNameProvider.nameMap.Add((object) TriggerActivationType.SaveLoan, (object) "Save Loan");
    }

    public TriggerActivationNameProvider()
      : base(typeof (TriggerActivationType), TriggerActivationNameProvider.nameMap)
    {
    }

    public TriggerActivationType GetActivationTypeForCondition(TriggerConditionType condType)
    {
      switch (condType)
      {
        case TriggerConditionType.ValueChange:
          return TriggerActivationType.FieldModified;
        case TriggerConditionType.FixedValue:
          return TriggerActivationType.FieldModified;
        case TriggerConditionType.Range:
          return TriggerActivationType.FieldModified;
        case TriggerConditionType.ValueList:
          return TriggerActivationType.FieldModified;
        case TriggerConditionType.NonEmptyValue:
          return TriggerActivationType.FieldModified;
        case TriggerConditionType.MilestoneCompleted:
          return TriggerActivationType.MilestoneCompleted;
        case TriggerConditionType.LockRequested:
          return TriggerActivationType.LockRequested;
        case TriggerConditionType.LockConfirmed:
          return TriggerActivationType.LockConfirmed;
        case TriggerConditionType.LockDenied:
          return TriggerActivationType.LockDenied;
        case TriggerConditionType.RegisterLoan:
          return TriggerActivationType.RegisterLoan;
        case TriggerConditionType.ImportAdditionalData:
          return TriggerActivationType.ImportAdditionalData;
        case TriggerConditionType.OrderReissueCredit:
          return TriggerActivationType.OrderReissueCredit;
        case TriggerConditionType.Disclosures:
          return TriggerActivationType.Disclosures;
        case TriggerConditionType.SubmitLoan:
          return TriggerActivationType.SubmitLoan;
        case TriggerConditionType.ChangedCircumstance:
          return TriggerActivationType.ChangedCircumstance;
        case TriggerConditionType.LockRequest:
          return TriggerActivationType.LockRequest;
        case TriggerConditionType.RunDUUnderwriting:
          return TriggerActivationType.RunDUUnderwriting;
        case TriggerConditionType.ReSubmitLoan:
          return TriggerActivationType.ReSubmitLoan;
        case TriggerConditionType.ViewPurchaseAdvice:
          return TriggerActivationType.ViewPurchaseAdvice;
        case TriggerConditionType.LockExtension:
          return TriggerActivationType.LockExtension;
        case TriggerConditionType.RunLPUnderwriting:
          return TriggerActivationType.RunLPUnderwriting;
        case TriggerConditionType.SubmitPurchase:
          return TriggerActivationType.SubmitPurchase;
        case TriggerConditionType.FloatLock:
          return TriggerActivationType.FloatLock;
        case TriggerConditionType.CancelLock:
          return TriggerActivationType.CancelLock;
        case TriggerConditionType.RePriceLock:
          return TriggerActivationType.RePriceLock;
        case TriggerConditionType.ReLockLock:
          return TriggerActivationType.ReLockLock;
        case TriggerConditionType.ChangeRequestOB:
          return TriggerActivationType.ChangeRequestOB;
        case TriggerConditionType.Withdrawal:
          return TriggerActivationType.Withdrawal;
        case TriggerConditionType.Cancel:
          return TriggerActivationType.Cancel;
        case TriggerConditionType.RequestLoanEstimate:
          return TriggerActivationType.RequestLoanEstimate;
        case TriggerConditionType.RequestTitleFees:
          return TriggerActivationType.RequestTitleFees;
        case TriggerConditionType.GenerateLoanEstimateDisclosure:
          return TriggerActivationType.GenerateLoanEstimateDisclosure;
        case TriggerConditionType.OrderAppraisalRequest:
          return TriggerActivationType.OrderAppraisalRequest;
        case TriggerConditionType.OrderAUS:
          return TriggerActivationType.OrderAUS;
        case TriggerConditionType.SaveLoan:
          return TriggerActivationType.SaveLoan;
        default:
          return TriggerActivationType.FieldModified;
      }
    }

    public string GetNameForConditionType(TriggerConditionType condType)
    {
      return this.GetName((object) this.GetActivationTypeForCondition(condType));
    }

    public string GetNameForConditionTypeToDisplay(TriggerConditionType condType)
    {
      switch (condType)
      {
        case TriggerConditionType.ValueChange:
        case TriggerConditionType.FixedValue:
        case TriggerConditionType.Range:
        case TriggerConditionType.ValueList:
        case TriggerConditionType.NonEmptyValue:
        case TriggerConditionType.MilestoneCompleted:
          return this.GetName((object) this.GetActivationTypeForCondition(condType));
        case TriggerConditionType.LockRequested:
        case TriggerConditionType.LockConfirmed:
        case TriggerConditionType.LockDenied:
          return "Rate Lock actions";
        default:
          return "TPO actions";
      }
    }

    public List<string> GetActivationTypes()
    {
      return new List<string>()
      {
        TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.FieldModified].ToString(),
        TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.MilestoneCompleted].ToString(),
        "Rate Lock actions",
        "TPO actions"
      };
    }

    public string[] GetActivationTypesByParent(string parentGroup)
    {
      List<string> stringList = new List<string>();
      if ("Rate Lock actions".Equals(parentGroup))
      {
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.LockRequested].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.LockConfirmed].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.LockDenied].ToString());
      }
      else if ("TPO actions".Equals(parentGroup))
      {
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.RegisterLoan].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.ImportAdditionalData].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.OrderReissueCredit].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.RequestTitleFees].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.GenerateLoanEstimateDisclosure].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.OrderAppraisalRequest].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.OrderAUS].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.RequestLoanEstimate].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.Disclosures].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.SubmitLoan].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.ChangedCircumstance].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.LockRequest].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.RunDUUnderwriting].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.RunLPUnderwriting].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.ReSubmitLoan].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.ViewPurchaseAdvice].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.LockExtension].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.Withdrawal].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.Cancel].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.SubmitPurchase].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.FloatLock].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.CancelLock].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.RePriceLock].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.ReLockLock].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.ChangeRequestOB].ToString());
        stringList.Add(TriggerActivationNameProvider.nameMap[(object) TriggerActivationType.SaveLoan].ToString());
      }
      return stringList.ToArray();
    }

    public TriggerActivationType GetTriggerActivationTypeFromDescription(
      string activationDescription)
    {
      return TriggerActivationNameProvider.nameMap.Keys.OfType<TriggerActivationType>().FirstOrDefault<TriggerActivationType>((Func<TriggerActivationType, bool>) (s => TriggerActivationNameProvider.nameMap[(object) s].Equals((object) activationDescription)));
    }

    public string GetDescriptionFromActivationType(TriggerActivationType triggerActivationType)
    {
      return TriggerActivationNameProvider.nameMap[(object) triggerActivationType].ToString();
    }

    public static bool IsLoanActionValid(string loanActionName)
    {
      return !string.IsNullOrEmpty(loanActionName) && ((IEnumerable<string>) TriggerActivationNameProvider.loanActions).Contains<string>(loanActionName);
    }
  }
}

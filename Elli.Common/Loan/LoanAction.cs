// Decompiled with JetBrains decompiler
// Type: Elli.Common.Loan.LoanAction
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Elli.ElliEnum;
using System.Collections.Generic;

#nullable disable
namespace Elli.Common.Loan
{
  public class LoanAction
  {
    private static Dictionary<string, string> _loanActionMapping = new Dictionary<string, string>();

    static LoanAction()
    {
      LoanAction._loanActionMapping.Add("tpo_registerloan", LoanActionType.RegisterLoan.ToString());
      LoanAction._loanActionMapping.Add("tpo_importadditionaldata", LoanActionType.ImportAdditionalData.ToString());
      LoanAction._loanActionMapping.Add("tpo_orderre-issuecredit", LoanActionType.OrderReissueCredit.ToString());
      LoanAction._loanActionMapping.Add("tpo_disclosures", LoanActionType.Disclosures.ToString());
      LoanAction._loanActionMapping.Add("tpo_submitloan", LoanActionType.SubmitLoan.ToString());
      LoanAction._loanActionMapping.Add("tpo_cocrequested", LoanActionType.ChangedCircumstance.ToString());
      LoanAction._loanActionMapping.Add("tpo_lockrequested", LoanActionType.LockRequest.ToString());
      LoanAction._loanActionMapping.Add("tpo_du", LoanActionType.RunDUUnderwriting.ToString());
      LoanAction._loanActionMapping.Add("tpo_lp", LoanActionType.RunLPUnderwriting.ToString());
      LoanAction._loanActionMapping.Add("tpo_re-submitloan", LoanActionType.ReSubmitLoan.ToString());
      LoanAction._loanActionMapping.Add("tpo_lockextensionrequested", LoanActionType.LockExtension.ToString());
      LoanAction._loanActionMapping.Add("tpo_withdrawalloan", LoanActionType.Withdrawal.ToString());
      LoanAction._loanActionMapping.Add("tpo_cancellock", LoanActionType.CancelLock.ToString());
      LoanAction._loanActionMapping.Add("tpo_repricelock", LoanActionType.RePriceLock.ToString());
      LoanAction._loanActionMapping.Add("tpo_relocklock", LoanActionType.ReLockLock.ToString());
      LoanAction._loanActionMapping.Add("tpo_requesttitlefees", LoanActionType.RequestTitleFees.ToString());
      LoanAction._loanActionMapping.Add("tpo_generateledisclosures", LoanActionType.GenerateLoanEstimateDisclosure.ToString());
      LoanAction._loanActionMapping.Add("tpo_orderappraisal", LoanActionType.OrderAppraisalRequest.ToString());
      LoanAction._loanActionMapping.Add("tpo_orderaus", LoanActionType.OrderAUS.ToString());
    }

    public static string GetActualLoanActionName(string actionName)
    {
      if (string.IsNullOrWhiteSpace(actionName))
        return "";
      string lower = actionName.ToLower();
      return LoanAction._loanActionMapping.ContainsKey(lower) ? LoanAction._loanActionMapping[lower] : "";
    }
  }
}

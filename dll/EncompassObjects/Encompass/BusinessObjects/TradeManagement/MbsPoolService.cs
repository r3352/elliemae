// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPoolService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Trading;
using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class MbsPoolService : SessionBoundObject, IMbsPoolService
  {
    private ISecurityTradeManager mngr;
    private ILoanTradeManager ltmngr;
    private IMbsPoolManager mbsmngr;

    internal MbsPoolService(Session session)
      : base(session)
    {
      this.mngr = (ISecurityTradeManager) session.GetObject("SecurityTradeManager");
      this.ltmngr = (ILoanTradeManager) session.GetObject("LoanTradeManager");
      this.mbsmngr = (IMbsPoolManager) session.GetObject("MbsPoolManager");
    }

    public MbsPool GetMbsPool(string mbsPoolId)
    {
      ICursor icursor = ((IMbsPoolManager) this.Session.GetObject("MbsPoolManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("MbsPoolDetails.Name", (Array) new string[1]
        {
          mbsPoolId
        }, true)
      }, (SortField[]) null, (string[]) null, true, false);
      return icursor != null && icursor.GetItemCount() > 0 ? new MbsPool(((MbsPoolViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))[0]) : (MbsPool) null;
    }

    public List<TradeLoanAssignment> GetTradeLoanAssignments(string poolId)
    {
      List<TradeLoanAssignment> tradeLoanAssignments = new List<TradeLoanAssignment>();
      this.mbsmngr = (IMbsPoolManager) this.Session.GetObject("MbsPoolManager");
      MbsPoolInfo tradeByName = this.mbsmngr.GetTradeByName(poolId);
      if (tradeByName == null)
        return tradeLoanAssignments;
      foreach (PipelineInfo assignedOrPendingLoan in this.mbsmngr.GetAssignedOrPendingLoans(((TradeBase) tradeByName).TradeID, TradeLoanAssignment.GetFieldList().ToArray(), false))
        tradeLoanAssignments.Add(new TradeLoanAssignment(tradeByName, assignedOrPendingLoan));
      return tradeLoanAssignments;
    }

    public List<MbsPoolSecurityTrade> GetPoolSecurityTradeAssignments(string poolId)
    {
      List<MbsPoolSecurityTrade> tradeAssignments = new List<MbsPoolSecurityTrade>();
      this.mbsmngr = (IMbsPoolManager) this.Session.GetObject("MbsPoolManager");
      MbsPoolInfo tradeByName = this.mbsmngr.GetTradeByName(poolId);
      if (tradeByName == null)
        return tradeAssignments;
      MbsPoolAssignment[] tradeAssigments = this.mbsmngr.GetTradeAssigments(((TradeBase) tradeByName).TradeID);
      if (tradeAssigments != null && tradeAssigments.Length != 0)
      {
        QueryCriterion[] queryCriterionArray = new QueryCriterion[tradeAssigments.Length];
        for (int index = 0; index < tradeAssigments.Length; ++index)
          queryCriterionArray[index] = (QueryCriterion) new OrdinalValueCriterion("SecurityTradeDetails.TradeID", (object) ((TradeBase) tradeAssigments[index].AssigneeTrade).TradeID);
        ICursor icursor = this.mngr.OpenTradeCursor(new QueryCriterion[1]
        {
          QueryCriterion.Join(queryCriterionArray, (BinaryOperator) 1)
        }, (SortField[]) null, (string[]) null, true, false);
        foreach (SecurityTradeViewModel securityTradeViewModel in (SecurityTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
        {
          SecurityTradeViewModel view = securityTradeViewModel;
          MbsPoolAssignment mbsPoolAssignment = ((IEnumerable<MbsPoolAssignment>) tradeAssigments).Where<MbsPoolAssignment>((Func<MbsPoolAssignment, bool>) (p =>
          {
            int? assigneeTradeId = ((TradeAssignmentByTradeBase) p).AssigneeTradeID;
            int tradeId = ((TradeBase) view).TradeID;
            return assigneeTradeId.GetValueOrDefault() == tradeId & assigneeTradeId.HasValue;
          })).FirstOrDefault<MbsPoolAssignment>();
          Decimal assignedAmount = mbsPoolAssignment != null ? ((TradeAssignmentByTradeBase) mbsPoolAssignment).AssignedAmount : 0M;
          tradeAssignments.Add(new MbsPoolSecurityTrade(view, assignedAmount));
        }
      }
      return tradeAssignments;
    }

    public List<MbsPool> GetMbsPools(TradeStatus[] tradeStatus)
    {
      List<MbsPool> mbsPools = new List<MbsPool>();
      IMbsPoolManager imbsPoolManager = (IMbsPoolManager) this.Session.GetObject("MbsPoolManager");
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (!((IEnumerable<TradeStatus>) tradeStatus).Any<TradeStatus>((Func<TradeStatus, bool>) (a => a == TradeStatus.None)))
      {
        int[] numArray = new int[tradeStatus.Length];
        for (int index = 0; index < tradeStatus.Length; ++index)
          numArray[index] = (int) tradeStatus[index];
        queryCriterion = (QueryCriterion) new ListValueCriterion("MbsPoolDetails.Status", (Array) numArray, true);
      }
      ICursor icursor = imbsPoolManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, (string[]) null, true, false);
      if (icursor == null || icursor.GetItemCount() <= 0)
        return mbsPools;
      foreach (MbsPoolViewModel info in (MbsPoolViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
        mbsPools.Add(new MbsPool(info));
      return mbsPools;
    }
  }
}

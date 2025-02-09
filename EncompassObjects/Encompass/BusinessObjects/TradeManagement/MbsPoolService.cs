// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPoolService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

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
  /// <summary>Represents Mbs pool service.</summary>
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

    /// <summary>
    /// Get The <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPool" /> by Mbs pool id
    /// </summary>
    /// <param name="mbsPoolId">Mbs pool Id</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPool" /> record for the request.</returns>
    /// <remarks>Return details of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPool" /> when passing in the identifier of Mbs pool
    /// </remarks>
    public MbsPool GetMbsPool(string mbsPoolId)
    {
      ICursor cursor = ((IMbsPoolManager) this.Session.GetObject("MbsPoolManager")).OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("MbsPoolDetails.Name", (Array) new string[1]
        {
          mbsPoolId
        }, true)
      }, (SortField[]) null, (string[]) null, true, false);
      return cursor != null && cursor.GetItemCount() > 0 ? new MbsPool(((MbsPoolViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))[0]) : (MbsPool) null;
    }

    /// <summary>
    /// Get The <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /> by poolId
    /// </summary>
    /// <param name="poolId">Pool Id</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /> record for the request.</returns>
    /// <remarks>Return details of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment" /> when passing in the identifier of pool id
    /// </remarks>
    public List<TradeLoanAssignment> GetTradeLoanAssignments(string poolId)
    {
      List<TradeLoanAssignment> tradeLoanAssignments = new List<TradeLoanAssignment>();
      this.mbsmngr = (IMbsPoolManager) this.Session.GetObject("MbsPoolManager");
      MbsPoolInfo tradeByName = this.mbsmngr.GetTradeByName(poolId);
      if (tradeByName == null)
        return tradeLoanAssignments;
      foreach (PipelineInfo assignedOrPendingLoan in this.mbsmngr.GetAssignedOrPendingLoans(tradeByName.TradeID, TradeLoanAssignment.GetFieldList().ToArray(), false))
        tradeLoanAssignments.Add(new TradeLoanAssignment(tradeByName, assignedOrPendingLoan));
      return tradeLoanAssignments;
    }

    /// <summary>
    /// Get The <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPoolSecurityTrade" /> by poolId
    /// </summary>
    /// <param name="poolId">Pool Id</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPoolSecurityTrade" /> record for the request.</returns>
    /// <remarks>Return details of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPoolSecurityTrade" /> when passing in the identifier of pool id
    /// </remarks>
    public List<MbsPoolSecurityTrade> GetPoolSecurityTradeAssignments(string poolId)
    {
      List<MbsPoolSecurityTrade> tradeAssignments = new List<MbsPoolSecurityTrade>();
      this.mbsmngr = (IMbsPoolManager) this.Session.GetObject("MbsPoolManager");
      MbsPoolInfo tradeByName = this.mbsmngr.GetTradeByName(poolId);
      if (tradeByName == null)
        return tradeAssignments;
      MbsPoolAssignment[] tradeAssigments = this.mbsmngr.GetTradeAssigments(tradeByName.TradeID);
      if (tradeAssigments != null && tradeAssigments.Length != 0)
      {
        QueryCriterion[] criteria = new QueryCriterion[tradeAssigments.Length];
        for (int index = 0; index < tradeAssigments.Length; ++index)
          criteria[index] = (QueryCriterion) new OrdinalValueCriterion("SecurityTradeDetails.TradeID", (object) tradeAssigments[index].AssigneeTrade.TradeID);
        ICursor cursor = this.mngr.OpenTradeCursor(new QueryCriterion[1]
        {
          QueryCriterion.Join(criteria, BinaryOperator.Or)
        }, (SortField[]) null, (string[]) null, true, false);
        foreach (SecurityTradeViewModel securityTradeViewModel in (SecurityTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
        {
          SecurityTradeViewModel view = securityTradeViewModel;
          MbsPoolAssignment mbsPoolAssignment = ((IEnumerable<MbsPoolAssignment>) tradeAssigments).Where<MbsPoolAssignment>((Func<MbsPoolAssignment, bool>) (p =>
          {
            int? assigneeTradeId = p.AssigneeTradeID;
            int tradeId = view.TradeID;
            return assigneeTradeId.GetValueOrDefault() == tradeId & assigneeTradeId.HasValue;
          })).FirstOrDefault<MbsPoolAssignment>();
          Decimal assignedAmount = mbsPoolAssignment != null ? mbsPoolAssignment.AssignedAmount : 0M;
          tradeAssignments.Add(new MbsPoolSecurityTrade(view, assignedAmount));
        }
      }
      return tradeAssignments;
    }

    /// <summary>
    /// Get The <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPool" /> by pool id
    /// </summary>
    /// <param name="tradeStatus">Loan Trade Id</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPool" /> record for the request.</returns>
    /// <remarks>Return details of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPool" /> when passing in the identifier of loan trade
    /// </remarks>
    public List<MbsPool> GetMbsPools(TradeStatus[] tradeStatus)
    {
      List<MbsPool> mbsPools = new List<MbsPool>();
      IMbsPoolManager mbsPoolManager = (IMbsPoolManager) this.Session.GetObject("MbsPoolManager");
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (!((IEnumerable<TradeStatus>) tradeStatus).Any<TradeStatus>((Func<TradeStatus, bool>) (a => a == TradeStatus.None)))
      {
        int[] valueList = new int[tradeStatus.Length];
        for (int index = 0; index < tradeStatus.Length; ++index)
          valueList[index] = (int) tradeStatus[index];
        queryCriterion = (QueryCriterion) new ListValueCriterion("MbsPoolDetails.Status", (Array) valueList, true);
      }
      ICursor cursor = mbsPoolManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, (string[]) null, true, false);
      if (cursor == null || cursor.GetItemCount() <= 0)
        return mbsPools;
      foreach (MbsPoolViewModel info in (MbsPoolViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
        mbsPools.Add(new MbsPool(info));
      return mbsPools;
    }
  }
}

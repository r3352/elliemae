// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Trading;
using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class SecurityTradeService : SessionBoundObject, ISecurityTradeService
  {
    private ISecurityTradeManager mngr;
    private ILoanTradeManager ltmngr;
    private IMbsPoolManager mbsmngr;

    public SecurityTradeService(Session session)
      : base(session)
    {
      this.mngr = (ISecurityTradeManager) session.GetObject("SecurityTradeManager");
      this.ltmngr = (ILoanTradeManager) session.GetObject("LoanTradeManager");
      this.mbsmngr = (IMbsPoolManager) session.GetObject("MbsPoolManager");
    }

    public SecurityTradeObject GetSercurityTradeByNumber(string tradeNumber)
    {
      ICursor icursor = this.mngr.OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("SecurityTradeDetails.Name", (Array) new string[1]
        {
          tradeNumber
        }, true)
      }, (SortField[]) null, (string[]) null, true, false);
      return icursor != null && icursor.GetItemCount() > 0 ? new SecurityTradeObject(((SecurityTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))[0]) : (SecurityTradeObject) null;
    }

    public AssignedLoanTrades[] GetAssingedLoanTradesInSecurityTrade(string tradeNumber)
    {
      List<AssignedLoanTrades> assignedLoanTradesList = new List<AssignedLoanTrades>();
      ICursor icursor = this.ltmngr.OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("SecurityTradeDetails.Name", (Array) new string[1]
        {
          tradeNumber
        }, true)
      }, (SortField[]) null, (string[]) null, true, false);
      foreach (LoanTradeViewModel vm in (LoanTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
        assignedLoanTradesList.Add(new AssignedLoanTrades(vm));
      return assignedLoanTradesList.ToArray();
    }

    public AssignedMBSPools[] GetAssingedMBSPoolsInSecurityTrade(string tradeNumber)
    {
      SecurityTradeInfo tradeByName = this.mngr.GetTradeByName(tradeNumber);
      List<AssignedMBSPools> assignedMbsPoolsList = new List<AssignedMBSPools>();
      if (tradeByName == null)
        return assignedMbsPoolsList.ToArray();
      MbsPoolAssignment[] assigmentsBySecurityTrade = this.mbsmngr.GetTradeAssigmentsBySecurityTrade(((TradeBase) tradeByName).TradeID);
      int?[] array = ((IEnumerable<MbsPoolAssignment>) assigmentsBySecurityTrade).Select<MbsPoolAssignment, int?>((Func<MbsPoolAssignment, int?>) (m => ((TradeAssignmentByTradeBase) m).TradeID)).ToArray<int?>();
      if (array != null)
      {
        QueryCriterion[] queryCriterionArray = new QueryCriterion[array.Length];
        for (int index = 0; index < array.Length; ++index)
          queryCriterionArray[index] = (QueryCriterion) new OrdinalValueCriterion("MbsPoolDetails.TradeID", (object) array[index]);
        ICursor icursor = this.mbsmngr.OpenTradeCursor(new QueryCriterion[1]
        {
          QueryCriterion.Join(queryCriterionArray, (BinaryOperator) 1)
        }, (SortField[]) null, (string[]) null, true, false);
        foreach (MbsPoolViewModel mbsPoolViewModel in (MbsPoolViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
        {
          MbsPoolViewModel vm = mbsPoolViewModel;
          MbsPoolAssignment mbsPoolAssignment = ((IEnumerable<MbsPoolAssignment>) assigmentsBySecurityTrade).Where<MbsPoolAssignment>((Func<MbsPoolAssignment, bool>) (p =>
          {
            int? tradeId1 = ((TradeAssignmentByTradeBase) p).TradeID;
            int tradeId2 = ((TradeBase) vm).TradeID;
            return tradeId1.GetValueOrDefault() == tradeId2 & tradeId1.HasValue;
          })).FirstOrDefault<MbsPoolAssignment>();
          Decimal assignedAmount = mbsPoolAssignment != null ? ((TradeAssignmentByTradeBase) mbsPoolAssignment).AssignedAmount : 0M;
          AssignedMBSPools assignedMbsPools = new AssignedMBSPools(vm, assignedAmount);
          assignedMbsPoolsList.Add(assignedMbsPools);
        }
      }
      return assignedMbsPoolsList.ToArray();
    }

    public List<SecurityTradeObject> GetSecurityTradeByStatus(SecurityTradeStatus[] tradeStatus)
    {
      List<SecurityTradeObject> securityTradeByStatus = new List<SecurityTradeObject>();
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (!((IEnumerable<SecurityTradeStatus>) tradeStatus).Any<SecurityTradeStatus>((Func<SecurityTradeStatus, bool>) (a => a == SecurityTradeStatus.None)))
      {
        int[] numArray = new int[tradeStatus.Length];
        for (int index = 0; index < tradeStatus.Length; ++index)
          numArray[index] = (int) tradeStatus[index];
        queryCriterion = (QueryCriterion) new ListValueCriterion("SecurityTradeDetails.Status", (Array) numArray, true);
      }
      ICursor icursor = this.mngr.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, (string[]) null, true, false);
      if (icursor == null || icursor.GetItemCount() <= 0)
        return securityTradeByStatus;
      foreach (SecurityTradeViewModel info in (SecurityTradeViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
        securityTradeByStatus.Add(new SecurityTradeObject(info));
      return securityTradeByStatus;
    }

    public int CreateSecurityTrade(SecurityTradeObject trade)
    {
      if (string.IsNullOrEmpty(trade.SecurityId))
        throw new Exception("Security Trade with Security Id/Name can not be blank.");
      if (this.mngr.GetTradeByName(trade.SecurityId) != null)
        throw new Exception("Security Trade with Security Id/Name " + trade.SecurityId + " already exists.");
      return this.mngr.CreateTrade(trade.GetSecurityTradeInfo());
    }
  }
}

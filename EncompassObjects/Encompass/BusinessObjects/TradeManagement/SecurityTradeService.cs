// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

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
  /// <summary>SecurityTradeService</summary>
  public class SecurityTradeService : SessionBoundObject, ISecurityTradeService
  {
    private ISecurityTradeManager mngr;
    private ILoanTradeManager ltmngr;
    private IMbsPoolManager mbsmngr;

    /// <summary>SecurityTradeService</summary>
    /// <param name="session"></param>
    public SecurityTradeService(Session session)
      : base(session)
    {
      this.mngr = (ISecurityTradeManager) session.GetObject("SecurityTradeManager");
      this.ltmngr = (ILoanTradeManager) session.GetObject("LoanTradeManager");
      this.mbsmngr = (IMbsPoolManager) session.GetObject("MbsPoolManager");
    }

    /// <summary>
    /// Get The <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeObject" /> by security trade id
    /// </summary>
    /// <param name="tradeNumber">Security Trade Id</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeObject" /> record for the request.</returns>
    /// <remarks>Return details of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeObject" /> when passing in the identifier of security trade
    /// </remarks>
    public SecurityTradeObject GetSercurityTradeByNumber(string tradeNumber)
    {
      ICursor cursor = this.mngr.OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("SecurityTradeDetails.Name", (Array) new string[1]
        {
          tradeNumber
        }, true)
      }, (SortField[]) null, (string[]) null, true, false);
      return cursor != null && cursor.GetItemCount() > 0 ? new SecurityTradeObject(((SecurityTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))[0]) : (SecurityTradeObject) null;
    }

    /// <summary>
    /// Get The <see cref="T:AssignedLoanTrades[]" /> by security trade id
    /// </summary>
    /// <param name="tradeNumber">Security Trade Id</param>
    /// <returns>Returns the <see cref="T:AssignedLoanTrades[]" /> record for the request.</returns>
    /// <remarks>Return details of <see cref="T:AssignedLoanTrades[]" /> when passing in the identifier of security trade
    /// </remarks>
    public AssignedLoanTrades[] GetAssingedLoanTradesInSecurityTrade(string tradeNumber)
    {
      List<AssignedLoanTrades> assignedLoanTradesList = new List<AssignedLoanTrades>();
      ICursor cursor = this.ltmngr.OpenTradeCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new ListValueCriterion("SecurityTradeDetails.Name", (Array) new string[1]
        {
          tradeNumber
        }, true)
      }, (SortField[]) null, (string[]) null, true, false);
      foreach (LoanTradeViewModel vm in (LoanTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
        assignedLoanTradesList.Add(new AssignedLoanTrades(vm));
      return assignedLoanTradesList.ToArray();
    }

    /// <summary>
    /// Get The <see cref="T:AssignedMBSPools[]" /> by security trade id
    /// </summary>
    /// <param name="tradeNumber">Security Trade Id</param>
    /// <returns>Returns the <see cref="T:AssignedMBSPools[]" /> record for the request.</returns>
    /// <remarks>Return details of <see cref="T:AssignedMBSPools[]" /> when passing in the identifier of security trade
    /// </remarks>
    public AssignedMBSPools[] GetAssingedMBSPoolsInSecurityTrade(string tradeNumber)
    {
      SecurityTradeInfo tradeByName = this.mngr.GetTradeByName(tradeNumber);
      List<AssignedMBSPools> assignedMbsPoolsList = new List<AssignedMBSPools>();
      if (tradeByName == null)
        return assignedMbsPoolsList.ToArray();
      MbsPoolAssignment[] assigmentsBySecurityTrade = this.mbsmngr.GetTradeAssigmentsBySecurityTrade(tradeByName.TradeID);
      int?[] array = ((IEnumerable<MbsPoolAssignment>) assigmentsBySecurityTrade).Select<MbsPoolAssignment, int?>((Func<MbsPoolAssignment, int?>) (m => m.TradeID)).ToArray<int?>();
      if (array != null)
      {
        QueryCriterion[] criteria = new QueryCriterion[array.Length];
        for (int index = 0; index < array.Length; ++index)
          criteria[index] = (QueryCriterion) new OrdinalValueCriterion("MbsPoolDetails.TradeID", (object) array[index]);
        ICursor cursor = this.mbsmngr.OpenTradeCursor(new QueryCriterion[1]
        {
          QueryCriterion.Join(criteria, BinaryOperator.Or)
        }, (SortField[]) null, (string[]) null, true, false);
        foreach (MbsPoolViewModel mbsPoolViewModel in (MbsPoolViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
        {
          MbsPoolViewModel vm = mbsPoolViewModel;
          MbsPoolAssignment mbsPoolAssignment = ((IEnumerable<MbsPoolAssignment>) assigmentsBySecurityTrade).Where<MbsPoolAssignment>((Func<MbsPoolAssignment, bool>) (p =>
          {
            int? tradeId1 = p.TradeID;
            int tradeId2 = vm.TradeID;
            return tradeId1.GetValueOrDefault() == tradeId2 & tradeId1.HasValue;
          })).FirstOrDefault<MbsPoolAssignment>();
          Decimal assignedAmount = mbsPoolAssignment != null ? mbsPoolAssignment.AssignedAmount : 0M;
          AssignedMBSPools assignedMbsPools = new AssignedMBSPools(vm, assignedAmount);
          assignedMbsPoolsList.Add(assignedMbsPools);
        }
      }
      return assignedMbsPoolsList.ToArray();
    }

    /// <summary>
    /// Get a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeObject" /> by security trade status
    /// </summary>
    /// <param name="tradeStatus"><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeStatus" />List</param>
    /// <returns>A list of <see cref="T:System.Collections.Generic.List`1" /></returns>
    /// <remarks>
    /// Example 1: Get all active loan trades
    ///                  SecurityTradeStatus[] statusList = new TradeStatus[4] { SecurityTradeStatus.Open, SecurityTradeStatus.Committed};
    ///                  EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ActiveLoanTrades = session.SecurityTradeService.GetSecurityTradeByStatus(statusList);
    /// Example 2: Get all archived loan trades
    ///                  SecurityTradeStatus[] statusList = new TradeStatus[4] { SecurityTradeStatus.Archived };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ActiveLoanTrades = session.SecurityTradeService.GetSecurityTradeByStatus(statusList);
    /// Example 3: Get all loan trades
    ///                  SecurityTradeStatus[] statusList = new TradeStatus[4] { SecurityTradeStatus.None };
    ///                  EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ActiveLoanTrades = session.SecurityTradeService.GetSecurityTradeByStatus(statusList);
    /// </remarks>
    public List<SecurityTradeObject> GetSecurityTradeByStatus(SecurityTradeStatus[] tradeStatus)
    {
      List<SecurityTradeObject> securityTradeByStatus = new List<SecurityTradeObject>();
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (!((IEnumerable<SecurityTradeStatus>) tradeStatus).Any<SecurityTradeStatus>((Func<SecurityTradeStatus, bool>) (a => a == SecurityTradeStatus.None)))
      {
        int[] valueList = new int[tradeStatus.Length];
        for (int index = 0; index < tradeStatus.Length; ++index)
          valueList[index] = (int) tradeStatus[index];
        queryCriterion = (QueryCriterion) new ListValueCriterion("SecurityTradeDetails.Status", (Array) valueList, true);
      }
      ICursor cursor = this.mngr.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, (string[]) null, true, false);
      if (cursor == null || cursor.GetItemCount() <= 0)
        return securityTradeByStatus;
      foreach (SecurityTradeViewModel info in (SecurityTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
        securityTradeByStatus.Add(new SecurityTradeObject(info));
      return securityTradeByStatus;
    }

    /// <summary>
    /// Create new Security Trade from <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeObject" />
    /// </summary>
    /// <param name="trade"><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeObject" /></param>
    /// <returns>Returns the security Id upon successful creation</returns>
    /// <remarks>Creates new Security Trade</remarks>
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

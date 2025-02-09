// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.MasterContractService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Trading;
using EllieMae.Encompass.Client;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Represents Master Contract Service.</summary>
  public class MasterContractService : SessionBoundObject, IMasterContractService
  {
    private IMasterContractManager mngr;

    /// <summary>Coonstructor to initialize class members</summary>
    /// <param name="session"></param>
    public MasterContractService(Session session)
      : base(session)
    {
      this.mngr = (IMasterContractManager) session.GetObject("MasterContractManager");
    }

    /// <summary>
    /// Get Master Contract by Master Contract Number <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MasterContract" />
    /// </summary>
    /// <param name="masterContractNumber">Master Contract Number</param>
    /// <returns>Master Contract <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MasterContract" /></returns>
    /// <remarks>Return specific <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MasterContract" /> with its information
    /// </remarks>
    public MasterContract GetMasterContractbyMasterContractNumber(string masterContractNumber)
    {
      MasterContractInfo byContractNumber = this.mngr.GetContractByContractNumber(masterContractNumber);
      return byContractNumber == null ? (MasterContract) null : new MasterContract(byContractNumber);
    }

    /// <summary>
    /// Get a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MasterContract" /> by security trade status
    /// </summary>
    /// <param name="tradeStatus"><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MasterCommitmentStatus" />List</param>
    /// <returns>A list of <see cref="T:System.Collections.Generic.List`1" /></returns>
    /// <remarks>
    /// Example 1: Get all active loan trades
    ///                  SecurityTradeStatus[] statusList = new TradeStatus[] { MasterCommitmentStatus.Active};
    ///                  EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ActiveLoanTrades = session.MasterContractService.GetMasterTradeByStatus(statusList);
    /// Example 2: Get all archived loan trades
    ///                  SecurityTradeStatus[] statusList = new TradeStatus[] { MasterCommitmentStatus.Archived };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ActiveLoanTrades = session.MasterContractService.GetMasterTradeByStatus(statusList);
    /// Example 3: Get all loan trades
    ///                  EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ActiveLoanTrades = session.MasterContractService.GetMasterTradeByStatus();
    /// </remarks>
    public List<MasterContract> GetMasterTradeByStatus(MasterCommitmentStatus[] tradeStatus = null)
    {
      List<MasterContract> masterTradeByStatus = new List<MasterContract>();
      int[] source;
      if (tradeStatus == null)
      {
        source = new int[2]{ 0, 1 };
      }
      else
      {
        source = new int[tradeStatus.Length];
        for (int index = 0; index < tradeStatus.Length; ++index)
          source[index] = (int) tradeStatus[index];
      }
      if (((IEnumerable<int>) source).Contains<int>(0))
      {
        foreach (MasterContractSummaryInfo activeContract in this.mngr.GetActiveContracts(true))
          masterTradeByStatus.Add(new MasterContract(activeContract));
      }
      if (((IEnumerable<int>) source).Contains<int>(1))
      {
        foreach (MasterContractSummaryInfo archivedContract in this.mngr.GetArchivedContracts(true))
          masterTradeByStatus.Add(new MasterContract(archivedContract));
      }
      return masterTradeByStatus;
    }
  }
}

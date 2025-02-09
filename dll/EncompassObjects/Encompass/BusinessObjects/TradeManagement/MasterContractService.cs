// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.MasterContractService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Trading;
using EllieMae.Encompass.Client;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class MasterContractService : SessionBoundObject, IMasterContractService
  {
    private IMasterContractManager mngr;

    public MasterContractService(Session session)
      : base(session)
    {
      this.mngr = (IMasterContractManager) session.GetObject("MasterContractManager");
    }

    public MasterContract GetMasterContractbyMasterContractNumber(string masterContractNumber)
    {
      MasterContractInfo byContractNumber = this.mngr.GetContractByContractNumber(masterContractNumber);
      return byContractNumber == null ? (MasterContract) null : new MasterContract(byContractNumber);
    }

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

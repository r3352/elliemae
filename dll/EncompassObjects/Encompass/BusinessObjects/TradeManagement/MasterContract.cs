// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.MasterContract
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class MasterContract
  {
    public string MasterContractNum { get; set; }

    public string InvestorName { get; set; }

    public Decimal ContractAmount { get; set; }

    public DateTime EndDate { get; set; }

    public int NumOfTradesorPools { get; set; }

    public Decimal TotalAssignedAmount { get; internal set; }

    public Decimal CompletionPercent { get; internal set; }

    public string InvestorMasterContractNum { get; set; }

    public DateTime StartDate { get; set; }

    public MasterContractStatus Status { get; set; }

    public MasterContractTerm Term { get; set; }

    public Decimal Tolerance { get; set; }

    public Decimal TotalProfit { get; set; }

    public int ContractId { get; private set; }

    public string StatusText => ((Enum) (object) this.Status).ToDescription();

    public string TermText => ((Enum) (object) this.Term).ToDescription();

    internal MasterContract(MasterContractInfo info)
    {
      this.ContractId = ((MasterContractSummaryInfo) info).ContractID;
      this.MasterContractNum = ((MasterContractSummaryInfo) info).ContractNumber;
      this.InvestorName = ((MasterContractSummaryInfo) info).InvestorName;
      this.ContractAmount = ((MasterContractSummaryInfo) info).ContractAmount;
      this.EndDate = ((MasterContractSummaryInfo) info).EndDate;
      this.NumOfTradesorPools = ((MasterContractSummaryInfo) info).TradeCount;
      this.TotalAssignedAmount = ((MasterContractSummaryInfo) info).AssignedAmount;
      this.CompletionPercent = ((MasterContractSummaryInfo) info).CompletionPercent;
      this.InvestorMasterContractNum = ((MasterContractSummaryInfo) info).InvestorContractNumber;
      this.StartDate = ((MasterContractSummaryInfo) info).StartDate;
      this.Status = (MasterContractStatus) ((MasterContractSummaryInfo) info).Status;
      this.Term = ((MasterContractSummaryInfo) info).Term;
      this.Tolerance = ((MasterContractSummaryInfo) info).Tolerance;
      this.TotalProfit = ((MasterContractSummaryInfo) info).TotalProfit;
    }

    internal MasterContract(MasterContractSummaryInfo info)
    {
      this.ContractId = info.ContractID;
      this.MasterContractNum = info.ContractNumber;
      this.InvestorName = info.InvestorName;
      this.ContractAmount = info.ContractAmount;
      this.EndDate = info.EndDate;
      this.NumOfTradesorPools = info.TradeCount;
      this.TotalAssignedAmount = info.AssignedAmount;
      this.CompletionPercent = info.CompletionPercent;
      this.InvestorMasterContractNum = info.InvestorContractNumber;
      this.StartDate = info.StartDate;
      this.Status = (MasterContractStatus) info.Status;
      this.Term = info.Term;
      this.Tolerance = info.Tolerance;
      this.TotalProfit = info.TotalProfit;
    }
  }
}

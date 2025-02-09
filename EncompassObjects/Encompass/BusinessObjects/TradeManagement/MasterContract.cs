// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.MasterContract
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Represents Master Contract class.</summary>
  public class MasterContract
  {
    /// <summary>Gets the master contract num of master contract</summary>
    public string MasterContractNum { get; set; }

    /// <summary>Getsthe investor name of master contract</summary>
    public string InvestorName { get; set; }

    /// <summary>Gets the contract amount of master contract</summary>
    public Decimal ContractAmount { get; set; }

    /// <summary>Gets the end date of master contract</summary>
    public DateTime EndDate { get; set; }

    /// <summary>Gets the number of trades or pools of master contract</summary>
    public int NumOfTradesorPools { get; set; }

    /// <summary>Gets the total assigned amount of master contract</summary>
    public Decimal TotalAssignedAmount { get; internal set; }

    /// <summary>Gets the completion percdent of master contract</summary>
    public Decimal CompletionPercent { get; internal set; }

    /// <summary>
    /// Gets the investor master contract number of master contract
    /// </summary>
    public string InvestorMasterContractNum { get; set; }

    /// <summary>Gets the start date of master contract</summary>
    public DateTime StartDate { get; set; }

    /// <summary>Gets the master contract status</summary>
    public MasterContractStatus Status { get; set; }

    /// <summary>Gets the master contract term</summary>
    public MasterContractTerm Term { get; set; }

    /// <summary>Gets the tolerance of master contract</summary>
    public Decimal Tolerance { get; set; }

    /// <summary>Gets the total profit of master cosntract</summary>
    public Decimal TotalProfit { get; set; }

    /// <summary>Returns contract id</summary>
    public int ContractId { get; private set; }

    /// <summary>Gets the text for status</summary>
    public string StatusText => this.Status.ToDescription();

    /// <summary>Gets the text for term</summary>
    public string TermText => this.Term.ToDescription();

    /// <summary>Constructor of master contract</summary>
    internal MasterContract(MasterContractInfo info)
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

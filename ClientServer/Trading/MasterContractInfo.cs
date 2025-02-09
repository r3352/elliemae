// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MasterContractInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class MasterContractInfo : MasterContractSummaryInfo
  {
    private Investor investor;

    public MasterContractInfo() => this.investor = new Investor();

    public MasterContractInfo(
      int contractId,
      string contractNumber,
      int status,
      string investorName,
      string investorContractNumber,
      MasterContractTerm term,
      Decimal contractAmount,
      Decimal tolerance,
      DateTime startDate,
      DateTime endDate,
      int tradeCount,
      Decimal assignedAmount,
      Decimal totalProfit,
      string investorXml,
      Decimal completionPercent)
      : base(contractId, contractNumber, status, investorName, investorContractNumber, term, contractAmount, tolerance, startDate, endDate, tradeCount, assignedAmount, totalProfit, completionPercent)
    {
      this.investor = BinaryConvertible<Investor>.Parse(investorXml);
      if (this.investor != null)
        return;
      this.investor = new Investor();
    }

    private MasterContractInfo(MasterContractInfo source)
      : base(source)
    {
      this.investor = new Investor(source.investor);
    }

    public Investor Investor
    {
      get => this.investor;
      set
      {
        this.investor = value != null ? value : throw new ArgumentNullException("Investor cannot be null");
      }
    }

    public override string InvestorName => this.investor.Name;

    public MasterContractInfo Duplicate() => new MasterContractInfo(this);

    public override string ToString() => this.ContractNumber;
  }
}

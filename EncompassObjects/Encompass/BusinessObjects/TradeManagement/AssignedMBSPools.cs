// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.AssignedMBSPools
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Assinged Loan Trades</summary>
  public class AssignedMBSPools
  {
    private Decimal allocatedPoolAmount;
    private DateTime commitmentDate;
    private string poolId;
    private string poolNumber;
    private Decimal poolAmount;
    private string amortizationType;
    private int term;
    private string masterNumber;
    private Decimal assignedAmount;

    /// <summary>Gets Allocated Pool Amount</summary>
    public Decimal AllocatedPoolAmount => this.allocatedPoolAmount;

    /// <summary>Gets Commitment Date</summary>
    public DateTime CommitmentDate => this.commitmentDate;

    /// <summary>Gets Pool Id</summary>
    public string PoolId => this.poolId;

    /// <summary>Gets Pool number</summary>
    public string PoolNumber => this.poolNumber;

    /// <summary>Gets Pool Amount</summary>
    public Decimal PoolAmount => this.poolAmount;

    /// <summary>Gets Amortizaton Type</summary>
    public string AmortizationType => this.amortizationType;

    /// <summary>Gets Term</summary>
    public int Term => this.term;

    /// <summary>Gets Net profit</summary>
    public string MasterNumber => this.masterNumber;

    /// <summary>Gets Net profit</summary>
    public Decimal AssignedAmount => this.assignedAmount;

    internal AssignedMBSPools(MbsPoolViewModel vm, Decimal allocatedPoolAmount)
    {
      this.allocatedPoolAmount = allocatedPoolAmount;
      this.amortizationType = vm.AmortizationType;
      this.assignedAmount = vm.AssignedAmount;
      this.commitmentDate = vm.CommitmentDate;
      this.masterNumber = vm.ContractNumber;
      this.poolAmount = vm.TradeAmount;
      this.poolId = vm.Name;
      this.poolNumber = vm.PoolNumber;
      this.term = vm.Term;
    }
  }
}

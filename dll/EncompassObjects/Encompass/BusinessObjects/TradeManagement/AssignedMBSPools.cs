// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.AssignedMBSPools
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
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

    public Decimal AllocatedPoolAmount => this.allocatedPoolAmount;

    public DateTime CommitmentDate => this.commitmentDate;

    public string PoolId => this.poolId;

    public string PoolNumber => this.poolNumber;

    public Decimal PoolAmount => this.poolAmount;

    public string AmortizationType => this.amortizationType;

    public int Term => this.term;

    public string MasterNumber => this.masterNumber;

    public Decimal AssignedAmount => this.assignedAmount;

    internal AssignedMBSPools(MbsPoolViewModel vm, Decimal allocatedPoolAmount)
    {
      this.allocatedPoolAmount = allocatedPoolAmount;
      this.amortizationType = vm.AmortizationType;
      this.assignedAmount = ((TradeViewModel) vm).AssignedAmount;
      this.commitmentDate = vm.CommitmentDate;
      this.masterNumber = vm.ContractNumber;
      this.poolAmount = ((TradeViewModel) vm).TradeAmount;
      this.poolId = ((TradeBase) vm).Name;
      this.poolNumber = vm.PoolNumber;
      this.term = vm.Term;
    }
  }
}

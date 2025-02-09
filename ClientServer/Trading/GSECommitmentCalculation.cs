// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GSECommitmentCalculation
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class GSECommitmentCalculation
  {
    public static Decimal CalculatePairOffAmount(GSECommitmentPairOffs pairOffs)
    {
      Decimal num = 0M;
      foreach (GSECommitmentPairOff pairOff in pairOffs)
        num += pairOff.TradeAmount;
      return !(num > 0M) ? num : num * -1M;
    }

    public static Decimal CalculateCompletionPercentage(Decimal tradeAmount, Decimal openAmount)
    {
      return tradeAmount != 0M ? (tradeAmount - openAmount) / tradeAmount * 100M : 0M;
    }

    public static Decimal CalculateOutstandingBalance(
      Decimal tradeAmount,
      Decimal pairOffAmount,
      Decimal allocatedPoolAmount)
    {
      return tradeAmount + pairOffAmount - allocatedPoolAmount;
    }

    public static Decimal CalculateAllocatedAmount(GSECommitmentAssignment[] assignments)
    {
      Decimal num = 0M;
      return assignments == null ? num : ((IEnumerable<GSECommitmentAssignment>) assignments).Sum<GSECommitmentAssignment>((Func<GSECommitmentAssignment, Decimal>) (a => a.AssignedAmount));
    }

    public static Decimal CalculateOpenAmount(
      Decimal tradeAmount,
      GSECommitmentAssignment[] assignments)
    {
      Decimal allocatedAmount = GSECommitmentCalculation.CalculateAllocatedAmount(assignments);
      return GSECommitmentCalculation.CalculateOpenAmount(tradeAmount, allocatedAmount);
    }

    public static Decimal CalculateOpenAmount(Decimal tradeAmount, Decimal assignedAmount)
    {
      return tradeAmount - assignedAmount;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentMasterCalculation
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
  public class CorrespondentMasterCalculation : TradeCalculation
  {
    internal CorrespondentMasterCalculation(ITradeInfoObject masterInfo)
      : base(masterInfo)
    {
    }

    private CorrespondentMasterInfo MasterInfo
    {
      get
      {
        return this.Trade != null ? (CorrespondentMasterInfo) this.Trade : (CorrespondentMasterInfo) null;
      }
    }

    public static Decimal CalculateAvailableAmountForCmc(
      Decimal commitmentAmount,
      bool isCommitmentUseBestEffortLimited,
      List<CorrespondentTradeInfo> trades)
    {
      Dictionary<CorrespondentMasterDeliveryType, Decimal> standingCommitments = CorrespondentTradeCalculation.CalculateOutStandingCommitments(trades, isCommitmentUseBestEffortLimited);
      return commitmentAmount - standingCommitments.Sum<KeyValuePair<CorrespondentMasterDeliveryType, Decimal>>((Func<KeyValuePair<CorrespondentMasterDeliveryType, Decimal>, Decimal>) (a => a.Value));
    }
  }
}

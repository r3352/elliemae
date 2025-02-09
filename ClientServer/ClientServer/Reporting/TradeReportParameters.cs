// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.TradeReportParameters
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class TradeReportParameters : ReportParameters
  {
    private TradeType tradeType = TradeType.LoanTrade;

    public TradeType TradeType
    {
      get => this.tradeType;
      set => this.tradeType = value;
    }

    public QueryCriterion CreateCombinedFilter(FieldFilterList newFilterList)
    {
      return newFilterList.CreateEvaluator().ToQueryCriterion();
    }
  }
}

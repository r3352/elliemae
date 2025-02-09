// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradePairOffHistoryItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class CorrespondentTradePairOffHistoryItem : TradeHistoryItemBase
  {
    private XmlDictionary<string> priorTradeValues;

    public XmlDictionary<string> PriorTradeValues => this.priorTradeValues;

    public CorrespondentTradePairOffHistoryItem(
      CorrespondentTradePairOff pairOff,
      int tradeId,
      TradeHistoryAction action,
      int status,
      UserInfo user,
      CorrespondentTradePairOff priorTrade = null)
    {
      this.Action = action;
      this.Status = status;
      this.TradeID = tradeId;
      this.Data["PairOffDate"] = pairOff.Date.ToString("MM/dd/yyyy");
      this.Data["RequestedBy"] = pairOff.RequestedBy;
      this.Data["TradeAmount"] = pairOff.DisplayedTradeAmount.ToString("#,0.00#");
      this.Data["PairOffFee"] = pairOff.PairOffFeePercentage.ToString("N5");
      this.Data["GainLoss"] = pairOff.DisplayCalculatedPairOffFee.ToString("#,0.00#");
      this.Data["Comments"] = pairOff.Comments;
      if (priorTrade != null)
      {
        this.priorTradeValues = new XmlDictionary<string>();
        this.priorTradeValues["PairOffDate"] = priorTrade.Date.ToString("MM/dd/yyyy");
        this.priorTradeValues["RequestedBy"] = priorTrade.RequestedBy;
        this.priorTradeValues["TradeAmount"] = priorTrade.DisplayedTradeAmount.ToString("#,0.00#");
        this.priorTradeValues["PairOffFee"] = priorTrade.PairOffFeePercentage.ToString("N5");
        this.priorTradeValues["GainLoss"] = priorTrade.DisplayCalculatedPairOffFee.ToString("#,0.00#");
        this.priorTradeValues["Comments"] = priorTrade.Comments;
      }
      if (!(user != (UserInfo) null))
        return;
      this.UserID = user.Userid;
      this.Data["UserName"] = user.FullName;
    }
  }
}

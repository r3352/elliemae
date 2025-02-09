// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeAssignment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradeAssignment
  {
    public int TradeID { get; set; }

    public TradeType TradeType { get; set; }

    public List<string> SkipFieldList { get; set; }

    public string LoanSyncOption { get; set; }

    public string FinalStatus { get; set; }

    public string TradeExtensionInfo { get; set; }

    public string SessionId { get; set; }

    [ScriptIgnore]
    public List<TradeAssignmentItem> TradeAssignmentItems { get; set; }

    public TradeAssignment()
    {
    }

    public TradeAssignment(int tradeID, TradeType tradeType, List<string> skipFieldList)
    {
      this.TradeID = tradeID;
      this.TradeType = tradeType;
      this.SkipFieldList = skipFieldList;
    }

    public void AddTradeAssignmentItem(TradeAssignmentItem item)
    {
      if (this.TradeAssignmentItems == null)
        this.TradeAssignmentItems = new List<TradeAssignmentItem>();
      this.TradeAssignmentItems.Add(item);
    }

    public BatchJobEntityType BatchJobEntityType
    {
      get
      {
        switch (this.TradeType)
        {
          case TradeType.LoanTrade:
            return BatchJobEntityType.LoanTrade;
          case TradeType.MbsPool:
            return BatchJobEntityType.MBSPool;
          case TradeType.CorrespondentTrade:
            return BatchJobEntityType.CorrespondentTrade;
          default:
            return BatchJobEntityType.None;
        }
      }
    }

    public static string SerializeToJSON(TradeAssignment assigment)
    {
      return new JavaScriptSerializer().Serialize((object) assigment);
    }
  }
}

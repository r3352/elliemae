// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeAssignmentItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Web.Script.Serialization;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradeAssignmentItem
  {
    public string EntityId { get; set; }

    public string LoanNumber { get; set; }

    public BatchJobItemEntityType Type { get; set; }

    public BatchJobItemAction Action { get; set; }

    public string CommitmentContractNumber { get; set; }

    public Decimal CPA { get; set; }

    public string ProductName { get; set; }

    public Decimal TotalPrice { get; set; }

    public string AssignedStatus { get; set; }

    public string InitialPendingStatus { get; set; }

    public string PendingStatus { get; set; }

    public bool Rejected { get; set; }

    public TradeAssignmentItem()
    {
    }

    public TradeAssignmentItem(string jsonString)
    {
      TradeAssignmentItem tradeAssignmentItem = new JavaScriptSerializer().Deserialize<TradeAssignmentItem>(jsonString);
      this.EntityId = tradeAssignmentItem.EntityId;
      this.Type = tradeAssignmentItem.Type;
      this.Action = tradeAssignmentItem.Action;
      this.CommitmentContractNumber = tradeAssignmentItem.CommitmentContractNumber;
      this.ProductName = tradeAssignmentItem.ProductName;
      this.TotalPrice = tradeAssignmentItem.TotalPrice;
      this.CPA = tradeAssignmentItem.CPA;
      this.AssignedStatus = tradeAssignmentItem.AssignedStatus;
      this.InitialPendingStatus = tradeAssignmentItem.InitialPendingStatus;
      this.PendingStatus = tradeAssignmentItem.PendingStatus;
      this.Rejected = tradeAssignmentItem.Rejected;
      this.LoanNumber = tradeAssignmentItem.LoanNumber;
    }

    public static string SerializeToJSON(TradeAssignmentItem item)
    {
      return new JavaScriptSerializer().Serialize((object) item);
    }
  }
}

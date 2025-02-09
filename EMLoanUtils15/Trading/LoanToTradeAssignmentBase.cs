// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanToTradeAssignmentBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class LoanToTradeAssignmentBase
  {
    protected SessionObjects sessionObjects;
    protected int tradeId;
    protected PipelineInfo pinfo;
    private string rejectedReason = string.Empty;

    internal LoanToTradeAssignmentBase(
      SessionObjects sessionObjects,
      int tradeId,
      PipelineInfo pinfo)
    {
      this.sessionObjects = sessionObjects;
      this.tradeId = tradeId;
      this.pinfo = pinfo;
    }

    public string Guid => this.pinfo.GUID;

    public int TradeId => this.tradeId;

    public PipelineInfo PipelineInfo
    {
      get => this.pinfo;
      set => this.pinfo = value;
    }

    protected bool rejected { get; set; }

    public string RejectedReason
    {
      get => this.rejected ? this.rejectedReason : "";
      set => this.rejectedReason = value;
    }
  }
}

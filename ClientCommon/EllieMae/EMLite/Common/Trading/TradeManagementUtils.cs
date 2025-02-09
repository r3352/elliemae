// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Trading.TradeManagementUtils
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Common.Trading
{
  public class TradeManagementUtils
  {
    private const string className = "TradeManagementUtils";
    private static readonly string sw = Tracing.SwCommon;

    public static void SyncLoanTradeData(string loanGuid, bool isUpdateStatus = true)
    {
      LoanTradeInfo tradeForLoan = Session.LoanTradeManager.GetTradeForLoan(loanGuid);
      if (tradeForLoan == null)
        return;
      Tracing.Log(TradeManagementUtils.sw, TraceLevel.Verbose, nameof (TradeManagementUtils), "Creating Thread");
      ThreadPool.QueueUserWorkItem(new WaitCallback(TradeManagementUtils.syncLoanTrade), (object) new object[2]
      {
        (object) tradeForLoan.TradeID,
        (object) isUpdateStatus
      });
    }

    private static void syncLoanTrade(object parameter)
    {
      object[] objArray = parameter as object[];
      int tradeId = Utils.ParseInt(objArray[0]);
      bool isUpdateStatus = true;
      if (objArray != null && objArray.Length == 2)
        isUpdateStatus = Utils.ParseBoolean(objArray[1]);
      LoanTradeInfo trade = Session.LoanTradeManager.GetTrade(tradeId);
      TradeAssignmentManager assignmentManager = new TradeAssignmentManager(Session.SessionObjects, trade.TradeID, false);
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
      PipelineInfo[] pipeline = Session.LoanManager.GetPipeline(assignmentManager.GetAllLoanGuids(), trade.GetPricingFields(), PipelineData.Trade, false);
      trade.ResetCalculatedField(pipeline);
      try
      {
        Session.LoanTradeManager.UpdateTrade(trade, true, isUpdateStatus);
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeManagementUtils.sw, TraceLevel.Error, nameof (TradeManagementUtils), "Error in syncLoanTrade(tradeID): " + ex.Message);
      }
    }
  }
}

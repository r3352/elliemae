// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TradeSynchronization.TradeLoanUpdateUtils
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace EllieMae.EMLite.ClientServer.TradeSynchronization
{
  public class TradeLoanUpdateUtils
  {
    public static void UnlockTradesCancelBatchJobs(
      SessionObjects sessionObject,
      UserInfo user,
      List<TradeUnlockInfo> tradeBatchJobs)
    {
      IEnumerable<int> source = tradeBatchJobs.Select<TradeUnlockInfo, int>((Func<TradeUnlockInfo, int>) (x => x.batchJobId)).Distinct<int>();
      TradeLoanUpdateUtils.UpdateTradeStatus(sessionObject, tradeBatchJobs);
      sessionObject.BatchJobsManager.CancelBatchJobs(source.ToArray<int>(), "Encompass", user.Userid, DateTime.UtcNow);
      TradeLoanUpdateUtils.AddTradeHistory(sessionObject, tradeBatchJobs);
    }

    private static void UpdateTradeStatus(
      SessionObjects sessionObject,
      List<TradeUnlockInfo> tradeBatchJobs)
    {
      int[] tradeIdsByType1 = TradeLoanUpdateUtils.GetTradeIdsByType(tradeBatchJobs, TradeType.LoanTrade);
      if (((IEnumerable<int>) tradeIdsByType1).Count<int>() > 0)
        sessionObject.LoanTradeManager.SetTradeStatus(((IEnumerable<int>) tradeIdsByType1).ToArray<int>(), TradeStatus.Open, TradeHistoryAction.UnlockPendingTrade, false);
      int[] tradeIdsByType2 = TradeLoanUpdateUtils.GetTradeIdsByType(tradeBatchJobs, TradeType.MbsPool);
      if (((IEnumerable<int>) tradeIdsByType2).Count<int>() > 0)
        sessionObject.MbsPoolManager.SetTradeStatus(((IEnumerable<int>) tradeIdsByType2).ToArray<int>(), TradeStatus.Open, TradeHistoryAction.UnlockPendingTrade, false);
      if (((IEnumerable<int>) TradeLoanUpdateUtils.GetTradeIdsByType(tradeBatchJobs, TradeType.CorrespondentTrade)).Count<int>() <= 0)
        return;
      Dictionary<TradeStatus, List<int>> dictionary = new Dictionary<TradeStatus, List<int>>();
      foreach (TradeUnlockInfo tradeBatchJob in tradeBatchJobs)
      {
        if (!string.IsNullOrEmpty(tradeBatchJob.jobMetaData))
        {
          TradeStatus tradeStatus = TradeLoanUpdateUtils.GetTradeStatus(new CorrespondentTradeAssignment(tradeBatchJob.jobMetaData).FinalStatus);
          if (tradeStatus != TradeStatus.None)
          {
            if (!dictionary.ContainsKey(tradeStatus))
              dictionary.Add(tradeStatus, new List<int>());
            if (!dictionary[tradeStatus].Contains(tradeBatchJob.tradeInfo.TradeID))
              dictionary[tradeStatus].Add(tradeBatchJob.tradeInfo.TradeID);
          }
        }
      }
      foreach (KeyValuePair<TradeStatus, List<int>> keyValuePair in dictionary)
        sessionObject.CorrespondentTradeManager.SetTradeStatus(keyValuePair.Value.ToArray(), keyValuePair.Key, TradeHistoryAction.UnlockPendingTrade, false);
    }

    private static TradeStatus GetTradeStatus(string status)
    {
      try
      {
        return (TradeStatus) Enum.Parse(typeof (TradeStatus), status);
      }
      catch (Exception ex)
      {
        return TradeStatus.None;
      }
    }

    private static int[] GetTradeIdsByType(
      List<TradeUnlockInfo> tradeBatchJobs,
      TradeType tradeType)
    {
      IEnumerable<int> source = (IEnumerable<int>) new List<int>();
      if (tradeBatchJobs != null && tradeBatchJobs.Count > 0)
        source = tradeBatchJobs.Where<TradeUnlockInfo>((Func<TradeUnlockInfo, bool>) (x => x.tradeInfo.TradeType == tradeType)).Select<TradeUnlockInfo, TradeInfoObj>((Func<TradeUnlockInfo, TradeInfoObj>) (x => x.tradeInfo)).Select<TradeInfoObj, int>((Func<TradeInfoObj, int>) (y => y.TradeID)).Distinct<int>();
      return source.ToArray<int>();
    }

    private static void AddTradeHistory(
      SessionObjects sessionObject,
      List<TradeUnlockInfo> batchJobs)
    {
      if (batchJobs == null || batchJobs.Count <= 0)
        return;
      IEnumerable<int> source = batchJobs.Select<TradeUnlockInfo, int>((Func<TradeUnlockInfo, int>) (x => x.batchJobId)).Distinct<int>();
      foreach (BatchJobSummaryInfo batchJob in sessionObject.BatchJobsManager.GetBatchJobs(source.ToArray<int>()))
      {
        BatchJobSummaryInfo job = batchJob;
        TradeUnlockInfo tradeUnlockInfo = batchJobs.Where<TradeUnlockInfo>((Func<TradeUnlockInfo, bool>) (x => x.batchJobId == job.BatchJobId)).FirstOrDefault<TradeUnlockInfo>();
        string message = string.Format("Processed: {0}, Successful: {1}, Errors: {2}, Cancelled: {3}. Trade was unlocked and batch job was cancelled from Encompass 'Unlock Trade' by {4}", (object) job.TotalJobItemsCount, (object) job.TotalJobItemsSucceeded, (object) job.TotalJobItemsErrored, (object) job.TotalJobItemsCancelled, (object) sessionObject.UserID);
        TradeLoanUpdateUtils.AddTradeHistoryItem(tradeUnlockInfo.tradeInfo.TradeID, tradeUnlockInfo.tradeInfo.TradeType, TradeLoanUpdateUtils.ConvertTradeAction(job.Status), message, sessionObject);
      }
    }

    private static TradeHistoryAction ConvertTradeAction(BatchJobStatus jobStatus)
    {
      TradeHistoryAction tradeHistoryAction = TradeHistoryAction.LoanUpdateErrors;
      switch (jobStatus)
      {
        case BatchJobStatus.Completed:
          tradeHistoryAction = TradeHistoryAction.LoanUpdateCompleted;
          break;
        case BatchJobStatus.Cancelled:
          tradeHistoryAction = TradeHistoryAction.LoanUpdateCancelled;
          break;
        case BatchJobStatus.Error:
          tradeHistoryAction = TradeHistoryAction.LoanUpdateErrors;
          break;
        case BatchJobStatus.Deleted:
          tradeHistoryAction = TradeHistoryAction.LoanUpdateCleared;
          break;
        case BatchJobStatus.CompletedWithError:
          tradeHistoryAction = TradeHistoryAction.LoanUpdatePending;
          break;
      }
      return tradeHistoryAction;
    }

    public static void AddTradeHistoryItem(
      int tradeId,
      TradeType tradeType,
      TradeHistoryAction tradeHistoryAction,
      string message,
      SessionObjects sessionObject)
    {
      switch (tradeType)
      {
        case TradeType.LoanTrade:
          LoanTradeInfo trade1 = new LoanTradeInfo();
          trade1.TradeID = tradeId;
          LoanTradeHistoryItem tradeHistoryItem1 = new LoanTradeHistoryItem(trade1, tradeHistoryAction, message, sessionObject.UserInfo);
          sessionObject.LoanTradeManager.AddTradeHistoryItem(tradeHistoryItem1);
          break;
        case TradeType.MbsPool:
          MbsPoolInfo trade2 = new MbsPoolInfo();
          trade2.TradeID = tradeId;
          MbsPoolHistoryItem mbsPoolHistoryItem = new MbsPoolHistoryItem(trade2, tradeHistoryAction, message, sessionObject.UserInfo);
          sessionObject.MbsPoolManager.AddTradeHistoryItem(mbsPoolHistoryItem);
          break;
        case TradeType.CorrespondentTrade:
          CorrespondentTradeInfo trade3 = new CorrespondentTradeInfo();
          trade3.TradeID = tradeId;
          CorrespondentTradeHistoryItem tradeHistoryItem2 = new CorrespondentTradeHistoryItem(trade3, tradeHistoryAction, message, sessionObject.UserInfo);
          sessionObject.CorrespondentTradeManager.AddTradeHistoryItem(tradeHistoryItem2);
          break;
        default:
          throw new NotImplementedException();
      }
    }

    public static SRPTable BuildSrpTable(object srpTable)
    {
      SRPTable.SRPPricingItems source = new SRPTable.SRPPricingItems();
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IEnumerable> target1 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__27.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IEnumerable>> p27 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__27;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PricingItems", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__0, srpTable);
      foreach (object obj2 in target1((CallSite) p27, obj1))
      {
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__7 = CallSite<Func<CallSite, Type, object, object, Range<Decimal>>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object, Range<Decimal>> target2 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__7.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object, Range<Decimal>>> p7 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__7;
        Type type1 = typeof (Range<Decimal>);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__3 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target3 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p3 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__3;
        Type type2 = typeof (Convert);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Minimum", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target4 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p2 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LoanAmount", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__1.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__1, obj2);
        object obj4 = target4((CallSite) p2, obj3);
        object obj5 = target3((CallSite) p3, type2, obj4);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__6 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target5 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p6 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__6;
        Type type3 = typeof (Convert);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Maximum", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target6 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p5 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LoanAmount", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__4.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__4, obj2);
        object obj7 = target6((CallSite) p5, obj6);
        object obj8 = target5((CallSite) p6, type3, obj7);
        Range<Decimal> loanAmount = target2((CallSite) p7, type1, obj5, obj8);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (TradeLoanUpdateUtils)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Decimal> target7 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Decimal>> p9 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__9;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BaseAdjustment", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj9 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__8.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__8, obj2);
        Decimal baseAdjustment = target7((CallSite) p9, obj9);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (TradeLoanUpdateUtils)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Decimal> target8 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__11.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Decimal>> p11 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__11;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ImpoundsAdjustment", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj10 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__10.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__10, obj2);
        Decimal impoundsAdjustment = target8((CallSite) p11, obj10);
        SRPTable.SRPStateAdjustments stateAdjustments1 = new SRPTable.SRPStateAdjustments();
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__23 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (TradeLoanUpdateUtils)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, IEnumerable> target9 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__23.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, IEnumerable>> p23 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__23;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "StateAdjustments", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj11 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__12.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__12, obj2);
        foreach (object obj12 in target9((CallSite) p23, obj11))
        {
          SRPTable.SRPStateAdjustments stateAdjustments2 = stateAdjustments1;
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__22 = CallSite<Func<CallSite, Type, Guid, object, object, object, SRPTable.StateAdjustment>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, Guid, object, object, object, SRPTable.StateAdjustment> target10 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__22.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, Guid, object, object, object, SRPTable.StateAdjustment>> p22 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__22;
          Type type4 = typeof (SRPTable.StateAdjustment);
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__15 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__15 = CallSite<Func<CallSite, Type, object, Guid>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, Guid> target11 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__15.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, Guid>> p15 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__15;
          Type type5 = typeof (Guid);
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__14 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target12 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__14.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p14 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__14;
          Type type6 = typeof (Convert);
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Id", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj13 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__13.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__13, obj12);
          object obj14 = target12((CallSite) p14, type6, obj13);
          Guid guid = target11((CallSite) p15, type5, obj14);
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__17 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target13 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__17.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p17 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__17;
          Type type7 = typeof (Convert);
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "State", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj15 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__16.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__16, obj12);
          object obj16 = target13((CallSite) p17, type7, obj15);
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__19 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target14 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__19.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p19 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__19;
          Type type8 = typeof (Convert);
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Adjustment", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj17 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__18.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__18, obj12);
          object obj18 = target14((CallSite) p19, type8, obj17);
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__21 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target15 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__21.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p21 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__21;
          Type type9 = typeof (Convert);
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ImpoundAdjustment", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj19 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__20.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__20, obj12);
          object obj20 = target15((CallSite) p21, type9, obj19);
          SRPTable.StateAdjustment adj = target10((CallSite) p22, type4, guid, obj16, obj18, obj20);
          stateAdjustments2.Add(adj);
        }
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__26 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__26 = CallSite<Func<CallSite, Type, object, Guid>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, Guid> target16 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__26.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, Guid>> p26 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__26;
        Type type10 = typeof (Guid);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__25 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__25 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target17 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__25.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p25 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__25;
        Type type11 = typeof (Convert);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__24 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Id", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj21 = TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__24.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__7.\u003C\u003Ep__24, obj2);
        object obj22 = target17((CallSite) p25, type11, obj21);
        SRPTable.PricingItem pricingItem = new SRPTable.PricingItem(target16((CallSite) p26, type10, obj22), loanAmount, baseAdjustment, impoundsAdjustment, stateAdjustments1);
        source.Add(pricingItem);
      }
      return new SRPTable(source);
    }

    public static TradePriceAdjustments BuildPriceAdjustments(object priceAdjustments)
    {
      TradePriceAdjustments priceAdjustments1 = new TradePriceAdjustments();
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (object obj1 in TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__3.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__3, priceAdjustments))
      {
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__2 = CallSite<Action<CallSite, TradePriceAdjustments, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, TradePriceAdjustments, object> target1 = TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, TradePriceAdjustments, object>> p2 = TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__2;
        TradePriceAdjustments priceAdjustments2 = priceAdjustments1;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<Type>) new Type[1]
          {
            typeof (TradePriceAdjustment)
          }, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target2 = TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p1 = TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__1;
        Type type = typeof (JsonConvert);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__8.\u003C\u003Ep__0, typeof (JsonConvert), obj1);
        object obj3 = target2((CallSite) p1, type, obj2);
        target1((CallSite) p2, priceAdjustments2, obj3);
      }
      return priceAdjustments1;
    }

    public static Investor BuildInvestors(object investor)
    {
      Investor investor1 = new Investor();
      Investor investor2 = investor1;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target1 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p1 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__0.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__0, investor);
      string str1 = target1((CallSite) p1, obj1);
      investor2.Name = str1;
      Hashtable hashtable = new Hashtable();
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IEnumerable> target2 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IEnumerable>> p5 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__4 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<Type>) new Type[1]
        {
          typeof (Hashtable)
        }, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target3 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p4 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__4;
      Type type1 = typeof (JsonConvert);
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__3 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target4 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p3 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__3;
      Type type2 = typeof (JsonConvert);
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ContactInformations", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__2.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__2, investor);
      object obj3 = target4((CallSite) p3, type2, obj2);
      object obj4 = target3((CallSite) p4, type1, obj3);
      foreach (object obj5 in target2((CallSite) p5, obj4))
      {
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, DictionaryEntry>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (DictionaryEntry), typeof (TradeLoanUpdateUtils)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DictionaryEntry dictionaryEntry = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__6.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__6, obj5);
        hashtable.Add(dictionaryEntry.Key, (object) JsonConvert.DeserializeObject<ContactInformation>(JsonConvert.SerializeObject(dictionaryEntry.Value)));
      }
      investor1.ContactInformations = hashtable;
      Investor investor3 = investor1;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, ContactInformation>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (ContactInformation), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, ContactInformation> target5 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, ContactInformation>> p10 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__9 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<Type>) new Type[1]
        {
          typeof (ContactInformation)
        }, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target6 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p9 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__9;
      Type type3 = typeof (JsonConvert);
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__8 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target7 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p8 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__8;
      Type type4 = typeof (JsonConvert);
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ShippingInformation", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__7.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__7, investor);
      object obj7 = target7((CallSite) p8, type4, obj6);
      object obj8 = target6((CallSite) p9, type3, obj7);
      ContactInformation contactInformation = target5((CallSite) p10, obj8);
      investor3.ShippingInformation = contactInformation;
      Investor investor4 = investor1;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target8 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__12.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p12 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__12;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "DeliveryTimeFrame", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj9 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__11.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__11, investor);
      int num1 = target8((CallSite) p12, obj9);
      investor4.DeliveryTimeFrame = num1;
      Investor investor5 = investor1;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target9 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__14.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p14 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__14;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PairOffFee", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__13.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__13, investor);
      Decimal num2 = target9((CallSite) p14, obj10);
      investor5.PairOffFee = num2;
      Investor investor6 = investor1;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target10 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__16.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p16 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__16;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TypeOfPurchaser", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj11 = TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__15.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__9.\u003C\u003Ep__15, investor);
      string str2 = target10((CallSite) p16, obj11);
      investor6.TypeOfPurchaser = str2;
      return investor1;
    }

    public static TradePricingInfo BuildPricingInfo(object pricing)
    {
      TradePricingInfo tradePricingInfo1 = new TradePricingInfo();
      TradePricingInfo tradePricingInfo2 = tradePricingInfo1;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IsAdvancedPricing", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__0, pricing);
      int num1 = target1((CallSite) p1, obj1) ? 1 : 0;
      tradePricingInfo2.IsAdvancedPricing = num1 != 0;
      TradePricingItems tradePricingItems1 = new TradePricingItems();
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IEnumerable> target2 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IEnumerable>> p6 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SimplePricingItems", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__2.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__2, pricing);
      foreach (object obj3 in target2((CallSite) p6, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, TradePricingItem>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TradePricingItem), typeof (TradeLoanUpdateUtils)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, TradePricingItem> target3 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, TradePricingItem>> p5 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__4 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<Type>) new Type[1]
          {
            typeof (TradePricingItem)
          }, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target4 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p4 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__4;
        Type type = typeof (JsonConvert);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__3 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__3.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__3, typeof (JsonConvert), obj3);
        object obj5 = target4((CallSite) p4, type, obj4);
        TradePricingItem tradePricingItem = target3((CallSite) p5, obj5);
        tradePricingItems1.Add(tradePricingItem);
      }
      tradePricingInfo1.SimplePricingItems = tradePricingItems1;
      TradePricingItems tradePricingItems2 = new TradePricingItems();
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IEnumerable> target5 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IEnumerable>> p11 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MSRPricingItems", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__7.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__7, pricing);
      foreach (object obj7 in target5((CallSite) p11, obj6))
      {
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, TradePricingItem>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TradePricingItem), typeof (TradeLoanUpdateUtils)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, TradePricingItem> target6 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__10.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, TradePricingItem>> p10 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__10;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__9 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<Type>) new Type[1]
          {
            typeof (TradePricingItem)
          }, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target7 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p9 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__9;
        Type type = typeof (JsonConvert);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__8 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj8 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__8.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__8, typeof (JsonConvert), obj7);
        object obj9 = target7((CallSite) p9, type, obj8);
        TradePricingItem tradePricingItem = target6((CallSite) p10, obj9);
        tradePricingItems2.Add(tradePricingItem);
      }
      tradePricingInfo1.MSRPricingItems = tradePricingItems2;
      TradeAdvancedPricingInfo advancedPricingInfo1 = tradePricingInfo1.AdvancedPricingInfo;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target8 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__14.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p14 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__14;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Coupon", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target9 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p13 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AdvancedPricingInfo", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__12.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__12, pricing);
      object obj11 = target9((CallSite) p13, obj10);
      Decimal num2 = target8((CallSite) p14, obj11);
      advancedPricingInfo1.Coupon = num2;
      TradeAdvancedPricingInfo advancedPricingInfo2 = tradePricingInfo1.AdvancedPricingInfo;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target10 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__17.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p17 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__17;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "GuaranteeFee", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target11 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__16.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p16 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__16;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AdvancedPricingInfo", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj12 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__15.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__15, pricing);
      object obj13 = target11((CallSite) p16, obj12);
      Decimal num3 = target10((CallSite) p17, obj13);
      advancedPricingInfo2.GuaranteeFee = num3;
      TradeAdvancedPricingInfo advancedPricingInfo3 = tradePricingInfo1.AdvancedPricingInfo;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target12 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__20.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p20 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__20;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ServiceFee", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target13 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__19.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p19 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__19;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AdvancedPricingInfo", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj14 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__18.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__18, pricing);
      object obj15 = target13((CallSite) p19, obj14);
      Decimal num4 = target12((CallSite) p20, obj15);
      advancedPricingInfo3.ServiceFee = num4;
      TradeAdvancedPricingInfo advancedPricingInfo4 = tradePricingInfo1.AdvancedPricingInfo;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__23 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target14 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__23.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p23 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__23;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__22 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Price", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target15 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__22.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p22 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__22;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__21 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AdvancedPricingInfo", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj16 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__21.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__21, pricing);
      object obj17 = target15((CallSite) p22, obj16);
      Decimal num5 = target14((CallSite) p23, obj17);
      advancedPricingInfo4.Price = num5;
      TradeAdvancedPricingInfo advancedPricingInfo5 = tradePricingInfo1.AdvancedPricingInfo;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target16 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__26.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p26 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__26;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "EarlyDeliveryCredit", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target17 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__25.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p25 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__25;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AdvancedPricingInfo", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj18 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__24.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__24, pricing);
      object obj19 = target17((CallSite) p25, obj18);
      Decimal num6 = target16((CallSite) p26, obj19);
      advancedPricingInfo5.EarlyDeliveryCredit = num6;
      TradeAdvancedPricingInfo advancedPricingInfo6 = tradePricingInfo1.AdvancedPricingInfo;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__29 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target18 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__29.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p29 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__29;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__28 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "NegotiatedIncentive", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target19 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__28.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p28 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__28;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AdvancedPricingInfo", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj20 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__27.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__27, pricing);
      object obj21 = target19((CallSite) p28, obj20);
      Decimal num7 = target18((CallSite) p29, obj21);
      advancedPricingInfo6.NegotiatedIncentive = num7;
      TradeAdvancedPricingItems advancedPricingItems = new TradeAdvancedPricingItems();
      tradePricingInfo1.AdvancedPricingInfo.PricingItems = advancedPricingItems;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__35 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__35 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IEnumerable> target20 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__35.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IEnumerable>> p35 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__35;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__31 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PricingItems", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target21 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__31.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p31 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__31;
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__30 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AdvancedPricingInfo", typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj22 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__30.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__30, pricing);
      object obj23 = target21((CallSite) p31, obj22);
      foreach (object obj24 in target20((CallSite) p35, obj23))
      {
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__34 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__34 = CallSite<Func<CallSite, object, TradeAdvancedPricingItem>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TradeAdvancedPricingItem), typeof (TradeLoanUpdateUtils)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, TradeAdvancedPricingItem> target22 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__34.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, TradeAdvancedPricingItem>> p34 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__34;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__33 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__33 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<Type>) new Type[1]
          {
            typeof (TradeAdvancedPricingItem)
          }, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target23 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__33.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p33 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__33;
        Type type = typeof (JsonConvert);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__32 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__32 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj25 = TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__32.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__10.\u003C\u003Ep__32, typeof (JsonConvert), obj24);
        object obj26 = target23((CallSite) p33, type, obj25);
        TradeAdvancedPricingItem advancedPricingItem = target22((CallSite) p34, obj26);
        advancedPricingItems.Add(advancedPricingItem);
      }
      return tradePricingInfo1;
    }

    public static List<string> BuildSkipFieldList(object fieldList)
    {
      List<string> stringList1 = new List<string>();
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (object obj1 in TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__2.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__2, fieldList))
      {
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__1 = CallSite<Action<CallSite, List<string>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, List<string>, object> target = TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, List<string>, object>> p1 = TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__1;
        List<string> stringList2 = stringList1;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__11.\u003C\u003Ep__0, obj1);
        target((CallSite) p1, stringList2, obj2);
      }
      return stringList1;
    }

    public static LoanTradePairOffs BuildLoanTradePairOffs(object pairOffs)
    {
      LoanTradePairOffs loanTradePairOffs1 = new LoanTradePairOffs();
      // ISSUE: reference to a compiler-generated field
      if (TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (TradeLoanUpdateUtils)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (object obj1 in TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__3.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__3, pairOffs))
      {
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Action<CallSite, LoanTradePairOffs, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, LoanTradePairOffs, object> target1 = TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, LoanTradePairOffs, object>> p2 = TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__2;
        LoanTradePairOffs loanTradePairOffs2 = loanTradePairOffs1;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<Type>) new Type[1]
          {
            typeof (LoanTradePairOff)
          }, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target2 = TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p1 = TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__1;
        Type type = typeof (JsonConvert);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (TradeLoanUpdateUtils), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) TradeLoanUpdateUtils.\u003C\u003Eo__12.\u003C\u003Ep__0, typeof (JsonConvert), obj1);
        object obj3 = target2((CallSite) p1, type, obj2);
        target1((CallSite) p2, loanTradePairOffs2, obj3);
      }
      return loanTradePairOffs1;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.LoanTradeManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Cursors;
using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class LoanTradeManager : TradeManagerBase, ILoanTradeManager
  {
    private const string className = "LoanTradeManager";

    public LoanTradeManager Initialize(ISession session)
    {
      this.Initialize(session, nameof (LoanTradeManager).ToLower(), TradeType.LoanTrade);
      return this;
    }

    public virtual List<LoanTradeInfo> GetAllTrades()
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetAllTrades), Array.Empty<object>());
      try
      {
        return new List<LoanTradeInfo>((IEnumerable<LoanTradeInfo>) LoanTrades.GetAllTrades());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (List<LoanTradeInfo>) null;
      }
    }

    public virtual LoanTradeInfo GetTrade(int tradeId)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return LoanTrades.GetTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (LoanTradeInfo) null;
      }
    }

    public virtual TradeEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization)
    {
      return this.GetTradeEditorScreenData(tradeId, assignedLoanFields, isExternalOrganization, 0);
    }

    public virtual TradeEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization,
      int sqlRead)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetTradeEditorScreenData), new object[2]
      {
        (object) tradeId,
        (object) assignedLoanFields
      });
      try
      {
        TradeEditorScreenData editorScreenData = new TradeEditorScreenData();
        editorScreenData.ActiveContracts = LoanTrades.GetActiveContracts(false);
        if (tradeId > 0)
        {
          editorScreenData.LoanTrade = LoanTrades.GetTrade(tradeId);
          editorScreenData.TradeHistory = LoanTrades.GetTradeHistory(tradeId);
          editorScreenData.AssignedLoans = LoanTrades.GetAssignedOrPendingLoans(this.Session.GetUserInfo(), tradeId, assignedLoanFields, isExternalOrganization, sqlRead);
          editorScreenData.AssignedContract = LoanTrades.GetContractForTrade(tradeId);
        }
        return editorScreenData;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (TradeEditorScreenData) null;
      }
    }

    public virtual void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      LoanTradeStatus status)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (CommitPendingTradeStatus), new object[3]
      {
        (object) tradeId,
        (object) loanGuid,
        (object) status
      });
      if ((loanGuid ?? "") == "")
        Err.Raise(nameof (LoanTradeManager), (ServerException) new ServerArgumentException("Loan GUID cannot be blank or null", nameof (loanGuid), this.Session.SessionInfo));
      if (status == LoanTradeStatus.None)
        Err.Raise(nameof (LoanTradeManager), (ServerException) new ServerArgumentException("Status value cannot be None", nameof (status), this.Session.SessionInfo));
      try
      {
        LoanTrades.CommitPendingTradeStatus(tradeId, loanGuid, status);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      LoanTradeStatus status,
      bool rejected)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (CommitPendingTradeStatus), new object[4]
      {
        (object) tradeId,
        (object) loanGuid,
        (object) status,
        (object) rejected
      });
      if ((loanGuid ?? "") == "")
        Err.Raise(nameof (LoanTradeManager), (ServerException) new ServerArgumentException("Loan GUID cannot be blank or null", nameof (loanGuid), this.Session.SessionInfo));
      if (status == LoanTradeStatus.None)
        Err.Raise(nameof (LoanTradeManager), (ServerException) new ServerArgumentException("Status value cannot be None", nameof (status), this.Session.SessionInfo));
      try
      {
        LoanTrades.CommitPendingTradeStatus(tradeId, loanGuid, status, rejected);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int CreateTrade(LoanTradeInfo tradeInfo)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (CreateTrade), new object[1]
      {
        (object) tradeInfo
      });
      try
      {
        return LoanTrades.CreateTrade(tradeInfo, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void UpdateTrade(LoanTradeInfo tradeInfo, bool checkStatus)
    {
      this.UpdateTrade(tradeInfo, checkStatus, true);
    }

    public virtual void UpdateTrade(LoanTradeInfo tradeInfo, bool checkStatus, bool isUpdateStatus = true)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (UpdateTrade), new object[3]
      {
        (object) tradeInfo,
        (object) checkStatus,
        (object) isUpdateStatus
      });
      try
      {
        LoanTrades.UpdateTrade(tradeInfo, this.Session.GetUserInfo(), false, checkStatus, isUpdateStatus);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanTradeInfo GetTradeByName(string tradeName)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetTradeByName), new object[1]
      {
        (object) tradeName
      });
      try
      {
        return LoanTrades.GetTradeByName(tradeName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (LoanTradeInfo) null;
      }
    }

    public virtual ICursor GetEligibleLoanCursor(
      int tradeId,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(new int[1]
      {
        tradeId
      }, fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      int[] tradeIds,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetEligibleLoanCursor), new object[4]
      {
        (object) tradeIds,
        (object) dataToInclude,
        (object) fields,
        (object) sortOrder
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<TradeLoanCursor>().Initialize(this.Session, tradeIds, fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor GetEligibleLoanCursor(
      LoanTradeInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(new LoanTradeInfo[1]
      {
        trade
      }, fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      LoanTradeInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(new LoanTradeInfo[1]
      {
        trade
      }, fields, dataToInclude, sortOrder, guidsToOmit, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      LoanTradeInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(trades, fields, dataToInclude, sortOrder, (string[]) null, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      LoanTradeInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetEligibleLoanCursor), new object[5]
      {
        (object) trades,
        (object) fields,
        (object) dataToInclude,
        (object) sortOrder,
        (object) guidsToOmit
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<TradeLoanCursor>().Initialize(this.Session, trades, fields, dataToInclude, sortOrder, guidsToOmit, isExternalOrganization, filterOption);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual void AddTradeHistoryItem(LoanTradeHistoryItem item)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (AddTradeHistoryItem), new object[1]
      {
        (object) item
      });
      try
      {
        LoanTrades.AddTradeHistoryItem(item);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ICursor OpenTradeCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (OpenTradeCursor), new object[4]
      {
        (object) criteria,
        (object) sortOrder,
        (object) fields,
        (object) summaryOnly
      });
      try
      {
        if (fields == null || fields.Length == 0)
          fields = this.defaultTradeFields;
        return (ICursor) InterceptionUtils.NewInstance<TradeCursor>().Initialize(this.Session, LoanTrades.QueryTradeIds(this.Session.GetUserInfo(), criteria, sortOrder, isExternalOrganization), fields, summaryOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual void DeleteTrade(int tradeId)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (DeleteTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        LoanTrades.DeleteTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanTradeViewModel GetTradeViewForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetTradeViewForLoan), new object[2]
      {
        (object) loanGuid,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return LoanTrades.GetTradeViewForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (LoanTradeViewModel) null;
      }
    }

    public virtual LoanTradeInfo GetTradeForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetTradeForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return LoanTrades.GetTradeForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (LoanTradeInfo) null;
      }
    }

    public virtual LoanTradeInfo GetTradeForRejectedLoan(string loanGuid)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetTradeForRejectedLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return LoanTrades.GetTradeForRejectedLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (LoanTradeInfo) null;
      }
    }

    public override void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      bool needPendingCheck)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (SetTradeStatus), new object[4]
      {
        (object) tradeIds,
        (object) status,
        (object) action,
        (object) needPendingCheck
      });
      try
      {
        LoanTrades.SetTradeStatus(tradeIds, status, action, this.Session.GetUserInfo(), needPendingCheck);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanTradeViewModel[] GetActiveTradeView()
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetActiveTradeView), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.HomePage)
      });
      try
      {
        return LoanTrades.GetActiveTradeView();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (LoanTradeViewModel[]) null;
      }
    }

    public virtual LoanTradeViewModel[] GetTradeViewsByContractID(int contractId)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetTradeViewsByContractID), new object[2]
      {
        (object) contractId,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return LoanTrades.GetTradeViewsByContractID(contractId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (LoanTradeViewModel[]) null;
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      LoanTradeStatus[] statuses,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (SetPendingTradeStatuses), new object[5]
      {
        (object) tradeId,
        (object) loanGuids,
        (object) statuses,
        (object) isExternalOrganization,
        (object) removePendingLoan
      });
      try
      {
        LoanTrades.SetPendingTradeStatus(tradeId, loanGuids, statuses, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      LoanTradeStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (SetPendingTradeStatuses), new object[6]
      {
        (object) tradeId,
        (object) loanGuids,
        (object) statuses,
        (object) comments,
        (object) isExternalOrganization,
        (object) removePendingLoan
      });
      try
      {
        LoanTrades.SetPendingTradeStatus(tradeId, loanGuids, statuses, comments, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      LoanTradeStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan,
      Decimal[] totalPrices,
      bool forceUpdateAllLoans = false)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (SetPendingTradeStatuses), new object[8]
      {
        (object) tradeId,
        (object) loanGuids,
        (object) statuses,
        (object) comments,
        (object) isExternalOrganization,
        (object) removePendingLoan,
        (object) totalPrices,
        (object) forceUpdateAllLoans
      });
      try
      {
        LoanTrades.SetPendingTradeStatus(tradeId, loanGuids, statuses, comments, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan, totalPrices, forceUpdateAllLoans);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual PipelineInfo[] GetAssignedOrPendingLoans(
      int tradeId,
      string[] fields,
      bool isExternalOrganization)
    {
      return this.GetAssignedOrPendingLoans(tradeId, fields, isExternalOrganization, 0);
    }

    public virtual PipelineInfo[] GetAssignedOrPendingLoans(
      int tradeId,
      string[] fields,
      bool isExternalOrganization,
      int sqlRead)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetAssignedOrPendingLoans), new object[2]
      {
        (object) tradeId,
        (object) fields
      });
      try
      {
        return LoanTrades.GetAssignedOrPendingLoans(this.Session.GetUserInfo(), tradeId, fields, isExternalOrganization, sqlRead);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (PipelineInfo[]) null;
      }
    }

    public virtual LoanTradeHistoryItem[] GetTradeHistoryForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetTradeHistoryForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return LoanTrades.GetTradeHistoryForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (LoanTradeHistoryItem[]) null;
      }
    }

    public virtual LoanTradeInfo GetTrade(string tradeNumber)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetTrade), new object[1]
      {
        (object) tradeNumber
      });
      try
      {
        return LoanTrades.GetTrade(tradeNumber);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (LoanTradeInfo) null;
      }
    }

    public virtual Dictionary<int, string> GetEligibleLoanTradeByLoanInfo()
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetEligibleLoanTradeByLoanInfo), Array.Empty<object>());
      try
      {
        IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
        return LoanTrades.GetEligibleLoanTradeByLoanInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (Dictionary<int, string>) null;
      }
    }

    public virtual bool IsTradeAssignmentUpdateCompleted(
      int tradeId,
      string loanGuid,
      LoanTradeStatus status)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (IsTradeAssignmentUpdateCompleted), Array.Empty<object>());
      try
      {
        return LoanTrades.IsTradeAssignmentUpdateCompleted(tradeId, loanGuid, status);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual List<TradeUnlockInfo> GetPendingTrades(List<TradeType> tradeTypes, int timeWait)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetPendingTrades), new object[2]
      {
        (object) tradeTypes,
        (object) timeWait
      });
      try
      {
        return LoanTrades.GetPendingTrades(tradeTypes, timeWait);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (List<TradeUnlockInfo>) null;
      }
    }

    public virtual List<TradeInfoObj> GetPendingTrades(List<TradeType> tradeTypes)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetPendingTrades), new object[1]
      {
        (object) tradeTypes
      });
      try
      {
        return LoanTrades.GetPendingTrades(tradeTypes, -1).Select<TradeUnlockInfo, TradeInfoObj>((Func<TradeUnlockInfo, TradeInfoObj>) (i => i.tradeInfo)).ToList<TradeInfoObj>();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return (List<TradeInfoObj>) null;
      }
    }

    public virtual TradeStatus GetTradeStatus(int tradeId)
    {
      this.onApiCalled(nameof (LoanTradeManager), nameof (GetTradeStatus), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return LoanTrades.GetTradeStatus(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTradeManager), ex, this.Session.SessionInfo);
        return TradeStatus.None;
      }
    }
  }
}

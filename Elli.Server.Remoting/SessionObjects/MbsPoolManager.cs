// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.MbsPoolManager
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

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class MbsPoolManager : TradeManagerBase, IMbsPoolManager
  {
    private const string className = "MbsPoolManager";

    public MbsPoolManager Initialize(ISession session)
    {
      this.Initialize(session, nameof (MbsPoolManager).ToLower(), TradeType.MbsPool);
      return this;
    }

    public virtual List<MbsPoolInfo> GetAllTrades()
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetAllTrades), Array.Empty<object>());
      try
      {
        return new List<MbsPoolInfo>((IEnumerable<MbsPoolInfo>) MbsPools.GetAllTrades());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (List<MbsPoolInfo>) null;
      }
    }

    public virtual MbsPoolInfo GetTrade(int tradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return MbsPools.GetTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolInfo) null;
      }
    }

    public virtual MbsPoolEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization)
    {
      return this.GetTradeEditorScreenData(tradeId, assignedLoanFields, isExternalOrganization, 0);
    }

    public virtual MbsPoolEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization,
      int sqlRead)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeEditorScreenData), new object[2]
      {
        (object) tradeId,
        (object) assignedLoanFields
      });
      try
      {
        MbsPoolEditorScreenData editorScreenData = new MbsPoolEditorScreenData();
        editorScreenData.ActiveContracts = MbsPools.GetActiveContracts(false);
        if (tradeId > 0)
        {
          editorScreenData.LoanTrade = MbsPools.GetTrade(tradeId);
          editorScreenData.TradeHistory = MbsPools.GetTradeHistory(tradeId);
          editorScreenData.AssignedLoans = MbsPools.GetAssignedOrPendingLoans(this.Session.GetUserInfo(), tradeId, assignedLoanFields, isExternalOrganization, sqlRead);
          editorScreenData.AssignedContract = MbsPools.GetContractForTrade(tradeId);
        }
        return editorScreenData;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolEditorScreenData) null;
      }
    }

    public virtual void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      MbsPoolLoanStatus status)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (CommitPendingTradeStatus), new object[3]
      {
        (object) tradeId,
        (object) loanGuid,
        (object) status
      });
      if ((loanGuid ?? "") == "")
        Err.Raise(nameof (MbsPoolManager), (ServerException) new ServerArgumentException("Loan GUID cannot be blank or null", nameof (loanGuid), this.Session.SessionInfo));
      if (status == MbsPoolLoanStatus.None)
        Err.Raise(nameof (MbsPoolManager), (ServerException) new ServerArgumentException("Status value cannot be None", nameof (status), this.Session.SessionInfo));
      try
      {
        MbsPools.CommitPendingTradeStatus(tradeId, loanGuid, status);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      MbsPoolLoanStatus status,
      bool rejected)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (CommitPendingTradeStatus), new object[4]
      {
        (object) tradeId,
        (object) loanGuid,
        (object) status,
        (object) rejected
      });
      if ((loanGuid ?? "") == "")
        Err.Raise(nameof (MbsPoolManager), (ServerException) new ServerArgumentException("Loan GUID cannot be blank or null", nameof (loanGuid), this.Session.SessionInfo));
      if (status == MbsPoolLoanStatus.None)
        Err.Raise(nameof (MbsPoolManager), (ServerException) new ServerArgumentException("Status value cannot be None", nameof (status), this.Session.SessionInfo));
      try
      {
        MbsPools.CommitPendingTradeStatus(tradeId, loanGuid, status, rejected);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int CreateTrade(MbsPoolInfo tradeInfo)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (CreateTrade), new object[1]
      {
        (object) tradeInfo
      });
      try
      {
        return MbsPools.CreateTrade(tradeInfo, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void UpdateTrade(
      MbsPoolInfo tradeInfo,
      bool isExternalOrganization,
      bool checkStatus)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (UpdateTrade), new object[2]
      {
        (object) tradeInfo,
        (object) checkStatus
      });
      try
      {
        MbsPools.UpdateTrade(tradeInfo, this.Session.GetUserInfo(), isExternalOrganization, checkStatus);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual MbsPoolInfo GetTradeByName(string tradeName)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeByName), new object[1]
      {
        (object) tradeName
      });
      try
      {
        return MbsPools.GetTradeByName(tradeName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolInfo) null;
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
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetEligibleLoanCursor), new object[4]
      {
        (object) tradeIds,
        (object) dataToInclude,
        (object) fields,
        (object) sortOrder
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<MbsPoolLoanCursor>().Initialize(this.Session, tradeIds, fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor GetEligibleLoanCursor(
      MbsPoolInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(new MbsPoolInfo[1]
      {
        trade
      }, fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      MbsPoolInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(new MbsPoolInfo[1]
      {
        trade
      }, fields, dataToInclude, sortOrder, guidsToOmit, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      MbsPoolInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(trades, fields, dataToInclude, sortOrder, (string[]) null, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      MbsPoolInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetEligibleLoanCursor), new object[5]
      {
        (object) trades,
        (object) fields,
        (object) dataToInclude,
        (object) sortOrder,
        (object) guidsToOmit
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<MbsPoolLoanCursor>().Initialize(this.Session, trades, fields, dataToInclude, sortOrder, guidsToOmit, isExternalOrganization, filterOption);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual void AddTradeHistoryItem(MbsPoolHistoryItem item)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (AddTradeHistoryItem), new object[1]
      {
        (object) item
      });
      try
      {
        MbsPools.AddTradeHistoryItem(item);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ICursor OpenTradeCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (OpenTradeCursor), new object[4]
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
        return (ICursor) InterceptionUtils.NewInstance<MbsPoolCursor>().Initialize(this.Session, MbsPools.QueryTradeIds(this.Session.GetUserInfo(), criteria, sortOrder, isExternalOrganization), fields, summaryOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual void DeleteTrade(int tradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (DeleteTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        MbsPools.DeleteTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual MbsPoolViewModel GetTradeViewForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeViewForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return MbsPools.GetTradeViewForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolViewModel) null;
      }
    }

    public virtual MbsPoolInfo GetTradeForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return MbsPools.GetTradeForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolInfo) null;
      }
    }

    public virtual MbsPoolInfo GetTradeForRejectedLoan(string loanGuid)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeForRejectedLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return MbsPools.GetTradeForRejectedLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolInfo) null;
      }
    }

    public override void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      bool needPendingCheck)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (SetTradeStatus), new object[4]
      {
        (object) tradeIds,
        (object) status,
        (object) action,
        (object) needPendingCheck
      });
      try
      {
        MbsPools.SetTradeStatus(tradeIds, status, action, this.Session.GetUserInfo(), needPendingCheck);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual MbsPoolViewModel[] GetActiveTradeView()
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetActiveTradeView), Array.Empty<object>());
      try
      {
        return MbsPools.GetActiveTradeView();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolViewModel[]) null;
      }
    }

    public virtual MbsPoolViewModel[] GetTradeViewsByContractID(int contractId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeViewsByContractID), new object[1]
      {
        (object) contractId
      });
      try
      {
        return MbsPools.GetTradeViewsByContractID(contractId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolViewModel[]) null;
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      MbsPoolLoanStatus[] statuses,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (SetPendingTradeStatuses), new object[5]
      {
        (object) tradeId,
        (object) loanGuids,
        (object) statuses,
        (object) isExternalOrganization,
        (object) removePendingLoan
      });
      try
      {
        MbsPools.SetPendingTradeStatus(tradeId, loanGuids, statuses, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      MbsPoolLoanStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (SetPendingTradeStatuses), new object[6]
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
        MbsPools.SetPendingTradeStatus(tradeId, loanGuids, statuses, comments, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      MbsPoolLoanStatus[] statuses,
      string[] comments,
      string[] commitmentContractNumber,
      string[] productName,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (SetPendingTradeStatuses), new object[8]
      {
        (object) tradeId,
        (object) loanGuids,
        (object) statuses,
        (object) comments,
        (object) commitmentContractNumber,
        (object) productName,
        (object) isExternalOrganization,
        (object) removePendingLoan
      });
      try
      {
        MbsPools.SetPendingTradeStatus(tradeId, loanGuids, statuses, comments, commitmentContractNumber, productName, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetCommitmentInfo(
      int tradeId,
      string[] loanGuids,
      string[] commitmentContractNumber,
      string[] productName,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (SetCommitmentInfo), new object[5]
      {
        (object) tradeId,
        (object) loanGuids,
        (object) commitmentContractNumber,
        (object) productName,
        (object) isExternalOrganization
      });
      try
      {
        MbsPools.SetCommitmentInfo(tradeId, loanGuids, commitmentContractNumber, productName, this.Session.GetUserInfo(), isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
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
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetAssignedOrPendingLoans), new object[2]
      {
        (object) tradeId,
        (object) fields
      });
      try
      {
        return MbsPools.GetAssignedOrPendingLoans(this.Session.GetUserInfo(), tradeId, fields, isExternalOrganization, sqlRead);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (PipelineInfo[]) null;
      }
    }

    public virtual MbsPoolHistoryItem[] GetTradeHistoryForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeHistoryForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return MbsPools.GetTradeHistoryForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolHistoryItem[]) null;
      }
    }

    public virtual void AssignSecurityTradeToTrade(int tradeId, int assigneeTradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (AssignSecurityTradeToTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.AssignTradeToTrade(TradeType.MbsPool, tradeId, TradeType.SecurityTrade, assigneeTradeId, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AssignSecurityTradeToTrade(
      int tradeId,
      int assigneeTradeId,
      Decimal assignedAmount)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (AssignSecurityTradeToTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.AssignTradeToTrade(TradeType.MbsPool, tradeId, TradeType.SecurityTrade, assigneeTradeId, assignedAmount, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AssignSecurityTradeToTrade(
      int tradeId,
      int assigneeTradeId,
      MbsPoolSecurityTradeStatus status,
      DateTime assignedStatusDate)
    {
      this.onApiCalled(nameof (MbsPoolManager), "AssignLoanTradeToTrade", new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.AssignTradeToTrade(TradeType.MbsPool, tradeId, TradeType.SecurityTrade, assigneeTradeId, (object) status, assignedStatusDate, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AssignSecurityTradeToTrade(
      int tradeId,
      int assigneeTradeId,
      MbsPoolSecurityTradeStatus status,
      DateTime assignedStatusDate,
      Decimal assignedAmount)
    {
      this.onApiCalled(nameof (MbsPoolManager), "AssignLoanTradeToTrade", new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.AssignTradeToTrade(TradeType.MbsPool, tradeId, TradeType.SecurityTrade, assigneeTradeId, (object) status, assignedStatusDate, assignedAmount, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UnassignSecurityTradeToTrade(int tradeId, int assigneeTradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (UnassignSecurityTradeToTrade), new object[2]
      {
        (object) tradeId,
        (object) assigneeTradeId
      });
      try
      {
        TradeAssignmentByTrade.UnassignTradeToTrade(TradeType.MbsPool, tradeId, TradeType.SecurityTrade, assigneeTradeId, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UnassignGSECommitmentToTrade(int tradeId, int assigneeTradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (UnassignGSECommitmentToTrade), new object[2]
      {
        (object) tradeId,
        (object) assigneeTradeId
      });
      try
      {
        TradeAssignmentByTrade.UnassignTradeToTrade(TradeType.MbsPool, tradeId, TradeType.GSECommitment, assigneeTradeId, this.Session.GetUserInfo(), TradeHistoryAction.GSEAssigneeUnassigned);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AssignGSECommitmentToTrade(
      int tradeId,
      int assigneeTradeId,
      Decimal assignedAmount)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (AssignGSECommitmentToTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.AssignTradeToTrade(TradeType.MbsPool, tradeId, TradeType.GSECommitment, assigneeTradeId, assignedAmount, this.Session.GetUserInfo(), TradeHistoryAction.GSEAssigneeAssigned);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateAssignedAmountToTrade(
      int tradeId,
      int assigneeTradeId,
      Decimal assignedAmount)
    {
      this.onApiCalled(nameof (MbsPoolManager), "AssignLoanTradeToTrade", new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.UpdateAssignedAmountToTrade(TradeType.MbsPool, tradeId, TradeType.SecurityTrade, assigneeTradeId, assignedAmount, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual MbsPoolAssignment[] GetTradeAssigments(int tradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeAssigments), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetMbsPoolAssignments(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolAssignment[]) null;
      }
    }

    public virtual MbsPoolAssignment[] GetUnassignedTradeAssigments(int tradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetUnassignedTradeAssigments), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetUnassignedMbsPoolAssignments(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolAssignment[]) null;
      }
    }

    public virtual MbsPoolAssignment[] GetTradeAssigmentsBySecurityTrade(int tradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeAssigmentsBySecurityTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetMbsPoolAssignmentsBySecurityTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolAssignment[]) null;
      }
    }

    public virtual MbsPoolAssignment[] GetUnassignedTradeAssigmentsBySecurityTrade(int tradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetUnassignedTradeAssigmentsBySecurityTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetUnassignedMbsPoolAssignmentsBySecurityTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolAssignment[]) null;
      }
    }

    public virtual MbsPoolAssignment[] GetTradeAssigmentsByGseCommitment(int tradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeAssigmentsByGseCommitment), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetMbsPoolAssignmentsByGseCommitment(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolAssignment[]) null;
      }
    }

    public virtual MbsPoolAssignment[] GetUnassignedTradeAssigmentsByGseCommitment(int tradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetUnassignedTradeAssigmentsByGseCommitment), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetUnassignedMbsPoolAssignmentsByGseCommitment(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return (MbsPoolAssignment[]) null;
      }
    }

    public virtual void UpdateTradeAfterAssignSecurityTrade(MbsPoolInfo tradeInfo)
    {
    }

    public virtual bool IsTradeAssignmentUpdateCompleted(
      int tradeId,
      string loanGuid,
      MbsPoolLoanStatus status)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (IsTradeAssignmentUpdateCompleted), Array.Empty<object>());
      try
      {
        return MbsPools.IsTradeAssignmentUpdateCompleted(tradeId, loanGuid, status);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual TradeStatus GetTradeStatus(int tradeId)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetTradeStatus), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return MbsPools.GetTradeStatus(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return TradeStatus.None;
      }
    }

    public virtual MbsPoolMortgageType GetMbsPoolMortgageType(string tradeGuid)
    {
      this.onApiCalled(nameof (MbsPoolManager), nameof (GetMbsPoolMortgageType), new object[1]
      {
        (object) tradeGuid
      });
      try
      {
        return MbsPools.GetMbsPoolMortgageType(tradeGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MbsPoolManager), ex, this.Session.SessionInfo);
        return MbsPoolMortgageType.None;
      }
    }
  }
}

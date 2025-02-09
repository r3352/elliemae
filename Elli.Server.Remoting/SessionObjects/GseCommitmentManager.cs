// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.GseCommitmentManager
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
  public class GseCommitmentManager : TradeManagerBase, IGseCommitmentManager
  {
    private const string className = "GseCommitmentManager";

    public GseCommitmentManager Initialize(ISession session)
    {
      this.Initialize(session, nameof (GseCommitmentManager).ToLower(), TradeType.GSECommitment);
      return this;
    }

    public virtual List<GSECommitmentInfo> GetAllTrades()
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetAllTrades), Array.Empty<object>());
      try
      {
        return new List<GSECommitmentInfo>((IEnumerable<GSECommitmentInfo>) GseCommitments.GetAllTrades());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (List<GSECommitmentInfo>) null;
      }
    }

    public virtual GSECommitmentInfo GetTrade(int tradeId)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return GseCommitments.GetTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GSECommitmentInfo) null;
      }
    }

    public virtual GseCommitmentEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization)
    {
      return this.GetTradeEditorScreenData(tradeId, assignedLoanFields, isExternalOrganization, 0);
    }

    public virtual GseCommitmentEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization,
      int sqlRead)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTradeEditorScreenData), new object[2]
      {
        (object) tradeId,
        (object) assignedLoanFields
      });
      try
      {
        GseCommitmentEditorScreenData editorScreenData = new GseCommitmentEditorScreenData();
        if (tradeId > 0)
        {
          editorScreenData.TradeHistory = GseCommitments.GetTradeHistory(tradeId);
          editorScreenData.AssignedLoans = GseCommitments.GetAssignedOrPendingLoans(this.Session.GetUserInfo(), tradeId, assignedLoanFields, isExternalOrganization, sqlRead);
        }
        return editorScreenData;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GseCommitmentEditorScreenData) null;
      }
    }

    public virtual void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      GseCommitmentLoanStatus status)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (CommitPendingTradeStatus), new object[3]
      {
        (object) tradeId,
        (object) loanGuid,
        (object) status
      });
      if ((loanGuid ?? "") == "")
        Err.Raise(nameof (GseCommitmentManager), (ServerException) new ServerArgumentException("Loan GUID cannot be blank or null", nameof (loanGuid), this.Session.SessionInfo));
      if (status == GseCommitmentLoanStatus.None)
        Err.Raise(nameof (GseCommitmentManager), (ServerException) new ServerArgumentException("Status value cannot be None", nameof (status), this.Session.SessionInfo));
      try
      {
        GseCommitments.CommitPendingTradeStatus(tradeId, loanGuid, status);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      GseCommitmentLoanStatus status,
      bool rejected)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (CommitPendingTradeStatus), new object[4]
      {
        (object) tradeId,
        (object) loanGuid,
        (object) status,
        (object) rejected
      });
      if ((loanGuid ?? "") == "")
        Err.Raise(nameof (GseCommitmentManager), (ServerException) new ServerArgumentException("Loan GUID cannot be blank or null", nameof (loanGuid), this.Session.SessionInfo));
      if (status == GseCommitmentLoanStatus.None)
        Err.Raise(nameof (GseCommitmentManager), (ServerException) new ServerArgumentException("Status value cannot be None", nameof (status), this.Session.SessionInfo));
      try
      {
        GseCommitments.CommitPendingTradeStatus(tradeId, loanGuid, status, rejected);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int CreateTrade(GSECommitmentInfo tradeInfo)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (CreateTrade), new object[1]
      {
        (object) tradeInfo
      });
      try
      {
        return GseCommitments.CreateTrade(tradeInfo, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void UpdateTrade(GSECommitmentInfo tradeInfo, bool isExternalOrganization)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (UpdateTrade), new object[1]
      {
        (object) tradeInfo
      });
      try
      {
        GseCommitments.UpdateTrade(tradeInfo, this.Session.GetUserInfo(), isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual GSECommitmentInfo GetTradeByName(string tradeName)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTradeByName), new object[1]
      {
        (object) tradeName
      });
      try
      {
        return GseCommitments.GetTradeByName(tradeName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GSECommitmentInfo) null;
      }
    }

    public virtual GSECommitmentInfo GetTradeByContractNumber(string contractNumber)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTradeByContractNumber), new object[1]
      {
        (object) contractNumber
      });
      try
      {
        return GseCommitments.GetTradeByContractNumber(contractNumber);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GSECommitmentInfo) null;
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
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetEligibleLoanCursor), new object[4]
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
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor GetEligibleLoanCursor(
      GSECommitmentInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(new GSECommitmentInfo[1]
      {
        trade
      }, fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      GSECommitmentInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(new GSECommitmentInfo[1]
      {
        trade
      }, fields, dataToInclude, sortOrder, guidsToOmit, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      GSECommitmentInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return this.GetEligibleLoanCursor(trades, fields, dataToInclude, sortOrder, (string[]) null, isExternalOrganization, filterOption);
    }

    public virtual ICursor GetEligibleLoanCursor(
      GSECommitmentInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All)
    {
      return (ICursor) null;
    }

    public virtual void AddTradeHistoryItem(GseCommitmentHistoryItem item)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (AddTradeHistoryItem), new object[1]
      {
        (object) item
      });
      try
      {
        GseCommitments.AddTradeHistoryItem(item);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ICursor OpenTradeCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (OpenTradeCursor), new object[4]
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
        return (ICursor) InterceptionUtils.NewInstance<GseCommitmentCursor>().Initialize(this.Session, GseCommitments.QueryTradeIds(this.Session.GetUserInfo(), criteria, sortOrder, isExternalOrganization), fields, summaryOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual void DeleteTrade(int tradeId)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (DeleteTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        GseCommitments.DeleteTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual GSECommitmentViewModel GetTradeViewForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTradeViewForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return GseCommitments.GetTradeViewForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GSECommitmentViewModel) null;
      }
    }

    public virtual GSECommitmentInfo GetTradeForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTradeForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return GseCommitments.GetTradeForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GSECommitmentInfo) null;
      }
    }

    public virtual GSECommitmentInfo GetTradeForRejectedLoan(string loanGuid)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTradeForRejectedLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return GseCommitments.GetTradeForRejectedLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GSECommitmentInfo) null;
      }
    }

    public override void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      bool needPendingCheck = true)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (SetTradeStatus), new object[4]
      {
        (object) tradeIds,
        (object) status,
        (object) action,
        (object) needPendingCheck
      });
      try
      {
        GseCommitments.SetTradeStatus(tradeIds, status, action, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual GSECommitmentViewModel[] GetActiveTradeView()
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetActiveTradeView), Array.Empty<object>());
      try
      {
        return GseCommitments.GetActiveTradeView();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GSECommitmentViewModel[]) null;
      }
    }

    public virtual GSECommitmentViewModel[] GetTradeViewsByContractID(int contractId)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTradeViewsByContractID), new object[1]
      {
        (object) contractId
      });
      try
      {
        return GseCommitments.GetTradeViewsByContractID(contractId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GSECommitmentViewModel[]) null;
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      GseCommitmentLoanStatus[] statuses,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (SetPendingTradeStatuses), new object[5]
      {
        (object) tradeId,
        (object) loanGuids,
        (object) statuses,
        (object) isExternalOrganization,
        (object) removePendingLoan
      });
      try
      {
        GseCommitments.SetPendingTradeStatus(tradeId, loanGuids, statuses, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      GseCommitmentLoanStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (SetPendingTradeStatuses), new object[6]
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
        GseCommitments.SetPendingTradeStatus(tradeId, loanGuids, statuses, comments, this.Session.GetUserInfo(), isExternalOrganization, removePendingLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
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
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetAssignedOrPendingLoans), new object[2]
      {
        (object) tradeId,
        (object) fields
      });
      try
      {
        return GseCommitments.GetAssignedOrPendingLoans(this.Session.GetUserInfo(), tradeId, fields, isExternalOrganization, sqlRead);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (PipelineInfo[]) null;
      }
    }

    public virtual GseCommitmentHistoryItem[] GetTradeHistoryForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTradeHistoryForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return GseCommitments.GetTradeHistoryForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GseCommitmentHistoryItem[]) null;
      }
    }

    public virtual void AssignSecurityTradeToTrade(int tradeId, int assigneeTradeId)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (AssignSecurityTradeToTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.AssignTradeToTrade(TradeType.GSECommitment, tradeId, TradeType.SecurityTrade, assigneeTradeId, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AssignSecurityTradeToTrade(
      int tradeId,
      int assigneeTradeId,
      Decimal assignedAmount)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (AssignSecurityTradeToTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.AssignTradeToTrade(TradeType.GSECommitment, tradeId, TradeType.SecurityTrade, assigneeTradeId, assignedAmount, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateAssignedAmountToTrade(
      int tradeId,
      int assigneeTradeId,
      Decimal assignedAmount)
    {
      this.onApiCalled(nameof (GseCommitmentManager), "AssignLoanTradeToTrade", new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.UpdateAssignedAmountToTrade(TradeType.GSECommitment, tradeId, TradeType.SecurityTrade, assigneeTradeId, assignedAmount, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual GSECommitmentAssignment[] GetTradeAssigmentsByMbsPool(int tradeId)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTradeAssigmentsByMbsPool), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetGSECommintmentByMbsPool(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GSECommitmentAssignment[]) null;
      }
    }

    public virtual GSECommitmentAssignment[] GetUnassignedTradeAssigmentsByMbsPool(int tradeId)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetUnassignedTradeAssigmentsByMbsPool), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetUnassignedGSECommintmentByMbsPool(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (GSECommitmentAssignment[]) null;
      }
    }

    public virtual MbsPoolAssignment[] GetTradeAssigmentsBySecurityTrade(int tradeId)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetTradeAssigmentsBySecurityTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetMbsPoolAssignmentsBySecurityTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (MbsPoolAssignment[]) null;
      }
    }

    public virtual MbsPoolAssignment[] GetUnassignedTradeAssigmentsBySecurityTrade(int tradeId)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (GetUnassignedTradeAssigmentsBySecurityTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetUnassignedMbsPoolAssignmentsBySecurityTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return (MbsPoolAssignment[]) null;
      }
    }

    public virtual void UpdateTradeAfterAssignSecurityTrade(GSECommitmentInfo tradeInfo)
    {
    }

    public virtual List<GSECommitmentInfo> ValidateContractNumbers(
      List<string> commitmentContractNums)
    {
      this.onApiCalled(nameof (GseCommitmentManager), nameof (ValidateContractNumbers), new object[1]
      {
        (object) commitmentContractNums
      });
      try
      {
        return GseCommitments.ValidateContractNumbers(commitmentContractNums);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (GseCommitmentManager), ex, this.Session.SessionInfo);
        return new List<GSECommitmentInfo>();
      }
    }
  }
}

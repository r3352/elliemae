// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.SecurityTradeManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Cursors;
using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class SecurityTradeManager : TradeManagerBase, ISecurityTradeManager
  {
    private const string className = "SecurityTradeManager";

    public SecurityTradeManager Initialize(ISession session)
    {
      this.Initialize(session, nameof (SecurityTradeManager).ToLower(), TradeType.SecurityTrade);
      return this;
    }

    public virtual int CreateTrade(SecurityTradeInfo tradeInfo)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (CreateTrade), new object[1]
      {
        (object) tradeInfo
      });
      try
      {
        return SecurityTrades.CreateTrade(tradeInfo, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual SecurityTradeInfo GetTrade(int tradeId)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (GetTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return SecurityTrades.GetTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
        return (SecurityTradeInfo) null;
      }
    }

    public virtual SecurityTradeInfo GetTradeByName(string tradeName)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (GetTradeByName), new object[1]
      {
        (object) tradeName
      });
      try
      {
        return SecurityTrades.GetTradeByName(tradeName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
        return (SecurityTradeInfo) null;
      }
    }

    public virtual SecurityTradeInfo[] GetTradesByName(string tradeName)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (GetTradesByName), new object[1]
      {
        (object) tradeName
      });
      try
      {
        return SecurityTrades.GetTradesByName(tradeName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
        return (SecurityTradeInfo[]) null;
      }
    }

    public virtual SecurityTradeInfo[] GetTradesByName(string tradeName, bool includeHidden)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (GetTradesByName), new object[2]
      {
        (object) tradeName,
        (object) includeHidden
      });
      try
      {
        return SecurityTrades.GetTradesByName(tradeName, includeHidden);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
        return (SecurityTradeInfo[]) null;
      }
    }

    public virtual Dictionary<int, SecurityTradeInfo> GetTrades(int[] tradeIds)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (GetTrades), new object[1]
      {
        (object) tradeIds
      });
      try
      {
        return SecurityTrades.GetTradeDictionary(tradeIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
        return (Dictionary<int, SecurityTradeInfo>) null;
      }
    }

    public virtual SecurityTradeInfo[] GetActiveTrades()
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (GetActiveTrades), Array.Empty<object>());
      try
      {
        return SecurityTrades.GetActiveTrades();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
        return (SecurityTradeInfo[]) null;
      }
    }

    public virtual void UpdateTrade(SecurityTradeInfo tradeInfo)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (UpdateTrade), new object[1]
      {
        (object) tradeInfo
      });
      try
      {
        SecurityTrades.UpdateTrade(tradeInfo, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteTrade(int tradeId)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (DeleteTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        SecurityTrades.DeleteTrade(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual SecurityTradeEditorScreenData GetTradeEditorScreenData(int tradeId)
    {
      this.onApiCalled(nameof (SecurityTradeManager), "GetSecurityTradeEditorScreenData", new object[1]
      {
        (object) tradeId
      });
      try
      {
        SecurityTradeEditorScreenData editorScreenData = new SecurityTradeEditorScreenData();
        if (tradeId > 0)
        {
          editorScreenData.Trade = SecurityTrades.GetTrade(tradeId);
          editorScreenData.TradeHistory = SecurityTrades.GetTradeHistory(tradeId);
          editorScreenData.Assignments = TradeAssignmentByTrade.GetSecurityTradeAssignments(tradeId);
        }
        return editorScreenData;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
        return (SecurityTradeEditorScreenData) null;
      }
    }

    public virtual SecurityTradeHistoryItem[] GetTradeHistory(int tradeId)
    {
      return (SecurityTradeHistoryItem[]) null;
    }

    public virtual void AddTradeHistoryItem(SecurityTradeHistoryItem item)
    {
    }

    public virtual void AssignLoanTradeToTrade(int tradeId, int assigneeTradeId)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (AssignLoanTradeToTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.AssignTradeToTrade(TradeType.SecurityTrade, tradeId, TradeType.LoanTrade, assigneeTradeId, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AssignLoanTradeToTrade(
      int tradeId,
      int assigneeTradeId,
      SecurityLoanTradeStatus status,
      DateTime assignedStatusDate)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (AssignLoanTradeToTrade), new object[1]
      {
        (object) tradeId
      });
      try
      {
        TradeAssignmentByTrade.AssignTradeToTrade(TradeType.SecurityTrade, tradeId, TradeType.LoanTrade, assigneeTradeId, (object) status, assignedStatusDate, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UnassignLoanTradeToTrade(int tradeId, int assigneeTradeId)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (UnassignLoanTradeToTrade), new object[2]
      {
        (object) tradeId,
        (object) assigneeTradeId
      });
      try
      {
        TradeAssignmentByTrade.UnassignTradeToTrade(TradeType.SecurityTrade, tradeId, TradeType.LoanTrade, assigneeTradeId, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual SecurityTradeAssignment[] GetTradeAssigments(int tradeId)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (GetTradeAssigments), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return TradeAssignmentByTrade.GetSecurityTradeAssignments(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
        return (SecurityTradeAssignment[]) null;
      }
    }

    public virtual ICursor OpenTradeCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (OpenTradeCursor), new object[4]
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
        return (ICursor) InterceptionUtils.NewInstance<SecurityTradeCursor>().Initialize(this.Session, SecurityTrades.QueryTradeIds(this.Session.GetUserInfo(), criteria, sortOrder, isExternalOrganization), fields, summaryOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public override void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      bool needPendingCheck = true)
    {
      this.onApiCalled(nameof (SecurityTradeManager), nameof (SetTradeStatus), new object[4]
      {
        (object) tradeIds,
        (object) status,
        (object) action,
        (object) needPendingCheck
      });
      try
      {
        SecurityTrades.SetTradeStatus(tradeIds, status, action, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SecurityTradeManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateTradeAfterAssignLoanTrade(SecurityTradeInfo tradeInfo)
    {
      SecurityTradeAssignment[] tradeAssigments = this.GetTradeAssigments(tradeInfo.TradeID);
      MbsPoolAssignment[] assigmentsBySecurityTrade = ((MbsPoolManager) this.Session.GetObject("MbsPoolManager")).GetTradeAssigmentsBySecurityTrade(tradeInfo.TradeID);
      tradeInfo.OpenAmount = tradeInfo.Calculation.CalculateOpenAmount(tradeAssigments, assigmentsBySecurityTrade);
      tradeInfo.TotalPairOffGainLoss = tradeInfo.Calculation.CalculateTotalGainLossAmount(tradeAssigments);
      this.UpdateTrade(tradeInfo);
    }
  }
}

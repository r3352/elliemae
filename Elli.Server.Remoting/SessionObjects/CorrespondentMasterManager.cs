// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.CorrespondentMasterManager
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

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class CorrespondentMasterManager : SessionBoundObject, ICorrespondentMasterManager
  {
    private const string className = "CorrespondentMasterManager";

    public CorrespondentMasterManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (CorrespondentMasterManager).ToLower());
      return this;
    }

    public virtual CorrespondentMasterInfo GetCorrespondentMasterByTradeId(
      int correspondentMasterTradeId)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), "GetCorrespondentMasterByCMTradeId", new object[1]
      {
        (object) correspondentMasterTradeId
      });
      try
      {
        return CorrespondentMasters.GetCorrespondentMasterByTradeId(correspondentMasterTradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return (CorrespondentMasterInfo) null;
      }
    }

    public virtual CorrespondentMasterInfo GetCorrespondentMaster(int correspondentMasterId)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (GetCorrespondentMaster), new object[1]
      {
        (object) correspondentMasterId
      });
      try
      {
        return CorrespondentMasters.GetCorrespondentMaster(correspondentMasterId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return (CorrespondentMasterInfo) null;
      }
    }

    public virtual CorrespondentMasterInfo CheckCorrespondentMasterByMasterNumber(
      string masterNumber)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), "GetCorrespondentMasterByMasterNumber", new object[1]
      {
        (object) masterNumber
      });
      try
      {
        return CorrespondentMasters.GetCorrespondentMasterSummaryByMasterNumber(masterNumber);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return (CorrespondentMasterInfo) null;
      }
    }

    public virtual int CreateCorrespondentMaster(CorrespondentMasterInfo correspondentMaster)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (CreateCorrespondentMaster), new object[1]
      {
        (object) correspondentMaster
      });
      try
      {
        return CorrespondentMasters.CreateCorrespondentMaster(correspondentMaster, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void UpdateCorrespondentMaster(CorrespondentMasterInfo correspondentMaster)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (UpdateCorrespondentMaster), new object[1]
      {
        (object) correspondentMaster
      });
      try
      {
        CorrespondentMasters.UpdateCorrespondentMaster(correspondentMaster, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteCorrespondentMaster(int correspondentMaster)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (DeleteCorrespondentMaster), new object[1]
      {
        (object) correspondentMaster
      });
      try
      {
        CorrespondentMasters.DeleteCorrespondentMaster(correspondentMaster, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetCorrespondentMasterStatus(
      int correspondentMasterId,
      MasterCommitmentStatus status)
    {
      this.SetCorrespondentMasterStatus(new int[1]
      {
        correspondentMasterId
      }, status);
    }

    public virtual void SetCorrespondentMasterStatus(
      int[] correspondentMasterIds,
      MasterCommitmentStatus status)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (SetCorrespondentMasterStatus), new object[2]
      {
        (object) correspondentMasterIds,
        (object) status
      });
      try
      {
        CorrespondentMasters.SetCorrespondentMasterStatus(correspondentMasterIds, status, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ICursor OpenCorrespondentMasterCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (OpenCorrespondentMasterCursor), new object[2]
      {
        (object) criteria,
        (object) sortOrder
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<CorrespondentMasterCursor>().Initialize(this.Session, CorrespondentMasters.QueryCorrespondentMasterIds(this.Session.GetUserInfo(), criteria, sortOrder, isExternalOrganization));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual void AddCorrespondentMasterHistoryItem(CorrespondentMasterHistoryItem item)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (AddCorrespondentMasterHistoryItem), new object[1]
      {
        (object) item
      });
      try
      {
        CorrespondentMasters.AddCorrespondentMasterHistoryItem(item);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual CorrespondentMasterHistoryItem[] GetCorrespondentMasterHistory(
      int correspondentMasterId)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (GetCorrespondentMasterHistory), new object[1]
      {
        (object) correspondentMasterId
      });
      try
      {
        return CorrespondentMasters.GetCorrespondentMasterHistory(correspondentMasterId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return (CorrespondentMasterHistoryItem[]) null;
      }
    }

    public virtual bool IsAllLoanPurcahsedInMaster(int[] correspondentMasterIds)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (IsAllLoanPurcahsedInMaster), new object[1]
      {
        (object) correspondentMasterIds
      });
      try
      {
        return CorrespondentMasters.IsAllLoanPurcahsedInMaster(correspondentMasterIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool AreTradesInCorrespondentMasters()
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (AreTradesInCorrespondentMasters), Array.Empty<object>());
      try
      {
        return CorrespondentMasters.AreTradesInCorrespondentMasters();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual CorrespondentMasterSummaryInfo GetCorrespondentMasterSummary(int id)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), "GetTotalGainLossAmount", Array.Empty<object>());
      try
      {
        return CorrespondentMasters.GetCorrespondentMasterSummary(id);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return (CorrespondentMasterSummaryInfo) null;
      }
    }

    public virtual CorrespondentMasterInfo[] GetCorrespondentMastersByTPOID(string oId)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (GetCorrespondentMastersByTPOID), new object[1]
      {
        (object) oId
      });
      try
      {
        return CorrespondentMasters.GetCorrespondentMastersByTPOID(oId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return (CorrespondentMasterInfo[]) null;
      }
    }

    public virtual CorrespondentMasterInfo[] GetCorrespondentMastersByOrganizationID(string oId)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (GetCorrespondentMastersByOrganizationID), new object[1]
      {
        (object) oId
      });
      try
      {
        return CorrespondentMasters.GetCorrespondentMastersByOrgID(oId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return (CorrespondentMasterInfo[]) null;
      }
    }

    public virtual CorrespondentMasterInfo GetCorrespondentMastersByContractNumber(string oId)
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (GetCorrespondentMastersByContractNumber), new object[1]
      {
        (object) oId
      });
      try
      {
        return CorrespondentMasters.GetCorrespondentMastersByContractNumber(oId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return (CorrespondentMasterInfo) null;
      }
    }

    public virtual string GetNextAutoCreateMasterName()
    {
      this.onApiCalled(nameof (CorrespondentMasterManager), nameof (GetNextAutoCreateMasterName), Array.Empty<object>());
      try
      {
        return CorrespondentMasters.GetNextAutoCreateMasterName();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CorrespondentMasterManager), ex, this.Session.SessionInfo);
        return "";
      }
    }
  }
}

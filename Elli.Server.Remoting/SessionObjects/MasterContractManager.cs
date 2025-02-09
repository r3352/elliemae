// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.MasterContractManager
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
  public class MasterContractManager : SessionBoundObject, IMasterContractManager
  {
    private const string className = "MasterContractManager";
    private string[] defaultMasterContractFields;
    private string[] defaultTradeFields;

    public MasterContractManager()
    {
      this.defaultMasterContractFields = new string[2]
      {
        "MasterContracts.ContractID",
        "MasterContracts.ContractNumber"
      };
      this.defaultTradeFields = new string[2]
      {
        "Trades.TradeID",
        "Trades.Name"
      };
    }

    public MasterContractManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (MasterContractManager).ToLower());
      return this;
    }

    public virtual MasterContractInfo GetContract(int contractId)
    {
      this.onApiCalled(nameof (MasterContractManager), nameof (GetContract), new object[1]
      {
        (object) contractId
      });
      try
      {
        return MasterContracts.GetContract(contractId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MasterContractManager), ex, this.Session.SessionInfo);
        return (MasterContractInfo) null;
      }
    }

    public virtual MasterContractInfo GetContractByContractNumber(string contractNumber)
    {
      this.onApiCalled(nameof (MasterContractManager), nameof (GetContractByContractNumber), new object[1]
      {
        (object) contractNumber
      });
      try
      {
        return MasterContracts.GetContractByContractNumber(contractNumber);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MasterContractManager), ex, this.Session.SessionInfo);
        return (MasterContractInfo) null;
      }
    }

    public virtual MasterContractSummaryInfo[] GetAllContracts(bool includeTradeData)
    {
      this.onApiCalled(nameof (MasterContractManager), nameof (GetAllContracts), new object[1]
      {
        (object) includeTradeData
      });
      try
      {
        return MasterContracts.GetAllContracts(includeTradeData);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MasterContractManager), ex, this.Session.SessionInfo);
        return (MasterContractSummaryInfo[]) null;
      }
    }

    public virtual MasterContractSummaryInfo[] GetActiveContracts(bool includeTradeData)
    {
      this.onApiCalled(nameof (MasterContractManager), nameof (GetActiveContracts), new object[1]
      {
        (object) includeTradeData
      });
      try
      {
        return MasterContracts.GetActiveContracts(includeTradeData);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MasterContractManager), ex, this.Session.SessionInfo);
        return (MasterContractSummaryInfo[]) null;
      }
    }

    public virtual MasterContractSummaryInfo[] GetArchivedContracts(bool includeTradeData)
    {
      this.onApiCalled(nameof (MasterContractManager), nameof (GetArchivedContracts), new object[1]
      {
        (object) includeTradeData
      });
      try
      {
        return MasterContracts.GetArchivedContracts(includeTradeData);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MasterContractManager), ex, this.Session.SessionInfo);
        return (MasterContractSummaryInfo[]) null;
      }
    }

    public virtual int CreateContract(MasterContractInfo contract)
    {
      this.onApiCalled(nameof (MasterContractManager), nameof (CreateContract), new object[1]
      {
        (object) contract
      });
      try
      {
        return MasterContracts.CreateContract(contract);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MasterContractManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void UpdateContract(MasterContractInfo contract)
    {
      this.onApiCalled(nameof (MasterContractManager), nameof (UpdateContract), new object[1]
      {
        (object) contract
      });
      try
      {
        MasterContracts.UpdateContract(contract);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MasterContractManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteContract(int contractId)
    {
      this.onApiCalled(nameof (MasterContractManager), nameof (DeleteContract), new object[1]
      {
        (object) contractId
      });
      try
      {
        MasterContracts.DeleteContract(contractId, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MasterContractManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetContractStatus(int contractId, MasterContractStatus status)
    {
      this.SetContractStatus(new int[1]{ contractId }, status);
    }

    public virtual void SetContractStatus(int[] contractIds, MasterContractStatus status)
    {
      this.onApiCalled(nameof (MasterContractManager), nameof (SetContractStatus), new object[2]
      {
        (object) contractIds,
        (object) status
      });
      try
      {
        MasterContracts.SetContractStatus(contractIds, status);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MasterContractManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ICursor OpenMasterContractCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (MasterContractManager), nameof (OpenMasterContractCursor), new object[4]
      {
        (object) criteria,
        (object) sortOrder,
        (object) fields,
        (object) summaryOnly
      });
      try
      {
        if (fields == null || fields.Length == 0)
          fields = this.defaultMasterContractFields;
        return (ICursor) InterceptionUtils.NewInstance<MasterContractCursor>().Initialize(this.Session, MasterContracts.QueryMasterContractIds(this.Session.GetUserInfo(), criteria, sortOrder, isExternalOrganization), fields, summaryOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MasterContractManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }
  }
}

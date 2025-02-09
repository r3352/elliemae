// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.CampaignManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Cursors;
using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Server;
using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class CampaignManager : SessionBoundObject, ICampaign
  {
    private const string className = "CampaignManager";

    public CampaignManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (CampaignManager).ToLower());
      return this;
    }

    public virtual int[] GetCampaignQueryListForUser(string userId)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (GetCampaignQueryListForUser), new object[1]
      {
        (object) userId
      });
      try
      {
        return CampaignProvider.GetCampaignQueryListForUser(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (int[]) null;
      }
    }

    public virtual CampaignInfo GetCampaignFromStep(int campaignStepID)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (GetCampaignFromStep), new object[1]
      {
        (object) campaignStepID
      });
      try
      {
        return CampaignProvider.GetCampaign(CampaignProvider.GetCampaignStepInfo(campaignStepID).CampaignId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignInfo) null;
      }
    }

    public virtual CampaignStepInfo GetCampaignStepInfo(int campaignStepID)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (GetCampaignStepInfo), new object[1]
      {
        (object) campaignStepID
      });
      try
      {
        return CampaignProvider.GetCampaignStepInfo(campaignStepID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignStepInfo) null;
      }
    }

    public virtual CampaignInfo RunCampaignQueries(int campaignId)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (RunCampaignQueries), new object[1]
      {
        (object) campaignId
      });
      try
      {
        return CampaignProvider.RunCampaignQueries(UserStore.GetLatestVersion(this.Session.UserID), campaignId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignInfo) null;
      }
    }

    public virtual CampaignInfo[] GetCampaignsForUser(CampaignCollectionCriteria criteria)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (GetCampaignsForUser), new object[1]
      {
        (object) criteria.UserId
      });
      try
      {
        return CampaignProvider.GetCampaignsForUser(criteria);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignInfo[]) null;
      }
    }

    public virtual CampaignInfo GetCampaign(int campaignId)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (GetCampaign), new object[1]
      {
        (object) campaignId
      });
      try
      {
        return CampaignProvider.GetCampaign(campaignId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignInfo) null;
      }
    }

    public virtual CampaignInfo SaveCampaign(CampaignInfo campaignInfo)
    {
      this.onApiCalled(nameof (CampaignManager), "UpdateCampaign", new object[1]
      {
        (object) campaignInfo.CampaignId
      });
      try
      {
        return CampaignProvider.SaveCampaign(campaignInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignInfo) null;
      }
    }

    public virtual void DeleteCampaign(int campaignId)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (DeleteCampaign), new object[1]
      {
        (object) campaignId
      });
      try
      {
        CampaignProvider.DeleteCampaign(campaignId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CopyCampaign(
      int oldCampaignId,
      bool copyContacts,
      string newCampaignName,
      string newCampaignDesc)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (CopyCampaign), new object[1]
      {
        (object) oldCampaignId
      });
      try
      {
        CampaignProvider.CopyCampaign(oldCampaignId, copyContacts, newCampaignName, newCampaignDesc);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual CampaignInfo StartCampaign(int campaignId)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (StartCampaign), new object[1]
      {
        (object) campaignId
      });
      try
      {
        return CampaignProvider.StartCampaign(UserStore.GetLatestVersion(this.Session.UserID), campaignId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignInfo) null;
      }
    }

    public virtual CampaignInfo StopCampaign(int campaignId)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (StopCampaign), new object[1]
      {
        (object) campaignId
      });
      try
      {
        return CampaignProvider.StopCampaign(campaignId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignInfo) null;
      }
    }

    public virtual CampaignContactInfo[] GetCampaignContacts(
      CampaignContactCollectionCritera criteria)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (GetCampaignContacts), new object[1]
      {
        (object) criteria.CampaignId
      });
      try
      {
        return CampaignProvider.GetCampaignContacts(criteria, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignContactInfo[]) null;
      }
    }

    public virtual CampaignInfo UpdateCampaignContacts(
      int campaignId,
      CrudRequestParameter[] crudRequests)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (UpdateCampaignContacts), new object[1]
      {
        (object) campaignId
      });
      try
      {
        return CampaignProvider.UpdateCampaignContacts(campaignId, crudRequests);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignInfo) null;
      }
    }

    public virtual CampaignTasksDueInfo[] GetCampaignsTasksDue()
    {
      string userId = UserStore.GetLatestVersion(this.Session.UserID).UserID;
      this.onApiCalled(nameof (CampaignManager), nameof (GetCampaignsTasksDue), new object[2]
      {
        (object) userId,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return CampaignProvider.GetCampaignsTasksDue(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignTasksDueInfo[]) null;
      }
    }

    public virtual int GetTasksDueForUser(string userId)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (GetTasksDueForUser), new object[1]
      {
        (object) userId
      });
      try
      {
        return CampaignProvider.GetTasksDueForUser(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual CampaignStepInfo GetCampaignStepActivity(
      CampaignActivityCollectionCriteria criteria)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (GetCampaignStepActivity), new object[1]
      {
        (object) criteria.CampaignStepId
      });
      try
      {
        return CampaignProvider.GetCampaignStepActivity(criteria);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignStepInfo) null;
      }
    }

    public virtual CampaignStepInfo UpdateCampaignStepActiviity(
      CampaignActivityCollectionCriteria criteria,
      ActivityUpdateParameter activityUpdateParameter)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (UpdateCampaignStepActiviity), new object[1]
      {
        (object) criteria.CampaignStepId
      });
      try
      {
        return CampaignProvider.UpdateCampaignStepActiviity(criteria, activityUpdateParameter);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (CampaignStepInfo) null;
      }
    }

    public virtual ICursor OpenCampaignContactCursor(CampaignContactCollectionCritera criteria)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (OpenCampaignContactCursor), new object[1]
      {
        (object) criteria.CampaignId
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<CampaignContactCursor>().Initialize(this.Session, CampaignProvider.GetCampaignContacts(criteria, this.Session.UserID));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenCampaignHistoryCursor(
      CampaignHistoryCollectionCritera criteria,
      SortField[] sortFields)
    {
      this.onApiCalled(nameof (CampaignManager), nameof (OpenCampaignHistoryCursor), new object[1]
      {
        (object) criteria.CampaignId
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<CampaignHistoryCursor>().Initialize(this.Session, CampaignProvider.GetCampaignHistory(criteria, sortFields));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CampaignManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }
  }
}

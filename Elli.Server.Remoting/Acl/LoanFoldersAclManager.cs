// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.LoanFoldersAclManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Acl.Interfaces;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class LoanFoldersAclManager : SessionBoundObject, ILoanFoldersAclManager
  {
    private const string className = "LoanFoldersAclManager";
    private const string objKeyPrefix = "aclmanager#";
    private const string featuresDefaultCacheName = "AclFeaturesDefault";

    public LoanFoldersAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 6);
      return this;
    }

    public virtual LoanFolderAclInfo[] GetAccessibleLoanFolders(
      AclFeature feature,
      string userId,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (GetAccessibleLoanFolders), new object[3]
      {
        (object) feature,
        (object) userId,
        (object) personaList
      });
      try
      {
        return LoanFoldersAclDbAccessor.GetUserApplicationLoanFolders(feature, userId, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
        return (LoanFolderAclInfo[]) null;
      }
    }

    public virtual LoanFolderAclInfo GetAccessibleLoanFolder(
      AclFeature feature,
      string userId,
      Persona[] personaList,
      string folderName)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (GetAccessibleLoanFolder), new object[4]
      {
        (object) feature,
        (object) userId,
        (object) personaList,
        (object) folderName
      });
      try
      {
        return LoanFoldersAclDbAccessor.GetUserApplicationLoanFolder(feature, folderName, userId, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
        return (LoanFolderAclInfo) null;
      }
    }

    public virtual LoanFolderAclInfo GetPermission(
      AclFeature feature,
      int personaID,
      string folderName)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (GetPermission), new object[3]
      {
        (object) feature,
        (object) personaID,
        (object) folderName
      });
      try
      {
        LoanFolderAclInfo[] loanFolders = this.GetLoanFolders(feature, folderName, personaID.ToString(), false);
        return loanFolders != null && loanFolders.Length != 0 ? loanFolders[0] : (LoanFolderAclInfo) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
        return (LoanFolderAclInfo) null;
      }
    }

    public virtual LoanFolderAclInfo GetPermission(
      AclFeature feature,
      Persona[] personaList,
      string folderName)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (GetPermission), new object[3]
      {
        (object) feature,
        (object) personaList,
        (object) folderName
      });
      try
      {
        return this.GetLoanFolder(feature, folderName, personaList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
        return (LoanFolderAclInfo) null;
      }
    }

    public virtual LoanFolderAclInfo[] GetPermissions(AclFeature feature, int personaID)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (GetPermissions), new object[2]
      {
        (object) feature,
        (object) personaID
      });
      try
      {
        return this.GetLoanFolders(feature, "", personaID.ToString(), false);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
        return (LoanFolderAclInfo[]) null;
      }
    }

    public virtual LoanFolderAclInfo GetPermission(
      AclFeature feature,
      string userID,
      string folderName)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (GetPermission), new object[3]
      {
        (object) feature,
        (object) userID,
        (object) folderName
      });
      try
      {
        LoanFolderAclInfo[] loanFolders = this.GetLoanFolders(feature, folderName, userID, true);
        return loanFolders != null && loanFolders.Length != 0 ? loanFolders[0] : (LoanFolderAclInfo) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
        return (LoanFolderAclInfo) null;
      }
    }

    public virtual LoanFolderAclInfo[] GetPermissions(AclFeature feature, string userID)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (GetPermissions), new object[2]
      {
        (object) feature,
        (object) userID
      });
      try
      {
        return this.GetLoanFolders(feature, "", userID, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
        return (LoanFolderAclInfo[]) null;
      }
    }

    public virtual void SetPermission(
      AclFeature feature,
      LoanFolderAclInfo loanFolderAclInfo,
      string userid)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (SetPermission), new object[3]
      {
        (object) feature,
        (object) loanFolderAclInfo,
        (object) userid
      });
      try
      {
        this.SetPermissions(feature, new LoanFolderAclInfo[1]
        {
          loanFolderAclInfo
        }, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermission(
      AclFeature feature,
      LoanFolderAclInfo loanFolderAclInfo,
      int personaID)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (SetPermission), new object[3]
      {
        (object) feature,
        (object) loanFolderAclInfo,
        (object) personaID
      });
      try
      {
        this.SetPermissions(feature, new LoanFolderAclInfo[1]
        {
          loanFolderAclInfo
        }, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(
      AclFeature feature,
      LoanFolderAclInfo[] loanFolderAclInfoList,
      string userid)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (SetPermissions), new object[3]
      {
        (object) feature,
        (object) loanFolderAclInfoList,
        (object) userid
      });
      try
      {
        this.UpdateLoanFolderSettings(feature, loanFolderAclInfoList, userid, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(
      AclFeature feature,
      LoanFolderAclInfo[] loanFolderAclInfoList,
      int personaID)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (SetPermissions), new object[3]
      {
        (object) feature,
        (object) loanFolderAclInfoList,
        (object) personaID
      });
      try
      {
        this.UpdateLoanFolderSettings(feature, loanFolderAclInfoList, personaID.ToString(), false);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateACLLoanFolder(int sourcePersonaID, int desPersonaID)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (DuplicateACLLoanFolder), new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        LoanFoldersAclDbAccessor.DuplicateACLLoanFolders(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
      }
    }

    private LoanFolderAclInfo[] GetLoanFolders(
      AclFeature feature,
      string folderName,
      string keyID,
      bool isUser)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (GetLoanFolders), new object[4]
      {
        (object) feature,
        (object) folderName,
        (object) keyID,
        (object) isUser
      });
      try
      {
        return isUser ? LoanFoldersAclDbAccessor.GetUserLoanFolders(feature, folderName, keyID) : LoanFoldersAclDbAccessor.GetPersonaLoanFolders(feature, folderName, keyID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
      }
      return (LoanFolderAclInfo[]) null;
    }

    private LoanFolderAclInfo GetLoanFolder(
      AclFeature feature,
      string folderName,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), "GetLoanFolders", new object[3]
      {
        (object) feature,
        (object) folderName,
        (object) personaList
      });
      try
      {
        return LoanFoldersAclDbAccessor.GetLoanFolderAclInfo(feature, folderName, personaList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
      }
      return (LoanFolderAclInfo) null;
    }

    private void UpdateLoanFolderSettings(
      AclFeature feature,
      LoanFolderAclInfo[] loanFolderAclInfoList,
      string keyID,
      bool isUser)
    {
      this.onApiCalled(nameof (LoanFoldersAclManager), nameof (UpdateLoanFolderSettings), new object[4]
      {
        (object) feature,
        (object) loanFolderAclInfoList,
        (object) keyID,
        (object) isUser
      });
      try
      {
        if (isUser)
          LoanFoldersAclDbAccessor.UpdateUserLoanFolderConfiguration(feature, loanFolderAclInfoList, keyID);
        else
          LoanFoldersAclDbAccessor.UpdatePersonaLoanFolderConfiguration(feature, loanFolderAclInfoList, keyID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFoldersAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}

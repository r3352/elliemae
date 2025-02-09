// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.ToolsAclManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Acl.Interfaces;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class ToolsAclManager : SessionBoundObject, IToolsAclManager
  {
    private const string className = "ToolsAclManager";
    private const string objKeyPrefix = "aclmanager#";
    private const string featuresDefaultCacheName = "AclFeaturesDefault";

    public ToolsAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 7);
      return this;
    }

    public virtual ToolsAclInfo[] GetAccessibleToolsAclInfo(string userId, Persona[] personaList)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (GetAccessibleToolsAclInfo), new object[2]
      {
        (object) userId,
        (object) personaList
      });
      try
      {
        return this.GetUserApplicationToolsSetting("", -1, userId, personaList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
        return (ToolsAclInfo[]) null;
      }
    }

    public virtual ToolsAclInfo GetAccessibleToolsAclInfo(
      string userId,
      Persona[] personaList,
      int roleID,
      string milestoneID)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (GetAccessibleToolsAclInfo), new object[4]
      {
        (object) userId,
        (object) personaList,
        (object) roleID,
        (object) milestoneID
      });
      try
      {
        ToolsAclInfo[] applicationToolsSetting = this.GetUserApplicationToolsSetting(milestoneID, roleID, userId, personaList);
        return applicationToolsSetting != null && applicationToolsSetting.Length != 0 ? applicationToolsSetting[0] : (ToolsAclInfo) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
        return (ToolsAclInfo) null;
      }
    }

    public virtual ToolsAclInfo[] GetFileContactGrantWriteAccessRights(
      string userID,
      Persona[] personaList,
      string[] loanMilestoneIDs,
      int[] loanFreeRoleIDs)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (GetFileContactGrantWriteAccessRights), new object[4]
      {
        (object) userID,
        (object) personaList,
        (object) loanMilestoneIDs,
        (object) loanFreeRoleIDs
      });
      try
      {
        return ToolsAclDbAccessor.GetFileContactsGrantWriteAccessRights(userID, personaList, loanMilestoneIDs, loanFreeRoleIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
        return (ToolsAclInfo[]) null;
      }
    }

    public virtual ToolsAclInfo GetPermission(int personaID, int roleID)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (GetPermission), new object[2]
      {
        (object) personaID,
        (object) roleID
      });
      try
      {
        ToolsAclInfo[] tools = this.GetTools("", roleID, personaID.ToString(), false);
        return tools != null && tools.Length != 0 ? tools[0] : (ToolsAclInfo) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
        return (ToolsAclInfo) null;
      }
    }

    public virtual ToolsAclInfo GetPermission(
      Persona[] personaList,
      int roleID,
      string milestoneID)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (GetPermission), new object[3]
      {
        (object) personaList,
        (object) roleID,
        (object) milestoneID
      });
      try
      {
        return this.GetTool(milestoneID, roleID, personaList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
        return (ToolsAclInfo) null;
      }
    }

    public virtual ToolsAclInfo[] GetPermissions(int personaID)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (GetPermissions), new object[1]
      {
        (object) personaID
      });
      try
      {
        return this.GetTools("", -1, personaID.ToString(), false);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
        return (ToolsAclInfo[]) null;
      }
    }

    public virtual ToolsAclInfo GetPermission(string userID, int roleID)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (GetPermission), new object[2]
      {
        (object) userID,
        (object) roleID
      });
      try
      {
        ToolsAclInfo[] tools = this.GetTools("", roleID, userID, true);
        return tools != null && tools.Length != 0 ? tools[0] : (ToolsAclInfo) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
        return (ToolsAclInfo) null;
      }
    }

    public virtual ToolsAclInfo[] GetPermissions(string userID)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (GetPermissions), new object[1]
      {
        (object) userID
      });
      try
      {
        return this.GetTools("", -1, userID, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
        return (ToolsAclInfo[]) null;
      }
    }

    public virtual void SetPermission(ToolsAclInfo toolsAclInfo, string userid)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (SetPermission), new object[2]
      {
        (object) toolsAclInfo,
        (object) userid
      });
      try
      {
        this.SetPermissions(new ToolsAclInfo[1]
        {
          toolsAclInfo
        }, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermission(ToolsAclInfo toolsAclInfo, int personaID)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (SetPermission), new object[2]
      {
        (object) toolsAclInfo,
        (object) personaID
      });
      try
      {
        this.SetPermissions(new ToolsAclInfo[1]
        {
          toolsAclInfo
        }, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(ToolsAclInfo[] toolsAclInfoList, string userid)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (SetPermissions), new object[2]
      {
        (object) toolsAclInfoList,
        (object) userid
      });
      try
      {
        this.UpdateToolsSettings(toolsAclInfoList, userid, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(ToolsAclInfo[] toolsAclInfoList, int personaID)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (SetPermissions), new object[2]
      {
        (object) toolsAclInfoList,
        (object) personaID
      });
      try
      {
        this.UpdateToolsSettings(toolsAclInfoList, personaID.ToString(), false);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateACLTools(int sourcePersonaID, int desPersonaID)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (DuplicateACLTools), new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        ToolsAclDbAccessor.DuplicateACLTool(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SynchronizeAdminRight()
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (SynchronizeAdminRight), Array.Empty<object>());
      try
      {
        ToolsAclDbAccessor.SynchronizeAdminSetting();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SynchronizeBrokerSetting(string baseMilestoneID, string currentMilestoneID)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (SynchronizeBrokerSetting), new object[2]
      {
        (object) baseMilestoneID,
        (object) currentMilestoneID
      });
      try
      {
        ToolsAclDbAccessor.SynchronizeBrokerSetting(baseMilestoneID, currentMilestoneID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
    }

    private ToolsAclInfo[] GetUserApplicationToolsSetting(
      string milestoneID,
      int roleID,
      string userID,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (ToolsAclManager), "GetUserApplicationLoanFolder", new object[3]
      {
        (object) roleID,
        (object) userID,
        (object) personaList
      });
      try
      {
        return ToolsAclDbAccessor.GetUserApplicationToolsSetting(milestoneID, roleID, userID, personaList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
      return (ToolsAclInfo[]) null;
    }

    private ToolsAclInfo[] GetTools(string milestoneID, int roleID, string keyID, bool isUser)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (GetTools), new object[3]
      {
        (object) roleID,
        (object) keyID,
        (object) isUser
      });
      try
      {
        return isUser ? ToolsAclDbAccessor.GetUserToolsConfiguration(milestoneID, roleID, keyID) : ToolsAclDbAccessor.GetPersonaToolsConfiguration(milestoneID, roleID, keyID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
      return (ToolsAclInfo[]) null;
    }

    private ToolsAclInfo GetTool(string milestoneID, int roleID, Persona[] personaList)
    {
      this.onApiCalled(nameof (ToolsAclManager), nameof (GetTool), new object[3]
      {
        (object) milestoneID,
        (object) roleID,
        (object) personaList
      });
      try
      {
        return ToolsAclDbAccessor.GetPersonaToolsConfiguration(milestoneID, roleID, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
      return (ToolsAclInfo) null;
    }

    private void UpdateToolsSettings(
      ToolsAclInfo[] toolsAclInfoInfoList,
      string keyID,
      bool isUser)
    {
      this.onApiCalled(nameof (ToolsAclManager), "UpdateLoanFolderSettings", new object[3]
      {
        (object) toolsAclInfoInfoList,
        (object) keyID,
        (object) isUser
      });
      try
      {
        if (isUser)
          ToolsAclDbAccessor.UpdateUserToolsConfiguration(toolsAclInfoInfoList, keyID);
        else
          ToolsAclDbAccessor.UpdatePersonaToolsConfiguration(toolsAclInfoInfoList, keyID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ToolsAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}

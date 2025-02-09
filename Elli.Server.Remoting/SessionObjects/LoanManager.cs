// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.LoanManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.ElliEnum;
using Elli.Server.Remoting.Cursors;
using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Server.ServerObjects.StatusOnline;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class LoanManager : SessionBoundObject, ILoanManager
  {
    private const string className = "LoanManager";

    public LoanManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (LoanManager).ToLower());
      return this;
    }

    public virtual LoanTemplateInfo[] GetLoanTemplates()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanTemplates), Array.Empty<object>());
      try
      {
        return EllieMae.EMLite.Server.LoanTemplate.GetLoanTemplates();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanTemplateInfo[]) null;
      }
    }

    public virtual LoanData InstantiateLoanTemplate(string name)
    {
      this.onApiCalled(nameof (LoanManager), nameof (InstantiateLoanTemplate), new object[1]
      {
        (object) name
      });
      if ((name ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Template name cannot be blank or null", nameof (name)));
      try
      {
        using (PerformanceMeter.StartNew("LoanManager.InstantiateLoanTemplate", 74, nameof (InstantiateLoanTemplate), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs"))
          return new EllieMae.EMLite.Server.LoanTemplate(name).Instantiate(this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanData) null;
      }
    }

    public virtual LoanData InstantiateLoanTemplateFromPlatform(string name)
    {
      this.onApiCalled(nameof (LoanManager), "InstantiateLoanTemplateFromPlatfrom", new object[1]
      {
        (object) name
      });
      if ((name ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Template name cannot be blank or null", nameof (name)));
      try
      {
        using (PerformanceMeter.StartNew("LoanManager.InstantiateLoanTemplateFromPlatfrom", 95, nameof (InstantiateLoanTemplateFromPlatform), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs"))
          return new EllieMae.EMLite.Server.LoanTemplate(name).Instantiate(this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanData) null;
      }
    }

    public virtual ILoan OpenLoan(LoanIdentity id)
    {
      return this.OpenLoan(id, LoanInfo.LockReason.NotLocked);
    }

    public virtual ILoan OpenLoan(string guid)
    {
      if ((guid ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid cannot be blank or null", nameof (guid)));
      return this.OpenLoan(new LoanIdentity(guid));
    }

    public virtual ILoan OpenLoan(string guid, LoanInfo.LockReason reason)
    {
      if ((guid ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid cannot be blank or null", nameof (guid)));
      return this.OpenLoan(new LoanIdentity(guid), reason);
    }

    public virtual ILoan OpenLoan(string folderName, string loanName)
    {
      return this.OpenLoan(new LoanIdentity(folderName, loanName), LoanInfo.LockReason.NotLocked);
    }

    public virtual ILoan OpenLoan(string folderName, string loanName, LoanInfo.LockReason reason)
    {
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (folderName)));
      if ((loanName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan name cannot be blank or null", nameof (loanName)));
      return this.OpenLoan(new LoanIdentity(folderName, loanName), reason);
    }

    public virtual ILoan OpenLoan(LoanIdentity id, LoanInfo.LockReason reason)
    {
      this.onApiCalled(nameof (LoanManager), nameof (OpenLoan), new object[2]
      {
        (object) id,
        (object) reason
      });
      return this.OpenLoanInternal(id, reason);
    }

    private ILoan OpenLoanInternal(LoanIdentity id, LoanInfo.LockReason reason)
    {
      if (id == (LoanIdentity) null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan Identity name cannot be null", nameof (id)));
      try
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("LoanManager.OpenLoan", 164, nameof (OpenLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs"))
        {
          if (id.Guid == "")
            id = Loan.LookupIdentity(id.LoanFolder, id.LoanName);
          if (id == (LoanIdentity) null)
            return (ILoan) null;
          performanceMeter.AddVariable("User", (object) this.Session.UserID);
          performanceMeter.AddVariable("Guid", (object) id.Guid);
          using (Loan latestVersion = LoanStore.GetLatestVersion(id.Guid))
          {
            if (!latestVersion.Exists)
              return (ILoan) null;
            if (reason == LoanInfo.LockReason.NotLocked)
              ((SecurityManager) this.Security).DemandLoanRights(latestVersion, LoanInfo.Right.Read);
            else
              ((SecurityManager) this.Security).DemandLoanRights(latestVersion, LoanInfo.Right.Access);
          }
          LoanProxy loanProxy = this.createLoanProxy(id.Guid);
          this.raiseEvent(LoanEventType.Opened, id);
          if (reason != LoanInfo.LockReason.NotLocked)
            loanProxy.Lock(reason, LockInfo.ExclusiveLock.Nonexclusive);
          if (this.Session.Context.IsConcurrentUpdateNotificationEnabled)
            this.Session.ConcurrentUpdatesNotificationCache.TryRemove(id.Guid, out string _);
          return (ILoan) loanProxy;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
      return (ILoan) null;
    }

    public virtual ILoan CreateLoan(
      string folderName,
      string loanName,
      LoanData data,
      DuplicateLoanAction dupAction)
    {
      return this.CreateLoan(folderName, loanName, data, dupAction, true);
    }

    public virtual ILoan CreateLoan(
      string folderName,
      string loanName,
      LoanData data,
      DuplicateLoanAction dupAction,
      bool addLoanNumber)
    {
      this.onApiCalled(nameof (LoanManager), nameof (CreateLoan), new object[4]
      {
        (object) folderName,
        (object) loanName,
        (object) data,
        (object) dupAction
      });
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (folderName)));
      if ((loanName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan name cannot be blank or null", nameof (loanName)));
      if (data == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan data cannot be null", nameof (data)));
      try
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("LoanManager.CreateLoan", 240, nameof (CreateLoan), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs"))
        {
          data.Parse();
          performanceMeter.AddCheckpoint("Uploaded & parsed LoanData object...", 246, nameof (CreateLoan), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
          performanceMeter.AddVariable("User", (object) this.Session.UserID);
          performanceMeter.AddVariable("Guid", (object) data.GUID);
          data = data.MergeChanges();
          performanceMeter.AddCheckpoint("LoanData changes merged...", 251, nameof (CreateLoan), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
          if (addLoanNumber)
          {
            this.autoAssignLoanNumbers(data);
            performanceMeter.AddCheckpoint("Loan number assigned...", 257, nameof (CreateLoan), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
          }
          return this.createLoanInternal(folderName, loanName, data, dupAction);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return (ILoan) null;
      }
    }

    public virtual void DeleteLoan(string guid) => this.DeleteLoan(guid, true, false);

    public virtual void DeleteLoan(string guid, bool demandAdmin)
    {
      this.DeleteLoan(guid, demandAdmin, false);
    }

    public virtual void DeleteLoan(string guid, bool demandAdmin, bool skipSystemLogging)
    {
      this.onApiCalled(nameof (LoanManager), nameof (DeleteLoan), new object[3]
      {
        (object) guid,
        (object) demandAdmin,
        (object) skipSystemLogging
      });
      try
      {
        if ((guid ?? "") == "")
          Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid cannot be blank or null", nameof (guid)));
        this.OpenLoan(guid)?.Delete(demandAdmin, skipSystemLogging);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
      }
    }

    public virtual LoanIdentity GetLoanIdentity(string guid)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanIdentity), new object[1]
      {
        (object) guid
      });
      if ((guid ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid cannot be blank or null", nameof (guid)));
      try
      {
        return Loan.LookupIdentity(guid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanIdentity) null;
      }
    }

    public virtual LoanIdentity GetLoanIdentity(string folderName, string loanName)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanIdentity), new object[2]
      {
        (object) folderName,
        (object) loanName
      });
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (folderName)));
      if ((loanName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan name cannot be blank or null", nameof (loanName)));
      if (!LoanIdentity.IsValidName(folderName))
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name contains invalid characters", nameof (folderName)));
      try
      {
        return Loan.LookupIdentity(folderName, loanName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanIdentity) null;
      }
    }

    public virtual LoanProperties GetLoanProperties(string guid)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanProperties), new object[1]
      {
        (object) guid
      });
      try
      {
        using (Loan latestVersion = LoanStore.GetLatestVersion(guid))
          return !latestVersion.Exists ? (LoanProperties) null : latestVersion.GetProperties();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanProperties) null;
      }
    }

    public virtual bool LoanExists(string guid)
    {
      this.onApiCalled(nameof (LoanManager), nameof (LoanExists), new object[1]
      {
        (object) guid
      });
      if ((guid ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid cannot be blank or null", nameof (guid)));
      try
      {
        return LoanStore.GetLatestVersion(guid).Exists;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool LoanExists(string folderName, string loanName)
    {
      this.onApiCalled(nameof (LoanManager), nameof (LoanExists), new object[2]
      {
        (object) folderName,
        (object) loanName
      });
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (folderName)));
      if ((loanName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan name cannot be blank or null", nameof (loanName)));
      if (!LoanIdentity.IsValidName(folderName))
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name contains invalid characters", nameof (folderName)));
      try
      {
        return Loan.LookupIdentity(folderName, loanName) != (LoanIdentity) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual string GetNextLoanNumber(OrgInfo orgInfo)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetNextLoanNumber), new object[1]
      {
        (object) orgInfo
      });
      try
      {
        if (orgInfo == null)
          orgInfo = OrganizationStore.GetFirstAvaliableOrganization(this.Session.GetUserInfo().OrgId);
        return LoanNumberGenerator.GetNextLoanNumber(orgInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual string GetNextLoanNumber() => this.GetNextLoanNumber((OrgInfo) null);

    public virtual string GetNextMersNumber(bool alwaysGenerate, OrgInfo orgInfo)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetNextMersNumber), new object[2]
      {
        (object) alwaysGenerate,
        (object) orgInfo
      });
      try
      {
        if (orgInfo == null)
          orgInfo = OrganizationStore.GetFirstOrganizationWithMERSMIN(this.Session.GetUserInfo().OrgId);
        return MersNumberGenerator.GetNextMersNumber(alwaysGenerate, orgInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual string GenerateUniqueLoanName(string folder, string baseLoanName)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GenerateUniqueLoanName), new object[2]
      {
        (object) folder,
        (object) baseLoanName
      });
      try
      {
        return Loan.GenerateUniqueName(folder, baseLoanName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual void CreateLoanFolder(string name)
    {
      this.onApiCalled(nameof (LoanManager), nameof (CreateLoanFolder), new object[1]
      {
        (object) name
      });
      if ((name ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (name)));
      if (!LoanIdentity.IsValidName(name))
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name contains invalid characters", nameof (name)));
      try
      {
        new LoanFolder(name).Create();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteLoanFolder(string name, bool forceDelete)
    {
      this.onApiCalled(nameof (LoanManager), nameof (DeleteLoanFolder), new object[2]
      {
        (object) name,
        (object) forceDelete
      });
      if ((name ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (name)));
      if (!LoanIdentity.IsValidName(name))
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name contains invalid characters", nameof (name)));
      try
      {
        LoanFolder loanFolder = new LoanFolder(name);
        LoanIdentity[] contents = loanFolder.GetContents();
        if (contents.Length != 0 && !forceDelete)
          Err.Raise(TraceLevel.Warning, nameof (LoanManager), new ServerException("The specified loan folder cannot be deleted because it contains loan files."));
        for (int index = 0; index < contents.Length; ++index)
          this.OpenLoan(contents[index])?.Delete(false);
        loanFolder.Delete(forceDelete);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
      }
    }

    public virtual bool DoesLoanFolderExist(string name)
    {
      this.onApiCalled(nameof (LoanManager), nameof (DoesLoanFolderExist), new object[1]
      {
        (object) name
      });
      if ((name ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (name)));
      try
      {
        return new LoanFolder(name).Exists;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual LoanFolderInfo GetLoanFolder(string name)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanFolder), new object[1]
      {
        (object) name
      });
      if ((name ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (name)));
      if (!LoanIdentity.IsValidName(name))
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name contains invalid characters", nameof (name)));
      try
      {
        return new LoanFolder(name).GetFolderInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanFolderInfo) null;
      }
    }

    public virtual string[] GetAllLoanFolderNames(bool includeTrashFolder)
    {
      return this.GetAllLoanFolderNames(includeTrashFolder, true);
    }

    public virtual string[] GetAllLoanFolderNames(bool includeTrashFolder, bool applySecurity)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetAllLoanFolderNames), new object[1]
      {
        (object) applySecurity
      });
      try
      {
        if (!applySecurity)
          return LoanFolder.GetAllLoanFolderNames(includeTrashFolder);
        User latestVersion = UserStore.GetLatestVersion(this.Session.UserID);
        return LoanFolder.GetAllLoanFolderNames(includeTrashFolder, latestVersion.UserInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual LoanFolderInfo[] GetAllLoanFolderInfos(bool includeTrashFolder)
    {
      return this.GetAllLoanFolderInfos(includeTrashFolder, true);
    }

    public virtual LoanFolderInfo[] GetAllLoanFolderInfos(
      bool includeTrashFolder,
      bool applySecurity)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetAllLoanFolderInfos), new object[1]
      {
        (object) applySecurity
      });
      try
      {
        if (!applySecurity)
          return LoanFolder.GetAllLoanFolderInfos(includeTrashFolder);
        User latestVersion = UserStore.GetLatestVersion(this.Session.UserID);
        return LoanFolder.GetAllLoanFolderInfos(includeTrashFolder, latestVersion.UserInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanFolderInfo[]) null;
      }
    }

    public virtual int GetLoanFolderPhysicalSize(string name)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanFolderPhysicalSize), new object[1]
      {
        (object) name
      });
      if ((name ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (name)));
      if (!LoanIdentity.IsValidName(name))
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name contains invalid characters", nameof (name)));
      try
      {
        return new LoanFolder(name).GetLoanDirectoriesFromDisk().Length;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void SetLoanFolderType(
      string folderName,
      LoanFolderInfo.LoanFolderType folderType)
    {
      this.onApiCalled(nameof (LoanManager), nameof (SetLoanFolderType), Array.Empty<object>());
      try
      {
        User latestVersion = UserStore.GetLatestVersion(this.Session.UserID);
        LoanFolder.SetLoanFolderType(folderName, folderType, latestVersion.UserInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetIncludeInDuplicateLoanCheck(string folderName, bool dupLoanCheck)
    {
      this.onApiCalled(nameof (LoanManager), nameof (SetIncludeInDuplicateLoanCheck), Array.Empty<object>());
      try
      {
        LoanFolder.SetIncludeInDuplicateLoanCheck(folderName, dupLoanCheck);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool GetIncludeInDuplicateLoanCheck(string folderName)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetIncludeInDuplicateLoanCheck), Array.Empty<object>());
      try
      {
        return LoanFolder.GetIncludeInDuplicateLoanCheck(folderName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual LoanFolderInfo.LoanFolderType GetLoanFolderType(string folderName)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanFolderType), Array.Empty<object>());
      try
      {
        return LoanFolder.GetLoanFolderType(folderName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return LoanFolderInfo.LoanFolderType.NotSpecified;
      }
    }

    public virtual string GetAllIncludeInDuplicateLoanCheck()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetAllIncludeInDuplicateLoanCheck), Array.Empty<object>());
      try
      {
        return LoanFolder.GetAllIncludeInDuplicateLoanCheck();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return "";
      }
    }

    public virtual LoanIdentity[] GetLoanFolderContents(
      string folderName,
      LoanInfo.Right accessRights,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanFolderContents), new object[2]
      {
        (object) folderName,
        (object) accessRights
      });
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (folderName)));
      if (!LoanIdentity.IsValidName(folderName))
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name contains invalid characters", nameof (folderName)));
      try
      {
        return new LoanFolder(folderName).GetContentsSDK(this.Session.GetUserInfo(), accessRights, isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanIdentity[]) null;
      }
    }

    public virtual StatusOnlineLoanSetup GetStatusOnlineSetup(LoanIdentity loanid)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetStatusOnlineSetup), new object[1]
      {
        (object) loanid
      });
      try
      {
        return StatusOnlineLoanStore.GetSetup(loanid, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (StatusOnlineLoanSetup) null;
      }
    }

    public virtual StatusOnlineLoanSetup SaveStatusOnlineTriggers(
      LoanIdentity loanid,
      StatusOnlineTrigger[] triggerList)
    {
      this.onApiCalled(nameof (LoanManager), nameof (SaveStatusOnlineTriggers), new object[2]
      {
        (object) loanid,
        (object) triggerList
      });
      try
      {
        return StatusOnlineLoanStore.SaveTriggers(loanid, triggerList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (StatusOnlineLoanSetup) null;
      }
    }

    public virtual StatusOnlineLoanSetup DeleteStatusOnlineTriggers(
      LoanIdentity loanid,
      StatusOnlineTrigger[] triggerList)
    {
      this.onApiCalled(nameof (LoanManager), nameof (DeleteStatusOnlineTriggers), new object[2]
      {
        (object) loanid,
        (object) triggerList
      });
      try
      {
        return StatusOnlineLoanStore.DeleteTriggers(loanid, triggerList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (StatusOnlineLoanSetup) null;
      }
    }

    public virtual StatusOnlineLoanSetup SetStatusOnlinePrompt(LoanIdentity loanid, bool showPrompt)
    {
      this.onApiCalled(nameof (LoanManager), nameof (SetStatusOnlinePrompt), new object[2]
      {
        (object) loanid,
        (object) showPrompt
      });
      try
      {
        return StatusOnlineLoanStore.SetShowPrompt(loanid, showPrompt);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (StatusOnlineLoanSetup) null;
      }
    }

    public virtual void MoveLoan(
      LoanIdentity id,
      string targetFolder,
      DuplicateLoanAction dupAction)
    {
      this.onApiCalled(nameof (LoanManager), nameof (MoveLoan), new object[3]
      {
        (object) id,
        (object) targetFolder,
        (object) dupAction
      });
      if (id == (LoanIdentity) null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan Identity folder name cannot be null", nameof (id)));
      if ((targetFolder ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Target folder name cannot be blank or null", nameof (targetFolder)));
      if (!LoanIdentity.IsValidName(targetFolder))
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name contains invalid characters", nameof (targetFolder)));
      try
      {
        ILoan loan = this.OpenLoan(id);
        if (loan == null)
          throw new Exception("The loan no longer exists in this folder");
        if (loan.GetLockInfo().LockedFor == LoanInfo.LockReason.OpenForWork)
          throw new Exception("The loan is currently locked, opened by another user");
        try
        {
          loan.Move(targetFolder, dupAction);
        }
        finally
        {
          loan.Close();
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanIdentity[] QueryLoans(QueryCriterion[] criteria, bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (QueryLoans), (object[]) criteria);
      if (criteria == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Criteria cannot be null", "id"));
      try
      {
        return new ReportEngine(this.Session.UserID).QueryLoans(criteria, this.Session.GetUserInfo(), isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanIdentity[]) null;
      }
    }

    public virtual QueryResult QueryPipeline(DataQuery query, bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (QueryPipeline), new object[1]
      {
        (object) query
      });
      if (query == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Criteria cannot be null", "id"));
      try
      {
        return new LoanQuery(this.Session.GetUserInfo()).Execute(query, isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (QueryResult) null;
      }
    }

    public virtual QueryResult QueryPipelineFromHomepage(
      DataQuery query,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (QueryPipelineFromHomepage), new object[2]
      {
        (object) query,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.HomePage)
      });
      if (query == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Criteria cannot be null", "id"));
      try
      {
        bool useReadReplica = !Utils.ParseBoolean((object) Company.GetCompanySetting(this.Session.Context, "DisableReadReplica", DBReadReplicaFeature.HomePage.ToString()));
        return new LoanQuery(this.Session.GetUserInfo(), useReadReplica).Execute(query, isExternalOrganization, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (QueryResult) null;
      }
    }

    public virtual QueryCursor OpenQuery(DataQuery query, bool isExternalOrganization)
    {
      return this.OpenQuery(query, false, (string) null, isExternalOrganization);
    }

    public virtual QueryCursor OpenQuery(
      DataQuery query,
      bool erdb,
      string callingApplication,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (OpenQuery), new object[1]
      {
        (object) query
      });
      if (query == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Criteria cannot be null", "id"));
      try
      {
        LoanQuery loanQuery = new LoanQuery(false, this.Session.GetUserInfo());
        loanQuery.CallingApplication = callingApplication;
        QueryResult res = loanQuery.Execute(query, isExternalOrganization);
        return new QueryCursor(res.Columns, (ICursor) InterceptionUtils.NewInstance<QueryResultCursor>().Initialize(this.Session, res));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (QueryCursor) null;
      }
    }

    public virtual PipelineInfo[] GetPipeline(string loanFolder, bool isExternalOrganization)
    {
      return this.GetPipeline(loanFolder, LoanInfo.Right.Read, isExternalOrganization, 0);
    }

    public virtual PipelineInfo[] GetPipeline(
      string loanFolder,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization)
    {
      return this.GetPipeline(loanFolder, LoanInfo.Right.Read, fields, dataToInclude, isExternalOrganization);
    }

    public virtual PipelineInfo[] GetPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      bool isExternalOrganization)
    {
      return this.GetPipeline(loanFolder, rights, (string[]) null, PipelineData.All, isExternalOrganization);
    }

    private PipelineInfo[] GetPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      bool isExternalOrganization,
      int sqlRead)
    {
      return this.GetPipeline(loanFolder, rights, (string[]) null, PipelineData.All, isExternalOrganization, sqlRead);
    }

    public virtual PipelineInfo[] GetPipeline(
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization)
    {
      return this.GetPipeline((string) null, rights, fields, PipelineData.All, isExternalOrganization);
    }

    public virtual PipelineInfo[] GetPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization)
    {
      return this.GetPipeline(loanFolder, rights, fields, dataToInclude, (SortField[]) null, (QueryCriterion) null, isExternalOrganization);
    }

    public virtual PipelineInfo[] GetPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      int sqlRead)
    {
      return this.GetPipeline(loanFolder, rights, fields, dataToInclude, (SortField[]) null, (QueryCriterion) null, isExternalOrganization, sqlRead);
    }

    public virtual PipelineInfo[] GetPipeline(
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return this.GetPipeline((string) null, rights, fields, dataToInclude, sortFields, filter, isExternalOrganization);
    }

    public virtual PipelineInfo[] GetPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return this.GetPipeline(loanFolder, rights, fields, dataToInclude, sortFields, filter, isExternalOrganization, 0);
    }

    private PipelineInfo[] GetPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization,
      int sqlRead)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetPipeline), new object[6]
      {
        (object) loanFolder,
        (object) rights,
        (object) fields,
        (object) dataToInclude,
        (object) sortFields,
        (object) filter
      });
      try
      {
        return Pipeline.Generate(this.Session.GetUserInfo(), loanFolder, rights, fields, dataToInclude, filter, sortFields, isExternalOrganization, sqlRead);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (PipelineInfo[]) null;
      }
    }

    public virtual PipelineInfo[] GetPipeline(string[] guids, bool isExternalOrganization)
    {
      return this.GetPipeline(guids, (string[]) null, PipelineData.All, isExternalOrganization, 0);
    }

    public virtual PipelineInfo[] GetPipeline(
      string[] guids,
      bool isExternalOrganization,
      int sqlRead)
    {
      return this.GetPipeline(guids, (string[]) null, PipelineData.All, isExternalOrganization, sqlRead);
    }

    public virtual PipelineInfo[] GetPipeline(
      string[] guids,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      TradeType tradeType = TradeType.None)
    {
      return this.GetPipeline(guids, fields, dataToInclude, isExternalOrganization, 0, tradeType);
    }

    private PipelineInfo[] GetPipeline(
      string[] guids,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      int sqlRead,
      TradeType tradeType = TradeType.None)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetPipeline), new object[3]
      {
        (object) guids,
        (object) fields,
        (object) dataToInclude
      });
      if (guids == null || guids.Length == 0)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid list cannot be empty or null", nameof (guids)));
      try
      {
        TraceLog.WriteVerbose(nameof (LoanManager), "Pipeline requested for " + (object) guids.Length + " loans by user \"" + this.Session.UserID + "\"");
        return Pipeline.Generate(this.Session.GetUserInfo(), guids, fields, dataToInclude, isExternalOrganization, sqlRead, tradeType: tradeType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (PipelineInfo[]) null;
      }
    }

    private ICursor OpenPipelineForReassignment(
      string roleID,
      string userID,
      string folderName,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      QueryCriterion queryCriterion = null,
      SortField[] sortFields = null,
      string externalOrgID = null,
      bool filter = false,
      int sqlRead = 0)
    {
      this.onApiCalled(nameof (LoanManager), nameof (OpenPipelineForReassignment), new object[5]
      {
        (object) roleID,
        (object) userID,
        (object) folderName,
        (object) fields,
        (object) dataToInclude
      });
      try
      {
        TraceLog.WriteVerbose(nameof (LoanManager), "Pipeline requested for reassignment for Organization (" + (isExternalOrganization ? "TPO: " + externalOrgID : "Internal") + "), Role ID (" + roleID + "), User ID (" + userID + ", and folder name (" + folderName + ") by user \"" + this.Session.UserID + "\"");
        PipelineData dataToInclude1 = PipelineData.Fields;
        if (filter)
          dataToInclude1 = PipelineData.CurrentUserAccessRightsOnly;
        PipelineInfo[] pipelineInfoArray1 = Pipeline.Generate(this.Session.GetUserInfo(), roleID, userID, folderName, new string[1]
        {
          "Loan.Guid"
        }, dataToInclude1, LoanInfo.Right.Access, (isExternalOrganization ? 1 : 0) != 0, externalOrgID, sqlRead, queryCriterion, sortFields);
        if (this.Security.IsSuperAdministrator())
          return (ICursor) InterceptionUtils.NewInstance<PipelineCursor>().Initialize(this.Session, pipelineInfoArray1, fields, dataToInclude, PipelineSortOrder.None);
        int count1 = 750;
        if (!filter)
          return (ICursor) InterceptionUtils.NewInstance<PipelineCursor>().Initialize(this.Session, pipelineInfoArray1, fields, dataToInclude, PipelineSortOrder.None);
        List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
        PipelineInfo[] pipelineInfoArray2 = new PipelineInfo[count1];
        for (int count2 = 0; count2 < pipelineInfoArray1.Length; count2 += count1)
        {
          PipelineInfo[] array = ((IEnumerable<PipelineInfo>) pipelineInfoArray1).Skip<PipelineInfo>(count2).Take<PipelineInfo>(count1).ToArray<PipelineInfo>();
          LoanInfo.Right[] rights = this.getEffectiveRightsForLoans(array);
          pipelineInfoList.AddRange(((IEnumerable<PipelineInfo>) array).Where<PipelineInfo>((Func<PipelineInfo, int, bool>) ((t, i) => (rights[i] & LoanInfo.Right.Access) != 0)));
        }
        PipelineInfo[] array1 = pipelineInfoList.ToArray();
        return (ICursor) InterceptionUtils.NewInstance<PipelineCursor>().Initialize(this.Session, array1, fields, dataToInclude, PipelineSortOrder.None);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenPipelineForReassignment(
      string roleID,
      string userID,
      string folderName,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      string externalOrgID = null,
      bool filter = false,
      int sqlRead = 0)
    {
      return this.OpenPipelineForReassignment(roleID, userID, folderName, fields, dataToInclude, isExternalOrganization, (QueryCriterion) null, (SortField[]) null, externalOrgID, filter, sqlRead);
    }

    public virtual ICursor OpenPipelineForReassignment(
      string roleID,
      string userID,
      string folderName,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion queryCriterion,
      SortField[] sortFields,
      bool isExternalOrganization,
      string externalOrgID = null,
      bool filter = false,
      int sqlRead = 0)
    {
      return this.OpenPipelineForReassignment(roleID, userID, folderName, fields, dataToInclude, isExternalOrganization, queryCriterion, sortFields, externalOrgID, filter, sqlRead);
    }

    public virtual ICursor UpdatePipelineForReassignment(
      PipelineInfo[] pinfos,
      string[] fields,
      PipelineData dataToInclude)
    {
      this.onApiCalled(nameof (LoanManager), nameof (UpdatePipelineForReassignment), new object[2]
      {
        (object) fields,
        (object) dataToInclude
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<PipelineCursor>().Initialize(this.Session, pinfos, fields, dataToInclude, PipelineSortOrder.None);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    private LoanInfo.Right[] getEffectiveRightsForLoans(PipelineInfo[] pinfos)
    {
      LoanInfo.Right[] effectiveRightsForLoans = new LoanInfo.Right[pinfos.Length];
      ArrayList arrayList1 = new ArrayList(pinfos.Length);
      ArrayList arrayList2 = new ArrayList(pinfos.Length);
      UserInfo userInfo = this.Session.GetUserInfo();
      for (int index = 0; index < pinfos.Length; ++index)
      {
        if (pinfos[index] == null)
          effectiveRightsForLoans[index] = LoanInfo.Right.NoRight;
        else if (pinfos[index].Rights.ContainsKey((object) this.Session.UserID))
          effectiveRightsForLoans[index] = (LoanInfo.Right) pinfos[index].Rights[(object) this.Session.UserID];
        else if (userInfo.IsSuperAdministrator())
        {
          effectiveRightsForLoans[index] = LoanInfo.Right.FullRight;
        }
        else
        {
          arrayList1.Add((object) pinfos[index].GUID);
          arrayList2.Add((object) index);
        }
      }
      if (arrayList1.Count > 0)
      {
        LoanInfo.Right[] loanAccessRights = this.getEffectiveLoanAccessRights((string[]) arrayList1.ToArray(typeof (string)));
        for (int index = 0; index < loanAccessRights.Length; ++index)
          effectiveRightsForLoans[(int) arrayList2[index]] = loanAccessRights[index];
      }
      return effectiveRightsForLoans;
    }

    public virtual void ClearReportingDatabase(IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (LoanManager), nameof (ClearReportingDatabase), new object[1]
      {
        (object) feedback
      });
      this.Security.DemandAdministrator();
      try
      {
        Pipeline.ClearReportingDatabase(feedback);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RebuildPipeline(
      IServerProgressFeedback feedback,
      DatabaseToRebuild dbToRebuild)
    {
      this.RebuildPipeline(feedback, false, dbToRebuild);
    }

    public virtual void RebuildPipeline(
      IServerProgressFeedback feedback,
      bool fastRebuildOnly,
      DatabaseToRebuild dbToRebuild)
    {
      this.onApiCalled(nameof (LoanManager), nameof (RebuildPipeline), new object[1]
      {
        (object) fastRebuildOnly
      });
      this.Security.DemandFeatureAccess(AclFeature.GlobalTab_Pipeline);
      string sessionId1 = Guid.NewGuid().ToString();
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        UserInfo userInfo = fastRebuildOnly ? (UserInfo) null : this.Session.GetUserInfo();
        current.Sessions.AddServiceSession(sessionId1, "admin", "<encompassServer>");
        IServerProgressFeedback feedback1 = feedback;
        int dbToRebuild1 = (int) dbToRebuild;
        string sessionId2 = sessionId1;
        Pipeline.Rebuild(userInfo, feedback1, (DatabaseToRebuild) dbToRebuild1, sessionId2);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
      finally
      {
        current.Sessions.RemoveServiceSession(sessionId1);
      }
    }

    public virtual void RebuildPipeline(
      string folderName,
      IServerProgressFeedback feedback,
      DatabaseToRebuild dbToRebuild)
    {
      this.RebuildPipeline(new string[1]{ folderName }, feedback, dbToRebuild);
    }

    public virtual void RebuildPipeline(
      string[] folderNames,
      IServerProgressFeedback feedback,
      DatabaseToRebuild dbToRebuild)
    {
      this.onApiCalled(nameof (LoanManager), nameof (RebuildPipeline), (object[]) folderNames);
      if (folderNames == null || folderNames.Length == 0)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name list cannot be null or empty", nameof (folderNames)));
      this.Security.DemandAdministrator();
      string sessionId = Guid.NewGuid().ToString();
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        current.Sessions.AddServiceSession(sessionId, "admin", "<encompassServer>");
        foreach (string folderName in folderNames)
          Pipeline.Rebuild(folderName, this.Session.GetUserInfo(), feedback, dbToRebuild, false, sessionId);
        feedback?.SetFeedback("Freeing unused space...", "", -1);
        Loan.RemoveOrphanedLoanData();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
      finally
      {
        current.Sessions.RemoveServiceSession(sessionId);
      }
    }

    public virtual void RebuildIndex(string[] fieldNames, IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (LoanManager), nameof (RebuildIndex), (object[]) fieldNames);
      this.Security.DemandAdministrator();
      try
      {
        Pipeline.RebuildIndex(fieldNames, feedback);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateExtendedLoanSummary(IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (LoanManager), nameof (UpdateExtendedLoanSummary), Array.Empty<object>());
      this.Security.DemandAdministrator();
      try
      {
        Pipeline.UpdateExtendedLoanSummary(feedback);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RebuildReportingDb(
      bool useERDB,
      bool pendingFieldsOnly,
      IServerProgressFeedback2 feedback)
    {
      this.RebuildReportingDb(useERDB, pendingFieldsOnly, feedback, true);
    }

    public virtual void RebuildReportingDb(
      bool useERDB,
      bool pendingFieldsOnly,
      IServerProgressFeedback2 feedback,
      bool updateAllLoans)
    {
      this.onApiCalled(nameof (LoanManager), nameof (RebuildReportingDb), new object[1]
      {
        (object) pendingFieldsOnly
      });
      this.Security.DemandAdministrator();
      try
      {
        Pipeline.RebuildReportingDb(useERDB, pendingFieldsOnly, this.Session.GetUserInfo(), feedback, updateAllLoans);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ICursor OpenPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      PipelineSortOrder sortOrder,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (OpenPipeline), new object[6]
      {
        (object) loanFolder,
        (object) rights,
        (object) fields,
        (object) dataToInclude,
        (object) sortOrder,
        (object) filter
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<PipelineCursor>().Initialize(this.Session, loanFolder, rights, fields, dataToInclude, sortOrder, filter, isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      PipelineSortOrder sortOrder,
      QueryCriterion filter,
      bool isExternalOrganization,
      bool excludeArchivedLoans)
    {
      this.onApiCalled(nameof (LoanManager), nameof (OpenPipeline), new object[8]
      {
        (object) loanFolder,
        (object) rights,
        (object) fields,
        (object) dataToInclude,
        (object) sortOrder,
        (object) filter,
        (object) isExternalOrganization,
        (object) excludeArchivedLoans
      });
      try
      {
        return (ICursor) InterceptionUtils.NewInstance<PipelineCursor>().Initialize(this.Session, loanFolder, rights, fields, dataToInclude, sortOrder, filter, isExternalOrganization, excludeArchivedLoans);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenPipeline(
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return this.OpenPipeline((string) null, rights, fields, dataToInclude, sortFields, filter, isExternalOrganization);
    }

    public virtual ICursor OpenPipeline(
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization,
      int sqlRead)
    {
      return this.OpenPipeline((string[]) null, rights, fields, dataToInclude, sortFields, filter, isExternalOrganization, sqlRead);
    }

    public virtual ICursor OpenPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return this.OpenPipeline(loanFolder, rights, fields, dataToInclude, sortFields, filter, isExternalOrganization, 0);
    }

    public virtual ICursor OpenPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization,
      int sqlRead)
    {
      string[] loanFolders;
      if (!string.IsNullOrWhiteSpace(loanFolder))
        loanFolders = new string[1]{ loanFolder };
      else
        loanFolders = (string[]) null;
      return this.OpenPipeline(loanFolders, rights, fields, dataToInclude, sortFields, filter, isExternalOrganization, sqlRead);
    }

    public virtual ICursor OpenPipeline(
      string[] loanFolders,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization,
      int sqlRead)
    {
      this.onApiCalled(nameof (LoanManager), nameof (OpenPipeline), new object[6]
      {
        loanFolders == null ? (object) "All Folders" : (object) string.Join(",", loanFolders),
        (object) rights,
        (object) fields,
        (object) dataToInclude,
        (object) sortFields,
        (object) filter
      });
      try
      {
        PipelineInfo[] pinfoArray = Pipeline.Generate(this.Session.GetUserInfo(), loanFolders, rights, new string[0], PipelineData.Fields, filter, sortFields, (ICriterionTranslator) null, isExternalOrganization, sqlRead, TradeType.None, new int?(), false);
        return (ICursor) InterceptionUtils.NewInstance<PipelineCursor>().Initialize(this.Session, pinfoArray, fields, dataToInclude, PipelineSortOrder.None);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenPipeline(
      string[] loanFolders,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization,
      int sqlRead,
      bool isGlobalSearch,
      bool excludeArchivedLoans)
    {
      this.onApiCalled(nameof (LoanManager), nameof (OpenPipeline), new object[6]
      {
        loanFolders == null ? (object) "All Folders" : (object) string.Join(",", loanFolders),
        (object) rights,
        (object) fields,
        (object) dataToInclude,
        (object) sortFields,
        (object) filter
      });
      try
      {
        PipelineInfo[] pinfoArray = Pipeline.Generate(this.Session.GetUserInfo(), loanFolders, rights, new string[0], PipelineData.Fields, filter, sortFields, (ICriterionTranslator) null, isExternalOrganization, sqlRead, isGlobalSearch, excludeArchivedLoans);
        return (ICursor) InterceptionUtils.NewInstance<PipelineCursor>().Initialize(this.Session, pinfoArray, fields, dataToInclude, PipelineSortOrder.None);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual ICursor OpenContactPipeline(
      string[] fields,
      SortField[] sortFields,
      int contactID,
      CRMLogType logType,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (OpenContactPipeline), new object[4]
      {
        (object) fields,
        (object) sortFields,
        (object) contactID,
        (object) logType
      });
      try
      {
        string[] strArray = (string[]) null;
        switch (logType)
        {
          case CRMLogType.BorrowerContact:
            strArray = BorrowerContact.GetCRMLoanGuids(contactID);
            break;
          case CRMLogType.BusinessContact:
            strArray = BizPartnerContact.GetCRMLoanGuids(contactID);
            break;
        }
        if (strArray == null || strArray.Length == 0)
          return (ICursor) null;
        QueryCriterion filter = (QueryCriterion) new ListValueCriterion("Loan.XrefId", (Array) strArray);
        List<string> stringList1 = new List<string>((IEnumerable<string>) fields);
        if (!stringList1.Contains("Loan.XrefId"))
          stringList1.Add("Loan.XrefId");
        PipelineInfo[] collection = Pipeline.Generate(this.Session.GetUserInfo(), (string) null, LoanInfo.Right.NoRight, stringList1.ToArray(), PipelineData.All, filter, sortFields, isExternalOrganization);
        List<string> stringList2 = new List<string>((IEnumerable<string>) strArray);
        foreach (PipelineInfo pipelineInfo in collection)
        {
          if (stringList2.Contains(string.Concat(pipelineInfo.GetField("Loan.XrefId"))))
            stringList2.Remove(string.Concat(pipelineInfo.GetField("Loan.XrefId")));
        }
        PipelineInfo[] invisibleContactLoans = Pipeline.GenerateInvisibleContactLoans(stringList2.ToArray());
        List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>((IEnumerable<PipelineInfo>) collection);
        pipelineInfoList.AddRange((IEnumerable<PipelineInfo>) invisibleContactLoans);
        return (ICursor) InterceptionUtils.NewInstance<PipelineCursor>().Initialize(this.Session, pipelineInfoList.ToArray(), fields, PipelineData.All, PipelineSortOrder.None);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ICursor) null;
      }
    }

    public virtual PipelineScreenData GetUserCustomPipelineScreenData()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetUserCustomPipelineScreenData), Array.Empty<object>());
      try
      {
        return new PipelineScreenData()
        {
          UserCustomViews = this.GetUserCustomPipelineViews()
        };
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (PipelineScreenData) null;
      }
    }

    public virtual UserPipelineView[] GetUserCustomPipelineViews()
    {
      this.onApiCalled(nameof (LoanManager), "GetUserCustomPipelineScreenData", Array.Empty<object>());
      try
      {
        return PipelineViewAclDbAccessor.GetUserCustomPipelineViews(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (UserPipelineView[]) null;
      }
    }

    public virtual List<PersonaPipelineView> GetPersonaPipelineData()
    {
      PersonaPipelineView[] userPipelineViews = PipelineViewAclDbAccessor.GetUserPipelineViews(this.Session.GetUserInfo()?.Userid);
      return userPipelineViews == null ? (List<PersonaPipelineView>) null : ((IEnumerable<PersonaPipelineView>) userPipelineViews).ToList<PersonaPipelineView>();
    }

    public virtual List<UserPipelineView> GetPipelineData()
    {
      this.onApiCalled(nameof (LoanManager), "GetPipelineScreenData", Array.Empty<object>());
      try
      {
        UserPipelineView[] customPipelineViews = this.GetUserCustomPipelineViews();
        return customPipelineViews != null ? ((IEnumerable<UserPipelineView>) customPipelineViews).ToList<UserPipelineView>() : (List<UserPipelineView>) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (List<UserPipelineView>) null;
      }
    }

    public virtual PipelineScreenData GetPipelineScreenData()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetPipelineScreenData), Array.Empty<object>());
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        return new PipelineScreenData()
        {
          PipelineSettings = this.Session.Context.Settings.GetServerSettings("Pipeline"),
          CustomViews = TemplateSettingsStore.GetDirectoryEntries(TemplateSettingsType.PipelineView, FileSystemEntry.PrivateRoot(this.Session.UserID)),
          PersonaViews = PipelineViewAclDbAccessor.GetUserPipelineViews(userInfo.Userid),
          LoanFolders = LoanFolder.GetAllLoanFolderInfos(true, userInfo)
        };
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (PipelineScreenData) null;
      }
    }

    public virtual AlertSetupData GetAlertSetupData()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetAlertSetupData), Array.Empty<object>());
      try
      {
        return LoanConfiguration.GetAlertSetupData();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (AlertSetupData) null;
      }
    }

    public virtual LoanInfo.Right GetEffectiveLoanAccessRights(string guid)
    {
      return this.GetEffectiveLoanAccessRights(new string[1]
      {
        guid
      })[0];
    }

    public virtual LoanInfo.Right[] GetEffectiveLoanAccessRights(string[] guids)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetEffectiveLoanAccessRights), (object[]) guids);
      return this.getEffectiveLoanAccessRights(guids);
    }

    private LoanInfo.Right[] getEffectiveLoanAccessRights(string[] guids)
    {
      if (guids == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid list cannot be null", nameof (guids)));
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        return Pipeline.GetEffectiveLoanAccessRights(guids, userInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanInfo.Right[]) null;
      }
    }

    public virtual LoanInfo.Right GetEffectiveLoanAccessRights(string userId, string guid)
    {
      return this.GetEffectiveLoanAccessRights(userId, new string[1]
      {
        guid
      })[0];
    }

    public virtual LoanInfo.Right[] GetEffectiveLoanAccessRights(string userId, string[] guids)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetEffectiveLoanAccessRights), new object[3]
      {
        (object) userId,
        (object) guids,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      if (guids == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid list cannot be null", nameof (guids)));
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (!latestVersion.Exists)
            throw new ObjectNotFoundException("Invalid user ID '" + userId + "'", ObjectType.User, (object) userId);
          return Pipeline.GetEffectiveLoanAccessRights(guids, latestVersion.UserInfo);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanInfo.Right[]) null;
      }
    }

    public virtual void AddLoanAccessRights(
      string[] guids,
      string[] userIds,
      LoanInfo.Right rights)
    {
      this.onApiCalled(nameof (LoanManager), nameof (AddLoanAccessRights), new object[3]
      {
        (object) guids,
        (object) userIds,
        (object) rights
      });
      if (guids == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid list cannot be null", nameof (guids)));
      if (userIds == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("User ID list cannot be null", nameof (userIds)));
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        for (int index1 = 0; index1 < guids.Length; ++index1)
        {
          using (Loan loan = LoanStore.CheckOut(guids[index1]))
          {
            if (loan.Exists)
            {
              if (!((SecurityManager) this.Security).HasLoanRights(loan, LoanInfo.Right.Assignment))
                Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new SecurityException("User does not have assignment rights to loan " + (object) loan.Identity));
              for (int index2 = 0; index2 < userIds.Length; ++index2)
                loan.AddRightsForUser(userIds[index2], rights);
              loan.CheckIn(userInfo, false, this.Session.SessionID);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RemoveLoanAccessRights(
      string[] guids,
      string[] userIds,
      LoanInfo.Right rights)
    {
      this.onApiCalled(nameof (LoanManager), nameof (RemoveLoanAccessRights), new object[3]
      {
        (object) guids,
        (object) userIds,
        (object) rights
      });
      if (guids == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid list cannot be null", nameof (guids)));
      if (userIds == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("User ID list cannot be null", nameof (userIds)));
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        for (int index1 = 0; index1 < guids.Length; ++index1)
        {
          using (Loan loan = LoanStore.CheckOut(guids[index1]))
          {
            if (loan.Exists)
            {
              if (!((SecurityManager) this.Security).HasLoanRights(loan, LoanInfo.Right.Assignment))
                Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new SecurityException("User does not have assignment rights to loan " + (object) loan.Identity));
              for (int index2 = 0; index2 < userIds.Length; ++index2)
                loan.RemoveRightsForUser(userIds[index2], rights);
              loan.CheckIn(userInfo, false, this.Session.SessionID);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RebuildLoan(
      string folderName,
      string loanName,
      DatabaseToRebuild dbToRebuild)
    {
      this.onApiCalled(nameof (LoanManager), nameof (RebuildLoan), new object[2]
      {
        (object) folderName,
        (object) loanName
      });
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (folderName)));
      this.Security.DemandAdministrator();
      try
      {
        Pipeline.RebuildLoan(folderName, loanName, this.Session.GetUserInfo(), dbToRebuild, this.Session.SessionID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string[] GetRecentLoanGuids(int maxCount, bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetRecentLoanGuids), new object[1]
      {
        (object) maxCount
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(this.Session.UserID))
          return latestVersion.GetRecentLoanGuids(maxCount, isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual PipelineInfo[] GetRecentLoanInfo(int maxCount, bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetRecentLoanInfo), new object[1]
      {
        (object) maxCount
      });
      try
      {
        return Pipeline.Generate(this.Session.GetUserInfo(), this.GetRecentLoanGuids(maxCount, isExternalOrganization), (string[]) null, PipelineData.All, isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (PipelineInfo[]) null;
      }
    }

    public virtual LockInfo[] GetCurrentLocks(
      bool includeCrashedSessionLocks,
      bool isExternalOrganization)
    {
      return this.GetCurrentLocks(includeCrashedSessionLocks, false, isExternalOrganization);
    }

    public virtual LockInfo[] GetCurrentLocks(
      bool includeCrashedSessionLocks,
      bool refreshCache,
      bool isExternalOrganization)
    {
      return this.GetCurrentLocks(includeCrashedSessionLocks, (string) null, refreshCache, isExternalOrganization);
    }

    public virtual LockInfo[] GetCurrentLocks(
      bool includeCrashedSessionLocks,
      string loanFolder,
      bool isExternalOrganization)
    {
      return this.GetCurrentLocks(includeCrashedSessionLocks, loanFolder, false, isExternalOrganization);
    }

    public virtual LockInfo[] GetCurrentLocks(
      bool includeCrashedSessionLocks,
      string loanFolder,
      bool refreshCache,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetCurrentLocks), new object[1]
      {
        (object) (loanFolder ?? "")
      });
      try
      {
        return Loan.GetCurrentLocks(includeCrashedSessionLocks, loanFolder, this.Session.GetUserInfo(), refreshCache, isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LockInfo[]) null;
      }
    }

    private void updateTargetFolder(LoanData ld, string lf)
    {
      if (!(ld.GetField("5016") != "Y"))
        return;
      ld.SetField("5016", this.GetLoanFolderType(lf) == LoanFolderInfo.LoanFolderType.Archive ? "Y" : "N");
    }

    public virtual ILoan Import(
      string folderName,
      string loanName,
      BinaryObject encryptedData,
      DuplicateLoanAction dupAction)
    {
      this.onApiCalled(nameof (LoanManager), nameof (Import), new object[4]
      {
        (object) folderName,
        (object) loanName,
        (object) encryptedData,
        (object) dupAction
      });
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (folderName)));
      if ((loanName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan name cannot be blank or null", nameof (loanName)));
      if (encryptedData == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan data cannot be null", "data"));
      try
      {
        LoanData data = new LoanDataFormatter().Deserialize(encryptedData, folderName);
        encryptedData.DisposeDeserialized();
        return this.Import(folderName, loanName, data, dupAction);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return (ILoan) null;
      }
    }

    public virtual ILoan Import(
      string folderName,
      string loanName,
      LoanData data,
      DuplicateLoanAction dupAction)
    {
      this.onApiCalled(nameof (LoanManager), nameof (Import), new object[4]
      {
        (object) folderName,
        (object) loanName,
        (object) data,
        (object) dupAction
      });
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (folderName)));
      if ((loanName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan name cannot be blank or null", nameof (loanName)));
      if (data == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan data cannot be null", nameof (data)));
      try
      {
        ILoan loan = this.OpenLoan(data.GUID);
        if (loan != null)
        {
          if (dupAction != DuplicateLoanAction.Overwrite)
            dupAction = DuplicateLoanAction.Overwrite;
          loan.Lock(LoanInfo.LockReason.Downloaded, LockInfo.ExclusiveLock.Exclusive);
          loan.Import(data);
          return loan;
        }
        data.LoanNumber = "";
        data.SetField("3051", "Y");
        return this.CreateLoan(folderName, loanName, data, dupAction);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return (ILoan) null;
      }
    }

    public virtual ILoan ImportNew(
      string folderName,
      string loanName,
      BinaryObject encryptedData,
      DuplicateLoanAction dupAction)
    {
      this.onApiCalled(nameof (LoanManager), nameof (ImportNew), new object[4]
      {
        (object) folderName,
        (object) loanName,
        (object) encryptedData,
        (object) dupAction
      });
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (folderName)));
      if ((loanName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan name cannot be blank or null", nameof (loanName)));
      if (encryptedData == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan data cannot be null", "data"));
      try
      {
        LoanData data = new LoanDataFormatter().Deserialize(encryptedData, folderName);
        encryptedData.DisposeDeserialized();
        data.GUID = LoanData.NewGuid();
        data.LoanNumber = "";
        data.MersNumber = "";
        return this.CreateLoan(folderName, loanName, data, dupAction);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return (ILoan) null;
      }
    }

    public virtual ILoan ImportNew(
      string folderName,
      string loanName,
      BinaryObject encryptedData,
      DuplicateLoanAction dupAction,
      bool transfer)
    {
      this.onApiCalled(nameof (LoanManager), nameof (ImportNew), new object[4]
      {
        (object) folderName,
        (object) loanName,
        (object) encryptedData,
        (object) dupAction
      });
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Folder name cannot be blank or null", nameof (folderName)));
      if ((loanName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan name cannot be blank or null", nameof (loanName)));
      if (encryptedData == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Loan data cannot be null", "data"));
      try
      {
        LoanData data = new LoanDataFormatter().Deserialize(encryptedData, folderName);
        encryptedData.DisposeDeserialized();
        data.GUID = LoanData.NewGuid();
        data.LoanNumber = "";
        data.MersNumber = "";
        return this.CreateLoan(folderName, loanName, data, dupAction);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return (ILoan) null;
      }
    }

    public virtual void LoanReassign(
      int index,
      PipelineInfo pipeLine,
      string userID,
      int roleID,
      IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (LoanManager), nameof (LoanReassign), new object[1]
      {
        (object) pipeLine.LoanName
      });
      try
      {
        Pipeline.LoanReassign(index, pipeLine, userID, roleID, this.Session, feedback);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string GetFieldDescriptionFromReportFieldCache(string fieldId)
    {
      return ReportFieldCache.getFieldDescription(fieldId);
    }

    public virtual LoanXDBTableList GetLoanXDBTableList(bool useERDB)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanXDBTableList), Array.Empty<object>());
      try
      {
        return LoanXDBStore.GetLoanXDBTableList(useERDB);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanXDBTableList) null;
      }
    }

    public virtual string CalculateDescription(string name)
    {
      string description = !name.StartsWith("Fields.") ? this.GetFieldDescriptionFromReportFieldCache(name) : this.GetFieldDescriptionFromReportFieldCache(name.Substring(7));
      if (string.IsNullOrWhiteSpace(description) && name.StartsWith("Fields."))
      {
        LoanXDBField field = this.GetLoanXDBTableList(false).GetField(name.Substring(7));
        if (field != null)
          description = field.Description;
      }
      return description;
    }

    public virtual LoanXDBField[] GetAuditTrailLoanXDBField()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetAuditTrailLoanXDBField), Array.Empty<object>());
      try
      {
        return LoanXDBStore.GetAuditTrailLoanXDBField();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanXDBField[]) null;
      }
    }

    public virtual LoanXDBAuditField[] GetAuditTrailReportingLoanXDBField()
    {
      this.onApiCalled(nameof (LoanManager), "GetAuditTrailLoanXDBField", Array.Empty<object>());
      try
      {
        return LoanXDBStore.GetAuditTrailReportingLoanXDBField();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanXDBAuditField[]) null;
      }
    }

    public virtual Dictionary<string, AuditRecord> GetLastAuditRecordsForLoan(string guid)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLastAuditRecordsForLoan), new object[1]
      {
        (object) guid
      });
      try
      {
        return AuditTrailAccessor.GetLastAuditRecords(guid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, AuditRecord>) null;
      }
    }

    public virtual Dictionary<string, AuditRecord> GetLastAuditRecordsForLoan(
      string guid,
      string[] fieldIds)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLastAuditRecordsForLoan), new object[2]
      {
        (object) guid,
        (object) fieldIds
      });
      try
      {
        return AuditTrailAccessor.GetLastAuditRecords(guid, fieldIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, AuditRecord>) null;
      }
    }

    public virtual void SetLoanXDBTableList(bool useERDB, LoanXDBTableList tableList)
    {
      this.onApiCalled(nameof (LoanManager), nameof (SetLoanXDBTableList), new object[1]
      {
        (object) tableList
      });
      if (tableList == null)
        Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Report database table cannot be blank or null", "source", this.Session.SessionInfo));
      try
      {
        LoanXDBStore.SetLoanXDBTableList(useERDB, tableList, this.Session.SessionInfo.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanXDBStatusInfo ApplyReportingDatabaseChanges(
      bool useERDB,
      LoanXDBTableList tableList,
      string validationKey)
    {
      this.onApiCalled(nameof (LoanManager), nameof (ApplyReportingDatabaseChanges), new object[2]
      {
        (object) tableList,
        (object) validationKey
      });
      try
      {
        this.Session.SecurityManager.DemandSuperAdministrator();
        return LoanXDBStore.ApplyChanges(useERDB, tableList, validationKey, this.Session.SessionInfo.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), new Exception("Error Applying Reporting Database Changes.", ex), this.Session.SessionInfo);
        return (LoanXDBStatusInfo) null;
      }
    }

    public virtual bool ResetReportingDatabase(
      bool useERDB,
      LoanXDBTableList tableList,
      string validationKey,
      bool keepTables,
      IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (LoanManager), nameof (ResetReportingDatabase), new object[4]
      {
        (object) tableList,
        (object) validationKey,
        (object) keepTables,
        (object) feedback
      });
      try
      {
        this.Session.SecurityManager.DemandSuperAdministrator();
        return LoanXDBStore.ResetReportingDatabase(useERDB, tableList, validationKey, keepTables, feedback, this.Session.SessionInfo.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
      return false;
    }

    public virtual LoanXDBStatusInfo GetLoanXDBStatus(bool useERDB)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanXDBStatus), Array.Empty<object>());
      try
      {
        return LoanXDBStore.GetLoanXDBStatus(useERDB);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanXDBStatusInfo) null;
      }
    }

    public virtual void SubmitBatch(LoanBatch loanBatch, bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanManager), nameof (SubmitBatch), new object[1]
      {
        (object) loanBatch
      });
      try
      {
        this.Security.DemandAdministrator();
        LoanBatchUpdateAccessor.SubmitBatch(loanBatch, this.Session.GetUserInfo(), isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool IsTimeToSetLoanNumber(LoanData data)
    {
      this.onApiCalled(nameof (LoanManager), nameof (IsTimeToSetLoanNumber), new object[1]
      {
        (object) data
      });
      try
      {
        MilestoneLog milestoneLog = (MilestoneLog) null;
        LogList logList = data.GetLogList();
        IBpmManager bpmManager = (IBpmManager) this.Session.GetObject("BpmManager");
        MilestoneTemplate milestoneTemplate = bpmManager.GetMilestoneTemplate(logList.MilestoneTemplate.TemplateID);
        if (milestoneTemplate != null && milestoneTemplate.Active && milestoneTemplate.AutoLoanNumberingMilestoneID != string.Empty)
          milestoneLog = logList.GetMilestoneByID(milestoneTemplate.AutoLoanNumberingMilestoneID);
        if (milestoneLog == null)
        {
          string companySetting = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanySetting("POLICIES", "LOANNUMBER");
          milestoneLog = logList.GetMilestoneByID(companySetting);
        }
        if (milestoneLog == null)
        {
          Hashtable templateDefaultSettings = bpmManager.GetMilestoneTemplateDefaultSettings();
          milestoneLog = logList.GetMilestoneByID(string.Concat(templateDefaultSettings[(object) "POLICIES.LOANNUMBER"]));
        }
        return milestoneLog != null ? milestoneLog.Done : throw new Exception("Can't find milestone to set loan number.");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual int GetLoanCount()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanCount), Array.Empty<object>());
      try
      {
        return LoanFolder.GetLoanCount();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual int GetLoanCount(string loanFolder)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanCount), new object[1]
      {
        (object) loanFolder
      });
      try
      {
        return LoanFolder.GetLoanCount(loanFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual ILoanConfigurationInfo GetLoanConfigurationInfo()
    {
      return this.GetLoanConfigurationInfo((LoanConfigurationParameters) null);
    }

    public virtual ILoanConfigurationInfo GetLoanConfigurationInfo(
      LoanConfigurationParameters configParams)
    {
      return this.GetLoanConfigurationInfo(configParams, (string) null, (string) null, (string) null);
    }

    public virtual ILoanConfigurationInfo GetLoanConfigurationInfo(
      LoanConfigurationParameters configParams,
      string loanFolder,
      string loanName,
      string hmdaProfileID = null)
    {
      using (PerformanceMeter.StartNew("LoanManager.GetLoanConfigurationInfo", 2356, nameof (GetLoanConfigurationInfo), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs"))
      {
        this.onApiCalled(nameof (LoanManager), nameof (GetLoanConfigurationInfo), new object[1]
        {
          (object) configParams
        });
        try
        {
          return LoanConfiguration.GetLoanConfigurationInfo(this.Session.GetUserInfo(), configParams, loanFolder, loanName, hmdaProfileID);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
          return (ILoanConfigurationInfo) null;
        }
      }
    }

    public virtual ILoanConfigurationInfo GetLoanConfigurationInfo(
      LoanConfigurationParameters configParams,
      ILoan loandata,
      string loanFolder,
      string loanName)
    {
      using (PerformanceMeter.StartNew("LoanManager.GetLoanConfigurationInfo", 2376, nameof (GetLoanConfigurationInfo), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs"))
      {
        this.onApiCalled(nameof (LoanManager), nameof (GetLoanConfigurationInfo), new object[1]
        {
          (object) configParams
        });
        try
        {
          return LoanConfiguration.GetLoanConfigurationInfo(this.Session.GetUserInfo(), configParams, loanFolder, loanName, loandata?.SelectField("HMDA.X100"));
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
          return (ILoanConfigurationInfo) null;
        }
      }
    }

    public virtual LoanDefaults GetLoanDefaultData()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanDefaultData), Array.Empty<object>());
      try
      {
        return LoanConfiguration.GetLoanDefaultData(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (LoanDefaults) null;
      }
    }

    public virtual ILoanSettings GetLoanSettings()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanSettings), Array.Empty<object>());
      try
      {
        return LoanConfiguration.GetLoanSettings(this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ILoanSettings) null;
      }
    }

    public virtual ILoanSettings GetLoanSettings(string hmdaProfileID)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanSettings), Array.Empty<object>());
      try
      {
        return LoanConfiguration.GetLoanSettings(this.Session.GetUserInfo(), hmdaProfileID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ILoanSettings) null;
      }
    }

    public virtual FieldSettings GetFieldSettings()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetFieldSettings), Array.Empty<object>());
      try
      {
        return LoanConfiguration.GetFieldSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (FieldSettings) null;
      }
    }

    private PiggybackFields getPiggybackSyncFields()
    {
      try
      {
        Type valueType = typeof (PiggybackFields);
        using (BinaryObject systemSettings = SystemConfiguration.GetSystemSettings(valueType.Name))
          return systemSettings == null ? (PiggybackFields) null : (PiggybackFields) new XmlSerializer().Deserialize(systemSettings.AsStream(), valueType);
      }
      catch (Exception ex)
      {
        return (PiggybackFields) null;
      }
    }

    public virtual DateTime GetServerCurrentTime()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetServerCurrentTime), Array.Empty<object>());
      try
      {
        return LoanFolder.GetServerCurrentTime();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return DateTime.MinValue;
      }
    }

    public virtual DateTime GetLoanCountLastUpdatedTime()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanCountLastUpdatedTime), Array.Empty<object>());
      try
      {
        return LoanFolder.GetLoanCountLastUpdatedTime();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return DateTime.MinValue;
      }
    }

    public virtual TimeSpan GetTimeSpanSinceLoanCountLastUpdated()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetTimeSpanSinceLoanCountLastUpdated), Array.Empty<object>());
      try
      {
        return LoanFolder.GetTimeSpanSinceLoanCountLastUpdated();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return TimeSpan.Zero;
      }
    }

    public virtual void SetLoanCountLastUpdatedTime()
    {
      this.onApiCalled(nameof (LoanManager), nameof (SetLoanCountLastUpdatedTime), Array.Empty<object>());
      try
      {
        LoanFolder.SetLoanCountLastUpdatedTime();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ArrayList GetAllLenders(string folderName)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetAllLenders), new object[1]
      {
        (object) folderName
      });
      try
      {
        return LoanFolder.GetAllLenders(folderName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ArrayList) null;
      }
    }

    public virtual ArrayList GetAllInvestors(string folderName)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetAllInvestors), new object[1]
      {
        (object) folderName
      });
      try
      {
        return LoanFolder.GetAllInvestors(folderName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ArrayList) null;
      }
    }

    public virtual ArrayList GetAllBrokers(string folderName)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetAllBrokers), new object[1]
      {
        (object) folderName
      });
      try
      {
        return LoanFolder.GetAllBrokers(folderName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ArrayList) null;
      }
    }

    private LoanProxy createLoanProxy(string guid)
    {
      return InterceptionUtils.NewInstance<LoanProxy>().Initialize(this.Session, guid, this);
    }

    private void createStatusOnlineLoanSetup(LoanIdentity loanId, LoanData loanData)
    {
      StatusOnlineLoanSetup loanSetup = new StatusOnlineLoanSetup();
      StatusOnlineSetup setup = StatusOnlineStore.GetSetup((string) null);
      foreach (StatusOnlineTrigger trigger in (CollectionBase) setup.Triggers)
        loanSetup.Triggers.Add(trigger);
      string str = (string) null;
      switch (setup.PersonalTriggersType)
      {
        case ApplyPersonalTriggersType.None:
          loanSetup.AppliedPersonalTriggers = true;
          break;
        case ApplyPersonalTriggersType.FileStarter:
          str = this.Session.UserID;
          break;
        case ApplyPersonalTriggersType.LoanOfficer:
          str = loanData.GetSimpleField("LOID");
          break;
      }
      if (!string.IsNullOrEmpty(str))
      {
        UserInfo userInfo = (UserInfo) null;
        using (User latestVersion = UserStore.GetLatestVersion(str))
        {
          if (latestVersion.Exists)
            userInfo = latestVersion.UserInfo;
        }
        if (userInfo != (UserInfo) null && userInfo.PersonalStatusOnline)
        {
          foreach (StatusOnlineTrigger trigger in (CollectionBase) StatusOnlineStore.GetSetup(str).Triggers)
            loanSetup.Triggers.Add(trigger);
        }
        loanSetup.AppliedPersonalTriggers = true;
      }
      StatusOnlineLoanStore.SaveSetup(loanId, loanSetup);
    }

    private ILoan createLoanInternal(
      string folderName,
      string loanName,
      LoanData data,
      DuplicateLoanAction dupAction)
    {
      try
      {
        PerformanceMeter current = PerformanceMeter.Current;
        UserInfo userInfo = this.Session.GetUserInfo();
        current.AddCheckpoint("Retrieved current user info...", 2661, nameof (createLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
        loanName = data.GUID;
        LoanIdentity objectId = Loan.LookupIdentity(folderName, loanName);
        if (objectId != (LoanIdentity) null)
        {
          switch (dupAction)
          {
            case DuplicateLoanAction.Overwrite:
              this.DeleteLoan(objectId.Guid);
              break;
            case DuplicateLoanAction.Fail:
              Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new DuplicateObjectException("Cannot replace existing loan", ObjectType.Loan, (object) objectId));
              break;
          }
        }
        current.AddCheckpoint("Generated new loan name...", 2682, nameof (createLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
        using (Loan loan = LoanStore.CheckOut(data.GUID))
        {
          if (loan.Exists)
          {
            if (dupAction == DuplicateLoanAction.Overwrite)
              this.DeleteLoan(data.GUID);
            else
              Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new DuplicateObjectException("A loan with the specified GUID already exists", ObjectType.Loan, (object) loan.Identity));
          }
        }
        current.AddCheckpoint("Deleted existing loan, if any...", 2696, nameof (createLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
        using (Loan loan = LoanStore.CheckOut(data.GUID))
        {
          bool allowDeferrable = this.AllowDeferrable();
          current.AddCheckpoint("Obtained lock on new loan GUID...", 2702, nameof (createLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
          loan.CreateNew(folderName, loanName, this.initializeNewLoanInfo(data.GUID), data, userInfo, this.Session.SessionID, allowDeferrable);
          current.AddCheckpoint("Created new Loan object...", 2704, nameof (createLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
          loan.AddRightsForUser(this.Session.UserID, LoanInfo.Right.FullRight);
          current.AddCheckpoint("Added access rights for new loan...", 2706, nameof (createLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
          loan.CheckIn(userInfo, false, this.Session.SessionID);
          current.AddCheckpoint("Checked in new loan to LoanStore...", 2708, nameof (createLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
        }
        LoanIdentity loanIdentity = new LoanIdentity(folderName, loanName, data.GUID);
        this.createStatusOnlineLoanSetup(loanIdentity, data);
        current.AddCheckpoint("Created status online setup data...", 2714, nameof (createLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
        EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType actionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanCreated;
        if (data.GetField("3051") == "Y")
          actionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanImported;
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new LoanFileAuditRecord(this.Session.UserID, userInfo.FullName, actionType, DateTime.Now, data.GUID, folderName, data.LoanNumber, data.GetField("37"), data.GetField("36"), data.GetField("11") + " " + data.GetField("12") + ", " + data.GetField("14") + " " + data.GetField("15"), data.GetField("2024"), this.Session.LoginParams.AppName));
        current.AddCheckpoint("Added system audit trail record...", 2724, nameof (createLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
        this.raiseEvent(LoanEventType.Created, loanIdentity);
        current.AddCheckpoint("Raised LoanCreated notification event...", 2728, nameof (createLoanInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs");
        return (ILoan) this.createLoanProxy(data.GUID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (ILoan) null;
      }
    }

    private LoanServerInfo initializeNewLoanInfo(string guid)
    {
      LoanServerInfo loanServerInfo = new LoanServerInfo(guid);
      loanServerInfo.AddLockInfo(new LockInfo(guid, this.Session.UserID, this.Session.GetUserInfo().FirstName, this.Session.GetUserInfo().LastName, this.Session.SessionID, this.Session.Server.ToString(), LoanInfo.LockReason.OpenForWork, DateTime.Now, LockInfo.ExclusiveLock.Nonexclusive));
      return loanServerInfo;
    }

    private void autoAssignLoanNumbers(LoanData data)
    {
      if (data.IsCreatedWithoutLoanNumber())
        return;
      if ((data.LoanNumber ?? "") == "" && this.IsTimeToSetLoanNumber(data))
      {
        OrgInfo avaliableOrganization = OrganizationStore.GetFirstAvaliableOrganization(this.Session.GetUserInfo().OrgId);
        data.LoanNumber = LoanNumberGenerator.GetNextLoanNumber(avaliableOrganization);
      }
      if (!((data.MersNumber ?? "") == ""))
        return;
      OrgInfo organizationWithMersmin = OrganizationStore.GetFirstOrganizationWithMERSMIN(this.Session.GetUserInfo().OrgId);
      data.MersNumber = MersNumberGenerator.GetNextMersNumber(organizationWithMersmin);
    }

    private void raiseEvent(LoanEventType eventType, LoanIdentity id)
    {
      LoanEvent e = new LoanEvent(eventType, id, this.Session.SessionInfo);
      TraceLog.WriteInfo(nameof (LoanManager), e.ToString());
      EncompassServer.RaiseEvent(this.Session.Context, (ServerEvent) e);
    }

    public virtual PipelineInfo.Alert[] GetLoanAlerts(string guid)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoanAlerts), new object[1]
      {
        (object) guid
      });
      try
      {
        if ((guid ?? "") == "")
          Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid cannot be blank or null", nameof (guid)));
        return AlertConfigAccessor.GetLoanAlerts(guid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return (PipelineInfo.Alert[]) null;
      }
    }

    public virtual void UpdateLoanAlerts(string guid, PipelineInfo.Alert[] alertList)
    {
      this.onApiCalled(nameof (LoanManager), nameof (UpdateLoanAlerts), new object[2]
      {
        (object) guid,
        (object) alertList
      });
      try
      {
        if ((guid ?? "") == "")
          Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid cannot be blank or null", nameof (guid)));
        AlertConfigAccessor.UpdateLoanAlerts(guid, alertList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
      }
    }

    public virtual AuditRecord[] GetAuditRecords(string guid, string columnID)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetAuditRecords), new object[2]
      {
        (object) guid,
        (object) columnID
      });
      try
      {
        if ((guid ?? "") == "")
          Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid cannot be blank or null", nameof (guid)));
        else if ((columnID ?? "") == "")
          Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Field ID cannot be blank or null", "fieldID"));
        return AuditTrailAccessor.GetAuditRecords(guid, columnID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return (AuditRecord[]) null;
      }
    }

    public virtual AuditRecord[] GetAuditRecords(string guid, string[] columnIDs)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetAuditRecords), new object[2]
      {
        (object) guid,
        (object) columnIDs
      });
      try
      {
        if ((guid ?? "") == "")
          Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Guid cannot be blank or null", nameof (guid)));
        else if (columnIDs == null || columnIDs.Length == 0)
          Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Field IDs cannot be blank or null", "fieldID"));
        foreach (string columnId in columnIDs)
        {
          if ((columnId ?? "") == "")
            Err.Raise(TraceLevel.Warning, nameof (LoanManager), (ServerException) new ServerArgumentException("Field ID cannot be blank or null", "fieldID"));
        }
        return AuditTrailAccessor.GetAuditRecords(guid, columnIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return (AuditRecord[]) null;
      }
    }

    public virtual StandardFields GetStandardFields()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetStandardFields), Array.Empty<object>());
      try
      {
        return StandardFields.Instance;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return (StandardFields) null;
      }
    }

    public virtual bool AddWebCenterImportID(
      string importID,
      string emSiteID,
      string loanGUID,
      DateTime importDateTime,
      string whoImports)
    {
      this.onApiCalled(nameof (LoanManager), nameof (AddWebCenterImportID), new object[5]
      {
        (object) importID,
        (object) emSiteID,
        (object) loanGUID,
        (object) importDateTime,
        (object) whoImports
      });
      try
      {
        return Loan.AddWebCenterImportID(importID, emSiteID, loanGUID, importDateTime, whoImports);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return false;
      }
    }

    public virtual WebCenterImpotStatus GetWebCenterImportStatus(string loanGUID)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetWebCenterImportStatus), new object[1]
      {
        (object) loanGUID
      });
      try
      {
        return Loan.GetWebCenterImportStatus(loanGUID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
        return (WebCenterImpotStatus) null;
      }
    }

    public virtual void DeleteWebCenterImportID(string importID)
    {
      this.onApiCalled(nameof (LoanManager), nameof (DeleteWebCenterImportID), new object[1]
      {
        (object) importID
      });
      try
      {
        Loan.DeleteWebCenterImportID(importID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex);
      }
    }

    public virtual PipelineInfo[] GetLoansByLoanNumbers(List<string> loanNumbers)
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetLoansByLoanNumbers), new object[1]
      {
        (object) loanNumbers
      });
      try
      {
        return Loan.GetLoansByLoanNumbers(loanNumbers);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (PipelineInfo[]) null;
      }
    }

    public virtual FieldRuleInfo[] GetAdvancedConditionMilestoneTemplates(
      LoanData loan,
      UserInfo userInfo,
      EllieMae.EMLite.ClientServer.SessionObjects sessionObj)
    {
      throw new NotImplementedException();
    }

    public virtual bool AllowDeferrable()
    {
      bool flag = true;
      if (EncompassServer.ServerMode != EncompassServerMode.Service)
      {
        DeferrableLoanProcessMode serverSetting = (DeferrableLoanProcessMode) this.Session.Context.Settings.GetServerSetting("Policies.DeferrableLoanProcessMode");
        flag = serverSetting == DeferrableLoanProcessMode.Deferred || serverSetting == DeferrableLoanProcessMode.EncompassDeferred;
      }
      return flag;
    }

    public virtual string SyncLoanFolder(string guid, bool cleanup)
    {
      this.onApiCalled(nameof (LoanManager), nameof (SyncLoanFolder), new object[2]
      {
        (object) guid,
        (object) cleanup
      });
      try
      {
        return Loan.SyncLoanFolder(guid, cleanup);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual SellConditionTrackingSetup GetUpdatedSellConditionSetup()
    {
      this.onApiCalled(nameof (LoanManager), nameof (GetUpdatedSellConditionSetup), Array.Empty<object>());
      try
      {
        return LoanConfiguration.GetUpdatedSellConditionSetup();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
        return (SellConditionTrackingSetup) null;
      }
    }

    public virtual FastLoanAccessResponse GetFastLoanAccess(string guid)
    {
      FastLoanAccessResponse fastLoanAccess = new FastLoanAccessResponse();
      string[] guids = new string[1]{ guid };
      PipelineInfo pipelineInfo = this.GetPipeline(guids, false, 0)?[0];
      if (pipelineInfo == null)
        return fastLoanAccess;
      LoanInfo.Right[] loanAccessRights = this.GetEffectiveLoanAccessRights(guids);
      if (loanAccessRights == null || loanAccessRights[0] == LoanInfo.Right.NoRight)
      {
        fastLoanAccess.Right = LoanInfo.Right.NoRight;
        return fastLoanAccess;
      }
      fastLoanAccess.Right = loanAccessRights[0];
      fastLoanAccess.PipelineInfo = pipelineInfo;
      return fastLoanAccess;
    }

    public virtual ILoanSpecificConfigurationInfo GetLoanSpecificConfigurationInfo(
      string userid,
      string loanFolder,
      string loanName,
      HMDAInformation hmdaInfo)
    {
      using (PerformanceMeter.StartNew("LoanManager.GetLoanSpecificConfigurationInfo", 3017, nameof (GetLoanSpecificConfigurationInfo), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanManager.cs"))
      {
        try
        {
          return LoanConfiguration.GetLoanSpecificConfigurationInfo(userid, loanFolder, loanName, hmdaInfo);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanManager), ex, this.Session.SessionInfo);
          return (ILoanSpecificConfigurationInfo) null;
        }
      }
    }

    public virtual FastLoanLoadResponse FastLoanLoad(FastLoanLoadRequest request)
    {
      FastLoanLoadResponse loanLoadResponse = new FastLoanLoadResponse();
      ILoan loan = (ILoan) null;
      this.onApiCalled(nameof (LoanManager), nameof (FastLoanLoad), new object[2]
      {
        (object) request,
        (object) request.Guid
      });
      try
      {
        loan = this.OpenLoanInternal(new LoanIdentity(request.Guid), LoanInfo.LockReason.NotLocked);
        if (loan == null)
          throw new ObjectNotFoundException("Loan not found", ObjectType.Loan, (object) request.Guid);
      }
      catch
      {
        loan?.Close();
        throw;
      }
      if (request.ShouldLock)
      {
        try
        {
          loan.LockInternal(LoanInfo.LockReason.OpenForWork, request.ExclusiveLock, false);
        }
        catch (Exception ex)
        {
          throw;
        }
      }
      PipelineInfo pipelineInfoInternal = loan.GetPipelineInfoInternal(false, 0);
      LoanData loanDataInternal = loan.GetLoanDataInternal(false);
      loanLoadResponse.PipelineInfo = pipelineInfoInternal;
      loanLoadResponse.Proxy = loan;
      loanLoadResponse.LoanData = loanDataInternal;
      loanLoadResponse.LoanProperties = loan.GetLoanPropertySettingsInternal();
      loanLoadResponse.LoanSpecificConfigInfo = this.GetLoanSpecificConfigurationInfo(this.Session.UserID, pipelineInfoInternal.LoanFolder, pipelineInfoInternal.LoanName, loanDataInternal.Settings.HMDAInfo);
      loanLoadResponse.ConfigParameters = request.ConfigParams.Clone();
      DateTime date1 = Utils.ParseDate((object) Company.GetCompanySettingFromDB("DDM", "LastModifiedDateTime"));
      if (request.ConfigParams.DDMLastModifiedDateTime < date1)
      {
        loanLoadResponse.DDMFeeRules = DDMFeeRuleDbAccessor.GetAllDDMFeeRulesAndScenarios(true);
        loanLoadResponse.DDMFieldRules = DDMFieldRuleDbAccessor.GetAllDDMFieldRulesAndScenarios(true);
        loanLoadResponse.ConfigParameters.DDMLastModifiedDateTime = date1;
      }
      else
        loanLoadResponse.ConfigParameters.DDMLastModifiedDateTime = request.ConfigParams.DDMLastModifiedDateTime;
      DateTime date2 = Utils.ParseDate((object) Company.GetCompanySettingFromDB("ROLE", "LastModifiedDateTime"));
      if (request.ConfigParams.RolesModificationTime < date2)
      {
        loanLoadResponse.AllRoles = WorkflowBpmDbAccessor.GetAllRoleFunctions();
        loanLoadResponse.ConfigParameters.RolesModificationTime = date2;
      }
      else
        loanLoadResponse.ConfigParameters.RolesModificationTime = request.ConfigParams.RolesModificationTime;
      FieldRulesBpmDbAccessor accessor1 = (FieldRulesBpmDbAccessor) BpmDbAccessor.GetAccessor(BizRuleType.FieldRules);
      DateTime timeFromDatabase1 = accessor1.getRuleLastModificationTimeFromDatabase(-1);
      if (request.ConfigParams.FieldRulesModificationTime < timeFromDatabase1)
      {
        loanLoadResponse.ConfigParameters.FieldRulesModificationTime = timeFromDatabase1;
        loanLoadResponse.FieldRulesInfo = (FieldRuleInfo[]) accessor1.GetRules(true, true, (string) null);
      }
      else
        loanLoadResponse.ConfigParameters.FieldRulesModificationTime = request.ConfigParams.FieldRulesModificationTime;
      TriggersBpmDbAccessor accessor2 = (TriggersBpmDbAccessor) BpmDbAccessor.GetAccessor(BizRuleType.Triggers);
      DateTime timeFromDatabase2 = accessor2.getRuleLastModificationTimeFromDatabase(-1);
      if (request.ConfigParams.TriggersModificationTime < timeFromDatabase2)
      {
        loanLoadResponse.ConfigParameters.TriggersModificationTime = timeFromDatabase2;
        loanLoadResponse.TriggersInfo = (TriggerInfo[]) accessor2.GetRules(true, true, (string) null);
      }
      else
        loanLoadResponse.ConfigParameters.TriggersModificationTime = request.ConfigParams.TriggersModificationTime;
      PrintSelectionBpmDbAccessor accessor3 = (PrintSelectionBpmDbAccessor) BpmDbAccessor.GetAccessor(BizRuleType.PrintSelection);
      DateTime timeFromDatabase3 = accessor3.getRuleLastModificationTimeFromDatabase(-1);
      if (request.ConfigParams.PrintSelectionModificationTime < timeFromDatabase3)
      {
        loanLoadResponse.ConfigParameters.PrintSelectionModificationTime = timeFromDatabase3;
        loanLoadResponse.PrintSelectionRulesInfo = (PrintSelectionRuleInfo[]) accessor3.GetRules(true, true, (string) null);
      }
      else
        loanLoadResponse.ConfigParameters.PrintSelectionModificationTime = request.ConfigParams.PrintSelectionModificationTime;
      DateTime modificationDate = SystemConfiguration.GetLoanCustomFieldsModificationDate();
      if (request.ConfigParams.CustomFieldsModificationTime < modificationDate)
      {
        ClientContext.GetCurrent().Cache.Put("CachedCustomFields", (object) null);
        loanLoadResponse.CustomFields = SystemConfiguration.GetLoanCustomFields();
        loanLoadResponse.ConfigParameters.CustomFieldsModificationTime = modificationDate;
      }
      if (loanLoadResponse.LoanData != null)
        loanLoadResponse.LoanData.Settings = (ILoanSettings) null;
      return loanLoadResponse;
    }
  }
}

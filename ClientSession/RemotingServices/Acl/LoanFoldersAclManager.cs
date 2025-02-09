// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Acl.LoanFoldersAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Acl.Interfaces;
using EllieMae.EMLite.Common;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.Acl
{
  public class LoanFoldersAclManager : ManagerBase
  {
    private ILoanFoldersAclManager loanFoldersAclMgr;

    internal LoanFoldersAclManager(Sessions.Session session)
      : base(session)
    {
      this.loanFoldersAclMgr = (ILoanFoldersAclManager) this.session.GetAclManager(AclCategory.LoanFolderMove);
    }

    public LoanFolderAclInfo[] GetAccessibleLoanFolders(
      AclFeature feature,
      string userId,
      Persona[] personaList)
    {
      string key = feature.ToString() + "/" + userId;
      LoanFolderAclInfo[] subject = (LoanFolderAclInfo[]) null;
      if (this.session.UserID == userId)
        subject = (LoanFolderAclInfo[]) this.getSubjectsFromCache(key);
      if (subject == null)
      {
        subject = this.loanFoldersAclMgr.GetAccessibleLoanFolders(feature, userId, personaList);
        if (this.session.UserID == userId)
          this.setSubjectCache(key, (object) subject);
      }
      return subject;
    }

    public LoanFolderAclInfo GetAccessibleLoanFolder(
      AclFeature feature,
      string userId,
      Persona[] personaList,
      string folderName)
    {
      return this.loanFoldersAclMgr.GetAccessibleLoanFolder(feature, userId, personaList, folderName);
    }

    public LoanFolderAclInfo GetPermission(AclFeature feature, int personaID, string folderName)
    {
      return this.loanFoldersAclMgr.GetPermission(feature, personaID, folderName);
    }

    public LoanFolderAclInfo GetPermission(
      AclFeature feature,
      Persona[] personaList,
      string folderName)
    {
      return this.loanFoldersAclMgr.GetPermission(feature, personaList, folderName);
    }

    public LoanFolderAclInfo[] GetPermissions(AclFeature feature, int personaID)
    {
      return this.loanFoldersAclMgr.GetPermissions(feature, personaID);
    }

    public LoanFolderAclInfo GetPermission(AclFeature feature, string userID, string folderName)
    {
      return this.loanFoldersAclMgr.GetPermission(feature, userID, folderName);
    }

    public LoanFolderAclInfo[] GetPermissions(AclFeature feature, string userID)
    {
      return this.loanFoldersAclMgr.GetPermissions(feature, userID);
    }

    public void SetPermission(
      AclFeature feature,
      LoanFolderAclInfo loanFolderAclInfo,
      string userid)
    {
      this.loanFoldersAclMgr.SetPermission(feature, loanFolderAclInfo, userid);
    }

    public void SetPermission(
      AclFeature feature,
      LoanFolderAclInfo loanFolderAclInfo,
      int personaID)
    {
      this.loanFoldersAclMgr.SetPermission(feature, loanFolderAclInfo, personaID);
    }

    public void SetPermissions(
      AclFeature feature,
      LoanFolderAclInfo[] loanFolderAclInfoList,
      string userid)
    {
      this.loanFoldersAclMgr.SetPermissions(feature, loanFolderAclInfoList, userid);
    }

    public void SetPermissions(
      AclFeature feature,
      LoanFolderAclInfo[] loanFolderAclInfoList,
      int personaID)
    {
      this.loanFoldersAclMgr.SetPermissions(feature, loanFolderAclInfoList, personaID);
    }

    public void DuplicateACLLoanFolder(int sourcePersonaID, int desPersonaID)
    {
      this.loanFoldersAclMgr.DuplicateACLLoanFolder(sourcePersonaID, desPersonaID);
    }

    public override void ClearCaches(string key) => this.clearCache(key);

    public void GetAccessibleFoldersForMove(out List<string> fromList, out List<string> toList)
    {
      fromList = new List<string>();
      toList = new List<string>();
      if (!this.session.UserInfo.IsSuperAdministrator())
      {
        foreach (LoanFolderAclInfo accessibleLoanFolder in this.GetAccessibleLoanFolders(AclFeature.LoanMgmt_Move, this.session.UserID, this.session.UserInfo.UserPersonas))
        {
          if (accessibleLoanFolder.MoveFromAccess == 1)
            fromList.Add(accessibleLoanFolder.FolderName);
          if (accessibleLoanFolder.MoveToAccess == 1)
            toList.Add(accessibleLoanFolder.FolderName);
        }
      }
      else
      {
        foreach (string allLoanFolderName in this.session.LoanManager.GetAllLoanFolderNames(true, false))
        {
          toList.Add(allLoanFolderName);
          fromList.Add(allLoanFolderName);
        }
      }
    }
  }
}

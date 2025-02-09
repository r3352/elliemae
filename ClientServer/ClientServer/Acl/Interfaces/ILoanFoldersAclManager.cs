// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Acl.Interfaces.ILoanFoldersAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Acl.Interfaces
{
  public interface ILoanFoldersAclManager
  {
    LoanFolderAclInfo[] GetAccessibleLoanFolders(
      AclFeature feature,
      string userId,
      Persona[] personaList);

    LoanFolderAclInfo GetAccessibleLoanFolder(
      AclFeature feature,
      string userId,
      Persona[] personaList,
      string folderName);

    LoanFolderAclInfo GetPermission(AclFeature feature, int personaID, string folderName);

    LoanFolderAclInfo GetPermission(AclFeature feature, Persona[] personaList, string folderName);

    LoanFolderAclInfo[] GetPermissions(AclFeature feature, int personaID);

    LoanFolderAclInfo GetPermission(AclFeature feature, string userID, string folderName);

    LoanFolderAclInfo[] GetPermissions(AclFeature feature, string userID);

    void SetPermission(AclFeature feature, LoanFolderAclInfo loanFolderAclInfo, string userid);

    void SetPermission(AclFeature feature, LoanFolderAclInfo loanFolderAclInfo, int personaID);

    void SetPermissions(
      AclFeature feature,
      LoanFolderAclInfo[] loanFolderAclInfoList,
      string userid);

    void SetPermissions(
      AclFeature feature,
      LoanFolderAclInfo[] loanFolderAclInfoList,
      int personaID);

    void DuplicateACLLoanFolder(int sourcePersonaID, int desPersonaID);
  }
}

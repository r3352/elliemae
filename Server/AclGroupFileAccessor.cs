// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AclGroupFileAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class AclGroupFileAccessor
  {
    private const string className = "AclGroupFileAccessor�";
    private const string tableName_AclGroupFileRef = "[AclGroupFileRef]�";

    public static void UpdateAclGroupFileRef(int groupId, FileInGroup fileInGroup)
    {
      if (fileInGroup == (FileInGroup) null)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update AclGroupFileRef set access = " + (object) (int) fileInGroup.Access + ", Inclusive = " + SQL.EncodeFlag(fileInGroup.IsInclusive) + " where GroupID = " + (object) groupId + " and FileID = " + (object) fileInGroup.FileID);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static FileInGroup GetAclGroupFileRef(int groupId, int fileId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select * from AclGroupFileRef where FileID = " + (object) fileId + " and GroupID = " + (object) groupId);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection == null || dataRowCollection.Count == 0 ? (FileInGroup) null : new FileInGroup((int) dataRowCollection[0]["FileID"], (bool) SQL.Decode((object) ((byte) dataRowCollection[0]["Inclusive"] == (byte) 1), (object) false), (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), dataRowCollection[0]["Access"].ToString()));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupFileAccessor), ex);
        return (FileInGroup) null;
      }
    }

    public static FileInGroup[] GetAclGroupFileRefs(int groupId, AclFileType fileType)
    {
      return AclGroupFileAccessor.GetAclGroupFileRefs(new int[1]
      {
        groupId
      }, fileType);
    }

    public static Dictionary<AclFileType, FileInGroup[]> GetAclGroupFileRefs(
      int[] groupIds,
      AclFileType[] fileTypes)
    {
      Dictionary<AclFileType, FileInGroup[]> aclGroupFileRefs = new Dictionary<AclFileType, FileInGroup[]>();
      foreach (AclFileType fileType in fileTypes)
        aclGroupFileRefs.Add(fileType, AclGroupFileAccessor.GetAclGroupFileRefs(groupIds, fileType));
      return aclGroupFileRefs;
    }

    public static FileInGroup[] GetAclGroupFileRefs(int[] groupIds, AclFileType fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select ref.FileID, ref.Inclusive, max(ref.Access) as Access from AclGroupFileRef ref, FileResource res");
      dbQueryBuilder.AppendLine("   where ref.FileID = res.FileID");
      dbQueryBuilder.AppendLine("     and ref.GroupID in (" + SQL.Encode((object) groupIds) + ")");
      dbQueryBuilder.AppendLine("     and res.FileType = " + (object) (int) fileType);
      dbQueryBuilder.AppendLine("   group by ref.FileID, ref.Inclusive");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      FileInGroup[] aclGroupFileRefs = new FileInGroup[dataRowCollection.Count];
      for (int index = 0; index < aclGroupFileRefs.Length; ++index)
        aclGroupFileRefs[index] = new FileInGroup((int) dataRowCollection[index]["FileID"], (bool) SQL.Decode((object) ((byte) dataRowCollection[index]["Inclusive"] == (byte) 1), (object) false), (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), dataRowCollection[index]["Access"].ToString()));
      return aclGroupFileRefs;
    }

    public static FileInGroup[] GetUsersAclGroupFileRefs(string userId, AclFileType fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select ref.FileID, ref.Inclusive, max(ref.Access) as Access from AclGroupFileRef ref");
      dbQueryBuilder.AppendLine("   inner join FileResource res on ref.FileID = res.FileID");
      dbQueryBuilder.AppendLine("   inner join AclGroupMembers agm on ref.GroupID = agm.GroupID");
      dbQueryBuilder.AppendLine("where agm.UserID = " + SQL.Encode((object) userId));
      dbQueryBuilder.AppendLine("   and res.FileType = " + (object) (int) fileType);
      dbQueryBuilder.AppendLine("group by ref.FileID, ref.Inclusive");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      FileInGroup[] aclGroupFileRefs = new FileInGroup[dataRowCollection.Count];
      for (int index = 0; index < aclGroupFileRefs.Length; ++index)
        aclGroupFileRefs[index] = new FileInGroup((int) dataRowCollection[index]["FileID"], (bool) SQL.Decode((object) ((byte) dataRowCollection[index]["Inclusive"] == (byte) 1), (object) false), (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), dataRowCollection[index]["Access"].ToString()));
      return aclGroupFileRefs;
    }

    public static int[] GetAclGroupFileRefIDs(int groupId, AclFileType fileType)
    {
      return AclGroupFileAccessor.GetAclGroupFileRefIDs(new int[1]
      {
        groupId
      }, fileType);
    }

    public static int[] GetAclGroupFileRefIDs(int[] groupIds, AclFileType fileType)
    {
      return AclGroupFileAccessor.GetAclGroupFileRefIDs(groupIds, new AclFileType[1]
      {
        fileType
      });
    }

    public static int[] GetAclGroupFileRefIDs(int[] groupIds, AclFileType[] fileTypes)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct ref.FileID from AclGroupFileRef ref, FileResource res");
      dbQueryBuilder.AppendLine("   where ref.FileID = res.FileID");
      dbQueryBuilder.AppendLine("     and ref.GroupID in (" + SQL.Encode((object) groupIds) + ")");
      dbQueryBuilder.AppendLine("     and res.FileType in (" + SQL.Encode((object) AclUtils.GetEnumValueArray((Array) fileTypes)) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int[] aclGroupFileRefIds = new int[dataRowCollection.Count];
      for (int index = 0; index < aclGroupFileRefIds.Length; ++index)
        aclGroupFileRefIds[index] = (int) dataRowCollection[index]["FileID"];
      return aclGroupFileRefIds;
    }

    public static AclFileResource GetAclFileResource(int fileId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select * from FileResource where FileID = " + (object) fileId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection == null ? (AclFileResource) null : AclGroupFileAccessor.dataRowToAclFileResource(dataRowCollection[0]);
    }

    public static AclFileResource[] GetAclFileResources(int[] fileIds)
    {
      AclFileResource[] aclFileResources = new AclFileResource[fileIds.Length];
      for (int index = 0; index < fileIds.Length; ++index)
        aclFileResources[index] = AclGroupFileAccessor.GetAclFileResource(fileIds[index]);
      return aclFileResources;
    }

    public static AclFileResource[] GetUsersAclFileResources(string userId, AclFileType fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct res.* from FileResource res");
      dbQueryBuilder.AppendLine("  inner join AclGroupFileRef agfr on agfr.FileID = res.FileID");
      dbQueryBuilder.AppendLine("  inner join AclGroupMembers agm on agm.GroupID = agfr.GroupID");
      dbQueryBuilder.AppendLine("where res.FileType = " + (object) (int) fileType);
      dbQueryBuilder.AppendLine("  and agm.UserID = " + SQL.Encode((object) userId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ArrayList arrayList = new ArrayList();
      foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
        arrayList.Add((object) AclGroupFileAccessor.dataRowToAclFileResource(r));
      return (AclFileResource[]) arrayList.ToArray(typeof (AclFileResource));
    }

    private static AclFileResource dataRowToAclFileResource(DataRow r)
    {
      return new AclFileResource((int) r["FileID"], (string) r["FilePath"], (AclFileType) r["FileType"], (bool) SQL.Decode(r["IsFolder"], (object) false), (string) r["Owner"]);
    }

    public static void ResetAclGroupFileRefs(
      int groupId,
      FileInGroup[] filesInGroup,
      AclFileType fileType,
      int[] resetFileIDs,
      string loggedInUser)
    {
      try
      {
        if (filesInGroup == null)
          return;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        foreach (int resetFileId in resetFileIDs)
          dbQueryBuilder.AppendLine("delete AclGroupFileRef from AclGroupFileRef inner join FileResource on AclGroupFileRef.FileID = FileResource.FileID where AclGroupFileRef.GroupID = " + (object) groupId + " and FileResource.FileType = " + (object) (int) fileType + " and AclGroupFileRef.FileID = " + (object) resetFileId);
        for (int index = 0; index < filesInGroup.Length; ++index)
          dbQueryBuilder.AppendLine("insert into [AclGroupFileRef] (GroupID, FileID, Inclusive, Access) values (" + (object) groupId + ", " + (object) filesInGroup[index].FileID + ", " + SQL.EncodeFlag(filesInGroup[index].IsInclusive) + ", " + (object) (int) filesInGroup[index].Access + ")");
        if (!(dbQueryBuilder.ToString() != ""))
          return;
        if (loggedInUser != null)
          dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupId));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupFileAccessor), ex);
      }
    }

    public static void ResetAclGroupFileRefs(
      int groupId,
      FileInGroup[] filesInGroup,
      AclFileType fileType,
      string loggedInUser)
    {
      try
      {
        if (filesInGroup == null)
          return;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete AclGroupFileRef from AclGroupFileRef inner join FileResource on AclGroupFileRef.FileID = FileResource.FileID where AclGroupFileRef.GroupID = " + (object) groupId + " and FileResource.FileType = " + (object) (int) fileType);
        for (int index = 0; index < filesInGroup.Length; ++index)
          dbQueryBuilder.AppendLine("insert into [AclGroupFileRef] (GroupID, FileID, Inclusive, Access) values (" + (object) groupId + ", " + (object) filesInGroup[index].FileID + ", " + SQL.EncodeFlag(filesInGroup[index].IsInclusive) + ", " + (object) (int) filesInGroup[index].Access + ")");
        if (dbQueryBuilder.ToString() != "")
          dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupId));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupFileAccessor), ex);
      }
    }

    public static void UpdateFileResources(
      string oldName,
      string newName,
      int fileType,
      bool isFolder)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select FileID from FileResource where FilePath = " + SQL.Encode((object) oldName) + " and fileType = " + (object) fileType);
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1 != null && dataRowCollection1.Count > 0)
      {
        string str = string.Concat(dataRowCollection1[0][0]);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("Update FileResource set FilePath = " + SQL.Encode((object) newName) + " where FileID = " + str);
        dbQueryBuilder.ExecuteNonQuery();
      }
      if (!isFolder)
        return;
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("Select * from FileResource where FilePath like '" + SQL.Escape(oldName) + "%' and fileType = " + (object) fileType);
      DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
      if (dataRowCollection2 == null || dataRowCollection2.Count <= 0)
        return;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection2)
      {
        string str = string.Concat(dataRow["FilePath"]).Replace(oldName, newName);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("Update FileResource set FilePath = " + SQL.Encode((object) str) + " where FileID = " + dataRow["FileID"]);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static Dictionary<int, AclFileResource> AddAclFileResources(
      AclFileResource[] fileResources)
    {
      try
      {
        int[] numArray = new int[fileResources.Length];
        Dictionary<int, AclFileResource> dictionary = new Dictionary<int, AclFileResource>();
        for (int index = 0; index < fileResources.Length; ++index)
        {
          AclFileResource fileResource = fileResources[index];
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          if (fileResource.Owner == null)
            dbQueryBuilder.Append("select FileID from FileResource where FilePath = " + SQL.Encode((object) fileResource.FilePath) + " and FileType = " + (object) (int) fileResource.FileType + " and IsFolder = " + SQL.EncodeFlag(fileResource.IsFolder) + " and Owner = '' ");
          else
            dbQueryBuilder.Append("select FileID from FileResource where FilePath = " + SQL.Encode((object) fileResource.FilePath) + " and FileType = " + (object) (int) fileResource.FileType + " and IsFolder = " + SQL.EncodeFlag(fileResource.IsFolder) + " and Owner = " + SQL.Encode((object) fileResource.Owner));
          object obj = dbQueryBuilder.ExecuteScalar();
          int key;
          if (obj == null)
          {
            dbQueryBuilder.Reset();
            dbQueryBuilder.Declare("@fileId", "int");
            dbQueryBuilder.InsertInto(DbAccessManager.GetTable("FileResource"), new DbValueList()
            {
              {
                "FilePath",
                (object) fileResource.FilePath
              },
              {
                "FileType",
                (object) (int) fileResource.FileType
              },
              {
                "IsFolder",
                (object) fileResource.IsFolder,
                (IDbEncoder) DbEncoding.Flag
              },
              {
                "Owner",
                fileResource.Owner == null ? (object) "" : (object) fileResource.Owner
              }
            }, true, false);
            dbQueryBuilder.SelectIdentity("@fileId");
            dbQueryBuilder.Select("@fileId");
            key = (int) dbQueryBuilder.ExecuteScalar();
          }
          else
            key = (int) obj;
          fileResource.FileID = key;
          dictionary.Add(key, fileResource);
        }
        return dictionary;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupFileAccessor), ex);
        return (Dictionary<int, AclFileResource>) null;
      }
    }

    public static AclFileType ConvertToAclFileType(TemplateSettingsType type)
    {
      AclFileType aclFileType = AclFileType.None;
      switch (type)
      {
        case TemplateSettingsType.CustomLetter:
          aclFileType = AclFileType.CustomPrintForms;
          break;
        case TemplateSettingsType.LoanProgram:
          aclFileType = AclFileType.LoanProgram;
          break;
        case TemplateSettingsType.ClosingCost:
          aclFileType = AclFileType.ClosingCost;
          break;
        case TemplateSettingsType.MiscData:
          aclFileType = AclFileType.MiscData;
          break;
        case TemplateSettingsType.FormList:
          aclFileType = AclFileType.FormList;
          break;
        case TemplateSettingsType.DocumentSet:
          aclFileType = AclFileType.DocumentSet;
          break;
        case TemplateSettingsType.LoanTemplate:
          aclFileType = AclFileType.LoanTemplate;
          break;
        case TemplateSettingsType.UnderwritingConditionSet:
          aclFileType = AclFileType.UnderwritingConditionSet;
          break;
        case TemplateSettingsType.PostClosingConditionSet:
          aclFileType = AclFileType.PostClosingConditionSet;
          break;
        case TemplateSettingsType.Campaign:
          aclFileType = AclFileType.CampaignTemplate;
          break;
        case TemplateSettingsType.DashboardTemplate:
          aclFileType = AclFileType.DashboardTemplate;
          break;
        case TemplateSettingsType.DashboardViewTemplate:
          aclFileType = AclFileType.DashboardViewTemplate;
          break;
        case TemplateSettingsType.TaskSet:
          aclFileType = AclFileType.TaskSet;
          break;
        case TemplateSettingsType.ConditionalLetter:
          aclFileType = AclFileType.ConditionalApprovalLetter;
          break;
        case TemplateSettingsType.SettlementServiceProviders:
          aclFileType = AclFileType.SettlementServiceProviders;
          break;
        case TemplateSettingsType.AffiliatedBusinessArrangements:
          aclFileType = AclFileType.AffiliatedBusinessArrangements;
          break;
        case TemplateSettingsType.ChangeOfCircumstanceOptions:
          aclFileType = AclFileType.ChangeOfCircumstanceOptions;
          break;
      }
      return aclFileType;
    }

    public static AclFileType ConvertToAclFileType(CustomLetterType type)
    {
      AclFileType aclFileType = AclFileType.None;
      switch (type)
      {
        case CustomLetterType.Borrower:
          aclFileType = AclFileType.BorrowerCustomLetters;
          break;
        case CustomLetterType.BizPartner:
          aclFileType = AclFileType.BizCustomLetters;
          break;
        case CustomLetterType.Generic:
          aclFileType = AclFileType.CustomPrintForms;
          break;
      }
      return aclFileType;
    }

    public static bool DeleteFileResource(AclFileType aclType, FileSystemEntry entry)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (aclType == AclFileType.None)
          return false;
        bool flag = entry.Type == FileSystemEntry.Types.Folder;
        if (!flag)
          dbQueryBuilder.AppendLine("delete AclGroupFileRef where FileID in (Select FileID from FileResource Where FilePath = " + SQL.EncodeString("Public:" + entry.Path) + " and FileType = " + (object) (int) aclType + ")");
        else
          dbQueryBuilder.AppendLine("delete AclGroupFileRef where FileID in (Select FileID from FileResource Where FilePath like (" + SQL.EncodeString("Public:" + entry.Path) + ") and FileType = " + (object) (int) aclType + ")");
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
        if (!flag)
          dbQueryBuilder.AppendLine("Delete FileResource Where FilePath = " + SQL.EncodeString("Public:" + entry.Path) + " and FileType = " + (object) (int) aclType);
        else
          dbQueryBuilder.AppendLine("Delete FileResource Where FilePath like (" + SQL.EncodeString("Public:" + entry.Path) + ") and FileType = " + (object) (int) aclType);
        dbQueryBuilder.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupFileAccessor), ex);
        return false;
      }
    }

    public static bool CheckPublicAccessPermission(AclFileType fileType, string userId)
    {
      AclGroup[] groupsOfUser = AclGroupAccessor.GetGroupsOfUser(userId);
      return groupsOfUser.Length != 0 && AclGroupFileAccessor.GetAclGroupFileRefIDs(AclUtils.GetAclGroupIDs(groupsOfUser), fileType).Length != 0;
    }

    public static bool CheckPublicAccessPermissionToAny(AclFileType[] fileTypes, string userId)
    {
      AclGroup[] groupsOfUser = AclGroupAccessor.GetGroupsOfUser(userId);
      return groupsOfUser.Length != 0 && AclGroupFileAccessor.GetAclGroupFileRefIDs(AclUtils.GetAclGroupIDs(groupsOfUser), fileTypes).Length != 0;
    }

    public static AclResourceAccess GetMaxPublicFolderAccess(AclFileType fileType, string userId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select max(access) from AclGroupFileRef ref");
      dbQueryBuilder.AppendLine("   inner join FileResource res on ref.FileID = res.FileID");
      dbQueryBuilder.AppendLine("   inner join AclGroupMembers agm on agm.GroupID = ref.GroupID");
      dbQueryBuilder.AppendLine("where agm.UserID = " + SQL.Encode((object) userId));
      dbQueryBuilder.AppendLine("   and res.FileType = " + (object) (int) fileType);
      dbQueryBuilder.AppendLine("   and res.IsFolder = 1");
      return (AclResourceAccess) Convert.ToInt32(SQL.Decode(dbQueryBuilder.ExecuteScalar(), (object) -1));
    }

    public static AclResourceAccess GetUserFileFolderAccess(
      AclFileType fileType,
      FileSystemEntry fsEntry,
      string userId)
    {
      int[] aclGroupIds = AclUtils.GetAclGroupIDs(AclGroupAccessor.GetGroupsOfUser(userId));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      while (fsEntry != null)
      {
        dbQueryBuilder.AppendLine("select access from AclGroupFileRef ref, FileResource res");
        dbQueryBuilder.AppendLine("   where ref.FileID = res.FileID");
        dbQueryBuilder.AppendLine("     and ref.GroupID in (" + SQL.Encode((object) aclGroupIds) + ")");
        dbQueryBuilder.AppendLine("     and res.FileType = " + (object) (int) fileType);
        dbQueryBuilder.AppendLine("     and res.IsFolder = " + SQL.EncodeFlag(fsEntry.Type == FileSystemEntry.Types.Folder));
        string str;
        if (fsEntry.Path != "\\")
        {
          str = fsEntry.ToDisplayString();
        }
        else
        {
          switch (fileType)
          {
            case AclFileType.LoanProgram:
              str = "Public:\\Public Loan Programs\\";
              break;
            case AclFileType.ClosingCost:
              str = "Public:\\Public Closing Cost Templates\\";
              break;
            case AclFileType.MiscData:
              str = "Public:\\Public Data Templates\\";
              break;
            case AclFileType.FormList:
              str = "Public:\\Public Form Lists\\";
              break;
            case AclFileType.DocumentSet:
              str = "Public:\\Public Document Sets\\";
              break;
            case AclFileType.LoanTemplate:
              str = "Public:\\Public Loan Templates\\";
              break;
            case AclFileType.CustomPrintForms:
              str = "Public:\\Public Custom Forms\\";
              break;
            case AclFileType.PrintGroups:
              str = "Public:\\Public Forms Groups\\";
              break;
            case AclFileType.Reports:
              str = "Public:\\Public Reports\\";
              break;
            case AclFileType.BorrowerCustomLetters:
              str = "Public:\\Public Custom Letters\\";
              break;
            case AclFileType.BizCustomLetters:
              str = "Public:\\Public Custom Letters\\";
              break;
            case AclFileType.CampaignTemplate:
              str = "Public:\\Public Campaign Templates\\";
              break;
            case AclFileType.DashboardTemplate:
              str = "Public:\\Public Dashboard Templates\\";
              break;
            case AclFileType.DashboardViewTemplate:
              str = "Public:\\Public DashboardView Templates\\";
              break;
            case AclFileType.TaskSet:
              str = "Public:\\Public Task Sets\\";
              break;
            case AclFileType.SettlementServiceProviders:
              str = "Public:\\Public Settlement Service Providers\\";
              break;
            case AclFileType.AffiliatedBusinessArrangements:
              str = "Public:\\Public Affiliates\\";
              break;
            default:
              str = fsEntry.ToDisplayString();
              break;
          }
        }
        dbQueryBuilder.AppendLine("     and res.FilePath like '" + SQL.Escape(str) + "'");
        dbQueryBuilder.AppendLine("order by access desc");
        object obj = SQL.Decode(dbQueryBuilder.ExecuteScalar(), (object) null);
        if (obj != null)
          return (AclResourceAccess) Convert.ToInt32(obj);
        fsEntry = fsEntry.ParentFolder;
        dbQueryBuilder.Reset();
      }
      return AclResourceAccess.None;
    }

    public static void Clone(int sourceGroupID, int desGroupID)
    {
      try
      {
        DbQueryBuilder sql = new DbQueryBuilder();
        sql.Append("select * from [AclGroupFileRef] where GroupID = " + (object) sourceGroupID);
        DataTable sourceTable = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable == null || sourceTable.Rows.Count <= 0)
          return;
        AclGroupFileAccessor.CloneStatementHelper(sourceTable, sql, "[AclGroupFileRef]", "GroupID", desGroupID);
        sql.ExecuteNonQuery();
        sql.Reset();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupFileAccessor), ex);
      }
    }

    private static void CloneStatementHelper(
      DataTable sourceTable,
      DbQueryBuilder sql,
      string tableName,
      string keyColumnName,
      int desKeyIDValue)
    {
      AclGroupAccessor.CloneStatementHelper(sourceTable, sql, tableName, keyColumnName, desKeyIDValue);
    }

    private static FileSystemEntry[] applyUserAccessRightsFromUserGroup(
      string userID,
      int groupID,
      FileSystemEntry[] fsEntries,
      AclFileType fileType)
    {
      if (fsEntries == null)
        return (FileSystemEntry[]) null;
      bool flag = false;
      foreach (FileSystemEntry fsEntry in fsEntries)
      {
        if (fsEntry.IsPublic)
          flag = true;
      }
      if (fileType == AclFileType.None || !flag)
        return fsEntries;
      FileInGroup[] aclGroupFileRefs = AclGroupFileAccessor.GetAclGroupFileRefs(groupID, fileType);
      AclFileResource[] aclFileResources = AclGroupFileAccessor.GetUsersAclFileResources(userID, fileType);
      Dictionary<int, AclFileResource> dictionary1 = new Dictionary<int, AclFileResource>();
      foreach (AclFileResource aclFileResource in aclFileResources)
        dictionary1[aclFileResource.FileID] = aclFileResource;
      FileSystemEntry rootFileSystemEntry = AclGroupFileAccessor.GetRootFileSystemEntry(fileType);
      Dictionary<string, AclResourceAccess> dictionary2 = new Dictionary<string, AclResourceAccess>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      Dictionary<string, AclResourceAccess> dictionary3 = new Dictionary<string, AclResourceAccess>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      Dictionary<string, AclResourceAccess> dictionary4 = new Dictionary<string, AclResourceAccess>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (FileInGroup fileInGroup in aclGroupFileRefs)
      {
        AclFileResource aclFileResource = (AclFileResource) null;
        if (dictionary1.TryGetValue(fileInGroup.FileID, out aclFileResource))
        {
          string filePath = aclFileResource.FilePath;
          if (filePath == rootFileSystemEntry.ToString())
            filePath = FileSystemEntry.PublicRoot.ToString();
          if (!dictionary2.ContainsKey(filePath))
            dictionary2[filePath] = fileInGroup.Access;
          else if (dictionary2[filePath] < fileInGroup.Access)
            dictionary2[filePath] = fileInGroup.Access;
          if (fileInGroup.IsInclusive)
            dictionary3[filePath] = fileInGroup.Access;
          if (fileInGroup.Access != AclResourceAccess.None)
          {
            for (FileSystemEntry parentFolder = FileSystemEntry.Parse(filePath); parentFolder.ParentFolder != null; parentFolder = parentFolder.ParentFolder)
              dictionary4[parentFolder.ParentFolder.ToString()] = AclResourceAccess.ReadOnly;
          }
        }
      }
      foreach (FileSystemEntry fsEntry in fsEntries)
      {
        if (fsEntry.IsPublic && fsEntry.Access != AclResourceAccess.ReadWrite)
        {
          if (dictionary2.ContainsKey(fsEntry.ToString()))
            fsEntry.Access = dictionary2[fsEntry.ToString()];
          else if (fsEntry.Type == FileSystemEntry.Types.File && dictionary2.ContainsKey(fsEntry.ParentFolder.ToString()))
            fsEntry.Access = dictionary2[fsEntry.ParentFolder.ToString()];
          else if (dictionary3.Count > 0)
          {
            FileSystemEntry parentFolder1 = fsEntry.ParentFolder;
            while (parentFolder1 != null && !dictionary3.ContainsKey(parentFolder1.ToString()))
              parentFolder1 = parentFolder1.ParentFolder;
            if (parentFolder1 != null)
            {
              fsEntry.Access = dictionary3[parentFolder1.ToString()];
              for (FileSystemEntry parentFolder2 = fsEntry.ParentFolder; !parentFolder2.Equals((object) parentFolder1); parentFolder2 = parentFolder2.ParentFolder)
                dictionary2[parentFolder2.ToString()] = fsEntry.Access;
            }
          }
          if (fsEntry.Access == AclResourceAccess.None && fsEntry.Type == FileSystemEntry.Types.Folder && dictionary4.ContainsKey(fsEntry.ToString()))
            fsEntry.Access = dictionary4[fsEntry.ToString()];
        }
      }
      return fsEntries;
    }

    public static FileSystemEntry[] ApplyUserAccessRights(
      UserInfo userInfo,
      FileSystemEntry[] fsEntries,
      AclFileType fileType)
    {
      if (fsEntries == null)
        return (FileSystemEntry[]) null;
      bool flag = false;
      foreach (FileSystemEntry fsEntry in fsEntries)
      {
        fsEntry.Access = fileType != AclFileType.None ? (userInfo.IsSuperAdministrator() || userInfo.Userid == fsEntry.Owner ? AclResourceAccess.ReadWrite : AclResourceAccess.None) : AclResourceAccess.ReadWrite;
        if (fsEntry.IsPublic)
          flag = true;
      }
      if (userInfo.IsSuperAdministrator() || fileType == AclFileType.None || !flag)
        return fsEntries;
      foreach (AclGroup aclGroup in AclGroupAccessor.GetGroupsOfUser(userInfo.Userid))
        AclGroupFileAccessor.applyUserAccessRightsFromUserGroup(userInfo.Userid, aclGroup.ID, fsEntries, fileType);
      List<FileSystemEntry> fileSystemEntryList = new List<FileSystemEntry>();
      for (int index = 0; index < fsEntries.Length; ++index)
      {
        if (fsEntries[index].Access != AclResourceAccess.None)
          fileSystemEntryList.Add(fsEntries[index]);
      }
      return fileSystemEntryList.ToArray();
    }

    public static FileSystemEntry GetRootFileSystemEntry(AclFileType type)
    {
      switch (type)
      {
        case AclFileType.LoanProgram:
          return new FileSystemEntry("\\", "Public Loan Programs", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.ClosingCost:
          return new FileSystemEntry("\\", "Public Closing Cost Templates", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.MiscData:
          return new FileSystemEntry("\\", "Public Data Templates", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.FormList:
          return new FileSystemEntry("\\", "Public Form Lists", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.DocumentSet:
          return new FileSystemEntry("\\", "Public Document Sets", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.LoanTemplate:
          return new FileSystemEntry("\\", "Public Loan Templates", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.CustomPrintForms:
          return new FileSystemEntry("\\", "Public Custom Forms", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.PrintGroups:
          return new FileSystemEntry("\\", "Public Forms Groups", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.Reports:
          return new FileSystemEntry("\\", "Public Reports", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.BorrowerCustomLetters:
          return new FileSystemEntry("\\", "Public Custom Letters", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.BizCustomLetters:
          return new FileSystemEntry("\\", "Public Custom Letters", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.CampaignTemplate:
          return new FileSystemEntry("\\", "Public Campaign Templates", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.DashboardTemplate:
          return new FileSystemEntry("\\", "Public Dashboard Templates", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.DashboardViewTemplate:
          return new FileSystemEntry("\\", "Public DashboardView Templates", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.TaskSet:
          return new FileSystemEntry("\\", "Public Task Sets", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.ConditionalApprovalLetter:
          return new FileSystemEntry("\\", "Public Conditional Letters", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.SettlementServiceProviders:
          return new FileSystemEntry("\\", "Public Settlement Service Providers", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.AffiliatedBusinessArrangements:
          return new FileSystemEntry("\\", "Public Affiliates", FileSystemEntry.Types.Folder, (string) null);
        case AclFileType.ChangeOfCircumstanceOptions:
          return new FileSystemEntry("\\", "Change Of Circumstance Options", FileSystemEntry.Types.Folder, (string) null);
        default:
          throw new Exception("AclGroupFileAccessor.GetRootFileSystemEntry: No matching file type for '" + (object) type + "'");
      }
    }
  }
}

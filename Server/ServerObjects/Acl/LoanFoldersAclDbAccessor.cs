// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.LoanFoldersAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class LoanFoldersAclDbAccessor
  {
    private const string tableName = "[AclF_LoanFolderConfig]�";
    private const string tableName_User = "[AclF_LoanFolderConfig_User]�";

    private LoanFoldersAclDbAccessor()
    {
    }

    public static void UpdateUserLoanFolderConfiguration(
      AclFeature feature,
      LoanFolderAclInfo[] loanFolderAclInfoList,
      string userID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text1 = "DELETE [AclF_LoanFolderConfig_User] WHERE userID = '" + userID + "' AND featureID = " + (object) (int) feature;
      dbQueryBuilder.Append(text1);
      dbQueryBuilder.ExecuteNonQuery();
      dbQueryBuilder.Reset();
      str1 = "";
      if (loanFolderAclInfoList == null)
        return;
      foreach (LoanFolderAclInfo loanFolderAclInfo in loanFolderAclInfoList)
      {
        str2 = "";
        string text2 = "INSERT INTO [AclF_LoanFolderConfig_User] (featureID, userID, folderName, moveFrom, moveTo) VALUES (" + (object) loanFolderAclInfo.FeatureID + ", '" + userID + "', " + SQL.Encode((object) loanFolderAclInfo.FolderName) + ", " + (object) loanFolderAclInfo.MoveFromAccess + ", " + (object) loanFolderAclInfo.MoveToAccess + ") ";
        dbQueryBuilder.AppendLine(text2);
        str2 = "";
      }
      if (loanFolderAclInfoList.Length == 0)
        return;
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdatePersonaLoanFolderConfiguration(
      AclFeature feature,
      LoanFolderAclInfo[] loanFolderAclInfoList,
      string personaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text1 = "DELETE [AclF_LoanFolderConfig] WHERE personaID = " + personaID + " AND featureID = " + (object) (int) feature;
      dbQueryBuilder.AppendLine(text1);
      str1 = "";
      LicenseInfo serverLicense = Company.GetServerLicense();
      if (feature == AclFeature.LoanMgmt_Import || serverLicense.Edition == EncompassEdition.Banker)
      {
        foreach (LoanFolderAclInfo loanFolderAclInfo in loanFolderAclInfoList)
        {
          str2 = "";
          string text2 = "INSERT INTO [AclF_LoanFolderConfig] (featureID, personaID, folderName, moveFrom, moveTo) VALUES (" + (object) loanFolderAclInfo.FeatureID + ", " + personaID + ", " + SQL.Encode((object) loanFolderAclInfo.FolderName) + ", " + (object) loanFolderAclInfo.MoveFromAccess + ", " + (object) loanFolderAclInfo.MoveToAccess + ") ";
          dbQueryBuilder.AppendLine(text2);
          str2 = "";
        }
      }
      else
      {
        foreach (LoanFolder allLoanFolder in LoanFolder.GetAllLoanFolders())
        {
          str2 = "";
          string text3 = "INSERT INTO [AclF_LoanFolderConfig] (featureID, personaID, folderName, moveFrom, moveTo) VALUES (" + (object) (int) feature + ", " + personaID + ", " + SQL.Encode((object) allLoanFolder.Name) + ", 1, 1) ";
          dbQueryBuilder.AppendLine(text3);
          str2 = "";
        }
      }
      if (loanFolderAclInfoList.Length == 0)
        return;
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static LoanFolderAclInfo[] GetPersonaLoanFolderConfiguration(
      AclFeature feature,
      string folderName,
      string personaID,
      string[] folderFilterList,
      bool defaultAccess)
    {
      LoanFolderAclInfo[] loanFolderAclInfoList = (LoanFolderAclInfo[]) null;
      Hashtable hashtable = new Hashtable((IEqualityComparer) StringComparer.InvariantCultureIgnoreCase);
      bool flag = false;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (int.Parse(personaID) == 1 && folderName != "")
      {
        dbQueryBuilder.AppendLine("If not exists (select 1 from [AclF_LoanFolderConfig] where featureID = " + (object) (int) feature + " and personaID = 1 and folderName = " + SQL.EncodeString(folderName) + ")");
        dbQueryBuilder.AppendLine("Begin");
        dbQueryBuilder.AppendLine("INSERT INTO [AclF_LoanFolderConfig] (featureID, personaID, folderName, moveFrom, moveTo) VALUES (" + (object) (int) feature + ", '" + personaID + "', " + SQL.Encode((object) folderName) + ", 1, 1)");
        dbQueryBuilder.AppendLine("End");
        dbQueryBuilder.AppendLine("Else");
        dbQueryBuilder.AppendLine("Begin");
        dbQueryBuilder.AppendLine("Update [AclF_LoanFolderConfig] set moveFrom = 1 where personaID = " + personaID);
        dbQueryBuilder.AppendLine("Update [AclF_LoanFolderConfig] set moveTo = 1 where personaID = " + personaID);
        dbQueryBuilder.AppendLine("End");
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
      }
      string str1 = "SELECT * FROM [AclF_LoanFolderConfig] ";
      string str2 = "WHERE personaID = " + personaID + " AND featureID = " + (object) (int) feature + " AND folderName in (" + SQL.EncodeArray((Array) folderFilterList) + ")";
      if ((folderName ?? "") != "")
        str2 = str2 + "AND folderName = " + SQL.Encode((object) folderName);
      string text = str1 + str2;
      if (folderFilterList.Length != 0)
      {
        dbQueryBuilder.AppendLine("Delete [AclF_LoanFolderConfig] where folderName Not In (" + SQL.EncodeArray((Array) folderFilterList) + ") and featureID = " + (object) (int) feature);
        dbQueryBuilder.AppendLine("Delete [AclF_LoanFolderConfig_User] where folderName Not In (" + SQL.EncodeArray((Array) folderFilterList) + ") and featureID = " + (object) (int) feature);
      }
      dbQueryBuilder.AppendLine(text);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          hashtable.Add((object) string.Concat(row[nameof (folderName)]), (object) LoanFoldersAclDbAccessor.ConvertDataRowToObject(row));
      }
      foreach (string folderFilter in folderFilterList)
      {
        if (!hashtable.ContainsKey((object) folderFilter))
        {
          flag = true;
          LoanFolderAclInfo loanFolderAclInfo = !defaultAccess ? new LoanFolderAclInfo((int) feature, int.Parse(personaID), folderFilter, 0, 0) : new LoanFolderAclInfo((int) feature, int.Parse(personaID), folderFilter, 1, 1);
          hashtable.Add((object) folderFilter, (object) loanFolderAclInfo);
        }
      }
      if (hashtable.Count > 0)
      {
        ArrayList arrayList = new ArrayList();
        IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
        while (enumerator.MoveNext())
          arrayList.Add(enumerator.Value);
        loanFolderAclInfoList = (LoanFolderAclInfo[]) arrayList.ToArray(typeof (LoanFolderAclInfo));
      }
      if (flag && loanFolderAclInfoList.Length != 0)
        LoanFoldersAclDbAccessor.UpdatePersonaLoanFolderConfiguration(feature, loanFolderAclInfoList, personaID);
      return loanFolderAclInfoList;
    }

    public static LoanFolderAclInfo GetPersonaLoanFolderConfiguration(
      AclFeature feature,
      string folderName,
      int[] personaIDList,
      bool defaultAccess)
    {
      string str = SQL.Encode((object) personaIDList);
      DataTable dataTable1 = new DataTable();
      LoanFolderAclInfo folderConfiguration = new LoanFolderAclInfo((int) feature, -1, folderName, -1, -1);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (defaultAccess)
      {
        folderConfiguration.MoveFromAccess = 1;
        folderConfiguration.MoveToAccess = 1;
      }
      else
      {
        folderConfiguration.MoveFromAccess = 0;
        folderConfiguration.MoveToAccess = 0;
      }
      string text = "SELECT A.moveFrom as PMoveFrom, A.moveTo as PMoveTo FROM AclF_LoanFolderConfig A WHERE A.personaID in (" + str + ") AND A.folderName = " + SQL.Encode((object) folderName) + " AND featureID =" + (object) (int) feature;
      dbQueryBuilder.Reset();
      foreach (int personaId in personaIDList)
      {
        if (personaId == 1)
        {
          if (folderName != "")
          {
            dbQueryBuilder.AppendLine("If not exists (select 1 from [AclF_LoanFolderConfig] where featureID = " + (object) (int) feature + " and personaID = 1 and folderName = " + SQL.EncodeString(folderName) + ")");
            dbQueryBuilder.AppendLine("Begin");
            dbQueryBuilder.AppendLine("INSERT INTO [AclF_LoanFolderConfig] (featureID, personaID, folderName, moveFrom, moveTo) VALUES (" + (object) (int) feature + ", '" + (object) personaId + "', " + SQL.Encode((object) folderName) + ", 1, 1)");
            dbQueryBuilder.AppendLine("End");
            dbQueryBuilder.AppendLine("Else");
            dbQueryBuilder.AppendLine("Begin");
            dbQueryBuilder.AppendLine("Update [AclF_LoanFolderConfig] set moveFrom = 1 where personaID = " + (object) personaId);
            dbQueryBuilder.AppendLine("Update [AclF_LoanFolderConfig] set moveTo = 1 where personaID = " + (object) personaId);
            dbQueryBuilder.AppendLine("End");
            dbQueryBuilder.ExecuteNonQuery();
            dbQueryBuilder.Reset();
            break;
          }
          break;
        }
      }
      dbQueryBuilder.AppendLine(text);
      DataTable dataTable2 = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable2 != null && dataTable2.Rows.Count > 0)
      {
        DataRow[] dataRowArray1 = dataTable2.Select("PMoveFrom > -1", "PMoveFrom");
        if (dataRowArray1 != null && dataRowArray1.Length != 0)
          folderConfiguration.MoveFromAccess = int.Parse(string.Concat(dataRowArray1[0]["PMoveFrom"]));
        DataRow[] dataRowArray2 = dataTable2.Select("PMoveTo > -1", "PMoveTo");
        if (dataRowArray2 != null && dataRowArray2.Length != 0)
          folderConfiguration.MoveToAccess = int.Parse(string.Concat(dataRowArray2[0]["PMoveTo"]));
      }
      return folderConfiguration;
    }

    public static LoanFolderAclInfo[] GetUserLoanFolderConfiguration(
      AclFeature feature,
      string folderName,
      string userId)
    {
      LoanFolderAclInfo[] folderConfiguration = (LoanFolderAclInfo[]) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str1 = "SELECT * FROM [AclF_LoanFolderConfig_User] ";
      string str2 = "WHERE ";
      string str3;
      if (folderName != "")
        str3 = str2 + "folderName = " + SQL.Encode((object) folderName) + " AND userID = '" + userId + "' AND featureID = " + (object) (int) feature;
      else
        str3 = str2 + "userID = '" + userId + "' AND featureID = " + (object) (int) feature;
      string text = str1 + str3;
      dbQueryBuilder.Append(text);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery(DbTransactionType.None);
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        ArrayList arrayList = new ArrayList(dataTable.Rows.Count);
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          arrayList.Add((object) LoanFoldersAclDbAccessor.ConvertDataRowToObject(row));
        folderConfiguration = (LoanFolderAclInfo[]) arrayList.ToArray(typeof (LoanFolderAclInfo));
      }
      return folderConfiguration;
    }

    public static LoanFolderAclInfo[] GetUserLoanFolderAccessibility(
      AclFeature feature,
      string folderName,
      string userId,
      int[] personaIDList,
      string[] folderNameList,
      bool defaultAccess)
    {
      UserInfo userInfo = UserStore.GetLatestVersion(userId).UserInfo;
      bool flag = userId == "admin" || UserInfo.IsSuperAdministrator(userInfo.Userid, userInfo.UserPersonas);
      ArrayList arrayList = new ArrayList();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (folderNameList != null)
      {
        foreach (Persona userPersona in userInfo.UserPersonas)
        {
          if (userPersona.ID == 1)
          {
            foreach (string folderName1 in folderNameList)
            {
              dbQueryBuilder.AppendLine("If not exists (select 1 from [AclF_LoanFolderConfig] where featureID = " + (object) (int) feature + " and personaID = 1 and folderName = " + SQL.EncodeString(folderName1) + ")");
              dbQueryBuilder.AppendLine("Begin");
              dbQueryBuilder.AppendLine("INSERT INTO [AclF_LoanFolderConfig] (featureID, personaID, folderName, moveFrom, moveTo) VALUES (" + (object) (int) feature + ", '" + (object) userPersona.ID + "', " + SQL.Encode((object) folderName1) + ", 1, 1)");
              dbQueryBuilder.AppendLine("End");
              dbQueryBuilder.AppendLine("Else");
              dbQueryBuilder.AppendLine("Begin");
              dbQueryBuilder.AppendLine("Update [AclF_LoanFolderConfig] set moveFrom = 1 where personaID = " + (object) userPersona.ID);
              dbQueryBuilder.AppendLine("Update [AclF_LoanFolderConfig] set moveTo = 1 where personaID = " + (object) userPersona.ID);
              dbQueryBuilder.AppendLine("End");
              dbQueryBuilder.ExecuteNonQuery();
              dbQueryBuilder.Reset();
            }
            break;
          }
        }
        EncompassEdition currentEdition = Company.GetCurrentEdition();
        string str = "";
        if (personaIDList != null && personaIDList.Length != 0)
          str = SQL.Encode((object) personaIDList);
        DataTable dataTable1 = new DataTable();
        if (folderName != "")
          folderNameList = new string[1]{ folderName };
        foreach (string folderName2 in folderNameList)
        {
          LoanFolderAclInfo loanFolderAclInfo = new LoanFolderAclInfo((int) feature, -1, folderName2, -1, -1);
          if (flag)
          {
            loanFolderAclInfo.MoveFromAccess = 1;
            loanFolderAclInfo.MoveToAccess = 1;
          }
          else if (currentEdition == EncompassEdition.Broker && feature == AclFeature.LoanMgmt_Move)
          {
            loanFolderAclInfo.MoveFromAccess = 1;
            loanFolderAclInfo.MoveToAccess = 1;
          }
          else if (str == "")
          {
            loanFolderAclInfo.MoveFromAccess = 0;
            loanFolderAclInfo.MoveToAccess = 0;
          }
          else
          {
            LoanFolderAclInfo[] folderConfiguration = LoanFoldersAclDbAccessor.GetUserLoanFolderConfiguration(feature, folderName2, userId);
            if (folderConfiguration != null && folderConfiguration.Length != 0)
            {
              if (folderConfiguration[0].MoveFromAccess != 2)
              {
                loanFolderAclInfo.MoveFromAccess = folderConfiguration[0].MoveFromAccess;
                loanFolderAclInfo.CustomMoveFrom = true;
              }
              if (folderConfiguration[0].MoveToAccess != 2)
              {
                loanFolderAclInfo.MoveToAccess = folderConfiguration[0].MoveToAccess;
                loanFolderAclInfo.CustomMoveTo = true;
              }
            }
            if (loanFolderAclInfo.MoveFromAccess == -1 || loanFolderAclInfo.MoveToAccess == -1)
            {
              string text = "SELECT A.moveFrom as PMoveFrom, A.moveTo as PMoveTo FROM AclF_LoanFolderConfig A WHERE A.personaID in (" + str + ") AND A.folderName = " + SQL.Encode((object) folderName2) + " AND featureID = " + (object) (int) feature;
              dbQueryBuilder.Reset();
              dbQueryBuilder.Append(text);
              DataTable dataTable2 = dbQueryBuilder.ExecuteTableQuery();
              if (dataTable2 != null && dataTable2.Rows.Count > 0)
              {
                if (loanFolderAclInfo.MoveFromAccess == -1)
                {
                  DataRow[] dataRowArray = dataTable2.Select("PMoveFrom > 0", "PMoveFrom");
                  loanFolderAclInfo.MoveFromAccess = dataRowArray == null || dataRowArray.Length == 0 ? 0 : int.Parse(string.Concat(dataRowArray[0]["PMoveFrom"]));
                }
                if (loanFolderAclInfo.MoveToAccess == -1)
                {
                  DataRow[] dataRowArray = dataTable2.Select("PMoveTo > 0", "PMoveTo");
                  loanFolderAclInfo.MoveToAccess = dataRowArray == null || dataRowArray.Length == 0 ? 0 : int.Parse(string.Concat(dataRowArray[0]["PMoveTo"]));
                }
              }
              else
              {
                if (loanFolderAclInfo.MoveFromAccess == -1)
                  loanFolderAclInfo.MoveFromAccess = defaultAccess ? 1 : 0;
                if (loanFolderAclInfo.MoveToAccess == -1)
                  loanFolderAclInfo.MoveToAccess = defaultAccess ? 1 : 0;
              }
            }
          }
          arrayList.Add((object) loanFolderAclInfo);
        }
      }
      return (LoanFolderAclInfo[]) arrayList.ToArray(typeof (LoanFolderAclInfo));
    }

    public static void DuplicateACLLoanFolders(int sourcePersonaID, int desPersonaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [AclF_LoanFolderConfig] where personaID = " + (object) sourcePersonaID);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [AclF_LoanFolderConfig] (";
        for (int index = 0; index < columns.Count; ++index)
        {
          if (index == 0)
          {
            str3 += columns[index].ColumnName;
            str2 = !(columns[index].ColumnName != "personaID") ? str2 + SQL.Encode((object) desPersonaID) : str2 + SQL.Encode(row[columns[index].ColumnName]);
          }
          else
          {
            str3 = str3 + ", " + columns[index].ColumnName;
            str2 = !(columns[index].ColumnName != "personaID") ? str2 + ", " + SQL.Encode((object) desPersonaID) : str2 + ", " + SQL.Encode(row[columns[index].ColumnName]);
          }
        }
        string text = str3 + " ) Values (" + str2 + ")";
        dbQueryBuilder.AppendLine(text);
        str1 = "";
        str2 = "";
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static LoanFolderAclInfo[] GetUserApplicationLoanFolders(
      AclFeature feature,
      UserInfo userInfo)
    {
      return LoanFoldersAclDbAccessor.GetUserApplicationLoanFolders(feature, userInfo.Userid, userInfo.UserPersonas);
    }

    public static LoanFolderAclInfo[] GetUserApplicationLoanFolders(
      AclFeature feature,
      string userID,
      Persona[] personas)
    {
      return LoanFoldersAclDbAccessor.GetUserApplicationLoanFolders(feature, userID, AclUtils.GetPersonaIDs(personas));
    }

    public static LoanFolderAclInfo[] GetUserApplicationLoanFolders(
      AclFeature feature,
      string userID,
      int[] personaIDList)
    {
      return LoanFoldersAclDbAccessor.getUserApplicationLoanFolders(feature, "", userID, personaIDList);
    }

    public static LoanFolderAclInfo GetUserApplicationLoanFolder(
      AclFeature feature,
      string folderName,
      string userID,
      int[] personaIDList)
    {
      LoanFolderAclInfo[] applicationLoanFolders = LoanFoldersAclDbAccessor.getUserApplicationLoanFolders(feature, folderName, userID, personaIDList);
      return applicationLoanFolders.Length == 0 ? (LoanFolderAclInfo) null : applicationLoanFolders[0];
    }

    public static LoanFolderAclInfo[] GetUserLoanFolders(
      AclFeature feature,
      string folderName,
      string userId)
    {
      return LoanFoldersAclDbAccessor.GetUserLoanFolderConfiguration(feature, folderName, userId);
    }

    public static LoanFolderAclInfo[] GetPersonaLoanFolders(
      AclFeature feature,
      string folderName,
      string personaId)
    {
      string[] folderFilterList = feature != AclFeature.LoanMgmt_Move ? AclUtils.ImportFolderNames : LoanFolder.GetAllLoanFolderNames(false);
      bool aclFeaturesDefault = FeaturesAclDbAccessor.GetAclFeaturesDefault(int.Parse(personaId));
      return LoanFoldersAclDbAccessor.GetPersonaLoanFolderConfiguration(feature, folderName, personaId, folderFilterList, aclFeaturesDefault);
    }

    public static LoanFolderAclInfo GetLoanFolderAclInfo(
      AclFeature feature,
      string folderName,
      Persona[] personaList)
    {
      int[] personaIds = AclUtils.GetPersonaIDs(personaList);
      bool aclFeaturesDefault = FeaturesAclDbAccessor.GetAclFeaturesDefault(personaIds);
      return LoanFoldersAclDbAccessor.GetPersonaLoanFolderConfiguration(feature, folderName, personaIds, aclFeaturesDefault);
    }

    private static LoanFolderAclInfo[] getUserApplicationLoanFolders(
      AclFeature feature,
      string folderName,
      string userID,
      int[] personaIDList)
    {
      string[] folderNameList = feature != AclFeature.LoanMgmt_Move ? AclUtils.ImportFolderNames : LoanFolder.GetAllLoanFolderNames(false);
      bool defaultAccess = false;
      if (userID == "admin")
        defaultAccess = true;
      else if (personaIDList.Length != 0)
        defaultAccess = FeaturesAclDbAccessor.GetAclFeaturesDefault(personaIDList);
      return LoanFoldersAclDbAccessor.GetUserLoanFolderAccessibility(feature, folderName, userID, personaIDList, folderNameList, defaultAccess);
    }

    private static LoanFolderAclInfo ConvertDataRowToObject(DataRow dr)
    {
      int featureID = int.Parse(string.Concat(dr["featureID"]));
      int personaID = -1;
      if (dr.Table.Columns.Contains("personaID"))
        personaID = int.Parse(string.Concat(dr["personaID"]));
      string folderName = string.Concat(dr["folderName"]);
      int moveFromAccess = int.Parse(string.Concat(dr["moveFrom"]));
      int moveToAccess = int.Parse(string.Concat(dr["moveTo"]));
      return new LoanFolderAclInfo(featureID, personaID, folderName, moveFromAccess, moveToAccess);
    }
  }
}

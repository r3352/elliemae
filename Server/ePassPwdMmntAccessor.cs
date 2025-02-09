// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ePassPwdMmntAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.DbTableEncryption;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class ePassPwdMmntAccessor
  {
    private const string className = "ePassPwdMmntAccessor�";
    public static List<string> EncryptColNames = new List<string>()
    {
      "UIDValue",
      "PasswordValue"
    };

    public static bool UseEncryption()
    {
      return !string.IsNullOrEmpty(Company.GetCompanySetting("MIGRATION", "EPassCredentialsEncryption"));
    }

    private static string EncryptedColName(string name) => name + "_enc";

    private static string SqlCsv<T>(params T[] values)
    {
      return string.Join(", ", ((IEnumerable<T>) values).Select<T, string>((System.Func<T, string>) (v => SQL.Encode((object) v))));
    }

    public static ePassCredentialSetting UpdateePassCredentialSetting(
      ePassCredentialSetting newSetting)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      int credentialID = newSetting.CredentialID;
      DbTableInfo table = DbAccessManager.GetTable("ePassCredentialSettings");
      DbValueList values = new DbValueList()
      {
        {
          "Category",
          (object) newSetting.Category
        },
        {
          "Title",
          (object) newSetting.Title
        },
        {
          "UIDName",
          (object) newSetting.UIDName
        },
        {
          "UIDValue",
          (object) newSetting.UIDValue
        },
        {
          "PasswordName",
          (object) newSetting.PasswordName
        },
        {
          "PasswordValue",
          (object) newSetting.PasswordValue
        },
        {
          "Auth1Name",
          (object) newSetting.Auth1Name
        },
        {
          "Auth1Value",
          (object) newSetting.Auth1Value
        },
        {
          "Auth2Name",
          (object) newSetting.Auth2Name
        },
        {
          "Auth2Value",
          (object) newSetting.Auth2Value
        },
        {
          "Description",
          (object) newSetting.Description
        },
        {
          "ValidDuration",
          (object) newSetting.ValidDuration
        },
        {
          "UIDFieldName",
          (object) newSetting.UIDFieldName
        },
        {
          "PasswordFieldName",
          (object) newSetting.PasswordFieldName
        },
        {
          "Auth1FieldName",
          (object) newSetting.Auth1FieldName
        },
        {
          "Auth2FieldName",
          (object) newSetting.Auth2FieldName
        },
        {
          "PartnerSection",
          (object) newSetting.PartnerSection
        },
        {
          "SaveLoginFieldName",
          (object) newSetting.SaveLoginFieldName
        },
        {
          "SaveLoginValue",
          (object) newSetting.SaveLoginValue
        },
        {
          "EncryptionType",
          (object) newSetting.EncryptionType
        },
        {
          "LastModifiedDTTM",
          (object) DateTime.Now
        },
        {
          "TPONumberName",
          (object) newSetting.TPOName
        },
        {
          "TPONumberFieldName",
          (object) newSetting.TPOFieldName
        },
        {
          "TPONumberFieldValue",
          (object) newSetting.TPOFieldValue
        }
      };
      if (ePassPwdMmntAccessor.UseEncryption())
      {
        ClientContextEncryptor contextEncryptor = new ClientContextEncryptor();
        DbValueList dbValueList = new DbValueList();
        foreach (DbValue dbValue in values)
        {
          if (ePassPwdMmntAccessor.EncryptColNames.Contains(dbValue.ColumnName))
            dbValueList.Add(dbValue.ColumnName, (object) contextEncryptor.Encrypt(dbValue.Value.ToString()));
          else
            dbValueList.Add(dbValue.ColumnName, dbValue.Value);
        }
        values = dbValueList;
      }
      if (credentialID > 0)
      {
        dbQueryBuilder.Update(table, values, new DbValue("ePassCredentialSettingID", (object) credentialID));
        dbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        dbQueryBuilder.Declare("@ePassCredentialSettingID", "int");
        dbQueryBuilder.InsertInto(table, values, true, false);
        dbQueryBuilder.SelectIdentity("@ePassCredentialSettingID");
        dbQueryBuilder.Select("@ePassCredentialSettingID");
        credentialID = (int) dbQueryBuilder.ExecuteScalar();
      }
      return ePassPwdMmntAccessor.GetePassCredentialSetting(credentialID);
    }

    public static List<ePassCredentialSetting> GetAllePassCredentialSettings()
    {
      return ePassPwdMmntAccessor.getePassCredentialSettings();
    }

    public static ePassCredentialSetting GetePassCredentialSetting(int credentialID)
    {
      return ePassPwdMmntAccessor.getePassCredentialSettings(credentialID).First<ePassCredentialSetting>();
    }

    private static List<ePassCredentialSetting> getePassCredentialSettings(
      params int[] credentialIDs)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      bool useEncryption = ePassPwdMmntAccessor.UseEncryption();
      EPassCredentialSettingsTableEncryptor settingsTableEncryptor = new EPassCredentialSettingsTableEncryptor(ClientContext.GetCurrent().InstanceName);
      string str1 = string.Join(", ", settingsTableEncryptor.PkColumnNames.Union<string>((IEnumerable<string>) settingsTableEncryptor.OtherColNames).Select<string, string>((System.Func<string, string>) (n => "[" + n + "]")));
      string str2 = string.Join(", ", ePassPwdMmntAccessor.EncryptColNames.Select<string, string>((System.Func<string, string>) (n =>
      {
        string str3 = n;
        string str4;
        if (!useEncryption)
          str4 = "";
        else
          str4 = " AS [" + ePassPwdMmntAccessor.EncryptedColName(n) + "], '' AS [" + n + "]";
        return "[" + str3 + "]" + str4;
      })));
      dbQueryBuilder.AppendLine("Select " + str1 + ", " + str2 + ", [ExpirationDTTM] from ePassCredentialSettings");
      if (credentialIDs != null && credentialIDs.Length != 0)
        dbQueryBuilder.AppendLine(" where ePassCredentialSettingID in (" + string.Join<int>(",", (IEnumerable<int>) credentialIDs) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ClientContextEncryptor contextEncryptor = useEncryption ? new ClientContextEncryptor() : (ClientContextEncryptor) null;
      List<ePassCredentialSetting> credentialSettingList = new List<ePassCredentialSetting>();
      foreach (DataRow rawRow in (InternalDataCollectionBase) dataRowCollection)
      {
        if (useEncryption)
        {
          foreach (string encryptColName in ePassPwdMmntAccessor.EncryptColNames)
            rawRow[encryptColName] = (object) contextEncryptor.Decrypt(rawRow[ePassPwdMmntAccessor.EncryptedColName(encryptColName)] as byte[]);
        }
        ePassCredentialSetting credentialSetting = new ePassCredentialSetting(rawRow);
        if (!credentialSettingList.Contains(credentialSetting))
          credentialSettingList.Add(credentialSetting);
      }
      return credentialSettingList;
    }

    public static void DeleteePassCredentialSetting(int credentialID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete ePassCredentialSettings where ePassCredentialSettingID = " + (object) credentialID);
      dbQueryBuilder.ExecuteNonQueryWithRowCount();
    }

    public static List<string> GetUserIDList(int credentialID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select UserID from ePassCrendentialSettingUser where ePassCredentialSettingID = " + (object) credentialID);
      List<string> userIdList = new List<string>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        if (!userIdList.Contains(string.Concat(dataRow["UserID"])))
          userIdList.Add(string.Concat(dataRow["UserID"]));
      }
      return userIdList;
    }

    public static List<ePassCredentialSetting> GetUserSettings(string userID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select distinct ePassCredentialSettingID from ePassCrendentialSettingUser where UserID = " + userID);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
        return new List<ePassCredentialSetting>();
      List<int> intList = new List<int>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        int result;
        if (int.TryParse(string.Concat(dataRow["ePassCredentialSettingID"]), out result) && !intList.Contains(result))
          intList.Add(result);
      }
      return ePassPwdMmntAccessor.getePassCredentialSettings(intList.ToArray());
    }

    public static List<string> GetDuplicateUsers(
      int currentCredentialID,
      string providerName,
      string[] userList)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select distinct B.UserID from ePassCredentialSettings A inner join ePassCrendentialSettingUser B on A.ePassCredentialSettingID = B.ePassCredentialSettingID where A.Title = " + SQL.EncodeString(providerName) + " and A.ePassCredentialSettingID !=" + (object) currentCredentialID + " and B.UserID in (" + ePassPwdMmntAccessor.SqlCsv<string>(userList) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<string> duplicateUsers = new List<string>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        duplicateUsers.Add(string.Concat(dataRow[0]));
      return duplicateUsers;
    }

    public static void UpdateUserList(int credentialID, string providerName, string[] userList)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete ePassCrendentialSettingUser where ePassCredentialSettingID = " + (object) credentialID);
      if (userList != null && userList.Length != 0)
      {
        dbQueryBuilder.AppendLine("Delete ePassCrendentialSettingUser where ePassCredentialSettingID in (Select ePassCredentialSettingID from ePassCredentialSettings where Title = " + SQL.EncodeString(providerName) + ") and UserID in (" + ePassPwdMmntAccessor.SqlCsv<string>(userList) + ")");
        foreach (string user in userList)
          dbQueryBuilder.AppendLine(string.Format("Insert into ePassCrendentialSettingUser (ePassCredentialSettingID, UserID) Values ({0}, {1})", (object) credentialID, (object) SQL.EncodeString(user)));
      }
      dbQueryBuilder.ExecuteNonQuery();
      if (userList == null || userList.Length == 0)
        return;
      ePassPwdMmntAccessor.updateUserCredential(credentialID, userList);
    }

    public static void updateUserCredential(int credentialID)
    {
      List<string> userIdList = ePassPwdMmntAccessor.GetUserIDList(credentialID);
      ePassPwdMmntAccessor.updateUserCredential(credentialID, userIdList.ToArray());
    }

    public static void updateUserCredential(int credentialID, string[] userList)
    {
      ePassCredentialSetting credentialSetting = ePassPwdMmntAccessor.GetePassCredentialSetting(credentialID);
      Dictionary<string, string> values = new Dictionary<string, string>();
      insertValue(credentialSetting.UIDFieldName, credentialSetting.UIDValue);
      insertValue(credentialSetting.PasswordFieldName, credentialSetting.PasswordValue);
      insertValue(credentialSetting.Auth1FieldName, credentialSetting.Auth1Value);
      insertValue(credentialSetting.Auth2FieldName, credentialSetting.Auth2Value);
      insertValue(credentialSetting.TPOFieldName, credentialSetting.TPOFieldValue);
      insertValue(credentialSetting.SaveLoginFieldName, credentialSetting.SaveLoginValue);
      if (ePassCredentialSetting.RequirePasswordEncryption(credentialSetting.EncryptionType))
        values.Add("Encrypted", "Yes");
      new UserSettingsAccessor().ReplaceCategory((IEnumerable<string>) userList, credentialSetting.PartnerSection, (IDictionary<string, string>) values);

      void insertValue(string attr, string value)
      {
        if (string.IsNullOrEmpty(attr))
          return;
        values.Add(attr, value);
      }
    }

    public static List<ePassCredential> GetePassCredential(string category)
    {
      bool flag = !string.IsNullOrEmpty(Company.GetCompanySetting("MIGRATION", "UserSettingsEncryption"));
      string columnName = ePassPwdMmntAccessor.EncryptedColName("value");
      string str = flag ? "AS " + columnName + ", '' AS [value]" : "";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT DISTINCT CS.ePassCredentialSettingID, CS.Category, CS.Title, cs.UIDValue as UIDValue_enc, '' as UIDValue, CS.PartnerSection, CT.userid, CT.attribute, CT.value " + str + ", U.FirstLastName, CS.EncryptionType, CS.PasswordFieldName\nFROM ePassCredentialSettings CS\nINNER JOIN ePassCrendentialSettingUser CSU ON CS.ePassCredentialSettingID=CSU.ePassCredentialSettingID\nINNER JOIN users U ON U.userid=CSU.UserID\nINNER JOIN user_settings CT ON CT.userid=CSU.UserID AND CT.category=CS.PartnerSection AND CT.attribute IN (CS.UIDFieldName,CS.PasswordFieldName)\nwhere cs.Category = @category;");
      DbCommandParameter[] parameters = new DbCommandParameter[1]
      {
        new DbCommandParameter(nameof (category), (object) category, DbType.String)
      };
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(parameters);
      if (dataRowCollection != null && dataRowCollection.Count == 0)
        return new List<ePassCredential>();
      ClientContextEncryptor contextEncryptor = flag ? new ClientContextEncryptor() : (ClientContextEncryptor) null;
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        if (flag)
        {
          dataRow["value"] = (object) contextEncryptor.Decrypt(dataRow[columnName] as byte[]);
          dataRow["UIDValue"] = (object) contextEncryptor.Decrypt(dataRow["UIDValue_enc"] as byte[]);
        }
        string key = dataRow[nameof (category)].ToString() + dataRow["title"] + dataRow["UIDValue"];
        if (!dictionary.ContainsKey(key))
          dictionary.Add(key, int.Parse(string.Concat(dataRow["ePassCredentialSettingID"])));
      }
      List<int> list = dictionary.Values.Distinct<int>().ToList<int>();
      List<ePassCredential> ePassCredentialList = new List<ePassCredential>();
      foreach (DataRow rawRow in (InternalDataCollectionBase) dataRowCollection)
      {
        int id = int.Parse(string.Concat(rawRow["ePassCredentialSettingID"]));
        if (list.Exists((Predicate<int>) (x => x == id)))
          ePassCredentialList.Add(ePassPwdMmntAccessor.GetePassCredential(rawRow));
      }
      return ePassCredentialList;
    }

    private static ePassCredential GetePassCredential(DataRow rawRow)
    {
      return new ePassCredential()
      {
        ePassCredentialSettingID = int.Parse(string.Concat(rawRow["ePassCredentialSettingID"])),
        Category = string.Concat(rawRow["Category"]),
        Title = string.Concat(rawRow["Title"]),
        UIDValue = string.Concat(rawRow["UIDValue"]),
        PartnerSection = string.Concat(rawRow["PartnerSection"]),
        UserId = string.Concat(rawRow["userid"]),
        Name = string.Concat(rawRow["FirstLastName"]),
        Attribute = string.Concat(rawRow["attribute"]),
        Value = string.Concat(rawRow["value"]),
        IsEncrypted = string.Concat(rawRow["EncryptionType"]).Equals("None", StringComparison.OrdinalIgnoreCase),
        PasswordFieldName = string.Concat(rawRow["PasswordFieldName"])
      };
    }
  }
}

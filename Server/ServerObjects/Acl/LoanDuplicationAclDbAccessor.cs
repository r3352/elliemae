// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.LoanDuplicationAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class LoanDuplicationAclDbAccessor
  {
    private const string tableName = "[AclF_LoanDuplicationConfig]�";

    private LoanDuplicationAclDbAccessor()
    {
    }

    public static LoanDuplicationTemplateAclInfo[] GetAccessibleLoanDuplicationTemplates(
      int[] personaIDs)
    {
      if (personaIDs == null || personaIDs.Length == 0)
        return (LoanDuplicationTemplateAclInfo[]) null;
      List<LoanDuplicationTemplateAclInfo> duplicationTemplateAclInfoList = new List<LoanDuplicationTemplateAclInfo>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "SELECT * FROM [AclF_LoanDuplicationConfig] " + ("WHERE personaID in (" + SQL.EncodeArray((Array) personaIDs) + ")");
      dbQueryBuilder.AppendLine(text);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        int personaID = -1;
        string empty = string.Empty;
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          if (row.Table.Columns.Contains("personaID"))
            personaID = int.Parse(string.Concat(row["personaID"]));
          string templateName = string.Concat(row["templateName"]);
          duplicationTemplateAclInfoList.Add(new LoanDuplicationTemplateAclInfo(personaID, templateName, true));
        }
      }
      return duplicationTemplateAclInfoList == null || duplicationTemplateAclInfoList.Count <= 0 ? (LoanDuplicationTemplateAclInfo[]) null : duplicationTemplateAclInfoList.ToArray();
    }

    public static void UpdateLoanDuplicationSettings(
      LoanDuplicationTemplateAclInfo[] loanDuplicationAclInfoList,
      int personaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text1 = "DELETE [AclF_LoanDuplicationConfig] WHERE personaID = " + (object) personaID;
      dbQueryBuilder.AppendLine(text1);
      str1 = "";
      Company.GetServerLicense();
      foreach (LoanDuplicationTemplateAclInfo duplicationAclInfo in loanDuplicationAclInfoList)
      {
        str2 = "";
        string text2 = "INSERT INTO [AclF_LoanDuplicationConfig] (personaID, templateName) VALUES (" + (object) personaID + ", " + SQL.Encode((object) duplicationAclInfo.TemplateName) + ") ";
        dbQueryBuilder.AppendLine(text2);
        str2 = "";
      }
      dbQueryBuilder.ExecuteNonQuery();
      if (loanDuplicationAclInfoList == null || loanDuplicationAclInfoList.Length == 0)
        FeaturesAclDbAccessor.SetPermission(AclFeature.LoanMgmt_Duplicate, personaID, AclTriState.False);
      else
        FeaturesAclDbAccessor.SetPermission(AclFeature.LoanMgmt_Duplicate, personaID, AclTriState.True);
    }

    public static void DuplicateACLLoanDuplication(FileSystemEntry[] entries, int desPersonaID)
    {
      if (entries == null || entries.Length == 0)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      for (int index = 0; index < entries.Length; ++index)
        dbQueryBuilder.AppendLine("Insert into [AclF_LoanDuplicationConfig] (personaID, templateName) Values (" + (object) desPersonaID + ", " + SQL.Encode(entries[index].Name.Length > 256 ? (object) entries[index].Name.Substring(0, 256) : (object) entries[index].Name) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DuplicateACLLoanDuplication(int sourcePersonaID, int desPersonaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [AclF_LoanDuplicationConfig] where personaID = " + (object) sourcePersonaID);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [AclF_LoanDuplicationConfig] (";
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
  }
}

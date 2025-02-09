// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.UserSettingsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Serialization;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class UserSettingsAccessor
  {
    private static readonly string sw = Tracing.SwCommon;
    private readonly ClientContextEncryptor _encryptor = new ClientContextEncryptor();

    public UserSettingsAccessor()
    {
      string companySetting = Company.GetCompanySetting("MIGRATION", "UserSettingsEncryption");
      this.UseEncryption = !string.IsNullOrEmpty(companySetting);
      if (!this.UseEncryption)
        return;
      string[] strArray = companySetting.Split(new char[1]
      {
        ','
      }, StringSplitOptions.None);
      this.BulkInstanceName = strArray == null || strArray.Length <= 2 ? (string) null : strArray[1];
      if (this.BulkInstanceName != null && !this.BulkInstanceName.Equals(this._encryptor.InstanceName, StringComparison.OrdinalIgnoreCase))
        throw new ArgumentException("Mismatched instance name during instantiation of UserSettingsAccessor.\n(Was/expected = \"" + this._encryptor.InstanceName + "\"/\"" + this.BulkInstanceName + "\")");
    }

    protected bool UseEncryption { get; set; }

    protected string BulkInstanceName { get; set; }

    private string EncryptDbValue(string cleartext)
    {
      if (string.IsNullOrEmpty(cleartext))
        return "NULL";
      return this.UseEncryption ? "0x" + HexEncoding.Instance.GetString(this._encryptor.Encrypt(cleartext)) : SQL.Encode((object) cleartext);
    }

    private string DecryptDbValue(DataRow row)
    {
      if (!(row["enc"] is byte[] bytes))
        return row["enc"].ToString();
      try
      {
        return this._encryptor.Decrypt(bytes);
      }
      catch (Exception ex)
      {
        string instanceName = this._encryptor.InstanceName;
        throw new Exception("Error while decrypting user_settings for the " + (string.IsNullOrEmpty(instanceName) ? "default" : "'" + instanceName + "'") + " instance", ex);
      }
    }

    private IList<string> GetWhereTerms(string value, UserSettingsAccessor.PrimaryKeyValues pk)
    {
      List<string> whereTerms = new List<string>();
      if (!string.IsNullOrEmpty(value))
        whereTerms.Add("[value] = " + this.EncryptDbValue(value));
      if (!string.IsNullOrEmpty(pk.Attribute))
        whereTerms.Add("[attribute] = " + SQL.Encode((object) pk.Attribute));
      if (!string.IsNullOrEmpty(pk.Category))
        whereTerms.Add("[category] = " + SQL.Encode((object) pk.Category));
      if (!string.IsNullOrEmpty(pk.UserID))
        whereTerms.Add("[userID] = " + SQL.Encode((object) pk.UserID));
      return (IList<string>) whereTerms;
    }

    private string MakeWhereClause(IEnumerable<string> terms)
    {
      return " WHERE " + string.Join(" AND ", terms) + " ";
    }

    private string SqlCsv<T>(params T[] values)
    {
      return string.Join(", ", ((IEnumerable<T>) values).Select<T, string>((System.Func<T, string>) (v => SQL.Encode((object) v))));
    }

    public DataRowCollection GetRows(string value, UserSettingsAccessor.PrimaryKeyValues pk)
    {
      IList<string> whereTerms = this.GetWhereTerms(value, pk);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT [userid], [category], [attribute], [value] AS [enc], CAST(NULL AS NVARCHAR) AS [value] FROM [user_settings] " + this.MakeWhereClause((IEnumerable<string>) whereTerms));
      DataRowCollection rows = dbQueryBuilder.Execute();
      foreach (DataRow row in (InternalDataCollectionBase) rows)
        row[nameof (value)] = (object) this.DecryptDbValue(row);
      return rows;
    }

    public void UpsertValue(string value, UserSettingsAccessor.PrimaryKeyValues pk)
    {
      string str = this.MakeWhereClause((IEnumerable<string>) this.GetWhereTerms((string) null, pk));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("IF EXISTS (SELECT 1 FROM [user_settings] " + str + ")");
      dbQueryBuilder.AppendLine("    UPDATE [user_settings] SET [value] = @dbValue " + str);
      dbQueryBuilder.AppendLine("ELSE");
      dbQueryBuilder.AppendLine("    INSERT INTO [user_settings] ([userid], [category], [attribute], [value]) VALUES (" + this.SqlCsv<string>(pk.UserID, pk.Category, pk.Attribute) + ", @dbValue)");
      DbCommandParameter commandParameter = this.UseEncryption ? new DbCommandParameter("dbValue", (object) this._encryptor.Encrypt(value), DbType.Binary) : new DbCommandParameter("dbValue", (object) (value ?? ""), DbType.String);
      dbQueryBuilder.ExecuteNonQueryWithRowCount(new DbCommandParameter[1]
      {
        commandParameter
      });
    }

    public void ReplaceCategory(
      IEnumerable<string> userIDs,
      string category,
      IDictionary<string, string> attributeValues)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE FROM [user_settings] WHERE [userid] IN (" + this.SqlCsv<IEnumerable<string>>(userIDs) + ") and [category] = " + SQL.Encode((object) category));
      foreach (string userId in userIDs)
      {
        foreach (KeyValuePair<string, string> attributeValue in (IEnumerable<KeyValuePair<string, string>>) attributeValues)
          dbQueryBuilder.AppendLine("INSERT INTO [user_settings] (userid, category, attribute, value) VALUES (" + this.SqlCsv<string>(userId, category, attributeValue.Key) + ", " + this.EncryptDbValue(attributeValue.Value) + ")");
      }
      dbQueryBuilder.ExecuteNonQueryWithRowCount();
    }

    public void UpdateValues(
      string existingValue,
      string newValue,
      UserSettingsAccessor.PrimaryKeyValues pk)
    {
      IList<string> whereTerms = this.GetWhereTerms(existingValue, pk);
      string str = this.EncryptDbValue(newValue);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("UPDATE [user_settings] SET [value] = " + str + " " + this.MakeWhereClause((IEnumerable<string>) whereTerms));
      dbQueryBuilder.ExecuteNonQueryWithRowCount();
    }

    public void ReplaceAllInValues(
      string find,
      string replacement,
      UserSettingsAccessor.PrimaryKeyValues pk)
    {
      if (this.UseEncryption)
        this.ReplaceInValues(pk, new System.Func<string, string>(replaceAnywhere));
      else
        this.UpdateInValues(pk, find, replacement, false);

      string replaceAnywhere(string value)
      {
        return !value.Contains(find) ? (string) null : value.Replace(find, replacement);
      }
    }

    public void ReplacePrefixInValues(
      string prefix,
      string replacement,
      UserSettingsAccessor.PrimaryKeyValues pk)
    {
      if (this.UseEncryption)
        this.ReplaceInValues(pk, new System.Func<string, string>(replacePrefix));
      else
        this.UpdateInValues(pk, prefix, replacement, true);

      string replacePrefix(string value)
      {
        return !value.StartsWith(prefix) ? (string) null : replacement + value.Substring(prefix.Length);
      }
    }

    private void UpdateInValues(
      UserSettingsAccessor.PrimaryKeyValues pk,
      string target,
      string replacement,
      bool startsWith)
    {
      IList<string> whereTerms = this.GetWhereTerms((string) null, pk);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str1 = startsWith ? "@target + '%'" : "'%' + @target + '%'";
      string str2 = "REPLACE([value], @target, @replacement)";
      dbQueryBuilder.AppendLine("UPDATE [user_settings] SET [value] = " + str2 + " " + this.MakeWhereClause((IEnumerable<string>) whereTerms) + " AND [value] LIKE " + str1);
      DbCommandParameter[] parameters = new DbCommandParameter[2]
      {
        new DbCommandParameter(nameof (target), (object) target, DbType.String),
        new DbCommandParameter(nameof (replacement), (object) replacement, DbType.String)
      };
      dbQueryBuilder.ExecuteNonQueryWithRowCount(parameters);
    }

    private void ReplaceInValues(
      UserSettingsAccessor.PrimaryKeyValues pk,
      System.Func<string, string> replace)
    {
      IList<string> whereTerms = this.GetWhereTerms((string) null, pk);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str1 = this.MakeWhereClause((IEnumerable<string>) whereTerms);
      dbQueryBuilder.AppendLine("SELECT [userid], [category], [attribute], [value] AS [enc] FROM [user_settings] " + str1);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      dbQueryBuilder.Reset();
      HexEncoding hexEncoding = new HexEncoding();
      string[] source = new string[3]
      {
        "attribute",
        "category",
        "userid"
      };
      System.Func<object, string> compareValue = (System.Func<object, string>) (value => !(value is string str2) ? "IS NULL" : "= " + SQL.Encode((object) str2));
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        DataRow drSetting = dataRow;
        string str3 = this.DecryptDbValue(drSetting);
        string cleartext;
        if ((cleartext = replace(str3)) != null)
        {
          IList<string> list = (IList<string>) ((IEnumerable<string>) source).Select<string, string>((System.Func<string, string>) (name => "[" + name + "] " + compareValue(drSetting[name]))).ToList<string>();
          dbQueryBuilder.AppendLine("UPDATE [user_settings] SET [value] = " + this.EncryptDbValue(cleartext) + " " + str1 + " AND [value] = 0x" + hexEncoding.GetString(drSetting["enc"] as byte[]));
        }
      }
      if (dbQueryBuilder.Length == 0)
        return;
      dbQueryBuilder.ExecuteNonQueryWithRowCount();
    }

    public void Delete(string existingValue, UserSettingsAccessor.PrimaryKeyValues pk)
    {
      IList<string> whereTerms = this.GetWhereTerms(existingValue, pk);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE FROM [user_settings] " + this.MakeWhereClause((IEnumerable<string>) whereTerms));
      dbQueryBuilder.ExecuteNonQueryWithRowCount();
    }

    public class PrimaryKeyValues
    {
      public string UserID { get; set; }

      public string Category { get; set; }

      public string Attribute { get; set; }

      public PrimaryKeyValues(string attribute, string category, string userID)
      {
        this.UserID = this.Truncate(userID, 16, nameof (userID));
        this.Category = this.Truncate(category, 32, nameof (category));
        this.Attribute = this.Truncate(attribute, 32, nameof (attribute));
      }

      private string Truncate(string value, int maxLength, string name)
      {
        if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
        {
          Tracing.Log(UserSettingsAccessor.sw, TraceLevel.Warning, nameof (UserSettingsAccessor), "User_settings primary key [" + name + "] truncated; original value = '" + value + "'");
          value = value.Substring(0, maxLength);
        }
        return value;
      }
    }
  }
}

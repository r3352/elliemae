// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.DbTableEncryption.EncryptDbTablePassword
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using Encompass.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.DbTableEncryption
{
  public class EncryptDbTablePassword
  {
    private Mode action;
    private EncryptTable encryptTable;
    private string modeDec = string.Empty;
    private const string BackupSuffix = "_backup�";

    public EncryptDbTablePassword(Mode mode, EncryptTable tableName)
    {
      this.action = mode;
      this.encryptTable = tableName;
      this.modeDec = this.action == Mode.Decrypt ? "Decrypting" : (this.action == Mode.Encrypt ? "Encrypting" : "Migrating");
    }

    public void MigrateExternalOrgDetailTable(string tgtInstanceId, string srcInstanceId)
    {
      if (this.action == Mode.Decrypt)
      {
        Console.WriteLine(string.Format("\n\n=== {0} {1} is not supported===", (object) this.modeDec, (object) this.encryptTable));
      }
      else
      {
        Console.WriteLine(string.Format("\n\n=== {0} {1} ===", (object) this.modeDec, (object) this.encryptTable));
        string encryptTable = "ExternalOrgDetail";
        string encryptColumn = "LPASponsorLPAPassword";
        string pkColumn = "externalOrgID";
        try
        {
          tgtInstanceId = tgtInstanceId.ToUpper();
          srcInstanceId = srcInstanceId.ToUpper();
          EnGlobalSettings enGlobalSettings = new EnGlobalSettings(tgtInstanceId);
          InstanceEncryptor instanceEncryptor = new InstanceEncryptor(tgtInstanceId);
          using (SqlConnection dbConn = new SqlConnection(enGlobalSettings.DatabaseConnectionString))
          {
            if (dbConn.State == ConnectionState.Closed)
              dbConn.Open();
            Dictionary<string, string> aesEncryption = this.ConvertToAesEncryption(dbConn, tgtInstanceId, srcInstanceId, encryptTable, encryptColumn, pkColumn);
            this.PromoteMigration(dbConn, aesEncryption, encryptTable, encryptColumn, pkColumn);
            Console.WriteLine(string.Format("\n\n=== {0} {1} completed ===", (object) this.modeDec, (object) this.encryptTable));
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(string.Format("\n\n===Error occured while {0} table '{1}' with exception {2}===", (object) this.modeDec, (object) this.encryptTable, (object) ex.Message));
        }
      }
    }

    public void MigrateTitleFeeCredentialsTable(string tgtInstanceId, string srcInstanceId)
    {
      if (this.action == Mode.Decrypt)
      {
        Console.WriteLine(string.Format("\n\n=== {0} {1} is not supported===", (object) this.modeDec, (object) this.encryptTable));
      }
      else
      {
        Console.WriteLine(string.Format("\n\n=== {0} {1} ===", (object) this.modeDec, (object) this.encryptTable));
        string encryptTable = "TitleFeeCredentials";
        string encryptColumn = "credentials";
        string pkColumn = "orderUID";
        try
        {
          tgtInstanceId = tgtInstanceId.ToUpper();
          srcInstanceId = srcInstanceId.ToUpper();
          EnGlobalSettings enGlobalSettings = new EnGlobalSettings(tgtInstanceId);
          InstanceEncryptor instanceEncryptor = new InstanceEncryptor(tgtInstanceId);
          using (SqlConnection dbConn = new SqlConnection(enGlobalSettings.DatabaseConnectionString))
          {
            if (dbConn.State == ConnectionState.Closed)
              dbConn.Open();
            Dictionary<string, string> aesEncryption = this.ConvertToAesEncryption(dbConn, tgtInstanceId, srcInstanceId, encryptTable, encryptColumn, pkColumn);
            this.PromoteMigration(dbConn, aesEncryption, encryptTable, encryptColumn, pkColumn);
            Console.WriteLine(string.Format("\n\n=== {0} {1} completed ===", (object) this.modeDec, (object) this.encryptTable));
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(string.Format("\n\n===Error occured while {0} table '{1}' with exception {2}===", (object) this.modeDec, (object) this.encryptTable, (object) ex.Message));
        }
      }
    }

    private string MakeWhereClause(string PkColumnName, IEnumerable<string> terms)
    {
      return "END WHERE " + PkColumnName + " IN (  '" + string.Join("','", terms) + "'  ) ";
    }

    private string BackupTableQuery(string tableName)
    {
      return string.Format("IF EXISTS (select * from sysobjects where id = object_id(N'[{0}{1}]'))\nBEGIN\n    DROP TABLE {0}{1}\nEND\nSELECT * INTO {0}{1} FROM {0} ", (object) tableName, (object) "_backup");
    }

    private void PromoteMigration(
      SqlConnection dbConn,
      Dictionary<string, string> keyValues,
      string encryptTable,
      string encryptColumn,
      string pkColumn)
    {
      if (!keyValues.Any<KeyValuePair<string, string>>())
        return;
      int num = keyValues.Count<KeyValuePair<string, string>>();
      Console.WriteLine(string.Format("\n\n===Started creating table backup and updating table '{0}' with new values for '{1}' records===", (object) encryptTable, (object) num));
      string str1 = this.BackupTableQuery(encryptTable);
      string str2 = "UPDATE " + encryptTable + " SET " + encryptColumn + " = CASE";
      string empty = string.Empty;
      foreach (KeyValuePair<string, string> keyValue in keyValues)
      {
        string key = keyValue.Key;
        string str3 = keyValue.Value;
        string str4 = "WHEN " + pkColumn + " = '" + key + "' THEN '" + str3 + "' \n";
        empty += str4;
      }
      string str5 = this.MakeWhereClause(pkColumn, (IEnumerable<string>) keyValues.Keys);
      new SqlCommand(" " + str1 + " \n " + str2 + " \n " + empty + " \n " + str5 + " ", dbConn).ExecuteNonQuery();
      Console.WriteLine(string.Format("\n\n===Completed creating table backup and updating table '{0}' with new values for '{1}' records===", (object) encryptTable, (object) num));
    }

    private Dictionary<string, string> ConvertToAesEncryption(
      SqlConnection dbConn,
      string tgtInstanceId,
      string srcInstanceId,
      string encryptTable,
      string encryptColumn,
      string pkColumn)
    {
      Console.WriteLine("\n\n===Started converting from old encryption to new encryption for table " + encryptTable + "===");
      SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand("SELECT " + pkColumn + ", " + encryptColumn + " FROM " + encryptTable, dbConn));
      DataSet dataSet = new DataSet();
      sqlDataAdapter.Fill(dataSet);
      DataRow[] array = dataSet.Tables[0].Rows.OfType<DataRow>().ToArray<DataRow>();
      Dictionary<string, string> aesEncryption = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      DataProtection dataProtection1 = new DataProtection((IKeyProvider) new BaseKeyProvider(Encoding.UTF8.GetBytes(srcInstanceId.ToUpperInvariant())));
      DataProtection dataProtection2 = new DataProtection((IKeyProvider) new BaseKeyProvider(Encoding.UTF8.GetBytes(tgtInstanceId.ToUpperInvariant())));
      if (this.action == Mode.Encrypt)
      {
        foreach (DataRow dataRow in array)
        {
          string key = dataRow.ItemArray[0].ToString();
          string str = dataRow.ItemArray[1].ToString();
          string empty = string.Empty;
          if (!string.IsNullOrEmpty(str) && !DataProtection.CanDecrypt(Convert.FromBase64String(str)))
          {
            string s = XT.DSB64x2(str, KB.SC64);
            string base64String = Convert.ToBase64String(dataProtection2.Encrypt(Encoding.UTF8.GetBytes(s)));
            aesEncryption.Add(key, base64String);
          }
        }
      }
      else if (this.action == Mode.Decrypt)
      {
        foreach (DataRow dataRow in array)
        {
          string key = dataRow.ItemArray[0].ToString();
          string str1 = dataRow.ItemArray[1].ToString();
          string empty = string.Empty;
          if (!string.IsNullOrEmpty(str1))
          {
            byte[] cipherText = Convert.FromBase64String(str1);
            string str2 = !DataProtection.CanDecrypt(cipherText) ? XT.DSB64x2(str1, KB.SC64) : Encoding.UTF8.GetString(dataProtection2.Decrypt(cipherText));
            aesEncryption.Add(key, str2);
          }
        }
      }
      else if (this.action == Mode.Migrate)
      {
        foreach (DataRow dataRow in array)
        {
          string key = dataRow.ItemArray[0].ToString();
          string str = dataRow.ItemArray[1].ToString();
          string empty = string.Empty;
          if (!string.IsNullOrEmpty(str))
          {
            byte[] cipherText = Convert.FromBase64String(str);
            string s = !DataProtection.CanDecrypt(cipherText) ? XT.DSB64x2(str, KB.SC64) : Encoding.UTF8.GetString(dataProtection1.Decrypt(cipherText));
            string base64String = Convert.ToBase64String(dataProtection2.Encrypt(Encoding.UTF8.GetBytes(s)));
            aesEncryption.Add(key, base64String);
          }
        }
      }
      Console.WriteLine("\n\n===Completed converting from old encryption to new encryption for table " + encryptTable + " ===");
      return aesEncryption;
    }
  }
}

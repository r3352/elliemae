// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.DbTableEncryption.UserSettingsTableEncryptor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.Collections.Generic;
using System.Data.SqlClient;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.DbTableEncryption
{
  public sealed class UserSettingsTableEncryptor : DbTableEncryptor
  {
    public UserSettingsTableEncryptor(string instanceName)
      : base(instanceName)
    {
      this.EncryptionFlagGetter = (DbTableEncryptor.FlagGetter) (dbConn => (CompanySettingsFlag) new UserSettingsEncryptionFlag(dbConn, instanceName));
    }

    public override IList<string> PkColumnNames
    {
      get
      {
        return (IList<string>) new List<string>()
        {
          "userid",
          "category",
          "attribute"
        };
      }
    }

    public override IList<string> EncryptColNames
    {
      get => (IList<string>) new List<string>() { "value" };
    }

    public override IList<string> OtherColNames => (IList<string>) new List<string>();

    public override string TableName => "user_settings";

    protected override void EnsureMigrationTable(SqlConnection dbConn, Mode mode)
    {
      new SqlCommand(string.Format("BEGIN TRANSACTION\nIF NOT EXISTS (select * from sysobjects where id = object_id(N'[{0}{2}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\nBEGIN\n    CREATE TABLE [{0}{2}] (userid VARCHAR(16) NOT NULL, category VARCHAR(32) NOT NULL, attribute VARCHAR(32) NOT NULL, [value] {1} NULL,\n    CONSTRAINT PK_{0}{2} PRIMARY KEY CLUSTERED (userid, category, attribute))\nEND\nCOMMIT", (object) this.TableName, mode == Mode.Decrypt ? (object) "NVARCHAR(1024)" : (object) "VARBINARY(3400)", (object) "_new"), dbConn).ExecuteNonQuery();
    }

    protected override void PromoteMigrationTable(SqlConnection dbConn)
    {
      new SqlCommand(string.Format("IF EXISTS (select * from sysobjects where id = object_id(N'[{0}{2}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\nBEGIN\n    BEGIN TRANSACTION\n    IF EXISTS (select * from sysobjects where id = object_id(N'[{0}{1}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\n        DROP TABLE {0}{1}\n    EXEC sp_rename '{0}', '{0}{1}'\n    EXEC sp_rename 'PK_{0}', 'PK_{0}{1}'\n    EXEC sp_rename '{0}{2}', '{0}'\n    EXEC sp_rename 'PK_{0}{2}', 'PK_{0}'\n    COMMIT\nEND", (object) this.TableName, (object) "_backup", (object) "_new"), dbConn).ExecuteNonQuery();
    }
  }
}

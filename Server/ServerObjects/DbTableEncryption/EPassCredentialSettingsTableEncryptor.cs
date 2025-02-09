// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.DbTableEncryption.EPassCredentialSettingsTableEncryptor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.Collections.Generic;
using System.Data.SqlClient;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.DbTableEncryption
{
  public sealed class EPassCredentialSettingsTableEncryptor : DbTableEncryptor
  {
    public EPassCredentialSettingsTableEncryptor(string instanceName)
      : base(instanceName)
    {
      this.EncryptionFlagGetter = (DbTableEncryptor.FlagGetter) (dbConn => (CompanySettingsFlag) new EPassCredentialsEncryptionFlag(dbConn, instanceName));
    }

    public override string TableName => "ePassCredentialSettings";

    public override IList<string> PkColumnNames
    {
      get
      {
        return (IList<string>) new List<string>()
        {
          "ePassCredentialSettingID"
        };
      }
    }

    public override IList<string> EncryptColNames
    {
      get
      {
        return (IList<string>) new List<string>()
        {
          "UIDValue",
          "PasswordValue"
        };
      }
    }

    public override IList<string> OtherColNames
    {
      get
      {
        return (IList<string>) new List<string>()
        {
          "Category",
          "Title",
          "UIDName",
          "PasswordName",
          "Auth1Name",
          "Auth1Value",
          "Auth2Name",
          "Auth2Value",
          "ValidDuration",
          "Description",
          "LastModifiedDTTM",
          "UIDFieldName",
          "PasswordFieldName",
          "Auth1FieldName",
          "Auth2FieldName",
          "PartnerSection",
          "SaveLoginFieldName",
          "SaveLoginValue",
          "EncryptionType",
          "TPONumberName",
          "TPONumberFieldName",
          "TPONumberFieldValue"
        };
      }
    }

    protected override void EnsureMigrationTable(SqlConnection dbConn, Mode mode)
    {
      new SqlCommand(string.Format("BEGIN TRANSACTION\nIF NOT EXISTS (select * from sysobjects where id = object_id(N'[{0}{2}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\nBEGIN\r\n    CREATE TABLE [{0}{2}]\r\n    (\r\n        [ePassCredentialSettingID] [int] IDENTITY(1,1) NOT NULL,\r\n        [Category][nvarchar](50) NOT NULL,\r\n        [Title][nvarchar](60) NOT NULL,\r\n        [UIDName][nvarchar](50) NULL,\r\n        [UIDValue]{1} NULL,\r\n        [PasswordName][nvarchar](50) NULL,\r\n        [PasswordValue]{1} NULL,\r\n        [Auth1Name][nvarchar](50) NULL,\r\n        [Auth1Value][nvarchar](50) NULL,\r\n        [Auth2Name][nvarchar](50) NULL,\r\n        [Auth2Value][nvarchar](50) NULL,\r\n        [ValidDuration][int] NULL,\r\n        [Description][nvarchar](100) NULL,\r\n        [LastModifiedDTTM][datetime] CONSTRAINT [DF_ePassCredentialPair_LastModifiedDTTM{2}] NOT NULL DEFAULT (getdate()),\r\n        [ExpirationDTTM]  AS(case when[ValidDuration] > (0) then dateadd(day,[ValidDuration],[LastModifiedDTTM])  end),\r\n        [UIDFieldName][varchar](50) NULL,\r\n        [PasswordFieldName][varchar](50) NULL,\r\n        [Auth1FieldName][varchar](50) NULL,\r\n        [Auth2FieldName][varchar](50) NULL,\r\n        [PartnerSection][varchar](32) NULL,\r\n        [SaveLoginFieldName][varchar](50) NULL,\r\n        [SaveLoginValue][varchar](20) NULL,\r\n        [EncryptionType][varchar](10) NULL,\r\n        [TPONumberName][nvarchar](20) NULL,\r\n        [TPONumberFieldName][nvarchar](20) NULL,\r\n        [TPONumberFieldValue][nvarchar](20) NULL,\r\n        CONSTRAINT [{3}{2}] PRIMARY KEY CLUSTERED ([ePassCredentialSettingID] ASC)\r\n    )\r\nEND\nCOMMIT", (object) this.TableName, (object) (mode == Mode.Decrypt ? "[NVARCHAR](50)" : "[VARBINARY](160)"), (object) "_new", (object) "PK_ePassCredentialSetting"), dbConn).ExecuteNonQuery();
    }

    protected override void PromoteMigrationTable(SqlConnection dbConn)
    {
      new SqlCommand(string.Format("IF EXISTS (select * from sysobjects where id = object_id(N'[{0}{2}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\nBEGIN\n    BEGIN TRANSACTION\n    IF EXISTS (select * from sysobjects where id = object_id(N'[{0}{1}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\n        DROP TABLE {0}{1}\n    ALTER TABLE [ePassCrendentialSettingUser] DROP CONSTRAINT [FK_ePassCrendentialSettingUser_ePassCredentialSettingID]\n    EXEC sp_rename '{0}', '{0}{1}'\n    EXEC sp_rename '{3}', '{3}{1}'\n    EXEC sp_rename '{0}{2}', '{0}'\n    EXEC sp_rename '{3}{2}', '{3}'\n    ALTER TABLE [ePassCrendentialSettingUser] WITH CHECK ADD CONSTRAINT [FK_ePassCrendentialSettingUser_ePassCredentialSettingID]\n        FOREIGN KEY ([ePassCredentialSettingID]) REFERENCES [ePassCredentialSettings] ([ePassCredentialSettingID]) ON DELETE CASCADE\n    COMMIT\nEND", (object) this.TableName, (object) "_backup", (object) "_new", (object) "PK_ePassCredentialSetting"), dbConn).ExecuteNonQuery();
    }
  }
}

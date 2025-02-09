// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ePassCredentialSetting
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ePassCredentialSetting : IXmlSerializable
  {
    private int settingID = -1;
    private string category = "";
    private string title = "";
    private string uidName = "";
    private string uidFieldName = "";
    private string uidValue = "";
    private string passwordName = "";
    private string passwordFieldName = "";
    private string passwordValue = "";
    private string auth1Name = "";
    private string auth1FieldName = "";
    private string auth1Value = "";
    private string tpoName = "";
    private string tpoFieldName = "";
    private string tpoFieldValue = "";
    private string auth2Name = "";
    private string auth2FieldName = "";
    private string auth2Value = "";
    private int validDuration = -1;
    private string description = "";
    private string partnerSection = "";
    private string saveLoginFieldName = "";
    private string saveLoginValue = "";
    private string encryptionType = "";
    private DateTime lastModifiedDTTM = DateTime.MinValue;
    private DateTime expirationDTTM = DateTime.MinValue;

    public ePassCredentialSetting(
      string category,
      string title,
      string uidName,
      string uidValue,
      string passwordName,
      string passwordValue,
      string auth1Name,
      string auth1Value,
      string auth2Name,
      string auth2Value,
      string description,
      int validDuration,
      string uidFieldName,
      string passwordFieldName,
      string auth1FieldName,
      string auth2FieldName,
      string partnerSection,
      string saveLoginFieldName,
      string saveLoginValue,
      string encryptionType,
      string tpoName,
      string tpoFieldName,
      string tpoFieldValue)
    {
      this.category = category;
      this.title = title;
      this.uidName = uidName;
      this.uidValue = uidValue;
      this.passwordName = passwordName;
      this.passwordValue = passwordValue;
      this.auth1Name = auth1Name;
      this.auth1Value = auth1Value;
      this.auth2Name = auth2Name;
      this.auth2Value = auth2Value;
      this.description = description;
      this.validDuration = validDuration;
      this.uidFieldName = uidFieldName;
      this.passwordFieldName = passwordFieldName;
      this.auth1FieldName = auth1FieldName;
      this.auth2FieldName = auth2FieldName;
      this.partnerSection = partnerSection;
      this.saveLoginFieldName = saveLoginFieldName;
      this.saveLoginValue = saveLoginValue;
      this.encryptionType = encryptionType;
      this.tpoName = tpoName;
      this.tpoFieldName = tpoFieldName;
      this.tpoFieldValue = tpoFieldValue;
    }

    public ePassCredentialSetting(DataRow rawRow)
    {
      this.settingID = int.Parse(string.Concat(rawRow["ePassCredentialSettingID"]));
      this.category = string.Concat(rawRow[nameof (Category)]);
      this.title = string.Concat(rawRow[nameof (Title)]);
      this.uidName = string.Concat(rawRow[nameof (UIDName)]);
      this.uidValue = string.Concat(rawRow[nameof (UIDValue)]);
      this.passwordName = string.Concat(rawRow[nameof (PasswordName)]);
      this.passwordValue = string.Concat(rawRow[nameof (PasswordValue)]);
      this.auth1Name = string.Concat(rawRow[nameof (Auth1Name)]);
      this.auth1Value = string.Concat(rawRow[nameof (Auth1Value)]);
      this.auth2Name = string.Concat(rawRow[nameof (Auth2Name)]);
      this.auth2Value = string.Concat(rawRow[nameof (Auth2Value)]);
      this.description = string.Concat(rawRow[nameof (Description)]);
      this.validDuration = int.Parse(string.Concat(rawRow[nameof (ValidDuration)]));
      this.lastModifiedDTTM = Utils.ParseDate((object) string.Concat(rawRow[nameof (LastModifiedDTTM)]), DateTime.MinValue);
      this.expirationDTTM = Utils.ParseDate((object) string.Concat(rawRow[nameof (ExpirationDTTM)]), DateTime.MinValue);
      this.uidFieldName = string.Concat(rawRow[nameof (UIDFieldName)]);
      this.passwordFieldName = string.Concat(rawRow[nameof (PasswordFieldName)]);
      this.auth1FieldName = string.Concat(rawRow[nameof (Auth1FieldName)]);
      this.auth2FieldName = string.Concat(rawRow[nameof (Auth2FieldName)]);
      this.partnerSection = string.Concat(rawRow[nameof (PartnerSection)]);
      this.saveLoginFieldName = string.Concat(rawRow[nameof (SaveLoginFieldName)]);
      this.saveLoginValue = string.Concat(rawRow[nameof (SaveLoginValue)]);
      this.encryptionType = string.Concat(rawRow[nameof (EncryptionType)]);
      this.tpoName = string.Concat(rawRow["TPONumberName"]);
      this.tpoFieldName = string.Concat(rawRow["TPONumberFieldName"]);
      this.tpoFieldValue = string.Concat(rawRow["TPONumberFieldValue"]);
    }

    public ePassCredentialSetting(XmlSerializationInfo info)
    {
      this.settingID = info.GetInteger("ePassCredentialSettingID");
      this.category = info.GetString(nameof (Category));
      this.title = info.GetString(nameof (Title));
      this.uidName = info.GetString(nameof (UIDName));
      this.uidValue = info.GetString(nameof (UIDValue));
      this.passwordName = info.GetString(nameof (PasswordName));
      this.passwordValue = info.GetString(nameof (PasswordValue));
      this.auth1Name = info.GetString(nameof (Auth1Name));
      this.auth1Value = info.GetString(nameof (Auth1Value));
      this.auth2Name = info.GetString(nameof (Auth2Name));
      this.auth2Value = info.GetString(nameof (Auth2Value));
      this.description = info.GetString(nameof (Description));
      this.validDuration = info.GetInteger(nameof (ValidDuration));
      this.lastModifiedDTTM = info.GetDateTime(nameof (LastModifiedDTTM), DateTime.MinValue);
      this.expirationDTTM = info.GetDateTime(nameof (ExpirationDTTM), DateTime.MinValue);
      this.uidFieldName = info.GetString(nameof (UIDFieldName));
      this.passwordFieldName = info.GetString(nameof (PasswordFieldName));
      this.auth1FieldName = info.GetString(nameof (Auth1FieldName));
      this.auth2FieldName = info.GetString(nameof (Auth2FieldName));
      this.partnerSection = info.GetString(nameof (PartnerSection));
      this.saveLoginFieldName = info.GetString(nameof (SaveLoginFieldName));
      this.saveLoginValue = info.GetString(nameof (SaveLoginValue));
      this.encryptionType = info.GetString(nameof (EncryptionType));
      this.tpoName = info.GetString("TPONumberName");
      this.tpoFieldName = info.GetString("TPONumberFieldName");
      this.tpoFieldValue = info.GetString("TPONumberFieldValue");
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("ePassCredentialSettingID", (object) this.settingID);
      info.AddValue("Category", (object) this.category);
      info.AddValue("Title", (object) this.title);
      info.AddValue("UIDName", (object) this.uidName);
      info.AddValue("UIDValue", (object) this.uidValue);
      info.AddValue("PasswordName", (object) this.passwordName);
      info.AddValue("PasswordValue", (object) this.passwordValue);
      info.AddValue("Auth1Name", (object) this.auth1Name);
      info.AddValue("Auth1Value", (object) this.auth1Value);
      info.AddValue("Auth2Name", (object) this.auth2Name);
      info.AddValue("Auth2Value", (object) this.auth2Value);
      info.AddValue("Description", (object) this.description);
      info.AddValue("ValidDuration", (object) this.validDuration);
      info.AddValue("LastModifiedDTTM", (object) this.lastModifiedDTTM);
      info.AddValue("ExpirationDTTM", (object) this.expirationDTTM);
      info.AddValue("UIDFieldName", (object) this.uidFieldName);
      info.AddValue("PasswordFieldName", (object) this.passwordFieldName);
      info.AddValue("Auth1FieldName", (object) this.auth1FieldName);
      info.AddValue("Auth2FieldName", (object) this.auth2FieldName);
      info.AddValue("PartnerSection", (object) this.partnerSection);
      info.AddValue("SaveLoginFieldName", (object) this.saveLoginFieldName);
      info.AddValue("SaveLoginValue", (object) this.saveLoginValue);
      info.AddValue("EncryptionType", (object) this.encryptionType);
      info.AddValue("TPONumberName", (object) this.tpoName);
      info.AddValue("TPONumberFieldName", (object) this.tpoFieldName);
      info.AddValue("TPONumberFieldValue", (object) this.tpoFieldValue);
    }

    public int CredentialID => this.settingID;

    public string Category
    {
      get => this.category;
      set => this.category = value;
    }

    public string Title
    {
      get => this.title;
      set => this.title = value;
    }

    public string UIDName
    {
      get => this.uidName;
      set => this.uidName = value;
    }

    public string UIDValue
    {
      get => this.uidValue;
      set => this.uidValue = value;
    }

    public string PasswordName
    {
      get => this.passwordName;
      set => this.passwordName = value;
    }

    public string PasswordValue
    {
      get => this.passwordValue;
      set => this.passwordValue = value;
    }

    public string Auth1Name
    {
      get => this.auth1Name;
      set => this.auth1Name = value;
    }

    public string Auth1Value
    {
      get => this.auth1Value;
      set => this.auth1Value = value;
    }

    public string Auth2Name
    {
      get => this.auth2Name;
      set => this.auth2Name = value;
    }

    public string Auth2Value
    {
      get => this.auth2Value;
      set => this.auth2Value = value;
    }

    public int ValidDuration
    {
      get => this.validDuration;
      set => this.validDuration = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public DateTime LastModifiedDTTM => this.lastModifiedDTTM;

    public DateTime ExpirationDTTM => this.expirationDTTM;

    public string UIDFieldName
    {
      get => this.uidFieldName;
      set => this.uidFieldName = value;
    }

    public string PasswordFieldName
    {
      get => this.passwordFieldName;
      set => this.passwordFieldName = value;
    }

    public string Auth1FieldName
    {
      get => this.auth1FieldName;
      set => this.auth1FieldName = value;
    }

    public string TPOName
    {
      get => this.tpoName;
      set => this.tpoName = value;
    }

    public string TPOFieldName
    {
      get => this.tpoFieldName;
      set => this.tpoFieldName = value;
    }

    public string TPOFieldValue
    {
      get => this.tpoFieldValue;
      set => this.tpoFieldValue = value;
    }

    public string Auth2FieldName
    {
      get => this.auth2FieldName;
      set => this.auth2FieldName = value;
    }

    public string PartnerSection
    {
      get => this.partnerSection;
      set => this.partnerSection = value;
    }

    public string SaveLoginFieldName
    {
      get => this.saveLoginFieldName;
      set => this.saveLoginFieldName = value;
    }

    public string SaveLoginValue
    {
      get => this.saveLoginValue;
      set => this.saveLoginValue = value;
    }

    public string EncryptionType
    {
      get => this.encryptionType;
      set => this.encryptionType = value;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is ePassCredentialSetting))
        return false;
      ePassCredentialSetting credentialSetting = (ePassCredentialSetting) obj;
      return credentialSetting.Category == this.category && credentialSetting.Title == this.title && credentialSetting.UIDValue == this.uidValue;
    }

    public override int GetHashCode() => this.settingID;

    public static bool RequirePasswordEncryption(string encryptionType)
    {
      return !string.IsNullOrWhiteSpace(encryptionType) && encryptionType == "Generic";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ePassCredential
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ePassCredential : IXmlSerializable
  {
    public int ePassCredentialSettingID { get; set; }

    public string Category { get; set; }

    public string Title { get; set; }

    public string UIDValue { get; set; }

    public string PartnerSection { get; set; }

    public string UserId { get; set; }

    public string Name { get; set; }

    public string Attribute { get; set; }

    public string Value { get; set; }

    public bool IsEncrypted { get; set; }

    public string PasswordFieldName { get; set; }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.ePassCredentialSettingID = info.GetInteger("ePassCredentialSettingID");
      this.Category = info.GetString("Category");
      this.Title = info.GetString("Title");
      this.PartnerSection = info.GetString("PartnerSection");
      this.UserId = info.GetString("UserId");
      this.Name = info.GetString("Name");
      this.Attribute = info.GetString("Attribute");
      this.Value = info.GetString("Value");
      this.IsEncrypted = info.GetBoolean("IsEncrypted");
      this.PasswordFieldName = info.GetString("PasswordFieldName");
    }
  }
}

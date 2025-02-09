// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.BizContactLicenseInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class BizContactLicenseInfo : IPropertyDictionary
  {
    private string licenseNumber;
    private string licenseAuthName;
    private string licenseAuthType;
    private string licenseStateCode;
    private DateTime licenseIssueDate;

    public BizContactLicenseInfo()
    {
      this.licenseNumber = string.Empty;
      this.licenseAuthName = string.Empty;
      this.licenseAuthType = string.Empty;
      this.licenseStateCode = string.Empty;
      this.licenseIssueDate = DateTime.MinValue;
    }

    public BizContactLicenseInfo(
      string licenseNumber,
      string licenseAuthName,
      string licenseAuthType,
      string licenseStateCode,
      DateTime licenseIssueDate)
    {
      this.licenseNumber = licenseNumber;
      this.licenseAuthName = licenseAuthName;
      this.licenseAuthType = licenseAuthType;
      this.licenseStateCode = licenseStateCode;
      this.licenseIssueDate = licenseIssueDate;
    }

    public string LicenseNumber
    {
      get => this.licenseNumber;
      set => this.licenseNumber = value;
    }

    public string LicenseAuthName
    {
      get => this.licenseAuthName;
      set => this.licenseAuthName = value;
    }

    public string LicenseAuthType
    {
      get => this.licenseAuthType;
      set => this.licenseAuthType = value;
    }

    public string LicenseStateCode
    {
      get => this.licenseStateCode;
      set => this.licenseStateCode = value;
    }

    public DateTime LicenseIssueDate
    {
      get => this.licenseIssueDate;
      set => this.licenseIssueDate = value;
    }

    public object this[string propertyName]
    {
      get
      {
        propertyName = propertyName.ToLower();
        switch (propertyName)
        {
          case "licensenumber":
            return (object) this.licenseNumber;
          case "licenseauthname":
            return (object) this.licenseAuthName;
          case "licenseauthtype":
            return (object) this.licenseAuthType;
          case "licensestatecode":
            return (object) this.licenseStateCode;
          case "licenseissuedate":
            return (object) this.licenseIssueDate;
          default:
            return (object) null;
        }
      }
      set
      {
        propertyName = propertyName.ToLower();
        switch (propertyName)
        {
          case "licensenumber":
            this.licenseNumber = (string) value;
            break;
          case "licenseauthname":
            this.licenseAuthName = (string) value;
            break;
          case "licenseauthtype":
            this.licenseAuthType = (string) value;
            break;
          case "licensestatecode":
            this.licenseStateCode = (string) value;
            break;
          case "licenseissuedate":
            switch (value)
            {
              case string _:
                DateTime result = DateTime.MinValue;
                DateTime.TryParse((string) value, out result);
                this.licenseIssueDate = result;
                return;
              case DateTime dateTime:
                this.licenseIssueDate = dateTime;
                return;
              default:
                this.licenseIssueDate = DateTime.MinValue;
                return;
            }
        }
      }
    }
  }
}

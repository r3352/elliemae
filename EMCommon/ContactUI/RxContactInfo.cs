// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.RxContactInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Contact;
using System;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class RxContactInfo
  {
    private static readonly Regex nameDelimiter = new Regex("[,\\s]+", RegexOptions.RightToLeft);
    private int _ContactID;
    private string _FirstName;
    private string _LastName;
    private string _CompanyName;
    private string _BizAddress1;
    private string _BizAddress2;
    private string _BizCity;
    private string _BizState;
    private string _BizZip;
    private string _WorkPhone;
    private string _FaxNumber;
    private string _CellPhone;
    private string _BizEmail;
    private string _LicenseNumber;
    private int _CategoryID;
    private string _CategoryName;
    private string _Aba;
    private string _AccountName;
    private string _WebSite;
    private string _MortgageeClauseCompany;
    private string _MortgageeClauseName;
    private string _MortgageeClauseAddressLine;
    private string _MortgageeClauseCity;
    private string _MortgageeClauseState;
    private string _MortgageeClauseZipCode;
    private string _MortgageeClausePhone;
    private string _MortgageeClauseFax;
    private string _MortgageeClauseText;
    private Guid _ContactGuid;
    private string _HomePhone;
    private string _MortgageeClauseLocationCode;
    private string _MortgageeClauseInvestorCode;
    private string _OrganizationID;
    private string _CompanyLicAuthName;
    private string _CompanyLicAuthType;
    private string _CompanyLicIssDate;
    private string _ContactLicNo;
    private string _ConatactLicAuthName;
    private string _ContactLicAuthType;
    private string _ContactLicIssDate;
    private string _CompanyLicAuthStateCode;
    private string _ContactLicAuthStateCode;

    public override int GetHashCode()
    {
      return this._FirstName.GetHashCode() ^ this._LastName.GetHashCode();
    }

    public RxContactInfo()
    {
      this._ContactID = -1;
      this._FirstName = "";
      this._LastName = "";
      this._CompanyName = "";
      this._BizAddress1 = "";
      this._BizAddress2 = "";
      this._BizCity = "";
      this._BizState = "";
      this._BizZip = "";
      this._WorkPhone = "";
      this._FaxNumber = "";
      this._CellPhone = "";
      this._BizEmail = "";
      this._LicenseNumber = "";
      this._CategoryID = -1;
      this._CategoryName = "";
      this._Aba = "";
      this._AccountName = "";
      this._WebSite = "";
      this._MortgageeClauseCompany = "";
      this._MortgageeClauseName = "";
      this._MortgageeClauseAddressLine = "";
      this._MortgageeClauseCity = "";
      this._MortgageeClauseState = "";
      this._MortgageeClauseZipCode = "";
      this._MortgageeClausePhone = "";
      this._MortgageeClauseFax = "";
      this._MortgageeClauseText = "";
      this._MortgageeClauseLocationCode = "";
      this._MortgageeClauseInvestorCode = "";
      this._ContactGuid = Guid.NewGuid();
      this._HomePhone = "";
      this._OrganizationID = "";
      this._CompanyLicAuthName = "";
      this._CompanyLicAuthType = "";
      this._CompanyLicIssDate = "";
      this._ContactLicNo = "";
      this._ConatactLicAuthName = "";
      this._ContactLicAuthType = "";
      this._ContactLicIssDate = "";
      this.CompanyLicAuthStateCode = "";
      this._ContactLicAuthStateCode = "";
    }

    public static bool operator ==(RxContactInfo o1, RxContactInfo o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(RxContactInfo o1, RxContactInfo o2) => !(o1 == o2);

    public override bool Equals(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      RxContactInfo rxContactInfo = (RxContactInfo) obj;
      return object.Equals((object) this._FirstName, (object) rxContactInfo._FirstName) && object.Equals((object) this._LastName, (object) rxContactInfo._LastName) && object.Equals((object) this._CompanyName, (object) rxContactInfo._CompanyName) && object.Equals((object) this._BizAddress1, (object) rxContactInfo._BizAddress1) && object.Equals((object) this._BizAddress2, (object) rxContactInfo._BizAddress2) && object.Equals((object) this._BizCity, (object) rxContactInfo._BizCity) && object.Equals((object) this._BizState, (object) rxContactInfo._BizState) && object.Equals((object) this._BizZip, (object) rxContactInfo._BizZip) && object.Equals((object) this._WorkPhone, (object) rxContactInfo._WorkPhone) && object.Equals((object) this._FaxNumber, (object) rxContactInfo._FaxNumber) && object.Equals((object) this._CellPhone, (object) rxContactInfo._CellPhone) && object.Equals((object) this._BizEmail, (object) rxContactInfo._BizEmail) && object.Equals((object) this._LicenseNumber, (object) rxContactInfo._LicenseNumber) && object.Equals((object) this._CategoryName, (object) rxContactInfo._CategoryName) && object.Equals((object) this._Aba, (object) rxContactInfo._Aba) && object.Equals((object) this._AccountName, (object) rxContactInfo._AccountName) && object.Equals((object) this._MortgageeClauseCompany, (object) rxContactInfo._MortgageeClauseCompany) && object.Equals((object) this._MortgageeClauseName, (object) rxContactInfo._MortgageeClauseName) && object.Equals((object) this._MortgageeClauseAddressLine, (object) rxContactInfo._MortgageeClauseAddressLine) && object.Equals((object) this._MortgageeClauseCity, (object) rxContactInfo._MortgageeClauseCity) && object.Equals((object) this._MortgageeClauseState, (object) rxContactInfo._MortgageeClauseState) && object.Equals((object) this._MortgageeClauseZipCode, (object) rxContactInfo._MortgageeClauseZipCode) && object.Equals((object) this._MortgageeClausePhone, (object) rxContactInfo._MortgageeClausePhone) && object.Equals((object) this._MortgageeClauseFax, (object) rxContactInfo._MortgageeClauseFax) && object.Equals((object) this._MortgageeClauseText, (object) rxContactInfo._MortgageeClauseText) && object.Equals((object) this._MortgageeClauseLocationCode, (object) rxContactInfo._MortgageeClauseLocationCode) && object.Equals((object) this._MortgageeClauseInvestorCode, (object) rxContactInfo._MortgageeClauseInvestorCode) && object.Equals((object) this._ContactGuid, (object) rxContactInfo._ContactGuid) && object.Equals((object) this._HomePhone, (object) rxContactInfo._HomePhone) && object.Equals((object) this._OrganizationID, (object) rxContactInfo._OrganizationID) && object.Equals((object) this._CompanyLicAuthName, (object) rxContactInfo._CompanyLicAuthName) && object.Equals((object) this._CompanyLicAuthStateCode, (object) rxContactInfo._CompanyLicAuthStateCode) && object.Equals((object) this._CompanyLicAuthType, (object) rxContactInfo._CompanyLicAuthType) && object.Equals((object) this._CompanyLicIssDate, (object) rxContactInfo._CompanyLicIssDate) && object.Equals((object) this._ConatactLicAuthName, (object) rxContactInfo._ConatactLicAuthName) && object.Equals((object) this._ContactLicAuthStateCode, (object) rxContactInfo._ContactLicAuthStateCode) && object.Equals((object) this._ContactLicAuthType, (object) rxContactInfo._ContactLicAuthType) && object.Equals((object) this._ContactLicIssDate, (object) rxContactInfo._ContactLicIssDate) && object.Equals((object) this._ContactLicNo, (object) rxContactInfo._ContactLicNo);
    }

    public string this[RolodexField field]
    {
      get
      {
        switch (field)
        {
          case RolodexField.Company:
            return this.CompanyName;
          case RolodexField.Name:
            return this.FirstName + " " + this.LastName;
          case RolodexField.AddressLine1:
            return this.BizAddress1;
          case RolodexField.AddressLine2:
            return this.BizAddress2;
          case RolodexField.FullAddress:
            return this.BizAddress2 == "" ? this.BizAddress1 : this.BizAddress1 + ", " + this.BizAddress2;
          case RolodexField.City:
            return this.BizCity;
          case RolodexField.State:
            return this.BizState;
          case RolodexField.ZipCode:
            return this.BizZip;
          case RolodexField.Phone:
            return this.WorkPhone;
          case RolodexField.Fax:
            return this.FaxNumber;
          case RolodexField.CellPhone:
            return this.CellPhone;
          case RolodexField.Email:
            return this.BizEmail;
          case RolodexField.License:
            return this.LicenseNumber;
          case RolodexField.Category:
            return this.CategoryName;
          case RolodexField.ABA:
            return this.Aba;
          case RolodexField.AccountName:
            return this.AccountName;
          case RolodexField.WebSite:
            return this._WebSite;
          case RolodexField.MortgageeClauseCompany:
            return this._MortgageeClauseCompany;
          case RolodexField.MortgageeClauseName:
            return this.MortgageeClauseName;
          case RolodexField.MortgageeClauseAddressLine:
            return this.MortgageeClauseAddressLine;
          case RolodexField.MortgageeClauseCity:
            return this.MortgageeClauseCity;
          case RolodexField.MortgageeClauseState:
            return this.MortgageeClauseState;
          case RolodexField.MortgageeClauseZipCode:
            return this.MortgageeClauseZipCode;
          case RolodexField.MortgageeClausePhone:
            return this.MortgageeClausePhone;
          case RolodexField.MortgageeClauseFax:
            return this.MortgageeClauseFax;
          case RolodexField.MortgageeClauseText:
            return this.MortgageeClauseText;
          case RolodexField.ContactGuid:
            return this.ContactGuid.ToString();
          case RolodexField.HomePhone:
            return this.HomePhone;
          case RolodexField.OrganizationID:
            return this.OrganizationID;
          case RolodexField.ContactLicenseNo:
            return this.ContactLicNo;
          case RolodexField.ContactLicenseIssuingAuthorityName:
            return this.ContactLicAuthName;
          case RolodexField.ContactLicenseAuthorityType:
            return this.ContactLicAuthType;
          case RolodexField.ContactLicenseAuthorityStateCode:
            return this.ContactLicAuthStateCode;
          case RolodexField.ContactLicenseIssueDate:
            return this.ContactLicIssDate.ToString();
          case RolodexField.CompanyLicenseIssuingAuthorityName:
            return this.CompanyLicAuthName;
          case RolodexField.CompanyLicenseAuthorityType:
            return this.CompanyLicAuthType;
          case RolodexField.CompanyLicenseAuthorityStateCode:
            return this.CompanyLicAuthStateCode;
          case RolodexField.CompanyLicenseIssueDate:
            return this.CompanyLicIssDate.ToString();
          case RolodexField.MortgageeClauseLocationCode:
            return this.MortgageeClauseLocationCode;
          case RolodexField.MortgageeClauseInvestorCode:
            return this.MortgageeClauseInvestorCode;
          default:
            return "";
        }
      }
      set
      {
        switch (field)
        {
          case RolodexField.Company:
            this.CompanyName = value;
            break;
          case RolodexField.Name:
            string[] firstLastName = RxContactInfo.getFirstLastName(value);
            this.FirstName = firstLastName[0];
            this.LastName = firstLastName[1];
            break;
          case RolodexField.AddressLine1:
            this.BizAddress1 = value;
            break;
          case RolodexField.AddressLine2:
            this.BizAddress2 = value;
            break;
          case RolodexField.FullAddress:
            this.BizAddress1 = value;
            this.BizAddress2 = "";
            break;
          case RolodexField.City:
            this.BizCity = value;
            break;
          case RolodexField.State:
            this.BizState = value;
            break;
          case RolodexField.ZipCode:
            this.BizZip = value;
            break;
          case RolodexField.Phone:
            this.WorkPhone = value;
            break;
          case RolodexField.Fax:
            this.FaxNumber = value;
            break;
          case RolodexField.CellPhone:
            this.CellPhone = value;
            break;
          case RolodexField.Email:
            this.BizEmail = value;
            break;
          case RolodexField.License:
            this.LicenseNumber = value;
            break;
          case RolodexField.Category:
            this.CategoryName = value;
            break;
          case RolodexField.ABA:
            this.Aba = value;
            break;
          case RolodexField.AccountName:
            this.AccountName = value;
            break;
          case RolodexField.WebSite:
            this.WebSite = value;
            break;
          case RolodexField.MortgageeClauseCompany:
            this.MortgageeClauseCompany = value;
            break;
          case RolodexField.MortgageeClauseName:
            this.MortgageeClauseName = value;
            break;
          case RolodexField.MortgageeClauseAddressLine:
            this.MortgageeClauseAddressLine = value;
            break;
          case RolodexField.MortgageeClauseCity:
            this.MortgageeClauseCity = value;
            break;
          case RolodexField.MortgageeClauseState:
            this.MortgageeClauseState = value;
            break;
          case RolodexField.MortgageeClauseZipCode:
            this.MortgageeClauseZipCode = value;
            break;
          case RolodexField.MortgageeClausePhone:
            this.MortgageeClausePhone = value;
            break;
          case RolodexField.MortgageeClauseFax:
            this.MortgageeClauseFax = value;
            break;
          case RolodexField.MortgageeClauseText:
            this.MortgageeClauseText = value;
            break;
          case RolodexField.ContactGuid:
            this.ContactGuid = new Guid(value);
            break;
          case RolodexField.HomePhone:
            this.HomePhone = value;
            break;
          case RolodexField.OrganizationID:
            this.OrganizationID = value;
            break;
          case RolodexField.ContactLicenseNo:
            this.ContactLicNo = value;
            break;
          case RolodexField.ContactLicenseIssuingAuthorityName:
            this.ContactLicAuthName = value;
            break;
          case RolodexField.ContactLicenseAuthorityType:
            this.ContactLicAuthType = value;
            break;
          case RolodexField.ContactLicenseAuthorityStateCode:
            this.ContactLicAuthStateCode = value;
            break;
          case RolodexField.ContactLicenseIssueDate:
            this.ContactLicIssDate = value == "//" ? "" : value;
            break;
          case RolodexField.CompanyLicenseIssuingAuthorityName:
            this.CompanyLicAuthName = value;
            break;
          case RolodexField.CompanyLicenseAuthorityType:
            this.CompanyLicAuthType = value;
            break;
          case RolodexField.CompanyLicenseAuthorityStateCode:
            this.CompanyLicAuthStateCode = value;
            break;
          case RolodexField.CompanyLicenseIssueDate:
            this.CompanyLicIssDate = value == "//" ? "" : value;
            break;
          case RolodexField.MortgageeClauseLocationCode:
            this.MortgageeClauseLocationCode = value;
            break;
          case RolodexField.MortgageeClauseInvestorCode:
            this.MortgageeClauseInvestorCode = value;
            break;
        }
      }
    }

    public int ContactID
    {
      get => this._ContactID;
      set => this._ContactID = value;
    }

    public string FirstName
    {
      get => this._FirstName;
      set => this._FirstName = value;
    }

    public string OrganizationID
    {
      get => this._OrganizationID;
      set => this._OrganizationID = value;
    }

    public Guid ContactGuid
    {
      get => this._ContactGuid;
      set => this._ContactGuid = value;
    }

    public string LastName
    {
      get => this._LastName;
      set => this._LastName = value;
    }

    public string CompanyName
    {
      get => this._CompanyName;
      set => this._CompanyName = value;
    }

    public string BizAddress1
    {
      get => this._BizAddress1;
      set => this._BizAddress1 = value;
    }

    public string BizAddress2
    {
      get => this._BizAddress2;
      set => this._BizAddress2 = value;
    }

    public string BizCity
    {
      get => this._BizCity;
      set => this._BizCity = value;
    }

    public string BizState
    {
      get => this._BizState;
      set => this._BizState = value;
    }

    public string BizZip
    {
      get => this._BizZip;
      set => this._BizZip = value;
    }

    public string WorkPhone
    {
      get => this._WorkPhone;
      set => this._WorkPhone = value;
    }

    public string FaxNumber
    {
      get => this._FaxNumber;
      set => this._FaxNumber = value;
    }

    public string CellPhone
    {
      get => this._CellPhone;
      set => this._CellPhone = value;
    }

    public string BizEmail
    {
      get => this._BizEmail;
      set => this._BizEmail = value;
    }

    public string LicenseNumber
    {
      get => this._LicenseNumber;
      set => this._LicenseNumber = value;
    }

    public int CategoryID
    {
      get => this._CategoryID;
      set => this._CategoryID = value;
    }

    public string CategoryName
    {
      get => this._CategoryName;
      set => this._CategoryName = value;
    }

    public string Aba
    {
      get => this._Aba;
      set => this._Aba = value;
    }

    public string AccountName
    {
      get => this._AccountName;
      set => this._AccountName = value;
    }

    public string WebSite
    {
      get => this._WebSite;
      set => this._WebSite = value;
    }

    public string MortgageeClauseCompany
    {
      get => this._MortgageeClauseCompany;
      set => this._MortgageeClauseCompany = value;
    }

    public string MortgageeClauseName
    {
      get => this._MortgageeClauseName;
      set => this._MortgageeClauseName = value;
    }

    public string MortgageeClauseAddressLine
    {
      get => this._MortgageeClauseAddressLine;
      set => this._MortgageeClauseAddressLine = value;
    }

    public string MortgageeClauseCity
    {
      get => this._MortgageeClauseCity;
      set => this._MortgageeClauseCity = value;
    }

    public string MortgageeClauseState
    {
      get => this._MortgageeClauseState;
      set => this._MortgageeClauseState = value;
    }

    public string MortgageeClauseZipCode
    {
      get => this._MortgageeClauseZipCode;
      set => this._MortgageeClauseZipCode = value;
    }

    public string MortgageeClausePhone
    {
      get => this._MortgageeClausePhone;
      set => this._MortgageeClausePhone = value;
    }

    public string MortgageeClauseFax
    {
      get => this._MortgageeClauseFax;
      set => this._MortgageeClauseFax = value;
    }

    public string MortgageeClauseText
    {
      get => this._MortgageeClauseText;
      set => this._MortgageeClauseText = value;
    }

    public string HomePhone
    {
      get => this._HomePhone;
      set => this._HomePhone = value;
    }

    public string MortgageeClauseLocationCode
    {
      get => this._MortgageeClauseLocationCode;
      set => this._MortgageeClauseLocationCode = value;
    }

    public string MortgageeClauseInvestorCode
    {
      get => this._MortgageeClauseInvestorCode;
      set => this._MortgageeClauseInvestorCode = value;
    }

    private static string[] getFirstLastName(string name)
    {
      if ((name ?? "") == "")
        return new string[2]{ string.Empty, string.Empty };
      name = name.Trim();
      Match match = RxContactInfo.nameDelimiter.Match(name);
      if (match.Success)
      {
        int length = name.LastIndexOf(match.Value);
        return new string[2]
        {
          name.Substring(0, length),
          name.Substring(length + match.Value.Length)
        };
      }
      return new string[2]{ name, string.Empty };
    }

    public string CompanyLicAuthName
    {
      get => this._CompanyLicAuthName;
      set => this._CompanyLicAuthName = value;
    }

    public string CompanyLicAuthType
    {
      get => this._CompanyLicAuthType;
      set => this._CompanyLicAuthType = value;
    }

    public string CompanyLicIssDate
    {
      get => this._CompanyLicIssDate;
      set => this._CompanyLicIssDate = value;
    }

    public string ContactLicNo
    {
      get => this._ContactLicNo;
      set => this._ContactLicNo = value;
    }

    public string ContactLicAuthName
    {
      get => this._ConatactLicAuthName;
      set => this._ConatactLicAuthName = value;
    }

    public string ContactLicAuthType
    {
      get => this._ContactLicAuthType;
      set => this._ContactLicAuthType = value;
    }

    public string ContactLicIssDate
    {
      get => this._ContactLicIssDate;
      set => this._ContactLicIssDate = value;
    }

    public string CompanyLicAuthStateCode
    {
      get => this._CompanyLicAuthStateCode;
      set => this._CompanyLicAuthStateCode = value;
    }

    public string ContactLicAuthStateCode
    {
      get => this._ContactLicAuthStateCode;
      set => this._ContactLicAuthStateCode = value;
    }
  }
}

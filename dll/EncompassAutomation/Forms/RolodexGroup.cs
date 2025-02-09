// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.RolodexGroup
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.Common.Contact;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessEnums;
using mshtml;
using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public class RolodexGroup
  {
    internal static RolodexGroup Empty = new RolodexGroup();
    private RolodexGroups parentGroups;
    private IHTMLElement groupElement;

    internal RolodexGroup(RolodexGroups parentGroups, IHTMLElement groupElement)
    {
      this.parentGroups = parentGroups;
      this.groupElement = groupElement;
      if (this.parentGroups == null || this.groupElement == null)
        throw new ArgumentNullException();
      if (this.GroupID == "")
        throw new Exception("The specified rolodex group does not have a valid ID");
      if (!(this.Name == ""))
        return;
      if (!(string.Concat(groupElement.getAttribute("name", 1)) != ""))
        throw new Exception("The specified rolodex group does not have a valid name");
      groupElement.setAttribute("groupName", (object) string.Concat(groupElement.getAttribute("name", 1)), 1);
      groupElement.removeAttribute("name", 1);
    }

    private RolodexGroup()
    {
    }

    public string GroupID
    {
      get
      {
        return this.groupElement == null ? "" : string.Concat(this.groupElement.getAttribute("id", 1));
      }
    }

    public string Name
    {
      get
      {
        return this.groupElement == null ? "" : string.Concat(this.groupElement.getAttribute("groupName", 1));
      }
    }

    public BizCategory BusinessCategory
    {
      get => RolodexGroup.stringToCategory(string.Concat(this.groupElement.getAttribute("cat", 1)));
      set
      {
        this.groupElement.setAttribute("cat", (object) RolodexGroup.categoryToString(value), 1);
      }
    }

    public string this[RolodexField field]
    {
      get
      {
        string str = RolodexGroup.fieldToString(field);
        return str == "" ? "" : string.Concat(this.groupElement.getAttribute(str, 1));
      }
      set
      {
        string str = RolodexGroup.fieldToString(field);
        if (str == "")
          throw new Exception("Invalid RolodexField specified. There is no attribute name for field " + (object) field);
        this.groupElement.setAttribute(str, (object) value, 1);
      }
    }

    public RolodexField GetFieldMap(string fieldId)
    {
      foreach (RolodexField field in Enum.GetValues(typeof (RolodexField)))
      {
        if (this[field] == fieldId)
          return field;
      }
      return RolodexField.None;
    }

    public override bool Equals(object obj)
    {
      RolodexGroup rolodexGroup = obj as RolodexGroup;
      return !object.Equals(obj, (object) null) && this.GroupID == rolodexGroup.GroupID;
    }

    internal void Delete() => this.parentGroups.Remove(this.Name);

    internal void ChangeRolodexName(string newValue)
    {
      this.groupElement.setAttribute("groupName", (object) newValue, 1);
    }

    internal IHTMLElement GroupElement => this.groupElement;

    public override int GetHashCode() => this.GroupID.GetHashCode();

    public override string ToString() => this.Name;

    private static string categoryToString(BizCategory category)
    {
      return string.Concat((object) category);
    }

    private static BizCategory stringToCategory(string value)
    {
      string name = value;
      switch (value.ToLower())
      {
        case "app":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 2);
          break;
        case "att":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 5);
          break;
        case "bui":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 7);
          break;
        case "cre":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 12);
          break;
        case "doc":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 15);
          break;
        case "esc":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 3);
          break;
        case "flo":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 11);
          break;
        case "haz":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 8);
          break;
        case "inv":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 18);
          break;
        case "len":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 1);
          break;
        case "mor":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 9);
          break;
        case "noc":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 0);
          break;
        case "org":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 16);
          break;
        case "rea":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 6);
          break;
        case "ser":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 14);
          break;
        case "sur":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 10);
          break;
        case "tit":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 4);
          break;
        case "und":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 13);
          break;
        case "ver":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 17);
          break;
        case "war":
          name = BusinessCategoryEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.BusinessCategory) 19);
          break;
      }
      return EncompassApplication.Session.Contacts.BizCategories.GetItemByName(name);
    }

    private static string fieldToString(RolodexField field)
    {
      switch (field)
      {
        case RolodexField.Company:
          return "comp";
        case RolodexField.Name:
          return "name";
        case RolodexField.AddressLine1:
          return "addr1";
        case RolodexField.AddressLine2:
          return "addr2";
        case RolodexField.FullAddress:
          return "addr";
        case RolodexField.City:
          return "city";
        case RolodexField.State:
          return "st";
        case RolodexField.ZipCode:
          return "zip";
        case RolodexField.Phone:
          return "phone";
        case RolodexField.Fax:
          return "fax";
        case RolodexField.CellPhone:
          return "cell";
        case RolodexField.Email:
          return "email";
        case RolodexField.CompanyLicenseNo:
          return "lnc";
        case RolodexField.Category:
          return "ctry";
        case RolodexField.ABA:
          return "aba";
        case RolodexField.AccountName:
          return "acct";
        case RolodexField.WebSite:
          return "website";
        case RolodexField.MortgageeClauseCompany:
          return "mccomp";
        case RolodexField.MortgageeClauseName:
          return "mcname";
        case RolodexField.MortgageeClauseAddressLine:
          return "mcaddr";
        case RolodexField.MortgageeClauseCity:
          return "mccity";
        case RolodexField.MortgageeClauseState:
          return "mcst";
        case RolodexField.MortgageeClauseZipCode:
          return "mczip";
        case RolodexField.MortgageeClausePhone:
          return "mcphone";
        case RolodexField.MortgageeClauseFax:
          return "mcfax";
        case RolodexField.MortgageeClauseText:
          return "mctext";
        case RolodexField.HomePhone:
          return "homephone";
        case RolodexField.OrganizationID:
          return "orgid";
        case RolodexField.ContactLicenseNo:
          return "ContLicNo";
        case RolodexField.ContactLicenseIssuingAuthorityName:
          return "ContLicIssAuthName";
        case RolodexField.ContactLicenseAuthorityType:
          return "ContLicAuthType";
        case RolodexField.ContactLicenseAuthorityStateCode:
          return "ContLicAuthStateCode";
        case RolodexField.ContactLicenseIssueDate:
          return "ContAuthIssueDate";
        case RolodexField.CompanyLicenseIssuingAuthorityName:
          return "CompLicIssAuthName";
        case RolodexField.CompanyLicenseAuthorityType:
          return "CompLicIssAuthType";
        case RolodexField.CompanyLicenseAuthorityStateCode:
          return "CompaLicIssAuthStateCode";
        case RolodexField.CompanyLicenseIssueDate:
          return "CompaLicIssueDate";
        case RolodexField.MortgageeClauseLocationCode:
          return "mcloccode";
        case RolodexField.MortgageeClauseInvestorCode:
          return "mcinvcode";
        default:
          return "";
      }
    }
  }
}

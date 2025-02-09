// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.RolodexGroup
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.Common.Contact;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessEnums;
using mshtml;
using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents a set of field which are used to populate the form from a Rolodex control.
  /// </summary>
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
      groupElement.setAttribute("groupName", (object) string.Concat(groupElement.getAttribute("name", 1)));
      groupElement.removeAttribute("name");
    }

    private RolodexGroup()
    {
    }

    /// <summary>Gets the ID of the Rolodex group.</summary>
    public string GroupID
    {
      get
      {
        return this.groupElement == null ? "" : string.Concat(this.groupElement.getAttribute("id", 1));
      }
    }

    /// <summary>
    /// Gets the name of the rolodex group, which corresponds to the name of the Rolodex control.
    /// </summary>
    public string Name
    {
      get
      {
        return this.groupElement == null ? "" : string.Concat(this.groupElement.getAttribute("groupName", 1));
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="P:EllieMae.Encompass.Forms.RolodexGroup.BusinessCategory" /> for this rolodex group.
    /// </summary>
    public BizCategory BusinessCategory
    {
      get => RolodexGroup.stringToCategory(string.Concat(this.groupElement.getAttribute("cat", 1)));
      set => this.groupElement.setAttribute("cat", (object) RolodexGroup.categoryToString(value));
    }

    /// <summary>
    /// Gets or sets the field ID of the field which has been mapped to the
    /// specified <see cref="T:EllieMae.Encompass.Forms.RolodexField" />.
    /// </summary>
    public string this[RolodexField field]
    {
      get
      {
        string strAttributeName = RolodexGroup.fieldToString(field);
        return strAttributeName == "" ? "" : string.Concat(this.groupElement.getAttribute(strAttributeName, 1));
      }
      set
      {
        string strAttributeName = RolodexGroup.fieldToString(field);
        if (strAttributeName == "")
          throw new Exception("Invalid RolodexField specified. There is no attribute name for field " + (object) field);
        this.groupElement.setAttribute(strAttributeName, (object) value);
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.Forms.RolodexField" /> which is mapped to the specified Field ID.
    /// </summary>
    /// <param name="fieldId">The Field ID for which to find the mapping.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.Forms.RolodexField" /> which is tied to this field ID,
    /// of RolodexField.None if no mapping is present.</returns>
    public RolodexField GetFieldMap(string fieldId)
    {
      foreach (RolodexField field in Enum.GetValues(typeof (RolodexField)))
      {
        if (this[field] == fieldId)
          return field;
      }
      return RolodexField.None;
    }

    /// <summary>Provides a comparison operator for two RolodexGroups.</summary>
    /// <param name="obj">The group to compare against.</param>
    /// <returns>Returns <c>true</c> if the two groups have the same <see cref="P:EllieMae.Encompass.Forms.RolodexGroup.GroupID" />,
    /// <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      RolodexGroup rolodexGroup = obj as RolodexGroup;
      return !object.Equals(obj, (object) null) && this.GroupID == rolodexGroup.GroupID;
    }

    internal void Delete() => this.parentGroups.Remove(this.Name);

    internal void ChangeRolodexName(string newValue)
    {
      this.groupElement.setAttribute("groupName", (object) newValue);
    }

    internal IHTMLElement GroupElement => this.groupElement;

    /// <summary>Provides a has code implementation for the class.</summary>
    /// <returns>Returns a hash code based on the <see cref="P:EllieMae.Encompass.Forms.RolodexGroup.GroupID" />.</returns>
    public override int GetHashCode() => this.GroupID.GetHashCode();

    /// <summary>Provides a string representation of the group.</summary>
    /// <returns></returns>
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
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Appraiser);
          break;
        case "att":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Attorney);
          break;
        case "bui":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Builder);
          break;
        case "cre":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Credit);
          break;
        case "doc":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.DocSigning);
          break;
        case "esc":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Escrow);
          break;
        case "flo":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.FloodInsurer);
          break;
        case "haz":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.HazardInsurer);
          break;
        case "inv":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Investor);
          break;
        case "len":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Lender);
          break;
        case "mor":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.MortgageInsurer);
          break;
        case "noc":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.NoCategory);
          break;
        case "org":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Organization);
          break;
        case "rea":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.RealEstateAgent);
          break;
        case "ser":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Servicing);
          break;
        case "sur":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Surveyor);
          break;
        case "tit":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Title);
          break;
        case "und":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Underwriter);
          break;
        case "ver":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.Verification);
          break;
        case "war":
          name = BusinessCategoryEnumUtil.ValueToName(EllieMae.EMLite.Common.Contact.BusinessCategory.WarehouseBank);
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

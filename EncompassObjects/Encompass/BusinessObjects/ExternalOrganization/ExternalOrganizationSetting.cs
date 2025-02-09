// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganizationSetting
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Defines the possible settings for External Organization
  /// </summary>
  public enum ExternalOrganizationSetting
  {
    /// <summary>license</summary>
    License,
    /// <summary>organization loan types settings</summary>
    LoanTypes,
    /// <summary>list of assignable sales reps</summary>
    AssignableSalesReps,
    /// <summary>company status list</summary>
    CompanyStatus,
    /// <summary>contact status list</summary>
    ContactStatus,
    /// <summary>company rating list</summary>
    CompanyRating,
    /// <summary>attachment category list</summary>
    AttachmentCategory,
    /// <summary>price group list</summary>
    PriceGroup,
    /// <summary>selected urls</summary>
    UrlList,
    /// <summary>primary manager</summary>
    PrimaryManager,
    /// <summary>organization note</summary>
    Note,
    /// <summary>attachments</summary>
    Attachment,
    /// <summary>Lo Comp</summary>
    LOComp,
    /// <summary>LO Comp History</summary>
    LOCompHistory,
    /// <summary>list of external sales rep for the organization</summary>
    ExternalSalesRepListForOrg,
  }
}

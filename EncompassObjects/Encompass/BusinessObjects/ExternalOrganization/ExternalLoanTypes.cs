// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanTypes
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Represents loan types information of an external organization
  /// </summary>
  [Guid("C332A836-36F5-4961-8365-6633AAAF8D27")]
  public class ExternalLoanTypes
  {
    private string advancedCode = "";
    private string advancedCodeXml = "";

    internal ExternalLoanTypes(ExternalOrgLoanTypes loanTypes)
    {
      this.Id = loanTypes.Id;
      this.ExternalOrgID = loanTypes.ExternalOrgID;
      this.FHAId = loanTypes.FHAId;
      this.FHASponsorId = loanTypes.FHASonsorId;
      this.FHAStatus = loanTypes.FHAStatus;
      this.FHACompareRatio = loanTypes.FHACompareRatio;
      this.FHAApprovedDate = loanTypes.FHAApprovedDate;
      this.FHAExpirationDate = loanTypes.FHAExpirationDate;
      this.VAId = loanTypes.VAId;
      this.VAStatus = loanTypes.VAStatus;
      this.VAApprovedDate = loanTypes.VAApprovedDate;
      this.VAExpirationDate = loanTypes.VAExpirationDate;
      this.UseParentInfoFhaVa = loanTypes.UseParentInfoFhaVa;
      this.Underwriting = (ExternalUnderwriting) loanTypes.Underwriting;
      this.FHADirectEndorsement = loanTypes.FHADirectEndorsement;
      this.VASponsorID = loanTypes.VASponsorID;
      this.FHMLCApproved = loanTypes.FHMLCApproved;
      this.FNMAApproved = loanTypes.FNMAApproved;
      this.FannieMaeID = loanTypes.FannieMaeID;
      this.FreddieMacID = loanTypes.FreddieMacID;
      this.AUSMethod = loanTypes.AUSMethod;
      if (this.Underwriting == ExternalUnderwriting.ConditionallyDelegated)
      {
        this.advancedCode = loanTypes.AdvancedCode;
        this.advancedCodeXml = loanTypes.AdvancedCodeXml;
      }
      if (loanTypes.Broker != null)
        this.Broker = new ExternalChannelLoanType(loanTypes.Broker);
      if (loanTypes.CorrespondentDelegated != null)
        this.CorrespondentDelegated = new ExternalChannelLoanType(loanTypes.CorrespondentDelegated);
      if (loanTypes.CorrespondentNonDelegated == null)
        return;
      this.CorrespondentNonDelegated = new ExternalChannelLoanType(loanTypes.CorrespondentNonDelegated);
    }

    /// <summary>Gets or sets id</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets External organization ID</summary>
    public int ExternalOrgID { get; set; }

    /// <summary>Gets or sets FHA Id</summary>
    public string FHAId { get; set; }

    /// <summary>Gets or sets FHA SponsorId</summary>
    public string FHASponsorId { get; set; }

    /// <summary>Gets or sets FHA Status</summary>
    public string FHAStatus { get; set; }

    /// <summary>Gets or sets FHA Compare Ratio</summary>
    public Decimal FHACompareRatio { get; set; }

    /// <summary>Gets or sets FHA Approved Date</summary>
    public DateTime FHAApprovedDate { get; set; }

    /// <summary>Gets or sets FHA Expiration date</summary>
    public DateTime FHAExpirationDate { get; set; }

    /// <summary>Gets or sets VA Id</summary>
    public string VAId { get; set; }

    /// <summary>Gets or sets VA Status</summary>
    public string VAStatus { get; set; }

    /// <summary>Gets or sets VA approved date</summary>
    public DateTime VAApprovedDate { get; set; }

    /// <summary>Gets or sets VA Expiration date</summary>
    public DateTime VAExpirationDate { get; set; }

    /// <summary>Gets or sets Use parent info for FHA VA section</summary>
    public bool UseParentInfoFhaVa { get; set; }

    /// <summary>
    /// Gets or sets <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalChannelLoanType">Broker</see> information
    /// </summary>
    public ExternalChannelLoanType Broker { get; set; }

    /// <summary>
    /// Gets or sets <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalChannelLoanType">CorrespondentDelegated</see> information
    /// </summary>
    public ExternalChannelLoanType CorrespondentDelegated { get; set; }

    /// <summary>
    /// Gets or sets <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalChannelLoanType">CorrespondentNonDelegated</see> information
    /// </summary>
    public ExternalChannelLoanType CorrespondentNonDelegated { get; set; }

    /// <summary>
    /// Gets or sets <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalChannelLoanType">Underwriting</see> information
    /// </summary>
    public ExternalUnderwriting Underwriting { get; set; }

    /// <summary>
    /// Gets or sets <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalChannelLoanType">Advanced Code </see> information for Conditional Underwriting
    /// </summary>
    public string AdvancedCode
    {
      get
      {
        return this.Underwriting == ExternalUnderwriting.ConditionallyDelegated ? this.advancedCode : "";
      }
    }

    /// <summary>
    /// Gets or sets <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalChannelLoanType">Advanced Code Xml</see> information for Conditional Underwriting
    /// </summary>
    public string AdvancedCodeXml
    {
      get
      {
        return this.Underwriting == ExternalUnderwriting.ConditionallyDelegated ? this.advancedCodeXml : "";
      }
    }

    /// <summary>Gets or sets FHA Direct Endorsement</summary>
    public string FHADirectEndorsement { get; set; }

    /// <summary>Gets or sets VA Sponsor ID</summary>
    public string VASponsorID { get; set; }

    /// <summary>Gets or sets FHMLC Approved</summary>
    public bool FHMLCApproved { get; set; }

    /// <summary>Gets or sets FNMA Approved</summary>
    public bool FNMAApproved { get; set; }

    /// <summary>Gets or sets Fannie Mae ID</summary>
    public string FannieMaeID { get; set; }

    /// <summary>Gets or sets FreddieMac ID</summary>
    public string FreddieMacID { get; set; }

    /// <summary>Gets or sets AUS Method</summary>
    public string AUSMethod { get; set; }
  }
}

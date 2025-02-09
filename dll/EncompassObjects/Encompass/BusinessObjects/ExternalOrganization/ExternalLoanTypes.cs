// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanTypes
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
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

    public int Id { get; set; }

    public int ExternalOrgID { get; set; }

    public string FHAId { get; set; }

    public string FHASponsorId { get; set; }

    public string FHAStatus { get; set; }

    public Decimal FHACompareRatio { get; set; }

    public DateTime FHAApprovedDate { get; set; }

    public DateTime FHAExpirationDate { get; set; }

    public string VAId { get; set; }

    public string VAStatus { get; set; }

    public DateTime VAApprovedDate { get; set; }

    public DateTime VAExpirationDate { get; set; }

    public bool UseParentInfoFhaVa { get; set; }

    public ExternalChannelLoanType Broker { get; set; }

    public ExternalChannelLoanType CorrespondentDelegated { get; set; }

    public ExternalChannelLoanType CorrespondentNonDelegated { get; set; }

    public ExternalUnderwriting Underwriting { get; set; }

    public string AdvancedCode
    {
      get
      {
        return this.Underwriting == ExternalUnderwriting.ConditionallyDelegated ? this.advancedCode : "";
      }
    }

    public string AdvancedCodeXml
    {
      get
      {
        return this.Underwriting == ExternalUnderwriting.ConditionallyDelegated ? this.advancedCodeXml : "";
      }
    }

    public string FHADirectEndorsement { get; set; }

    public string VASponsorID { get; set; }

    public bool FHMLCApproved { get; set; }

    public bool FNMAApproved { get; set; }

    public string FannieMaeID { get; set; }

    public string FreddieMacID { get; set; }

    public string AUSMethod { get; set; }
  }
}

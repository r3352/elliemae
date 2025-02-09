// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalChannelLoanType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Represents external channel loan type of an external organization
  /// </summary>
  [Guid("68791F4C-2562-4BAE-B666-110FC02BF4F5")]
  public class ExternalChannelLoanType
  {
    internal ExternalChannelLoanType(
      ExternalOrgLoanTypes.ExternalOrgChannelLoanType channel)
    {
      this.ExternalOrgID = channel.ExternalOrgID;
      this.ChannelType = channel.ChannelType;
      this.LoanTypes = channel.LoanTypes;
      this.LoanPurpose = channel.LoanPurpose;
      this.AllowLoansWithIssues = channel.AllowLoansWithIssues;
      this.MsgUploadNonApprovedLoans = channel.MsgUploadNonApprovedLoans;
    }

    /// <summary>Gets or sets External org id</summary>
    public int ExternalOrgID { get; set; }

    /// <summary>Gets or sets Channel Type</summary>
    public int ChannelType { get; set; }

    /// <summary>Method to check if a particular loan type is allowed</summary>
    /// <param name="loanType">The loan type to check</param>
    /// <returns>Indicates if a particular loan type is selected.</returns>
    public bool ContainsLoanType(ExternalLoanTypeEnum loanType)
    {
      return ((ExternalLoanTypeEnum) this.LoanTypes & loanType) == loanType;
    }

    /// <summary>Method to uncheck a particular loan type</summary>
    /// <param name="loanType">The loan type to remove.</param>
    public void RemoveLoanType(ExternalLoanTypeEnum loanType)
    {
      if (!this.ContainsLoanType(loanType))
        return;
      this.LoanTypes -= (int) loanType;
    }

    /// <summary>Method to add a specified loan type</summary>
    /// <param name="loanType">The loan type to add.</param>
    public void AddLoanType(ExternalLoanTypeEnum loanType)
    {
      if (this.ContainsLoanType(loanType))
        return;
      this.LoanTypes += (int) loanType;
    }

    /// <summary>Method to get a loan type value</summary>
    public int GetLoanTypeValue() => this.LoanTypes;

    private int LoanTypes { get; set; }

    /// <summary>
    /// Method to check if a particular loan purpose is allowed
    /// </summary>
    /// <param name="loanPurpose">The loan purpose to check.</param>
    /// <returns>Indicates if a particular loan purpose is selected.</returns>
    public bool ContainsLoanPurpose(ExternalLoanPurposeEnums loanPurpose)
    {
      return ((ExternalLoanPurposeEnums) this.LoanPurpose & loanPurpose) == loanPurpose;
    }

    /// <summary>Method to remove loan purpose</summary>
    /// <param name="loanPurpose">The loan purpose to remove.</param>
    public void RemoveLoanPurpose(ExternalLoanPurposeEnums loanPurpose)
    {
      if (!this.ContainsLoanPurpose(loanPurpose))
        return;
      this.LoanPurpose -= (int) loanPurpose;
    }

    /// <summary>Method to add loan purpose</summary>
    /// <param name="loanPurpose">The loan purpose to add.</param>
    public void AddLoanPurpose(ExternalLoanPurposeEnums loanPurpose)
    {
      if (this.ContainsLoanPurpose(loanPurpose))
        return;
      this.LoanPurpose += (int) loanPurpose;
    }

    private int LoanPurpose { get; set; }

    /// <summary>Method to get a loan purpose value</summary>
    public int GetLoanPurposeValue() => this.LoanPurpose;

    /// <summary>Gets or sets FHA Streamline Type</summary>
    public string FHAStreamlineType { get; set; }

    /// <summary>Gets or sets flag to allow loans with issues</summary>
    public int AllowLoansWithIssues { get; set; }

    /// <summary>Gets or sets message for uploading unapproved loans</summary>
    public string MsgUploadNonApprovedLoans { get; set; }
  }
}

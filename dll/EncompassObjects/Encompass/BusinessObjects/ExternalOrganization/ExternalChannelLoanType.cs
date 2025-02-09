// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalChannelLoanType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
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

    public int ExternalOrgID { get; set; }

    public int ChannelType { get; set; }

    public bool ContainsLoanType(ExternalLoanTypeEnum loanType)
    {
      return ((ExternalLoanTypeEnum) this.LoanTypes & loanType) == loanType;
    }

    public void RemoveLoanType(ExternalLoanTypeEnum loanType)
    {
      if (!this.ContainsLoanType(loanType))
        return;
      this.LoanTypes -= (int) loanType;
    }

    public void AddLoanType(ExternalLoanTypeEnum loanType)
    {
      if (this.ContainsLoanType(loanType))
        return;
      this.LoanTypes += (int) loanType;
    }

    public int GetLoanTypeValue() => this.LoanTypes;

    private int LoanTypes { get; set; }

    public bool ContainsLoanPurpose(ExternalLoanPurposeEnums loanPurpose)
    {
      return ((ExternalLoanPurposeEnums) this.LoanPurpose & loanPurpose) == loanPurpose;
    }

    public void RemoveLoanPurpose(ExternalLoanPurposeEnums loanPurpose)
    {
      if (!this.ContainsLoanPurpose(loanPurpose))
        return;
      this.LoanPurpose -= (int) loanPurpose;
    }

    public void AddLoanPurpose(ExternalLoanPurposeEnums loanPurpose)
    {
      if (this.ContainsLoanPurpose(loanPurpose))
        return;
      this.LoanPurpose += (int) loanPurpose;
    }

    private int LoanPurpose { get; set; }

    public int GetLoanPurposeValue() => this.LoanPurpose;

    public string FHAStreamlineType { get; set; }

    public int AllowLoansWithIssues { get; set; }

    public string MsgUploadNonApprovedLoans { get; set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOrgLoanTypes
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ExternalOrgLoanTypes
  {
    private int underwriting = -1;

    public int Id { get; set; }

    public int ExternalOrgID { get; set; }

    public string FHAId { get; set; }

    public string FHASonsorId { get; set; }

    public string FHAStatus { get; set; }

    public Decimal FHACompareRatio { get; set; }

    public DateTime FHAApprovedDate { get; set; }

    public DateTime FHAExpirationDate { get; set; }

    public string VAId { get; set; }

    public string VAStatus { get; set; }

    public DateTime VAApprovedDate { get; set; }

    public DateTime VAExpirationDate { get; set; }

    public bool UseParentInfoFhaVa { get; set; }

    public ExternalOrgLoanTypes.ExternalOrgChannelLoanType Broker { get; set; }

    public ExternalOrgLoanTypes.ExternalOrgChannelLoanType CorrespondentDelegated { get; set; }

    public ExternalOrgLoanTypes.ExternalOrgChannelLoanType CorrespondentNonDelegated { get; set; }

    public int Underwriting
    {
      get => this.underwriting;
      set => this.underwriting = value;
    }

    public string AdvancedCode { get; set; }

    public string AdvancedCodeXml { get; set; }

    public string FHADirectEndorsement { get; set; }

    public string VASponsorID { get; set; }

    public bool FHMLCApproved { get; set; }

    public bool FNMAApproved { get; set; }

    public string FannieMaeID { get; set; }

    public string FreddieMacID { get; set; }

    public string AUSMethod { get; set; }

    [Serializable]
    public class ExternalOrgChannelLoanType
    {
      public int ExternalOrgID { get; set; }

      public int ChannelType { get; set; }

      public int LoanTypes { get; set; }

      public int LoanPurpose { get; set; }

      public int AllowLoansWithIssues { get; set; }

      public string MsgUploadNonApprovedLoans { get; set; }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.VerificationTimelineObligationLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class VerificationTimelineObligationLog : VerificationTimelineLog
  {
    private bool isInstallmentLoans;
    private bool isRealEstateLoans;
    private bool isAlimonyOrMaintenance;
    private bool isMonthlyHousingExpense;
    private bool isRevolvingChargeAccounts;
    private bool isSimultaneousLoansOnProperty;
    private bool isChildSupport;
    private bool isRequiredEscrows;
    private bool isOtherMonthlyObligation;
    private string otherMonthlyObligationDescription = string.Empty;
    private bool isNoAndAgeOfCreditLines;
    private bool isJudgements;
    private bool isBackruptcies;
    private bool isMortgageLates;
    private int numberOfMortgageLates;
    private bool isHELOC;
    private Decimal repayAmountToHELOC;
    private bool isPaymentHistory;
    private bool isCollections;
    private bool isRentalPaymentHistory;
    private bool isDebtObligationsCurrent;
    private bool is2ndLien;
    private Decimal amount2ndLien;
    private bool isOtherCreditHistory;
    private string otherCreditHistoryDescription = string.Empty;

    public VerificationTimelineObligationLog(XmlElement e)
      : base(e)
    {
    }

    public VerificationTimelineObligationLog()
    {
    }

    public bool IsInstallmentLoans
    {
      set => this.isInstallmentLoans = value;
      get => this.isInstallmentLoans;
    }

    public bool IsRealEstateLoans
    {
      set => this.isRealEstateLoans = value;
      get => this.isRealEstateLoans;
    }

    public bool IsAlimonyOrMaintenance
    {
      set => this.isAlimonyOrMaintenance = value;
      get => this.isAlimonyOrMaintenance;
    }

    public bool IsMonthlyHousingExpense
    {
      set => this.isMonthlyHousingExpense = value;
      get => this.isMonthlyHousingExpense;
    }

    public bool IsRevolvingChargeAccounts
    {
      set => this.isRevolvingChargeAccounts = value;
      get => this.isRevolvingChargeAccounts;
    }

    public bool IsSimultaneousLoansOnProperty
    {
      set => this.isSimultaneousLoansOnProperty = value;
      get => this.isSimultaneousLoansOnProperty;
    }

    public bool IsChildSupport
    {
      set => this.isChildSupport = value;
      get => this.isChildSupport;
    }

    public bool IsRequiredEscrows
    {
      set => this.isRequiredEscrows = value;
      get => this.isRequiredEscrows;
    }

    public bool IsOtherMonthlyObligation
    {
      set => this.isOtherMonthlyObligation = value;
      get => this.isOtherMonthlyObligation;
    }

    public string OtherMonthlyObligationDescription
    {
      set => this.otherMonthlyObligationDescription = value;
      get => this.otherMonthlyObligationDescription;
    }

    public bool IsNoAndAgeOfCreditLines
    {
      set => this.isNoAndAgeOfCreditLines = value;
      get => this.isNoAndAgeOfCreditLines;
    }

    public bool IsJudgements
    {
      set => this.isJudgements = value;
      get => this.isJudgements;
    }

    public bool IsBackruptcies
    {
      set => this.isBackruptcies = value;
      get => this.isBackruptcies;
    }

    public bool IsMortgageLates
    {
      set => this.isMortgageLates = value;
      get => this.isMortgageLates;
    }

    public int NumberOfMortgageLates
    {
      set => this.numberOfMortgageLates = value;
      get => this.numberOfMortgageLates;
    }

    public bool IsHELOC
    {
      set => this.isHELOC = value;
      get => this.isHELOC;
    }

    public Decimal RepayAmountToHELOC
    {
      set => this.repayAmountToHELOC = value;
      get => this.repayAmountToHELOC;
    }

    public bool IsPaymentHistory
    {
      set => this.isPaymentHistory = value;
      get => this.isPaymentHistory;
    }

    public bool IsCollections
    {
      set => this.isCollections = value;
      get => this.isCollections;
    }

    public bool IsRentalPaymentHistory
    {
      set => this.isRentalPaymentHistory = value;
      get => this.isRentalPaymentHistory;
    }

    public bool IsDebtObligationsCurrent
    {
      set => this.isDebtObligationsCurrent = value;
      get => this.isDebtObligationsCurrent;
    }

    public bool Is2ndLien
    {
      set => this.is2ndLien = value;
      get => this.is2ndLien;
    }

    public Decimal Amount2ndLien
    {
      set => this.amount2ndLien = value;
      get => this.amount2ndLien;
    }

    public bool IsOtherCreditHistory
    {
      set => this.isOtherCreditHistory = value;
      get => this.isOtherCreditHistory;
    }

    public string OtherCreditHistoryDescription
    {
      set => this.otherCreditHistoryDescription = value;
      get => this.otherCreditHistoryDescription;
    }

    public string BuildWhatVerified()
    {
      string str = string.Empty;
      if (this.IsInstallmentLoans)
        str += "Installment Loans";
      if (this.IsRealEstateLoans)
        str = str + (str != string.Empty ? "," : "") + "Real Estate Loans";
      if (this.IsAlimonyOrMaintenance)
        str = str + (str != string.Empty ? "," : "") + "Alimony/Maintenance";
      if (this.IsMonthlyHousingExpense)
        str = str + (str != string.Empty ? "," : "") + "Monthly Housing Expense [P & I]";
      if (this.IsRevolvingChargeAccounts)
        str = str + (str != string.Empty ? "," : "") + "Revolving Charge Accounts";
      if (this.IsSimultaneousLoansOnProperty)
        str = str + (str != string.Empty ? "," : "") + "Simultaneous Loans on Property";
      if (this.IsChildSupport)
        str = str + (str != string.Empty ? "," : "") + "Child Support";
      if (this.IsRequiredEscrows)
        str = str + (str != string.Empty ? "," : "") + "Required Escrows";
      if (this.IsOtherMonthlyObligation)
        str = str + (str != string.Empty ? "," : "") + "Other Monthly Obligation";
      if (this.IsNoAndAgeOfCreditLines)
        str = str + (str != string.Empty ? "," : "") + "No. and Age of Credit Lines";
      if (this.IsJudgements)
        str = str + (str != string.Empty ? "," : "") + "Judgements";
      if (this.IsBackruptcies)
        str = str + (str != string.Empty ? "," : "") + "Backruptcies";
      if (this.IsMortgageLates)
        str = str + (str != string.Empty ? "," : "") + "Mortgage Lates";
      if (this.IsHELOC)
        str = str + (str != string.Empty ? "," : "") + "HELOC";
      if (this.IsPaymentHistory)
        str = str + (str != string.Empty ? "," : "") + "Payment History";
      if (this.IsCollections)
        str = str + (str != string.Empty ? "," : "") + "Collections";
      if (this.IsRentalPaymentHistory)
        str = str + (str != string.Empty ? "," : "") + "Rental Payment History";
      if (this.IsDebtObligationsCurrent)
        str = str + (str != string.Empty ? "," : "") + "Debt Obligations Current";
      if (this.Is2ndLien)
        str = str + (str != string.Empty ? "," : "") + "2nd Lien";
      if (this.IsOtherCreditHistory)
        str = str + (str != string.Empty ? "," : "") + "Other Credit History";
      return str;
    }

    public void SetStatusToXml(XmlElement fieldXml)
    {
      fieldXml.SetAttribute("IsInstallmentLoans", this.IsInstallmentLoans ? "Y" : "N");
      fieldXml.SetAttribute("IsRealEstateLoans", this.IsRealEstateLoans ? "Y" : "N");
      fieldXml.SetAttribute("IsAlimonyOrMaintenance", this.IsAlimonyOrMaintenance ? "Y" : "N");
      fieldXml.SetAttribute("IsMonthlyHousingExpense", this.IsMonthlyHousingExpense ? "Y" : "N");
      fieldXml.SetAttribute("IsRevolvingChargeAccounts", this.IsRevolvingChargeAccounts ? "Y" : "N");
      fieldXml.SetAttribute("IsSimultaneousLoansOnProperty", this.IsSimultaneousLoansOnProperty ? "Y" : "N");
      fieldXml.SetAttribute("IsChildSupport", this.IsChildSupport ? "Y" : "N");
      fieldXml.SetAttribute("IsRequiredEscrows", this.IsRequiredEscrows ? "Y" : "N");
      fieldXml.SetAttribute("IsOtherMonthlyObligation", this.IsOtherMonthlyObligation ? "Y" : "N");
      fieldXml.SetAttribute("OtherMonthlyObligationDescription", this.OtherMonthlyObligationDescription);
      fieldXml.SetAttribute("IsNoAndAgeOfCreditLines", this.IsNoAndAgeOfCreditLines ? "Y" : "N");
      fieldXml.SetAttribute("IsJudgements", this.IsJudgements ? "Y" : "N");
      fieldXml.SetAttribute("IsBackruptcies", this.IsBackruptcies ? "Y" : "N");
      fieldXml.SetAttribute("IsMortgageLates", this.IsMortgageLates ? "Y" : "N");
      fieldXml.SetAttribute("NumberOfMortgageLates", this.NumberOfMortgageLates.ToString());
      fieldXml.SetAttribute("IsHELOC", this.IsHELOC ? "Y" : "N");
      fieldXml.SetAttribute("RepayAmountToHELOC", this.RepayAmountToHELOC.ToString("N2"));
      fieldXml.SetAttribute("IsPaymentHistory", this.IsPaymentHistory ? "Y" : "N");
      fieldXml.SetAttribute("IsCollections", this.IsCollections ? "Y" : "N");
      fieldXml.SetAttribute("IsRentalPaymentHistory", this.IsRentalPaymentHistory ? "Y" : "N");
      fieldXml.SetAttribute("IsDebtObligationsCurrent", this.IsDebtObligationsCurrent ? "Y" : "N");
      fieldXml.SetAttribute("Is2ndLien", this.Is2ndLien ? "Y" : "N");
      fieldXml.SetAttribute("Amount2ndLien", this.Amount2ndLien.ToString("N2"));
      fieldXml.SetAttribute("IsOtherCreditHistory", this.IsOtherCreditHistory ? "Y" : "N");
      fieldXml.SetAttribute("OtherCreditHistoryDescription", this.OtherCreditHistoryDescription);
    }

    public void GetStatusFromXml(XmlElement e)
    {
      XmlElement xmlElement = (XmlElement) e.SelectSingleNode("STATUS");
      if (xmlElement == null)
        return;
      if (xmlElement.HasAttribute("IsInstallmentLoans"))
        this.IsInstallmentLoans = xmlElement.GetAttribute("IsInstallmentLoans") == "Y";
      if (xmlElement.HasAttribute("IsRealEstateLoans"))
        this.IsRealEstateLoans = xmlElement.GetAttribute("IsRealEstateLoans") == "Y";
      if (xmlElement.HasAttribute("IsAlimonyOrMaintenance"))
        this.IsAlimonyOrMaintenance = xmlElement.GetAttribute("IsAlimonyOrMaintenance") == "Y";
      if (xmlElement.HasAttribute("IsMonthlyHousingExpense"))
        this.IsMonthlyHousingExpense = xmlElement.GetAttribute("IsMonthlyHousingExpense") == "Y";
      if (xmlElement.HasAttribute("IsRevolvingChargeAccounts"))
        this.IsRevolvingChargeAccounts = xmlElement.GetAttribute("IsRevolvingChargeAccounts") == "Y";
      if (xmlElement.HasAttribute("IsSimultaneousLoansOnProperty"))
        this.IsSimultaneousLoansOnProperty = xmlElement.GetAttribute("IsSimultaneousLoansOnProperty") == "Y";
      if (xmlElement.HasAttribute("IsChildSupport"))
        this.IsChildSupport = xmlElement.GetAttribute("IsChildSupport") == "Y";
      if (xmlElement.HasAttribute("IsRequiredEscrows"))
        this.IsRequiredEscrows = xmlElement.GetAttribute("IsRequiredEscrows") == "Y";
      if (xmlElement.HasAttribute("IsOtherMonthlyObligation"))
        this.IsOtherMonthlyObligation = xmlElement.GetAttribute("IsOtherMonthlyObligation") == "Y";
      if (xmlElement.HasAttribute("OtherMonthlyObligationDescription"))
        this.OtherMonthlyObligationDescription = xmlElement.GetAttribute("OtherMonthlyObligationDescription");
      if (xmlElement.HasAttribute("IsNoAndAgeOfCreditLines"))
        this.IsNoAndAgeOfCreditLines = xmlElement.GetAttribute("IsNoAndAgeOfCreditLines") == "Y";
      if (xmlElement.HasAttribute("IsJudgements"))
        this.IsJudgements = xmlElement.GetAttribute("IsJudgements") == "Y";
      if (xmlElement.HasAttribute("IsBackruptcies"))
        this.IsBackruptcies = xmlElement.GetAttribute("IsBackruptcies") == "Y";
      if (xmlElement.HasAttribute("IsMortgageLates"))
        this.IsMortgageLates = xmlElement.GetAttribute("IsMortgageLates") == "Y";
      if (xmlElement.HasAttribute("NumberOfMortgageLates"))
        this.NumberOfMortgageLates = xmlElement.GetAttribute("NumberOfMortgageLates") == "" ? 0 : Utils.ParseInt((object) xmlElement.GetAttribute("NumberOfMortgageLates"));
      if (xmlElement.HasAttribute("IsHELOC"))
        this.IsHELOC = xmlElement.GetAttribute("IsHELOC") == "Y";
      if (xmlElement.HasAttribute("RepayAmountToHELOC"))
        this.RepayAmountToHELOC = xmlElement.GetAttribute("RepayAmountToHELOC") == "" ? 0M : Utils.ParseDecimal((object) xmlElement.GetAttribute("RepayAmountToHELOC"));
      if (xmlElement.HasAttribute("IsPaymentHistory"))
        this.IsPaymentHistory = xmlElement.GetAttribute("IsPaymentHistory") == "Y";
      if (xmlElement.HasAttribute("IsCollections"))
        this.IsCollections = xmlElement.GetAttribute("IsCollections") == "Y";
      if (xmlElement.HasAttribute("IsRentalPaymentHistory"))
        this.IsRentalPaymentHistory = xmlElement.GetAttribute("IsRentalPaymentHistory") == "Y";
      if (xmlElement.HasAttribute("IsDebtObligationsCurrent"))
        this.IsDebtObligationsCurrent = xmlElement.GetAttribute("IsDebtObligationsCurrent") == "Y";
      if (xmlElement.HasAttribute("Is2ndLien"))
        this.Is2ndLien = xmlElement.GetAttribute("Is2ndLien") == "Y";
      if (xmlElement.HasAttribute("Amount2ndLien"))
        this.Amount2ndLien = xmlElement.GetAttribute("Amount2ndLien") == "" ? 0M : Utils.ParseDecimal((object) xmlElement.GetAttribute("Amount2ndLien"));
      if (xmlElement.HasAttribute("IsOtherCreditHistory"))
        this.IsOtherCreditHistory = xmlElement.GetAttribute("IsOtherCreditHistory") == "Y";
      if (!xmlElement.HasAttribute("OtherCreditHistoryDescription"))
        return;
      this.OtherCreditHistoryDescription = xmlElement.GetAttribute("OtherCreditHistoryDescription");
    }
  }
}

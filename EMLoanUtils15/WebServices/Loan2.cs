// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.Loan2
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://dataservices.elliemaeservices.com/")]
  [Serializable]
  public class Loan2
  {
    private Borrower2[] borrowersField;
    private string clientIDField;
    private string systemIDField;
    private string loanGUIDField;
    private string userIDField;
    private string encompassVersionField;
    private string loanOfficerNameField;
    private string channelField;
    private string loanStatusField;
    private string loanStatusDateField;
    private string propertyAddressField;
    private string propertyCityField;
    private string propertyStateField;
    private string propertyZipField;
    private string lTVField;
    private string cLTVField;
    private string dtiBackField;
    private string dtiFrontField;
    private string documentationTypeField;
    private string propertyTypeField;
    private string estimatedValueField;
    private string appraisedValueField;
    private string loanProgramField;
    private string lenderField;
    private string loanPurposeField;
    private string occupancyTypeField;
    private string loanTypeField;
    private string lienPositionField;
    private string amortizationTypeField;
    private string armTypeField;
    private string interestOnlyField;
    private string loanAmountField;
    private string lockDateField;
    private string purchasePriceField;
    private string downPaymentField;
    private string lockDaysField;
    private string lockedField;
    private string noteRateField;
    private string termField;
    private string consentReceivedField;
    private string currentCoreMilestoneField;
    private string creditOrderedField;
    private string submittedToLenderField;
    private string investorNameField;
    private string creditDecisionScoreField;
    private string lockBaseBuyRateField;
    private string lockNetBuyRateField;
    private string lockBaseBuyPriceField;
    private string lockNetBuyPriceField;
    private string lockBaseBuyMarginField;
    private string lockNetBuyMarginField;
    private string buyBaseRateField;
    private string buyNetRateField;
    private string buyBasePriceField;
    private string buyNetPriceField;
    private string buyBaseMarginField;
    private string buyNetMarginField;
    private string buySRPPaidOutField;
    private string buyTotalPriceField;
    private string sellBaseRateField;
    private string sellNetRateField;
    private string sellBasePriceField;
    private string sellNetPriceField;
    private string sellBaseMarginField;
    private string sellNetMarginField;
    private string sellSRPFromInvestorField;
    private string sellDiscountYSPField;
    private string sellTotalPriceField;
    private string sellInvestorField;
    private string loanOriginationFeeField;
    private string lOFPaidByField;
    private string lOFPaidToField;
    private string loanDiscountFeeField;
    private string cTBDoNotPrintField;
    private string cTB824DescriptionField;
    private string cTB824PercentageField;
    private string cTB824AmountField;
    private string cTB825DescriptionField;
    private string cTB825PercentageField;
    private string cTB825AmountField;
    private string loanNumberField;
    private string closingDateField;
    private string closingDateForBillingField;
    private string fundsSentDateField;

    public Borrower2[] Borrowers
    {
      get => this.borrowersField;
      set => this.borrowersField = value;
    }

    public string ClientID
    {
      get => this.clientIDField;
      set => this.clientIDField = value;
    }

    public string SystemID
    {
      get => this.systemIDField;
      set => this.systemIDField = value;
    }

    public string LoanGUID
    {
      get => this.loanGUIDField;
      set => this.loanGUIDField = value;
    }

    public string UserID
    {
      get => this.userIDField;
      set => this.userIDField = value;
    }

    public string EncompassVersion
    {
      get => this.encompassVersionField;
      set => this.encompassVersionField = value;
    }

    public string LoanOfficerName
    {
      get => this.loanOfficerNameField;
      set => this.loanOfficerNameField = value;
    }

    public string Channel
    {
      get => this.channelField;
      set => this.channelField = value;
    }

    public string LoanStatus
    {
      get => this.loanStatusField;
      set => this.loanStatusField = value;
    }

    public string LoanStatusDate
    {
      get => this.loanStatusDateField;
      set => this.loanStatusDateField = value;
    }

    public string PropertyAddress
    {
      get => this.propertyAddressField;
      set => this.propertyAddressField = value;
    }

    public string PropertyCity
    {
      get => this.propertyCityField;
      set => this.propertyCityField = value;
    }

    public string PropertyState
    {
      get => this.propertyStateField;
      set => this.propertyStateField = value;
    }

    public string PropertyZip
    {
      get => this.propertyZipField;
      set => this.propertyZipField = value;
    }

    public string LTV
    {
      get => this.lTVField;
      set => this.lTVField = value;
    }

    public string CLTV
    {
      get => this.cLTVField;
      set => this.cLTVField = value;
    }

    public string DtiBack
    {
      get => this.dtiBackField;
      set => this.dtiBackField = value;
    }

    public string DtiFront
    {
      get => this.dtiFrontField;
      set => this.dtiFrontField = value;
    }

    public string DocumentationType
    {
      get => this.documentationTypeField;
      set => this.documentationTypeField = value;
    }

    public string PropertyType
    {
      get => this.propertyTypeField;
      set => this.propertyTypeField = value;
    }

    public string EstimatedValue
    {
      get => this.estimatedValueField;
      set => this.estimatedValueField = value;
    }

    public string AppraisedValue
    {
      get => this.appraisedValueField;
      set => this.appraisedValueField = value;
    }

    public string LoanProgram
    {
      get => this.loanProgramField;
      set => this.loanProgramField = value;
    }

    public string Lender
    {
      get => this.lenderField;
      set => this.lenderField = value;
    }

    public string LoanPurpose
    {
      get => this.loanPurposeField;
      set => this.loanPurposeField = value;
    }

    public string OccupancyType
    {
      get => this.occupancyTypeField;
      set => this.occupancyTypeField = value;
    }

    public string LoanType
    {
      get => this.loanTypeField;
      set => this.loanTypeField = value;
    }

    public string LienPosition
    {
      get => this.lienPositionField;
      set => this.lienPositionField = value;
    }

    public string AmortizationType
    {
      get => this.amortizationTypeField;
      set => this.amortizationTypeField = value;
    }

    public string ArmType
    {
      get => this.armTypeField;
      set => this.armTypeField = value;
    }

    public string InterestOnly
    {
      get => this.interestOnlyField;
      set => this.interestOnlyField = value;
    }

    public string LoanAmount
    {
      get => this.loanAmountField;
      set => this.loanAmountField = value;
    }

    public string LockDate
    {
      get => this.lockDateField;
      set => this.lockDateField = value;
    }

    public string PurchasePrice
    {
      get => this.purchasePriceField;
      set => this.purchasePriceField = value;
    }

    public string DownPayment
    {
      get => this.downPaymentField;
      set => this.downPaymentField = value;
    }

    public string LockDays
    {
      get => this.lockDaysField;
      set => this.lockDaysField = value;
    }

    public string Locked
    {
      get => this.lockedField;
      set => this.lockedField = value;
    }

    public string NoteRate
    {
      get => this.noteRateField;
      set => this.noteRateField = value;
    }

    public string Term
    {
      get => this.termField;
      set => this.termField = value;
    }

    public string ConsentReceived
    {
      get => this.consentReceivedField;
      set => this.consentReceivedField = value;
    }

    public string CurrentCoreMilestone
    {
      get => this.currentCoreMilestoneField;
      set => this.currentCoreMilestoneField = value;
    }

    public string CreditOrdered
    {
      get => this.creditOrderedField;
      set => this.creditOrderedField = value;
    }

    public string SubmittedToLender
    {
      get => this.submittedToLenderField;
      set => this.submittedToLenderField = value;
    }

    public string InvestorName
    {
      get => this.investorNameField;
      set => this.investorNameField = value;
    }

    public string CreditDecisionScore
    {
      get => this.creditDecisionScoreField;
      set => this.creditDecisionScoreField = value;
    }

    public string LockBaseBuyRate
    {
      get => this.lockBaseBuyRateField;
      set => this.lockBaseBuyRateField = value;
    }

    public string LockNetBuyRate
    {
      get => this.lockNetBuyRateField;
      set => this.lockNetBuyRateField = value;
    }

    public string LockBaseBuyPrice
    {
      get => this.lockBaseBuyPriceField;
      set => this.lockBaseBuyPriceField = value;
    }

    public string LockNetBuyPrice
    {
      get => this.lockNetBuyPriceField;
      set => this.lockNetBuyPriceField = value;
    }

    public string LockBaseBuyMargin
    {
      get => this.lockBaseBuyMarginField;
      set => this.lockBaseBuyMarginField = value;
    }

    public string LockNetBuyMargin
    {
      get => this.lockNetBuyMarginField;
      set => this.lockNetBuyMarginField = value;
    }

    public string BuyBaseRate
    {
      get => this.buyBaseRateField;
      set => this.buyBaseRateField = value;
    }

    public string BuyNetRate
    {
      get => this.buyNetRateField;
      set => this.buyNetRateField = value;
    }

    public string BuyBasePrice
    {
      get => this.buyBasePriceField;
      set => this.buyBasePriceField = value;
    }

    public string BuyNetPrice
    {
      get => this.buyNetPriceField;
      set => this.buyNetPriceField = value;
    }

    public string BuyBaseMargin
    {
      get => this.buyBaseMarginField;
      set => this.buyBaseMarginField = value;
    }

    public string BuyNetMargin
    {
      get => this.buyNetMarginField;
      set => this.buyNetMarginField = value;
    }

    public string BuySRPPaidOut
    {
      get => this.buySRPPaidOutField;
      set => this.buySRPPaidOutField = value;
    }

    public string BuyTotalPrice
    {
      get => this.buyTotalPriceField;
      set => this.buyTotalPriceField = value;
    }

    public string SellBaseRate
    {
      get => this.sellBaseRateField;
      set => this.sellBaseRateField = value;
    }

    public string SellNetRate
    {
      get => this.sellNetRateField;
      set => this.sellNetRateField = value;
    }

    public string SellBasePrice
    {
      get => this.sellBasePriceField;
      set => this.sellBasePriceField = value;
    }

    public string SellNetPrice
    {
      get => this.sellNetPriceField;
      set => this.sellNetPriceField = value;
    }

    public string SellBaseMargin
    {
      get => this.sellBaseMarginField;
      set => this.sellBaseMarginField = value;
    }

    public string SellNetMargin
    {
      get => this.sellNetMarginField;
      set => this.sellNetMarginField = value;
    }

    public string SellSRPFromInvestor
    {
      get => this.sellSRPFromInvestorField;
      set => this.sellSRPFromInvestorField = value;
    }

    public string SellDiscountYSP
    {
      get => this.sellDiscountYSPField;
      set => this.sellDiscountYSPField = value;
    }

    public string SellTotalPrice
    {
      get => this.sellTotalPriceField;
      set => this.sellTotalPriceField = value;
    }

    public string SellInvestor
    {
      get => this.sellInvestorField;
      set => this.sellInvestorField = value;
    }

    public string LoanOriginationFee
    {
      get => this.loanOriginationFeeField;
      set => this.loanOriginationFeeField = value;
    }

    public string LOFPaidBy
    {
      get => this.lOFPaidByField;
      set => this.lOFPaidByField = value;
    }

    public string LOFPaidTo
    {
      get => this.lOFPaidToField;
      set => this.lOFPaidToField = value;
    }

    public string LoanDiscountFee
    {
      get => this.loanDiscountFeeField;
      set => this.loanDiscountFeeField = value;
    }

    public string CTBDoNotPrint
    {
      get => this.cTBDoNotPrintField;
      set => this.cTBDoNotPrintField = value;
    }

    public string CTB824Description
    {
      get => this.cTB824DescriptionField;
      set => this.cTB824DescriptionField = value;
    }

    public string CTB824Percentage
    {
      get => this.cTB824PercentageField;
      set => this.cTB824PercentageField = value;
    }

    public string CTB824Amount
    {
      get => this.cTB824AmountField;
      set => this.cTB824AmountField = value;
    }

    public string CTB825Description
    {
      get => this.cTB825DescriptionField;
      set => this.cTB825DescriptionField = value;
    }

    public string CTB825Percentage
    {
      get => this.cTB825PercentageField;
      set => this.cTB825PercentageField = value;
    }

    public string CTB825Amount
    {
      get => this.cTB825AmountField;
      set => this.cTB825AmountField = value;
    }

    public string LoanNumber
    {
      get => this.loanNumberField;
      set => this.loanNumberField = value;
    }

    public string ClosingDate
    {
      get => this.closingDateField;
      set => this.closingDateField = value;
    }

    public string ClosingDateForBilling
    {
      get => this.closingDateForBillingField;
      set => this.closingDateForBillingField = value;
    }

    public string FundsSentDate
    {
      get => this.fundsSentDateField;
      set => this.fundsSentDateField = value;
    }
  }
}

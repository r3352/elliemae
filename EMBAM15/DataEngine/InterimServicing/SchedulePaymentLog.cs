// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.SchedulePaymentLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class SchedulePaymentLog : ServicingTransactionBase
  {
    private int paymentNo;
    private DateTime latePaymentDate = DateTime.MinValue;
    private DateTime paymentReceivedDate = DateTime.MinValue;
    private double indexRate;
    private double interestRate;
    private double principalDue;
    private double interestDue;
    private double escrowDue;
    private double buydownSubsidyAmountDue;
    private double miscFeeDue;
    private double unpaidLateFeeDue;
    private double totalPastDue;
    private double principal;
    private double interest;
    private double escrow;
    private double buydownSubsidyAmount;
    private double miscFee;
    private double lateFee;
    private double additionalPrincipal;
    private double additionalEscrow;
    private double taxes;
    private double hazardInsurance;
    private double mortgageInsurance;
    private double floodInsurance;
    private double citypropertytax;
    private double other1Escrow;
    private double other2Escrow;
    private double other3Escrow;
    private double usdamonthlyPremium;
    private double escrowTaxDue;
    private double escrowMortgageInsuranceDue;
    private double escrowFloodInsuranceDue;
    private double escrowHazardInsuranceDue;
    private double escrowCityPropertyTaxDue;
    private double escrowOther1Due;
    private double escrowOther2Due;
    private double escrowOther3Due;
    private double escrowUSDAMonthlyPremiumDue;

    public SchedulePaymentLog() => this.TransactionType = ServicingTransactionTypes.SchedulePayment;

    public SchedulePaymentLog(XmlElement e)
      : base(e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.paymentNo = attributeReader.GetInteger(nameof (PaymentNo));
      this.latePaymentDate = attributeReader.GetDate(nameof (LatePaymentDate));
      this.indexRate = attributeReader.GetDouble(nameof (IndexRate), 0.0);
      this.interestRate = attributeReader.GetDouble(nameof (InterestRate), 0.0);
      this.principalDue = attributeReader.GetDouble(nameof (PrincipalDue), 0.0);
      this.interestDue = attributeReader.GetDouble(nameof (InterestDue), 0.0);
      this.escrowDue = attributeReader.GetDouble(nameof (EscrowDue), 0.0);
      this.buydownSubsidyAmountDue = attributeReader.GetDouble(nameof (BuydownSubsidyAmountDue), 0.0);
      this.miscFeeDue = attributeReader.GetDouble(nameof (MiscFeeDue), 0.0);
      this.totalPastDue = attributeReader.GetDouble(nameof (TotalPastDue), 0.0);
      this.unpaidLateFeeDue = attributeReader.GetDouble(nameof (UnpaidLateFeeDue), 0.0);
      this.taxes = attributeReader.GetDouble(nameof (Taxes), 0.0);
      this.hazardInsurance = attributeReader.GetDouble(nameof (HazardInsurance), 0.0);
      this.mortgageInsurance = attributeReader.GetDouble(nameof (MortgageInsurance), 0.0);
      this.floodInsurance = attributeReader.GetDouble(nameof (FloodInsurance), 0.0);
      this.citypropertytax = attributeReader.GetDouble("CityPropertyTax", 0.0);
      this.other1Escrow = attributeReader.GetDouble(nameof (Other1Escrow), 0.0);
      this.other2Escrow = attributeReader.GetDouble(nameof (Other2Escrow), 0.0);
      this.other3Escrow = attributeReader.GetDouble(nameof (Other3Escrow), 0.0);
      this.usdamonthlyPremium = attributeReader.GetDouble(nameof (USDAMonthlyPremium), 0.0);
      this.escrowTaxDue = attributeReader.GetDouble(nameof (EscrowTaxDue), 0.0);
      this.escrowMortgageInsuranceDue = attributeReader.GetDouble(nameof (EscrowMortgageInsuranceDue), 0.0);
      this.escrowFloodInsuranceDue = attributeReader.GetDouble(nameof (EscrowFloodInsuranceDue), 0.0);
      this.escrowHazardInsuranceDue = attributeReader.GetDouble(nameof (EscrowHazardInsuranceDue), 0.0);
      this.escrowOther1Due = attributeReader.GetDouble(nameof (EscrowOther1Due), 0.0);
      this.escrowOther2Due = attributeReader.GetDouble(nameof (EscrowOther2Due), 0.0);
      this.escrowOther3Due = attributeReader.GetDouble(nameof (EscrowOther3Due), 0.0);
      this.escrowCityPropertyTaxDue = attributeReader.GetDouble(nameof (EscrowCityPropertyTaxDue), 0.0);
      this.escrowUSDAMonthlyPremiumDue = attributeReader.GetDouble(nameof (EscrowUSDAMonthlyPremiumDue), 0.0);
    }

    public override void Add(XmlElement newlog, bool use5DecimalsForIndexRates)
    {
      base.Add(newlog, use5DecimalsForIndexRates);
      newlog.SetAttribute("PaymentNo", this.paymentNo.ToString());
      newlog.SetAttribute("Type", ServicingTransactionTypes.SchedulePayment.ToString());
      if (this.latePaymentDate != DateTime.MinValue)
        newlog.SetAttribute("LatePaymentDate", this.latePaymentDate.ToString("MM/dd/yyyy"));
      else
        newlog.SetAttribute("LatePaymentDate", "");
      newlog.SetAttribute("IndexRate", use5DecimalsForIndexRates ? this.indexRate.ToString("N5") : this.indexRate.ToString("N3"));
      newlog.SetAttribute("InterestRate", this.interestRate.ToString("N3"));
      newlog.SetAttribute("PrincipalDue", this.principalDue.ToString("N2"));
      newlog.SetAttribute("InterestDue", this.interestDue.ToString("N2"));
      newlog.SetAttribute("EscrowDue", this.escrowDue.ToString("N2"));
      newlog.SetAttribute("BuydownSubsidyAmountDue", this.buydownSubsidyAmountDue.ToString("N2"));
      newlog.SetAttribute("MiscFeeDue", this.miscFeeDue.ToString("N2"));
      newlog.SetAttribute("TotalPastDue", this.totalPastDue.ToString("N2"));
      newlog.SetAttribute("UnpaidLateFeeDue", this.unpaidLateFeeDue.ToString("N2"));
      newlog.SetAttribute("Taxes", this.taxes.ToString("N2"));
      newlog.SetAttribute("HazardInsurance", this.hazardInsurance.ToString("N2"));
      newlog.SetAttribute("MortgageInsurance", this.mortgageInsurance.ToString("N2"));
      newlog.SetAttribute("FloodInsurance", this.floodInsurance.ToString("N2"));
      newlog.SetAttribute("CityPropertyTax", this.citypropertytax.ToString("N2"));
      newlog.SetAttribute("Other1Escrow", this.other1Escrow.ToString("N2"));
      newlog.SetAttribute("Other2Escrow", this.other2Escrow.ToString("N2"));
      newlog.SetAttribute("Other3Escrow", this.other3Escrow.ToString("N2"));
      newlog.SetAttribute("USDAMonthlyPremium", this.usdamonthlyPremium.ToString("N2"));
      newlog.SetAttribute("EscrowTaxDue", this.escrowTaxDue.ToString("N2"));
      newlog.SetAttribute("EscrowMortgageInsuranceDue", this.escrowMortgageInsuranceDue.ToString("N2"));
      newlog.SetAttribute("EscrowHazardInsuranceDue", this.escrowHazardInsuranceDue.ToString("N2"));
      newlog.SetAttribute("EscrowFloodInsuranceDue", this.escrowFloodInsuranceDue.ToString("N2"));
      newlog.SetAttribute("EscrowOther1Due", this.escrowOther1Due.ToString("N2"));
      newlog.SetAttribute("EscrowOther2Due", this.escrowOther2Due.ToString("N2"));
      newlog.SetAttribute("EscrowOther3Due", this.escrowOther3Due.ToString("N2"));
      newlog.SetAttribute("EscrowUSDAMonthlyPremiumDue", this.escrowUSDAMonthlyPremiumDue.ToString("N2"));
      newlog.SetAttribute("EscrowCityPropertyTaxDue", this.escrowCityPropertyTaxDue.ToString("N2"));
    }

    public void ClearReceivedAmount()
    {
      this.interest = 0.0;
      this.principal = 0.0;
      this.escrow = 0.0;
      this.miscFee = 0.0;
      this.lateFee = 0.0;
      this.taxes = 0.0;
      this.hazardInsurance = 0.0;
      this.mortgageInsurance = 0.0;
      this.floodInsurance = 0.0;
      this.citypropertytax = 0.0;
      this.other1Escrow = 0.0;
      this.other2Escrow = 0.0;
      this.other3Escrow = 0.0;
      this.usdamonthlyPremium = 0.0;
    }

    public double TotalPayment => this.interest + this.principal + this.escrow + this.miscFee;

    public double TotalPaymentDue
    {
      get => this.interestDue + this.principalDue + this.escrowDue + this.miscFeeDue;
    }

    public double PaymentDifference
    {
      get
      {
        return this.interestDue - this.interest + (this.principalDue - this.principal) + (this.escrowDue - this.escrow) + (this.miscFeeDue - this.miscFee);
      }
    }

    public int PaymentNo
    {
      get => this.paymentNo;
      set => this.paymentNo = value;
    }

    public DateTime LatePaymentDate
    {
      get => this.latePaymentDate;
      set => this.latePaymentDate = value;
    }

    public DateTime PaymentReceivedDate
    {
      get => this.paymentReceivedDate;
      set => this.paymentReceivedDate = value;
    }

    public double IndexRate
    {
      get => this.indexRate;
      set => this.indexRate = value;
    }

    public double InterestRate
    {
      get => this.interestRate;
      set => this.interestRate = value;
    }

    public double PrincipalDue
    {
      get => this.principalDue;
      set => this.principalDue = value;
    }

    public double InterestDue
    {
      get => this.interestDue;
      set => this.interestDue = value;
    }

    public double EscrowDue
    {
      get => this.escrowDue;
      set => this.escrowDue = value;
    }

    public double BuydownSubsidyAmountDue
    {
      get => this.buydownSubsidyAmountDue;
      set => this.buydownSubsidyAmountDue = value;
    }

    public double MiscFeeDue
    {
      get => this.miscFeeDue;
      set => this.miscFeeDue = value;
    }

    public double UnpaidLateFeeDue
    {
      get => this.unpaidLateFeeDue;
      set => this.unpaidLateFeeDue = value;
    }

    public double TotalPastDue
    {
      get => this.totalPastDue;
      set => this.totalPastDue = value;
    }

    public double Principal
    {
      get => this.principal;
      set => this.principal = value;
    }

    public double Interest
    {
      get => this.interest;
      set => this.interest = value;
    }

    public double Escrow
    {
      get => this.escrow;
      set => this.escrow = value;
    }

    public double BuydownSubsidyAmount
    {
      get => this.buydownSubsidyAmount;
      set => this.buydownSubsidyAmount = value;
    }

    public double MiscFee
    {
      get => this.miscFee;
      set => this.miscFee = value;
    }

    public double LateFee
    {
      get => this.lateFee;
      set => this.lateFee = value;
    }

    public double AdditionalPrincipal
    {
      get => this.additionalPrincipal;
      set => this.additionalPrincipal = value;
    }

    public double AdditionalEscrow
    {
      get => this.additionalEscrow;
      set => this.additionalEscrow = value;
    }

    public double Taxes
    {
      get => this.taxes;
      set => this.taxes = value;
    }

    public double HazardInsurance
    {
      get => this.hazardInsurance;
      set => this.hazardInsurance = value;
    }

    public double MortgageInsurance
    {
      get => this.mortgageInsurance;
      set => this.mortgageInsurance = value;
    }

    public double FloodInsurance
    {
      get => this.floodInsurance;
      set => this.floodInsurance = value;
    }

    public double CityPropertytax
    {
      get => this.citypropertytax;
      set => this.citypropertytax = value;
    }

    public double Other1Escrow
    {
      get => this.other1Escrow;
      set => this.other1Escrow = value;
    }

    public double Other2Escrow
    {
      get => this.other2Escrow;
      set => this.other2Escrow = value;
    }

    public double Other3Escrow
    {
      get => this.other3Escrow;
      set => this.other3Escrow = value;
    }

    public double USDAMonthlyPremium
    {
      get => this.usdamonthlyPremium;
      set => this.usdamonthlyPremium = value;
    }

    public double EscrowTaxDue
    {
      get => this.escrowTaxDue;
      set => this.escrowTaxDue = value;
    }

    public double EscrowMortgageInsuranceDue
    {
      get => this.escrowMortgageInsuranceDue;
      set => this.escrowMortgageInsuranceDue = value;
    }

    public double EscrowFloodInsuranceDue
    {
      get => this.escrowFloodInsuranceDue;
      set => this.escrowFloodInsuranceDue = value;
    }

    public double EscrowHazardInsuranceDue
    {
      get => this.escrowHazardInsuranceDue;
      set => this.escrowHazardInsuranceDue = value;
    }

    public double EscrowCityPropertyTaxDue
    {
      get => this.escrowCityPropertyTaxDue;
      set => this.escrowCityPropertyTaxDue = value;
    }

    public double EscrowOther1Due
    {
      get => this.escrowOther1Due;
      set => this.escrowOther1Due = value;
    }

    public double EscrowOther2Due
    {
      get => this.escrowOther2Due;
      set => this.escrowOther2Due = value;
    }

    public double EscrowOther3Due
    {
      get => this.escrowOther3Due;
      set => this.escrowOther3Due = value;
    }

    public double EscrowUSDAMonthlyPremiumDue
    {
      get => this.escrowUSDAMonthlyPremiumDue;
      set => this.escrowUSDAMonthlyPremiumDue = value;
    }
  }
}

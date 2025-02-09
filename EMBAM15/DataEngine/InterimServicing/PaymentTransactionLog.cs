// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.PaymentTransactionLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class PaymentTransactionLog : ServicingTransactionBase
  {
    private int paymentNo;
    private DateTime paymentIndexDate = DateTime.MinValue;
    private DateTime statementDate = DateTime.MinValue;
    private DateTime paymentDueDate = DateTime.MinValue;
    private DateTime latePaymentDate = DateTime.MinValue;
    private DateTime paymentReceivedDate = DateTime.MinValue;
    private DateTime paymentDepositedDate = DateTime.MinValue;
    private double indexRate;
    private double interestRate;
    private double totalAmountDue;
    private double principal;
    private double interest;
    private double escrow;
    private double buydownSubsidyAmount;
    private double lateFee;
    private double miscFee;
    private double additionalPrincipal;
    private double additionalEscrow;
    private double lateFeeIfLate;
    private string institutionName = string.Empty;
    private string institutionRouting = string.Empty;
    private string accountNumber = string.Empty;
    private string accountHolder = string.Empty;
    private string reference = string.Empty;
    private string comments = string.Empty;
    private string checkNumber = string.Empty;
    private double commonAmount;
    private DateTime commonDate = DateTime.MinValue;
    private double schedulePayLogMiscFee;
    private double escrowTaxes;
    private double harzardInsurance;
    private double mortgageInsurance;
    private double floodInsurance;
    private double cityPropertyTax;
    private double other1Escrow;
    private double other2Escrow;
    private double other3Escrow;
    private double usdaMonthlyPremium;

    public PaymentTransactionLog() => this.TransactionType = ServicingTransactionTypes.Payment;

    public PaymentTransactionLog(XmlElement e)
      : base(e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.paymentNo = attributeReader.GetInteger(nameof (PaymentNo));
      this.paymentIndexDate = attributeReader.GetDate(nameof (PaymentIndexDate));
      this.statementDate = attributeReader.GetDate(nameof (StatementDate));
      this.paymentDueDate = attributeReader.GetDate(nameof (PaymentDueDate));
      this.latePaymentDate = attributeReader.GetDate(nameof (LatePaymentDate));
      this.paymentReceivedDate = attributeReader.GetDate(nameof (PaymentReceivedDate));
      this.paymentDepositedDate = attributeReader.GetDate(nameof (PaymentDepositedDate));
      this.indexRate = attributeReader.GetDouble(nameof (IndexRate), 0.0);
      this.interestRate = attributeReader.GetDouble(nameof (InterestRate), 0.0);
      this.totalAmountDue = attributeReader.GetDouble(nameof (TotalAmountDue), 0.0);
      this.PaymentMethod = (ServicingPaymentMethods) ServicingEnum.ToEnum(attributeReader.GetString("PaymentMethod").Replace(" ", ""), typeof (ServicingPaymentMethods));
      this.principal = attributeReader.GetDouble(nameof (Principal), 0.0);
      this.interest = attributeReader.GetDouble(nameof (Interest), 0.0);
      this.escrow = attributeReader.GetDouble(nameof (Escrow), 0.0);
      this.buydownSubsidyAmount = attributeReader.GetDouble(nameof (BuydownSubsidyAmount), 0.0);
      this.lateFee = attributeReader.GetDouble(nameof (LateFee), 0.0);
      this.lateFeeIfLate = attributeReader.GetDouble(nameof (LateFeeIfLate), 0.0);
      this.miscFee = attributeReader.GetDouble(nameof (MiscFee), 0.0);
      this.additionalPrincipal = attributeReader.GetDouble(nameof (AdditionalPrincipal), 0.0);
      this.additionalEscrow = attributeReader.GetDouble(nameof (AdditionalEscrow), 0.0);
      this.institutionName = attributeReader.GetString(nameof (InstitutionName));
      this.institutionRouting = attributeReader.GetString(nameof (InstitutionRouting));
      this.accountNumber = attributeReader.GetString(nameof (AccountNumber));
      this.accountHolder = attributeReader.GetString(nameof (AccountHolder));
      this.reference = attributeReader.GetString(nameof (Reference));
      this.comments = attributeReader.GetString(nameof (Comments));
      this.checkNumber = attributeReader.GetString(nameof (CheckNumber));
      this.commonAmount = attributeReader.GetDouble("CommonAmount", 0.0);
      this.commonDate = attributeReader.GetDate("CommonDate");
      this.schedulePayLogMiscFee = attributeReader.GetDouble(nameof (SchedulePayLogMiscFee), 0.0);
      this.escrowTaxes = attributeReader.GetDouble("EscrowTaxes", 0.0);
      this.harzardInsurance = attributeReader.GetDouble(nameof (HazardInsurance), 0.0);
      this.mortgageInsurance = attributeReader.GetDouble(nameof (MortgageInsurance), 0.0);
      this.floodInsurance = attributeReader.GetDouble(nameof (FloodInsurance), 0.0);
      this.cityPropertyTax = attributeReader.GetDouble(nameof (CityPropertyTax), 0.0);
      this.other1Escrow = attributeReader.GetDouble(nameof (Other1Escrow), 0.0);
      this.other2Escrow = attributeReader.GetDouble(nameof (Other2Escrow), 0.0);
      this.other3Escrow = attributeReader.GetDouble(nameof (Other3Escrow), 0.0);
      this.usdaMonthlyPremium = attributeReader.GetDouble(nameof (USDAMonthlyPremium), 0.0);
    }

    public override void Add(XmlElement newlog, bool use5DecimalsForIndexRates)
    {
      base.Add(newlog, use5DecimalsForIndexRates);
      newlog.SetAttribute("PaymentNo", this.paymentNo.ToString());
      newlog.SetAttribute("Type", ServicingTransactionTypes.Payment.ToString());
      if (this.paymentIndexDate != DateTime.MinValue)
        newlog.SetAttribute("PaymentIndexDate", this.paymentIndexDate.ToString("MM/dd/yyyy"));
      else
        newlog.SetAttribute("PaymentIndexDate", "");
      if (this.statementDate != DateTime.MinValue)
        newlog.SetAttribute("StatementDate", this.statementDate.ToString("MM/dd/yyyy"));
      else
        newlog.SetAttribute("StatementDate", "");
      if (this.paymentDueDate != DateTime.MinValue)
        newlog.SetAttribute("PaymentDueDate", this.paymentDueDate.ToString("MM/dd/yyyy"));
      else
        newlog.SetAttribute("PaymentDueDate", "");
      if (this.latePaymentDate != DateTime.MinValue)
        newlog.SetAttribute("LatePaymentDate", this.latePaymentDate.ToString("MM/dd/yyyy"));
      else
        newlog.SetAttribute("LatePaymentDate", "");
      if (this.paymentReceivedDate != DateTime.MinValue)
        newlog.SetAttribute("PaymentReceivedDate", this.paymentReceivedDate.ToString("MM/dd/yyyy"));
      else
        newlog.SetAttribute("PaymentReceivedDate", "");
      if (this.paymentDepositedDate != DateTime.MinValue)
        newlog.SetAttribute("PaymentDepositedDate", this.paymentDepositedDate.ToString("MM/dd/yyyy"));
      else
        newlog.SetAttribute("PaymentDepositedDate", "");
      newlog.SetAttribute("IndexRate", use5DecimalsForIndexRates ? this.indexRate.ToString("N5") : this.indexRate.ToString("N3"));
      newlog.SetAttribute("InterestRate", this.interestRate.ToString("N3"));
      newlog.SetAttribute("TotalAmountDue", this.totalAmountDue.ToString("N2"));
      newlog.SetAttribute("PaymentMethod", ServicingEnum.ServicingPaymentMethodsToUI(this.PaymentMethod));
      newlog.SetAttribute("Principal", this.principal.ToString("N2"));
      newlog.SetAttribute("Interest", this.interest.ToString("N2"));
      newlog.SetAttribute("Escrow", this.escrow.ToString("N2"));
      newlog.SetAttribute("BuydownSubsidyAmount", this.buydownSubsidyAmount.ToString("N2"));
      newlog.SetAttribute("LateFee", this.lateFee.ToString("N2"));
      newlog.SetAttribute("LateFeeIfLate", this.lateFeeIfLate.ToString("N2"));
      newlog.SetAttribute("MiscFee", this.miscFee.ToString("N2"));
      newlog.SetAttribute("AdditionalPrincipal", this.additionalPrincipal.ToString("N2"));
      newlog.SetAttribute("AdditionalEscrow", this.additionalEscrow.ToString("N2"));
      newlog.SetAttribute("InstitutionName", this.institutionName);
      newlog.SetAttribute("InstitutionRouting", this.institutionRouting);
      newlog.SetAttribute("AccountNumber", this.accountNumber);
      newlog.SetAttribute("AccountHolder", this.accountHolder);
      newlog.SetAttribute("Reference", this.reference);
      newlog.SetAttribute("Comments", this.comments);
      newlog.SetAttribute("CheckNumber", this.checkNumber);
      newlog.SetAttribute("CommonAmount", this.commonAmount.ToString("N2"));
      if (this.commonDate != DateTime.MinValue)
        newlog.SetAttribute("CommonDate", this.commonDate.ToString("MM/dd/yyyy"));
      else
        newlog.SetAttribute("CommonDate", "");
      newlog.SetAttribute("SchedulePayLogMiscFee", this.schedulePayLogMiscFee.ToString("N2"));
      newlog.SetAttribute("EscrowTaxes", this.escrowTaxes.ToString("N2"));
      newlog.SetAttribute("HazardInsurance", this.harzardInsurance.ToString("N2"));
      newlog.SetAttribute("MortgageInsurance", this.mortgageInsurance.ToString("N2"));
      newlog.SetAttribute("FloodInsurance", this.floodInsurance.ToString("N2"));
      newlog.SetAttribute("CityPropertyTax", this.cityPropertyTax.ToString("N2"));
      newlog.SetAttribute("Other1Escrow", this.other1Escrow.ToString("N2"));
      newlog.SetAttribute("Other2Escrow", this.other2Escrow.ToString("N2"));
      newlog.SetAttribute("Other3Escrow", this.other3Escrow.ToString("N2"));
      newlog.SetAttribute("USDAMonthlyPremium", this.usdaMonthlyPremium.ToString("N2"));
    }

    public string GetField(string id, bool use5DecimalsForIndexRates = false)
    {
      switch (id)
      {
        case "AccountHolder":
          return this.toString((object) this.accountHolder);
        case "AccountNumber":
          return this.toString((object) this.accountNumber);
        case "AdditionalEscrow":
          return this.toString((object) this.additionalEscrow);
        case "AdditionalPrincipal":
          return this.toString((object) this.additionalPrincipal);
        case "BuydownSubsidyAmount":
          return this.toString((object) this.buydownSubsidyAmount);
        case "CheckAmount":
          return this.toString((object) this.CheckAmount);
        case "CheckDate":
          return this.toString((object) this.CheckDate);
        case "CheckNumber":
          return this.toString((object) this.checkNumber);
        case "CityPropertyTax":
          return this.toString((object) this.cityPropertyTax);
        case "Comments":
          return this.toString((object) this.comments);
        case "CreditDate":
          return this.toString((object) this.CreditDate);
        case "Escrow":
          return this.toString((object) this.escrow);
        case "EscrowTaxes":
          return this.toString((object) this.escrowTaxes);
        case "FloodInsurance":
          return this.toString((object) this.floodInsurance);
        case "HazardInsurance":
          return this.toString((object) this.harzardInsurance);
        case "IndexRate":
          if (this.indexRate == 0.0)
            return string.Empty;
          return !use5DecimalsForIndexRates ? this.indexRate.ToString("N3") : this.indexRate.ToString("N5");
        case "InstitutionName":
          return this.toString((object) this.institutionName);
        case "InstitutionRouting":
          return this.toString((object) this.institutionRouting);
        case "Interest":
          return this.toString((object) this.interest);
        case "InterestRate":
          return this.interestRate != 0.0 ? this.interestRate.ToString("N3") : string.Empty;
        case "LateFee":
          return this.toString((object) this.lateFee);
        case "LateFeeIfLate":
          return this.toString((object) this.lateFeeIfLate);
        case "LatePaymentDate":
          return this.toString((object) this.latePaymentDate);
        case "LockBoxAmount":
          return this.toString((object) this.LockBoxAmount);
        case "MiscFee":
          return this.toString((object) this.miscFee);
        case "MortgageInsurance":
          return this.toString((object) this.mortgageInsurance);
        case "Other1Escrow":
          return this.toString((object) this.other1Escrow);
        case "Other2Escrow":
          return this.toString((object) this.other2Escrow);
        case "Other3Escrow":
          return this.toString((object) this.other3Escrow);
        case "PaymentDepositedDate":
          return this.toString((object) this.paymentDepositedDate);
        case "PaymentDueDate":
          return this.toString((object) this.paymentDueDate);
        case "PaymentIndexDate":
          return this.toString((object) this.paymentIndexDate);
        case "PaymentMethod":
          return this.toString((object) this.PaymentMethod);
        case "PaymentNo":
          return this.toString((object) this.paymentNo);
        case "PaymentReceivedDate":
          return this.toString((object) this.paymentReceivedDate);
        case "Principal":
          return this.toString((object) this.principal);
        case "Reference":
          return this.toString((object) this.reference);
        case "SchedulePayLogMiscFee":
          return this.toString((object) this.schedulePayLogMiscFee);
        case "StatementDate":
          return this.toString((object) this.statementDate);
        case "TotalAmountDue":
          return this.toString((object) this.totalAmountDue);
        case "TotalAmountReceived":
          return this.toString((object) this.TotalAmountReceived);
        case "TransactionAmount":
          return this.toString((object) this.TransactionAmount);
        case "TransactionDate":
          return this.toString((object) this.TransactionDate);
        case "USDAMonthlyPremium":
          return this.toString((object) this.usdaMonthlyPremium);
        case "WireAmount":
          return this.toString((object) this.WireAmount);
        case "WireDate":
          return this.toString((object) this.WireDate);
        default:
          return string.Empty;
      }
    }

    private string toString(object o)
    {
      switch (o)
      {
        case DateTime dateTime:
          return dateTime == DateTime.MinValue ? string.Empty : dateTime.ToString("MM/dd/yyyy");
        case double num1:
          return num1 != 0.0 ? num1.ToString("N2") : string.Empty;
        case int num2:
          return num2 != 0 ? num2.ToString() : string.Empty;
        case ServicingPaymentMethods _:
          return o.ToString();
        default:
          return (string) o;
      }
    }

    public void SetField(string id, string val)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(id))
      {
        case 79151362:
          if (!(id == "PaymentReceivedDate"))
            return;
          this.paymentReceivedDate = Utils.ParseDate((object) val);
          return;
        case 157644703:
          if (!(id == "LateFee"))
            return;
          this.lateFee = Utils.ParseDouble((object) val);
          return;
        case 249996928:
          if (!(id == "CreditDate"))
            return;
          this.CreditDate = Utils.ParseDate((object) val);
          return;
        case 310626857:
          if (!(id == "InstitutionRouting"))
            return;
          this.institutionRouting = val;
          return;
        case 374419415:
          if (!(id == "TransactionDate"))
            return;
          this.TransactionDate = Utils.ParseDate((object) val);
          return;
        case 702341521:
          if (!(id == "TotalAmountDue"))
            return;
          this.totalAmountDue = Utils.ParseDouble((object) val);
          return;
        case 803173271:
          if (!(id == "MiscFee"))
            return;
          this.miscFee = Utils.ParseDouble((object) val);
          return;
        case 835748201:
          if (!(id == "Other3Escrow"))
            return;
          this.other3Escrow = Utils.ParseDouble((object) val);
          return;
        case 861437700:
          if (!(id == "WireAmount"))
            return;
          break;
        case 909384029:
          if (!(id == "FloodInsurance"))
            return;
          this.floodInsurance = Utils.ParseDouble((object) val);
          return;
        case 916472355:
          if (!(id == "PaymentIndexDate"))
            return;
          this.paymentIndexDate = Utils.ParseDate((object) val);
          return;
        case 927245734:
          if (!(id == "InstitutionName"))
            return;
          this.institutionName = val;
          return;
        case 976305171:
          if (!(id == "EscrowTaxes"))
            return;
          this.escrowTaxes = Utils.ParseDouble((object) val);
          return;
        case 1148703409:
          if (!(id == "IndexRate"))
            return;
          this.indexRate = Utils.ParseDouble((object) val);
          return;
        case 1381509879:
          if (!(id == "Comments"))
            return;
          this.comments = val;
          return;
        case 1435381961:
          if (!(id == "Principal"))
            return;
          this.principal = Utils.ParseDouble((object) val);
          return;
        case 1718303712:
          if (!(id == "LateFeeIfLate"))
            return;
          this.lateFeeIfLate = Utils.ParseDouble((object) val);
          return;
        case 1845105833:
          if (!(id == "LatePaymentDate"))
            return;
          this.latePaymentDate = Utils.ParseDate((object) val);
          return;
        case 1905938285:
          if (!(id == "AccountNumber"))
            return;
          this.accountNumber = val;
          return;
        case 1975382340:
          if (!(id == "PaymentDepositedDate"))
            return;
          this.paymentDepositedDate = Utils.ParseDate((object) val);
          return;
        case 2528463075:
          if (!(id == "CheckDate"))
            return;
          this.CheckDate = Utils.ParseDate((object) val);
          return;
        case 2697531732:
          if (!(id == "USDAMonthlyPremium"))
            return;
          this.usdaMonthlyPremium = Utils.ParseDouble((object) val);
          return;
        case 2733925462:
          if (!(id == "AdditionalPrincipal"))
            return;
          this.additionalPrincipal = Utils.ParseDouble((object) val);
          return;
        case 2775468747:
          if (!(id == "LockBoxAmount"))
            return;
          break;
        case 2887527207:
          if (!(id == "TransactionAmount"))
            return;
          this.TransactionAmount = Utils.ParseDouble((object) val);
          return;
        case 2915615546:
          if (!(id == "Reference"))
            return;
          this.reference = val;
          return;
        case 2951216050:
          if (!(id == "Escrow"))
            return;
          this.escrow = Utils.ParseDouble((object) val);
          return;
        case 2980399535:
          if (!(id == "HazardInsurance"))
            return;
          this.harzardInsurance = Utils.ParseDouble((object) val);
          return;
        case 3015575336:
          if (!(id == "BuydownSubsidyAmount"))
            return;
          this.buydownSubsidyAmount = Utils.ParseDouble((object) val);
          return;
        case 3067078256:
          if (!(id == "CityPropertyTax"))
            return;
          this.cityPropertyTax = Utils.ParseDouble((object) val);
          return;
        case 3074641908:
          if (!(id == "PaymentMethod"))
            return;
          this.PaymentMethod = ServicingPaymentMethods.None;
          val = val.Replace(" ", "");
          if (!(val != string.Empty))
            return;
          this.PaymentMethod = (ServicingPaymentMethods) ServicingEnum.ToEnum(val, typeof (ServicingPaymentMethods));
          return;
        case 3221228233:
          if (!(id == "MortgageInsurance"))
            return;
          this.mortgageInsurance = Utils.ParseDouble((object) val);
          return;
        case 3327971020:
          if (!(id == "WireDate"))
            return;
          this.WireDate = Utils.ParseDate((object) val);
          return;
        case 3414596986:
          if (!(id == "AccountHolder"))
            return;
          this.accountHolder = val;
          return;
        case 3442403398:
          if (!(id == "CheckNumber"))
            return;
          this.checkNumber = val;
          return;
        case 3521551302:
          if (!(id == "PaymentNo"))
            return;
          this.paymentNo = Utils.ParseInt((object) val);
          return;
        case 3532622067:
          if (!(id == "Interest"))
            return;
          this.interest = Utils.ParseDouble((object) val);
          return;
        case 3749953183:
          if (!(id == "AdditionalEscrow"))
            return;
          this.additionalEscrow = Utils.ParseDouble((object) val);
          return;
        case 3828392066:
          if (!(id == "StatementDate"))
            return;
          this.statementDate = Utils.ParseDate((object) val);
          return;
        case 3897349010:
          if (!(id == "Other2Escrow"))
            return;
          this.other2Escrow = Utils.ParseDouble((object) val);
          return;
        case 3907614553:
          if (!(id == "InterestRate"))
            return;
          this.interestRate = Utils.ParseDouble((object) val);
          return;
        case 3965610774:
          if (!(id == "TotalAmountReceived"))
            return;
          this.TotalAmountReceived = Utils.ParseDouble((object) val);
          return;
        case 4039953389:
          if (!(id == "PaymentDueDate"))
            return;
          this.paymentDueDate = Utils.ParseDate((object) val);
          return;
        case 4170675683:
          if (!(id == "CheckAmount"))
            return;
          break;
        case 4259536559:
          if (!(id == "Other1Escrow"))
            return;
          this.other1Escrow = Utils.ParseDouble((object) val);
          return;
        default:
          return;
      }
      double num = Utils.ParseDouble((object) val);
      this.commonAmount = num;
      this.TransactionAmount = num;
    }

    public int PaymentNo
    {
      get => this.paymentNo;
      set => this.paymentNo = value;
    }

    public DateTime PaymentIndexDate
    {
      get => this.paymentIndexDate;
      set => this.paymentIndexDate = value;
    }

    public DateTime StatementDate
    {
      get => this.statementDate;
      set => this.statementDate = value;
    }

    public DateTime PaymentDueDate
    {
      get => this.paymentDueDate;
      set => this.paymentDueDate = value;
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

    public DateTime PaymentDepositedDate
    {
      get => this.paymentDepositedDate;
      set => this.paymentDepositedDate = value;
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

    public double TotalAmountDue
    {
      get => this.totalAmountDue;
      set => this.totalAmountDue = value;
    }

    public double TotalAmountReceived
    {
      get => this.TransactionAmount;
      set => this.TransactionAmount = value;
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

    public double LateFee
    {
      get => this.lateFee;
      set => this.lateFee = value;
    }

    public double MiscFee
    {
      get => this.miscFee;
      set => this.miscFee = value;
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

    public double LateFeeIfLate
    {
      get => this.lateFeeIfLate;
      set => this.lateFeeIfLate = value;
    }

    public string InstitutionName
    {
      get => this.institutionName;
      set => this.institutionName = value;
    }

    public string InstitutionRouting
    {
      get => this.institutionRouting;
      set => this.institutionRouting = value;
    }

    public string AccountNumber
    {
      get => this.accountNumber;
      set => this.accountNumber = value;
    }

    public string AccountHolder
    {
      get => this.accountHolder;
      set => this.accountHolder = value;
    }

    public string Reference
    {
      get => this.reference;
      set => this.reference = value;
    }

    public string Comments
    {
      get => this.comments;
      set => this.comments = value;
    }

    public string CheckNumber
    {
      get => this.checkNumber;
      set => this.checkNumber = value;
    }

    public double WireAmount
    {
      get => this.commonAmount;
      set => this.commonAmount = value;
    }

    public double LockBoxAmount
    {
      get => this.commonAmount;
      set => this.commonAmount = value;
    }

    public double CheckAmount
    {
      get => this.commonAmount;
      set => this.commonAmount = value;
    }

    public DateTime WireDate
    {
      get => this.commonDate;
      set => this.commonDate = value;
    }

    public DateTime CreditDate
    {
      get => this.commonDate;
      set => this.commonDate = value;
    }

    public DateTime CheckDate
    {
      get => this.commonDate;
      set => this.commonDate = value;
    }

    public double SchedulePayLogMiscFee
    {
      get => this.schedulePayLogMiscFee;
      set => this.schedulePayLogMiscFee = value;
    }

    public double EscowTaxes
    {
      get => this.escrowTaxes;
      set => this.escrowTaxes = value;
    }

    public double HazardInsurance
    {
      get => this.harzardInsurance;
      set => this.harzardInsurance = value;
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

    public double CityPropertyTax
    {
      get => this.cityPropertyTax;
      set => this.cityPropertyTax = value;
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
      get => this.usdaMonthlyPremium;
      set => this.usdaMonthlyPremium = value;
    }
  }
}

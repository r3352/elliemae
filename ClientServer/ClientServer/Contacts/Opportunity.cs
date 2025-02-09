// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.Opportunity
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using System;
using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class Opportunity : IPropertyDictionary
  {
    private int opportunityId = -1;
    private int contactId = -1;
    private Decimal loanAmount;
    private Decimal cashOut;
    private EllieMae.EMLite.Common.Contact.LoanPurpose purpose;
    private string purposeOther = "";
    private LoanTypeEnum loanType;
    private string typeOther = "";
    private string estimatedCreditScore = "";
    private int term = -1;
    private AmortizationType amortization;
    private Decimal downPayment;
    private Address propertyAddress = new Address();
    private PropertyUse propertyUse;
    private EllieMae.EMLite.Common.Contact.PropertyType propertyType;
    private Decimal propertyValue;
    private DateTime purchaseDate;
    private Decimal mortgageBalance;
    private Decimal mortgageRate;
    private Decimal housingPayment;
    private Decimal nonhousingPayment;
    private string creditRating = "";
    private bool isBankruptcy;
    private EmploymentStatus employment;

    public Opportunity(
      int opportunityId,
      int contactId,
      Decimal loanAmount,
      EllieMae.EMLite.Common.Contact.LoanPurpose purpose,
      string purposeOther,
      int term,
      AmortizationType amortization,
      Decimal downPayment,
      Address propertyAddress,
      PropertyUse propertyUse,
      EllieMae.EMLite.Common.Contact.PropertyType propertyType,
      Decimal propertyValue,
      DateTime purchaseDate,
      Decimal mortgageBalance,
      Decimal mortgageRate,
      Decimal housingPayment,
      Decimal nonhousingPayment,
      string creditRating,
      bool isBankruptcy,
      EmploymentStatus employment,
      Decimal cashOut,
      LoanTypeEnum loanType,
      string typeOther,
      string estimatedCreditScore)
    {
      this.opportunityId = opportunityId;
      this.contactId = contactId;
      this.loanAmount = loanAmount;
      this.purpose = purpose;
      this.purposeOther = purposeOther;
      this.term = term;
      this.amortization = amortization;
      this.downPayment = downPayment;
      this.propertyAddress = propertyAddress == null ? new Address() : propertyAddress;
      this.propertyUse = propertyUse;
      this.propertyType = propertyType;
      this.propertyValue = propertyValue;
      this.purchaseDate = purchaseDate;
      this.mortgageBalance = mortgageBalance;
      this.mortgageRate = mortgageRate;
      this.housingPayment = housingPayment;
      this.nonhousingPayment = nonhousingPayment;
      this.creditRating = creditRating;
      this.isBankruptcy = isBankruptcy;
      this.employment = employment;
      this.cashOut = cashOut;
      this.loanType = loanType;
      this.typeOther = typeOther;
      this.estimatedCreditScore = estimatedCreditScore;
    }

    public Opportunity()
    {
    }

    public object this[string columnName]
    {
      get
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "amortization":
            return (object) this.amortization;
          case "bankruptcy":
            return (object) this.isBankruptcy;
          case "cashout":
            return (object) this.cashOut;
          case "contactid":
            return (object) this.contactId;
          case "creditrating":
            return (object) this.creditRating;
          case "downpayment":
            return (object) this.downPayment;
          case "employment":
            return (object) this.employment;
          case "estimatedcreditscore":
            return (object) this.estimatedCreditScore;
          case "housingpayment":
            return (object) this.housingPayment;
          case "loanamount":
            return (object) this.loanAmount;
          case "loantype":
            return (object) this.loanType;
          case "mortgagebalance":
            return (object) this.mortgageBalance;
          case "mortgagerate":
            return (object) this.mortgageRate;
          case "nonhousingpayment":
            return (object) this.nonhousingPayment;
          case "opportunityid":
            return (object) this.opportunityId;
          case "propertyaddress":
            return (object) this.propertyAddress.Street1;
          case "propertycity":
            return (object) this.propertyAddress.City;
          case "propertystate":
            return (object) this.propertyAddress.State;
          case "propertytype":
            return (object) this.propertyType;
          case "propertyuse":
            return (object) this.propertyUse;
          case "propertyvalue":
            return (object) this.propertyValue;
          case "propertyzip":
            return (object) this.propertyAddress.Zip;
          case "purchasedate":
            return (object) this.purchaseDate;
          case "purpose":
            return (object) this.purpose;
          case "purposeother":
            return (object) this.purposeOther;
          case "term":
            return (object) this.term;
          case "typeOther":
            return (object) this.typeOther;
          default:
            return (object) null;
        }
      }
      set
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "amortization":
            if (value is string)
            {
              this.amortization = AmortizationTypeEnumUtil.NameToValue(string.Concat(value));
              break;
            }
            this.amortization = (AmortizationType) value;
            break;
          case "bankruptcy":
            this.isBankruptcy = Utils.ParseBoolean(value);
            break;
          case "cashout":
            this.cashOut = Utils.ParseDecimal(value);
            break;
          case "creditrating":
            this.creditRating = string.Concat(value);
            break;
          case "downpayment":
            this.downPayment = Utils.ParseDecimal(value);
            break;
          case "employment":
            if (value is string)
            {
              this.employment = EmploymentStatusEnumUtil.NameToValue(string.Concat(value));
              break;
            }
            this.employment = (EmploymentStatus) value;
            break;
          case "estimatedcreditscore":
            this.estimatedCreditScore = Convert.ToString(value);
            break;
          case "housingpayment":
            this.housingPayment = Utils.ParseDecimal(value);
            break;
          case "loanamount":
            this.loanAmount = Utils.ParseDecimal(value);
            break;
          case "loantype":
            if (value is string)
            {
              this.loanType = LoanTypeEnumUtil.NameToValue(string.Concat(value));
              if (this.loanType != LoanTypeEnum.Blank || !(string.Concat(value) != ""))
                break;
              this.loanType = LoanTypeEnum.Other;
              this.typeOther = string.Concat(value);
              break;
            }
            this.loanType = (LoanTypeEnum) value;
            break;
          case "mortgagebalance":
            this.mortgageBalance = Utils.ParseDecimal(value);
            break;
          case "mortgagerate":
            this.mortgageRate = Utils.ParseDecimal(value);
            break;
          case "nonhousingpayment":
            this.nonhousingPayment = Utils.ParseDecimal(value);
            break;
          case "propertyaddress":
            this.propertyAddress.Street1 = string.Concat(value);
            break;
          case "propertycity":
            this.propertyAddress.City = string.Concat(value);
            break;
          case "propertystate":
            this.propertyAddress.State = string.Concat(value);
            break;
          case "propertytype":
            if (value is string)
            {
              this.propertyType = PropertyTypeEnumUtil.NameToValue(string.Concat(value));
              break;
            }
            this.propertyType = (EllieMae.EMLite.Common.Contact.PropertyType) value;
            break;
          case "propertyuse":
            if (value is string)
            {
              this.propertyUse = PropertyUseEnumUtil.NameToValue(string.Concat(value));
              break;
            }
            this.propertyUse = (PropertyUse) value;
            break;
          case "propertyvalue":
            this.propertyValue = Utils.ParseDecimal(value);
            break;
          case "propertyzip":
            this.propertyAddress.Zip = string.Concat(value);
            break;
          case "purchasedate":
            this.purchaseDate = Utils.ParseDate(value);
            break;
          case "purpose":
            if (value is string)
            {
              this.purpose = LoanPurposeEnumUtil.NameToValue(string.Concat(value));
              if (this.purpose != EllieMae.EMLite.Common.Contact.LoanPurpose.Blank || !(string.Concat(value) != ""))
                break;
              this.purpose = EllieMae.EMLite.Common.Contact.LoanPurpose.Other;
              this.purposeOther = string.Concat(value);
              break;
            }
            this.purpose = (EllieMae.EMLite.Common.Contact.LoanPurpose) value;
            break;
          case "purposeother":
            this.purposeOther = Convert.ToString(value);
            break;
          case "term":
            this.term = Utils.ParseInt(value);
            break;
          case "typeother":
            this.typeOther = Convert.ToString(value);
            break;
          default:
            throw new ArgumentException("Invalid field name \"" + columnName + "\"");
        }
      }
    }

    public string ColumnValueToString(string columnName)
    {
      columnName = columnName.ToLower();
      switch (columnName)
      {
        case "amortization":
          return this.AmortizationString;
        case "bankruptcy":
          return this.IsBankruptcyString;
        case "cashout":
          return this.CashOutString;
        case "contactid":
          return this.ContactIDString;
        case "creditrating":
          return this.creditRating;
        case "downpayment":
          return this.DownPaymentString;
        case "employment":
          return this.EmploymentStatusString;
        case "estimatedcreditscore":
          return this.EstimatedCreditScoreString;
        case "housingpayment":
          return this.HousingPaymentString;
        case "loanamount":
          return this.LoanAmountString;
        case "loantype":
          return this.LoanTypeString;
        case "mortgagebalance":
          return this.MortgageBalanceString;
        case "mortgagerate":
          return this.MortgageRateString;
        case "nonhousingpayment":
          return this.NonhousingPaymentString;
        case "opportunityid":
          return this.OpportunityIDString;
        case "propertyaddress":
          return this.propertyAddress.Street1;
        case "propertycity":
          return this.propertyAddress.City;
        case "propertystate":
          return this.propertyAddress.State;
        case "propertytype":
          return this.PropTypeString;
        case "propertyuse":
          return this.PropUseString;
        case "propertyvalue":
          return this.PropertyValueString;
        case "propertyzip":
          return this.propertyAddress.Zip;
        case "purchasedate":
          return this.PurchaseDateString;
        case "purpose":
          return this.PurposeString;
        case "purposeother":
          return this.PurposeOtherString;
        case "term":
          return this.TermString;
        case "typeOther":
          return this.TypeOtherString;
        default:
          return (string) null;
      }
    }

    public int OpportunityID
    {
      get => this.opportunityId;
      set => this.opportunityId = value;
    }

    public string OpportunityIDString => this.opportunityId.ToString();

    public int ContactID
    {
      get => this.contactId;
      set => this.contactId = value;
    }

    public string ContactIDString => this.contactId.ToString();

    public Decimal LoanAmount
    {
      get => this.loanAmount;
      set => this.loanAmount = value;
    }

    public string LoanAmountString
    {
      get => !(this.loanAmount == 0M) ? this.loanAmount.ToString("N0") : "";
    }

    public Decimal CashOut
    {
      get => this.cashOut;
      set => this.cashOut = value;
    }

    public string CashOutString => !(this.cashOut == 0M) ? this.cashOut.ToString("N0") : "";

    public EllieMae.EMLite.Common.Contact.LoanPurpose Purpose
    {
      get => this.purpose;
      set => this.purpose = value;
    }

    public string PurposeString => LoanPurposeEnumUtil.ValueToName(this.purpose);

    public string PurposeOther
    {
      get => this.purposeOther;
      set => this.purposeOther = value;
    }

    public string PurposeOtherString => this.purposeOther;

    public LoanTypeEnum LoanType
    {
      get => this.loanType;
      set => this.loanType = value;
    }

    public string LoanTypeString => LoanTypeEnumUtil.ValueToName(this.loanType);

    public string TypeOther
    {
      get => this.typeOther;
      set => this.typeOther = value;
    }

    public string TypeOtherString => this.typeOther;

    public string EstimatedCreditScore
    {
      get => this.estimatedCreditScore;
      set => this.estimatedCreditScore = value;
    }

    public string EstimatedCreditScoreString => this.estimatedCreditScore;

    public int Term
    {
      get => this.term;
      set => this.term = value;
    }

    public string TermString => this.term >= 0 ? this.term.ToString() : "";

    public AmortizationType Amortization
    {
      get => this.amortization;
      set => this.amortization = value;
    }

    public string AmortizationString => AmortizationTypeEnumUtil.ValueToName(this.amortization);

    public Decimal DownPayment
    {
      get => this.downPayment;
      set => this.downPayment = value;
    }

    public string DownPaymentString
    {
      get => !(this.downPayment == 0M) ? this.downPayment.ToString("N0") : "";
    }

    public Address PropertyAddress
    {
      get => this.propertyAddress;
      set => this.propertyAddress = value;
    }

    public PropertyUse PropUse
    {
      get => this.propertyUse;
      set => this.propertyUse = value;
    }

    public string PropUseString => PropertyUseEnumUtil.ValueToName(this.propertyUse);

    public EllieMae.EMLite.Common.Contact.PropertyType PropType
    {
      get => this.propertyType;
      set => this.propertyType = value;
    }

    public string PropTypeString => PropertyTypeEnumUtil.ValueToName(this.propertyType);

    public Decimal PropertyValue
    {
      get => this.propertyValue;
      set => this.propertyValue = value;
    }

    public string PropertyValueString
    {
      get => !(this.propertyValue == 0M) ? this.propertyValue.ToString("N0") : "";
    }

    public DateTime PurchaseDate
    {
      get => this.purchaseDate;
      set => this.purchaseDate = value;
    }

    public string PurchaseDateString
    {
      get
      {
        return !(this.purchaseDate == DateTime.MinValue) ? this.purchaseDate.ToString("d", (IFormatProvider) DateTimeFormatInfo.InvariantInfo) : string.Empty;
      }
    }

    public Decimal MortgageBalance
    {
      get => this.mortgageBalance;
      set => this.mortgageBalance = value;
    }

    public string MortgageBalanceString
    {
      get => !(this.mortgageBalance == 0M) ? this.mortgageBalance.ToString("N0") : "";
    }

    public Decimal MortgageRate
    {
      get => this.mortgageRate;
      set => this.mortgageRate = value;
    }

    public string MortgageRateString
    {
      get => !(this.mortgageRate == 0M) ? this.mortgageRate.ToString("F3") : "";
    }

    public Decimal HousingPayment
    {
      get => this.housingPayment;
      set => this.housingPayment = value;
    }

    public string HousingPaymentString
    {
      get => !(this.housingPayment == 0M) ? this.housingPayment.ToString("N2") : "";
    }

    public Decimal NonhousingPayment
    {
      get => this.nonhousingPayment;
      set => this.nonhousingPayment = value;
    }

    public string NonhousingPaymentString
    {
      get => !(this.nonhousingPayment == 0M) ? this.nonhousingPayment.ToString("N2") : "";
    }

    public string CreditRating
    {
      get => this.creditRating;
      set => this.creditRating = value;
    }

    public bool IsBankruptcy
    {
      get => this.isBankruptcy;
      set => this.isBankruptcy = value;
    }

    public string IsBankruptcyString => !this.isBankruptcy ? "" : "X";

    public EmploymentStatus Employment
    {
      get => this.employment;
      set => this.employment = value;
    }

    public string EmploymentStatusString => EmploymentStatusEnumUtil.ValueToName(this.employment);
  }
}

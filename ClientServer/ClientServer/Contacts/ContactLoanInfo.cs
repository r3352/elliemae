// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.ContactLoanInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common.Contact;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class ContactLoanInfo
  {
    private int loanID;
    private int borrowerID;
    private string loanStatus;
    private Decimal appraisedValue;
    private Decimal loanAmount;
    private Decimal interestRate;
    private int term;
    private EllieMae.EMLite.Common.Contact.LoanPurpose purpose;
    private Decimal downPayment;
    private Decimal ltv;
    private AmortizationType amortization;
    private DateTime dateCompleted;
    private LoanTypeEnum loanType;
    private LienEnum lienPosition;

    public ContactLoanInfo()
    {
      this.loanID = -1;
      this.borrowerID = -1;
      this.loanStatus = "";
      this.appraisedValue = 0M;
      this.loanAmount = 0M;
      this.interestRate = 0M;
      this.term = 0;
      this.purpose = EllieMae.EMLite.Common.Contact.LoanPurpose.Blank;
      this.downPayment = 0M;
      this.ltv = 0M;
      this.amortization = AmortizationType.Blank;
      this.dateCompleted = new DateTime(2003, 1, 1, 10, 0, 0);
      this.loanType = LoanTypeEnum.Blank;
      this.lienPosition = LienEnum.Blank;
    }

    public ContactLoanInfo(
      int loanId,
      int borrowerID,
      string loanStatus,
      Decimal appraisedValue,
      Decimal loanAmount,
      Decimal interestRate,
      int term,
      EllieMae.EMLite.Common.Contact.LoanPurpose purpose,
      Decimal downPayment,
      Decimal ltv,
      AmortizationType amortization,
      DateTime dateCompleted,
      LoanTypeEnum loanType,
      LienEnum lienPosition)
    {
      this.loanID = loanId;
      this.borrowerID = borrowerID;
      this.loanStatus = loanStatus;
      this.appraisedValue = appraisedValue;
      this.loanAmount = loanAmount;
      this.interestRate = interestRate;
      this.term = term;
      this.purpose = purpose;
      this.downPayment = downPayment;
      this.ltv = ltv;
      this.amortization = amortization;
      this.dateCompleted = dateCompleted;
      this.loanType = loanType;
      this.lienPosition = lienPosition;
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
          case "appraisedvalue":
            return (object) this.appraisedValue;
          case "borrowerid":
            return (object) this.borrowerID;
          case "datecompleted":
            return (object) this.dateCompleted;
          case "downpayment":
            return (object) this.downPayment;
          case "interestrate":
            return (object) this.interestRate;
          case "lienposition":
            return (object) this.lienPosition;
          case "loanamount":
            return (object) this.loanAmount;
          case "loanid":
            return (object) this.loanID;
          case "loanstatus":
            return (object) this.loanStatus;
          case "loantype":
            return (object) this.loanType;
          case "ltv":
            return (object) this.ltv;
          case "purpose":
            return (object) this.purpose;
          case "term":
            return (object) this.term;
          default:
            return (object) null;
        }
      }
    }

    public int LoanID
    {
      get => this.loanID;
      set => this.loanID = value;
    }

    public int BorrowerID
    {
      get => this.borrowerID;
      set => this.borrowerID = value;
    }

    public string LoanStatus
    {
      get => this.loanStatus;
      set => this.loanStatus = value;
    }

    public Decimal AppraisedValue
    {
      get => this.appraisedValue;
      set => this.appraisedValue = value;
    }

    public Decimal LoanAmount
    {
      get => this.loanAmount;
      set => this.loanAmount = value;
    }

    public Decimal InterestRate
    {
      get => this.interestRate;
      set => this.interestRate = value;
    }

    public int Term
    {
      get => this.term;
      set => this.term = value;
    }

    public EllieMae.EMLite.Common.Contact.LoanPurpose Purpose
    {
      get => this.purpose;
      set => this.purpose = value;
    }

    public Decimal DownPayment
    {
      get => this.downPayment;
      set => this.downPayment = value;
    }

    public Decimal LTV
    {
      get => this.ltv;
      set => this.ltv = value;
    }

    public AmortizationType Amortization
    {
      get => this.amortization;
      set => this.amortization = value;
    }

    public DateTime DateCompleted
    {
      get => this.dateCompleted;
      set => this.dateCompleted = value;
    }

    public LoanTypeEnum LoanType
    {
      get => this.loanType;
      set => this.loanType = value;
    }

    public LienEnum LienPosition
    {
      get => this.lienPosition;
      set => this.lienPosition = value;
    }
  }
}

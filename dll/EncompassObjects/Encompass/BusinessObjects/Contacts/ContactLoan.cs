// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactLoan
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  public class ContactLoan : SessionBoundObject, IContactLoan
  {
    private IContactManager mngr;
    private ContactLoanInfo info;

    internal ContactLoan(Session session, ContactLoanInfo info)
      : base(session)
    {
      this.mngr = session.Contacts.ContactManager;
      this.info = info;
    }

    public int ID => this.info.LoanID;

    public int BorrowerID => this.info.BorrowerID;

    public string LoanStatus => this.info.LoanStatus;

    public Decimal AppraisedValue => this.info.AppraisedValue;

    public Decimal LoanAmount => this.info.LoanAmount;

    public Decimal InterestRate => this.info.InterestRate;

    public int Term => this.info.Term;

    public string Purpose => this.info.Purpose.ToString();

    public Decimal DownPayment => this.info.DownPayment;

    public Decimal LTV => this.info.LTV;

    public string Amortization => this.info.Amortization.ToString();

    public DateTime DateCompleted => this.info.DateCompleted;

    public string LoanType => this.info.LoanType.ToString();

    public string LienPosition => this.info.LienPosition.ToString();
  }
}

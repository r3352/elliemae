// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactOpportunity
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Represents a business opportunity with a borrower contact.
  /// </summary>
  /// <example>
  /// The following code demonstrates how to add an Opportunity record to a
  /// Borrower Contact.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Contacts;
  /// 
  /// class ContactManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Create a new contact
  ///       BorrowerContact contact = (BorrowerContact) session.Contacts.CreateNew(ContactType.Borrower);
  /// 
  ///       // Set some basic properties
  ///       contact.FirstName = "Mary";
  ///       contact.LastName = "Jones";
  ///       contact.HomePhone = "(555) 555-5555";
  ///       contact.BorrowerType = BorrowerContactType.Propspect;
  /// 
  ///       // Save the contact before we attempt to create the opportunity
  ///       contact.Commit();
  /// 
  ///       // Create an opportunity for the contact
  ///       ContactOpportunity opp = contact.Opportunities.Add();
  /// 
  ///       opp.LoanAmount = 245000;
  ///       opp.MortgageBalance = 255000;
  ///       opp.MortgageRate = 6.75f;
  ///       opp.EmploymentStatus = EmploymentStatus.Employed;
  ///       opp.PropertyUse = PropertyUse.Primary;
  ///       opp.Commit();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class ContactOpportunity : SessionBoundObject, IContactOpportunity
  {
    private EllieMae.Encompass.BusinessObjects.Contacts.Contact contact;
    private Opportunity opp;
    private Address propertyAddress;

    internal ContactOpportunity(EllieMae.Encompass.BusinessObjects.Contacts.Contact contact, Opportunity opp)
      : base(contact.Session)
    {
      this.contact = contact;
      this.opp = opp;
    }

    /// <summary>The unique identifier for this Opportunity.</summary>
    public int ID => this.opp.OpportunityID;

    /// <summary>Gets or sets the loan amount for the opportunity.</summary>
    public Decimal LoanAmount
    {
      get => this.opp.LoanAmount;
      set => this.opp.LoanAmount = value;
    }

    /// <summary>Gets or sets the loan purpose.</summary>
    public LoanPurpose Purpose
    {
      get => (LoanPurpose) this.opp.Purpose;
      set => this.opp.Purpose = (EllieMae.EMLite.Common.Contact.LoanPurpose) value;
    }

    /// <summary>
    /// Gets or sets the purpose of the loan when <see cref="F:EllieMae.Encompass.BusinessObjects.Contacts.LoanPurpose.Other" />
    /// is selected for the <see cref="P:EllieMae.Encompass.BusinessObjects.Contacts.ContactOpportunity.Purpose" /> field.
    /// </summary>
    public string PurposeOther
    {
      get => this.opp.PurposeOther;
      set => this.opp.PurposeOther = value;
    }

    /// <summary>
    /// Gets or sets the term, in months, of the proposed loan.
    /// </summary>
    public int Term
    {
      get => this.opp.Term;
      set => this.opp.Term = value;
    }

    /// <summary>
    /// Getsor sets the amortization type of the proposed loan.
    /// </summary>
    public AmortizationType Amortization
    {
      get => (AmortizationType) this.opp.Amortization;
      set => this.opp.Amortization = (EllieMae.EMLite.Common.Contact.AmortizationType) value;
    }

    /// <summary>Gets or sets the borrower's proposed down payment.</summary>
    public Decimal DownPayment
    {
      get => this.opp.DownPayment;
      set => this.opp.DownPayment = value;
    }

    /// <summary>
    /// Gets the property address for which the borrower is seeking a mortgage.
    /// </summary>
    public Address PropertyAddress
    {
      get
      {
        if (this.propertyAddress == null)
          this.propertyAddress = new Address(this.opp.PropertyAddress);
        return this.propertyAddress;
      }
    }

    /// <summary>
    /// Gets or sets the property use type for the borrower's property.
    /// </summary>
    public PropertyUse PropertyUse
    {
      get => (PropertyUse) this.opp.PropUse;
      set => this.opp.PropUse = (EllieMae.EMLite.Common.Contact.PropertyUse) value;
    }

    /// <summary>Gets or sets the type of property being financed.</summary>
    public PropertyType PropertyType
    {
      get => (PropertyType) this.opp.PropType;
      set => this.opp.PropType = (EllieMae.EMLite.Common.Contact.PropertyType) value;
    }

    /// <summary>
    /// Gets or sets the estimated or actual value of the property.
    /// </summary>
    public Decimal PropertyValue
    {
      get => this.opp.PropertyValue;
      set => this.opp.PropertyValue = value;
    }

    /// <summary>Gets or sets the borrower's current mortgage balance.</summary>
    public Decimal MortgageBalance
    {
      get => this.opp.MortgageBalance;
      set => this.opp.MortgageBalance = value;
    }

    /// <summary>
    /// Gets or sets the borrower's current mortgage interest rate.
    /// </summary>
    public float MortgageRate
    {
      get => (float) this.opp.MortgageRate;
      set => this.opp.MortgageRate = (Decimal) value;
    }

    /// <summary>
    /// Gets or sets the borrower's current housing-related monthly payment.
    /// </summary>
    public Decimal HousingPayment
    {
      get => this.opp.HousingPayment;
      set => this.opp.HousingPayment = value;
    }

    /// <summary>
    /// Gets or sets the borrower's current non-housing-related montly payments.
    /// </summary>
    public Decimal NonHousingPayment
    {
      get => this.opp.NonhousingPayment;
      set => this.opp.NonhousingPayment = value;
    }

    /// <summary>
    /// Gets or sets the date on which the property was (or will be) purchased by the borrower.
    /// </summary>
    public DateTime PurchaseDate
    {
      get => this.opp.PurchaseDate;
      set => this.opp.PurchaseDate = value;
    }

    /// <summary>Gets or sets the borrower's credit score or rating.</summary>
    public string CreditRating
    {
      get => this.opp.CreditRating;
      set => this.opp.CreditRating = value;
    }

    /// <summary>
    /// Gets or sets a flag indicating if the borrower is currenly in bankruptcy.
    /// </summary>
    public bool InBankruptcy
    {
      get => this.opp.IsBankruptcy;
      set => this.opp.IsBankruptcy = value;
    }

    /// <summary>
    /// Gets or sets the borrower's current employment status.
    /// </summary>
    public EmploymentStatus EmploymentStatus
    {
      get => (EmploymentStatus) this.opp.Employment;
      set => this.opp.Employment = (EllieMae.EMLite.Common.Contact.EmploymentStatus) value;
    }

    /// <summary>Commits the opportunity to the server.</summary>
    /// <example>
    /// The following code demonstrates how to add an Opportunity record to a
    /// Borrower Contact.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create a new contact
    ///       BorrowerContact contact = (BorrowerContact) session.Contacts.CreateNew(ContactType.Borrower);
    /// 
    ///       // Set some basic properties
    ///       contact.FirstName = "Mary";
    ///       contact.LastName = "Jones";
    ///       contact.HomePhone = "(555) 555-5555";
    ///       contact.BorrowerType = BorrowerContactType.Propspect;
    /// 
    ///       // Save the contact before we attempt to create the opportunity
    ///       contact.Commit();
    /// 
    ///       // Create an opportunity for the contact
    ///       ContactOpportunity opp = contact.Opportunities.Add();
    /// 
    ///       opp.LoanAmount = 245000;
    ///       opp.MortgageBalance = 255000;
    ///       opp.MortgageRate = 6.75f;
    ///       opp.EmploymentStatus = EmploymentStatus.Employed;
    ///       opp.PropertyUse = PropertyUse.Primary;
    ///       opp.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Commit()
    {
      this.Session.Contacts.ContactManager.UpdateBorrowerOpportunity(this.opp);
    }

    /// <summary>Refreshes the opportunity from the server.</summary>
    /// <remarks>Any changes to the object which has been made since the last
    /// call to <see cref="M:EllieMae.Encompass.BusinessObjects.Contacts.ContactOpportunity.Commit" /> are discarded.</remarks>
    /// <example>
    /// The following code demonstrates how to use the Refresh method to restore
    /// the prior data to an opportunity.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Retrieve an existing contact from the server
    ///       BorrowerContact contact = (BorrowerContact) session.Contacts.Open(209, ContactType.Borrower);
    /// 
    ///       // Retrieve the borrower's Opportunity
    ///       ContactOpportunity opp = contact.Opportunities[0];
    /// 
    ///       // Change the loan amount on the opportunity
    ///       opp.LoanAmount = opp.LoanAmount + 10000;
    /// 
    ///       // Refresh the opportunity to restore the original values
    ///       opp.Refresh();
    /// 
    ///       // Print the Loan Amount -- it should reflect the value before we changed
    ///       // it with the code above
    ///       Console.WriteLine(opp.LoanAmount.ToString());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Refresh()
    {
      this.opp = this.Session.Contacts.ContactManager.GetBorrowerOpportunity(this.ID);
    }
  }
}

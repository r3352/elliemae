// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents the collections of contacts linked to the current loan.
  /// </summary>
  /// <remarks>Encompass permits <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan" /> and <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact" /> objects to be linked
  /// together. This permits you to easily determine all of the contacts that are tied to a particular
  /// loan, or all loans tied to a particular contact.</remarks>
  /// <example>
  ///       The following code creates a new BorrowerContact and then links it to an
  ///       existing loan file. It then retrieves all loans for that contact to verify
  ///       that the link loan is returned.
  ///       <code>
  ///         <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Contacts;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// 
  /// class SampleApp
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.StartOffline("mary", "maryspwd");
  /// 
  ///       // Open the loan using the GUID specified on the command line
  ///       Loan loan = session.Loans.Open(args[0]);
  ///       loan.Lock();
  /// 
  ///       // Create a new Borrower Contact. The contact myst be Commited before
  ///       // we can link it to the loan.
  ///       BorrowerContact contact = (BorrowerContact) session.Contacts.CreateNew(ContactType.Borrower);
  ///       contact.FirstName = "CRM";
  ///       contact.LastName = "Test";
  ///       contact.Commit();
  /// 
  ///       // Retrieve the Borrower from from the primary BorrowerPair. This is the borrower
  ///       // we will link the contact to.
  ///       Borrower primaryBorrower = loan.BorrowerPairs[0].Borrower;
  ///       loan.Contacts.LinkToBorrowerContact(contact, primaryBorrower);
  /// 
  ///       // Commit the loan to save this relationship
  ///       loan.Commit();
  ///       loan.Close();
  /// 
  ///       // Retrieve all loans for our new contact
  ///       LoanContactRelationshipList relations = contact.GetLoanRelationships();
  /// 
  ///       foreach (LoanContactRelationship relation in relations)
  ///         Console.WriteLine("This contact is the " + relation.RelationshipType
  ///           + " for the loan " + relation.LoanGuid);
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///       </code>
  ///     </example>
  public class LoanContacts : ILoanContacts, IEnumerable
  {
    private Loan loan;

    internal LoanContacts(Loan loan) => this.loan = loan;

    /// <summary>
    /// Gets a relationship from the loan based on the specified relationship type.
    /// </summary>
    /// <param name="relation">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanContactRelationshipType" /> for which to retrieve the relationship object.</param>
    /// <returns>If a relationship exists with the specified type, the desired <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanContactRelationship" />
    /// will be returned. Otherwise, <c>null</c> will be returned.</returns>
    /// <example>
    ///       The following code demonstrates how to iterate over the set of all contacts
    ///       linked to the current loan file.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class SampleApp
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Retrieve the Appraiser contact relationship, if one exists
    ///       LoanContactRelationship relation =
    ///         loan.Contacts.GetBizContactRelationship(LoanContactRelationshipType.Appraiser);
    /// 
    ///       if (relation != null)
    ///       {
    ///         // Open the appraiser's information
    ///         BizContact contact = (BizContact) session.Contacts.Open(relation.ContactID, relation.ContactType);
    /// 
    ///         // Retrieve all of the appraiser's other related loans
    ///         LoanContactRelationshipList appraisedLoans = contact.GetLoanRelationships();
    /// 
    ///         // For each loan the contact has appraised, retrieve the loan's LoanNumber
    ///         foreach (LoanContactRelationship rel in appraisedLoans)
    ///         {
    ///           // Set up the list of fields to retrieve from the loans.
    ///           StringList fieldsToRetrieve = new StringList();
    ///           fieldsToRetrieve.Add("Loan.LoanNumber");
    ///           fieldsToRetrieve.Add("Loan.BorrowerLastName");
    /// 
    ///           // Retrieve the data
    ///           LoanReportData reportData = session.Reports.SelectReportingFieldsForLoan(rel.LoanGuid, fieldsToRetrieve);
    /// 
    ///           // Display the output
    ///           Console.WriteLine("The loan " + reportData["Loan.LoanNumber"]
    ///             + " (" + reportData["Loan.BorrowerLastName"] + ") was appraised by " + contact.FullName);
    ///         }
    ///       }
    /// 
    ///       // Close the loan
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanContactRelationship GetBizContactRelationship(LoanContactRelationshipType relation)
    {
      CRMLog crmMapping = this.loan.LoanData.GetLogList().GetCRMMapping(((int) relation).ToString());
      return crmMapping == null ? (LoanContactRelationship) null : this.createRelationship(crmMapping);
    }

    /// <summary>Gets all business relationships with the loan.</summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.LoanContactRelationshipList" /> containing all of the
    /// relationships that are tied to business contacts in the loan.</returns>
    public LoanContactRelationshipList GetBizContactRelationships()
    {
      LoanContactRelationshipList contactRelationships = new LoanContactRelationshipList();
      foreach (CRMLog logItem in this.loan.LoanData.GetLogList().GetAllCRMMapping())
      {
        if (logItem.MappingType == CRMLogType.BusinessContact)
        {
          LoanContactRelationship relationship = this.createRelationship(logItem);
          if (relationship != null)
            contactRelationships.Add(relationship);
        }
      }
      return contactRelationships;
    }

    /// <summary>
    /// Gets a borrower contact relationship for the specified borrower.
    /// </summary>
    /// <param name="borrower" />
    /// <returns>If a relationship exists for the specified borrower, a
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanContactRelationship" /> is returned. Otherwise, <c>null</c> is returned.</returns>
    /// <example>
    ///       The following code retrieves the BorrowerContacts that are linked to the
    ///       primary Borrower and Coborrower records in the loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class SampleApp
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Retrieve the BorrowerContacts for the primary borrower coborrower, if they are linked
    ///       BorrowerPair p = loan.BorrowerPairs[0];
    /// 
    ///       // Retrieve the contact relationship for the borrower and coborrower
    ///       LoanContactRelationship borrowerRel = loan.Contacts.GetBorrowerRelationship(p.Borrower);
    ///       LoanContactRelationship coborrowerRel = loan.Contacts.GetBorrowerRelationship(p.CoBorrower);
    /// 
    ///       // Open the contacts and display their info
    ///       if (borrowerRel != null)
    ///       {
    ///         // Retrieve the contact's information
    ///         BorrowerContact borrower = (BorrowerContact)session.Contacts.Open(borrowerRel.ContactID,
    ///           borrowerRel.ContactType);
    /// 
    ///         Console.WriteLine("Linked Borrower:  " + borrower.FullName + " (" + borrower.PersonalEmail + ")");
    ///       }
    /// 
    ///       if (coborrowerRel != null)
    ///       {
    ///         // Retrieve the contact's information
    ///         BorrowerContact coborrower = (BorrowerContact)session.Contacts.Open(coborrowerRel.ContactID,
    ///           coborrowerRel.ContactType);
    /// 
    ///         Console.WriteLine("Linked Co-Borrower:  " + coborrower.FullName + " (" + coborrower.PersonalEmail + ")");
    ///       }
    /// 
    ///       // Close the loan
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanContactRelationship GetBorrowerRelationship(Borrower borrower)
    {
      CRMLog crmMapping = this.loan.LoanData.GetLogList().GetCRMMapping(borrower.ID);
      return crmMapping == null ? (LoanContactRelationship) null : this.createRelationship(crmMapping);
    }

    /// <summary>
    /// Gets all borrower and coborrower relationships with the loan.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.LoanContactRelationshipList" /> containing all of the
    /// relationships that are tied to borrower or coborrower records in the loan.</returns>
    public LoanContactRelationshipList GetBorrowerRelationships()
    {
      LoanContactRelationshipList borrowerRelationships = new LoanContactRelationshipList();
      foreach (CRMLog logItem in this.loan.LoanData.GetLogList().GetAllCRMMapping())
      {
        if (logItem.MappingType == CRMLogType.BorrowerContact)
        {
          LoanContactRelationship relationship = this.createRelationship(logItem);
          if (relationship != null)
            borrowerRelationships.Add(relationship);
        }
      }
      return borrowerRelationships;
    }

    /// <summary>
    /// Retrieves all <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanContactRelationship" />s for the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact" />.
    /// </summary>
    /// <param name="contact">The <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact" /> for which the relationships are being requested.</param>
    /// <returns>A collection of the LoanContactRelationships to which the contact is assigned. Either
    /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact" /> or <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" /> can be passed and the method will
    /// return the appropriate relations.</returns>
    public LoanContactRelationshipList GetRelationshipsForContact(Contact contact)
    {
      LoanContactRelationshipList relationshipsForContact = new LoanContactRelationshipList();
      foreach (CRMLog crmLog in this.loan.LoanData.GetLogList().GetAllCRMMapping())
      {
        if (crmLog.ContactGuid == contact.Guid)
        {
          LoanContactRelationship contactRelationship = new LoanContactRelationship(this.loan.Session, this.loan.Guid, contact.ID, contact.Type, (LoanContactRelationshipType) crmLog.RoleType, contact.Type == ContactType.Borrower ? crmLog.GetBorrowerPairIndex() : -1);
          relationshipsForContact.Add(contactRelationship);
        }
      }
      return relationshipsForContact;
    }

    /// <summary>Sets a business contact relationship within the loan.</summary>
    /// <param name="contact">The contact to add as part of the relationship.</param>
    /// <param name="relationType">The type of relation the contact has to the loan.</param>
    /// <returns>Returns a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanContactRelationship" /> for this relationship.</returns>
    /// <remarks>Only a single business contact can fulfill a relationship in any given loan. If
    /// the specified relationship is already assigned to an existing contact, that relationship
    /// will be discarded and replaced with this new relationship.
    /// This method only creates the relationship between the loan and contact. The contact data is not copied to the loan.</remarks>
    /// <example>
    ///       The following code searches the Business Contacts for a company whose name
    ///       matches the Appraisal Company name in a loan. If a match is found, the
    ///       BizContact is linked to the Loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class SampleApp
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    ///       loan.Lock();
    /// 
    ///       // Run a query to locate a Business Contact that matches the Appraiser in the loan file
    ///       StringFieldCriterion cri = new StringFieldCriterion();
    ///       cri.FieldName = "Contact.CompanyName";
    ///       cri.Value = loan.Fields["617"].FormattedValue;   // Field 617 is the Appraisal Company Name
    /// 
    ///       // Execute the query to get the matching contact(s)
    ///       ContactList contacts = session.Contacts.Query(cri, ContactLoanMatchType.None, ContactType.Biz);
    /// 
    ///       if (contacts.Count > 0)
    ///       {
    ///         // Retrieve the first BizContact from the result set
    ///         BizContact contact = (BizContact)contacts[0];
    /// 
    ///         // Link the contact to the loan in the Appraisal relation
    ///         loan.Contacts.LinkToBizContact(contact, LoanContactRelationshipType.Appraiser);
    /// 
    ///         loan.Fields["617"].Value = contact.CompanyName;
    /// 
    ///         string fullStreet = contact.BizAddress.Street1;
    ///         if (!string.IsNullOrEmpty(contact.BizAddress.Street2.Trim()))
    ///           fullStreet += ", " + contact.BizAddress.Street2;
    ///         loan.Fields["619"].Value = fullStreet;
    /// 
    ///         loan.Fields["620"].Value = contact.BizAddress.City;
    ///         loan.Fields["1244"].Value = contact.BizAddress.State;
    ///         loan.Fields["621"].Value = contact.BizAddress.Zip;
    ///         loan.Fields["618"].Value = contact.FullName;
    ///         loan.Fields["974"].Value = contact.LicenseNumber;
    ///         loan.Fields["622"].Value = contact.WorkPhone;
    ///         loan.Fields["89"].Value = contact.BizEmail;
    ///         loan.Fields["VEND.X526"].Value = contact.MobilePhone;
    ///         loan.Fields["1246"].Value = contact.FaxNumber;
    /// 
    ///       }
    /// 
    ///       // Commit the loan to save this relationship
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void LinkToBizContact(BizContact contact, LoanContactRelationshipType relationType)
    {
      if (contact == null)
        throw new ArgumentNullException(nameof (contact));
      if (contact.IsNew)
        throw new ArgumentException("The specified contact must be committed before it can be set into a loan relationship");
      this.loan.LoanData.GetLogList().AddCRMMapping(((int) relationType).ToString(), CRMLogType.BusinessContact, contact.Unwrap().ContactGuid.ToString(), (CRMRoleType) Enum.Parse(typeof (CRMRoleType), string.Concat((object) relationType)));
    }

    /// <summary>
    /// Breaks the link between a <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" /> and the loan.
    /// </summary>
    /// <param name="relationType">The relation type to be broken.</param>
    public void UnlinkBizContact(LoanContactRelationshipType relationType)
    {
      LogList logList = this.loan.LoanData.GetLogList();
      CRMLog crmMapping = logList.GetCRMMapping(((int) relationType).ToString());
      if (crmMapping == null)
        return;
      logList.RemoveCRMMapping(crmMapping);
    }

    /// <summary>
    /// Breaks all links between <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" /> records and the loan.
    /// </summary>
    public void UnlinkAllBizContacts()
    {
      LogList logList = this.loan.LoanData.GetLogList();
      foreach (CRMLog rec in logList.GetAllCRMMapping())
      {
        if (rec.MappingType == CRMLogType.BusinessContact)
          logList.RemoveCRMMapping(rec);
      }
    }

    /// <summary>
    /// Links a <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact" /> to the loan.
    /// </summary>
    /// <param name="contact">The <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact" /> to be linked to the loan.</param>
    /// <param name="loanBorrower">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Borrower" /> from the loan file to which the contact
    /// will be linked.</param>
    /// <remarks>For each <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Borrower" /> within the loan, a single <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact" />
    /// can be linked. If a BorrowerContact is already linked to the specified Borrower, that
    /// relationship will be overwritten by this new relationship.
    /// </remarks>
    /// <example>
    ///       The following code creates a new BorrowerContact and then links it to an
    ///       existing loan file. It then retrieves all loans for that contact to verify
    ///       that the link loan is returned.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class SampleApp
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    ///       loan.Lock();
    /// 
    ///       // Create a new Borrower Contact. The contact myst be Commited before
    ///       // we can link it to the loan.
    ///       BorrowerContact contact = (BorrowerContact) session.Contacts.CreateNew(ContactType.Borrower);
    ///       contact.FirstName = "CRM";
    ///       contact.LastName = "Test";
    ///       contact.Commit();
    /// 
    ///       // Retrieve the Borrower from from the primary BorrowerPair. This is the borrower
    ///       // we will link the contact to.
    ///       Borrower primaryBorrower = loan.BorrowerPairs[0].Borrower;
    ///       loan.Contacts.LinkToBorrowerContact(contact, primaryBorrower);
    /// 
    ///       // Commit the loan to save this relationship
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // Retrieve all loans for our new contact
    ///       LoanContactRelationshipList relations = contact.GetLoanRelationships();
    /// 
    ///       foreach (LoanContactRelationship relation in relations)
    ///         Console.WriteLine("This contact is the " + relation.RelationshipType
    ///           + " for the loan " + relation.LoanGuid);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void LinkToBorrowerContact(BorrowerContact contact, Borrower loanBorrower)
    {
      if (contact == null)
        throw new ArgumentNullException(nameof (contact));
      if (contact.IsNew)
        throw new ArgumentException("The specified contact must be committed before it can be set into a loan relationship");
      LogList logList = this.loan.LoanData.GetLogList();
      foreach (EllieMae.EMLite.DataEngine.BorrowerPair borrowerPair in this.loan.LoanData.GetBorrowerPairs())
      {
        if (borrowerPair.Borrower.Id == loanBorrower.ID)
        {
          logList.AddCRMMapping(loanBorrower.ID, CRMLogType.BorrowerContact, contact.Unwrap().ContactGuid.ToString(), CRMRoleType.Borrower);
          break;
        }
        if (borrowerPair.CoBorrower != null && borrowerPair.CoBorrower.Id == loanBorrower.ID)
        {
          logList.AddCRMMapping(loanBorrower.ID, CRMLogType.BorrowerContact, contact.Unwrap().ContactGuid.ToString(), CRMRoleType.Coborrower);
          break;
        }
      }
    }

    /// <summary>
    /// Breaks the link between a <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact" /> and the loan.
    /// </summary>
    /// <param name="loanBorrower">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Borrower" /> for which the relationship should be broken.</param>
    public void UnlinkBorrower(Borrower loanBorrower)
    {
      LogList logList = this.loan.LoanData.GetLogList();
      CRMLog crmMapping = logList.GetCRMMapping(loanBorrower.ID);
      if (crmMapping == null)
        return;
      logList.RemoveCRMMapping(crmMapping);
    }

    /// <summary>
    /// Breaks all link between <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Borrower" />s  and the loan.
    /// </summary>
    public void UnlinkAllBorrowerContacts()
    {
      LogList logList = this.loan.LoanData.GetLogList();
      foreach (CRMLog rec in logList.GetAllCRMMapping())
      {
        if (rec.MappingType == CRMLogType.BorrowerContact)
          logList.RemoveCRMMapping(rec);
      }
    }

    /// <summary>
    /// Breaks a specific link between a contact and the loan.
    /// </summary>
    /// <param name="relation">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanContactRelationship" /> to be broken.</param>
    public void Unlink(LoanContactRelationship relation)
    {
      if (relation.ContactType == ContactType.Biz)
      {
        this.UnlinkBizContact(relation.RelationshipType);
      }
      else
      {
        BorrowerPair borrowerPair = this.loan.BorrowerPairs[relation.BorrowerPairIndex];
        if (relation.RelationshipType == LoanContactRelationshipType.Borrower)
        {
          this.UnlinkBorrower(borrowerPair.Borrower);
        }
        else
        {
          if (relation.RelationshipType != LoanContactRelationshipType.Coborrower)
            return;
          this.UnlinkBorrower(borrowerPair.CoBorrower);
        }
      }
    }

    /// <summary>Breaks all links between contacts and the loan.</summary>
    /// <remarks>This method is equivalent to calling the pair of methods <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.LoanContacts.UnlinkAllBizContacts" />
    /// and <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.LoanContacts.UnlinkAllBorrowerContacts" />.</remarks>
    public void UnlinkAll()
    {
      this.UnlinkAllBizContacts();
      this.UnlinkAllBorrowerContacts();
    }

    /// <summary>
    /// Breaks all links between the loan and the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact" />.
    /// </summary>
    /// <param name="contact">The contact, either a <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" /> or <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact" />
    /// to be unlinked from the loan.</param>
    /// <remarks>This method will remove all links between the specified contact and the loan.</remarks>
    public void UnlinkContact(Contact contact)
    {
      if (contact == null)
        throw new ArgumentNullException(nameof (contact));
      LogList logList = this.loan.LoanData.GetLogList();
      foreach (CRMLog rec in logList.GetAllCRMMapping())
      {
        if (rec.ContactGuid == contact.Guid)
          logList.RemoveCRMMapping(rec);
      }
    }

    /// <summary>
    /// Returns an enumerator for the collection of all loan contact relationships.
    /// </summary>
    /// <returns>An IEnumerator implementation for the collection.</returns>
    /// <example>
    ///       The following code demonstrates how to iterate over the set of all contacts
    ///       linked to the current loan file.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class SampleApp
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Iterate over all of the contacts related to the current loan, opening each to
    ///       // display the user's contact information.
    ///       foreach (LoanContactRelationship relation in loan.Contacts)
    ///       {
    ///         // Retrieve the specified contact
    ///         Contact contact = session.Contacts.Open(relation.ContactID, relation.ContactType);
    /// 
    ///         // Print the contact's contact info
    ///         Console.WriteLine(relation.RelationshipType + ": "
    ///           + contact.FullName + " " + contact.PersonalEmail);
    ///       }
    /// 
    ///       // Close the loan
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public IEnumerator GetEnumerator()
    {
      LoanContactRelationshipList relationshipList = new LoanContactRelationshipList();
      foreach (CRMLog logItem in this.loan.LoanData.GetLogList().GetAllCRMMapping())
      {
        LoanContactRelationship relationship = this.createRelationship(logItem);
        if (relationship != null)
          relationshipList.Add(relationship);
      }
      return relationshipList.GetEnumerator();
    }

    private LoanContactRelationship createRelationship(CRMLog logItem)
    {
      if (logItem == null)
        return (LoanContactRelationship) null;
      ContactType contactType;
      int contactId;
      if (logItem.MappingType == CRMLogType.BusinessContact)
      {
        BizPartnerInfo bizPartner = this.loan.Session.SessionObjects.ContactManager.GetBizPartner(logItem.ContactGuid);
        if (bizPartner == null)
          return (LoanContactRelationship) null;
        contactType = ContactType.Biz;
        contactId = bizPartner.ContactID;
      }
      else
      {
        BorrowerInfo borrower = this.loan.Session.SessionObjects.ContactManager.GetBorrower(logItem.ContactGuid);
        if (borrower == null)
          return (LoanContactRelationship) null;
        contactType = ContactType.Borrower;
        contactId = borrower.ContactID;
      }
      return new LoanContactRelationship(this.loan.Session, this.loan.Guid, contactId, contactType, (LoanContactRelationshipType) logItem.RoleType, contactType == ContactType.Borrower ? logItem.GetBorrowerPairIndex() : -1);
    }
  }
}

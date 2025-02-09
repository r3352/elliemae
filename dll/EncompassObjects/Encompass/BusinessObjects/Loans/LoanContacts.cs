// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanContacts : ILoanContacts, IEnumerable
  {
    private Loan loan;

    internal LoanContacts(Loan loan) => this.loan = loan;

    public LoanContactRelationship GetBizContactRelationship(LoanContactRelationshipType relation)
    {
      CRMLog crmMapping = this.loan.LoanData.GetLogList().GetCRMMapping(((int) relation).ToString());
      return crmMapping == null ? (LoanContactRelationship) null : this.createRelationship(crmMapping);
    }

    public LoanContactRelationshipList GetBizContactRelationships()
    {
      LoanContactRelationshipList contactRelationships = new LoanContactRelationshipList();
      foreach (CRMLog logItem in this.loan.LoanData.GetLogList().GetAllCRMMapping())
      {
        if (logItem.MappingType == 1)
        {
          LoanContactRelationship relationship = this.createRelationship(logItem);
          if (relationship != null)
            contactRelationships.Add(relationship);
        }
      }
      return contactRelationships;
    }

    public LoanContactRelationship GetBorrowerRelationship(Borrower borrower)
    {
      CRMLog crmMapping = this.loan.LoanData.GetLogList().GetCRMMapping(borrower.ID);
      return crmMapping == null ? (LoanContactRelationship) null : this.createRelationship(crmMapping);
    }

    public LoanContactRelationshipList GetBorrowerRelationships()
    {
      LoanContactRelationshipList borrowerRelationships = new LoanContactRelationshipList();
      foreach (CRMLog logItem in this.loan.LoanData.GetLogList().GetAllCRMMapping())
      {
        if (logItem.MappingType == null)
        {
          LoanContactRelationship relationship = this.createRelationship(logItem);
          if (relationship != null)
            borrowerRelationships.Add(relationship);
        }
      }
      return borrowerRelationships;
    }

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

    public void LinkToBizContact(BizContact contact, LoanContactRelationshipType relationType)
    {
      if (contact == null)
        throw new ArgumentNullException(nameof (contact));
      if (contact.IsNew)
        throw new ArgumentException("The specified contact must be committed before it can be set into a loan relationship");
      this.loan.LoanData.GetLogList().AddCRMMapping(((int) relationType).ToString(), (CRMLogType) 1, contact.Unwrap().ContactGuid.ToString(), (CRMRoleType) Enum.Parse(typeof (CRMRoleType), string.Concat((object) relationType)));
    }

    public void UnlinkBizContact(LoanContactRelationshipType relationType)
    {
      LogList logList = this.loan.LoanData.GetLogList();
      CRMLog crmMapping = logList.GetCRMMapping(((int) relationType).ToString());
      if (crmMapping == null)
        return;
      logList.RemoveCRMMapping(crmMapping);
    }

    public void UnlinkAllBizContacts()
    {
      LogList logList = this.loan.LoanData.GetLogList();
      foreach (CRMLog crmLog in logList.GetAllCRMMapping())
      {
        if (crmLog.MappingType == 1)
          logList.RemoveCRMMapping(crmLog);
      }
    }

    public void LinkToBorrowerContact(BorrowerContact contact, Borrower loanBorrower)
    {
      if (contact == null)
        throw new ArgumentNullException(nameof (contact));
      if (contact.IsNew)
        throw new ArgumentException("The specified contact must be committed before it can be set into a loan relationship");
      LogList logList = this.loan.LoanData.GetLogList();
      foreach (BorrowerPair borrowerPair in this.loan.LoanData.GetBorrowerPairs())
      {
        if (borrowerPair.Borrower.Id == loanBorrower.ID)
        {
          logList.AddCRMMapping(loanBorrower.ID, (CRMLogType) 0, contact.Unwrap().ContactGuid.ToString(), (CRMRoleType) 0);
          break;
        }
        if (borrowerPair.CoBorrower != null && borrowerPair.CoBorrower.Id == loanBorrower.ID)
        {
          logList.AddCRMMapping(loanBorrower.ID, (CRMLogType) 0, contact.Unwrap().ContactGuid.ToString(), (CRMRoleType) 1);
          break;
        }
      }
    }

    public void UnlinkBorrower(Borrower loanBorrower)
    {
      LogList logList = this.loan.LoanData.GetLogList();
      CRMLog crmMapping = logList.GetCRMMapping(loanBorrower.ID);
      if (crmMapping == null)
        return;
      logList.RemoveCRMMapping(crmMapping);
    }

    public void UnlinkAllBorrowerContacts()
    {
      LogList logList = this.loan.LoanData.GetLogList();
      foreach (CRMLog crmLog in logList.GetAllCRMMapping())
      {
        if (crmLog.MappingType == null)
          logList.RemoveCRMMapping(crmLog);
      }
    }

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

    public void UnlinkAll()
    {
      this.UnlinkAllBizContacts();
      this.UnlinkAllBorrowerContacts();
    }

    public void UnlinkContact(Contact contact)
    {
      if (contact == null)
        throw new ArgumentNullException(nameof (contact));
      LogList logList = this.loan.LoanData.GetLogList();
      foreach (CRMLog crmLog in logList.GetAllCRMMapping())
      {
        if (crmLog.ContactGuid == contact.Guid)
          logList.RemoveCRMMapping(crmLog);
      }
    }

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
      if (logItem.MappingType == 1)
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

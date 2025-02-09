// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactGroup.BorrowerGroupMember
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ContactUI;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactGroup
{
  [Serializable]
  public class BorrowerGroupMember : ContactGroupMember
  {
    private BorrowerSummaryInfo borrowerInfo;

    public override string LastName => this.borrowerInfo.LastName;

    public override string FirstName => this.borrowerInfo.FirstName;

    public override string PhoneNumber => this.borrowerInfo.HomePhone;

    public override string EmailAddress => this.borrowerInfo.PersonalEmail;

    public override bool NoSpam => this.borrowerInfo.NoSpam;

    public override DateTime DateAdded => this.borrowerInfo.LastModified;

    public string OwnerId => this.borrowerInfo.OwnerID;

    public string BorrowerType
    {
      get => Enum.Format(typeof (EllieMae.EMLite.Common.Contact.BorrowerType), (object) this.borrowerInfo.ContactType, "G");
    }

    public string Status => this.borrowerInfo.Status;

    public static BorrowerGroupMember NewBorrowerGroupMember(
      int groupId,
      BorrowerSummaryInfo borrowerInfo)
    {
      return new BorrowerGroupMember(groupId, borrowerInfo);
    }

    public static BorrowerGroupMember NewBorrowerGroupMember(
      int groupId,
      BorrowerGroupMember borrower)
    {
      return new BorrowerGroupMember(groupId, borrower);
    }

    public static BorrowerGroupMember NewBorrowerGroupMember(int groupId, int contactId)
    {
      return new BorrowerGroupMember(groupId, contactId);
    }

    protected BorrowerGroupMember(int groupId, BorrowerSummaryInfo borrowerInfo)
      : base(groupId, borrowerInfo.ContactID, ContactType.Borrower)
    {
      this.MarkOld();
      this.borrowerInfo = borrowerInfo;
    }

    protected BorrowerGroupMember(int groupId, BorrowerGroupMember borrower)
      : base(groupId, borrower.ContactId, ContactType.Borrower)
    {
      this.MarkNew();
      this.borrowerInfo = new BorrowerSummaryInfo(new Hashtable());
      this.borrowerInfo.ContactID = borrower.ContactId;
      this.borrowerInfo.ContactType = (EllieMae.EMLite.Common.Contact.BorrowerType) Enum.Parse(typeof (EllieMae.EMLite.Common.Contact.BorrowerType), borrower.BorrowerType, true);
      this.borrowerInfo.FirstName = borrower.FirstName;
      this.borrowerInfo.LastModified = DateTime.Today;
      this.borrowerInfo.LastName = borrower.LastName;
      this.borrowerInfo.NoSpam = borrower.NoSpam;
      this.borrowerInfo.OwnerID = borrower.OwnerId;
      this.borrowerInfo.Status = borrower.Status;
    }

    protected BorrowerGroupMember(int groupId, int contactId)
      : base(groupId, contactId, ContactType.Borrower)
    {
      this.MarkNew();
      this.borrowerInfo = new BorrowerSummaryInfo(new Hashtable());
      this.borrowerInfo.ContactID = contactId;
      this.borrowerInfo.NoSpam = false;
      this.borrowerInfo.LastModified = DateTime.Today;
    }
  }
}

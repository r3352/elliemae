// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactGroup.PartnerGroupMember
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ContactGroup
{
  [Serializable]
  public class PartnerGroupMember : ContactGroupMember
  {
    private BizPartnerSummaryInfo partnerInfo;

    public override string LastName => this.partnerInfo.LastName;

    public override string FirstName => this.partnerInfo.FirstName;

    public override string PhoneNumber => this.partnerInfo.WorkPhone;

    public override string EmailAddress => this.partnerInfo.BizEmail;

    public override bool NoSpam => this.partnerInfo.NoSpam;

    public override DateTime DateAdded => this.partnerInfo.LastModified;

    public int CategoryId => this.partnerInfo.CategoryID;

    public string CompanyName => this.partnerInfo.CompanyName;

    public ContactAccess AccessLevel => this.partnerInfo.AccessLevel;

    public static PartnerGroupMember NewPartnerGroupMember(
      int groupId,
      BizPartnerSummaryInfo partnerInfo)
    {
      return new PartnerGroupMember(groupId, partnerInfo);
    }

    public static PartnerGroupMember NewPartnerGroupMember(int groupId, PartnerGroupMember partner)
    {
      return new PartnerGroupMember(groupId, partner);
    }

    public static PartnerGroupMember NewPartnerGroupMember(int groupId, int contactId)
    {
      return new PartnerGroupMember(groupId, contactId);
    }

    protected PartnerGroupMember(int groupId, BizPartnerSummaryInfo partnerInfo)
      : base(groupId, partnerInfo.ContactID, ContactType.BizPartner)
    {
      this.MarkOld();
      this.partnerInfo = partnerInfo;
    }

    protected PartnerGroupMember(int groupId, PartnerGroupMember partner)
      : base(groupId, partner.ContactId, ContactType.BizPartner)
    {
      this.MarkNew();
      this.partnerInfo = new BizPartnerSummaryInfo(partner.ContactId, partner.CategoryId, partner.CompanyName, partner.LastName, partner.FirstName, partner.PhoneNumber, partner.EmailAddress, partner.NoSpam, partner.AccessLevel, DateTime.Today);
    }

    protected PartnerGroupMember(int groupId, int contactId)
      : base(groupId, contactId, ContactType.BizPartner)
    {
      this.MarkNew();
      this.partnerInfo = new BizPartnerSummaryInfo(contactId, 0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, ContactAccess.Public, DateTime.Today);
    }
  }
}

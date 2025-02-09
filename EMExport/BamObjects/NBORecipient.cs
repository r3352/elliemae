// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.BamObjects.NBORecipient
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Export.BamObjects
{
  public class NBORecipient : DisclosureRecipient
  {
    public NBORecipient(
      string id,
      string firstName,
      string middleName,
      string lastName,
      string suffix,
      Address address,
      string vestingType,
      string homePhone,
      bool isNoThirdPartyEmail,
      string businessPhone,
      string cell,
      string fax,
      DateTime dateOfBirth,
      string orderId,
      string tRGuid,
      string guid,
      string email,
      string role,
      string roleDescription,
      string disclosedMethod,
      string disclosedMethodOther,
      string borrowerType,
      DateTime presumedReceivedDate,
      DateTime actualReceivedDate,
      DisclosureRecipientTracking tracking)
      : base(id, firstName + middleName + lastName, email, role, roleDescription, disclosedMethod, disclosedMethodOther, borrowerType, presumedReceivedDate, actualReceivedDate, tracking)
    {
      this.FirstName = firstName;
      this.MiddleName = middleName;
      this.LastName = lastName;
      this.Suffix = suffix;
      this.Address = address;
      this.VestingType = vestingType;
      this.HomePhone = homePhone;
      this.IsNoThirdPartyEmail = isNoThirdPartyEmail;
      this.BusinessPhone = businessPhone;
      this.Cell = cell;
      this.Fax = fax;
      this.DateOfBirth = dateOfBirth;
      this.OrderId = orderId;
      this.TRGuid = tRGuid;
      this.Guid = guid;
    }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string Suffix { get; set; }

    public Address Address { get; set; }

    public string VestingType { get; set; }

    public string HomePhone { get; set; }

    public bool IsNoThirdPartyEmail { get; set; }

    public string BusinessPhone { get; set; }

    public string Cell { get; set; }

    public string Fax { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string OrderId { get; set; }

    public string TRGuid { get; set; }

    public string Guid { get; set; }
  }
}

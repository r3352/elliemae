// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.NBORecipient
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
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

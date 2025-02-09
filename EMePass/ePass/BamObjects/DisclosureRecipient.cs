// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
{
  public class DisclosureRecipient
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }

    public string RoleDescription { get; set; }

    public string DisclosedMethod { get; set; }

    public string DisclosedMethodOther { get; set; }

    public string BorrowerType { get; set; }

    public DateTime PresumedReceivedDate { get; set; }

    public DateTime ActualReceivedDate { get; set; }

    public string UserId { get; set; }

    public DisclosureRecipientTracking Tracking { get; set; }

    public DisclosureRecipient(
      string id,
      string name,
      string email,
      string role,
      string roleDescription,
      string disclosedMethod,
      string disclosedMethodOther,
      string borrowerType,
      DateTime presumedReceivedDate,
      DateTime actualReceivedDate,
      DisclosureRecipientTracking tracking,
      string userId = "")
    {
      this.Id = id;
      this.Name = name;
      this.Email = email;
      this.Role = role;
      this.RoleDescription = roleDescription;
      this.DisclosedMethod = disclosedMethod;
      this.DisclosedMethodOther = disclosedMethodOther;
      this.BorrowerType = borrowerType;
      this.PresumedReceivedDate = presumedReceivedDate;
      this.ActualReceivedDate = actualReceivedDate;
      this.Tracking = tracking;
      this.UserId = userId;
    }
  }
}

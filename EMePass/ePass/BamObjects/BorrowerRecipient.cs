// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.BorrowerRecipient
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
{
  public class BorrowerRecipient : DisclosureRecipient
  {
    public string BorrowerPairId { get; set; }

    public BorrowerRecipient(
      string id,
      string borrowerPairId,
      string name,
      string email,
      string role,
      string roleDescription,
      string disclosedMethod,
      string disclosedMethodOther,
      string borrowerType,
      DateTime presumedReceivedDate,
      DateTime actualReceivedDate,
      DisclosureRecipientTracking tracking)
      : base(id, name, email, role, roleDescription, disclosedMethod, disclosedMethodOther, borrowerType, presumedReceivedDate, actualReceivedDate, tracking)
    {
      this.BorrowerPairId = borrowerPairId;
    }
  }
}

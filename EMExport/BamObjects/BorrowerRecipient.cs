// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.BamObjects.BorrowerRecipient
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Export.BamObjects
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

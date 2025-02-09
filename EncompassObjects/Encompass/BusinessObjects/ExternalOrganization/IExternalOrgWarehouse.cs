// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.IExternalOrgWarehouse
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Interface for IExternalOrgWarehouse class</summary>
  public interface IExternalOrgWarehouse
  {
    int WarehouseID { get; }

    int BankID { get; }

    int ExternalOrgID { get; }

    string BankName { get; }

    string Address { get; }

    string Address1 { get; }

    string City { get; }

    string State { get; }

    string Zip { get; }

    string ABANumber { get; }

    DateTime DateAdded { get; }

    bool UseBankContact { get; set; }

    string ContactName { get; set; }

    string ContactEmail { get; set; }

    string ContactPhone { get; set; }

    string ContactFax { get; set; }

    string BankContactName { get; set; }

    string BankContactEmail { get; set; }

    string BankContactPhone { get; set; }

    string BankContactFax { get; set; }

    string Notes { get; set; }

    string AcctNumber { get; set; }

    string Description { get; set; }

    int SelfFunder { get; set; }

    int BaileeReq { get; set; }

    DateTime Expiration { get; set; }

    int TriParty { get; set; }

    string OrgName { get; set; }

    int OrgType { get; set; }

    bool Approved { get; set; }

    DateTime StatusDate { get; set; }

    string AcctName { get; set; }

    string CreditAcctNumber { get; set; }

    string CreditAcctName { get; set; }
  }
}

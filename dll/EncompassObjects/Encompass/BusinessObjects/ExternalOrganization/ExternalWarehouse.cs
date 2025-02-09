// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalWarehouse
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class ExternalWarehouse : IExternalOrgWarehouse
  {
    private ExternalOrgWarehouse externalOrgWarehouse;

    internal ExternalWarehouse(ExternalOrgWarehouse externalOrgWarehouse)
    {
      this.externalOrgWarehouse = externalOrgWarehouse;
    }

    public int WarehouseID => this.externalOrgWarehouse.WarehouseID;

    public int BankID => this.externalOrgWarehouse.BankID;

    public int ExternalOrgID => this.externalOrgWarehouse.ExternalOrgID;

    public string BankName => this.externalOrgWarehouse.BankName;

    public string Address => this.externalOrgWarehouse.Address;

    public string Address1 => this.externalOrgWarehouse.Address1;

    public string City => this.externalOrgWarehouse.City;

    public string State => this.externalOrgWarehouse.State;

    public string Zip => this.externalOrgWarehouse.Zip;

    public string ABANumber => this.externalOrgWarehouse.ABANumber;

    public DateTime DateAdded => this.externalOrgWarehouse.DateAdded;

    public bool UseBankContact
    {
      get => this.externalOrgWarehouse.UseBankContact;
      set => this.externalOrgWarehouse.UseBankContact = value;
    }

    public string ContactName
    {
      get => this.externalOrgWarehouse.ContactName;
      set => this.externalOrgWarehouse.ContactName = value;
    }

    public string ContactEmail
    {
      get => this.externalOrgWarehouse.ContactEmail;
      set => this.externalOrgWarehouse.ContactEmail = value;
    }

    public string ContactPhone
    {
      get => this.externalOrgWarehouse.ContactPhone;
      set => this.externalOrgWarehouse.ContactPhone = value;
    }

    public string ContactFax
    {
      get => this.externalOrgWarehouse.ContactFax;
      set => this.externalOrgWarehouse.ContactFax = value;
    }

    public string BankContactName
    {
      get => this.externalOrgWarehouse.BankContactName;
      set => this.externalOrgWarehouse.BankContactName = value;
    }

    public string BankContactEmail
    {
      get => this.externalOrgWarehouse.BankContactEmail;
      set => this.externalOrgWarehouse.BankContactEmail = value;
    }

    public string BankContactPhone
    {
      get => this.externalOrgWarehouse.BankContactPhone;
      set => this.externalOrgWarehouse.BankContactPhone = value;
    }

    public string BankContactFax
    {
      get => this.externalOrgWarehouse.BankContactFax;
      set => this.externalOrgWarehouse.BankContactFax = value;
    }

    public string Notes
    {
      get => this.externalOrgWarehouse.Notes;
      set => this.externalOrgWarehouse.Notes = value;
    }

    public string AcctNumber
    {
      get => this.externalOrgWarehouse.AcctNumber;
      set => this.externalOrgWarehouse.AcctNumber = value;
    }

    public string Description
    {
      get => this.externalOrgWarehouse.Description;
      set => this.externalOrgWarehouse.Description = value;
    }

    public int SelfFunder
    {
      get => this.externalOrgWarehouse.SelfFunder;
      set => this.externalOrgWarehouse.SelfFunder = value;
    }

    public int BaileeReq
    {
      get => this.externalOrgWarehouse.BaileeReq;
      set => this.externalOrgWarehouse.BaileeReq = value;
    }

    public DateTime Expiration
    {
      get => this.externalOrgWarehouse.Expiration;
      set => this.externalOrgWarehouse.Expiration = value;
    }

    public int TriParty
    {
      get => this.externalOrgWarehouse.TriParty;
      set => this.externalOrgWarehouse.TriParty = value;
    }

    internal static ExternalWarehouseList ToList(List<ExternalOrgWarehouse> obj)
    {
      ExternalWarehouseList list = new ExternalWarehouseList();
      for (int index = 0; index < obj.Count; ++index)
        list.Add(new ExternalWarehouse(obj[index]));
      return list;
    }

    public string OrgName
    {
      get => this.externalOrgWarehouse.OrgName;
      set => this.externalOrgWarehouse.OrgName = value;
    }

    public int OrgType
    {
      get => this.externalOrgWarehouse.OrgType;
      set => this.externalOrgWarehouse.OrgType = value;
    }

    public bool Approved
    {
      get => this.externalOrgWarehouse.Approved;
      set => this.externalOrgWarehouse.Approved = value;
    }

    public string AcctName
    {
      get => this.externalOrgWarehouse.AcctName;
      set => this.externalOrgWarehouse.AcctName = value;
    }

    public string CreditAcctNumber
    {
      get => this.externalOrgWarehouse.CreditAcctNumber;
      set => this.externalOrgWarehouse.CreditAcctNumber = value;
    }

    public string TimeZone
    {
      get => this.externalOrgWarehouse.TimeZone;
      set => this.externalOrgWarehouse.TimeZone = value;
    }

    public string CreditAcctName
    {
      get => this.externalOrgWarehouse.CreditAcctName;
      set => this.externalOrgWarehouse.CreditAcctName = value;
    }

    public DateTime StatusDate
    {
      get => this.externalOrgWarehouse.StatusDate;
      set => this.externalOrgWarehouse.StatusDate = value;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalWarehouse
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents a single external warehouse</summary>
  public class ExternalWarehouse : IExternalOrgWarehouse
  {
    private ExternalOrgWarehouse externalOrgWarehouse;

    internal ExternalWarehouse(ExternalOrgWarehouse externalOrgWarehouse)
    {
      this.externalOrgWarehouse = externalOrgWarehouse;
    }

    /// <summary>gets external warehouseID</summary>
    public int WarehouseID => this.externalOrgWarehouse.WarehouseID;

    /// <summary>gets external bankID</summary>
    public int BankID => this.externalOrgWarehouse.BankID;

    /// <summary>gets externalOrgID</summary>
    public int ExternalOrgID => this.externalOrgWarehouse.ExternalOrgID;

    /// <summary>gets externalBankName</summary>
    public string BankName => this.externalOrgWarehouse.BankName;

    /// <summary>gets address</summary>
    public string Address => this.externalOrgWarehouse.Address;

    /// <summary>gets address1</summary>
    public string Address1 => this.externalOrgWarehouse.Address1;

    /// <summary>gets city</summary>
    public string City => this.externalOrgWarehouse.City;

    /// <summary>gets state</summary>
    public string State => this.externalOrgWarehouse.State;

    /// <summary>gets zip</summary>
    public string Zip => this.externalOrgWarehouse.Zip;

    /// <summary>gets/sets ABANumber</summary>
    public string ABANumber => this.externalOrgWarehouse.ABANumber;

    /// <summary>gets DateAdded</summary>
    public DateTime DateAdded => this.externalOrgWarehouse.DateAdded;

    /// <summary>gets/sets useBankContact</summary>
    public bool UseBankContact
    {
      get => this.externalOrgWarehouse.UseBankContact;
      set => this.externalOrgWarehouse.UseBankContact = value;
    }

    /// <summary>gets/sets contactName</summary>
    public string ContactName
    {
      get => this.externalOrgWarehouse.ContactName;
      set => this.externalOrgWarehouse.ContactName = value;
    }

    /// <summary>gets/sets contactEmail</summary>
    public string ContactEmail
    {
      get => this.externalOrgWarehouse.ContactEmail;
      set => this.externalOrgWarehouse.ContactEmail = value;
    }

    /// <summary>gets/sets ContactPhone</summary>
    public string ContactPhone
    {
      get => this.externalOrgWarehouse.ContactPhone;
      set => this.externalOrgWarehouse.ContactPhone = value;
    }

    /// <summary>gets/sets ContactFax</summary>
    public string ContactFax
    {
      get => this.externalOrgWarehouse.ContactFax;
      set => this.externalOrgWarehouse.ContactFax = value;
    }

    /// <summary>gets/sets BankContactName</summary>
    public string BankContactName
    {
      get => this.externalOrgWarehouse.BankContactName;
      set => this.externalOrgWarehouse.BankContactName = value;
    }

    /// <summary>gets/sets BankContactEmail</summary>
    public string BankContactEmail
    {
      get => this.externalOrgWarehouse.BankContactEmail;
      set => this.externalOrgWarehouse.BankContactEmail = value;
    }

    /// <summary>gets/sets BankContactPhone</summary>
    public string BankContactPhone
    {
      get => this.externalOrgWarehouse.BankContactPhone;
      set => this.externalOrgWarehouse.BankContactPhone = value;
    }

    /// <summary>gets/sets BankContactFax</summary>
    public string BankContactFax
    {
      get => this.externalOrgWarehouse.BankContactFax;
      set => this.externalOrgWarehouse.BankContactFax = value;
    }

    /// <summary>gets/sets notes</summary>
    public string Notes
    {
      get => this.externalOrgWarehouse.Notes;
      set => this.externalOrgWarehouse.Notes = value;
    }

    /// <summary>gets/sets AcctNumber</summary>
    public string AcctNumber
    {
      get => this.externalOrgWarehouse.AcctNumber;
      set => this.externalOrgWarehouse.AcctNumber = value;
    }

    /// <summary>gets/sets Description</summary>
    public string Description
    {
      get => this.externalOrgWarehouse.Description;
      set => this.externalOrgWarehouse.Description = value;
    }

    /// <summary>gets/sets SelfFunder</summary>
    public int SelfFunder
    {
      get => this.externalOrgWarehouse.SelfFunder;
      set => this.externalOrgWarehouse.SelfFunder = value;
    }

    /// <summary>gets/sets BaileeReq (Bailee Required)</summary>
    public int BaileeReq
    {
      get => this.externalOrgWarehouse.BaileeReq;
      set => this.externalOrgWarehouse.BaileeReq = value;
    }

    /// <summary>gets/sets Expiration (Bailee Expiration Date)</summary>
    public DateTime Expiration
    {
      get => this.externalOrgWarehouse.Expiration;
      set => this.externalOrgWarehouse.Expiration = value;
    }

    /// <summary>gets/sets TriParty</summary>
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

    /// <summary>gets/sets OrgName</summary>
    public string OrgName
    {
      get => this.externalOrgWarehouse.OrgName;
      set => this.externalOrgWarehouse.OrgName = value;
    }

    /// <summary>gets/sets OrgType</summary>
    public int OrgType
    {
      get => this.externalOrgWarehouse.OrgType;
      set => this.externalOrgWarehouse.OrgType = value;
    }

    /// <summary>gets/sets Approved flag</summary>
    public bool Approved
    {
      get => this.externalOrgWarehouse.Approved;
      set => this.externalOrgWarehouse.Approved = value;
    }

    /// <summary>gets/sets AcctName</summary>
    public string AcctName
    {
      get => this.externalOrgWarehouse.AcctName;
      set => this.externalOrgWarehouse.AcctName = value;
    }

    /// <summary>gets/sets CreditAcctNumber</summary>
    public string CreditAcctNumber
    {
      get => this.externalOrgWarehouse.CreditAcctNumber;
      set => this.externalOrgWarehouse.CreditAcctNumber = value;
    }

    /// <summary>gets/sets TimeZone</summary>
    public string TimeZone
    {
      get => this.externalOrgWarehouse.TimeZone;
      set => this.externalOrgWarehouse.TimeZone = value;
    }

    /// <summary>gets/sets CreditAcctName</summary>
    public string CreditAcctName
    {
      get => this.externalOrgWarehouse.CreditAcctName;
      set => this.externalOrgWarehouse.CreditAcctName = value;
    }

    /// <summary>Gets or Sets Status Date</summary>
    public DateTime StatusDate
    {
      get => this.externalOrgWarehouse.StatusDate;
      set => this.externalOrgWarehouse.StatusDate = value;
    }
  }
}

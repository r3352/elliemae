// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalBanks
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
  public class ExternalBanks : IExternalBanks
  {
    private ExternalBank externalBank;

    internal ExternalBanks(ExternalBank externalBank) => this.externalBank = externalBank;

    public int BankID => this.externalBank.BankID;

    public string BankName => this.externalBank.BankName;

    public string Address => this.externalBank.Address;

    public string Address1 => this.externalBank.Address1;

    public string City => this.externalBank.City;

    public string State => this.externalBank.State;

    public string Zip => this.externalBank.Zip;

    public string ContactName => this.externalBank.ContactName;

    public string ContactEmail => this.externalBank.ContactEmail;

    public string ContactPhone => this.externalBank.ContactPhone;

    public string ContactFax => this.externalBank.ContactFax;

    public string ABANumber => this.externalBank.ABANumber;

    public string TimeZone => this.externalBank.TimeZone;

    public DateTime DateAdded => this.externalBank.DateAdded;

    internal static ExternalBanksList ToList(List<ExternalBank> obj)
    {
      ExternalBanksList list = new ExternalBanksList();
      for (int index = 0; index < obj.Count; ++index)
        list.Add(new ExternalBanks(obj[index]));
      return list;
    }
  }
}

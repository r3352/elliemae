// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalBanks
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
  /// <summary>Represents a single external bank</summary>
  public class ExternalBanks : IExternalBanks
  {
    private ExternalBank externalBank;

    internal ExternalBanks(ExternalBank externalBank) => this.externalBank = externalBank;

    /// <summary>gets external bankID</summary>
    public int BankID => this.externalBank.BankID;

    /// <summary>gets externalBankName</summary>
    public string BankName => this.externalBank.BankName;

    /// <summary>gets address</summary>
    public string Address => this.externalBank.Address;

    /// <summary>gets Address1</summary>
    public string Address1 => this.externalBank.Address1;

    /// <summary>gets city</summary>
    public string City => this.externalBank.City;

    /// <summary>gets state</summary>
    public string State => this.externalBank.State;

    /// <summary>gets zip</summary>
    public string Zip => this.externalBank.Zip;

    /// <summary>gets ContactName</summary>
    public string ContactName => this.externalBank.ContactName;

    /// <summary>gets ContactEmail</summary>
    public string ContactEmail => this.externalBank.ContactEmail;

    /// <summary>gets ContactPhone</summary>
    public string ContactPhone => this.externalBank.ContactPhone;

    /// <summary>gets ContactFax</summary>
    public string ContactFax => this.externalBank.ContactFax;

    /// <summary>gets ABANumber</summary>
    public string ABANumber => this.externalBank.ABANumber;

    /// <summary>gets TimeZone</summary>
    public string TimeZone => this.externalBank.TimeZone;

    /// <summary>gets DateAdded</summary>
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

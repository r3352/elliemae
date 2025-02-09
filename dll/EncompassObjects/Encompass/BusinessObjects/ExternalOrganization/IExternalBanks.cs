// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.IExternalBanks
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public interface IExternalBanks
  {
    int BankID { get; }

    string BankName { get; }

    string Address { get; }

    string Address1 { get; }

    string City { get; }

    string State { get; }

    string Zip { get; }

    string ContactName { get; }

    string ContactEmail { get; }

    string ContactPhone { get; }

    string ContactFax { get; }

    string ABANumber { get; }

    DateTime DateAdded { get; }

    string TimeZone { get; }
  }
}

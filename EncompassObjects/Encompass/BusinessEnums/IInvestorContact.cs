// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.IInvestorContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [Guid("361FF768-B9CC-470D-8373-2E0CA7D3A4CE")]
  public interface IInvestorContact
  {
    InvestorContactType ContactType { get; }

    string EntityName { get; }

    string ContactName { get; }

    string Street1 { get; }

    string Street2 { get; }

    string City { get; }

    string State { get; }

    string Zip { get; }

    string PhoneNumber { get; }

    string FaxNumber { get; }

    string EmailAddress { get; }

    string WebSite { get; }
  }
}

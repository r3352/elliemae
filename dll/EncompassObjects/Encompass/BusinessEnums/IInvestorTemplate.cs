// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.IInvestorTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [Guid("C5664319-71EB-42CB-BC26-4CC5F2981BE4")]
  public interface IInvestorTemplate
  {
    bool BulkSale { get; }

    int DeliveryTimeFrame { get; }

    string TypeOfPurchaser { get; }

    InvestorContacts Contacts { get; }
  }
}

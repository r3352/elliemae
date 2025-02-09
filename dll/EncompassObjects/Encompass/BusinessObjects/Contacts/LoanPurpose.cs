// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.LoanPurpose
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [Guid("17B92736-7FF8-4c3f-BA90-9AF3BA4C32E4")]
  public enum LoanPurpose
  {
    Unspecified,
    Purchase,
    CashOutRefi,
    NoCashOutRefi,
    Construction,
    ConstructionPerm,
    Other,
  }
}

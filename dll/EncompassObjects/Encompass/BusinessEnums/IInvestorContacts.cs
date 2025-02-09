// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.IInvestorContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [Guid("8D74D563-16D7-4783-93B4-2814D0667E27")]
  public interface IInvestorContacts
  {
    InvestorContact this[int index] { get; }

    InvestorContact this[InvestorContactType contactType] { get; }

    int Count { get; }

    IEnumerator GetEnumerator();
  }
}

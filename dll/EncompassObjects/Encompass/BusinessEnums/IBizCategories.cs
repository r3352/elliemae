// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.IBizCategories
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [Guid("22D8F029-5D18-45cc-AF0F-3A132E03FA60")]
  public interface IBizCategories
  {
    BizCategory this[int index] { get; }

    BizCategory GetItemByID(int itemId);

    BizCategory GetItemByName(string itemName);

    int Count { get; }

    IEnumerator GetEnumerator();
  }
}

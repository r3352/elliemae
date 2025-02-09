// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.IContactStatuses
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [Guid("1402686C-6573-4ea0-BA94-9C04D4780FFD")]
  public interface IContactStatuses
  {
    ContactStatus this[int index] { get; }

    ContactStatus GetItemByID(int itemId);

    ContactStatus GetItemByName(string itemName);

    int Count { get; }

    IEnumerator GetEnumerator();
  }
}

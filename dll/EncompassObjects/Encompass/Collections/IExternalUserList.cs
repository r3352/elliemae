// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IExternalUserList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("AECFBAF9-E220-4DE0-B910-3350948FF94E")]
  public interface IExternalUserList
  {
    ExternalUser this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(ExternalUser value);

    bool Contains(ExternalUser value);

    int IndexOf(ExternalUser value);

    void Insert(int index, ExternalUser value);

    void Remove(ExternalUser value);

    ExternalUser[] ToArray();

    IEnumerator GetEnumerator();
  }
}

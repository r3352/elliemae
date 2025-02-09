// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IPersonaList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("37EB319E-A0FA-4598-AFF2-A16DE13CFC60")]
  public interface IPersonaList
  {
    Persona this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(Persona value);

    bool Contains(Persona value);

    int IndexOf(Persona value);

    void Insert(int index, Persona value);

    void Remove(Persona value);

    Persona[] ToArray();

    IEnumerator GetEnumerator();
  }
}

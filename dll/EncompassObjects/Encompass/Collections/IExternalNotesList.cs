// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IExternalNotesList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public interface IExternalNotesList
  {
    ExternalNote this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(ExternalNote value);

    bool Contains(ExternalNote value);

    void Remove(ExternalNote value);

    ExternalNote[] ToArray();

    IEnumerator GetEnumerator();
  }
}

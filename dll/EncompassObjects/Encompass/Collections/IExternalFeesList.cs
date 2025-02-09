// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IExternalFeesList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public interface IExternalFeesList
  {
    ExternalFees this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(ExternalFees value);

    bool Contains(ExternalFees value);

    void Remove(ExternalFees value);

    ExternalFees[] ToArray();

    IEnumerator GetEnumerator();
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IExternalWarehouseList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public interface IExternalWarehouseList
  {
    ExternalWarehouse this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(ExternalWarehouse value);

    bool Contains(ExternalWarehouse value);

    void Remove(ExternalWarehouse value);

    ExternalWarehouse[] ToArray();

    IEnumerator GetEnumerator();
  }
}

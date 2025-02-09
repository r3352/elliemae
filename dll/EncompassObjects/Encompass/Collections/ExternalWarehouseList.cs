// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalWarehouseList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ExternalWarehouseList : ListBase, IExternalWarehouseList
  {
    public ExternalWarehouseList()
      : base(typeof (ExternalWarehouse))
    {
    }

    public ExternalWarehouseList(IList source)
      : base(typeof (ExternalWarehouse), source)
    {
    }

    public ExternalWarehouse this[int index]
    {
      get => (ExternalWarehouse) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ExternalWarehouse value) => this.List.Add((object) value);

    public bool Contains(ExternalWarehouse value) => this.List.Contains((object) value);

    public void Remove(ExternalWarehouse value) => this.List.Remove((object) value);

    public ExternalWarehouse[] ToArray()
    {
      ExternalWarehouse[] array = new ExternalWarehouse[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}

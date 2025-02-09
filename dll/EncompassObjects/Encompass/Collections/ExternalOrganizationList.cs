// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalOrganizationList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ExternalOrganizationList : ListBase, IExternalOrganizationList
  {
    public ExternalOrganizationList()
      : base(typeof (EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization))
    {
    }

    public ExternalOrganizationList(IList source)
      : base(typeof (EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization), source)
    {
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization this[
      int index]
    {
      get => (EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization value)
    {
      this.List.Add((object) value);
    }

    public bool Contains(EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization value)
    {
      return this.List.Contains((object) value);
    }

    public int IndexOf(EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization value)
    {
      return this.List.IndexOf((object) value);
    }

    public void Insert(int index, EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization value)
    {
      this.List.Insert(index, (object) value);
    }

    public void Remove(EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization value)
    {
      this.List.Remove((object) value);
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization[] ToArray()
    {
      EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization[] array = new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IContactOpportunityList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("B822F9DD-5246-48f2-81BD-50A03DD88577")]
  public interface IContactOpportunityList
  {
    ContactOpportunity this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(ContactOpportunity value);

    bool Contains(ContactOpportunity value);

    int IndexOf(ContactOpportunity value);

    void Insert(int index, ContactOpportunity value);

    void Remove(ContactOpportunity value);

    ContactOpportunity[] ToArray();

    IEnumerator GetEnumerator();
  }
}

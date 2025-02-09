// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IFieldDescriptorList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("BA0BED19-10BF-4b59-9603-63FE4505DC6C")]
  public interface IFieldDescriptorList
  {
    FieldDescriptor this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(FieldDescriptor value);

    bool Contains(FieldDescriptor value);

    int IndexOf(FieldDescriptor value);

    void Insert(int index, FieldDescriptor value);

    void Remove(FieldDescriptor value);

    FieldDescriptor[] ToArray();

    IEnumerator GetEnumerator();
  }
}

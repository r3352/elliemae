// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IIntegerList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for IntegerList class.</summary>
  /// <exclude />
  [Guid("6250E58A-8239-42ed-885E-3CC9025E9FB0")]
  public interface IIntegerList
  {
    int this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(int value);

    bool Contains(int value);

    int IndexOf(int value);

    void Insert(int index, int value);

    void Remove(int value);

    int[] ToArray();

    IEnumerator GetEnumerator();
  }
}

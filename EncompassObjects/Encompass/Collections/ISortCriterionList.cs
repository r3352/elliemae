// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ISortCriterionList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Query;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for SortCriterionList class.</summary>
  /// <exclude />
  [Guid("91E487D8-94AC-4f75-B218-9EE2E87FE51E")]
  public interface ISortCriterionList
  {
    SortCriterion this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(SortCriterion value);

    bool Contains(SortCriterion value);

    int IndexOf(SortCriterion value);

    void Insert(int index, SortCriterion value);

    void Remove(SortCriterion value);

    SortCriterion[] ToArray();

    IEnumerator GetEnumerator();
  }
}

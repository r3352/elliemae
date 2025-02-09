// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IPipelineDataList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for PipelineDataList class.</summary>
  /// <exclude />
  [Guid("F285741C-E797-466d-9F54-E85F8B14791C")]
  public interface IPipelineDataList
  {
    PipelineData this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(PipelineData value);

    bool Contains(PipelineData value);

    int IndexOf(PipelineData value);

    void Insert(int index, PipelineData value);

    void Remove(PipelineData value);

    PipelineData[] ToArray();

    IEnumerator GetEnumerator();
  }
}

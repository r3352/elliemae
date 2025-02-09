// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ITaskTemplateList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for TaskTemplateList class.</summary>
  /// <exclude />
  [Guid("197BFF24-240D-46bc-80AF-291D0E4A56BE")]
  public interface ITaskTemplateList
  {
    TaskTemplate this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(TaskTemplate value);

    bool Contains(TaskTemplate value);

    int IndexOf(TaskTemplate value);

    void Insert(int index, TaskTemplate value);

    void Remove(TaskTemplate value);

    TaskTemplate[] ToArray();

    IEnumerator GetEnumerator();
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IPageImageAnnotations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for PageImageAnnotations class.</summary>
  /// <exclude />
  [Guid("4c78dcd5-c0da-4ac6-a567-54575742ea69")]
  public interface IPageImageAnnotations
  {
    int Count { get; }

    int Add(PageImageAnnotation annotation);

    void Remove(PageImageAnnotation annotation);

    PageImageAnnotation this[int index] { get; }

    IEnumerator GetEnumerator();
  }
}

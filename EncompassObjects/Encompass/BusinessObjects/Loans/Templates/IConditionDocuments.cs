// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.IConditionDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents the interface for the ConditionDocuments object.
  /// </summary>
  /// <exclude />
  [Guid("81754D35-CFC8-461d-A2CF-71C06A5CFA90")]
  public interface IConditionDocuments
  {
    int Count { get; }

    DocumentTemplate this[int index] { get; }

    DocumentTemplate GetTemplateByID(string templateId);

    bool Contains(DocumentTemplate template);

    IEnumerator GetEnumerator();
  }
}

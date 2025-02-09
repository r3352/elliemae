// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogPostClosingConditions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for PostClosingCondition class.</summary>
  /// <exclude />
  [Guid("BBA8EC65-1969-4170-A3E1-2FE9B84A6838")]
  public interface ILogPostClosingConditions
  {
    int Count { get; }

    PostClosingCondition this[int index] { get; }

    PostClosingCondition Add(string conditionTitle);

    void Remove(PostClosingCondition condEntry);

    IEnumerator GetEnumerator();

    LogEntryList GetConditionsByTitle(string conditionTitle);

    PostClosingCondition AddFromTemplate(PostClosingConditionTemplate template);
  }
}

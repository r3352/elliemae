// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogPreliminaryConditions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for PreliminaryCondition class.</summary>
  /// <exclude />
  [Guid("DB3B8662-7D25-4fa1-957B-A0DF3A8FFDE8")]
  public interface ILogPreliminaryConditions
  {
    int Count { get; }

    PreliminaryCondition this[int index] { get; }

    PreliminaryCondition Add(string conditionTitle);

    void Remove(PreliminaryCondition condEntry);

    IEnumerator GetEnumerator();

    LogEntryList GetConditionsByTitle(string conditionTitle);
  }
}

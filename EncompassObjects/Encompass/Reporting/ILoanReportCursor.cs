// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.ILoanReportCursor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  /// <summary>Interface for LoanReportCursor class.</summary>
  /// <exclude />
  [Guid("72CF13C2-8702-4196-9ED5-B519482B2392")]
  public interface ILoanReportCursor
  {
    int Count { get; }

    void Close();

    IEnumerator GetEnumerator();

    LoanReportData GetItem(int index);

    LoanReportDataList GetItems(int startIndex, int count);
  }
}

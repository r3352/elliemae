// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.ILoanReportData
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  /// <summary>Interface for LoanReportData class.</summary>
  /// <exclude />
  [System.Runtime.InteropServices.Guid("D9F22508-DA41-4f86-B94E-CB8D9F5469D8")]
  public interface ILoanReportData
  {
    string Guid { get; }

    string this[string fname] { get; }

    StringList GetFieldNames();
  }
}

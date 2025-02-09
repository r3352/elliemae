// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.ILoanReportData
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  [System.Runtime.InteropServices.Guid("D9F22508-DA41-4f86-B94E-CB8D9F5469D8")]
  public interface ILoanReportData
  {
    string Guid { get; }

    string this[string fname] { get; }

    StringList GetFieldNames();
  }
}

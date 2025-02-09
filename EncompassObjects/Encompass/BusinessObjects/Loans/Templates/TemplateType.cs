// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Enumerator of the template types supported by Encompass.
  /// </summary>
  [Guid("D77D4EEB-86E8-4ebf-865F-9AE1795CA73C")]
  public enum TemplateType
  {
    /// <summary>Loan Program template</summary>
    LoanProgram = 1,
    /// <summary>Closing Cost template</summary>
    ClosingCost = 2,
    /// <summary>Data template</summary>
    DataTemplate = 3,
    /// <summary>Input Form Set template</summary>
    InputFormSet = 4,
    /// <summary>Document Set template</summary>
    DocumentSet = 5,
    /// <summary>Loan Template</summary>
    LoanTemplate = 6,
    /// <summary>Investor Template</summary>
    Investor = 14, // 0x0000000E
    /// <summary>Task Set Template</summary>
    TaskSet = 24, // 0x00000018
  }
}

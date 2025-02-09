// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ILoanProgram
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>Interface for the LoanProgram class.</summary>
  /// <exclude />
  [Guid("E80EC554-E6F6-401c-959F-1E8DF89965D1")]
  public interface ILoanProgram
  {
    string Name { get; }

    string Path { get; }

    TemplateType TemplateType { get; }

    string Description { get; }

    TemplateFields Fields { get; }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplateType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Identifies the type of document represented by a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" />.
  /// </summary>
  [Guid("F3F2DA95-CA69-44f4-A2E3-E081B91039D1")]
  public enum DocumentTemplateType
  {
    /// <summary>No document type specified</summary>
    None = 0,
    /// <summary>Document represetns a standard print form</summary>
    StandardForm = 1,
    /// <summary>Document represents a custom print form</summary>
    CustomForm = 2,
    /// <summary>Document is one that must be provided by the borrower</summary>
    Needed = 3,
    /// <summary>The document does not fall into any of the other categories</summary>
    Other = 100, // 0x00000064
  }
}

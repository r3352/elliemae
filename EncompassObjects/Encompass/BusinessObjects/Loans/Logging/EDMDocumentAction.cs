// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMDocumentAction
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the possible actions associated with an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMDocument" />.
  /// </summary>
  [Guid("F5CCC7CF-0F76-49db-A6E3-A28DD3299E8C")]
  public enum EDMDocumentAction
  {
    /// <summary>No action is specified.</summary>
    None = 0,
    /// <summary>The document was sent to the borrower for review.</summary>
    Sent = 1,
    /// <summary>The document was request from the borrower for submission.</summary>
    Needed = 2,
    /// <summary>The document was sent to the borrower, who is to sign and return the document.</summary>
    SignAndReturn = 3,
    /// <summary>An unknown action is specified.</summary>
    Other = 100, // 0x00000064
  }
}

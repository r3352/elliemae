// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosedDocumentType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Enumerates the different document types for a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosedDocument" />.
  /// </summary>
  [Guid("0FCE99AC-1B4D-4224-82DA-D558B0F5999C")]
  public enum DisclosedDocumentType
  {
    /// <summary>The document type is not specified.</summary>
    NotSpecified,
    /// <summary>The document represents an Electronic Disclosure.</summary>
    eDisclosure,
    /// <summary>The document is a standard Encompass form.</summary>
    StandardForm,
    /// <summary>The document is a custom print form.</summary>
    CustomForm,
    /// <summary>The document is being requested from the user (i.e. no document delivered).</summary>
    Needed,
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalDocumentSettingViolationType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Defines the possible channel type for External Document Setting Violation
  /// </summary>
  public enum ExternalDocumentSettingViolationType
  {
    /// <summary>Indicates the the input is invalid</summary>
    InvalidInputArgument = 1,
    /// <summary>Indicates the the document does nt exist</summary>
    DocumentDoesNotExist = 2,
    /// <summary>Indicates the extension of the document is invalid</summary>
    InvalidDocumentExtension = 3,
    /// <summary>Indicates the document exceeds the size limit</summary>
    DocumentExceedSize = 4,
  }
}

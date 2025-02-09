// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalDocumentSettingException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>ExternalDocumentSettingException class</summary>
  public class ExternalDocumentSettingException : ApplicationException
  {
    internal ExternalDocumentSettingException(
      ExternalDocumentSettingViolationType type,
      string message)
      : base(message)
    {
      this.ExceptionType = type;
    }

    /// <summary>Gets or Sets the type of Exception</summary>
    public ExternalDocumentSettingViolationType ExceptionType { get; set; }
  }
}

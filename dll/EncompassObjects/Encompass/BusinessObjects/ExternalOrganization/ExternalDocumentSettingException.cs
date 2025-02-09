// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalDocumentSettingException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class ExternalDocumentSettingException : ApplicationException
  {
    internal ExternalDocumentSettingException(
      ExternalDocumentSettingViolationType type,
      string message)
      : base(message)
    {
      this.ExceptionType = type;
    }

    public ExternalDocumentSettingViolationType ExceptionType { get; set; }
  }
}

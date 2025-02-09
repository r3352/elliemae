// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.FieldValidationException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class FieldValidationException : Exception
  {
    private string fieldId;
    private string invalidValue;

    public FieldValidationException(string fieldId, string invalidValue, string description)
      : base(description)
    {
      this.fieldId = fieldId;
      this.invalidValue = invalidValue;
    }

    public string FieldID => this.fieldId;

    public string InvalidValue => this.invalidValue;
  }
}

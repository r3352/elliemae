// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.FieldValidationException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>FieldValidationException</summary>
  public class FieldValidationException : Exception
  {
    private string fieldId;
    private string invalidValue;

    /// <summary>Constructor</summary>
    /// <param name="fieldId"></param>
    /// <param name="invalidValue"></param>
    /// <param name="description"></param>
    public FieldValidationException(string fieldId, string invalidValue, string description)
      : base(description)
    {
      this.fieldId = fieldId;
      this.invalidValue = invalidValue;
    }

    /// <summary>Accesesors - FieldID</summary>
    public string FieldID => this.fieldId;

    /// <summary>Accesesors - InvalidValue</summary>
    public string InvalidValue => this.invalidValue;
  }
}

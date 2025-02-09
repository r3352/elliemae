// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataTableImportErrorProvider
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataTableImportErrorProvider : IImportErrorProvider
  {
    public int GetMessageCode(ValidationErrorType errorType)
    {
      switch (errorType)
      {
        case ValidationErrorType.FormatMismatch:
        case ValidationErrorType.DataTypeMismatch:
          return 2;
        case ValidationErrorType.InvalidRange:
          return 3;
        default:
          return 0;
      }
    }
  }
}

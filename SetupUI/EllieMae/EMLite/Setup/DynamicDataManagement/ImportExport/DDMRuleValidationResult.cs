// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport.DDMRuleValidationResult
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport
{
  public class DDMRuleValidationResult
  {
    public List<DDMFieldValueValidation> fieldValidationResults;
    public bool validationSucceeded;
    public string exceptionLog;

    public DDMRuleValidationResult(
      List<DDMFieldValueValidation> fieldValidationResults,
      bool validationSucceeded,
      string exceptionLog)
    {
      this.fieldValidationResults = fieldValidationResults;
      this.validationSucceeded = validationSucceeded;
      this.exceptionLog = exceptionLog;
    }
  }
}

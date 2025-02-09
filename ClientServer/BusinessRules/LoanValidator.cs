// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.LoanValidator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class LoanValidator : ILoanValidator
  {
    private ExecutionContext context;
    private FieldRuleValidators validators;
    private bool enabled = true;
    private List<string> failedValidationList;

    public LoanValidator(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData loan)
    {
      if (loan.Validator != null)
        throw new InvalidOperationException("LoanData object already has a validator attached");
      this.failedValidationList = new List<string>();
      this.context = new ExecutionContext(sessionObjects.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(sessionObjects), true);
      this.validators = FieldValidatorCache.GetFieldValidators(sessionObjects, configInfo);
      loan.AttachValidator((ILoanValidator) this);
    }

    public bool Enabled
    {
      get => this.enabled;
      set => this.enabled = value;
    }

    public List<string> FailedValidationListFieldIds => this.failedValidationList;

    public void Validate(string fieldId, string value)
    {
      if (!this.enabled)
        return;
      this.validators.ValidateField(fieldId, (object) value, this.context);
    }

    public void ValidateAll(bool fromImport = false, List<string> failedValidationList = null)
    {
      if (!this.enabled)
        return;
      this.validators.ValidateAll(this.context, fromImport, this.failedValidationList);
    }
  }
}

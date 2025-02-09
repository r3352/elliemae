// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Workflow.UpdateLECDIssueDateWorkflow
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.BizLayer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.LoanUtils.Validators;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Workflow
{
  public class UpdateLECDIssueDateWorkflow : EncBizFlowBase<UpdateLECDIssueDateWorkflow_Data, bool>
  {
    private LoanIsEditableValidator _loanIsEditableValidator;
    private IEncInput<object, DateTime> _lecdIssueDateSelector;
    private bool _result;

    public UpdateLECDIssueDateWorkflow(
      LoanIsEditableValidator loanIsEditableValidator,
      IEncInput<object, DateTime> lecdIssueDateSelector)
    {
      this._loanIsEditableValidator = loanIsEditableValidator;
      this._lecdIssueDateSelector = lecdIssueDateSelector;
    }

    public override bool ExecuteWorkflow(UpdateLECDIssueDateWorkflow_Data workflowData)
    {
      using (PerformanceMeter.StartNew(nameof (UpdateLECDIssueDateWorkflow), 34, nameof (ExecuteWorkflow), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Workflow\\UpdateLECDIssueDateWorkflow.cs"))
      {
        DateTime minValue = DateTime.MinValue;
        IEnumerable<string> brokenSpecs = (IEnumerable<string>) new List<string>();
        if (workflowData.ForLE)
        {
          if (!this._loanIsEditableValidator.IsValid(new LoanIsEditableValidator_Data()
          {
            LoanData = workflowData.LoanData,
            FieldId = "LE1.X1"
          }, out brokenSpecs))
            throw new Exception("The LE cannot be printed or disclosed without providing the LE Date Issued (field LE1.X1).\n You do not have the required access rights to update this field. Please contact your system administrator for more information.");
          DateTime result = this._lecdIssueDateSelector.GetResult();
          if (this._lecdIssueDateSelector.CheckStatus())
          {
            workflowData.LoanData.SetField("LE1.X1", result.ToString("MM/dd/yyyy"));
            this._result = true;
          }
          else
            this._result = false;
        }
        else
        {
          if (!this._loanIsEditableValidator.IsValid(new LoanIsEditableValidator_Data()
          {
            LoanData = workflowData.LoanData,
            FieldId = "CD1.X1"
          }, out brokenSpecs))
            throw new Exception("The CD Date Issued is blank and you do not have the permission to modify this date. Hence, this CD can not be printed / disclosed.");
          DateTime result = this._lecdIssueDateSelector.GetResult();
          if (this._lecdIssueDateSelector.CheckStatus())
          {
            workflowData.LoanData.SetField("CD1.X1", result.ToString("MM/dd/yyyy"));
            this._result = true;
          }
          else
            this._result = false;
        }
        return this._result;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Workflow.EnforcePrintFormBpmWorkflow
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.BizLayer;
using EllieMae.EMLite.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Workflow
{
  public class EnforcePrintFormBpmWorkflow : 
    EncBizFlowBase<EnforcePrintFormBpmWorkflow_Data, FormItemInfo[]>
  {
    private IEncInput<PrintableFormSelector_Data, FormItemInfo[]> _printableFormSelector;
    private IPrintFormsBpmService _printFormsBpmService;
    private FormItemInfo[] _result;
    private ActivatedPrintFormRule printRule;
    private Dictionary<string, Hashtable> _customFormRequiredFields;

    public EnforcePrintFormBpmWorkflow(
      IPrintFormsBpmService printFormsBpmService,
      IEncInput<PrintableFormSelector_Data, FormItemInfo[]> printableFormSelector)
    {
      this._printableFormSelector = printableFormSelector;
      this._printFormsBpmService = printFormsBpmService;
    }

    public void AddCustomFormRequiredFields(string formID, Hashtable reqFields)
    {
      if (this._customFormRequiredFields == null)
        this._customFormRequiredFields = new Dictionary<string, Hashtable>();
      if (this._customFormRequiredFields.ContainsKey(formID))
        return;
      this._customFormRequiredFields.Add(formID, reqFields);
    }

    public ActivatedPrintFormRule GetActivatedPrintFormRule() => this.printRule;

    public override FormItemInfo[] ExecuteWorkflow(EnforcePrintFormBpmWorkflow_Data command)
    {
      using (PerformanceMeter.StartNew(nameof (EnforcePrintFormBpmWorkflow), 59, nameof (ExecuteWorkflow), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\Workflow\\EnforcePrintFormBpmWorkflow.cs"))
      {
        this._result = command.PrintForms;
        this.printRule = this._printFormsBpmService.GetAllRequiredFields(new LoanBusinessRuleInfo(command.LoanData).CurrentLoanForBusinessRule(), command.LoanData);
        if (this._customFormRequiredFields != null && this._customFormRequiredFields.Any<KeyValuePair<string, Hashtable>>())
        {
          foreach (string key in this._customFormRequiredFields.Keys)
            this.printRule.AddFormRequiredFields(key, this._customFormRequiredFields[key]);
        }
        this.printRule.CheckRequiredFields(command.PrintForms, command.LoanData, command.PrintBlankForm);
        this.printRule.CheckRequiredCodes(command.PrintForms, command.LoanData, command.PrintBlankForm, command.SessionObjects);
        PrintableFormSelector_Data entity = new PrintableFormSelector_Data()
        {
          LoanData = command.LoanData,
          PrintForms = command.PrintForms,
          PrintFormRule = this.printRule
        };
        if (this.printRule.BadFormsCount > 0)
        {
          if (command.EnablePrintRuleCheckStatusForm)
          {
            this._printableFormSelector.SetData(entity);
            this._result = this._printableFormSelector.GetResult();
            if (!this._printableFormSelector.CheckStatus())
              this._result = new FormItemInfo[0];
          }
          else
            this._result = new FormItemInfo[0];
        }
        else
        {
          this._result = new FormItemInfo[0];
          this.ViolationMessage = "There are " + (object) this.printRule.BadFormsCount + " forms that can't be printed.";
        }
        return this._result;
      }
    }
  }
}

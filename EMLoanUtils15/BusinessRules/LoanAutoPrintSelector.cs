// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.LoanAutoPrintSelector
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class LoanAutoPrintSelector : IPrintFormSelector
  {
    private const string className = "LoanAutoPrintSelector�";
    private static string sw = Tracing.SwDataEngine;
    private ExecutionContext context;
    private CompiledPrintFormSelectors compiledSelectors;
    private List<TriggerInvoker> invokers = new List<TriggerInvoker>();

    public LoanAutoPrintSelector(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData loan)
    {
      if (loan.PrintFormSelector != null)
        throw new InvalidOperationException("LoanData object already has a Print Form Selector attached");
      this.context = new ExecutionContext(sessionObjects.UserInfo, loan, (IServerDataProvider) new CustomCodeSessionDataProvider(sessionObjects), true);
      this.compiledSelectors = PrintFormSelectorCache.GetFormSelectors(sessionObjects, configInfo);
      loan.AttachPrintFormSelector((IPrintFormSelector) this);
    }

    public Dictionary<string, OutputFormType> GetPreselectedForms()
    {
      Dictionary<string, OutputFormType> preselectedForms = new Dictionary<string, OutputFormType>();
      string empty = string.Empty;
      Tracing.Log(LoanAutoPrintSelector.sw, nameof (LoanAutoPrintSelector), TraceLevel.Info, "GetPreselectedForms: Going through each Auto Print Form Selector Event.");
      for (int index1 = 0; index1 < this.compiledSelectors.Count; ++index1)
      {
        if (!this.compiledSelectors[index1].ExecutePrintFormSelector(this.context))
        {
          Tracing.Log(LoanAutoPrintSelector.sw, nameof (LoanAutoPrintSelector), TraceLevel.Info, "GetPreselectedForms: Event " + this.compiledSelectors[index1].Definition.Description + " doesn't match loan criteria.");
        }
        else
        {
          PrintFormSelectorImplDef definition = (PrintFormSelectorImplDef) this.compiledSelectors[index1].Definition;
          if (definition.Event != null && definition.Event.SelectedForms != null)
          {
            for (int index2 = 0; index2 < definition.Event.SelectedForms.Length; ++index2)
            {
              string str = definition.Event.SelectedForms[index2].Name;
              if (string.Compare(str, "FHA Informed Consumer Choice Dis", true) == 0)
                str = "FHA Informed Consumer Choice Disclosure";
              else if (string.Compare(str, "Loans Where Credit Score is Not Available", true) == 0)
                str += " Model H5";
              else if (string.Compare(str, "Risk-Based Pricing Notice", true) == 0)
                str += " Model H1";
              if (!preselectedForms.ContainsKey(str))
                preselectedForms.Add(str, definition.Event.SelectedForms[index2].Type);
            }
          }
        }
      }
      return preselectedForms;
    }
  }
}

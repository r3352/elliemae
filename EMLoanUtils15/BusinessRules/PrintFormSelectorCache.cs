// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.PrintFormSelectorCache
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class PrintFormSelectorCache
  {
    private const string className = "PrintFormSelectorCache�";
    private static readonly string sw = Tracing.SwDataEngine;
    private static Hashtable systemPrintFormSelectors = new Hashtable();
    private static object syncRoot = new object();

    private PrintFormSelectorCache()
    {
    }

    public static CompiledPrintFormSelectors GetFormSelectors(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo)
    {
      string key = sessionObjects.StartupInfo.ServerInstanceName + "@" + sessionObjects.StartupInfo.ServerID;
      PrintFormSelectorCache.ServerPrintFormSelectors printFormSelectors = (PrintFormSelectorCache.ServerPrintFormSelectors) null;
      lock (PrintFormSelectorCache.syncRoot)
      {
        if (!PrintFormSelectorCache.systemPrintFormSelectors.Contains((object) key))
          PrintFormSelectorCache.systemPrintFormSelectors[(object) key] = (object) new PrintFormSelectorCache.ServerPrintFormSelectors();
        printFormSelectors = (PrintFormSelectorCache.ServerPrintFormSelectors) PrintFormSelectorCache.systemPrintFormSelectors[(object) key];
      }
      return printFormSelectors.GetFormSelectors(sessionObjects, configInfo);
    }

    public static DateTime GetFormSelectorsModificationTimestamp(SessionObjects sessionObjects)
    {
      string key = sessionObjects.StartupInfo.ServerInstanceName + "@" + sessionObjects.StartupInfo.ServerID;
      PrintFormSelectorCache.ServerPrintFormSelectors printFormSelectors = (PrintFormSelectorCache.ServerPrintFormSelectors) null;
      lock (PrintFormSelectorCache.syncRoot)
      {
        if (!PrintFormSelectorCache.systemPrintFormSelectors.Contains((object) key))
          return DateTime.MinValue;
        printFormSelectors = (PrintFormSelectorCache.ServerPrintFormSelectors) PrintFormSelectorCache.systemPrintFormSelectors[(object) key];
      }
      return printFormSelectors.LastModificationTime;
    }

    private class ServerPrintFormSelectors
    {
      private CompiledPrintFormSelectors selecters;
      private DateTime lastModified = DateTime.MinValue;
      private object syncRoot = new object();

      public DateTime LastModificationTime
      {
        get
        {
          lock (this.syncRoot)
            return this.lastModified;
        }
      }

      public CompiledPrintFormSelectors GetFormSelectors(
        SessionObjects sessionObjects,
        ILoanConfigurationInfo configInfo)
      {
        lock (this.syncRoot)
        {
          if (this.selecters == null || configInfo.PrintSelectionModificationTime > this.lastModified)
          {
            this.selecters = new CompiledPrintFormSelectors(this.generateFormSelectors(configInfo));
            this.lastModified = configInfo.PrintSelectionModificationTime;
          }
          return this.selecters;
        }
      }

      private PrintFormSelector[] generateFormSelectors(ILoanConfigurationInfo configInfo)
      {
        ArrayList arrayList = new ArrayList();
        foreach (PrintSelectionRuleInfo printSelectionRule in configInfo.PrintSelectionRules)
        {
          if (!printSelectionRule.Inactive)
            arrayList.AddRange((ICollection) this.generateFormSelector(configInfo, printSelectionRule));
        }
        return (PrintFormSelector[]) arrayList.ToArray(typeof (PrintFormSelector));
      }

      private ArrayList generateFormSelector(
        ILoanConfigurationInfo configInfo,
        PrintSelectionRuleInfo ruleDef)
      {
        RuleCondition ruleCondition = BizRuleTranslator.GetRuleCondition((BizRuleInfo) ruleDef);
        ArrayList formSelector = new ArrayList();
        foreach (PrintSelectionEvent formSelectorEvent in ruleDef.Events)
        {
          string description = ruleDef.RuleName + " - " + formSelectorEvent.Conditions[0].FieldID;
          formSelector.Add((object) new PrintFormSelectorImplDef(description, formSelectorEvent, ruleCondition));
        }
        return formSelector;
      }
    }
  }
}

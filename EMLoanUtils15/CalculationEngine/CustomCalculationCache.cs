// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CustomCalculationCache
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class CustomCalculationCache
  {
    private const string className = "CustomCalculationCache�";
    private static readonly string sw = Tracing.SwDataEngine;
    private static Hashtable calcs = new Hashtable();
    private static object syncRoot = new object();

    private CustomCalculationCache()
    {
    }

    public static CustomFieldCalculators GetFieldCalculators(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo)
    {
      string key = sessionObjects.StartupInfo.ServerInstanceName + "@" + sessionObjects.StartupInfo.ServerID;
      CustomCalculationCache.ServerCustomCalculations customCalculations = (CustomCalculationCache.ServerCustomCalculations) null;
      lock (CustomCalculationCache.syncRoot)
      {
        if (!CustomCalculationCache.calcs.Contains((object) key))
          CustomCalculationCache.calcs[(object) key] = (object) new CustomCalculationCache.ServerCustomCalculations();
        customCalculations = (CustomCalculationCache.ServerCustomCalculations) CustomCalculationCache.calcs[(object) key];
      }
      return customCalculations.GetFieldCalculators(sessionObjects, configInfo);
    }

    private class ServerCustomCalculations
    {
      private CustomFieldCalculators calcs;
      private DateTime lastModified = DateTime.MinValue;
      private object syncRoot = new object();

      public CustomFieldCalculators GetFieldCalculators(
        SessionObjects sessionObjects,
        ILoanConfigurationInfo configInfo)
      {
        lock (this.syncRoot)
        {
          if (this.lastModified == DateTime.MinValue || configInfo.CustomFieldsModificationTime > this.lastModified)
          {
            PerformanceMeter.Current.AddCheckpoint("Starting to build and compile custom calculations", 69, nameof (GetFieldCalculators), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\CustomCalculationCache.cs");
            this.calcs = new CustomFieldCalculators(configInfo.CustomFields, !sessionObjects.Interactive);
            PerformanceMeter.Current.AddCheckpoint("Finished building and compiling custom calculations", 71, nameof (GetFieldCalculators), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\CalculationEngine\\CustomCalculationCache.cs");
            this.lastModified = configInfo.CustomFieldsModificationTime;
          }
          return this.calcs;
        }
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationEngineConfiguration
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.Configuration;
using System;
using System.Configuration;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public class CalculationEngineConfiguration : ConfigurationSection
  {
    static CalculationEngineConfiguration()
    {
      try
      {
        CalculationEngineConfiguration.CurrentConfiguration = (CalculationEngineConfiguration) ConfigurationManager.GetSection("calcEngineConfig");
      }
      catch (Exception ex)
      {
        CalculationEngineConfiguration.CurrentConfiguration = (CalculationEngineConfiguration) null;
      }
    }

    public static CalculationEngineConfiguration CurrentConfiguration { get; private set; }

    [ConfigurationProperty("UniqueOrDefaultSetting", DefaultValue = "FirstOrDefault", IsRequired = false)]
    public UniqueOrDefaultSettingType UniqueOrDefaultSetting
    {
      get
      {
        return !(this[nameof (UniqueOrDefaultSetting)].ToString().ToUpper() == "SINGLEORDEFAULT") ? UniqueOrDefaultSettingType.FirstOrDefault : UniqueOrDefaultSettingType.SingleOrDefault;
      }
    }

    [ConfigurationProperty("DebugCalculationPerformance", DefaultValue = "false", IsRequired = false)]
    public bool DebugCalculationPerformance => (bool) this[nameof (DebugCalculationPerformance)];

    [ConfigurationProperty("DebugLogCalculatedResults", DefaultValue = "false", IsRequired = false)]
    public bool DebugLogCalculatedResults => (bool) this[nameof (DebugLogCalculatedResults)];

    [ConfigurationProperty("DebugLogExecutionPlan", DefaultValue = "false", IsRequired = false)]
    public bool DebugLogExecutionPlan => (bool) this[nameof (DebugLogExecutionPlan)];

    [ConfigurationProperty("DebugLogCalcForMod", DefaultValue = "false", IsRequired = false)]
    public bool DebugLogCalcForMod => (bool) this[nameof (DebugLogCalcForMod)];

    [ConfigurationProperty("DebugLogDataSourceValues", DefaultValue = "false", IsRequired = false)]
    public bool DebugLogDataSourceValues => (bool) this[nameof (DebugLogDataSourceValues)];

    [ConfigurationProperty("EnableShortCircuiting", DefaultValue = "true", IsRequired = false)]
    public bool EnableShortCircuiting => (bool) this[nameof (EnableShortCircuiting)];
  }
}

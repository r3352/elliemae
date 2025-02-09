// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.SourceControl.ReasonCode
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System.Configuration;

#nullable disable
namespace Elli.CalculationEngine.Core.SourceControl
{
  public class ReasonCode : ConfigurationElement
  {
    [ConfigurationProperty("key", IsRequired = true)]
    public string Key => (string) this["key"];

    [ConfigurationProperty("code", IsRequired = true)]
    public string Code => (string) this["code"];

    [ConfigurationProperty("type", IsRequired = true)]
    public string Type => (string) this["type"];
  }
}

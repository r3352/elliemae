// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.SourceControl.ReasonCodeCollection
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System.Configuration;

#nullable disable
namespace Elli.CalculationEngine.Core.SourceControl
{
  public class ReasonCodeCollection : ConfigurationElementCollection
  {
    protected override ConfigurationElement CreateNewElement()
    {
      return (ConfigurationElement) new ReasonCode();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      return (object) ((ReasonCode) element).Key;
    }
  }
}

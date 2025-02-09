// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.LockDeskSettings.ONRPRetailBranchSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.LockDeskSettings
{
  public class ONRPRetailBranchSettings : ONRPBranchSettings
  {
    public bool UseParentInfo;

    public ONRPRetailBranchSettings Convert(ONRPBranchSettings setting)
    {
      this.ChannelDefaultOrCustomize = setting.ChannelDefaultOrCustomize;
      this.ContinousCoverageOrSpecifyTime = setting.ContinousCoverageOrSpecifyTime;
      this.DollarLimit = setting.DollarLimit;
      this.EnableONRP = setting.EnableONRP;
      this.EndTime = setting.EndTime;
      this.NoMaxLimit = setting.NoMaxLimit;
      this.StartTime = setting.StartTime;
      this.Tolerance = setting.Tolerance;
      this.WeekendHolidayCoverage = setting.WeekendHolidayCoverage;
      return this;
    }
  }
}

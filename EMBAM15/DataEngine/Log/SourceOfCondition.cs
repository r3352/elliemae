// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.SourceOfCondition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public enum SourceOfCondition
  {
    [StringValue("User")] User,
    [StringValue("Manual")] Manual,
    [StringValue("Investor Delivery")] InvestorDelivery,
    [StringValue("Conditions List")] ConditionList,
    [StringValue("Automated (Rule)")] AutomatedByRule,
    [StringValue("Automated (User)")] AutomatedByUser,
    [StringValue("Duplicate")] Duplicate,
    [StringValue("FHA")] FHA,
    [StringValue("DU Findings")] DUFindings,
    [StringValue("EarlyCheck Findings")] EarlyCheckFindings,
    [StringValue("LPA Findings")] LPAFindings,
    [StringValue("FHA Findings")] FHAFindings,
    [StringValue("Partner Connect")] PartnerConnect,
    [StringValue("LCLA Findings")] LCLAFindings,
  }
}

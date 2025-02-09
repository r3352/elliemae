// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.IncomeType
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public enum IncomeType
  {
    [Description("None")] None,
    [Description("Paystubs")] Paystub,
    [Description("Tax Return Year")] TaxReturn,
    [Description("Pension")] Pension,
    [Description("Military")] Military,
    [Description("Social Security")] SocialSecurity,
    [Description("Alimony/Maintenance")] AlimonyOrMaintenance,
    [Description("Rental Income")] RentalIncome,
    [Description("Child Support")] ChildSupport,
    [Description("401K")] Four01K,
    [Description("1099")] Ten99,
    [Description("W2")] W2,
    [Description("Other")] OtherEmployment,
    [Description("Other")] OtherNonEmployment,
  }
}

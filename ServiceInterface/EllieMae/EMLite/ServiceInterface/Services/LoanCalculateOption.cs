// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.Services.LoanCalculateOption
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface.Services
{
  [Flags]
  public enum LoanCalculateOption
  {
    Default = 0,
    UseCalculatAll = 1,
    UseCalculateModifiedFields = 2,
    UseCalculateModifiedFieldsBulk = 4,
    UseElliDomainCalculationCalculator = 8,
    UseLoanCalculator = 16, // 0x00000010
    UseSingleLoanCalculator = UseLoanCalculator | UseElliDomainCalculationCalculator | UseCalculateModifiedFields, // 0x0000001A
  }
}

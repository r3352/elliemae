// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.RateLocks.OnrpCalcInfo
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.RateLocks
{
  public class OnrpCalcInfo
  {
    public string EffectiveEntityId { get; set; }

    public DateTime OnrpStartDate { get; set; }

    public string OnrpLockDate { get; set; }

    public string OnrpLockTime { get; set; }

    public bool IsONRPEligible { get; set; }

    public bool IsManual { get; set; }
  }
}

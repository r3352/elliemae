// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.RateLocks.ILockDeskHoursHelper
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.RateLocks
{
  public interface ILockDeskHoursHelper
  {
    void Log(string sw, string className, TraceLevel l, string msg);

    DateTime GetServerEasternTime();

    LockDeskHoursInfo GetLockDeskHoursSettings(LoanChannel loanChannel);

    void GetONRPGlobalSettings(
      out LockDeskOnrpInfo globalRetailSettings,
      out LockDeskOnrpInfo globalWholesaleSettings,
      out LockDeskOnrpInfo globalCorrespondentSettings);

    void GetONRPTPOSettings(
      string tpoId,
      out LockDeskOnrpInfo brokerONRPSettings,
      out LockDeskOnrpInfo correspondentONRPSettings);

    void GetONRPRetailSettings(out LockDeskOnrpInfo branchONRPSettings);

    double GetOnrpAccruedDollarAmount(LoanChannel channel, string entityId, DateTime onrpStartDate);

    bool IsFirstLockRequest();

    LoanChannel GetLoanChannel();
  }
}

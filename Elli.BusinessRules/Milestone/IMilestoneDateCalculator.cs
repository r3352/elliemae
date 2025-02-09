// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Milestone.IMilestoneDateCalculator
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.Domain.Mortgage;
using System;

#nullable disable
namespace Elli.BusinessRules.Milestone
{
  public interface IMilestoneDateCalculator
  {
    void AdjustAllDates(MilestoneLog log, DateTime newDate);

    void AdjustDate(
      MilestoneLog log,
      DateTime newDate,
      bool allowAdjustPreviousMilestones,
      bool allowAdjustFutureMilestones);

    void AdjustCorrectionDatesOnly(MilestoneLog log, DateTime newDate);
  }
}

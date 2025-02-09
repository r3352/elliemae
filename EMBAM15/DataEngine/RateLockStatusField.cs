// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockStatusField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockStatusField : VirtualField
  {
    public const string NotLocked = "NotLocked";
    public const string Locked = "Locked";
    public const string Expired = "Expired";
    public const string Cancelled = "Cancelled";
    public const string Voided = "Voided";
    private FieldOptionCollection options;

    public RateLockStatusField()
      : base("LOCKRATE.RATESTATUS", "Rate Lock Status", FieldFormat.STRING)
    {
      this.options = new FieldOptionCollection(new List<FieldOption>()
      {
        new FieldOption("Not Locked", nameof (NotLocked)),
        new FieldOption(nameof (Locked), nameof (Locked)),
        new FieldOption(nameof (Expired), nameof (Expired)),
        new FieldOption(nameof (Cancelled), nameof (Cancelled)),
        new FieldOption(nameof (Voided), nameof (Voided))
      }.ToArray(), true);
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    public override FieldOptionCollection Options => this.options;

    public override bool AllowInReportingDatabase => false;

    protected override string Evaluate(LoanData loan)
    {
      if (loan.GetField("LOCKRATE.ISCANCELLED") == "Y")
        return "Cancelled";
      LockConfirmLog confirmForCurrentLock = loan.GetLogList().GetMostRecentConfirmForCurrentLock();
      bool flag1 = false;
      if (confirmForCurrentLock != null)
        flag1 = confirmForCurrentLock.CommitmentTermEnabled;
      DateTime dateTime = string.Equals(loan.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) & flag1 ? Utils.ParseDate((object) loan.GetField("4529")) : Utils.ParseDate((object) loan.GetField("762"));
      int days = dateTime == DateTime.MinValue ? 0 : (dateTime.Date - DateTime.Now.Date).Days;
      MilestoneLog milestone = loan.GetLogList().GetMilestone("Completion");
      bool flag2 = milestone != null && milestone.Done;
      if (dateTime == DateTime.MinValue)
        return "NotLocked";
      return flag2 || days >= 0 ? "Locked" : "Expired";
    }
  }
}

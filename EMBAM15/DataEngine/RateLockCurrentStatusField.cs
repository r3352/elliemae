// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockCurrentStatusField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockCurrentStatusField : BankerEditionVirtualField
  {
    private FieldOptionCollection options;

    public RateLockCurrentStatusField()
      : base("LOCKRATE.CURRENTSTATUS", "Current Lock Request Status", FieldFormat.STRING)
    {
      this.options = new FieldOptionCollection(new List<FieldOption>()
      {
        new FieldOption("No Active Request", ""),
        new FieldOption("Lock Requested", "Active Request"),
        new FieldOption("Request Locked", "Locked"),
        new FieldOption("Request Denied", "Denied"),
        new FieldOption("Lock Expired", "Lock Expired"),
        new FieldOption("Long Loan", "Long Loan"),
        new FieldOption("Lock Expired", "Lock Expired"),
        new FieldOption("Shipped", "Shipped"),
        new FieldOption("Purchased", "Purchased")
      }.ToArray(), true);
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    public override FieldOptionCollection Options => this.options;

    public override bool AllowInReportingDatabase => false;

    protected override string Evaluate(LoanData loan) => loan.GetLogList().GetLockCurrentStatus();
  }
}

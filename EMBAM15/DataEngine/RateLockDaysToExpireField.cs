// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockDaysToExpireField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockDaysToExpireField : VirtualField
  {
    public RateLockDaysToExpireField()
      : base("MS.LOCKDAYS", "Rate Lock Days to Expire", FieldFormat.INTEGER)
    {
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    public override bool AllowInReportingDatabase => false;

    protected override string Evaluate(LoanData loan)
    {
      try
      {
        DateTime date = Utils.ParseDate((object) loan.GetField("762"), true);
        if (!loan.GetLogList().GetMilestone("Completion").Done)
          return (date - DateTime.Now.Date).Days.ToString();
      }
      catch
      {
      }
      return "";
    }
  }
}

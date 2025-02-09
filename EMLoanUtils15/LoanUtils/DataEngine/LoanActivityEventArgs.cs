// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityEventArgs
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.DataEngine
{
  public class LoanActivityEventArgs : EventArgs
  {
    public LoanActivityEventArgs()
    {
    }

    public LoanActivityEventArgs(LoanActivityType type, string message)
    {
      this.ActivityType = type;
      this.Message = message;
    }

    public LoanActivityType ActivityType { get; set; }

    public string Message { get; set; }

    public TimeSpan? Elapsed { get; set; }

    internal static LoanActivityEventArgs Before(LoanActivityType type)
    {
      LoanActivityEventArgs activityEventArgs = LoanActivityEventArgs.Create(type);
      if (!string.IsNullOrEmpty(activityEventArgs.Message))
        activityEventArgs.Message = activityEventArgs.Message.Insert(0, "[Before] ");
      return activityEventArgs;
    }

    internal static LoanActivityEventArgs After(LoanActivityType type, TimeSpan? elapsed = null)
    {
      LoanActivityEventArgs activityEventArgs = LoanActivityEventArgs.Create(type);
      activityEventArgs.Elapsed = elapsed;
      if (!string.IsNullOrEmpty(activityEventArgs.Message))
        activityEventArgs.Message = activityEventArgs.Message.Insert(0, "[After] ");
      return activityEventArgs;
    }

    internal static LoanActivityEventArgs Create(LoanActivityType type)
    {
      LoanActivityEventArgs activityEventArgs = new LoanActivityEventArgs()
      {
        ActivityType = type
      };
      switch (type)
      {
        case LoanActivityType.CalcOnDemand:
          activityEventArgs.Message = "Calculator.OnDemand()";
          break;
        case LoanActivityType.CalcAll:
          activityEventArgs.Message = "Calculator.CalculateAll()";
          break;
        case LoanActivityType.LoanCommit:
          activityEventArgs.Message = "Committing Loan";
          break;
      }
      return activityEventArgs;
    }
  }
}

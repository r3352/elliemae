// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanToTradeAssignmentExtensions
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public static class LoanToTradeAssignmentExtensions
  {
    public static List<LoanToTradeAssignmentBase> ToBaseList(this List<LoanTradeAssignment> list)
    {
      List<LoanToTradeAssignmentBase> baseList = new List<LoanToTradeAssignmentBase>();
      foreach (LoanTradeAssignment loanTradeAssignment in list)
        baseList.Add((LoanToTradeAssignmentBase) loanTradeAssignment);
      return baseList;
    }

    public static List<LoanToTradeAssignmentBase> ToBaseList(this List<MbsPoolLoanAssignment> list)
    {
      List<LoanToTradeAssignmentBase> baseList = new List<LoanToTradeAssignmentBase>();
      foreach (MbsPoolLoanAssignment poolLoanAssignment in list)
        baseList.Add((LoanToTradeAssignmentBase) poolLoanAssignment);
      return baseList;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.UserLoanStatistics
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class UserLoanStatistics
  {
    public readonly string UserID;
    public readonly string FirstName;
    public readonly string LastName;
    public int AssignedLoanCount;
    public int ActiveLoanCount;
    public int LoansStarted;
    public int LoansCompleted;
    public int LoansAdverse;

    public UserLoanStatistics(string userId, string firstName, string lastName)
    {
      this.FirstName = firstName;
      this.LastName = lastName;
      this.UserID = userId;
    }

    public int LoansCompletedOrAdverse => this.LoansAdverse + this.LoansCompleted;

    public Decimal PullThruRate
    {
      get
      {
        return this.LoansCompletedOrAdverse == 0 ? Decimal.MinValue : (Decimal) this.LoansCompleted * 100M / (Decimal) this.LoansCompletedOrAdverse;
      }
    }
  }
}

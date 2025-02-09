// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanSnapshotProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.ElliEnum;
using Elli.Interface;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class LoanSnapshotProvider : ILoanSnapshotProvider
  {
    private Loan _loan;

    public LoanSnapshotProvider(Loan loan) => this._loan = loan;

    public void SaveSnapshot(LogSnapshotType type, Guid key, string snapshot)
    {
      throw new NotImplementedException();
    }

    public void SaveSnapshot(
      LogSnapshotType type,
      Guid key,
      string snapshot,
      bool isAlwaysSaveNew)
    {
      throw new NotImplementedException();
    }

    public Dictionary<string, string> GetLoanSnapshot(
      LogSnapshotType type,
      Guid key,
      bool isLEorCD)
    {
      return LoanLogSnapshotStore.GetLoanSnapshot(this._loan, type, key, isLEorCD);
    }

    public Dictionary<string, Dictionary<string, string>> GetLoanSnapshots(
      LogSnapshotType type,
      Dictionary<string, bool> keys)
    {
      return LoanLogSnapshotStore.GetLoanSnapshots(this._loan, type, keys);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.LoanSnapshotProvider
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.ElliEnum;
using Elli.Interface;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  internal class LoanSnapshotProvider : ILoanSnapshotProvider
  {
    private LoanProxy _proxy;

    public LoanSnapshotProvider(LoanProxy proxy) => this._proxy = proxy;

    public void SaveSnapshot(LogSnapshotType type, Guid snapshotGuid, string snapshot)
    {
      throw new NotImplementedException();
    }

    public void SaveSnapshot(
      LogSnapshotType type,
      Guid snapshotGuid,
      string snapshot,
      bool isAlwaysSaveNew)
    {
      throw new NotImplementedException();
    }

    public Dictionary<string, string> GetLoanSnapshot(
      LogSnapshotType type,
      Guid snapshotGuid,
      bool ucdExists)
    {
      return this._proxy.GetLoanSnapshot(type, snapshotGuid, ucdExists);
    }

    public Dictionary<string, Dictionary<string, string>> GetLoanSnapshots(
      LogSnapshotType type,
      Dictionary<string, bool> snapshotGuids)
    {
      return this._proxy.GetLoanSnapshots(type, snapshotGuids);
    }
  }
}

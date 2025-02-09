// Decompiled with JetBrains decompiler
// Type: Elli.Interface.ILoanSnapshotProvider
// Assembly: Elli.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E2665EB5-354E-4837-8094-9A1310A6D998
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Interface.dll

using Elli.ElliEnum;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Interface
{
  [CLSCompliant(true)]
  public interface ILoanSnapshotProvider
  {
    void SaveSnapshot(LogSnapshotType type, Guid snapshotGuid, string snapshot);

    void SaveSnapshot(
      LogSnapshotType type,
      Guid snapshotGuid,
      string snapshot,
      bool isAlwaysSaveNew);

    Dictionary<string, string> GetLoanSnapshot(
      LogSnapshotType type,
      Guid snapshotGuid,
      bool ucdExists);

    Dictionary<string, Dictionary<string, string>> GetLoanSnapshots(
      LogSnapshotType type,
      Dictionary<string, bool> snapshotGuids);
  }
}

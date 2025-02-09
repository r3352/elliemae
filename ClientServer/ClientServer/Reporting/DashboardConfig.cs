// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.DashboardConfig
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class DashboardConfig
  {
    private int id;
    private string name;
    private int layoutId;
    private string snapshotCriteria;
    private string snapshot;
    private bool isDefault;
    private string fromDate;
    private string toDate;
    private bool isGlobal;

    public DashboardConfig(
      int id,
      string name,
      bool isGlobal,
      int layoutId,
      string snapshotCriteria,
      string snapshot,
      bool isDefault,
      string fromDate,
      string toDate)
    {
      this.id = id;
      this.name = name;
      this.layoutId = layoutId;
      this.snapshotCriteria = snapshotCriteria;
      this.snapshot = snapshot;
      this.isDefault = isDefault;
      this.fromDate = fromDate;
      this.toDate = toDate;
      this.isGlobal = isGlobal;
    }

    public DashboardConfig(
      string name,
      int layoutId,
      string snapshotCriteria,
      string snapshot,
      bool isDefault,
      string fromDate,
      string toDate)
      : this(-1, name, false, layoutId, snapshotCriteria, snapshot, isDefault, fromDate, toDate)
    {
    }

    public int ConfigurationID => this.id;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public int LayoutID
    {
      get => this.layoutId;
      set => this.layoutId = value;
    }

    public string SnapshotCriteria
    {
      get => this.snapshotCriteria;
      set => this.snapshotCriteria = value;
    }

    public string Snapshot
    {
      get => this.snapshot;
      set => this.snapshot = value;
    }

    public bool IsDefault
    {
      get => this.isDefault;
      set => this.isDefault = value;
    }

    public bool IsGlobal => this.isGlobal;

    public string FromDate
    {
      get => this.fromDate;
      set => this.fromDate = value;
    }

    public string ToDate
    {
      get => this.toDate;
      set => this.toDate = value;
    }
  }
}

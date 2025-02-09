// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.SnapshotEventArgs
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class SnapshotEventArgs : EventArgs
  {
    public int SnapshotHandle = -1;
    public string EventString = string.Empty;

    public SnapshotEventArgs(int snapshotHandle) => this.SnapshotHandle = snapshotHandle;

    public SnapshotEventArgs(int snapshotHandle, string eventString)
    {
      this.SnapshotHandle = snapshotHandle;
      this.EventString = eventString;
    }
  }
}

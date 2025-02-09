// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SnapshotObject
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class SnapshotObject : IDisposable
  {
    public SnapshotObject() => this.Id = Guid.Empty;

    public Guid Id { get; set; }

    public Guid ParentId { get; set; }

    public LogSnapshotType Type { get; set; }

    public string Data { get; set; }

    public long Length
    {
      get
      {
        return string.IsNullOrEmpty(this.Data) ? 0L : (long) Encoding.ASCII.GetByteCount(this.Data.ToCharArray());
      }
    }

    public BinaryObject ToBinaryObject() => new BinaryObject(this.Data, Encoding.ASCII);

    public override string ToString() => this.Data;

    public void Dispose()
    {
    }

    public static string GetLoanSnapshotFileName(LogSnapshotType type, string key)
    {
      switch (type)
      {
        case LogSnapshotType.DisclosureTracking:
          return "DTSnapshot" + key + ".TXT";
        case LogSnapshotType.DisclosureTrackingUCD:
          return "UCD" + key + ".XML";
        case LogSnapshotType.LockRequest:
          return "LockRequestLogSnapShot." + key + ".XML";
        case LogSnapshotType.DocumentTracking:
          return "DocumentTrackingLogSnapShot." + key + ".XML";
        default:
          return "";
      }
    }
  }
}

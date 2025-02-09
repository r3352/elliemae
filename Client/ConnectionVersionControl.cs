// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Client.ConnectionVersionControl
// Assembly: Client, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD6D6217-37E4-4BE3-B44A-5E3BA190AF3A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Client.dll

using EllieMae.EMLite.VersionInterface15;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Client
{
  public class ConnectionVersionControl : IVersionControl, ISupportsVersion
  {
    public bool IsVersionUpdateAvailable(JedVersion sourceVersion) => false;

    public FileStream GetVersionUpdateFileStream(JedVersion sourceVersion) => (FileStream) null;

    public JedVersion Version => new JedVersion(7, 5, 0);

    public bool IsCompatibleWithVersion(JedVersion version)
    {
      return this.IsCompatibleWithVersion(version, false);
    }

    public bool IsCompatibleWithVersion(JedVersion version, bool allowRevisionDiscrepancy) => false;
  }
}

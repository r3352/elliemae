// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.VersionException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.VersionInterface15;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [ComVisible(false)]
  public class VersionException : ApplicationException
  {
    private string serverVersion;

    internal VersionException(VersionMismatchException ex)
    {
      JedVersion version = VersionInformation.CurrentVersion.Version;
      string str1 = version.ToString();
      version = ((ISupportsVersion) ex.ServerVersionControl).Version;
      string str2 = version.ToString();
      // ISSUE: explicit constructor call
      base.\u002Ector("Version mismatch attempting to connect to server. Client version = " + str1 + ", Server version = " + str2);
      this.HResult = -2147212798;
      this.serverVersion = ((ISupportsVersion) ex.ServerVersionControl).Version.ToString();
    }

    public string ClientVersion => VersionInformation.CurrentVersion.Version.ToString();

    public string ServerVersion => this.serverVersion;
  }
}

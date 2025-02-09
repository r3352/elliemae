// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.VersionException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Client;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.VersionInterface15;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Exception that indicates that an attempt was made to connect to an Encompass Server
  /// which has an incompatible version number.
  /// </summary>
  /// <remarks>Encompass requires that the client and server be running the same
  /// software version in order to ensure compatibility. If an attempt is made to
  /// connect to a server which is running a different version of Encompass (whether
  /// newer or older), this exception will be raised and the session will not be started.
  /// <p>This exception can be identified in COM-based applications using the
  /// HRESULT value 0x80042202.</p>
  /// </remarks>
  [ComVisible(false)]
  public class VersionException : ApplicationException
  {
    private string serverVersion;

    internal VersionException(VersionMismatchException ex)
    {
      JedVersion version = VersionInformation.CurrentVersion.Version;
      string str1 = version.ToString();
      version = ex.ServerVersionControl.Version;
      string str2 = version.ToString();
      // ISSUE: explicit constructor call
      base.\u002Ector("Version mismatch attempting to connect to server. Client version = " + str1 + ", Server version = " + str2);
      this.HResult = -2147212798;
      this.serverVersion = ex.ServerVersionControl.Version.ToString();
    }

    /// <summary>
    /// Gets the Encompass software version of the local (client) machine.
    /// </summary>
    public string ClientVersion => VersionInformation.CurrentVersion.Version.ToString();

    /// <summary>
    /// Gets the Encompass software version running on the remote server.
    /// </summary>
    public string ServerVersion => this.serverVersion;
  }
}

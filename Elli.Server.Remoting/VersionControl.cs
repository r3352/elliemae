// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.VersionControl
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.VersionInterface15;
using System;
using System.IO;
using System.Runtime.Remoting;

#nullable disable
namespace Elli.Server.Remoting
{
  public class VersionControl : RemotedObject, IVersionControl, ISupportsVersion
  {
    private const string className = "VersionControl";
    public static string updatesFolder = ServerGlobals.UpdatesFolder.FullName;
    public static readonly VersionControl RemotingInstance = InterceptionUtils.NewInstance<VersionControl>();

    public virtual bool IsVersionUpdateAvailable(JedVersion sourceVersion)
    {
      TraceLog.WriteApi((ISession) null, nameof (VersionControl), nameof (IsVersionUpdateAvailable), (object) sourceVersion);
      try
      {
        return this.getVersionUpdatePath(sourceVersion) != null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (VersionControl), ex);
        return false;
      }
    }

    public virtual FileStream GetVersionUpdateFileStream(JedVersion sourceVersion)
    {
      TraceLog.WriteApi((ISession) null, nameof (VersionControl), nameof (GetVersionUpdateFileStream), (object) sourceVersion);
      try
      {
        VersionDownloadItem versionUpdatePath = this.getVersionUpdatePath(sourceVersion);
        if (versionUpdatePath != null)
          return versionUpdatePath.OpenUpdateFileStream();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (VersionControl), ex);
      }
      return (FileStream) null;
    }

    private VersionDownloadItem getVersionUpdatePath(JedVersion sourceVersion)
    {
      if (sourceVersion >= this.Version)
        return (VersionDownloadItem) null;
      VersionDownloadHistory versionHistory = this.getVersionHistory();
      if (versionHistory == null)
        return (VersionDownloadItem) null;
      VersionDownloadItem versionDownloadItem = versionHistory.FindItem(sourceVersion, this.Version, VersionMatchType.UpgradePath);
      return versionDownloadItem != null && versionDownloadItem.UpdateFileExists() ? versionDownloadItem : (VersionDownloadItem) null;
    }

    public JedVersion Version => VersionInformation.CurrentVersion.Version;

    public virtual bool IsCompatibleWithVersion(JedVersion version)
    {
      return this.IsCompatibleWithVersion(version, false);
    }

    public virtual bool IsCompatibleWithVersion(JedVersion version, bool allowRevisionDiscrepancy)
    {
      TraceLog.WriteApi((ISession) null, nameof (VersionControl), nameof (IsCompatibleWithVersion), (object) version, (object) allowRevisionDiscrepancy);
      if (!allowRevisionDiscrepancy)
        return this.Version == version;
      JedVersion version1 = this.Version;
      if (version1.MajorVersion == version.MajorVersion)
      {
        version1 = this.Version;
        if (version1.MinorVersion == version.MinorVersion)
        {
          version1 = this.Version;
          return version1.Revision == version.Revision;
        }
      }
      return false;
    }

    private VersionDownloadHistory getVersionHistory()
    {
      try
      {
        return new VersionDownloadHistory(VersionControl.updatesFolder);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (VersionControl), "Error reading version history: " + (object) ex);
        return (VersionDownloadHistory) null;
      }
    }

    public override sealed object InitializeLifetimeService() => base.InitializeLifetimeService();

    public override sealed ObjRef CreateObjRef(Type requestedType)
    {
      return base.CreateObjRef(requestedType);
    }
  }
}

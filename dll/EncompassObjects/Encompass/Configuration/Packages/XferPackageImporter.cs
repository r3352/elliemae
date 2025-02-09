// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.Packages.XferPackageImporter
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Packages;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.Configuration.Packages
{
  public class XferPackageImporter : SessionBoundObject
  {
    private XferPackageImportConflictResolver conflictResolver;

    internal XferPackageImporter(Session session)
      : base(session)
    {
    }

    public bool Import(XferPackage package, XferPackageImportConflictOption conflictOption)
    {
      if (package == null)
        throw new ArgumentNullException(nameof (package));
      this.conflictResolver = (XferPackageImportConflictResolver) null;
      return new PackageImporter(this.Session.Connection, (PackageImportConflictOption) conflictOption).Import(package.Unwrap());
    }

    public bool Import(XferPackage package, XferPackageImportConflictResolver conflictResolver)
    {
      if (package == null)
        throw new ArgumentNullException(nameof (package));
      this.conflictResolver = conflictResolver != null ? conflictResolver : throw new ArgumentNullException(nameof (conflictResolver));
      // ISSUE: method pointer
      return new PackageImporter(this.Session.Connection, new PackageImportConflictResolver((object) this, __methodptr(resolvePackageConflict))).Import(package.Unwrap());
    }

    private PackageImportConflictOption resolvePackageConflict(
      ExportPackageObjectType objectType,
      string objectName)
    {
      return (PackageImportConflictOption) this.conflictResolver((XferPackageObjectType) objectType, objectName);
    }
  }
}

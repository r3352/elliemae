// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.Packages.XferPackageImporter
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Packages;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.Configuration.Packages
{
  /// <summary>
  /// Provides the methods needed to import a <see cref="T:EllieMae.Encompass.Configuration.Packages.XferPackage" /> into an Encompass system.
  /// </summary>
  public class XferPackageImporter : SessionBoundObject
  {
    private XferPackageImportConflictResolver conflictResolver;

    internal XferPackageImporter(Session session)
      : base(session)
    {
    }

    /// <summary>Imports a package into the Encompass system.</summary>
    /// <param name="package">The <see cref="T:EllieMae.Encompass.Configuration.Packages.XferPackage" /> to be imported.</param>
    /// <param name="conflictOption">Determines how the import process will handle conflicts between
    /// the package contents and data already on the server.</param>
    /// <returns>Returns <c>true</c> if the import completed successfully, <c>false</c> if the import
    /// was aborted.</returns>
    /// <remarks>A <c>false</c> return value is only possible if <see cref="F:EllieMae.EMLite.Packages.PackageImportConflictOption.Abort" />
    /// is specified as the <c>conflictOption</c> parameter.</remarks>
    public bool Import(XferPackage package, XferPackageImportConflictOption conflictOption)
    {
      if (package == null)
        throw new ArgumentNullException(nameof (package));
      this.conflictResolver = (XferPackageImportConflictResolver) null;
      return new PackageImporter(this.Session.Connection, (PackageImportConflictOption) conflictOption).Import(package.Unwrap());
    }

    /// <summary>Imports a transfer package into the Encompass system.</summary>
    /// <param name="package">The <see cref="T:EllieMae.Encompass.Configuration.Packages.XferPackage" /> to be imported.</param>
    /// <param name="conflictResolver">A method which will be invoked whenever a conflict is detected
    /// between the package contents and data already on the server.</param>
    /// <returns>Returns <c>true</c> if the import completed successfully, <c>false</c> if the import
    /// was aborted.</returns>
    /// <remarks>A <c>false</c> return value is only possible if the <c>conflictResolver</c>
    /// method returns the <see cref="F:EllieMae.EMLite.Packages.PackageImportConflictOption.Abort" /> value.</remarks>
    public bool Import(XferPackage package, XferPackageImportConflictResolver conflictResolver)
    {
      if (package == null)
        throw new ArgumentNullException(nameof (package));
      this.conflictResolver = conflictResolver != null ? conflictResolver : throw new ArgumentNullException(nameof (conflictResolver));
      return new PackageImporter(this.Session.Connection, new PackageImportConflictResolver(this.resolvePackageConflict)).Import(package.Unwrap());
    }

    /// <summary>Resolves conflicts during the import process.</summary>
    private PackageImportConflictOption resolvePackageConflict(
      ExportPackageObjectType objectType,
      string objectName)
    {
      return (PackageImportConflictOption) this.conflictResolver((XferPackageObjectType) objectType, objectName);
    }
  }
}

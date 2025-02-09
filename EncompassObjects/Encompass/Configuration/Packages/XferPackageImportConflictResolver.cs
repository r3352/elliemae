// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.Packages.XferPackageImportConflictResolver
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.Configuration.Packages
{
  /// <summary>
  /// A delegate used to allow a custom implementation of how conflicts are handled during the import process.
  /// </summary>
  /// <param name="objectType">The type of object for which the conflict occurred.</param>
  /// <param name="objectName">The name of the object in conflict.</param>
  /// <returns>The method's return value will indicate how the import resolves the conflict. A value
  /// of <see cref="F:EllieMae.Encompass.Configuration.Packages.XferPackageImportConflictOption.Abort" /> will cause the export to end
  /// immediately.</returns>
  public delegate XferPackageImportConflictOption XferPackageImportConflictResolver(
    XferPackageObjectType objectType,
    string objectName);
}

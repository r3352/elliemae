// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.Packages.XferPackageImportConflictOption
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.Configuration.Packages
{
  /// <summary>
  /// Indicates the default action when a conflict is found during a package import.
  /// </summary>
  public enum XferPackageImportConflictOption
  {
    /// <summary>No option specified.</summary>
    None,
    /// <summary>Existing items will be overwritten with content from the package.</summary>
    Overwrite,
    /// <summary>Items in he package will be skipped if a matching item already exists.</summary>
    Skip,
    /// <summary>The import process should abort if a duplicate occurs.</summary>
    Abort,
  }
}

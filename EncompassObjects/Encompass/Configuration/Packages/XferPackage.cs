// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.Packages.XferPackage
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Packages;

#nullable disable
namespace EllieMae.Encompass.Configuration.Packages
{
  /// <summary>
  /// Represents a package of Encompass setting data which can be transferred between Encompass systems.
  /// </summary>
  public class XferPackage
  {
    private ExportPackage package;

    internal XferPackage(ExportPackage package) => this.package = package;

    /// <summary>Constructs a new, empty transfer package.</summary>
    public XferPackage() => this.package = new ExportPackage();

    /// <summary>Constructs a transfer package from a file on disk.</summary>
    /// <param name="path">The path of the EMPKG file containing the transfer package.</param>
    public XferPackage(string path) => this.package = new ExportPackage(path);

    /// <summary>Returns the internal ExportPackage object.</summary>
    internal ExportPackage Unwrap() => this.package;
  }
}

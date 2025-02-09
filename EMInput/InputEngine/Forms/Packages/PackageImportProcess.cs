// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.PackageImportProcess
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.Packages;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms.Packages
{
  public class PackageImportProcess
  {
    private PackageImporter importer;
    private Hashtable promptStatus;

    public PackageImportProcess(IConnection conn)
    {
      this.importer = new PackageImporter(conn, new PackageImportConflictResolver(this.resolveImportConflict));
    }

    public bool ImportPackage(ExportPackage package)
    {
      this.promptStatus = new Hashtable();
      return this.importer.Import(package);
    }

    public string[] GetImportedFormIDs() => this.importer.GetImportedFormIDs();

    private PackageImportConflictOption resolveImportConflict(
      ExportPackageObjectType objectType,
      string objectName)
    {
      if (this.promptStatus.Contains((object) objectType))
        return (PackageImportConflictOption) this.promptStatus[(object) objectType];
      using (ImportOverwritePrompt importOverwritePrompt = new ImportOverwritePrompt(objectType, objectName))
      {
        PackageImportConflictOption importConflictOption = importOverwritePrompt.ShowOverwritePrompt((IWin32Window) Session.Application);
        if (importConflictOption == PackageImportConflictOption.Abort || !importOverwritePrompt.ApplyToAll)
          return importConflictOption;
        this.promptStatus[(object) objectType] = (object) importConflictOption;
        return importConflictOption;
      }
    }
  }
}

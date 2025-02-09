// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.PointImportOptions
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Import;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class PointImportOptions
  {
    public string DatabasePath = "";
    public bool AutoDetectDatabase;

    public PointImportOptions()
    {
      using (ImportPointSettings importPointSettings = new ImportPointSettings(""))
        this.DatabasePath = importPointSettings.GetPointInstallationPath("Templates");
      if (!(this.DatabasePath != ""))
        return;
      this.DatabasePath = Path.Combine(this.DatabasePath, "Database\\");
      this.AutoDetectDatabase = true;
    }
  }
}

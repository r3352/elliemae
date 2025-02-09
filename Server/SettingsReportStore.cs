// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SettingsReportStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class SettingsReportStore
  {
    private const string className = "SettingsReportStore�";

    private SettingsReportStore()
    {
    }

    public static void Create(string fileName, string xmlData)
    {
      File.WriteAllText(SettingsReportStore.getFilePath(fileName), xmlData);
    }

    public static string GetFileData(string fileName)
    {
      string filePath = SettingsReportStore.getFilePath(fileName);
      return File.Exists(filePath) ? File.ReadAllText(filePath) : (string) null;
    }

    private static string getFilePath(string fileName)
    {
      return ClientContext.GetCurrent().Settings.GetDataFilePath("SettingsReports\\" + fileName + ".xml");
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.SkyDrive.SupportingDataFile
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.SkyDrive
{
  public class SupportingDataFile
  {
    public SupportingDataFile(string key, string localPath, Task task)
    {
      this.Key = key;
      this.LocalPath = localPath;
      this.Task = task;
    }

    public string Key { get; }

    public string LocalPath { get; }

    public Task Task { get; }
  }
}

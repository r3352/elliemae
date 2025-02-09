// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ReportSettingsFile
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.RemotingServices;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ReportSettingsFile : IDisposable
  {
    private const string className = "ReportSettingsFile�";
    private DataFile dataFile;
    private ReportSettings settings;
    private string name;

    public ReportSettingsFile(DataFile dataFile)
    {
      this.dataFile = dataFile;
      this.name = Path.GetFileNameWithoutExtension(dataFile.Path);
    }

    public bool Exists => this.dataFile.Exists;

    public string Name => this.name;

    public ReportSettings Settings
    {
      get
      {
        if (this.settings == null && this.dataFile.Exists)
          this.settings = new ReportSettings(this.name, this.dataFile.GetText());
        return this.settings;
      }
    }

    public DataFile Data => this.dataFile;

    public void CreateNew(ReportSettings settings)
    {
      this.dataFile.CreateNew(new BinaryObject(settings.ToString(), Encoding.Default));
      this.settings = settings;
    }

    public void Delete()
    {
      this.dataFile.Delete();
      this.settings = (ReportSettings) null;
    }

    public void CheckIn(ReportSettings settings) => this.CheckIn(settings, false);

    public void CheckIn(ReportSettings settings, bool keepCheckedOut)
    {
      if (settings == null)
        this.dataFile.CheckIn((BinaryObject) null, keepCheckedOut);
      else
        this.dataFile.CheckIn(new BinaryObject(settings.ToString(), Encoding.Default));
      this.settings = settings;
    }

    public void CheckIn(BinaryObject rawData)
    {
      if (rawData == null)
        this.dataFile.CheckIn((BinaryObject) null);
      else
        this.dataFile.CheckIn(rawData);
    }

    public void UndoCheckout() => this.dataFile.UndoCheckout();

    public void Dispose() => this.dataFile.Dispose();
  }
}

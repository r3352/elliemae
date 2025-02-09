// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanSettingsProvider
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanSettingsProvider
  {
    private IConfigurationManager configMngr;

    public LoanSettingsProvider(SessionObjects sessionObjects)
    {
      this.configMngr = sessionObjects.ConfigurationManager;
    }

    public object GetSettings(Type settingsType)
    {
      BinaryObject systemSettings = this.configMngr.GetSystemSettings(settingsType.Name);
      return systemSettings == null ? settingsType.GetConstructor(Type.EmptyTypes).Invoke((object[]) null) : new XmlSerializer().Deserialize(systemSettings.OpenStream(), settingsType);
    }

    public void SaveSettings(object settings)
    {
      MemoryStream memoryStream = new MemoryStream();
      new XmlSerializer().Serialize((Stream) memoryStream, settings);
      using (BinaryObject data = new BinaryObject((Stream) memoryStream))
        this.configMngr.SaveSystemSettings(settings.GetType().Name, data);
    }
  }
}

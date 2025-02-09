// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.AttachmentXmlProviderFactory
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class AttachmentXmlProviderFactory : IAttachmentXmlProviderFactory
  {
    private readonly IServerSettings _settings;

    public AttachmentXmlProviderFactory(IServerSettings settings) => this._settings = settings;

    public IAttachmentXmlProvider CreateInstance()
    {
      StorageMode storageMode = StorageMode.FileSystemOnly;
      IStorageSettings storageSettings = (IStorageSettings) null;
      if (this._settings != null)
      {
        storageMode = (StorageMode) this._settings.GetStorageSetting("DataStore.StorageMode");
        storageSettings = this._settings.GetStorageModeSettings();
      }
      switch (storageMode)
      {
        case StorageMode.PostgresOnly:
          return (IAttachmentXmlProvider) new PostgresAttachmentXmlProvider();
        case StorageMode.BothPostgresFileSystemMaster:
          return (IAttachmentXmlProvider) new BothPostgresFileSystemMasterAttachmentXmlProvider();
        case StorageMode.BothFileSystemPostgresMaster:
          return (IAttachmentXmlProvider) new BothFileSystemPostgresMasterAttachmentXmlProvider();
        default:
          return (IAttachmentXmlProvider) new FileSystemAttachmentXmlProvider();
      }
    }
  }
}

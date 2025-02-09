// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.ImageAttachmentSettingsStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class ImageAttachmentSettingsStore
  {
    private const string className = "ImageAttachmentSettingsStore�";

    private ImageAttachmentSettingsStore()
    {
    }

    public static ImageAttachmentSettings GetImageAttachmentSettings()
    {
      return ClientContext.GetCurrent().Cache.Get<ImageAttachmentSettings>(nameof (ImageAttachmentSettingsStore), new Func<ImageAttachmentSettings>(ImageAttachmentSettingsStore.GetImageAttachmentSettingsFromDB), CacheSetting.Low);
    }

    private static ImageAttachmentSettings GetImageAttachmentSettingsFromDB()
    {
      ImageAttachmentSettings attachmentSettingsFromDb = (ImageAttachmentSettings) null;
      try
      {
        using (BinaryObject systemSettings = SystemConfiguration.GetSystemSettings("ImageAttachmentSettings"))
        {
          if (systemSettings != null)
            attachmentSettingsFromDb = systemSettings.ToObject<ImageAttachmentSettings>();
        }
      }
      catch
      {
      }
      if (attachmentSettingsFromDb == null)
        attachmentSettingsFromDb = new ImageAttachmentSettings();
      return attachmentSettingsFromDb;
    }

    public static void SaveImageAttachmentSettings(ImageAttachmentSettings setup)
    {
      ClientContext.GetCurrent().Cache.Put<ImageAttachmentSettings>(nameof (ImageAttachmentSettingsStore), (Action) (() =>
      {
        using (BinaryObject data = new BinaryObject((IXmlSerializable) setup))
          SystemConfiguration.SaveSystemSettings("ImageAttachmentSettings", data);
      }), new Func<ImageAttachmentSettings>(ImageAttachmentSettingsStore.GetImageAttachmentSettingsFromDB), CacheSetting.Low);
    }
  }
}

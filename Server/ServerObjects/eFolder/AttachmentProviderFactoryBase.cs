// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.AttachmentProviderFactoryBase
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class AttachmentProviderFactoryBase
  {
    private const string DB = "DB�";

    public static IAttachmentProviderBase CreateInstance(
      string instanceLevelFlag,
      IAttachmentXmlProviderFactory existingFactory)
    {
      IAttachmentXmlProvider instance = existingFactory?.CreateInstance();
      switch (instance)
      {
        case null:
        case FileSystemAttachmentXmlProvider _:
          if (string.Equals(instanceLevelFlag, "DB", StringComparison.InvariantCultureIgnoreCase))
            return (IAttachmentProviderBase) new SQLAttachmentXMLProvider();
          break;
      }
      return (IAttachmentProviderBase) instance;
    }
  }
}

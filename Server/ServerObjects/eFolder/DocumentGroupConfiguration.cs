// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.DocumentGroupConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class DocumentGroupConfiguration
  {
    private DocumentGroupConfiguration()
    {
    }

    public static DocumentGroupSetup GetDocumentGroupSetupFromXml()
    {
      try
      {
        using (BinaryObject systemSettings = SystemConfiguration.GetSystemSettings("DocumentGroupList"))
        {
          if (systemSettings != null)
            return systemSettings.ToObject<DocumentGroupSetup>();
        }
      }
      catch
      {
      }
      return new DocumentGroupSetup();
    }

    public static void SaveDocumentGroupSetupToXml(DocumentGroupSetup setup)
    {
      using (BinaryObject data = new BinaryObject((IXmlSerializable) setup))
        SystemConfiguration.SaveSystemSettings("DocumentGroupList", data);
    }
  }
}

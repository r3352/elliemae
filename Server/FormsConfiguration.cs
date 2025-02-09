// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.FormsConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class FormsConfiguration
  {
    private static ConcurrentDictionary<FormConfigFile, string> CachedFormsConfiguration = new ConcurrentDictionary<FormConfigFile, string>();
    public const string className = "FormsConfiguration�";

    private FormsConfiguration()
    {
    }

    public static string GetFormConfigurationFile(FormConfigFile fileType)
    {
      ClientContext context = ClientContext.GetCurrent();
      return FormsConfiguration.CachedFormsConfiguration.GetOrAdd(fileType, (Func<FormConfigFile, string>) (ft => FormsConfiguration.getFormData(context, ft)));
    }

    private static string getFormData(ClientContext context, FormConfigFile fileType)
    {
      string formData = (string) null;
      using (DataFile latestVersion = FileStore.GetLatestVersion(FormsConfiguration.getFormConfigFilePath(context, fileType)))
      {
        if (!latestVersion.Exists)
          throw new ServerException("The form configuration file of type '" + (object) fileType + "' was not found.");
        using (BinaryObject data = latestVersion.GetData())
          formData = data.ToString(Encoding.UTF8);
      }
      int startIndex = formData.IndexOf("<");
      if (startIndex > 0)
        formData = formData.Substring(startIndex);
      return formData;
    }

    private static string getFormConfigFilePath(ClientContext context, FormConfigFile fileType)
    {
      switch (fileType)
      {
        case FormConfigFile.FormGroupList:
          return Path.Combine(context.Settings.ApplicationDir, "Documents\\EMFormGroupList.xml");
        case FormConfigFile.InOutFormMapping:
          return Path.Combine(context.Settings.ApplicationDir, "Documents\\InOutFormMapping.xml");
        case FormConfigFile.OutFormAndFileMapping:
          return Path.Combine(context.Settings.ApplicationDir, "Documents\\OutFormAndFileMapping.xml");
        default:
          throw new ArgumentException("Invalid form configuration file type: " + (object) fileType);
      }
    }
  }
}

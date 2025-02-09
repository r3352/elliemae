// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.HtmlEmail.HtmlEmailTemplateStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.HtmlEmail
{
  internal sealed class HtmlEmailTemplateStore
  {
    private const string className = "HtmlEmailTemplateStore�";

    public static HtmlEmailTemplate[] GetTemplates(string userID, HtmlEmailTemplateType type)
    {
      return HtmlEmailTemplateStore.getTemplateCollection(userID).GetByType(type);
    }

    public static HtmlEmailTemplate GetTemplate(string userID, string guid)
    {
      return HtmlEmailTemplateStore.getTemplateCollection(userID).GetByID(guid);
    }

    public static void SaveTemplate(HtmlEmailTemplate template)
    {
      HtmlEmailTemplateCollection templateCollection = HtmlEmailTemplateStore.getTemplateCollection(template.OwnerID);
      templateCollection.Add(template);
      HtmlEmailTemplateStore.setTemplateCollection(template.OwnerID, templateCollection);
    }

    public static void DeleteTemplate(HtmlEmailTemplate template)
    {
      HtmlEmailTemplateCollection templateCollection = HtmlEmailTemplateStore.getTemplateCollection(template.OwnerID);
      if (!templateCollection.Contains(template))
        return;
      templateCollection.Remove(template);
      HtmlEmailTemplateStore.setTemplateCollection(template.OwnerID, templateCollection);
    }

    private static string getXmlFilePath(string userID)
    {
      ClientContext current = ClientContext.GetCurrent();
      return string.IsNullOrEmpty(userID) ? current.Settings.GetDataFilePath("HtmlEmailTemplates.xml") : current.Settings.GetUserDataFilePath(userID, "HtmlEmailTemplates.xml");
    }

    private static HtmlEmailTemplateCollection getTemplateCollection(string userID)
    {
      string xmlFilePath = HtmlEmailTemplateStore.getXmlFilePath(userID);
      BinaryObject binaryObject = (BinaryObject) null;
      using (DataFile latestVersion = FileStore.GetLatestVersion(xmlFilePath))
      {
        if (latestVersion.Exists)
          binaryObject = latestVersion.GetData();
      }
      HtmlEmailTemplateCollection templateCollection;
      try
      {
        if (binaryObject != null)
        {
          templateCollection = (HtmlEmailTemplateCollection) new XmlSerializer().Deserialize(binaryObject.AsStream(), typeof (HtmlEmailTemplateCollection));
          binaryObject.Dispose();
        }
        else
          templateCollection = HtmlEmailTemplateStore.migrateLoanStatusData(userID);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (HtmlEmailTemplateStore), "Error deserializing xml file: " + xmlFilePath + ": " + (object) ex);
        templateCollection = new HtmlEmailTemplateCollection();
      }
      finally
      {
        binaryObject?.Dispose();
      }
      return templateCollection;
    }

    private static void setTemplateCollection(string userID, HtmlEmailTemplateCollection collection)
    {
      string xmlFilePath = HtmlEmailTemplateStore.getXmlFilePath(userID);
      XmlDataStore.Serialize((object) collection, xmlFilePath);
    }

    private static HtmlEmailTemplateCollection migrateLoanStatusData(string userID)
    {
      if (string.IsNullOrEmpty(userID))
      {
        ClientContext current = ClientContext.GetCurrent();
        HtmlEmailTemplateCollection collection = (HtmlEmailTemplateCollection) XmlDataStore.Deserialize(typeof (HtmlEmailTemplateCollection), Company.GetServerLicense((IClientContext) current).Edition != EncompassEdition.Banker ? Path.Combine(current.Settings.ApplicationDir, "Samples\\Broker\\Data\\HtmlEmailTemplates.xml") : Path.Combine(current.Settings.ApplicationDir, "Samples\\Banker\\Data\\HtmlEmailTemplates.xml"));
        HtmlEmailTemplateStore.setTemplateCollection(userID, collection);
        return collection;
      }
      HtmlEmailTemplateCollection collection1 = LoanStatusEmailTemplateStore.Get(userID).MigrateData(userID);
      HtmlEmailTemplateStore.setTemplateCollection(userID, collection1);
      return collection1;
    }
  }
}

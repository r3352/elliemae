// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.StatusOnline.StatusOnlineStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.StatusOnline
{
  public class StatusOnlineStore
  {
    private const string className = "StatusOnlineStore�";

    public static StatusOnlineSetup GetSetup(string userID)
    {
      string xmlFilePath = StatusOnlineStore.getXmlFilePath(userID);
      BinaryObject binaryObject = (BinaryObject) null;
      using (DataFile latestVersion = FileStore.GetLatestVersion(xmlFilePath))
      {
        if (latestVersion.Exists)
          binaryObject = latestVersion.GetData();
      }
      StatusOnlineSetup setup;
      try
      {
        setup = binaryObject == null ? StatusOnlineStore.migrateLoanStatusData(userID) : (StatusOnlineSetup) new XmlSerializer().Deserialize(binaryObject.AsStream(), typeof (StatusOnlineSetup));
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (StatusOnlineStore), "Error deserializing xml file: " + xmlFilePath + ": " + (object) ex);
        setup = new StatusOnlineSetup();
      }
      finally
      {
        binaryObject?.Dispose();
      }
      return setup;
    }

    public static void SaveSetup(string userID, StatusOnlineSetup setup)
    {
      string xmlFilePath = StatusOnlineStore.getXmlFilePath(userID);
      XmlDataStore.Serialize((object) setup, xmlFilePath);
    }

    private static string getXmlFilePath(string userID)
    {
      ClientContext current = ClientContext.GetCurrent();
      return string.IsNullOrEmpty(userID) ? current.Settings.GetDataFilePath("StatusOnline.xml") : current.Settings.GetUserDataFilePath(userID, "StatusOnline.xml");
    }

    private static StatusOnlineSetup migrateLoanStatusData(string userID)
    {
      if (string.IsNullOrEmpty(userID))
      {
        StatusOnlineSetup setup = new StatusOnlineSetup();
        StatusOnlineStore.SaveSetup(userID, setup);
        return setup;
      }
      StatusOnlineSetup setup1 = LoanStatusCollectionStore.Get(userID).MigrateData(userID);
      StatusOnlineStore.SaveSetup(userID, setup1);
      return setup1;
    }
  }
}

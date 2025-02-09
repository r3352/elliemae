// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SyncConfigurations
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class SyncConfigurations
  {
    private SyncConfigurations()
    {
    }

    public static string[] GetSyncConfigurationNames(string userId)
    {
      string[] files = Directory.GetFiles(SyncConfigurations.getSyncConfigFolder(userId), "*.xml");
      string[] configurationNames = new string[files.Length];
      int num = 0;
      foreach (string path in files)
        configurationNames[num++] = FileSystem.DecodeFilename(Path.GetFileNameWithoutExtension(path));
      return configurationNames;
    }

    public static BinaryObject GetSyncConfiguration(string userId, string name)
    {
      using (DataFile latestVersion = FileStore.GetLatestVersion(SyncConfigurations.getSyncConfigFilePath(userId, name)))
        return !latestVersion.Exists ? (BinaryObject) null : latestVersion.GetData();
    }

    public static void SaveSyncConfiguration(string userId, string name, BinaryObject o)
    {
      using (DataFile dataFile = FileStore.CheckOut(SyncConfigurations.getSyncConfigFilePath(userId, name), MutexAccess.Write))
        dataFile.CheckIn(o);
    }

    public static void DeleteSyncConfiguration(string userId, string name)
    {
      using (DataFile dataFile = FileStore.CheckOut(SyncConfigurations.getSyncConfigFilePath(userId, name), MutexAccess.Write))
      {
        if (dataFile.Exists)
          dataFile.Delete();
      }
      using (DataFile dataFile = FileStore.CheckOut(SyncConfigurations.getSyncMapFilePath(userId, name), MutexAccess.Write))
      {
        if (!dataFile.Exists)
          return;
        dataFile.Delete();
      }
    }

    public static void RenameSyncConfiguration(
      string userId,
      BinaryObject o,
      string oldName,
      string newName)
    {
      if (oldName.ToLower() == newName.ToLower())
        return;
      SyncConfigurations.DeleteSyncConfiguration(userId, newName);
      SyncConfigurations.SaveSyncConfiguration(userId, newName, o);
      if (File.Exists(SyncConfigurations.getSyncMapFilePath(userId, oldName)))
        File.Move(SyncConfigurations.getSyncMapFilePath(userId, oldName), SyncConfigurations.getSyncMapFilePath(userId, newName));
      SyncConfigurations.DeleteSyncConfiguration(userId, oldName);
    }

    public static BinaryObject GetSyncMap(string userId, string name)
    {
      using (DataFile latestVersion = FileStore.GetLatestVersion(SyncConfigurations.getSyncMapFilePath(userId, name)))
        return !latestVersion.Exists ? (BinaryObject) null : latestVersion.GetData();
    }

    public static void SaveSyncMap(string userId, string name, BinaryObject o)
    {
      using (DataFile dataFile = FileStore.CheckOut(SyncConfigurations.getSyncMapFilePath(userId, name), MutexAccess.Write))
        dataFile.CheckIn(o);
    }

    private static string getSyncConfigFilePath(string userId, string name)
    {
      return Path.Combine(SyncConfigurations.getSyncConfigFolder(userId), FileSystem.EncodeFilename(name, false) + ".xml");
    }

    private static string getSyncMapFilePath(string userId, string name)
    {
      return Path.Combine(SyncConfigurations.getSyncConfigFolder(userId), FileSystem.EncodeFilename(name, false) + ".map");
    }

    private static string getSyncConfigFolder(string userId)
    {
      return ClientContext.GetCurrent().Settings.GetUserDataFolderPath(userId, "Synchronization");
    }
  }
}

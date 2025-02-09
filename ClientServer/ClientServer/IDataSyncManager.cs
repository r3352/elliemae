// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IDataSyncManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IDataSyncManager
  {
    Dictionary<string, string> PostSqlParameterNameValues { get; set; }

    Dictionary<string, object> GetDataAsXmlString(
      string headTableName,
      string[] uiKeyValues,
      bool fetchFileSystemData);

    string GetMappingDataAsXmlString(string headTableName);

    DataSyncDebuggingInfo ImportData(
      string uiKey,
      string srcDataXmlString,
      string srcMappingDataXmlString,
      string headTableName,
      Dictionary<string, int> newMilestoneMapping,
      List<FileSystemSyncRecord> filesToSync);

    DataSyncDebuggingInfo GetImportOption(
      string uiKey,
      string srcDataXmlString,
      string srcMappingDataXmlString,
      string headTableName);

    Dictionary<string, string> GetHeadTablesCategoryNames();

    List<FileSystemSyncRecord> GetFileToSync(List<FileSystemSyncRecord> filesToSync);
  }
}

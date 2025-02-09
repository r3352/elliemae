// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.LoanFolderManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class LoanFolderManager : IFileManager
  {
    private List<LoanFolderInfo> extractLoanFolderInfo(string xmlDataFile, TableDef tableDef)
    {
      LoanFolderInfo[] allLoanFolderInfos = LoanFolder.GetAllLoanFolderInfos(true);
      List<LoanFolderInfo> loanFolderInfo1 = new List<LoanFolderInfo>();
      string elementXpath = tableDef.GetElementXPath(".");
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlDataFile);
      foreach (XmlNode selectNode in xmlDocument.SelectNodes(elementXpath))
      {
        string str = "";
        if (tableDef.TableName == "BR_LoanFolderRules")
          str = selectNode.SelectSingleNode("loanFolder").InnerText;
        foreach (LoanFolderInfo loanFolderInfo2 in allLoanFolderInfos)
        {
          if (loanFolderInfo2.Name == str)
          {
            loanFolderInfo1.Add(loanFolderInfo2);
            break;
          }
        }
      }
      return loanFolderInfo1;
    }

    public List<FileSystemSyncRecord> GetAssociatedFileSystemEntities(
      string xmlDataFile,
      TableDef tableDef,
      bool fetchData)
    {
      List<LoanFolderInfo> loanFolderInfo1 = this.extractLoanFolderInfo(xmlDataFile, tableDef);
      List<FileSystemSyncRecord> fileSystemEntities = new List<FileSystemSyncRecord>();
      foreach (LoanFolderInfo loanFolderInfo2 in loanFolderInfo1)
      {
        FileSystemSyncRecord systemSyncRecord = new FileSystemSyncRecord(loanFolderInfo2.Name, loanFolderInfo2.Name, "Loan Folder:" + loanFolderInfo2.DisplayName, "", tableDef.TableName, true, "");
        systemSyncRecord["Type"] = (object) Enum.GetName(typeof (LoanFolderInfo.LoanFolderType), (object) loanFolderInfo2.Type);
        if (!fileSystemEntities.Contains(systemSyncRecord))
          fileSystemEntities.Add(systemSyncRecord);
      }
      return fileSystemEntities;
    }

    public List<FileSystemSyncRecord> GetImportOption(string xmlDataFile, TableDef tableDef)
    {
      LoanFolderInfo[] allLoanFolderInfos = LoanFolder.GetAllLoanFolderInfos(true);
      List<FileSystemSyncRecord> importOption = new List<FileSystemSyncRecord>();
      string elementXpath = tableDef.GetElementXPath(".");
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlDataFile);
      foreach (XmlNode selectNode in xmlDocument.SelectNodes(elementXpath))
      {
        string str = "";
        if (tableDef.TableName == "BR_LoanFolderRules")
          str = selectNode.SelectSingleNode("loanFolder").InnerText;
        bool flag = false;
        foreach (LoanFolderInfo loanFolderInfo in allLoanFolderInfos)
        {
          if (loanFolderInfo.Name == str)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          importOption.Add(new FileSystemSyncRecord(str, str, "Loan Folder:" + str, "", tableDef.TableName, true, "")
          {
            AlreadyExist = false
          });
      }
      return importOption;
    }

    public FileSystemSyncRecord GetActualData(FileSystemSyncRecord emptyRecord) => emptyRecord;

    public Dictionary<string, string> GetDisplayPathMapping(
      List<string> physicalPaths,
      string basePath)
    {
      return new Dictionary<string, string>();
    }

    public bool SynchronizeFileSystemSyncRecord(FileSystemSyncRecord updatedRecord)
    {
      LoanFolderInfo[] allLoanFolderInfos = LoanFolder.GetAllLoanFolderInfos(true);
      try
      {
        foreach (LoanFolderInfo loanFolderInfo in allLoanFolderInfos)
        {
          if (loanFolderInfo.Name == updatedRecord.UIKey)
            return false;
        }
        new LoanFolder(updatedRecord.UIKey).Create();
        if (updatedRecord["Type"] != null)
        {
          LoanFolderInfo.LoanFolderType folderType = (LoanFolderInfo.LoanFolderType) Enum.Parse(typeof (LoanFolderInfo.LoanFolderType), string.Concat(updatedRecord["Type"]));
          LoanFolder.SetLoanFolderType(updatedRecord.UIKey, folderType, (UserInfo) null);
        }
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}

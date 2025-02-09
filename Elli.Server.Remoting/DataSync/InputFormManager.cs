// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.InputFormManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class InputFormManager : IFileManager
  {
    private List<InputFormInfo> extractInputFormInfo(string xmlDataFile, TableDef tableDef)
    {
      List<InputFormInfo> inputFormInfo = new List<InputFormInfo>();
      string elementXpath = tableDef.GetElementXPath(".");
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlDataFile);
      foreach (XmlNode selectNode in xmlDocument.SelectNodes(elementXpath))
      {
        InputFormInfo formInfoByName = InputForms.GetFormInfoByName(InputFormInfo.NormalizeName(selectNode.SelectSingleNode("Name").InnerText));
        if (formInfoByName.Type == InputFormType.Custom && !inputFormInfo.Contains(formInfoByName))
          inputFormInfo.Add(formInfoByName);
      }
      return inputFormInfo;
    }

    public List<FileSystemSyncRecord> GetImportOption(string xmlDataFile, TableDef tableDef)
    {
      List<FileSystemSyncRecord> importOption = new List<FileSystemSyncRecord>();
      string elementXpath = tableDef.GetElementXPath(".");
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlDataFile);
      foreach (XmlNode selectNode in xmlDocument.SelectNodes(elementXpath))
      {
        string innerText = selectNode.SelectSingleNode("Name").InnerText;
        InputFormInfo formInfoByName = InputForms.GetFormInfoByName(InputFormInfo.NormalizeName(innerText));
        if (formInfoByName == (InputFormInfo) null)
        {
          FileSystemSyncRecord systemSyncRecord = new FileSystemSyncRecord(selectNode.SelectSingleNode("FormID").InnerText, InputFormInfo.NormalizeName(innerText), "Input Form:" + InputFormInfo.NormalizeName(innerText), "", tableDef.TableName, false, "");
          systemSyncRecord.AlreadyExist = false;
          if (!importOption.Contains(systemSyncRecord))
            importOption.Add(systemSyncRecord);
        }
        else if (formInfoByName.Type == InputFormType.Custom)
        {
          BinaryObject customForm = InputForms.GetCustomForm(formInfoByName.FormID);
          string hashB64 = HashUtil.ComputeHashB64(customForm == null ? "" : customForm.ToString());
          FileSystemSyncRecord systemSyncRecord = new FileSystemSyncRecord(formInfoByName.FormID, formInfoByName.Name, "Input Form:" + formInfoByName.Name, "", tableDef.TableName, false, hashB64);
          systemSyncRecord.AlreadyExist = true;
          if (!importOption.Contains(systemSyncRecord))
            importOption.Add(systemSyncRecord);
        }
      }
      return importOption;
    }

    public FileSystemSyncRecord GetActualData(FileSystemSyncRecord emptyRecord)
    {
      using (BinaryObject customForm = InputForms.GetCustomForm(emptyRecord.ID))
      {
        emptyRecord.RawData = customForm == null ? "" : customForm.ToString();
        return emptyRecord;
      }
    }

    public List<FileSystemSyncRecord> GetAssociatedFileSystemEntities(
      string xmlDataFile,
      TableDef tableDef,
      bool fatchData)
    {
      List<InputFormInfo> inputFormInfo1 = this.extractInputFormInfo(xmlDataFile, tableDef);
      List<FileSystemSyncRecord> fileSystemEntities = new List<FileSystemSyncRecord>();
      foreach (InputFormInfo inputFormInfo2 in inputFormInfo1)
      {
        using (BinaryObject customForm = InputForms.GetCustomForm(inputFormInfo2.FormID))
        {
          string str = customForm == null ? "" : customForm.ToString();
          string hashB64 = HashUtil.ComputeHashB64(str);
          if (!fatchData)
            str = "";
          FileSystemSyncRecord systemSyncRecord = new FileSystemSyncRecord(inputFormInfo2.FormID, inputFormInfo2.Name, "Input Form:" + inputFormInfo2.Name, str, tableDef.TableName, false, hashB64);
          if (!fileSystemEntities.Contains(systemSyncRecord))
            fileSystemEntities.Add(systemSyncRecord);
        }
      }
      return fileSystemEntities;
    }

    public bool SynchronizeFileSystemSyncRecord(FileSystemSyncRecord updatedRecord)
    {
      InputFormInfo formInfoByName = InputForms.GetFormInfoByName(updatedRecord.UIKey);
      if (formInfoByName == (InputFormInfo) null)
        return false;
      BinaryObject formData = new BinaryObject(updatedRecord.RawData, Encoding.Default);
      try
      {
        InputForms.SaveCustomForm(formInfoByName, formData);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}

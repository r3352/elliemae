// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DocEngine.SyncedDataHelper
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DocEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.DocEngine
{
  public static class SyncedDataHelper
  {
    private const string className = "SyncedDataHelper�";
    private static readonly string sw = Tracing.SwDataEngine;
    private static ReaderWriterLock _syncedItemsLock = new ReaderWriterLock();
    private static TimeSpan _syncedItemsLockTimeout = new TimeSpan(0, 0, 30);
    private static bool _initialized = false;
    public const string CUSTOM_FIELDS_KEY = "CustomFieldDefinitions�";
    private static Dictionary<string, SyncedDataHelper.SyncedDataItem> _syncedDataItems = new Dictionary<string, SyncedDataHelper.SyncedDataItem>();
    private static Dictionary<string, string> _syncedItemsEdsChecksums = new Dictionary<string, string>();

    internal static void ReadItemsNeededByEds(XmlDocument responseXml)
    {
      if (responseXml == null)
        return;
      XmlElement xmlElement = (XmlElement) responseXml.DocumentElement.SelectSingleNode("ENCOMPASS_INSTANCE_DATA_NEEDED");
      if (xmlElement == null)
        return;
      foreach (XmlElement selectNode in xmlElement.SelectNodes("ITEM"))
        SyncedDataHelper.SetEdsItemChecksum(selectNode.GetAttribute("itemKey"), selectNode.GetAttribute("itemDM5Checksum"));
    }

    public static SyncedDataHelper.SyncedDataItem GetDataItem(string itemKey)
    {
      SyncedDataHelper.SyncedDataItem dataItem = (SyncedDataHelper.SyncedDataItem) null;
      bool flag = false;
      if (!string.IsNullOrWhiteSpace(itemKey))
      {
        try
        {
          SyncedDataHelper._syncedItemsLock.AcquireReaderLock(SyncedDataHelper._syncedItemsLockTimeout);
          flag = true;
          if (SyncedDataHelper._syncedDataItems.ContainsKey(itemKey))
            dataItem = SyncedDataHelper._syncedDataItems[itemKey];
        }
        catch (Exception ex)
        {
          Tracing.Log(SyncedDataHelper.sw, nameof (SyncedDataHelper), TraceLevel.Error, "Exception in GetDataItem: " + (object) ex);
        }
        finally
        {
          if (flag)
            SyncedDataHelper._syncedItemsLock.ReleaseReaderLock();
        }
      }
      return dataItem;
    }

    public static void SetDataItem(string itemKey, SyncedDataHelper.SyncedDataItem value)
    {
      bool flag = false;
      if (string.IsNullOrWhiteSpace(itemKey))
        return;
      try
      {
        SyncedDataHelper._syncedItemsLock.AcquireWriterLock(SyncedDataHelper._syncedItemsLockTimeout);
        flag = true;
        SyncedDataHelper._syncedDataItems[itemKey] = value;
      }
      catch (Exception ex)
      {
        Tracing.Log(SyncedDataHelper.sw, nameof (SyncedDataHelper), TraceLevel.Error, "Exception in SetDataItem: " + (object) ex);
      }
      finally
      {
        if (flag)
          SyncedDataHelper._syncedItemsLock.ReleaseWriterLock();
      }
    }

    public static string GetEdsItemChecksum(string itemKey)
    {
      string edsItemChecksum = (string) null;
      bool flag = false;
      if (!string.IsNullOrWhiteSpace(itemKey))
      {
        try
        {
          SyncedDataHelper._syncedItemsLock.AcquireReaderLock(SyncedDataHelper._syncedItemsLockTimeout);
          flag = true;
          if (SyncedDataHelper._syncedItemsEdsChecksums.ContainsKey(itemKey))
            edsItemChecksum = SyncedDataHelper._syncedItemsEdsChecksums[itemKey];
        }
        catch (Exception ex)
        {
          Tracing.Log(SyncedDataHelper.sw, nameof (SyncedDataHelper), TraceLevel.Error, "Exception in GetEdsItemChecksum: " + (object) ex);
        }
        finally
        {
          if (flag)
            SyncedDataHelper._syncedItemsLock.ReleaseReaderLock();
        }
      }
      return edsItemChecksum;
    }

    public static void SetEdsItemChecksum(string itemKey, string itemDM5Checksum)
    {
      bool flag = false;
      if (string.IsNullOrWhiteSpace(itemKey))
        return;
      try
      {
        SyncedDataHelper._syncedItemsLock.AcquireWriterLock(SyncedDataHelper._syncedItemsLockTimeout);
        flag = true;
        SyncedDataHelper._syncedItemsEdsChecksums[itemKey] = itemDM5Checksum;
      }
      catch (Exception ex)
      {
        Tracing.Log(SyncedDataHelper.sw, nameof (SyncedDataHelper), TraceLevel.Error, "Exception in SetEdsItemChecksum: " + (object) ex);
      }
      finally
      {
        if (flag)
          SyncedDataHelper._syncedItemsLock.ReleaseWriterLock();
      }
    }

    public static string GetMd5Base64String(string clearString)
    {
      return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(clearString)));
    }

    internal static void SendSyncedItems(
      DocEngineRequest req,
      SessionObjects sessionObjects,
      bool forceSendData = false)
    {
      if (!SyncedDataHelper._initialized)
        SyncedDataHelper.Initialize(sessionObjects);
      XmlElement element = req.RequestXml.CreateElement("ENCOMPASS_INSTANCE_DATA");
      bool flag1 = false;
      bool flag2 = false;
      try
      {
        SyncedDataHelper._syncedItemsLock.AcquireReaderLock(SyncedDataHelper._syncedItemsLockTimeout);
        flag2 = true;
        List<string> stringList = new List<string>();
        if (forceSendData)
          stringList.AddRange((IEnumerable<string>) SyncedDataHelper._syncedDataItems.Keys);
        else
          stringList.AddRange((IEnumerable<string>) SyncedDataHelper._syncedItemsEdsChecksums.Keys);
        foreach (string itemKey in stringList)
        {
          string edsItemChecksum = SyncedDataHelper.GetEdsItemChecksum(itemKey);
          SyncedDataHelper.SyncedDataItem dataItem = SyncedDataHelper.GetDataItem(itemKey);
          if (dataItem != null)
          {
            flag1 = true;
            XmlElement xmlElement = (XmlElement) element.AppendChild((XmlNode) element.OwnerDocument.CreateElement("ITEM"));
            xmlElement.SetAttribute("itemKey", itemKey);
            xmlElement.SetAttribute("itemDM5Checksum", dataItem.ItemMD5Checksum);
            if (dataItem.ItemMD5Checksum != edsItemChecksum)
              xmlElement.AppendChild((XmlNode) xmlElement.OwnerDocument.CreateElement("DATA")).InnerXml = dataItem.ItemDataXml;
          }
        }
        if (!flag1)
          return;
        req.RequestXml.DocumentElement.AppendChild((XmlNode) element);
      }
      catch (Exception ex)
      {
        Tracing.Log(SyncedDataHelper.sw, nameof (SyncedDataHelper), TraceLevel.Error, "Exception in SendSyncedItems: " + (object) ex);
      }
      finally
      {
        if (flag2)
          SyncedDataHelper._syncedItemsLock.ReleaseReaderLock();
      }
    }

    private static void Initialize(SessionObjects sessionObjects)
    {
      bool flag = false;
      try
      {
        SyncedDataHelper._syncedItemsLock.AcquireWriterLock(SyncedDataHelper._syncedItemsLockTimeout);
        flag = true;
        if (SyncedDataHelper._initialized)
          return;
        string xml = sessionObjects.LoanManager.GetLoanSettings().FieldSettings.CustomFields.ToXML();
        SyncedDataHelper.SetDataItem("CustomFieldDefinitions", new SyncedDataHelper.SyncedDataItem("CustomFieldDefinitions", SyncedDataHelper.GetMd5Base64String(xml), xml));
        SyncedDataHelper._initialized = true;
      }
      catch (Exception ex)
      {
        Tracing.Log(SyncedDataHelper.sw, nameof (SyncedDataHelper), TraceLevel.Error, "Exception in Initialize: " + (object) ex);
      }
      finally
      {
        if (flag)
          SyncedDataHelper._syncedItemsLock.ReleaseWriterLock();
      }
    }

    public class SyncedDataItem
    {
      public readonly string ItemKey;
      public readonly string ItemMD5Checksum;
      public readonly string ItemDataXml;

      public SyncedDataItem(string itemKey, string itemMD5Checksum, string itemDataXml)
      {
        this.ItemKey = itemKey;
        this.ItemMD5Checksum = itemMD5Checksum;
        this.ItemDataXml = itemDataXml;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.FileAttachmentStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Attachment;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects.LoanEvent;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public static class FileAttachmentStore
  {
    internal const string xmlFile = "Attachments.xml�";
    private const string className = "FileAttachmentStore�";
    private const string migrationHazelCastLockName = "AttachmentMigrations�";

    public static string AttachmentFileName => "Attachments.xml";

    public static FileAttachment[] GetFileAttachments(
      Loan loan,
      bool includeRemoved,
      IAttachmentXmlProviderFactory providerFactory = null,
      IEnumerable<string> attachmentIds = null,
      bool forceLatest = false)
    {
      IAttachmentProviderBase attachmentProvider1 = FileAttachmentStore.GetAttachmentProvider(loan, providerFactory);
      XmlDocument attachmentXml;
      if (attachmentProvider1 != null)
      {
        if (attachmentProvider1 is IAttachmentProvider attachmentProvider2)
          return attachmentProvider2.GetAttachments(loan, attachmentIds, includeRemoved);
        attachmentXml = attachmentProvider1.GetAttachmentXml(loan);
      }
      else
      {
        attachmentXml = FileAttachmentStore.getAttachmentXml(loan);
        if (forceLatest)
          FileAttachmentStore.CheckAttachmentXmlVersion(loan, ref attachmentXml, nameof (GetFileAttachments), false);
      }
      return FileAttachmentStore.BuildAttachments(attachmentXml, includeRemoved);
    }

    public static FileAttachment[] BuildAttachments(XmlDocument xmlDoc, bool includeRemoved)
    {
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (XmlElement childNode1 in xmlDoc.DocumentElement.ChildNodes)
      {
        if (childNode1.Name == "File")
        {
          NativeAttachment nativeAttachment = new NativeAttachment(childNode1, false);
          fileAttachmentList.Add((FileAttachment) nativeAttachment);
        }
        else if (childNode1.Name == "Image")
        {
          ImageAttachment imageAttachment = new ImageAttachment(childNode1, false);
          fileAttachmentList.Add((FileAttachment) imageAttachment);
        }
        else if (childNode1.Name == "Background")
        {
          BackgroundAttachment backgroundAttachment = new BackgroundAttachment(childNode1, false);
          fileAttachmentList.Add((FileAttachment) backgroundAttachment);
        }
        else if (childNode1.Name == "Cloud")
        {
          CloudAttachment cloudAttachment = new CloudAttachment(childNode1, false);
          fileAttachmentList.Add((FileAttachment) cloudAttachment);
        }
        else if (includeRemoved && childNode1.Name == "Removed")
        {
          foreach (XmlElement childNode2 in childNode1.ChildNodes)
          {
            if (childNode2.Name == "File")
            {
              NativeAttachment nativeAttachment = new NativeAttachment(childNode2, true);
              fileAttachmentList.Add((FileAttachment) nativeAttachment);
            }
            else if (childNode2.Name == "Image")
            {
              ImageAttachment imageAttachment = new ImageAttachment(childNode2, true);
              fileAttachmentList.Add((FileAttachment) imageAttachment);
            }
            else if (childNode2.Name == "Background")
            {
              BackgroundAttachment backgroundAttachment = new BackgroundAttachment(childNode2, true);
              fileAttachmentList.Add((FileAttachment) backgroundAttachment);
            }
            else if (childNode2.Name == "Cloud")
            {
              CloudAttachment cloudAttachment = new CloudAttachment(childNode2, true);
              fileAttachmentList.Add((FileAttachment) cloudAttachment);
            }
          }
        }
      }
      return fileAttachmentList.ToArray();
    }

    public static bool DeleteAttachment(
      Loan loan,
      FileAttachment attachment,
      IAttachmentXmlProviderFactory providerFactory = null)
    {
      IAttachmentProviderBase attachmentProvider = FileAttachmentStore.GetAttachmentProvider(loan, providerFactory);
      return attachmentProvider != null && attachmentProvider is IAttachmentProvider ? ((IAttachmentProvider) attachmentProvider).DeleteAttachments(loan, attachment) : FileAttachmentStore.DeleteAttachmentXml(loan, attachment, (IAttachmentXmlProvider) attachmentProvider);
    }

    private static bool DeleteAttachmentXml(
      Loan loan,
      FileAttachment attachment,
      IAttachmentXmlProvider providerInstance)
    {
      using (FileAttachmentStore.CheckOut(loan.Identity.Guid))
      {
        XmlDocument xmlDoc = providerInstance != null ? providerInstance.GetAttachmentXml(loan) : FileAttachmentStore.getAttachmentXml(loan);
        string outerXml = xmlDoc.OuterXml;
        bool flag = FileAttachmentStore.deleteAttachmentXmlNode(xmlDoc, attachment);
        if (outerXml != xmlDoc.OuterXml)
        {
          if (providerInstance != null)
            providerInstance.SaveAttachmentXml(loan, xmlDoc);
          else
            FileAttachmentStore.saveAttachmentXml(loan, xmlDoc);
        }
        return flag;
      }
    }

    public static FileAttachment GetFileAttachment(
      Loan loan,
      string attachmentId,
      IAttachmentXmlProviderFactory providerFactory = null,
      bool forceLatest = false)
    {
      IAttachmentProviderBase attachmentProvider = FileAttachmentStore.GetAttachmentProvider(loan, providerFactory);
      if (attachmentProvider == null || !(attachmentProvider is IAttachmentProvider))
        return FileAttachmentStore.GetFileAttachmentXML(loan, attachmentId, (IAttachmentXmlProvider) attachmentProvider, forceLatest);
      FileAttachment[] attachments = ((IAttachmentProvider) attachmentProvider).GetAttachments(loan, new string[1]
      {
        attachmentId
      });
      return attachments == null ? (FileAttachment) null : ((IEnumerable<FileAttachment>) attachments).FirstOrDefault<FileAttachment>();
    }

    private static FileAttachment GetFileAttachmentXML(
      Loan loan,
      string attachmentId,
      IAttachmentXmlProvider providerInstance,
      bool forceLatest = false)
    {
      FileAttachment fileAttachmentXml = (FileAttachment) null;
      bool isRemoved = false;
      XmlDocument xmlDoc = providerInstance != null ? providerInstance.GetAttachmentXml(loan) : FileAttachmentStore.getAttachmentXml(loan);
      if (forceLatest)
        FileAttachmentStore.CheckAttachmentXmlVersion(loan, ref xmlDoc, nameof (GetFileAttachmentXML), false);
      string xpath1 = string.Format("//File[@Filename='{0}']", (object) attachmentId);
      string xpath2 = string.Format("//Image[@ID='{0}']", (object) attachmentId);
      string xpath3 = string.Format("//Cloud[@ID='{0}']", (object) attachmentId);
      string xpath4 = string.Format("//Background[@ID='{0}']", (object) attachmentId);
      XmlNode elm = ((xmlDoc.SelectSingleNode(xpath1) ?? xmlDoc.SelectSingleNode(xpath2)) ?? xmlDoc.SelectSingleNode(xpath3)) ?? xmlDoc.SelectSingleNode(xpath4);
      if (elm != null)
      {
        if (elm.ParentNode.Name.Equals("Removed", StringComparison.InvariantCultureIgnoreCase))
          isRemoved = true;
        if (elm.Name == "File")
          fileAttachmentXml = (FileAttachment) new NativeAttachment((XmlElement) elm, isRemoved);
        else if (elm.Name == "Image")
          fileAttachmentXml = (FileAttachment) new ImageAttachment((XmlElement) elm, isRemoved);
        else if (elm.Name == "Cloud")
          fileAttachmentXml = (FileAttachment) new CloudAttachment((XmlElement) elm, isRemoved);
        else if (elm.Name == "Background")
          fileAttachmentXml = (FileAttachment) new BackgroundAttachment((XmlElement) elm, isRemoved);
      }
      return fileAttachmentXml;
    }

    public static void ReplaceBackgroundAttachment(
      Loan loan,
      FileAttachment attachment,
      IAttachmentXmlProviderFactory providerFactory = null)
    {
      IAttachmentProviderBase attachmentProvider = FileAttachmentStore.GetAttachmentProvider(loan, providerFactory);
      if (attachmentProvider != null && attachmentProvider is IAttachmentProvider)
      {
        if (attachment.AttachmentType != AttachmentType.Native && attachment.AttachmentType != AttachmentType.Image)
          throw new ArgumentException(string.Format("{0} attachmentType is not supported for this operation", (object) attachment.AttachmentType));
        ((IAttachmentProvider) attachmentProvider).ReplaceAttachment(loan, attachment);
      }
      else
        FileAttachmentStore.ReplaceBackgroundAttachmentXML(loan, attachment, (IAttachmentXmlProvider) attachmentProvider);
    }

    private static void ReplaceBackgroundAttachmentXML(
      Loan loan,
      FileAttachment attachment,
      IAttachmentXmlProvider providerInstance)
    {
      using (FileAttachmentStore.CheckOut(loan.Identity.Guid))
      {
        XmlDocument xmlDoc = providerInstance != null ? providerInstance.GetAttachmentXml(loan) : FileAttachmentStore.getAttachmentXml(loan);
        string outerXml = xmlDoc.OuterXml;
        FileAttachmentStore.replaceAttachment(xmlDoc, attachment);
        if (!(outerXml != xmlDoc.OuterXml))
          return;
        if (providerInstance != null)
          providerInstance.SaveAttachmentXml(loan, xmlDoc);
        else
          FileAttachmentStore.saveAttachmentXml(loan, xmlDoc);
      }
    }

    public static void SaveFileAttachments(
      Loan loan,
      FileAttachment[] attachmentList,
      IAttachmentXmlProviderFactory providerFactory = null)
    {
      if (attachmentList.Length == 0)
        return;
      IAttachmentProviderBase attachmentProvider = FileAttachmentStore.GetAttachmentProvider(loan, providerFactory);
      bool flag = attachmentProvider == null || !(attachmentProvider is IAttachmentProvider) ? FileAttachmentStore.SaveFileAttachmentsXMLFormat(loan, attachmentList, (IAttachmentXmlProvider) attachmentProvider) : ((IAttachmentProvider) attachmentProvider).SaveAttachments(loan, attachmentList);
      string loanFileLocation = string.Format("\\Loans\\{0}\\", (object) loan.Identity.LoanFolder) + loan.LoanData.GUID;
      bool isSourceEncompass = EncompassServer.ServerMode != EncompassServerMode.Service;
      FileAttachment fileAttachment = ((IEnumerable<FileAttachment>) attachmentList).FirstOrDefault<FileAttachment>();
      if (!flag)
        return;
      FileAttachmentStore.PublishLoanAttachmentKafkaEvent(loan.LoanData.GUID, fileAttachment.UserID, isSourceEncompass, attachmentList, LoanEventType.Unspecified, loan.LastModifiedUTC, loanFileLocation);
    }

    private static bool SaveFileAttachmentsXMLFormat(
      Loan loan,
      FileAttachment[] attachmentList,
      IAttachmentXmlProvider providerInstance,
      bool overWrite = false)
    {
      using (FileAttachmentStore.CheckOut(loan.Identity.Guid))
      {
        XmlDocument xmlDoc = (XmlDocument) null;
        bool flag = false;
        if (!overWrite)
          xmlDoc = providerInstance != null ? providerInstance.GetAttachmentXml(loan) : FileAttachmentStore.getAttachmentXml(loan, true);
        bool isNewAttachment = false;
        if (xmlDoc == null)
        {
          isNewAttachment = true;
          xmlDoc = new XmlDocument();
          xmlDoc.LoadXml("<Attachments/>");
        }
        if (!isNewAttachment)
          FileAttachmentStore.CheckAttachmentXmlVersion(loan, ref xmlDoc, nameof (SaveFileAttachmentsXMLFormat));
        string outerXml = xmlDoc.OuterXml;
        TraceLog.WriteVerbose(nameof (FileAttachmentStore), string.Format("SaveFileAttachment: LoanId:{0} GetAttachmentXml Response:{1}", (object) loan.Identity.Guid, (object) outerXml));
        foreach (FileAttachment attachment in attachmentList)
        {
          if (FileAttachmentStore.writeAttachment(xmlDoc, attachment))
            flag = true;
        }
        int attachmentsCount = FileAttachmentStore.GetAttachmentsCount(xmlDoc);
        if (outerXml != xmlDoc.OuterXml)
        {
          try
          {
            if (providerInstance != null)
              providerInstance.SaveAttachmentXml(loan, xmlDoc);
            else
              FileAttachmentStore.saveAttachmentXml(loan, xmlDoc, isNewAttachment);
            TraceLog.WriteVerbose(nameof (FileAttachmentStore), string.Format("SaveFileAttachment: Write to Attachment xml Complete. LoanId:{0} Xml:{1}", (object) loan.Identity.Guid, (object) xmlDoc.OuterXml));
          }
          catch (IOException ex)
          {
            TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("SaveFileAttachment: Creating Attachment xml failed because file already exists, retrying to read existing. LoanId:{0} Xml:{1}  Message: {2}", (object) loan.Identity.Guid, (object) xmlDoc.OuterXml, (object) ex.Message));
            int num1 = 0;
            int num2 = 10;
            int millisecondsTimeout = 150;
            XmlDocument attachmentXml;
            do
            {
              ++num1;
              Thread.Sleep(millisecondsTimeout);
              attachmentXml = FileAttachmentStore.getAttachmentXml(loan, true);
              if (attachmentXml == null)
                TraceLog.WriteVerbose(nameof (FileAttachmentStore), string.Format("SaveFileAttachment: Retrying reading attachment. ReTryCount {2}, result: File not found:  LoanId:{0} Xml:{1}", (object) loan.Identity.Guid, (object) xmlDoc.OuterXml, (object) num1));
              else
                goto label_20;
            }
            while (num1 < num2);
            goto label_29;
label_20:
            TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("SaveFileAttachment: Reading attachment successful. ReTryCount {2}. LoanId:{0} Xml:{1}", (object) loan.Identity.Guid, (object) xmlDoc.OuterXml, (object) num1));
            foreach (FileAttachment attachment in attachmentList)
            {
              if (FileAttachmentStore.writeAttachment(attachmentXml, attachment))
                flag = true;
            }
            attachmentsCount = FileAttachmentStore.GetAttachmentsCount(attachmentXml);
            if (providerInstance != null)
              providerInstance.SaveAttachmentXml(loan, attachmentXml);
            else
              FileAttachmentStore.saveAttachmentXml(loan, attachmentXml);
            TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("Write to Attachment xml Complete. LoanId:{0} Xml:{1}", (object) loan.Identity.Guid, (object) attachmentXml.OuterXml));
            goto label_30;
label_29:
            TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("Create Attachment failed after retring for {2} times elapsed {3}ms.  Please try again. LoanId:{0}, Xml:{1}", (object) loan.Identity.Guid, (object) xmlDoc.OuterXml, (object) num1, (object) (num1 * millisecondsTimeout)));
            throw new Exception(string.Format("Create Attachment failed.  Please try again.  Class: FileAttachmentStore, Method: SaveAtachmentXml, LoanId {0}", (object) loan.Identity.Guid));
          }
label_30:
          FileAttachmentStore.ReadAttachmentWithLatestChanges(loan, attachmentsCount);
        }
        return flag;
      }
    }

    private static IAttachmentProviderBase GetAttachmentProvider(
      Loan loan,
      IAttachmentXmlProviderFactory providerFactory)
    {
      return AttachmentProviderFactoryBase.CreateInstance(FileAttachmentStore.ReadRetry((Func<string>) (() =>
      {
        LoanProperty[] propertySettings = LoanPropertySettingsAccessor.GetLoanPropertySettings(loan.Identity.Guid, "LoanStorage");
        if (propertySettings == null || propertySettings.Length == 0)
          return (string) null;
        return ((IEnumerable<LoanProperty>) propertySettings).FirstOrDefault<LoanProperty>((System.Func<LoanProperty, bool>) (x => string.Equals(x.Attribute, "AttachmentsMetaData", StringComparison.InvariantCultureIgnoreCase)))?.Value;
      }), "InProcess", 150, 10), providerFactory);
    }

    private static string ReadRetry(
      Func<string> func,
      string value,
      int sleepTime,
      int maxRetryCount)
    {
      for (int index = 0; index < maxRetryCount; ++index)
      {
        string a = func();
        if (!string.Equals(a, value, StringComparison.InvariantCultureIgnoreCase))
          return a;
        Thread.Sleep(sleepTime);
      }
      throw new TimeoutException("Unable to obtain LOCK in specified time Taken.");
    }

    internal static XmlDocument getAttachmentXml(Loan loan)
    {
      using (BinaryObject supportingData = loan.GetSupportingData("Attachments.xml", false))
      {
        XmlDocument attachmentXml = new XmlDocument();
        if (supportingData == null)
        {
          attachmentXml.LoadXml("<Attachments/>");
        }
        else
        {
          try
          {
            attachmentXml.LoadXml(supportingData.ToString(Encoding.UTF8));
          }
          catch (Exception ex)
          {
            attachmentXml.LoadXml(supportingData.ToString());
          }
        }
        return attachmentXml;
      }
    }

    internal static XmlDocument getAttachmentXml(Loan loan, bool readExisting)
    {
      using (BinaryObject supportingData = loan.GetSupportingData("Attachments.xml"))
      {
        XmlDocument attachmentXml = new XmlDocument();
        if (supportingData == null)
        {
          if (readExisting)
            return (XmlDocument) null;
          attachmentXml.LoadXml("<Attachments/>");
        }
        else
        {
          try
          {
            attachmentXml.LoadXml(supportingData.ToString(Encoding.UTF8));
          }
          catch (Exception ex)
          {
            attachmentXml.LoadXml(supportingData.ToString());
          }
        }
        return attachmentXml;
      }
    }

    internal static void saveAttachmentXml(Loan loan, XmlDocument xmlDoc, bool isNewAttachment = false)
    {
      FileAttachmentStore.AddOrUpdateAttachmentFileSequenceNumber(xmlDoc);
      try
      {
        using (BinaryObject data = new BinaryObject(xmlDoc.OuterXml, Encoding.UTF8))
          loan.SaveSupportingData("Attachments.xml", data, isNewAttachment);
      }
      catch (Exception ex)
      {
        using (BinaryObject data = new BinaryObject(xmlDoc.OuterXml, Encoding.ASCII))
          loan.SaveSupportingData("Attachments.xml", data, isNewAttachment);
      }
      FileAttachmentStore.AddOrUpdateAttachmentFileSequenceNumberInDatabase(loan, xmlDoc);
    }

    internal static void deleteAttachmentXML(Loan loan)
    {
      loan.SaveSupportingData("Attachments.xml", (BinaryObject) null);
    }

    internal static void replaceAttachment(XmlDocument xmlDoc, FileAttachment attachment)
    {
      string xpath = string.Format("//Background[@ID='{0}']", (object) attachment.ID);
      XmlNode oldChild = xmlDoc.SelectSingleNode(xpath);
      if (oldChild == null)
        return;
      string name = (string) null;
      switch (attachment)
      {
        case NativeAttachment _:
          name = "File";
          break;
        case ImageAttachment _:
          name = "Image";
          break;
      }
      XmlElement element = xmlDoc.CreateElement(name);
      attachment.ToXml(element);
      oldChild.ParentNode.ReplaceChild((XmlNode) element, oldChild);
    }

    internal static bool deleteAttachmentXmlNode(XmlDocument xmlDoc, FileAttachment attachment)
    {
      if (xmlDoc == null || attachment == null)
        return false;
      if (attachment.IsRemoved)
      {
        XmlNode oldChild = xmlDoc.DocumentElement.SelectSingleNode("Removed");
        if (oldChild != null && oldChild.ChildNodes.Count == 1)
        {
          oldChild.ParentNode.RemoveChild(oldChild);
          return true;
        }
      }
      for (int index = 0; index <= 3; ++index)
      {
        string xpath = (string) null;
        switch (index)
        {
          case 0:
            xpath = string.Format("//File[@Filename='{0}']", (object) attachment.ID);
            break;
          case 1:
            xpath = string.Format("//Image[@ID='{0}']", (object) attachment.ID);
            break;
          case 2:
            xpath = string.Format("//Background[@ID='{0}']", (object) attachment.ID);
            break;
          case 3:
            xpath = string.Format("//Cloud[@ID='{0}']", (object) attachment.ID);
            break;
        }
        XmlNode oldChild = xmlDoc.SelectSingleNode(xpath);
        if (oldChild != null)
        {
          oldChild.ParentNode.RemoveChild(oldChild);
          return true;
        }
      }
      return false;
    }

    internal static bool writeAttachment(XmlDocument xmlDoc, FileAttachment attachment)
    {
      XmlNode newChild = (XmlNode) xmlDoc.DocumentElement;
      if (attachment.IsRemoved)
      {
        newChild = xmlDoc.DocumentElement.SelectSingleNode("Removed");
        if (newChild == null)
        {
          newChild = (XmlNode) xmlDoc.CreateElement("Removed");
          xmlDoc.DocumentElement.AppendChild(newChild);
        }
      }
      XmlNode oldChild = (XmlNode) null;
      string[] strArray = new string[4]
      {
        "//File[@Filename='{0}']",
        "//Image[@ID='{0}']",
        "//Background[@ID='{0}']",
        "//Cloud[@ID='{0}']"
      };
      string str = attachment.ID.Replace("'", "&apos;");
      for (int index = 0; index < strArray.Length; ++index)
      {
        string xpath = string.Format(strArray[index], (object) str);
        oldChild = xmlDoc.SelectSingleNode(xpath);
        if (oldChild != null)
        {
          if (attachment is BackgroundAttachment && index != 2)
            return false;
          if (oldChild.ParentNode != newChild)
          {
            oldChild.ParentNode.RemoveChild(oldChild);
            oldChild = (XmlNode) null;
            break;
          }
          break;
        }
      }
      string name = (string) null;
      switch (attachment)
      {
        case NativeAttachment _:
          name = "File";
          break;
        case ImageAttachment _:
          name = "Image";
          break;
        case BackgroundAttachment _:
          name = "Background";
          break;
        case CloudAttachment _:
          name = "Cloud";
          break;
      }
      XmlElement element = xmlDoc.CreateElement(name);
      attachment.ToXml(element);
      if (oldChild != null)
        newChild.ReplaceChild((XmlNode) element, oldChild);
      else
        newChild.AppendChild((XmlNode) element);
      return oldChild == null;
    }

    internal static IEnumerable<FileAttachment> BuildAttachmentsFromXML(
      IEnumerable<Tuple<XmlElement, bool>> attachmentXmls)
    {
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (Tuple<XmlElement, bool> attachmentXml in attachmentXmls)
      {
        switch (attachmentXml.Item1.Name)
        {
          case "File":
            fileAttachmentList.Add((FileAttachment) new NativeAttachment(attachmentXml.Item1, attachmentXml.Item2));
            continue;
          case "Image":
            fileAttachmentList.Add((FileAttachment) new ImageAttachment(attachmentXml.Item1, attachmentXml.Item2));
            continue;
          case "Cloud":
            fileAttachmentList.Add((FileAttachment) new CloudAttachment(attachmentXml.Item1, attachmentXml.Item2));
            continue;
          case "Background":
            fileAttachmentList.Add((FileAttachment) new BackgroundAttachment(attachmentXml.Item1, attachmentXml.Item2));
            continue;
          default:
            continue;
        }
      }
      return (IEnumerable<FileAttachment>) fileAttachmentList;
    }

    private static int GetAttachmentsCount(XmlDocument xmlDoc)
    {
      return xmlDoc == null || xmlDoc.DocumentElement == null ? 0 : xmlDoc.DocumentElement.SelectNodes("//File").Count + xmlDoc.DocumentElement.SelectNodes("//Image").Count + xmlDoc.DocumentElement.SelectNodes("//Background").Count + xmlDoc.DocumentElement.SelectNodes("//Cloud").Count;
    }

    private static void ReadAttachmentWithLatestChanges(Loan loan, int fileAttachmentCount)
    {
      if (loan == null || fileAttachmentCount == 0)
        return;
      int num1 = 0;
      int num2 = 10;
      int millisecondsTimeout = 150;
      int num3;
      XmlDocument attachmentXml;
      while (true)
      {
        num3 = 0;
        attachmentXml = FileAttachmentStore.getAttachmentXml(loan, true);
        if (attachmentXml == null)
        {
          TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("SaveFileAttachment: GetAttachmentXml after writing changes doesn't match count. LoanId:{0}, Count:{1}, Expected Count: {2}.  RetryCount: {3}.", (object) loan.Identity.Guid, (object) 0, (object) fileAttachmentCount, (object) num1));
        }
        else
        {
          num3 = FileAttachmentStore.GetAttachmentsCount(attachmentXml);
          if (num3 < fileAttachmentCount)
            TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("SaveFileAttachment: GetAttachmentXml after writing changes doesn't match count. LoanId:{0}, Count:{1}, Expected Count: {2}.  RetryCount: {3}.", (object) loan.Identity.Guid, (object) num3, (object) fileAttachmentCount, (object) num1));
          else
            break;
        }
        if (++num1 < num2)
          Thread.Sleep(millisecondsTimeout);
        else
          goto label_10;
      }
      if (num1 <= 0)
        return;
      TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("SaveFileAttachment: GetAttachmentXml after writing changes matched count. LoanId:{0}, Count:{1}, Expected Count: {2}.  RetryCount: {3}. elapsed {4}ms", (object) loan.Identity.Guid, (object) num3, (object) fileAttachmentCount, (object) num1, (object) (num1 * millisecondsTimeout)));
      return;
label_10:
      TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("SaveFileAttachment: GetAttachmentXml after writing changes doesn't match count after retrying {3} times elapsed {4}. LoanId:{0}, Count:{1}, Expected Count: {2}. GetAttachment response: {5}", (object) loan.Identity.Guid, (object) num3, (object) fileAttachmentCount, (object) num1, (object) (num1 * millisecondsTimeout), (object) attachmentXml.OuterXml));
    }

    private static void CheckAttachmentXmlVersion(
      Loan loan,
      ref XmlDocument xmlDoc,
      string methodName = "CheckAttachmentXmlVersion�",
      bool throwOnError = true)
    {
      int result1 = 0;
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append(string.Format("SELECT value \r\n                            FROM   loanproperties \r\n                            WHERE  guid = '{0}' \r\n                            AND category = 'Attachments' \r\n                            AND attribute = 'FileSequenceNumber'", (object) loan.Identity.Guid));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count == 0 || dataRowCollection[0]["value"] == DBNull.Value)
          return;
        string[] source = dataRowCollection[0]["value"].ToString().Split(';');
        DateTime result2;
        if (((IEnumerable<string>) source).Count<string>() < 2 || !DateTime.TryParse(source[1], (IFormatProvider) null, DateTimeStyles.AdjustToUniversal, out result2) || (DateTime.UtcNow - result2).TotalSeconds > 150.0)
          return;
        if (!int.TryParse(source[0], out result1))
          return;
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("{0}: Failed while reading attachment xml file sequenceNumber from DB.  LoanId: {1}.  Message: {2}", (object) methodName, (object) loan.Identity.Guid, (object) ex.Message));
        return;
      }
      XmlNode xmlNode1 = xmlDoc.SelectSingleNode("/Attachments/@FileSequenceNumber");
      int result3;
      if (xmlNode1 == null || !int.TryParse(xmlNode1.Value, out result3))
      {
        TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("{0}: Failed while reading attachment xml FileSequenceNumber from file. FileSequenceNumber is not in correct format. LoanId: {1}.  FileSequenceNumber in DB: {2}.", (object) methodName, (object) loan.Identity.Guid, (object) result1));
      }
      else
      {
        if (result3 >= result1)
          return;
        TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("{0}: Failed while reading attachment xml file.  FileSequenceNumber doesn't match with FileSequenceNumber in DB.  LoanId: {1}.  FileSequenceNumber in DB: {2}.  FileSequenceNumber in file: {2}", (object) methodName, (object) loan.Identity.Guid, (object) result1, (object) result3));
        int num1 = 1;
        int num2 = 10;
        int millisecondsTimeout = 150;
        do
        {
          Thread.Sleep(millisecondsTimeout);
          XmlDocument attachmentXml = FileAttachmentStore.getAttachmentXml(loan, true);
          if (attachmentXml != null)
          {
            XmlNode xmlNode2 = attachmentXml.SelectSingleNode("/Attachments/@FileSequenceNumber");
            if (xmlNode2 == null || !int.TryParse(xmlNode2.Value, out result3))
              TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("{0}: Failed while reading attachment xml FileSequenceNumber from file. FileSequenceNumber is not in correct format. LoanId: {1}.  FileSequenceNumber in DB: {2}.", (object) methodName, (object) loan.Identity.Guid, (object) result1));
            else if (result3 >= result1)
            {
              TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("{0}: Attachment xml FileSequenceNumber matched. LoanId: {1}.  FileSequenceNumber in DB: {2}.  FileSequenceNumber in file: {3},  Retrycount: {4}.", (object) methodName, (object) loan.Identity.Guid, (object) result1, (object) result3, (object) num1));
              xmlDoc = attachmentXml;
              return;
            }
          }
          TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("{0}: Failed while reading attachment xml file.  FileSequenceNumber doesn't match with FileSequenceNumber in DB.  LoanId: {1}.  FileSequenceNumber in DB: {2}.  FileSequenceNumber in file: {3},  Retrycount: {4}.", (object) methodName, (object) loan.Identity.Guid, (object) result1, (object) result3, (object) num1));
        }
        while (++num1 <= num2);
        if (throwOnError)
          throw new Exception(string.Format("Create/Get latest Attachment failed.  Please try again. Message: Couldn't read latest version of Attachment xml file.  Class: FileAttachmentStore, Method: {0}, LoanId {1}.", (object) methodName, (object) loan.Identity.Guid));
      }
    }

    private static void AddOrUpdateAttachmentFileSequenceNumberInDatabase(
      Loan loan,
      XmlDocument xmlDoc)
    {
      int result = 0;
      XmlNode xmlNode = xmlDoc.SelectSingleNode("/Attachments/@FileSequenceNumber");
      if (xmlNode != null)
        int.TryParse(xmlNode.Value, out result);
      string str = result.ToString() + ";" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append(string.Format("UPDATE loanproperties \r\n                                            SET    value = '{1}' \r\n                                            WHERE  guid = '{0}' \r\n                                            AND    category = 'Attachments' \r\n                                            AND    attribute = 'FileSequenceNumber'\r\n                                         IF @@ROWCOUNT = 0 \r\n                                            BEGIN \r\n                                                INSERT INTO loanproperties \r\n                                                            ( \r\n                                                                        guid, \r\n                                                                        category, \r\n                                                                        attribute, \r\n                                                                        value \r\n                                                            ) \r\n                                                            VALUES \r\n                                                            ( \r\n                                                                        '{0}', \r\n                                                                        'Attachments', \r\n                                                                        'FileSequenceNumber', \r\n                                                                        '{1}' \r\n                                                            ) \r\n                                            END", (object) loan.Identity.Guid, (object) str));
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (FileAttachmentStore), string.Format("CheckAttachmentXmlVersion: Failed while writing attachment xml FileSequenceNumber to DB.  LoanId: {0}.  FileSequenceNumber: {1}.  Message: {2}", (object) loan.Identity.Guid, (object) result, (object) ex.Message));
      }
    }

    private static void AddOrUpdateAttachmentFileSequenceNumber(XmlDocument xmlDoc)
    {
      int result = 0;
      XmlNode xmlNode = xmlDoc.SelectSingleNode("/Attachments/@FileSequenceNumber");
      if (xmlNode != null)
        int.TryParse(xmlNode.Value, out result);
      xmlDoc.DocumentElement.SetAttribute("FileSequenceNumber", (result + 1).ToString());
    }

    private static bool PublishLoanAttachmentKafkaEvent(
      string loanId,
      string userId,
      bool isSourceEncompass,
      FileAttachment[] attachmentList,
      LoanEventType loanEventType,
      DateTime loanModifiedTime,
      string loanFileLocation)
    {
      ClientContext current = ClientContext.GetCurrent();
      bool flag = false;
      try
      {
        TraceLog.WriteInfo(nameof (FileAttachmentStore), string.Format("PublishLoanEventKafka : Values of isSourceEncompassRequest - {0} ", (object) isSourceEncompass));
        WebHooksEvent queueEvent = new WebHooksEvent("serviceId", current.InstanceName, "siteId", loanEventType.ToString(), userId, isSourceEncompass ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS, loanModifiedTime);
        if (loanId != null && loanId.StartsWith("{") && loanId.EndsWith("}"))
          loanId = loanId.TrimStart('{').TrimEnd('}');
        queueEvent.StandardMessage.EntityId = loanId;
        List<EllieMae.EMLite.ClientServer.MessageServices.Message.Attachment.Attachment> attachmentList1 = new List<EllieMae.EMLite.ClientServer.MessageServices.Message.Attachment.Attachment>();
        foreach (FileAttachment attachment1 in attachmentList)
        {
          EllieMae.EMLite.ClientServer.MessageServices.Message.Attachment.Attachment attachment2 = new EllieMae.EMLite.ClientServer.MessageServices.Message.Attachment.Attachment()
          {
            Id = attachment1.ID,
            Title = attachment1.Title
          };
          attachmentList1.Add(attachment2);
        }
        AttachmentEvent resourceList = new AttachmentEvent()
        {
          attachmentCreated = attachmentList1
        };
        if (attachmentList1.Count > 0)
          queueEvent.AddAttachmentKafkaMessage(loanId, Guid.NewGuid().ToString(), current.ClientID, current.InstanceName, resourceList, loanFileLocation, isSourceEncompass);
        if (queueEvent.QueueMessages.Count > 0)
        {
          IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
          IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
          queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
          flag = true;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (FileAttachmentStore), string.Format("Exception publishing attachment webhook loanEvent to kafka for loanId - {0}. Exception details {1}", (object) loanId, (object) ex.StackTrace));
      }
      return flag;
    }

    private static FileAttachmentStore.LockHelper CheckOut(string guid)
    {
      return FileAttachmentStore.CheckOut(nameof (FileAttachmentStore), guid);
    }

    private static FileAttachmentStore.LockHelper CheckOut(string lockKey, string guid)
    {
      if ((guid ?? "") == "")
        throw new ArgumentException("Guid cannot be blank or null", nameof (guid));
      ICacheLock<bool?> objLock = (ICacheLock<bool?>) null;
      try
      {
        if (HzcLoanLockFactory.UseHzcLock)
          objLock = HzcLoanLockFactory.Instance.CheckOutLoan(lockKey, guid);
        return new FileAttachmentStore.LockHelper(objLock);
      }
      catch (Exception ex)
      {
        objLock?.Dispose();
        throw;
      }
    }

    private class LockHelper : IDisposable
    {
      private IDisposable objLock;

      public LockHelper(ICacheLock<bool?> objLock) => this.objLock = (IDisposable) objLock;

      public void Dispose()
      {
        if (this.objLock == null)
          return;
        this.objLock.Dispose();
      }
    }
  }
}

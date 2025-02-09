// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic.SDCUtils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Serialization;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic
{
  public static class SDCUtils
  {
    private const string className = "SDCStaticHelper�";
    private static readonly string sw = Tracing.SwEFolder;
    private const string DOC_ORIGINAL = "DocOriginal�";
    private const string DOC_CURRENT = "DocCurrent�";

    public static string GetAnnotationTitleWithoutSeconds(string titleWithSeconds)
    {
      string[] strArray = titleWithSeconds.Split('-');
      string str = DateTime.Parse(strArray[strArray.Length - 1]).ToString("MM/dd/yy hh:mm tt");
      strArray[strArray.Length - 1] = str;
      return string.Concat(strArray);
    }

    public static int GetPageIdFromFileName(string fileName)
    {
      int result;
      return int.TryParse(Path.GetFileNameWithoutExtension(fileName), out result) ? result : -1;
    }

    public static void AppendLoanAutoSaveSDCXmls(
      FileAttachment fileAttachment,
      XmlDocument parentDocument,
      XmlElement attachmentElement)
    {
      try
      {
        (SDCDocument sdcDocument1, SDCDocument sdcDocument2) = SDCUtils.SeparateDocumentJson(fileAttachment);
        if (sdcDocument2 == null || !new SDCHelper().HasDocJsonChanged(JsonConvert.SerializeObject((object) sdcDocument1), JsonConvert.SerializeObject((object) sdcDocument2)))
          return;
        XmlElement element1 = parentDocument.CreateElement("DocOriginal");
        XmlElement element2 = parentDocument.CreateElement("DocCurrent");
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (SDCDocument));
        using (MemoryStream stream = StreamHelper.NewMemoryStream())
        {
          stream.SetLength(0L);
          xmlSerializer.Serialize((Stream) stream, (object) sdcDocument1);
          element1.InnerText = stream.ToString(Encoding.UTF8, true);
          stream.SetLength(0L);
          xmlSerializer.Serialize((Stream) stream, (object) sdcDocument2);
          element2.InnerText = stream.ToString(Encoding.UTF8, true);
        }
        attachmentElement.AppendChild((XmlNode) element1);
        attachmentElement.AppendChild((XmlNode) element2);
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCUtils.sw, TraceLevel.Error, "SDCStaticHelper", string.Format("SkyDriveClassic: Error Appending Auto Save XMLs. Ex: {0}", (object) ex));
        throw;
      }
    }

    public static void LoadAutoSaveSDCDocuments(
      FileAttachment attachment,
      XmlElement attachmentElement)
    {
      SDCDocument originalSDCDocument = (SDCDocument) null;
      SDCDocument currentSDCDocument = (SDCDocument) null;
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (SDCDocument));
      XmlElement xmlElement1 = attachmentElement.GetElementsByTagName("DocOriginal").OfType<XmlElement>().FirstOrDefault<XmlElement>();
      if (xmlElement1 != null)
      {
        try
        {
          using (StringReader stringReader = new StringReader(xmlElement1.InnerText))
            originalSDCDocument = (SDCDocument) xmlSerializer.Deserialize((TextReader) stringReader);
        }
        catch (Exception ex)
        {
          Tracing.Log(SDCUtils.sw, TraceLevel.Error, "SDCStaticHelper", string.Format("SkyDriveClassic: Error deserializing original SDC document from Auto Save XML. Ex: {0}", (object) ex));
          throw;
        }
      }
      XmlElement xmlElement2 = attachmentElement.GetElementsByTagName("DocCurrent").OfType<XmlElement>().FirstOrDefault<XmlElement>();
      if (xmlElement2 != null)
      {
        try
        {
          using (StringReader stringReader = new StringReader(xmlElement2.InnerText))
            currentSDCDocument = (SDCDocument) xmlSerializer.Deserialize((TextReader) stringReader);
        }
        catch (Exception ex)
        {
          Tracing.Log(SDCUtils.sw, TraceLevel.Error, "SDCStaticHelper", string.Format("SkyDriveClassic: Error deserializing current SDC document from Auto Save XML. Ex: {0}", (object) ex));
          throw;
        }
      }
      SDCUtils.AssignSDCDocuments(attachment, originalSDCDocument, currentSDCDocument);
    }

    public static void AssignSDCDocuments(
      FileAttachment attachment,
      SDCDocument originalSDCDocument,
      SDCDocument currentSDCDocument)
    {
      if (attachment is NativeAttachment nativeAttachment)
      {
        nativeAttachment.OriginalSDCDocument = originalSDCDocument;
        nativeAttachment.CurrentSDCDocument = currentSDCDocument;
      }
      else
      {
        ImageAttachment imageAttachment = attachment as ImageAttachment;
      }
    }

    public static (SDCDocument originalSDCDocument, SDCDocument currentSDCDocument) SeparateDocumentJson(
      FileAttachment attachment)
    {
      if (attachment is NativeAttachment nativeAttachment)
        return (nativeAttachment.OriginalSDCDocument, nativeAttachment.CurrentSDCDocument);
      ImageAttachment imageAttachment = attachment as ImageAttachment;
      return ((SDCDocument) null, (SDCDocument) null);
    }
  }
}

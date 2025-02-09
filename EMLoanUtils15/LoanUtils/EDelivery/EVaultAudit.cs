// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.EDelivery.EVaultAudit
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.EDelivery
{
  public class EVaultAudit
  {
    public DateTime defaultDate = new DateTime(1900, 1, 1);

    public EVaultAudit()
    {
    }

    public EVaultAudit(EVaultAudit data)
    {
      this.AuditData = data.AuditData;
      this.CertificateData = data.CertificateData;
      this.ConsentData = data.ConsentData;
      this.SignedDocumentData = data.SignedDocumentData;
      this.SignedFieldData = data.SignedFieldData;
    }

    public byte[] CertificateData { get; set; }

    public byte[] SignedDocumentData { get; set; }

    public AuditData AuditData { get; set; }

    public object SignedFieldData { get; set; }

    public byte[] ConsentData { get; set; }

    public string GetDataInXMLFormat()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (EVaultAudit));
      XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
      {
        Indent = true,
        OmitXmlDeclaration = true
      };
      XmlDocument xmlDocument = new XmlDocument();
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        xmlDocument.LoadXml("<EVaultAudit Version='Docu Sign'/>");
        string certificateData = this.getCertificateData();
        stringBuilder.Append(certificateData);
        stringBuilder.Append(this.getSignedDocumentData());
        stringBuilder.Append(this.getAuditDataDocuSign());
        stringBuilder.Append(this.getConsentDataPdf());
        stringBuilder.Append(this.getSignedFieldDataAuditDataDocuSign());
      }
      catch (Exception ex)
      {
        string message = ex.Message;
      }
      xmlDocument.DocumentElement.InnerXml = stringBuilder.ToString();
      return xmlDocument.OuterXml;
    }

    private string getConsentDataPdf()
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<ConsentData/>");
      string empty = string.Empty;
      xmlDocument.DocumentElement.InnerXml = Convert.ToBase64String(this.ConsentData);
      return xmlDocument.DocumentElement.OuterXml;
    }

    private string getCertificateData()
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<CertificateData/>");
      xmlDocument.DocumentElement.InnerXml = Convert.ToBase64String(this.CertificateData);
      return xmlDocument.DocumentElement.OuterXml;
    }

    private string getSignedDocumentData()
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<SignedDocumentData/>");
      xmlDocument.DocumentElement.InnerXml = Convert.ToBase64String(this.SignedDocumentData);
      return xmlDocument.DocumentElement.OuterXml;
    }

    private string getAuditDataDocuSign()
    {
      XmlDocument xmlDocument1 = new XmlDocument();
      xmlDocument1.LoadXml("<AuditData/>");
      string xml = this.CreateXML().Replace("<?xml version=\"1.0\"?>", "");
      XmlDocument xmlDocument2 = new XmlDocument();
      xmlDocument2.LoadXml(xml);
      xmlDocument1.DocumentElement.InnerXml = xmlDocument2.InnerXml;
      return xmlDocument1.DocumentElement.OuterXml;
    }

    private string getSignedFieldDataAuditDataDocuSign()
    {
      if (this.SignedFieldData == null)
        return (string) null;
      XmlDocument xmlDocument1 = new XmlDocument();
      xmlDocument1.LoadXml("<signedFieldData/>");
      XmlDocument xmlDocument2 = JsonConvert.DeserializeXmlNode(this.SignedFieldData.ToString(), "SignedFieldData");
      if (xmlDocument2 != null)
      {
        string innerXml = xmlDocument2.InnerXml;
        XmlDocument xmlDocument3 = new XmlDocument();
        xmlDocument3.LoadXml(innerXml);
        xmlDocument1.DocumentElement.InnerXml = xmlDocument3.InnerXml;
      }
      return xmlDocument1.DocumentElement.InnerXml;
    }

    private string CreateXML()
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (MemoryStream inStream = new MemoryStream())
      {
        new XmlSerializer(this.AuditData.AuditTrail.GetType()).Serialize((Stream) inStream, (object) this.AuditData.AuditTrail);
        inStream.Position = 0L;
        xmlDocument.Load((Stream) inStream);
        return xmlDocument.OuterXml;
      }
    }
  }
}

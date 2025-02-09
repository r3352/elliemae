// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DocumentAudit
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DocumentAudit
  {
    private DateTime auditDateTime;
    private string auditReportFileKey;
    private DocumentAudit.AuditAlert[] alerts;

    public DocumentAudit(
      DateTime timeStamp,
      string auditReportFileKey,
      DocumentAudit.AuditAlert[] alerts)
    {
      this.auditDateTime = timeStamp;
      this.auditReportFileKey = auditReportFileKey;
      this.alerts = alerts;
    }

    public DocumentAudit(XmlElement e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.auditDateTime = attributeReader.GetDate("AuditTime");
      this.auditReportFileKey = attributeReader.GetString("ReportKey", (string) null);
      List<DocumentAudit.AuditAlert> auditAlertList = new List<DocumentAudit.AuditAlert>();
      foreach (XmlElement selectNode in e.SelectNodes("Alert"))
        auditAlertList.Add(new DocumentAudit.AuditAlert(selectNode));
      this.alerts = auditAlertList.ToArray();
    }

    public DateTime AuditDateTime => this.auditDateTime;

    public string ReportFileKey => this.auditReportFileKey;

    public DocumentAudit.AuditAlert[] Alerts => this.alerts;

    public DocumentAudit.AuditAlert[] GetAlertsBySource(string alertSource)
    {
      List<DocumentAudit.AuditAlert> auditAlertList = new List<DocumentAudit.AuditAlert>();
      foreach (DocumentAudit.AuditAlert alert in this.alerts)
      {
        if (alert.Source == alertSource)
          auditAlertList.Add(alert);
      }
      return auditAlertList.ToArray();
    }

    public void ToXml(XmlElement e)
    {
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("AuditTime", (object) this.auditDateTime);
      if (this.auditReportFileKey != null)
        attributeWriter.Write("ReportKey", (object) this.auditReportFileKey);
      foreach (DocumentAudit.AuditAlert alert in this.alerts)
        alert.ToXml((XmlElement) e.AppendChild((XmlNode) e.OwnerDocument.CreateElement("Alert")));
    }

    public class AuditAlert
    {
      public readonly string Source;
      public readonly string AlertType;
      public readonly string Text;
      public readonly string[] Fields;

      public AuditAlert(string source, string type, string text, string[] fields)
      {
        this.Source = source;
        this.AlertType = type;
        this.Text = text;
        this.Fields = fields;
      }

      public AuditAlert(XmlElement e)
      {
        AttributeReader attributeReader = new AttributeReader(e);
        this.Source = attributeReader.GetString(nameof (Source));
        this.AlertType = attributeReader.GetString("Type");
        this.Text = attributeReader.GetString(nameof (Text));
        this.Fields = attributeReader.GetString(nameof (Fields)).Split(',');
      }

      public void ToXml(XmlElement e)
      {
        AttributeWriter attributeWriter = new AttributeWriter(e);
        attributeWriter.Write("Type", (object) this.AlertType);
        attributeWriter.Write("Source", (object) this.Source);
        attributeWriter.Write("Text", (object) this.Text);
        attributeWriter.Write("Fields", (object) string.Join(",", this.Fields));
      }
    }
  }
}

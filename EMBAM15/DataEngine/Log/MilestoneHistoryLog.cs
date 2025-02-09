// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.MilestoneHistoryLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  [Serializable]
  public class MilestoneHistoryLog : LogRecordBase
  {
    public static readonly string XmlType = nameof (MilestoneHistoryLog);
    private List<DocumentLog> documents = new List<DocumentLog>();
    private List<MilestoneLog> milestoneList = new List<MilestoneLog>();
    private List<MilestoneTaskLog> tasks = new List<MilestoneTaskLog>();
    private string template = "";
    private DateTime datechanged;
    private string userID = "";
    private string changeReason = "";
    private LogList log;

    public MilestoneHistoryLog(
      LogList log,
      List<LogRecordBase> historyLogs,
      string userID,
      string reason,
      EllieMae.EMLite.Workflow.MilestoneTemplate template,
      bool previousTemplateLockStatus,
      bool previousTemplateDateLockStatus)
    {
      this.log = log;
      this.Date = this.datechanged = DateTime.Now;
      this.userID = userID;
      this.changeReason = reason;
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement element1 = xmlDocument.CreateElement("PreviousMilestoneLog");
      this.template = template.Name;
      XmlElement element2 = xmlDocument.CreateElement(nameof (MilestoneTemplate));
      template.ToXml(element2, previousTemplateLockStatus, previousTemplateDateLockStatus);
      element1.AppendChild((XmlNode) element2);
      foreach (LogRecordBase logRecordBase in historyLogs.ToArray())
      {
        XmlElement xmlElement = !(logRecordBase is MilestoneLog) ? xmlDocument.CreateElement("Record") : xmlDocument.CreateElement("Milestone");
        logRecordBase.ToXml(xmlElement);
        element1.AppendChild((XmlNode) xmlElement);
      }
      this.parseXML(element1);
    }

    public MilestoneHistoryLog(LogList log, XmlElement e)
      : base(log, e)
    {
      this.log = log;
      AttributeReader attributeReader = new AttributeReader(e);
      this.Date = this.datechanged = attributeReader.GetDate(nameof (DateAddedUtc));
      this.userID = attributeReader.GetString(nameof (AddedByUserId));
      this.template = attributeReader.GetString(nameof (MilestoneTemplate));
      this.changeReason = attributeReader.GetString(nameof (ChangeReason));
      this.parseXML(e);
      this.MarkAsClean();
    }

    private void parseXML(XmlElement xml)
    {
      XmlDocument xmlDocument1 = new XmlDocument();
      xmlDocument1.AppendChild(xmlDocument1.ImportNode((XmlNode) xml, true));
      XElement xelement1 = XElement.Parse(xmlDocument1.InnerXml);
      XElement xelement2 = xelement1.Element((XName) "PreviousMilestoneLog") ?? xelement1;
      this.RecordXML = xelement2.ToString();
      foreach (XElement descendant in xelement2.Descendants())
      {
        if (descendant.Name == (XName) "Milestone")
        {
          XmlDocument xmlDocument2 = new XmlDocument();
          xmlDocument2.Load(descendant.CreateReader());
          this.milestoneList.Add(this.createMilestoneLogEntity(xmlDocument2.DocumentElement));
        }
        else if (descendant.Name == (XName) "Record")
        {
          string str = (string) descendant.Attribute((XName) "Type");
          XmlDocument xmlDocument3 = new XmlDocument();
          xmlDocument3.Load(descendant.CreateReader());
          switch (str)
          {
            case "Document":
              this.documents.Add(this.createDocumentLogEntity(xmlDocument3.DocumentElement));
              continue;
            case "Task":
              this.tasks.Add(this.createMilestoneTaskLogEntity(xmlDocument3.DocumentElement));
              continue;
            default:
              continue;
          }
        }
      }
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) MilestoneHistoryLog.XmlType);
      attributeWriter.Write("MilestoneTemplate", (object) this.MilestoneTemplate);
      attributeWriter.Write("AddedByUserId", (object) this.AddedByUserId);
      attributeWriter.Write("DateAddedUtc", (object) this.DateAddedUtc);
      attributeWriter.Write("ChangeReason", (object) this.ChangeReason);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(this.RecordXML);
      XmlNode documentElement = (XmlNode) xmlDocument.DocumentElement;
      XmlNode newChild = e.OwnerDocument.ImportNode(documentElement, true);
      e.AppendChild(newChild);
    }

    internal override bool IsSystemSpecific() => true;

    public virtual string RecordXML { get; set; }

    public List<MilestoneLog> Milestones
    {
      get => this.milestoneList;
      set => this.milestoneList = value;
    }

    public List<DocumentLog> Documents
    {
      get => this.documents;
      set => this.documents = value;
    }

    public string MilestoneTemplate
    {
      get => this.template;
      set => this.template = value;
    }

    public List<MilestoneTaskLog> Tasks
    {
      get => this.tasks;
      set => this.tasks = value;
    }

    public DateTime DateAddedUtc
    {
      get => this.datechanged;
      set => this.datechanged = value;
    }

    public string AddedByUserId
    {
      get => this.userID;
      set => this.userID = value;
    }

    public string ChangeReason
    {
      get => this.changeReason;
      set => this.changeReason = value;
    }

    private MilestoneLog createMilestoneLogEntity(XmlElement milestoneElement)
    {
      return new MilestoneLog(milestoneElement, (IMilestoneDateCalculator) null);
    }

    private DocumentLog createDocumentLogEntity(XmlElement documentElement)
    {
      return new DocumentLog(this.log, documentElement);
    }

    private MilestoneTaskLog createMilestoneTaskLogEntity(XmlElement milestoneTaskElement)
    {
      return new MilestoneTaskLog(this.log, milestoneTaskElement);
    }
  }
}

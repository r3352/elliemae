// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DocumentTrackingLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using Elli.ElliEnum;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DocumentTrackingLog : LogRecordBase
  {
    public static readonly string XmlType = "DocumentTracking";
    private DocTrackingActionCd actionCd;
    private string action = string.Empty;
    private string logDate = string.Empty;
    private string logBy = string.Empty;
    private bool dot;
    private bool ftp;
    private bool en;
    private string organization = string.Empty;
    private string contact = string.Empty;
    private bool email;
    private bool phone;
    private Hashtable docTrackingSnapshot;
    public string docTrackingSnapshotString;

    public DocumentTrackingLog(DateTime date)
      : base(date, "")
    {
    }

    public DocumentTrackingLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.actionCd = (DocTrackingActionCd) Enum.Parse(typeof (DocTrackingActionCd), attributeReader.GetString(nameof (ActionCd)), true);
      this.action = attributeReader.GetString(nameof (Action));
      this.logDate = attributeReader.GetString(nameof (LogDate));
      this.logBy = attributeReader.GetString(nameof (LogBy));
      this.dot = attributeReader.GetString("DOT").ToUpper().Equals("Y");
      this.ftp = attributeReader.GetString("FTP").ToUpper().Equals("Y");
      this.en = attributeReader.GetString("EN").ToUpper().Equals("Y");
      this.organization = attributeReader.GetString(nameof (Organization));
      this.contact = attributeReader.GetString(nameof (Contact));
      this.phone = attributeReader.GetString(nameof (Phone)).ToUpper().Equals("Y");
      this.email = attributeReader.GetString(nameof (Email)).ToUpper().Equals("Y");
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) DocumentTrackingLog.XmlType);
      attributeWriter.Write("ActionCd", (object) this.actionCd);
      attributeWriter.Write("Action", (object) this.action);
      attributeWriter.Write("LogDate", (object) this.logDate);
      attributeWriter.Write("LogBy", (object) this.logBy);
      attributeWriter.Write("DOT", (object) this.dot);
      attributeWriter.Write("FTP", (object) this.ftp);
      attributeWriter.Write("EN", (object) this.en);
      attributeWriter.Write("Organization", (object) this.organization);
      attributeWriter.Write("Contact", (object) this.contact);
      attributeWriter.Write("Email", (object) this.email);
      attributeWriter.Write("Phone", (object) this.phone);
    }

    public DocTrackingActionCd ActionCd
    {
      get => this.actionCd;
      set
      {
        this.actionCd = value;
        this.MarkAsDirty();
      }
    }

    public string Action
    {
      get => this.action;
      set
      {
        this.action = value;
        this.MarkAsDirty();
      }
    }

    public string LogDate
    {
      get => this.logDate;
      set
      {
        this.logDate = value;
        this.MarkAsDirty();
      }
    }

    public string LogBy
    {
      get => this.logBy;
      set
      {
        this.logBy = value;
        this.MarkAsDirty();
      }
    }

    public bool Dot
    {
      get => this.dot;
      set
      {
        this.dot = value;
        this.MarkAsDirty();
      }
    }

    public bool Ftp
    {
      get => this.ftp;
      set
      {
        this.ftp = value;
        this.MarkAsDirty();
      }
    }

    public bool En
    {
      get => this.en;
      set
      {
        this.en = value;
        this.MarkAsDirty();
      }
    }

    public string Organization
    {
      get => this.organization;
      set
      {
        this.organization = value;
        this.MarkAsDirty();
      }
    }

    public string Contact
    {
      get => this.contact;
      set
      {
        this.contact = value;
        this.MarkAsDirty();
      }
    }

    public bool Email
    {
      get => this.email;
      set
      {
        this.email = value;
        this.MarkAsDirty();
      }
    }

    public bool Phone
    {
      get => this.phone;
      set
      {
        this.phone = value;
        this.MarkAsDirty();
      }
    }

    public bool IsSnapShotDirty { get; set; }

    public Hashtable DocTrackingSnapshot
    {
      get
      {
        if (this.docTrackingSnapshot == null && this.Log.Loan.SnapshotProvider != null)
        {
          Dictionary<string, string> loanSnapshot = this.Log.Loan.SnapshotProvider.GetLoanSnapshot(LogSnapshotType.DocumentTracking, new System.Guid(this.Guid), false);
          if (loanSnapshot != null)
            this.docTrackingSnapshot = new Hashtable((IDictionary) loanSnapshot);
        }
        return this.docTrackingSnapshot;
      }
      set
      {
        Hashtable hashtable = new Hashtable();
        foreach (object key in (IEnumerable) value.Keys)
          hashtable.Add((object) key.ToString(), (object) value[key].ToString());
        this.IsSnapShotDirty = true;
        this.docTrackingSnapshot = hashtable;
      }
    }

    [CLSCompliant(false)]
    public string DocTrackingSnapshotString
    {
      get
      {
        if (this.docTrackingSnapshot == null || this.docTrackingSnapshot.Count == 0)
          return string.Empty;
        XmlDocument xmlDocument = new XmlDocument();
        XmlElement element = xmlDocument.CreateElement("Log");
        element.SetAttribute("GUID", this.Guid);
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        foreach (DictionaryEntry dictionaryEntry in this.docTrackingSnapshot)
        {
          string str1 = dictionaryEntry.Key.ToString();
          string str2 = dictionaryEntry.Value.ToString();
          if (!(str2 == string.Empty))
          {
            XmlElement xmlElement = (XmlElement) element.AppendChild((XmlNode) xmlDocument.CreateElement("FIELD"));
            xmlElement.SetAttribute("id", str1);
            xmlElement.SetAttribute("val", str2);
          }
        }
        this.docTrackingSnapshotString = element.OuterXml;
        return this.docTrackingSnapshotString;
      }
      set => this.docTrackingSnapshotString = value;
    }
  }
}

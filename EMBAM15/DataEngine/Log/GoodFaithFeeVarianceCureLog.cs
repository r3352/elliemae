// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.GoodFaithFeeVarianceCureLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class GoodFaithFeeVarianceCureLog : LogRecordBase
  {
    public static readonly string XmlType = "GoodFaithFeeVarianceCure";
    private string resolvedById = string.Empty;
    private string resolvedBy = string.Empty;
    private string appliedCureAmount = string.Empty;
    private DateTime dateResolved;
    private string cureComments = string.Empty;
    private string name = string.Empty;
    private DateTime alertDate;
    private bool inLog = true;
    private GFFVAlertTriggerFieldList triggerFieldList;
    private string totalVariance = string.Empty;

    public GoodFaithFeeVarianceCureLog(
      DateTime dateResolved,
      string resolvedById,
      string resolvedBy,
      string appliedCureAmount,
      string totalVariance,
      string cureComments,
      string name,
      DateTime alertDate)
      : base(alertDate, string.Empty)
    {
      this.resolvedById = resolvedById;
      this.resolvedBy = resolvedBy;
      this.dateResolved = dateResolved;
      this.appliedCureAmount = appliedCureAmount;
      this.cureComments = cureComments;
      this.name = name;
      this.alertDate = alertDate;
      this.totalVariance = totalVariance;
      this.triggerFieldList = new GFFVAlertTriggerFieldList(this);
    }

    public GoodFaithFeeVarianceCureLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.resolvedById = attributeReader.GetString(nameof (ResolvedById));
      this.resolvedBy = attributeReader.GetString(nameof (ResolvedBy));
      this.appliedCureAmount = attributeReader.GetString(nameof (AppliedCureAmount));
      this.dateResolved = attributeReader.GetDate(nameof (DateResolved));
      this.cureComments = attributeReader.GetString(nameof (CureComments));
      this.name = attributeReader.GetString(nameof (Name));
      this.alertDate = attributeReader.GetDate(nameof (AlertDate));
      this.totalVariance = attributeReader.GetString(nameof (TotalVariance));
      this.triggerFieldList = new GFFVAlertTriggerFieldList(this, (XmlElement) e.SelectSingleNode("GFFVAlertTriggerFields"));
    }

    public override bool DisplayInLog
    {
      get => this.inLog;
      set
      {
        this.inLog = value;
        this.MarkAsDirty();
      }
    }

    public string Name
    {
      get => this.name;
      set
      {
        this.name = value;
        this.MarkAsDirty();
      }
    }

    public string ResolvedById => this.resolvedById;

    public string ResolvedBy
    {
      get => this.resolvedBy;
      set
      {
        this.resolvedBy = value;
        this.MarkAsDirty();
      }
    }

    public string AppliedCureAmount
    {
      get => this.appliedCureAmount;
      set
      {
        this.appliedCureAmount = value;
        this.MarkAsDirty();
      }
    }

    public string TotalVariance
    {
      get => this.totalVariance;
      set
      {
        this.totalVariance = value;
        this.MarkAsDirty();
      }
    }

    public string CureComments
    {
      get => this.cureComments;
      set
      {
        this.cureComments = value;
        this.MarkAsDirty();
      }
    }

    public DateTime DateResolved
    {
      get => this.dateResolved;
      set
      {
        this.dateResolved = value;
        this.MarkAsDirty();
      }
    }

    public GFFVAlertTriggerFieldList TriggerFieldList => this.triggerFieldList;

    public DateTime AlertDate
    {
      get => this.alertDate;
      set
      {
        this.alertDate = value;
        this.MarkAsDirty();
      }
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      return this.AlertList.ToPipelineAlerts(StandardAlertID.GoodFaithFeeVarianceViolation, this.Name);
    }

    internal override bool IncludeInLog() => base.IncludeInLog() && this.inLog;

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) GoodFaithFeeVarianceCureLog.XmlType);
      attributeWriter.Write("Name", (object) this.name);
      attributeWriter.Write("ResolvedById", (object) this.resolvedById);
      attributeWriter.Write("ResolvedBy", (object) this.resolvedBy);
      attributeWriter.Write("TotalVariance", (object) this.totalVariance);
      attributeWriter.Write("AppliedCureAmount", (object) this.appliedCureAmount);
      attributeWriter.Write("DateResolved", (object) this.dateResolved.ToString("M/d/yyyy h:mm tt"));
      attributeWriter.Write("InLog", (object) this.inLog);
      attributeWriter.Write("CureComments", (object) this.cureComments);
      attributeWriter.Write("AlertDate", (object) this.alertDate);
      this.triggerFieldList.ToXml((XmlElement) e.AppendChild((XmlNode) e.OwnerDocument.CreateElement("GFFVAlertTriggerFields")));
    }
  }
}

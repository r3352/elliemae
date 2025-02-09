// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.MilestoneTemplateLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Workflow;
using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  [Serializable]
  internal class MilestoneTemplateLog : LogRecordBase
  {
    public static readonly string XmlType = nameof (MilestoneTemplateLog);
    private string milestoneTemplateID;
    private string milestoneTemplateName;
    private bool isTemplateLocked;
    private bool isTemplateDatesLocked;
    private LogList log;

    public MilestoneTemplateLog(LogList log, MilestoneTemplate template)
      : base(log)
    {
      this.log = log;
      this.milestoneTemplateID = template.TemplateID;
      this.milestoneTemplateName = template.Name;
      this.isTemplateLocked = log.MSLock;
      this.isTemplateDatesLocked = log.MSDateLock;
    }

    public MilestoneTemplateLog(LogList log, XmlElement e)
      : base(log, e)
    {
      this.log = log;
      AttributeReader attributeReader = new AttributeReader(e);
      this.MilestoneTemplateID = attributeReader.GetString(nameof (MilestoneTemplateID));
      this.MilestoneTemplateName = attributeReader.GetString(nameof (MilestoneTemplateName));
      this.IsTemplateLocked = attributeReader.GetBoolean(nameof (IsTemplateLocked));
      this.IsTemplateDatesLocked = attributeReader.GetBoolean(nameof (IsTemplateDatesLocked));
      this.MarkAsClean();
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) MilestoneTemplateLog.XmlType);
      attributeWriter.Write("MilestoneTemplateID", (object) this.MilestoneTemplateID);
      attributeWriter.Write("MilestoneTemplateName", (object) this.MilestoneTemplateName);
      attributeWriter.Write("IsTemplateLocked", this.log.MSLock ? (object) "Y" : (object) "N");
      attributeWriter.Write("IsTemplateDatesLocked", this.log.MSDateLock ? (object) "Y" : (object) "N");
    }

    public string MilestoneTemplateID
    {
      get => this.milestoneTemplateID;
      set
      {
        this.milestoneTemplateID = value;
        this.markAsDirty(MilestoneTemplateProperty.TemplateId);
      }
    }

    public string MilestoneTemplateName
    {
      get => this.milestoneTemplateName;
      set
      {
        this.milestoneTemplateName = value;
        this.markAsDirty(MilestoneTemplateProperty.Name);
      }
    }

    public bool IsTemplateLocked
    {
      get => this.isTemplateLocked;
      set
      {
        this.isTemplateLocked = value;
        this.markAsDirty(MilestoneTemplateProperty.IsLocked);
      }
    }

    public bool IsTemplateDatesLocked
    {
      get => this.isTemplateDatesLocked;
      set
      {
        this.isTemplateDatesLocked = value;
        this.markAsDirty(MilestoneTemplateProperty.IsDateLocked);
      }
    }

    internal override bool IsSystemSpecific() => true;

    private void markAsDirty(MilestoneTemplateProperty property)
    {
      this.MarkAsDirty("Log.MT." + (object) property);
    }
  }
}

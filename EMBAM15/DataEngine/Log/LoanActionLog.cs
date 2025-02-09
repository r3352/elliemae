// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LoanActionLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using Elli.ElliEnum;
using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LoanActionLog : LogRecordBase
  {
    public static readonly string XmlType = nameof (LoanActionLog);

    public string TriggeredBy { get; set; }

    [CLSCompliant(false)]
    public LoanActionType LoanActionType { get; set; }

    public LoanActionLog(LoanActionType actionType, string triggeredBy)
      : base(DateTime.Now, string.Empty)
    {
      this.TriggeredBy = triggeredBy;
      this.LoanActionType = actionType;
      if (DateTimeKind.Local != this.date.Kind)
        return;
      this.date = this.date.ToUniversalTime();
    }

    public LoanActionLog(DateTime date, LoanActionType actionType, string triggeredBy)
      : base(date, string.Empty)
    {
      this.TriggeredBy = triggeredBy;
      this.LoanActionType = actionType;
      if (DateTimeKind.Local != date.Kind)
        return;
      date = date.ToUniversalTime();
    }

    public LoanActionLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      if (DateTimeKind.Local == this.date.Kind)
        this.date = this.date.ToUniversalTime();
      this.LoanActionType = (LoanActionType) Enum.Parse(typeof (LoanActionType), attributeReader.GetString(nameof (LoanActionType)));
      this.TriggeredBy = attributeReader.GetString(nameof (TriggeredBy));
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) LoanActionLog.XmlType);
      attributeWriter.Write("LoanActionType", (object) this.LoanActionType.ToString());
      attributeWriter.Write("TriggeredBy", (object) this.TriggeredBy);
    }
  }
}

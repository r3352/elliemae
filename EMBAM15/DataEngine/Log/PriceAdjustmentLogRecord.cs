// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.PriceAdjustmentLogRecord
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class PriceAdjustmentLogRecord
  {
    private static string XmlType = nameof (PriceAdjustmentLogRecord);

    public PriceAdjustmentLogRecord()
    {
      this.Description = string.Empty;
      this.Rate = 0M;
    }

    public static PriceAdjustmentLogRecord FromXml(XmlElement e, out int idx)
    {
      PriceAdjustmentLogRecord adjustmentLogRecord = new PriceAdjustmentLogRecord();
      AttributeReader attributeReader = new AttributeReader(e);
      adjustmentLogRecord.Description = attributeReader.GetString("Description");
      adjustmentLogRecord.Rate = attributeReader.GetDecimal("Rate", 0M);
      idx = attributeReader.GetInteger("Index", -1);
      return adjustmentLogRecord;
    }

    public PriceAdjustmentLogRecord(PriceAdjustmentLogRecord rec)
    {
      this.Description = rec.Description;
      this.Rate = rec.Rate;
    }

    public string Description { get; set; }

    public Decimal Rate { get; set; }

    public static PriceAdjustmentLogRecord Copy(PriceAdjustmentLogRecord rec)
    {
      return rec != null ? new PriceAdjustmentLogRecord(rec) : (PriceAdjustmentLogRecord) null;
    }

    public void ToXml(XmlElement e, int idx)
    {
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) PriceAdjustmentLogRecord.XmlType);
      attributeWriter.Write("Index", (object) idx);
      attributeWriter.Write("Description", (object) this.Description);
      attributeWriter.Write("Rate", (object) this.Rate);
    }
  }
}

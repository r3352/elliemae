// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.PriceAdjustmentList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class PriceAdjustmentList
  {
    private PriceAdjustmentLogRecord[] values = new PriceAdjustmentLogRecord[20];

    public void ToXml(XmlElement e)
    {
      for (int index = 0; index < 20; ++index)
      {
        if (this.values[index] != null)
        {
          XmlElement xmlElement = (XmlElement) e.AppendChild((XmlNode) e.OwnerDocument.CreateElement("PriceAdjustment"));
          this.values[index].ToXml(xmlElement, index + 1);
          e.AppendChild((XmlNode) xmlElement);
        }
      }
    }

    public PriceAdjustmentList()
    {
    }

    public PriceAdjustmentLogRecord this[int idx]
    {
      get => idx >= 0 && idx < 20 ? this.values[idx] : (PriceAdjustmentLogRecord) null;
      set
      {
        if (idx < 0 || idx >= 20)
          return;
        this.values[idx] = value;
      }
    }

    public PriceAdjustmentList(XmlElement e)
    {
      foreach (XmlElement childNode in e.ChildNodes)
      {
        int idx = -1;
        PriceAdjustmentLogRecord adjustmentLogRecord = PriceAdjustmentLogRecord.FromXml(childNode, out idx);
        if (idx >= 0 && idx < 20)
          this.values[idx - 1] = adjustmentLogRecord;
      }
    }
  }
}

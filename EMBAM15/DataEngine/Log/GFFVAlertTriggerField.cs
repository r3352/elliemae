// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.GFFVAlertTriggerField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class GFFVAlertTriggerField
  {
    private string guid;
    private string fieldId;
    private string description;
    private string initialLEValue;
    private string baseline;
    private string disclosedValue;
    private string itemizationValue;
    private string varianceValue;
    private string varianceLimit;

    public GFFVAlertTriggerField(
      string fieldId,
      string description,
      string initialLEValue,
      string baseline,
      string disclosedValue,
      string itemizationValue,
      string varianceValue,
      string varianceLimit)
    {
      this.fieldId = fieldId;
      this.description = description;
      this.initialLEValue = initialLEValue;
      this.baseline = baseline;
      this.disclosedValue = disclosedValue;
      this.itemizationValue = itemizationValue;
      this.varianceValue = varianceValue;
      this.varianceLimit = varianceLimit;
    }

    internal GFFVAlertTriggerField(XmlElement xml)
    {
      AttributeReader attributeReader = new AttributeReader(xml);
      this.guid = attributeReader.GetString(nameof (Guid));
      this.fieldId = attributeReader.GetString(nameof (FieldId));
      this.description = attributeReader.GetString(nameof (Description));
      this.initialLEValue = attributeReader.GetString(nameof (InitialLEValue));
      this.baseline = attributeReader.GetString(nameof (Baseline));
      this.disclosedValue = attributeReader.GetString(nameof (DisclosedValue));
      this.itemizationValue = attributeReader.GetString(nameof (ItemizationValue));
      this.varianceValue = attributeReader.GetString(nameof (VarianceValue));
      this.varianceLimit = attributeReader.GetString(nameof (VarianceLimit));
    }

    public string Guid => this.guid;

    public string FieldId => this.fieldId;

    public string Description => this.description;

    public string InitialLEValue => this.initialLEValue;

    public string Baseline => this.baseline;

    public string DisclosedValue => this.disclosedValue;

    public string ItemizationValue => this.itemizationValue;

    public string VarianceValue => this.varianceValue;

    public string VarianceLimit => this.varianceLimit;

    internal void ToXml(XmlElement xml)
    {
      AttributeWriter attributeWriter = new AttributeWriter(xml);
      attributeWriter.Write("Guid", (object) this.guid);
      attributeWriter.Write("FieldId", (object) this.fieldId);
      attributeWriter.Write("Description", (object) this.description);
      attributeWriter.Write("InitialLEValue", (object) this.initialLEValue);
      attributeWriter.Write("Baseline", (object) this.baseline);
      attributeWriter.Write("DisclosedValue", (object) this.disclosedValue);
      attributeWriter.Write("ItemizationValue", (object) this.itemizationValue);
      attributeWriter.Write("VarianceValue", (object) this.varianceValue);
      attributeWriter.Write("VarianceLimit", (object) this.varianceLimit);
    }
  }
}

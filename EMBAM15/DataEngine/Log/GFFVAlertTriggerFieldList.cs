// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.GFFVAlertTriggerFieldList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class GFFVAlertTriggerFieldList : IEnumerable<GFFVAlertTriggerField>, IEnumerable
  {
    private GoodFaithFeeVarianceCureLog goodFaithFeeVarianceLog;
    private List<GFFVAlertTriggerField> triggerFieldList = new List<GFFVAlertTriggerField>();

    internal GFFVAlertTriggerFieldList(
      GoodFaithFeeVarianceCureLog goodFaithFeeVarianceLog)
    {
      this.goodFaithFeeVarianceLog = goodFaithFeeVarianceLog;
    }

    internal GFFVAlertTriggerFieldList(
      GoodFaithFeeVarianceCureLog goodFaithFeeVarianceLog,
      XmlElement xml)
    {
      this.goodFaithFeeVarianceLog = goodFaithFeeVarianceLog;
      foreach (XmlElement selectNode in xml.SelectNodes("GFFVAlertTriggerField"))
        this.triggerFieldList.Add(new GFFVAlertTriggerField(selectNode));
    }

    public int Count => this.triggerFieldList.Count;

    public void Add(GFFVAlertTriggerField triggerField)
    {
      this.triggerFieldList.Add(triggerField);
      this.goodFaithFeeVarianceLog.MarkAsDirty();
    }

    public void Remove(GFFVAlertTriggerField doc)
    {
      this.triggerFieldList.Remove(doc);
      this.goodFaithFeeVarianceLog.MarkAsDirty();
    }

    public void Clear()
    {
      this.triggerFieldList.Clear();
      this.goodFaithFeeVarianceLog.MarkAsDirty();
    }

    public GFFVAlertTriggerField this[int index] => this.triggerFieldList[index];

    public bool Contains(string title) => this.GetTriggerFieldByID(title) != null;

    public GFFVAlertTriggerField GetTriggerFieldByID(string fieldID)
    {
      foreach (GFFVAlertTriggerField triggerFieldById in this)
      {
        if (string.Compare(triggerFieldById.FieldId, fieldID, true) == 0)
          return triggerFieldById;
      }
      return (GFFVAlertTriggerField) null;
    }

    public GFFVAlertTriggerField GetTriggerFieldByGuid(string guid)
    {
      foreach (GFFVAlertTriggerField triggerFieldByGuid in this)
      {
        if (string.Compare(triggerFieldByGuid.Guid, guid, true) == 0)
          return triggerFieldByGuid;
      }
      return (GFFVAlertTriggerField) null;
    }

    internal void ToXml(XmlElement xml)
    {
      foreach (GFFVAlertTriggerField alertTriggerField in this)
        alertTriggerField.ToXml((XmlElement) xml.AppendChild((XmlNode) xml.OwnerDocument.CreateElement("GFFVAlertTriggerField")));
    }

    public IEnumerator<GFFVAlertTriggerField> GetEnumerator()
    {
      return (IEnumerator<GFFVAlertTriggerField>) this.triggerFieldList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
